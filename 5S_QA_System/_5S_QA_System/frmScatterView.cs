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

public class frmScatterView : MetroForm
{
	private HitTestResult mPreHit;

	private IContainer components = null;

	private SaveFileDialog saveFileDialogMain;

	private OpenFileDialog openFileDialogMain;

	private ToolTip toolTipMain;

	private ToolStripProgressBar sprogbarStatus;

	public ToolStripStatusLabel slblStatus;

	private ContextMenuStrip contextMenuStripMain;

	private ToolStripMenuItem main_refreshToolStripMenuItem;

	private StatusStrip statusStripfrmMain;

	private Panel panelLogout;

	private Label lblFullname;

	private PictureBox ptbAvatar;

	private mSearchScatter mSearchMain;

	private Chart chartMain;

	private TableLayoutPanel tpanelInfor;

	private Label lblTitleSlope;

	private Label lblTitleCorrel;

	private Label lblIntercept;

	private Label lblTitleIntercept;

	private Label lblSlope;

	private Label lblTitleRSquared;

	private Label lblCorrel;

	private Label lblRSquared;

	private Label lblStartPointY;

	private Label lblTitleStartPointY;

	private Label lblStartPointX;

	private Label lblTitleStartPointX;

	private Label lblTitleEndPointX;

	private Label lblTitleEndPointY;

	private Label lblEndPointY;

	private Label lblEndPointX;

	public frmScatterView()
	{
		InitializeComponent();
		Common.setControls(this, toolTipMain, new List<ContextMenuStrip> { contextMenuStripMain });
	}

	private void frmScatterView_Load(object sender, EventArgs e)
	{
		Init();
	}

	private void frmScatterView_Shown(object sender, EventArgs e)
	{
		mSearchMain.Init();
		mSearchMain.Click += main_refreshToolStripMenuItem_Click;
		load_AllData();
	}

	private void frmScatterView_FormClosed(object sender, FormClosedEventArgs e)
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
		chartMain.Series[0].Name = Common.getTextLanguage(this, "Scatter");
		chartMain.Series[1].Name = Common.getTextLanguage(this, "TrendLine");
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

	public void load_AllData()
	{
		System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
		try
		{
			start_Proccessor();
			QueryArgs body = new QueryArgs
			{
				Predicate = "(MeasurementId=@0 || MeasurementId=@1) && Request.Date>=@2 && Request.Date<=@3 && !Request.Status.Contains(@4) && !Request.Status.Contains(@5) && !string.IsNullOrEmpty(Result)",
				PredicateParameters = new List<object>
				{
					(mSearchMain.cbbAxisX.SelectedIndex == -1) ? Guid.Empty.ToString() : mSearchMain.cbbAxisX.SelectedValue.ToString(),
					(mSearchMain.cbbAxisY.SelectedIndex == -1) ? Guid.Empty.ToString() : mSearchMain.cbbAxisY.SelectedValue.ToString(),
					mSearchMain.dtpDateFrom.Value.ToString("MM/dd/yyyy"),
					mSearchMain.dtpDateTo.Value.ToString("MM/dd/yyyy"),
					"Activated",
					"Rejected"
				},
				Order = "Request.Date, Request.Created, Request.Name, Measurement.Sort, Measurement.Created, Sample, Cavity",
				Page = 1,
				Limit = int.MaxValue
			};
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/RequestResult/Gets").Result;
			List<RequestResultViewModel> objectToList = Common.getObjectToList<RequestResultViewModel>(result);
			DrawChart(objectToList);
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

	private void DrawChart(List<RequestResultViewModel> models)
	{
		start_Proccessor();
		Guid xId = ((mSearchMain.cbbAxisX.SelectedIndex == -1) ? Guid.Empty : Guid.Parse(mSearchMain.cbbAxisX.SelectedValue.ToString()));
		List<RequestResultViewModel> source = models.Where((RequestResultViewModel x) => x.MeasurementId == xId).ToList();
		List<double> list = source.Select((RequestResultViewModel x) => double.Parse(x.Result)).ToList();
		Guid yId = ((mSearchMain.cbbAxisY.SelectedIndex == -1) ? Guid.Empty : Guid.Parse(mSearchMain.cbbAxisY.SelectedValue.ToString()));
		List<RequestResultViewModel> source2 = models.Where((RequestResultViewModel x) => x.MeasurementId == yId).ToList();
		List<double> list2 = source2.Select((RequestResultViewModel x) => double.Parse(x.Result)).ToList();
		chartMain.Series[0].Points.Clear();
		chartMain.Series[1].Points.Clear();
		LinearTrend linearTrend = new LinearTrend(list, list2);
		if (linearTrend.Count == 0)
		{
			ResetInfor();
			return;
		}
		double num = list.Max();
		double num2 = list.Min();
		double num3 = (num - num2) / (double)linearTrend.Count;
		double num4 = list2.Max();
		double num5 = list2.Min();
		double num6 = (num4 - num5) / (double)linearTrend.Count;
		chartMain.ChartAreas[0].AxisX.Maximum = num + num3;
		chartMain.ChartAreas[0].AxisX.Minimum = num2 - num3;
		chartMain.ChartAreas[0].AxisX.Interval = ((num3 == 0.0) ? 1.0 : num3);
		chartMain.ChartAreas[0].AxisX.Title = mSearchMain.cbbAxisX.Text;
		chartMain.ChartAreas[0].AxisY.Maximum = num4 + num6;
		chartMain.ChartAreas[0].AxisY.Minimum = num5 - num6;
		chartMain.ChartAreas[0].AxisY.Interval = ((num6 == 0.0) ? 1.0 : num6);
		chartMain.ChartAreas[0].AxisY.Title = mSearchMain.cbbAxisY.Text;
		lblSlope.Text = linearTrend.Slope?.ToString("0.####");
		lblIntercept.Text = linearTrend.Intercept?.ToString("0.####");
		lblCorrel.Text = linearTrend.Correl?.ToString("0.####");
		lblRSquared.Text = linearTrend.R2?.ToString("0.####");
		lblStartPointX.Text = linearTrend.StartPoint.X.ToString("0.####");
		lblStartPointY.Text = linearTrend.StartPoint.Y.ToString("0.####");
		lblEndPointX.Text = linearTrend.EndPoint.X.ToString("0.####");
		lblEndPointY.Text = linearTrend.EndPoint.Y.ToString("0.####");
		foreach (ValueItem dataItem in linearTrend.DataItems)
		{
			chartMain.Series[0].Points.AddXY(dataItem.X, dataItem.Y);
		}
		foreach (ValueItem trendItem in linearTrend.TrendItems)
		{
			chartMain.Series[1].Points.AddXY(trendItem.X, trendItem.Y);
		}
		debugOutput(Common.getTextLanguage(this, "Successful"));
	}

	private void ResetInfor()
	{
		lblSlope.Text = "...";
		lblIntercept.Text = "...";
		lblCorrel.Text = "...";
		lblRSquared.Text = "...";
		lblStartPointX.Text = "...";
		lblStartPointY.Text = "...";
		lblEndPointX.Text = "...";
		lblEndPointY.Text = "...";
	}

	private void main_refreshToolStripMenuItem_Click(object sender, EventArgs e)
	{
		load_AllData();
	}

	private void ptbAvatar_DoubleClick(object sender, EventArgs e)
	{
		Common.activeForm(typeof(frmLogin));
	}

	private void chartMain_MouseMove(object sender, MouseEventArgs e)
	{
		if (mPreHit != null)
		{
			DataPoint dataPoint = mPreHit.Object as DataPoint;
			dataPoint.IsValueShownAsLabel = false;
			dataPoint.MarkerSize = 5;
		}
		HitTestResult hitTestResult = chartMain.HitTest(e.X, e.Y, ChartElementType.DataPoint);
		if (hitTestResult.ChartElementType.Equals(ChartElementType.DataPoint))
		{
			mPreHit = hitTestResult;
			if (mPreHit != null)
			{
				DataPoint dataPoint2 = mPreHit.Object as DataPoint;
				dataPoint2.IsValueShownAsLabel = true;
				dataPoint2.MarkerSize += 3;
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
		this.components = new System.ComponentModel.Container();
		System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
		System.Windows.Forms.DataVisualization.Charting.Legend legend = new System.Windows.Forms.DataVisualization.Charting.Legend();
		System.Windows.Forms.DataVisualization.Charting.Series series = new System.Windows.Forms.DataVisualization.Charting.Series();
		System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_5S_QA_System.frmScatterView));
		this.saveFileDialogMain = new System.Windows.Forms.SaveFileDialog();
		this.openFileDialogMain = new System.Windows.Forms.OpenFileDialog();
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.sprogbarStatus = new System.Windows.Forms.ToolStripProgressBar();
		this.slblStatus = new System.Windows.Forms.ToolStripStatusLabel();
		this.contextMenuStripMain = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.main_refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.statusStripfrmMain = new System.Windows.Forms.StatusStrip();
		this.panelLogout = new System.Windows.Forms.Panel();
		this.lblFullname = new System.Windows.Forms.Label();
		this.ptbAvatar = new System.Windows.Forms.PictureBox();
		this.chartMain = new System.Windows.Forms.DataVisualization.Charting.Chart();
		this.tpanelInfor = new System.Windows.Forms.TableLayoutPanel();
		this.lblTitleEndPointY = new System.Windows.Forms.Label();
		this.lblEndPointY = new System.Windows.Forms.Label();
		this.lblEndPointX = new System.Windows.Forms.Label();
		this.lblTitleEndPointX = new System.Windows.Forms.Label();
		this.lblStartPointY = new System.Windows.Forms.Label();
		this.lblTitleStartPointY = new System.Windows.Forms.Label();
		this.lblStartPointX = new System.Windows.Forms.Label();
		this.lblTitleStartPointX = new System.Windows.Forms.Label();
		this.lblRSquared = new System.Windows.Forms.Label();
		this.lblTitleRSquared = new System.Windows.Forms.Label();
		this.lblCorrel = new System.Windows.Forms.Label();
		this.lblTitleCorrel = new System.Windows.Forms.Label();
		this.lblIntercept = new System.Windows.Forms.Label();
		this.lblTitleIntercept = new System.Windows.Forms.Label();
		this.lblSlope = new System.Windows.Forms.Label();
		this.lblTitleSlope = new System.Windows.Forms.Label();
		this.mSearchMain = new _5S_QA_System.mSearchScatter();
		this.contextMenuStripMain.SuspendLayout();
		this.statusStripfrmMain.SuspendLayout();
		this.panelLogout.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.ptbAvatar).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.chartMain).BeginInit();
		this.tpanelInfor.SuspendLayout();
		base.SuspendLayout();
		this.saveFileDialogMain.Filter = "File zip (*.zip)|*.zip";
		this.saveFileDialogMain.Title = "Select folder and enter file name";
		this.openFileDialogMain.Filter = "Files PDF(*.pdf)| *.pdf; ";
		this.openFileDialogMain.Title = "Please select a file pdf";
		this.sprogbarStatus.Name = "sprogbarStatus";
		this.sprogbarStatus.Size = new System.Drawing.Size(200, 20);
		this.slblStatus.Name = "slblStatus";
		this.slblStatus.Size = new System.Drawing.Size(16, 21);
		this.slblStatus.Text = "...";
		this.contextMenuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.main_refreshToolStripMenuItem });
		this.contextMenuStripMain.Name = "contextMenuStripStaff";
		this.contextMenuStripMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
		this.contextMenuStripMain.Size = new System.Drawing.Size(114, 26);
		this.main_refreshToolStripMenuItem.Name = "main_refreshToolStripMenuItem";
		this.main_refreshToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
		this.main_refreshToolStripMenuItem.Text = "Refresh";
		this.main_refreshToolStripMenuItem.Click += new System.EventHandler(main_refreshToolStripMenuItem_Click);
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
		this.statusStripfrmMain.TabIndex = 32;
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
		this.chartMain.BorderlineColor = System.Drawing.Color.Black;
		this.chartMain.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
		chartArea.AxisX.LabelStyle.Format = "0.0000";
		chartArea.AxisX.MajorGrid.Enabled = false;
		chartArea.AxisX.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		chartArea.AxisY.LabelStyle.Format = "0.0000";
		chartArea.AxisY.MajorGrid.Enabled = false;
		chartArea.AxisY.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		chartArea.Name = "ChartArea1";
		this.chartMain.ChartAreas.Add(chartArea);
		this.chartMain.ContextMenuStrip = this.contextMenuStripMain;
		this.chartMain.Dock = System.Windows.Forms.DockStyle.Fill;
		legend.Name = "Legend1";
		this.chartMain.Legends.Add(legend);
		this.chartMain.Location = new System.Drawing.Point(20, 153);
		this.chartMain.Name = "chartMain";
		series.ChartArea = "ChartArea1";
		series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
		series.Legend = "Legend1";
		series.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
		series.Name = "Scatter";
		series2.ChartArea = "ChartArea1";
		series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
		series2.Legend = "Legend1";
		series2.Name = "Trend line";
		this.chartMain.Series.Add(series);
		this.chartMain.Series.Add(series2);
		this.chartMain.Size = new System.Drawing.Size(1060, 401);
		this.chartMain.TabIndex = 178;
		this.chartMain.Text = "chart1";
		this.chartMain.MouseMove += new System.Windows.Forms.MouseEventHandler(chartMain_MouseMove);
		this.tpanelInfor.AutoSize = true;
		this.tpanelInfor.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		this.tpanelInfor.ColumnCount = 17;
		this.tpanelInfor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelInfor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelInfor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelInfor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelInfor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelInfor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelInfor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelInfor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelInfor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelInfor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelInfor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelInfor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelInfor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelInfor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelInfor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelInfor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelInfor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tpanelInfor.Controls.Add(this.lblTitleEndPointY, 14, 0);
		this.tpanelInfor.Controls.Add(this.lblEndPointY, 15, 0);
		this.tpanelInfor.Controls.Add(this.lblEndPointX, 13, 0);
		this.tpanelInfor.Controls.Add(this.lblTitleEndPointX, 12, 0);
		this.tpanelInfor.Controls.Add(this.lblStartPointY, 11, 0);
		this.tpanelInfor.Controls.Add(this.lblTitleStartPointY, 10, 0);
		this.tpanelInfor.Controls.Add(this.lblStartPointX, 9, 0);
		this.tpanelInfor.Controls.Add(this.lblTitleStartPointX, 8, 0);
		this.tpanelInfor.Controls.Add(this.lblRSquared, 7, 0);
		this.tpanelInfor.Controls.Add(this.lblTitleRSquared, 6, 0);
		this.tpanelInfor.Controls.Add(this.lblCorrel, 5, 0);
		this.tpanelInfor.Controls.Add(this.lblTitleCorrel, 4, 0);
		this.tpanelInfor.Controls.Add(this.lblIntercept, 3, 0);
		this.tpanelInfor.Controls.Add(this.lblTitleIntercept, 2, 0);
		this.tpanelInfor.Controls.Add(this.lblSlope, 1, 0);
		this.tpanelInfor.Controls.Add(this.lblTitleSlope, 0, 0);
		this.tpanelInfor.Dock = System.Windows.Forms.DockStyle.Top;
		this.tpanelInfor.Location = new System.Drawing.Point(20, 135);
		this.tpanelInfor.Name = "tpanelInfor";
		this.tpanelInfor.RowCount = 1;
		this.tpanelInfor.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tpanelInfor.Size = new System.Drawing.Size(1060, 18);
		this.tpanelInfor.TabIndex = 179;
		this.lblTitleEndPointY.AutoSize = true;
		this.lblTitleEndPointY.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitleEndPointY.Location = new System.Drawing.Point(617, 1);
		this.lblTitleEndPointY.Name = "lblTitleEndPointY";
		this.lblTitleEndPointY.Size = new System.Drawing.Size(16, 16);
		this.lblTitleEndPointY.TabIndex = 72;
		this.lblTitleEndPointY.Text = "Y";
		this.lblTitleEndPointY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblEndPointY.AutoSize = true;
		this.lblEndPointY.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblEndPointY.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblEndPointY.Location = new System.Drawing.Point(640, 1);
		this.lblEndPointY.Name = "lblEndPointY";
		this.lblEndPointY.Size = new System.Drawing.Size(19, 16);
		this.lblEndPointY.TabIndex = 71;
		this.lblEndPointY.Text = "...";
		this.lblEndPointY.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblEndPointX.AutoSize = true;
		this.lblEndPointX.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblEndPointX.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblEndPointX.Location = new System.Drawing.Point(591, 1);
		this.lblEndPointX.Name = "lblEndPointX";
		this.lblEndPointX.Size = new System.Drawing.Size(19, 16);
		this.lblEndPointX.TabIndex = 70;
		this.lblEndPointX.Text = "...";
		this.lblEndPointX.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblTitleEndPointX.AutoSize = true;
		this.lblTitleEndPointX.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitleEndPointX.Location = new System.Drawing.Point(510, 1);
		this.lblTitleEndPointX.Name = "lblTitleEndPointX";
		this.lblTitleEndPointX.Size = new System.Drawing.Size(74, 16);
		this.lblTitleEndPointX.TabIndex = 69;
		this.lblTitleEndPointX.Text = "End point X";
		this.lblTitleEndPointX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblStartPointY.AutoSize = true;
		this.lblStartPointY.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblStartPointY.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblStartPointY.Location = new System.Drawing.Point(484, 1);
		this.lblStartPointY.Name = "lblStartPointY";
		this.lblStartPointY.Size = new System.Drawing.Size(19, 16);
		this.lblStartPointY.TabIndex = 68;
		this.lblStartPointY.Text = "...";
		this.lblStartPointY.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblTitleStartPointY.AutoSize = true;
		this.lblTitleStartPointY.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitleStartPointY.Location = new System.Drawing.Point(461, 1);
		this.lblTitleStartPointY.Name = "lblTitleStartPointY";
		this.lblTitleStartPointY.Size = new System.Drawing.Size(16, 16);
		this.lblTitleStartPointY.TabIndex = 67;
		this.lblTitleStartPointY.Text = "Y";
		this.lblTitleStartPointY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblStartPointX.AutoSize = true;
		this.lblStartPointX.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblStartPointX.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblStartPointX.Location = new System.Drawing.Point(435, 1);
		this.lblStartPointX.Name = "lblStartPointX";
		this.lblStartPointX.Size = new System.Drawing.Size(19, 16);
		this.lblStartPointX.TabIndex = 66;
		this.lblStartPointX.Text = "...";
		this.lblStartPointX.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblTitleStartPointX.AutoSize = true;
		this.lblTitleStartPointX.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitleStartPointX.Location = new System.Drawing.Point(351, 1);
		this.lblTitleStartPointX.Name = "lblTitleStartPointX";
		this.lblTitleStartPointX.Size = new System.Drawing.Size(77, 16);
		this.lblTitleStartPointX.TabIndex = 65;
		this.lblTitleStartPointX.Text = "Start point X";
		this.lblTitleStartPointX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblRSquared.AutoSize = true;
		this.lblRSquared.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblRSquared.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblRSquared.Location = new System.Drawing.Point(325, 1);
		this.lblRSquared.Name = "lblRSquared";
		this.lblRSquared.Size = new System.Drawing.Size(19, 16);
		this.lblRSquared.TabIndex = 64;
		this.lblRSquared.Text = "...";
		this.lblRSquared.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblTitleRSquared.AutoSize = true;
		this.lblTitleRSquared.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitleRSquared.Location = new System.Drawing.Point(247, 1);
		this.lblTitleRSquared.Name = "lblTitleRSquared";
		this.lblTitleRSquared.Size = new System.Drawing.Size(71, 16);
		this.lblTitleRSquared.TabIndex = 63;
		this.lblTitleRSquared.Text = "R-squared";
		this.lblTitleRSquared.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblCorrel.AutoSize = true;
		this.lblCorrel.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCorrel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCorrel.Location = new System.Drawing.Point(221, 1);
		this.lblCorrel.Name = "lblCorrel";
		this.lblCorrel.Size = new System.Drawing.Size(19, 16);
		this.lblCorrel.TabIndex = 62;
		this.lblCorrel.Text = "...";
		this.lblCorrel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblTitleCorrel.AutoSize = true;
		this.lblTitleCorrel.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitleCorrel.Location = new System.Drawing.Point(171, 1);
		this.lblTitleCorrel.Name = "lblTitleCorrel";
		this.lblTitleCorrel.Size = new System.Drawing.Size(43, 16);
		this.lblTitleCorrel.TabIndex = 61;
		this.lblTitleCorrel.Text = "Correl";
		this.lblTitleCorrel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblIntercept.AutoSize = true;
		this.lblIntercept.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblIntercept.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblIntercept.Location = new System.Drawing.Point(145, 1);
		this.lblIntercept.Name = "lblIntercept";
		this.lblIntercept.Size = new System.Drawing.Size(19, 16);
		this.lblIntercept.TabIndex = 60;
		this.lblIntercept.Text = "...";
		this.lblIntercept.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblTitleIntercept.AutoSize = true;
		this.lblTitleIntercept.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitleIntercept.Location = new System.Drawing.Point(80, 1);
		this.lblTitleIntercept.Name = "lblTitleIntercept";
		this.lblTitleIntercept.Size = new System.Drawing.Size(58, 16);
		this.lblTitleIntercept.TabIndex = 59;
		this.lblTitleIntercept.Text = "Intercept";
		this.lblTitleIntercept.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblSlope.AutoSize = true;
		this.lblSlope.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblSlope.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSlope.Location = new System.Drawing.Point(54, 1);
		this.lblSlope.Name = "lblSlope";
		this.lblSlope.Size = new System.Drawing.Size(19, 16);
		this.lblSlope.TabIndex = 58;
		this.lblSlope.Text = "...";
		this.lblSlope.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblTitleSlope.AutoSize = true;
		this.lblTitleSlope.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitleSlope.Location = new System.Drawing.Point(4, 1);
		this.lblTitleSlope.Name = "lblTitleSlope";
		this.lblTitleSlope.Size = new System.Drawing.Size(43, 16);
		this.lblTitleSlope.TabIndex = 57;
		this.lblTitleSlope.Text = "Slope";
		this.lblTitleSlope.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.mSearchMain.AutoSize = true;
		this.mSearchMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.mSearchMain.Dock = System.Windows.Forms.DockStyle.Top;
		this.mSearchMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.mSearchMain.Location = new System.Drawing.Point(20, 70);
		this.mSearchMain.Name = "mSearchMain";
		this.mSearchMain.Padding = new System.Windows.Forms.Padding(3);
		this.mSearchMain.Size = new System.Drawing.Size(1060, 65);
		this.mSearchMain.TabIndex = 177;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1100, 600);
		base.Controls.Add(this.chartMain);
		base.Controls.Add(this.panelLogout);
		base.Controls.Add(this.statusStripfrmMain);
		base.Controls.Add(this.tpanelInfor);
		base.Controls.Add(this.mSearchMain);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "frmScatterView";
		base.Padding = new System.Windows.Forms.Padding(20, 70, 20, 20);
		this.Text = "5S QA System * SCATTER";
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmScatterView_FormClosed);
		base.Load += new System.EventHandler(frmScatterView_Load);
		base.Shown += new System.EventHandler(frmScatterView_Shown);
		this.contextMenuStripMain.ResumeLayout(false);
		this.statusStripfrmMain.ResumeLayout(false);
		this.statusStripfrmMain.PerformLayout();
		this.panelLogout.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.ptbAvatar).EndInit();
		((System.ComponentModel.ISupportInitialize)this.chartMain).EndInit();
		this.tpanelInfor.ResumeLayout(false);
		this.tpanelInfor.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
