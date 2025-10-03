using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using _5S_QA_DL.APIContext;
using _5S_QA_Entities.Constants;
using _5S_QA_Entities.Dtos;
using _5S_QA_Entities.Models;
using _5S_QA_System.Controls;
using _5S_QA_System.Properties;
using MetroFramework.Forms;

namespace _5S_QA_System;

public class frmStatisticView : MetroForm
{
	private HitTestResult mPreHit;

	private List<StatisticViewModel> mDataTree;

	private List<StatisticViewModel> mDataType;

	private IContainer components = null;

	private ToolTip toolTipMain;

	private StatusStrip statusStripfrmMain;

	private ToolStripProgressBar sprogbarStatus;

	public ToolStripStatusLabel slblStatus;

	private Panel panelSearch;

	private TableLayoutPanel tpanelSelect;

	private RadioButton rbDays;

	private RadioButton rbWeeklys;

	private RadioButton rbMonth;

	private MonthCalendar mcalendarSearch;

	private Panel panelDetail;

	private TreeView treeViewDetail;

	private Panel panelMain;

	private TableLayoutPanel tpanelChart;

	private Chart chartResultNGOne;

	private Chart chartTypeNG;

	private Chart chartResultNG;

	private Chart chartProductNG;

	private TableLayoutPanel tpanelTop;

	private Label lblTop;

	private ComboBox cbbTop;

	private Panel panelLogout;

	private Label lblFullname;

	private PictureBox ptbAvatar;

	private Panel panelSearchResize;

	public frmStatisticView()
	{
		InitializeComponent();
		Common.setControls(this, toolTipMain);
	}

	private void frmStatisticView_Load(object sender, EventArgs e)
	{
		Init();
	}

	private void frmStatisticView_Shown(object sender, EventArgs e)
	{
		load_AllData();
		rbDays.CheckedChanged += rb_CheckedChanged;
		rbWeeklys.CheckedChanged += rb_CheckedChanged;
		rbMonth.CheckedChanged += rb_CheckedChanged;
	}

	private void frmStatisticView_FormClosed(object sender, FormClosedEventArgs e)
	{
		GC.Collect();
	}

	private void Init()
	{
		ControlResize.Init(panelSearchResize, panelSearch, ControlResize.Direction.Horizontal, Cursors.SizeWE);
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
		getSetting();
		cbbTop.SelectedIndexChanged += cbbTop_SelectedIndexChanged;
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

	private void load_AllData()
	{
		System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
		start_Proccessor();
		EnDisControlMain(en: false);
		DrawChartProductNG();
		DrawChartResultNG();
		DrawChartTypeNG();
		IEnumerable<KeyValuePair<string, double>> statistics = DrawChartResultNGOne();
		DrawTreeDetailNGOne(statistics);
		GetTypeNGOne();
		debugOutput(Common.getTextLanguage(this, "Successful"));
	}

	private void EnDisControlMain(bool en)
	{
		panelMain.Visible = en;
		tpanelChart.Visible = !en;
	}

	private void getSetting()
	{
		cbbTop.Text = Settings.Default.TopProductNG.ToString();
	}

	private void setSetting()
	{
		Settings.Default.TopProductNG = int.Parse(cbbTop.Text);
		Settings.Default.Save();
	}

	private void DrawChartProductNG()
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		try
		{
			string uriName = "/api/Statistic/GetProductNGForDates";
			if (rbWeeklys.Checked)
			{
				uriName = "/api/Statistic/GetProductNGForWeeklys";
			}
			else if (rbMonth.Checked)
			{
				uriName = "/api/Statistic/GetProductNGForMonths";
			}
			StatisticDto body = new StatisticDto
			{
				Date = mcalendarSearch.SelectionRange.Start
			};
			ResponseDto result = frmLogin.client.GetsAsync(body, uriName).Result;
			StatisticViewModel val = Common.getObject<StatisticViewModel>((object)result);
			chartProductNG.Series.Clear();
			if (string.IsNullOrEmpty(val.Name))
			{
				return;
			}
			Series series = new Series
			{
				ChartArea = "ChartArea1",
				IsValueShownAsLabel = true,
				LabelBackColor = Color.Gray,
				LabelForeColor = Color.White,
				LabelFormat = "0.##'%'",
				Legend = "Legend1",
				Name = val.Name
			};
			chartProductNG.Series.Add(series);
			foreach (KeyValuePair<string, double> value in val.Values)
			{
				series.Points.AddXY(value.Key, value.Value);
			}
			chartProductNG.ChartAreas[0].RecalculateAxesScale();
		}
		catch (Exception ex)
		{
			if (ex.InnerException is ApiException { StatusCode: var statusCode } ex2)
			{
				if (statusCode.Equals(401))
				{
					if (MessageBox.Show(Common.getTextLanguage(this, "LoginAgain"), Common.getTextLanguage(this, "INFORMATION"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk).Equals(DialogResult.OK))
					{
						Close();
					}
				}
				else
				{
					debugOutput("ERR: " + ex2.Message.Replace("\n", ""));
					MessageBox.Show(ex2.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			else
			{
				debugOutput("ERR: " + ex.Message);
				MessageBox.Show(ex.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
	}

	private void DrawChartResultNG()
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		try
		{
			string uriName = "/api/Statistic/GetResultNGForDates";
			if (rbWeeklys.Checked)
			{
				uriName = "/api/Statistic/GetResultNGForWeeklys";
			}
			else if (rbMonth.Checked)
			{
				uriName = "/api/Statistic/GetResultNGForMonths";
			}
			StatisticDto body = new StatisticDto
			{
				Date = mcalendarSearch.SelectionRange.Start
			};
			ResponseDto result = frmLogin.client.GetsAsync(body, uriName).Result;
			List<StatisticViewModel> objectToList = Common.getObjectToList<StatisticViewModel>(result);
			chartResultNG.Series.Clear();
			foreach (StatisticViewModel item in objectToList)
			{
				Series series = new Series
				{
					ChartArea = "ChartArea1",
					LabelBackColor = Color.Gray,
					LabelForeColor = Color.White,
					LabelFormat = "0.##'%'",
					Legend = "Legend1",
					Name = item.Name
				};
				if (item == objectToList.Last())
				{
					series.BorderWidth = 2;
					series.ChartType = SeriesChartType.Line;
					series.Color = Color.Black;
					series.IsValueShownAsLabel = true;
					series.MarkerBorderColor = Color.Black;
					series.MarkerColor = Color.White;
					series.MarkerSize = 10;
					series.MarkerStyle = MarkerStyle.Circle;
				}
				chartResultNG.Series.Add(series);
				foreach (KeyValuePair<string, double> value in item.Values)
				{
					series.Points.AddXY(value.Key, value.Value);
				}
			}
			chartResultNG.ChartAreas[0].RecalculateAxesScale();
		}
		catch (Exception ex)
		{
			if (ex.InnerException is ApiException { StatusCode: var statusCode } ex2)
			{
				if (statusCode.Equals(401))
				{
					if (MessageBox.Show(Common.getTextLanguage(this, "LoginAgain"), Common.getTextLanguage(this, "INFORMATION"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk).Equals(DialogResult.OK))
					{
						Close();
					}
				}
				else
				{
					debugOutput("ERR: " + ex2.Message.Replace("\n", ""));
					MessageBox.Show(ex2.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			else
			{
				debugOutput("ERR: " + ex.Message);
				MessageBox.Show(ex.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
	}

	private void DrawChartTypeNG()
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		try
		{
			string uriName = "/api/Statistic/GetTypeNGForDates";
			if (rbWeeklys.Checked)
			{
				uriName = "/api/Statistic/GetTypeNGForWeeklys";
			}
			else if (rbMonth.Checked)
			{
				uriName = "/api/Statistic/GetTypeNGForMonths";
			}
			StatisticDto body = new StatisticDto
			{
				Date = mcalendarSearch.SelectionRange.Start
			};
			ResponseDto result = frmLogin.client.GetsAsync(body, uriName).Result;
			List<StatisticViewModel> objectToList = Common.getObjectToList<StatisticViewModel>(result);
			chartTypeNG.Series.Clear();
			foreach (StatisticViewModel item in objectToList)
			{
				Series series = new Series
				{
					ChartArea = "ChartArea1",
					LabelBackColor = Color.Gray,
					LabelForeColor = Color.White,
					LabelFormat = "0.##'%'",
					Legend = "Legend1",
					Name = item.Name
				};
				if (item == objectToList.Last())
				{
					series.BorderWidth = 2;
					series.ChartType = SeriesChartType.Line;
					series.Color = Color.Black;
					series.IsValueShownAsLabel = true;
					series.MarkerBorderColor = Color.Black;
					series.MarkerColor = Color.White;
					series.MarkerSize = 10;
					series.MarkerStyle = MarkerStyle.Circle;
				}
				chartTypeNG.Series.Add(series);
				foreach (KeyValuePair<string, double> value in item.Values)
				{
					series.Points.AddXY(value.Key, value.Value);
				}
			}
			chartTypeNG.ChartAreas[0].RecalculateAxesScale();
		}
		catch (Exception ex)
		{
			if (ex.InnerException is ApiException { StatusCode: var statusCode } ex2)
			{
				if (statusCode.Equals(401))
				{
					if (MessageBox.Show(Common.getTextLanguage(this, "LoginAgain"), Common.getTextLanguage(this, "INFORMATION"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk).Equals(DialogResult.OK))
					{
						Close();
					}
				}
				else
				{
					debugOutput("ERR: " + ex2.Message.Replace("\n", ""));
					MessageBox.Show(ex2.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			else
			{
				debugOutput("ERR: " + ex.Message);
				MessageBox.Show(ex.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
	}

	private IEnumerable<KeyValuePair<string, double>> DrawChartResultNGOne()
	{
		//IL_0366: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Expected O, but got Unknown
		IEnumerable<KeyValuePair<string, double>> enumerable = null;
		try
		{
			string uriName = "/api/Statistic/GetResultNGForOneDates";
			chartResultNGOne.Titles[0].Text = Common.getTextLanguage(this, "RateNGOne") + " " + rbDays.Text.ToLower();
			chartResultNGOne.Titles[1].Text = "← " + Common.getTextLanguage(this, "Product") + " →";
			chartProductNG.Titles[0].Text = Common.getTextLanguage(this, "RateNGProduct") ?? "";
			chartProductNG.Titles[1].Text = "← " + rbDays.Text + " →";
			chartResultNG.Titles[0].Text = Common.getTextLanguage(this, "RateNGResult") ?? "";
			chartResultNG.Titles[1].Text = "← " + rbDays.Text + " →";
			chartTypeNG.Titles[0].Text = Common.getTextLanguage(this, "RateNGType") ?? "";
			chartTypeNG.Titles[1].Text = "← " + rbDays.Text + " →";
			if (rbWeeklys.Checked)
			{
				uriName = "/api/Statistic/GetResultNGForOneWeeklys";
				chartResultNGOne.Titles[0].Text = Common.getTextLanguage(this, "RateNGOne") + " " + rbWeeklys.Text.ToLower();
				chartProductNG.Titles[1].Text = "← " + rbWeeklys.Text + " →";
				chartResultNG.Titles[1].Text = "← " + rbWeeklys.Text + " →";
				chartTypeNG.Titles[1].Text = "← " + rbWeeklys.Text + " →";
			}
			else if (rbMonth.Checked)
			{
				uriName = "/api/Statistic/GetResultNGForOneMonths";
				chartResultNGOne.Titles[0].Text = Common.getTextLanguage(this, "RateNGOne") + " " + rbMonth.Text.ToLower();
				chartProductNG.Titles[1].Text = "← " + rbMonth.Text + " →";
				chartResultNG.Titles[1].Text = "← " + rbMonth.Text + " →";
				chartTypeNG.Titles[1].Text = "← " + rbMonth.Text + " →";
			}
			StatisticDto body = new StatisticDto
			{
				Date = mcalendarSearch.SelectionRange.Start
			};
			ResponseDto result = frmLogin.client.GetsAsync(body, uriName).Result;
			StatisticViewModel val = Common.getObject<StatisticViewModel>((object)result);
			chartResultNGOne.Series.Clear();
			if (string.IsNullOrEmpty(val.Name))
			{
				return null;
			}
			enumerable = val.Values.OrderByDescending((KeyValuePair<string, double> x) => x.Value).Take(int.Parse(cbbTop.Text));
			Series series = new Series
			{
				ChartArea = "ChartArea1",
				IsValueShownAsLabel = true,
				LabelBackColor = Color.Gray,
				LabelForeColor = Color.White,
				LabelFormat = "0.##'%'",
				Legend = "Legend1",
				Name = val.Name
			};
			chartResultNGOne.Series.Add(series);
			foreach (KeyValuePair<string, double> item in enumerable)
			{
				series.Points.AddXY(item.Key, item.Value);
			}
			chartResultNGOne.ChartAreas[0].RecalculateAxesScale();
		}
		catch (Exception ex)
		{
			if (ex.InnerException is ApiException { StatusCode: var statusCode } ex2)
			{
				if (statusCode.Equals(401))
				{
					if (MessageBox.Show(Common.getTextLanguage(this, "LoginAgain"), Common.getTextLanguage(this, "INFORMATION"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk).Equals(DialogResult.OK))
					{
						Close();
					}
				}
				else
				{
					debugOutput("ERR: " + ex2.Message.Replace("\n", ""));
					MessageBox.Show(ex2.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			else
			{
				debugOutput("ERR: " + ex.Message);
				MessageBox.Show(ex.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
		return enumerable;
	}

	private void DrawTreeDetailNGOne(IEnumerable<KeyValuePair<string, double>> statistics)
	{
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Expected O, but got Unknown
		try
		{
			treeViewDetail.Nodes.Clear();
			mDataTree = new List<StatisticViewModel>();
			if (statistics == null)
			{
				return;
			}
			string uriName = "/api/Statistic/GetDetailNGForOneDates";
			if (rbWeeklys.Checked)
			{
				uriName = "/api/Statistic/GetDetailNGForOneWeeklys";
			}
			else if (rbMonth.Checked)
			{
				uriName = "/api/Statistic/GetDetailNGForOneMonths";
			}
			StatisticDto body = new StatisticDto
			{
				Date = mcalendarSearch.SelectionRange.Start
			};
			ResponseDto result = frmLogin.client.GetsAsync(body, uriName).Result;
			List<StatisticViewModel> objectToList = Common.getObjectToList<StatisticViewModel>(result);
			foreach (KeyValuePair<string, double> statistic in statistics)
			{
				StatisticViewModel val = objectToList.FirstOrDefault((StatisticViewModel x) => x.Name.Equals(statistic.Key));
				if (val == null)
				{
					continue;
				}
				IEnumerable<KeyValuePair<string, double>> enumerable = val.Values.OrderByDescending((KeyValuePair<string, double> x) => x.Value).Take(int.Parse(cbbTop.Text));
				mDataTree.Add(val);
				TreeNode treeNode = new TreeNode
				{
					Text = $"{val.Name} ({statistic.Value:0.##}%)"
				};
				foreach (KeyValuePair<string, double> item in enumerable)
				{
					TreeNode node = new TreeNode
					{
						Text = $"{item.Key} ({item.Value})"
					};
					treeNode.Nodes.Add(node);
				}
				treeViewDetail.Nodes.Add(treeNode);
			}
		}
		catch (Exception ex)
		{
			if (ex.InnerException is ApiException { StatusCode: var statusCode } ex2)
			{
				if (statusCode.Equals(401))
				{
					if (MessageBox.Show(Common.getTextLanguage(this, "LoginAgain"), Common.getTextLanguage(this, "INFORMATION"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk).Equals(DialogResult.OK))
					{
						Close();
					}
				}
				else
				{
					debugOutput("ERR: " + ex2.Message.Replace("\n", ""));
					MessageBox.Show(ex2.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			else
			{
				debugOutput("ERR: " + ex.Message);
				MessageBox.Show(ex.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
	}

	private void GetTypeNGOne()
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		try
		{
			string uriName = "/api/Statistic/GetTypeNGForOneDates";
			if (rbWeeklys.Checked)
			{
				uriName = "/api/Statistic/GetTypeNGForOneWeeklys";
			}
			else if (rbMonth.Checked)
			{
				uriName = "/api/Statistic/GetTypeNGForOneMonths";
			}
			StatisticDto body = new StatisticDto
			{
				Date = mcalendarSearch.SelectionRange.Start
			};
			ResponseDto result = frmLogin.client.GetsAsync(body, uriName).Result;
			List<StatisticViewModel> objectToList = Common.getObjectToList<StatisticViewModel>(result);
			mDataType = objectToList;
		}
		catch (Exception ex)
		{
			if (ex.InnerException is ApiException { StatusCode: var statusCode } ex2)
			{
				if (statusCode.Equals(401))
				{
					if (MessageBox.Show(Common.getTextLanguage(this, "LoginAgain"), Common.getTextLanguage(this, "INFORMATION"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk).Equals(DialogResult.OK))
					{
						Close();
					}
				}
				else
				{
					debugOutput("ERR: " + ex2.Message.Replace("\n", ""));
					MessageBox.Show(ex2.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			else
			{
				debugOutput("ERR: " + ex.Message);
				MessageBox.Show(ex.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
	}

	private void ptbAvatar_DoubleClick(object sender, EventArgs e)
	{
		Common.activeForm(typeof(frmLogin));
	}

	private void mcalendarSearch_DateSelected(object sender, DateRangeEventArgs e)
	{
		load_AllData();
	}

	private void chartMain_MouseMove(object sender, MouseEventArgs e)
	{
		Chart chart = sender as Chart;
		if (mPreHit != null)
		{
			DataPoint dataPoint = mPreHit.Object as DataPoint;
			if (!mPreHit.Series.Name.Equals("Average"))
			{
				dataPoint.IsValueShownAsLabel = false;
			}
		}
		HitTestResult hitTestResult = chart.HitTest(e.X, e.Y, ChartElementType.DataPoint);
		if (hitTestResult.ChartElementType.Equals(ChartElementType.DataPoint))
		{
			mPreHit = hitTestResult;
			if (mPreHit != null)
			{
				DataPoint dataPoint2 = mPreHit.Object as DataPoint;
				dataPoint2.IsValueShownAsLabel = true;
			}
		}
	}

	private void rb_CheckedChanged(object sender, EventArgs e)
	{
		RadioButton radioButton = sender as RadioButton;
		if (radioButton.Checked)
		{
			load_AllData();
		}
	}

	private void cbbTop_SelectedIndexChanged(object sender, EventArgs e)
	{
		setSetting();
		load_AllData();
	}

	private void treeViewDetail_AfterSelect(object sender, TreeViewEventArgs e)
	{
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Expected O, but got Unknown
		System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
		EnDisControlMain(en: true);
		panelMain.Controls.Clear();
		string type = "Day";
		if (rbWeeklys.Checked)
		{
			type = "Week";
		}
		else if (rbMonth.Checked)
		{
			type = "Month";
		}
		foreach (StatisticViewModel model in mDataTree)
		{
			StatisticDetailDto dto = new StatisticDetailDto
			{
				Date = mcalendarSearch.SelectionRange.Start,
				ProductCode = model.Name,
				Type = type
			};
			StatisticViewModel type2 = mDataType.FirstOrDefault((StatisticViewModel x) => x.Name.Equals(model.Name));
			mChartDetail mChartDetail2 = new mChartDetail(model, type2, int.Parse(cbbTop.Text), dto)
			{
				Dock = DockStyle.Top,
				Size = new Size(800, 230)
			};
			panelMain.Controls.Add(mChartDetail2);
			mChartDetail2.BringToFront();
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
		System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
		System.Windows.Forms.DataVisualization.Charting.Legend legend = new System.Windows.Forms.DataVisualization.Charting.Legend();
		System.Windows.Forms.DataVisualization.Charting.Title title = new System.Windows.Forms.DataVisualization.Charting.Title();
		System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
		System.Windows.Forms.DataVisualization.Charting.Title title3 = new System.Windows.Forms.DataVisualization.Charting.Title();
		System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
		System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
		System.Windows.Forms.DataVisualization.Charting.Title title4 = new System.Windows.Forms.DataVisualization.Charting.Title();
		System.Windows.Forms.DataVisualization.Charting.Title title5 = new System.Windows.Forms.DataVisualization.Charting.Title();
		System.Windows.Forms.DataVisualization.Charting.Title title6 = new System.Windows.Forms.DataVisualization.Charting.Title();
		System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
		System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
		System.Windows.Forms.DataVisualization.Charting.Title title7 = new System.Windows.Forms.DataVisualization.Charting.Title();
		System.Windows.Forms.DataVisualization.Charting.Title title8 = new System.Windows.Forms.DataVisualization.Charting.Title();
		System.Windows.Forms.DataVisualization.Charting.Title title9 = new System.Windows.Forms.DataVisualization.Charting.Title();
		System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
		System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
		System.Windows.Forms.DataVisualization.Charting.Title title10 = new System.Windows.Forms.DataVisualization.Charting.Title();
		System.Windows.Forms.DataVisualization.Charting.Title title11 = new System.Windows.Forms.DataVisualization.Charting.Title();
		System.Windows.Forms.DataVisualization.Charting.Title title12 = new System.Windows.Forms.DataVisualization.Charting.Title();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_5S_QA_System.frmStatisticView));
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.rbDays = new System.Windows.Forms.RadioButton();
		this.rbWeeklys = new System.Windows.Forms.RadioButton();
		this.rbMonth = new System.Windows.Forms.RadioButton();
		this.cbbTop = new System.Windows.Forms.ComboBox();
		this.statusStripfrmMain = new System.Windows.Forms.StatusStrip();
		this.sprogbarStatus = new System.Windows.Forms.ToolStripProgressBar();
		this.slblStatus = new System.Windows.Forms.ToolStripStatusLabel();
		this.panelSearch = new System.Windows.Forms.Panel();
		this.panelDetail = new System.Windows.Forms.Panel();
		this.treeViewDetail = new System.Windows.Forms.TreeView();
		this.tpanelTop = new System.Windows.Forms.TableLayoutPanel();
		this.lblTop = new System.Windows.Forms.Label();
		this.mcalendarSearch = new System.Windows.Forms.MonthCalendar();
		this.tpanelSelect = new System.Windows.Forms.TableLayoutPanel();
		this.panelSearchResize = new System.Windows.Forms.Panel();
		this.panelMain = new System.Windows.Forms.Panel();
		this.tpanelChart = new System.Windows.Forms.TableLayoutPanel();
		this.chartResultNGOne = new System.Windows.Forms.DataVisualization.Charting.Chart();
		this.chartTypeNG = new System.Windows.Forms.DataVisualization.Charting.Chart();
		this.chartResultNG = new System.Windows.Forms.DataVisualization.Charting.Chart();
		this.chartProductNG = new System.Windows.Forms.DataVisualization.Charting.Chart();
		this.panelLogout = new System.Windows.Forms.Panel();
		this.lblFullname = new System.Windows.Forms.Label();
		this.ptbAvatar = new System.Windows.Forms.PictureBox();
		this.statusStripfrmMain.SuspendLayout();
		this.panelSearch.SuspendLayout();
		this.panelDetail.SuspendLayout();
		this.tpanelTop.SuspendLayout();
		this.tpanelSelect.SuspendLayout();
		this.tpanelChart.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.chartResultNGOne).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.chartTypeNG).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.chartResultNG).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.chartProductNG).BeginInit();
		this.panelLogout.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.ptbAvatar).BeginInit();
		base.SuspendLayout();
		this.rbDays.AutoSize = true;
		this.rbDays.Checked = true;
		this.rbDays.Cursor = System.Windows.Forms.Cursors.Hand;
		this.rbDays.Dock = System.Windows.Forms.DockStyle.Fill;
		this.rbDays.Location = new System.Drawing.Point(4, 4);
		this.rbDays.Name = "rbDays";
		this.rbDays.Size = new System.Drawing.Size(67, 20);
		this.rbDays.TabIndex = 0;
		this.rbDays.TabStop = true;
		this.rbDays.Text = "Day";
		this.toolTipMain.SetToolTip(this.rbDays, "Select statistic for day");
		this.rbDays.UseVisualStyleBackColor = true;
		this.rbWeeklys.AutoSize = true;
		this.rbWeeklys.Cursor = System.Windows.Forms.Cursors.Hand;
		this.rbWeeklys.Dock = System.Windows.Forms.DockStyle.Fill;
		this.rbWeeklys.Location = new System.Drawing.Point(78, 4);
		this.rbWeeklys.Name = "rbWeeklys";
		this.rbWeeklys.Size = new System.Drawing.Size(68, 20);
		this.rbWeeklys.TabIndex = 1;
		this.rbWeeklys.Text = "Week";
		this.toolTipMain.SetToolTip(this.rbWeeklys, "Select statistic for week");
		this.rbWeeklys.UseVisualStyleBackColor = true;
		this.rbMonth.AutoSize = true;
		this.rbMonth.Cursor = System.Windows.Forms.Cursors.Hand;
		this.rbMonth.Dock = System.Windows.Forms.DockStyle.Fill;
		this.rbMonth.Location = new System.Drawing.Point(153, 4);
		this.rbMonth.Name = "rbMonth";
		this.rbMonth.Size = new System.Drawing.Size(69, 20);
		this.rbMonth.TabIndex = 2;
		this.rbMonth.Text = "Month";
		this.toolTipMain.SetToolTip(this.rbMonth, "Select statistic for month");
		this.rbMonth.UseVisualStyleBackColor = true;
		this.cbbTop.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.cbbTop.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cbbTop.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbbTop.Dock = System.Windows.Forms.DockStyle.Fill;
		this.cbbTop.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbbTop.FormattingEnabled = true;
		this.cbbTop.Items.AddRange(new object[20]
		{
			"5", "10", "15", "20", "25", "30", "35", "40", "45", "50",
			"55", "60", "65", "70", "75", "80", "85", "90", "95", "100"
		});
		this.cbbTop.Location = new System.Drawing.Point(66, 4);
		this.cbbTop.Name = "cbbTop";
		this.cbbTop.Size = new System.Drawing.Size(154, 24);
		this.cbbTop.TabIndex = 1;
		this.toolTipMain.SetToolTip(this.cbbTop, "Select top ng");
		this.statusStripfrmMain.BackColor = System.Drawing.SystemColors.ControlLight;
		this.statusStripfrmMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.statusStripfrmMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.sprogbarStatus, this.slblStatus });
		this.statusStripfrmMain.Location = new System.Drawing.Point(20, 554);
		this.statusStripfrmMain.Name = "statusStripfrmMain";
		this.statusStripfrmMain.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
		this.statusStripfrmMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
		this.statusStripfrmMain.Size = new System.Drawing.Size(1060, 26);
		this.statusStripfrmMain.SizingGrip = false;
		this.statusStripfrmMain.Stretch = false;
		this.statusStripfrmMain.TabIndex = 8;
		this.sprogbarStatus.Name = "sprogbarStatus";
		this.sprogbarStatus.Size = new System.Drawing.Size(200, 20);
		this.slblStatus.Name = "slblStatus";
		this.slblStatus.Size = new System.Drawing.Size(16, 21);
		this.slblStatus.Text = "...";
		this.panelSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panelSearch.Controls.Add(this.panelDetail);
		this.panelSearch.Controls.Add(this.mcalendarSearch);
		this.panelSearch.Controls.Add(this.tpanelSelect);
		this.panelSearch.Controls.Add(this.panelSearchResize);
		this.panelSearch.Dock = System.Windows.Forms.DockStyle.Right;
		this.panelSearch.Location = new System.Drawing.Point(846, 70);
		this.panelSearch.Name = "panelSearch";
		this.panelSearch.Padding = new System.Windows.Forms.Padding(0, 3, 3, 3);
		this.panelSearch.Size = new System.Drawing.Size(234, 484);
		this.panelSearch.TabIndex = 21;
		this.panelDetail.AutoScroll = true;
		this.panelDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panelDetail.Controls.Add(this.treeViewDetail);
		this.panelDetail.Controls.Add(this.tpanelTop);
		this.panelDetail.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panelDetail.Location = new System.Drawing.Point(3, 193);
		this.panelDetail.Name = "panelDetail";
		this.panelDetail.Size = new System.Drawing.Size(226, 286);
		this.panelDetail.TabIndex = 2;
		this.treeViewDetail.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.treeViewDetail.Dock = System.Windows.Forms.DockStyle.Fill;
		this.treeViewDetail.Location = new System.Drawing.Point(0, 35);
		this.treeViewDetail.Name = "treeViewDetail";
		this.treeViewDetail.Size = new System.Drawing.Size(224, 249);
		this.treeViewDetail.TabIndex = 0;
		this.treeViewDetail.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(treeViewDetail_AfterSelect);
		this.tpanelTop.AutoSize = true;
		this.tpanelTop.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		this.tpanelTop.ColumnCount = 2;
		this.tpanelTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tpanelTop.Controls.Add(this.lblTop, 0, 0);
		this.tpanelTop.Controls.Add(this.cbbTop, 1, 0);
		this.tpanelTop.Dock = System.Windows.Forms.DockStyle.Top;
		this.tpanelTop.Location = new System.Drawing.Point(0, 0);
		this.tpanelTop.Name = "tpanelTop";
		this.tpanelTop.RowCount = 1;
		this.tpanelTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33f));
		this.tpanelTop.Size = new System.Drawing.Size(224, 35);
		this.tpanelTop.TabIndex = 1;
		this.lblTop.AutoSize = true;
		this.lblTop.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTop.Location = new System.Drawing.Point(4, 1);
		this.lblTop.Name = "lblTop";
		this.lblTop.Size = new System.Drawing.Size(55, 33);
		this.lblTop.TabIndex = 0;
		this.lblTop.Text = "Top NG";
		this.lblTop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.mcalendarSearch.Cursor = System.Windows.Forms.Cursors.Hand;
		this.mcalendarSearch.Dock = System.Windows.Forms.DockStyle.Top;
		this.mcalendarSearch.Location = new System.Drawing.Point(3, 31);
		this.mcalendarSearch.Name = "mcalendarSearch";
		this.mcalendarSearch.TabIndex = 1;
		this.mcalendarSearch.DateSelected += new System.Windows.Forms.DateRangeEventHandler(mcalendarSearch_DateSelected);
		this.tpanelSelect.AutoSize = true;
		this.tpanelSelect.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		this.tpanelSelect.ColumnCount = 3;
		this.tpanelSelect.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333f));
		this.tpanelSelect.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334f));
		this.tpanelSelect.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334f));
		this.tpanelSelect.Controls.Add(this.rbMonth, 2, 0);
		this.tpanelSelect.Controls.Add(this.rbWeeklys, 1, 0);
		this.tpanelSelect.Controls.Add(this.rbDays, 0, 0);
		this.tpanelSelect.Dock = System.Windows.Forms.DockStyle.Top;
		this.tpanelSelect.Location = new System.Drawing.Point(3, 3);
		this.tpanelSelect.Name = "tpanelSelect";
		this.tpanelSelect.RowCount = 1;
		this.tpanelSelect.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tpanelSelect.Size = new System.Drawing.Size(226, 28);
		this.tpanelSelect.TabIndex = 0;
		this.panelSearchResize.Dock = System.Windows.Forms.DockStyle.Left;
		this.panelSearchResize.Location = new System.Drawing.Point(0, 3);
		this.panelSearchResize.Name = "panelSearchResize";
		this.panelSearchResize.Size = new System.Drawing.Size(3, 476);
		this.panelSearchResize.TabIndex = 3;
		this.panelMain.AutoScroll = true;
		this.panelMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panelMain.Location = new System.Drawing.Point(20, 70);
		this.panelMain.Name = "panelMain";
		this.panelMain.Size = new System.Drawing.Size(826, 484);
		this.panelMain.TabIndex = 23;
		this.tpanelChart.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		this.tpanelChart.ColumnCount = 2;
		this.tpanelChart.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tpanelChart.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tpanelChart.Controls.Add(this.chartResultNGOne, 1, 1);
		this.tpanelChart.Controls.Add(this.chartTypeNG, 0, 1);
		this.tpanelChart.Controls.Add(this.chartResultNG, 1, 0);
		this.tpanelChart.Controls.Add(this.chartProductNG, 0, 0);
		this.tpanelChart.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tpanelChart.Location = new System.Drawing.Point(20, 70);
		this.tpanelChart.Name = "tpanelChart";
		this.tpanelChart.RowCount = 2;
		this.tpanelChart.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tpanelChart.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tpanelChart.Size = new System.Drawing.Size(826, 484);
		this.tpanelChart.TabIndex = 24;
		chartArea.AxisX.Interval = 1.0;
		chartArea.AxisX.MajorGrid.Enabled = false;
		chartArea.AxisY.LabelStyle.Format = "0'%'";
		chartArea.AxisY.MajorGrid.Enabled = false;
		chartArea.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
		chartArea.Name = "ChartArea1";
		this.chartResultNGOne.ChartAreas.Add(chartArea);
		this.chartResultNGOne.Dock = System.Windows.Forms.DockStyle.Fill;
		legend.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
		legend.Name = "Legend1";
		this.chartResultNGOne.Legends.Add(legend);
		this.chartResultNGOne.Location = new System.Drawing.Point(416, 245);
		this.chartResultNGOne.Name = "chartResultNGOne";
		this.chartResultNGOne.Size = new System.Drawing.Size(406, 235);
		this.chartResultNGOne.TabIndex = 3;
		this.chartResultNGOne.Text = "chart1";
		title.BackColor = System.Drawing.Color.PaleTurquoise;
		title.BorderColor = System.Drawing.Color.Black;
		title.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		title.ForeColor = System.Drawing.Color.Blue;
		title.Name = "Title1";
		title.Text = "Tỉ lệ NG theo sản phẩm / Tổng điểm đo một ngày";
		title2.BorderColor = System.Drawing.Color.Black;
		title2.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
		title2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		title2.Name = "Title2";
		title2.Text = "← Product →";
		title3.BorderColor = System.Drawing.Color.Black;
		title3.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Left;
		title3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		title3.Name = "Title3";
		title3.Text = "← % →";
		this.chartResultNGOne.Titles.Add(title);
		this.chartResultNGOne.Titles.Add(title2);
		this.chartResultNGOne.Titles.Add(title3);
		chartArea2.AxisX.Interval = 1.0;
		chartArea2.AxisX.MajorGrid.Enabled = false;
		chartArea2.AxisY.LabelStyle.Format = "0'%'";
		chartArea2.AxisY.MajorGrid.Enabled = false;
		chartArea2.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
		chartArea2.Name = "ChartArea1";
		this.chartTypeNG.ChartAreas.Add(chartArea2);
		this.chartTypeNG.Dock = System.Windows.Forms.DockStyle.Fill;
		legend2.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
		legend2.Name = "Legend1";
		this.chartTypeNG.Legends.Add(legend2);
		this.chartTypeNG.Location = new System.Drawing.Point(4, 245);
		this.chartTypeNG.Name = "chartTypeNG";
		this.chartTypeNG.Size = new System.Drawing.Size(405, 235);
		this.chartTypeNG.TabIndex = 2;
		this.chartTypeNG.Text = "chart1";
		title4.BackColor = System.Drawing.Color.PaleTurquoise;
		title4.BorderColor = System.Drawing.Color.Black;
		title4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		title4.ForeColor = System.Drawing.Color.Blue;
		title4.Name = "Title1";
		title4.Text = "Phân tích thời điểm phát sinh NG";
		title5.BorderColor = System.Drawing.Color.Black;
		title5.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
		title5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		title5.Name = "Title2";
		title5.Text = "← Day →";
		title6.BorderColor = System.Drawing.Color.Black;
		title6.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Left;
		title6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		title6.Name = "Title3";
		title6.Text = "← % →";
		this.chartTypeNG.Titles.Add(title4);
		this.chartTypeNG.Titles.Add(title5);
		this.chartTypeNG.Titles.Add(title6);
		this.chartTypeNG.MouseMove += new System.Windows.Forms.MouseEventHandler(chartMain_MouseMove);
		chartArea3.AxisX.Interval = 1.0;
		chartArea3.AxisX.MajorGrid.Enabled = false;
		chartArea3.AxisY.LabelStyle.Format = "0'%'";
		chartArea3.AxisY.MajorGrid.Enabled = false;
		chartArea3.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
		chartArea3.Name = "ChartArea1";
		this.chartResultNG.ChartAreas.Add(chartArea3);
		this.chartResultNG.Dock = System.Windows.Forms.DockStyle.Fill;
		legend3.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
		legend3.Name = "Legend1";
		this.chartResultNG.Legends.Add(legend3);
		this.chartResultNG.Location = new System.Drawing.Point(416, 4);
		this.chartResultNG.Name = "chartResultNG";
		this.chartResultNG.Size = new System.Drawing.Size(406, 234);
		this.chartResultNG.TabIndex = 1;
		this.chartResultNG.Text = "chart1";
		title7.BackColor = System.Drawing.Color.PaleTurquoise;
		title7.BorderColor = System.Drawing.Color.Black;
		title7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		title7.ForeColor = System.Drawing.Color.Blue;
		title7.Name = "Title1";
		title7.Text = "Tỉ lệ NG theo sản phẩm / Tổng điểm đo";
		title8.BorderColor = System.Drawing.Color.Black;
		title8.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
		title8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		title8.Name = "Title2";
		title8.Text = "← Day →";
		title9.BorderColor = System.Drawing.Color.Black;
		title9.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Left;
		title9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		title9.Name = "Title3";
		title9.Text = "← % →";
		this.chartResultNG.Titles.Add(title7);
		this.chartResultNG.Titles.Add(title8);
		this.chartResultNG.Titles.Add(title9);
		this.chartResultNG.MouseMove += new System.Windows.Forms.MouseEventHandler(chartMain_MouseMove);
		chartArea4.AxisX.Interval = 1.0;
		chartArea4.AxisX.MajorGrid.Enabled = false;
		chartArea4.AxisY.LabelStyle.Format = "0'%'";
		chartArea4.AxisY.MajorGrid.Enabled = false;
		chartArea4.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
		chartArea4.Name = "ChartArea1";
		this.chartProductNG.ChartAreas.Add(chartArea4);
		this.chartProductNG.Dock = System.Windows.Forms.DockStyle.Fill;
		legend4.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
		legend4.Name = "Legend1";
		this.chartProductNG.Legends.Add(legend4);
		this.chartProductNG.Location = new System.Drawing.Point(4, 4);
		this.chartProductNG.Name = "chartProductNG";
		this.chartProductNG.Size = new System.Drawing.Size(405, 234);
		this.chartProductNG.TabIndex = 0;
		this.chartProductNG.Text = "chart1";
		title10.BackColor = System.Drawing.Color.PaleTurquoise;
		title10.BorderColor = System.Drawing.Color.Black;
		title10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		title10.ForeColor = System.Drawing.Color.Blue;
		title10.Name = "Title1";
		title10.Text = "Tỉ lệ NG theo số lượng sản phẩm kiểm tra";
		title11.BorderColor = System.Drawing.Color.Black;
		title11.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
		title11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		title11.Name = "Title2";
		title11.Text = "← Day →";
		title12.BorderColor = System.Drawing.Color.Black;
		title12.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Left;
		title12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		title12.Name = "Title3";
		title12.Text = "← % →";
		this.chartProductNG.Titles.Add(title10);
		this.chartProductNG.Titles.Add(title11);
		this.chartProductNG.Titles.Add(title12);
		this.panelLogout.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.panelLogout.BackColor = System.Drawing.Color.Transparent;
		this.panelLogout.Controls.Add(this.lblFullname);
		this.panelLogout.Controls.Add(this.ptbAvatar);
		this.panelLogout.Location = new System.Drawing.Point(730, 27);
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
		base.ClientSize = new System.Drawing.Size(1100, 600);
		base.Controls.Add(this.panelLogout);
		base.Controls.Add(this.tpanelChart);
		base.Controls.Add(this.panelMain);
		base.Controls.Add(this.panelSearch);
		base.Controls.Add(this.statusStripfrmMain);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "frmStatisticView";
		base.Padding = new System.Windows.Forms.Padding(20, 70, 20, 20);
		this.Text = "5S QA System * STATISTIC";
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmStatisticView_FormClosed);
		base.Load += new System.EventHandler(frmStatisticView_Load);
		base.Shown += new System.EventHandler(frmStatisticView_Shown);
		this.statusStripfrmMain.ResumeLayout(false);
		this.statusStripfrmMain.PerformLayout();
		this.panelSearch.ResumeLayout(false);
		this.panelSearch.PerformLayout();
		this.panelDetail.ResumeLayout(false);
		this.panelDetail.PerformLayout();
		this.tpanelTop.ResumeLayout(false);
		this.tpanelTop.PerformLayout();
		this.tpanelSelect.ResumeLayout(false);
		this.tpanelSelect.PerformLayout();
		this.tpanelChart.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.chartResultNGOne).EndInit();
		((System.ComponentModel.ISupportInitialize)this.chartTypeNG).EndInit();
		((System.ComponentModel.ISupportInitialize)this.chartResultNG).EndInit();
		((System.ComponentModel.ISupportInitialize)this.chartProductNG).EndInit();
		this.panelLogout.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.ptbAvatar).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
