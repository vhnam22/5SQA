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

public class frmMeasurementAdd : MetroForm
{
	private readonly Form mForm;

	private Guid mId;

	private Guid mIdParent;

	private string mRemember;

	private readonly DataTable mData;

	public bool isClose;

	private readonly bool isAdd;

	private string rememberCode;

	public bool isFomulaOpen;

	private TextBox mTxt;

	private IContainer components = null;

	private ToolTip toolTipMain;

	private Panel panelMain;

	private TableLayoutPanel tableLayoutPanelMain;

	private Button btnConfirm;

	private Label lblLCL;

	private Label lblUCL;

	private Label lblLower;

	private Label lblUpper;

	private Label lblUnit;

	private Label lblValue;

	private Label lblImportant;

	private Label lblName;

	private Label lblCode;

	private Label lblMacType;

	private Label lblTemplate;

	private Label lblFormula;

	private Label lblCoordinate;

	private Label lblCreate;

	private Label lblModifi;

	private Label lblCreateBy;

	private Label lblModifiBy;

	private Label lblCreated;

	private Label lblModified;

	private Label lblCreatedBy;

	private Label lblModifiedBy;

	private Panel panel1;

	private ComboBox cbbMacType;

	private Button btnMacTypeAdd;

	private Panel panel2;

	private TextBox txtCode;

	private CheckBox cbCodeEnable;

	private TextBox txtValue;

	private TextBox txtLower;

	private TextBox txtUpper;

	private TextBox txtName;

	private TextBox txtFormula;

	private TextBox txtLCL;

	private TextBox txtUCL;

	private TextBox txtCoordinate;

	private Panel panel3;

	private ComboBox cbbImportant;

	private Button btnImportantAdd;

	private Panel panel4;

	private ComboBox cbbUnit;

	private Button btnUnitAdd;

	private Panel panel5;

	private ComboBox cbbTemplate;

	private Button btnTemplateAdd;

	private TableLayoutPanel tpanelSubDegrees;

	private TextBox txtSeconds;

	private Label lblSeconds;

	private TextBox txtMinutes;

	private Label lblMinutes;

	private TextBox txtDegrees;

	private Label lblDegrees;

	private Button btnOK;

	private TableLayoutPanel tpanelDegrees;

	private TextBox txtSign;

	public frmMeasurementAdd(Form frm, DataTable data, Guid idparent, Guid id = default(Guid), bool isadd = true)
	{
		InitializeComponent();
		Common.setControls(this, toolTipMain);
		mForm = frm;
		isClose = true;
		mData = data;
		mId = id;
		mIdParent = idparent;
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
		isFomulaOpen = false;
	}

	private void frmMeasurementAdd_Load(object sender, EventArgs e)
	{
		Init();
	}

	private void frmMeasurementAdd_Shown(object sender, EventArgs e)
	{
		load_cbbMacType();
		load_cbbUnit();
		load_cbbImportant();
		load_cbbTemplate();
		if (isAdd)
		{
			cbbImportant.SelectedIndex = -1;
			cbbUnit.SelectedIndex = -1;
			cbbTemplate.SelectedIndex = -1;
			txtCode.Text = set_Code();
		}
		load_Data();
		cbbMacType.Select();
		rememberCode = txtCode.Text;
	}

	private void frmMeasurementAdd_FormClosing(object sender, FormClosingEventArgs e)
	{
		GC.Collect();
	}

	private void frmMeasurementAdd_FormClosed(object sender, FormClosedEventArgs e)
	{
		List<Type> list = new List<Type>();
		list.Add(typeof(frmSubView));
		list.Add(typeof(frmTemplateView));
		list.Add(typeof(frmFormula));
		Common.closeForm(list);
		mData?.Dispose();
		if (!isClose)
		{
			((frmMeasurementView)mForm).isClose = false;
			((frmMeasurementView)mForm).load_AllData();
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

	public void load_cbbImportant()
	{
		try
		{
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "TypeId=@0";
			queryArgs.PredicateParameters = new string[1] { "6042BF53-9411-47D4-9BD6-F8AB7BABB663" };
			queryArgs.Order = "Created DESC";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs body = queryArgs;
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/MetadataValue/Gets").Result;
			DataTable dataTable = Common.getDataTable(result);
			if (dataTable != null)
			{
				dataTable.Dispose();
				cbbImportant.ValueMember = "Id";
				cbbImportant.DisplayMember = "Name";
				cbbImportant.DataSource = dataTable;
			}
			else
			{
				cbbImportant.DataSource = null;
			}
			if (!cbbImportant.Enabled)
			{
				cbbImportant.SelectedIndex = -1;
			}
		}
		catch (Exception ex)
		{
			cbbImportant.DataSource = null;
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

	public void load_cbbUnit()
	{
		try
		{
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "TypeId=@0";
			queryArgs.PredicateParameters = new string[1] { "7CA6130A-00D1-40CE-ED0F-08D7E9C5C77D" };
			queryArgs.Order = "Created DESC";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs body = queryArgs;
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/MetadataValue/Gets").Result;
			DataTable dataTable = Common.getDataTable(result);
			if (dataTable != null)
			{
				dataTable.Dispose();
				cbbUnit.ValueMember = "Id";
				cbbUnit.DisplayMember = "Name";
				cbbUnit.DataSource = dataTable;
			}
			else
			{
				cbbUnit.DataSource = null;
			}
			if (!cbbUnit.Enabled)
			{
				cbbUnit.SelectedIndex = -1;
			}
		}
		catch (Exception ex)
		{
			cbbUnit.DataSource = null;
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
			queryArgs.PredicateParameters = new string[1] { "Chart" };
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
			if (!cbbTemplate.Enabled)
			{
				cbbTemplate.SelectedIndex = -1;
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

	private void load_AutoComplete()
	{
		txtCode.AutoCompleteCustomSource = Common.getAutoComplete(mData, "Code");
		txtName.AutoCompleteCustomSource = Common.getAutoComplete(mData, "Name");
		txtValue.AutoCompleteCustomSource = Common.getAutoComplete(mData, "Value");
		txtUpper.AutoCompleteCustomSource = Common.getAutoComplete(mData, "Upper");
		txtLower.AutoCompleteCustomSource = Common.getAutoComplete(mData, "Lower");
		txtUCL.AutoCompleteCustomSource = Common.getAutoComplete(mData, "UCL");
		txtLCL.AutoCompleteCustomSource = Common.getAutoComplete(mData, "LCL");
		txtCoordinate.AutoCompleteCustomSource = Common.getAutoComplete(mData, "Coordinate");
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
			if (dataRow.ItemArray.Length == 0)
			{
				return;
			}
			cbbMacType.SelectedValue = dataRow["MachineTypeId"];
			txtName.Text = dataRow["Name"].ToString();
			if (dataRow["ImportantId"] != null)
			{
				cbbImportant.SelectedValue = dataRow["ImportantId"];
			}
			else
			{
				cbbImportant.SelectedIndex = -1;
			}
			txtValue.Text = dataRow["Value"].ToString();
			if (dataRow["UnitId"] != null)
			{
				cbbUnit.SelectedValue = dataRow["UnitId"];
			}
			else
			{
				cbbUnit.SelectedIndex = -1;
			}
			txtUpper.Text = dataRow["Upper"].ToString();
			txtLower.Text = dataRow["Lower"].ToString();
			txtUCL.Text = dataRow["UCL"].ToString();
			txtLCL.Text = dataRow["LCL"].ToString();
			txtFormula.Text = dataRow["Formula"].ToString();
			if (dataRow["TemplateId"] != null)
			{
				cbbTemplate.SelectedValue = dataRow["TemplateId"];
			}
			else
			{
				cbbTemplate.SelectedIndex = -1;
			}
			txtCoordinate.Text = dataRow["Coordinate"].ToString();
			if (!isAdd)
			{
				txtCode.Text = dataRow["Code"].ToString();
				lblCreated.Text = dataRow["Created"].ToString();
				lblModified.Text = dataRow["Modified"].ToString();
				lblCreatedBy.Text = dataRow["CreatedBy"].ToString();
				lblModifiedBy.Text = dataRow["ModifiedBy"].ToString();
			}
			if (!string.IsNullOrEmpty(cbbUnit.Text))
			{
				cbbImportant.Enabled = true;
				cbbUnit.Enabled = true;
				txtUpper.Enabled = true;
				txtLower.Enabled = true;
				txtUCL.Enabled = true;
				txtLCL.Enabled = true;
				txtFormula.Enabled = true;
				if (!string.IsNullOrEmpty(cbbImportant.Text))
				{
					cbbTemplate.Enabled = true;
				}
			}
		}
		catch
		{
		}
	}

	private void check_Value(bool enable = false)
	{
		try
		{
			cbbUnit.Enabled = enable;
			txtUpper.Enabled = enable;
			txtLower.Enabled = enable;
			txtUCL.Enabled = enable;
			txtLCL.Enabled = enable;
			cbbImportant.Enabled = enable;
			txtFormula.Enabled = enable;
			cbbTemplate.Enabled = enable;
			if (enable)
			{
				if (isFomulaOpen)
				{
					txtFormula.Enabled = false;
				}
				else
				{
					txtFormula.Enabled = true;
				}
				if (cbbUnit.SelectedIndex.Equals(-1))
				{
					cbbUnit.SelectedIndex = 0;
				}
				if (string.IsNullOrEmpty(txtUpper.Text))
				{
					txtUpper.Text = "0";
				}
				if (string.IsNullOrEmpty(txtLower.Text))
				{
					txtLower.Text = "0";
				}
				if (string.IsNullOrEmpty(cbbImportant.Text))
				{
					cbbTemplate.SelectedIndex = -1;
					cbbTemplate.Enabled = false;
				}
				else
				{
					cbbTemplate.Enabled = true;
				}
			}
			else
			{
				txtUpper.Text = string.Empty;
				txtLower.Text = string.Empty;
				txtUCL.Text = string.Empty;
				txtLCL.Text = string.Empty;
				txtFormula.Text = string.Empty;
				cbbUnit.SelectedIndex = -1;
				cbbTemplate.SelectedIndex = -1;
				cbbImportant.SelectedIndex = -1;
				List<Type> list = new List<Type>();
				list.Add(typeof(frmFormula));
				Common.closeForm(list);
			}
		}
		catch
		{
		}
	}

	private string set_Code()
	{
		string text = "MEAS-";
		try
		{
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "ProductId=@0";
			queryArgs.PredicateParameters = new string[1] { mIdParent.ToString() };
			queryArgs.Order = "Sort DESC, Created DESC";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs body = queryArgs;
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/Measurement/Gets").Result;
			DataTable dataTable = Common.getDataTable<MeasurementQuickViewModel>(result);
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
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Expected O, but got Unknown
		//IL_019c: Expected O, but got Unknown
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
			if (string.IsNullOrEmpty(txtValue.Text))
			{
				MessageBox.Show(Common.getTextLanguage(this, "wValue"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				txtValue.Focus();
				return;
			}
			MeasurementViewModel val = new MeasurementViewModel
			{
				MachineTypeId = Guid.Parse(cbbMacType.SelectedValue.ToString()),
				Code = txtCode.Text,
				Name = txtName.Text,
				Value = txtValue.Text,
				ProductId = mIdParent
			};
			((AuditableEntity)val).IsActivated = true;
			MeasurementViewModel val2 = val;
			double result;
			if (cbbUnit.Text == "°")
			{
				double? num = Common.ConvertDegreesToDouble(txtValue.Text);
				val2.Value = ((!num.HasValue) ? "0" : $"{num}");
				val2.Upper = Common.ConvertDegreesToDouble(txtUpper.Text) ?? new double?(0.0);
				val2.Lower = Common.ConvertDegreesToDouble(txtLower.Text) ?? new double?(0.0);
				num = Common.ConvertDegreesToDouble(txtUCL.Text);
				val2.UCL = num;
				num = Common.ConvertDegreesToDouble(txtLCL.Text);
				val2.LCL = num;
			}
			else if (double.TryParse(txtValue.Text, out result))
			{
				double.TryParse(txtUpper.Text, out var result2);
				val2.Upper = result2;
				double.TryParse(txtLower.Text, out result2);
				val2.Lower = result2;
				if (double.TryParse(txtUCL.Text, out result2))
				{
					val2.UCL = result2;
				}
				if (double.TryParse(txtLCL.Text, out result2))
				{
					val2.LCL = result2;
				}
			}
			if (cbbImportant.Enabled && !string.IsNullOrEmpty(cbbImportant.Text))
			{
				val2.ImportantId = Guid.Parse(cbbImportant.SelectedValue.ToString());
			}
			if (cbbUnit.Enabled && !string.IsNullOrEmpty(cbbUnit.Text))
			{
				val2.UnitId = Guid.Parse(cbbUnit.SelectedValue.ToString());
			}
			if (cbbUnit.Enabled && !string.IsNullOrEmpty(txtFormula.Text))
			{
				val2.Formula = txtFormula.Text;
			}
			if (cbbUnit.Enabled && !string.IsNullOrEmpty(cbbTemplate.Text))
			{
				val2.TemplateId = Guid.Parse(cbbTemplate.SelectedValue.ToString());
			}
			if (!string.IsNullOrEmpty(txtCoordinate.Text))
			{
				val2.Coordinate = txtCoordinate.Text;
			}
			if (isAdd || !MessageBox.Show(Common.getTextLanguage(this, "wSureEdit"), Common.getTextLanguage(this, "QUESTION"), MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.No))
			{
				Cursor.Current = Cursors.WaitCursor;
				if (!isAdd)
				{
					((AuditableEntity)(object)val2).Id = mId;
				}
				ResponseDto result3 = frmLogin.client.SaveAsync(val2, "/api/Measurement/Save").Result;
				if (!result3.Success)
				{
					throw new Exception(result3.Messages.ElementAt(0).Message);
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
		else
		{
			if (!(comboBox.Name == cbbUnit.Name))
			{
				return;
			}
			if (comboBox.Text == "°")
			{
				if (double.TryParse(txtValue.Text, out var result))
				{
					txtValue.Text = Common.ConvertDoubleToDegrees(result);
				}
				if (double.TryParse(txtUpper.Text, out result))
				{
					txtUpper.Text = Common.ConvertDoubleToDegrees(result);
				}
				if (double.TryParse(txtLower.Text, out result))
				{
					txtLower.Text = Common.ConvertDoubleToDegrees(result);
				}
				if (double.TryParse(txtUCL.Text, out result))
				{
					txtUCL.Text = Common.ConvertDoubleToDegrees(result);
				}
				if (double.TryParse(txtLCL.Text, out result))
				{
					txtLCL.Text = Common.ConvertDoubleToDegrees(result);
				}
				return;
			}
			double? num = Common.ConvertDegreesToDouble(txtValue.Text);
			if (num.HasValue)
			{
				txtValue.Text = num.ToString();
			}
			num = Common.ConvertDegreesToDouble(txtUpper.Text);
			if (num.HasValue)
			{
				txtUpper.Text = num.ToString();
			}
			num = Common.ConvertDegreesToDouble(txtLower.Text);
			if (num.HasValue)
			{
				txtLower.Text = num.ToString();
			}
			num = Common.ConvertDegreesToDouble(txtUCL.Text);
			if (num.HasValue)
			{
				txtUCL.Text = num.ToString();
			}
			num = Common.ConvertDegreesToDouble(txtLCL.Text);
			if (num.HasValue)
			{
				txtLCL.Text = num.ToString();
			}
		}
	}

	private void cbbNormal_Enter(object sender, EventArgs e)
	{
		tpanelDegrees.Visible = false;
		ComboBox comboBox = sender as ComboBox;
		mRemember = comboBox.Text;
	}

	private void txtValue_Leave(object sender, EventArgs e)
	{
		txtValue.Leave -= txtValue_Leave;
		TextBox textBox = sender as TextBox;
		double result;
		try
		{
			if (string.IsNullOrEmpty(textBox.Text))
			{
				throw new Exception();
			}
			if (cbbUnit.Text == "°")
			{
				if (!Common.ConvertDegreesToDouble(textBox.Text).HasValue)
				{
					throw new Exception();
				}
			}
			else
			{
				result = double.Parse(textBox.Text);
				textBox.Text = result.ToString("#,##0.####");
			}
			check_Value(enable: true);
		}
		catch
		{
			textBox.Text = Common.trimSpace(textBox.Text);
			check_Value();
			if (double.TryParse(textBox.Text, out result))
			{
				textBox.Text = "";
			}
		}
		txtValue.Leave += txtValue_Leave;
	}

	private void txtDouble_KeyPress(object sender, KeyPressEventArgs e)
	{
		if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b' && e.KeyChar != '.' && e.KeyChar != '-')
		{
			e.Handled = true;
		}
	}

	private void txtDouble_Leave(object sender, EventArgs e)
	{
		TextBox textBox = sender as TextBox;
		try
		{
			if (string.IsNullOrEmpty(textBox.Text))
			{
				throw new Exception();
			}
			if (cbbUnit.Text == "°")
			{
				if (!Common.ConvertDegreesToDouble(textBox.Text).HasValue)
				{
					throw new Exception();
				}
			}
			else
			{
				textBox.Text = double.Parse(textBox.Text).ToString("#,##0.####");
			}
		}
		catch
		{
			textBox.Text = ((cbbUnit.Text == "°") ? "0°" : "0");
		}
	}

	private void txtDoubleIsNull_Leave(object sender, EventArgs e)
	{
		TextBox textBox = sender as TextBox;
		try
		{
			if (string.IsNullOrEmpty(textBox.Text))
			{
				throw new Exception();
			}
			if (cbbUnit.Text == "°")
			{
				if (!Common.ConvertDegreesToDouble(textBox.Text).HasValue)
				{
					throw new Exception();
				}
			}
			else
			{
				textBox.Text = double.Parse(textBox.Text).ToString("#,##0.####");
			}
		}
		catch
		{
			textBox.Text = string.Empty;
		}
	}

	private void txtNormal_Leave(object sender, EventArgs e)
	{
		TextBox textBox = sender as TextBox;
		textBox.Text = Common.trimSpace(textBox.Text);
	}

	private void txtCode_KeyPress(object sender, KeyPressEventArgs e)
	{
		if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b')
		{
			e.Handled = true;
		}
		if (txtCode.Text.EndsWith("MEAS-") && e.KeyChar == '\b')
		{
			e.Handled = true;
		}
		e.KeyChar = char.ToUpper(e.KeyChar);
	}

	private void txtCode_Leave(object sender, EventArgs e)
	{
		if (!txtCode.Text.Contains("MEAS-") || txtCode.Text.Equals("MEAS-"))
		{
			txtCode.Text = rememberCode;
		}
	}

	private void cbCodeEnable_CheckedChanged(object sender, EventArgs e)
	{
		txtCode.Enabled = cbCodeEnable.Checked;
	}

	private void txtFormula_Leave(object sender, EventArgs e)
	{
		TextBox textBox = sender as TextBox;
		textBox.Text = Common.trimSpace(textBox.Text);
	}

	private void txtFormula_Enter(object sender, EventArgs e)
	{
		tpanelDegrees.Visible = false;
		Cursor.Current = Cursors.WaitCursor;
		if (!frmLogin.mFuntion.Contains("QC2107"))
		{
			txtFormula.Enabled = false;
			return;
		}
		List<Type> list = new List<Type>();
		list.Add(typeof(frmFormula));
		Common.closeForm(list);
		new frmFormula(this, mIdParent).Show();
	}

	private void btnMacTypeAdd_Click(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		List<Type> list = new List<Type>();
		list.Add(typeof(frmSubView));
		Common.closeForm(list);
		new frmSubView(this, (FormType)6).Show();
	}

	private void btnImportantAdd_Click(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		List<Type> list = new List<Type>();
		list.Add(typeof(frmSubView));
		Common.closeForm(list);
		new frmSubView(this, (FormType)7).Show();
	}

	private void btnUnitAdd_Click(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		List<Type> list = new List<Type>();
		list.Add(typeof(frmSubView));
		Common.closeForm(list);
		new frmSubView(this, (FormType)8).Show();
	}

	private void btnTemplateAdd_Click(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		List<Type> list = new List<Type>();
		list.Add(typeof(frmTemplateView));
		Common.closeForm(list);
		new frmTemplateView(this).Show();
	}

	private void cbbImportant_Leave(object sender, EventArgs e)
	{
		ComboBox comboBox = sender as ComboBox;
		if (comboBox.SelectedIndex.Equals(-1) && !string.IsNullOrEmpty(comboBox.Text))
		{
			comboBox.Text = mRemember;
		}
		if (string.IsNullOrEmpty(cbbImportant.Text))
		{
			cbbTemplate.SelectedIndex = -1;
			cbbTemplate.Enabled = false;
		}
		else
		{
			cbbTemplate.Enabled = true;
		}
	}

	private void txtValue_Enter(object sender, EventArgs e)
	{
		tpanelDegrees.Visible = cbbUnit.Text == "°";
		if (tpanelDegrees.Visible)
		{
			mTxt = sender as TextBox;
			List<string> list = Common.ConvertDegreesToList(mTxt.Text);
			txtSign.Text = list.First();
			txtDegrees.Text = list[1];
			txtMinutes.Text = list[2];
			txtSeconds.Text = list.Last();
		}
	}

	private void txtNormal_Enter(object sender, EventArgs e)
	{
		tpanelDegrees.Visible = false;
	}

	private void txtNormal_KeyPress(object sender, KeyPressEventArgs e)
	{
		if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b')
		{
			e.Handled = true;
		}
	}

	private void txtSign_KeyPress(object sender, KeyPressEventArgs e)
	{
		if (e.KeyChar != '\b' && e.KeyChar != '+' && e.KeyChar != '-')
		{
			e.Handled = true;
		}
	}

	private void btnOK_Click(object sender, EventArgs e)
	{
		if (mTxt != null)
		{
			string text = ((txtSign.Text == "-") ? "-" : "") + (string.IsNullOrEmpty(txtDegrees.Text) ? "0" : int.Parse(txtDegrees.Text).ToString()) + "°" + ((string.IsNullOrEmpty(txtMinutes.Text) || int.Parse(txtMinutes.Text) == 0) ? "" : string.Format("{0}{1}", int.Parse(txtMinutes.Text), "'")) + ((string.IsNullOrEmpty(txtSeconds.Text) || int.Parse(txtSeconds.Text) == 0) ? "" : string.Format("{0}{1}", int.Parse(txtSeconds.Text), "\""));
			mTxt.Text = text;
			tpanelDegrees.Visible = false;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_5S_QA_System.frmMeasurementAdd));
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.btnConfirm = new System.Windows.Forms.Button();
		this.cbbMacType = new System.Windows.Forms.ComboBox();
		this.btnMacTypeAdd = new System.Windows.Forms.Button();
		this.txtCode = new System.Windows.Forms.TextBox();
		this.cbCodeEnable = new System.Windows.Forms.CheckBox();
		this.txtName = new System.Windows.Forms.TextBox();
		this.txtUpper = new System.Windows.Forms.TextBox();
		this.txtLower = new System.Windows.Forms.TextBox();
		this.txtValue = new System.Windows.Forms.TextBox();
		this.txtUCL = new System.Windows.Forms.TextBox();
		this.txtLCL = new System.Windows.Forms.TextBox();
		this.txtFormula = new System.Windows.Forms.TextBox();
		this.txtCoordinate = new System.Windows.Forms.TextBox();
		this.cbbImportant = new System.Windows.Forms.ComboBox();
		this.btnImportantAdd = new System.Windows.Forms.Button();
		this.cbbUnit = new System.Windows.Forms.ComboBox();
		this.btnUnitAdd = new System.Windows.Forms.Button();
		this.cbbTemplate = new System.Windows.Forms.ComboBox();
		this.btnTemplateAdd = new System.Windows.Forms.Button();
		this.btnOK = new System.Windows.Forms.Button();
		this.txtSeconds = new System.Windows.Forms.TextBox();
		this.txtMinutes = new System.Windows.Forms.TextBox();
		this.txtDegrees = new System.Windows.Forms.TextBox();
		this.txtSign = new System.Windows.Forms.TextBox();
		this.panelMain = new System.Windows.Forms.Panel();
		this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
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
		this.lblCoordinate = new System.Windows.Forms.Label();
		this.lblTemplate = new System.Windows.Forms.Label();
		this.lblFormula = new System.Windows.Forms.Label();
		this.lblUCL = new System.Windows.Forms.Label();
		this.lblLCL = new System.Windows.Forms.Label();
		this.lblLower = new System.Windows.Forms.Label();
		this.lblUpper = new System.Windows.Forms.Label();
		this.lblUnit = new System.Windows.Forms.Label();
		this.lblValue = new System.Windows.Forms.Label();
		this.lblImportant = new System.Windows.Forms.Label();
		this.lblName = new System.Windows.Forms.Label();
		this.lblCode = new System.Windows.Forms.Label();
		this.lblMacType = new System.Windows.Forms.Label();
		this.tpanelSubDegrees = new System.Windows.Forms.TableLayoutPanel();
		this.lblSeconds = new System.Windows.Forms.Label();
		this.lblMinutes = new System.Windows.Forms.Label();
		this.lblDegrees = new System.Windows.Forms.Label();
		this.tpanelDegrees = new System.Windows.Forms.TableLayoutPanel();
		this.panelMain.SuspendLayout();
		this.tableLayoutPanelMain.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel1.SuspendLayout();
		this.tpanelSubDegrees.SuspendLayout();
		this.tpanelDegrees.SuspendLayout();
		base.SuspendLayout();
		this.btnConfirm.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnConfirm.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.btnConfirm.FlatAppearance.BorderSize = 0;
		this.btnConfirm.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
		this.btnConfirm.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
		this.btnConfirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnConfirm.Location = new System.Drawing.Point(20, 573);
		this.btnConfirm.Name = "btnConfirm";
		this.btnConfirm.Size = new System.Drawing.Size(500, 28);
		this.btnConfirm.TabIndex = 1;
		this.btnConfirm.Text = "Confirm";
		this.toolTipMain.SetToolTip(this.btnConfirm, "Confirm");
		this.btnConfirm.UseVisualStyleBackColor = true;
		this.btnConfirm.Click += new System.EventHandler(btnComfirm_Click);
		this.cbbMacType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cbbMacType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cbbMacType.Dock = System.Windows.Forms.DockStyle.Fill;
		this.cbbMacType.FormattingEnabled = true;
		this.cbbMacType.ItemHeight = 16;
		this.cbbMacType.Location = new System.Drawing.Point(0, 0);
		this.cbbMacType.MaxLength = 50;
		this.cbbMacType.Name = "cbbMacType";
		this.cbbMacType.Size = new System.Drawing.Size(366, 24);
		this.cbbMacType.TabIndex = 1;
		this.toolTipMain.SetToolTip(this.cbbMacType, "Please select or enter machine type");
		this.cbbMacType.Enter += new System.EventHandler(cbbNormal_Enter);
		this.cbbMacType.Leave += new System.EventHandler(cbbNormalNotNull_Leave);
		this.btnMacTypeAdd.BackColor = System.Drawing.Color.Gainsboro;
		this.btnMacTypeAdd.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnMacTypeAdd.Dock = System.Windows.Forms.DockStyle.Right;
		this.btnMacTypeAdd.FlatAppearance.BorderSize = 0;
		this.btnMacTypeAdd.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
		this.btnMacTypeAdd.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SkyBlue;
		this.btnMacTypeAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnMacTypeAdd.Location = new System.Drawing.Point(366, 0);
		this.btnMacTypeAdd.Name = "btnMacTypeAdd";
		this.btnMacTypeAdd.Size = new System.Drawing.Size(24, 24);
		this.btnMacTypeAdd.TabIndex = 2;
		this.btnMacTypeAdd.Text = "+";
		this.toolTipMain.SetToolTip(this.btnMacTypeAdd, "Goto manage machine type");
		this.btnMacTypeAdd.UseVisualStyleBackColor = false;
		this.btnMacTypeAdd.Click += new System.EventHandler(btnMacTypeAdd_Click);
		this.txtCode.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtCode.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtCode.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtCode.Enabled = false;
		this.txtCode.Location = new System.Drawing.Point(0, 0);
		this.txtCode.Name = "txtCode";
		this.txtCode.Size = new System.Drawing.Size(390, 22);
		this.txtCode.TabIndex = 1;
		this.toolTipMain.SetToolTip(this.txtCode, "Please enter code measurement");
		this.txtCode.Enter += new System.EventHandler(txtNormal_Enter);
		this.txtCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtCode_KeyPress);
		this.txtCode.Leave += new System.EventHandler(txtCode_Leave);
		this.cbCodeEnable.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.cbCodeEnable.AutoSize = true;
		this.cbCodeEnable.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbCodeEnable.Location = new System.Drawing.Point(372, 4);
		this.cbCodeEnable.Name = "cbCodeEnable";
		this.cbCodeEnable.Size = new System.Drawing.Size(15, 14);
		this.cbCodeEnable.TabIndex = 2;
		this.toolTipMain.SetToolTip(this.cbCodeEnable, "Select enable enter code");
		this.cbCodeEnable.UseVisualStyleBackColor = true;
		this.cbCodeEnable.CheckedChanged += new System.EventHandler(cbCodeEnable_CheckedChanged);
		this.txtName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtName.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtName.Location = new System.Drawing.Point(98, 62);
		this.txtName.Name = "txtName";
		this.txtName.Size = new System.Drawing.Size(390, 22);
		this.txtName.TabIndex = 3;
		this.toolTipMain.SetToolTip(this.txtName, "Please enter name measurement");
		this.txtName.Enter += new System.EventHandler(txtNormal_Enter);
		this.txtName.Leave += new System.EventHandler(txtNormal_Leave);
		this.txtUpper.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtUpper.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtUpper.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtUpper.Enabled = false;
		this.txtUpper.Location = new System.Drawing.Point(98, 178);
		this.txtUpper.Name = "txtUpper";
		this.txtUpper.Size = new System.Drawing.Size(390, 22);
		this.txtUpper.TabIndex = 7;
		this.toolTipMain.SetToolTip(this.txtUpper, "Please enter value upper measurement");
		this.txtUpper.Enter += new System.EventHandler(txtValue_Enter);
		this.txtUpper.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDouble_KeyPress);
		this.txtUpper.Leave += new System.EventHandler(txtDouble_Leave);
		this.txtLower.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtLower.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtLower.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtLower.Enabled = false;
		this.txtLower.Location = new System.Drawing.Point(98, 207);
		this.txtLower.Name = "txtLower";
		this.txtLower.Size = new System.Drawing.Size(390, 22);
		this.txtLower.TabIndex = 8;
		this.toolTipMain.SetToolTip(this.txtLower, "Please enter value lower measurement");
		this.txtLower.Enter += new System.EventHandler(txtValue_Enter);
		this.txtLower.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDouble_KeyPress);
		this.txtLower.Leave += new System.EventHandler(txtDouble_Leave);
		this.txtValue.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtValue.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtValue.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtValue.Location = new System.Drawing.Point(98, 120);
		this.txtValue.Name = "txtValue";
		this.txtValue.Size = new System.Drawing.Size(390, 22);
		this.txtValue.TabIndex = 5;
		this.toolTipMain.SetToolTip(this.txtValue, "Please enter value measurement");
		this.txtValue.Enter += new System.EventHandler(txtValue_Enter);
		this.txtValue.Leave += new System.EventHandler(txtValue_Leave);
		this.txtUCL.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtUCL.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtUCL.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtUCL.Enabled = false;
		this.txtUCL.Location = new System.Drawing.Point(98, 236);
		this.txtUCL.Name = "txtUCL";
		this.txtUCL.Size = new System.Drawing.Size(390, 22);
		this.txtUCL.TabIndex = 9;
		this.toolTipMain.SetToolTip(this.txtUCL, "Please enter value ucl measurement");
		this.txtUCL.Enter += new System.EventHandler(txtValue_Enter);
		this.txtUCL.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDouble_KeyPress);
		this.txtUCL.Leave += new System.EventHandler(txtDoubleIsNull_Leave);
		this.txtLCL.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtLCL.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtLCL.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtLCL.Enabled = false;
		this.txtLCL.Location = new System.Drawing.Point(98, 265);
		this.txtLCL.Name = "txtLCL";
		this.txtLCL.Size = new System.Drawing.Size(390, 22);
		this.txtLCL.TabIndex = 10;
		this.toolTipMain.SetToolTip(this.txtLCL, "Please enter value lcl measurement");
		this.txtLCL.Enter += new System.EventHandler(txtValue_Enter);
		this.txtLCL.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDouble_KeyPress);
		this.txtLCL.Leave += new System.EventHandler(txtDoubleIsNull_Leave);
		this.txtFormula.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtFormula.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtFormula.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtFormula.Enabled = false;
		this.txtFormula.Location = new System.Drawing.Point(98, 294);
		this.txtFormula.Name = "txtFormula";
		this.txtFormula.Size = new System.Drawing.Size(390, 22);
		this.txtFormula.TabIndex = 11;
		this.toolTipMain.SetToolTip(this.txtFormula, "Please enter formula format: [MEASxx], +, -, *, /, %, ()...");
		this.txtFormula.Enter += new System.EventHandler(txtFormula_Enter);
		this.txtFormula.Leave += new System.EventHandler(txtFormula_Leave);
		this.txtCoordinate.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtCoordinate.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtCoordinate.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtCoordinate.Enabled = false;
		this.txtCoordinate.Location = new System.Drawing.Point(98, 352);
		this.txtCoordinate.Name = "txtCoordinate";
		this.txtCoordinate.Size = new System.Drawing.Size(390, 22);
		this.txtCoordinate.TabIndex = 13;
		this.toolTipMain.SetToolTip(this.txtCoordinate, "Please enter coordinate");
		this.cbbImportant.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cbbImportant.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cbbImportant.Dock = System.Windows.Forms.DockStyle.Fill;
		this.cbbImportant.Enabled = false;
		this.cbbImportant.FormattingEnabled = true;
		this.cbbImportant.ItemHeight = 16;
		this.cbbImportant.Location = new System.Drawing.Point(0, 0);
		this.cbbImportant.MaxLength = 50;
		this.cbbImportant.Name = "cbbImportant";
		this.cbbImportant.Size = new System.Drawing.Size(366, 24);
		this.cbbImportant.TabIndex = 1;
		this.toolTipMain.SetToolTip(this.cbbImportant, "Please select or enter important");
		this.cbbImportant.Enter += new System.EventHandler(cbbNormal_Enter);
		this.cbbImportant.Leave += new System.EventHandler(cbbImportant_Leave);
		this.btnImportantAdd.BackColor = System.Drawing.Color.Gainsboro;
		this.btnImportantAdd.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnImportantAdd.Dock = System.Windows.Forms.DockStyle.Right;
		this.btnImportantAdd.FlatAppearance.BorderSize = 0;
		this.btnImportantAdd.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
		this.btnImportantAdd.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SkyBlue;
		this.btnImportantAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnImportantAdd.Location = new System.Drawing.Point(366, 0);
		this.btnImportantAdd.Name = "btnImportantAdd";
		this.btnImportantAdd.Size = new System.Drawing.Size(24, 24);
		this.btnImportantAdd.TabIndex = 2;
		this.btnImportantAdd.Text = "+";
		this.toolTipMain.SetToolTip(this.btnImportantAdd, "Goto manage important");
		this.btnImportantAdd.UseVisualStyleBackColor = false;
		this.btnImportantAdd.Click += new System.EventHandler(btnImportantAdd_Click);
		this.cbbUnit.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cbbUnit.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cbbUnit.Dock = System.Windows.Forms.DockStyle.Fill;
		this.cbbUnit.Enabled = false;
		this.cbbUnit.FormattingEnabled = true;
		this.cbbUnit.ItemHeight = 16;
		this.cbbUnit.Location = new System.Drawing.Point(0, 0);
		this.cbbUnit.MaxLength = 50;
		this.cbbUnit.Name = "cbbUnit";
		this.cbbUnit.Size = new System.Drawing.Size(366, 24);
		this.cbbUnit.TabIndex = 1;
		this.toolTipMain.SetToolTip(this.cbbUnit, "Select or enter value unit measurement");
		this.cbbUnit.Enter += new System.EventHandler(cbbNormal_Enter);
		this.cbbUnit.Leave += new System.EventHandler(cbbNormalNotNull_Leave);
		this.btnUnitAdd.BackColor = System.Drawing.Color.Gainsboro;
		this.btnUnitAdd.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnUnitAdd.Dock = System.Windows.Forms.DockStyle.Right;
		this.btnUnitAdd.FlatAppearance.BorderSize = 0;
		this.btnUnitAdd.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
		this.btnUnitAdd.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SkyBlue;
		this.btnUnitAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUnitAdd.Location = new System.Drawing.Point(366, 0);
		this.btnUnitAdd.Name = "btnUnitAdd";
		this.btnUnitAdd.Size = new System.Drawing.Size(24, 24);
		this.btnUnitAdd.TabIndex = 2;
		this.btnUnitAdd.Text = "+";
		this.toolTipMain.SetToolTip(this.btnUnitAdd, "Goto manage unit");
		this.btnUnitAdd.UseVisualStyleBackColor = false;
		this.btnUnitAdd.Click += new System.EventHandler(btnUnitAdd_Click);
		this.cbbTemplate.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cbbTemplate.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cbbTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
		this.cbbTemplate.Enabled = false;
		this.cbbTemplate.FormattingEnabled = true;
		this.cbbTemplate.ItemHeight = 16;
		this.cbbTemplate.Location = new System.Drawing.Point(0, 0);
		this.cbbTemplate.MaxLength = 50;
		this.cbbTemplate.Name = "cbbTemplate";
		this.cbbTemplate.Size = new System.Drawing.Size(366, 24);
		this.cbbTemplate.TabIndex = 1;
		this.toolTipMain.SetToolTip(this.cbbTemplate, "Select or enter template");
		this.cbbTemplate.Enter += new System.EventHandler(cbbNormal_Enter);
		this.cbbTemplate.Leave += new System.EventHandler(cbbNormal_Leave);
		this.btnTemplateAdd.BackColor = System.Drawing.Color.Gainsboro;
		this.btnTemplateAdd.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnTemplateAdd.Dock = System.Windows.Forms.DockStyle.Right;
		this.btnTemplateAdd.FlatAppearance.BorderSize = 0;
		this.btnTemplateAdd.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
		this.btnTemplateAdd.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SkyBlue;
		this.btnTemplateAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnTemplateAdd.Location = new System.Drawing.Point(366, 0);
		this.btnTemplateAdd.Name = "btnTemplateAdd";
		this.btnTemplateAdd.Size = new System.Drawing.Size(24, 24);
		this.btnTemplateAdd.TabIndex = 2;
		this.btnTemplateAdd.Text = "+";
		this.toolTipMain.SetToolTip(this.btnTemplateAdd, "Goto manager template");
		this.btnTemplateAdd.UseVisualStyleBackColor = false;
		this.btnTemplateAdd.Click += new System.EventHandler(btnTemplateAdd_Click);
		this.btnOK.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnOK.FlatAppearance.BorderSize = 0;
		this.btnOK.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
		this.btnOK.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
		this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnOK.Location = new System.Drawing.Point(235, 1);
		this.btnOK.Margin = new System.Windows.Forms.Padding(0);
		this.btnOK.Name = "btnOK";
		this.btnOK.Size = new System.Drawing.Size(40, 28);
		this.btnOK.TabIndex = 2;
		this.btnOK.Text = "OK";
		this.toolTipMain.SetToolTip(this.btnOK, "Confirm OK");
		this.btnOK.UseVisualStyleBackColor = true;
		this.btnOK.Click += new System.EventHandler(btnOK_Click);
		this.txtSeconds.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtSeconds.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtSeconds.Location = new System.Drawing.Point(168, 3);
		this.txtSeconds.MaxLength = 5;
		this.txtSeconds.Name = "txtSeconds";
		this.txtSeconds.Size = new System.Drawing.Size(50, 22);
		this.txtSeconds.TabIndex = 4;
		this.txtSeconds.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.toolTipMain.SetToolTip(this.txtSeconds, "Please enter seconds");
		this.txtSeconds.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtNormal_KeyPress);
		this.txtMinutes.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtMinutes.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtMinutes.Location = new System.Drawing.Point(102, 3);
		this.txtMinutes.MaxLength = 5;
		this.txtMinutes.Name = "txtMinutes";
		this.txtMinutes.Size = new System.Drawing.Size(50, 22);
		this.txtMinutes.TabIndex = 3;
		this.txtMinutes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.toolTipMain.SetToolTip(this.txtMinutes, "Please enter minutes");
		this.txtMinutes.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtNormal_KeyPress);
		this.txtDegrees.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtDegrees.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtDegrees.Location = new System.Drawing.Point(35, 3);
		this.txtDegrees.MaxLength = 5;
		this.txtDegrees.Name = "txtDegrees";
		this.txtDegrees.Size = new System.Drawing.Size(50, 22);
		this.txtDegrees.TabIndex = 2;
		this.txtDegrees.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.toolTipMain.SetToolTip(this.txtDegrees, "Please enter degrees");
		this.txtDegrees.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtNormal_KeyPress);
		this.txtSign.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtSign.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtSign.Location = new System.Drawing.Point(3, 3);
		this.txtSign.MaxLength = 1;
		this.txtSign.Name = "txtSign";
		this.txtSign.Size = new System.Drawing.Size(26, 22);
		this.txtSign.TabIndex = 1;
		this.txtSign.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.toolTipMain.SetToolTip(this.txtSign, "Please enter sign");
		this.txtSign.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtSign_KeyPress);
		this.panelMain.AutoScroll = true;
		this.panelMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panelMain.Controls.Add(this.tableLayoutPanelMain);
		this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panelMain.Location = new System.Drawing.Point(20, 70);
		this.panelMain.Name = "panelMain";
		this.panelMain.Padding = new System.Windows.Forms.Padding(3);
		this.panelMain.Size = new System.Drawing.Size(500, 503);
		this.panelMain.TabIndex = 1;
		this.tableLayoutPanelMain.AutoSize = true;
		this.tableLayoutPanelMain.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		this.tableLayoutPanelMain.ColumnCount = 2;
		this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanelMain.Controls.Add(this.panel5, 1, 11);
		this.tableLayoutPanelMain.Controls.Add(this.panel4, 1, 5);
		this.tableLayoutPanelMain.Controls.Add(this.panel3, 1, 3);
		this.tableLayoutPanelMain.Controls.Add(this.txtCoordinate, 1, 12);
		this.tableLayoutPanelMain.Controls.Add(this.txtValue, 1, 4);
		this.tableLayoutPanelMain.Controls.Add(this.txtFormula, 1, 10);
		this.tableLayoutPanelMain.Controls.Add(this.txtLCL, 1, 9);
		this.tableLayoutPanelMain.Controls.Add(this.txtUpper, 1, 6);
		this.tableLayoutPanelMain.Controls.Add(this.txtUCL, 1, 8);
		this.tableLayoutPanelMain.Controls.Add(this.panel2, 1, 1);
		this.tableLayoutPanelMain.Controls.Add(this.txtLower, 1, 7);
		this.tableLayoutPanelMain.Controls.Add(this.panel1, 1, 0);
		this.tableLayoutPanelMain.Controls.Add(this.lblModifiedBy, 1, 17);
		this.tableLayoutPanelMain.Controls.Add(this.txtName, 1, 2);
		this.tableLayoutPanelMain.Controls.Add(this.lblCreatedBy, 1, 16);
		this.tableLayoutPanelMain.Controls.Add(this.lblModified, 1, 15);
		this.tableLayoutPanelMain.Controls.Add(this.lblCreated, 1, 14);
		this.tableLayoutPanelMain.Controls.Add(this.lblModifiBy, 0, 17);
		this.tableLayoutPanelMain.Controls.Add(this.lblCreateBy, 0, 16);
		this.tableLayoutPanelMain.Controls.Add(this.lblModifi, 0, 15);
		this.tableLayoutPanelMain.Controls.Add(this.lblCreate, 0, 14);
		this.tableLayoutPanelMain.Controls.Add(this.lblCoordinate, 0, 12);
		this.tableLayoutPanelMain.Controls.Add(this.lblTemplate, 0, 11);
		this.tableLayoutPanelMain.Controls.Add(this.lblFormula, 0, 10);
		this.tableLayoutPanelMain.Controls.Add(this.lblUCL, 0, 8);
		this.tableLayoutPanelMain.Controls.Add(this.lblLCL, 0, 9);
		this.tableLayoutPanelMain.Controls.Add(this.lblLower, 0, 7);
		this.tableLayoutPanelMain.Controls.Add(this.lblUpper, 0, 6);
		this.tableLayoutPanelMain.Controls.Add(this.lblUnit, 0, 5);
		this.tableLayoutPanelMain.Controls.Add(this.lblValue, 0, 4);
		this.tableLayoutPanelMain.Controls.Add(this.lblImportant, 0, 3);
		this.tableLayoutPanelMain.Controls.Add(this.lblName, 0, 2);
		this.tableLayoutPanelMain.Controls.Add(this.lblCode, 0, 1);
		this.tableLayoutPanelMain.Controls.Add(this.lblMacType, 0, 0);
		this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Top;
		this.tableLayoutPanelMain.Location = new System.Drawing.Point(3, 3);
		this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
		this.tableLayoutPanelMain.RowCount = 18;
		this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0f));
		this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanelMain.Size = new System.Drawing.Size(492, 495);
		this.tableLayoutPanelMain.TabIndex = 1;
		this.panel5.Controls.Add(this.cbbTemplate);
		this.panel5.Controls.Add(this.btnTemplateAdd);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(98, 322);
		this.panel5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(390, 24);
		this.panel5.TabIndex = 12;
		this.panel5.TabStop = true;
		this.panel4.Controls.Add(this.cbbUnit);
		this.panel4.Controls.Add(this.btnUnitAdd);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(98, 148);
		this.panel4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(390, 24);
		this.panel4.TabIndex = 6;
		this.panel4.TabStop = true;
		this.panel3.Controls.Add(this.cbbImportant);
		this.panel3.Controls.Add(this.btnImportantAdd);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(98, 90);
		this.panel3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(390, 24);
		this.panel3.TabIndex = 4;
		this.panel3.TabStop = true;
		this.panel2.Controls.Add(this.cbCodeEnable);
		this.panel2.Controls.Add(this.txtCode);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(98, 33);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(390, 22);
		this.panel2.TabIndex = 2;
		this.panel2.TabStop = true;
		this.panel1.Controls.Add(this.cbbMacType);
		this.panel1.Controls.Add(this.btnMacTypeAdd);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(98, 3);
		this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(390, 24);
		this.panel1.TabIndex = 1;
		this.panel1.TabStop = true;
		this.lblModifiedBy.AutoSize = true;
		this.lblModifiedBy.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblModifiedBy.Location = new System.Drawing.Point(98, 466);
		this.lblModifiedBy.Name = "lblModifiedBy";
		this.lblModifiedBy.Size = new System.Drawing.Size(390, 28);
		this.lblModifiedBy.TabIndex = 82;
		this.lblModifiedBy.Text = "...";
		this.lblModifiedBy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblCreatedBy.AutoSize = true;
		this.lblCreatedBy.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCreatedBy.Location = new System.Drawing.Point(98, 437);
		this.lblCreatedBy.Name = "lblCreatedBy";
		this.lblCreatedBy.Size = new System.Drawing.Size(390, 28);
		this.lblCreatedBy.TabIndex = 81;
		this.lblCreatedBy.Text = "...";
		this.lblCreatedBy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblModified.AutoSize = true;
		this.lblModified.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblModified.Location = new System.Drawing.Point(98, 408);
		this.lblModified.Name = "lblModified";
		this.lblModified.Size = new System.Drawing.Size(390, 28);
		this.lblModified.TabIndex = 80;
		this.lblModified.Text = "...";
		this.lblModified.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblCreated.AutoSize = true;
		this.lblCreated.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCreated.Location = new System.Drawing.Point(98, 379);
		this.lblCreated.Name = "lblCreated";
		this.lblCreated.Size = new System.Drawing.Size(390, 28);
		this.lblCreated.TabIndex = 79;
		this.lblCreated.Text = "...";
		this.lblCreated.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblModifiBy.AutoSize = true;
		this.lblModifiBy.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblModifiBy.Location = new System.Drawing.Point(4, 466);
		this.lblModifiBy.Name = "lblModifiBy";
		this.lblModifiBy.Size = new System.Drawing.Size(87, 28);
		this.lblModifiBy.TabIndex = 78;
		this.lblModifiBy.Text = "Edited by";
		this.lblModifiBy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblCreateBy.AutoSize = true;
		this.lblCreateBy.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCreateBy.Location = new System.Drawing.Point(4, 437);
		this.lblCreateBy.Name = "lblCreateBy";
		this.lblCreateBy.Size = new System.Drawing.Size(87, 28);
		this.lblCreateBy.TabIndex = 77;
		this.lblCreateBy.Text = "Created by";
		this.lblCreateBy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblModifi.AutoSize = true;
		this.lblModifi.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblModifi.Location = new System.Drawing.Point(4, 408);
		this.lblModifi.Name = "lblModifi";
		this.lblModifi.Size = new System.Drawing.Size(87, 28);
		this.lblModifi.TabIndex = 76;
		this.lblModifi.Text = "Edited date";
		this.lblModifi.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblCreate.AutoSize = true;
		this.lblCreate.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCreate.Location = new System.Drawing.Point(4, 379);
		this.lblCreate.Name = "lblCreate";
		this.lblCreate.Size = new System.Drawing.Size(87, 28);
		this.lblCreate.TabIndex = 75;
		this.lblCreate.Text = "Create date";
		this.lblCreate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblCoordinate.AutoSize = true;
		this.lblCoordinate.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCoordinate.Location = new System.Drawing.Point(4, 349);
		this.lblCoordinate.Name = "lblCoordinate";
		this.lblCoordinate.Size = new System.Drawing.Size(87, 28);
		this.lblCoordinate.TabIndex = 31;
		this.lblCoordinate.Text = "Coordinate";
		this.lblCoordinate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblTemplate.AutoSize = true;
		this.lblTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTemplate.Location = new System.Drawing.Point(4, 320);
		this.lblTemplate.Name = "lblTemplate";
		this.lblTemplate.Size = new System.Drawing.Size(87, 28);
		this.lblTemplate.TabIndex = 30;
		this.lblTemplate.Text = "Template";
		this.lblTemplate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblFormula.AutoSize = true;
		this.lblFormula.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblFormula.Location = new System.Drawing.Point(4, 291);
		this.lblFormula.Name = "lblFormula";
		this.lblFormula.Size = new System.Drawing.Size(87, 28);
		this.lblFormula.TabIndex = 29;
		this.lblFormula.Text = "Formula";
		this.lblFormula.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblUCL.AutoSize = true;
		this.lblUCL.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblUCL.Location = new System.Drawing.Point(4, 233);
		this.lblUCL.Name = "lblUCL";
		this.lblUCL.Size = new System.Drawing.Size(87, 28);
		this.lblUCL.TabIndex = 27;
		this.lblUCL.Text = "UCL";
		this.lblUCL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblLCL.AutoSize = true;
		this.lblLCL.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblLCL.Location = new System.Drawing.Point(4, 262);
		this.lblLCL.Name = "lblLCL";
		this.lblLCL.Size = new System.Drawing.Size(87, 28);
		this.lblLCL.TabIndex = 28;
		this.lblLCL.Text = "LCL";
		this.lblLCL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblLower.AutoSize = true;
		this.lblLower.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblLower.Location = new System.Drawing.Point(4, 204);
		this.lblLower.Name = "lblLower";
		this.lblLower.Size = new System.Drawing.Size(87, 28);
		this.lblLower.TabIndex = 26;
		this.lblLower.Text = "Lower";
		this.lblLower.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblUpper.AutoSize = true;
		this.lblUpper.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblUpper.Location = new System.Drawing.Point(4, 175);
		this.lblUpper.Name = "lblUpper";
		this.lblUpper.Size = new System.Drawing.Size(87, 28);
		this.lblUpper.TabIndex = 25;
		this.lblUpper.Text = "Upper";
		this.lblUpper.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblUnit.AutoSize = true;
		this.lblUnit.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblUnit.Location = new System.Drawing.Point(4, 146);
		this.lblUnit.Name = "lblUnit";
		this.lblUnit.Size = new System.Drawing.Size(87, 28);
		this.lblUnit.TabIndex = 24;
		this.lblUnit.Text = "Unit";
		this.lblUnit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblValue.AutoSize = true;
		this.lblValue.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblValue.Location = new System.Drawing.Point(4, 117);
		this.lblValue.Name = "lblValue";
		this.lblValue.Size = new System.Drawing.Size(87, 28);
		this.lblValue.TabIndex = 23;
		this.lblValue.Text = "Value";
		this.lblValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblImportant.AutoSize = true;
		this.lblImportant.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblImportant.Location = new System.Drawing.Point(4, 88);
		this.lblImportant.Name = "lblImportant";
		this.lblImportant.Size = new System.Drawing.Size(87, 28);
		this.lblImportant.TabIndex = 22;
		this.lblImportant.Text = "Important";
		this.lblImportant.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
		this.lblMacType.AutoSize = true;
		this.lblMacType.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblMacType.Location = new System.Drawing.Point(4, 1);
		this.lblMacType.Name = "lblMacType";
		this.lblMacType.Size = new System.Drawing.Size(87, 28);
		this.lblMacType.TabIndex = 19;
		this.lblMacType.Text = "Machine type";
		this.lblMacType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.tpanelSubDegrees.AutoSize = true;
		this.tpanelSubDegrees.ColumnCount = 7;
		this.tpanelSubDegrees.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelSubDegrees.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelSubDegrees.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelSubDegrees.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelSubDegrees.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelSubDegrees.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelSubDegrees.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelSubDegrees.Controls.Add(this.txtSign, 0, 0);
		this.tpanelSubDegrees.Controls.Add(this.txtSeconds, 5, 0);
		this.tpanelSubDegrees.Controls.Add(this.lblSeconds, 6, 0);
		this.tpanelSubDegrees.Controls.Add(this.txtMinutes, 3, 0);
		this.tpanelSubDegrees.Controls.Add(this.lblMinutes, 4, 0);
		this.tpanelSubDegrees.Controls.Add(this.txtDegrees, 1, 0);
		this.tpanelSubDegrees.Controls.Add(this.lblDegrees, 2, 0);
		this.tpanelSubDegrees.Location = new System.Drawing.Point(1, 1);
		this.tpanelSubDegrees.Margin = new System.Windows.Forms.Padding(0);
		this.tpanelSubDegrees.Name = "tpanelSubDegrees";
		this.tpanelSubDegrees.RowCount = 1;
		this.tpanelSubDegrees.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tpanelSubDegrees.Size = new System.Drawing.Size(233, 28);
		this.tpanelSubDegrees.TabIndex = 1;
		this.lblSeconds.AutoSize = true;
		this.lblSeconds.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblSeconds.Location = new System.Drawing.Point(221, 0);
		this.lblSeconds.Margin = new System.Windows.Forms.Padding(0);
		this.lblSeconds.Name = "lblSeconds";
		this.lblSeconds.Size = new System.Drawing.Size(12, 28);
		this.lblSeconds.TabIndex = 27;
		this.lblSeconds.Text = "\"";
		this.lblSeconds.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblMinutes.AutoSize = true;
		this.lblMinutes.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblMinutes.Location = new System.Drawing.Point(155, 0);
		this.lblMinutes.Margin = new System.Windows.Forms.Padding(0);
		this.lblMinutes.Name = "lblMinutes";
		this.lblMinutes.Size = new System.Drawing.Size(10, 28);
		this.lblMinutes.TabIndex = 25;
		this.lblMinutes.Text = "'";
		this.lblMinutes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblDegrees.AutoSize = true;
		this.lblDegrees.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblDegrees.Location = new System.Drawing.Point(88, 0);
		this.lblDegrees.Margin = new System.Windows.Forms.Padding(0);
		this.lblDegrees.Name = "lblDegrees";
		this.lblDegrees.Size = new System.Drawing.Size(11, 28);
		this.lblDegrees.TabIndex = 23;
		this.lblDegrees.Text = "°";
		this.lblDegrees.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.tpanelDegrees.AutoSize = true;
		this.tpanelDegrees.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		this.tpanelDegrees.ColumnCount = 2;
		this.tpanelDegrees.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelDegrees.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelDegrees.Controls.Add(this.btnOK, 1, 0);
		this.tpanelDegrees.Controls.Add(this.tpanelSubDegrees, 0, 0);
		this.tpanelDegrees.Location = new System.Drawing.Point(20, 70);
		this.tpanelDegrees.Name = "tpanelDegrees";
		this.tpanelDegrees.RowCount = 1;
		this.tpanelDegrees.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tpanelDegrees.Size = new System.Drawing.Size(276, 30);
		this.tpanelDegrees.TabIndex = 3;
		this.tpanelDegrees.Visible = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(540, 621);
		base.Controls.Add(this.tpanelDegrees);
		base.Controls.Add(this.panelMain);
		base.Controls.Add(this.btnConfirm);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "frmMeasurementAdd";
		base.Padding = new System.Windows.Forms.Padding(20, 70, 20, 20);
		this.Text = "5S QA System * MEASUREMENT";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmMeasurementAdd_FormClosing);
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmMeasurementAdd_FormClosed);
		base.Load += new System.EventHandler(frmMeasurementAdd_Load);
		base.Shown += new System.EventHandler(frmMeasurementAdd_Shown);
		this.panelMain.ResumeLayout(false);
		this.panelMain.PerformLayout();
		this.tableLayoutPanelMain.ResumeLayout(false);
		this.tableLayoutPanelMain.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.tpanelSubDegrees.ResumeLayout(false);
		this.tpanelSubDegrees.PerformLayout();
		this.tpanelDegrees.ResumeLayout(false);
		this.tpanelDegrees.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
