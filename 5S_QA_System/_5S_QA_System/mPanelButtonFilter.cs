using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using _5S_QA_System.Controls;

namespace _5S_QA_System;

public class mPanelButtonFilter : UserControl
{
	private readonly List<string> Configs = new List<string> { "IsSelect", "No" };

	private TableLayoutPanel tpanelMain;

	private DataGridView dgvMain;

	private IContainer components = null;

	private ToolTip toolTipMain;

	public Dictionary<string, List<string>> mFilters { get; set; }

	public mPanelButtonFilter()
	{
		InitializeComponent();
		mFilters = new Dictionary<string, List<string>>();
	}

	private void mPanelButtonFilter_Load(object sender, EventArgs e)
	{
		if (base.ParentForm != null)
		{
			dgvMain = base.ParentForm.Controls["dgvMain"] as DataGridView;
			InitTablePanel();
		}
	}

	private void InitTablePanel()
	{
		tpanelMain = new TableLayoutPanel
		{
			Name = "tpanelMain",
			AutoSize = true,
			Dock = DockStyle.Fill,
			Location = new Point(0, 0),
			RowCount = 1
		};
		tpanelMain.RowStyles.Add(new RowStyle());
		base.Controls.Add(tpanelMain);
		tpanelMain.BringToFront();
		SetFilterControls();
	}

	public void SetFilterControls()
	{
		GC.Collect();
		tpanelMain.Controls.Clear();
		tpanelMain.ColumnStyles.Clear();
		tpanelMain.ColumnCount = 1;
		tpanelMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
		tpanelMain.Controls.Add(new Label
		{
			Text = Common.getTextLanguage(this, "Filters"),
			TextAlign = ContentAlignment.MiddleCenter,
			Dock = DockStyle.Fill
		}, 0, 0);
		int num = 0;
		foreach (DataGridViewColumn column in dgvMain.Columns)
		{
			if (column.Visible && Configs.IndexOf(column.Name).Equals(-1))
			{
				num++;
				Button button = new Button
				{
					Name = "btn" + column.Name,
					Text = column.HeaderText,
					Width = column.Width,
					Margin = new Padding(0),
					Cursor = Cursors.Hand
				};
				if (mFilters.TryGetValue(column.Name, out var _))
				{
					button.Text += " *";
				}
				button.Click += btnFilter_Click;
				toolTipMain.SetToolTip(button, Common.getTextLanguage(this, "WithField") + " " + column.HeaderText);
				tpanelMain.ColumnCount++;
				tpanelMain.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
				tpanelMain.Controls.Add(button, num, 0);
			}
		}
	}

	public void CloseFormFilter()
	{
		base.ParentForm.Controls.RemoveByKey("mPanelFilter");
	}

	private void btnFilter_Click(object sender, EventArgs e)
	{
		base.ParentForm.Controls.RemoveByKey("mPanelFilter");
		mPanelFilter mPanelFilter2 = new mPanelFilter(this, sender as Control);
		base.ParentForm.Controls.Add(mPanelFilter2);
		mPanelFilter2.BringToFront();
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
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		base.SuspendLayout();
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoSize = true;
		base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Margin = new System.Windows.Forms.Padding(0);
		base.Name = "mPanelButtonFilter";
		base.Size = new System.Drawing.Size(588, 24);
		base.Load += new System.EventHandler(mPanelButtonFilter_Load);
		base.ResumeLayout(false);
	}
}
