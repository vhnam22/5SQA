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
using _5S_QA_Entities.Constants;
using _5S_QA_Entities.Dtos;
using _5S_QA_Entities.Models;
using _5S_QA_System.Controls;
using _5S_QA_System.Properties;
using _5S_QA_System.View.User_control;
using MetroFramework.Forms;

namespace _5S_QA_System;

public class frmPlanView : MetroForm
{
	private readonly Form mForm;

	public bool isClose;

	private Guid mId;

	private Guid mIDParent;

	private Guid IdFrom;

	private Guid IdTo;

	private bool isEdit;

	private int mRow;

	private int mCol;

	private int mTotalMeas = 0;

	private ProductViewModel mProduct;

	private IContainer components = null;

	private mPanelView mPanelViewMain;

	private mSearch mSearchMain;

	private ContextMenuStrip contextMenuStripMain;

	private ToolStripMenuItem main_refreshToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripMenuItem main_newToolStripMenuItem;

	private ToolStripMenuItem main_editToolStripMenuItem;

	private ToolStripMenuItem main_viewToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator6;

	private ToolStripMenuItem main_deleteToolStripMenuItem;

	private StatusStrip statusStripfrmMain;

	private ToolStripProgressBar sprogbarStatus;

	public ToolStripStatusLabel slblStatus;

	private ToolTip toolTipMain;

	private DataGridView dgvMain;

	private TableLayoutPanel tpanelMain;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripMenuItem main_moveToolStripMenuItem;

	private DataGridViewTextBoxColumn No;

	private DataGridViewTextBoxColumn ProductId;

	private DataGridViewTextBoxColumn ProductCode;

	private new DataGridViewTextBoxColumn ProductName;

	private DataGridViewTextBoxColumn StageId;

	private DataGridViewTextBoxColumn StageName;

	private DataGridViewTextBoxColumn TemplateId;

	private DataGridViewTextBoxColumn TemplateName;

	private DataGridViewTextBoxColumn TotalDetails;

	private DataGridViewTextBoxColumn Id;

	private new DataGridViewTextBoxColumn Created;

	private DataGridViewTextBoxColumn Modified;

	private DataGridViewTextBoxColumn CreatedBy;

	private DataGridViewTextBoxColumn ModifiedBy;

	private DataGridViewTextBoxColumn IsActivated;

	private Panel panelLogout;

	private Label lblFullname;

	private PictureBox ptbAvatar;

	private Label lblMeasTotal;

	private Label lblMeasCreated;

	private Label lblMeasRemain;

	private Label lblTotal;

	private Label lblCreated;

	private Label lblRemain;

	public frmPlanView(Form frm, Guid id = default(Guid))
	{
		InitializeComponent();
		Common.setControls(this, toolTipMain, new List<ContextMenuStrip> { contextMenuStripMain });
		mForm = frm;
		isClose = true;
		mId = Guid.Empty;
		mIDParent = id;
		isEdit = false;
		mRow = 0;
		mCol = 0;
		load_Product();
	}

	private void frmPlanView_Load(object sender, EventArgs e)
	{
		Init();
	}

	private void frmPlanView_Shown(object sender, EventArgs e)
	{
		load_AllData();
	}

	private void frmPlanView_FormClosing(object sender, FormClosingEventArgs e)
	{
		GC.Collect();
	}

	private void frmPlanView_FormClosed(object sender, FormClosedEventArgs e)
	{
		mPanelViewMain.mDispose();
		List<Type> list = new List<Type>();
		list.Add(typeof(frmPlanAdd));
		list.Add(typeof(frmPlanDetailView));
		Common.closeForm(list);
		if (!isClose)
		{
			((frmProductView)mForm).load_AllData();
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
		mPanelViewMain.btnView.Click += main_viewToolStripMenuItem_Click;
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
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "ProductId=@0";
			queryArgs.PredicateParameters = new string[1] { mIDParent.ToString() };
			queryArgs.Order = "Sort, Created";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs body = queryArgs;
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/Plan/Gets").Result;
			mSearchMain.Init(Common.getDataTable<PlanViewModel>(result), dgvMain);
			load_dgvMain();
			load_Information();
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

	public void load_Product()
	{
		try
		{
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "Id=@0";
			queryArgs.PredicateParameters = new string[1] { mIDParent.ToString() };
			queryArgs.Order = "Created DESC";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs body = queryArgs;
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/Product/Gets").Result;
			DataTable dataTable = Common.getDataTable<ProductViewModel>(result);
			if (dataTable.Rows.Count > 0)
			{
				mProduct = Common.Cast<ProductViewModel>(dataTable.Rows[0]);
				Text = Text + " (" + mProduct.Code + "#" + mProduct.Name + ")";
				mTotalMeas = int.Parse(dataTable.Rows[0]["TotalMeas"].ToString());
				lblTotal.Text = mTotalMeas.ToString();
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

	public void load_Information()
	{
		int num = 0;
		foreach (DataRow row in mSearchMain.dataTable.Rows)
		{
			num += row.Field<int>("TotalDetails");
		}
		lblCreated.Text = num.ToString();
		lblRemain.Text = (mTotalMeas - num).ToString();
	}

	private void en_disControl(bool enable)
	{
		mPanelViewMain.btnEdit.Enabled = enable;
		mPanelViewMain.btnDelete.Enabled = enable;
		main_newToolStripMenuItem.Enabled = enable;
		main_editToolStripMenuItem.Enabled = enable;
		main_deleteToolStripMenuItem.Enabled = enable;
		main_moveToolStripMenuItem.Enabled = enable;
		main_viewToolStripMenuItem.Enabled = enable;
		mSearchMain.Enabled = true;
	}

	private void disControlMove()
	{
		mPanelViewMain.btnEdit.Enabled = false;
		mPanelViewMain.btnDelete.Enabled = false;
		main_newToolStripMenuItem.Enabled = false;
		main_editToolStripMenuItem.Enabled = false;
		main_deleteToolStripMenuItem.Enabled = false;
		main_viewToolStripMenuItem.Enabled = false;
		mSearchMain.Enabled = false;
	}

	private void updateMove()
	{
		Cursor.Current = Cursors.WaitCursor;
		try
		{
			start_Proccessor();
			ResponseDto result = frmLogin.client.MoveAsync(IdFrom, IdTo, "/api/Plan/Move/{idfrom}/{idto}").Result;
			IdFrom = Guid.Empty;
			IdTo = Guid.Empty;
			if (!result.Success)
			{
				throw new Exception(result.Messages.ElementAt(0).Message);
			}
			isClose = false;
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
			if (!IdFrom.Equals(Guid.Empty))
			{
				IdTo = mId;
			}
		}
		else
		{
			mId = Guid.Empty;
			IdFrom = Guid.Empty;
			IdTo = Guid.Empty;
			dgvMain.Cursor = Cursors.Default;
		}
	}

	private void dgvMain_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		mPanelViewMain.Visible = false;
		if (!IdTo.Equals(Guid.Empty) && !IdFrom.Equals(Guid.Empty) && !IdTo.Equals(IdFrom))
		{
			updateMove();
		}
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
			if (int.Parse(item.Cells["TotalDetails"].Value.ToString()) == 0)
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
		if (int.Parse(lblRemain.Text) <= 0)
		{
			MessageBox.Show(Common.getTextLanguage(this, "wFullMeasurement"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		List<Type> list = new List<Type>();
		list.Add(typeof(frmPlanAdd));
		Common.closeForm(list);
		new frmPlanAdd(this, (DataTable)dgvMain.DataSource, mProduct, mId).Show();
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
		list.Add(typeof(frmPlanAdd));
		list.Add(typeof(frmPlanDetailView));
		Common.closeForm(list);
		new frmPlanAdd(this, (DataTable)dgvMain.DataSource, mProduct, mId, isadd: false).Show();
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
			if (!MessageBox.Show(string.Format("{0} {1}", Common.getTextLanguage(this, "wSureDelete"), dgvMain.CurrentRow.Cells["StageName"].Value), Common.getTextLanguage(this, "QUESTION"), MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.Yes))
			{
				return;
			}
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				start_Proccessor();
				ResponseDto result = frmLogin.client.DeleteAsync(mId, "/api/Plan/Delete/{id}").Result;
				if (!result.Success)
				{
					throw new Exception(result.Messages.ElementAt(0).Message);
				}
				isClose = false;
				List<Type> list = new List<Type>();
				list.Add(typeof(frmPlanAdd));
				list.Add(typeof(frmPlanDetailView));
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

	private void main_viewToolStripMenuItem_Click(object sender, EventArgs e)
	{
		mSearchMain.btnSearch.Select();
		Cursor.Current = Cursors.WaitCursor;
		if (mId.Equals(Guid.Empty))
		{
			MessageBox.Show(Common.getTextLanguage(this, "wSelectRow"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		List<Type> list = new List<Type>();
		list.Add(typeof(frmPlanDetailView));
		Common.closeForm(list);
		new frmPlanDetailView(dgvMain.CurrentRow).Show();
	}

	private void main_moveToolStripMenuItem_Click(object sender, EventArgs e)
	{
		mSearchMain.btnSearch.Select();
		Cursor.Current = Cursors.WaitCursor;
		if (mIDParent.Equals(Guid.Empty) || mId.Equals(Guid.Empty))
		{
			MessageBox.Show(Common.getTextLanguage(this, "wSelectRow"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		IdFrom = mId;
		dgvMain.Cursor = Cursors.NoMoveVert;
		disControlMove();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_5S_QA_System.frmPlanView));
		this.contextMenuStripMain = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.main_refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.main_newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.main_editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.main_viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.main_deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.main_moveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.statusStripfrmMain = new System.Windows.Forms.StatusStrip();
		this.sprogbarStatus = new System.Windows.Forms.ToolStripProgressBar();
		this.slblStatus = new System.Windows.Forms.ToolStripStatusLabel();
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.dgvMain = new System.Windows.Forms.DataGridView();
		this.No = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ProductId = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ProductCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.StageId = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.StageName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.TemplateId = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.TemplateName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.TotalDetails = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Created = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Modified = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.CreatedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ModifiedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.IsActivated = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.tpanelMain = new System.Windows.Forms.TableLayoutPanel();
		this.lblMeasTotal = new System.Windows.Forms.Label();
		this.mPanelViewMain = new _5S_QA_System.View.User_control.mPanelView();
		this.mSearchMain = new _5S_QA_System.View.User_control.mSearch();
		this.panelLogout = new System.Windows.Forms.Panel();
		this.lblFullname = new System.Windows.Forms.Label();
		this.ptbAvatar = new System.Windows.Forms.PictureBox();
		this.lblMeasCreated = new System.Windows.Forms.Label();
		this.lblMeasRemain = new System.Windows.Forms.Label();
		this.lblTotal = new System.Windows.Forms.Label();
		this.lblCreated = new System.Windows.Forms.Label();
		this.lblRemain = new System.Windows.Forms.Label();
		this.contextMenuStripMain.SuspendLayout();
		this.statusStripfrmMain.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvMain).BeginInit();
		this.tpanelMain.SuspendLayout();
		this.panelLogout.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.ptbAvatar).BeginInit();
		base.SuspendLayout();
		this.contextMenuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.main_refreshToolStripMenuItem, this.toolStripSeparator1, this.main_newToolStripMenuItem, this.main_editToolStripMenuItem, this.main_viewToolStripMenuItem, this.toolStripSeparator6, this.main_deleteToolStripMenuItem, this.toolStripSeparator2, this.main_moveToolStripMenuItem });
		this.contextMenuStripMain.Name = "contextMenuStripStaff";
		this.contextMenuStripMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
		this.contextMenuStripMain.Size = new System.Drawing.Size(132, 154);
		this.main_refreshToolStripMenuItem.Name = "main_refreshToolStripMenuItem";
		this.main_refreshToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
		this.main_refreshToolStripMenuItem.Text = "Refresh";
		this.main_refreshToolStripMenuItem.Click += new System.EventHandler(main_refreshToolStripMenuItem_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(128, 6);
		this.main_newToolStripMenuItem.Name = "main_newToolStripMenuItem";
		this.main_newToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
		this.main_newToolStripMenuItem.Text = "New";
		this.main_newToolStripMenuItem.Click += new System.EventHandler(main_newToolStripMenuItem_Click);
		this.main_editToolStripMenuItem.Name = "main_editToolStripMenuItem";
		this.main_editToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
		this.main_editToolStripMenuItem.Text = "Edit";
		this.main_editToolStripMenuItem.Click += new System.EventHandler(main_editToolStripMenuItem_Click);
		this.main_viewToolStripMenuItem.Name = "main_viewToolStripMenuItem";
		this.main_viewToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
		this.main_viewToolStripMenuItem.Text = "View detail";
		this.main_viewToolStripMenuItem.Click += new System.EventHandler(main_viewToolStripMenuItem_Click);
		this.toolStripSeparator6.Name = "toolStripSeparator6";
		this.toolStripSeparator6.Size = new System.Drawing.Size(128, 6);
		this.main_deleteToolStripMenuItem.Name = "main_deleteToolStripMenuItem";
		this.main_deleteToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
		this.main_deleteToolStripMenuItem.Text = "Delete";
		this.main_deleteToolStripMenuItem.Click += new System.EventHandler(main_deleteToolStripMenuItem_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(128, 6);
		this.main_moveToolStripMenuItem.Name = "main_moveToolStripMenuItem";
		this.main_moveToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
		this.main_moveToolStripMenuItem.Text = "Move";
		this.main_moveToolStripMenuItem.Click += new System.EventHandler(main_moveToolStripMenuItem_Click);
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
		this.statusStripfrmMain.TabIndex = 31;
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
		this.dgvMain.Columns.AddRange(this.No, this.ProductId, this.ProductCode, this.ProductName, this.StageId, this.StageName, this.TemplateId, this.TemplateName, this.TotalDetails, this.Id, this.Created, this.Modified, this.CreatedBy, this.ModifiedBy, this.IsActivated);
		this.dgvMain.ContextMenuStrip = this.contextMenuStripMain;
		this.dgvMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.dgvMain.EnableHeadersVisualStyles = false;
		this.dgvMain.Location = new System.Drawing.Point(20, 134);
		this.dgvMain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.dgvMain.MultiSelect = false;
		this.dgvMain.Name = "dgvMain";
		this.dgvMain.ReadOnly = true;
		this.dgvMain.RowHeadersWidth = 25;
		this.dgvMain.Size = new System.Drawing.Size(1060, 402);
		this.dgvMain.TabIndex = 30;
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
		this.ProductId.DataPropertyName = "ProductId";
		this.ProductId.HeaderText = "Product id";
		this.ProductId.Name = "ProductId";
		this.ProductId.ReadOnly = true;
		this.ProductId.Visible = false;
		this.ProductCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.ProductCode.DataPropertyName = "ProductCode";
		this.ProductCode.FillWeight = 20f;
		this.ProductCode.HeaderText = "Product code";
		this.ProductCode.Name = "ProductCode";
		this.ProductCode.ReadOnly = true;
		this.ProductCode.Visible = false;
		this.ProductName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.ProductName.DataPropertyName = "ProductName";
		this.ProductName.FillWeight = 30f;
		this.ProductName.HeaderText = "Product name";
		this.ProductName.Name = "ProductName";
		this.ProductName.ReadOnly = true;
		this.ProductName.Visible = false;
		this.StageId.DataPropertyName = "StageId";
		this.StageId.FillWeight = 30f;
		this.StageId.HeaderText = "Stage id";
		this.StageId.Name = "StageId";
		this.StageId.ReadOnly = true;
		this.StageId.Visible = false;
		this.StageId.Width = 267;
		this.StageName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.StageName.DataPropertyName = "StageName";
		this.StageName.FillWeight = 40f;
		this.StageName.HeaderText = "Stage name";
		this.StageName.Name = "StageName";
		this.StageName.ReadOnly = true;
		this.TemplateId.DataPropertyName = "TemplateId";
		this.TemplateId.HeaderText = "Template id";
		this.TemplateId.Name = "TemplateId";
		this.TemplateId.ReadOnly = true;
		this.TemplateId.Visible = false;
		this.TemplateName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.TemplateName.DataPropertyName = "TemplateName";
		this.TemplateName.FillWeight = 50f;
		this.TemplateName.HeaderText = "Template name";
		this.TemplateName.Name = "TemplateName";
		this.TemplateName.ReadOnly = true;
		this.TotalDetails.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.TotalDetails.DataPropertyName = "TotalDetails";
		this.TotalDetails.FillWeight = 10f;
		this.TotalDetails.HeaderText = "Meas. q.ty";
		this.TotalDetails.Name = "TotalDetails";
		this.TotalDetails.ReadOnly = true;
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
		this.tpanelMain.AutoSize = true;
		this.tpanelMain.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		this.tpanelMain.ColumnCount = 6;
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334f));
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334f));
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334f));
		this.tpanelMain.Controls.Add(this.lblRemain, 5, 0);
		this.tpanelMain.Controls.Add(this.lblMeasRemain, 4, 0);
		this.tpanelMain.Controls.Add(this.lblCreated, 3, 0);
		this.tpanelMain.Controls.Add(this.lblMeasCreated, 2, 0);
		this.tpanelMain.Controls.Add(this.lblTotal, 1, 0);
		this.tpanelMain.Controls.Add(this.lblMeasTotal, 0, 0);
		this.tpanelMain.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.tpanelMain.Location = new System.Drawing.Point(20, 536);
		this.tpanelMain.Name = "tpanelMain";
		this.tpanelMain.RowCount = 1;
		this.tpanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tpanelMain.Size = new System.Drawing.Size(1060, 18);
		this.tpanelMain.TabIndex = 36;
		this.lblMeasTotal.AutoSize = true;
		this.lblMeasTotal.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblMeasTotal.Location = new System.Drawing.Point(4, 1);
		this.lblMeasTotal.Name = "lblMeasTotal";
		this.lblMeasTotal.Size = new System.Drawing.Size(123, 16);
		this.lblMeasTotal.TabIndex = 177;
		this.lblMeasTotal.Text = "Total measurement";
		this.lblMeasTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.mPanelViewMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.mPanelViewMain.Dock = System.Windows.Forms.DockStyle.Right;
		this.mPanelViewMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.mPanelViewMain.Location = new System.Drawing.Point(630, 134);
		this.mPanelViewMain.Margin = new System.Windows.Forms.Padding(0);
		this.mPanelViewMain.Name = "mPanelViewMain";
		this.mPanelViewMain.Size = new System.Drawing.Size(450, 402);
		this.mPanelViewMain.TabIndex = 35;
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
		this.mSearchMain.TabIndex = 34;
		this.panelLogout.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.panelLogout.BackColor = System.Drawing.Color.Transparent;
		this.panelLogout.Controls.Add(this.lblFullname);
		this.panelLogout.Controls.Add(this.ptbAvatar);
		this.panelLogout.Location = new System.Drawing.Point(730, 27);
		this.panelLogout.Name = "panelLogout";
		this.panelLogout.Size = new System.Drawing.Size(350, 42);
		this.panelLogout.TabIndex = 176;
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
		this.lblMeasCreated.AutoSize = true;
		this.lblMeasCreated.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblMeasCreated.Location = new System.Drawing.Point(375, 1);
		this.lblMeasCreated.Name = "lblMeasCreated";
		this.lblMeasCreated.Size = new System.Drawing.Size(138, 16);
		this.lblMeasCreated.TabIndex = 177;
		this.lblMeasCreated.Text = "Measurement created";
		this.lblMeasCreated.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblMeasRemain.AutoSize = true;
		this.lblMeasRemain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblMeasRemain.Location = new System.Drawing.Point(761, 1);
		this.lblMeasRemain.Name = "lblMeasRemain";
		this.lblMeasRemain.Size = new System.Drawing.Size(54, 16);
		this.lblMeasRemain.TabIndex = 177;
		this.lblMeasRemain.Text = "Remain";
		this.lblMeasRemain.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblTotal.AutoSize = true;
		this.lblTotal.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotal.Location = new System.Drawing.Point(134, 1);
		this.lblTotal.Name = "lblTotal";
		this.lblTotal.Size = new System.Drawing.Size(234, 16);
		this.lblTotal.TabIndex = 177;
		this.lblTotal.Text = "0";
		this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblCreated.AutoSize = true;
		this.lblCreated.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCreated.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCreated.ForeColor = System.Drawing.Color.Blue;
		this.lblCreated.Location = new System.Drawing.Point(520, 1);
		this.lblCreated.Name = "lblCreated";
		this.lblCreated.Size = new System.Drawing.Size(234, 16);
		this.lblCreated.TabIndex = 178;
		this.lblCreated.Text = "0";
		this.lblCreated.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblRemain.AutoSize = true;
		this.lblRemain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblRemain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblRemain.ForeColor = System.Drawing.Color.Red;
		this.lblRemain.Location = new System.Drawing.Point(822, 1);
		this.lblRemain.Name = "lblRemain";
		this.lblRemain.Size = new System.Drawing.Size(234, 16);
		this.lblRemain.TabIndex = 179;
		this.lblRemain.Text = "0";
		this.lblRemain.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1100, 600);
		base.Controls.Add(this.panelLogout);
		base.Controls.Add(this.mPanelViewMain);
		base.Controls.Add(this.dgvMain);
		base.Controls.Add(this.mSearchMain);
		base.Controls.Add(this.tpanelMain);
		base.Controls.Add(this.statusStripfrmMain);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "frmPlanView";
		base.Padding = new System.Windows.Forms.Padding(20, 70, 20, 20);
		this.Text = "5S QA System * PLAN";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmPlanView_FormClosing);
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmPlanView_FormClosed);
		base.Load += new System.EventHandler(frmPlanView_Load);
		base.Shown += new System.EventHandler(frmPlanView_Shown);
		this.contextMenuStripMain.ResumeLayout(false);
		this.statusStripfrmMain.ResumeLayout(false);
		this.statusStripfrmMain.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvMain).EndInit();
		this.tpanelMain.ResumeLayout(false);
		this.tpanelMain.PerformLayout();
		this.panelLogout.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.ptbAvatar).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
