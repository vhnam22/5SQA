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

public class frmMachineAdd : MetroForm
{
	private readonly Form mForm;

	private Guid mId;

	private string mRemember;

	private readonly DataTable mData;

	public bool isClose;

	private readonly bool isAdd;

	private IContainer components = null;

	private Panel panelMain;

	private Button btnConfirm;

	private TableLayoutPanel tableLayoutPanel2;

	private ComboBox cbbStatus;

	private Panel panel1;

	private ComboBox cbbFactory;

	private ToolTip toolTipMain;

	private Panel panel2;

	private ComboBox cbbMacType;

	private Label lblStatus;

	private Label lblFactory;

	private Label lblSerial;

	private Label lblModel;

	private Label lblName;

	private Label lblCode;

	private Label lblMacType;

	private Label lblCreate;

	private Label lblModifi;

	private Label lblCreateBy;

	private Label lblModifiBy;

	private Label lblCreated;

	private Label lblModified;

	private Label lblCreatedBy;

	private Label lblModifiedBy;

	private Button btnMacTypeAdd;

	private TextBox txtCode;

	private TextBox txtSerial;

	private TextBox txtModel;

	private TextBox txtName;

	private Button btnFactoryAdd;

	private TextBox txtMark;

	private Label lblMark;

	public frmMachineAdd(Form frm, DataTable data, Guid id = default(Guid), bool isadd = true)
	{
		InitializeComponent();
		Common.setControls(this, toolTipMain);
		mForm = frm;
		isClose = true;
		mData = data;
		mId = id;
		isAdd = isadd;
		string text = "ADD";
		if (!isAdd)
		{
			text = "EDIT";
			cbbStatus.Enabled = true;
		}
		string text2 = Common.getTextLanguage(this, text);
		if (string.IsNullOrEmpty(text2))
		{
			text2 = text;
		}
		Text = Text + " (" + text2 + ")";
	}

	private void frmMachineAdd_Load(object sender, EventArgs e)
	{
		Init();
	}

	private void frmMachineAdd_Shown(object sender, EventArgs e)
	{
		load_cbbMacType();
		load_cbbFactory();
		if (isAdd)
		{
			cbbFactory.SelectedIndex = -1;
			txtCode.Text = set_Code();
			cbbStatus.Text = "In Use";
		}
		load_Data();
		cbbMacType.Select();
	}

	private void frmMachineAdd_FormClosing(object sender, FormClosingEventArgs e)
	{
		GC.Collect();
	}

	private void frmMachineAdd_FormClosed(object sender, FormClosedEventArgs e)
	{
		List<Type> list = new List<Type>();
		list.Add(typeof(frmSubView));
		Common.closeForm(list);
		mData?.Dispose();
		if (!isClose)
		{
			((frmMachineView)mForm).load_AllData();
		}
	}

	private void Init()
	{
		load_AutoComplete();
		lblCreated.Text = "";
		lblModified.Text = "";
		lblCreatedBy.Text = "";
		lblModifiedBy.Text = "";
	}

	public void load_cbbMacType()
	{
		try
		{
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "TypeId=@0";
			queryArgs.PredicateParameters = new string[1] { "438D7052-25F3-4342-ED0C-08D7E9C5C77D" };
			queryArgs.Order = "Created DESC";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs body = queryArgs;
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/MetadataValue/Gets").Result;
			DataTable dataTable = Common.getDataTable(result);
			if (dataTable != null)
			{
				dataTable.Dispose();
				cbbMacType.ValueMember = "Id";
				cbbMacType.DisplayMember = "Name";
				cbbMacType.DataSource = dataTable;
			}
			else
			{
				cbbMacType.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			cbbMacType.DataSource = null;
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

	public void load_cbbFactory()
	{
		try
		{
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "TypeId=@0";
			queryArgs.PredicateParameters = new string[1] { "5EB07BDB-9086-4BC5-A02B-5D6E9CFCD476" };
			queryArgs.Order = "Created DESC";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs body = queryArgs;
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/MetadataValue/Gets").Result;
			DataTable dataTable = Common.getDataTable(result);
			if (dataTable != null)
			{
				dataTable.Dispose();
				cbbFactory.ValueMember = "Id";
				cbbFactory.DisplayMember = "Name";
				cbbFactory.DataSource = dataTable;
				cbbFactory.Refresh();
			}
			else
			{
				cbbFactory.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			cbbFactory.DataSource = null;
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

	private void load_AutoComplete()
	{
		txtCode.AutoCompleteCustomSource = Common.getAutoComplete(mData, "Code");
		txtName.AutoCompleteCustomSource = Common.getAutoComplete(mData, "Name");
		txtModel.AutoCompleteCustomSource = Common.getAutoComplete(mData, "Model");
		txtSerial.AutoCompleteCustomSource = Common.getAutoComplete(mData, "Serial");
		txtMark.AutoCompleteCustomSource = Common.getAutoComplete(mData, "Mark");
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
				cbbMacType.SelectedValue = dataRow["MachineTypeId"];
				txtName.Text = dataRow["Name"].ToString();
				txtModel.Text = dataRow["Model"].ToString();
				txtSerial.Text = dataRow["Serial"].ToString();
				if (dataRow["FactoryId"] != null)
				{
					cbbFactory.SelectedValue = dataRow["FactoryId"];
				}
				else
				{
					cbbFactory.SelectedIndex = -1;
				}
				if (!isAdd)
				{
					txtMark.Text = dataRow["Mark"].ToString();
					cbbStatus.Text = dataRow["Status"].ToString();
					txtCode.Text = dataRow["Code"].ToString();
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

	private string set_Code()
	{
		string text = "MAC-";
		try
		{
			QueryArgs body = new QueryArgs
			{
				Order = "Created DESC",
				Page = 1,
				Limit = 1
			};
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/Machine/Gets").Result;
			DataTable dataTable = Common.getDataTable<MachineViewModel>(result);
			if (dataTable.Rows.Count > 0)
			{
				string[] array = dataTable.Rows[0]["Code"].ToString().Split('-');
				if (array.Length > 1 && int.TryParse(array[1], out var result2))
				{
					text += result2 + 1;
				}
			}
			else
			{
				text += "1";
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
					MessageBox.Show(ex2.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			else
			{
				MessageBox.Show(ex.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
		return text;
	}

	private void btnComfirm_Click(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		try
		{
			if (string.IsNullOrEmpty(cbbMacType.Text))
			{
				MessageBox.Show(Common.getTextLanguage(this, "wMachineType"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				cbbMacType.Focus();
				return;
			}
			if (string.IsNullOrEmpty(txtCode.Text))
			{
				MessageBox.Show(Common.getTextLanguage(this, "wCode"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				txtCode.Focus();
				return;
			}
			if (string.IsNullOrEmpty(txtName.Text))
			{
				MessageBox.Show(Common.getTextLanguage(this, "wName"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				txtName.Focus();
				return;
			}
			if (string.IsNullOrEmpty(cbbStatus.Text))
			{
				MessageBox.Show(Common.getTextLanguage(this, "wStatus"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				cbbStatus.Focus();
				return;
			}
			MachineViewModel machineViewModel = new MachineViewModel
			{
				MachineTypeId = Guid.Parse(cbbMacType.SelectedValue.ToString()),
				Code = txtCode.Text,
				Name = txtName.Text,
				Status = cbbStatus.Text,
				IsActivated = true
			};
			if (!string.IsNullOrEmpty(txtModel.Text))
			{
				machineViewModel.Model = txtModel.Text;
			}
			if (!string.IsNullOrEmpty(txtSerial.Text))
			{
				machineViewModel.Serial = txtSerial.Text;
			}
			if (!string.IsNullOrEmpty(txtMark.Text))
			{
				machineViewModel.Mark = int.Parse(txtMark.Text);
			}
			if (!string.IsNullOrEmpty(cbbFactory.Text))
			{
				machineViewModel.FactoryId = Guid.Parse(cbbFactory.SelectedValue.ToString());
			}
			if (isAdd || !MessageBox.Show(Common.getTextLanguage(this, "wSureEdit"), Common.getTextLanguage(this, "QUESTION"), MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.No))
			{
				Cursor.Current = Cursors.WaitCursor;
				if (!isAdd)
				{
					((AuditableEntity)(object)machineViewModel).Id = mId;
				}
				ResponseDto result = frmLogin.client.SaveAsync(machineViewModel, "/api/Machine/Save").Result;
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

	private void txtNormal_Leave(object sender, EventArgs e)
	{
		TextBox textBox = sender as TextBox;
		textBox.Text = Common.trimSpace(textBox.Text);
	}

	private void btnFactoryAdd_Click(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		List<Type> list = new List<Type>();
		list.Add(typeof(frmSubView));
		Common.closeForm(list);
		new frmSubView(this, (FormType)5).Show();
	}

	private void btnMacTypeAdd_Click(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		List<Type> list = new List<Type>();
		list.Add(typeof(frmSubView));
		Common.closeForm(list);
		new frmSubView(this, (FormType)6).Show();
	}

	private void txtMark_Leave(object sender, EventArgs e)
	{
		TextBox textBox = sender as TextBox;
		if (int.TryParse(textBox.Text, out var result))
		{
			textBox.Text = result.ToString();
		}
		else
		{
			textBox.Text = string.Empty;
		}
	}

	private void txtMark_KeyPress(object sender, KeyPressEventArgs e)
	{
		if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b')
		{
			e.Handled = true;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_5S_QA_System.frmMachineAdd));
		this.panelMain = new System.Windows.Forms.Panel();
		this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
		this.txtSerial = new System.Windows.Forms.TextBox();
		this.txtCode = new System.Windows.Forms.TextBox();
		this.txtModel = new System.Windows.Forms.TextBox();
		this.lblModifiedBy = new System.Windows.Forms.Label();
		this.txtName = new System.Windows.Forms.TextBox();
		this.lblCreatedBy = new System.Windows.Forms.Label();
		this.lblModified = new System.Windows.Forms.Label();
		this.lblCreated = new System.Windows.Forms.Label();
		this.lblModifiBy = new System.Windows.Forms.Label();
		this.lblCreateBy = new System.Windows.Forms.Label();
		this.lblModifi = new System.Windows.Forms.Label();
		this.lblCreate = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.cbbMacType = new System.Windows.Forms.ComboBox();
		this.btnMacTypeAdd = new System.Windows.Forms.Button();
		this.lblStatus = new System.Windows.Forms.Label();
		this.lblFactory = new System.Windows.Forms.Label();
		this.lblSerial = new System.Windows.Forms.Label();
		this.lblModel = new System.Windows.Forms.Label();
		this.cbbStatus = new System.Windows.Forms.ComboBox();
		this.lblName = new System.Windows.Forms.Label();
		this.lblCode = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.cbbFactory = new System.Windows.Forms.ComboBox();
		this.btnFactoryAdd = new System.Windows.Forms.Button();
		this.lblMacType = new System.Windows.Forms.Label();
		this.btnConfirm = new System.Windows.Forms.Button();
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.lblMark = new System.Windows.Forms.Label();
		this.txtMark = new System.Windows.Forms.TextBox();
		this.panelMain.SuspendLayout();
		this.tableLayoutPanel2.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel1.SuspendLayout();
		base.SuspendLayout();
		this.panelMain.AutoScroll = true;
		this.panelMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panelMain.Controls.Add(this.tableLayoutPanel2);
		this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panelMain.Location = new System.Drawing.Point(20, 70);
		this.panelMain.Margin = new System.Windows.Forms.Padding(4);
		this.panelMain.Name = "panelMain";
		this.panelMain.Size = new System.Drawing.Size(500, 360);
		this.panelMain.TabIndex = 16;
		this.tableLayoutPanel2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.tableLayoutPanel2.AutoSize = true;
		this.tableLayoutPanel2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		this.tableLayoutPanel2.ColumnCount = 2;
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel2.Controls.Add(this.txtMark, 1, 7);
		this.tableLayoutPanel2.Controls.Add(this.lblMark, 0, 7);
		this.tableLayoutPanel2.Controls.Add(this.txtSerial, 1, 4);
		this.tableLayoutPanel2.Controls.Add(this.txtCode, 1, 1);
		this.tableLayoutPanel2.Controls.Add(this.txtModel, 1, 3);
		this.tableLayoutPanel2.Controls.Add(this.lblModifiedBy, 1, 12);
		this.tableLayoutPanel2.Controls.Add(this.txtName, 1, 2);
		this.tableLayoutPanel2.Controls.Add(this.lblCreatedBy, 1, 11);
		this.tableLayoutPanel2.Controls.Add(this.lblModified, 1, 10);
		this.tableLayoutPanel2.Controls.Add(this.lblCreated, 1, 9);
		this.tableLayoutPanel2.Controls.Add(this.lblModifiBy, 0, 12);
		this.tableLayoutPanel2.Controls.Add(this.lblCreateBy, 0, 11);
		this.tableLayoutPanel2.Controls.Add(this.lblModifi, 0, 10);
		this.tableLayoutPanel2.Controls.Add(this.lblCreate, 0, 9);
		this.tableLayoutPanel2.Controls.Add(this.panel2, 1, 0);
		this.tableLayoutPanel2.Controls.Add(this.lblStatus, 0, 6);
		this.tableLayoutPanel2.Controls.Add(this.lblFactory, 0, 5);
		this.tableLayoutPanel2.Controls.Add(this.lblSerial, 0, 4);
		this.tableLayoutPanel2.Controls.Add(this.lblModel, 0, 3);
		this.tableLayoutPanel2.Controls.Add(this.cbbStatus, 1, 6);
		this.tableLayoutPanel2.Controls.Add(this.lblName, 0, 2);
		this.tableLayoutPanel2.Controls.Add(this.lblCode, 0, 1);
		this.tableLayoutPanel2.Controls.Add(this.panel1, 1, 5);
		this.tableLayoutPanel2.Controls.Add(this.lblMacType, 0, 0);
		this.tableLayoutPanel2.Location = new System.Drawing.Point(4, 4);
		this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4);
		this.tableLayoutPanel2.Name = "tableLayoutPanel2";
		this.tableLayoutPanel2.RowCount = 13;
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.Size = new System.Drawing.Size(490, 350);
		this.tableLayoutPanel2.TabIndex = 15;
		this.txtSerial.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtSerial.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtSerial.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtSerial.Location = new System.Drawing.Point(98, 120);
		this.txtSerial.Name = "txtSerial";
		this.txtSerial.Size = new System.Drawing.Size(388, 22);
		this.txtSerial.TabIndex = 5;
		this.toolTipMain.SetToolTip(this.txtSerial, "Please enter serial");
		this.txtSerial.Leave += new System.EventHandler(txtNormal_Leave);
		this.txtCode.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtCode.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtCode.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtCode.Enabled = false;
		this.txtCode.Location = new System.Drawing.Point(98, 33);
		this.txtCode.Name = "txtCode";
		this.txtCode.Size = new System.Drawing.Size(388, 22);
		this.txtCode.TabIndex = 2;
		this.toolTipMain.SetToolTip(this.txtCode, "Please enter code");
		this.txtModel.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtModel.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtModel.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtModel.Location = new System.Drawing.Point(98, 91);
		this.txtModel.Name = "txtModel";
		this.txtModel.Size = new System.Drawing.Size(388, 22);
		this.txtModel.TabIndex = 4;
		this.toolTipMain.SetToolTip(this.txtModel, "Please enter model");
		this.txtModel.Leave += new System.EventHandler(txtNormal_Leave);
		this.lblModifiedBy.AutoSize = true;
		this.lblModifiedBy.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblModifiedBy.Location = new System.Drawing.Point(98, 321);
		this.lblModifiedBy.Name = "lblModifiedBy";
		this.lblModifiedBy.Size = new System.Drawing.Size(388, 28);
		this.lblModifiedBy.TabIndex = 80;
		this.lblModifiedBy.Text = "...";
		this.lblModifiedBy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.txtName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtName.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtName.Location = new System.Drawing.Point(98, 62);
		this.txtName.Name = "txtName";
		this.txtName.Size = new System.Drawing.Size(388, 22);
		this.txtName.TabIndex = 3;
		this.toolTipMain.SetToolTip(this.txtName, "Please enter name");
		this.txtName.Leave += new System.EventHandler(txtNormal_Leave);
		this.lblCreatedBy.AutoSize = true;
		this.lblCreatedBy.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCreatedBy.Location = new System.Drawing.Point(98, 292);
		this.lblCreatedBy.Name = "lblCreatedBy";
		this.lblCreatedBy.Size = new System.Drawing.Size(388, 28);
		this.lblCreatedBy.TabIndex = 79;
		this.lblCreatedBy.Text = "...";
		this.lblCreatedBy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblModified.AutoSize = true;
		this.lblModified.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblModified.Location = new System.Drawing.Point(98, 263);
		this.lblModified.Name = "lblModified";
		this.lblModified.Size = new System.Drawing.Size(388, 28);
		this.lblModified.TabIndex = 78;
		this.lblModified.Text = "...";
		this.lblModified.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblCreated.AutoSize = true;
		this.lblCreated.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCreated.Location = new System.Drawing.Point(98, 234);
		this.lblCreated.Name = "lblCreated";
		this.lblCreated.Size = new System.Drawing.Size(388, 28);
		this.lblCreated.TabIndex = 77;
		this.lblCreated.Text = "...";
		this.lblCreated.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblModifiBy.AutoSize = true;
		this.lblModifiBy.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblModifiBy.Location = new System.Drawing.Point(4, 321);
		this.lblModifiBy.Name = "lblModifiBy";
		this.lblModifiBy.Size = new System.Drawing.Size(87, 28);
		this.lblModifiBy.TabIndex = 76;
		this.lblModifiBy.Text = "Edited by";
		this.lblModifiBy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblCreateBy.AutoSize = true;
		this.lblCreateBy.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCreateBy.Location = new System.Drawing.Point(4, 292);
		this.lblCreateBy.Name = "lblCreateBy";
		this.lblCreateBy.Size = new System.Drawing.Size(87, 28);
		this.lblCreateBy.TabIndex = 75;
		this.lblCreateBy.Text = "Created by";
		this.lblCreateBy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblModifi.AutoSize = true;
		this.lblModifi.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblModifi.Location = new System.Drawing.Point(4, 263);
		this.lblModifi.Name = "lblModifi";
		this.lblModifi.Size = new System.Drawing.Size(87, 28);
		this.lblModifi.TabIndex = 74;
		this.lblModifi.Text = "Edited date";
		this.lblModifi.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblCreate.AutoSize = true;
		this.lblCreate.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCreate.Location = new System.Drawing.Point(4, 234);
		this.lblCreate.Name = "lblCreate";
		this.lblCreate.Size = new System.Drawing.Size(87, 28);
		this.lblCreate.TabIndex = 73;
		this.lblCreate.Text = "Create date";
		this.lblCreate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.panel2.Controls.Add(this.cbbMacType);
		this.panel2.Controls.Add(this.btnMacTypeAdd);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(98, 3);
		this.panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(388, 24);
		this.panel2.TabIndex = 1;
		this.panel2.TabStop = true;
		this.cbbMacType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cbbMacType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cbbMacType.Dock = System.Windows.Forms.DockStyle.Fill;
		this.cbbMacType.FormattingEnabled = true;
		this.cbbMacType.Location = new System.Drawing.Point(0, 0);
		this.cbbMacType.Name = "cbbMacType";
		this.cbbMacType.Size = new System.Drawing.Size(364, 24);
		this.cbbMacType.TabIndex = 1;
		this.toolTipMain.SetToolTip(this.cbbMacType, "Select or enter machine type");
		this.cbbMacType.Enter += new System.EventHandler(cbbNormal_Enter);
		this.cbbMacType.Leave += new System.EventHandler(cbbNormalNotNull_Leave);
		this.btnMacTypeAdd.BackColor = System.Drawing.Color.Gainsboro;
		this.btnMacTypeAdd.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnMacTypeAdd.Dock = System.Windows.Forms.DockStyle.Right;
		this.btnMacTypeAdd.FlatAppearance.BorderSize = 0;
		this.btnMacTypeAdd.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
		this.btnMacTypeAdd.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SkyBlue;
		this.btnMacTypeAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnMacTypeAdd.Location = new System.Drawing.Point(364, 0);
		this.btnMacTypeAdd.Name = "btnMacTypeAdd";
		this.btnMacTypeAdd.Size = new System.Drawing.Size(24, 24);
		this.btnMacTypeAdd.TabIndex = 2;
		this.btnMacTypeAdd.Text = "+";
		this.toolTipMain.SetToolTip(this.btnMacTypeAdd, "Goto manage machine type");
		this.btnMacTypeAdd.UseVisualStyleBackColor = false;
		this.btnMacTypeAdd.Click += new System.EventHandler(btnMacTypeAdd_Click);
		this.lblStatus.AutoSize = true;
		this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblStatus.Location = new System.Drawing.Point(4, 175);
		this.lblStatus.Name = "lblStatus";
		this.lblStatus.Size = new System.Drawing.Size(87, 28);
		this.lblStatus.TabIndex = 25;
		this.lblStatus.Text = "Status";
		this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblFactory.AutoSize = true;
		this.lblFactory.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblFactory.Location = new System.Drawing.Point(4, 146);
		this.lblFactory.Name = "lblFactory";
		this.lblFactory.Size = new System.Drawing.Size(87, 28);
		this.lblFactory.TabIndex = 24;
		this.lblFactory.Text = "Factory";
		this.lblFactory.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblSerial.AutoSize = true;
		this.lblSerial.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblSerial.Location = new System.Drawing.Point(4, 117);
		this.lblSerial.Name = "lblSerial";
		this.lblSerial.Size = new System.Drawing.Size(87, 28);
		this.lblSerial.TabIndex = 23;
		this.lblSerial.Text = "Serial";
		this.lblSerial.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblModel.AutoSize = true;
		this.lblModel.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblModel.Location = new System.Drawing.Point(4, 88);
		this.lblModel.Name = "lblModel";
		this.lblModel.Size = new System.Drawing.Size(87, 28);
		this.lblModel.TabIndex = 22;
		this.lblModel.Text = "Model";
		this.lblModel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.cbbStatus.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cbbStatus.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cbbStatus.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbbStatus.Dock = System.Windows.Forms.DockStyle.Fill;
		this.cbbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbbStatus.Enabled = false;
		this.cbbStatus.FormattingEnabled = true;
		this.cbbStatus.ItemHeight = 16;
		this.cbbStatus.Items.AddRange(new object[3] { "In Use", "Not Use", "Repairing" });
		this.cbbStatus.Location = new System.Drawing.Point(98, 177);
		this.cbbStatus.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.cbbStatus.Name = "cbbStatus";
		this.cbbStatus.Size = new System.Drawing.Size(388, 24);
		this.cbbStatus.TabIndex = 7;
		this.toolTipMain.SetToolTip(this.cbbStatus, "Select status");
		this.lblName.AutoSize = true;
		this.lblName.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblName.Location = new System.Drawing.Point(4, 59);
		this.lblName.Name = "lblName";
		this.lblName.Size = new System.Drawing.Size(87, 28);
		this.lblName.TabIndex = 21;
		this.lblName.Text = "Name";
		this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblCode.AutoSize = true;
		this.lblCode.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCode.Location = new System.Drawing.Point(4, 30);
		this.lblCode.Name = "lblCode";
		this.lblCode.Size = new System.Drawing.Size(87, 28);
		this.lblCode.TabIndex = 20;
		this.lblCode.Text = "Code";
		this.lblCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.panel1.Controls.Add(this.cbbFactory);
		this.panel1.Controls.Add(this.btnFactoryAdd);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(98, 148);
		this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(388, 24);
		this.panel1.TabIndex = 6;
		this.panel1.TabStop = true;
		this.cbbFactory.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cbbFactory.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cbbFactory.Dock = System.Windows.Forms.DockStyle.Fill;
		this.cbbFactory.FormattingEnabled = true;
		this.cbbFactory.Location = new System.Drawing.Point(0, 0);
		this.cbbFactory.Name = "cbbFactory";
		this.cbbFactory.Size = new System.Drawing.Size(364, 24);
		this.cbbFactory.TabIndex = 1;
		this.toolTipMain.SetToolTip(this.cbbFactory, "Select or enter factory");
		this.cbbFactory.Enter += new System.EventHandler(cbbNormal_Enter);
		this.cbbFactory.Leave += new System.EventHandler(cbbNormal_Leave);
		this.btnFactoryAdd.BackColor = System.Drawing.Color.Gainsboro;
		this.btnFactoryAdd.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnFactoryAdd.Dock = System.Windows.Forms.DockStyle.Right;
		this.btnFactoryAdd.FlatAppearance.BorderSize = 0;
		this.btnFactoryAdd.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
		this.btnFactoryAdd.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SkyBlue;
		this.btnFactoryAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnFactoryAdd.Location = new System.Drawing.Point(364, 0);
		this.btnFactoryAdd.Name = "btnFactoryAdd";
		this.btnFactoryAdd.Size = new System.Drawing.Size(24, 24);
		this.btnFactoryAdd.TabIndex = 2;
		this.btnFactoryAdd.Text = "+";
		this.toolTipMain.SetToolTip(this.btnFactoryAdd, "Goto manage factory");
		this.btnFactoryAdd.UseVisualStyleBackColor = false;
		this.btnFactoryAdd.Click += new System.EventHandler(btnFactoryAdd_Click);
		this.lblMacType.AutoSize = true;
		this.lblMacType.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblMacType.Location = new System.Drawing.Point(4, 1);
		this.lblMacType.Name = "lblMacType";
		this.lblMacType.Size = new System.Drawing.Size(87, 28);
		this.lblMacType.TabIndex = 19;
		this.lblMacType.Text = "Machine type";
		this.lblMacType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.btnConfirm.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnConfirm.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.btnConfirm.FlatAppearance.BorderSize = 0;
		this.btnConfirm.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
		this.btnConfirm.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
		this.btnConfirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnConfirm.Location = new System.Drawing.Point(20, 430);
		this.btnConfirm.Name = "btnConfirm";
		this.btnConfirm.Size = new System.Drawing.Size(500, 28);
		this.btnConfirm.TabIndex = 1;
		this.btnConfirm.Text = "Confirm";
		this.toolTipMain.SetToolTip(this.btnConfirm, "Confirm");
		this.btnConfirm.UseVisualStyleBackColor = true;
		this.btnConfirm.Click += new System.EventHandler(btnComfirm_Click);
		this.lblMark.AutoSize = true;
		this.lblMark.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblMark.Location = new System.Drawing.Point(4, 204);
		this.lblMark.Name = "lblMark";
		this.lblMark.Size = new System.Drawing.Size(87, 28);
		this.lblMark.TabIndex = 81;
		this.lblMark.Text = "Cable code";
		this.lblMark.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtMark.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtMark.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtMark.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtMark.Location = new System.Drawing.Point(98, 207);
		this.txtMark.Name = "txtMark";
		this.txtMark.Size = new System.Drawing.Size(388, 22);
		this.txtMark.TabIndex = 8;
		this.toolTipMain.SetToolTip(this.txtMark, "Please enter cable code");
		this.txtMark.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtMark_KeyPress);
		this.txtMark.Leave += new System.EventHandler(txtMark_Leave);
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(540, 478);
		base.Controls.Add(this.panelMain);
		base.Controls.Add(this.btnConfirm);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "frmMachineAdd";
		base.Padding = new System.Windows.Forms.Padding(20, 70, 20, 20);
		this.Text = "5S QA System * MACHINE";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmMachineAdd_FormClosing);
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmMachineAdd_FormClosed);
		base.Load += new System.EventHandler(frmMachineAdd_Load);
		base.Shown += new System.EventHandler(frmMachineAdd_Shown);
		this.panelMain.ResumeLayout(false);
		this.panelMain.PerformLayout();
		this.tableLayoutPanel2.ResumeLayout(false);
		this.tableLayoutPanel2.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
