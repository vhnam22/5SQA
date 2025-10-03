using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using _5S_QA_Entities.Dtos;
using _5S_QA_Entities.Models;
using _5S_QA_System.Controls;
using _5S_QA_System.View.User_control;

namespace _5S_QA_System;

public class mChartDetail : UserControl
{
	private readonly StatisticViewModel mModel;

	private readonly StatisticViewModel mType;

	private HitTestResult mPreHit;

	private readonly int mLimit;

	private readonly StatisticDetailDto mDto;

	private IContainer components = null;

	private TableLayoutPanel tpanelMain;

	private Chart chartProductNG;

	private Chart chartMeasNG;

	public mChartDetail(StatisticViewModel model, StatisticViewModel type, int limit, StatisticDetailDto dto)
	{
		InitializeComponent();
		mModel = model;
		mType = type;
		mLimit = limit;
		mDto = dto;
	}

	private void mChartDetail_Load(object sender, EventArgs e)
	{
		Init();
	}

	private void Init()
	{
		chartProductNG.Titles[0].Text = mModel.Name;
		chartProductNG.Titles[1].Text = "← " + Common.getTextLanguage(this, "NGType") + " →";
		chartProductNG.Titles[2].Text = "← " + Common.getTextLanguage(this, "Times") + " →";
		chartMeasNG.Titles[0].Text = mModel.Name;
		chartMeasNG.Titles[1].Text = "← " + Common.getTextLanguage(this, "NGItem") + " →";
		chartMeasNG.Titles[2].Text = "← " + Common.getTextLanguage(this, "Times") + " →";
		IEnumerable<KeyValuePair<string, double>> enumerable = mModel.Values.OrderByDescending((KeyValuePair<string, double> x) => x.Value).Take(mLimit);
		foreach (KeyValuePair<string, double> value in mType.Values)
		{
			chartProductNG.Series["Data"].Points.AddXY(value.Key, value.Value);
		}
		foreach (KeyValuePair<string, double> item in enumerable)
		{
			chartMeasNG.Series["Data"].Points.AddXY(item.Key, item.Value);
		}
	}

	private void chart_MouseMove(object sender, MouseEventArgs e)
	{
		Chart chart = sender as Chart;
		if (mPreHit != null)
		{
			DataPoint dataPoint = mPreHit.Object as DataPoint;
			dataPoint.IsValueShownAsLabel = false;
			chart.Cursor = Cursors.Default;
		}
		HitTestResult hitTestResult = chart.HitTest(e.X, e.Y, ChartElementType.DataPoint);
		if (hitTestResult.ChartElementType.Equals(ChartElementType.DataPoint))
		{
			mPreHit = hitTestResult;
			if (mPreHit != null)
			{
				DataPoint dataPoint2 = mPreHit.Object as DataPoint;
				dataPoint2.IsValueShownAsLabel = true;
				chart.Cursor = Cursors.Hand;
			}
		}
		else
		{
			mPreHit = null;
		}
	}

	private void chart_Click(object sender, EventArgs e)
	{
		Chart chart = sender as Chart;
		if (mPreHit != null)
		{
			DataPoint dataPoint = mPreHit.Object as DataPoint;
			mDto.MeasurementName = dataPoint.AxisLabel;
			mStatisticDetail mStatisticDetail = new mStatisticDetail(mDto, chart.Name == "chartMeasNG")
			{
				Width = base.Width / 2
			};
			base.Controls.Add(mStatisticDetail);
			mStatisticDetail.BringToFront();
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
		System.Windows.Forms.DataVisualization.Charting.Series series = new System.Windows.Forms.DataVisualization.Charting.Series();
		System.Windows.Forms.DataVisualization.Charting.Title title = new System.Windows.Forms.DataVisualization.Charting.Title();
		System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
		System.Windows.Forms.DataVisualization.Charting.Title title3 = new System.Windows.Forms.DataVisualization.Charting.Title();
		System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
		System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
		System.Windows.Forms.DataVisualization.Charting.Title title4 = new System.Windows.Forms.DataVisualization.Charting.Title();
		System.Windows.Forms.DataVisualization.Charting.Title title5 = new System.Windows.Forms.DataVisualization.Charting.Title();
		System.Windows.Forms.DataVisualization.Charting.Title title6 = new System.Windows.Forms.DataVisualization.Charting.Title();
		this.tpanelMain = new System.Windows.Forms.TableLayoutPanel();
		this.chartMeasNG = new System.Windows.Forms.DataVisualization.Charting.Chart();
		this.chartProductNG = new System.Windows.Forms.DataVisualization.Charting.Chart();
		this.tpanelMain.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.chartMeasNG).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.chartProductNG).BeginInit();
		base.SuspendLayout();
		this.tpanelMain.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		this.tpanelMain.ColumnCount = 2;
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tpanelMain.Controls.Add(this.chartMeasNG, 1, 0);
		this.tpanelMain.Controls.Add(this.chartProductNG, 0, 0);
		this.tpanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tpanelMain.Location = new System.Drawing.Point(0, 0);
		this.tpanelMain.Name = "tpanelMain";
		this.tpanelMain.RowCount = 1;
		this.tpanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tpanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tpanelMain.Size = new System.Drawing.Size(800, 230);
		this.tpanelMain.TabIndex = 0;
		chartArea.AxisX.Interval = 1.0;
		chartArea.AxisX.MajorGrid.Enabled = false;
		chartArea.AxisY.MajorGrid.Enabled = false;
		chartArea.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
		chartArea.Name = "ChartArea1";
		this.chartMeasNG.ChartAreas.Add(chartArea);
		this.chartMeasNG.Dock = System.Windows.Forms.DockStyle.Fill;
		this.chartMeasNG.Location = new System.Drawing.Point(403, 4);
		this.chartMeasNG.Name = "chartMeasNG";
		series.ChartArea = "ChartArea1";
		series.Color = System.Drawing.Color.Orange;
		series.LabelBackColor = System.Drawing.Color.Black;
		series.LabelForeColor = System.Drawing.Color.White;
		series.Name = "Data";
		this.chartMeasNG.Series.Add(series);
		this.chartMeasNG.Size = new System.Drawing.Size(393, 222);
		this.chartMeasNG.TabIndex = 2;
		this.chartMeasNG.Text = "chart1";
		title.BackColor = System.Drawing.Color.PaleTurquoise;
		title.BorderColor = System.Drawing.Color.Black;
		title.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		title.ForeColor = System.Drawing.Color.Blue;
		title.Name = "Title1";
		title.Text = "Measurement";
		title2.BorderColor = System.Drawing.Color.Black;
		title2.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
		title2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		title2.Name = "Title2";
		title2.Text = "← Item NG →";
		title3.BorderColor = System.Drawing.Color.Black;
		title3.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Left;
		title3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		title3.Name = "Title3";
		title3.Text = "← Số lần phát sinh →";
		this.chartMeasNG.Titles.Add(title);
		this.chartMeasNG.Titles.Add(title2);
		this.chartMeasNG.Titles.Add(title3);
		this.chartMeasNG.Click += new System.EventHandler(chart_Click);
		this.chartMeasNG.MouseMove += new System.Windows.Forms.MouseEventHandler(chart_MouseMove);
		chartArea2.AxisX.Interval = 1.0;
		chartArea2.AxisX.MajorGrid.Enabled = false;
		chartArea2.AxisY.MajorGrid.Enabled = false;
		chartArea2.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
		chartArea2.Name = "ChartArea1";
		this.chartProductNG.ChartAreas.Add(chartArea2);
		this.chartProductNG.Dock = System.Windows.Forms.DockStyle.Fill;
		this.chartProductNG.Location = new System.Drawing.Point(4, 4);
		this.chartProductNG.Name = "chartProductNG";
		series2.ChartArea = "ChartArea1";
		series2.LabelBackColor = System.Drawing.Color.Black;
		series2.LabelForeColor = System.Drawing.Color.White;
		series2.Name = "Data";
		this.chartProductNG.Series.Add(series2);
		this.chartProductNG.Size = new System.Drawing.Size(392, 222);
		this.chartProductNG.TabIndex = 1;
		this.chartProductNG.Text = "chart1";
		title4.BackColor = System.Drawing.Color.PaleTurquoise;
		title4.BorderColor = System.Drawing.Color.Black;
		title4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		title4.ForeColor = System.Drawing.Color.Blue;
		title4.Name = "Title1";
		title4.Text = "Measurement";
		title5.BorderColor = System.Drawing.Color.Black;
		title5.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
		title5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		title5.Name = "Title2";
		title5.Text = "← Thời điểm phát sinh NG →";
		title6.BorderColor = System.Drawing.Color.Black;
		title6.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Left;
		title6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		title6.Name = "Title3";
		title6.Text = "← Số lần phát sinh →";
		this.chartProductNG.Titles.Add(title4);
		this.chartProductNG.Titles.Add(title5);
		this.chartProductNG.Titles.Add(title6);
		this.chartProductNG.Click += new System.EventHandler(chart_Click);
		this.chartProductNG.MouseMove += new System.Windows.Forms.MouseEventHandler(chart_MouseMove);
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.tpanelMain);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "mChartDetail";
		base.Size = new System.Drawing.Size(800, 230);
		base.Load += new System.EventHandler(mChartDetail_Load);
		this.tpanelMain.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.chartMeasNG).EndInit();
		((System.ComponentModel.ISupportInitialize)this.chartProductNG).EndInit();
		base.ResumeLayout(false);
	}
}
