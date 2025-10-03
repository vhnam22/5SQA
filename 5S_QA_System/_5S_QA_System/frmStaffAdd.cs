using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using _5S_QA_DL.APIContext;
using _5S_QA_Entities.Constants;
using _5S_QA_Entities.Dtos;
using _5S_QA_Entities.Enums;
using _5S_QA_Entities.Models;
using _5S_QA_System.Controls;
using _5S_QA_System.Properties;
using MetroFramework.Forms;
using Newtonsoft.Json;

namespace _5S_QA_System;

public class frmStaffAdd : MetroForm
{
	private readonly Form mForm;

	private Guid mId;

	private string mRemember;

	private string mFileName;

	private readonly DataTable mData;

	public bool isClose;

	private readonly bool isAdd;

	private IContainer components = null;

	private ToolTip toolTipMain;

	private Panel panelMain;

	private TableLayoutPanel tableLayoutPanel2;

	private Panel panel2;

	private ComboBox cbbDepartment;

	private Button btnConfirm;

	private ComboBox cbbGender;

	private DateTimePicker dtpBirthday;

	private Panel panel1;

	private OpenFileDialog openFileDialogMain;

	private ComboBox cbbPosition;

	private Label lblBirthday;

	private Label lblEmail;

	private Label lblFullname;

	private Label lblPassword;

	private Label lblUsername;

	private Label lblCreate;

	private Label lblDepartment;

	private Label lblPosition;

	private Label lblAvatar;

	private Label lblAddress;

	private Label lblPhone;

	private Label lblGender;

	private Label lblModifiBy;

	private Label lblCreateBy;

	private Label lblModifi;

	private Label lblModifiedBy;

	private Label lblCreatedBy;

	private Label lblModified;

	private Label lblCreated;

	private TextBox txtUsername;

	private TextBox txtFullname;

	private Button btnResetPass;

	private TextBox txtPhone;

	private TextBox txtEmail;

	private TextBox txtAvatar;

	private TextBox txtAddress;

	private Button btnAvatarFolder;

	private Button btnAvatarDelete;

	private Button btnDepartmentAdd;

	private Label lblIsActivated;

	public CheckBox cbIsActivated;

	private Label lblJobTitle;

	private ComboBox cbbJobTitle;

	public CheckBox cbIsEmail;

	private Label lblIsEmail;

	public frmStaffAdd(Form frm, DataTable data, Guid id = default(Guid), bool isadd = true)
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
			cbIsActivated.Enabled = true;
		}
		string text2 = Common.getTextLanguage(this, text);
		if (string.IsNullOrEmpty(text2))
		{
			text2 = text;
		}
		Text = Text + " (" + text2 + ")";
		if (frmLogin.User.Role.Equals(RoleWeb.SuperAdministrator))
		{
			cbbPosition.Enabled = true;
		}
		else
		{
			cbbPosition.Enabled = false;
		}
	}

	private void frmStaffAdd_Load(object sender, EventArgs e)
	{
		Init();
	}

	private void frmStaffAdd_Shown(object sender, EventArgs e)
	{
		load_cbbDepartment();
		if (isAdd)
		{
			cbbDepartment.SelectedIndex = -1;
			btnResetPass.Enabled = false;
			cbbGender.Text = "Male";
			cbbPosition.Text = "Staff";
		}
		load_Data();
		txtUsername.Select();
	}

	private void frmStaffAdd_FormClosing(object sender, FormClosingEventArgs e)
	{
		GC.Collect();
	}

	private void frmStaffAdd_FormClosed(object sender, FormClosedEventArgs e)
	{
		List<Type> list = new List<Type>();
		list.Add(typeof(frmSubView));
		Common.closeForm(list);
		mData?.Dispose();
		if (!isClose)
		{
			((frmStaffView)mForm).load_AllData();
		}
	}

	private void Init()
	{
		btnAvatarDelete.Visible = false;
		load_AutoComplete();
		lblCreated.Text = "";
		lblModified.Text = "";
		lblCreatedBy.Text = "";
		lblModifiedBy.Text = "";
		cbbJobTitle.Items.AddRange(MetaType.lstJobTitle);
	}

	public void load_cbbDepartment()
	{
		try
		{
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "TypeId=@0";
			queryArgs.PredicateParameters = new string[1] { "55630EBA-6A11-4001-B161-9AE77ACCA43D" };
			queryArgs.Order = "Created DESC";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs body = queryArgs;
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/MetadataValue/Gets").Result;
			DataTable dataTable = Common.getDataTable(result);
			if (dataTable != null)
			{
				dataTable.Dispose();
				cbbDepartment.ValueMember = "Id";
				cbbDepartment.DisplayMember = "Name";
				cbbDepartment.DataSource = dataTable;
			}
			else
			{
				cbbDepartment.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			cbbDepartment.DataSource = null;
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
		txtUsername.AutoCompleteCustomSource = Common.getAutoComplete(mData, "Username");
		txtFullname.AutoCompleteCustomSource = Common.getAutoComplete(mData, "FullName");
		txtEmail.AutoCompleteCustomSource = Common.getAutoComplete(mData, "Email");
		txtPhone.AutoCompleteCustomSource = Common.getAutoComplete(mData, "PhoneNumber");
		txtAddress.AutoCompleteCustomSource = Common.getAutoComplete(mData, "Address");
	}

	private void load_Data()
	{
		try
		{
			if (mData == null || mData.Rows.Count <= 0 || !(mId != Guid.Empty))
			{
				return;
			}
			DataRow dataRow = mData.Select($"Id='{mId}'").FirstOrDefault();
			if (dataRow.ItemArray.Length != 0)
			{
				txtUsername.Text = dataRow["Username"].ToString();
				txtFullname.Text = dataRow["FullName"].ToString();
				txtEmail.Text = dataRow["Email"].ToString();
				dtpBirthday.Text = dataRow["BirthDate"].ToString();
				cbbGender.Text = dataRow["Gender"].ToString();
				txtPhone.Text = dataRow["PhoneNumber"].ToString();
				txtAddress.Text = dataRow["Address"].ToString();
				cbbJobTitle.Text = dataRow["JobTitle"].ToString();
				if (dataRow["Departmentid"] != null)
				{
					cbbDepartment.SelectedValue = dataRow["Departmentid"];
				}
				else
				{
					cbbDepartment.SelectedIndex = -1;
				}
				if (!isAdd)
				{
					cbIsActivated.Checked = (bool)dataRow["IsActivated"];
					cbbPosition.Text = dataRow["Position"].ToString();
					txtAvatar.Text = dataRow["ImageUrl"].ToString();
					mFileName = dataRow["ImageUrl"].ToString();
					lblCreated.Text = dataRow["Created"].ToString();
					lblModified.Text = dataRow["Modified"].ToString();
					lblCreatedBy.Text = dataRow["CreatedBy"].ToString();
					lblModifiedBy.Text = dataRow["ModifiedBy"].ToString();
					cbIsEmail.Checked = (bool)dataRow["IsEmail"];
				}
			}
		}
		catch
		{
		}
	}

	private void btnComfirm_Click(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		try
		{
			if (string.IsNullOrEmpty(txtUsername.Text))
			{
				MessageBox.Show(Common.getTextLanguage(this, "wUsername"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				txtUsername.Focus();
				return;
			}
			if (string.IsNullOrEmpty(txtFullname.Text))
			{
				MessageBox.Show(Common.getTextLanguage(this, "wFullname"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				txtFullname.Focus();
				return;
			}
			if (string.IsNullOrEmpty(cbbGender.Text))
			{
				MessageBox.Show(Common.getTextLanguage(this, "wGender"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				cbbGender.Focus();
				return;
			}
			if (string.IsNullOrEmpty(cbbJobTitle.Text))
			{
				MessageBox.Show(Common.getTextLanguage(this, "wJobTitle"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				cbbJobTitle.Focus();
				return;
			}
			if (string.IsNullOrEmpty(cbbPosition.Text))
			{
				MessageBox.Show(Common.getTextLanguage(this, "wPosition"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				cbbPosition.Focus();
				return;
			}
			AuthUserViewModel authUserViewModel = new AuthUserViewModel
			{
				Username = txtUsername.Text,
				FullName = txtFullname.Text,
				Position = cbbPosition.Text,
				Gender = cbbGender.Text,
				IsActivated = cbIsActivated.Checked,
				IsEmail = cbIsEmail.Checked
			};
			if (!string.IsNullOrEmpty(txtEmail.Text))
			{
				authUserViewModel.Email = txtEmail.Text;
			}
			if (!string.IsNullOrEmpty(dtpBirthday.Text))
			{
				authUserViewModel.BirthDate = dtpBirthday.Value;
			}
			if (!string.IsNullOrEmpty(txtPhone.Text))
			{
				authUserViewModel.PhoneNumber = txtPhone.Text;
			}
			if (!string.IsNullOrEmpty(txtAddress.Text))
			{
				authUserViewModel.Address = txtAddress.Text;
			}
			if (!string.IsNullOrEmpty(cbbJobTitle.Text))
			{
				authUserViewModel.JobTitle = cbbJobTitle.Text;
			}
			if (!string.IsNullOrEmpty(cbbDepartment.Text))
			{
				authUserViewModel.DepartmentId = Guid.Parse(cbbDepartment.SelectedValue.ToString());
			}
			switch (cbbPosition.Text)
			{
			case "Staff":
				authUserViewModel.Role = RoleWeb.Member;
				break;
			case "Leader":
				authUserViewModel.Role = RoleWeb.Administrator;
				break;
			case "Manager":
				authUserViewModel.Role = RoleWeb.SuperAdministrator;
				break;
			default:
				authUserViewModel.Role = RoleWeb.None;
				break;
			}
			if (!isAdd && MessageBox.Show(Common.getTextLanguage(this, "wSureEdit"), Common.getTextLanguage(this, "QUESTION"), MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.No))
			{
				return;
			}
			Cursor.Current = Cursors.WaitCursor;
			if (!isAdd)
			{
				authUserViewModel.ImageUrl = mFileName;
				authUserViewModel.Id = mId;
			}
			ResponseDto result = frmLogin.client.SaveAsync(authUserViewModel, "/api/AuthUser/Save").Result;
			if (!result.Success)
			{
				throw new Exception(result.Messages.ElementAt(0).Message);
			}
			if (isAdd)
			{
				AuthUserViewModel authUserViewModel2 = JsonConvert.DeserializeObject<AuthUserViewModel>(result.Data.ToString());
				if (File.Exists(txtAvatar.Text))
				{
					using FileStream data = File.Open(txtAvatar.Text, FileMode.Open);
					FileParameter file = new FileParameter(data, txtAvatar.Text);
					ResponseDto result2 = frmLogin.client.ImportAsync(authUserViewModel2.Id, file, "/api/AuthUser/UpdateImage/{id}").Result;
				}
			}
			else if (File.Exists(txtAvatar.Text))
			{
				using FileStream data2 = File.Open(txtAvatar.Text, FileMode.Open);
				FileParameter file2 = new FileParameter(data2, txtAvatar.Text);
				ResponseDto result3 = frmLogin.client.ImportAsync(mId, file2, "/api/AuthUser/UpdateImage/{id}").Result;
			}
			else if (string.IsNullOrEmpty(txtAvatar.Text))
			{
				Stream data3 = new MemoryStream();
				FileParameter file3 = new FileParameter(data3);
				ResponseDto result4 = frmLogin.client.ImportAsync(mId, file3, "/api/AuthUser/UpdateImage/{id}").Result;
			}
			isClose = false;
			Close();
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

	private void txtNormal_Leave(object sender, EventArgs e)
	{
		TextBox textBox = sender as TextBox;
		textBox.Text = Common.trimSpace(textBox.Text);
	}

	private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
	{
		if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b' && e.KeyChar != ' ')
		{
			e.Handled = true;
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

	private void cbbNormal_Enter(object sender, EventArgs e)
	{
		ComboBox comboBox = sender as ComboBox;
		mRemember = comboBox.Text;
	}

	private void btnResetPass_Click(object sender, EventArgs e)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		try
		{
			if (MessageBox.Show(Common.getTextLanguage(this, "wSureReset"), Common.getTextLanguage(this, "QUESTION"), MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.Yes))
			{
				Cursor.Current = Cursors.WaitCursor;
				ResetPasswordDto body = new ResetPasswordDto
				{
					UserId = mId
				};
				ResponseDto result = frmLogin.client.SaveAsync(body, "/api/AuthUser/ResetPassword").Result;
				if (!result.Success)
				{
					throw new Exception(result.Messages.ElementAt(0).Message);
				}
				MessageBox.Show(Common.getTextLanguage(this, "wResetDefault"), Common.getTextLanguage(this, "INFORMATION"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
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
	}

	private void btnDepartmentAdd_Click(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		List<Type> list = new List<Type>();
		list.Add(typeof(frmSubView));
		Common.closeForm(list);
		new frmSubView(this, (FormType)4).Show();
	}

	private void btnAvatarFolder_Click(object sender, EventArgs e)
	{
		if (openFileDialogMain.ShowDialog().Equals(DialogResult.OK))
		{
			txtAvatar.Text = openFileDialogMain.FileName;
		}
	}

	private void btnAvatarDelete_Click(object sender, EventArgs e)
	{
		txtAvatar.Text = "";
	}

	private void txtAvatar_TextChanged(object sender, EventArgs e)
	{
		TextBox textBox = sender as TextBox;
		if (string.IsNullOrEmpty(textBox.Text))
		{
			btnAvatarDelete.Visible = false;
		}
		else
		{
			btnAvatarDelete.Visible = true;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_5S_QA_System.frmStaffAdd));
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.cbbDepartment = new System.Windows.Forms.ComboBox();
		this.cbbGender = new System.Windows.Forms.ComboBox();
		this.dtpBirthday = new System.Windows.Forms.DateTimePicker();
		this.cbbPosition = new System.Windows.Forms.ComboBox();
		this.btnConfirm = new System.Windows.Forms.Button();
		this.txtUsername = new System.Windows.Forms.TextBox();
		this.txtFullname = new System.Windows.Forms.TextBox();
		this.btnResetPass = new System.Windows.Forms.Button();
		this.txtEmail = new System.Windows.Forms.TextBox();
		this.txtPhone = new System.Windows.Forms.TextBox();
		this.txtAddress = new System.Windows.Forms.TextBox();
		this.txtAvatar = new System.Windows.Forms.TextBox();
		this.btnAvatarFolder = new System.Windows.Forms.Button();
		this.btnAvatarDelete = new System.Windows.Forms.Button();
		this.btnDepartmentAdd = new System.Windows.Forms.Button();
		this.cbIsActivated = new System.Windows.Forms.CheckBox();
		this.cbbJobTitle = new System.Windows.Forms.ComboBox();
		this.panelMain = new System.Windows.Forms.Panel();
		this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
		this.lblJobTitle = new System.Windows.Forms.Label();
		this.lblIsActivated = new System.Windows.Forms.Label();
		this.lblModifiedBy = new System.Windows.Forms.Label();
		this.lblModifiBy = new System.Windows.Forms.Label();
		this.lblCreatedBy = new System.Windows.Forms.Label();
		this.lblModified = new System.Windows.Forms.Label();
		this.lblCreateBy = new System.Windows.Forms.Label();
		this.lblCreated = new System.Windows.Forms.Label();
		this.lblDepartment = new System.Windows.Forms.Label();
		this.lblModifi = new System.Windows.Forms.Label();
		this.lblEmail = new System.Windows.Forms.Label();
		this.lblCreate = new System.Windows.Forms.Label();
		this.lblPosition = new System.Windows.Forms.Label();
		this.lblAvatar = new System.Windows.Forms.Label();
		this.lblFullname = new System.Windows.Forms.Label();
		this.lblAddress = new System.Windows.Forms.Label();
		this.lblPhone = new System.Windows.Forms.Label();
		this.lblPassword = new System.Windows.Forms.Label();
		this.lblGender = new System.Windows.Forms.Label();
		this.lblBirthday = new System.Windows.Forms.Label();
		this.lblUsername = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.openFileDialogMain = new System.Windows.Forms.OpenFileDialog();
		this.lblIsEmail = new System.Windows.Forms.Label();
		this.cbIsEmail = new System.Windows.Forms.CheckBox();
		this.panelMain.SuspendLayout();
		this.tableLayoutPanel2.SuspendLayout();
		this.panel1.SuspendLayout();
		this.panel2.SuspendLayout();
		base.SuspendLayout();
		this.cbbDepartment.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cbbDepartment.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cbbDepartment.Dock = System.Windows.Forms.DockStyle.Fill;
		this.cbbDepartment.FormattingEnabled = true;
		this.cbbDepartment.ItemHeight = 16;
		this.cbbDepartment.Location = new System.Drawing.Point(0, 0);
		this.cbbDepartment.MaxLength = 50;
		this.cbbDepartment.Name = "cbbDepartment";
		this.cbbDepartment.Size = new System.Drawing.Size(359, 24);
		this.cbbDepartment.TabIndex = 1;
		this.toolTipMain.SetToolTip(this.cbbDepartment, "Please select or enter department");
		this.cbbDepartment.Enter += new System.EventHandler(cbbNormal_Enter);
		this.cbbDepartment.Leave += new System.EventHandler(cbbNormal_Leave);
		this.cbbGender.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cbbGender.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cbbGender.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbbGender.Dock = System.Windows.Forms.DockStyle.Fill;
		this.cbbGender.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbbGender.FormattingEnabled = true;
		this.cbbGender.ItemHeight = 16;
		this.cbbGender.Items.AddRange(new object[2] { "Male", "Female" });
		this.cbbGender.Location = new System.Drawing.Point(105, 177);
		this.cbbGender.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.cbbGender.Name = "cbbGender";
		this.cbbGender.Size = new System.Drawing.Size(383, 24);
		this.cbbGender.TabIndex = 7;
		this.toolTipMain.SetToolTip(this.cbbGender, "Please select gender");
		this.dtpBirthday.Dock = System.Windows.Forms.DockStyle.Fill;
		this.dtpBirthday.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.dtpBirthday.Location = new System.Drawing.Point(105, 149);
		this.dtpBirthday.Name = "dtpBirthday";
		this.dtpBirthday.Size = new System.Drawing.Size(383, 22);
		this.dtpBirthday.TabIndex = 6;
		this.toolTipMain.SetToolTip(this.dtpBirthday, "Select birthday for staff");
		this.cbbPosition.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cbbPosition.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cbbPosition.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbbPosition.Dock = System.Windows.Forms.DockStyle.Fill;
		this.cbbPosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbbPosition.FormattingEnabled = true;
		this.cbbPosition.ItemHeight = 16;
		this.cbbPosition.Items.AddRange(new object[3] { "Staff", "Leader", "Manager" });
		this.cbbPosition.Location = new System.Drawing.Point(105, 322);
		this.cbbPosition.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.cbbPosition.Name = "cbbPosition";
		this.cbbPosition.Size = new System.Drawing.Size(383, 24);
		this.cbbPosition.TabIndex = 12;
		this.toolTipMain.SetToolTip(this.cbbPosition, "Please select permission");
		this.btnConfirm.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnConfirm.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.btnConfirm.FlatAppearance.BorderSize = 0;
		this.btnConfirm.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
		this.btnConfirm.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
		this.btnConfirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnConfirm.Location = new System.Drawing.Point(20, 623);
		this.btnConfirm.Name = "btnConfirm";
		this.btnConfirm.Size = new System.Drawing.Size(500, 28);
		this.btnConfirm.TabIndex = 1;
		this.btnConfirm.Text = "Confirm";
		this.toolTipMain.SetToolTip(this.btnConfirm, "Confirm");
		this.btnConfirm.UseVisualStyleBackColor = true;
		this.btnConfirm.Click += new System.EventHandler(btnComfirm_Click);
		this.txtUsername.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtUsername.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtUsername.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtUsername.Location = new System.Drawing.Point(105, 4);
		this.txtUsername.Name = "txtUsername";
		this.txtUsername.Size = new System.Drawing.Size(383, 22);
		this.txtUsername.TabIndex = 1;
		this.toolTipMain.SetToolTip(this.txtUsername, "Please enter username");
		this.txtFullname.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtFullname.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtFullname.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtFullname.Location = new System.Drawing.Point(105, 62);
		this.txtFullname.Name = "txtFullname";
		this.txtFullname.Size = new System.Drawing.Size(383, 22);
		this.txtFullname.TabIndex = 3;
		this.toolTipMain.SetToolTip(this.txtFullname, "Please enter full name");
		this.txtFullname.Leave += new System.EventHandler(txtNormal_Leave);
		this.btnResetPass.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnResetPass.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnResetPass.FlatAppearance.BorderSize = 0;
		this.btnResetPass.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
		this.btnResetPass.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
		this.btnResetPass.Location = new System.Drawing.Point(105, 32);
		this.btnResetPass.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnResetPass.Name = "btnResetPass";
		this.btnResetPass.Size = new System.Drawing.Size(383, 24);
		this.btnResetPass.TabIndex = 2;
		this.btnResetPass.Text = "Reset";
		this.toolTipMain.SetToolTip(this.btnResetPass, "Reset default password");
		this.btnResetPass.UseVisualStyleBackColor = true;
		this.btnResetPass.Click += new System.EventHandler(btnResetPass_Click);
		this.txtEmail.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtEmail.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtEmail.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtEmail.Location = new System.Drawing.Point(105, 91);
		this.txtEmail.Name = "txtEmail";
		this.txtEmail.Size = new System.Drawing.Size(383, 22);
		this.txtEmail.TabIndex = 4;
		this.toolTipMain.SetToolTip(this.txtEmail, "Please enter email");
		this.txtEmail.Leave += new System.EventHandler(txtNormal_Leave);
		this.txtPhone.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtPhone.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtPhone.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtPhone.Location = new System.Drawing.Point(105, 207);
		this.txtPhone.Name = "txtPhone";
		this.txtPhone.Size = new System.Drawing.Size(383, 22);
		this.txtPhone.TabIndex = 8;
		this.toolTipMain.SetToolTip(this.txtPhone, "Please enter phone number");
		this.txtPhone.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtPhone_KeyPress);
		this.txtAddress.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtAddress.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtAddress.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtAddress.Location = new System.Drawing.Point(105, 236);
		this.txtAddress.Name = "txtAddress";
		this.txtAddress.Size = new System.Drawing.Size(383, 22);
		this.txtAddress.TabIndex = 9;
		this.toolTipMain.SetToolTip(this.txtAddress, "Please enter address");
		this.txtAddress.Leave += new System.EventHandler(txtNormal_Leave);
		this.txtAvatar.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtAvatar.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtAvatar.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtAvatar.Enabled = false;
		this.txtAvatar.Location = new System.Drawing.Point(0, 0);
		this.txtAvatar.Name = "txtAvatar";
		this.txtAvatar.Size = new System.Drawing.Size(361, 22);
		this.txtAvatar.TabIndex = 1;
		this.txtAvatar.TabStop = false;
		this.toolTipMain.SetToolTip(this.txtAvatar, "Please enter avatar url");
		this.txtAvatar.TextChanged += new System.EventHandler(txtAvatar_TextChanged);
		this.btnAvatarFolder.BackColor = System.Drawing.Color.Gainsboro;
		this.btnAvatarFolder.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnAvatarFolder.Dock = System.Windows.Forms.DockStyle.Right;
		this.btnAvatarFolder.FlatAppearance.BorderSize = 0;
		this.btnAvatarFolder.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
		this.btnAvatarFolder.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SkyBlue;
		this.btnAvatarFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAvatarFolder.Image = _5S_QA_System.Properties.Resources.folder;
		this.btnAvatarFolder.Location = new System.Drawing.Point(361, 0);
		this.btnAvatarFolder.Name = "btnAvatarFolder";
		this.btnAvatarFolder.Size = new System.Drawing.Size(22, 22);
		this.btnAvatarFolder.TabIndex = 2;
		this.toolTipMain.SetToolTip(this.btnAvatarFolder, "Open folder to select image");
		this.btnAvatarFolder.UseVisualStyleBackColor = false;
		this.btnAvatarFolder.Click += new System.EventHandler(btnAvatarFolder_Click);
		this.btnAvatarDelete.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnAvatarDelete.BackColor = System.Drawing.SystemColors.Control;
		this.btnAvatarDelete.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnAvatarDelete.FlatAppearance.BorderSize = 0;
		this.btnAvatarDelete.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
		this.btnAvatarDelete.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.AppWorkspace;
		this.btnAvatarDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAvatarDelete.Image = _5S_QA_System.Properties.Resources.cancel;
		this.btnAvatarDelete.Location = new System.Drawing.Point(339, 1);
		this.btnAvatarDelete.Margin = new System.Windows.Forms.Padding(0);
		this.btnAvatarDelete.Name = "btnAvatarDelete";
		this.btnAvatarDelete.Size = new System.Drawing.Size(20, 20);
		this.btnAvatarDelete.TabIndex = 3;
		this.btnAvatarDelete.TabStop = false;
		this.toolTipMain.SetToolTip(this.btnAvatarDelete, "Clear image url");
		this.btnAvatarDelete.UseVisualStyleBackColor = false;
		this.btnAvatarDelete.Visible = false;
		this.btnAvatarDelete.Click += new System.EventHandler(btnAvatarDelete_Click);
		this.btnDepartmentAdd.BackColor = System.Drawing.Color.Gainsboro;
		this.btnDepartmentAdd.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnDepartmentAdd.Dock = System.Windows.Forms.DockStyle.Right;
		this.btnDepartmentAdd.FlatAppearance.BorderSize = 0;
		this.btnDepartmentAdd.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
		this.btnDepartmentAdd.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SkyBlue;
		this.btnDepartmentAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnDepartmentAdd.Location = new System.Drawing.Point(359, 0);
		this.btnDepartmentAdd.Name = "btnDepartmentAdd";
		this.btnDepartmentAdd.Size = new System.Drawing.Size(24, 24);
		this.btnDepartmentAdd.TabIndex = 2;
		this.btnDepartmentAdd.Text = "+";
		this.toolTipMain.SetToolTip(this.btnDepartmentAdd, "Goto manage department");
		this.btnDepartmentAdd.UseVisualStyleBackColor = false;
		this.btnDepartmentAdd.Click += new System.EventHandler(btnDepartmentAdd_Click);
		this.cbIsActivated.AutoSize = true;
		this.cbIsActivated.Checked = true;
		this.cbIsActivated.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cbIsActivated.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbIsActivated.Dock = System.Windows.Forms.DockStyle.Fill;
		this.cbIsActivated.Enabled = false;
		this.cbIsActivated.Location = new System.Drawing.Point(105, 381);
		this.cbIsActivated.Name = "cbIsActivated";
		this.cbIsActivated.Size = new System.Drawing.Size(383, 22);
		this.cbIsActivated.TabIndex = 14;
		this.toolTipMain.SetToolTip(this.cbIsActivated, "Select to active");
		this.cbIsActivated.UseVisualStyleBackColor = true;
		this.cbbJobTitle.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cbbJobTitle.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cbbJobTitle.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbbJobTitle.Dock = System.Windows.Forms.DockStyle.Fill;
		this.cbbJobTitle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbbJobTitle.FormattingEnabled = true;
		this.cbbJobTitle.ItemHeight = 16;
		this.cbbJobTitle.Location = new System.Drawing.Point(105, 293);
		this.cbbJobTitle.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.cbbJobTitle.Name = "cbbJobTitle";
		this.cbbJobTitle.Size = new System.Drawing.Size(383, 24);
		this.cbbJobTitle.TabIndex = 11;
		this.toolTipMain.SetToolTip(this.cbbJobTitle, "Please select job title");
		this.panelMain.AutoScroll = true;
		this.panelMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panelMain.Controls.Add(this.tableLayoutPanel2);
		this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panelMain.Location = new System.Drawing.Point(20, 70);
		this.panelMain.Name = "panelMain";
		this.panelMain.Padding = new System.Windows.Forms.Padding(3);
		this.panelMain.Size = new System.Drawing.Size(500, 553);
		this.panelMain.TabIndex = 1;
		this.tableLayoutPanel2.AutoSize = true;
		this.tableLayoutPanel2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		this.tableLayoutPanel2.ColumnCount = 2;
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel2.Controls.Add(this.cbIsEmail, 1, 4);
		this.tableLayoutPanel2.Controls.Add(this.lblIsEmail, 0, 4);
		this.tableLayoutPanel2.Controls.Add(this.cbbJobTitle, 1, 10);
		this.tableLayoutPanel2.Controls.Add(this.lblJobTitle, 0, 10);
		this.tableLayoutPanel2.Controls.Add(this.cbIsActivated, 1, 13);
		this.tableLayoutPanel2.Controls.Add(this.lblIsActivated, 0, 13);
		this.tableLayoutPanel2.Controls.Add(this.txtPhone, 1, 7);
		this.tableLayoutPanel2.Controls.Add(this.btnResetPass, 1, 1);
		this.tableLayoutPanel2.Controls.Add(this.txtAddress, 1, 8);
		this.tableLayoutPanel2.Controls.Add(this.txtEmail, 1, 3);
		this.tableLayoutPanel2.Controls.Add(this.lblModifiedBy, 1, 18);
		this.tableLayoutPanel2.Controls.Add(this.txtFullname, 1, 2);
		this.tableLayoutPanel2.Controls.Add(this.txtUsername, 1, 0);
		this.tableLayoutPanel2.Controls.Add(this.lblModifiBy, 0, 18);
		this.tableLayoutPanel2.Controls.Add(this.lblCreatedBy, 1, 17);
		this.tableLayoutPanel2.Controls.Add(this.cbbPosition, 1, 11);
		this.tableLayoutPanel2.Controls.Add(this.lblModified, 1, 16);
		this.tableLayoutPanel2.Controls.Add(this.lblCreateBy, 0, 17);
		this.tableLayoutPanel2.Controls.Add(this.lblCreated, 1, 15);
		this.tableLayoutPanel2.Controls.Add(this.lblDepartment, 0, 12);
		this.tableLayoutPanel2.Controls.Add(this.lblModifi, 0, 16);
		this.tableLayoutPanel2.Controls.Add(this.lblEmail, 0, 3);
		this.tableLayoutPanel2.Controls.Add(this.lblCreate, 0, 15);
		this.tableLayoutPanel2.Controls.Add(this.lblPosition, 0, 11);
		this.tableLayoutPanel2.Controls.Add(this.lblAvatar, 0, 9);
		this.tableLayoutPanel2.Controls.Add(this.lblFullname, 0, 2);
		this.tableLayoutPanel2.Controls.Add(this.lblAddress, 0, 8);
		this.tableLayoutPanel2.Controls.Add(this.lblPhone, 0, 7);
		this.tableLayoutPanel2.Controls.Add(this.lblPassword, 0, 1);
		this.tableLayoutPanel2.Controls.Add(this.lblGender, 0, 6);
		this.tableLayoutPanel2.Controls.Add(this.lblBirthday, 0, 5);
		this.tableLayoutPanel2.Controls.Add(this.lblUsername, 0, 0);
		this.tableLayoutPanel2.Controls.Add(this.panel1, 1, 9);
		this.tableLayoutPanel2.Controls.Add(this.dtpBirthday, 1, 5);
		this.tableLayoutPanel2.Controls.Add(this.cbbGender, 1, 6);
		this.tableLayoutPanel2.Controls.Add(this.panel2, 1, 12);
		this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
		this.tableLayoutPanel2.Name = "tableLayoutPanel2";
		this.tableLayoutPanel2.RowCount = 19;
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel2.Size = new System.Drawing.Size(492, 524);
		this.tableLayoutPanel2.TabIndex = 1;
		this.lblJobTitle.AutoSize = true;
		this.lblJobTitle.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblJobTitle.Location = new System.Drawing.Point(4, 291);
		this.lblJobTitle.Name = "lblJobTitle";
		this.lblJobTitle.Size = new System.Drawing.Size(94, 28);
		this.lblJobTitle.TabIndex = 34;
		this.lblJobTitle.Text = "Job title";
		this.lblJobTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblIsActivated.AutoSize = true;
		this.lblIsActivated.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblIsActivated.Location = new System.Drawing.Point(4, 378);
		this.lblIsActivated.Name = "lblIsActivated";
		this.lblIsActivated.Size = new System.Drawing.Size(94, 28);
		this.lblIsActivated.TabIndex = 33;
		this.lblIsActivated.Text = "Is activated";
		this.lblIsActivated.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblModifiedBy.AutoSize = true;
		this.lblModifiedBy.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblModifiedBy.Location = new System.Drawing.Point(105, 495);
		this.lblModifiedBy.Name = "lblModifiedBy";
		this.lblModifiedBy.Size = new System.Drawing.Size(383, 28);
		this.lblModifiedBy.TabIndex = 32;
		this.lblModifiedBy.Text = "...";
		this.lblModifiedBy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblModifiBy.AutoSize = true;
		this.lblModifiBy.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblModifiBy.Location = new System.Drawing.Point(4, 495);
		this.lblModifiBy.Name = "lblModifiBy";
		this.lblModifiBy.Size = new System.Drawing.Size(94, 28);
		this.lblModifiBy.TabIndex = 32;
		this.lblModifiBy.Text = "Edited by";
		this.lblModifiBy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblCreatedBy.AutoSize = true;
		this.lblCreatedBy.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCreatedBy.Location = new System.Drawing.Point(105, 466);
		this.lblCreatedBy.Name = "lblCreatedBy";
		this.lblCreatedBy.Size = new System.Drawing.Size(383, 28);
		this.lblCreatedBy.TabIndex = 31;
		this.lblCreatedBy.Text = "...";
		this.lblCreatedBy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblModified.AutoSize = true;
		this.lblModified.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblModified.Location = new System.Drawing.Point(105, 437);
		this.lblModified.Name = "lblModified";
		this.lblModified.Size = new System.Drawing.Size(383, 28);
		this.lblModified.TabIndex = 30;
		this.lblModified.Text = "...";
		this.lblModified.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblCreateBy.AutoSize = true;
		this.lblCreateBy.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCreateBy.Location = new System.Drawing.Point(4, 466);
		this.lblCreateBy.Name = "lblCreateBy";
		this.lblCreateBy.Size = new System.Drawing.Size(94, 28);
		this.lblCreateBy.TabIndex = 31;
		this.lblCreateBy.Text = "Created by";
		this.lblCreateBy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblCreated.AutoSize = true;
		this.lblCreated.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCreated.Location = new System.Drawing.Point(105, 408);
		this.lblCreated.Name = "lblCreated";
		this.lblCreated.Size = new System.Drawing.Size(383, 28);
		this.lblCreated.TabIndex = 29;
		this.lblCreated.Text = "...";
		this.lblCreated.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblDepartment.AutoSize = true;
		this.lblDepartment.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblDepartment.Location = new System.Drawing.Point(4, 349);
		this.lblDepartment.Name = "lblDepartment";
		this.lblDepartment.Size = new System.Drawing.Size(94, 28);
		this.lblDepartment.TabIndex = 28;
		this.lblDepartment.Text = "Department";
		this.lblDepartment.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblModifi.AutoSize = true;
		this.lblModifi.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblModifi.Location = new System.Drawing.Point(4, 437);
		this.lblModifi.Name = "lblModifi";
		this.lblModifi.Size = new System.Drawing.Size(94, 28);
		this.lblModifi.TabIndex = 30;
		this.lblModifi.Text = "Edited date";
		this.lblModifi.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblEmail.AutoSize = true;
		this.lblEmail.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblEmail.Location = new System.Drawing.Point(4, 88);
		this.lblEmail.Name = "lblEmail";
		this.lblEmail.Size = new System.Drawing.Size(94, 28);
		this.lblEmail.TabIndex = 21;
		this.lblEmail.Text = "Email";
		this.lblEmail.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblCreate.AutoSize = true;
		this.lblCreate.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCreate.Location = new System.Drawing.Point(4, 408);
		this.lblCreate.Name = "lblCreate";
		this.lblCreate.Size = new System.Drawing.Size(94, 28);
		this.lblCreate.TabIndex = 29;
		this.lblCreate.Text = "Create date";
		this.lblCreate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblPosition.AutoSize = true;
		this.lblPosition.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblPosition.Location = new System.Drawing.Point(4, 320);
		this.lblPosition.Name = "lblPosition";
		this.lblPosition.Size = new System.Drawing.Size(94, 28);
		this.lblPosition.TabIndex = 27;
		this.lblPosition.Text = "Permission";
		this.lblPosition.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblAvatar.AutoSize = true;
		this.lblAvatar.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblAvatar.Location = new System.Drawing.Point(4, 262);
		this.lblAvatar.Name = "lblAvatar";
		this.lblAvatar.Size = new System.Drawing.Size(94, 28);
		this.lblAvatar.TabIndex = 26;
		this.lblAvatar.Text = "Avatar url";
		this.lblAvatar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblFullname.AutoSize = true;
		this.lblFullname.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblFullname.Location = new System.Drawing.Point(4, 59);
		this.lblFullname.Name = "lblFullname";
		this.lblFullname.Size = new System.Drawing.Size(94, 28);
		this.lblFullname.TabIndex = 20;
		this.lblFullname.Text = "Full name";
		this.lblFullname.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblAddress.AutoSize = true;
		this.lblAddress.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblAddress.Location = new System.Drawing.Point(4, 233);
		this.lblAddress.Name = "lblAddress";
		this.lblAddress.Size = new System.Drawing.Size(94, 28);
		this.lblAddress.TabIndex = 25;
		this.lblAddress.Text = "Address";
		this.lblAddress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblPhone.AutoSize = true;
		this.lblPhone.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblPhone.Location = new System.Drawing.Point(4, 204);
		this.lblPhone.Name = "lblPhone";
		this.lblPhone.Size = new System.Drawing.Size(94, 28);
		this.lblPhone.TabIndex = 24;
		this.lblPhone.Text = "Phone";
		this.lblPhone.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblPassword.AutoSize = true;
		this.lblPassword.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblPassword.Location = new System.Drawing.Point(4, 30);
		this.lblPassword.Name = "lblPassword";
		this.lblPassword.Size = new System.Drawing.Size(94, 28);
		this.lblPassword.TabIndex = 19;
		this.lblPassword.Text = "Password";
		this.lblPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblGender.AutoSize = true;
		this.lblGender.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblGender.Location = new System.Drawing.Point(4, 175);
		this.lblGender.Name = "lblGender";
		this.lblGender.Size = new System.Drawing.Size(94, 28);
		this.lblGender.TabIndex = 23;
		this.lblGender.Text = "Gender";
		this.lblGender.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblBirthday.AutoSize = true;
		this.lblBirthday.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblBirthday.Location = new System.Drawing.Point(4, 146);
		this.lblBirthday.Name = "lblBirthday";
		this.lblBirthday.Size = new System.Drawing.Size(94, 28);
		this.lblBirthday.TabIndex = 22;
		this.lblBirthday.Text = "Birthday";
		this.lblBirthday.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblUsername.AutoSize = true;
		this.lblUsername.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblUsername.Location = new System.Drawing.Point(4, 1);
		this.lblUsername.Name = "lblUsername";
		this.lblUsername.Size = new System.Drawing.Size(94, 28);
		this.lblUsername.TabIndex = 18;
		this.lblUsername.Text = "Username";
		this.lblUsername.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.panel1.Controls.Add(this.btnAvatarDelete);
		this.panel1.Controls.Add(this.txtAvatar);
		this.panel1.Controls.Add(this.btnAvatarFolder);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(105, 265);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(383, 22);
		this.panel1.TabIndex = 10;
		this.panel1.TabStop = true;
		this.panel2.Controls.Add(this.cbbDepartment);
		this.panel2.Controls.Add(this.btnDepartmentAdd);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(105, 351);
		this.panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(383, 24);
		this.panel2.TabIndex = 13;
		this.panel2.TabStop = true;
		this.openFileDialogMain.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png, *.bmp)| *.jpg; *.jpeg; *.jpe; *.jfif; *.png; *.bmp";
		this.openFileDialogMain.Title = "Please select a picture";
		this.lblIsEmail.AutoSize = true;
		this.lblIsEmail.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblIsEmail.Location = new System.Drawing.Point(4, 117);
		this.lblIsEmail.Name = "lblIsEmail";
		this.lblIsEmail.Size = new System.Drawing.Size(94, 28);
		this.lblIsEmail.TabIndex = 35;
		this.lblIsEmail.Text = "Receive email";
		this.lblIsEmail.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.cbIsEmail.AutoSize = true;
		this.cbIsEmail.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbIsEmail.Dock = System.Windows.Forms.DockStyle.Fill;
		this.cbIsEmail.Location = new System.Drawing.Point(105, 120);
		this.cbIsEmail.Name = "cbIsEmail";
		this.cbIsEmail.Size = new System.Drawing.Size(383, 22);
		this.cbIsEmail.TabIndex = 5;
		this.toolTipMain.SetToolTip(this.cbIsEmail, "Select to receive email");
		this.cbIsEmail.UseVisualStyleBackColor = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(540, 671);
		base.Controls.Add(this.panelMain);
		base.Controls.Add(this.btnConfirm);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "frmStaffAdd";
		base.Padding = new System.Windows.Forms.Padding(20, 70, 20, 20);
		this.Text = "5S QA System * STAFF";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmStaffAdd_FormClosing);
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmStaffAdd_FormClosed);
		base.Load += new System.EventHandler(frmStaffAdd_Load);
		base.Shown += new System.EventHandler(frmStaffAdd_Shown);
		this.panelMain.ResumeLayout(false);
		this.panelMain.PerformLayout();
		this.tableLayoutPanel2.ResumeLayout(false);
		this.tableLayoutPanel2.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.panel2.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
