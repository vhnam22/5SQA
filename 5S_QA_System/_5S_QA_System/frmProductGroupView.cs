using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using _5S_QA_DL.APIContext;
using _5S_QA_Entities.Abstracts;
using _5S_QA_Entities.Constants;
using _5S_QA_Entities.Dtos;
using _5S_QA_Entities.Enums;
using _5S_QA_Entities.Models;
using _5S_QA_System.Controls;
using _5S_QA_System.Properties;
using _5S_QA_System.View.User_control;
using MetroFramework.Forms;

namespace _5S_QA_System;

public class frmProductGroupView : MetroForm
{
	private Guid mId;

	private Guid mIdCopy;

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

	private ToolStripMenuItem main_stageToolStripMenuItem;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn16;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn17;

	private DataGridViewTextBoxColumn No;

	private DataGridViewTextBoxColumn Code;

	private DataGridViewTextBoxColumn name;

	private DataGridViewTextBoxColumn Description;

	private DataGridViewTextBoxColumn Version;

	private DataGridViewTextBoxColumn ReleaseDate;

	private DataGridViewTextBoxColumn RevisionHistory;

	private DataGridViewTextBoxColumn PreparedBy;

	private DataGridViewTextBoxColumn CheckedBy;

	private DataGridViewTextBoxColumn ApprovedBy;

	private DataGridViewTextBoxColumn ProductCount;

	private DataGridViewTextBoxColumn Id;

	private new DataGridViewTextBoxColumn Created;

	private DataGridViewTextBoxColumn Modified;

	private DataGridViewTextBoxColumn CreatedBy;

	private DataGridViewTextBoxColumn ModifiedBy;

	private DataGridViewTextBoxColumn IsActivated;

	private ToolStripMenuItem main_copyToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator2;

	public frmProductGroupView()
	{
		InitializeComponent();
		Common.setControls(this, toolTipMain, new List<ContextMenuStrip> { contextMenuStripMain });
		mId = Guid.Empty;
		isEdit = false;
		mRow = 0;
		mCol = 0;
	}

	private void frmProductGroupView_Load(object sender, EventArgs e)
	{
		Init();
	}

	private void frmProductGroupView_Shown(object sender, EventArgs e)
	{
		load_AllData();
	}

	private void frmProductGroupView_FormClosing(object sender, FormClosingEventArgs e)
	{
		GC.Collect();
	}

	private void frmProductGroupView_FormClosed(object sender, FormClosedEventArgs e)
	{
		mPanelViewMain.mDispose();
		List<Type> list = new List<Type>();
		list.Add(typeof(frmProductGroupAdd));
		list.Add(typeof(frmProductView));
		Common.closeForm(list);
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
			en_disControl(enable: true);
			QueryArgs body = new QueryArgs
			{
				Order = "Created DESC",
				Page = 1,
				Limit = int.MaxValue
			};
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/ProductGroup/Gets").Result;
			mSearchMain.Init(Common.getDataTable<ProductGroupViewModel>(result), dgvMain);
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

	private void en_disControl(bool enable)
	{
		mPanelViewMain.btnEdit.Enabled = enable;
		mPanelViewMain.btnDelete.Enabled = enable;
		main_newToolStripMenuItem.Enabled = enable;
		main_editToolStripMenuItem.Enabled = enable;
		main_deleteToolStripMenuItem.Enabled = enable;
		main_stageToolStripMenuItem.Enabled = enable;
		if (enable)
		{
			main_copyToolStripMenuItem.Text = Common.getTextLanguage(this, "Copy");
		}
		else
		{
			main_copyToolStripMenuItem.Text = Common.getTextLanguage(this, "Paste");
		}
	}

	public void OpenProduct(ProductGroupViewModel group)
	{
		Cursor.Current = Cursors.WaitCursor;
		mSearchMain.btnSearch.Select();
		if (((AuditableEntity)(object)group).Id.Equals(Guid.Empty))
		{
			MessageBox.Show(Common.getTextLanguage(this, "wSelectRow"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		List<Type> list = new List<Type>();
		list.Add(typeof(frmProductView));
		Common.closeForm(list);
		new frmProductView(this, group).Show();
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
			if (main_copyToolStripMenuItem.Text.Equals(Common.getTextLanguage(this, "Paste")))
			{
				item.DefaultCellStyle.ForeColor = Color.Green;
			}
			else if (int.Parse(item.Cells["ProductCount"].Value.ToString()) == 0 || !(bool)item.Cells["IsActivated"].Value)
			{
				item.DefaultCellStyle.ForeColor = SystemColors.GrayText;
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
		if (frmLogin.User.Role.Equals(RoleWeb.Administrator) && frmLogin.User.JobTitle != "Manager" && frmLogin.User.JobTitle != "Supervisor")
		{
			MessageBox.Show(Common.getTextLanguage(typeof(frmLogin).Name, "wNoAuthorization"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		List<Type> list = new List<Type>();
		list.Add(typeof(frmProductGroupAdd));
		Common.closeForm(list);
		new frmProductGroupAdd(this, (DataTable)dgvMain.DataSource, mId).Show();
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
		if (frmLogin.User.Role.Equals(RoleWeb.Administrator) && frmLogin.User.JobTitle != "Manager" && frmLogin.User.JobTitle != "Supervisor")
		{
			MessageBox.Show(Common.getTextLanguage(typeof(frmLogin).Name, "wNoAuthorization"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		List<Type> list = new List<Type>();
		list.Add(typeof(frmProductGroupAdd));
		Common.closeForm(list);
		new frmProductGroupAdd(this, (DataTable)dgvMain.DataSource, mId, isadd: false).Show();
	}

	private void main_deleteToolStripMenuItem_Click(object sender, EventArgs e)
	{
		mSearchMain.btnSearch.Select();
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
			if (!MessageBox.Show(string.Format("{0} {1}", Common.getTextLanguage(this, "wSureDelete"), dgvMain.CurrentRow.Cells["Code"].Value), Common.getTextLanguage(this, "QUESTION"), MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.Yes))
			{
				return;
			}
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				start_Proccessor();
				ResponseDto result = frmLogin.client.DeleteAsync(mId, "/api/ProductGroup/Delete/{id}").Result;
				if (!result.Success)
				{
					throw new Exception(result.Messages.ElementAt(0).Message);
				}
				List<Type> list = new List<Type>();
				list.Add(typeof(frmProductGroupAdd));
				list.Add(typeof(frmProductView));
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

	private void main_stageToolStripMenuItem_Click(object sender, EventArgs e)
	{
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Expected O, but got Unknown
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Expected O, but got Unknown
		mSearchMain.btnSearch.Select();
		if (mId.Equals(Guid.Empty))
		{
			MessageBox.Show(Common.getTextLanguage(this, "wSelectRow"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		List<Type> list = new List<Type>();
		list.Add(typeof(frmProductView));
		Common.closeForm(list);
		ProductGroupViewModel val = new ProductGroupViewModel();
		((AuditableEntity)val).Id = Guid.Parse(dgvMain.CurrentRow.Cells["Id"].Value.ToString());
		val.Code = dgvMain.CurrentRow.Cells["Code"].Value.ToString();
		val.Name = dgvMain.CurrentRow.Cells["Name"].Value.ToString();
		ProductGroupViewModel val2 = val;
		new frmProductView(this, val2).Show();
	}

	private void main_copyToolStripMenuItem_Click(object sender, EventArgs e)
	{
		mSearchMain.btnSearch.Select();
		Cursor.Current = Cursors.WaitCursor;
		if (frmLogin.User.Role.Equals(RoleWeb.Administrator) && frmLogin.User.JobTitle != "Manager" && frmLogin.User.JobTitle != "Supervisor")
		{
			MessageBox.Show(Common.getTextLanguage(typeof(frmLogin).Name, "wNoAuthorization"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		if (main_copyToolStripMenuItem.Text.Equals(Common.getTextLanguage(this, "Copy")))
		{
			if (mId.Equals(Guid.Empty))
			{
				MessageBox.Show(Common.getTextLanguage(this, "wSelectRow"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			mIdCopy = mId;
			en_disControl(enable: false);
			dgvMain_Sorted(dgvMain, null);
			return;
		}
		try
		{
			ResponseDto result = frmLogin.client.CopyAsync(mIdCopy, "/api/ProductGroup/Copy/{id}").Result;
			if (!result.Success)
			{
				throw new Exception(result.Messages.ElementAt(0).Message);
			}
			main_refreshToolStripMenuItem.PerformClick();
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
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_5S_QA_System.frmProductGroupView));
		this.contextMenuStripMain = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.main_refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.main_newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.main_editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.main_stageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.main_deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.statusStripfrmMain = new System.Windows.Forms.StatusStrip();
		this.sprogbarStatus = new System.Windows.Forms.ToolStripProgressBar();
		this.slblStatus = new System.Windows.Forms.ToolStripStatusLabel();
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.dgvMain = new System.Windows.Forms.DataGridView();
		this.No = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Version = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ReleaseDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.RevisionHistory = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.PreparedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.CheckedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ApprovedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ProductCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Created = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Modified = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.CreatedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ModifiedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.IsActivated = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.panelLogout = new System.Windows.Forms.Panel();
		this.lblFullname = new System.Windows.Forms.Label();
		this.ptbAvatar = new System.Windows.Forms.PictureBox();
		this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn15 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn16 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn17 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.mPanelViewMain = new _5S_QA_System.View.User_control.mPanelView();
		this.mSearchMain = new _5S_QA_System.View.User_control.mSearch();
		this.main_copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.contextMenuStripMain.SuspendLayout();
		this.statusStripfrmMain.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvMain).BeginInit();
		this.panelLogout.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.ptbAvatar).BeginInit();
		base.SuspendLayout();
		this.contextMenuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.main_refreshToolStripMenuItem, this.toolStripSeparator1, this.main_newToolStripMenuItem, this.main_editToolStripMenuItem, this.main_stageToolStripMenuItem, this.toolStripSeparator6, this.main_deleteToolStripMenuItem, this.toolStripSeparator2, this.main_copyToolStripMenuItem });
		this.contextMenuStripMain.Name = "contextMenuStripStaff";
		this.contextMenuStripMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
		this.contextMenuStripMain.Size = new System.Drawing.Size(181, 176);
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
		this.main_editToolStripMenuItem.Name = "main_editToolStripMenuItem";
		this.main_editToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
		this.main_editToolStripMenuItem.Text = "Edit";
		this.main_editToolStripMenuItem.Click += new System.EventHandler(main_editToolStripMenuItem_Click);
		this.main_stageToolStripMenuItem.Name = "main_stageToolStripMenuItem";
		this.main_stageToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
		this.main_stageToolStripMenuItem.Text = "Stage";
		this.main_stageToolStripMenuItem.Click += new System.EventHandler(main_stageToolStripMenuItem_Click);
		this.toolStripSeparator6.Name = "toolStripSeparator6";
		this.toolStripSeparator6.Size = new System.Drawing.Size(177, 6);
		this.main_deleteToolStripMenuItem.Name = "main_deleteToolStripMenuItem";
		this.main_deleteToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
		this.main_deleteToolStripMenuItem.Text = "Delete";
		this.main_deleteToolStripMenuItem.Click += new System.EventHandler(main_deleteToolStripMenuItem_Click);
		this.statusStripfrmMain.BackColor = System.Drawing.SystemColors.ControlLight;
		this.statusStripfrmMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.statusStripfrmMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.sprogbarStatus, this.slblStatus });
		this.statusStripfrmMain.Location = new System.Drawing.Point(20, 554);
		this.statusStripfrmMain.Name = "statusStripfrmMain";
		this.statusStripfrmMain.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
		this.statusStripfrmMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
		this.statusStripfrmMain.Size = new System.Drawing.Size(1060, 26);
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
		this.dgvMain.Columns.AddRange(this.No, this.Code, this.name, this.Description, this.Version, this.ReleaseDate, this.RevisionHistory, this.PreparedBy, this.CheckedBy, this.ApprovedBy, this.ProductCount, this.Id, this.Created, this.Modified, this.CreatedBy, this.ModifiedBy, this.IsActivated);
		this.dgvMain.ContextMenuStrip = this.contextMenuStripMain;
		this.dgvMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.dgvMain.EnableHeadersVisualStyles = false;
		this.dgvMain.Location = new System.Drawing.Point(20, 134);
		this.dgvMain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.dgvMain.MultiSelect = false;
		this.dgvMain.Name = "dgvMain";
		this.dgvMain.ReadOnly = true;
		this.dgvMain.RowHeadersWidth = 25;
		this.dgvMain.Size = new System.Drawing.Size(1060, 420);
		this.dgvMain.TabIndex = 2;
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
		this.Code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Code.DataPropertyName = "Code";
		this.Code.FillWeight = 30f;
		this.Code.HeaderText = "Code";
		this.Code.Name = "Code";
		this.Code.ReadOnly = true;
		this.name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.name.DataPropertyName = "Name";
		this.name.FillWeight = 30f;
		this.name.HeaderText = "Name";
		this.name.Name = "name";
		this.name.ReadOnly = true;
		this.Description.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Description.DataPropertyName = "Description";
		this.Description.FillWeight = 20f;
		this.Description.HeaderText = "Description";
		this.Description.Name = "Description";
		this.Description.ReadOnly = true;
		this.Version.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Version.DataPropertyName = "Version";
		this.Version.FillWeight = 20f;
		this.Version.HeaderText = "Version";
		this.Version.Name = "Version";
		this.Version.ReadOnly = true;
		this.ReleaseDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.ReleaseDate.DataPropertyName = "ReleaseDate";
		this.ReleaseDate.FillWeight = 20f;
		this.ReleaseDate.HeaderText = "Release date";
		this.ReleaseDate.Name = "ReleaseDate";
		this.ReleaseDate.ReadOnly = true;
		this.RevisionHistory.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.RevisionHistory.DataPropertyName = "RevisionHistory";
		this.RevisionHistory.FillWeight = 20f;
		this.RevisionHistory.HeaderText = "Rev. history";
		this.RevisionHistory.Name = "RevisionHistory";
		this.RevisionHistory.ReadOnly = true;
		this.PreparedBy.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.PreparedBy.DataPropertyName = "PreparedBy";
		this.PreparedBy.FillWeight = 20f;
		this.PreparedBy.HeaderText = "Prepared by";
		this.PreparedBy.Name = "PreparedBy";
		this.PreparedBy.ReadOnly = true;
		this.CheckedBy.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.CheckedBy.DataPropertyName = "CheckedBy";
		this.CheckedBy.FillWeight = 20f;
		this.CheckedBy.HeaderText = "Checked by";
		this.CheckedBy.Name = "CheckedBy";
		this.CheckedBy.ReadOnly = true;
		this.ApprovedBy.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.ApprovedBy.DataPropertyName = "ApprovedBy";
		this.ApprovedBy.FillWeight = 20f;
		this.ApprovedBy.HeaderText = "Approved by";
		this.ApprovedBy.Name = "ApprovedBy";
		this.ApprovedBy.ReadOnly = true;
		this.ProductCount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.ProductCount.DataPropertyName = "ProductCount";
		this.ProductCount.FillWeight = 20f;
		this.ProductCount.HeaderText = "Stage";
		this.ProductCount.Name = "ProductCount";
		this.ProductCount.ReadOnly = true;
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
		this.panelLogout.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.panelLogout.BackColor = System.Drawing.Color.Transparent;
		this.panelLogout.Controls.Add(this.lblFullname);
		this.panelLogout.Controls.Add(this.ptbAvatar);
		this.panelLogout.Location = new System.Drawing.Point(730, 27);
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
		this.dataGridViewTextBoxColumn1.DataPropertyName = "No";
		dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle4;
		this.dataGridViewTextBoxColumn1.HeaderText = "No.";
		this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
		this.dataGridViewTextBoxColumn1.ReadOnly = true;
		this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.dataGridViewTextBoxColumn1.Width = 40;
		this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn2.DataPropertyName = "Code";
		this.dataGridViewTextBoxColumn2.FillWeight = 30f;
		this.dataGridViewTextBoxColumn2.HeaderText = "Code";
		this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
		this.dataGridViewTextBoxColumn2.ReadOnly = true;
		this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn3.DataPropertyName = "Name";
		this.dataGridViewTextBoxColumn3.FillWeight = 30f;
		this.dataGridViewTextBoxColumn3.HeaderText = "Name";
		this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
		this.dataGridViewTextBoxColumn3.ReadOnly = true;
		this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn4.DataPropertyName = "Description";
		this.dataGridViewTextBoxColumn4.FillWeight = 30f;
		this.dataGridViewTextBoxColumn4.HeaderText = "Description";
		this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
		this.dataGridViewTextBoxColumn4.ReadOnly = true;
		this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn5.DataPropertyName = "Version";
		this.dataGridViewTextBoxColumn5.FillWeight = 20f;
		this.dataGridViewTextBoxColumn5.HeaderText = "Version";
		this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
		this.dataGridViewTextBoxColumn5.ReadOnly = true;
		this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn6.DataPropertyName = "ReleaseDate";
		this.dataGridViewTextBoxColumn6.FillWeight = 30f;
		this.dataGridViewTextBoxColumn6.HeaderText = "Release date";
		this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
		this.dataGridViewTextBoxColumn6.ReadOnly = true;
		this.dataGridViewTextBoxColumn7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn7.DataPropertyName = "RevisionHistory";
		this.dataGridViewTextBoxColumn7.FillWeight = 30f;
		this.dataGridViewTextBoxColumn7.HeaderText = "Rev. history";
		this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
		this.dataGridViewTextBoxColumn7.ReadOnly = true;
		this.dataGridViewTextBoxColumn8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn8.DataPropertyName = "PreparedBy";
		this.dataGridViewTextBoxColumn8.FillWeight = 30f;
		this.dataGridViewTextBoxColumn8.HeaderText = "Prepared by";
		this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
		this.dataGridViewTextBoxColumn8.ReadOnly = true;
		this.dataGridViewTextBoxColumn9.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn9.DataPropertyName = "CheckedBy";
		this.dataGridViewTextBoxColumn9.FillWeight = 30f;
		this.dataGridViewTextBoxColumn9.HeaderText = "Checked by";
		this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
		this.dataGridViewTextBoxColumn9.ReadOnly = true;
		this.dataGridViewTextBoxColumn10.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn10.DataPropertyName = "ApprovedBy";
		this.dataGridViewTextBoxColumn10.FillWeight = 30f;
		this.dataGridViewTextBoxColumn10.HeaderText = "Approved by";
		this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
		this.dataGridViewTextBoxColumn10.ReadOnly = true;
		this.dataGridViewTextBoxColumn11.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn11.DataPropertyName = "ProductCount";
		this.dataGridViewTextBoxColumn11.FillWeight = 20f;
		this.dataGridViewTextBoxColumn11.HeaderText = "Stage count";
		this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
		this.dataGridViewTextBoxColumn11.ReadOnly = true;
		this.dataGridViewTextBoxColumn12.DataPropertyName = "Id";
		this.dataGridViewTextBoxColumn12.HeaderText = "Id";
		this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
		this.dataGridViewTextBoxColumn12.ReadOnly = true;
		this.dataGridViewTextBoxColumn12.Visible = false;
		this.dataGridViewTextBoxColumn13.DataPropertyName = "Created";
		this.dataGridViewTextBoxColumn13.HeaderText = "Created";
		this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
		this.dataGridViewTextBoxColumn13.ReadOnly = true;
		this.dataGridViewTextBoxColumn13.Visible = false;
		this.dataGridViewTextBoxColumn14.DataPropertyName = "Modified";
		this.dataGridViewTextBoxColumn14.HeaderText = "Modified";
		this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
		this.dataGridViewTextBoxColumn14.ReadOnly = true;
		this.dataGridViewTextBoxColumn14.Visible = false;
		this.dataGridViewTextBoxColumn15.DataPropertyName = "CreatedBy";
		this.dataGridViewTextBoxColumn15.HeaderText = "Created by";
		this.dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
		this.dataGridViewTextBoxColumn15.ReadOnly = true;
		this.dataGridViewTextBoxColumn15.Visible = false;
		this.dataGridViewTextBoxColumn16.DataPropertyName = "ModifiedBy";
		this.dataGridViewTextBoxColumn16.HeaderText = "Modified by";
		this.dataGridViewTextBoxColumn16.Name = "dataGridViewTextBoxColumn16";
		this.dataGridViewTextBoxColumn16.ReadOnly = true;
		this.dataGridViewTextBoxColumn16.Visible = false;
		this.dataGridViewTextBoxColumn17.DataPropertyName = "IsActivated";
		this.dataGridViewTextBoxColumn17.HeaderText = "Is activated";
		this.dataGridViewTextBoxColumn17.Name = "dataGridViewTextBoxColumn17";
		this.dataGridViewTextBoxColumn17.ReadOnly = true;
		this.dataGridViewTextBoxColumn17.Visible = false;
		this.mPanelViewMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.mPanelViewMain.Dock = System.Windows.Forms.DockStyle.Right;
		this.mPanelViewMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.mPanelViewMain.Location = new System.Drawing.Point(630, 134);
		this.mPanelViewMain.Margin = new System.Windows.Forms.Padding(0);
		this.mPanelViewMain.Name = "mPanelViewMain";
		this.mPanelViewMain.Size = new System.Drawing.Size(450, 420);
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
		this.mSearchMain.Size = new System.Drawing.Size(1060, 64);
		this.mSearchMain.TabIndex = 16;
		this.main_copyToolStripMenuItem.Name = "main_copyToolStripMenuItem";
		this.main_copyToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
		this.main_copyToolStripMenuItem.Text = "Copy";
		this.main_copyToolStripMenuItem.Click += new System.EventHandler(main_copyToolStripMenuItem_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(177, 6);
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1100, 600);
		base.Controls.Add(this.panelLogout);
		base.Controls.Add(this.mPanelViewMain);
		base.Controls.Add(this.dgvMain);
		base.Controls.Add(this.mSearchMain);
		base.Controls.Add(this.statusStripfrmMain);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "frmProductGroupView";
		base.Padding = new System.Windows.Forms.Padding(20, 70, 20, 20);
		this.Text = "5S QA System * PRODUCT";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmProductGroupView_FormClosing);
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmProductGroupView_FormClosed);
		base.Load += new System.EventHandler(frmProductGroupView_Load);
		base.Shown += new System.EventHandler(frmProductGroupView_Shown);
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
