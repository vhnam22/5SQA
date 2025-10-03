using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using _5S_QA_Client.Controls;
using _5S_QA_Client.Properties;
using _5S_QA_DL.APIContext;
using _5S_QA_Entities.Constants;
using _5S_QA_Entities.Enums;

namespace _5S_QA_Client;

public class mPanelMapper : UserControl
{
	private DataGridView mDataGridView;

	private Guid mId;

	private Form mForm;

	private IContainer components = null;

	private DataGridView dgvFooter;

	private DataGridView dgvContent;

	private Panel panelResize;

	private ToolTip toolTipMain;

	private Button btnConfirm;

	private Button btnDown;

	private Button btnUp;

	private Panel panel1;

	private Label lblTittle;

	private TableLayoutPanel tpanelTitle;

	private Label lblValue;

	private DataGridViewTextBoxColumn FooterName;

	private DataGridViewTextBoxColumn FooterTitle;

	private DataGridViewTextBoxColumn FooterValue;

	private DataGridViewTextBoxColumn ContentName;

	private DataGridViewTextBoxColumn ContentTitle;

	private DataGridViewTextBoxColumn ContentValue;

	public mPanelMapper()
	{
		InitializeComponent();
		Common.setControls(this, toolTipMain);
		mId = default(Guid);
	}

	private void mPanelMapper_Load(object sender, EventArgs e)
	{
		ControlResize.Init(panelResize, this, ControlResize.Direction.Horizontal, Cursors.SizeWE);
		mDataGridView = base.Parent.Controls["dgvMain"] as DataGridView;
		mForm = base.ParentForm;
		base.Visible = false;
	}

	public void load_dgvContent(Enum type)
	{
		dgvContent.Rows.Clear();
		dgvFooter.Rows.Clear();
		try
		{
			List<string> list = MetaType.mPanelMapper.Cast<string>().ToList();
			if (mDataGridView.CurrentCell == null)
			{
				return;
			}
			mId = (string.IsNullOrEmpty(mDataGridView.CurrentRow.Cells["Id"].Value.ToString()) ? Guid.Empty : Guid.Parse(mDataGridView.CurrentRow.Cells["Id"].Value.ToString()));
			lblValue.Text = (mId.Equals(Guid.Empty) ? string.Empty : mId.ToString());
			foreach (DataGridViewColumn col in mDataGridView.Columns)
			{
				if (list.Find((string x) => x.Equals(col.Name)) == null)
				{
					continue;
				}
				if (type is FormType formType && formType == FormType.VIEW)
				{
					lblTittle.Text = Common.getTextLanguage(this, "VIEW");
					btnConfirm.Visible = false;
					if (MetaType.dgvFooter.Find((string x) => x.Equals(col.Name)) != null)
					{
						dgvFooter.Rows.Add(col.Name, col.HeaderText, mDataGridView.CurrentRow.Cells[col.Name].Value);
					}
					else
					{
						dgvContent.MultiSelect = true;
						dgvContent.Rows.Add(col.Name, col.HeaderText, mDataGridView.CurrentRow.Cells[col.Name].Value);
						dgvContent.Rows[dgvContent.RowCount - 1].Cells["ContentValue"].ReadOnly = true;
					}
					continue;
				}
				lblTittle.Text = Common.getTextLanguage(this, "EDIT");
				btnConfirm.Visible = true;
				if (MetaType.dgvFooter.Find((string x) => x.Equals(col.Name)) != null)
				{
					dgvFooter.Rows.Add(col.Name, col.HeaderText, mDataGridView.CurrentRow.Cells[col.Name].Value);
					continue;
				}
				dgvContent.MultiSelect = false;
				dgvContent.Rows.Add(col.Name, col.HeaderText, mDataGridView.CurrentRow.Cells[col.Name].Value);
				string name = col.Name;
				string text = name;
				if (!(text == "Code"))
				{
					if (text == "name")
					{
						dgvContent.Rows[dgvContent.RowCount - 1].Cells["ContentValue"].ReadOnly = true;
					}
					else
					{
						dgvContent.Rows[dgvContent.RowCount - 1].Cells["ContentValue"].ReadOnly = false;
					}
				}
				else
				{
					dgvContent.Rows[dgvContent.RowCount - 1].Cells["ContentValue"].ReadOnly = true;
				}
			}
		}
		finally
		{
			dgvContent.Size = new Size(base.Width, dgvContent.Rows.Count * 22 + 3);
			dgvFooter.Size = new Size(base.Width, dgvFooter.Rows.Count * 22 + 3);
			dgvContent.CurrentCell = null;
			dgvContent.Refresh();
			dgvFooter.CurrentCell = null;
			dgvFooter.Refresh();
			dgvFooter.BringToFront();
		}
	}

	private void btnUp_Click(object sender, EventArgs e)
	{
		mDataGridView.Select();
		SendKeys.SendWait("{up}");
	}

	private void btnDown_Click(object sender, EventArgs e)
	{
		mDataGridView.Select();
		SendKeys.SendWait("{down}");
	}

	private void dgvContent_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
	{
		DataGridViewTextBoxEditingControl dataGridViewTextBoxEditingControl = e.Control as DataGridViewTextBoxEditingControl;
		dataGridViewTextBoxEditingControl.AutoCompleteMode = AutoCompleteMode.Suggest;
		dataGridViewTextBoxEditingControl.AutoCompleteSource = AutoCompleteSource.CustomSource;
		dataGridViewTextBoxEditingControl.AutoCompleteCustomSource = Common.getAutoComplete((DataTable)mDataGridView.DataSource, "Mapper");
	}

	private void dgvContent_CellEndEdit(object sender, DataGridViewCellEventArgs e)
	{
		DataGridView dataGridView = sender as DataGridView;
		if (dataGridView.CurrentCell.Value != null && !string.IsNullOrEmpty(dataGridView.CurrentCell.Value.ToString()))
		{
			dataGridView.CurrentCell.Value = Common.trimSpace(dataGridView.CurrentCell.Value.ToString());
		}
	}

	private void btnConfirm_Click(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		try
		{
			frmMapperView frmMapperView2 = mForm as frmMapperView;
			if (Common.addMappers(frmMapperView2, frmMapperView2.mIdParent, mDataGridView, dgvContent))
			{
				frmMapperView2.isClose = false;
				frmMapperView2.main_refreshToolStripMenuItem_Click(null, null);
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
						mForm.Close();
					}
				}
				else
				{
					MessageBox.Show(ex2.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			else
			{
				MessageBox.Show(ex.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
	}

	private void dgvNormal_Leave(object sender, EventArgs e)
	{
		DataGridView dataGridView = sender as DataGridView;
		dataGridView.CurrentCell = null;
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
		this.dgvFooter = new System.Windows.Forms.DataGridView();
		this.FooterName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.FooterTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.FooterValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dgvContent = new System.Windows.Forms.DataGridView();
		this.ContentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ContentTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ContentValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.panelResize = new System.Windows.Forms.Panel();
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.btnConfirm = new System.Windows.Forms.Button();
		this.panel1 = new System.Windows.Forms.Panel();
		this.lblTittle = new System.Windows.Forms.Label();
		this.tpanelTitle = new System.Windows.Forms.TableLayoutPanel();
		this.lblValue = new System.Windows.Forms.Label();
		this.btnDown = new System.Windows.Forms.Button();
		this.btnUp = new System.Windows.Forms.Button();
		((System.ComponentModel.ISupportInitialize)this.dgvFooter).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.dgvContent).BeginInit();
		this.tpanelTitle.SuspendLayout();
		base.SuspendLayout();
		this.dgvFooter.AllowUserToAddRows = false;
		this.dgvFooter.AllowUserToDeleteRows = false;
		this.dgvFooter.AllowUserToResizeColumns = false;
		this.dgvFooter.AllowUserToResizeRows = false;
		this.dgvFooter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		this.dgvFooter.ColumnHeadersVisible = false;
		this.dgvFooter.Columns.AddRange(this.FooterName, this.FooterTitle, this.FooterValue);
		this.dgvFooter.Dock = System.Windows.Forms.DockStyle.Top;
		this.dgvFooter.Location = new System.Drawing.Point(4, 55);
		this.dgvFooter.Margin = new System.Windows.Forms.Padding(1);
		this.dgvFooter.Name = "dgvFooter";
		this.dgvFooter.ReadOnly = true;
		this.dgvFooter.RowHeadersVisible = false;
		this.dgvFooter.ScrollBars = System.Windows.Forms.ScrollBars.None;
		this.dgvFooter.Size = new System.Drawing.Size(396, 22);
		this.dgvFooter.TabIndex = 2;
		this.dgvFooter.Leave += new System.EventHandler(dgvNormal_Leave);
		this.FooterName.HeaderText = "Name";
		this.FooterName.Name = "FooterName";
		this.FooterName.ReadOnly = true;
		this.FooterName.Visible = false;
		dataGridViewCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.FooterTitle.DefaultCellStyle = dataGridViewCellStyle;
		this.FooterTitle.HeaderText = "Title";
		this.FooterTitle.Name = "FooterTitle";
		this.FooterTitle.ReadOnly = true;
		this.FooterTitle.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.FooterTitle.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.FooterTitle.Width = 120;
		this.FooterValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		this.FooterValue.DefaultCellStyle = dataGridViewCellStyle2;
		this.FooterValue.HeaderText = "Value";
		this.FooterValue.Name = "FooterValue";
		this.FooterValue.ReadOnly = true;
		this.FooterValue.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.FooterValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.dgvContent.AllowUserToAddRows = false;
		this.dgvContent.AllowUserToDeleteRows = false;
		this.dgvContent.AllowUserToResizeColumns = false;
		this.dgvContent.AllowUserToResizeRows = false;
		this.dgvContent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		this.dgvContent.ColumnHeadersVisible = false;
		this.dgvContent.Columns.AddRange(this.ContentName, this.ContentTitle, this.ContentValue);
		this.dgvContent.Dock = System.Windows.Forms.DockStyle.Top;
		this.dgvContent.Location = new System.Drawing.Point(4, 33);
		this.dgvContent.Margin = new System.Windows.Forms.Padding(1);
		this.dgvContent.Name = "dgvContent";
		this.dgvContent.RowHeadersVisible = false;
		this.dgvContent.Size = new System.Drawing.Size(396, 22);
		this.dgvContent.TabIndex = 1;
		this.dgvContent.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(dgvContent_CellEndEdit);
		this.dgvContent.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(dgvContent_EditingControlShowing);
		this.ContentName.HeaderText = "Name";
		this.ContentName.Name = "ContentName";
		this.ContentName.Visible = false;
		dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.ContentTitle.DefaultCellStyle = dataGridViewCellStyle3;
		this.ContentTitle.HeaderText = "Tittle";
		this.ContentTitle.Name = "ContentTitle";
		this.ContentTitle.ReadOnly = true;
		this.ContentTitle.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.ContentTitle.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.ContentTitle.Width = 120;
		this.ContentValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		this.ContentValue.DefaultCellStyle = dataGridViewCellStyle4;
		this.ContentValue.FillWeight = 70f;
		this.ContentValue.HeaderText = "Value";
		this.ContentValue.Name = "ContentValue";
		this.ContentValue.ReadOnly = true;
		this.ContentValue.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.ContentValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.panelResize.Dock = System.Windows.Forms.DockStyle.Left;
		this.panelResize.Location = new System.Drawing.Point(0, 33);
		this.panelResize.Margin = new System.Windows.Forms.Padding(5);
		this.panelResize.Name = "panelResize";
		this.panelResize.Padding = new System.Windows.Forms.Padding(1);
		this.panelResize.Size = new System.Drawing.Size(4, 279);
		this.panelResize.TabIndex = 145;
		this.btnConfirm.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnConfirm.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.btnConfirm.FlatAppearance.BorderSize = 0;
		this.btnConfirm.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
		this.btnConfirm.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
		this.btnConfirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnConfirm.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnConfirm.Location = new System.Drawing.Point(0, 312);
		this.btnConfirm.Margin = new System.Windows.Forms.Padding(1);
		this.btnConfirm.Name = "btnConfirm";
		this.btnConfirm.Size = new System.Drawing.Size(400, 26);
		this.btnConfirm.TabIndex = 3;
		this.btnConfirm.Text = "Confirm";
		this.toolTipMain.SetToolTip(this.btnConfirm, "Confirm");
		this.btnConfirm.UseVisualStyleBackColor = true;
		this.btnConfirm.Click += new System.EventHandler(btnConfirm_Click);
		this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 32);
		this.panel1.Margin = new System.Windows.Forms.Padding(0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(400, 1);
		this.panel1.TabIndex = 143;
		this.lblTittle.AutoSize = true;
		this.lblTittle.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTittle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTittle.Location = new System.Drawing.Point(1, 1);
		this.lblTittle.Margin = new System.Windows.Forms.Padding(1);
		this.lblTittle.Name = "lblTittle";
		this.lblTittle.Size = new System.Drawing.Size(45, 30);
		this.lblTittle.TabIndex = 0;
		this.lblTittle.Text = "VIEW";
		this.lblTittle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.tpanelTitle.AutoSize = true;
		this.tpanelTitle.ColumnCount = 4;
		this.tpanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tpanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelTitle.Controls.Add(this.lblValue, 1, 0);
		this.tpanelTitle.Controls.Add(this.btnDown, 3, 0);
		this.tpanelTitle.Controls.Add(this.btnUp, 2, 0);
		this.tpanelTitle.Controls.Add(this.lblTittle, 0, 0);
		this.tpanelTitle.Dock = System.Windows.Forms.DockStyle.Top;
		this.tpanelTitle.Location = new System.Drawing.Point(0, 0);
		this.tpanelTitle.Margin = new System.Windows.Forms.Padding(0);
		this.tpanelTitle.Name = "tpanelTitle";
		this.tpanelTitle.RowCount = 1;
		this.tpanelTitle.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tpanelTitle.Size = new System.Drawing.Size(400, 32);
		this.tpanelTitle.TabIndex = 142;
		this.lblValue.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblValue.ForeColor = System.Drawing.Color.Red;
		this.lblValue.Location = new System.Drawing.Point(48, 1);
		this.lblValue.Margin = new System.Windows.Forms.Padding(1);
		this.lblValue.Name = "lblValue";
		this.lblValue.Size = new System.Drawing.Size(303, 30);
		this.lblValue.TabIndex = 133;
		this.lblValue.Text = "...";
		this.lblValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnDown.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnDown.BackColor = System.Drawing.Color.Transparent;
		this.btnDown.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnDown.FlatAppearance.BorderSize = 0;
		this.btnDown.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.AppWorkspace;
		this.btnDown.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
		this.btnDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDown.ForeColor = System.Drawing.Color.White;
		this.btnDown.Image = _5S_QA_Client.Properties.Resources.arrow_down;
		this.btnDown.Location = new System.Drawing.Point(377, 1);
		this.btnDown.Margin = new System.Windows.Forms.Padding(1);
		this.btnDown.Name = "btnDown";
		this.btnDown.Size = new System.Drawing.Size(22, 30);
		this.btnDown.TabIndex = 129;
		this.btnDown.TabStop = false;
		this.toolTipMain.SetToolTip(this.btnDown, "Display lower row item");
		this.btnDown.UseVisualStyleBackColor = false;
		this.btnDown.Click += new System.EventHandler(btnDown_Click);
		this.btnUp.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnUp.BackColor = System.Drawing.Color.Transparent;
		this.btnUp.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnUp.FlatAppearance.BorderSize = 0;
		this.btnUp.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.AppWorkspace;
		this.btnUp.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
		this.btnUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUp.ForeColor = System.Drawing.Color.White;
		this.btnUp.Image = _5S_QA_Client.Properties.Resources.arrow_up;
		this.btnUp.Location = new System.Drawing.Point(353, 1);
		this.btnUp.Margin = new System.Windows.Forms.Padding(1);
		this.btnUp.Name = "btnUp";
		this.btnUp.Size = new System.Drawing.Size(22, 30);
		this.btnUp.TabIndex = 128;
		this.btnUp.TabStop = false;
		this.toolTipMain.SetToolTip(this.btnUp, "Display upper row item");
		this.btnUp.UseVisualStyleBackColor = false;
		this.btnUp.Click += new System.EventHandler(btnUp_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		base.Controls.Add(this.dgvFooter);
		base.Controls.Add(this.dgvContent);
		base.Controls.Add(this.panelResize);
		base.Controls.Add(this.btnConfirm);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.tpanelTitle);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "mPanelMapper";
		base.Size = new System.Drawing.Size(400, 338);
		base.Load += new System.EventHandler(mPanelMapper_Load);
		((System.ComponentModel.ISupportInitialize)this.dgvFooter).EndInit();
		((System.ComponentModel.ISupportInitialize)this.dgvContent).EndInit();
		this.tpanelTitle.ResumeLayout(false);
		this.tpanelTitle.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
