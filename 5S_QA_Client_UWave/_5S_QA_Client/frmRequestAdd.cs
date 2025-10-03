using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using _5S_QA_Client.Controls;
using _5S_QA_Client.Properties;
using _5S_QA_DL.APIContext;
using _5S_QA_Entities.Constants;
using _5S_QA_Entities.Dtos;
using _5S_QA_Entities.Models;
using MetroFramework.Forms;
using Newtonsoft.Json;
using WpfUserImage;
using MessageBox = System.Windows.Forms.MessageBox;

namespace _5S_QA_Client;

public class frmRequestAdd : MetroForm
{
	private readonly frmLogin mForm;

	private Guid mId;

	private string mRemember;

	private readonly DataTable mData;

	public bool isClose;

	private readonly bool isAdd;

	private readonly List<Control> disabledControl = new List<Control>();

	private readonly List<Control> importantControl = new List<Control>();

	private DataTable mDataGroup;

	private DataTable mDataProduct;

	private string mFileName;

	private RequestViewModel mRequest;

	private UcWpf ucWpf;

	private IContainer components = null;

	private ToolTip toolTipMain;

	private Button btnConfirm;

	private Panel panelMain;

	private Panel panelMainResize;

	private DataGridView dgvMain;

	private TableLayoutPanel tableLayoutPanel1;

	private Label lblMeas;

	private Label lblTotalRow;

	private Label lblMeasTotal;

	private Panel panel1;

	private TableLayoutPanel tableLayoutPanel2;

	private TextBox txtIntention;

	private TextBox txtQuantity;

	private TextBox txtName;

	private TextBox txtCode;

	private Label lblModifiedBy;

	private Label lblModifiBy;

	private Label lblCreatedBy;

	private Label lblModifi;

	private Label lblModified;

	private Label lblCreated;

	private Label lblCreateBy;

	private Label lblCreate;

	private Label lblSample;

	private Label lblIntention;

	private Label lblQuantity;

	private Label lblProduct;

	private Label lblRequestDate;

	private Label lblName;

	private Label lblCode;

	private ComboBox cbbSample;

	private DateTimePicker dtpRequestDate;

	private ComboBox cbbProduct;

	private Label lblType;

	private TextBox txtLot;

	private Label lblLine;

	private Label lblLot;

	private OpenFileDialog openFileDialogMain;

	private ComboBox cbbType;

	private Panel panelViewImage;

	private PictureBox ptbImage;

	private ElementHost elementHostZoomImage;

	private Panel panelViewImageResize;

	private ComboBox cbbLine;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;

	private DataGridViewTextBoxColumn No;

	private DataGridViewTextBoxColumn Code;

	private DataGridViewTextBoxColumn name;

	private DataGridViewTextBoxColumn Value;

	private DataGridViewTextBoxColumn UnitName;

	private DataGridViewTextBoxColumn Upper;

	private DataGridViewTextBoxColumn Lower;

	private DataGridViewTextBoxColumn MachineTypeName;

	private DataGridViewTextBoxColumn Coordinate;

	private DataGridViewTextBoxColumn Id;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;

	private ComboBox cbbGroup;

	private Label lblGroup;

	public frmRequestAdd(frmLogin frm, DataTable data, Guid id, bool isadd = true)
	{
		InitializeComponent();
		Common.setControls(this, toolTipMain);
		ControlResize.Init(panelViewImageResize, panelViewImage, ControlResize.Direction.Vertical, Cursors.SizeNS);
		panelViewImage.Visible = false;
		mForm = frm;
		isClose = true;
		mData = data;
		mId = id;
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
		importantControl = new List<Control> { cbbGroup, cbbProduct, txtLot, cbbLine, dtpRequestDate };
	}

	private void frmRequestAdd_Load(object sender, EventArgs e)
	{
		Init();
	}

	private void frmRequestAdd_Shown(object sender, EventArgs e)
	{
		load_cbbType();
		load_cbbLine();
		load_cbbGroup();
		load_cbbProduct();
		if (isAdd)
		{
			cbbGroup.Enabled = true;
			cbbProduct.Enabled = true;
			txtCode.Text = set_Code();
			created_Name();
		}
		load_Data();
		string text = cbbSample.Text;
		txtLot.Select();
		cbbNormal_SelectedIndexChanged(cbbProduct, null);
		if (!isAdd)
		{
			cbbSample.Text = text;
		}
		cbbProduct.SelectedIndexChanged += cbbNormal_SelectedIndexChanged;
		disabledFieldsNoUser();
		addEventImportantControls();
		cbbType.Text = Common.getRequestType();
	}

	private void frmRequestAdd_FormClosing(object sender, FormClosingEventArgs e)
	{
		GC.Collect();
	}

	private void frmRequestAdd_FormClosed(object sender, FormClosedEventArgs e)
	{
		mData?.Dispose();
		if (!isClose)
		{
			mForm.load_AllData();
			new frmResultView(mForm, mRequest).Show();
		}
	}

	private void Init()
	{
		ControlResize.Init(panelMainResize, panelMain);
		load_AutoComplete();
		cbbSample.Text = "1";
		lblCreated.Text = "";
		lblModified.Text = "";
		lblCreatedBy.Text = "";
		lblModifiedBy.Text = "";
	}

	private void disabledFieldsNoUser()
	{
		foreach (Control item in disabledControl)
		{
			item.Enabled = false;
		}
	}

	private void addEventImportantControls()
	{
		foreach (Control item in importantControl)
		{
			if (!item.GetType().Equals(typeof(DateTimePicker)))
			{
				item.Enter += cbbNormal_Enter;
				item.Leave += cbbNormalNotNull_Leave;
			}
		}
	}

	private void load_cbbType()
	{
		try
		{
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "TypeId=@0";
			queryArgs.PredicateParameters = new string[1] { "AC5FA813-C9EE-4805-A850-30A5EA5AB0A1" };
			queryArgs.Order = "Created DESC";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs body = queryArgs;
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/MetadataValue/Gets").Result;
			DataTable dataTable = Common.getDataTable(result);
			if (dataTable != null)
			{
				dataTable.Dispose();
				cbbType.ValueMember = "Id";
				cbbType.DisplayMember = "Name";
				cbbType.DataSource = dataTable;
			}
			else
			{
				cbbType.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			cbbType.DataSource = null;
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

	private void load_cbbLine()
	{
		try
		{
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "TypeId=@0";
			queryArgs.PredicateParameters = new string[1] { "AC5FA814-C9EE-4807-A851-30A5EA5AB0A2" };
			queryArgs.Order = "Created DESC";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs body = queryArgs;
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/MetadataValue/Gets").Result;
			DataTable dataTable = Common.getDataTable(result);
			if (dataTable != null)
			{
				dataTable.Dispose();
				cbbLine.ValueMember = "Id";
				cbbLine.DisplayMember = "Name";
				cbbLine.DataSource = dataTable;
			}
			else
			{
				cbbLine.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			cbbLine.DataSource = null;
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
		txtQuantity.AutoCompleteCustomSource = Common.getAutoComplete(mData, "Quantity");
		txtLot.AutoCompleteCustomSource = Common.getAutoComplete(mData, "Lot");
		txtIntention.AutoCompleteCustomSource = Common.getAutoComplete(mData, "Intention");
	}

	private void load_cbbGroup()
	{
		try
		{
			QueryArgs body = new QueryArgs
			{
				Predicate = "Products.Any() && IsActivated",
				Order = "Created DESC",
				Page = 1,
				Limit = int.MaxValue
			};
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/ProductGroup/Gets").Result;
			mDataGroup = Common.getDataTable(result);
			if (mDataGroup != null)
			{
				mDataGroup.Dispose();
				cbbGroup.ValueMember = "Id";
				cbbGroup.DisplayMember = "CodeName";
				cbbGroup.DataSource = mDataGroup;
			}
			else
			{
				cbbGroup.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			cbbGroup.DataSource = null;
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

	private void load_cbbProduct()
	{
		try
		{
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "Measurements.Any() && IsActivated && GroupId=@0";
			queryArgs.PredicateParameters = new string[1] { string.IsNullOrEmpty(cbbGroup.Text) ? Guid.Empty.ToString() : cbbGroup.SelectedValue.ToString() };
			queryArgs.Order = "Created DESC";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs body = queryArgs;
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/Product/Gets").Result;
			mDataProduct = Common.getDataTable(result);
			if (mDataProduct != null)
			{
				mDataProduct.Dispose();
				cbbProduct.ValueMember = "Id";
				cbbProduct.DisplayMember = "Name";
				cbbProduct.DataSource = mDataProduct;
			}
			else
			{
				cbbProduct.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			cbbProduct.DataSource = null;
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

	private void load_Measurement()
	{
		try
		{
			ResponseDto responseDto = new ResponseDto();
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "ProductId=@0";
			queryArgs.PredicateParameters = new string[1] { string.IsNullOrEmpty(cbbProduct.Text) ? Guid.Empty.ToString() : cbbProduct.SelectedValue.ToString() };
			queryArgs.Order = "Sort, Created";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs body = queryArgs;
			responseDto = frmLogin.client.GetsAsync(body, "/api/Measurement/Gets").Result;
			dgvMain.DataSource = Common.getDataTableNoType<MeasurementQuickViewModel>(responseDto);
			dgvMain.Refresh();
			lblMeasTotal.Text = dgvMain.RowCount.ToString();
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
		if (mData == null || mData.Rows.Count <= 0 || !(mId != default(Guid)))
		{
			return;
		}
		DataRow dataRow = mData.Select($"Id='{mId}'").FirstOrDefault();
		if (dataRow.ItemArray.Length != 0)
		{
			cbbGroup.SelectedValue = dataRow["GroupId"];
			cbbProduct.SelectedValue = dataRow["ProductId"];
			cbbSample.Text = dataRow["Sample"].ToString();
			dtpRequestDate.Value = DateTime.Parse(dataRow["Date"].ToString());
			txtQuantity.Text = dataRow["Quantity"].ToString();
			txtLot.Text = dataRow["Lot"].ToString();
			cbbLine.Text = dataRow["Line"].ToString();
			txtIntention.Text = dataRow["Intention"].ToString();
			cbbType.Text = dataRow["Type"].ToString();
			if (!isAdd)
			{
				mFileName = dataRow["Link"].ToString();
				txtCode.Text = dataRow["Code"].ToString();
				txtName.Text = dataRow["Name"].ToString();
				lblCreated.Text = dataRow["Created"].ToString();
				lblModified.Text = dataRow["Modified"].ToString();
				lblCreatedBy.Text = dataRow["CreatedBy"].ToString();
				lblModifiedBy.Text = dataRow["ModifiedBy"].ToString();
			}
		}
	}

	private void created_Name()
	{
		string text = string.Empty;
		foreach (Control item in importantControl)
		{
			if (string.IsNullOrEmpty(text))
			{
				if (string.IsNullOrEmpty(item.Text))
				{
					continue;
				}
				if (item.GetType().Equals(typeof(DateTimePicker)))
				{
					text += ((DateTimePicker)item).Value.ToString("yyMMdd");
				}
				else if (item.Name == cbbGroup.Name)
				{
					DataRow dataRow = (from DataRow x in mDataGroup.Rows
						where x.Field<string>("Id").Equals((cbbGroup.DataSource == null) ? string.Empty : cbbGroup.SelectedValue)
						select x).FirstOrDefault();
					if (dataRow != null)
					{
						text += dataRow["Code"].ToString();
					}
				}
				else
				{
					text += item.Text;
				}
			}
			else
			{
				if (string.IsNullOrEmpty(item.Text))
				{
					continue;
				}
				if (item.GetType().Equals(typeof(DateTimePicker)))
				{
					text += $"_{((DateTimePicker)item).Value:yyMMdd}";
				}
				else if (item.Name == cbbGroup.Name)
				{
					DataRow dataRow2 = (from DataRow x in mDataGroup.Rows
						where x.Field<string>("Id").Equals((cbbGroup.DataSource == null) ? string.Empty : cbbGroup.SelectedValue)
						select x).FirstOrDefault();
					if (dataRow2 != null)
					{
						text += string.Format("_{0}", dataRow2["Code"]);
					}
				}
				else
				{
					text = text + "_" + item.Text;
				}
			}
		}
		txtName.Text = Common.trimSpace(text);
	}

	private string set_Code()
	{
		string text = "REQ-";
		try
		{
			QueryArgs body = new QueryArgs
			{
				Order = "Created DESC",
				Page = 1,
				Limit = 1
			};
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/Request/Gets").Result;
			DataTable dataTable = Common.getDataTable<RequestViewModel>(result);
			if (dataTable.Rows.Count > 0)
			{
				string[] array = dataTable.Rows[0]["Code"].ToString().Split('-');
				if (array.Length > 1 && double.TryParse(array[1], out var result2))
				{
					text += result2 + 1.0;
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

	private void createdSample()
	{
		cbbSample.Items.Clear();
		if (mDataProduct == null)
		{
			return;
		}
		DataRow dataRow = (from DataRow x in mDataProduct.Rows
			where x.Field<string>("Id").Equals((cbbProduct.DataSource == null) ? string.Empty : cbbProduct.SelectedValue)
			select x).FirstOrDefault();
		if (dataRow != null)
		{
			int.TryParse(dataRow["SampleMax"].ToString(), out var result);
			if (result > 100)
			{
				result = 100;
			}
			for (int num = 0; num < result; num++)
			{
				cbbSample.Items.Add(num + 1);
			}
		}
	}

	private void Load_ImageProduct(string url)
	{
		if (string.IsNullOrEmpty(url))
		{
			panelViewImage.Visible = false;
		}
		else
		{
			panelViewImage.Visible = true;
		}
		try
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(APIUrl.APIHost + "/ProductImage/").Append(url);
			ptbImage.Load(stringBuilder.ToString());
			ptbImage_Click(ptbImage, null);
		}
		catch
		{
			ptbImage.Image = Resources._5S_QA_C;
		}
	}

	private void loadSampleWithAQL()
	{
		if (string.IsNullOrEmpty(txtQuantity.Text) || cbbProduct.DataSource == null || cbbProduct.SelectedValue == null)
		{
			return;
		}
		try
		{
			AQLDto body = new AQLDto
			{
				ProductId = Guid.Parse(cbbProduct.SelectedValue.ToString()),
				InputQuantity = int.Parse(txtQuantity.Text)
			};
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/AQL/Samples").Result;
			List<LimitViewModel> objects = Common.getObjects<LimitViewModel>(result);
			int? num = objects.Max((LimitViewModel x) => x.Sample);
			cbbSample.Text = ((!num.HasValue) ? "1" : num.ToString());
		}
		catch (Exception ex)
		{
			cbbSample.SelectedIndex = -1;
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

	private void btnComfirm_Click(object sender, EventArgs e)
	{
		try
		{
			foreach (Control item in importantControl)
			{
				if (string.IsNullOrEmpty(item.Text) && item.Enabled)
				{
					MessageBox.Show(Common.getTextLanguage(this, "wImportantField"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					item.Focus();
					return;
				}
			}
			if (string.IsNullOrEmpty(txtQuantity.Text))
			{
				MessageBox.Show(Common.getTextLanguage(this, "wQuantity"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				txtQuantity.Focus();
				return;
			}
			if (string.IsNullOrEmpty(cbbSample.Text))
			{
				MessageBox.Show(Common.getTextLanguage(this, "wSample"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				cbbSample.Focus();
				return;
			}
			if (string.IsNullOrEmpty(cbbType.Text))
			{
				MessageBox.Show(Common.getTextLanguage(this, "wType"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				cbbType.Focus();
				return;
			}
			if (dgvMain.RowCount.Equals(0))
			{
				MessageBox.Show(Common.getTextLanguage(this, "wMeasurementHast"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				cbbProduct.Focus();
				return;
			}
			RequestViewModel requestViewModel = new RequestViewModel
			{
				Date = dtpRequestDate.Value,
				ProductId = Guid.Parse(cbbProduct.SelectedValue.ToString()),
				Code = set_Code(),
				Name = txtName.Text,
				Sample = (string.IsNullOrEmpty(cbbSample.Text) ? 1 : int.Parse(cbbSample.Text)),
				Lot = (string.IsNullOrEmpty(txtLot.Text) ? null : txtLot.Text),
				Intention = (string.IsNullOrEmpty(txtIntention.Text) ? null : txtIntention.Text),
				Line = (string.IsNullOrEmpty(cbbLine.Text) ? null : cbbLine.Text),
				Type = cbbType.Text,
				IsActivated = true
			};
			if (!string.IsNullOrEmpty(txtQuantity.Text))
			{
				requestViewModel.Quantity = int.Parse(txtQuantity.Text);
			}
			if (isAdd || !MessageBox.Show(Common.getTextLanguage(this, "wSureEdit"), Common.getTextLanguage(this, "QUESTION"), MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.No))
			{
				Cursor.Current = Cursors.WaitCursor;
				mForm.isThis = true;
				if (!isAdd)
				{
					requestViewModel.Link = mFileName;
					requestViewModel.Id = mId;
				}
				ResponseDto result = frmLogin.client.SaveAsync(requestViewModel, "/api/Request/Save").Result;
				if (!result.Success)
				{
					throw new Exception(result.Messages.ElementAt(0).Message);
				}
				string value = result.Data.ToString();
				mRequest = JsonConvert.DeserializeObject<RequestViewModel>(value);
				isClose = false;
				Close();
			}
		}
		catch (Exception ex)
		{
			mForm.isThis = false;
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

	private void dtpRequestDate_ValueChanged(object sender, EventArgs e)
	{
		created_Name();
	}

	private void cbbNormalNotNull_Leave(object sender, EventArgs e)
	{
		if (sender.GetType().Equals(typeof(ComboBox)))
		{
			ComboBox comboBox = sender as ComboBox;
			if (comboBox.SelectedIndex.Equals(-1))
			{
				comboBox.Text = mRemember;
			}
		}
		else
		{
			TextBox textBox = sender as TextBox;
			textBox.Text = Common.trimSpace(textBox.Text);
		}
		created_Name();
	}

	private void cbbSample_Leave(object sender, EventArgs e)
	{
		ComboBox comboBox = sender as ComboBox;
		if (int.TryParse(comboBox.Text, out var result))
		{
			comboBox.Text = result.ToString();
		}
		else
		{
			comboBox.Text = mRemember;
		}
		comboBox.Text = ((!int.TryParse(comboBox.Text, out result)) ? "1" : (result.Equals(0) ? "1" : result.ToString()));
		if (mDataProduct != null)
		{
			DataRow dataRow = (from DataRow x in mDataProduct.Rows
				where x.Field<string>("Id").Equals((cbbProduct.DataSource == null) ? string.Empty : cbbProduct.SelectedValue)
				select x).FirstOrDefault();
			int.TryParse(dataRow["SampleMax"].ToString(), out var result2);
			if (int.Parse(comboBox.Text) > (result2.Equals(0) ? 1 : result2))
			{
				comboBox.Text = result2.ToString();
			}
		}
	}

	private void cbbNormal_Enter(object sender, EventArgs e)
	{
		if (sender.GetType().Equals(typeof(ComboBox)))
		{
			ComboBox comboBox = sender as ComboBox;
			mRemember = comboBox.Text;
		}
	}

	private void cbbGroup_SelectedIndexChanged(object sender, EventArgs e)
	{
		load_cbbProduct();
	}

	private void cbbNormal_SelectedIndexChanged(object sender, EventArgs e)
	{
		createdSample();
		load_Measurement();
		if (mDataProduct != null)
		{
			DataRow dataRow = (from DataRow x in mDataProduct.Rows
				where x.Field<string>("Id").Equals((cbbProduct.DataSource == null) ? string.Empty : cbbProduct.SelectedValue)
				select x).FirstOrDefault();
			if (dataRow != null)
			{
				Load_ImageProduct(dataRow["ImageUrl"].ToString());
				loadSampleWithAQL();
			}
		}
	}

	private void txtNormal_Leave(object sender, EventArgs e)
	{
		TextBox textBox = sender as TextBox;
		textBox.Text = Common.trimSpace(textBox.Text);
	}

	private void txtQuantity_Leave(object sender, EventArgs e)
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
		loadSampleWithAQL();
	}

	private void cbbSample_KeyPress(object sender, KeyPressEventArgs e)
	{
		if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b')
		{
			e.Handled = true;
		}
	}

	private void ptbImage_Click(object sender, EventArgs e)
	{
		elementHostZoomImage.Child = null;
		ucWpf = new UcWpf(Common.ToBitmapImage((Bitmap)ptbImage.Image));
		elementHostZoomImage.Child = (UIElement)(object)ucWpf;
	}

	private void dgvMain_Sorted(object sender, EventArgs e)
	{
		DataGridView dataGridView = sender as DataGridView;
		foreach (DataGridViewRow item in (IEnumerable)dataGridView.Rows)
		{
			item.Cells["No"].Value = item.Index + 1;
			if (item.Cells["UnitName"].Value?.ToString() == "°")
			{
				if (double.TryParse(item.Cells["Value"].Value?.ToString(), out var result))
				{
					item.Cells["Value"].Value = Common.ConvertDoubleToDegrees(result);
				}
				if (double.TryParse(item.Cells["Upper"].Value?.ToString(), out var result2))
				{
					item.Cells["Upper"].Value = Common.ConvertDoubleToDegrees(result2);
				}
				if (double.TryParse(item.Cells["Lower"].Value?.ToString(), out var result3))
				{
					item.Cells["Lower"].Value = Common.ConvertDoubleToDegrees(result3);
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
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_5S_QA_Client.frmRequestAdd));
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.btnConfirm = new System.Windows.Forms.Button();
		this.txtIntention = new System.Windows.Forms.TextBox();
		this.txtQuantity = new System.Windows.Forms.TextBox();
		this.txtName = new System.Windows.Forms.TextBox();
		this.txtCode = new System.Windows.Forms.TextBox();
		this.cbbSample = new System.Windows.Forms.ComboBox();
		this.dtpRequestDate = new System.Windows.Forms.DateTimePicker();
		this.cbbProduct = new System.Windows.Forms.ComboBox();
		this.txtLot = new System.Windows.Forms.TextBox();
		this.cbbType = new System.Windows.Forms.ComboBox();
		this.ptbImage = new System.Windows.Forms.PictureBox();
		this.cbbLine = new System.Windows.Forms.ComboBox();
		this.cbbGroup = new System.Windows.Forms.ComboBox();
		this.panelMain = new System.Windows.Forms.Panel();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
		this.lblGroup = new System.Windows.Forms.Label();
		this.lblLine = new System.Windows.Forms.Label();
		this.lblLot = new System.Windows.Forms.Label();
		this.lblType = new System.Windows.Forms.Label();
		this.lblModifiedBy = new System.Windows.Forms.Label();
		this.lblModifiBy = new System.Windows.Forms.Label();
		this.lblCreatedBy = new System.Windows.Forms.Label();
		this.lblModifi = new System.Windows.Forms.Label();
		this.lblModified = new System.Windows.Forms.Label();
		this.lblCreated = new System.Windows.Forms.Label();
		this.lblCreateBy = new System.Windows.Forms.Label();
		this.lblCreate = new System.Windows.Forms.Label();
		this.lblSample = new System.Windows.Forms.Label();
		this.lblIntention = new System.Windows.Forms.Label();
		this.lblQuantity = new System.Windows.Forms.Label();
		this.lblProduct = new System.Windows.Forms.Label();
		this.lblRequestDate = new System.Windows.Forms.Label();
		this.lblName = new System.Windows.Forms.Label();
		this.lblCode = new System.Windows.Forms.Label();
		this.panelMainResize = new System.Windows.Forms.Panel();
		this.dgvMain = new System.Windows.Forms.DataGridView();
		this.No = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.UnitName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Upper = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Lower = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.MachineTypeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Coordinate = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
		this.lblMeas = new System.Windows.Forms.Label();
		this.lblTotalRow = new System.Windows.Forms.Label();
		this.lblMeasTotal = new System.Windows.Forms.Label();
		this.openFileDialogMain = new System.Windows.Forms.OpenFileDialog();
		this.panelViewImage = new System.Windows.Forms.Panel();
		this.elementHostZoomImage = new System.Windows.Forms.Integration.ElementHost();
		this.panelViewImageResize = new System.Windows.Forms.Panel();
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
		((System.ComponentModel.ISupportInitialize)this.ptbImage).BeginInit();
		this.panelMain.SuspendLayout();
		this.panel1.SuspendLayout();
		this.tableLayoutPanel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvMain).BeginInit();
		this.tableLayoutPanel1.SuspendLayout();
		this.panelViewImage.SuspendLayout();
		base.SuspendLayout();
		this.btnConfirm.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnConfirm.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.btnConfirm.FlatAppearance.BorderSize = 0;
		this.btnConfirm.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
		this.btnConfirm.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
		this.btnConfirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnConfirm.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnConfirm.Location = new System.Drawing.Point(0, 444);
		this.btnConfirm.Margin = new System.Windows.Forms.Padding(4);
		this.btnConfirm.Name = "btnConfirm";
		this.btnConfirm.Size = new System.Drawing.Size(448, 28);
		this.btnConfirm.TabIndex = 2;
		this.btnConfirm.Text = "Confirm";
		this.toolTipMain.SetToolTip(this.btnConfirm, "Confirm");
		this.btnConfirm.UseVisualStyleBackColor = true;
		this.btnConfirm.Click += new System.EventHandler(btnComfirm_Click);
		this.txtIntention.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtIntention.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtIntention.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtIntention.Location = new System.Drawing.Point(96, 236);
		this.txtIntention.Name = "txtIntention";
		this.txtIntention.Size = new System.Drawing.Size(342, 22);
		this.txtIntention.TabIndex = 9;
		this.toolTipMain.SetToolTip(this.txtIntention, "Please enter intention");
		this.txtIntention.Leave += new System.EventHandler(txtNormal_Leave);
		this.txtQuantity.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtQuantity.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtQuantity.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtQuantity.Location = new System.Drawing.Point(96, 149);
		this.txtQuantity.Name = "txtQuantity";
		this.txtQuantity.Size = new System.Drawing.Size(342, 22);
		this.txtQuantity.TabIndex = 6;
		this.toolTipMain.SetToolTip(this.txtQuantity, "Please enter quantity");
		this.txtQuantity.KeyPress += new System.Windows.Forms.KeyPressEventHandler(cbbSample_KeyPress);
		this.txtQuantity.Leave += new System.EventHandler(txtQuantity_Leave);
		this.txtName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtName.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtName.Enabled = false;
		this.txtName.Location = new System.Drawing.Point(96, 33);
		this.txtName.Name = "txtName";
		this.txtName.Size = new System.Drawing.Size(342, 22);
		this.txtName.TabIndex = 2;
		this.toolTipMain.SetToolTip(this.txtName, "Please enter name");
		this.txtCode.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtCode.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtCode.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtCode.Enabled = false;
		this.txtCode.Location = new System.Drawing.Point(96, 4);
		this.txtCode.Name = "txtCode";
		this.txtCode.Size = new System.Drawing.Size(342, 22);
		this.txtCode.TabIndex = 1;
		this.toolTipMain.SetToolTip(this.txtCode, "Please enter code");
		this.cbbSample.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cbbSample.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cbbSample.Dock = System.Windows.Forms.DockStyle.Fill;
		this.cbbSample.FormattingEnabled = true;
		this.cbbSample.ItemHeight = 16;
		this.cbbSample.Location = new System.Drawing.Point(96, 264);
		this.cbbSample.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.cbbSample.Name = "cbbSample";
		this.cbbSample.Size = new System.Drawing.Size(342, 24);
		this.cbbSample.TabIndex = 10;
		this.toolTipMain.SetToolTip(this.cbbSample, "Select or enter produce quantity");
		this.cbbSample.Enter += new System.EventHandler(cbbNormal_Enter);
		this.cbbSample.KeyPress += new System.Windows.Forms.KeyPressEventHandler(cbbSample_KeyPress);
		this.cbbSample.Leave += new System.EventHandler(cbbSample_Leave);
		this.dtpRequestDate.Dock = System.Windows.Forms.DockStyle.Fill;
		this.dtpRequestDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.dtpRequestDate.Location = new System.Drawing.Point(96, 62);
		this.dtpRequestDate.Name = "dtpRequestDate";
		this.dtpRequestDate.Size = new System.Drawing.Size(342, 22);
		this.dtpRequestDate.TabIndex = 3;
		this.toolTipMain.SetToolTip(this.dtpRequestDate, "Select request date");
		this.dtpRequestDate.ValueChanged += new System.EventHandler(dtpRequestDate_ValueChanged);
		this.cbbProduct.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cbbProduct.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cbbProduct.Dock = System.Windows.Forms.DockStyle.Fill;
		this.cbbProduct.Enabled = false;
		this.cbbProduct.FormattingEnabled = true;
		this.cbbProduct.ItemHeight = 16;
		this.cbbProduct.Location = new System.Drawing.Point(96, 119);
		this.cbbProduct.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.cbbProduct.Name = "cbbProduct";
		this.cbbProduct.Size = new System.Drawing.Size(342, 24);
		this.cbbProduct.TabIndex = 5;
		this.toolTipMain.SetToolTip(this.cbbProduct, "Select or enter stage name");
		this.txtLot.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtLot.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtLot.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtLot.Location = new System.Drawing.Point(96, 178);
		this.txtLot.Name = "txtLot";
		this.txtLot.Size = new System.Drawing.Size(342, 22);
		this.txtLot.TabIndex = 7;
		this.toolTipMain.SetToolTip(this.txtLot, "Please enter lot no.");
		this.txtLot.Leave += new System.EventHandler(txtNormal_Leave);
		this.cbbType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cbbType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cbbType.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbbType.Dock = System.Windows.Forms.DockStyle.Fill;
		this.cbbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbbType.FormattingEnabled = true;
		this.cbbType.Location = new System.Drawing.Point(96, 293);
		this.cbbType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.cbbType.Name = "cbbType";
		this.cbbType.Size = new System.Drawing.Size(342, 24);
		this.cbbType.TabIndex = 11;
		this.toolTipMain.SetToolTip(this.cbbType, "Select or enter type");
		this.ptbImage.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ptbImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ptbImage.Cursor = System.Windows.Forms.Cursors.Hand;
		this.ptbImage.Location = new System.Drawing.Point(448, 0);
		this.ptbImage.Name = "ptbImage";
		this.ptbImage.Size = new System.Drawing.Size(60, 60);
		this.ptbImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.ptbImage.TabIndex = 154;
		this.ptbImage.TabStop = false;
		this.toolTipMain.SetToolTip(this.ptbImage, "Reset image");
		this.ptbImage.Click += new System.EventHandler(ptbImage_Click);
		this.cbbLine.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cbbLine.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cbbLine.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbbLine.Dock = System.Windows.Forms.DockStyle.Fill;
		this.cbbLine.FormattingEnabled = true;
		this.cbbLine.Location = new System.Drawing.Point(96, 207);
		this.cbbLine.Name = "cbbLine";
		this.cbbLine.Size = new System.Drawing.Size(342, 24);
		this.cbbLine.TabIndex = 8;
		this.toolTipMain.SetToolTip(this.cbbLine, "Select or enter produce machine");
		this.cbbGroup.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cbbGroup.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cbbGroup.Dock = System.Windows.Forms.DockStyle.Fill;
		this.cbbGroup.Enabled = false;
		this.cbbGroup.FormattingEnabled = true;
		this.cbbGroup.ItemHeight = 16;
		this.cbbGroup.Location = new System.Drawing.Point(96, 90);
		this.cbbGroup.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.cbbGroup.Name = "cbbGroup";
		this.cbbGroup.Size = new System.Drawing.Size(342, 24);
		this.cbbGroup.TabIndex = 4;
		this.toolTipMain.SetToolTip(this.cbbGroup, "Select or enter product");
		this.cbbGroup.SelectedIndexChanged += new System.EventHandler(cbbGroup_SelectedIndexChanged);
		this.panelMain.AutoScroll = true;
		this.panelMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panelMain.Controls.Add(this.panel1);
		this.panelMain.Controls.Add(this.panelMainResize);
		this.panelMain.Controls.Add(this.btnConfirm);
		this.panelMain.Dock = System.Windows.Forms.DockStyle.Left;
		this.panelMain.Location = new System.Drawing.Point(20, 70);
		this.panelMain.Margin = new System.Windows.Forms.Padding(4);
		this.panelMain.Name = "panelMain";
		this.panelMain.Size = new System.Drawing.Size(450, 474);
		this.panelMain.TabIndex = 23;
		this.panel1.AutoScroll = true;
		this.panel1.Controls.Add(this.tableLayoutPanel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Padding = new System.Windows.Forms.Padding(3, 3, 0, 3);
		this.panel1.Size = new System.Drawing.Size(445, 444);
		this.panel1.TabIndex = 18;
		this.tableLayoutPanel2.AutoSize = true;
		this.tableLayoutPanel2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		this.tableLayoutPanel2.ColumnCount = 2;
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel2.Controls.Add(this.cbbGroup, 1, 3);
		this.tableLayoutPanel2.Controls.Add(this.lblGroup, 0, 3);
		this.tableLayoutPanel2.Controls.Add(this.cbbLine, 1, 7);
		this.tableLayoutPanel2.Controls.Add(this.cbbType, 1, 10);
		this.tableLayoutPanel2.Controls.Add(this.txtLot, 1, 6);
		this.tableLayoutPanel2.Controls.Add(this.lblLine, 0, 7);
		this.tableLayoutPanel2.Controls.Add(this.lblLot, 0, 6);
		this.tableLayoutPanel2.Controls.Add(this.lblType, 0, 10);
		this.tableLayoutPanel2.Controls.Add(this.txtIntention, 1, 8);
		this.tableLayoutPanel2.Controls.Add(this.txtQuantity, 1, 5);
		this.tableLayoutPanel2.Controls.Add(this.txtName, 1, 1);
		this.tableLayoutPanel2.Controls.Add(this.txtCode, 1, 0);
		this.tableLayoutPanel2.Controls.Add(this.lblModifiedBy, 1, 15);
		this.tableLayoutPanel2.Controls.Add(this.lblModifiBy, 0, 15);
		this.tableLayoutPanel2.Controls.Add(this.lblCreatedBy, 1, 14);
		this.tableLayoutPanel2.Controls.Add(this.lblModifi, 0, 13);
		this.tableLayoutPanel2.Controls.Add(this.lblModified, 1, 13);
		this.tableLayoutPanel2.Controls.Add(this.lblCreated, 1, 12);
		this.tableLayoutPanel2.Controls.Add(this.lblCreateBy, 0, 14);
		this.tableLayoutPanel2.Controls.Add(this.lblCreate, 0, 12);
		this.tableLayoutPanel2.Controls.Add(this.lblSample, 0, 9);
		this.tableLayoutPanel2.Controls.Add(this.lblIntention, 0, 8);
		this.tableLayoutPanel2.Controls.Add(this.lblQuantity, 0, 5);
		this.tableLayoutPanel2.Controls.Add(this.lblProduct, 0, 4);
		this.tableLayoutPanel2.Controls.Add(this.lblRequestDate, 0, 2);
		this.tableLayoutPanel2.Controls.Add(this.lblName, 0, 1);
		this.tableLayoutPanel2.Controls.Add(this.lblCode, 0, 0);
		this.tableLayoutPanel2.Controls.Add(this.cbbSample, 1, 9);
		this.tableLayoutPanel2.Controls.Add(this.dtpRequestDate, 1, 2);
		this.tableLayoutPanel2.Controls.Add(this.cbbProduct, 1, 4);
		this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
		this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4);
		this.tableLayoutPanel2.Name = "tableLayoutPanel2";
		this.tableLayoutPanel2.RowCount = 16;
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.Size = new System.Drawing.Size(442, 437);
		this.tableLayoutPanel2.TabIndex = 1;
		this.tableLayoutPanel2.TabStop = true;
		this.lblGroup.AutoSize = true;
		this.lblGroup.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblGroup.Location = new System.Drawing.Point(4, 88);
		this.lblGroup.Name = "lblGroup";
		this.lblGroup.Size = new System.Drawing.Size(85, 28);
		this.lblGroup.TabIndex = 37;
		this.lblGroup.Text = "Product";
		this.lblGroup.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblLine.AutoSize = true;
		this.lblLine.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblLine.Location = new System.Drawing.Point(4, 204);
		this.lblLine.Name = "lblLine";
		this.lblLine.Size = new System.Drawing.Size(85, 28);
		this.lblLine.TabIndex = 33;
		this.lblLine.Text = "Line";
		this.lblLine.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblLot.AutoSize = true;
		this.lblLot.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblLot.Location = new System.Drawing.Point(4, 175);
		this.lblLot.Name = "lblLot";
		this.lblLot.Size = new System.Drawing.Size(85, 28);
		this.lblLot.TabIndex = 32;
		this.lblLot.Text = "Lot no.";
		this.lblLot.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblType.AutoSize = true;
		this.lblType.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblType.Location = new System.Drawing.Point(4, 291);
		this.lblType.Name = "lblType";
		this.lblType.Size = new System.Drawing.Size(85, 28);
		this.lblType.TabIndex = 31;
		this.lblType.Text = "Type";
		this.lblType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblModifiedBy.AutoSize = true;
		this.lblModifiedBy.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblModifiedBy.Location = new System.Drawing.Point(96, 408);
		this.lblModifiedBy.Name = "lblModifiedBy";
		this.lblModifiedBy.Size = new System.Drawing.Size(342, 28);
		this.lblModifiedBy.TabIndex = 30;
		this.lblModifiedBy.Text = "...";
		this.lblModifiedBy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblModifiBy.AutoSize = true;
		this.lblModifiBy.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblModifiBy.Location = new System.Drawing.Point(4, 408);
		this.lblModifiBy.Name = "lblModifiBy";
		this.lblModifiBy.Size = new System.Drawing.Size(85, 28);
		this.lblModifiBy.TabIndex = 30;
		this.lblModifiBy.Text = "Edited by";
		this.lblModifiBy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblCreatedBy.AutoSize = true;
		this.lblCreatedBy.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCreatedBy.Location = new System.Drawing.Point(96, 379);
		this.lblCreatedBy.Name = "lblCreatedBy";
		this.lblCreatedBy.Size = new System.Drawing.Size(342, 28);
		this.lblCreatedBy.TabIndex = 29;
		this.lblCreatedBy.Text = "...";
		this.lblCreatedBy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblModifi.AutoSize = true;
		this.lblModifi.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblModifi.Location = new System.Drawing.Point(4, 350);
		this.lblModifi.Name = "lblModifi";
		this.lblModifi.Size = new System.Drawing.Size(85, 28);
		this.lblModifi.TabIndex = 27;
		this.lblModifi.Text = "Edited date";
		this.lblModifi.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblModified.AutoSize = true;
		this.lblModified.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblModified.Location = new System.Drawing.Point(96, 350);
		this.lblModified.Name = "lblModified";
		this.lblModified.Size = new System.Drawing.Size(342, 28);
		this.lblModified.TabIndex = 27;
		this.lblModified.Text = "...";
		this.lblModified.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblCreated.AutoSize = true;
		this.lblCreated.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCreated.Location = new System.Drawing.Point(96, 321);
		this.lblCreated.Name = "lblCreated";
		this.lblCreated.Size = new System.Drawing.Size(342, 28);
		this.lblCreated.TabIndex = 28;
		this.lblCreated.Text = "...";
		this.lblCreated.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblCreateBy.AutoSize = true;
		this.lblCreateBy.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCreateBy.Location = new System.Drawing.Point(4, 379);
		this.lblCreateBy.Name = "lblCreateBy";
		this.lblCreateBy.Size = new System.Drawing.Size(85, 28);
		this.lblCreateBy.TabIndex = 25;
		this.lblCreateBy.Text = "Created by";
		this.lblCreateBy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblCreate.AutoSize = true;
		this.lblCreate.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCreate.Location = new System.Drawing.Point(4, 321);
		this.lblCreate.Name = "lblCreate";
		this.lblCreate.Size = new System.Drawing.Size(85, 28);
		this.lblCreate.TabIndex = 24;
		this.lblCreate.Text = "Created date";
		this.lblCreate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblSample.AutoSize = true;
		this.lblSample.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblSample.Location = new System.Drawing.Point(4, 262);
		this.lblSample.Name = "lblSample";
		this.lblSample.Size = new System.Drawing.Size(85, 28);
		this.lblSample.TabIndex = 23;
		this.lblSample.Text = "Sample";
		this.lblSample.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblIntention.AutoSize = true;
		this.lblIntention.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblIntention.Location = new System.Drawing.Point(4, 233);
		this.lblIntention.Name = "lblIntention";
		this.lblIntention.Size = new System.Drawing.Size(85, 28);
		this.lblIntention.TabIndex = 22;
		this.lblIntention.Text = "Intention";
		this.lblIntention.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblQuantity.AutoSize = true;
		this.lblQuantity.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblQuantity.Location = new System.Drawing.Point(4, 146);
		this.lblQuantity.Name = "lblQuantity";
		this.lblQuantity.Size = new System.Drawing.Size(85, 28);
		this.lblQuantity.TabIndex = 21;
		this.lblQuantity.Text = "Quantity";
		this.lblQuantity.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblProduct.AutoSize = true;
		this.lblProduct.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblProduct.Location = new System.Drawing.Point(4, 117);
		this.lblProduct.Name = "lblProduct";
		this.lblProduct.Size = new System.Drawing.Size(85, 28);
		this.lblProduct.TabIndex = 19;
		this.lblProduct.Text = "Stage name";
		this.lblProduct.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblRequestDate.AutoSize = true;
		this.lblRequestDate.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblRequestDate.Location = new System.Drawing.Point(4, 59);
		this.lblRequestDate.Name = "lblRequestDate";
		this.lblRequestDate.Size = new System.Drawing.Size(85, 28);
		this.lblRequestDate.TabIndex = 18;
		this.lblRequestDate.Text = "Date";
		this.lblRequestDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblName.AutoSize = true;
		this.lblName.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblName.Location = new System.Drawing.Point(4, 30);
		this.lblName.Name = "lblName";
		this.lblName.Size = new System.Drawing.Size(85, 28);
		this.lblName.TabIndex = 17;
		this.lblName.Text = "Name";
		this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblCode.AutoSize = true;
		this.lblCode.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCode.Location = new System.Drawing.Point(4, 1);
		this.lblCode.Name = "lblCode";
		this.lblCode.Size = new System.Drawing.Size(85, 28);
		this.lblCode.TabIndex = 16;
		this.lblCode.Text = "Code";
		this.lblCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.panelMainResize.Dock = System.Windows.Forms.DockStyle.Right;
		this.panelMainResize.Location = new System.Drawing.Point(445, 0);
		this.panelMainResize.Name = "panelMainResize";
		this.panelMainResize.Size = new System.Drawing.Size(3, 444);
		this.panelMainResize.TabIndex = 16;
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
		this.dgvMain.ColumnHeadersHeight = 29;
		this.dgvMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
		this.dgvMain.Columns.AddRange(this.No, this.Code, this.name, this.Value, this.UnitName, this.Upper, this.Lower, this.MachineTypeName, this.Coordinate, this.Id);
		this.dgvMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.dgvMain.EnableHeadersVisualStyles = false;
		this.dgvMain.Location = new System.Drawing.Point(470, 88);
		this.dgvMain.Margin = new System.Windows.Forms.Padding(1);
		this.dgvMain.Name = "dgvMain";
		this.dgvMain.RowHeadersVisible = false;
		this.dgvMain.RowHeadersWidth = 51;
		this.dgvMain.Size = new System.Drawing.Size(510, 236);
		this.dgvMain.TabIndex = 3;
		this.dgvMain.TabStop = false;
		this.dgvMain.Sorted += new System.EventHandler(dgvMain_Sorted);
		this.No.DataPropertyName = "No";
		dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		this.No.DefaultCellStyle = dataGridViewCellStyle3;
		this.No.HeaderText = "No";
		this.No.MinimumWidth = 6;
		this.No.Name = "No";
		this.No.ReadOnly = true;
		this.No.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.No.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.No.Width = 35;
		this.Code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Code.DataPropertyName = "Code";
		this.Code.FillWeight = 25f;
		this.Code.HeaderText = "Code";
		this.Code.MinimumWidth = 6;
		this.Code.Name = "Code";
		this.Code.ReadOnly = true;
		this.name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.name.DataPropertyName = "Name";
		this.name.FillWeight = 30f;
		this.name.HeaderText = "Name";
		this.name.MinimumWidth = 6;
		this.name.Name = "name";
		this.name.ReadOnly = true;
		this.Value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Value.DataPropertyName = "Value";
		dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.Value.DefaultCellStyle = dataGridViewCellStyle4;
		this.Value.FillWeight = 20f;
		this.Value.HeaderText = "Value";
		this.Value.MinimumWidth = 6;
		this.Value.Name = "Value";
		this.Value.ReadOnly = true;
		this.UnitName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.UnitName.DataPropertyName = "UnitName";
		this.UnitName.FillWeight = 20f;
		this.UnitName.HeaderText = "Unit";
		this.UnitName.MinimumWidth = 6;
		this.UnitName.Name = "UnitName";
		this.UnitName.ReadOnly = true;
		this.Upper.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Upper.DataPropertyName = "Upper";
		dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.Upper.DefaultCellStyle = dataGridViewCellStyle5;
		this.Upper.FillWeight = 20f;
		this.Upper.HeaderText = "Upper";
		this.Upper.MinimumWidth = 6;
		this.Upper.Name = "Upper";
		this.Upper.ReadOnly = true;
		this.Lower.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Lower.DataPropertyName = "Lower";
		dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.Lower.DefaultCellStyle = dataGridViewCellStyle6;
		this.Lower.FillWeight = 20f;
		this.Lower.HeaderText = "Lower";
		this.Lower.MinimumWidth = 6;
		this.Lower.Name = "Lower";
		this.Lower.ReadOnly = true;
		this.MachineTypeName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.MachineTypeName.DataPropertyName = "MachineTypeName";
		this.MachineTypeName.FillWeight = 25f;
		this.MachineTypeName.HeaderText = "Mac. Type";
		this.MachineTypeName.MinimumWidth = 6;
		this.MachineTypeName.Name = "MachineTypeName";
		this.MachineTypeName.ReadOnly = true;
		this.Coordinate.DataPropertyName = "Coordinate";
		this.Coordinate.HeaderText = "Coordinate";
		this.Coordinate.Name = "Coordinate";
		this.Coordinate.ReadOnly = true;
		this.Coordinate.Visible = false;
		this.Id.DataPropertyName = "Id";
		dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.Id.DefaultCellStyle = dataGridViewCellStyle7;
		this.Id.HeaderText = "Id";
		this.Id.MinimumWidth = 6;
		this.Id.Name = "Id";
		this.Id.ReadOnly = true;
		this.Id.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.Id.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.Id.Visible = false;
		this.Id.Width = 140;
		this.tableLayoutPanel1.AutoSize = true;
		this.tableLayoutPanel1.ColumnCount = 4;
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27f));
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27f));
		this.tableLayoutPanel1.Controls.Add(this.lblMeas, 0, 0);
		this.tableLayoutPanel1.Controls.Add(this.lblTotalRow, 2, 0);
		this.tableLayoutPanel1.Controls.Add(this.lblMeasTotal, 3, 0);
		this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.tableLayoutPanel1.Location = new System.Drawing.Point(470, 70);
		this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(1);
		this.tableLayoutPanel1.Name = "tableLayoutPanel1";
		this.tableLayoutPanel1.RowCount = 1;
		this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel1.Size = new System.Drawing.Size(510, 18);
		this.tableLayoutPanel1.TabIndex = 154;
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
		this.lblTotalRow.Location = new System.Drawing.Point(420, 1);
		this.lblTotalRow.Margin = new System.Windows.Forms.Padding(1);
		this.lblTotalRow.Name = "lblTotalRow";
		this.lblTotalRow.Size = new System.Drawing.Size(72, 16);
		this.lblTotalRow.TabIndex = 149;
		this.lblTotalRow.Text = "Total rows:";
		this.lblMeasTotal.AutoSize = true;
		this.lblMeasTotal.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblMeasTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblMeasTotal.Location = new System.Drawing.Point(494, 1);
		this.lblMeasTotal.Margin = new System.Windows.Forms.Padding(1);
		this.lblMeasTotal.Name = "lblMeasTotal";
		this.lblMeasTotal.Size = new System.Drawing.Size(15, 16);
		this.lblMeasTotal.TabIndex = 152;
		this.lblMeasTotal.Text = "0";
		this.openFileDialogMain.Filter = "Excel file (*.xlsx)| *.xlsx";
		this.openFileDialogMain.Title = "Please select a file";
		this.panelViewImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panelViewImage.Controls.Add(this.ptbImage);
		this.panelViewImage.Controls.Add(this.elementHostZoomImage);
		this.panelViewImage.Controls.Add(this.panelViewImageResize);
		this.panelViewImage.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panelViewImage.Location = new System.Drawing.Point(470, 324);
		this.panelViewImage.Name = "panelViewImage";
		this.panelViewImage.Size = new System.Drawing.Size(510, 220);
		this.panelViewImage.TabIndex = 156;
		this.elementHostZoomImage.BackColor = System.Drawing.SystemColors.Control;
		this.elementHostZoomImage.Dock = System.Windows.Forms.DockStyle.Fill;
		this.elementHostZoomImage.Location = new System.Drawing.Point(0, 3);
		this.elementHostZoomImage.Name = "elementHostZoomImage";
		this.elementHostZoomImage.Size = new System.Drawing.Size(508, 215);
		this.elementHostZoomImage.TabIndex = 153;
		this.elementHostZoomImage.Child = null;
		this.panelViewImageResize.Dock = System.Windows.Forms.DockStyle.Top;
		this.panelViewImageResize.Location = new System.Drawing.Point(0, 0);
		this.panelViewImageResize.Name = "panelViewImageResize";
		this.panelViewImageResize.Size = new System.Drawing.Size(508, 3);
		this.panelViewImageResize.TabIndex = 0;
		this.dataGridViewTextBoxColumn1.DataPropertyName = "No";
		dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle8;
		this.dataGridViewTextBoxColumn1.HeaderText = "No";
		this.dataGridViewTextBoxColumn1.MinimumWidth = 6;
		this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
		this.dataGridViewTextBoxColumn1.ReadOnly = true;
		this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.dataGridViewTextBoxColumn1.Width = 35;
		this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn2.DataPropertyName = "Code";
		this.dataGridViewTextBoxColumn2.FillWeight = 25f;
		this.dataGridViewTextBoxColumn2.HeaderText = "Code";
		this.dataGridViewTextBoxColumn2.MinimumWidth = 6;
		this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
		this.dataGridViewTextBoxColumn2.ReadOnly = true;
		this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn3.DataPropertyName = "Name";
		this.dataGridViewTextBoxColumn3.FillWeight = 30f;
		this.dataGridViewTextBoxColumn3.HeaderText = "Name";
		this.dataGridViewTextBoxColumn3.MinimumWidth = 6;
		this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
		this.dataGridViewTextBoxColumn3.ReadOnly = true;
		this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn4.DataPropertyName = "Value";
		dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.dataGridViewTextBoxColumn4.DefaultCellStyle = dataGridViewCellStyle9;
		this.dataGridViewTextBoxColumn4.FillWeight = 20f;
		this.dataGridViewTextBoxColumn4.HeaderText = "Value";
		this.dataGridViewTextBoxColumn4.MinimumWidth = 6;
		this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
		this.dataGridViewTextBoxColumn4.ReadOnly = true;
		this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn5.DataPropertyName = "UnitName";
		this.dataGridViewTextBoxColumn5.FillWeight = 20f;
		this.dataGridViewTextBoxColumn5.HeaderText = "Unit";
		this.dataGridViewTextBoxColumn5.MinimumWidth = 6;
		this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
		this.dataGridViewTextBoxColumn5.ReadOnly = true;
		this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn6.DataPropertyName = "Upper";
		dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.dataGridViewTextBoxColumn6.DefaultCellStyle = dataGridViewCellStyle10;
		this.dataGridViewTextBoxColumn6.FillWeight = 20f;
		this.dataGridViewTextBoxColumn6.HeaderText = "Upper";
		this.dataGridViewTextBoxColumn6.MinimumWidth = 6;
		this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
		this.dataGridViewTextBoxColumn6.ReadOnly = true;
		this.dataGridViewTextBoxColumn7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn7.DataPropertyName = "Lower";
		dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.dataGridViewTextBoxColumn7.DefaultCellStyle = dataGridViewCellStyle11;
		this.dataGridViewTextBoxColumn7.FillWeight = 20f;
		this.dataGridViewTextBoxColumn7.HeaderText = "Lower";
		this.dataGridViewTextBoxColumn7.MinimumWidth = 6;
		this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
		this.dataGridViewTextBoxColumn7.ReadOnly = true;
		this.dataGridViewTextBoxColumn8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn8.DataPropertyName = "MachineTypeName";
		this.dataGridViewTextBoxColumn8.FillWeight = 25f;
		this.dataGridViewTextBoxColumn8.HeaderText = "Mac. Type";
		this.dataGridViewTextBoxColumn8.MinimumWidth = 6;
		this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
		this.dataGridViewTextBoxColumn8.ReadOnly = true;
		this.dataGridViewTextBoxColumn9.DataPropertyName = "Id";
		dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.dataGridViewTextBoxColumn9.DefaultCellStyle = dataGridViewCellStyle12;
		this.dataGridViewTextBoxColumn9.HeaderText = "Id";
		this.dataGridViewTextBoxColumn9.MinimumWidth = 6;
		this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
		this.dataGridViewTextBoxColumn9.ReadOnly = true;
		this.dataGridViewTextBoxColumn9.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.dataGridViewTextBoxColumn9.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.dataGridViewTextBoxColumn9.Visible = false;
		this.dataGridViewTextBoxColumn9.Width = 140;
		this.dataGridViewTextBoxColumn10.DataPropertyName = "Id";
		dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.dataGridViewTextBoxColumn10.DefaultCellStyle = dataGridViewCellStyle13;
		this.dataGridViewTextBoxColumn10.HeaderText = "Id";
		this.dataGridViewTextBoxColumn10.MinimumWidth = 6;
		this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
		this.dataGridViewTextBoxColumn10.ReadOnly = true;
		this.dataGridViewTextBoxColumn10.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.dataGridViewTextBoxColumn10.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.dataGridViewTextBoxColumn10.Visible = false;
		this.dataGridViewTextBoxColumn10.Width = 140;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1000, 564);
		base.Controls.Add(this.dgvMain);
		base.Controls.Add(this.panelViewImage);
		base.Controls.Add(this.tableLayoutPanel1);
		base.Controls.Add(this.panelMain);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "frmRequestAdd";
		base.Padding = new System.Windows.Forms.Padding(20, 70, 20, 20);
		this.Text = "5S QA System * REQUEST";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmRequestAdd_FormClosing);
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmRequestAdd_FormClosed);
		base.Load += new System.EventHandler(frmRequestAdd_Load);
		base.Shown += new System.EventHandler(frmRequestAdd_Shown);
		((System.ComponentModel.ISupportInitialize)this.ptbImage).EndInit();
		this.panelMain.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.tableLayoutPanel2.ResumeLayout(false);
		this.tableLayoutPanel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvMain).EndInit();
		this.tableLayoutPanel1.ResumeLayout(false);
		this.tableLayoutPanel1.PerformLayout();
		this.panelViewImage.ResumeLayout(false);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
