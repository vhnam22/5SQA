using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using _5S_QA_DL.APIContext;
using _5S_QA_Entities.Abstracts;
using _5S_QA_Entities.Dtos;
using _5S_QA_Entities.Enums;
using _5S_QA_Entities.Models;
using _5S_QA_System.Controls;
using MetroFramework.Forms;

namespace _5S_QA_System;

public class frmPlanAdd : MetroForm
{
	private readonly Form mForm;

	private Guid mId;

	private readonly ProductViewModel mProduct;

	private readonly DataTable mData;

	private string mRemember;

	public bool isClose;

	private readonly bool isAdd;

	private IContainer components = null;

	private DataGridView dgvMain;

	private ContextMenuStrip contextMenuStripMain;

	private ToolStripMenuItem main_allToolStripMenuItem;

	private ToolStripMenuItem main_unallToolStripMenuItem;

	private ToolTip toolTipMain;

	private Panel panelMain;

	private Panel panelMainResize;

	private TableLayoutPanel tableLayoutPanel2;

	private Button btnConfirm;

	private TableLayoutPanel tableLayoutPanel1;

	private Label lblSelectTotal;

	private Label lblSelect;

	private Label lblMeas;

	private Label lblTotalRow;

	private Label lblMeasTotal;

	private DataGridViewCheckBoxColumn IsSelect;

	private DataGridViewTextBoxColumn No;

	private DataGridViewTextBoxColumn Code;

	private DataGridViewTextBoxColumn name;

	private DataGridViewTextBoxColumn ImportantName;

	private DataGridViewTextBoxColumn Value;

	private DataGridViewTextBoxColumn UnitName;

	private DataGridViewTextBoxColumn Upper;

	private DataGridViewTextBoxColumn Lower;

	private DataGridViewTextBoxColumn MachineTypeName;

	private DataGridViewTextBoxColumn Id;

	private Label lblTemplate;

	private Label lblStage;

	private Label lblCreate;

	private Label lblModifi;

	private Label lblCreateBy;

	private Label lblModifiBy;

	private Label lblCreated;

	private Label lblModified;

	private Label lblCreatedBy;

	private Label lblModifiedBy;

	private Panel panel1;

	private ComboBox cbbStage;

	private Button btnStageAdd;

	private Panel panel2;

	private ComboBox cbbTemplate;

	private Button btnTemplateAdd;

	public frmPlanAdd(Form frm, DataTable data, ProductViewModel product, Guid id = default(Guid), bool isadd = true)
	{
		InitializeComponent();
		Common.setControls(this, toolTipMain, new List<ContextMenuStrip> { contextMenuStripMain });
		mForm = frm;
		isClose = true;
		mData = data;
		mId = id;
		mProduct = product;
		isAdd = isadd;
		string text = "ADD";
		if (!isAdd)
		{
			text = "EDIT";
		}
		string text2 = Common.getTextLanguage(this, text);
		if (string.IsNullOrEmpty(text2))
		{
			text2 = text;
		}
		Text = Text + " (" + text2 + ")";
		if (!isAdd)
		{
			Text = Text + " (" + mProduct.Code + "#" + mProduct.Name + ")";
		}
		else
		{
			Text = Text + " (" + mProduct.Code + "#" + mProduct.Name + ")";
		}
	}

	private void frmPlanAdd_Load(object sender, EventArgs e)
	{
		Init();
	}

	private void frmPlanAdd_Shown(object sender, EventArgs e)
	{
		load_cbbStage();
		load_cbbTemplate();
		load_Measurement();
		if (isAdd)
		{
			cbbTemplate.SelectedIndex = -1;
		}
		load_Data();
		int selectedIndex = cbbTemplate.SelectedIndex;
		cbbTemplate.SelectedIndex = selectedIndex;
	}

	private void frmPlanAdd_FormClosing(object sender, FormClosingEventArgs e)
	{
		GC.Collect();
	}

	private void frmPlanAdd_FormClosed(object sender, FormClosedEventArgs e)
	{
		List<Type> list = new List<Type>();
		list.Add(typeof(frmSubView));
		list.Add(typeof(frmTemplateView));
		Common.closeForm(list);
		mData?.Dispose();
		if (!isClose)
		{
			((frmPlanView)mForm).isClose = false;
			((frmPlanView)mForm).load_AllData();
		}
	}

	private void Init()
	{
		lblCreated.Text = "";
		lblModified.Text = "";
		lblCreatedBy.Text = "";
		lblModifiedBy.Text = "";
	}

	public void load_cbbStage()
	{
		try
		{
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "TypeId=@0";
			queryArgs.PredicateParameters = new string[1] { "11C5FD56-AD45-4457-8DC9-6C8D9F6673A1" };
			queryArgs.Order = "Created DESC";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs body = queryArgs;
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/MetadataValue/Gets").Result;
			DataTable dataTable = Common.getDataTable(result);
			if (dataTable != null)
			{
				dataTable.Dispose();
				cbbStage.ValueMember = "Id";
				cbbStage.DisplayMember = "Name";
				cbbStage.DataSource = dataTable;
			}
			else
			{
				cbbStage.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			cbbStage.DataSource = null;
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
					MessageBox.Show(ex2.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			else
			{
				MessageBox.Show(ex.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
	}

	public void load_cbbTemplate()
	{
		try
		{
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "Type=@0";
			queryArgs.PredicateParameters = new string[1] { "Detail" };
			queryArgs.Order = "Created DESC";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs body = queryArgs;
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/Template/Gets").Result;
			DataTable dataTable = Common.getDataTable(result);
			if (dataTable != null)
			{
				dataTable.Dispose();
				cbbTemplate.ValueMember = "Id";
				cbbTemplate.DisplayMember = "Name";
				cbbTemplate.DataSource = dataTable;
			}
			else
			{
				cbbTemplate.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			cbbTemplate.DataSource = null;
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
					MessageBox.Show(ex2.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			else
			{
				MessageBox.Show(ex.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
	}

	private void load_Data()
	{
		try
		{
			if (mData == null || mData.Rows.Count <= 0 || !(mId != default(Guid)))
			{
				return;
			}
			DataRow dataRow = mData.Select($"Id='{mId}'").FirstOrDefault();
			if (dataRow.ItemArray.Length != 0)
			{
				cbbStage.SelectedValue = dataRow["StageId"];
				if (dataRow["TemplateId"] != null)
				{
					cbbTemplate.SelectedValue = dataRow["TemplateId"];
				}
				else
				{
					cbbTemplate.SelectedIndex = -1;
				}
				if (!isAdd)
				{
					lblCreated.Text = dataRow["Created"].ToString();
					lblModified.Text = dataRow["Modified"].ToString();
					lblCreatedBy.Text = dataRow["CreatedBy"].ToString();
					lblModifiedBy.Text = dataRow["ModifiedBy"].ToString();
				}
			}
		}
		catch
		{
		}
	}

	private void load_Measurement()
	{
		try
		{
			ResponseDto result = frmLogin.client.GetsAsync(((AuditableEntity)(object)mProduct).Id, isAdd ? Guid.Empty : mId, "/api/Measurement/Gets/{productid}/{id}").Result;
			dgvMain.DataSource = Common.getDataTable<MeasurementPlanViewModel>(result);
			dgvMain.Refresh();
			dgvMain.CurrentCell = null;
			lblMeasTotal.Text = dgvMain.RowCount.ToString();
			calSelectTotal();
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
					MessageBox.Show(ex2.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			else
			{
				MessageBox.Show(ex.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
	}

	private void calSelectTotal()
	{
		int num = 0;
		DataTable dataTable = dgvMain.DataSource as DataTable;
		foreach (DataRow row in dataTable.Rows)
		{
			if ((bool)row["IsSelect"])
			{
				num++;
			}
		}
		lblSelectTotal.Text = num.ToString();
	}

	private void btnComfirm_Click(object sender, EventArgs e)
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		//IL_0098: Expected O, but got Unknown
		Cursor.Current = Cursors.WaitCursor;
		try
		{
			if (string.IsNullOrEmpty(cbbStage.Text))
			{
				MessageBox.Show(Common.getTextLanguage(this, "wSelectStage"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				cbbStage.Focus();
				return;
			}
			PlanDto val = new PlanDto
			{
				ProductId = ((AuditableEntity)(object)mProduct).Id,
				StageId = Guid.Parse(cbbStage.SelectedValue.ToString())
			};
			((AuditableEntity)val).IsActivated = true;
			PlanDto val2 = val;
			if (!string.IsNullOrEmpty(cbbTemplate.Text))
			{
				val2.TemplateId = Guid.Parse(cbbTemplate.SelectedValue.ToString());
			}
			DataTable dataTable = dgvMain.DataSource as DataTable;
			foreach (DataRow row in dataTable.Rows)
			{
				if (bool.Parse(row["IsSelect"].ToString()))
				{
					val2.MeasurementIds.Add(new Guid(row["Id"].ToString()));
				}
			}
			if (val2.MeasurementIds.Count.Equals(0))
			{
				MessageBox.Show(Common.getTextLanguage(this, "wIsNull"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				dgvMain.Focus();
			}
			else if (isAdd || !MessageBox.Show(Common.getTextLanguage(this, "wSureEdit"), Common.getTextLanguage(this, "QUESTION"), MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.No))
			{
				Cursor.Current = Cursors.WaitCursor;
				if (!isAdd)
				{
					((AuditableEntity)(object)val2).Id = mId;
				}
				ResponseDto result = frmLogin.client.SaveAsync(val2, "/api/Plan/Save").Result;
				if (!result.Success)
				{
					throw new Exception(result.Messages.ElementAt(0).Message);
				}
				isClose = false;
				Close();
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
					MessageBox.Show(string.IsNullOrEmpty(textLanguage) ? ex.Message : textLanguage, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			else
			{
				MessageBox.Show(string.IsNullOrEmpty(textLanguage) ? ex.Message : textLanguage, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
	}

	private void cbbNormal_Leave(object sender, EventArgs e)
	{
		ComboBox comboBox = sender as ComboBox;
		if (comboBox.SelectedIndex.Equals(-1) && !string.IsNullOrEmpty(comboBox.Text))
		{
			comboBox.Text = mRemember;
		}
	}

	private void cbbNormalNotNull_Leave(object sender, EventArgs e)
	{
		ComboBox comboBox = sender as ComboBox;
		if (comboBox.SelectedIndex.Equals(-1))
		{
			comboBox.Text = mRemember;
		}
	}

	private void cbbNormal_Enter(object sender, EventArgs e)
	{
		ComboBox comboBox = sender as ComboBox;
		mRemember = comboBox.Text;
	}

	private void btnStageAdd_Click(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		List<Type> list = new List<Type>();
		list.Add(typeof(frmSubView));
		Common.closeForm(list);
		new frmSubView(this, (FormType)9).Show();
	}

	private void btnTemplateAdd_Click(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		List<Type> list = new List<Type>();
		list.Add(typeof(frmTemplateView));
		Common.closeForm(list);
		new frmTemplateView(this).Show();
	}

	private void main_allToolStripMenuItem_Click(object sender, EventArgs e)
	{
		if (!(dgvMain.DataSource is DataTable dataTable))
		{
			return;
		}
		foreach (DataRow row in dataTable.Rows)
		{
			row["IsSelect"] = true;
		}
		lblSelectTotal.Text = lblMeasTotal.Text;
	}

	private void main_unallToolStripMenuItem_Click(object sender, EventArgs e)
	{
		if (!(dgvMain.DataSource is DataTable dataTable))
		{
			return;
		}
		foreach (DataRow row in dataTable.Rows)
		{
			row["IsSelect"] = false;
		}
		lblSelectTotal.Text = "0";
	}

	private void dgvMain_CurrentCellDirtyStateChanged(object sender, EventArgs e)
	{
		dgvMain.CurrentCellDirtyStateChanged -= dgvMain_CurrentCellDirtyStateChanged;
		DataGridView dataGridView = sender as DataGridView;
		if (dataGridView.CurrentCell is DataGridViewCheckBoxCell dataGridViewCheckBoxCell)
		{
			int num = int.Parse(lblSelectTotal.Text);
			num = ((!(bool)dataGridViewCheckBoxCell.Value) ? (num + 1) : (num - 1));
			lblSelectTotal.Text = num.ToString();
		}
		dataGridView.EndEdit();
		dgvMain.CurrentCellDirtyStateChanged += dgvMain_CurrentCellDirtyStateChanged;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_5S_QA_System.frmPlanAdd));
		this.dgvMain = new System.Windows.Forms.DataGridView();
		this.IsSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
		this.No = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ImportantName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.UnitName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Upper = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Lower = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.MachineTypeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.contextMenuStripMain = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.main_allToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.main_unallToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.panelMain = new System.Windows.Forms.Panel();
		this.panelMainResize = new System.Windows.Forms.Panel();
		this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.btnConfirm = new System.Windows.Forms.Button();
		this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
		this.lblSelectTotal = new System.Windows.Forms.Label();
		this.lblSelect = new System.Windows.Forms.Label();
		this.lblMeas = new System.Windows.Forms.Label();
		this.lblTotalRow = new System.Windows.Forms.Label();
		this.lblMeasTotal = new System.Windows.Forms.Label();
		this.lblStage = new System.Windows.Forms.Label();
		this.lblTemplate = new System.Windows.Forms.Label();
		this.lblCreate = new System.Windows.Forms.Label();
		this.lblModifi = new System.Windows.Forms.Label();
		this.lblCreateBy = new System.Windows.Forms.Label();
		this.lblModifiBy = new System.Windows.Forms.Label();
		this.lblCreated = new System.Windows.Forms.Label();
		this.lblModified = new System.Windows.Forms.Label();
		this.lblCreatedBy = new System.Windows.Forms.Label();
		this.lblModifiedBy = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.cbbStage = new System.Windows.Forms.ComboBox();
		this.btnStageAdd = new System.Windows.Forms.Button();
		this.panel2 = new System.Windows.Forms.Panel();
		this.cbbTemplate = new System.Windows.Forms.ComboBox();
		this.btnTemplateAdd = new System.Windows.Forms.Button();
		((System.ComponentModel.ISupportInitialize)this.dgvMain).BeginInit();
		this.contextMenuStripMain.SuspendLayout();
		this.panelMain.SuspendLayout();
		this.tableLayoutPanel2.SuspendLayout();
		this.tableLayoutPanel1.SuspendLayout();
		this.panel1.SuspendLayout();
		this.panel2.SuspendLayout();
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
		this.dgvMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
		this.dgvMain.Columns.AddRange(this.IsSelect, this.No, this.Code, this.name, this.ImportantName, this.Value, this.UnitName, this.Upper, this.Lower, this.MachineTypeName, this.Id);
		this.dgvMain.ContextMenuStrip = this.contextMenuStripMain;
		this.dgvMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.dgvMain.EnableHeadersVisualStyles = false;
		this.dgvMain.Location = new System.Drawing.Point(370, 88);
		this.dgvMain.Margin = new System.Windows.Forms.Padding(1);
		this.dgvMain.Name = "dgvMain";
		this.dgvMain.RowHeadersVisible = false;
		this.dgvMain.Size = new System.Drawing.Size(610, 364);
		this.dgvMain.TabIndex = 27;
		this.dgvMain.CurrentCellDirtyStateChanged += new System.EventHandler(dgvMain_CurrentCellDirtyStateChanged);
		this.IsSelect.DataPropertyName = "IsSelect";
		this.IsSelect.FalseValue = "false";
		this.IsSelect.HeaderText = "";
		this.IsSelect.Name = "IsSelect";
		this.IsSelect.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.IsSelect.TrueValue = "true";
		this.IsSelect.Width = 30;
		this.No.DataPropertyName = "No";
		dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		this.No.DefaultCellStyle = dataGridViewCellStyle3;
		this.No.HeaderText = "No";
		this.No.Name = "No";
		this.No.ReadOnly = true;
		this.No.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.No.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.No.Width = 35;
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
		this.ImportantName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.ImportantName.DataPropertyName = "ImportantName";
		this.ImportantName.FillWeight = 20f;
		this.ImportantName.HeaderText = "Important";
		this.ImportantName.Name = "ImportantName";
		this.ImportantName.ReadOnly = true;
		this.ImportantName.Visible = false;
		this.Value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Value.DataPropertyName = "Value";
		dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.Value.DefaultCellStyle = dataGridViewCellStyle4;
		this.Value.FillWeight = 20f;
		this.Value.HeaderText = "Value";
		this.Value.Name = "Value";
		this.Value.ReadOnly = true;
		this.UnitName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.UnitName.DataPropertyName = "UnitName";
		this.UnitName.FillWeight = 20f;
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
		this.MachineTypeName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.MachineTypeName.DataPropertyName = "MachineTypeName";
		this.MachineTypeName.FillWeight = 25f;
		this.MachineTypeName.HeaderText = "Mac. Type";
		this.MachineTypeName.Name = "MachineTypeName";
		this.MachineTypeName.ReadOnly = true;
		this.Id.DataPropertyName = "Id";
		dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.Id.DefaultCellStyle = dataGridViewCellStyle7;
		this.Id.HeaderText = "Id";
		this.Id.Name = "Id";
		this.Id.ReadOnly = true;
		this.Id.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.Id.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.Id.Visible = false;
		this.Id.Width = 140;
		this.contextMenuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.main_allToolStripMenuItem, this.main_unallToolStripMenuItem });
		this.contextMenuStripMain.Name = "contextMenuStripRequestMeas";
		this.contextMenuStripMain.Size = new System.Drawing.Size(127, 48);
		this.main_allToolStripMenuItem.Name = "main_allToolStripMenuItem";
		this.main_allToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
		this.main_allToolStripMenuItem.Text = "Enable all";
		this.main_allToolStripMenuItem.Click += new System.EventHandler(main_allToolStripMenuItem_Click);
		this.main_unallToolStripMenuItem.Name = "main_unallToolStripMenuItem";
		this.main_unallToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
		this.main_unallToolStripMenuItem.Text = "Unable all";
		this.main_unallToolStripMenuItem.Click += new System.EventHandler(main_unallToolStripMenuItem_Click);
		this.panelMain.AutoScroll = true;
		this.panelMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panelMain.Controls.Add(this.tableLayoutPanel2);
		this.panelMain.Controls.Add(this.panelMainResize);
		this.panelMain.Dock = System.Windows.Forms.DockStyle.Left;
		this.panelMain.Location = new System.Drawing.Point(20, 70);
		this.panelMain.Name = "panelMain";
		this.panelMain.Padding = new System.Windows.Forms.Padding(3, 3, 0, 3);
		this.panelMain.Size = new System.Drawing.Size(350, 382);
		this.panelMain.TabIndex = 26;
		this.panelMainResize.Dock = System.Windows.Forms.DockStyle.Right;
		this.panelMainResize.Location = new System.Drawing.Point(345, 3);
		this.panelMainResize.Name = "panelMainResize";
		this.panelMainResize.Size = new System.Drawing.Size(3, 374);
		this.panelMainResize.TabIndex = 16;
		this.tableLayoutPanel2.AutoSize = true;
		this.tableLayoutPanel2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		this.tableLayoutPanel2.ColumnCount = 2;
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel2.Controls.Add(this.panel2, 1, 1);
		this.tableLayoutPanel2.Controls.Add(this.lblModifiedBy, 1, 6);
		this.tableLayoutPanel2.Controls.Add(this.panel1, 1, 0);
		this.tableLayoutPanel2.Controls.Add(this.lblCreatedBy, 1, 5);
		this.tableLayoutPanel2.Controls.Add(this.lblModified, 1, 4);
		this.tableLayoutPanel2.Controls.Add(this.lblCreated, 1, 3);
		this.tableLayoutPanel2.Controls.Add(this.lblModifiBy, 0, 6);
		this.tableLayoutPanel2.Controls.Add(this.lblCreateBy, 0, 5);
		this.tableLayoutPanel2.Controls.Add(this.lblModifi, 0, 4);
		this.tableLayoutPanel2.Controls.Add(this.lblCreate, 0, 3);
		this.tableLayoutPanel2.Controls.Add(this.lblTemplate, 0, 1);
		this.tableLayoutPanel2.Controls.Add(this.lblStage, 0, 0);
		this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
		this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.tableLayoutPanel2.Name = "tableLayoutPanel2";
		this.tableLayoutPanel2.RowCount = 7;
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.Size = new System.Drawing.Size(342, 176);
		this.tableLayoutPanel2.TabIndex = 15;
		this.btnConfirm.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnConfirm.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.btnConfirm.FlatAppearance.BorderSize = 0;
		this.btnConfirm.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
		this.btnConfirm.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
		this.btnConfirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnConfirm.Location = new System.Drawing.Point(20, 452);
		this.btnConfirm.Name = "btnConfirm";
		this.btnConfirm.Size = new System.Drawing.Size(960, 28);
		this.btnConfirm.TabIndex = 1;
		this.btnConfirm.Text = "Confirm";
		this.toolTipMain.SetToolTip(this.btnConfirm, "Confirm");
		this.btnConfirm.UseVisualStyleBackColor = true;
		this.btnConfirm.Click += new System.EventHandler(btnComfirm_Click);
		this.tableLayoutPanel1.AutoSize = true;
		this.tableLayoutPanel1.ColumnCount = 6;
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel1.Controls.Add(this.lblSelectTotal, 3, 0);
		this.tableLayoutPanel1.Controls.Add(this.lblSelect, 2, 0);
		this.tableLayoutPanel1.Controls.Add(this.lblMeas, 0, 0);
		this.tableLayoutPanel1.Controls.Add(this.lblTotalRow, 4, 0);
		this.tableLayoutPanel1.Controls.Add(this.lblMeasTotal, 5, 0);
		this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.tableLayoutPanel1.Location = new System.Drawing.Point(370, 70);
		this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(1);
		this.tableLayoutPanel1.Name = "tableLayoutPanel1";
		this.tableLayoutPanel1.RowCount = 1;
		this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel1.Size = new System.Drawing.Size(610, 18);
		this.tableLayoutPanel1.TabIndex = 155;
		this.lblSelectTotal.AutoSize = true;
		this.lblSelectTotal.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblSelectTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSelectTotal.ForeColor = System.Drawing.Color.Blue;
		this.lblSelectTotal.Location = new System.Drawing.Point(503, 1);
		this.lblSelectTotal.Margin = new System.Windows.Forms.Padding(1);
		this.lblSelectTotal.Name = "lblSelectTotal";
		this.lblSelectTotal.Size = new System.Drawing.Size(15, 16);
		this.lblSelectTotal.TabIndex = 154;
		this.lblSelectTotal.Text = "0";
		this.lblSelect.AutoSize = true;
		this.lblSelect.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSelect.Location = new System.Drawing.Point(453, 1);
		this.lblSelect.Margin = new System.Windows.Forms.Padding(1);
		this.lblSelect.Name = "lblSelect";
		this.lblSelect.Size = new System.Drawing.Size(48, 16);
		this.lblSelect.TabIndex = 153;
		this.lblSelect.Text = "Select:";
		this.lblMeas.AutoSize = true;
		this.lblMeas.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblMeas.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblMeas.Location = new System.Drawing.Point(1, 1);
		this.lblMeas.Margin = new System.Windows.Forms.Padding(1);
		this.lblMeas.Name = "lblMeas";
		this.lblMeas.Size = new System.Drawing.Size(100, 16);
		this.lblMeas.TabIndex = 146;
		this.lblMeas.Text = "Measurement";
		this.lblMeas.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblTotalRow.AutoSize = true;
		this.lblTotalRow.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTotalRow.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotalRow.Location = new System.Drawing.Point(520, 1);
		this.lblTotalRow.Margin = new System.Windows.Forms.Padding(1);
		this.lblTotalRow.Name = "lblTotalRow";
		this.lblTotalRow.Size = new System.Drawing.Size(72, 16);
		this.lblTotalRow.TabIndex = 149;
		this.lblTotalRow.Text = "Total rows:";
		this.lblMeasTotal.AutoSize = true;
		this.lblMeasTotal.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblMeasTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblMeasTotal.Location = new System.Drawing.Point(594, 1);
		this.lblMeasTotal.Margin = new System.Windows.Forms.Padding(1);
		this.lblMeasTotal.Name = "lblMeasTotal";
		this.lblMeasTotal.Size = new System.Drawing.Size(15, 16);
		this.lblMeasTotal.TabIndex = 152;
		this.lblMeasTotal.Text = "0";
		this.lblStage.AutoSize = true;
		this.lblStage.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblStage.Location = new System.Drawing.Point(4, 1);
		this.lblStage.Name = "lblStage";
		this.lblStage.Size = new System.Drawing.Size(77, 28);
		this.lblStage.TabIndex = 20;
		this.lblStage.Text = "Stage";
		this.lblStage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblTemplate.AutoSize = true;
		this.lblTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTemplate.Location = new System.Drawing.Point(4, 30);
		this.lblTemplate.Name = "lblTemplate";
		this.lblTemplate.Size = new System.Drawing.Size(77, 28);
		this.lblTemplate.TabIndex = 21;
		this.lblTemplate.Text = "Template";
		this.lblTemplate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblCreate.AutoSize = true;
		this.lblCreate.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCreate.Location = new System.Drawing.Point(4, 60);
		this.lblCreate.Name = "lblCreate";
		this.lblCreate.Size = new System.Drawing.Size(77, 28);
		this.lblCreate.TabIndex = 76;
		this.lblCreate.Text = "Create date";
		this.lblCreate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblModifi.AutoSize = true;
		this.lblModifi.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblModifi.Location = new System.Drawing.Point(4, 89);
		this.lblModifi.Name = "lblModifi";
		this.lblModifi.Size = new System.Drawing.Size(77, 28);
		this.lblModifi.TabIndex = 77;
		this.lblModifi.Text = "Edited date";
		this.lblModifi.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblCreateBy.AutoSize = true;
		this.lblCreateBy.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCreateBy.Location = new System.Drawing.Point(4, 118);
		this.lblCreateBy.Name = "lblCreateBy";
		this.lblCreateBy.Size = new System.Drawing.Size(77, 28);
		this.lblCreateBy.TabIndex = 78;
		this.lblCreateBy.Text = "Created by";
		this.lblCreateBy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblModifiBy.AutoSize = true;
		this.lblModifiBy.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblModifiBy.Location = new System.Drawing.Point(4, 147);
		this.lblModifiBy.Name = "lblModifiBy";
		this.lblModifiBy.Size = new System.Drawing.Size(77, 28);
		this.lblModifiBy.TabIndex = 79;
		this.lblModifiBy.Text = "Edited by";
		this.lblModifiBy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblCreated.AutoSize = true;
		this.lblCreated.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCreated.Location = new System.Drawing.Point(88, 60);
		this.lblCreated.Name = "lblCreated";
		this.lblCreated.Size = new System.Drawing.Size(250, 28);
		this.lblCreated.TabIndex = 80;
		this.lblCreated.Text = "...";
		this.lblCreated.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblModified.AutoSize = true;
		this.lblModified.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblModified.Location = new System.Drawing.Point(88, 89);
		this.lblModified.Name = "lblModified";
		this.lblModified.Size = new System.Drawing.Size(250, 28);
		this.lblModified.TabIndex = 81;
		this.lblModified.Text = "...";
		this.lblModified.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblCreatedBy.AutoSize = true;
		this.lblCreatedBy.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCreatedBy.Location = new System.Drawing.Point(88, 118);
		this.lblCreatedBy.Name = "lblCreatedBy";
		this.lblCreatedBy.Size = new System.Drawing.Size(250, 28);
		this.lblCreatedBy.TabIndex = 82;
		this.lblCreatedBy.Text = "...";
		this.lblCreatedBy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblModifiedBy.AutoSize = true;
		this.lblModifiedBy.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblModifiedBy.Location = new System.Drawing.Point(88, 147);
		this.lblModifiedBy.Name = "lblModifiedBy";
		this.lblModifiedBy.Size = new System.Drawing.Size(250, 28);
		this.lblModifiedBy.TabIndex = 83;
		this.lblModifiedBy.Text = "...";
		this.lblModifiedBy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel1.Controls.Add(this.cbbStage);
		this.panel1.Controls.Add(this.btnStageAdd);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(88, 3);
		this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(250, 24);
		this.panel1.TabIndex = 1;
		this.panel1.TabStop = true;
		this.cbbStage.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cbbStage.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cbbStage.Dock = System.Windows.Forms.DockStyle.Fill;
		this.cbbStage.FormattingEnabled = true;
		this.cbbStage.Location = new System.Drawing.Point(0, 0);
		this.cbbStage.Name = "cbbStage";
		this.cbbStage.Size = new System.Drawing.Size(226, 24);
		this.cbbStage.TabIndex = 1;
		this.toolTipMain.SetToolTip(this.cbbStage, "Please select or enter stage");
		this.cbbStage.Enter += new System.EventHandler(cbbNormal_Enter);
		this.cbbStage.Leave += new System.EventHandler(cbbNormalNotNull_Leave);
		this.btnStageAdd.BackColor = System.Drawing.Color.Gainsboro;
		this.btnStageAdd.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnStageAdd.Dock = System.Windows.Forms.DockStyle.Right;
		this.btnStageAdd.FlatAppearance.BorderSize = 0;
		this.btnStageAdd.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
		this.btnStageAdd.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SkyBlue;
		this.btnStageAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnStageAdd.Location = new System.Drawing.Point(226, 0);
		this.btnStageAdd.Name = "btnStageAdd";
		this.btnStageAdd.Size = new System.Drawing.Size(24, 24);
		this.btnStageAdd.TabIndex = 2;
		this.btnStageAdd.Text = "+";
		this.toolTipMain.SetToolTip(this.btnStageAdd, "Goto manage stage");
		this.btnStageAdd.UseVisualStyleBackColor = false;
		this.btnStageAdd.Click += new System.EventHandler(btnStageAdd_Click);
		this.panel2.Controls.Add(this.cbbTemplate);
		this.panel2.Controls.Add(this.btnTemplateAdd);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(88, 32);
		this.panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(250, 24);
		this.panel2.TabIndex = 2;
		this.panel2.TabStop = true;
		this.cbbTemplate.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cbbTemplate.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cbbTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
		this.cbbTemplate.FormattingEnabled = true;
		this.cbbTemplate.Location = new System.Drawing.Point(0, 0);
		this.cbbTemplate.Name = "cbbTemplate";
		this.cbbTemplate.Size = new System.Drawing.Size(226, 24);
		this.cbbTemplate.TabIndex = 1;
		this.toolTipMain.SetToolTip(this.cbbTemplate, "Please select or enter template");
		this.cbbTemplate.Enter += new System.EventHandler(cbbNormal_Enter);
		this.cbbTemplate.Leave += new System.EventHandler(cbbNormal_Leave);
		this.btnTemplateAdd.BackColor = System.Drawing.Color.Gainsboro;
		this.btnTemplateAdd.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnTemplateAdd.Dock = System.Windows.Forms.DockStyle.Right;
		this.btnTemplateAdd.FlatAppearance.BorderSize = 0;
		this.btnTemplateAdd.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
		this.btnTemplateAdd.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SkyBlue;
		this.btnTemplateAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnTemplateAdd.Location = new System.Drawing.Point(226, 0);
		this.btnTemplateAdd.Name = "btnTemplateAdd";
		this.btnTemplateAdd.Size = new System.Drawing.Size(24, 24);
		this.btnTemplateAdd.TabIndex = 2;
		this.btnTemplateAdd.Text = "+";
		this.toolTipMain.SetToolTip(this.btnTemplateAdd, "Goto manage template");
		this.btnTemplateAdd.UseVisualStyleBackColor = false;
		this.btnTemplateAdd.Click += new System.EventHandler(btnTemplateAdd_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1000, 500);
		base.Controls.Add(this.dgvMain);
		base.Controls.Add(this.tableLayoutPanel1);
		base.Controls.Add(this.panelMain);
		base.Controls.Add(this.btnConfirm);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		base.Name = "frmPlanAdd";
		base.Padding = new System.Windows.Forms.Padding(20, 70, 20, 20);
		this.Text = "5S QA System * PLAN";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmPlanAdd_FormClosing);
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmPlanAdd_FormClosed);
		base.Load += new System.EventHandler(frmPlanAdd_Load);
		base.Shown += new System.EventHandler(frmPlanAdd_Shown);
		((System.ComponentModel.ISupportInitialize)this.dgvMain).EndInit();
		this.contextMenuStripMain.ResumeLayout(false);
		this.panelMain.ResumeLayout(false);
		this.panelMain.PerformLayout();
		this.tableLayoutPanel2.ResumeLayout(false);
		this.tableLayoutPanel2.PerformLayout();
		this.tableLayoutPanel1.ResumeLayout(false);
		this.tableLayoutPanel1.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
