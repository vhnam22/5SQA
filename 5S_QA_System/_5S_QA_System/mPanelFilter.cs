using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using _5S_QA_System.Controls;

namespace _5S_QA_System;

public class mPanelFilter : UserControl
{
	private readonly Control mControl;

	private DataTable mDataTable;

	private Dictionary<string, List<string>> mOrigionFilters;

	private readonly mPanelButtonFilter mUserControl;

	private IContainer components = null;

	private TableLayoutPanel tpanelFooter;

	private Button btnClear;

	private ToolTip toolTipMain;

	private Button btnCancel;

	private Button btnFilter;

	private Panel panelView;

	private DataGridView dgvContent;

	private DataGridViewCheckBoxColumn title;

	private DataGridViewTextBoxColumn value;

	private TableLayoutPanel tableLayoutPanel1;

	private CheckBox cbCheckAll;

	private Button btnClose;

	public mPanelFilter(mPanelButtonFilter usercontrol, Control control)
	{
		InitializeComponent();
		mUserControl = usercontrol;
		mControl = control;
		base.Location = new Point(-base.Width, 0);
		Common.setControls(this, toolTipMain);
	}

	private void mPanelFilter_Load(object sender, EventArgs e)
	{
		int num = mControl.Location.X + 27;
		if (num + base.Width > base.ParentForm.Width)
		{
			num -= base.Width - mControl.Width;
		}
		base.Location = new Point(num, base.ParentForm.ActiveControl.Location.Y + mControl.Height);
		mOrigionFilters = mUserControl.mFilters;
		if (base.ParentForm.GetType().Equals(typeof(frmMonthView)))
		{
			mSearchMonth mSearchMonth2 = base.ParentForm.Controls["mSearchMain"] as mSearchMonth;
			mDataTable = mSearchMonth2.dataTable;
		}
		else if (base.ParentForm.GetType().Equals(typeof(frmExportProduct)))
		{
			mSearchExport mSearchExport2 = base.ParentForm.Controls["mSearchMain"] as mSearchExport;
			mDataTable = mSearchExport2.dataTable;
		}
		else
		{
			mSearchChart mSearchChart2 = base.ParentForm.Controls["mSearchMain"] as mSearchChart;
			mDataTable = mSearchChart2.dataTable;
		}
		Init();
	}

	private void Init()
	{
		foreach (DataRow row in mDataTable.Rows)
		{
			dgvContent.Visible = true;
			string itemFilterString = GetItemFilterString(row);
			string key = mControl.Name.Replace("btn", "");
			mOrigionFilters.TryGetValue(key, out var list);
			bool flag = list == null || list.IndexOf(itemFilterString) > -1;
			DataGridViewRow dataGridViewRow = (from DataGridViewRow x in dgvContent.Rows
				where x.Cells["value"].Value.Equals(row[key])
				select x).FirstOrDefault();
			if (dataGridViewRow == null)
			{
				dgvContent.Rows.Add(flag, row[key]);
			}
		}
		dgvContent.Size = new Size(panelView.Width, dgvContent.Rows.Count * 22 + 3);
		dgvContent.CurrentCell = null;
		dgvContent.Refresh();
	}

	private string GetFilterString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		List<string> list = new List<string>();
		foreach (DataGridViewRow item in (IEnumerable)dgvContent.Rows)
		{
			string itemFilterString = GetItemFilterString(item);
			int num = list.IndexOf(itemFilterString);
			if (itemFilterString != null && num.Equals(-1))
			{
				list.Add(itemFilterString);
			}
		}
		string key = mControl.Name.Replace("btn", "");
		if (mOrigionFilters.TryGetValue(key, out var _))
		{
			mOrigionFilters.Remove(key);
		}
		mOrigionFilters.Add(key, list);
		int num2 = 0;
		foreach (KeyValuePair<string, List<string>> mOrigionFilter in mOrigionFilters)
		{
			key = mOrigionFilter.Key;
			foreach (string item2 in mOrigionFilter.Value)
			{
				int num3 = mOrigionFilter.Value.IndexOf(item2);
				if (num3.Equals(0))
				{
					stringBuilder.Append("(Convert([" + key + "], System.String) IN (");
				}
				stringBuilder.Append("'" + item2 + "'");
				if (num3 < mOrigionFilter.Value.Count - 1)
				{
					stringBuilder.Append(", ");
				}
				else
				{
					stringBuilder.Append("))");
				}
			}
			if (num2 < mOrigionFilters.Count - 1)
			{
				stringBuilder.Append(" AND ");
			}
			num2++;
		}
		return stringBuilder.ToString();
	}

	private string GetItemFilterString(DataGridViewRow row)
	{
		string result = null;
		if (bool.Parse(row.Cells["title"].Value.ToString()) && row.Cells["value"].Value != null)
		{
			result = row.Cells["value"].Value.ToString();
		}
		return result;
	}

	private string GetItemFilterString(DataRow row)
	{
		string result = null;
		string columnName = mControl.Name.Replace("btn", "");
		if (row[columnName] != null)
		{
			result = row[columnName].ToString();
		}
		return result;
	}

	private bool IsSelectFilter()
	{
		List<bool> list = new List<bool>();
		foreach (DataGridViewRow item in (IEnumerable)dgvContent.Rows)
		{
			if (bool.Parse(item.Cells["title"].Value.ToString()))
			{
				list.Add(item: true);
			}
		}
		return list.Count > 0 && list.Count < dgvContent.Rows.Count;
	}

	private void cbCheckAll_CheckedChanged(object sender, EventArgs e)
	{
		foreach (DataGridViewRow item in (IEnumerable)dgvContent.Rows)
		{
			item.Cells["title"].Value = cbCheckAll.Checked;
		}
	}

	private void btnCancel_Click(object sender, EventArgs e)
	{
		Dispose();
	}

	private void btnFilter_Click(object sender, EventArgs e)
	{
		if (IsSelectFilter())
		{
			if (base.ParentForm.GetType().Equals(typeof(frmMonthView)))
			{
				mSearchMonth mSearchMonth2 = base.ParentForm.Controls["mSearchMain"] as mSearchMonth;
				mSearchMonth2.mFilter = GetFilterString();
				mSearchMonth2.btnSearch.PerformClick();
			}
			else if (base.ParentForm.GetType().Equals(typeof(frmExportProduct)))
			{
				mSearchExport mSearchExport2 = base.ParentForm.Controls["mSearchMain"] as mSearchExport;
				mSearchExport2.mFilter = GetFilterString();
				mSearchExport2.btnSearch.PerformClick();
			}
			else
			{
				mSearchChart mSearchChart2 = base.ParentForm.Controls["mSearchMain"] as mSearchChart;
				mSearchChart2.mFilter = GetFilterString();
				mSearchChart2.btnSearch.PerformClick();
			}
			if (!mControl.Text.EndsWith(" *"))
			{
				mControl.Text += " *";
			}
		}
		btnCancel.PerformClick();
	}

	private void btnClear_Click(object sender, EventArgs e)
	{
		mOrigionFilters.Clear();
		if (base.ParentForm.GetType().Equals(typeof(frmMonthView)))
		{
			mSearchMonth mSearchMonth2 = base.ParentForm.Controls["mSearchMain"] as mSearchMonth;
			mSearchMonth2.mFilter = string.Empty;
			mSearchMonth2.btnSearch.PerformClick();
		}
		else
		{
			mSearchChart mSearchChart2 = base.ParentForm.Controls["mSearchMain"] as mSearchChart;
			mSearchChart2.mFilter = string.Empty;
			mSearchChart2.btnSearch.PerformClick();
		}
		TableLayoutPanel tableLayoutPanel = mUserControl.Controls["tpanelMain"] as TableLayoutPanel;
		foreach (Control control in tableLayoutPanel.Controls)
		{
			control.Text = control.Text.TrimEnd(' ', '*');
		}
		btnCancel.PerformClick();
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
		this.tpanelFooter = new System.Windows.Forms.TableLayoutPanel();
		this.btnClear = new System.Windows.Forms.Button();
		this.btnCancel = new System.Windows.Forms.Button();
		this.btnFilter = new System.Windows.Forms.Button();
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.btnClose = new System.Windows.Forms.Button();
		this.panelView = new System.Windows.Forms.Panel();
		this.dgvContent = new System.Windows.Forms.DataGridView();
		this.title = new System.Windows.Forms.DataGridViewCheckBoxColumn();
		this.value = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
		this.cbCheckAll = new System.Windows.Forms.CheckBox();
		this.tpanelFooter.SuspendLayout();
		this.panelView.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvContent).BeginInit();
		this.tableLayoutPanel1.SuspendLayout();
		base.SuspendLayout();
		this.tpanelFooter.AutoSize = true;
		this.tpanelFooter.ColumnCount = 5;
		this.tpanelFooter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tpanelFooter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10f));
		this.tpanelFooter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelFooter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10f));
		this.tpanelFooter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelFooter.Controls.Add(this.btnClear, 0, 0);
		this.tpanelFooter.Controls.Add(this.btnCancel, 4, 0);
		this.tpanelFooter.Controls.Add(this.btnFilter, 2, 0);
		this.tpanelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.tpanelFooter.Location = new System.Drawing.Point(5, 367);
		this.tpanelFooter.Name = "tpanelFooter";
		this.tpanelFooter.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
		this.tpanelFooter.RowCount = 1;
		this.tpanelFooter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tpanelFooter.Size = new System.Drawing.Size(290, 28);
		this.tpanelFooter.TabIndex = 6;
		this.btnClear.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnClear.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnClear.Location = new System.Drawing.Point(0, 5);
		this.btnClear.Margin = new System.Windows.Forms.Padding(0);
		this.btnClear.Name = "btnClear";
		this.btnClear.Size = new System.Drawing.Size(120, 23);
		this.btnClear.TabIndex = 1;
		this.btnClear.Text = "Clear filter";
		this.toolTipMain.SetToolTip(this.btnClear, "Select clear filter");
		this.btnClear.UseVisualStyleBackColor = true;
		this.btnClear.Click += new System.EventHandler(btnClear_Click);
		this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnCancel.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnCancel.Location = new System.Drawing.Point(215, 5);
		this.btnCancel.Margin = new System.Windows.Forms.Padding(0);
		this.btnCancel.Name = "btnCancel";
		this.btnCancel.Size = new System.Drawing.Size(75, 23);
		this.btnCancel.TabIndex = 3;
		this.btnCancel.Text = "Cancel";
		this.toolTipMain.SetToolTip(this.btnCancel, "Select cancel");
		this.btnCancel.UseVisualStyleBackColor = true;
		this.btnCancel.Click += new System.EventHandler(btnCancel_Click);
		this.btnFilter.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnFilter.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnFilter.Location = new System.Drawing.Point(130, 5);
		this.btnFilter.Margin = new System.Windows.Forms.Padding(0);
		this.btnFilter.Name = "btnFilter";
		this.btnFilter.Size = new System.Drawing.Size(75, 23);
		this.btnFilter.TabIndex = 2;
		this.btnFilter.Text = "Filter";
		this.toolTipMain.SetToolTip(this.btnFilter, "Select filter");
		this.btnFilter.UseVisualStyleBackColor = true;
		this.btnFilter.Click += new System.EventHandler(btnFilter_Click);
		this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnClose.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnClose.Location = new System.Drawing.Point(260, 1);
		this.btnClose.Margin = new System.Windows.Forms.Padding(0);
		this.btnClose.Name = "btnClose";
		this.btnClose.Size = new System.Drawing.Size(29, 26);
		this.btnClose.TabIndex = 2;
		this.btnClose.Text = "X";
		this.toolTipMain.SetToolTip(this.btnClose, "Select close");
		this.btnClose.UseVisualStyleBackColor = true;
		this.btnClose.Click += new System.EventHandler(btnCancel_Click);
		this.panelView.AutoScroll = true;
		this.panelView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panelView.Controls.Add(this.dgvContent);
		this.panelView.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panelView.Location = new System.Drawing.Point(5, 33);
		this.panelView.Name = "panelView";
		this.panelView.Size = new System.Drawing.Size(290, 334);
		this.panelView.TabIndex = 7;
		this.dgvContent.AllowUserToAddRows = false;
		this.dgvContent.AllowUserToDeleteRows = false;
		this.dgvContent.AllowUserToResizeColumns = false;
		this.dgvContent.AllowUserToResizeRows = false;
		this.dgvContent.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.dgvContent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		this.dgvContent.ColumnHeadersVisible = false;
		this.dgvContent.Columns.AddRange(this.title, this.value);
		this.dgvContent.Dock = System.Windows.Forms.DockStyle.Top;
		this.dgvContent.Location = new System.Drawing.Point(0, 0);
		this.dgvContent.Margin = new System.Windows.Forms.Padding(1);
		this.dgvContent.Name = "dgvContent";
		this.dgvContent.RowHeadersVisible = false;
		this.dgvContent.ScrollBars = System.Windows.Forms.ScrollBars.None;
		this.dgvContent.Size = new System.Drawing.Size(288, 175);
		this.dgvContent.TabIndex = 3;
		this.dgvContent.Visible = false;
		this.title.FalseValue = "False";
		this.title.HeaderText = "Title";
		this.title.Name = "title";
		this.title.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.title.TrueValue = "True";
		this.title.Width = 23;
		this.value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		dataGridViewCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		this.value.DefaultCellStyle = dataGridViewCellStyle;
		this.value.HeaderText = "Value";
		this.value.Name = "value";
		this.value.ReadOnly = true;
		this.value.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.value.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.tableLayoutPanel1.AutoSize = true;
		this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		this.tableLayoutPanel1.ColumnCount = 2;
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 29f));
		this.tableLayoutPanel1.Controls.Add(this.btnClose, 0, 0);
		this.tableLayoutPanel1.Controls.Add(this.cbCheckAll, 0, 0);
		this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.tableLayoutPanel1.Location = new System.Drawing.Point(5, 5);
		this.tableLayoutPanel1.Name = "tableLayoutPanel1";
		this.tableLayoutPanel1.RowCount = 1;
		this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel1.Size = new System.Drawing.Size(290, 28);
		this.tableLayoutPanel1.TabIndex = 8;
		this.cbCheckAll.AutoSize = true;
		this.cbCheckAll.Checked = true;
		this.cbCheckAll.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cbCheckAll.Dock = System.Windows.Forms.DockStyle.Fill;
		this.cbCheckAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.cbCheckAll.Location = new System.Drawing.Point(4, 4);
		this.cbCheckAll.Name = "cbCheckAll";
		this.cbCheckAll.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
		this.cbCheckAll.Size = new System.Drawing.Size(252, 20);
		this.cbCheckAll.TabIndex = 1;
		this.cbCheckAll.Text = "(Select All)";
		this.toolTipMain.SetToolTip(this.cbCheckAll, "Select all");
		this.cbCheckAll.UseVisualStyleBackColor = true;
		this.cbCheckAll.CheckedChanged += new System.EventHandler(cbCheckAll_CheckedChanged);
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		base.Controls.Add(this.panelView);
		base.Controls.Add(this.tpanelFooter);
		base.Controls.Add(this.tableLayoutPanel1);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Margin = new System.Windows.Forms.Padding(0);
		base.Name = "mPanelFilter";
		base.Padding = new System.Windows.Forms.Padding(5);
		base.Size = new System.Drawing.Size(300, 400);
		base.Load += new System.EventHandler(mPanelFilter_Load);
		this.tpanelFooter.ResumeLayout(false);
		this.panelView.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.dgvContent).EndInit();
		this.tableLayoutPanel1.ResumeLayout(false);
		this.tableLayoutPanel1.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
