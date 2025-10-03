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

public class mChartRInterpreting : UserControl
{
	private CommonCpk mCommonCpk;

	private List<DataCpk> mDataCpks;

	private KeyValuePair<string, List<int>> mErrors;

	private HitTestResult mPreHit;

	private int mPointSize;

	private Color mPointColor;

	private IContainer components = null;

	private Chart chartMain;

	private Panel panelInfor;

	public mChartRInterpreting(CommonCpk commonCpk, List<DataCpk> dataCpks, KeyValuePair<string, List<int>> errors)
	{
		InitializeComponent();
		mCommonCpk = commonCpk;
		mDataCpks = dataCpks;
		mErrors = errors;
	}

	private void mChartRInterpreting_Load(object sender, EventArgs e)
	{
		Init();
	}

	private void Init()
	{
		chartMain.Titles[0].Text = mErrors.Key;
		chartMain.Titles[1].Text = "R" + mErrors.Key.Substring(1, 1);
		chartMain.ChartAreas[0].AxisX.Minimum = 1.0;
		chartMain.ChartAreas[0].AxisX.Maximum = mDataCpks.Count;
		List<double> source = new List<double> { mCommonCpk.R, mCommonCpk.RUCL, mCommonCpk.AveR };
		double num = Math.Round((mCommonCpk.Upper - mCommonCpk.Lower) * 0.1, 4, MidpointRounding.AwayFromZero);
		int num2 = 0;
		double num3 = Math.Round(source.Max(), 4, MidpointRounding.AwayFromZero) + num;
		chartMain.ChartAreas[0].AxisY.Minimum = num2;
		chartMain.ChartAreas[0].AxisY.Maximum = num3;
		chartMain.ChartAreas[0].AxisY.Interval = Math.Round((num3 - (double)num2) / 5.0, 4, MidpointRounding.AwayFromZero);
		foreach (DataCpk mDataCpk in mDataCpks)
		{
			int index = mDataCpks.IndexOf(mDataCpk);
			chartMain.Series["RU"].Points.AddXY(mDataCpk.Date, mCommonCpk.R);
			chartMain.Series["RUCL"].Points.AddXY(mDataCpk.Date, mCommonCpk.RUCL);
			chartMain.Series["AveR"].Points.AddXY(mDataCpk.Date, mCommonCpk.AveR);
			chartMain.Series["Range"].Points.AddXY(mDataCpk.Date, mDataCpk.Range);
			if (mErrors.Value.Any((int x) => x.Equals(index)))
			{
				switch (chartMain.Titles[1].Text)
				{
				case "R2":
					chartMain.Series["Range"].Points[index - 5].MarkerColor = Color.Yellow;
					chartMain.Series["Range"].Points[index - 4].Color = Color.Yellow;
					chartMain.Series["Range"].Points[index - 3].Color = Color.Yellow;
					chartMain.Series["Range"].Points[index - 2].Color = Color.Yellow;
					chartMain.Series["Range"].Points[index - 1].Color = Color.Yellow;
					chartMain.Series["Range"].Points[index].Color = Color.Yellow;
					break;
				case "R3":
					chartMain.Series["Range"].Points[index - 6].MarkerColor = Color.Yellow;
					chartMain.Series["Range"].Points[index - 5].Color = Color.Yellow;
					chartMain.Series["Range"].Points[index - 4].Color = Color.Yellow;
					chartMain.Series["Range"].Points[index - 3].Color = Color.Yellow;
					chartMain.Series["Range"].Points[index - 2].Color = Color.Yellow;
					chartMain.Series["Range"].Points[index - 1].Color = Color.Yellow;
					chartMain.Series["Range"].Points[index].Color = Color.Yellow;
					break;
				case "R4":
					chartMain.Series["Range"].Points[index - 13].MarkerColor = Color.Yellow;
					chartMain.Series["Range"].Points[index - 12].Color = Color.Yellow;
					chartMain.Series["Range"].Points[index - 11].Color = Color.Yellow;
					chartMain.Series["Range"].Points[index - 10].Color = Color.Yellow;
					chartMain.Series["Range"].Points[index - 9].Color = Color.Yellow;
					chartMain.Series["Range"].Points[index - 7].Color = Color.Yellow;
					chartMain.Series["Range"].Points[index - 6].Color = Color.Yellow;
					chartMain.Series["Range"].Points[index - 5].Color = Color.Yellow;
					chartMain.Series["Range"].Points[index - 4].Color = Color.Yellow;
					chartMain.Series["Range"].Points[index - 3].Color = Color.Yellow;
					chartMain.Series["Range"].Points[index - 2].Color = Color.Yellow;
					chartMain.Series["Range"].Points[index - 1].Color = Color.Yellow;
					chartMain.Series["Range"].Points[index].Color = Color.Yellow;
					break;
				}
				chartMain.Series["Range"].Points[index].MarkerColor = Color.Yellow;
				chartMain.Series["Range"].Points[index].MarkerSize = 8;
				chartMain.Series["Range"].Points[index].MarkerBorderColor = Color.Black;
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
		List<double> list = new List<double> { mCommonCpk.R, mCommonCpk.RUCL, mCommonCpk.AveR }.OrderByDescending((double x) => x).ToList();
		TransparentLabel transparentLabel = new TransparentLabel
		{
			Text = "RU " + ((mCommonCpk.Unit == "°") ? Common.ConvertDoubleToDegrees(mCommonCpk.R) : mCommonCpk.R.ToString("0.0000")),
			Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0),
			Margin = new Padding(0),
			AutoSize = true,
			ForeColor = Color.Red
		};
		panelInfor.Controls.Add(transparentLabel);
		transparentLabel.Location = new Point(0, GetLocationY(num3 + transparentLabel.Height / 2, num4 - transparentLabel.Height / 2, 3, list.IndexOf(mCommonCpk.R)) - transparentLabel.Height / 2);
		TransparentLabel transparentLabel2 = new TransparentLabel
		{
			Text = "R UCL " + ((mCommonCpk.Unit == "°") ? Common.ConvertDoubleToDegrees(mCommonCpk.RUCL) : mCommonCpk.RUCL.ToString("0.0000")),
			Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0),
			Margin = new Padding(0),
			AutoSize = true,
			ForeColor = Color.Blue
		};
		panelInfor.Controls.Add(transparentLabel2);
		transparentLabel2.Location = new Point(0, GetLocationY(num3 + transparentLabel2.Height / 2, num4 - transparentLabel2.Height / 2, 3, list.IndexOf(mCommonCpk.RUCL)) - transparentLabel2.Height / 2);
		TransparentLabel transparentLabel3 = new TransparentLabel
		{
			Text = "R\u00af " + ((mCommonCpk.Unit == "°") ? Common.ConvertDoubleToDegrees(mCommonCpk.AveR) : mCommonCpk.AveR.ToString("0.0000")),
			Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0),
			Margin = new Padding(0),
			AutoSize = true
		};
		panelInfor.Controls.Add(transparentLabel3);
		transparentLabel3.Location = new Point(0, GetLocationY(num3 + transparentLabel3.Height / 2, num4 - transparentLabel3.Height / 2, 3, list.IndexOf(mCommonCpk.AveR)) - transparentLabel3.Height / 2);
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
			if (mPreHit.Series.Name.Equals("Range"))
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
			if (mPreHit.Series.Name.Equals("Range"))
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
		System.Windows.Forms.DataVisualization.Charting.Title title = new System.Windows.Forms.DataVisualization.Charting.Title();
		System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
		this.chartMain = new System.Windows.Forms.DataVisualization.Charting.Chart();
		this.panelInfor = new System.Windows.Forms.Panel();
		((System.ComponentModel.ISupportInitialize)this.chartMain).BeginInit();
		base.SuspendLayout();
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
		series.Name = "RU";
		series2.BorderWidth = 2;
		series2.ChartArea = "ChartArea1";
		series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
		series2.Color = System.Drawing.Color.Blue;
		series2.LabelBackColor = System.Drawing.Color.Black;
		series2.LabelForeColor = System.Drawing.Color.White;
		series2.LabelFormat = "0.####";
		series2.Legend = "Legend1";
		series2.Name = "RUCL";
		series3.BorderWidth = 2;
		series3.ChartArea = "ChartArea1";
		series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
		series3.Color = System.Drawing.Color.DimGray;
		series3.LabelBackColor = System.Drawing.Color.Black;
		series3.LabelForeColor = System.Drawing.Color.White;
		series3.LabelFormat = "0.####";
		series3.Legend = "Legend1";
		series3.Name = "AveR";
		series4.BorderWidth = 2;
		series4.ChartArea = "ChartArea1";
		series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
		series4.Color = System.Drawing.Color.Black;
		series4.LabelBackColor = System.Drawing.Color.Black;
		series4.LabelForeColor = System.Drawing.Color.White;
		series4.LabelFormat = "0.####";
		series4.Legend = "Legend1";
		series4.MarkerBorderColor = System.Drawing.Color.Black;
		series4.MarkerSize = 6;
		series4.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
		series4.Name = "Range";
		this.chartMain.Series.Add(series);
		this.chartMain.Series.Add(series2);
		this.chartMain.Series.Add(series3);
		this.chartMain.Series.Add(series4);
		this.chartMain.Size = new System.Drawing.Size(908, 98);
		this.chartMain.TabIndex = 12;
		title.Alignment = System.Drawing.ContentAlignment.MiddleLeft;
		title.Font = new System.Drawing.Font("Microsoft Sans Serif", 10f);
		title.ForeColor = System.Drawing.Color.Blue;
		title.Name = "Title1";
		title.Position.Auto = false;
		title.Position.Height = 12f;
		title.Position.Width = 91f;
		title.Position.X = 9f;
		title.Position.Y = 1f;
		title.Text = "8";
		title2.BackColor = System.Drawing.Color.Green;
		title2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10f, System.Drawing.FontStyle.Bold);
		title2.ForeColor = System.Drawing.Color.White;
		title2.Name = "Title2";
		title2.Position.Auto = false;
		title2.Position.Height = 36f;
		title2.Position.Width = 2f;
		title2.Text = "R8";
		this.chartMain.Titles.Add(title);
		this.chartMain.Titles.Add(title2);
		this.chartMain.MouseMove += new System.Windows.Forms.MouseEventHandler(chartMain_MouseMove);
		this.panelInfor.Dock = System.Windows.Forms.DockStyle.Right;
		this.panelInfor.Location = new System.Drawing.Point(908, 0);
		this.panelInfor.MaximumSize = new System.Drawing.Size(90, 0);
		this.panelInfor.MinimumSize = new System.Drawing.Size(90, 0);
		this.panelInfor.Name = "panelInfor";
		this.panelInfor.Size = new System.Drawing.Size(90, 98);
		this.panelInfor.TabIndex = 11;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		base.Controls.Add(this.chartMain);
		base.Controls.Add(this.panelInfor);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "mChartRInterpreting";
		base.Size = new System.Drawing.Size(998, 98);
		base.Load += new System.EventHandler(mChartRInterpreting_Load);
		((System.ComponentModel.ISupportInitialize)this.chartMain).EndInit();
		base.ResumeLayout(false);
	}
}
