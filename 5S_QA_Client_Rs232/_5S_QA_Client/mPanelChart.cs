using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using _5S_QA_Client.Controls;
using _5S_QA_DL.APIContext;
using _5S_QA_Entities.Dtos;
using _5S_QA_Entities.Models;
using CommonCpk = _5S_QA_Entities.Models.CommonCpk;
using DataCpk = _5S_QA_Entities.Models.DataCpk;

namespace _5S_QA_Client;

public class mPanelChart : UserControl
{
	private HitTestResult mPreHit;

	private int mChartNumber;

	private IContainer components = null;

	private Panel panelResize;

	private Label lblRLCL;

	private Label label20;

	private Label lblRUCL;

	private Label label19;

	private Label lblR;

	private Label lblXLCL;

	private Label lblXUCL;

	private Label lblCpk;

	private Label lblCp;

	private Label lblSD;

	private Label lblMinimun;

	private Label lblMaximun;

	private Label lblAverage;

	private Label lblOutLSL;

	private Label lblOK;

	private Label lblOutUSL;

	private Label lblN;

	private Chart chartMain;

	private Panel panelChart;

	private Panel panelInfor;

	private TableLayoutPanel tpanelHeader;

	private Label label18;

	private Label label17;

	private Label label16;

	private Label label15;

	private Label lblMinimunSign;

	private Label lblOutLSLSign;

	private Label label12;

	private Label label10;

	private Label lblMaximunSign;

	private Label label8;

	private Label lblAverageSign;

	private Label label6;

	private Label lblOutUSLSign;

	private Label lblUSL;

	private Label lblLSL;

	private Label lblCL;

	private Label lblUCL;

	private Label lblLCL;

	private TableLayoutPanel tpanelDifference;

	private Label lblDataDifference;

	private Label lblTitle30pcs;

	private Label lblTitle5pcs;

	private Label lbl5pcs;

	private Label lblTitleDifference;

	private Label lblDifference;

	private Label lbl30pcs;

	private Label lblRange;

	private Label lblTitleRange;

	private Label lblDeviation;

	private Label lblTitleDeviation;

	private Label lblCountOut;

	private Label lblTitleCountOut;

	private Label lblTrend;

	public List<ChartViewModel> mModels { get; set; }

	public mPanelChart()
	{
		InitializeComponent();
	}

	private void mPanelChart_Load(object sender, EventArgs e)
	{
		Common.setControls(this);
		ControlResize.Init(panelResize, this, ControlResize.Direction.Vertical, Cursors.SizeNS);
	}

	public void Set_Chart(ResultForChartDto dto, int limit)
	{
		mChartNumber = limit;
		mModels = Load_Data(dto);
	}

	public void Init(Guid id)
	{
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_0315: Expected O, but got Unknown
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Expected O, but got Unknown
		IEnumerable<ChartViewModel> models = mModels.Where((ChartViewModel x) => x.MeasurementId == id);
		if (models == null || models.Count() == 0)
		{
			base.Visible = false;
			return;
		}
		base.Visible = true;
		System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
		List<DataCpk> datacpkAlls = new List<DataCpk>();
		List<double> list = new List<double>();
		IEnumerable<IGrouping<Guid?, ChartViewModel>> enumerable = from x in models
			group x by x.RequestId;
		foreach (IGrouping<Guid?, ChartViewModel> item in enumerable)
		{
			List<double> list2 = new List<double>();
			foreach (ChartViewModel item2 in item)
			{
				if (!string.IsNullOrEmpty(item2.Result) && double.TryParse(item2.Result, out var result))
				{
					list2.Add(result);
				}
			}
			list.AddRange(list2);
			DataCpk val = new DataCpk
			{
				Date = item.First().RequestDate?.ToString("yyMMdd"),
				n = list2.Count,
				Average = list2.Average(),
				Range = list2.Max() - list2.Min(),
				Max = list2.Max(),
				Min = list2.Min(),
				SD = mMath.CalStandardDeviation(list2)
			};
			val.UpperErrorBars = val.Max - val.Average;
			val.LowerErrorBars = val.Average - val.Min;
			datacpkAlls.Add(val);
		}
		CommonCpk commonCpk = new CommonCpk
		{
			Unit = models.First().Unit,
			Standard = double.Parse(models.First().Value),
			Upper = models.First().Upper.Value,
			Lower = models.First().Lower.Value,
			n = list.Count,
			Average = list.Average(),
			Maximun = list.Max(),
			Minimun = list.Min(),
			SD = mMath.CalStandardDeviation(list),
			Rtb = datacpkAlls.Average((DataCpk x) => x.Range)
		};
		commonCpk.USL = Math.Round(commonCpk.Standard + commonCpk.Upper, 4);
		commonCpk.LSL = Math.Round(commonCpk.Standard + commonCpk.Lower, 4);
		commonCpk.CL = commonCpk.Average;
		commonCpk.Sigma1 = commonCpk.CL + commonCpk.SD;
		commonCpk.MSigma1 = commonCpk.CL - commonCpk.SD;
		commonCpk.Sigma2 = commonCpk.Sigma1 + commonCpk.SD;
		commonCpk.MSigma2 = commonCpk.MSigma1 - commonCpk.SD;
		commonCpk.Sigma3 = commonCpk.Sigma2 + commonCpk.SD;
		commonCpk.MSigma3 = commonCpk.MSigma2 - commonCpk.SD;
		commonCpk.UCL = (models.First().UCL.HasValue ? models.First().UCL.Value : commonCpk.Sigma3);
		commonCpk.LCL = (models.First().LCL.HasValue ? models.First().LCL.Value : commonCpk.MSigma3);
		commonCpk.OutUSL = list.Count((double x) => x > commonCpk.USL);
		commonCpk.OutLSL = list.Count((double x) => x < commonCpk.LSL);
		commonCpk.OK = commonCpk.n - commonCpk.OutUSL - commonCpk.OutLSL;
		commonCpk.Cp = (commonCpk.USL - commonCpk.LSL) / (6.0 * commonCpk.SD);
		commonCpk.HCpk = (commonCpk.USL - commonCpk.CL) / (3.0 * commonCpk.SD);
		commonCpk.LCpk = (commonCpk.CL - commonCpk.LSL) / (3.0 * commonCpk.SD);
		commonCpk.Cpk = Math.Min(commonCpk.HCpk, commonCpk.LCpk);
		double constant = GetConstant(datacpkAlls.First().n, "A2");
		commonCpk.XUCL = commonCpk.Average + constant * commonCpk.Rtb;
		commonCpk.XLCL = commonCpk.Average - constant * commonCpk.Rtb;
		constant = GetConstant(datacpkAlls.First().n, "D4");
		commonCpk.RUCL = constant * commonCpk.Rtb;
		constant = GetConstant(datacpkAlls.First().n, "D3");
		commonCpk.RLCL = constant * commonCpk.Rtb;
		Invoke((MethodInvoker)delegate
		{
			Draw_Chart(commonCpk, datacpkAlls);
			SetDataDifference(models);
		});
	}

	private List<ChartViewModel> Load_Data(ResultForChartDto dto)
	{
		List<ChartViewModel> result = new List<ChartViewModel>();
		try
		{
			ResponseDto result2 = frmLogin.client.GetsAsync(dto, "/api/RequestResult/GetsForChart").Result;
			result = Common.getObjects<ChartViewModel>(result2);
		}
		catch
		{
		}
		return result;
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
		catch
		{
		}
		return result;
	}

	private void Draw_Chart(CommonCpk mCommonCpk, List<DataCpk> mDataCpks)
	{
		if (mCommonCpk.Unit == "°")
		{
			lblAverage.Text = Common.ConvertDoubleToDegrees(mCommonCpk.Average);
			lblMaximun.Text = Common.ConvertDoubleToDegrees(mCommonCpk.Maximun);
			lblMinimun.Text = Common.ConvertDoubleToDegrees(mCommonCpk.Minimun);
			lblSD.Text = Common.ConvertDoubleToDegrees(mCommonCpk.SD);
			lblXUCL.Text = Common.ConvertDoubleToDegrees(mCommonCpk.XUCL);
			lblXLCL.Text = Common.ConvertDoubleToDegrees(mCommonCpk.XLCL);
			lblR.Text = Common.ConvertDoubleToDegrees(mCommonCpk.Rtb);
			lblRUCL.Text = Common.ConvertDoubleToDegrees(mCommonCpk.RUCL);
			lblRLCL.Text = Common.ConvertDoubleToDegrees(mCommonCpk.RLCL);
		}
		else
		{
			lblAverage.Text = mCommonCpk.Average.ToString("0.0000");
			lblMaximun.Text = mCommonCpk.Maximun.ToString("0.0000");
			lblMinimun.Text = mCommonCpk.Minimun.ToString("0.0000");
			lblSD.Text = mCommonCpk.SD.ToString("0.0000");
			lblXUCL.Text = mCommonCpk.XUCL.ToString("0.0000");
			lblXLCL.Text = mCommonCpk.XLCL.ToString("0.0000");
			lblR.Text = mCommonCpk.Rtb.ToString("0.0000");
			lblRUCL.Text = mCommonCpk.RUCL.ToString("0.0000");
			lblRLCL.Text = mCommonCpk.RLCL.ToString("0.0000");
		}
		lblN.Text = mCommonCpk.n.ToString();
		lblOutUSL.Text = mCommonCpk.OutUSL.ToString();
		lblOutLSL.Text = mCommonCpk.OutLSL.ToString();
		lblOK.Text = mCommonCpk.OK.ToString();
		lblCp.Text = mCommonCpk.Cp.ToString("0.0000");
		lblCpk.Text = mCommonCpk.Cpk.ToString("0.0000");
		chartMain.Series["USL"].Points.Clear();
		chartMain.Series["UCL"].Points.Clear();
		chartMain.Series["+2σ"].Points.Clear();
		chartMain.Series["+1σ"].Points.Clear();
		chartMain.Series["CL"].Points.Clear();
		chartMain.Series["-1σ"].Points.Clear();
		chartMain.Series["-2σ"].Points.Clear();
		chartMain.Series["LCL"].Points.Clear();
		chartMain.Series["LSL"].Points.Clear();
		chartMain.Series["Data"].Points.Clear();
		chartMain.Series["ErrorBars"].Points.Clear();
		chartMain.ChartAreas[0].AxisX.Maximum = mChartNumber;
		chartMain.ChartAreas[0].AxisX.Minimum = 1.0;
		chartMain.ChartAreas[0].AxisX.Interval = 1.0;
		List<double> source = new List<double> { mCommonCpk.USL, mCommonCpk.UCL, mCommonCpk.Sigma3, mCommonCpk.CL, mCommonCpk.MSigma3, mCommonCpk.LCL, mCommonCpk.LSL };
		double num = Math.Round((mCommonCpk.Upper - mCommonCpk.Lower) * 0.1, 4, MidpointRounding.AwayFromZero);
		double num2 = Math.Round(source.Min(), 4, MidpointRounding.AwayFromZero) - num;
		double num3 = Math.Round(source.Max(), 4, MidpointRounding.AwayFromZero) + num;
		chartMain.ChartAreas[0].AxisY.Minimum = num2;
		chartMain.ChartAreas[0].AxisY.Maximum = num3;
		chartMain.ChartAreas[0].AxisY.Interval = Math.Round((num3 - num2) / 10.0, 4, MidpointRounding.AwayFromZero);
		for (int i = 0; i < mChartNumber; i++)
		{
			if (i < mDataCpks.Count)
			{
				chartMain.Series["USL"].Points.AddXY(mDataCpks[i].Date, mCommonCpk.USL);
				chartMain.Series["UCL"].Points.AddXY(mDataCpks[i].Date, mCommonCpk.UCL);
				chartMain.Series["+2σ"].Points.AddXY(mDataCpks[i].Date, mCommonCpk.Sigma2);
				chartMain.Series["+1σ"].Points.AddXY(mDataCpks[i].Date, mCommonCpk.Sigma1);
				chartMain.Series["CL"].Points.AddXY(mDataCpks[i].Date, mCommonCpk.CL);
				chartMain.Series["-1σ"].Points.AddXY(mDataCpks[i].Date, mCommonCpk.MSigma1);
				chartMain.Series["-2σ"].Points.AddXY(mDataCpks[i].Date, mCommonCpk.MSigma2);
				chartMain.Series["LCL"].Points.AddXY(mDataCpks[i].Date, mCommonCpk.LCL);
				chartMain.Series["LSL"].Points.AddXY(mDataCpks[i].Date, mCommonCpk.LSL);
				chartMain.Series["Data"].Points.AddXY(mDataCpks[i].Date, mDataCpks[i].Average);
				chartMain.Series["ErrorBars"].Points.AddXY(mDataCpks[i].Date, mDataCpks[i].Average, mDataCpks[i].Average - mDataCpks[i].LowerErrorBars, mDataCpks[i].Average + mDataCpks[i].UpperErrorBars);
				if (mDataCpks[i].Average < mCommonCpk.LSL || mDataCpks[i].Average > mCommonCpk.USL)
				{
					chartMain.Series["Data"].Points.Last().MarkerColor = Color.Red;
				}
			}
			else
			{
				chartMain.Series["USL"].Points.AddXY("", mCommonCpk.USL);
				chartMain.Series["UCL"].Points.AddXY("", mCommonCpk.UCL);
				chartMain.Series["+2σ"].Points.AddXY("", mCommonCpk.Sigma2);
				chartMain.Series["+1σ"].Points.AddXY("", mCommonCpk.Sigma1);
				chartMain.Series["CL"].Points.AddXY("", mCommonCpk.CL);
				chartMain.Series["-1σ"].Points.AddXY("", mCommonCpk.MSigma1);
				chartMain.Series["-2σ"].Points.AddXY("", mCommonCpk.MSigma2);
				chartMain.Series["LCL"].Points.AddXY("", mCommonCpk.LCL);
				chartMain.Series["LSL"].Points.AddXY("", mCommonCpk.LSL);
			}
		}
		chartMain.ChartAreas[0].AxisY.CustomLabels.Clear();
		for (double num4 = chartMain.ChartAreas[0].AxisY.Minimum; num4 <= chartMain.ChartAreas[0].AxisY.Maximum; num4 += chartMain.ChartAreas[0].AxisY.Interval)
		{
			chartMain.ChartAreas[0].AxisY.CustomLabels.Add(num4 - chartMain.ChartAreas[0].AxisY.Interval / 2.0, num4 + chartMain.ChartAreas[0].AxisY.Interval / 2.0, (mCommonCpk.Unit == "°") ? Common.ConvertDoubleToDegrees(num4, uniformity: true) : num4.ToString("0.0000"));
		}
		DrawInformation(mCommonCpk);
	}

	private void DrawInformation(CommonCpk mCommonCpk)
	{
		int num = (int)((float)chartMain.Height * chartMain.ChartAreas[0].InnerPlotPosition.Y / 100f);
		int num2 = (int)((float)chartMain.Height * (chartMain.ChartAreas[0].InnerPlotPosition.Y + chartMain.ChartAreas[0].InnerPlotPosition.Height) / 100f);
		List<double> list = new List<double> { mCommonCpk.USL, mCommonCpk.UCL, mCommonCpk.CL, mCommonCpk.LCL, mCommonCpk.LSL }.OrderByDescending((double x) => x).ToList();
		lblUSL.Text = "USL " + ((mCommonCpk.Unit == "°") ? Common.ConvertDoubleToDegrees(mCommonCpk.USL) : mCommonCpk.USL.ToString("0.0000"));
		lblUSL.Location = new Point(0, GetLocationY(num + lblUSL.Height / 2, num2 - lblUSL.Height / 2, 5, list.IndexOf(mCommonCpk.USL)) - lblUSL.Height / 2);
		lblLSL.Text = "LSL " + ((mCommonCpk.Unit == "°") ? Common.ConvertDoubleToDegrees(mCommonCpk.LSL) : mCommonCpk.LSL.ToString("0.0000"));
		lblLSL.Location = new Point(0, GetLocationY(num + lblLSL.Height / 2, num2 - lblLSL.Height / 2, 5, list.IndexOf(mCommonCpk.LSL)) - lblLSL.Height / 2);
		lblCL.Text = "CL " + ((mCommonCpk.Unit == "°") ? Common.ConvertDoubleToDegrees(mCommonCpk.CL) : mCommonCpk.CL.ToString("0.0000"));
		lblCL.Location = new Point(0, GetLocationY(num + lblCL.Height / 2, num2 - lblCL.Height / 2, 5, list.IndexOf(mCommonCpk.CL)) - lblCL.Height / 2);
		lblUCL.Text = "UCL " + ((mCommonCpk.Unit == "°") ? Common.ConvertDoubleToDegrees(mCommonCpk.UCL) : mCommonCpk.UCL.ToString("0.0000"));
		lblUCL.Location = new Point(0, GetLocationY(num + lblUCL.Height / 2, num2 - lblUCL.Height / 2, 5, list.IndexOf(mCommonCpk.UCL)) - lblUCL.Height / 2);
		lblLCL.Text = "LCL " + ((mCommonCpk.Unit == "°") ? Common.ConvertDoubleToDegrees(mCommonCpk.LCL) : mCommonCpk.LCL.ToString("0.0000"));
		lblLCL.Location = new Point(0, GetLocationY(num + lblLCL.Height / 2, num2 - lblLCL.Height / 2, 5, list.IndexOf(mCommonCpk.LCL)) - lblLCL.Height / 2);
	}

	private int GetLocationY(int xmin, int xmax, int number, int pos)
	{
		return pos * (xmax - xmin) / (number - 1) + xmin;
	}

	private void SetDataDifference(IEnumerable<ChartViewModel> items)
	{
		double? num = items.First().Upper - items.First().Lower;
		lblRange.Text = num.ToString();
		if (items.Count() < 6)
		{
			lbl5pcs.Text = "N/A";
			lbl30pcs.Text = "N/A";
			lblDeviation.Text = "N/A";
			lblDifference.Text = "N/A";
			lblDifference.ForeColor = Color.Black;
		}
		else
		{
			List<ChartViewModel> list = items.OrderByDescending((ChartViewModel x) => x.Modified).ToList();
			List<ChartViewModel> range = list.GetRange(0, 5);
			List<ChartViewModel> range2 = list.GetRange(5, list.Count - 5);
			if (list.Count > 35)
			{
				list = list.GetRange(5, mChartNumber - 5);
			}
			double num2 = range.Average((ChartViewModel x) => double.Parse(x.Result));
			double num3 = range2.Average((ChartViewModel x) => double.Parse(x.Result));
			double num4 = num2 - num3;
			lbl5pcs.Text = ((items.First().Unit == "°") ? Common.ConvertDoubleToDegrees(num2) : num2.ToString("0.####"));
			lbl30pcs.Text = ((items.First().Unit == "°") ? Common.ConvertDoubleToDegrees(num3) : num3.ToString("0.####"));
			lblDeviation.Text = ((items.First().Unit == "°") ? Common.ConvertDoubleToDegrees(num4) : num4.ToString("0.####"));
			double? num5 = num4 * 100.0 / num;
			if (num5 > 20.0 || num5 < -20.0)
			{
				lblDifference.ForeColor = Color.Red;
			}
			lblDifference.Text = num5?.ToString("0.##");
		}
		if (items.Count() < 2)
		{
			lblTrend.Text = "";
			lblCountOut.Text = "N/A";
			return;
		}
		List<ChartViewModel> list2 = items.OrderByDescending((ChartViewModel x) => x.Modified).ToList();
		double.TryParse(list2[0].Result, out var result);
		double.TryParse(list2[1].Result, out var result2);
		lblCountOut.ForeColor = Color.Black;
		if (result > items.First().USL || result < items.First().LSL)
		{
			lblTrend.Text = "";
			lblCountOut.Text = "NG";
			lblCountOut.ForeColor = Color.Red;
		}
		else if (result < result2)
		{
			lblTrend.Text = Common.getTextLanguage(this, "wTrendDown");
			double? obj = (result - items.First().LSL) / (result2 - result);
			lblCountOut.Text = obj?.ToString("0");
		}
		else if (result > result2)
		{
			lblTrend.Text = Common.getTextLanguage(this, "wTrendUp");
			double? obj2 = (items.First().USL - result) / (result - result2);
			lblCountOut.Text = obj2?.ToString("0");
		}
		else
		{
			lblTrend.Text = "";
			lblCountOut.Text = "N/A";
		}
	}

	private void chartMain_MouseMove(object sender, MouseEventArgs e)
	{
		if (mPreHit != null)
		{
			DataPoint dataPoint = mPreHit.Object as DataPoint;
			dataPoint.IsValueShownAsLabel = false;
			if (mPreHit.Series.Name.Equals("Data"))
			{
				dataPoint.MarkerSize = 6;
			}
			else if (mPreHit.Series.Name.Equals("ErrorBars"))
			{
				dataPoint.MarkerSize = 5;
			}
		}
		HitTestResult hitTestResult = chartMain.HitTest(e.X, e.Y, ChartElementType.DataPoint);
		if (!hitTestResult.ChartElementType.Equals(ChartElementType.DataPoint))
		{
			return;
		}
		mPreHit = hitTestResult;
		if (mPreHit != null)
		{
			DataPoint dataPoint2 = mPreHit.Object as DataPoint;
			dataPoint2.IsValueShownAsLabel = true;
			if (mPreHit.Series.Name.Equals("Data") || mPreHit.Series.Name.Equals("ErrorBars"))
			{
				dataPoint2.MarkerSize += 2;
			}
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
		System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
		System.Windows.Forms.DataVisualization.Charting.Legend legend = new System.Windows.Forms.DataVisualization.Charting.Legend();
		System.Windows.Forms.DataVisualization.Charting.Series series = new System.Windows.Forms.DataVisualization.Charting.Series();
		System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
		System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
		System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
		System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
		System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
		System.Windows.Forms.DataVisualization.Charting.Series series7 = new System.Windows.Forms.DataVisualization.Charting.Series();
		System.Windows.Forms.DataVisualization.Charting.Series series8 = new System.Windows.Forms.DataVisualization.Charting.Series();
		System.Windows.Forms.DataVisualization.Charting.Series series9 = new System.Windows.Forms.DataVisualization.Charting.Series();
		System.Windows.Forms.DataVisualization.Charting.Series series10 = new System.Windows.Forms.DataVisualization.Charting.Series();
		System.Windows.Forms.DataVisualization.Charting.Series series11 = new System.Windows.Forms.DataVisualization.Charting.Series();
		this.panelResize = new System.Windows.Forms.Panel();
		this.lblRLCL = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.lblRUCL = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.lblR = new System.Windows.Forms.Label();
		this.lblXLCL = new System.Windows.Forms.Label();
		this.lblXUCL = new System.Windows.Forms.Label();
		this.lblCpk = new System.Windows.Forms.Label();
		this.lblCp = new System.Windows.Forms.Label();
		this.lblSD = new System.Windows.Forms.Label();
		this.lblMinimun = new System.Windows.Forms.Label();
		this.lblMaximun = new System.Windows.Forms.Label();
		this.lblAverage = new System.Windows.Forms.Label();
		this.lblOutLSL = new System.Windows.Forms.Label();
		this.lblOK = new System.Windows.Forms.Label();
		this.lblOutUSL = new System.Windows.Forms.Label();
		this.lblN = new System.Windows.Forms.Label();
		this.chartMain = new System.Windows.Forms.DataVisualization.Charting.Chart();
		this.panelChart = new System.Windows.Forms.Panel();
		this.panelInfor = new System.Windows.Forms.Panel();
		this.lblLCL = new System.Windows.Forms.Label();
		this.lblUCL = new System.Windows.Forms.Label();
		this.lblCL = new System.Windows.Forms.Label();
		this.lblLSL = new System.Windows.Forms.Label();
		this.lblUSL = new System.Windows.Forms.Label();
		this.tpanelHeader = new System.Windows.Forms.TableLayoutPanel();
		this.label18 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.lblMinimunSign = new System.Windows.Forms.Label();
		this.lblOutLSLSign = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.lblMaximunSign = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.lblAverageSign = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.lblOutUSLSign = new System.Windows.Forms.Label();
		this.tpanelDifference = new System.Windows.Forms.TableLayoutPanel();
		this.lblTrend = new System.Windows.Forms.Label();
		this.lblCountOut = new System.Windows.Forms.Label();
		this.lblTitleCountOut = new System.Windows.Forms.Label();
		this.lblRange = new System.Windows.Forms.Label();
		this.lblTitleRange = new System.Windows.Forms.Label();
		this.lblDeviation = new System.Windows.Forms.Label();
		this.lblTitleDeviation = new System.Windows.Forms.Label();
		this.lblTitleDifference = new System.Windows.Forms.Label();
		this.lblDifference = new System.Windows.Forms.Label();
		this.lbl30pcs = new System.Windows.Forms.Label();
		this.lbl5pcs = new System.Windows.Forms.Label();
		this.lblTitle30pcs = new System.Windows.Forms.Label();
		this.lblTitle5pcs = new System.Windows.Forms.Label();
		this.lblDataDifference = new System.Windows.Forms.Label();
		((System.ComponentModel.ISupportInitialize)this.chartMain).BeginInit();
		this.panelChart.SuspendLayout();
		this.panelInfor.SuspendLayout();
		this.tpanelHeader.SuspendLayout();
		this.tpanelDifference.SuspendLayout();
		base.SuspendLayout();
		this.panelResize.Dock = System.Windows.Forms.DockStyle.Top;
		this.panelResize.Location = new System.Drawing.Point(0, 0);
		this.panelResize.Name = "panelResize";
		this.panelResize.Size = new System.Drawing.Size(998, 3);
		this.panelResize.TabIndex = 0;
		this.lblRLCL.AutoSize = true;
		this.lblRLCL.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblRLCL.Location = new System.Drawing.Point(822, 27);
		this.lblRLCL.Margin = new System.Windows.Forms.Padding(3);
		this.lblRLCL.Name = "lblRLCL";
		this.lblRLCL.Size = new System.Drawing.Size(65, 16);
		this.lblRLCL.TabIndex = 39;
		this.lblRLCL.Text = "...";
		this.lblRLCL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label20.AutoSize = true;
		this.label20.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label20.Location = new System.Drawing.Point(769, 27);
		this.label20.Margin = new System.Windows.Forms.Padding(3);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(46, 16);
		this.label20.TabIndex = 38;
		this.label20.Text = "R LCL";
		this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblRUCL.AutoSize = true;
		this.lblRUCL.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblRUCL.Location = new System.Drawing.Point(822, 4);
		this.lblRUCL.Margin = new System.Windows.Forms.Padding(3);
		this.lblRUCL.Name = "lblRUCL";
		this.lblRUCL.Size = new System.Drawing.Size(65, 16);
		this.lblRUCL.TabIndex = 37;
		this.lblRUCL.Text = "...";
		this.lblRUCL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label19.AutoSize = true;
		this.label19.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label19.Location = new System.Drawing.Point(769, 4);
		this.label19.Margin = new System.Windows.Forms.Padding(3);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(46, 16);
		this.label19.TabIndex = 36;
		this.label19.Text = "R UCL";
		this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblR.AutoSize = true;
		this.lblR.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblR.Location = new System.Drawing.Point(567, 27);
		this.lblR.Margin = new System.Windows.Forms.Padding(3);
		this.lblR.Name = "lblR";
		this.lblR.Size = new System.Drawing.Size(65, 16);
		this.lblR.TabIndex = 35;
		this.lblR.Text = "...";
		this.lblR.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblXLCL.AutoSize = true;
		this.lblXLCL.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblXLCL.Location = new System.Drawing.Point(697, 27);
		this.lblXLCL.Margin = new System.Windows.Forms.Padding(3);
		this.lblXLCL.Name = "lblXLCL";
		this.lblXLCL.Size = new System.Drawing.Size(65, 16);
		this.lblXLCL.TabIndex = 34;
		this.lblXLCL.Text = "...";
		this.lblXLCL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblXUCL.AutoSize = true;
		this.lblXUCL.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblXUCL.Location = new System.Drawing.Point(697, 4);
		this.lblXUCL.Margin = new System.Windows.Forms.Padding(3);
		this.lblXUCL.Name = "lblXUCL";
		this.lblXUCL.Size = new System.Drawing.Size(65, 16);
		this.lblXUCL.TabIndex = 33;
		this.lblXUCL.Text = "...";
		this.lblXUCL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblCpk.AutoSize = true;
		this.lblCpk.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCpk.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCpk.Location = new System.Drawing.Point(567, 4);
		this.lblCpk.Margin = new System.Windows.Forms.Padding(3);
		this.lblCpk.Name = "lblCpk";
		this.lblCpk.Size = new System.Drawing.Size(65, 16);
		this.lblCpk.TabIndex = 32;
		this.lblCpk.Text = "...";
		this.lblCpk.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblCp.AutoSize = true;
		this.lblCp.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCp.Location = new System.Drawing.Point(457, 27);
		this.lblCp.Margin = new System.Windows.Forms.Padding(3);
		this.lblCp.Name = "lblCp";
		this.lblCp.Size = new System.Drawing.Size(65, 16);
		this.lblCp.TabIndex = 31;
		this.lblCp.Text = "...";
		this.lblCp.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblSD.AutoSize = true;
		this.lblSD.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblSD.Location = new System.Drawing.Point(457, 4);
		this.lblSD.Margin = new System.Windows.Forms.Padding(3);
		this.lblSD.Name = "lblSD";
		this.lblSD.Size = new System.Drawing.Size(65, 16);
		this.lblSD.TabIndex = 30;
		this.lblSD.Text = "...";
		this.lblSD.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblMinimun.AutoSize = true;
		this.lblMinimun.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblMinimun.Location = new System.Drawing.Point(352, 27);
		this.lblMinimun.Margin = new System.Windows.Forms.Padding(3);
		this.lblMinimun.Name = "lblMinimun";
		this.lblMinimun.Size = new System.Drawing.Size(65, 16);
		this.lblMinimun.TabIndex = 29;
		this.lblMinimun.Text = "...";
		this.lblMinimun.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblMaximun.AutoSize = true;
		this.lblMaximun.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblMaximun.Location = new System.Drawing.Point(352, 4);
		this.lblMaximun.Margin = new System.Windows.Forms.Padding(3);
		this.lblMaximun.Name = "lblMaximun";
		this.lblMaximun.Size = new System.Drawing.Size(65, 16);
		this.lblMaximun.TabIndex = 28;
		this.lblMaximun.Text = "...";
		this.lblMaximun.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblAverage.AutoSize = true;
		this.lblAverage.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblAverage.Location = new System.Drawing.Point(213, 27);
		this.lblAverage.Margin = new System.Windows.Forms.Padding(3);
		this.lblAverage.Name = "lblAverage";
		this.lblAverage.Size = new System.Drawing.Size(65, 16);
		this.lblAverage.TabIndex = 27;
		this.lblAverage.Text = "...";
		this.lblAverage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblOutLSL.AutoSize = true;
		this.lblOutLSL.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblOutLSL.ForeColor = System.Drawing.Color.Red;
		this.lblOutLSL.Location = new System.Drawing.Point(75, 27);
		this.lblOutLSL.Margin = new System.Windows.Forms.Padding(3);
		this.lblOutLSL.Name = "lblOutLSL";
		this.lblOutLSL.Size = new System.Drawing.Size(65, 16);
		this.lblOutLSL.TabIndex = 26;
		this.lblOutLSL.Text = "...";
		this.lblOutLSL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblOK.AutoSize = true;
		this.lblOK.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblOK.ForeColor = System.Drawing.Color.Blue;
		this.lblOK.Location = new System.Drawing.Point(213, 4);
		this.lblOK.Margin = new System.Windows.Forms.Padding(3);
		this.lblOK.Name = "lblOK";
		this.lblOK.Size = new System.Drawing.Size(65, 16);
		this.lblOK.TabIndex = 25;
		this.lblOK.Text = "...";
		this.lblOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblOutUSL.AutoSize = true;
		this.lblOutUSL.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblOutUSL.ForeColor = System.Drawing.Color.Red;
		this.lblOutUSL.Location = new System.Drawing.Point(75, 4);
		this.lblOutUSL.Margin = new System.Windows.Forms.Padding(3);
		this.lblOutUSL.Name = "lblOutUSL";
		this.lblOutUSL.Size = new System.Drawing.Size(65, 16);
		this.lblOutUSL.TabIndex = 24;
		this.lblOutUSL.Text = "...";
		this.lblOutUSL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblN.AutoSize = true;
		this.lblN.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblN.Location = new System.Drawing.Point(923, 4);
		this.lblN.Margin = new System.Windows.Forms.Padding(3);
		this.lblN.Name = "lblN";
		this.lblN.Size = new System.Drawing.Size(71, 16);
		this.lblN.TabIndex = 23;
		this.lblN.Text = "...";
		this.lblN.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		chartArea.AxisX.LabelStyle.Angle = 90;
		chartArea.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
		chartArea.AxisY.IsLabelAutoFit = false;
		chartArea.AxisY.LabelStyle.Format = "0.0000";
		chartArea.AxisY.MajorGrid.Enabled = false;
		chartArea.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
		chartArea.Name = "ChartArea1";
		this.chartMain.ChartAreas.Add(chartArea);
		this.chartMain.Dock = System.Windows.Forms.DockStyle.Fill;
		legend.Enabled = false;
		legend.Name = "Legend1";
		this.chartMain.Legends.Add(legend);
		this.chartMain.Location = new System.Drawing.Point(0, 0);
		this.chartMain.Name = "chartMain";
		series.BorderWidth = 2;
		series.ChartArea = "ChartArea1";
		series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
		series.Color = System.Drawing.Color.Red;
		series.LabelBackColor = System.Drawing.Color.Black;
		series.LabelForeColor = System.Drawing.Color.White;
		series.LabelFormat = "0.####";
		series.Legend = "Legend1";
		series.Name = "USL";
		series2.BorderWidth = 2;
		series2.ChartArea = "ChartArea1";
		series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
		series2.Color = System.Drawing.Color.Blue;
		series2.LabelBackColor = System.Drawing.Color.Black;
		series2.LabelForeColor = System.Drawing.Color.White;
		series2.LabelFormat = "0.####";
		series2.Legend = "Legend1";
		series2.Name = "UCL";
		series3.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
		series3.ChartArea = "ChartArea1";
		series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
		series3.Color = System.Drawing.Color.Blue;
		series3.LabelBackColor = System.Drawing.Color.Black;
		series3.LabelForeColor = System.Drawing.Color.White;
		series3.LabelFormat = "0.####";
		series3.Legend = "Legend1";
		series3.Name = "+2σ";
		series4.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
		series4.ChartArea = "ChartArea1";
		series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
		series4.Color = System.Drawing.Color.Blue;
		series4.LabelBackColor = System.Drawing.Color.Black;
		series4.LabelForeColor = System.Drawing.Color.White;
		series4.LabelFormat = "0.####";
		series4.Legend = "Legend1";
		series4.Name = "+1σ";
		series5.BorderWidth = 2;
		series5.ChartArea = "ChartArea1";
		series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
		series5.Color = System.Drawing.Color.Blue;
		series5.LabelBackColor = System.Drawing.Color.Black;
		series5.LabelForeColor = System.Drawing.Color.White;
		series5.LabelFormat = "0.####";
		series5.Legend = "Legend1";
		series5.Name = "CL";
		series6.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
		series6.ChartArea = "ChartArea1";
		series6.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
		series6.Color = System.Drawing.Color.Blue;
		series6.LabelBackColor = System.Drawing.Color.Black;
		series6.LabelForeColor = System.Drawing.Color.White;
		series6.LabelFormat = "0.####";
		series6.Legend = "Legend1";
		series6.Name = "-1σ";
		series7.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
		series7.ChartArea = "ChartArea1";
		series7.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
		series7.Color = System.Drawing.Color.Blue;
		series7.LabelBackColor = System.Drawing.Color.Black;
		series7.LabelForeColor = System.Drawing.Color.White;
		series7.LabelFormat = "0.####";
		series7.Legend = "Legend1";
		series7.Name = "-2σ";
		series8.BorderWidth = 2;
		series8.ChartArea = "ChartArea1";
		series8.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
		series8.Color = System.Drawing.Color.Blue;
		series8.LabelBackColor = System.Drawing.Color.Black;
		series8.LabelForeColor = System.Drawing.Color.White;
		series8.LabelFormat = "0.####";
		series8.Legend = "Legend1";
		series8.Name = "LCL";
		series9.BorderWidth = 2;
		series9.ChartArea = "ChartArea1";
		series9.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
		series9.Color = System.Drawing.Color.Red;
		series9.LabelBackColor = System.Drawing.Color.Black;
		series9.LabelForeColor = System.Drawing.Color.White;
		series9.LabelFormat = "0.####";
		series9.Legend = "Legend1";
		series9.Name = "LSL";
		series10.BorderWidth = 2;
		series10.ChartArea = "ChartArea1";
		series10.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
		series10.Color = System.Drawing.Color.Black;
		series10.LabelBackColor = System.Drawing.Color.Black;
		series10.LabelForeColor = System.Drawing.Color.White;
		series10.LabelFormat = "0.####";
		series10.Legend = "Legend1";
		series10.MarkerSize = 6;
		series10.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
		series10.Name = "Data";
		series11.ChartArea = "ChartArea1";
		series11.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.ErrorBar;
		series11.Color = System.Drawing.Color.Black;
		series11.CustomProperties = "PixelPointWidth=15";
		series11.LabelBackColor = System.Drawing.Color.Black;
		series11.LabelForeColor = System.Drawing.Color.White;
		series11.LabelFormat = "0.####";
		series11.Legend = "Legend1";
		series11.Name = "ErrorBars";
		series11.YValuesPerPoint = 3;
		this.chartMain.Series.Add(series);
		this.chartMain.Series.Add(series2);
		this.chartMain.Series.Add(series3);
		this.chartMain.Series.Add(series4);
		this.chartMain.Series.Add(series5);
		this.chartMain.Series.Add(series6);
		this.chartMain.Series.Add(series7);
		this.chartMain.Series.Add(series8);
		this.chartMain.Series.Add(series9);
		this.chartMain.Series.Add(series10);
		this.chartMain.Series.Add(series11);
		this.chartMain.Size = new System.Drawing.Size(906, 174);
		this.chartMain.TabIndex = 5;
		this.chartMain.MouseMove += new System.Windows.Forms.MouseEventHandler(chartMain_MouseMove);
		this.panelChart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panelChart.Controls.Add(this.chartMain);
		this.panelChart.Controls.Add(this.panelInfor);
		this.panelChart.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panelChart.Location = new System.Drawing.Point(0, 74);
		this.panelChart.Name = "panelChart";
		this.panelChart.Size = new System.Drawing.Size(998, 176);
		this.panelChart.TabIndex = 6;
		this.panelInfor.Controls.Add(this.lblLCL);
		this.panelInfor.Controls.Add(this.lblUCL);
		this.panelInfor.Controls.Add(this.lblCL);
		this.panelInfor.Controls.Add(this.lblLSL);
		this.panelInfor.Controls.Add(this.lblUSL);
		this.panelInfor.Dock = System.Windows.Forms.DockStyle.Right;
		this.panelInfor.Location = new System.Drawing.Point(906, 0);
		this.panelInfor.MaximumSize = new System.Drawing.Size(90, 0);
		this.panelInfor.MinimumSize = new System.Drawing.Size(90, 0);
		this.panelInfor.Name = "panelInfor";
		this.panelInfor.Size = new System.Drawing.Size(90, 174);
		this.panelInfor.TabIndex = 4;
		this.lblLCL.AutoSize = true;
		this.lblLCL.BackColor = System.Drawing.Color.Transparent;
		this.lblLCL.ForeColor = System.Drawing.Color.Blue;
		this.lblLCL.Location = new System.Drawing.Point(20, 144);
		this.lblLCL.Name = "lblLCL";
		this.lblLCL.Size = new System.Drawing.Size(30, 16);
		this.lblLCL.TabIndex = 4;
		this.lblLCL.Text = "LCL";
		this.lblUCL.AutoSize = true;
		this.lblUCL.BackColor = System.Drawing.Color.Transparent;
		this.lblUCL.ForeColor = System.Drawing.Color.Blue;
		this.lblUCL.Location = new System.Drawing.Point(20, 62);
		this.lblUCL.Name = "lblUCL";
		this.lblUCL.Size = new System.Drawing.Size(33, 16);
		this.lblUCL.TabIndex = 3;
		this.lblUCL.Text = "UCL";
		this.lblCL.AutoSize = true;
		this.lblCL.BackColor = System.Drawing.Color.Transparent;
		this.lblCL.ForeColor = System.Drawing.Color.Blue;
		this.lblCL.Location = new System.Drawing.Point(20, 107);
		this.lblCL.Name = "lblCL";
		this.lblCL.Size = new System.Drawing.Size(23, 16);
		this.lblCL.TabIndex = 2;
		this.lblCL.Text = "CL";
		this.lblLSL.AutoSize = true;
		this.lblLSL.BackColor = System.Drawing.Color.Transparent;
		this.lblLSL.ForeColor = System.Drawing.Color.Red;
		this.lblLSL.Location = new System.Drawing.Point(20, 188);
		this.lblLSL.Name = "lblLSL";
		this.lblLSL.Size = new System.Drawing.Size(30, 16);
		this.lblLSL.TabIndex = 1;
		this.lblLSL.Text = "LSL";
		this.lblUSL.AutoSize = true;
		this.lblUSL.BackColor = System.Drawing.Color.Transparent;
		this.lblUSL.ForeColor = System.Drawing.Color.Red;
		this.lblUSL.Location = new System.Drawing.Point(20, 24);
		this.lblUSL.Name = "lblUSL";
		this.lblUSL.Size = new System.Drawing.Size(33, 16);
		this.lblUSL.TabIndex = 0;
		this.lblUSL.Text = "USL";
		this.tpanelHeader.AutoSize = true;
		this.tpanelHeader.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		this.tpanelHeader.ColumnCount = 16;
		this.tpanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.49917f));
		this.tpanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.49917f));
		this.tpanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.49917f));
		this.tpanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.49917f));
		this.tpanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.50167f));
		this.tpanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.49917f));
		this.tpanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.49918f));
		this.tpanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.50328f));
		this.tpanelHeader.Controls.Add(this.lblRLCL, 13, 1);
		this.tpanelHeader.Controls.Add(this.label20, 12, 1);
		this.tpanelHeader.Controls.Add(this.lblRUCL, 13, 0);
		this.tpanelHeader.Controls.Add(this.label19, 12, 0);
		this.tpanelHeader.Controls.Add(this.lblR, 9, 1);
		this.tpanelHeader.Controls.Add(this.lblXLCL, 11, 1);
		this.tpanelHeader.Controls.Add(this.lblXUCL, 11, 0);
		this.tpanelHeader.Controls.Add(this.lblCpk, 9, 0);
		this.tpanelHeader.Controls.Add(this.lblCp, 7, 1);
		this.tpanelHeader.Controls.Add(this.lblSD, 7, 0);
		this.tpanelHeader.Controls.Add(this.lblMinimun, 5, 1);
		this.tpanelHeader.Controls.Add(this.lblMaximun, 5, 0);
		this.tpanelHeader.Controls.Add(this.lblAverage, 3, 1);
		this.tpanelHeader.Controls.Add(this.lblOutLSL, 1, 1);
		this.tpanelHeader.Controls.Add(this.lblOK, 3, 0);
		this.tpanelHeader.Controls.Add(this.lblOutUSL, 1, 0);
		this.tpanelHeader.Controls.Add(this.lblN, 15, 0);
		this.tpanelHeader.Controls.Add(this.label18, 8, 1);
		this.tpanelHeader.Controls.Add(this.label17, 10, 1);
		this.tpanelHeader.Controls.Add(this.label16, 10, 0);
		this.tpanelHeader.Controls.Add(this.label15, 8, 0);
		this.tpanelHeader.Controls.Add(this.lblMinimunSign, 4, 1);
		this.tpanelHeader.Controls.Add(this.lblOutLSLSign, 0, 1);
		this.tpanelHeader.Controls.Add(this.label12, 14, 0);
		this.tpanelHeader.Controls.Add(this.label10, 6, 1);
		this.tpanelHeader.Controls.Add(this.lblMaximunSign, 4, 0);
		this.tpanelHeader.Controls.Add(this.label8, 6, 0);
		this.tpanelHeader.Controls.Add(this.lblAverageSign, 2, 1);
		this.tpanelHeader.Controls.Add(this.label6, 2, 0);
		this.tpanelHeader.Controls.Add(this.lblOutUSLSign, 0, 0);
		this.tpanelHeader.Dock = System.Windows.Forms.DockStyle.Top;
		this.tpanelHeader.Location = new System.Drawing.Point(0, 27);
		this.tpanelHeader.Margin = new System.Windows.Forms.Padding(4);
		this.tpanelHeader.Name = "tpanelHeader";
		this.tpanelHeader.RowCount = 2;
		this.tpanelHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22f));
		this.tpanelHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22f));
		this.tpanelHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tpanelHeader.Size = new System.Drawing.Size(998, 47);
		this.tpanelHeader.TabIndex = 5;
		this.label18.AutoSize = true;
		this.label18.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label18.Location = new System.Drawing.Point(529, 27);
		this.label18.Margin = new System.Windows.Forms.Padding(3);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(31, 16);
		this.label18.TabIndex = 19;
		this.label18.Text = "R\u00af";
		this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label17.AutoSize = true;
		this.label17.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label17.Location = new System.Drawing.Point(639, 27);
		this.label17.Margin = new System.Windows.Forms.Padding(3);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(51, 16);
		this.label17.TabIndex = 18;
		this.label17.Text = "X\u00af LCL";
		this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label16.AutoSize = true;
		this.label16.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label16.Location = new System.Drawing.Point(639, 4);
		this.label16.Margin = new System.Windows.Forms.Padding(3);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(51, 16);
		this.label16.TabIndex = 17;
		this.label16.Text = "X\u00af UCL";
		this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label15.AutoSize = true;
		this.label15.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label15.Location = new System.Drawing.Point(529, 4);
		this.label15.Margin = new System.Windows.Forms.Padding(3);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(31, 16);
		this.label15.TabIndex = 16;
		this.label15.Text = "Cpk";
		this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblMinimunSign.AutoSize = true;
		this.lblMinimunSign.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblMinimunSign.Location = new System.Drawing.Point(285, 27);
		this.lblMinimunSign.Margin = new System.Windows.Forms.Padding(3);
		this.lblMinimunSign.Name = "lblMinimunSign";
		this.lblMinimunSign.Size = new System.Drawing.Size(60, 16);
		this.lblMinimunSign.TabIndex = 15;
		this.lblMinimunSign.Text = "Minimun";
		this.lblMinimunSign.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblOutLSLSign.AutoSize = true;
		this.lblOutLSLSign.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblOutLSLSign.Location = new System.Drawing.Point(4, 27);
		this.lblOutLSLSign.Margin = new System.Windows.Forms.Padding(3);
		this.lblOutLSLSign.Name = "lblOutLSLSign";
		this.lblOutLSLSign.Size = new System.Drawing.Size(64, 16);
		this.lblOutLSLSign.TabIndex = 14;
		this.lblOutLSLSign.Text = "Out (LSL)";
		this.lblOutLSLSign.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label12.AutoSize = true;
		this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label12.Location = new System.Drawing.Point(894, 4);
		this.label12.Margin = new System.Windows.Forms.Padding(3);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(22, 16);
		this.label12.TabIndex = 13;
		this.label12.Text = "(n)";
		this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label10.AutoSize = true;
		this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label10.Location = new System.Drawing.Point(424, 27);
		this.label10.Margin = new System.Windows.Forms.Padding(3);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(26, 16);
		this.label10.TabIndex = 11;
		this.label10.Text = "Cp";
		this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblMaximunSign.AutoSize = true;
		this.lblMaximunSign.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblMaximunSign.Location = new System.Drawing.Point(285, 4);
		this.lblMaximunSign.Margin = new System.Windows.Forms.Padding(3);
		this.lblMaximunSign.Name = "lblMaximunSign";
		this.lblMaximunSign.Size = new System.Drawing.Size(60, 16);
		this.lblMaximunSign.TabIndex = 10;
		this.lblMaximunSign.Text = "Maximun";
		this.lblMaximunSign.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label8.AutoSize = true;
		this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label8.Location = new System.Drawing.Point(424, 4);
		this.label8.Margin = new System.Windows.Forms.Padding(3);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(26, 16);
		this.label8.TabIndex = 9;
		this.label8.Text = "SD";
		this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblAverageSign.AutoSize = true;
		this.lblAverageSign.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblAverageSign.Location = new System.Drawing.Point(147, 27);
		this.lblAverageSign.Margin = new System.Windows.Forms.Padding(3);
		this.lblAverageSign.Name = "lblAverageSign";
		this.lblAverageSign.Size = new System.Drawing.Size(59, 16);
		this.lblAverageSign.TabIndex = 8;
		this.lblAverageSign.Text = "Average";
		this.lblAverageSign.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label6.AutoSize = true;
		this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label6.Location = new System.Drawing.Point(147, 4);
		this.label6.Margin = new System.Windows.Forms.Padding(3);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(59, 16);
		this.label6.TabIndex = 7;
		this.label6.Text = "OK";
		this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblOutUSLSign.AutoSize = true;
		this.lblOutUSLSign.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblOutUSLSign.Location = new System.Drawing.Point(4, 4);
		this.lblOutUSLSign.Margin = new System.Windows.Forms.Padding(3);
		this.lblOutUSLSign.Name = "lblOutUSLSign";
		this.lblOutUSLSign.Size = new System.Drawing.Size(64, 16);
		this.lblOutUSLSign.TabIndex = 6;
		this.lblOutUSLSign.Text = "Out (USL)";
		this.lblOutUSLSign.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.tpanelDifference.AutoSize = true;
		this.tpanelDifference.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		this.tpanelDifference.ColumnCount = 15;
		this.tpanelDifference.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelDifference.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelDifference.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelDifference.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelDifference.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelDifference.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelDifference.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelDifference.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelDifference.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelDifference.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelDifference.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelDifference.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelDifference.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelDifference.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelDifference.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tpanelDifference.Controls.Add(this.lblTrend, 11, 0);
		this.tpanelDifference.Controls.Add(this.lblCountOut, 13, 0);
		this.tpanelDifference.Controls.Add(this.lblTitleCountOut, 12, 0);
		this.tpanelDifference.Controls.Add(this.lblRange, 8, 0);
		this.tpanelDifference.Controls.Add(this.lblTitleRange, 7, 0);
		this.tpanelDifference.Controls.Add(this.lblDeviation, 6, 0);
		this.tpanelDifference.Controls.Add(this.lblTitleDeviation, 5, 0);
		this.tpanelDifference.Controls.Add(this.lblTitleDifference, 9, 0);
		this.tpanelDifference.Controls.Add(this.lblDifference, 10, 0);
		this.tpanelDifference.Controls.Add(this.lbl30pcs, 4, 0);
		this.tpanelDifference.Controls.Add(this.lbl5pcs, 2, 0);
		this.tpanelDifference.Controls.Add(this.lblTitle30pcs, 3, 0);
		this.tpanelDifference.Controls.Add(this.lblTitle5pcs, 1, 0);
		this.tpanelDifference.Controls.Add(this.lblDataDifference, 0, 0);
		this.tpanelDifference.Dock = System.Windows.Forms.DockStyle.Top;
		this.tpanelDifference.Location = new System.Drawing.Point(0, 3);
		this.tpanelDifference.Name = "tpanelDifference";
		this.tpanelDifference.RowCount = 1;
		this.tpanelDifference.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tpanelDifference.Size = new System.Drawing.Size(998, 24);
		this.tpanelDifference.TabIndex = 7;
		this.lblTrend.AutoSize = true;
		this.lblTrend.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTrend.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTrend.ForeColor = System.Drawing.Color.Blue;
		this.lblTrend.Location = new System.Drawing.Point(740, 4);
		this.lblTrend.Margin = new System.Windows.Forms.Padding(3);
		this.lblTrend.Name = "lblTrend";
		this.lblTrend.Size = new System.Drawing.Size(19, 16);
		this.lblTrend.TabIndex = 20;
		this.lblTrend.Text = "...";
		this.lblTrend.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblCountOut.AutoSize = true;
		this.lblCountOut.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCountOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCountOut.Location = new System.Drawing.Point(835, 4);
		this.lblCountOut.Margin = new System.Windows.Forms.Padding(3);
		this.lblCountOut.Name = "lblCountOut";
		this.lblCountOut.Size = new System.Drawing.Size(19, 16);
		this.lblCountOut.TabIndex = 19;
		this.lblCountOut.Text = "...";
		this.lblCountOut.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblTitleCountOut.AutoSize = true;
		this.lblTitleCountOut.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitleCountOut.Location = new System.Drawing.Point(766, 4);
		this.lblTitleCountOut.Margin = new System.Windows.Forms.Padding(3);
		this.lblTitleCountOut.Name = "lblTitleCountOut";
		this.lblTitleCountOut.Size = new System.Drawing.Size(62, 16);
		this.lblTitleCountOut.TabIndex = 18;
		this.lblTitleCountOut.Text = "Count out";
		this.lblTitleCountOut.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblRange.AutoSize = true;
		this.lblRange.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblRange.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblRange.Location = new System.Drawing.Point(590, 4);
		this.lblRange.Margin = new System.Windows.Forms.Padding(3);
		this.lblRange.Name = "lblRange";
		this.lblRange.Size = new System.Drawing.Size(19, 16);
		this.lblRange.TabIndex = 17;
		this.lblRange.Text = "...";
		this.lblRange.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblTitleRange.AutoSize = true;
		this.lblTitleRange.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitleRange.Location = new System.Drawing.Point(535, 4);
		this.lblTitleRange.Margin = new System.Windows.Forms.Padding(3);
		this.lblTitleRange.Name = "lblTitleRange";
		this.lblTitleRange.Size = new System.Drawing.Size(48, 16);
		this.lblTitleRange.TabIndex = 16;
		this.lblTitleRange.Text = "Range";
		this.lblTitleRange.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblDeviation.AutoSize = true;
		this.lblDeviation.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblDeviation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblDeviation.Location = new System.Drawing.Point(509, 4);
		this.lblDeviation.Margin = new System.Windows.Forms.Padding(3);
		this.lblDeviation.Name = "lblDeviation";
		this.lblDeviation.Size = new System.Drawing.Size(19, 16);
		this.lblDeviation.TabIndex = 15;
		this.lblDeviation.Text = "...";
		this.lblDeviation.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblTitleDeviation.AutoSize = true;
		this.lblTitleDeviation.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitleDeviation.Location = new System.Drawing.Point(438, 4);
		this.lblTitleDeviation.Margin = new System.Windows.Forms.Padding(3);
		this.lblTitleDeviation.Name = "lblTitleDeviation";
		this.lblTitleDeviation.Size = new System.Drawing.Size(64, 16);
		this.lblTitleDeviation.TabIndex = 14;
		this.lblTitleDeviation.Text = "Deviation";
		this.lblTitleDeviation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblTitleDifference.AutoSize = true;
		this.lblTitleDifference.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitleDifference.Location = new System.Drawing.Point(616, 4);
		this.lblTitleDifference.Margin = new System.Windows.Forms.Padding(3);
		this.lblTitleDifference.Name = "lblTitleDifference";
		this.lblTitleDifference.Size = new System.Drawing.Size(91, 16);
		this.lblTitleDifference.TabIndex = 13;
		this.lblTitleDifference.Text = "Difference (%)";
		this.lblTitleDifference.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblDifference.AutoSize = true;
		this.lblDifference.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblDifference.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblDifference.Location = new System.Drawing.Point(714, 4);
		this.lblDifference.Margin = new System.Windows.Forms.Padding(3);
		this.lblDifference.Name = "lblDifference";
		this.lblDifference.Size = new System.Drawing.Size(19, 16);
		this.lblDifference.TabIndex = 12;
		this.lblDifference.Text = "...";
		this.lblDifference.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lbl30pcs.AutoSize = true;
		this.lbl30pcs.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lbl30pcs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbl30pcs.Location = new System.Drawing.Point(412, 4);
		this.lbl30pcs.Margin = new System.Windows.Forms.Padding(3);
		this.lbl30pcs.Name = "lbl30pcs";
		this.lbl30pcs.Size = new System.Drawing.Size(19, 16);
		this.lbl30pcs.TabIndex = 11;
		this.lbl30pcs.Text = "...";
		this.lbl30pcs.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lbl5pcs.AutoSize = true;
		this.lbl5pcs.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lbl5pcs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbl5pcs.Location = new System.Drawing.Point(273, 4);
		this.lbl5pcs.Margin = new System.Windows.Forms.Padding(3);
		this.lbl5pcs.Name = "lbl5pcs";
		this.lbl5pcs.Size = new System.Drawing.Size(19, 16);
		this.lbl5pcs.TabIndex = 10;
		this.lbl5pcs.Text = "...";
		this.lbl5pcs.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblTitle30pcs.AutoSize = true;
		this.lblTitle30pcs.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitle30pcs.Location = new System.Drawing.Point(299, 4);
		this.lblTitle30pcs.Margin = new System.Windows.Forms.Padding(3);
		this.lblTitle30pcs.Name = "lblTitle30pcs";
		this.lblTitle30pcs.Size = new System.Drawing.Size(106, 16);
		this.lblTitle30pcs.TabIndex = 9;
		this.lblTitle30pcs.Text = "Average (30pcs)";
		this.lblTitle30pcs.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblTitle5pcs.AutoSize = true;
		this.lblTitle5pcs.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitle5pcs.Location = new System.Drawing.Point(167, 4);
		this.lblTitle5pcs.Margin = new System.Windows.Forms.Padding(3);
		this.lblTitle5pcs.Name = "lblTitle5pcs";
		this.lblTitle5pcs.Size = new System.Drawing.Size(99, 16);
		this.lblTitle5pcs.TabIndex = 8;
		this.lblTitle5pcs.Text = "Average (5pcs)";
		this.lblTitle5pcs.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblDataDifference.AutoSize = true;
		this.lblDataDifference.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblDataDifference.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblDataDifference.Location = new System.Drawing.Point(4, 4);
		this.lblDataDifference.Margin = new System.Windows.Forms.Padding(3);
		this.lblDataDifference.Name = "lblDataDifference";
		this.lblDataDifference.Size = new System.Drawing.Size(156, 16);
		this.lblDataDifference.TabIndex = 7;
		this.lblDataDifference.Text = "Data difference (20%)";
		this.lblDataDifference.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		base.Controls.Add(this.panelChart);
		base.Controls.Add(this.tpanelHeader);
		base.Controls.Add(this.tpanelDifference);
		base.Controls.Add(this.panelResize);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Name = "mPanelChart";
		base.Size = new System.Drawing.Size(998, 250);
		base.Load += new System.EventHandler(mPanelChart_Load);
		((System.ComponentModel.ISupportInitialize)this.chartMain).EndInit();
		this.panelChart.ResumeLayout(false);
		this.panelInfor.ResumeLayout(false);
		this.panelInfor.PerformLayout();
		this.tpanelHeader.ResumeLayout(false);
		this.tpanelHeader.PerformLayout();
		this.tpanelDifference.ResumeLayout(false);
		this.tpanelDifference.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
