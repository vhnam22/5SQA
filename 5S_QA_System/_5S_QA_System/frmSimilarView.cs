using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using _5S_QA_DL.APIContext;
using _5S_QA_Entities.Abstracts;
using _5S_QA_Entities.Enums;
using _5S_QA_Entities.Models;
using _5S_QA_System.Controls;
using MetroFramework.Forms;

namespace _5S_QA_System;

public class frmSimilarView : MetroForm
{
	private readonly Form mForm;

	public Guid mIdParent = Guid.Empty;

	private Guid mId;

	private bool isEdit;

	private int mRow;

	private int mCol;

	public bool isClose;

	private IContainer components = null;

	private DataGridView dgvMain;

	private ContextMenuStrip contextMenuStripMain;

	private ToolStripMenuItem main_refreshToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripMenuItem main_newToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator6;

	private ToolStripMenuItem main_deleteToolStripMenuItem;

	private ToolTip toolTipMain;

	public ToolStripStatusLabel slblStatus;

	private ToolStripProgressBar sprogbarStatus;

	private StatusStrip statusStripfrmMain;

	private mPanelSimilar mPanelViewMain;

	private DataGridViewTextBoxColumn No;

	private DataGridViewTextBoxColumn Value;

	private DataGridViewTextBoxColumn ProductId;

	private DataGridViewTextBoxColumn ProductCode;

	private new DataGridViewTextBoxColumn ProductName;

	private DataGridViewTextBoxColumn Id;

	private new DataGridViewTextBoxColumn Created;

	private DataGridViewTextBoxColumn Modified;

	private DataGridViewTextBoxColumn CreatedBy;

	private DataGridViewTextBoxColumn ModifiedBy;

	private DataGridViewTextBoxColumn IsActivated;

	public frmSimilarView(Form frm, Guid id = default(Guid))
	{
		InitializeComponent();
		Common.setControls(this, toolTipMain, new List<ContextMenuStrip> { contextMenuStripMain });
		mForm = frm;
		mIdParent = id;
		Init_Title();
	}

	private void frmSimilarView_Load(object sender, EventArgs e)
	{
		Init();
	}

	private void frmSimilarView_Shown(object sender, EventArgs e)
	{
		load_AllData();
		mPanelViewMain.load_cbbProduct(mIdParent);
	}

	private void frmSimilarView_FormClosing(object sender, FormClosingEventArgs e)
	{
		GC.Collect();
	}

	private void frmSimilarView_FormClosed(object sender, FormClosedEventArgs e)
	{
		if (!isClose)
		{
			((frmProductView)mForm).load_AllData();
		}
	}

	private void Init()
	{
		mPanelViewMain.Visible = false;
		isClose = true;
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

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		if ((msg.Msg.Equals(256) || msg.Msg.Equals(260)) && base.ActiveControl.Name.Equals("dgvMain"))
		{
			if (keyData.Equals(Keys.Return))
			{
				dgvMain_CellDoubleClick(this, null);
				return true;
			}
			if (keyData.Equals(Keys.Escape))
			{
				dgvMain_CellClick(this, null);
				return true;
			}
		}
		return base.ProcessCmdKey(ref msg, keyData);
	}

	public void load_AllData()
	{
		Cursor.Current = Cursors.WaitCursor;
		try
		{
			start_Proccessor();
			isEdit = true;
			ResponseDto result = frmLogin.client.GetsAsync(mIdParent, "/api/Similar/Gets/{id}").Result;
			dgvMain.DataSource = Common.getDataTable<SimilarViewModel>(result);
			dgvMain.Refresh();
			if (dgvMain.CurrentCell == null)
			{
				mPanelViewMain.Visible = false;
			}
			if (isEdit)
			{
				try
				{
					dgvMain.Rows[mRow].Cells[mCol].Selected = true;
				}
				catch
				{
				}
			}
			debugOutput(Common.getTextLanguage(this, "Successful"));
			dgvMain_Sorted(dgvMain, null);
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
		finally
		{
			isEdit = false;
		}
	}

	public void Init_Title()
	{
		DataGridView dataGridView = mForm.Controls["dgvMain"] as DataGridView;
		Text = string.Format("{0} ({1}#{2})", Text, dataGridView.CurrentRow.Cells["Code"].Value, dataGridView.CurrentRow.Cells["Name"].Value);
	}

	private void dgvMain_CurrentCellChanged(object sender, EventArgs e)
	{
		DataGridView dataGridView = sender as DataGridView;
		if (dataGridView.CurrentCell != null)
		{
			if (!isEdit)
			{
				mRow = dataGridView.CurrentCell.RowIndex;
				mCol = dataGridView.CurrentCell.ColumnIndex;
			}
			mId = Guid.Parse(dataGridView.CurrentRow.Cells["Id"].Value.ToString());
			mPanelViewMain.load_dgvContent(FormType.VIEW);
		}
		else
		{
			mId = Guid.Empty;
		}
	}

	private void dgvMain_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		mPanelViewMain.Visible = false;
	}

	private void dgvMain_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
	{
		if (mId.Equals(Guid.Empty))
		{
			mPanelViewMain.Visible = false;
		}
		else
		{
			mPanelViewMain.Visible = true;
		}
	}

	private void dgvMain_Sorted(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		DataGridView dataGridView = sender as DataGridView;
		foreach (DataGridViewRow item in (IEnumerable)dataGridView.Rows)
		{
			item.Cells["No"].Value = item.Index + 1;
		}
	}

	private void main_refreshToolStripMenuItem_Click(object sender, EventArgs e)
	{
		load_AllData();
	}

	private void main_newToolStripMenuItem_Click(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		mPanelViewMain.Visible = true;
		mPanelViewMain.load_dgvContent(FormType.ADD);
	}

	private void main_deleteToolStripMenuItem_Click(object sender, EventArgs e)
	{
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Expected O, but got Unknown
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Expected O, but got Unknown
		if (mId.Equals(Guid.Empty))
		{
			MessageBox.Show(Common.getTextLanguage(this, "wSelectRow"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
		else
		{
			if (!MessageBox.Show(string.Format("{0} {1}", Common.getTextLanguage(this, "wSureDelete"), dgvMain.CurrentRow.Cells["ProductCode"].Value), Common.getTextLanguage(this, "QUESTION"), MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.Yes))
			{
				return;
			}
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				start_Proccessor();
				SimilarViewModel val = new SimilarViewModel();
				((AuditableEntity)val).Id = mIdParent;
				val.ProductId = Guid.Parse(dgvMain.CurrentRow.Cells["ProductId"].Value.ToString());
				SimilarViewModel body = val;
				ResponseDto result = frmLogin.client.SaveAsync(body, "/api/Similar/Delete").Result;
				if (!result.Success)
				{
					throw new Exception(result.Messages.ElementAt(0).Message);
				}
				isClose = false;
				main_refreshToolStripMenuItem.PerformClick();
			}
			catch (Exception ex)
			{
				string textLanguage = Common.getTextLanguage("Error", ex.Message);
				if (ex.InnerException is ApiException { StatusCode: var statusCode })
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
						debugOutput("ERR: " + (string.IsNullOrEmpty(textLanguage) ? ex.Message.Replace("\n", "") : textLanguage));
						MessageBox.Show(string.IsNullOrEmpty(textLanguage) ? ex.Message : textLanguage, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
					}
				}
				else
				{
					debugOutput("ERR: " + (string.IsNullOrEmpty(textLanguage) ? ex.Message.Replace("\n", "") : textLanguage));
					MessageBox.Show(string.IsNullOrEmpty(textLanguage) ? ex.Message : textLanguage, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_5S_QA_System.frmSimilarView));
		this.dgvMain = new System.Windows.Forms.DataGridView();
		this.No = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ProductId = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ProductCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Created = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Modified = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.CreatedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ModifiedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.IsActivated = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.contextMenuStripMain = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.main_refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.main_newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.main_deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.slblStatus = new System.Windows.Forms.ToolStripStatusLabel();
		this.sprogbarStatus = new System.Windows.Forms.ToolStripProgressBar();
		this.statusStripfrmMain = new System.Windows.Forms.StatusStrip();
		this.mPanelViewMain = new _5S_QA_System.mPanelSimilar();
		((System.ComponentModel.ISupportInitialize)this.dgvMain).BeginInit();
		this.contextMenuStripMain.SuspendLayout();
		this.statusStripfrmMain.SuspendLayout();
		base.SuspendLayout();
		this.dgvMain.AllowUserToAddRows = false;
		this.dgvMain.AllowUserToDeleteRows = false;
		this.dgvMain.AllowUserToOrderColumns = true;
		this.dgvMain.AllowUserToResizeRows = false;
		dataGridViewCellStyle.BackColor = System.Drawing.SystemColors.Control;
		this.dgvMain.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle;
		this.dgvMain.BackgroundColor = System.Drawing.SystemColors.Window;
		dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.AppWorkspace;
		dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
		dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
		dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
		dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.dgvMain.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
		this.dgvMain.ColumnHeadersHeight = 26;
		this.dgvMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
		this.dgvMain.Columns.AddRange(this.No, this.Value, this.ProductId, this.ProductCode, this.ProductName, this.Id, this.Created, this.Modified, this.CreatedBy, this.ModifiedBy, this.IsActivated);
		this.dgvMain.ContextMenuStrip = this.contextMenuStripMain;
		this.dgvMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.dgvMain.EnableHeadersVisualStyles = false;
		this.dgvMain.Location = new System.Drawing.Point(20, 70);
		this.dgvMain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.dgvMain.MultiSelect = false;
		this.dgvMain.Name = "dgvMain";
		this.dgvMain.ReadOnly = true;
		this.dgvMain.RowHeadersWidth = 25;
		this.dgvMain.Size = new System.Drawing.Size(610, 284);
		this.dgvMain.TabIndex = 7;
		this.dgvMain.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvMain_CellClick);
		this.dgvMain.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvMain_CellDoubleClick);
		this.dgvMain.CurrentCellChanged += new System.EventHandler(dgvMain_CurrentCellChanged);
		this.dgvMain.Sorted += new System.EventHandler(dgvMain_Sorted);
		this.No.DataPropertyName = "No";
		dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		this.No.DefaultCellStyle = dataGridViewCellStyle3;
		this.No.HeaderText = "No.";
		this.No.Name = "No";
		this.No.ReadOnly = true;
		this.No.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.No.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.No.Width = 40;
		this.Value.DataPropertyName = "Value";
		this.Value.HeaderText = "Value";
		this.Value.Name = "Value";
		this.Value.ReadOnly = true;
		this.Value.Visible = false;
		this.ProductId.DataPropertyName = "ProductId";
		this.ProductId.HeaderText = "Product id";
		this.ProductId.Name = "ProductId";
		this.ProductId.ReadOnly = true;
		this.ProductId.Visible = false;
		this.ProductCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.ProductCode.DataPropertyName = "ProductCode";
		this.ProductCode.FillWeight = 50f;
		this.ProductCode.HeaderText = "Product code";
		this.ProductCode.Name = "ProductCode";
		this.ProductCode.ReadOnly = true;
		this.ProductName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.ProductName.DataPropertyName = "ProductName";
		this.ProductName.FillWeight = 50f;
		this.ProductName.HeaderText = "Product name";
		this.ProductName.Name = "ProductName";
		this.ProductName.ReadOnly = true;
		this.Id.DataPropertyName = "Id";
		this.Id.HeaderText = "Id";
		this.Id.Name = "Id";
		this.Id.ReadOnly = true;
		this.Id.Visible = false;
		this.Created.DataPropertyName = "Created";
		this.Created.HeaderText = "Created";
		this.Created.Name = "Created";
		this.Created.ReadOnly = true;
		this.Created.Visible = false;
		this.Modified.DataPropertyName = "Modified";
		this.Modified.HeaderText = "Modified";
		this.Modified.Name = "Modified";
		this.Modified.ReadOnly = true;
		this.Modified.Visible = false;
		this.CreatedBy.DataPropertyName = "CreatedBy";
		this.CreatedBy.HeaderText = "Created by";
		this.CreatedBy.Name = "CreatedBy";
		this.CreatedBy.ReadOnly = true;
		this.CreatedBy.Visible = false;
		this.ModifiedBy.DataPropertyName = "ModifiedBy";
		this.ModifiedBy.HeaderText = "Modified by";
		this.ModifiedBy.Name = "ModifiedBy";
		this.ModifiedBy.ReadOnly = true;
		this.ModifiedBy.Visible = false;
		this.IsActivated.DataPropertyName = "IsActivated";
		this.IsActivated.HeaderText = "Is activated";
		this.IsActivated.Name = "IsActivated";
		this.IsActivated.ReadOnly = true;
		this.IsActivated.Visible = false;
		this.contextMenuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.main_refreshToolStripMenuItem, this.toolStripSeparator1, this.main_newToolStripMenuItem, this.toolStripSeparator6, this.main_deleteToolStripMenuItem });
		this.contextMenuStripMain.Name = "contextMenuStripStaff";
		this.contextMenuStripMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
		this.contextMenuStripMain.Size = new System.Drawing.Size(181, 104);
		this.main_refreshToolStripMenuItem.Name = "main_refreshToolStripMenuItem";
		this.main_refreshToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
		this.main_refreshToolStripMenuItem.Text = "Refresh";
		this.main_refreshToolStripMenuItem.Click += new System.EventHandler(main_refreshToolStripMenuItem_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
		this.main_newToolStripMenuItem.Name = "main_newToolStripMenuItem";
		this.main_newToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
		this.main_newToolStripMenuItem.Text = "New";
		this.main_newToolStripMenuItem.Click += new System.EventHandler(main_newToolStripMenuItem_Click);
		this.toolStripSeparator6.Name = "toolStripSeparator6";
		this.toolStripSeparator6.Size = new System.Drawing.Size(177, 6);
		this.main_deleteToolStripMenuItem.Name = "main_deleteToolStripMenuItem";
		this.main_deleteToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
		this.main_deleteToolStripMenuItem.Text = "Delete";
		this.main_deleteToolStripMenuItem.Click += new System.EventHandler(main_deleteToolStripMenuItem_Click);
		this.slblStatus.Name = "slblStatus";
		this.slblStatus.Size = new System.Drawing.Size(16, 21);
		this.slblStatus.Text = "...";
		this.sprogbarStatus.Name = "sprogbarStatus";
		this.sprogbarStatus.Size = new System.Drawing.Size(100, 20);
		this.statusStripfrmMain.BackColor = System.Drawing.SystemColors.ControlLight;
		this.statusStripfrmMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.statusStripfrmMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.sprogbarStatus, this.slblStatus });
		this.statusStripfrmMain.Location = new System.Drawing.Point(20, 354);
		this.statusStripfrmMain.Name = "statusStripfrmMain";
		this.statusStripfrmMain.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
		this.statusStripfrmMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
		this.statusStripfrmMain.Size = new System.Drawing.Size(610, 26);
		this.statusStripfrmMain.SizingGrip = false;
		this.statusStripfrmMain.Stretch = false;
		this.statusStripfrmMain.TabIndex = 9;
		this.mPanelViewMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.mPanelViewMain.Dock = System.Windows.Forms.DockStyle.Right;
		this.mPanelViewMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.mPanelViewMain.Location = new System.Drawing.Point(280, 70);
		this.mPanelViewMain.Name = "mPanelViewMain";
		this.mPanelViewMain.Size = new System.Drawing.Size(350, 284);
		this.mPanelViewMain.TabIndex = 10;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(650, 400);
		base.Controls.Add(this.mPanelViewMain);
		base.Controls.Add(this.dgvMain);
		base.Controls.Add(this.statusStripfrmMain);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "frmSimilarView";
		base.Padding = new System.Windows.Forms.Padding(20, 70, 20, 20);
		this.Text = "5S QA System * SIMILAR";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmSimilarView_FormClosing);
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmSimilarView_FormClosed);
		base.Load += new System.EventHandler(frmSimilarView_Load);
		base.Shown += new System.EventHandler(frmSimilarView_Shown);
		((System.ComponentModel.ISupportInitialize)this.dgvMain).EndInit();
		this.contextMenuStripMain.ResumeLayout(false);
		this.statusStripfrmMain.ResumeLayout(false);
		this.statusStripfrmMain.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
