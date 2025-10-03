using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using _5S_QA_DL.APIContext;
using _5S_QA_Entities.Constants;
using _5S_QA_Entities.Dtos;
using _5S_QA_Entities.Models;
using _5S_QA_System.Controls;
using _5S_QA_System.Properties;
using MetroFramework.Controls;
using MetroFramework.Forms;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using CommonCpk = _5S_QA_Entities.Models.CommonCpk;
using DataCpk = _5S_QA_Entities.Models.DataCpk;

namespace _5S_QA_System;

public class frmCpkView : MetroForm
{
	private readonly Button mBtn;

	private readonly ProductViewModel mProduct;

	private readonly List<ChartViewModel> mModels;

	private List<Control> mControls;

	private readonly List<Bitmap> mBitmaps = new List<Bitmap>();

	private readonly string mTitle;

	private IContainer components = null;

	private SaveFileDialog saveFileDialogMain;

	private ToolTip toolTipMain;

	private Panel panelMain;

	private StatusStrip statusStripfrmMain;

	private ToolStripProgressBar sprogbarStatus;

	public ToolStripStatusLabel slblStatus;

	private MetroProgressSpinner prgMain;

	private TableLayoutPanel tpanelButtonView;

	public Button btnSaveToImage;

	public Button btnSaveToPdf;

	private Panel panelLogout;

	private Label lblFullname;

	private PictureBox ptbAvatar;

	public frmCpkView(string title, Button btn, ProductViewModel product, List<ChartViewModel> models)
	{
		InitializeComponent();
		Text = Text + " - " + title;
		mTitle = title;
		mBtn = btn;
		mProduct = product;
		mModels = models;
		Common.setControls(this, toolTipMain);
	}

	private void frmCpkView_Load(object sender, EventArgs e)
	{
		Init();
	}

	private void frmCpkView_Shown(object sender, EventArgs e)
	{
		new Thread((ThreadStart)delegate
		{
			Thread.CurrentThread.IsBackground = true;
			mControls = CalDataCpk();
			if (!base.IsDisposed)
			{
				Invoke((MethodInvoker)delegate
				{
					start_Proccessor();
					for (int num = mControls.Count; num > 0; num--)
					{
						panelMain.Controls.Add(mControls[num - 1]);
					}
					foreach (Control mControl in mControls)
					{
						mBitmaps.Add(ControlToBitmap(mControl));
					}
					debugOutput(Common.getTextLanguage(this, "Successful"));
					prgMain.Visible = false;
				});
			}
		}).Start();
	}

	private void frmCpkView_FormClosed(object sender, FormClosedEventArgs e)
	{
		GC.Collect();
	}

	private void Init()
	{
		lblFullname.Text = frmLogin.User.FullName;
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(APIUrl.APIHost + "/AuthUserImage/").Append(frmLogin.User.ImageUrl);
		try
		{
			ptbAvatar.Load(stringBuilder.ToString());
		}
		catch
		{
			ptbAvatar.Image = Resources._5S_QA_S;
		}
	}

	private double GetConstant(int n, string title)
	{
		double result = 0.0;
		try
		{
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "n=@0";
			queryArgs.PredicateParameters = new string[1] { n.ToString() };
			queryArgs.Order = "Created";
			queryArgs.Page = 1;
			queryArgs.Limit = 1;
			QueryArgs body = queryArgs;
			ResponseDto result2 = frmLogin.client.GetsAsync(body, "/api/Constant/Gets").Result;
			DataTable dataTable = Common.getDataTable<ConstantViewModel>(result2);
			if (dataTable.Rows.Count > 0)
			{
				result = (double)dataTable.Rows[0][title];
			}
		}
		finally
		{
		}
		return result;
	}

	private List<Control> CalDataCpk()
	{
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0305: Expected O, but got Unknown
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Expected O, but got Unknown
		//IL_0be7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c10: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c38: Expected O, but got Unknown
		IEnumerable<IGrouping<Guid?, ChartViewModel>> enumerable = from x in mModels
			group x by x.MeasurementId;
		List<Tuple<CommonCpk, List<DataCpk>, List<OtherDataCpk>, int, List<ChartViewModel>>> list = new List<Tuple<CommonCpk, List<DataCpk>, List<OtherDataCpk>, int, List<ChartViewModel>>>();
		foreach (IGrouping<Guid?, ChartViewModel> item in enumerable)
		{
			List<DataCpk> list2 = new List<DataCpk>();
			List<double> list3 = new List<double>();
			IEnumerable<IGrouping<Guid?, ChartViewModel>> enumerable2 = from x in item
				group x by x.RequestId;
			foreach (IGrouping<Guid?, ChartViewModel> item2 in enumerable2)
			{
				List<double> list4 = new List<double>();
				foreach (ChartViewModel item3 in item2)
				{
					list4.Add(double.Parse(item3.Result));
				}
				list3.AddRange(list4);
				DataCpk val = new DataCpk
				{
					Date = item2.First().RequestDate?.ToString("yyMMdd"),
					n = list4.Count,
					Average = list4.Average(),
					Range = list4.Max() - list4.Min(),
					Max = list4.Max(),
					Min = list4.Min(),
					SD = mMath.CalStandardDeviation(list4)
				};
				val.UpperErrorBars = val.Max - val.Average;
				val.LowerErrorBars = val.Average - val.Min;
				list2.Add(val);
			}
			CommonCpk commonCpk = new CommonCpk
			{
				ItemNo = item.First().Name,
				Tool = item.First().MachineTypeName,
				Unit = item.First().Unit,
				Standard = double.Parse(item.First().Value),
				Upper = item.First().Upper.Value,
				Lower = item.First().Lower.Value,
				n = list3.Count,
				Average = list3.Average(),
				Maximun = list3.Max(),
				Minimun = list3.Min(),
				SD = mMath.CalStandardDeviation(list3),
				Variance = mMath.CalVarianceP(list3),
				Summation = list3.Sum(),
				Rtb = list2.Average((DataCpk x) => x.Range)
			};
			commonCpk.nTerm = (commonCpk.Upper.Equals(9999.0) ? 2 : ((!commonCpk.Lower.Equals(-9999.0)) ? 1 : 3));
			commonCpk.USL = Math.Round(commonCpk.Standard + commonCpk.Upper, 4);
			commonCpk.LSL = Math.Round(commonCpk.Standard + commonCpk.Lower, 4);
			commonCpk.CL = commonCpk.Average;
			commonCpk.Sigma1 = commonCpk.CL + commonCpk.SD;
			commonCpk.MSigma1 = commonCpk.CL - commonCpk.SD;
			commonCpk.Sigma2 = commonCpk.Sigma1 + commonCpk.SD;
			commonCpk.MSigma2 = commonCpk.MSigma1 - commonCpk.SD;
			commonCpk.Sigma3 = commonCpk.Sigma2 + commonCpk.SD;
			commonCpk.MSigma3 = commonCpk.MSigma2 - commonCpk.SD;
			commonCpk.Sigma4 = commonCpk.Sigma3 + commonCpk.SD;
			commonCpk.MSigma4 = commonCpk.MSigma3 - commonCpk.SD;
			commonCpk.Sigma5 = commonCpk.Sigma4 + commonCpk.SD;
			commonCpk.MSigma5 = commonCpk.MSigma4 - commonCpk.SD;
			commonCpk.Sigma6 = commonCpk.Sigma5 + commonCpk.SD;
			commonCpk.MSigma6 = commonCpk.MSigma5 - commonCpk.SD;
			commonCpk.UCL = (item.First().UCL.HasValue ? item.First().UCL.Value : commonCpk.Sigma3);
			commonCpk.LCL = (item.First().LCL.HasValue ? item.First().LCL.Value : commonCpk.MSigma3);
			commonCpk.OutUSL = list3.Count((double x) => x > commonCpk.USL);
			commonCpk.OutLSL = list3.Count((double x) => x < commonCpk.LSL);
			commonCpk.OK = commonCpk.n - commonCpk.OutUSL - commonCpk.OutLSL;
			commonCpk.Range = commonCpk.Maximun - commonCpk.Minimun;
			commonCpk.Cp = (commonCpk.USL - commonCpk.LSL) / (6.0 * commonCpk.SD);
			commonCpk.HCpk = (commonCpk.USL - commonCpk.CL) / (3.0 * commonCpk.SD);
			commonCpk.LCpk = (commonCpk.CL - commonCpk.LSL) / (3.0 * commonCpk.SD);
			commonCpk.Cpk = Math.Min(commonCpk.HCpk, commonCpk.LCpk);
			double constant = GetConstant(list2.First().n, "A2");
			commonCpk.XUCL = commonCpk.Average + constant * commonCpk.Rtb;
			commonCpk.XLCL = commonCpk.Average - constant * commonCpk.Rtb;
			commonCpk.R = Math.Abs(commonCpk.Upper - commonCpk.Lower);
			commonCpk.AveR = commonCpk.Rtb;
			constant = GetConstant(list2.First().n, "D4");
			commonCpk.RUCL = constant * commonCpk.Rtb;
			constant = GetConstant(list2.First().n, "D3");
			commonCpk.RLCL = constant * commonCpk.Rtb;
			commonCpk.Varition = commonCpk.SD / commonCpk.Average;
			commonCpk.K = Math.Abs(commonCpk.USL + commonCpk.LSL - 2.0 * commonCpk.Average) / commonCpk.R;
			commonCpk.u1 = -3.0 * (1.0 - commonCpk.K) * commonCpk.Cp;
			commonCpk.u2 = 3.0 * (1.0 + commonCpk.K) * commonCpk.Cp;
			commonCpk.p1 = mMath.CalNormdist(commonCpk.u1);
			commonCpk.p2 = 1.0 - mMath.CalNormdist(commonCpk.u2);
			commonCpk.Defective = commonCpk.p1 + commonCpk.p2;
			commonCpk.LHTerm = (commonCpk.nTerm.Equals(3) ? (commonCpk.Minimun * 0.98) : commonCpk.LSL);
			commonCpk.RHTerm = (commonCpk.nTerm.Equals(2) ? (commonCpk.Maximun * 1.02) : commonCpk.USL);
			commonCpk.AveHTerm = (commonCpk.LHTerm + commonCpk.RHTerm) / 2.0;
			commonCpk.LowEnd = commonCpk.LHTerm - 2.0 * (commonCpk.RHTerm - commonCpk.LHTerm) / 5.0;
			commonCpk.HighEnd = commonCpk.RHTerm + 2.0 * (commonCpk.RHTerm - commonCpk.LHTerm) / 5.0;
			commonCpk.Min = commonCpk.LowEnd;
			commonCpk.Max = commonCpk.HighEnd;
			commonCpk.Diff = commonCpk.Max - commonCpk.Min;
			commonCpk.Class = commonCpk.Diff / 13.0;
			List<OtherDataCpk> list5 = new List<OtherDataCpk>();
			for (int num = 0; num < 15; num++)
			{
				OtherDataCpk val2 = new OtherDataCpk
				{
					Term1 = commonCpk.Min + (double)num * commonCpk.Class,
					Term2 = commonCpk.Min + (double)(num - 1) * commonCpk.Class
				};
				val2.Avegare = (val2.Term1 + val2.Term2) / 2.0;
				List<double> bins = new List<double>
				{
					num.Equals(0) ? 0.0 : list5[num - 1].Term1,
					val2.Term1
				};
				val2.Frequency = mMath.CalFrequency(list3, bins);
				val2.NormalDist = mMath.CalNormdist(val2.Avegare, commonCpk.Average, commonCpk.SD, cumulative: false);
				list5.Add(val2);
			}
			list.Add(Tuple.Create<CommonCpk, List<DataCpk>, List<OtherDataCpk>, int, List<ChartViewModel>>(commonCpk, list2, list5, list5.Max((OtherDataCpk x) => x.Frequency), item.ToList()));
		}
		List<object> dates = new List<object>
		{
			enumerable.First().First().RequestDate,
			enumerable.First().Last().RequestDate,
			enumerable.First().First().Lot,
			enumerable.First().Last().Lot,
			enumerable.First().First().Created,
			enumerable.First().Last().Created,
			enumerable.First().First().Line,
			enumerable.First().Last().Line
		};
		List<Control> list6 = new List<Control>
		{
			new mInformation(mBtn.Text, mProduct, dates)
			{
				Dock = DockStyle.Top,
				Size = new Size(944, 110)
			}
		};
		foreach (Tuple<CommonCpk, List<DataCpk>, List<OtherDataCpk>, int, List<ChartViewModel>> item4 in list)
		{
			switch (mBtn.Name)
			{
			case "btnQualityReport":
				list6.Add(new mQualityReport(item4.Item1, item4.Item3, (double)list.Max((Tuple<CommonCpk, List<DataCpk>, List<OtherDataCpk>, int, List<ChartViewModel>> x) => x.Item4) * 1.02, item4.Item5)
				{
					Dock = DockStyle.Top,
					Size = new Size(944, 290)
				});
				break;
			case "btnHistogram":
				list6.Add(new mHistogram(item4.Item1, item4.Item3, item4.Item5)
				{
					Dock = DockStyle.Top,
					Size = new Size(944, 390)
				});
				break;
			case "btnXControlN1":
				list6.Add(new mXControl(item4.Item1, item4.Item2, item4.Item5)
				{
					Dock = DockStyle.Top,
					Size = new Size(944, 290)
				});
				break;
			case "btnXtbControl":
				list6.Add(new mXtbControl(item4.Item1, item4.Item2, item4.Item5)
				{
					Dock = DockStyle.Top,
					Size = new Size(944, 290)
				});
				break;
			case "btnXtbRControl":
				list6.Add(new mXtbRControl(item4.Item1, item4.Item2, item4.Item5)
				{
					Dock = DockStyle.Top,
					Size = new Size(944, 390)
				});
				break;
			case "btnXtbSControl":
				list6.Add(new mXtbSControl(item4.Item1, item4.Item2, item4.Item5)
				{
					Dock = DockStyle.Top,
					Size = new Size(944, 390)
				});
				break;
			case "btnInterpreting":
			{
				Dictionary<string, List<int>> dictionary = new Dictionary<string, List<int>>();
				List<int> list7 = new List<int>();
				List<int> list8 = new List<int>();
				List<int> list9 = new List<int>();
				List<int> list10 = new List<int>();
				List<int> list11 = new List<int>();
				List<int> list12 = new List<int>();
				List<int> list13 = new List<int>();
				List<int> list14 = new List<int>();
				Dictionary<string, List<int>> dictionary2 = new Dictionary<string, List<int>>();
				List<int> list15 = new List<int>();
				List<int> list16 = new List<int>();
				List<int> list17 = new List<int>();
				List<int> list18 = new List<int>();
				foreach (DataCpk item5 in item4.Item2)
				{
					int num2 = item4.Item2.IndexOf(item5);
					if (item5.Average > item4.Item1.UCL || item5.Average < item4.Item1.LCL)
					{
						list7.Add(num2);
					}
					if (num2 > 1 && ((item4.Item2[num2 - 2].Average > item4.Item1.Sigma2 && item4.Item2[num2 - 1].Average > item4.Item1.Sigma2) || (item4.Item2[num2 - 2].Average > item4.Item1.Sigma2 && item4.Item2[num2].Average > item4.Item1.Sigma2) || (item4.Item2[num2 - 1].Average > item4.Item1.Sigma2 && item4.Item2[num2].Average > item4.Item1.Sigma2) || (item4.Item2[num2 - 2].Average < item4.Item1.MSigma2 && item4.Item2[num2 - 1].Average < item4.Item1.MSigma2) || (item4.Item2[num2 - 2].Average < item4.Item1.MSigma2 && item4.Item2[num2].Average < item4.Item1.MSigma2) || (item4.Item2[num2 - 1].Average < item4.Item1.MSigma2 && item4.Item2[num2].Average < item4.Item1.MSigma2)))
					{
						list8.Add(num2);
					}
					if (num2 > 3 && ((item4.Item2[num2 - 4].Average > item4.Item1.Sigma1 && item4.Item2[num2 - 3].Average > item4.Item1.Sigma1 && item4.Item2[num2 - 2].Average > item4.Item1.Sigma1 && item4.Item2[num2 - 1].Average > item4.Item1.Sigma1) || (item4.Item2[num2 - 4].Average > item4.Item1.Sigma1 && item4.Item2[num2 - 3].Average > item4.Item1.Sigma1 && item4.Item2[num2 - 2].Average > item4.Item1.Sigma1 && item4.Item2[num2].Average > item4.Item1.Sigma1) || (item4.Item2[num2 - 4].Average > item4.Item1.Sigma1 && item4.Item2[num2 - 3].Average > item4.Item1.Sigma1 && item4.Item2[num2 - 1].Average > item4.Item1.Sigma1 && item4.Item2[num2].Average > item4.Item1.Sigma1) || (item4.Item2[num2 - 4].Average > item4.Item1.Sigma1 && item4.Item2[num2 - 2].Average > item4.Item1.Sigma1 && item4.Item2[num2 - 1].Average > item4.Item1.Sigma1 && item4.Item2[num2].Average > item4.Item1.Sigma1) || (item4.Item2[num2 - 3].Average > item4.Item1.Sigma1 && item4.Item2[num2 - 2].Average > item4.Item1.Sigma1 && item4.Item2[num2 - 1].Average > item4.Item1.Sigma1 && item4.Item2[num2].Average > item4.Item1.Sigma1) || (item4.Item2[num2 - 4].Average < item4.Item1.MSigma1 && item4.Item2[num2 - 3].Average < item4.Item1.MSigma1 && item4.Item2[num2 - 2].Average < item4.Item1.MSigma1 && item4.Item2[num2 - 1].Average < item4.Item1.MSigma1) || (item4.Item2[num2 - 4].Average < item4.Item1.MSigma1 && item4.Item2[num2 - 3].Average < item4.Item1.MSigma1 && item4.Item2[num2 - 2].Average < item4.Item1.MSigma1 && item4.Item2[num2].Average < item4.Item1.MSigma1) || (item4.Item2[num2 - 4].Average < item4.Item1.MSigma1 && item4.Item2[num2 - 3].Average < item4.Item1.MSigma1 && item4.Item2[num2 - 1].Average < item4.Item1.MSigma1 && item4.Item2[num2].Average < item4.Item1.MSigma1) || (item4.Item2[num2 - 4].Average < item4.Item1.MSigma1 && item4.Item2[num2 - 2].Average < item4.Item1.MSigma1 && item4.Item2[num2 - 1].Average < item4.Item1.MSigma1 && item4.Item2[num2].Average < item4.Item1.MSigma1) || (item4.Item2[num2 - 3].Average < item4.Item1.MSigma1 && item4.Item2[num2 - 2].Average < item4.Item1.MSigma1 && item4.Item2[num2 - 1].Average < item4.Item1.MSigma1 && item4.Item2[num2].Average < item4.Item1.MSigma1)))
					{
						list9.Add(num2);
					}
					if (num2 > 4 && ((item4.Item2[num2 - 5].Average > item4.Item2[num2 - 4].Average && item4.Item2[num2 - 4].Average > item4.Item2[num2 - 3].Average && item4.Item2[num2 - 3].Average > item4.Item2[num2 - 2].Average && item4.Item2[num2 - 2].Average > item4.Item2[num2 - 1].Average && item4.Item2[num2 - 1].Average > item4.Item2[num2].Average) || (item4.Item2[num2 - 5].Average < item4.Item2[num2 - 4].Average && item4.Item2[num2 - 4].Average < item4.Item2[num2 - 3].Average && item4.Item2[num2 - 3].Average < item4.Item2[num2 - 2].Average && item4.Item2[num2 - 2].Average < item4.Item2[num2 - 1].Average && item4.Item2[num2 - 1].Average < item4.Item2[num2].Average)))
					{
						list10.Add(num2);
					}
					if (num2 > 5 && ((item4.Item2[num2 - 6].Average > item4.Item1.CL && item4.Item2[num2 - 5].Average > item4.Item1.CL && item4.Item2[num2 - 4].Average > item4.Item1.CL && item4.Item2[num2 - 3].Average > item4.Item1.CL && item4.Item2[num2 - 2].Average > item4.Item1.CL && item4.Item2[num2 - 1].Average > item4.Item1.CL && item4.Item2[num2].Average > item4.Item1.CL) || (item4.Item2[num2 - 6].Average < item4.Item1.CL && item4.Item2[num2 - 5].Average < item4.Item1.CL && item4.Item2[num2 - 4].Average < item4.Item1.CL && item4.Item2[num2 - 3].Average < item4.Item1.CL && item4.Item2[num2 - 2].Average < item4.Item1.CL && item4.Item2[num2 - 1].Average < item4.Item1.CL && item4.Item2[num2].Average < item4.Item1.CL)))
					{
						list11.Add(num2);
					}
					if (num2 > 6 && (item4.Item2[num2 - 7].Average > item4.Item1.Sigma1 || item4.Item2[num2 - 7].Average < item4.Item1.MSigma1) && (item4.Item2[num2 - 6].Average > item4.Item1.Sigma1 || item4.Item2[num2 - 6].Average < item4.Item1.MSigma1) && (item4.Item2[num2 - 5].Average > item4.Item1.Sigma1 || item4.Item2[num2 - 5].Average < item4.Item1.MSigma1) && (item4.Item2[num2 - 4].Average > item4.Item1.Sigma1 || item4.Item2[num2 - 4].Average < item4.Item1.MSigma1) && (item4.Item2[num2 - 3].Average > item4.Item1.Sigma1 || item4.Item2[num2 - 3].Average < item4.Item1.MSigma1) && (item4.Item2[num2 - 2].Average > item4.Item1.Sigma1 || item4.Item2[num2 - 2].Average < item4.Item1.MSigma1) && (item4.Item2[num2 - 1].Average > item4.Item1.Sigma1 || item4.Item2[num2 - 1].Average < item4.Item1.MSigma1) && (item4.Item2[num2].Average > item4.Item1.Sigma1 || item4.Item2[num2].Average < item4.Item1.MSigma1))
					{
						list12.Add(num2);
					}
					if (num2 > 12 && ((item4.Item2[num2 - 13].Average > item4.Item2[num2 - 12].Average && item4.Item2[num2 - 12].Average < item4.Item2[num2 - 11].Average && item4.Item2[num2 - 11].Average > item4.Item2[num2 - 10].Average && item4.Item2[num2 - 10].Average < item4.Item2[num2 - 9].Average && item4.Item2[num2 - 9].Average > item4.Item2[num2 - 8].Average && item4.Item2[num2 - 8].Average < item4.Item2[num2 - 7].Average && item4.Item2[num2 - 7].Average > item4.Item2[num2 - 6].Average && item4.Item2[num2 - 6].Average < item4.Item2[num2 - 5].Average && item4.Item2[num2 - 5].Average > item4.Item2[num2 - 4].Average && item4.Item2[num2 - 4].Average < item4.Item2[num2 - 3].Average && item4.Item2[num2 - 3].Average > item4.Item2[num2 - 2].Average && item4.Item2[num2 - 2].Average < item4.Item2[num2 - 1].Average && item4.Item2[num2 - 1].Average > item4.Item2[num2].Average) || (item4.Item2[num2 - 13].Average < item4.Item2[num2 - 12].Average && item4.Item2[num2 - 12].Average > item4.Item2[num2 - 11].Average && item4.Item2[num2 - 11].Average < item4.Item2[num2 - 10].Average && item4.Item2[num2 - 10].Average > item4.Item2[num2 - 9].Average && item4.Item2[num2 - 9].Average < item4.Item2[num2 - 8].Average && item4.Item2[num2 - 8].Average > item4.Item2[num2 - 7].Average && item4.Item2[num2 - 7].Average < item4.Item2[num2 - 6].Average && item4.Item2[num2 - 6].Average > item4.Item2[num2 - 5].Average && item4.Item2[num2 - 5].Average < item4.Item2[num2 - 4].Average && item4.Item2[num2 - 4].Average > item4.Item2[num2 - 3].Average && item4.Item2[num2 - 3].Average < item4.Item2[num2 - 2].Average && item4.Item2[num2 - 2].Average > item4.Item2[num2 - 1].Average && item4.Item2[num2 - 1].Average < item4.Item2[num2].Average)))
					{
						list13.Add(num2);
					}
					if (num2 > 13 && item4.Item2[num2 - 14].Average < item4.Item1.Sigma1 && item4.Item2[num2 - 14].Average > item4.Item1.MSigma1 && item4.Item2[num2 - 13].Average < item4.Item1.Sigma1 && item4.Item2[num2 - 13].Average > item4.Item1.MSigma1 && item4.Item2[num2 - 12].Average < item4.Item1.Sigma1 && item4.Item2[num2 - 12].Average > item4.Item1.MSigma1 && item4.Item2[num2 - 11].Average < item4.Item1.Sigma1 && item4.Item2[num2 - 11].Average > item4.Item1.MSigma1 && item4.Item2[num2 - 10].Average < item4.Item1.Sigma1 && item4.Item2[num2 - 10].Average > item4.Item1.MSigma1 && item4.Item2[num2 - 9].Average < item4.Item1.Sigma1 && item4.Item2[num2 - 9].Average > item4.Item1.MSigma1 && item4.Item2[num2 - 8].Average < item4.Item1.Sigma1 && item4.Item2[num2 - 8].Average > item4.Item1.MSigma1 && item4.Item2[num2 - 7].Average < item4.Item1.Sigma1 && item4.Item2[num2 - 7].Average > item4.Item1.MSigma1 && item4.Item2[num2 - 6].Average < item4.Item1.Sigma1 && item4.Item2[num2 - 6].Average > item4.Item1.MSigma1 && item4.Item2[num2 - 5].Average < item4.Item1.Sigma1 && item4.Item2[num2 - 5].Average > item4.Item1.MSigma1 && item4.Item2[num2 - 4].Average < item4.Item1.Sigma1 && item4.Item2[num2 - 4].Average > item4.Item1.MSigma1 && item4.Item2[num2 - 3].Average < item4.Item1.Sigma1 && item4.Item2[num2 - 3].Average > item4.Item1.MSigma1 && item4.Item2[num2 - 2].Average < item4.Item1.Sigma1 && item4.Item2[num2 - 2].Average > item4.Item1.MSigma1 && item4.Item2[num2 - 1].Average < item4.Item1.Sigma1 && item4.Item2[num2 - 1].Average > item4.Item1.MSigma1 && item4.Item2[num2].Average < item4.Item1.Sigma1 && item4.Item2[num2].Average > item4.Item1.MSigma1)
					{
						list14.Add(num2);
					}
					if (item5.Range > item4.Item1.RUCL)
					{
						list15.Add(num2);
					}
					if (num2 > 4 && ((item4.Item2[num2 - 5].Range > item4.Item2[num2 - 4].Range && item4.Item2[num2 - 4].Range > item4.Item2[num2 - 3].Range && item4.Item2[num2 - 3].Range > item4.Item2[num2 - 2].Range && item4.Item2[num2 - 2].Range > item4.Item2[num2 - 1].Range && item4.Item2[num2 - 1].Range > item4.Item2[num2].Range) || (item4.Item2[num2 - 5].Range < item4.Item2[num2 - 4].Range && item4.Item2[num2 - 4].Range < item4.Item2[num2 - 3].Range && item4.Item2[num2 - 3].Range < item4.Item2[num2 - 2].Range && item4.Item2[num2 - 2].Range < item4.Item2[num2 - 1].Range && item4.Item2[num2 - 1].Range < item4.Item2[num2].Range)))
					{
						list16.Add(num2);
					}
					if (num2 > 5 && ((item4.Item2[num2 - 6].Range > item4.Item1.AveR && item4.Item2[num2 - 5].Range > item4.Item1.AveR && item4.Item2[num2 - 4].Range > item4.Item1.AveR && item4.Item2[num2 - 3].Range > item4.Item1.AveR && item4.Item2[num2 - 2].Range > item4.Item1.AveR && item4.Item2[num2 - 1].Range > item4.Item1.AveR && item4.Item2[num2].Range > item4.Item1.AveR) || (item4.Item2[num2 - 6].Range < item4.Item1.AveR && item4.Item2[num2 - 5].Range < item4.Item1.AveR && item4.Item2[num2 - 4].Range < item4.Item1.AveR && item4.Item2[num2 - 3].Range < item4.Item1.AveR && item4.Item2[num2 - 2].Range < item4.Item1.AveR && item4.Item2[num2 - 1].Range < item4.Item1.AveR && item4.Item2[num2].Range < item4.Item1.AveR)))
					{
						list17.Add(num2);
					}
					if (num2 > 12 && ((item4.Item2[num2 - 13].Range > item4.Item2[num2 - 12].Range && item4.Item2[num2 - 12].Range < item4.Item2[num2 - 11].Range && item4.Item2[num2 - 11].Range > item4.Item2[num2 - 10].Range && item4.Item2[num2 - 10].Range < item4.Item2[num2 - 9].Range && item4.Item2[num2 - 9].Range > item4.Item2[num2 - 8].Range && item4.Item2[num2 - 8].Range < item4.Item2[num2 - 7].Range && item4.Item2[num2 - 7].Range > item4.Item2[num2 - 6].Range && item4.Item2[num2 - 6].Range < item4.Item2[num2 - 5].Range && item4.Item2[num2 - 5].Range > item4.Item2[num2 - 4].Range && item4.Item2[num2 - 4].Range < item4.Item2[num2 - 3].Range && item4.Item2[num2 - 3].Range > item4.Item2[num2 - 2].Range && item4.Item2[num2 - 2].Range < item4.Item2[num2 - 1].Range && item4.Item2[num2 - 1].Range > item4.Item2[num2].Range) || (item4.Item2[num2 - 13].Range < item4.Item2[num2 - 12].Range && item4.Item2[num2 - 12].Range > item4.Item2[num2 - 11].Range && item4.Item2[num2 - 11].Range < item4.Item2[num2 - 10].Range && item4.Item2[num2 - 10].Range > item4.Item2[num2 - 9].Range && item4.Item2[num2 - 9].Range < item4.Item2[num2 - 8].Range && item4.Item2[num2 - 8].Range > item4.Item2[num2 - 7].Range && item4.Item2[num2 - 7].Range < item4.Item2[num2 - 6].Range && item4.Item2[num2 - 6].Range > item4.Item2[num2 - 5].Range && item4.Item2[num2 - 5].Range < item4.Item2[num2 - 4].Range && item4.Item2[num2 - 4].Range > item4.Item2[num2 - 3].Range && item4.Item2[num2 - 3].Range < item4.Item2[num2 - 2].Range && item4.Item2[num2 - 2].Range > item4.Item2[num2 - 1].Range && item4.Item2[num2 - 1].Range < item4.Item2[num2].Range)))
					{
						list18.Add(num2);
					}
				}
				string interpreting = Settings.Default.Interpreting;
				if (interpreting.Contains("cbXtb1") && list7.Count > 0)
				{
					dictionary.Add(Common.getTextLanguage(this, "(1)"), list7);
				}
				if (interpreting.Contains("cbXtb2") && list8.Count > 0)
				{
					dictionary.Add(Common.getTextLanguage(this, "(2)"), list8);
				}
				if (interpreting.Contains("cbXtb3") && list9.Count > 0)
				{
					dictionary.Add(Common.getTextLanguage(this, "(3)"), list9);
				}
				if (interpreting.Contains("cbXtb4") && list10.Count > 0)
				{
					dictionary.Add(Common.getTextLanguage(this, "(4)"), list10);
				}
				if (interpreting.Contains("cbXtb5") && list11.Count > 0)
				{
					dictionary.Add(Common.getTextLanguage(this, "(5)"), list11);
				}
				if (interpreting.Contains("cbXtb6") && list12.Count > 0)
				{
					dictionary.Add(Common.getTextLanguage(this, "(6)"), list12);
				}
				if (interpreting.Contains("cbXtb7") && list13.Count > 0)
				{
					dictionary.Add(Common.getTextLanguage(this, "(7)"), list13);
				}
				if (interpreting.Contains("cbXtb8") && list14.Count > 0)
				{
					dictionary.Add(Common.getTextLanguage(this, "(8)"), list14);
				}
				if (interpreting.Contains("cbR1") && list15.Count > 0)
				{
					dictionary2.Add(Common.getTextLanguage(this, "(R1)"), list15);
				}
				if (interpreting.Contains("cbR2") && list16.Count > 0)
				{
					dictionary2.Add(Common.getTextLanguage(this, "(R2)"), list16);
				}
				if (interpreting.Contains("cbR3") && list17.Count > 0)
				{
					dictionary2.Add(Common.getTextLanguage(this, "(R3)"), list17);
				}
				if (interpreting.Contains("cbR4") && list18.Count > 0)
				{
					dictionary2.Add(Common.getTextLanguage(this, "(R4)"), list18);
				}
				if (dictionary.Count <= 0 && dictionary2.Count <= 0)
				{
					break;
				}
				list6.Add(new mInterpretingControl(item4.Item1)
				{
					Dock = DockStyle.Top,
					Size = new Size(944, 70)
				});
				foreach (KeyValuePair<string, List<int>> item6 in dictionary)
				{
					list6.Add(new mChartInterpreting(item4.Item1, item4.Item2, item6)
					{
						Dock = DockStyle.Top,
						Size = new Size(944, 150)
					});
				}
				foreach (KeyValuePair<string, List<int>> item7 in dictionary2)
				{
					list6.Add(new mChartRInterpreting(item4.Item1, item4.Item2, item7)
					{
						Dock = DockStyle.Top,
						Size = new Size(944, 100)
					});
				}
				break;
			}
			}
		}
		return list6;
	}

	private void debugOutput(string strDebugText)
	{
		slblStatus.Text = strDebugText;
	}

	private void start_Proccessor()
	{
		sprogbarStatus.Maximum = 100;
		sprogbarStatus.Value = 0;
		sprogbarStatus.Value = 100;
	}

	private Bitmap ControlToBitmap(Control control)
	{
		Bitmap bitmap = new Bitmap(control.Width, control.Height);
		control.DrawToBitmap(bitmap, new Rectangle(0, 0, control.Width, bitmap.Height));
		return bitmap;
	}

	private Bitmap MergeImages(List<Bitmap> images)
	{
		int num = images.Max((Bitmap x) => x.Width);
		int num2 = images.Sum((Bitmap x) => x.Height);
		Bitmap bitmap = new Bitmap(num, num2);
		using (Graphics graphics = Graphics.FromImage(bitmap))
		{
			int num3 = 0;
			foreach (Bitmap image in images)
			{
				graphics.DrawImage(image, 0, num3);
				num3 += image.Height;
			}
		}
		return bitmap;
	}

	private int DrawImagePosY(XGraphics gfx, Bitmap bitmap, int xpos, int ypos)
	{
		int num = 0;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			bitmap.Save(memoryStream, ImageFormat.Bmp);
			XImage val = XImage.FromStream((Stream)memoryStream);
			double num2 = 595.0 - (double)xpos * 1.5;
			num = (int)Math.Ceiling(num2 * (double)val.PixelHeight / (double)val.PixelWidth);
			gfx.DrawImage(val, (double)xpos, (double)ypos, num2, (double)num);
		}
		return num;
	}

	private int CalTotalPage(int XPOS, int YPOS)
	{
		int num = 1;
		int num2 = YPOS;
		foreach (Bitmap mBitmap in mBitmaps)
		{
			double num3 = 595.0 - (double)XPOS * 1.5;
			int num4 = (int)Math.Ceiling(num3 * (double)mBitmap.Height / (double)mBitmap.Width);
			num2 += num4;
			if ((double)num2 > 842.0 - (double)YPOS * 1.5 - (double)num4 && !mBitmaps.Last().Equals(mBitmap))
			{
				num2 = YPOS;
				num4 = (int)Math.Ceiling(num3 * (double)mBitmaps[0].Height / (double)mBitmaps[0].Width);
				num2 += num4;
				num++;
			}
		}
		return num;
	}

	private void ptbAvatar_DoubleClick(object sender, EventArgs e)
	{
		Common.activeForm(typeof(frmLogin));
	}

	private void btnSaveToImage_Click(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		if (!mControls.Count.Equals(0))
		{
			saveFileDialogMain.FileName = mTitle;
			saveFileDialogMain.Filter = "File image (*.jpg)|*.jpg";
			if (saveFileDialogMain.ShowDialog().Equals(DialogResult.OK))
			{
				Bitmap bitmap = MergeImages(mBitmaps);
				bitmap.Save(saveFileDialogMain.FileName, ImageFormat.Jpeg);
				Common.ExecuteBatFile(saveFileDialogMain.FileName);
			}
		}
	}

	private void btnSaveToPdf_Click(object sender, EventArgs e)
	{
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Expected O, but got Unknown
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Expected O, but got Unknown
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Expected O, but got Unknown
		Cursor.Current = Cursors.WaitCursor;
		if (mControls.Count.Equals(0))
		{
			return;
		}
		saveFileDialogMain.FileName = mTitle;
		saveFileDialogMain.Filter = "File pdf (*.pdf)|*.pdf";
		if (!saveFileDialogMain.ShowDialog().Equals(DialogResult.OK))
		{
			return;
		}
		try
		{
			if (Common.FileInUse(saveFileDialogMain.FileName))
			{
				throw new Exception(Common.getTextLanguage(this, "wOpenning"));
			}
			PdfDocument val = new PdfDocument();
			try
			{
				val.Info.Title = "5SQA System - A&A Technology";
				int num = 40;
				int num2 = 40;
				int num3 = 0;
				int num4 = CalTotalPage(num, num2);
				int num5 = num2;
				PdfPage val2 = new PdfPage
				{
					Size = (PageSize)5,
					Orientation = (PageOrientation)0
				};
				val.Pages.Add(val2);
				XGraphics val3 = XGraphics.FromPdfPage(val2);
				XFont val4 = new XFont("Microsoft Sans Serif", 9.75);
				val3.DrawString("Export by 5SQA System", val4, (XBrush)(object)XBrushes.Black, (double)num, (double)(num2 / 2));
				val3.DrawString($"Page {val.Pages.Count}/{num4}", val4, (XBrush)(object)XBrushes.Black, (double)(565 - num), (double)(842 - num2 / 2));
				foreach (Bitmap mBitmap in mBitmaps)
				{
					num3 = DrawImagePosY(val3, mBitmap, num, num5);
					num5 += num3;
					if ((double)num5 > 842.0 - (double)num2 * 1.5 - (double)num3 && !mBitmaps.Last().Equals(mBitmap))
					{
						val2 = new PdfPage
						{
							Size = (PageSize)5,
							Orientation = (PageOrientation)0
						};
						val.Pages.Add(val2);
						val3 = XGraphics.FromPdfPage(val2);
						val3.DrawString("Export by 5SQA System", val4, (XBrush)(object)XBrushes.Black, (double)num, (double)(num2 / 2));
						val3.DrawString($"Page {val.Pages.Count}/{num4}", val4, (XBrush)(object)XBrushes.Black, (double)(565 - num), (double)(842 - num2 / 2));
						num5 = num2;
						num3 = DrawImagePosY(val3, mBitmaps[0], num, num5);
						num5 += num3;
					}
				}
				val.Save(saveFileDialogMain.FileName);
				Common.ExecuteBatFile(saveFileDialogMain.FileName);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_5S_QA_System.frmCpkView));
		this.saveFileDialogMain = new System.Windows.Forms.SaveFileDialog();
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.btnSaveToImage = new System.Windows.Forms.Button();
		this.btnSaveToPdf = new System.Windows.Forms.Button();
		this.panelMain = new System.Windows.Forms.Panel();
		this.statusStripfrmMain = new System.Windows.Forms.StatusStrip();
		this.sprogbarStatus = new System.Windows.Forms.ToolStripProgressBar();
		this.slblStatus = new System.Windows.Forms.ToolStripStatusLabel();
		this.prgMain = new MetroFramework.Controls.MetroProgressSpinner();
		this.tpanelButtonView = new System.Windows.Forms.TableLayoutPanel();
		this.panelLogout = new System.Windows.Forms.Panel();
		this.lblFullname = new System.Windows.Forms.Label();
		this.ptbAvatar = new System.Windows.Forms.PictureBox();
		this.statusStripfrmMain.SuspendLayout();
		this.tpanelButtonView.SuspendLayout();
		this.panelLogout.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.ptbAvatar).BeginInit();
		base.SuspendLayout();
		this.saveFileDialogMain.Filter = "File pdf (*.pdf)|*.pdf";
		this.saveFileDialogMain.Title = "Select folder and enter file name";
		this.btnSaveToImage.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnSaveToImage.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnSaveToImage.FlatAppearance.BorderSize = 0;
		this.btnSaveToImage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
		this.btnSaveToImage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
		this.btnSaveToImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSaveToImage.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnSaveToImage.Location = new System.Drawing.Point(1, 1);
		this.btnSaveToImage.Margin = new System.Windows.Forms.Padding(1);
		this.btnSaveToImage.Name = "btnSaveToImage";
		this.btnSaveToImage.Size = new System.Drawing.Size(478, 24);
		this.btnSaveToImage.TabIndex = 2;
		this.btnSaveToImage.Text = "Save to image";
		this.toolTipMain.SetToolTip(this.btnSaveToImage, "Select save to image");
		this.btnSaveToImage.UseVisualStyleBackColor = true;
		this.btnSaveToImage.Click += new System.EventHandler(btnSaveToImage_Click);
		this.btnSaveToPdf.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnSaveToPdf.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnSaveToPdf.FlatAppearance.BorderSize = 0;
		this.btnSaveToPdf.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
		this.btnSaveToPdf.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
		this.btnSaveToPdf.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSaveToPdf.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnSaveToPdf.Location = new System.Drawing.Point(481, 1);
		this.btnSaveToPdf.Margin = new System.Windows.Forms.Padding(1);
		this.btnSaveToPdf.Name = "btnSaveToPdf";
		this.btnSaveToPdf.Size = new System.Drawing.Size(478, 24);
		this.btnSaveToPdf.TabIndex = 3;
		this.btnSaveToPdf.Text = "Save to pdf";
		this.toolTipMain.SetToolTip(this.btnSaveToPdf, "Select save to pdf");
		this.btnSaveToPdf.UseVisualStyleBackColor = true;
		this.btnSaveToPdf.Click += new System.EventHandler(btnSaveToPdf_Click);
		this.panelMain.AutoScroll = true;
		this.panelMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panelMain.Location = new System.Drawing.Point(20, 70);
		this.panelMain.Name = "panelMain";
		this.panelMain.Size = new System.Drawing.Size(960, 558);
		this.panelMain.TabIndex = 165;
		this.statusStripfrmMain.BackColor = System.Drawing.SystemColors.ControlLight;
		this.statusStripfrmMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.statusStripfrmMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.sprogbarStatus, this.slblStatus });
		this.statusStripfrmMain.Location = new System.Drawing.Point(20, 654);
		this.statusStripfrmMain.Name = "statusStripfrmMain";
		this.statusStripfrmMain.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
		this.statusStripfrmMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
		this.statusStripfrmMain.Size = new System.Drawing.Size(960, 26);
		this.statusStripfrmMain.SizingGrip = false;
		this.statusStripfrmMain.Stretch = false;
		this.statusStripfrmMain.TabIndex = 166;
		this.sprogbarStatus.Name = "sprogbarStatus";
		this.sprogbarStatus.Size = new System.Drawing.Size(200, 20);
		this.slblStatus.Name = "slblStatus";
		this.slblStatus.Size = new System.Drawing.Size(16, 21);
		this.slblStatus.Text = "...";
		this.prgMain.Anchor = System.Windows.Forms.AnchorStyles.None;
		this.prgMain.Location = new System.Drawing.Point(475, 326);
		this.prgMain.Maximum = 100;
		this.prgMain.Name = "prgMain";
		this.prgMain.Size = new System.Drawing.Size(50, 50);
		this.prgMain.Speed = 3f;
		this.prgMain.TabIndex = 169;
		this.prgMain.UseSelectable = true;
		this.prgMain.Value = 99;
		this.tpanelButtonView.AutoSize = true;
		this.tpanelButtonView.ColumnCount = 2;
		this.tpanelButtonView.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tpanelButtonView.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tpanelButtonView.Controls.Add(this.btnSaveToPdf, 1, 0);
		this.tpanelButtonView.Controls.Add(this.btnSaveToImage, 0, 0);
		this.tpanelButtonView.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.tpanelButtonView.Location = new System.Drawing.Point(20, 628);
		this.tpanelButtonView.Margin = new System.Windows.Forms.Padding(0);
		this.tpanelButtonView.Name = "tpanelButtonView";
		this.tpanelButtonView.RowCount = 1;
		this.tpanelButtonView.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tpanelButtonView.Size = new System.Drawing.Size(960, 26);
		this.tpanelButtonView.TabIndex = 170;
		this.panelLogout.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.panelLogout.BackColor = System.Drawing.Color.Transparent;
		this.panelLogout.Controls.Add(this.lblFullname);
		this.panelLogout.Controls.Add(this.ptbAvatar);
		this.panelLogout.Location = new System.Drawing.Point(630, 25);
		this.panelLogout.Name = "panelLogout";
		this.panelLogout.Size = new System.Drawing.Size(350, 42);
		this.panelLogout.TabIndex = 176;
		this.panelLogout.TabStop = true;
		this.lblFullname.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.lblFullname.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f);
		this.lblFullname.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.lblFullname.Location = new System.Drawing.Point(0, 26);
		this.lblFullname.Name = "lblFullname";
		this.lblFullname.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
		this.lblFullname.Size = new System.Drawing.Size(308, 16);
		this.lblFullname.TabIndex = 34;
		this.lblFullname.Text = "...";
		this.lblFullname.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.ptbAvatar.Dock = System.Windows.Forms.DockStyle.Right;
		this.ptbAvatar.Image = _5S_QA_System.Properties.Resources._5S_QA_S;
		this.ptbAvatar.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.ptbAvatar.Location = new System.Drawing.Point(308, 0);
		this.ptbAvatar.Margin = new System.Windows.Forms.Padding(4);
		this.ptbAvatar.Name = "ptbAvatar";
		this.ptbAvatar.Size = new System.Drawing.Size(42, 42);
		this.ptbAvatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.ptbAvatar.TabIndex = 14;
		this.ptbAvatar.TabStop = false;
		this.ptbAvatar.DoubleClick += new System.EventHandler(ptbAvatar_DoubleClick);
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1000, 700);
		base.Controls.Add(this.panelLogout);
		base.Controls.Add(this.prgMain);
		base.Controls.Add(this.panelMain);
		base.Controls.Add(this.tpanelButtonView);
		base.Controls.Add(this.statusStripfrmMain);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "frmCpkView";
		base.Padding = new System.Windows.Forms.Padding(20, 70, 20, 20);
		base.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
		this.Text = "5S QA System * Cpk";
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmCpkView_FormClosed);
		base.Load += new System.EventHandler(frmCpkView_Load);
		base.Shown += new System.EventHandler(frmCpkView_Shown);
		this.statusStripfrmMain.ResumeLayout(false);
		this.statusStripfrmMain.PerformLayout();
		this.tpanelButtonView.ResumeLayout(false);
		this.panelLogout.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.ptbAvatar).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
