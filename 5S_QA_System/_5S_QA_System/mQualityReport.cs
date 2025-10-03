using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using _5S_QA_Entities.Models;
using _5S_QA_System.Controls;

namespace _5S_QA_System;

public class mQualityReport : UserControl
{
	private CommonCpk mCommonCpk;

	private List<OtherDataCpk> mDataCpks;

	private double mMaxValue;

	private HitTestResult mPreHit;

	private List<ChartViewModel> mModels;

	private IContainer components = null;

	private Label lblDefective;

	private Label lblTitleDefective;

	private Label label17;

	private Label label16;

	private Label label15;

	private Label lblTitleMinimun;

	private Label lblTitleOutLSL;

	private Label label12;

	private Label lblTitleStandard;

	private Label label10;

	private Label lblTitleMaximun;

	private Label label8;

	private Label lblTitleAverage;

	private Label label6;

	private Label lblTitleOutUSL;

	private Label lblTitleLower;

	private Label lblTitleUpper;

	private Label lblTool;

	private Label lblItemNo;

	private Label lblTitleTool;

	private Label lblTitleItemNo;

	private Label lblVarition;

	private Label lblTitleVarition;

	private Label lblVariance;

	private Label lblTitleVariance;

	private Label lblRange;

	private Label lblXLCL;

	private Label lblXUCL;

	private Label lblCpk;

	private Label lblCp;

	private Label lblSD;

	private Label lblMinimun;

	private Label lblAverage;

	private Label lblOutLSL;

	private Label lblOK;

	private Label lblOutUSL;

	private Label lblN;

	private Label lblLower;

	private Label lblUpper;

	private Label lblStandard;

	private Label lblTitleRange;

	private TableLayoutPanel tpanelHeader;

	private Label lblMaximun;

	private Panel panelChart;

	private Chart chartMain;

	private Panel panelInfor;

	private TableLayoutPanel tpanelDifference;

	private Label lblTrend;

	private Label lblCountOut;

	private Label lblTitleCountOut;

	private Label lblDifferenceRange;

	private Label lblTitleDifferenceRange;

	private Label lblDeviation;

	private Label lblTitleDeviation;

	private Label lblTitleDifference;

	private Label lblDifference;

	private Label lbl30pcs;

	private Label lbl5pcs;

	private Label lblTitle30pcs;

	private Label lblTitle5pcs;

	private Label lblDataDifference;

	public mQualityReport(CommonCpk commonCpk, List<OtherDataCpk> dataCpks, double max, List<ChartViewModel> models)
	{
		InitializeComponent();
		mCommonCpk = commonCpk;
		mDataCpks = dataCpks;
		mMaxValue = max;
		mModels = models;
		Common.setControls(this);
	}

	private void mQualityReport_Load(object sender, EventArgs e)
	{
		Init();
	}

	private void Init()
	{
		lblItemNo.Text = mCommonCpk.ItemNo;
		lblTool.Text = mCommonCpk.Tool;
		if (mCommonCpk.Unit == "°")
		{
			lblStandard.Text = Common.ConvertDoubleToDegrees(mCommonCpk.Standard);
			lblUpper.Text = Common.ConvertDoubleToDegrees(mCommonCpk.Upper);
			lblLower.Text = Common.ConvertDoubleToDegrees(mCommonCpk.Lower);
			lblAverage.Text = Common.ConvertDoubleToDegrees(mCommonCpk.Average);
			lblMaximun.Text = Common.ConvertDoubleToDegrees(mCommonCpk.Maximun);
			lblMinimun.Text = Common.ConvertDoubleToDegrees(mCommonCpk.Minimun);
			lblSD.Text = Common.ConvertDoubleToDegrees(mCommonCpk.SD);
			lblXUCL.Text = Common.ConvertDoubleToDegrees(mCommonCpk.XUCL);
			lblXLCL.Text = Common.ConvertDoubleToDegrees(mCommonCpk.XLCL);
			lblRange.Text = Common.ConvertDoubleToDegrees(mCommonCpk.Range);
		}
		else
		{
			lblStandard.Text = mCommonCpk.Standard.ToString();
			lblUpper.Text = mCommonCpk.Upper.ToString();
			lblLower.Text = mCommonCpk.Lower.ToString();
			lblAverage.Text = mCommonCpk.Average.ToString("0.0000");
			lblMaximun.Text = mCommonCpk.Maximun.ToString("0.0000");
			lblMinimun.Text = mCommonCpk.Minimun.ToString("0.0000");
			lblSD.Text = mCommonCpk.SD.ToString("0.0000");
			lblXUCL.Text = mCommonCpk.XUCL.ToString("0.0000");
			lblXLCL.Text = mCommonCpk.XLCL.ToString("0.0000");
			lblRange.Text = mCommonCpk.Range.ToString("0.0000");
		}
		lblN.Text = mCommonCpk.n.ToString();
		lblOutUSL.Text = mCommonCpk.OutUSL.ToString();
		lblOutLSL.Text = mCommonCpk.OutLSL.ToString();
		lblOK.Text = mCommonCpk.OK.ToString();
		lblCp.Text = mCommonCpk.Cp.ToString("0.0000");
		lblCpk.Text = mCommonCpk.Cpk.ToString("0.0000");
		lblVariance.Text = mCommonCpk.Variance.ToString("0.0000");
		lblVarition.Text = mCommonCpk.Varition.ToString("0.0000");
		lblDefective.Text = $"{mCommonCpk.Defective:0.000}%";
		List<double> source = new List<double> { mCommonCpk.USL, mCommonCpk.UCL, mCommonCpk.Sigma3, mCommonCpk.CL, mCommonCpk.MSigma3, mCommonCpk.LCL, mCommonCpk.LSL };
		double num = Math.Round((mCommonCpk.Upper - mCommonCpk.Lower) * 0.1, 4, MidpointRounding.AwayFromZero);
		double num2 = Math.Round(source.Min(), 4, MidpointRounding.AwayFromZero) - num;
		double num3 = Math.Round(source.Max(), 4, MidpointRounding.AwayFromZero) + num;
		chartMain.ChartAreas[0].AxisY.Minimum = num2;
		chartMain.ChartAreas[0].AxisY.Maximum = num3;
		chartMain.ChartAreas[0].AxisY.Interval = Math.Round((num3 - num2) / 10.0, 4, MidpointRounding.AwayFromZero);
		chartMain.ChartAreas[1].AxisX.Minimum = num2;
		chartMain.ChartAreas[1].AxisX.Maximum = num3;
		num3 = mMaxValue;
		chartMain.ChartAreas[0].AxisX.Minimum = 0.0;
		chartMain.ChartAreas[0].AxisX.Maximum = num3;
		double num4 = Math.Round(num3 / 10.0, 0, MidpointRounding.AwayFromZero);
		chartMain.ChartAreas[0].AxisX.Interval = ((num4 == 0.0) ? 1.0 : num4);
		chartMain.ChartAreas[1].AxisY.Minimum = 0.0;
		chartMain.ChartAreas[1].AxisY.Maximum = num3;
		int num5 = 0;
		foreach (OtherDataCpk mDataCpk in mDataCpks)
		{
			double num6 = Math.Round(mDataCpk.Avegare, 4, MidpointRounding.AwayFromZero);
			chartMain.Series["Frequency"].Points.AddXY(num6, mDataCpk.Frequency);
			double num7 = mDataCpks.Max((OtherDataCpk c) => c.NormalDist);
			double xValue = 0.0;
			if (!num7.Equals(0.0))
			{
				xValue = mDataCpk.NormalDist * (double)mDataCpks.Max((OtherDataCpk c) => c.Frequency) / mDataCpks.Max((OtherDataCpk c) => c.NormalDist);
			}
			chartMain.Series["FrequencyL"].Points.AddXY(xValue, num6);
			chartMain.Series["USL"].Points.AddXY(num5, mCommonCpk.USL);
			chartMain.Series["LSL"].Points.AddXY(num5, mCommonCpk.LSL);
			chartMain.Series["UCL"].Points.AddXY(num5, mCommonCpk.UCL);
			chartMain.Series["LCL"].Points.AddXY(num5, mCommonCpk.LCL);
			chartMain.Series["Avegare"].Points.AddXY(num5, mCommonCpk.Average);
			num5++;
		}
		if (mCommonCpk.Unit == "°")
		{
			chartMain.ChartAreas[0].AxisY.CustomLabels.Clear();
			for (double num8 = chartMain.ChartAreas[0].AxisY.Minimum; num8 <= chartMain.ChartAreas[0].AxisY.Maximum; num8 += chartMain.ChartAreas[0].AxisY.Interval)
			{
				chartMain.ChartAreas[0].AxisY.CustomLabels.Add(num8 - chartMain.ChartAreas[0].AxisY.Interval / 2.0, num8 + chartMain.ChartAreas[0].AxisY.Interval / 2.0, Common.ConvertDoubleToDegrees(num8, uniformity: true));
			}
		}
		DrawInformation();
		SetDataDifference(mModels);
	}

	private void DrawInformation()
	{
		int num = (int)((float)chartMain.Height * chartMain.ChartAreas[0].InnerPlotPosition.Y / 100f);
		int num2 = (int)((float)chartMain.Height * (chartMain.ChartAreas[0].InnerPlotPosition.Y + chartMain.ChartAreas[0].InnerPlotPosition.Height) / 100f);
		List<double> list = new List<double> { mCommonCpk.USL, mCommonCpk.UCL, mCommonCpk.Average, mCommonCpk.LCL, mCommonCpk.LSL }.OrderByDescending((double x) => x).ToList();
		TransparentLabel transparentLabel = new TransparentLabel
		{
			Text = "USL " + ((mCommonCpk.Unit == "°") ? Common.ConvertDoubleToDegrees(mCommonCpk.USL) : mCommonCpk.USL.ToString("0.0000")),
			Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0),
			Margin = new Padding(0),
			AutoSize = true,
			ForeColor = Color.Red
		};
		panelInfor.Controls.Add(transparentLabel);
		transparentLabel.Location = new Point(0, GetLocationY(num + transparentLabel.Height / 2, num2 - transparentLabel.Height / 2, 5, list.IndexOf(mCommonCpk.USL)) - transparentLabel.Height / 2);
		TransparentLabel transparentLabel2 = new TransparentLabel
		{
			Text = "LSL " + ((mCommonCpk.Unit == "°") ? Common.ConvertDoubleToDegrees(mCommonCpk.LSL) : mCommonCpk.LSL.ToString("0.0000")),
			Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0),
			Margin = new Padding(0),
			AutoSize = true,
			ForeColor = Color.Red
		};
		panelInfor.Controls.Add(transparentLabel2);
		transparentLabel2.Location = new Point(0, GetLocationY(num + transparentLabel2.Height / 2, num2 - transparentLabel2.Height / 2, 5, list.IndexOf(mCommonCpk.LSL)) - transparentLabel2.Height / 2);
		TransparentLabel transparentLabel3 = new TransparentLabel
		{
			Text = "Ave " + ((mCommonCpk.Unit == "°") ? Common.ConvertDoubleToDegrees(mCommonCpk.Average) : mCommonCpk.Average.ToString("0.0000")),
			Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0),
			Margin = new Padding(0),
			AutoSize = true
		};
		panelInfor.Controls.Add(transparentLabel3);
		transparentLabel3.Location = new Point(0, GetLocationY(num + transparentLabel3.Height / 2, num2 - transparentLabel3.Height / 2, 5, list.IndexOf(mCommonCpk.Average)) - transparentLabel3.Height / 2);
		TransparentLabel transparentLabel4 = new TransparentLabel
		{
			Text = "UCL " + ((mCommonCpk.Unit == "°") ? Common.ConvertDoubleToDegrees(mCommonCpk.UCL) : mCommonCpk.UCL.ToString("0.0000")),
			Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0),
			Margin = new Padding(0),
			AutoSize = true,
			ForeColor = Color.Blue
		};
		panelInfor.Controls.Add(transparentLabel4);
		transparentLabel4.Location = new Point(0, GetLocationY(num + transparentLabel4.Height / 2, num2 - transparentLabel4.Height / 2, 5, list.IndexOf(mCommonCpk.UCL)) - transparentLabel4.Height / 2);
		TransparentLabel transparentLabel5 = new TransparentLabel
		{
			Text = "LCL " + ((mCommonCpk.Unit == "°") ? Common.ConvertDoubleToDegrees(mCommonCpk.LCL) : mCommonCpk.LCL.ToString("0.0000")),
			Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0),
			Margin = new Padding(0),
			AutoSize = true,
			ForeColor = Color.Blue
		};
		panelInfor.Controls.Add(transparentLabel5);
		transparentLabel5.Location = new Point(0, GetLocationY(num + transparentLabel5.Height / 2, num2 - transparentLabel5.Height / 2, 5, list.IndexOf(mCommonCpk.LCL)) - transparentLabel5.Height / 2);
	}

	private int GetLocationY(int xmin, int xmax, int number, int pos)
	{
		return pos * (xmax - xmin) / (number - 1) + xmin;
	}

	private void SetDataDifference(List<ChartViewModel> models)
	{
		models.Reverse();
		double? num = models.First().Upper - models.First().Lower;
		lblDifferenceRange.Text = num.ToString();
		if (models.Count < 6)
		{
			lbl5pcs.Text = "N/A";
			lbl30pcs.Text = "N/A";
			lblDeviation.Text = "N/A";
			lblDifference.Text = "N/A";
			lblDifference.ForeColor = Color.Black;
		}
		else
		{
			List<ChartViewModel> range = models.GetRange(0, 5);
			List<ChartViewModel> range2 = models.GetRange(5, models.Count - 5);
			if (models.Count > 35)
			{
				models = models.GetRange(5, 30);
			}
			double num2 = range.Average((ChartViewModel x) => double.Parse(x.Result));
			double num3 = range2.Average((ChartViewModel x) => double.Parse(x.Result));
			double num4 = num2 - num3;
			lbl5pcs.Text = ((mCommonCpk.Unit == "°") ? Common.ConvertDoubleToDegrees(num2) : num2.ToString("0.####"));
			lbl30pcs.Text = ((mCommonCpk.Unit == "°") ? Common.ConvertDoubleToDegrees(num3) : num3.ToString("0.####"));
			lblDeviation.Text = ((mCommonCpk.Unit == "°") ? Common.ConvertDoubleToDegrees(num4) : num4.ToString("0.####"));
			double? num5 = num4 * 100.0 / num;
			if (num5 > 20.0 || num5 < -20.0)
			{
				lblDifference.ForeColor = Color.Red;
			}
			lblDifference.Text = num5?.ToString("0.##");
		}
		if (models.Count() < 2)
		{
			lblTrend.Text = "";
			lblCountOut.Text = "N/A";
			return;
		}
		double.TryParse(models[0].Result, out var result);
		double.TryParse(models[1].Result, out var result2);
		lblCountOut.ForeColor = Color.Black;
		if (result > models.First().USL || result < models.First().LSL)
		{
			lblTrend.Text = "";
			lblCountOut.Text = "NG";
			lblCountOut.ForeColor = Color.Red;
		}
		else if (result < result2)
		{
			lblTrend.Text = Common.getTextLanguage(this, "wTrendDown");
			double? obj = (result - models.First().LSL) / (result2 - result);
			lblCountOut.Text = obj?.ToString("0");
		}
		else if (result > result2)
		{
			lblTrend.Text = Common.getTextLanguage(this, "wTrendUp");
			double? obj2 = (models.First().USL - result) / (result - result2);
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
			if (mPreHit.Series.Name.Equals("Frequency"))
			{
				dataPoint.Color = Color.Transparent;
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
			if (mPreHit.Series.Name.Equals("Frequency"))
			{
				dataPoint2.Color = Color.Blue;
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
		System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
		System.Windows.Forms.DataVisualization.Charting.Legend legend = new System.Windows.Forms.DataVisualization.Charting.Legend();
		System.Windows.Forms.DataVisualization.Charting.Series series = new System.Windows.Forms.DataVisualization.Charting.Series();
		System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
		System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
		System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
		System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
		System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
		System.Windows.Forms.DataVisualization.Charting.Series series7 = new System.Windows.Forms.DataVisualization.Charting.Series();
		this.lblDefective = new System.Windows.Forms.Label();
		this.lblTitleDefective = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.lblTitleMinimun = new System.Windows.Forms.Label();
		this.lblTitleOutLSL = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.lblTitleStandard = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.lblTitleMaximun = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.lblTitleAverage = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.lblTitleOutUSL = new System.Windows.Forms.Label();
		this.lblTitleLower = new System.Windows.Forms.Label();
		this.lblTitleUpper = new System.Windows.Forms.Label();
		this.lblTool = new System.Windows.Forms.Label();
		this.lblItemNo = new System.Windows.Forms.Label();
		this.lblTitleTool = new System.Windows.Forms.Label();
		this.lblTitleItemNo = new System.Windows.Forms.Label();
		this.lblVarition = new System.Windows.Forms.Label();
		this.lblTitleVarition = new System.Windows.Forms.Label();
		this.lblVariance = new System.Windows.Forms.Label();
		this.lblTitleVariance = new System.Windows.Forms.Label();
		this.lblRange = new System.Windows.Forms.Label();
		this.lblXLCL = new System.Windows.Forms.Label();
		this.lblXUCL = new System.Windows.Forms.Label();
		this.lblCpk = new System.Windows.Forms.Label();
		this.lblCp = new System.Windows.Forms.Label();
		this.lblSD = new System.Windows.Forms.Label();
		this.lblMinimun = new System.Windows.Forms.Label();
		this.lblAverage = new System.Windows.Forms.Label();
		this.lblOutLSL = new System.Windows.Forms.Label();
		this.lblOK = new System.Windows.Forms.Label();
		this.lblOutUSL = new System.Windows.Forms.Label();
		this.lblN = new System.Windows.Forms.Label();
		this.lblLower = new System.Windows.Forms.Label();
		this.lblUpper = new System.Windows.Forms.Label();
		this.lblStandard = new System.Windows.Forms.Label();
		this.lblTitleRange = new System.Windows.Forms.Label();
		this.tpanelHeader = new System.Windows.Forms.TableLayoutPanel();
		this.lblMaximun = new System.Windows.Forms.Label();
		this.panelChart = new System.Windows.Forms.Panel();
		this.chartMain = new System.Windows.Forms.DataVisualization.Charting.Chart();
		this.panelInfor = new System.Windows.Forms.Panel();
		this.tpanelDifference = new System.Windows.Forms.TableLayoutPanel();
		this.lblTrend = new System.Windows.Forms.Label();
		this.lblCountOut = new System.Windows.Forms.Label();
		this.lblTitleCountOut = new System.Windows.Forms.Label();
		this.lblDifferenceRange = new System.Windows.Forms.Label();
		this.lblTitleDifferenceRange = new System.Windows.Forms.Label();
		this.lblDeviation = new System.Windows.Forms.Label();
		this.lblTitleDeviation = new System.Windows.Forms.Label();
		this.lblTitleDifference = new System.Windows.Forms.Label();
		this.lblDifference = new System.Windows.Forms.Label();
		this.lbl30pcs = new System.Windows.Forms.Label();
		this.lbl5pcs = new System.Windows.Forms.Label();
		this.lblTitle30pcs = new System.Windows.Forms.Label();
		this.lblTitle5pcs = new System.Windows.Forms.Label();
		this.lblDataDifference = new System.Windows.Forms.Label();
		this.tpanelHeader.SuspendLayout();
		this.panelChart.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.chartMain).BeginInit();
		this.tpanelDifference.SuspendLayout();
		base.SuspendLayout();
		this.lblDefective.AutoSize = true;
		this.lblDefective.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblDefective.Location = new System.Drawing.Point(929, 50);
		this.lblDefective.Margin = new System.Windows.Forms.Padding(3);
		this.lblDefective.Name = "lblDefective";
		this.lblDefective.Size = new System.Drawing.Size(67, 16);
		this.lblDefective.TabIndex = 41;
		this.lblDefective.Text = "...";
		this.lblDefective.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblTitleDefective.AutoSize = true;
		this.lblTitleDefective.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitleDefective.Location = new System.Drawing.Point(858, 50);
		this.lblTitleDefective.Margin = new System.Windows.Forms.Padding(3);
		this.lblTitleDefective.Name = "lblTitleDefective";
		this.lblTitleDefective.Size = new System.Drawing.Size(64, 16);
		this.lblTitleDefective.TabIndex = 40;
		this.lblTitleDefective.Text = "Defective";
		this.lblTitleDefective.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label17.AutoSize = true;
		this.label17.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label17.Location = new System.Drawing.Point(729, 27);
		this.label17.Margin = new System.Windows.Forms.Padding(3);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(51, 16);
		this.label17.TabIndex = 18;
		this.label17.Text = "X\u00af LCL";
		this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label16.AutoSize = true;
		this.label16.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label16.Location = new System.Drawing.Point(729, 4);
		this.label16.Margin = new System.Windows.Forms.Padding(3);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(51, 16);
		this.label16.TabIndex = 17;
		this.label16.Text = "X\u00af UCL";
		this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label15.AutoSize = true;
		this.label15.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label15.Location = new System.Drawing.Point(620, 50);
		this.label15.Margin = new System.Windows.Forms.Padding(3);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(31, 16);
		this.label15.TabIndex = 16;
		this.label15.Text = "Cpk";
		this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblTitleMinimun.AutoSize = true;
		this.lblTitleMinimun.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitleMinimun.Location = new System.Drawing.Point(482, 50);
		this.lblTitleMinimun.Margin = new System.Windows.Forms.Padding(3);
		this.lblTitleMinimun.Name = "lblTitleMinimun";
		this.lblTitleMinimun.Size = new System.Drawing.Size(60, 16);
		this.lblTitleMinimun.TabIndex = 15;
		this.lblTitleMinimun.Text = "Minimun";
		this.lblTitleMinimun.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblTitleOutLSL.AutoSize = true;
		this.lblTitleOutLSL.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitleOutLSL.Location = new System.Drawing.Point(340, 50);
		this.lblTitleOutLSL.Margin = new System.Windows.Forms.Padding(3);
		this.lblTitleOutLSL.Name = "lblTitleOutLSL";
		this.lblTitleOutLSL.Size = new System.Drawing.Size(64, 16);
		this.lblTitleOutLSL.TabIndex = 14;
		this.lblTitleOutLSL.Text = "Out (LSL)";
		this.lblTitleOutLSL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label12.AutoSize = true;
		this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label12.Location = new System.Drawing.Point(191, 50);
		this.label12.Margin = new System.Windows.Forms.Padding(3);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(71, 16);
		this.label12.TabIndex = 13;
		this.label12.Text = "(n)";
		this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblTitleStandard.AutoSize = true;
		this.lblTitleStandard.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitleStandard.Location = new System.Drawing.Point(4, 50);
		this.lblTitleStandard.Margin = new System.Windows.Forms.Padding(3);
		this.lblTitleStandard.Name = "lblTitleStandard";
		this.lblTitleStandard.Size = new System.Drawing.Size(62, 16);
		this.lblTitleStandard.TabIndex = 12;
		this.lblTitleStandard.Text = "Standard";
		this.lblTitleStandard.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label10.AutoSize = true;
		this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label10.Location = new System.Drawing.Point(620, 27);
		this.label10.Margin = new System.Windows.Forms.Padding(3);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(31, 16);
		this.label10.TabIndex = 11;
		this.label10.Text = "Cp";
		this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblTitleMaximun.AutoSize = true;
		this.lblTitleMaximun.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitleMaximun.Location = new System.Drawing.Point(482, 27);
		this.lblTitleMaximun.Margin = new System.Windows.Forms.Padding(3);
		this.lblTitleMaximun.Name = "lblTitleMaximun";
		this.lblTitleMaximun.Size = new System.Drawing.Size(60, 16);
		this.lblTitleMaximun.TabIndex = 10;
		this.lblTitleMaximun.Text = "Maximun";
		this.lblTitleMaximun.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label8.AutoSize = true;
		this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label8.Location = new System.Drawing.Point(620, 4);
		this.label8.Margin = new System.Windows.Forms.Padding(3);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(31, 16);
		this.label8.TabIndex = 9;
		this.label8.Text = "SD";
		this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblTitleAverage.AutoSize = true;
		this.lblTitleAverage.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitleAverage.Location = new System.Drawing.Point(482, 4);
		this.lblTitleAverage.Margin = new System.Windows.Forms.Padding(3);
		this.lblTitleAverage.Name = "lblTitleAverage";
		this.lblTitleAverage.Size = new System.Drawing.Size(60, 16);
		this.lblTitleAverage.TabIndex = 8;
		this.lblTitleAverage.Text = "Average";
		this.lblTitleAverage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label6.AutoSize = true;
		this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label6.Location = new System.Drawing.Point(340, 27);
		this.label6.Margin = new System.Windows.Forms.Padding(3);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(64, 16);
		this.label6.TabIndex = 7;
		this.label6.Text = "OK";
		this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblTitleOutUSL.AutoSize = true;
		this.lblTitleOutUSL.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitleOutUSL.Location = new System.Drawing.Point(340, 4);
		this.lblTitleOutUSL.Margin = new System.Windows.Forms.Padding(3);
		this.lblTitleOutUSL.Name = "lblTitleOutUSL";
		this.lblTitleOutUSL.Size = new System.Drawing.Size(64, 16);
		this.lblTitleOutUSL.TabIndex = 6;
		this.lblTitleOutUSL.Text = "Out (USL)";
		this.lblTitleOutUSL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblTitleLower.AutoSize = true;
		this.lblTitleLower.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitleLower.Location = new System.Drawing.Point(191, 27);
		this.lblTitleLower.Margin = new System.Windows.Forms.Padding(3);
		this.lblTitleLower.Name = "lblTitleLower";
		this.lblTitleLower.Size = new System.Drawing.Size(71, 16);
		this.lblTitleLower.TabIndex = 5;
		this.lblTitleLower.Text = "Lower limit";
		this.lblTitleLower.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblTitleUpper.AutoSize = true;
		this.lblTitleUpper.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitleUpper.Location = new System.Drawing.Point(191, 4);
		this.lblTitleUpper.Margin = new System.Windows.Forms.Padding(3);
		this.lblTitleUpper.Name = "lblTitleUpper";
		this.lblTitleUpper.Size = new System.Drawing.Size(71, 16);
		this.lblTitleUpper.TabIndex = 4;
		this.lblTitleUpper.Text = "Upper limit";
		this.lblTitleUpper.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblTool.AutoSize = true;
		this.lblTool.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTool.Location = new System.Drawing.Point(73, 27);
		this.lblTool.Margin = new System.Windows.Forms.Padding(3);
		this.lblTool.Name = "lblTool";
		this.lblTool.Size = new System.Drawing.Size(111, 16);
		this.lblTool.TabIndex = 3;
		this.lblTool.Text = "...";
		this.lblTool.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblItemNo.AutoSize = true;
		this.lblItemNo.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblItemNo.Location = new System.Drawing.Point(73, 4);
		this.lblItemNo.Margin = new System.Windows.Forms.Padding(3);
		this.lblItemNo.Name = "lblItemNo";
		this.lblItemNo.Size = new System.Drawing.Size(111, 16);
		this.lblItemNo.TabIndex = 2;
		this.lblItemNo.Text = "...";
		this.lblItemNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblTitleTool.AutoSize = true;
		this.lblTitleTool.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitleTool.Location = new System.Drawing.Point(4, 27);
		this.lblTitleTool.Margin = new System.Windows.Forms.Padding(3);
		this.lblTitleTool.Name = "lblTitleTool";
		this.lblTitleTool.Size = new System.Drawing.Size(62, 16);
		this.lblTitleTool.TabIndex = 1;
		this.lblTitleTool.Text = "Tool";
		this.lblTitleTool.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblTitleItemNo.AutoSize = true;
		this.lblTitleItemNo.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitleItemNo.Location = new System.Drawing.Point(4, 4);
		this.lblTitleItemNo.Margin = new System.Windows.Forms.Padding(3);
		this.lblTitleItemNo.Name = "lblTitleItemNo";
		this.lblTitleItemNo.Size = new System.Drawing.Size(62, 16);
		this.lblTitleItemNo.TabIndex = 0;
		this.lblTitleItemNo.Text = "Item no.";
		this.lblTitleItemNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblVarition.AutoSize = true;
		this.lblVarition.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblVarition.Location = new System.Drawing.Point(929, 27);
		this.lblVarition.Margin = new System.Windows.Forms.Padding(3);
		this.lblVarition.Name = "lblVarition";
		this.lblVarition.Size = new System.Drawing.Size(67, 16);
		this.lblVarition.TabIndex = 39;
		this.lblVarition.Text = "...";
		this.lblVarition.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblTitleVarition.AutoSize = true;
		this.lblTitleVarition.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitleVarition.Location = new System.Drawing.Point(858, 27);
		this.lblTitleVarition.Margin = new System.Windows.Forms.Padding(3);
		this.lblTitleVarition.Name = "lblTitleVarition";
		this.lblTitleVarition.Size = new System.Drawing.Size(64, 16);
		this.lblTitleVarition.TabIndex = 38;
		this.lblTitleVarition.Text = "Varition";
		this.lblTitleVarition.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblVariance.AutoSize = true;
		this.lblVariance.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblVariance.Location = new System.Drawing.Point(929, 4);
		this.lblVariance.Margin = new System.Windows.Forms.Padding(3);
		this.lblVariance.Name = "lblVariance";
		this.lblVariance.Size = new System.Drawing.Size(67, 16);
		this.lblVariance.TabIndex = 37;
		this.lblVariance.Text = "...";
		this.lblVariance.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblTitleVariance.AutoSize = true;
		this.lblTitleVariance.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitleVariance.Location = new System.Drawing.Point(858, 4);
		this.lblTitleVariance.Margin = new System.Windows.Forms.Padding(3);
		this.lblTitleVariance.Name = "lblTitleVariance";
		this.lblTitleVariance.Size = new System.Drawing.Size(64, 16);
		this.lblTitleVariance.TabIndex = 36;
		this.lblTitleVariance.Text = "Variance";
		this.lblTitleVariance.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblRange.AutoSize = true;
		this.lblRange.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblRange.Location = new System.Drawing.Point(787, 50);
		this.lblRange.Margin = new System.Windows.Forms.Padding(3);
		this.lblRange.Name = "lblRange";
		this.lblRange.Size = new System.Drawing.Size(64, 16);
		this.lblRange.TabIndex = 35;
		this.lblRange.Text = "...";
		this.lblRange.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblXLCL.AutoSize = true;
		this.lblXLCL.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblXLCL.Location = new System.Drawing.Point(787, 27);
		this.lblXLCL.Margin = new System.Windows.Forms.Padding(3);
		this.lblXLCL.Name = "lblXLCL";
		this.lblXLCL.Size = new System.Drawing.Size(64, 16);
		this.lblXLCL.TabIndex = 34;
		this.lblXLCL.Text = "...";
		this.lblXLCL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblXUCL.AutoSize = true;
		this.lblXUCL.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblXUCL.Location = new System.Drawing.Point(787, 4);
		this.lblXUCL.Margin = new System.Windows.Forms.Padding(3);
		this.lblXUCL.Name = "lblXUCL";
		this.lblXUCL.Size = new System.Drawing.Size(64, 16);
		this.lblXUCL.TabIndex = 33;
		this.lblXUCL.Text = "...";
		this.lblXUCL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblCpk.AutoSize = true;
		this.lblCpk.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCpk.Location = new System.Drawing.Point(658, 50);
		this.lblCpk.Margin = new System.Windows.Forms.Padding(3);
		this.lblCpk.Name = "lblCpk";
		this.lblCpk.Size = new System.Drawing.Size(64, 16);
		this.lblCpk.TabIndex = 32;
		this.lblCpk.Text = "...";
		this.lblCpk.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblCp.AutoSize = true;
		this.lblCp.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCp.Location = new System.Drawing.Point(658, 27);
		this.lblCp.Margin = new System.Windows.Forms.Padding(3);
		this.lblCp.Name = "lblCp";
		this.lblCp.Size = new System.Drawing.Size(64, 16);
		this.lblCp.TabIndex = 31;
		this.lblCp.Text = "...";
		this.lblCp.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblSD.AutoSize = true;
		this.lblSD.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblSD.Location = new System.Drawing.Point(658, 4);
		this.lblSD.Margin = new System.Windows.Forms.Padding(3);
		this.lblSD.Name = "lblSD";
		this.lblSD.Size = new System.Drawing.Size(64, 16);
		this.lblSD.TabIndex = 30;
		this.lblSD.Text = "...";
		this.lblSD.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblMinimun.AutoSize = true;
		this.lblMinimun.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblMinimun.Location = new System.Drawing.Point(549, 50);
		this.lblMinimun.Margin = new System.Windows.Forms.Padding(3);
		this.lblMinimun.Name = "lblMinimun";
		this.lblMinimun.Size = new System.Drawing.Size(64, 16);
		this.lblMinimun.TabIndex = 29;
		this.lblMinimun.Text = "...";
		this.lblMinimun.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblAverage.AutoSize = true;
		this.lblAverage.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblAverage.Location = new System.Drawing.Point(549, 4);
		this.lblAverage.Margin = new System.Windows.Forms.Padding(3);
		this.lblAverage.Name = "lblAverage";
		this.lblAverage.Size = new System.Drawing.Size(64, 16);
		this.lblAverage.TabIndex = 27;
		this.lblAverage.Text = "...";
		this.lblAverage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblOutLSL.AutoSize = true;
		this.lblOutLSL.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblOutLSL.ForeColor = System.Drawing.Color.Red;
		this.lblOutLSL.Location = new System.Drawing.Point(411, 50);
		this.lblOutLSL.Margin = new System.Windows.Forms.Padding(3);
		this.lblOutLSL.Name = "lblOutLSL";
		this.lblOutLSL.Size = new System.Drawing.Size(64, 16);
		this.lblOutLSL.TabIndex = 26;
		this.lblOutLSL.Text = "...";
		this.lblOutLSL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblOK.AutoSize = true;
		this.lblOK.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblOK.ForeColor = System.Drawing.Color.Blue;
		this.lblOK.Location = new System.Drawing.Point(411, 27);
		this.lblOK.Margin = new System.Windows.Forms.Padding(3);
		this.lblOK.Name = "lblOK";
		this.lblOK.Size = new System.Drawing.Size(64, 16);
		this.lblOK.TabIndex = 25;
		this.lblOK.Text = "...";
		this.lblOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblOutUSL.AutoSize = true;
		this.lblOutUSL.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblOutUSL.ForeColor = System.Drawing.Color.Red;
		this.lblOutUSL.Location = new System.Drawing.Point(411, 4);
		this.lblOutUSL.Margin = new System.Windows.Forms.Padding(3);
		this.lblOutUSL.Name = "lblOutUSL";
		this.lblOutUSL.Size = new System.Drawing.Size(64, 16);
		this.lblOutUSL.TabIndex = 24;
		this.lblOutUSL.Text = "...";
		this.lblOutUSL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblN.AutoSize = true;
		this.lblN.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblN.Location = new System.Drawing.Point(269, 50);
		this.lblN.Margin = new System.Windows.Forms.Padding(3);
		this.lblN.Name = "lblN";
		this.lblN.Size = new System.Drawing.Size(64, 16);
		this.lblN.TabIndex = 23;
		this.lblN.Text = "...";
		this.lblN.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblLower.AutoSize = true;
		this.lblLower.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblLower.Location = new System.Drawing.Point(269, 27);
		this.lblLower.Margin = new System.Windows.Forms.Padding(3);
		this.lblLower.Name = "lblLower";
		this.lblLower.Size = new System.Drawing.Size(64, 16);
		this.lblLower.TabIndex = 22;
		this.lblLower.Text = "...";
		this.lblLower.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblUpper.AutoSize = true;
		this.lblUpper.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblUpper.Location = new System.Drawing.Point(269, 4);
		this.lblUpper.Margin = new System.Windows.Forms.Padding(3);
		this.lblUpper.Name = "lblUpper";
		this.lblUpper.Size = new System.Drawing.Size(64, 16);
		this.lblUpper.TabIndex = 21;
		this.lblUpper.Text = "...";
		this.lblUpper.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblStandard.AutoSize = true;
		this.lblStandard.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblStandard.Location = new System.Drawing.Point(73, 50);
		this.lblStandard.Margin = new System.Windows.Forms.Padding(3);
		this.lblStandard.Name = "lblStandard";
		this.lblStandard.Size = new System.Drawing.Size(111, 16);
		this.lblStandard.TabIndex = 20;
		this.lblStandard.Text = "...";
		this.lblStandard.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblTitleRange.AutoSize = true;
		this.lblTitleRange.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitleRange.Location = new System.Drawing.Point(729, 50);
		this.lblTitleRange.Margin = new System.Windows.Forms.Padding(3);
		this.lblTitleRange.Name = "lblTitleRange";
		this.lblTitleRange.Size = new System.Drawing.Size(51, 16);
		this.lblTitleRange.TabIndex = 19;
		this.lblTitleRange.Text = "Range";
		this.lblTitleRange.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.tpanelHeader.AutoSize = true;
		this.tpanelHeader.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		this.tpanelHeader.ColumnCount = 14;
		this.tpanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21.73913f));
		this.tpanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.04348f));
		this.tpanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.04348f));
		this.tpanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.04348f));
		this.tpanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.04348f));
		this.tpanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.04348f));
		this.tpanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.04348f));
		this.tpanelHeader.Controls.Add(this.lblDefective, 13, 2);
		this.tpanelHeader.Controls.Add(this.lblTitleDefective, 12, 2);
		this.tpanelHeader.Controls.Add(this.lblVarition, 13, 1);
		this.tpanelHeader.Controls.Add(this.lblTitleVarition, 12, 1);
		this.tpanelHeader.Controls.Add(this.lblVariance, 13, 0);
		this.tpanelHeader.Controls.Add(this.lblTitleVariance, 12, 0);
		this.tpanelHeader.Controls.Add(this.lblRange, 11, 2);
		this.tpanelHeader.Controls.Add(this.lblXLCL, 11, 1);
		this.tpanelHeader.Controls.Add(this.lblXUCL, 11, 0);
		this.tpanelHeader.Controls.Add(this.lblCpk, 9, 2);
		this.tpanelHeader.Controls.Add(this.lblCp, 9, 1);
		this.tpanelHeader.Controls.Add(this.lblSD, 9, 0);
		this.tpanelHeader.Controls.Add(this.lblMinimun, 7, 2);
		this.tpanelHeader.Controls.Add(this.lblMaximun, 7, 1);
		this.tpanelHeader.Controls.Add(this.lblAverage, 7, 0);
		this.tpanelHeader.Controls.Add(this.lblOutLSL, 5, 2);
		this.tpanelHeader.Controls.Add(this.lblOK, 5, 1);
		this.tpanelHeader.Controls.Add(this.lblOutUSL, 5, 0);
		this.tpanelHeader.Controls.Add(this.lblN, 3, 2);
		this.tpanelHeader.Controls.Add(this.lblLower, 3, 1);
		this.tpanelHeader.Controls.Add(this.lblUpper, 3, 0);
		this.tpanelHeader.Controls.Add(this.lblStandard, 1, 2);
		this.tpanelHeader.Controls.Add(this.lblTitleRange, 10, 2);
		this.tpanelHeader.Controls.Add(this.label17, 10, 1);
		this.tpanelHeader.Controls.Add(this.label16, 10, 0);
		this.tpanelHeader.Controls.Add(this.label15, 8, 2);
		this.tpanelHeader.Controls.Add(this.lblTitleMinimun, 6, 2);
		this.tpanelHeader.Controls.Add(this.lblTitleOutLSL, 4, 2);
		this.tpanelHeader.Controls.Add(this.label12, 2, 2);
		this.tpanelHeader.Controls.Add(this.lblTitleStandard, 0, 2);
		this.tpanelHeader.Controls.Add(this.label10, 8, 1);
		this.tpanelHeader.Controls.Add(this.lblTitleMaximun, 6, 1);
		this.tpanelHeader.Controls.Add(this.label8, 8, 0);
		this.tpanelHeader.Controls.Add(this.lblTitleAverage, 6, 0);
		this.tpanelHeader.Controls.Add(this.label6, 4, 1);
		this.tpanelHeader.Controls.Add(this.lblTitleOutUSL, 4, 0);
		this.tpanelHeader.Controls.Add(this.lblTitleLower, 2, 1);
		this.tpanelHeader.Controls.Add(this.lblTitleUpper, 2, 0);
		this.tpanelHeader.Controls.Add(this.lblTool, 1, 1);
		this.tpanelHeader.Controls.Add(this.lblItemNo, 1, 0);
		this.tpanelHeader.Controls.Add(this.lblTitleTool, 0, 1);
		this.tpanelHeader.Controls.Add(this.lblTitleItemNo, 0, 0);
		this.tpanelHeader.Dock = System.Windows.Forms.DockStyle.Top;
		this.tpanelHeader.Location = new System.Drawing.Point(0, 0);
		this.tpanelHeader.Margin = new System.Windows.Forms.Padding(4);
		this.tpanelHeader.Name = "tpanelHeader";
		this.tpanelHeader.RowCount = 3;
		this.tpanelHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22f));
		this.tpanelHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22f));
		this.tpanelHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22f));
		this.tpanelHeader.Size = new System.Drawing.Size(1000, 70);
		this.tpanelHeader.TabIndex = 7;
		this.lblMaximun.AutoSize = true;
		this.lblMaximun.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblMaximun.Location = new System.Drawing.Point(549, 27);
		this.lblMaximun.Margin = new System.Windows.Forms.Padding(3);
		this.lblMaximun.Name = "lblMaximun";
		this.lblMaximun.Size = new System.Drawing.Size(64, 16);
		this.lblMaximun.TabIndex = 28;
		this.lblMaximun.Text = "...";
		this.lblMaximun.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.panelChart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panelChart.Controls.Add(this.chartMain);
		this.panelChart.Controls.Add(this.panelInfor);
		this.panelChart.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panelChart.Location = new System.Drawing.Point(0, 117);
		this.panelChart.Name = "panelChart";
		this.panelChart.Size = new System.Drawing.Size(1000, 213);
		this.panelChart.TabIndex = 10;
		chartArea.AlignWithChartArea = "ChartArea2";
		chartArea.AxisX.IsLabelAutoFit = false;
		chartArea.AxisX.MajorGrid.Enabled = false;
		chartArea.AxisX.Minimum = 0.0;
		chartArea.AxisX2.Interval = 1.0;
		chartArea.AxisX2.LabelStyle.Enabled = false;
		chartArea.AxisX2.MajorGrid.Enabled = false;
		chartArea.AxisX2.MajorTickMark.Enabled = false;
		chartArea.AxisX2.Maximum = 14.0;
		chartArea.AxisX2.Minimum = 0.0;
		chartArea.AxisY.IsLabelAutoFit = false;
		chartArea.AxisY.LabelStyle.Format = "0.0000";
		chartArea.AxisY.MajorGrid.Enabled = false;
		chartArea.AxisY2.LabelStyle.Enabled = false;
		chartArea.AxisY2.MajorGrid.Enabled = false;
		chartArea.AxisY2.MajorTickMark.Enabled = false;
		chartArea.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
		chartArea.InnerPlotPosition.Auto = false;
		chartArea.InnerPlotPosition.Height = 84f;
		chartArea.InnerPlotPosition.Width = 90f;
		chartArea.InnerPlotPosition.X = 9f;
		chartArea.InnerPlotPosition.Y = 4f;
		chartArea.Name = "ChartArea1";
		chartArea.Position.Auto = false;
		chartArea.Position.Height = 100f;
		chartArea.Position.Width = 100f;
		chartArea2.AlignmentOrientation = System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations.All;
		chartArea2.AlignWithChartArea = "ChartArea1";
		chartArea2.AxisX.IsLabelAutoFit = false;
		chartArea2.AxisX.LabelStyle.Enabled = false;
		chartArea2.AxisX.MajorGrid.Enabled = false;
		chartArea2.AxisX.MajorTickMark.Enabled = false;
		chartArea2.AxisX2.LabelStyle.Enabled = false;
		chartArea2.AxisY.IsLabelAutoFit = false;
		chartArea2.AxisY.LabelStyle.Enabled = false;
		chartArea2.AxisY.MajorGrid.Enabled = false;
		chartArea2.AxisY.MajorTickMark.Enabled = false;
		chartArea2.AxisY.Minimum = 0.0;
		chartArea2.AxisY2.LabelStyle.Enabled = false;
		chartArea2.BackColor = System.Drawing.Color.Transparent;
		chartArea2.InnerPlotPosition.Auto = false;
		chartArea2.InnerPlotPosition.Height = 84f;
		chartArea2.InnerPlotPosition.Width = 90f;
		chartArea2.InnerPlotPosition.X = 9f;
		chartArea2.InnerPlotPosition.Y = 4f;
		chartArea2.Name = "ChartArea2";
		chartArea2.Position.Auto = false;
		chartArea2.Position.Height = 100f;
		chartArea2.Position.Width = 100f;
		this.chartMain.ChartAreas.Add(chartArea);
		this.chartMain.ChartAreas.Add(chartArea2);
		this.chartMain.Dock = System.Windows.Forms.DockStyle.Fill;
		legend.Enabled = false;
		legend.Name = "Legend1";
		this.chartMain.Legends.Add(legend);
		this.chartMain.Location = new System.Drawing.Point(0, 0);
		this.chartMain.Name = "chartMain";
		series.BorderColor = System.Drawing.Color.Black;
		series.ChartArea = "ChartArea2";
		series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bar;
		series.Color = System.Drawing.Color.Transparent;
		series.CustomProperties = "DrawSideBySide=False, PointWidth=1";
		series.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		series.LabelBackColor = System.Drawing.Color.Black;
		series.LabelForeColor = System.Drawing.Color.White;
		series.LabelFormat = "0.####";
		series.Legend = "Legend1";
		series.Name = "Frequency";
		series.SmartLabelStyle.CalloutBackColor = System.Drawing.Color.Empty;
		series2.BorderWidth = 2;
		series2.ChartArea = "ChartArea1";
		series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
		series2.Color = System.Drawing.Color.Black;
		series2.LabelBackColor = System.Drawing.Color.Black;
		series2.LabelForeColor = System.Drawing.Color.White;
		series2.LabelFormat = "0.####";
		series2.Legend = "Legend1";
		series2.Name = "FrequencyL";
		series3.BorderWidth = 2;
		series3.ChartArea = "ChartArea1";
		series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
		series3.Color = System.Drawing.Color.Red;
		series3.CustomProperties = "DrawSideBySide=False, PointWidth=0, PixelPointWidth=4";
		series3.LabelBackColor = System.Drawing.Color.Black;
		series3.LabelForeColor = System.Drawing.Color.White;
		series3.LabelFormat = "0.####";
		series3.Legend = "Legend1";
		series3.Name = "USL";
		series3.XAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
		series4.BorderWidth = 2;
		series4.ChartArea = "ChartArea1";
		series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
		series4.Color = System.Drawing.Color.Red;
		series4.LabelBackColor = System.Drawing.Color.Black;
		series4.LabelForeColor = System.Drawing.Color.White;
		series4.LabelFormat = "0.####";
		series4.Legend = "Legend1";
		series4.Name = "LSL";
		series4.XAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
		series5.BorderWidth = 2;
		series5.ChartArea = "ChartArea1";
		series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
		series5.Color = System.Drawing.Color.Blue;
		series5.LabelBackColor = System.Drawing.Color.Black;
		series5.LabelForeColor = System.Drawing.Color.White;
		series5.LabelFormat = "0.####";
		series5.Legend = "Legend1";
		series5.Name = "UCL";
		series5.XAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
		series6.BorderWidth = 2;
		series6.ChartArea = "ChartArea1";
		series6.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
		series6.Color = System.Drawing.Color.Blue;
		series6.LabelBackColor = System.Drawing.Color.Black;
		series6.LabelForeColor = System.Drawing.Color.White;
		series6.LabelFormat = "0.####";
		series6.Legend = "Legend1";
		series6.Name = "LCL";
		series6.XAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
		series7.BorderWidth = 2;
		series7.ChartArea = "ChartArea1";
		series7.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
		series7.Color = System.Drawing.Color.Black;
		series7.LabelBackColor = System.Drawing.Color.Black;
		series7.LabelForeColor = System.Drawing.Color.White;
		series7.LabelFormat = "0.####";
		series7.Legend = "Legend1";
		series7.Name = "Avegare";
		series7.XAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
		this.chartMain.Series.Add(series);
		this.chartMain.Series.Add(series2);
		this.chartMain.Series.Add(series3);
		this.chartMain.Series.Add(series4);
		this.chartMain.Series.Add(series5);
		this.chartMain.Series.Add(series6);
		this.chartMain.Series.Add(series7);
		this.chartMain.Size = new System.Drawing.Size(908, 211);
		this.chartMain.TabIndex = 9;
		this.chartMain.MouseMove += new System.Windows.Forms.MouseEventHandler(chartMain_MouseMove);
		this.panelInfor.Dock = System.Windows.Forms.DockStyle.Right;
		this.panelInfor.Location = new System.Drawing.Point(908, 0);
		this.panelInfor.MaximumSize = new System.Drawing.Size(90, 0);
		this.panelInfor.MinimumSize = new System.Drawing.Size(90, 0);
		this.panelInfor.Name = "panelInfor";
		this.panelInfor.Size = new System.Drawing.Size(90, 211);
		this.panelInfor.TabIndex = 10;
		this.tpanelDifference.AutoSize = true;
		this.tpanelDifference.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		this.tpanelDifference.ColumnCount = 11;
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
		this.tpanelDifference.Controls.Add(this.lblTrend, 0, 1);
		this.tpanelDifference.Controls.Add(this.lblCountOut, 2, 1);
		this.tpanelDifference.Controls.Add(this.lblTitleCountOut, 1, 1);
		this.tpanelDifference.Controls.Add(this.lblDifferenceRange, 8, 0);
		this.tpanelDifference.Controls.Add(this.lblTitleDifferenceRange, 7, 0);
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
		this.tpanelDifference.Location = new System.Drawing.Point(0, 70);
		this.tpanelDifference.Name = "tpanelDifference";
		this.tpanelDifference.RowCount = 2;
		this.tpanelDifference.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tpanelDifference.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tpanelDifference.Size = new System.Drawing.Size(1000, 47);
		this.tpanelDifference.TabIndex = 11;
		this.lblTrend.AutoSize = true;
		this.lblTrend.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTrend.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTrend.ForeColor = System.Drawing.Color.Blue;
		this.lblTrend.Location = new System.Drawing.Point(4, 27);
		this.lblTrend.Margin = new System.Windows.Forms.Padding(3);
		this.lblTrend.Name = "lblTrend";
		this.lblTrend.Size = new System.Drawing.Size(156, 16);
		this.lblTrend.TabIndex = 20;
		this.lblTrend.Text = "...";
		this.lblTrend.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblCountOut.AutoSize = true;
		this.lblCountOut.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCountOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCountOut.Location = new System.Drawing.Point(273, 27);
		this.lblCountOut.Margin = new System.Windows.Forms.Padding(3);
		this.lblCountOut.Name = "lblCountOut";
		this.lblCountOut.Size = new System.Drawing.Size(19, 16);
		this.lblCountOut.TabIndex = 19;
		this.lblCountOut.Text = "...";
		this.lblCountOut.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblTitleCountOut.AutoSize = true;
		this.lblTitleCountOut.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitleCountOut.Location = new System.Drawing.Point(167, 27);
		this.lblTitleCountOut.Margin = new System.Windows.Forms.Padding(3);
		this.lblTitleCountOut.Name = "lblTitleCountOut";
		this.lblTitleCountOut.Size = new System.Drawing.Size(99, 16);
		this.lblTitleCountOut.TabIndex = 18;
		this.lblTitleCountOut.Text = "Count out";
		this.lblTitleCountOut.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblDifferenceRange.AutoSize = true;
		this.lblDifferenceRange.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblDifferenceRange.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblDifferenceRange.Location = new System.Drawing.Point(590, 4);
		this.lblDifferenceRange.Margin = new System.Windows.Forms.Padding(3);
		this.lblDifferenceRange.Name = "lblDifferenceRange";
		this.lblDifferenceRange.Size = new System.Drawing.Size(19, 16);
		this.lblDifferenceRange.TabIndex = 17;
		this.lblDifferenceRange.Text = "...";
		this.lblDifferenceRange.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblTitleDifferenceRange.AutoSize = true;
		this.lblTitleDifferenceRange.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitleDifferenceRange.Location = new System.Drawing.Point(535, 4);
		this.lblTitleDifferenceRange.Margin = new System.Windows.Forms.Padding(3);
		this.lblTitleDifferenceRange.Name = "lblTitleDifferenceRange";
		this.lblTitleDifferenceRange.Size = new System.Drawing.Size(48, 16);
		this.lblTitleDifferenceRange.TabIndex = 16;
		this.lblTitleDifferenceRange.Text = "Range";
		this.lblTitleDifferenceRange.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
		this.lblDifference.Size = new System.Drawing.Size(282, 16);
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
		base.Controls.Add(this.panelChart);
		base.Controls.Add(this.tpanelDifference);
		base.Controls.Add(this.tpanelHeader);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "mQualityReport";
		base.Size = new System.Drawing.Size(1000, 330);
		base.Load += new System.EventHandler(mQualityReport_Load);
		this.tpanelHeader.ResumeLayout(false);
		this.tpanelHeader.PerformLayout();
		this.panelChart.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.chartMain).EndInit();
		this.tpanelDifference.ResumeLayout(false);
		this.tpanelDifference.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
