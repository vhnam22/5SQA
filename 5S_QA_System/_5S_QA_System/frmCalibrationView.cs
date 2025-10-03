using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using _5S_QA_DL.APIContext;
using _5S_QA_Entities.Constants;
using _5S_QA_Entities.Dtos;
using _5S_QA_Entities.Models;
using _5S_QA_System.Controls;
using _5S_QA_System.Properties;
using _5S_QA_System.View.User_control;
using MetroFramework.Forms;

namespace _5S_QA_System;

public class frmCalibrationView : MetroForm
{
	private readonly Form mForm;

	public bool isClose;

	private Guid mId;

	private Guid mIdParent;

	private readonly DataGridViewRow mMachine;

	private bool isEdit;

	private int mRow;

	private int mCol;

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

	private mSearch mSearchMain;

	private mPanelView mPanelViewMain;

	private Panel panelLogout;

	private Label lblFullname;

	private PictureBox ptbAvatar;

	private ToolStripMenuItem main_downloadToolStripMenuItem;

	private DataGridViewTextBoxColumn No;

	private DataGridViewTextBoxColumn CalibrationNo;

	private DataGridViewTextBoxColumn TypeId;

	private DataGridViewTextBoxColumn TypeName;

	private DataGridViewTextBoxColumn MachineId;

	private DataGridViewTextBoxColumn MachineName;

	private DataGridViewTextBoxColumn Company;

	private DataGridViewTextBoxColumn Staff;

	private DataGridViewTextBoxColumn CalDate;

	private DataGridViewTextBoxColumn ExpDate;

	private DataGridViewTextBoxColumn Period;

	private DataGridViewTextBoxColumn File;

	private DataGridViewTextBoxColumn Id;

	private new DataGridViewTextBoxColumn Created;

	private DataGridViewTextBoxColumn Modified;

	private DataGridViewTextBoxColumn CreatedBy;

	private DataGridViewTextBoxColumn ModifiedBy;

	private DataGridViewTextBoxColumn IsActivated;

	public frmCalibrationView(Form frm, DataGridViewRow row)
	{
		InitializeComponent();
		Common.setControls(this, toolTipMain, new List<ContextMenuStrip> { contextMenuStripMain });
		mForm = frm;
		mMachine = row;
		isClose = true;
		mId = Guid.Empty;
		mIdParent = Guid.Parse(mMachine.Cells["Id"].Value.ToString());
		isEdit = false;
		mRow = 0;
		mCol = 0;
		Text = string.Format("{0} ({1}#{2})", Text, mMachine.Cells["Name"].Value, mMachine.Cells["Model"].Value);
	}

	private void frmCalibrationView_Load(object sender, EventArgs e)
	{
		Init();
	}

	private void frmCalibrationView_Shown(object sender, EventArgs e)
	{
		load_AllData();
	}

	private void frmCalibrationView_FormClosing(object sender, FormClosingEventArgs e)
	{
		GC.Collect();
	}

	private void frmCalibrationView_FormClosed(object sender, FormClosedEventArgs e)
	{
		mPanelViewMain.mDispose();
		List<Type> list = new List<Type>();
		list.Add(typeof(frmCalibrationAdd));
		Common.closeForm(list);
		if (!isClose)
		{
			((frmMachineView)mForm).load_AllData();
		}
	}

	private void Init()
	{
		mPanelViewMain.Visible = false;
		lblFullname.Text = frmLogin.User.FullName;
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(APIUrl.APIHost + "/AuthUserImage/").Append(frmLogin.User.ImageUrl);
		try
		{
			ptbAvatar.Load(stringBuilder.ToString());
		}
		catch
		{
			ptbAvatar.Image = Resources._5S_QA_S;
		}
		mSearchMain.btnSearch.Click += btnSearch_Click;
		mPanelViewMain.btnEdit.Click += main_editToolStripMenuItem_Click;
		mPanelViewMain.btnDelete.Click += main_deleteToolStripMenuItem_Click;
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
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "MachineId=@0";
			queryArgs.PredicateParameters = new string[1] { mIdParent.ToString() };
			queryArgs.Order = "Created DESC";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs body = queryArgs;
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/Calibration/Gets").Result;
			mSearchMain.Init(Common.getDataTable<CalibrationViewModel>(result), dgvMain);
			load_dgvMain();
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
	}

	private void load_dgvMain()
	{
		start_Proccessor();
		dgvMain.DataSource = mSearchMain.SearchInAllColums();
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
		isEdit = false;
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
				mPanelViewMain.Display();
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
		int.TryParse(mSearchMain.txtPage.Text, out var result);
		int.TryParse(mSearchMain.cbbLimit.Text, out var result2);
		foreach (DataGridViewRow item in (IEnumerable)dataGridView.Rows)
		{
			item.Cells["No"].Value = (result - 1) * result2 + item.Index + 1;
			if (DateTime.Parse(item.Cells["ExpDate"].Value.ToString()) >= DateTime.Now.AddMonths(-1) && DateTime.Parse(item.Cells["ExpDate"].Value.ToString()) < DateTime.Now)
			{
				item.DefaultCellStyle.ForeColor = Color.Orange;
			}
			else if (DateTime.Parse(item.Cells["ExpDate"].Value.ToString()) < DateTime.Now)
			{
				item.DefaultCellStyle.ForeColor = Color.Red;
			}
		}
	}

	private void btnSearch_Click(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		load_dgvMain();
	}

	private void main_refreshToolStripMenuItem_Click(object sender, EventArgs e)
	{
		mSearchMain.btnSearch.Select();
		load_AllData();
	}

	private void main_newToolStripMenuItem_Click(object sender, EventArgs e)
	{
		mSearchMain.btnSearch.Select();
		Cursor.Current = Cursors.WaitCursor;
		List<Type> list = new List<Type>();
		list.Add(typeof(frmCalibrationAdd));
		Common.closeForm(list);
		new frmCalibrationAdd(this, (DataTable)dgvMain.DataSource, mIdParent, mId).Show();
	}

	private void main_editToolStripMenuItem_Click(object sender, EventArgs e)
	{
		mSearchMain.btnSearch.Select();
		Cursor.Current = Cursors.WaitCursor;
		if (mId.Equals(Guid.Empty))
		{
			MessageBox.Show(Common.getTextLanguage(this, "wSelectRow"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		List<Type> list = new List<Type>();
		list.Add(typeof(frmCalibrationAdd));
		Common.closeForm(list);
		new frmCalibrationAdd(this, (DataTable)dgvMain.DataSource, mIdParent, mId, isadd: false).Show();
	}

	private void main_downloadToolStripMenuItem_Click(object sender, EventArgs e)
	{
		mSearchMain.btnSearch.Select();
		Cursor.Current = Cursors.WaitCursor;
		if (mId.Equals(Guid.Empty))
		{
			MessageBox.Show(Common.getTextLanguage(this, "wSelectRow"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		if (dgvMain.CurrentRow.Cells["File"].Value == null || string.IsNullOrEmpty(dgvMain.CurrentRow.Cells["File"].Value.ToString()))
		{
			MessageBox.Show(Common.getTextLanguage(this, "wNoFile"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		try
		{
			ExportExcelDto exportExcelDto = frmLogin.client.DownloadAsync(mId, "/api/Calibration/DownloadFile/{id}").Result ?? throw new Exception(Common.getTextLanguage(this, "wNoFile"));
			string path = exportExcelDto.FileName.Replace("\"", "");
			string text = Path.Combine("C:\\Windows\\Temp\\5SQA_System", "VIEWS");
			Directory.CreateDirectory(text);
			path = Path.Combine(text, path);
			if (System.IO.File.Exists(path))
			{
				System.IO.File.Delete(path);
			}
			Cursor.Current = Cursors.WaitCursor;
			if (Common.ByteArrayToFile(path, exportExcelDto.Value))
			{
				Common.ExecuteBatFile(path);
			}
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

	private void main_deleteToolStripMenuItem_Click(object sender, EventArgs e)
	{
		mSearchMain.btnSearch.Select();
		if (mId.Equals(Guid.Empty))
		{
			MessageBox.Show(Common.getTextLanguage(this, "wSelectRow"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
		else
		{
			if (!MessageBox.Show(string.Format("{0} {1}", Common.getTextLanguage(this, "wSureDelete"), dgvMain.CurrentRow.Cells["CalibrationNo"].Value), Common.getTextLanguage(this, "QUESTION"), MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.Yes))
			{
				return;
			}
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				start_Proccessor();
				ResponseDto result = frmLogin.client.DeleteAsync(mId, "/api/Calibration/Delete/{id}").Result;
				if (!result.Success)
				{
					throw new Exception(result.Messages.ElementAt(0).Message);
				}
				isClose = false;
				List<Type> list = new List<Type>();
				list.Add(typeof(frmCalibrationAdd));
				Common.closeForm(list);
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

	private void ptbAvatar_DoubleClick(object sender, EventArgs e)
	{
		Common.activeForm(typeof(frmLogin));
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_5S_QA_System.frmCalibrationView));
		this.contextMenuStripMain = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.main_refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.main_newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.main_editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.main_deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.main_downloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.statusStripfrmMain = new System.Windows.Forms.StatusStrip();
		this.sprogbarStatus = new System.Windows.Forms.ToolStripProgressBar();
		this.slblStatus = new System.Windows.Forms.ToolStripStatusLabel();
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.dgvMain = new System.Windows.Forms.DataGridView();
		this.panelLogout = new System.Windows.Forms.Panel();
		this.lblFullname = new System.Windows.Forms.Label();
		this.ptbAvatar = new System.Windows.Forms.PictureBox();
		this.mPanelViewMain = new _5S_QA_System.View.User_control.mPanelView();
		this.mSearchMain = new _5S_QA_System.View.User_control.mSearch();
		this.No = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.CalibrationNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.TypeId = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.TypeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.MachineId = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.MachineName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Company = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Staff = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.CalDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ExpDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Period = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.File = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Created = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Modified = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.CreatedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ModifiedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.IsActivated = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.contextMenuStripMain.SuspendLayout();
		this.statusStripfrmMain.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvMain).BeginInit();
		this.panelLogout.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.ptbAvatar).BeginInit();
		base.SuspendLayout();
		this.contextMenuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.main_refreshToolStripMenuItem, this.toolStripSeparator1, this.main_newToolStripMenuItem, this.main_editToolStripMenuItem, this.toolStripSeparator6, this.main_deleteToolStripMenuItem, this.main_downloadToolStripMenuItem });
		this.contextMenuStripMain.Name = "contextMenuStripStaff";
		this.contextMenuStripMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
		this.contextMenuStripMain.Size = new System.Drawing.Size(148, 126);
		this.main_refreshToolStripMenuItem.Name = "main_refreshToolStripMenuItem";
		this.main_refreshToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
		this.main_refreshToolStripMenuItem.Text = "Refresh";
		this.main_refreshToolStripMenuItem.Click += new System.EventHandler(main_refreshToolStripMenuItem_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(144, 6);
		this.main_newToolStripMenuItem.Name = "main_newToolStripMenuItem";
		this.main_newToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
		this.main_newToolStripMenuItem.Text = "New";
		this.main_newToolStripMenuItem.Click += new System.EventHandler(main_newToolStripMenuItem_Click);
		this.main_editToolStripMenuItem.Name = "main_editToolStripMenuItem";
		this.main_editToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
		this.main_editToolStripMenuItem.Text = "Edit";
		this.main_editToolStripMenuItem.Click += new System.EventHandler(main_editToolStripMenuItem_Click);
		this.toolStripSeparator6.Name = "toolStripSeparator6";
		this.toolStripSeparator6.Size = new System.Drawing.Size(144, 6);
		this.main_deleteToolStripMenuItem.Name = "main_deleteToolStripMenuItem";
		this.main_deleteToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
		this.main_deleteToolStripMenuItem.Text = "Delete";
		this.main_deleteToolStripMenuItem.Click += new System.EventHandler(main_deleteToolStripMenuItem_Click);
		this.main_downloadToolStripMenuItem.Name = "main_downloadToolStripMenuItem";
		this.main_downloadToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
		this.main_downloadToolStripMenuItem.Text = "Download file";
		this.main_downloadToolStripMenuItem.Click += new System.EventHandler(main_downloadToolStripMenuItem_Click);
		this.statusStripfrmMain.BackColor = System.Drawing.SystemColors.ControlLight;
		this.statusStripfrmMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.statusStripfrmMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.sprogbarStatus, this.slblStatus });
		this.statusStripfrmMain.Location = new System.Drawing.Point(20, 534);
		this.statusStripfrmMain.Name = "statusStripfrmMain";
		this.statusStripfrmMain.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
		this.statusStripfrmMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
		this.statusStripfrmMain.Size = new System.Drawing.Size(960, 26);
		this.statusStripfrmMain.SizingGrip = false;
		this.statusStripfrmMain.Stretch = false;
		this.statusStripfrmMain.TabIndex = 6;
		this.sprogbarStatus.Name = "sprogbarStatus";
		this.sprogbarStatus.Size = new System.Drawing.Size(200, 20);
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
		this.dgvMain.Columns.AddRange(this.No, this.CalibrationNo, this.TypeId, this.TypeName, this.MachineId, this.MachineName, this.Company, this.Staff, this.CalDate, this.ExpDate, this.Period, this.File, this.Id, this.Created, this.Modified, this.CreatedBy, this.ModifiedBy, this.IsActivated);
		this.dgvMain.ContextMenuStrip = this.contextMenuStripMain;
		this.dgvMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.dgvMain.EnableHeadersVisualStyles = false;
		this.dgvMain.Location = new System.Drawing.Point(20, 134);
		this.dgvMain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.dgvMain.MultiSelect = false;
		this.dgvMain.Name = "dgvMain";
		this.dgvMain.ReadOnly = true;
		this.dgvMain.RowHeadersWidth = 25;
		this.dgvMain.Size = new System.Drawing.Size(960, 400);
		this.dgvMain.TabIndex = 2;
		this.dgvMain.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvMain_CellClick);
		this.dgvMain.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvMain_CellDoubleClick);
		this.dgvMain.CurrentCellChanged += new System.EventHandler(dgvMain_CurrentCellChanged);
		this.dgvMain.Sorted += new System.EventHandler(dgvMain_Sorted);
		this.panelLogout.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.panelLogout.BackColor = System.Drawing.Color.Transparent;
		this.panelLogout.Controls.Add(this.lblFullname);
		this.panelLogout.Controls.Add(this.ptbAvatar);
		this.panelLogout.Location = new System.Drawing.Point(630, 27);
		this.panelLogout.Name = "panelLogout";
		this.panelLogout.Size = new System.Drawing.Size(350, 42);
		this.panelLogout.TabIndex = 175;
		this.panelLogout.TabStop = true;
		this.lblFullname.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.lblFullname.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f);
		this.lblFullname.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.lblFullname.Location = new System.Drawing.Point(0, 26);
		this.lblFullname.Name = "lblFullname";
		this.lblFullname.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
		this.lblFullname.Size = new System.Drawing.Size(308, 16);
		this.lblFullname.TabIndex = 34;
		this.lblFullname.Text = "...";
		this.lblFullname.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.ptbAvatar.Dock = System.Windows.Forms.DockStyle.Right;
		this.ptbAvatar.Image = _5S_QA_System.Properties.Resources._5S_QA_S;
		this.ptbAvatar.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.ptbAvatar.Location = new System.Drawing.Point(308, 0);
		this.ptbAvatar.Margin = new System.Windows.Forms.Padding(4);
		this.ptbAvatar.Name = "ptbAvatar";
		this.ptbAvatar.Size = new System.Drawing.Size(42, 42);
		this.ptbAvatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.ptbAvatar.TabIndex = 14;
		this.ptbAvatar.TabStop = false;
		this.ptbAvatar.DoubleClick += new System.EventHandler(ptbAvatar_DoubleClick);
		this.mPanelViewMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.mPanelViewMain.Dock = System.Windows.Forms.DockStyle.Right;
		this.mPanelViewMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.mPanelViewMain.Location = new System.Drawing.Point(530, 134);
		this.mPanelViewMain.Margin = new System.Windows.Forms.Padding(0);
		this.mPanelViewMain.Name = "mPanelViewMain";
		this.mPanelViewMain.Size = new System.Drawing.Size(450, 400);
		this.mPanelViewMain.TabIndex = 17;
		this.mSearchMain.AutoSize = true;
		this.mSearchMain.BackColor = System.Drawing.SystemColors.Control;
		this.mSearchMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.mSearchMain.dataTable = null;
		this.mSearchMain.Dock = System.Windows.Forms.DockStyle.Top;
		this.mSearchMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.mSearchMain.Location = new System.Drawing.Point(20, 70);
		this.mSearchMain.Margin = new System.Windows.Forms.Padding(4);
		this.mSearchMain.MaximumSize = new System.Drawing.Size(5000, 64);
		this.mSearchMain.MinimumSize = new System.Drawing.Size(25, 64);
		this.mSearchMain.Name = "mSearchMain";
		this.mSearchMain.Padding = new System.Windows.Forms.Padding(3);
		this.mSearchMain.Size = new System.Drawing.Size(960, 64);
		this.mSearchMain.TabIndex = 16;
		this.No.DataPropertyName = "No";
		dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		this.No.DefaultCellStyle = dataGridViewCellStyle3;
		this.No.HeaderText = "No.";
		this.No.Name = "No";
		this.No.ReadOnly = true;
		this.No.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.No.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.No.Width = 40;
		this.CalibrationNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.CalibrationNo.DataPropertyName = "CalibrationNo";
		this.CalibrationNo.FillWeight = 30f;
		this.CalibrationNo.HeaderText = "Cali. no";
		this.CalibrationNo.Name = "CalibrationNo";
		this.CalibrationNo.ReadOnly = true;
		this.TypeId.DataPropertyName = "TypeId";
		this.TypeId.HeaderText = "Type id";
		this.TypeId.Name = "TypeId";
		this.TypeId.ReadOnly = true;
		this.TypeId.Visible = false;
		this.TypeName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.TypeName.DataPropertyName = "TypeName";
		this.TypeName.FillWeight = 30f;
		this.TypeName.HeaderText = "Type";
		this.TypeName.Name = "TypeName";
		this.TypeName.ReadOnly = true;
		this.MachineId.DataPropertyName = "MachineId";
		this.MachineId.HeaderText = "Machine id";
		this.MachineId.Name = "MachineId";
		this.MachineId.ReadOnly = true;
		this.MachineId.Visible = false;
		this.MachineName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.MachineName.DataPropertyName = "MachineName";
		this.MachineName.FillWeight = 30f;
		this.MachineName.HeaderText = "Machine";
		this.MachineName.Name = "MachineName";
		this.MachineName.ReadOnly = true;
		this.MachineName.Visible = false;
		this.Company.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Company.DataPropertyName = "Company";
		this.Company.FillWeight = 30f;
		this.Company.HeaderText = "Company";
		this.Company.Name = "Company";
		this.Company.ReadOnly = true;
		this.Staff.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Staff.DataPropertyName = "Staff";
		this.Staff.FillWeight = 30f;
		this.Staff.HeaderText = "Staff";
		this.Staff.Name = "Staff";
		this.Staff.ReadOnly = true;
		this.CalDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.CalDate.DataPropertyName = "CalDate";
		this.CalDate.FillWeight = 30f;
		this.CalDate.HeaderText = "Cali. date";
		this.CalDate.Name = "CalDate";
		this.CalDate.ReadOnly = true;
		this.ExpDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.ExpDate.DataPropertyName = "ExpDate";
		this.ExpDate.FillWeight = 30f;
		this.ExpDate.HeaderText = "Exp. date";
		this.ExpDate.Name = "ExpDate";
		this.ExpDate.ReadOnly = true;
		this.Period.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Period.DataPropertyName = "Period";
		this.Period.FillWeight = 20f;
		this.Period.HeaderText = "Period";
		this.Period.Name = "Period";
		this.Period.ReadOnly = true;
		this.File.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.File.DataPropertyName = "File";
		this.File.FillWeight = 30f;
		this.File.HeaderText = "File";
		this.File.Name = "File";
		this.File.ReadOnly = true;
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
		base.ClientSize = new System.Drawing.Size(1000, 580);
		base.Controls.Add(this.panelLogout);
		base.Controls.Add(this.mPanelViewMain);
		base.Controls.Add(this.dgvMain);
		base.Controls.Add(this.mSearchMain);
		base.Controls.Add(this.statusStripfrmMain);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "frmCalibrationView";
		base.Padding = new System.Windows.Forms.Padding(20, 70, 20, 20);
		this.Text = "5S QA System * CALIBRATION";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmCalibrationView_FormClosing);
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmCalibrationView_FormClosed);
		base.Load += new System.EventHandler(frmCalibrationView_Load);
		base.Shown += new System.EventHandler(frmCalibrationView_Shown);
		this.contextMenuStripMain.ResumeLayout(false);
		this.statusStripfrmMain.ResumeLayout(false);
		this.statusStripfrmMain.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvMain).EndInit();
		this.panelLogout.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.ptbAvatar).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
