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

public class frmCalibrationAdd : MetroForm
{
	private readonly Form mForm;

	private Guid mId;

	private readonly Guid mIdParent;

	private readonly DataTable mData;

	public bool isClose;

	private readonly bool isAdd;

	private string mFileName;

	private IContainer components = null;

	private Panel panelMain;

	private Button btnConfirm;

	private TableLayoutPanel tableLayoutPanel2;

	private ToolTip toolTipMain;

	private Label lblFile;

	private Label lblPeriod;

	private Label lblCalDate;

	private Label lblStaff;

	private Label lblType;

	private Label lblCalibrationNo;

	private Label lblCreate;

	private Label lblModifi;

	private Label lblCreateBy;

	private Label lblModifiBy;

	private Label lblCreated;

	private Label lblModified;

	private Label lblCreatedBy;

	private Label lblModifiedBy;

	private TextBox txtCalibrationNo;

	private TextBox txtStaff;

	private ComboBox cbbType;

	private ComboBox cbbPeriod;

	private DateTimePicker dtpCalDate;

	private Panel panel1;

	private Button btnFileDelete;

	private TextBox txtFile;

	private Button btnFileFolder;

	private OpenFileDialog openFileDialogMain;

	private Panel panel2;

	private Button btnTypeAdd;

	private TextBox txtCompany;

	private Label lblCompany;

	public frmCalibrationAdd(Form frm, DataTable data, Guid idparent, Guid id = default(Guid), bool isadd = true)
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
	}

	private void frmCalibrationAdd_Load(object sender, EventArgs e)
	{
		Init();
	}

	private void frmCalibrationAdd_Shown(object sender, EventArgs e)
	{
		if (isAdd)
		{
			cbbPeriod.SelectedIndex = 1;
		}
		load_cbbType();
		load_Data();
		txtCalibrationNo.Select();
	}

	private void frmCalibrationAdd_FormClosing(object sender, FormClosingEventArgs e)
	{
		GC.Collect();
	}

	private void frmCalibrationAdd_FormClosed(object sender, FormClosedEventArgs e)
	{
		mData?.Dispose();
		List<Type> list = new List<Type>();
		list.Add(typeof(frmSubView));
		Common.closeForm(list);
		if (!isClose)
		{
			((frmCalibrationView)mForm).isClose = false;
			((frmCalibrationView)mForm).load_AllData();
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

	public void load_cbbType()
	{
		try
		{
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "TypeId=@0";
			queryArgs.PredicateParameters = new string[1] { "AC5FA815-C9EE-4807-A852-30A5EA5AB0A3" };
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

	private void load_AutoComplete()
	{
		txtCalibrationNo.AutoCompleteCustomSource = Common.getAutoComplete(mData, "CalibrationNo");
		txtCompany.AutoCompleteCustomSource = Common.getAutoComplete(mData, "Company");
		txtStaff.AutoCompleteCustomSource = Common.getAutoComplete(mData, "Staff");
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
				txtCalibrationNo.Text = dataRow["CalibrationNo"].ToString();
				if (dataRow["TypeId"] != null)
				{
					cbbType.SelectedValue = dataRow["TypeId"];
				}
				else
				{
					cbbType.SelectedIndex = -1;
				}
				txtCompany.Text = dataRow["Company"].ToString();
				txtStaff.Text = dataRow["Staff"].ToString();
				dtpCalDate.Text = dataRow["CalDate"].ToString();
				cbbPeriod.Text = dataRow["Period"].ToString();
				if (!isAdd)
				{
					txtFile.Text = dataRow["File"].ToString();
					mFileName = dataRow["File"].ToString();
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

	private void btnComfirm_Click(object sender, EventArgs e)
	{
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Expected O, but got Unknown
		//IL_018f: Expected O, but got Unknown
		Cursor.Current = Cursors.WaitCursor;
		try
		{
			if (string.IsNullOrEmpty(txtCalibrationNo.Text))
			{
				MessageBox.Show(Common.getTextLanguage(this, "wCalibrationNo"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				txtCalibrationNo.Focus();
				return;
			}
			if (string.IsNullOrEmpty(cbbType.Text))
			{
				MessageBox.Show(Common.getTextLanguage(this, "wType"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				cbbType.Focus();
				return;
			}
			if (string.IsNullOrEmpty(dtpCalDate.Text))
			{
				MessageBox.Show(Common.getTextLanguage(this, "wCalDate"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				dtpCalDate.Focus();
				return;
			}
			if (string.IsNullOrEmpty(cbbPeriod.Text))
			{
				MessageBox.Show(Common.getTextLanguage(this, "wPeriod"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				cbbPeriod.Focus();
				return;
			}
			CalibrationViewModel val = new CalibrationViewModel
			{
				CalibrationNo = txtCalibrationNo.Text,
				MachineId = mIdParent,
				CalDate = dtpCalDate.Value,
				Period = int.Parse(cbbPeriod.Text)
			};
			((AuditableEntity)val).IsActivated = true;
			CalibrationViewModel val2 = val;
			val2.ExpDate = val2.CalDate.Value.AddMonths(val2.Period.Value);
			if (!string.IsNullOrEmpty(txtCompany.Text))
			{
				val2.Company = txtCompany.Text;
			}
			if (!string.IsNullOrEmpty(txtStaff.Text))
			{
				val2.Staff = txtStaff.Text;
			}
			if (!string.IsNullOrEmpty(cbbType.Text))
			{
				val2.TypeId = Guid.Parse(cbbType.SelectedValue.ToString());
			}
			if (!isAdd && MessageBox.Show(Common.getTextLanguage(this, "wSureEdit"), Common.getTextLanguage(this, "QUESTION"), MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.No))
			{
				return;
			}
			Cursor.Current = Cursors.WaitCursor;
			if (!isAdd)
			{
				val2.File = mFileName;
				((AuditableEntity)(object)val2).Id = mId;
			}
			ResponseDto result = frmLogin.client.SaveAsync(val2, "/api/Calibration/Save").Result;
			if (!result.Success)
			{
				throw new Exception(result.Messages.ElementAt(0).Message);
			}
			if (isAdd)
			{
				CalibrationViewModel val3 = JsonConvert.DeserializeObject<CalibrationViewModel>(result.Data.ToString());
				mId = ((AuditableEntity)(object)val3).Id;
				if (File.Exists(txtFile.Text))
				{
					using FileStream data = File.Open(txtFile.Text, FileMode.Open);
					FileParameter file = new FileParameter(data, txtFile.Text);
					ResponseDto result2 = frmLogin.client.ImportAsync(((AuditableEntity)(object)val3).Id, file, "/api/Calibration/UpdateFile/{id}").Result;
				}
			}
			else if (File.Exists(txtFile.Text))
			{
				using FileStream data2 = File.Open(txtFile.Text, FileMode.Open);
				FileParameter file2 = new FileParameter(data2, txtFile.Text);
				ResponseDto result3 = frmLogin.client.ImportAsync(mId, file2, "/api/Calibration/UpdateFile/{id}").Result;
			}
			else if (string.IsNullOrEmpty(txtFile.Text))
			{
				Stream data3 = new MemoryStream();
				FileParameter file3 = new FileParameter(data3);
				ResponseDto result4 = frmLogin.client.ImportAsync(mId, file3, "/api/Calibration/UpdateFile/{id}").Result;
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

	private void btnFileDelete_Click(object sender, EventArgs e)
	{
		txtFile.Text = "";
	}

	private void txtFile_TextChanged(object sender, EventArgs e)
	{
		TextBox textBox = sender as TextBox;
		if (string.IsNullOrEmpty(textBox.Text))
		{
			btnFileDelete.Visible = false;
		}
		else
		{
			btnFileDelete.Visible = true;
		}
	}

	private void btnTypeAdd_Click(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		List<Type> list = new List<Type>();
		list.Add(typeof(frmSubView));
		Common.closeForm(list);
		new frmSubView(this, (FormType)12).Show();
	}

	private void btnFileFolder_Click(object sender, EventArgs e)
	{
		if (openFileDialogMain.ShowDialog().Equals(DialogResult.OK))
		{
			txtFile.Text = openFileDialogMain.FileName;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_5S_QA_System.frmCalibrationAdd));
		this.panelMain = new System.Windows.Forms.Panel();
		this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
		this.panel1 = new System.Windows.Forms.Panel();
		this.btnFileDelete = new System.Windows.Forms.Button();
		this.txtFile = new System.Windows.Forms.TextBox();
		this.btnFileFolder = new System.Windows.Forms.Button();
		this.dtpCalDate = new System.Windows.Forms.DateTimePicker();
		this.cbbPeriod = new System.Windows.Forms.ComboBox();
		this.cbbType = new System.Windows.Forms.ComboBox();
		this.txtCalibrationNo = new System.Windows.Forms.TextBox();
		this.lblModifiedBy = new System.Windows.Forms.Label();
		this.txtStaff = new System.Windows.Forms.TextBox();
		this.lblCreatedBy = new System.Windows.Forms.Label();
		this.lblModified = new System.Windows.Forms.Label();
		this.lblCreated = new System.Windows.Forms.Label();
		this.lblModifiBy = new System.Windows.Forms.Label();
		this.lblCreateBy = new System.Windows.Forms.Label();
		this.lblModifi = new System.Windows.Forms.Label();
		this.lblCreate = new System.Windows.Forms.Label();
		this.lblFile = new System.Windows.Forms.Label();
		this.lblPeriod = new System.Windows.Forms.Label();
		this.lblCalDate = new System.Windows.Forms.Label();
		this.lblStaff = new System.Windows.Forms.Label();
		this.lblType = new System.Windows.Forms.Label();
		this.lblCalibrationNo = new System.Windows.Forms.Label();
		this.btnConfirm = new System.Windows.Forms.Button();
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.openFileDialogMain = new System.Windows.Forms.OpenFileDialog();
		this.panel2 = new System.Windows.Forms.Panel();
		this.btnTypeAdd = new System.Windows.Forms.Button();
		this.lblCompany = new System.Windows.Forms.Label();
		this.txtCompany = new System.Windows.Forms.TextBox();
		this.panelMain.SuspendLayout();
		this.tableLayoutPanel2.SuspendLayout();
		this.panel1.SuspendLayout();
		this.panel2.SuspendLayout();
		base.SuspendLayout();
		this.panelMain.AutoScroll = true;
		this.panelMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panelMain.Controls.Add(this.tableLayoutPanel2);
		this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panelMain.Location = new System.Drawing.Point(20, 70);
		this.panelMain.Margin = new System.Windows.Forms.Padding(4);
		this.panelMain.Name = "panelMain";
		this.panelMain.Size = new System.Drawing.Size(500, 330);
		this.panelMain.TabIndex = 16;
		this.tableLayoutPanel2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.tableLayoutPanel2.AutoSize = true;
		this.tableLayoutPanel2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		this.tableLayoutPanel2.ColumnCount = 2;
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel2.Controls.Add(this.txtCompany, 1, 2);
		this.tableLayoutPanel2.Controls.Add(this.lblCompany, 0, 2);
		this.tableLayoutPanel2.Controls.Add(this.panel2, 1, 1);
		this.tableLayoutPanel2.Controls.Add(this.panel1, 1, 6);
		this.tableLayoutPanel2.Controls.Add(this.dtpCalDate, 1, 4);
		this.tableLayoutPanel2.Controls.Add(this.cbbPeriod, 1, 5);
		this.tableLayoutPanel2.Controls.Add(this.txtCalibrationNo, 1, 0);
		this.tableLayoutPanel2.Controls.Add(this.lblModifiedBy, 1, 11);
		this.tableLayoutPanel2.Controls.Add(this.txtStaff, 1, 3);
		this.tableLayoutPanel2.Controls.Add(this.lblCreatedBy, 1, 10);
		this.tableLayoutPanel2.Controls.Add(this.lblModified, 1, 9);
		this.tableLayoutPanel2.Controls.Add(this.lblCreated, 1, 8);
		this.tableLayoutPanel2.Controls.Add(this.lblModifiBy, 0, 11);
		this.tableLayoutPanel2.Controls.Add(this.lblCreateBy, 0, 10);
		this.tableLayoutPanel2.Controls.Add(this.lblModifi, 0, 9);
		this.tableLayoutPanel2.Controls.Add(this.lblCreate, 0, 8);
		this.tableLayoutPanel2.Controls.Add(this.lblFile, 0, 6);
		this.tableLayoutPanel2.Controls.Add(this.lblPeriod, 0, 5);
		this.tableLayoutPanel2.Controls.Add(this.lblCalDate, 0, 4);
		this.tableLayoutPanel2.Controls.Add(this.lblStaff, 0, 3);
		this.tableLayoutPanel2.Controls.Add(this.lblType, 0, 1);
		this.tableLayoutPanel2.Controls.Add(this.lblCalibrationNo, 0, 0);
		this.tableLayoutPanel2.Location = new System.Drawing.Point(4, 4);
		this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4);
		this.tableLayoutPanel2.Name = "tableLayoutPanel2";
		this.tableLayoutPanel2.RowCount = 12;
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.Size = new System.Drawing.Size(490, 321);
		this.tableLayoutPanel2.TabIndex = 15;
		this.panel1.Controls.Add(this.btnFileDelete);
		this.panel1.Controls.Add(this.txtFile);
		this.panel1.Controls.Add(this.btnFileFolder);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(112, 178);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(374, 22);
		this.panel1.TabIndex = 82;
		this.panel1.TabStop = true;
		this.btnFileDelete.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnFileDelete.BackColor = System.Drawing.SystemColors.Control;
		this.btnFileDelete.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnFileDelete.FlatAppearance.BorderSize = 0;
		this.btnFileDelete.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
		this.btnFileDelete.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.AppWorkspace;
		this.btnFileDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnFileDelete.Image = _5S_QA_System.Properties.Resources.cancel;
		this.btnFileDelete.Location = new System.Drawing.Point(330, 1);
		this.btnFileDelete.Margin = new System.Windows.Forms.Padding(0);
		this.btnFileDelete.Name = "btnFileDelete";
		this.btnFileDelete.Size = new System.Drawing.Size(20, 20);
		this.btnFileDelete.TabIndex = 3;
		this.btnFileDelete.TabStop = false;
		this.toolTipMain.SetToolTip(this.btnFileDelete, "Clear file url");
		this.btnFileDelete.UseVisualStyleBackColor = false;
		this.btnFileDelete.Visible = false;
		this.btnFileDelete.Click += new System.EventHandler(btnFileDelete_Click);
		this.txtFile.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtFile.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtFile.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtFile.Enabled = false;
		this.txtFile.Location = new System.Drawing.Point(0, 0);
		this.txtFile.Name = "txtFile";
		this.txtFile.Size = new System.Drawing.Size(352, 22);
		this.txtFile.TabIndex = 1;
		this.txtFile.TabStop = false;
		this.toolTipMain.SetToolTip(this.txtFile, "Please enter file url");
		this.txtFile.TextChanged += new System.EventHandler(txtFile_TextChanged);
		this.btnFileFolder.BackColor = System.Drawing.Color.Gainsboro;
		this.btnFileFolder.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnFileFolder.Dock = System.Windows.Forms.DockStyle.Right;
		this.btnFileFolder.FlatAppearance.BorderSize = 0;
		this.btnFileFolder.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
		this.btnFileFolder.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SkyBlue;
		this.btnFileFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnFileFolder.Image = _5S_QA_System.Properties.Resources.folder;
		this.btnFileFolder.Location = new System.Drawing.Point(352, 0);
		this.btnFileFolder.Name = "btnFileFolder";
		this.btnFileFolder.Size = new System.Drawing.Size(22, 22);
		this.btnFileFolder.TabIndex = 2;
		this.toolTipMain.SetToolTip(this.btnFileFolder, "Open folder to select file");
		this.btnFileFolder.UseVisualStyleBackColor = false;
		this.btnFileFolder.Click += new System.EventHandler(btnFileFolder_Click);
		this.dtpCalDate.Dock = System.Windows.Forms.DockStyle.Fill;
		this.dtpCalDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.dtpCalDate.Location = new System.Drawing.Point(112, 120);
		this.dtpCalDate.Name = "dtpCalDate";
		this.dtpCalDate.Size = new System.Drawing.Size(374, 22);
		this.dtpCalDate.TabIndex = 81;
		this.toolTipMain.SetToolTip(this.dtpCalDate, "Select or enter calibration date");
		this.cbbPeriod.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cbbPeriod.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cbbPeriod.Dock = System.Windows.Forms.DockStyle.Fill;
		this.cbbPeriod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbbPeriod.FormattingEnabled = true;
		this.cbbPeriod.Items.AddRange(new object[6] { "6", "12", "24", "36", "48", "60" });
		this.cbbPeriod.Location = new System.Drawing.Point(112, 148);
		this.cbbPeriod.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.cbbPeriod.Name = "cbbPeriod";
		this.cbbPeriod.Size = new System.Drawing.Size(374, 24);
		this.cbbPeriod.TabIndex = 5;
		this.toolTipMain.SetToolTip(this.cbbPeriod, "Select period");
		this.cbbType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cbbType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cbbType.Dock = System.Windows.Forms.DockStyle.Fill;
		this.cbbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbbType.FormattingEnabled = true;
		this.cbbType.Location = new System.Drawing.Point(0, 0);
		this.cbbType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.cbbType.Name = "cbbType";
		this.cbbType.Size = new System.Drawing.Size(350, 24);
		this.cbbType.TabIndex = 1;
		this.toolTipMain.SetToolTip(this.cbbType, "Select type");
		this.txtCalibrationNo.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtCalibrationNo.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtCalibrationNo.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtCalibrationNo.Location = new System.Drawing.Point(112, 4);
		this.txtCalibrationNo.Name = "txtCalibrationNo";
		this.txtCalibrationNo.Size = new System.Drawing.Size(374, 22);
		this.txtCalibrationNo.TabIndex = 1;
		this.toolTipMain.SetToolTip(this.txtCalibrationNo, "Please enter calibration no");
		this.txtCalibrationNo.Leave += new System.EventHandler(txtNormal_Leave);
		this.lblModifiedBy.AutoSize = true;
		this.lblModifiedBy.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblModifiedBy.Location = new System.Drawing.Point(112, 292);
		this.lblModifiedBy.Name = "lblModifiedBy";
		this.lblModifiedBy.Size = new System.Drawing.Size(374, 28);
		this.lblModifiedBy.TabIndex = 80;
		this.lblModifiedBy.Text = "...";
		this.lblModifiedBy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.txtStaff.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtStaff.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtStaff.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtStaff.Location = new System.Drawing.Point(112, 91);
		this.txtStaff.Name = "txtStaff";
		this.txtStaff.Size = new System.Drawing.Size(374, 22);
		this.txtStaff.TabIndex = 3;
		this.toolTipMain.SetToolTip(this.txtStaff, "Please enter staff");
		this.txtStaff.Leave += new System.EventHandler(txtNormal_Leave);
		this.lblCreatedBy.AutoSize = true;
		this.lblCreatedBy.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCreatedBy.Location = new System.Drawing.Point(112, 263);
		this.lblCreatedBy.Name = "lblCreatedBy";
		this.lblCreatedBy.Size = new System.Drawing.Size(374, 28);
		this.lblCreatedBy.TabIndex = 79;
		this.lblCreatedBy.Text = "...";
		this.lblCreatedBy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblModified.AutoSize = true;
		this.lblModified.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblModified.Location = new System.Drawing.Point(112, 234);
		this.lblModified.Name = "lblModified";
		this.lblModified.Size = new System.Drawing.Size(374, 28);
		this.lblModified.TabIndex = 78;
		this.lblModified.Text = "...";
		this.lblModified.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblCreated.AutoSize = true;
		this.lblCreated.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCreated.Location = new System.Drawing.Point(112, 205);
		this.lblCreated.Name = "lblCreated";
		this.lblCreated.Size = new System.Drawing.Size(374, 28);
		this.lblCreated.TabIndex = 77;
		this.lblCreated.Text = "...";
		this.lblCreated.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblModifiBy.AutoSize = true;
		this.lblModifiBy.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblModifiBy.Location = new System.Drawing.Point(4, 292);
		this.lblModifiBy.Name = "lblModifiBy";
		this.lblModifiBy.Size = new System.Drawing.Size(101, 28);
		this.lblModifiBy.TabIndex = 76;
		this.lblModifiBy.Text = "Edited by";
		this.lblModifiBy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblCreateBy.AutoSize = true;
		this.lblCreateBy.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCreateBy.Location = new System.Drawing.Point(4, 263);
		this.lblCreateBy.Name = "lblCreateBy";
		this.lblCreateBy.Size = new System.Drawing.Size(101, 28);
		this.lblCreateBy.TabIndex = 75;
		this.lblCreateBy.Text = "Created by";
		this.lblCreateBy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblModifi.AutoSize = true;
		this.lblModifi.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblModifi.Location = new System.Drawing.Point(4, 234);
		this.lblModifi.Name = "lblModifi";
		this.lblModifi.Size = new System.Drawing.Size(101, 28);
		this.lblModifi.TabIndex = 74;
		this.lblModifi.Text = "Edited date";
		this.lblModifi.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblCreate.AutoSize = true;
		this.lblCreate.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCreate.Location = new System.Drawing.Point(4, 205);
		this.lblCreate.Name = "lblCreate";
		this.lblCreate.Size = new System.Drawing.Size(101, 28);
		this.lblCreate.TabIndex = 73;
		this.lblCreate.Text = "Create date";
		this.lblCreate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblFile.AutoSize = true;
		this.lblFile.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblFile.Location = new System.Drawing.Point(4, 175);
		this.lblFile.Name = "lblFile";
		this.lblFile.Size = new System.Drawing.Size(101, 28);
		this.lblFile.TabIndex = 25;
		this.lblFile.Text = "File";
		this.lblFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblPeriod.AutoSize = true;
		this.lblPeriod.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblPeriod.Location = new System.Drawing.Point(4, 146);
		this.lblPeriod.Name = "lblPeriod";
		this.lblPeriod.Size = new System.Drawing.Size(101, 28);
		this.lblPeriod.TabIndex = 24;
		this.lblPeriod.Text = "Period";
		this.lblPeriod.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblCalDate.AutoSize = true;
		this.lblCalDate.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCalDate.Location = new System.Drawing.Point(4, 117);
		this.lblCalDate.Name = "lblCalDate";
		this.lblCalDate.Size = new System.Drawing.Size(101, 28);
		this.lblCalDate.TabIndex = 22;
		this.lblCalDate.Text = "Calibration date";
		this.lblCalDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblStaff.AutoSize = true;
		this.lblStaff.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblStaff.Location = new System.Drawing.Point(4, 88);
		this.lblStaff.Name = "lblStaff";
		this.lblStaff.Size = new System.Drawing.Size(101, 28);
		this.lblStaff.TabIndex = 21;
		this.lblStaff.Text = "Staff";
		this.lblStaff.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblType.AutoSize = true;
		this.lblType.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblType.Location = new System.Drawing.Point(4, 30);
		this.lblType.Name = "lblType";
		this.lblType.Size = new System.Drawing.Size(101, 28);
		this.lblType.TabIndex = 20;
		this.lblType.Text = "Type";
		this.lblType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblCalibrationNo.AutoSize = true;
		this.lblCalibrationNo.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCalibrationNo.Location = new System.Drawing.Point(4, 1);
		this.lblCalibrationNo.Name = "lblCalibrationNo";
		this.lblCalibrationNo.Size = new System.Drawing.Size(101, 28);
		this.lblCalibrationNo.TabIndex = 19;
		this.lblCalibrationNo.Text = "Calibration no";
		this.lblCalibrationNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.btnConfirm.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnConfirm.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.btnConfirm.FlatAppearance.BorderSize = 0;
		this.btnConfirm.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
		this.btnConfirm.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
		this.btnConfirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnConfirm.Location = new System.Drawing.Point(20, 400);
		this.btnConfirm.Name = "btnConfirm";
		this.btnConfirm.Size = new System.Drawing.Size(500, 28);
		this.btnConfirm.TabIndex = 1;
		this.btnConfirm.Text = "Confirm";
		this.toolTipMain.SetToolTip(this.btnConfirm, "Confirm");
		this.btnConfirm.UseVisualStyleBackColor = true;
		this.btnConfirm.Click += new System.EventHandler(btnComfirm_Click);
		this.openFileDialogMain.Filter = "All files (*.*)| *.*";
		this.openFileDialogMain.Title = "Please select a file";
		this.panel2.Controls.Add(this.cbbType);
		this.panel2.Controls.Add(this.btnTypeAdd);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(112, 32);
		this.panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(374, 24);
		this.panel2.TabIndex = 2;
		this.panel2.TabStop = true;
		this.btnTypeAdd.BackColor = System.Drawing.Color.Gainsboro;
		this.btnTypeAdd.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnTypeAdd.Dock = System.Windows.Forms.DockStyle.Right;
		this.btnTypeAdd.FlatAppearance.BorderSize = 0;
		this.btnTypeAdd.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
		this.btnTypeAdd.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SkyBlue;
		this.btnTypeAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnTypeAdd.Location = new System.Drawing.Point(350, 0);
		this.btnTypeAdd.Name = "btnTypeAdd";
		this.btnTypeAdd.Size = new System.Drawing.Size(24, 24);
		this.btnTypeAdd.TabIndex = 2;
		this.btnTypeAdd.Text = "+";
		this.toolTipMain.SetToolTip(this.btnTypeAdd, "Goto manage type");
		this.btnTypeAdd.UseVisualStyleBackColor = false;
		this.btnTypeAdd.Click += new System.EventHandler(btnTypeAdd_Click);
		this.lblCompany.AutoSize = true;
		this.lblCompany.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCompany.Location = new System.Drawing.Point(4, 59);
		this.lblCompany.Name = "lblCompany";
		this.lblCompany.Size = new System.Drawing.Size(101, 28);
		this.lblCompany.TabIndex = 83;
		this.lblCompany.Text = "Company";
		this.lblCompany.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtCompany.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtCompany.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtCompany.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtCompany.Location = new System.Drawing.Point(112, 62);
		this.txtCompany.Name = "txtCompany";
		this.txtCompany.Size = new System.Drawing.Size(374, 22);
		this.txtCompany.TabIndex = 84;
		this.toolTipMain.SetToolTip(this.txtCompany, "Please enter company");
		this.txtCompany.Leave += new System.EventHandler(txtNormal_Leave);
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(540, 448);
		base.Controls.Add(this.panelMain);
		base.Controls.Add(this.btnConfirm);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "frmCalibrationAdd";
		base.Padding = new System.Windows.Forms.Padding(20, 70, 20, 20);
		this.Text = "5S QA System * CALIBRATION";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmCalibrationAdd_FormClosing);
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmCalibrationAdd_FormClosed);
		base.Load += new System.EventHandler(frmCalibrationAdd_Load);
		base.Shown += new System.EventHandler(frmCalibrationAdd_Shown);
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
