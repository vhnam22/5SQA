using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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

public class frmPlanDetailView : MetroForm
{
	private Guid mId;

	private Guid mIDParent;

	private bool isEdit;

	private int mRow;

	private int mCol;

	private IContainer components = null;

	private mSearch mSearchMain;

	private ContextMenuStrip contextMenuStripMain;

	private ToolStripMenuItem main_refreshToolStripMenuItem;

	private ToolTip toolTipMain;

	public ToolStripStatusLabel slblStatus;

	private ToolStripProgressBar sprogbarStatus;

	private StatusStrip statusStripfrmMain;

	private mPanelView mPanelViewMain;

	private DataGridView dgvMain;

	private DataGridViewTextBoxColumn No;

	private DataGridViewTextBoxColumn Code;

	private DataGridViewTextBoxColumn name;

	private DataGridViewTextBoxColumn ImportantId;

	private DataGridViewTextBoxColumn ImportantName;

	private DataGridViewTextBoxColumn Value;

	private DataGridViewTextBoxColumn UnitId;

	private DataGridViewTextBoxColumn UnitName;

	private DataGridViewTextBoxColumn Upper;

	private DataGridViewTextBoxColumn Lower;

	private DataGridViewTextBoxColumn UCL;

	private DataGridViewTextBoxColumn LCL;

	private DataGridViewTextBoxColumn Formula;

	private DataGridViewTextBoxColumn MachineTypeId;

	private DataGridViewTextBoxColumn MachineTypeName;

	private DataGridViewTextBoxColumn TemplateId;

	private DataGridViewTextBoxColumn TemplateName;

	private DataGridViewTextBoxColumn ProductId;

	private DataGridViewTextBoxColumn Coordinate;

	private DataGridViewTextBoxColumn Id;

	private new DataGridViewTextBoxColumn Created;

	private DataGridViewTextBoxColumn Modified;

	private DataGridViewTextBoxColumn CreatedBy;

	private DataGridViewTextBoxColumn ModifiedBy;

	private DataGridViewTextBoxColumn IsActivated;

	private Panel panelLogout;

	private Label lblFullname;

	private PictureBox ptbAvatar;

	public frmPlanDetailView(DataGridViewRow row)
	{
		InitializeComponent();
		Common.setControls(this, toolTipMain, new List<ContextMenuStrip> { contextMenuStripMain });
		mIDParent = new Guid(row.Cells["Id"].Value.ToString());
		mId = Guid.Empty;
		isEdit = false;
		mRow = 0;
		mCol = 0;
		Text += string.Format(" ({0}#{1})", row.Cells["ProductCode"].Value, row.Cells["StageName"].Value);
	}

	private void frmPlanDetailView_Load(object sender, EventArgs e)
	{
		Init();
	}

	private void frmPlanDetailView_Shown(object sender, EventArgs e)
	{
		load_AllData();
	}

	private void frmPlanDetailView_FormClosing(object sender, FormClosingEventArgs e)
	{
		GC.Collect();
	}

	private void frmPlanDetailView_FormClosed(object sender, FormClosedEventArgs e)
	{
		mPanelViewMain.mDispose();
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
			queryArgs.Predicate = "PlanId=@0";
			queryArgs.PredicateParameters = new string[1] { mIDParent.ToString() };
			queryArgs.Order = "Measurement.Sort, Measurement.Created";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs body = queryArgs;
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/PlanDetail/Gets").Result;
			mSearchMain.Init(Common.getDataTable<MeasurementViewModel>(result), dgvMain);
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
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_5S_QA_System.frmPlanDetailView));
		this.contextMenuStripMain = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.main_refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.slblStatus = new System.Windows.Forms.ToolStripStatusLabel();
		this.sprogbarStatus = new System.Windows.Forms.ToolStripProgressBar();
		this.statusStripfrmMain = new System.Windows.Forms.StatusStrip();
		this.dgvMain = new System.Windows.Forms.DataGridView();
		this.No = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ImportantId = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ImportantName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.UnitId = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.UnitName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Upper = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Lower = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.UCL = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.LCL = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Formula = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.MachineTypeId = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.MachineTypeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.TemplateId = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.TemplateName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ProductId = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Coordinate = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Created = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Modified = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.CreatedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ModifiedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.IsActivated = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.mPanelViewMain = new _5S_QA_System.View.User_control.mPanelView();
		this.mSearchMain = new _5S_QA_System.View.User_control.mSearch();
		this.panelLogout = new System.Windows.Forms.Panel();
		this.lblFullname = new System.Windows.Forms.Label();
		this.ptbAvatar = new System.Windows.Forms.PictureBox();
		this.contextMenuStripMain.SuspendLayout();
		this.statusStripfrmMain.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvMain).BeginInit();
		this.panelLogout.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.ptbAvatar).BeginInit();
		base.SuspendLayout();
		this.contextMenuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.main_refreshToolStripMenuItem });
		this.contextMenuStripMain.Name = "contextMenuStripStaff";
		this.contextMenuStripMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
		this.contextMenuStripMain.Size = new System.Drawing.Size(114, 26);
		this.main_refreshToolStripMenuItem.Name = "main_refreshToolStripMenuItem";
		this.main_refreshToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
		this.main_refreshToolStripMenuItem.Text = "Refresh";
		this.main_refreshToolStripMenuItem.Click += new System.EventHandler(main_refreshToolStripMenuItem_Click);
		this.slblStatus.Name = "slblStatus";
		this.slblStatus.Size = new System.Drawing.Size(16, 21);
		this.slblStatus.Text = "...";
		this.sprogbarStatus.Name = "sprogbarStatus";
		this.sprogbarStatus.Size = new System.Drawing.Size(200, 20);
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
		this.statusStripfrmMain.TabIndex = 37;
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
		this.dgvMain.Columns.AddRange(this.No, this.Code, this.name, this.ImportantId, this.ImportantName, this.Value, this.UnitId, this.UnitName, this.Upper, this.Lower, this.UCL, this.LCL, this.Formula, this.MachineTypeId, this.MachineTypeName, this.TemplateId, this.TemplateName, this.ProductId, this.Coordinate, this.Id, this.Created, this.Modified, this.CreatedBy, this.ModifiedBy, this.IsActivated);
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
		this.dgvMain.TabIndex = 42;
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
		this.Code.FillWeight = 25f;
		this.Code.HeaderText = "Code";
		this.Code.Name = "Code";
		this.Code.ReadOnly = true;
		this.name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.name.DataPropertyName = "Name";
		this.name.FillWeight = 30f;
		this.name.HeaderText = "Name";
		this.name.Name = "name";
		this.name.ReadOnly = true;
		this.ImportantId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.ImportantId.DataPropertyName = "ImportantId";
		this.ImportantId.FillWeight = 30f;
		this.ImportantId.HeaderText = "Important id";
		this.ImportantId.Name = "ImportantId";
		this.ImportantId.ReadOnly = true;
		this.ImportantId.Visible = false;
		this.ImportantName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.ImportantName.DataPropertyName = "ImportantName";
		this.ImportantName.FillWeight = 20f;
		this.ImportantName.HeaderText = "Important";
		this.ImportantName.Name = "ImportantName";
		this.ImportantName.ReadOnly = true;
		this.Value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Value.DataPropertyName = "Value";
		dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.Value.DefaultCellStyle = dataGridViewCellStyle4;
		this.Value.FillWeight = 20f;
		this.Value.HeaderText = "Value";
		this.Value.Name = "Value";
		this.Value.ReadOnly = true;
		this.UnitId.DataPropertyName = "UnitId";
		this.UnitId.HeaderText = "Unit id";
		this.UnitId.Name = "UnitId";
		this.UnitId.ReadOnly = true;
		this.UnitId.Visible = false;
		this.UnitName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.UnitName.DataPropertyName = "UnitName";
		this.UnitName.FillWeight = 15f;
		this.UnitName.HeaderText = "Unit";
		this.UnitName.Name = "UnitName";
		this.UnitName.ReadOnly = true;
		this.Upper.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Upper.DataPropertyName = "Upper";
		dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.Upper.DefaultCellStyle = dataGridViewCellStyle5;
		this.Upper.FillWeight = 20f;
		this.Upper.HeaderText = "Upper";
		this.Upper.Name = "Upper";
		this.Upper.ReadOnly = true;
		this.Lower.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Lower.DataPropertyName = "Lower";
		dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.Lower.DefaultCellStyle = dataGridViewCellStyle6;
		this.Lower.FillWeight = 20f;
		this.Lower.HeaderText = "Lower";
		this.Lower.Name = "Lower";
		this.Lower.ReadOnly = true;
		this.UCL.DataPropertyName = "UCL";
		this.UCL.HeaderText = "UCL";
		this.UCL.Name = "UCL";
		this.UCL.ReadOnly = true;
		this.UCL.Visible = false;
		this.LCL.DataPropertyName = "LCL";
		this.LCL.HeaderText = "LCL";
		this.LCL.Name = "LCL";
		this.LCL.ReadOnly = true;
		this.LCL.Visible = false;
		this.Formula.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Formula.DataPropertyName = "Formula";
		this.Formula.FillWeight = 20f;
		this.Formula.HeaderText = "Formula";
		this.Formula.Name = "Formula";
		this.Formula.ReadOnly = true;
		this.MachineTypeId.DataPropertyName = "MachineTypeId";
		this.MachineTypeId.HeaderText = "Machine type id";
		this.MachineTypeId.Name = "MachineTypeId";
		this.MachineTypeId.ReadOnly = true;
		this.MachineTypeId.Visible = false;
		this.MachineTypeName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.MachineTypeName.DataPropertyName = "MachineTypeName";
		this.MachineTypeName.FillWeight = 30f;
		this.MachineTypeName.HeaderText = "Mac. type";
		this.MachineTypeName.Name = "MachineTypeName";
		this.MachineTypeName.ReadOnly = true;
		this.TemplateId.DataPropertyName = "TemplateId";
		this.TemplateId.HeaderText = "Template id";
		this.TemplateId.Name = "TemplateId";
		this.TemplateId.ReadOnly = true;
		this.TemplateId.Visible = false;
		this.TemplateName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.TemplateName.DataPropertyName = "TemplateName";
		this.TemplateName.FillWeight = 30f;
		this.TemplateName.HeaderText = "Template";
		this.TemplateName.Name = "TemplateName";
		this.TemplateName.ReadOnly = true;
		this.TemplateName.Visible = false;
		this.ProductId.DataPropertyName = "ProductId";
		this.ProductId.HeaderText = "Product id";
		this.ProductId.Name = "ProductId";
		this.ProductId.ReadOnly = true;
		this.ProductId.Visible = false;
		this.Coordinate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Coordinate.DataPropertyName = "Coordinate";
		this.Coordinate.FillWeight = 30f;
		this.Coordinate.HeaderText = "Coordinate";
		this.Coordinate.Name = "Coordinate";
		this.Coordinate.ReadOnly = true;
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
		this.mPanelViewMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.mPanelViewMain.Dock = System.Windows.Forms.DockStyle.Right;
		this.mPanelViewMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.mPanelViewMain.Location = new System.Drawing.Point(630, 134);
		this.mPanelViewMain.Margin = new System.Windows.Forms.Padding(0);
		this.mPanelViewMain.Name = "mPanelViewMain";
		this.mPanelViewMain.Size = new System.Drawing.Size(450, 420);
		this.mPanelViewMain.TabIndex = 41;
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
		this.mSearchMain.TabIndex = 40;
		this.panelLogout.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.panelLogout.BackColor = System.Drawing.Color.Transparent;
		this.panelLogout.Controls.Add(this.lblFullname);
		this.panelLogout.Controls.Add(this.ptbAvatar);
		this.panelLogout.Location = new System.Drawing.Point(730, 27);
		this.panelLogout.Name = "panelLogout";
		this.panelLogout.Size = new System.Drawing.Size(350, 42);
		this.panelLogout.TabIndex = 177;
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
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1100, 600);
		base.Controls.Add(this.panelLogout);
		base.Controls.Add(this.mPanelViewMain);
		base.Controls.Add(this.dgvMain);
		base.Controls.Add(this.statusStripfrmMain);
		base.Controls.Add(this.mSearchMain);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "frmPlanDetailView";
		base.Padding = new System.Windows.Forms.Padding(20, 70, 20, 20);
		this.Text = "5S QA System * PLAN DETAIL";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmPlanDetailView_FormClosing);
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmPlanDetailView_FormClosed);
		base.Load += new System.EventHandler(frmPlanDetailView_Load);
		base.Shown += new System.EventHandler(frmPlanDetailView_Shown);
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
