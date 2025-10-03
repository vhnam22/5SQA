using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using _5SQA_Config_UWave.Constants;
using _5SQA_Config_UWave.Funtions;
using _5SQA_Config_UWave.Properties;

namespace _5SQA_Config_UWave;

public class mPanelView : UserControl
{
	private DataGridView dgvMain;

	private IContainer components = null;

	private DataGridViewTextBoxColumn content_title;

	private DataGridView dgvContent;

	private DataGridViewTextBoxColumn content_value;

	private ToolTip toolTipmPanelView;

	private Button btnDown;

	private Button btnUp;

	private Label lblValue;

	private Label lblTittle;

	private Panel panelView;

	private Panel panelResize;

	private Panel panelFooter;

	private Panel panelTitle;

	private TableLayoutPanel tblPanelTitle;

	private DataGridView dgvFooter;

	private DataGridViewTextBoxColumn footer_title;

	private DataGridViewTextBoxColumn footer_value;

	public mPanelView()
	{
		InitializeComponent();
	}

	private void mPanelView_Load(object sender, EventArgs e)
	{
		Init();
	}

	private void Init()
	{
		ControlResize.Init(panelResize, this, ControlResize.Direction.Horizontal, Cursors.SizeWE);
		dgvMain = base.Parent.Controls["dgvMain"] as DataGridView;
	}

	public void Display()
	{
		try
		{
			lblTittle.Text = "VIEW";
			lblValue.Text = dgvMain.CurrentRow.Cells["Id"].Value.ToString();
			dgvContent.Rows.Clear();
			dgvFooter.Rows.Clear();
			foreach (DataGridViewColumn col in dgvMain.Columns)
			{
				if (Constant.dgvContent.Find((string x) => x.Equals(col.Name)) != null)
				{
					dgvContent.Visible = true;
					dgvContent.Rows.Add(col.HeaderText, dgvMain.CurrentRow.Cells[col.Name].Value);
				}
				else if (Constant.dgvFooter.Find((string x) => x.Equals(col.Name)) != null)
				{
					dgvFooter.Visible = true;
					dgvFooter.Rows.Add(col.HeaderText, dgvMain.CurrentRow.Cells[col.Name].Value);
				}
			}
		}
		finally
		{
			dgvContent.Size = new Size(panelView.Width, dgvContent.Rows.Count * 22 + 3);
			dgvFooter.Size = new Size(panelView.Width, dgvFooter.Rows.Count * 22 + 3);
			dgvContent.CurrentCell = null;
			dgvContent.Refresh();
			dgvFooter.CurrentCell = null;
			dgvFooter.Refresh();
			dgvContent.SendToBack();
			dgvFooter.BringToFront();
		}
	}

	private void dgvNormal_Leave(object sender, EventArgs e)
	{
		DataGridView dataGridView = sender as DataGridView;
		dataGridView.CurrentCell = null;
	}

	private void btnUp_Click(object sender, EventArgs e)
	{
		dgvMain.Select();
		SendKeys.SendWait("{up}");
	}

	private void btnDown_Click(object sender, EventArgs e)
	{
		dgvMain.Select();
		SendKeys.SendWait("{down}");
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
		this.content_title = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dgvContent = new System.Windows.Forms.DataGridView();
		this.content_value = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.toolTipmPanelView = new System.Windows.Forms.ToolTip(this.components);
		this.btnDown = new System.Windows.Forms.Button();
		this.btnUp = new System.Windows.Forms.Button();
		this.lblValue = new System.Windows.Forms.Label();
		this.lblTittle = new System.Windows.Forms.Label();
		this.panelView = new System.Windows.Forms.Panel();
		this.panelResize = new System.Windows.Forms.Panel();
		this.panelFooter = new System.Windows.Forms.Panel();
		this.panelTitle = new System.Windows.Forms.Panel();
		this.tblPanelTitle = new System.Windows.Forms.TableLayoutPanel();
		this.dgvFooter = new System.Windows.Forms.DataGridView();
		this.footer_title = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.footer_value = new System.Windows.Forms.DataGridViewTextBoxColumn();
		((System.ComponentModel.ISupportInitialize)this.dgvContent).BeginInit();
		this.panelView.SuspendLayout();
		this.tblPanelTitle.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvFooter).BeginInit();
		base.SuspendLayout();
		dataGridViewCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.content_title.DefaultCellStyle = dataGridViewCellStyle;
		this.content_title.HeaderText = "Title";
		this.content_title.Name = "content_title";
		this.content_title.ReadOnly = true;
		this.content_title.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.content_title.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.content_title.Width = 140;
		this.dgvContent.AllowUserToAddRows = false;
		this.dgvContent.AllowUserToDeleteRows = false;
		this.dgvContent.AllowUserToResizeColumns = false;
		this.dgvContent.AllowUserToResizeRows = false;
		this.dgvContent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		this.dgvContent.ColumnHeadersVisible = false;
		this.dgvContent.Columns.AddRange(this.content_title, this.content_value);
		this.dgvContent.Dock = System.Windows.Forms.DockStyle.Top;
		this.dgvContent.Location = new System.Drawing.Point(0, 0);
		this.dgvContent.Margin = new System.Windows.Forms.Padding(1);
		this.dgvContent.Name = "dgvContent";
		this.dgvContent.ReadOnly = true;
		this.dgvContent.RowHeadersVisible = false;
		this.dgvContent.ScrollBars = System.Windows.Forms.ScrollBars.None;
		this.dgvContent.Size = new System.Drawing.Size(596, 22);
		this.dgvContent.TabIndex = 1;
		this.dgvContent.Visible = false;
		this.dgvContent.Leave += new System.EventHandler(dgvNormal_Leave);
		this.content_value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		this.content_value.DefaultCellStyle = dataGridViewCellStyle2;
		this.content_value.HeaderText = "Value";
		this.content_value.Name = "content_value";
		this.content_value.ReadOnly = true;
		this.content_value.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.content_value.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.btnDown.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnDown.BackColor = System.Drawing.Color.Transparent;
		this.btnDown.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnDown.FlatAppearance.BorderSize = 0;
		this.btnDown.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
		this.btnDown.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
		this.btnDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDown.ForeColor = System.Drawing.Color.White;
		this.btnDown.Image = _5SQA_Config_UWave.Properties.Resources.down_arrow;
		this.btnDown.Location = new System.Drawing.Point(574, 1);
		this.btnDown.Margin = new System.Windows.Forms.Padding(1);
		this.btnDown.Name = "btnDown";
		this.btnDown.Size = new System.Drawing.Size(25, 25);
		this.btnDown.TabIndex = 129;
		this.btnDown.TabStop = false;
		this.toolTipmPanelView.SetToolTip(this.btnDown, "Display lower row item");
		this.btnDown.UseVisualStyleBackColor = false;
		this.btnDown.Click += new System.EventHandler(btnDown_Click);
		this.btnUp.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnUp.BackColor = System.Drawing.Color.Transparent;
		this.btnUp.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnUp.FlatAppearance.BorderSize = 0;
		this.btnUp.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
		this.btnUp.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
		this.btnUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUp.ForeColor = System.Drawing.Color.White;
		this.btnUp.Image = _5SQA_Config_UWave.Properties.Resources.up_arrow;
		this.btnUp.Location = new System.Drawing.Point(547, 1);
		this.btnUp.Margin = new System.Windows.Forms.Padding(1);
		this.btnUp.Name = "btnUp";
		this.btnUp.Size = new System.Drawing.Size(25, 25);
		this.btnUp.TabIndex = 128;
		this.btnUp.TabStop = false;
		this.toolTipmPanelView.SetToolTip(this.btnUp, "Display upper row item");
		this.btnUp.UseVisualStyleBackColor = false;
		this.btnUp.Click += new System.EventHandler(btnUp_Click);
		this.lblValue.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblValue.ForeColor = System.Drawing.Color.Crimson;
		this.lblValue.Location = new System.Drawing.Point(46, 1);
		this.lblValue.Margin = new System.Windows.Forms.Padding(1);
		this.lblValue.Name = "lblValue";
		this.lblValue.Size = new System.Drawing.Size(499, 25);
		this.lblValue.TabIndex = 131;
		this.lblValue.Text = "...";
		this.lblValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblTittle.AutoSize = true;
		this.lblTittle.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTittle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTittle.Location = new System.Drawing.Point(1, 1);
		this.lblTittle.Margin = new System.Windows.Forms.Padding(1);
		this.lblTittle.Name = "lblTittle";
		this.lblTittle.Size = new System.Drawing.Size(43, 25);
		this.lblTittle.TabIndex = 0;
		this.lblTittle.Text = "Title:";
		this.lblTittle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.panelView.AutoScroll = true;
		this.panelView.Controls.Add(this.dgvFooter);
		this.panelView.Controls.Add(this.dgvContent);
		this.panelView.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panelView.Location = new System.Drawing.Point(4, 28);
		this.panelView.Margin = new System.Windows.Forms.Padding(0);
		this.panelView.Name = "panelView";
		this.panelView.Size = new System.Drawing.Size(596, 382);
		this.panelView.TabIndex = 145;
		this.panelResize.Dock = System.Windows.Forms.DockStyle.Left;
		this.panelResize.Location = new System.Drawing.Point(0, 28);
		this.panelResize.Margin = new System.Windows.Forms.Padding(1);
		this.panelResize.Name = "panelResize";
		this.panelResize.Size = new System.Drawing.Size(4, 382);
		this.panelResize.TabIndex = 144;
		this.panelFooter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panelFooter.Location = new System.Drawing.Point(0, 410);
		this.panelFooter.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.panelFooter.Name = "panelFooter";
		this.panelFooter.Size = new System.Drawing.Size(600, 1);
		this.panelFooter.TabIndex = 143;
		this.panelTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panelTitle.Dock = System.Windows.Forms.DockStyle.Top;
		this.panelTitle.Location = new System.Drawing.Point(0, 27);
		this.panelTitle.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.panelTitle.Name = "panelTitle";
		this.panelTitle.Size = new System.Drawing.Size(600, 1);
		this.panelTitle.TabIndex = 142;
		this.tblPanelTitle.AutoSize = true;
		this.tblPanelTitle.ColumnCount = 4;
		this.tblPanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tblPanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tblPanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tblPanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tblPanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tblPanelTitle.Controls.Add(this.btnDown, 3, 0);
		this.tblPanelTitle.Controls.Add(this.lblValue, 1, 0);
		this.tblPanelTitle.Controls.Add(this.btnUp, 2, 0);
		this.tblPanelTitle.Controls.Add(this.lblTittle, 0, 0);
		this.tblPanelTitle.Dock = System.Windows.Forms.DockStyle.Top;
		this.tblPanelTitle.Location = new System.Drawing.Point(0, 0);
		this.tblPanelTitle.Margin = new System.Windows.Forms.Padding(0);
		this.tblPanelTitle.Name = "tblPanelTitle";
		this.tblPanelTitle.RowCount = 1;
		this.tblPanelTitle.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tblPanelTitle.Size = new System.Drawing.Size(600, 27);
		this.tblPanelTitle.TabIndex = 140;
		this.dgvFooter.AllowUserToAddRows = false;
		this.dgvFooter.AllowUserToDeleteRows = false;
		this.dgvFooter.AllowUserToResizeColumns = false;
		this.dgvFooter.AllowUserToResizeRows = false;
		this.dgvFooter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		this.dgvFooter.ColumnHeadersVisible = false;
		this.dgvFooter.Columns.AddRange(this.footer_title, this.footer_value);
		this.dgvFooter.Dock = System.Windows.Forms.DockStyle.Top;
		this.dgvFooter.Location = new System.Drawing.Point(0, 22);
		this.dgvFooter.Margin = new System.Windows.Forms.Padding(1);
		this.dgvFooter.Name = "dgvFooter";
		this.dgvFooter.ReadOnly = true;
		this.dgvFooter.RowHeadersVisible = false;
		this.dgvFooter.ScrollBars = System.Windows.Forms.ScrollBars.None;
		this.dgvFooter.Size = new System.Drawing.Size(596, 22);
		this.dgvFooter.TabIndex = 4;
		this.dgvFooter.Visible = false;
		this.dgvFooter.Leave += new System.EventHandler(dgvNormal_Leave);
		dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.footer_title.DefaultCellStyle = dataGridViewCellStyle3;
		this.footer_title.HeaderText = "Title";
		this.footer_title.Name = "footer_title";
		this.footer_title.ReadOnly = true;
		this.footer_title.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.footer_title.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.footer_title.Width = 140;
		this.footer_value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		this.footer_value.DefaultCellStyle = dataGridViewCellStyle4;
		this.footer_value.HeaderText = "Value";
		this.footer_value.Name = "footer_value";
		this.footer_value.ReadOnly = true;
		this.footer_value.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.footer_value.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		base.Controls.Add(this.panelView);
		base.Controls.Add(this.panelResize);
		base.Controls.Add(this.panelFooter);
		base.Controls.Add(this.panelTitle);
		base.Controls.Add(this.tblPanelTitle);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Margin = new System.Windows.Forms.Padding(0);
		base.Name = "mPanelView";
		base.Size = new System.Drawing.Size(600, 411);
		base.Load += new System.EventHandler(mPanelView_Load);
		((System.ComponentModel.ISupportInitialize)this.dgvContent).EndInit();
		this.panelView.ResumeLayout(false);
		this.tblPanelTitle.ResumeLayout(false);
		this.tblPanelTitle.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvFooter).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
