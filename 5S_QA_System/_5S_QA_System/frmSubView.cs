using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using _5S_QA_DL.APIContext;
using _5S_QA_Entities.Dtos;
using _5S_QA_Entities.Enums;
using _5S_QA_Entities.Models;
using _5S_QA_System.Controls;
using _5S_QA_System.View.User_control;
using MetroFramework.Forms;

namespace _5S_QA_System;

public class frmSubView : MetroForm
{
	private readonly Form mForm;

	public FormType mFrmType;

	private Guid mId;

	private bool isEdit;

	private int mRow;

	private int mCol;

	public bool isClose;

	private IContainer components = null;

	private ContextMenuStrip contextMenuStripMain;

	private ToolStripMenuItem main_refreshToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripMenuItem main_newToolStripMenuItem;

	private ToolStripMenuItem main_editToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator6;

	private ToolStripMenuItem main_deleteToolStripMenuItem;

	private StatusStrip statusStripfrmMain;

	private ToolStripProgressBar sprogbarStatus;

	public ToolStripStatusLabel slblStatus;

	private ToolTip toolTipMain;

	private DataGridView dgvMain;

	private mPanelOther mPanelViewMain;

	private DataGridViewTextBoxColumn No;

	private DataGridViewTextBoxColumn Code;

	private DataGridViewTextBoxColumn name;

	private DataGridViewTextBoxColumn Description;

	private DataGridViewTextBoxColumn Value;

	private DataGridViewTextBoxColumn ParentId;

	private DataGridViewTextBoxColumn ParentCode;

	private DataGridViewTextBoxColumn ParentName;

	private DataGridViewTextBoxColumn TypeId;

	private DataGridViewTextBoxColumn TypeCode;

	private DataGridViewTextBoxColumn TypeName;

	private DataGridViewTextBoxColumn Id;

	private new DataGridViewTextBoxColumn Created;

	private DataGridViewTextBoxColumn Modified;

	private DataGridViewTextBoxColumn CreatedBy;

	private DataGridViewTextBoxColumn ModifiedBy;

	private DataGridViewTextBoxColumn IsActivated;

	public frmSubView(Form frm, FormType frmType)
	{
		InitializeComponent();
		Common.setControls(this, toolTipMain, new List<ContextMenuStrip> { contextMenuStripMain });
		mForm = frm;
		mFrmType = frmType;
	}

	private void frmSubView_Load(object sender, EventArgs e)
	{
		Init();
	}

	private void frmSubView_Shown(object sender, EventArgs e)
	{
		load_AllData();
	}

	private void frmSubView_FormClosing(object sender, FormClosingEventArgs e)
	{
		GC.Collect();
	}

	private void frmSubView_FormClosed(object sender, FormClosedEventArgs e)
	{
		if (isClose)
		{
			return;
		}
		if (mForm is frmStaffAdd frmStaffAdd2)
		{
			frmStaffAdd2.load_cbbDepartment();
		}
		else if (mForm is frmProductAdd frmProductAdd2)
		{
			frmProductAdd2.load_cbbDepartment();
		}
		else if (mForm is frmMachineAdd frmMachineAdd2)
		{
			switch (mFrmType)
			{
			case (FormType)6:
				frmMachineAdd2.load_cbbMacType();
				break;
			case (FormType)5:
				frmMachineAdd2.load_cbbFactory();
				break;
			}
		}
		else if (mForm is frmMeasurementAdd frmMeasurementAdd2)
		{
			switch (mFrmType)
			{
			case (FormType)6:
				frmMeasurementAdd2.load_cbbMacType();
				break;
			case (FormType)7:
				frmMeasurementAdd2.load_cbbImportant();
				break;
			case (FormType)8:
				frmMeasurementAdd2.load_cbbUnit();
				break;
			}
		}
		else if (mForm is frmPlanAdd frmPlanAdd2)
		{
			frmPlanAdd2.load_cbbStage();
		}
		else if (mForm is frmRequestAdd frmRequestAdd2)
		{
			switch (mFrmType)
			{
			case (FormType)10:
				frmRequestAdd2.load_cbbType();
				break;
			case (FormType)11:
				frmRequestAdd2.load_cbbLine();
				break;
			}
		}
		else if (mForm is frmCalibrationAdd frmCalibrationAdd2)
		{
			frmCalibrationAdd2.load_cbbType();
		}
	}

	private void Init()
	{
		if (mFrmType == (FormType)6)
		{
			dgvMain.Columns["value"].Visible = true;
		}
		mPanelViewMain.Visible = false;
		isClose = true;
		Text = Common.getTextLanguage(this, mFrmType.ToString()) ?? "";
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
			QueryArgs queryArgs = new QueryArgs
			{
				Predicate = "TypeId=@0",
				Order = "Created DESC",
				Page = 1,
				Limit = int.MaxValue
			};
			switch (mFrmType)
			{
			case (FormType)4:
				queryArgs.PredicateParameters = new string[1] { "55630EBA-6A11-4001-B161-9AE77ACCA43D" };
				break;
			case (FormType)5:
				queryArgs.PredicateParameters = new string[1] { "5EB07BDB-9086-4BC5-A02B-5D6E9CFCD476" };
				break;
			case (FormType)6:
				queryArgs.PredicateParameters = new string[1] { "438D7052-25F3-4342-ED0C-08D7E9C5C77D" };
				break;
			case (FormType)7:
				queryArgs.PredicateParameters = new string[1] { "6042BF53-9411-47D4-9BD6-F8AB7BABB663" };
				break;
			case (FormType)8:
				queryArgs.PredicateParameters = new string[1] { "7CA6130A-00D1-40CE-ED0F-08D7E9C5C77D" };
				break;
			case (FormType)9:
				queryArgs.PredicateParameters = new string[1] { "11C5FD56-AD45-4457-8DC9-6C8D9F6673A1" };
				break;
			case (FormType)10:
				queryArgs.PredicateParameters = new string[1] { "AC5FA813-C9EE-4805-A850-30A5EA5AB0A1" };
				break;
			case (FormType)11:
				queryArgs.PredicateParameters = new string[1] { "AC5FA814-C9EE-4807-A851-30A5EA5AB0A2" };
				break;
			case (FormType)12:
				queryArgs.PredicateParameters = new string[1] { "AC5FA815-C9EE-4807-A852-30A5EA5AB0A3" };
				break;
			}
			ResponseDto result = frmLogin.client.GetsAsync(queryArgs, "/api/MetadataValue/Gets").Result;
			dgvMain.DataSource = Common.getDataTable<MetadataValueViewModel>(result);
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
			Guid guid = Guid.Parse(dataGridView.CurrentRow.Cells["Id"].Value.ToString());
			if (!guid.Equals(mId))
			{
				mId = guid;
				mPanelViewMain.load_dgvContent((Enum)FormType.VIEW);
			}
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
		if (frmLogin.User.Role.Equals(RoleWeb.Administrator) && frmLogin.User.JobTitle != "Manager" && frmLogin.User.JobTitle != "Supervisor")
		{
			MessageBox.Show(Common.getTextLanguage(typeof(frmLogin).Name, "wNoAuthorization"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		mPanelViewMain.Visible = true;
		mPanelViewMain.load_dgvContent((Enum)FormType.ADD);
	}

	private void main_editToolStripMenuItem_Click(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		if (mId.Equals(Guid.Empty))
		{
			MessageBox.Show(Common.getTextLanguage(this, "wSelectRow"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		if (frmLogin.User.Role.Equals(RoleWeb.Administrator) && frmLogin.User.JobTitle != "Manager" && frmLogin.User.JobTitle != "Supervisor")
		{
			MessageBox.Show(Common.getTextLanguage(typeof(frmLogin).Name, "wNoAuthorization"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		mPanelViewMain.Visible = true;
		mPanelViewMain.load_dgvContent((Enum)FormType.EDIT);
	}

	private void main_deleteToolStripMenuItem_Click(object sender, EventArgs e)
	{
		if (mId.Equals(Guid.Empty))
		{
			MessageBox.Show(Common.getTextLanguage(this, "wSelectRow"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
		else if (frmLogin.User.Role.Equals(RoleWeb.Administrator) && frmLogin.User.JobTitle != "Manager" && frmLogin.User.JobTitle != "Supervisor")
		{
			MessageBox.Show(Common.getTextLanguage(typeof(frmLogin).Name, "wNoAuthorization"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
		else
		{
			if (!MessageBox.Show(string.Format("{0} {1}", Common.getTextLanguage(this, "wSureDelete"), dgvMain.CurrentRow.Cells["Name"].Value), Common.getTextLanguage(this, "QUESTION"), MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.Yes))
			{
				return;
			}
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				start_Proccessor();
				ResponseDto result = frmLogin.client.DeleteAsync(mId, "/api/MetadataValue/Delete/{id}").Result;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_5S_QA_System.frmSubView));
		this.contextMenuStripMain = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.main_refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.main_newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.main_editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.main_deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.statusStripfrmMain = new System.Windows.Forms.StatusStrip();
		this.sprogbarStatus = new System.Windows.Forms.ToolStripProgressBar();
		this.slblStatus = new System.Windows.Forms.ToolStripStatusLabel();
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.dgvMain = new System.Windows.Forms.DataGridView();
		this.mPanelViewMain = new _5S_QA_System.View.User_control.mPanelOther();
		this.No = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ParentId = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ParentCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ParentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.TypeId = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.TypeCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.TypeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Created = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Modified = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.CreatedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ModifiedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.IsActivated = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.contextMenuStripMain.SuspendLayout();
		this.statusStripfrmMain.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvMain).BeginInit();
		base.SuspendLayout();
		this.contextMenuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.main_refreshToolStripMenuItem, this.toolStripSeparator1, this.main_newToolStripMenuItem, this.main_editToolStripMenuItem, this.toolStripSeparator6, this.main_deleteToolStripMenuItem });
		this.contextMenuStripMain.Name = "contextMenuStripStaff";
		this.contextMenuStripMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
		this.contextMenuStripMain.Size = new System.Drawing.Size(114, 104);
		this.main_refreshToolStripMenuItem.Name = "main_refreshToolStripMenuItem";
		this.main_refreshToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
		this.main_refreshToolStripMenuItem.Text = "Refresh";
		this.main_refreshToolStripMenuItem.Click += new System.EventHandler(main_refreshToolStripMenuItem_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(110, 6);
		this.main_newToolStripMenuItem.Name = "main_newToolStripMenuItem";
		this.main_newToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
		this.main_newToolStripMenuItem.Text = "New";
		this.main_newToolStripMenuItem.Click += new System.EventHandler(main_newToolStripMenuItem_Click);
		this.main_editToolStripMenuItem.Name = "main_editToolStripMenuItem";
		this.main_editToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
		this.main_editToolStripMenuItem.Text = "Edit";
		this.main_editToolStripMenuItem.Click += new System.EventHandler(main_editToolStripMenuItem_Click);
		this.toolStripSeparator6.Name = "toolStripSeparator6";
		this.toolStripSeparator6.Size = new System.Drawing.Size(110, 6);
		this.main_deleteToolStripMenuItem.Name = "main_deleteToolStripMenuItem";
		this.main_deleteToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
		this.main_deleteToolStripMenuItem.Text = "Delete";
		this.main_deleteToolStripMenuItem.Click += new System.EventHandler(main_deleteToolStripMenuItem_Click);
		this.statusStripfrmMain.BackColor = System.Drawing.SystemColors.ControlLight;
		this.statusStripfrmMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.statusStripfrmMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.sprogbarStatus, this.slblStatus });
		this.statusStripfrmMain.Location = new System.Drawing.Point(20, 354);
		this.statusStripfrmMain.Name = "statusStripfrmMain";
		this.statusStripfrmMain.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
		this.statusStripfrmMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
		this.statusStripfrmMain.Size = new System.Drawing.Size(540, 26);
		this.statusStripfrmMain.SizingGrip = false;
		this.statusStripfrmMain.Stretch = false;
		this.statusStripfrmMain.TabIndex = 6;
		this.sprogbarStatus.Name = "sprogbarStatus";
		this.sprogbarStatus.Size = new System.Drawing.Size(100, 20);
		this.slblStatus.Name = "slblStatus";
		this.slblStatus.Size = new System.Drawing.Size(16, 21);
		this.slblStatus.Text = "...";
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
		this.dgvMain.Columns.AddRange(this.No, this.Code, this.name, this.Description, this.Value, this.ParentId, this.ParentCode, this.ParentName, this.TypeId, this.TypeCode, this.TypeName, this.Id, this.Created, this.Modified, this.CreatedBy, this.ModifiedBy, this.IsActivated);
		this.dgvMain.ContextMenuStrip = this.contextMenuStripMain;
		this.dgvMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.dgvMain.EnableHeadersVisualStyles = false;
		this.dgvMain.Location = new System.Drawing.Point(20, 70);
		this.dgvMain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.dgvMain.MultiSelect = false;
		this.dgvMain.Name = "dgvMain";
		this.dgvMain.ReadOnly = true;
		this.dgvMain.RowHeadersWidth = 25;
		this.dgvMain.Size = new System.Drawing.Size(540, 284);
		this.dgvMain.TabIndex = 2;
		this.dgvMain.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvMain_CellClick);
		this.dgvMain.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvMain_CellDoubleClick);
		this.dgvMain.CurrentCellChanged += new System.EventHandler(dgvMain_CurrentCellChanged);
		this.dgvMain.Sorted += new System.EventHandler(dgvMain_Sorted);
		this.mPanelViewMain.AutoScroll = true;
		this.mPanelViewMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.mPanelViewMain.Dock = System.Windows.Forms.DockStyle.Right;
		this.mPanelViewMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.mPanelViewMain.Location = new System.Drawing.Point(310, 70);
		this.mPanelViewMain.Margin = new System.Windows.Forms.Padding(5);
		this.mPanelViewMain.Name = "mPanelViewMain";
		this.mPanelViewMain.Size = new System.Drawing.Size(250, 284);
		this.mPanelViewMain.TabIndex = 3;
		this.No.DataPropertyName = "No";
		dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		this.No.DefaultCellStyle = dataGridViewCellStyle3;
		this.No.HeaderText = "No.";
		this.No.Name = "No";
		this.No.ReadOnly = true;
		this.No.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.No.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.No.Width = 40;
		this.Code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Code.DataPropertyName = "Code";
		this.Code.FillWeight = 25f;
		this.Code.HeaderText = "Code";
		this.Code.Name = "Code";
		this.Code.ReadOnly = true;
		this.name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.name.DataPropertyName = "Name";
		this.name.FillWeight = 35f;
		this.name.HeaderText = "Name";
		this.name.Name = "name";
		this.name.ReadOnly = true;
		this.Description.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Description.DataPropertyName = "Description";
		this.Description.FillWeight = 65f;
		this.Description.HeaderText = "Description";
		this.Description.Name = "Description";
		this.Description.ReadOnly = true;
		this.Value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Value.DataPropertyName = "Value";
		this.Value.FillWeight = 30f;
		this.Value.HeaderText = "Value";
		this.Value.Name = "Value";
		this.Value.ReadOnly = true;
		this.Value.Visible = false;
		this.ParentId.DataPropertyName = "ParentId";
		this.ParentId.HeaderText = "Parent id";
		this.ParentId.Name = "ParentId";
		this.ParentId.ReadOnly = true;
		this.ParentId.Visible = false;
		this.ParentCode.DataPropertyName = "ParentCode";
		this.ParentCode.HeaderText = "Parent code";
		this.ParentCode.Name = "ParentCode";
		this.ParentCode.ReadOnly = true;
		this.ParentCode.Visible = false;
		this.ParentName.DataPropertyName = "ParentName";
		this.ParentName.HeaderText = "Parent name";
		this.ParentName.Name = "ParentName";
		this.ParentName.ReadOnly = true;
		this.ParentName.Visible = false;
		this.TypeId.DataPropertyName = "TypeId";
		this.TypeId.HeaderText = "Type id";
		this.TypeId.Name = "TypeId";
		this.TypeId.ReadOnly = true;
		this.TypeId.Visible = false;
		this.TypeCode.DataPropertyName = "TypeCode";
		this.TypeCode.HeaderText = "Type code";
		this.TypeCode.Name = "TypeCode";
		this.TypeCode.ReadOnly = true;
		this.TypeCode.Visible = false;
		this.TypeName.DataPropertyName = "TypeName";
		this.TypeName.HeaderText = "Type name";
		this.TypeName.Name = "TypeName";
		this.TypeName.ReadOnly = true;
		this.TypeName.Visible = false;
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
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(580, 400);
		base.Controls.Add(this.mPanelViewMain);
		base.Controls.Add(this.dgvMain);
		base.Controls.Add(this.statusStripfrmMain);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "frmSubView";
		base.Padding = new System.Windows.Forms.Padding(20, 70, 20, 20);
		this.Text = "5S QA System * SUB";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmSubView_FormClosing);
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmSubView_FormClosed);
		base.Load += new System.EventHandler(frmSubView_Load);
		base.Shown += new System.EventHandler(frmSubView_Shown);
		this.contextMenuStripMain.ResumeLayout(false);
		this.statusStripfrmMain.ResumeLayout(false);
		this.statusStripfrmMain.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvMain).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
