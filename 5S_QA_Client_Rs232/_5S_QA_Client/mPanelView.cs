using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using _5S_QA_Client.Controls;
using _5S_QA_Entities.Abstracts;
using _5S_QA_Entities.Constants;
using _5S_QA_Entities.Models;

namespace _5S_QA_Client;

public class mPanelView : UserControl
{
	private string mUnit;

	private IContainer components = null;

	private ToolTip toolTipmPanelView;

	private DataGridViewTextBoxColumn FooterValue;

	private DataGridViewTextBoxColumn FooterTitle;

	private DataGridViewTextBoxColumn FooterName;

	private DataGridView dgvHistory;

	private DataGridViewTextBoxColumn ContentTitle;

	private DataGridViewTextBoxColumn ContentName;

	private DataGridView dgvContent;

	private DataGridViewTextBoxColumn ContentValue;

	private Panel panelResize;

	private Label lblHistory;

	private DataGridView dgvFooter;

	private Panel panel1;

	private Label lblValue;

	private Label lblTitle;

	private TableLayoutPanel tableLayoutPanelTitle;

	private DataGridViewTextBoxColumn No;

	private DataGridViewTextBoxColumn Value;

	private DataGridViewTextBoxColumn CreatedBy;

	private DataGridViewTextBoxColumn MachineName;

	private new DataGridViewTextBoxColumn Created;

	public mPanelView()
	{
		InitializeComponent();
	}

	private void mPanelView_Load(object sender, EventArgs e)
	{
		base.Visible = false;
		Common.setControls(this);
		ControlResize.Init(panelResize, this, ControlResize.Direction.Horizontal, Cursors.SizeWE);
	}

	private void load_dgvHistory(ResultViewModel item)
	{
		mUnit = item.MeasurementUnit;
		if (!string.IsNullOrEmpty(item.History))
		{
			DataTable dataTable = Common.getDataTable<HistoryViewModel>(item.History);
			dgvHistory.DataSource = Common.reverseDatatable(dataTable);
		}
		dgvHistory.Refresh();
		dgvHistory_Sorted(dgvHistory, null);
	}

	public void load_dgvContent(ResultViewModel item)
	{
		dgvContent.Rows.Clear();
		dgvFooter.Rows.Clear();
		try
		{
			lblValue.Text = ((AuditableEntity)(object)item).Id.ToString();
			List<string> list = MetaType.mPanelOther.Cast<string>().ToList();
			foreach (string colName in list)
			{
				PropertyInfo property = ((object)item).GetType().GetProperty(colName);
				if (!(property != null))
				{
					continue;
				}
				if (MetaType.dgvFooter.Find((string x) => x.Equals(colName)) != null)
				{
					dgvFooter.Rows.Add(colName, Common.getTextLanguage(this, colName), property.GetValue(item, null));
					continue;
				}
				object obj = property.GetValue(item, null);
				if (item.MeasurementUnit == "°" && colName == "Result" && obj != null)
				{
					obj = Common.ConvertDoubleToDegrees(double.Parse(obj.ToString()));
				}
				dgvContent.Rows.Add(colName, Common.getTextLanguage(this, colName), obj);
			}
			load_dgvHistory(item);
		}
		finally
		{
			dgvContent.Size = new Size(base.Width, dgvContent.Rows.Count * 22 + 3);
			dgvFooter.Size = new Size(base.Width, dgvFooter.Rows.Count * 22 + 3);
			dgvHistory.Size = new Size(base.Width, 22 + dgvHistory.Rows.Count * 22 + 3);
			dgvContent.CurrentCell = null;
			dgvContent.Refresh();
			dgvFooter.CurrentCell = null;
			dgvFooter.Refresh();
			dgvHistory.CurrentCell = null;
			dgvHistory.Refresh();
			dgvHistory.BringToFront();
		}
	}

	private void dgvHistory_Sorted(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		DataGridView dataGridView = sender as DataGridView;
		foreach (DataGridViewRow item in (IEnumerable)dataGridView.Rows)
		{
			item.Cells["No"].Value = item.Index + 1;
			if (mUnit == "°" && double.TryParse(item.Cells["Value"].Value?.ToString(), out var result))
			{
				item.Cells["Value"].Value = Common.ConvertDoubleToDegrees(result);
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
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
		this.FooterValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.FooterTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.FooterName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dgvHistory = new System.Windows.Forms.DataGridView();
		this.No = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.CreatedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.MachineName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Created = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ContentTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ContentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dgvContent = new System.Windows.Forms.DataGridView();
		this.ContentValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.panelResize = new System.Windows.Forms.Panel();
		this.toolTipmPanelView = new System.Windows.Forms.ToolTip(this.components);
		this.lblHistory = new System.Windows.Forms.Label();
		this.dgvFooter = new System.Windows.Forms.DataGridView();
		this.panel1 = new System.Windows.Forms.Panel();
		this.lblValue = new System.Windows.Forms.Label();
		this.lblTitle = new System.Windows.Forms.Label();
		this.tableLayoutPanelTitle = new System.Windows.Forms.TableLayoutPanel();
		((System.ComponentModel.ISupportInitialize)this.dgvHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.dgvContent).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.dgvFooter).BeginInit();
		this.tableLayoutPanelTitle.SuspendLayout();
		base.SuspendLayout();
		this.FooterValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		dataGridViewCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		this.FooterValue.DefaultCellStyle = dataGridViewCellStyle;
		this.FooterValue.HeaderText = "Value";
		this.FooterValue.Name = "FooterValue";
		this.FooterValue.ReadOnly = true;
		this.FooterValue.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.FooterValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.FooterTitle.DefaultCellStyle = dataGridViewCellStyle2;
		this.FooterTitle.HeaderText = "Title";
		this.FooterTitle.Name = "FooterTitle";
		this.FooterTitle.ReadOnly = true;
		this.FooterTitle.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.FooterTitle.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.FooterTitle.Width = 120;
		this.FooterName.HeaderText = "Name";
		this.FooterName.Name = "FooterName";
		this.FooterName.ReadOnly = true;
		this.FooterName.Visible = false;
		this.dgvHistory.AllowUserToAddRows = false;
		this.dgvHistory.AllowUserToDeleteRows = false;
		this.dgvHistory.AllowUserToOrderColumns = true;
		this.dgvHistory.AllowUserToResizeRows = false;
		dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
		this.dgvHistory.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
		this.dgvHistory.BackgroundColor = System.Drawing.SystemColors.Window;
		dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.AppWorkspace;
		dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
		dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
		dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
		dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.dgvHistory.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
		this.dgvHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
		this.dgvHistory.Columns.AddRange(this.No, this.Value, this.CreatedBy, this.MachineName, this.Created);
		this.dgvHistory.Dock = System.Windows.Forms.DockStyle.Top;
		this.dgvHistory.EnableHeadersVisualStyles = false;
		this.dgvHistory.Location = new System.Drawing.Point(4, 98);
		this.dgvHistory.Margin = new System.Windows.Forms.Padding(1);
		this.dgvHistory.Name = "dgvHistory";
		this.dgvHistory.ReadOnly = true;
		this.dgvHistory.RowHeadersVisible = false;
		this.dgvHistory.ScrollBars = System.Windows.Forms.ScrollBars.None;
		this.dgvHistory.Size = new System.Drawing.Size(396, 119);
		this.dgvHistory.TabIndex = 147;
		this.dgvHistory.Sorted += new System.EventHandler(dgvHistory_Sorted);
		this.No.DataPropertyName = "No";
		dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		this.No.DefaultCellStyle = dataGridViewCellStyle5;
		this.No.HeaderText = "No";
		this.No.Name = "No";
		this.No.ReadOnly = true;
		this.No.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.No.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.No.Width = 35;
		this.Value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Value.DataPropertyName = "Value";
		dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.Value.DefaultCellStyle = dataGridViewCellStyle6;
		this.Value.FillWeight = 20f;
		this.Value.HeaderText = "Value";
		this.Value.Name = "Value";
		this.Value.ReadOnly = true;
		this.CreatedBy.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.CreatedBy.DataPropertyName = "CreatedBy";
		this.CreatedBy.FillWeight = 30f;
		this.CreatedBy.HeaderText = "Staff";
		this.CreatedBy.Name = "CreatedBy";
		this.CreatedBy.ReadOnly = true;
		this.MachineName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.MachineName.DataPropertyName = "MachineName";
		this.MachineName.FillWeight = 30f;
		this.MachineName.HeaderText = "Machine";
		this.MachineName.Name = "MachineName";
		this.MachineName.ReadOnly = true;
		this.Created.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Created.DataPropertyName = "Created";
		this.Created.FillWeight = 40f;
		this.Created.HeaderText = "Created";
		this.Created.Name = "Created";
		this.Created.ReadOnly = true;
		dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.ContentTitle.DefaultCellStyle = dataGridViewCellStyle7;
		this.ContentTitle.HeaderText = "Tittle";
		this.ContentTitle.Name = "ContentTitle";
		this.ContentTitle.ReadOnly = true;
		this.ContentTitle.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.ContentTitle.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.ContentTitle.Width = 120;
		this.ContentName.HeaderText = "Name";
		this.ContentName.Name = "ContentName";
		this.ContentName.ReadOnly = true;
		this.ContentName.Visible = false;
		this.dgvContent.AllowUserToAddRows = false;
		this.dgvContent.AllowUserToDeleteRows = false;
		this.dgvContent.AllowUserToResizeColumns = false;
		this.dgvContent.AllowUserToResizeRows = false;
		this.dgvContent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		this.dgvContent.ColumnHeadersVisible = false;
		this.dgvContent.Columns.AddRange(this.ContentName, this.ContentTitle, this.ContentValue);
		this.dgvContent.Dock = System.Windows.Forms.DockStyle.Top;
		this.dgvContent.Location = new System.Drawing.Point(4, 28);
		this.dgvContent.Margin = new System.Windows.Forms.Padding(1);
		this.dgvContent.Name = "dgvContent";
		this.dgvContent.ReadOnly = true;
		this.dgvContent.RowHeadersVisible = false;
		this.dgvContent.Size = new System.Drawing.Size(396, 22);
		this.dgvContent.TabIndex = 146;
		this.ContentValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		this.ContentValue.DefaultCellStyle = dataGridViewCellStyle8;
		this.ContentValue.FillWeight = 70f;
		this.ContentValue.HeaderText = "Value";
		this.ContentValue.Name = "ContentValue";
		this.ContentValue.ReadOnly = true;
		this.ContentValue.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.ContentValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.panelResize.Dock = System.Windows.Forms.DockStyle.Left;
		this.panelResize.Location = new System.Drawing.Point(0, 0);
		this.panelResize.Margin = new System.Windows.Forms.Padding(1);
		this.panelResize.Name = "panelResize";
		this.panelResize.Size = new System.Drawing.Size(4, 338);
		this.panelResize.TabIndex = 145;
		this.lblHistory.Dock = System.Windows.Forms.DockStyle.Top;
		this.lblHistory.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblHistory.Location = new System.Drawing.Point(4, 72);
		this.lblHistory.Margin = new System.Windows.Forms.Padding(1);
		this.lblHistory.Name = "lblHistory";
		this.lblHistory.Size = new System.Drawing.Size(396, 26);
		this.lblHistory.TabIndex = 148;
		this.lblHistory.Text = "History";
		this.lblHistory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.dgvFooter.AllowUserToAddRows = false;
		this.dgvFooter.AllowUserToDeleteRows = false;
		this.dgvFooter.AllowUserToResizeColumns = false;
		this.dgvFooter.AllowUserToResizeRows = false;
		this.dgvFooter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		this.dgvFooter.ColumnHeadersVisible = false;
		this.dgvFooter.Columns.AddRange(this.FooterName, this.FooterTitle, this.FooterValue);
		this.dgvFooter.Dock = System.Windows.Forms.DockStyle.Top;
		this.dgvFooter.Location = new System.Drawing.Point(4, 50);
		this.dgvFooter.Margin = new System.Windows.Forms.Padding(1);
		this.dgvFooter.Name = "dgvFooter";
		this.dgvFooter.ReadOnly = true;
		this.dgvFooter.RowHeadersVisible = false;
		this.dgvFooter.ScrollBars = System.Windows.Forms.ScrollBars.None;
		this.dgvFooter.Size = new System.Drawing.Size(396, 22);
		this.dgvFooter.TabIndex = 149;
		this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(4, 27);
		this.panel1.Margin = new System.Windows.Forms.Padding(0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(396, 1);
		this.panel1.TabIndex = 143;
		this.lblValue.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblValue.ForeColor = System.Drawing.Color.Red;
		this.lblValue.Location = new System.Drawing.Point(48, 1);
		this.lblValue.Margin = new System.Windows.Forms.Padding(1);
		this.lblValue.Name = "lblValue";
		this.lblValue.Size = new System.Drawing.Size(347, 25);
		this.lblValue.TabIndex = 134;
		this.lblValue.Text = "...";
		this.lblValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblTitle.AutoSize = true;
		this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTitle.Location = new System.Drawing.Point(1, 1);
		this.lblTitle.Margin = new System.Windows.Forms.Padding(1);
		this.lblTitle.Name = "lblTitle";
		this.lblTitle.Size = new System.Drawing.Size(45, 25);
		this.lblTitle.TabIndex = 0;
		this.lblTitle.Text = "VIEW";
		this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.tableLayoutPanelTitle.AutoSize = true;
		this.tableLayoutPanelTitle.ColumnCount = 2;
		this.tableLayoutPanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanelTitle.Controls.Add(this.lblValue, 1, 0);
		this.tableLayoutPanelTitle.Controls.Add(this.lblTitle, 0, 0);
		this.tableLayoutPanelTitle.Dock = System.Windows.Forms.DockStyle.Top;
		this.tableLayoutPanelTitle.Location = new System.Drawing.Point(4, 0);
		this.tableLayoutPanelTitle.Margin = new System.Windows.Forms.Padding(0);
		this.tableLayoutPanelTitle.Name = "tableLayoutPanelTitle";
		this.tableLayoutPanelTitle.RowCount = 1;
		this.tableLayoutPanelTitle.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanelTitle.Size = new System.Drawing.Size(396, 27);
		this.tableLayoutPanelTitle.TabIndex = 142;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		base.Controls.Add(this.dgvHistory);
		base.Controls.Add(this.lblHistory);
		base.Controls.Add(this.dgvFooter);
		base.Controls.Add(this.dgvContent);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.tableLayoutPanelTitle);
		base.Controls.Add(this.panelResize);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "mPanelView";
		base.Size = new System.Drawing.Size(400, 338);
		base.Load += new System.EventHandler(mPanelView_Load);
		((System.ComponentModel.ISupportInitialize)this.dgvHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.dgvContent).EndInit();
		((System.ComponentModel.ISupportInitialize)this.dgvFooter).EndInit();
		this.tableLayoutPanelTitle.ResumeLayout(false);
		this.tableLayoutPanelTitle.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
