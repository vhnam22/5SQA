using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using _5S_QA_DL.APIContext;
using _5S_QA_Entities.Abstracts;
using _5S_QA_Entities.Dtos;
using _5S_QA_Entities.Enums;
using _5S_QA_Entities.Models;
using _5S_QA_System.Controls;
using _5S_QA_System.Properties;
using MetroFramework.Forms;
using Newtonsoft.Json;

namespace _5S_QA_System;

public class frmProductAdd : MetroForm
{
	private readonly Form mForm;

	private Guid mId;

	private readonly DataTable mData;

	private readonly ProductGroupViewModel mGroup;

	private string mRemember;

	private string mFileName;

	public bool isClose;

	private readonly bool isAdd;

	private IContainer components = null;

	private ToolTip toolTipMain;

	private Panel panelMain;

	private TableLayoutPanel tableLayoutPanel2;

	private Button btnConfirm;

	private OpenFileDialog openFileDialogMain;

	private ComboBox cbbCavity;

	private ComboBox cbbSampleMax;

	private Label lblTemplate;

	private Label lblSampleMax;

	private Label lblCavity;

	private Label lblImageUrl;

	private Label lblDescription;

	private Label lblName;

	private Label lblCode;

	private Label lblCreate;

	private Label lblModifi;

	private Label lblCreateBy;

	private Label lblModifiBy;

	private Label lblCreated;

	private Label lblModified;

	private Label lblCreatedBy;

	private Label lblModifiedBy;

	private TextBox txtCode;

	private TextBox txtName;

	private TextBox txtDescription;

	private Panel panel1;

	private Button btnAvatarDelete;

	private TextBox txtImageUrl;

	private Button btnAvatarFolder;

	private Panel panel2;

	private ComboBox cbbTemplate;

	private Button btnTemplateAdd;

	private Label lblIsActivated;

	public CheckBox cbIsActivated;

	private Label lblDepartment;

	private Panel panel3;

	private ComboBox cbbDepartment;

	private Button btnDepartmentAdd;

	private Label lblChecksheet;

	private Panel panelChecksheet;

	private Button btnChecksheetDelete;

	private TextBox txtChecksheet;

	private Button btnChecksheetFolder;

	public frmProductAdd(Form frm, DataTable data, ProductGroupViewModel group, Guid id = default(Guid), bool isadd = true)
	{
		InitializeComponent();
		Common.setControls(this, toolTipMain);
		mForm = frm;
		isClose = true;
		mData = data;
		mId = id;
		mGroup = group;
		isAdd = isadd;
		string text = "ADD";
		if (!isAdd)
		{
			text = "EDIT";
			cbIsActivated.Enabled = true;
			panelChecksheet.Enabled = false;
		}
		string text2 = Common.getTextLanguage(this, text);
		if (string.IsNullOrEmpty(text2))
		{
			text2 = text;
		}
		Text = Text + " (" + text2 + ")";
	}

	private void frmProductAdd_Load(object sender, EventArgs e)
	{
		Init();
	}

	private void frmProductAdd_Shown(object sender, EventArgs e)
	{
		load_cbbTemplate();
		load_cbbDepartment();
		if (isAdd)
		{
			cbbTemplate.SelectedIndex = -1;
			cbbCavity.SelectedIndex = 0;
			cbbSampleMax.SelectedIndex = 0;
			txtCode.Text = set_Code();
		}
		load_Data();
		txtCode.Select();
	}

	private void frmProductAdd_FormClosing(object sender, FormClosingEventArgs e)
	{
		GC.Collect();
	}

	private void frmProductAdd_FormClosed(object sender, FormClosedEventArgs e)
	{
		List<Type> list = new List<Type>();
		list.Add(typeof(frmSubView));
		list.Add(typeof(frmTemplateAdd));
		Common.closeForm(list);
		mData?.Dispose();
		if (!isClose)
		{
			((frmProductView)mForm).isClose = false;
			((frmProductView)mForm).load_AllData();
			if (isAdd)
			{
				((frmProductView)mForm).OpenMeasurement(mId, txtChecksheet.Text);
			}
		}
	}

	private void Init()
	{
		btnAvatarDelete.Visible = false;
		load_AutoComplete();
		cbbSampleMax.Text = Settings.Default.Limit.ToString();
		lblCreated.Text = "";
		lblModified.Text = "";
		lblCreatedBy.Text = "";
		lblModifiedBy.Text = "";
	}

	private void load_AutoComplete()
	{
		txtCode.AutoCompleteCustomSource = Common.getAutoComplete(mData, "Code");
		txtName.AutoCompleteCustomSource = Common.getAutoComplete(mData, "Name");
		txtDescription.AutoCompleteCustomSource = Common.getAutoComplete(mData, "Description");
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
				txtName.Text = dataRow["Name"].ToString();
				txtDescription.Text = dataRow["Description"].ToString();
				cbbCavity.Text = dataRow["Cavity"].ToString();
				cbbSampleMax.Text = dataRow["SampleMax"].ToString();
				if (dataRow["TemplateId"] != null)
				{
					cbbTemplate.SelectedValue = dataRow["TemplateId"];
				}
				else
				{
					cbbTemplate.SelectedIndex = -1;
				}
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
					txtCode.Text = dataRow["Code"].ToString();
					cbIsActivated.Checked = (bool)dataRow["IsActivated"];
					txtImageUrl.Text = dataRow["ImageUrl"].ToString();
					mFileName = dataRow["ImageUrl"].ToString();
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

	private string setCodeTemplate()
	{
		string text = "TEMP-";
		try
		{
			QueryArgs body = new QueryArgs
			{
				Order = "Created DESC",
				Page = 1,
				Limit = 1
			};
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/Template/Gets").Result;
			DataTable dataTable = Common.getDataTable<TemplateViewModel>(result);
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
		catch
		{
		}
		return text;
	}

	private string set_Code()
	{
		string text = "STA-";
		try
		{
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "GroupId=@0";
			queryArgs.PredicateParameters = new string[1] { ((AuditableEntity)(object)mGroup).Id.ToString() };
			queryArgs.Order = "Created DESC";
			queryArgs.Page = 1;
			queryArgs.Limit = 1;
			QueryArgs body = queryArgs;
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/Product/Gets").Result;
			DataTable dataTable = Common.getDataTable<ProductViewModel>(result);
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
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Expected O, but got Unknown
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Expected O, but got Unknown
		Cursor.Current = Cursors.WaitCursor;
		try
		{
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
			if (string.IsNullOrEmpty(cbbCavity.Text))
			{
				MessageBox.Show(Common.getTextLanguage(this, "wCavity"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				cbbCavity.Focus();
				return;
			}
			if (string.IsNullOrEmpty(cbbSampleMax.Text))
			{
				MessageBox.Show(Common.getTextLanguage(this, "wMaxSample"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				cbbSampleMax.Focus();
				return;
			}
			int result;
			ProductViewModel val = new ProductViewModel
			{
				Name = txtName.Text,
				Code = txtCode.Text,
				Cavity = ((!int.TryParse(cbbCavity.Text, out result)) ? 1 : result),
				SampleMax = ((!int.TryParse(cbbSampleMax.Text, out result)) ? 1 : result)
			};
			((AuditableEntity)val).IsActivated = cbIsActivated.Checked;
			val.GroupId = ((AuditableEntity)(object)mGroup).Id;
			ProductViewModel val2 = val;
			if (!string.IsNullOrEmpty(txtDescription.Text))
			{
				val2.Description = txtDescription.Text;
			}
			if (!string.IsNullOrEmpty(cbbTemplate.Text))
			{
				val2.TemplateId = Guid.Parse(cbbTemplate.SelectedValue.ToString());
			}
			if (!string.IsNullOrEmpty(cbbDepartment.Text))
			{
				val2.DepartmentId = Guid.Parse(cbbDepartment.SelectedValue.ToString());
			}
			if (!isAdd && MessageBox.Show(Common.getTextLanguage(this, "wSureEdit"), Common.getTextLanguage(this, "QUESTION"), MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.No))
			{
				return;
			}
			Cursor.Current = Cursors.WaitCursor;
			if (!isAdd)
			{
				val2.ImageUrl = mFileName;
				((AuditableEntity)(object)val2).Id = mId;
			}
			ResponseDto result2 = frmLogin.client.SaveAsync(val2, "/api/Product/Save").Result;
			if (!result2.Success)
			{
				throw new Exception(result2.Messages.ElementAt(0).Message);
			}
			if (isAdd)
			{
				ProductViewModel val3 = JsonConvert.DeserializeObject<ProductViewModel>(result2.Data.ToString());
				mId = ((AuditableEntity)(object)val3).Id;
				if (File.Exists(txtImageUrl.Text))
				{
					using FileStream data = File.Open(txtImageUrl.Text, FileMode.Open);
					FileParameter file = new FileParameter(data, txtImageUrl.Text);
					ResponseDto result3 = frmLogin.client.ImportAsync(((AuditableEntity)(object)val3).Id, file, "/api/Product/UpdateImage/{id}").Result;
				}
			}
			else if (File.Exists(txtImageUrl.Text))
			{
				using FileStream data2 = File.Open(txtImageUrl.Text, FileMode.Open);
				FileParameter file2 = new FileParameter(data2, txtImageUrl.Text);
				ResponseDto result4 = frmLogin.client.ImportAsync(mId, file2, "/api/Product/UpdateImage/{id}").Result;
			}
			else if (string.IsNullOrEmpty(txtImageUrl.Text))
			{
				Stream data3 = new MemoryStream();
				FileParameter file3 = new FileParameter(data3);
				ResponseDto result5 = frmLogin.client.ImportAsync(mId, file3, "/api/Product/UpdateImage/{id}").Result;
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
		TextBox obj = sender as TextBox;
		obj.Text = Common.trimSpace(obj.Text);
		if (obj.Name.Equals(txtName.Name) && cbbTemplate.DataSource != null)
		{
			DataTable dataTable = cbbTemplate.DataSource as DataTable;
			DataRow dataRow = (from DataRow x in dataTable.Rows
				where x.Field<string>("Name").Equals(mGroup.Code + "#" + obj.Text)
				select x).FirstOrDefault();
			if (dataRow == null)
			{
				cbbTemplate.SelectedIndex = -1;
			}
			else
			{
				cbbTemplate.Text = dataRow["Name"].ToString();
			}
		}
	}

	private void btnAvatarDelete_Click(object sender, EventArgs e)
	{
		txtImageUrl.Text = "";
	}

	private void btnAvatarFolder_Click(object sender, EventArgs e)
	{
		openFileDialogMain.Filter = "Image file (*.jpg, *.jpeg, *.jpe, *.jfif, *.png, *.bmp)| *.jpg; *.jpeg; *.jpe; *.jfif; *.png; *.bmp";
		if (openFileDialogMain.ShowDialog().Equals(DialogResult.OK))
		{
			txtImageUrl.Text = openFileDialogMain.FileName;
		}
	}

	private void txtImageUrl_TextChanged(object sender, EventArgs e)
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

	private void cbbNormal_Enter(object sender, EventArgs e)
	{
		ComboBox comboBox = sender as ComboBox;
		mRemember = comboBox.Text;
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
		if (string.IsNullOrEmpty(comboBox.Text))
		{
			comboBox.Text = mRemember;
		}
		comboBox.Text = ((!int.TryParse(comboBox.Text, out var result)) ? "1" : (result.Equals(0) ? "1" : result.ToString()));
	}

	private void cbbSample_KeyPress(object sender, KeyPressEventArgs e)
	{
		if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b')
		{
			e.Handled = true;
		}
	}

	private void btnTemplateAdd_Click(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		if (cbbTemplate.SelectedIndex.Equals(-1))
		{
			List<Type> list = new List<Type>();
			list.Add(typeof(frmTemplateAdd));
			Common.closeForm(list);
			new frmTemplateAdd(this, mGroup.Code + "#" + txtName.Text, txtDescription.Text, cbbSampleMax.Text).Show();
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

	private void btnChecksheetFolder_Click(object sender, EventArgs e)
	{
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		openFileDialogMain.Filter = "File excel (*.xls, *.xlsx, *.xlsm)| *.xls; *.xlsx; *.xlsm";
		if (!openFileDialogMain.ShowDialog().Equals(DialogResult.OK))
		{
			return;
		}
		Cursor.Current = Cursors.WaitCursor;
		try
		{
			CellAddressDto val = Common.ReadExcelForDetail(openFileDialogMain.FileName);
			if (!string.IsNullOrEmpty(val.Message))
			{
				throw new Exception(val.Message);
			}
			RequestViewModel request = Common.GetRequest(val.CellAddresses, openFileDialogMain.FileName);
			if (string.IsNullOrEmpty(request.ProductCode))
			{
				throw new Exception(Common.getTextLanguage(this, "wIncorrectFormat"));
			}
			string text = Common.SaveFileForDetail(val.CellAddresses, openFileDialogMain.FileName, 0);
			if (text.Contains("ERROR: "))
			{
				throw new Exception(text);
			}
			TemplateViewModel val2 = new TemplateViewModel
			{
				Name = mGroup.Code + "#" + request.ProductStage,
				Code = setCodeTemplate()
			};
			((AuditableEntity)val2).IsActivated = true;
			val2.Type = "Detail";
			val2.Limit = request.Sample;
			TemplateViewModel val3 = val2;
			ResponseDto result = frmLogin.client.SaveAsync(val3, "/api/Template/Save").Result;
			if (result.Success)
			{
				TemplateViewModel val4 = JsonConvert.DeserializeObject<TemplateViewModel>(result.Data.ToString());
				if (File.Exists(text))
				{
					using FileStream data = File.OpenRead(text);
					FileParameter file = new FileParameter(data, text);
					ResponseDto result2 = frmLogin.client.ImportAsync(((AuditableEntity)(object)val4).Id, file, "/api/Template/UpdateExcel/{id}").Result;
				}
				load_cbbTemplate();
			}
			Common.ExecuteBatFile(text);
			txtName.Text = request.ProductStage;
			cbbCavity.Text = ((!request.ProductCavity.HasValue) ? "1" : request.ProductCavity.ToString());
			cbbTemplate.Text = val3.Name;
			txtChecksheet.Text = openFileDialogMain.FileName;
			cbbSampleMax.Text = request.Sample.ToString();
			cbbDepartment.SelectedIndex = -1;
			cbbDepartment.Text = request.ProductDepartment;
			if (cbbDepartment.SelectedIndex == -1)
			{
				cbbDepartment.Text = "";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			txtChecksheet.Text = null;
		}
	}

	private void btnChecksheetDelete_Click(object sender, EventArgs e)
	{
		txtChecksheet.Text = "";
	}

	private void txtChecksheet_TextChanged(object sender, EventArgs e)
	{
		TextBox textBox = sender as TextBox;
		if (string.IsNullOrEmpty(textBox.Text))
		{
			btnChecksheetDelete.Visible = false;
		}
		else
		{
			btnChecksheetDelete.Visible = true;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_5S_QA_System.frmProductAdd));
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.btnConfirm = new System.Windows.Forms.Button();
		this.cbbCavity = new System.Windows.Forms.ComboBox();
		this.cbbSampleMax = new System.Windows.Forms.ComboBox();
		this.txtCode = new System.Windows.Forms.TextBox();
		this.txtName = new System.Windows.Forms.TextBox();
		this.txtDescription = new System.Windows.Forms.TextBox();
		this.btnAvatarDelete = new System.Windows.Forms.Button();
		this.txtImageUrl = new System.Windows.Forms.TextBox();
		this.btnAvatarFolder = new System.Windows.Forms.Button();
		this.cbbTemplate = new System.Windows.Forms.ComboBox();
		this.btnTemplateAdd = new System.Windows.Forms.Button();
		this.cbIsActivated = new System.Windows.Forms.CheckBox();
		this.cbbDepartment = new System.Windows.Forms.ComboBox();
		this.btnDepartmentAdd = new System.Windows.Forms.Button();
		this.btnChecksheetDelete = new System.Windows.Forms.Button();
		this.txtChecksheet = new System.Windows.Forms.TextBox();
		this.btnChecksheetFolder = new System.Windows.Forms.Button();
		this.panelMain = new System.Windows.Forms.Panel();
		this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
		this.panelChecksheet = new System.Windows.Forms.Panel();
		this.lblChecksheet = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.lblDepartment = new System.Windows.Forms.Label();
		this.lblIsActivated = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel1 = new System.Windows.Forms.Panel();
		this.lblModifiedBy = new System.Windows.Forms.Label();
		this.lblCreatedBy = new System.Windows.Forms.Label();
		this.lblModified = new System.Windows.Forms.Label();
		this.lblCreated = new System.Windows.Forms.Label();
		this.lblModifiBy = new System.Windows.Forms.Label();
		this.lblCreateBy = new System.Windows.Forms.Label();
		this.lblModifi = new System.Windows.Forms.Label();
		this.lblCreate = new System.Windows.Forms.Label();
		this.lblTemplate = new System.Windows.Forms.Label();
		this.lblSampleMax = new System.Windows.Forms.Label();
		this.lblCavity = new System.Windows.Forms.Label();
		this.lblImageUrl = new System.Windows.Forms.Label();
		this.lblDescription = new System.Windows.Forms.Label();
		this.lblName = new System.Windows.Forms.Label();
		this.lblCode = new System.Windows.Forms.Label();
		this.openFileDialogMain = new System.Windows.Forms.OpenFileDialog();
		this.panelMain.SuspendLayout();
		this.tableLayoutPanel2.SuspendLayout();
		this.panelChecksheet.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel1.SuspendLayout();
		base.SuspendLayout();
		this.btnConfirm.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnConfirm.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.btnConfirm.FlatAppearance.BorderSize = 0;
		this.btnConfirm.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
		this.btnConfirm.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
		this.btnConfirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnConfirm.Location = new System.Drawing.Point(20, 486);
		this.btnConfirm.Name = "btnConfirm";
		this.btnConfirm.Size = new System.Drawing.Size(500, 28);
		this.btnConfirm.TabIndex = 1;
		this.btnConfirm.Text = "Confirm";
		this.toolTipMain.SetToolTip(this.btnConfirm, "Confirm");
		this.btnConfirm.UseVisualStyleBackColor = true;
		this.btnConfirm.Click += new System.EventHandler(btnComfirm_Click);
		this.cbbCavity.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cbbCavity.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cbbCavity.Dock = System.Windows.Forms.DockStyle.Fill;
		this.cbbCavity.FormattingEnabled = true;
		this.cbbCavity.Items.AddRange(new object[10] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" });
		this.cbbCavity.Location = new System.Drawing.Point(94, 148);
		this.cbbCavity.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.cbbCavity.Name = "cbbCavity";
		this.cbbCavity.Size = new System.Drawing.Size(394, 24);
		this.cbbCavity.TabIndex = 5;
		this.toolTipMain.SetToolTip(this.cbbCavity, "Select or enter cavity quantity");
		this.cbbCavity.Enter += new System.EventHandler(cbbNormal_Enter);
		this.cbbCavity.KeyPress += new System.Windows.Forms.KeyPressEventHandler(cbbSample_KeyPress);
		this.cbbCavity.Leave += new System.EventHandler(cbbNormalNotNull_Leave);
		this.cbbSampleMax.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cbbSampleMax.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cbbSampleMax.Dock = System.Windows.Forms.DockStyle.Fill;
		this.cbbSampleMax.FormattingEnabled = true;
		this.cbbSampleMax.Items.AddRange(new object[10] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" });
		this.cbbSampleMax.Location = new System.Drawing.Point(94, 177);
		this.cbbSampleMax.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.cbbSampleMax.Name = "cbbSampleMax";
		this.cbbSampleMax.Size = new System.Drawing.Size(394, 24);
		this.cbbSampleMax.TabIndex = 6;
		this.toolTipMain.SetToolTip(this.cbbSampleMax, "Select or enter maximum sample");
		this.cbbSampleMax.Enter += new System.EventHandler(cbbNormal_Enter);
		this.cbbSampleMax.KeyPress += new System.Windows.Forms.KeyPressEventHandler(cbbSample_KeyPress);
		this.cbbSampleMax.Leave += new System.EventHandler(cbbNormalNotNull_Leave);
		this.txtCode.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtCode.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtCode.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtCode.Enabled = false;
		this.txtCode.Location = new System.Drawing.Point(94, 33);
		this.txtCode.Name = "txtCode";
		this.txtCode.Size = new System.Drawing.Size(394, 22);
		this.txtCode.TabIndex = 1;
		this.toolTipMain.SetToolTip(this.txtCode, "Please enter code");
		this.txtCode.Leave += new System.EventHandler(txtNormal_Leave);
		this.txtName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtName.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtName.Location = new System.Drawing.Point(94, 62);
		this.txtName.Name = "txtName";
		this.txtName.Size = new System.Drawing.Size(394, 22);
		this.txtName.TabIndex = 2;
		this.toolTipMain.SetToolTip(this.txtName, "Please enter name");
		this.txtName.Leave += new System.EventHandler(txtNormal_Leave);
		this.txtDescription.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtDescription.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtDescription.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtDescription.Location = new System.Drawing.Point(94, 91);
		this.txtDescription.Name = "txtDescription";
		this.txtDescription.Size = new System.Drawing.Size(394, 22);
		this.txtDescription.TabIndex = 3;
		this.toolTipMain.SetToolTip(this.txtDescription, "Please enter description");
		this.txtDescription.Leave += new System.EventHandler(txtNormal_Leave);
		this.btnAvatarDelete.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnAvatarDelete.BackColor = System.Drawing.SystemColors.Control;
		this.btnAvatarDelete.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnAvatarDelete.FlatAppearance.BorderSize = 0;
		this.btnAvatarDelete.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
		this.btnAvatarDelete.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.AppWorkspace;
		this.btnAvatarDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAvatarDelete.Image = _5S_QA_System.Properties.Resources.cancel;
		this.btnAvatarDelete.Location = new System.Drawing.Point(350, 1);
		this.btnAvatarDelete.Margin = new System.Windows.Forms.Padding(0);
		this.btnAvatarDelete.Name = "btnAvatarDelete";
		this.btnAvatarDelete.Size = new System.Drawing.Size(20, 20);
		this.btnAvatarDelete.TabIndex = 3;
		this.btnAvatarDelete.TabStop = false;
		this.toolTipMain.SetToolTip(this.btnAvatarDelete, "Clear image url");
		this.btnAvatarDelete.UseVisualStyleBackColor = false;
		this.btnAvatarDelete.Visible = false;
		this.btnAvatarDelete.Click += new System.EventHandler(btnAvatarDelete_Click);
		this.txtImageUrl.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtImageUrl.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtImageUrl.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtImageUrl.Enabled = false;
		this.txtImageUrl.Location = new System.Drawing.Point(0, 0);
		this.txtImageUrl.Name = "txtImageUrl";
		this.txtImageUrl.Size = new System.Drawing.Size(372, 22);
		this.txtImageUrl.TabIndex = 1;
		this.txtImageUrl.TabStop = false;
		this.toolTipMain.SetToolTip(this.txtImageUrl, "Please enter avatar url");
		this.txtImageUrl.TextChanged += new System.EventHandler(txtImageUrl_TextChanged);
		this.btnAvatarFolder.BackColor = System.Drawing.Color.Gainsboro;
		this.btnAvatarFolder.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnAvatarFolder.Dock = System.Windows.Forms.DockStyle.Right;
		this.btnAvatarFolder.FlatAppearance.BorderSize = 0;
		this.btnAvatarFolder.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
		this.btnAvatarFolder.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SkyBlue;
		this.btnAvatarFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAvatarFolder.Image = _5S_QA_System.Properties.Resources.folder;
		this.btnAvatarFolder.Location = new System.Drawing.Point(372, 0);
		this.btnAvatarFolder.Name = "btnAvatarFolder";
		this.btnAvatarFolder.Size = new System.Drawing.Size(22, 22);
		this.btnAvatarFolder.TabIndex = 1;
		this.toolTipMain.SetToolTip(this.btnAvatarFolder, "Open folder to select image");
		this.btnAvatarFolder.UseVisualStyleBackColor = false;
		this.btnAvatarFolder.Click += new System.EventHandler(btnAvatarFolder_Click);
		this.cbbTemplate.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cbbTemplate.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cbbTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
		this.cbbTemplate.FormattingEnabled = true;
		this.cbbTemplate.ItemHeight = 16;
		this.cbbTemplate.Location = new System.Drawing.Point(0, 0);
		this.cbbTemplate.MaxLength = 50;
		this.cbbTemplate.Name = "cbbTemplate";
		this.cbbTemplate.Size = new System.Drawing.Size(370, 24);
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
		this.btnTemplateAdd.Location = new System.Drawing.Point(370, 0);
		this.btnTemplateAdd.Name = "btnTemplateAdd";
		this.btnTemplateAdd.Size = new System.Drawing.Size(24, 24);
		this.btnTemplateAdd.TabIndex = 2;
		this.btnTemplateAdd.Text = "+";
		this.toolTipMain.SetToolTip(this.btnTemplateAdd, "Goto manage template");
		this.btnTemplateAdd.UseVisualStyleBackColor = false;
		this.btnTemplateAdd.Click += new System.EventHandler(btnTemplateAdd_Click);
		this.cbIsActivated.AutoSize = true;
		this.cbIsActivated.Checked = true;
		this.cbIsActivated.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cbIsActivated.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbIsActivated.Dock = System.Windows.Forms.DockStyle.Fill;
		this.cbIsActivated.Enabled = false;
		this.cbIsActivated.Location = new System.Drawing.Point(94, 265);
		this.cbIsActivated.Name = "cbIsActivated";
		this.cbIsActivated.Size = new System.Drawing.Size(394, 22);
		this.cbIsActivated.TabIndex = 9;
		this.toolTipMain.SetToolTip(this.cbIsActivated, "Select to active");
		this.cbIsActivated.UseVisualStyleBackColor = true;
		this.cbbDepartment.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cbbDepartment.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cbbDepartment.Dock = System.Windows.Forms.DockStyle.Fill;
		this.cbbDepartment.FormattingEnabled = true;
		this.cbbDepartment.ItemHeight = 16;
		this.cbbDepartment.Location = new System.Drawing.Point(0, 0);
		this.cbbDepartment.MaxLength = 50;
		this.cbbDepartment.Name = "cbbDepartment";
		this.cbbDepartment.Size = new System.Drawing.Size(370, 24);
		this.cbbDepartment.TabIndex = 1;
		this.toolTipMain.SetToolTip(this.cbbDepartment, "Please select or enter department");
		this.btnDepartmentAdd.BackColor = System.Drawing.Color.Gainsboro;
		this.btnDepartmentAdd.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnDepartmentAdd.Dock = System.Windows.Forms.DockStyle.Right;
		this.btnDepartmentAdd.FlatAppearance.BorderSize = 0;
		this.btnDepartmentAdd.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
		this.btnDepartmentAdd.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SkyBlue;
		this.btnDepartmentAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnDepartmentAdd.Location = new System.Drawing.Point(370, 0);
		this.btnDepartmentAdd.Name = "btnDepartmentAdd";
		this.btnDepartmentAdd.Size = new System.Drawing.Size(24, 24);
		this.btnDepartmentAdd.TabIndex = 2;
		this.btnDepartmentAdd.Text = "+";
		this.toolTipMain.SetToolTip(this.btnDepartmentAdd, "Goto manage department");
		this.btnDepartmentAdd.UseVisualStyleBackColor = false;
		this.btnDepartmentAdd.Click += new System.EventHandler(btnDepartmentAdd_Click);
		this.btnChecksheetDelete.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnChecksheetDelete.BackColor = System.Drawing.SystemColors.Control;
		this.btnChecksheetDelete.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnChecksheetDelete.FlatAppearance.BorderSize = 0;
		this.btnChecksheetDelete.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
		this.btnChecksheetDelete.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.AppWorkspace;
		this.btnChecksheetDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnChecksheetDelete.Image = _5S_QA_System.Properties.Resources.cancel;
		this.btnChecksheetDelete.Location = new System.Drawing.Point(350, 1);
		this.btnChecksheetDelete.Margin = new System.Windows.Forms.Padding(0);
		this.btnChecksheetDelete.Name = "btnChecksheetDelete";
		this.btnChecksheetDelete.Size = new System.Drawing.Size(20, 20);
		this.btnChecksheetDelete.TabIndex = 3;
		this.btnChecksheetDelete.TabStop = false;
		this.toolTipMain.SetToolTip(this.btnChecksheetDelete, "Clear checksheet url");
		this.btnChecksheetDelete.UseVisualStyleBackColor = false;
		this.btnChecksheetDelete.Visible = false;
		this.btnChecksheetDelete.Click += new System.EventHandler(btnChecksheetDelete_Click);
		this.txtChecksheet.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtChecksheet.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtChecksheet.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtChecksheet.Enabled = false;
		this.txtChecksheet.Location = new System.Drawing.Point(0, 0);
		this.txtChecksheet.Name = "txtChecksheet";
		this.txtChecksheet.Size = new System.Drawing.Size(372, 22);
		this.txtChecksheet.TabIndex = 1;
		this.txtChecksheet.TabStop = false;
		this.toolTipMain.SetToolTip(this.txtChecksheet, "Please enter checksheet url");
		this.txtChecksheet.TextChanged += new System.EventHandler(txtChecksheet_TextChanged);
		this.btnChecksheetFolder.BackColor = System.Drawing.Color.Gainsboro;
		this.btnChecksheetFolder.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnChecksheetFolder.Dock = System.Windows.Forms.DockStyle.Right;
		this.btnChecksheetFolder.FlatAppearance.BorderSize = 0;
		this.btnChecksheetFolder.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
		this.btnChecksheetFolder.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SkyBlue;
		this.btnChecksheetFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnChecksheetFolder.Image = _5S_QA_System.Properties.Resources.folder;
		this.btnChecksheetFolder.Location = new System.Drawing.Point(372, 0);
		this.btnChecksheetFolder.Name = "btnChecksheetFolder";
		this.btnChecksheetFolder.Size = new System.Drawing.Size(22, 22);
		this.btnChecksheetFolder.TabIndex = 1;
		this.toolTipMain.SetToolTip(this.btnChecksheetFolder, "Open folder to select checksheet");
		this.btnChecksheetFolder.UseVisualStyleBackColor = false;
		this.btnChecksheetFolder.Click += new System.EventHandler(btnChecksheetFolder_Click);
		this.panelMain.AutoScroll = true;
		this.panelMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panelMain.Controls.Add(this.tableLayoutPanel2);
		this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panelMain.Location = new System.Drawing.Point(20, 70);
		this.panelMain.Name = "panelMain";
		this.panelMain.Padding = new System.Windows.Forms.Padding(3);
		this.panelMain.Size = new System.Drawing.Size(500, 416);
		this.panelMain.TabIndex = 1;
		this.tableLayoutPanel2.AutoSize = true;
		this.tableLayoutPanel2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		this.tableLayoutPanel2.ColumnCount = 2;
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel2.Controls.Add(this.panelChecksheet, 1, 0);
		this.tableLayoutPanel2.Controls.Add(this.lblChecksheet, 0, 0);
		this.tableLayoutPanel2.Controls.Add(this.panel3, 1, 8);
		this.tableLayoutPanel2.Controls.Add(this.lblDepartment, 0, 8);
		this.tableLayoutPanel2.Controls.Add(this.cbIsActivated, 1, 9);
		this.tableLayoutPanel2.Controls.Add(this.lblIsActivated, 0, 9);
		this.tableLayoutPanel2.Controls.Add(this.panel2, 1, 7);
		this.tableLayoutPanel2.Controls.Add(this.panel1, 1, 4);
		this.tableLayoutPanel2.Controls.Add(this.txtDescription, 1, 3);
		this.tableLayoutPanel2.Controls.Add(this.txtName, 1, 2);
		this.tableLayoutPanel2.Controls.Add(this.txtCode, 1, 1);
		this.tableLayoutPanel2.Controls.Add(this.lblModifiedBy, 1, 14);
		this.tableLayoutPanel2.Controls.Add(this.lblCreatedBy, 1, 13);
		this.tableLayoutPanel2.Controls.Add(this.lblModified, 1, 12);
		this.tableLayoutPanel2.Controls.Add(this.lblCreated, 1, 11);
		this.tableLayoutPanel2.Controls.Add(this.lblModifiBy, 0, 14);
		this.tableLayoutPanel2.Controls.Add(this.lblCreateBy, 0, 13);
		this.tableLayoutPanel2.Controls.Add(this.lblModifi, 0, 12);
		this.tableLayoutPanel2.Controls.Add(this.lblCreate, 0, 11);
		this.tableLayoutPanel2.Controls.Add(this.lblTemplate, 0, 7);
		this.tableLayoutPanel2.Controls.Add(this.lblSampleMax, 0, 6);
		this.tableLayoutPanel2.Controls.Add(this.lblCavity, 0, 5);
		this.tableLayoutPanel2.Controls.Add(this.cbbSampleMax, 1, 6);
		this.tableLayoutPanel2.Controls.Add(this.lblImageUrl, 0, 4);
		this.tableLayoutPanel2.Controls.Add(this.cbbCavity, 1, 5);
		this.tableLayoutPanel2.Controls.Add(this.lblDescription, 0, 3);
		this.tableLayoutPanel2.Controls.Add(this.lblName, 0, 2);
		this.tableLayoutPanel2.Controls.Add(this.lblCode, 0, 1);
		this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
		this.tableLayoutPanel2.Name = "tableLayoutPanel2";
		this.tableLayoutPanel2.RowCount = 15;
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.Size = new System.Drawing.Size(492, 408);
		this.tableLayoutPanel2.TabIndex = 1;
		this.panelChecksheet.Controls.Add(this.btnChecksheetDelete);
		this.panelChecksheet.Controls.Add(this.txtChecksheet);
		this.panelChecksheet.Controls.Add(this.btnChecksheetFolder);
		this.panelChecksheet.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panelChecksheet.Location = new System.Drawing.Point(94, 4);
		this.panelChecksheet.Name = "panelChecksheet";
		this.panelChecksheet.Size = new System.Drawing.Size(394, 22);
		this.panelChecksheet.TabIndex = 0;
		this.panelChecksheet.TabStop = true;
		this.lblChecksheet.AutoSize = true;
		this.lblChecksheet.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblChecksheet.Location = new System.Drawing.Point(4, 1);
		this.lblChecksheet.Name = "lblChecksheet";
		this.lblChecksheet.Size = new System.Drawing.Size(83, 28);
		this.lblChecksheet.TabIndex = 85;
		this.lblChecksheet.Text = "Checksheet";
		this.lblChecksheet.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.panel3.Controls.Add(this.cbbDepartment);
		this.panel3.Controls.Add(this.btnDepartmentAdd);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(94, 235);
		this.panel3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(394, 24);
		this.panel3.TabIndex = 8;
		this.panel3.TabStop = true;
		this.lblDepartment.AutoSize = true;
		this.lblDepartment.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblDepartment.Location = new System.Drawing.Point(4, 233);
		this.lblDepartment.Name = "lblDepartment";
		this.lblDepartment.Size = new System.Drawing.Size(83, 28);
		this.lblDepartment.TabIndex = 83;
		this.lblDepartment.Text = "Department";
		this.lblDepartment.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblIsActivated.AutoSize = true;
		this.lblIsActivated.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblIsActivated.Location = new System.Drawing.Point(4, 262);
		this.lblIsActivated.Name = "lblIsActivated";
		this.lblIsActivated.Size = new System.Drawing.Size(83, 28);
		this.lblIsActivated.TabIndex = 82;
		this.lblIsActivated.Text = "Is activated";
		this.lblIsActivated.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.panel2.Controls.Add(this.cbbTemplate);
		this.panel2.Controls.Add(this.btnTemplateAdd);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(94, 206);
		this.panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(394, 24);
		this.panel2.TabIndex = 7;
		this.panel2.TabStop = true;
		this.panel1.Controls.Add(this.btnAvatarDelete);
		this.panel1.Controls.Add(this.txtImageUrl);
		this.panel1.Controls.Add(this.btnAvatarFolder);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(94, 120);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(394, 22);
		this.panel1.TabIndex = 4;
		this.panel1.TabStop = true;
		this.lblModifiedBy.AutoSize = true;
		this.lblModifiedBy.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblModifiedBy.Location = new System.Drawing.Point(94, 379);
		this.lblModifiedBy.Name = "lblModifiedBy";
		this.lblModifiedBy.Size = new System.Drawing.Size(394, 28);
		this.lblModifiedBy.TabIndex = 81;
		this.lblModifiedBy.Text = "...";
		this.lblModifiedBy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblCreatedBy.AutoSize = true;
		this.lblCreatedBy.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCreatedBy.Location = new System.Drawing.Point(94, 350);
		this.lblCreatedBy.Name = "lblCreatedBy";
		this.lblCreatedBy.Size = new System.Drawing.Size(394, 28);
		this.lblCreatedBy.TabIndex = 80;
		this.lblCreatedBy.Text = "...";
		this.lblCreatedBy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblModified.AutoSize = true;
		this.lblModified.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblModified.Location = new System.Drawing.Point(94, 321);
		this.lblModified.Name = "lblModified";
		this.lblModified.Size = new System.Drawing.Size(394, 28);
		this.lblModified.TabIndex = 79;
		this.lblModified.Text = "...";
		this.lblModified.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblCreated.AutoSize = true;
		this.lblCreated.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCreated.Location = new System.Drawing.Point(94, 292);
		this.lblCreated.Name = "lblCreated";
		this.lblCreated.Size = new System.Drawing.Size(394, 28);
		this.lblCreated.TabIndex = 78;
		this.lblCreated.Text = "...";
		this.lblCreated.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblModifiBy.AutoSize = true;
		this.lblModifiBy.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblModifiBy.Location = new System.Drawing.Point(4, 379);
		this.lblModifiBy.Name = "lblModifiBy";
		this.lblModifiBy.Size = new System.Drawing.Size(83, 28);
		this.lblModifiBy.TabIndex = 77;
		this.lblModifiBy.Text = "Edited by";
		this.lblModifiBy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblCreateBy.AutoSize = true;
		this.lblCreateBy.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCreateBy.Location = new System.Drawing.Point(4, 350);
		this.lblCreateBy.Name = "lblCreateBy";
		this.lblCreateBy.Size = new System.Drawing.Size(83, 28);
		this.lblCreateBy.TabIndex = 76;
		this.lblCreateBy.Text = "Created by";
		this.lblCreateBy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblModifi.AutoSize = true;
		this.lblModifi.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblModifi.Location = new System.Drawing.Point(4, 321);
		this.lblModifi.Name = "lblModifi";
		this.lblModifi.Size = new System.Drawing.Size(83, 28);
		this.lblModifi.TabIndex = 75;
		this.lblModifi.Text = "Edited date";
		this.lblModifi.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblCreate.AutoSize = true;
		this.lblCreate.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCreate.Location = new System.Drawing.Point(4, 292);
		this.lblCreate.Name = "lblCreate";
		this.lblCreate.Size = new System.Drawing.Size(83, 28);
		this.lblCreate.TabIndex = 74;
		this.lblCreate.Text = "Create date";
		this.lblCreate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblTemplate.AutoSize = true;
		this.lblTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTemplate.Location = new System.Drawing.Point(4, 204);
		this.lblTemplate.Name = "lblTemplate";
		this.lblTemplate.Size = new System.Drawing.Size(83, 28);
		this.lblTemplate.TabIndex = 25;
		this.lblTemplate.Text = "Template";
		this.lblTemplate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblSampleMax.AutoSize = true;
		this.lblSampleMax.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblSampleMax.Location = new System.Drawing.Point(4, 175);
		this.lblSampleMax.Name = "lblSampleMax";
		this.lblSampleMax.Size = new System.Drawing.Size(83, 28);
		this.lblSampleMax.TabIndex = 24;
		this.lblSampleMax.Text = "Max. sample";
		this.lblSampleMax.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblCavity.AutoSize = true;
		this.lblCavity.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCavity.Location = new System.Drawing.Point(4, 146);
		this.lblCavity.Name = "lblCavity";
		this.lblCavity.Size = new System.Drawing.Size(83, 28);
		this.lblCavity.TabIndex = 23;
		this.lblCavity.Text = "Cavity";
		this.lblCavity.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblImageUrl.AutoSize = true;
		this.lblImageUrl.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblImageUrl.Location = new System.Drawing.Point(4, 117);
		this.lblImageUrl.Name = "lblImageUrl";
		this.lblImageUrl.Size = new System.Drawing.Size(83, 28);
		this.lblImageUrl.TabIndex = 22;
		this.lblImageUrl.Text = "Image url";
		this.lblImageUrl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblDescription.AutoSize = true;
		this.lblDescription.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblDescription.Location = new System.Drawing.Point(4, 88);
		this.lblDescription.Name = "lblDescription";
		this.lblDescription.Size = new System.Drawing.Size(83, 28);
		this.lblDescription.TabIndex = 21;
		this.lblDescription.Text = "Description";
		this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblName.AutoSize = true;
		this.lblName.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblName.Location = new System.Drawing.Point(4, 59);
		this.lblName.Name = "lblName";
		this.lblName.Size = new System.Drawing.Size(83, 28);
		this.lblName.TabIndex = 20;
		this.lblName.Text = "Name";
		this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblCode.AutoSize = true;
		this.lblCode.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCode.Location = new System.Drawing.Point(4, 30);
		this.lblCode.Name = "lblCode";
		this.lblCode.Size = new System.Drawing.Size(83, 28);
		this.lblCode.TabIndex = 19;
		this.lblCode.Text = "Code";
		this.lblCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.openFileDialogMain.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png, *.bmp)| *.jpg; *.jpeg; *.jpe; *.jfif; *.png; *.bmp";
		this.openFileDialogMain.Title = "Please select a picture";
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(540, 534);
		base.Controls.Add(this.panelMain);
		base.Controls.Add(this.btnConfirm);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "frmProductAdd";
		base.Padding = new System.Windows.Forms.Padding(20, 70, 20, 20);
		this.Text = "5S QA System * PRODUCT";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmProductAdd_FormClosing);
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmProductAdd_FormClosed);
		base.Load += new System.EventHandler(frmProductAdd_Load);
		base.Shown += new System.EventHandler(frmProductAdd_Shown);
		this.panelMain.ResumeLayout(false);
		this.panelMain.PerformLayout();
		this.tableLayoutPanel2.ResumeLayout(false);
		this.tableLayoutPanel2.PerformLayout();
		this.panelChecksheet.ResumeLayout(false);
		this.panelChecksheet.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		base.ResumeLayout(false);
	}
}
