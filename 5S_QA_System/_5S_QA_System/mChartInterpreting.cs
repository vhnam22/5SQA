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

public class mChartInterpreting : UserControl
{
	private CommonCpk mCommonCpk;

	private List<DataCpk> mDataCpks;

	private KeyValuePair<string, List<int>> mErrors;

	private HitTestResult mPreHit;

	private int mPointSize;

	private Color mPointColor;

	private IContainer components = null;

	private Panel panelInfor;

	private Chart chartMain;

	public mChartInterpreting(CommonCpk commonCpk, List<DataCpk> dataCpks, KeyValuePair<string, List<int>> errors)
	{
		InitializeComponent();
		mCommonCpk = commonCpk;
		mDataCpks = dataCpks;
		mErrors = errors;
	}

	private void mChartInterpreting_Load(object sender, EventArgs e)
	{
		Init();
	}

	private void Init()
	{
		chartMain.Titles[0].Text = mErrors.Key;
		chartMain.Titles[1].Text = mErrors.Key.Substring(1, 1);
		chartMain.ChartAreas[0].AxisX.Minimum = 1.0;
		chartMain.ChartAreas[0].AxisX.Maximum = mDataCpks.Count;
		List<double> source = new List<double> { mCommonCpk.USL, mCommonCpk.UCL, mCommonCpk.Sigma3, mCommonCpk.CL, mCommonCpk.MSigma3, mCommonCpk.LCL, mCommonCpk.LSL };
		double num = Math.Round((mCommonCpk.Upper - mCommonCpk.Lower) * 0.1, 4, MidpointRounding.AwayFromZero);
		double num2 = Math.Round(source.Min(), 4, MidpointRounding.AwayFromZero) - num;
		double num3 = Math.Round(source.Max(), 4, MidpointRounding.AwayFromZero) + num;
		chartMain.ChartAreas[0].AxisY.Minimum = num2;
		chartMain.ChartAreas[0].AxisY.Maximum = num3;
		chartMain.ChartAreas[0].AxisY.Interval = Math.Round((num3 - num2) / 10.0, 4, MidpointRounding.AwayFromZero);
		foreach (DataCpk mDataCpk in mDataCpks)
		{
			int index = mDataCpks.IndexOf(mDataCpk);
			chartMain.Series["USL"].Points.AddXY(mDataCpk.Date, mCommonCpk.USL);
			chartMain.Series["UCL"].Points.AddXY(mDataCpk.Date, mCommonCpk.UCL);
			chartMain.Series["+2σ"].Points.AddXY(mDataCpk.Date, mCommonCpk.Sigma2);
			chartMain.Series["+1σ"].Points.AddXY(mDataCpk.Date, mCommonCpk.Sigma1);
			chartMain.Series["CL"].Points.AddXY(mDataCpk.Date, mCommonCpk.CL);
			chartMain.Series["-1σ"].Points.AddXY(mDataCpk.Date, mCommonCpk.MSigma1);
			chartMain.Series["-2σ"].Points.AddXY(mDataCpk.Date, mCommonCpk.MSigma2);
			chartMain.Series["LCL"].Points.AddXY(mDataCpk.Date, mCommonCpk.LCL);
			chartMain.Series["LSL"].Points.AddXY(mDataCpk.Date, mCommonCpk.LSL);
			chartMain.Series["Data"].Points.AddXY(mDataCpk.Date, mDataCpk.Average);
			if (mErrors.Value.Any((int x) => x.Equals(index)))
			{
				switch (chartMain.Titles[1].Text)
				{
				case "2":
					chartMain.Series["Data"].Points[index - 2].MarkerColor = Color.Yellow;
					chartMain.Series["Data"].Points[index - 1].Color = Color.Yellow;
					chartMain.Series["Data"].Points[index].Color = Color.Yellow;
					break;
				case "3":
					chartMain.Series["Data"].Points[index - 4].MarkerColor = Color.Yellow;
					chartMain.Series["Data"].Points[index - 3].Color = Color.Yellow;
					chartMain.Series["Data"].Points[index - 2].Color = Color.Yellow;
					chartMain.Series["Data"].Points[index - 1].Color = Color.Yellow;
					chartMain.Series["Data"].Points[index].Color = Color.Yellow;
					break;
				case "4":
					chartMain.Series["Data"].Points[index - 5].MarkerColor = Color.Yellow;
					chartMain.Series["Data"].Points[index - 4].Color = Color.Yellow;
					chartMain.Series["Data"].Points[index - 3].Color = Color.Yellow;
					chartMain.Series["Data"].Points[index - 2].Color = Color.Yellow;
					chartMain.Series["Data"].Points[index - 1].Color = Color.Yellow;
					chartMain.Series["Data"].Points[index].Color = Color.Yellow;
					break;
				case "5":
					chartMain.Series["Data"].Points[index - 6].MarkerColor = Color.Yellow;
					chartMain.Series["Data"].Points[index - 5].Color = Color.Yellow;
					chartMain.Series["Data"].Points[index - 4].Color = Color.Yellow;
					chartMain.Series["Data"].Points[index - 3].Color = Color.Yellow;
					chartMain.Series["Data"].Points[index - 2].Color = Color.Yellow;
					chartMain.Series["Data"].Points[index - 1].Color = Color.Yellow;
					chartMain.Series["Data"].Points[index].Color = Color.Yellow;
					break;
				case "6":
					chartMain.Series["Data"].Points[index - 7].MarkerColor = Color.Yellow;
					chartMain.Series["Data"].Points[index - 6].Color = Color.Yellow;
					chartMain.Series["Data"].Points[index - 5].Color = Color.Yellow;
					chartMain.Series["Data"].Points[index - 4].Color = Color.Yellow;
					chartMain.Series["Data"].Points[index - 3].Color = Color.Yellow;
					chartMain.Series["Data"].Points[index - 2].Color = Color.Yellow;
					chartMain.Series["Data"].Points[index - 1].Color = Color.Yellow;
					chartMain.Series["Data"].Points[index].Color = Color.Yellow;
					break;
				case "7":
					chartMain.Series["Data"].Points[index - 13].MarkerColor = Color.Yellow;
					chartMain.Series["Data"].Points[index - 12].Color = Color.Yellow;
					chartMain.Series["Data"].Points[index - 11].Color = Color.Yellow;
					chartMain.Series["Data"].Points[index - 10].Color = Color.Yellow;
					chartMain.Series["Data"].Points[index - 9].Color = Color.Yellow;
					chartMain.Series["Data"].Points[index - 8].Color = Color.Yellow;
					chartMain.Series["Data"].Points[index - 7].Color = Color.Yellow;
					chartMain.Series["Data"].Points[index - 6].Color = Color.Yellow;
					chartMain.Series["Data"].Points[index - 5].Color = Color.Yellow;
					chartMain.Series["Data"].Points[index - 4].Color = Color.Yellow;
					chartMain.Series["Data"].Points[index - 3].Color = Color.Yellow;
					chartMain.Series["Data"].Points[index - 2].Color = Color.Yellow;
					chartMain.Series["Data"].Points[index - 1].Color = Color.Yellow;
					chartMain.Series["Data"].Points[index].Color = Color.Yellow;
					break;
				case "8":
					chartMain.Series["Data"].Points[index - 14].MarkerColor = Color.Yellow;
					chartMain.Series["Data"].Points[index - 13].Color = Color.Yellow;
					chartMain.Series["Data"].Points[index - 12].Color = Color.Yellow;
					chartMain.Series["Data"].Points[index - 11].Color = Color.Yellow;
					chartMain.Series["Data"].Points[index - 10].Color = Color.Yellow;
					chartMain.Series["Data"].Points[index - 9].Color = Color.Yellow;
					chartMain.Series["Data"].Points[index - 8].Color = Color.Yellow;
					chartMain.Series["Data"].Points[index - 7].Color = Color.Yellow;
					chartMain.Series["Data"].Points[index - 6].Color = Color.Yellow;
					chartMain.Series["Data"].Points[index - 5].Color = Color.Yellow;
					chartMain.Series["Data"].Points[index - 4].Color = Color.Yellow;
					chartMain.Series["Data"].Points[index - 3].Color = Color.Yellow;
					chartMain.Series["Data"].Points[index - 2].Color = Color.Yellow;
					chartMain.Series["Data"].Points[index - 1].Color = Color.Yellow;
					chartMain.Series["Data"].Points[index].Color = Color.Yellow;
					break;
				}
				chartMain.Series["Data"].Points[index].MarkerColor = Color.Yellow;
				chartMain.Series["Data"].Points[index].MarkerSize = 8;
				chartMain.Series["Data"].Points[index].MarkerBorderColor = Color.Black;
			}
		}
		if (mCommonCpk.Unit == "°")
		{
			chartMain.ChartAreas[0].AxisY.CustomLabels.Clear();
			for (double num4 = chartMain.ChartAreas[0].AxisY.Minimum; num4 <= chartMain.ChartAreas[0].AxisY.Maximum; num4 += chartMain.ChartAreas[0].AxisY.Interval)
			{
				chartMain.ChartAreas[0].AxisY.CustomLabels.Add(num4 - chartMain.ChartAreas[0].AxisY.Interval / 2.0, num4 + chartMain.ChartAreas[0].AxisY.Interval / 2.0, Common.ConvertDoubleToDegrees(num4, uniformity: true));
			}
		}
		DrawInformation();
	}

	private void DrawInformation()
	{
		int num = (int)((float)chartMain.Height * chartMain.ChartAreas[0].Position.Y / 100f);
		int num2 = (int)((float)chartMain.Height * chartMain.ChartAreas[0].Position.Height / 100f);
		int num3 = (int)((float)num + (float)num2 * chartMain.ChartAreas[0].InnerPlotPosition.Y / 100f);
		int num4 = (int)((float)num3 + (float)num2 * (chartMain.ChartAreas[0].InnerPlotPosition.Y + chartMain.ChartAreas[0].InnerPlotPosition.Height) / 100f);
		List<double> list = new List<double> { mCommonCpk.USL, mCommonCpk.UCL, mCommonCpk.CL, mCommonCpk.LCL, mCommonCpk.LSL }.OrderByDescending((double x) => x).ToList();
		TransparentLabel transparentLabel = new TransparentLabel
		{
			Text = "USL " + ((mCommonCpk.Unit == "°") ? Common.ConvertDoubleToDegrees(mCommonCpk.USL) : mCommonCpk.USL.ToString("0.0000")),
			Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0),
			Margin = new Padding(0),
			AutoSize = true,
			ForeColor = Color.Red
		};
		panelInfor.Controls.Add(transparentLabel);
		transparentLabel.Location = new Point(0, GetLocationY(num3 + transparentLabel.Height / 2, num4 - transparentLabel.Height / 2, 5, list.IndexOf(mCommonCpk.USL)) - transparentLabel.Height / 2);
		TransparentLabel transparentLabel2 = new TransparentLabel
		{
			Text = "LSL " + ((mCommonCpk.Unit == "°") ? Common.ConvertDoubleToDegrees(mCommonCpk.LSL) : mCommonCpk.LSL.ToString("0.0000")),
			Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0),
			Margin = new Padding(0),
			AutoSize = true,
			ForeColor = Color.Red
		};
		panelInfor.Controls.Add(transparentLabel2);
		transparentLabel2.Location = new Point(0, GetLocationY(num3 + transparentLabel2.Height / 2, num4 - transparentLabel2.Height / 2, 5, list.IndexOf(mCommonCpk.LSL)) - transparentLabel2.Height / 2);
		TransparentLabel transparentLabel3 = new TransparentLabel
		{
			Text = "CL " + ((mCommonCpk.Unit == "°") ? Common.ConvertDoubleToDegrees(mCommonCpk.CL) : mCommonCpk.CL.ToString("0.0000")),
			Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0),
			Margin = new Padding(0),
			AutoSize = true,
			ForeColor = Color.Blue
		};
		panelInfor.Controls.Add(transparentLabel3);
		transparentLabel3.Location = new Point(0, GetLocationY(num3 + transparentLabel3.Height / 2, num4 - transparentLabel3.Height / 2, 5, list.IndexOf(mCommonCpk.CL)) - transparentLabel3.Height / 2);
		TransparentLabel transparentLabel4 = new TransparentLabel
		{
			Text = "UCL " + ((mCommonCpk.Unit == "°") ? Common.ConvertDoubleToDegrees(mCommonCpk.UCL) : mCommonCpk.UCL.ToString("0.0000")),
			Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0),
			Margin = new Padding(0),
			AutoSize = true,
			ForeColor = Color.Blue
		};
		panelInfor.Controls.Add(transparentLabel4);
		transparentLabel4.Location = new Point(0, GetLocationY(num3 + transparentLabel4.Height / 2, num4 - transparentLabel4.Height / 2, 5, list.IndexOf(mCommonCpk.UCL)) - transparentLabel4.Height / 2);
		TransparentLabel transparentLabel5 = new TransparentLabel
		{
			Text = "LCL " + ((mCommonCpk.Unit == "°") ? Common.ConvertDoubleToDegrees(mCommonCpk.LCL) : mCommonCpk.LCL.ToString("0.0000")),
			Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0),
			Margin = new Padding(0),
			AutoSize = true,
			ForeColor = Color.Blue
		};
		panelInfor.Controls.Add(transparentLabel5);
		transparentLabel5.Location = new Point(0, GetLocationY(num3 + transparentLabel5.Height / 2, num4 - transparentLabel5.Height / 2, 5, list.IndexOf(mCommonCpk.LCL)) - transparentLabel5.Height / 2);
	}

	private int GetLocationY(int xmin, int xmax, int number, int pos)
	{
		return pos * (xmax - xmin) / (number - 1) + xmin;
	}

	private void chartMain_MouseMove(object sender, MouseEventArgs e)
	{
		if (mPreHit != null)
		{
			DataPoint dataPoint = mPreHit.Object as DataPoint;
			dataPoint.IsValueShownAsLabel = false;
			if (mPreHit.Series.Name.Equals("Data"))
			{
				dataPoint.MarkerStyle = MarkerStyle.Circle;
				dataPoint.MarkerColor = mPointColor;
				dataPoint.MarkerSize = mPointSize;
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
			if (mPreHit.Series.Name.Equals("Data"))
			{
				mPointSize = dataPoint2.MarkerSize;
				mPointColor = dataPoint2.MarkerColor;
				dataPoint2.MarkerStyle = MarkerStyle.Diamond;
				dataPoint2.MarkerColor = Color.Red;
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
		System.Windows.Forms.DataVisualization.Charting.Title title = new System.Windows.Forms.DataVisualization.Charting.Title();
		System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
		this.panelInfor = new System.Windows.Forms.Panel();
		this.chartMain = new System.Windows.Forms.DataVisualization.Charting.Chart();
		((System.ComponentModel.ISupportInitialize)this.chartMain).BeginInit();
		base.SuspendLayout();
		this.panelInfor.Dock = System.Windows.Forms.DockStyle.Right;
		this.panelInfor.Location = new System.Drawing.Point(910, 0);
		this.panelInfor.MaximumSize = new System.Drawing.Size(90, 0);
		this.panelInfor.MinimumSize = new System.Drawing.Size(90, 0);
		this.panelInfor.Name = "panelInfor";
		this.panelInfor.Size = new System.Drawing.Size(90, 150);
		this.panelInfor.TabIndex = 9;
		chartArea.AxisX.Interval = 1.0;
		chartArea.AxisX.IsLabelAutoFit = false;
		chartArea.AxisX.LabelStyle.Angle = -90;
		chartArea.AxisX.LabelStyle.Enabled = false;
		chartArea.AxisX.MajorGrid.Enabled = false;
		chartArea.AxisX.Minimum = 1.0;
		chartArea.AxisY.IsLabelAutoFit = false;
		chartArea.AxisY.LabelStyle.Format = "0.0000";
		chartArea.AxisY.MajorGrid.Enabled = false;
		chartArea.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
		chartArea.InnerPlotPosition.Auto = false;
		chartArea.InnerPlotPosition.Height = 92f;
		chartArea.InnerPlotPosition.Width = 90f;
		chartArea.InnerPlotPosition.X = 9f;
		chartArea.InnerPlotPosition.Y = 4f;
		chartArea.Name = "ChartArea1";
		chartArea.Position.Auto = false;
		chartArea.Position.Height = 88f;
		chartArea.Position.Width = 100f;
		chartArea.Position.Y = 8f;
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
		series10.MarkerBorderColor = System.Drawing.Color.Black;
		series10.MarkerSize = 6;
		series10.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
		series10.Name = "Data";
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
		this.chartMain.Size = new System.Drawing.Size(910, 150);
		this.chartMain.TabIndex = 10;
		title.Alignment = System.Drawing.ContentAlignment.MiddleLeft;
		title.Font = new System.Drawing.Font("Microsoft Sans Serif", 10f);
		title.ForeColor = System.Drawing.Color.Blue;
		title.Name = "Title1";
		title.Position.Auto = false;
		title.Position.Height = 9f;
		title.Position.Width = 91f;
		title.Position.X = 9f;
		title.Position.Y = 1f;
		title.Text = "8";
		title2.BackColor = System.Drawing.Color.Green;
		title2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10f, System.Drawing.FontStyle.Bold);
		title2.ForeColor = System.Drawing.Color.White;
		title2.Name = "Title2";
		title2.Position.Auto = false;
		title2.Position.Height = 14f;
		title2.Position.Width = 2f;
		title2.Text = "8";
		this.chartMain.Titles.Add(title);
		this.chartMain.Titles.Add(title2);
		this.chartMain.MouseMove += new System.Windows.Forms.MouseEventHandler(chartMain_MouseMove);
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		base.Controls.Add(this.chartMain);
		base.Controls.Add(this.panelInfor);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "mChartInterpreting";
		base.Size = new System.Drawing.Size(1000, 150);
		base.Load += new System.EventHandler(mChartInterpreting_Load);
		((System.ComponentModel.ISupportInitialize)this.chartMain).EndInit();
		base.ResumeLayout(false);
	}
}
