using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using _5S_QA_DL.APIContext;
using _5S_QA_Entities.Abstracts;
using _5S_QA_Entities.Models;
using _5S_QA_System.Controls;
using _5S_QA_System.Properties;
using MetroFramework.Forms;
using Newtonsoft.Json;

namespace _5S_QA_System;

public class frmProductGroupAdd : MetroForm
{
	private readonly Form mForm;

	private Guid mId;

	private readonly DataTable mData;

	public bool isClose;

	private readonly bool isAdd;

	private ProductGroupViewModel mGroup;

	private IContainer components = null;

	private Panel panelMain;

	private Button btnConfirm;

	private TableLayoutPanel tableLayoutPanel2;

	private ToolTip toolTipMain;

	private Label lblRevisionHistory;

	private Label lblReleaseDate;

	private Label lblVersion;

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

	private TextBox txtVersion;

	private TextBox txtDescription;

	private TextBox txtName;

	private TextBox txtPreparedBy;

	private Label lblPreparedBy;

	private Label lblApprovedBy;

	private Label lblCheckedBy;

	private TextBox txtCheckedBy;

	private TextBox txtApprovedBy;

	private TextBox txtRevisionHistory;

	private DateTimePicker dtpReleaseDate;

	private Label lblIsActivated;

	public CheckBox cbIsActivated;

	private Label lblChecksheet;

	private Panel panelChecksheet;

	private Button btnChecksheetDelete;

	private TextBox txtChecksheet;

	private Button btnChecksheetFolder;

	private OpenFileDialog openFileDialogMain;

	public frmProductGroupAdd(Form frm, DataTable data, Guid id = default(Guid), bool isadd = true)
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
	}

	private void frmProductGroupAdd_Load(object sender, EventArgs e)
	{
		Init();
	}

	private void frmProductGroupAdd_Shown(object sender, EventArgs e)
	{
		load_Data();
		txtCode.Select();
	}

	private void frmProductGroupAdd_FormClosing(object sender, FormClosingEventArgs e)
	{
		GC.Collect();
	}

	private void frmProductGroupAdd_FormClosed(object sender, FormClosedEventArgs e)
	{
		mData?.Dispose();
		if (!isClose)
		{
			((frmProductGroupView)mForm).load_AllData();
			if (isAdd)
			{
				((frmProductGroupView)mForm).OpenProduct(mGroup);
			}
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

	private void load_AutoComplete()
	{
		txtCode.AutoCompleteCustomSource = Common.getAutoComplete(mData, "Code");
		txtName.AutoCompleteCustomSource = Common.getAutoComplete(mData, "Name");
		txtDescription.AutoCompleteCustomSource = Common.getAutoComplete(mData, "Description");
		txtVersion.AutoCompleteCustomSource = Common.getAutoComplete(mData, "Version");
		txtRevisionHistory.AutoCompleteCustomSource = Common.getAutoComplete(mData, "RevisionHistory");
		txtPreparedBy.AutoCompleteCustomSource = Common.getAutoComplete(mData, "PreparedBy");
		txtCheckedBy.AutoCompleteCustomSource = Common.getAutoComplete(mData, "CheckedBy");
		txtApprovedBy.AutoCompleteCustomSource = Common.getAutoComplete(mData, "ApprovedBy");
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
				txtCode.Text = dataRow["Code"].ToString();
				txtName.Text = dataRow["Name"].ToString();
				txtDescription.Text = dataRow["Description"].ToString();
				txtVersion.Text = dataRow["Version"].ToString();
				txtRevisionHistory.Text = dataRow["RevisionHistory"].ToString();
				txtPreparedBy.Text = dataRow["PreparedBy"].ToString();
				txtCheckedBy.Text = dataRow["CheckedBy"].ToString();
				txtApprovedBy.Text = dataRow["ApprovedBy"].ToString();
				if (!string.IsNullOrEmpty(dataRow["ReleaseDate"].ToString()))
				{
					dtpReleaseDate.Checked = true;
					dtpReleaseDate.Value = DateTime.Parse(dataRow["ReleaseDate"].ToString());
				}
				if (!isAdd)
				{
					cbIsActivated.Checked = (bool)dataRow["IsActivated"];
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
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Expected O, but got Unknown
		//IL_01bd: Expected O, but got Unknown
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
			ProductGroupViewModel val = new ProductGroupViewModel
			{
				Code = txtCode.Text,
				Name = txtName.Text,
				Description = (string.IsNullOrEmpty(txtDescription.Text) ? null : txtDescription.Text),
				Version = (string.IsNullOrEmpty(txtVersion.Text) ? null : txtVersion.Text),
				RevisionHistory = (string.IsNullOrEmpty(txtRevisionHistory.Text) ? null : txtRevisionHistory.Text),
				PreparedBy = (string.IsNullOrEmpty(txtPreparedBy.Text) ? null : txtPreparedBy.Text),
				CheckedBy = (string.IsNullOrEmpty(txtCheckedBy.Text) ? null : txtCheckedBy.Text),
				ApprovedBy = (string.IsNullOrEmpty(txtApprovedBy.Text) ? null : txtApprovedBy.Text)
			};
			((AuditableEntity)val).IsActivated = cbIsActivated.Checked;
			ProductGroupViewModel val2 = val;
			if (dtpReleaseDate.Checked)
			{
				val2.ReleaseDate = dtpReleaseDate.Value;
			}
			if (isAdd || !MessageBox.Show(Common.getTextLanguage(this, "wSureEdit"), Common.getTextLanguage(this, "QUESTION"), MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.No))
			{
				Cursor.Current = Cursors.WaitCursor;
				if (!isAdd)
				{
					((AuditableEntity)(object)val2).Id = mId;
				}
				ResponseDto result = frmLogin.client.SaveAsync(val2, "/api/ProductGroup/Save").Result;
				if (!result.Success)
				{
					throw new Exception(result.Messages.ElementAt(0).Message);
				}
				if (isAdd)
				{
					mGroup = JsonConvert.DeserializeObject<ProductGroupViewModel>(result.Data.ToString());
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

	private void txtNormal_Leave(object sender, EventArgs e)
	{
		TextBox textBox = sender as TextBox;
		textBox.Text = Common.trimSpace(textBox.Text);
	}

	private void btnChecksheetFolder_Click(object sender, EventArgs e)
	{
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
			txtCode.Text = request.ProductCode;
			txtName.Text = request.ProductName;
			txtDescription.Text = request.ProductDescription;
			txtChecksheet.Text = openFileDialogMain.FileName;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_5S_QA_System.frmProductGroupAdd));
		this.panelMain = new System.Windows.Forms.Panel();
		this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
		this.cbIsActivated = new System.Windows.Forms.CheckBox();
		this.lblIsActivated = new System.Windows.Forms.Label();
		this.dtpReleaseDate = new System.Windows.Forms.DateTimePicker();
		this.txtRevisionHistory = new System.Windows.Forms.TextBox();
		this.txtApprovedBy = new System.Windows.Forms.TextBox();
		this.txtCheckedBy = new System.Windows.Forms.TextBox();
		this.lblApprovedBy = new System.Windows.Forms.Label();
		this.lblCheckedBy = new System.Windows.Forms.Label();
		this.txtPreparedBy = new System.Windows.Forms.TextBox();
		this.lblPreparedBy = new System.Windows.Forms.Label();
		this.txtVersion = new System.Windows.Forms.TextBox();
		this.txtCode = new System.Windows.Forms.TextBox();
		this.txtDescription = new System.Windows.Forms.TextBox();
		this.lblModifiedBy = new System.Windows.Forms.Label();
		this.txtName = new System.Windows.Forms.TextBox();
		this.lblCreatedBy = new System.Windows.Forms.Label();
		this.lblModified = new System.Windows.Forms.Label();
		this.lblCreated = new System.Windows.Forms.Label();
		this.lblModifiBy = new System.Windows.Forms.Label();
		this.lblCreateBy = new System.Windows.Forms.Label();
		this.lblModifi = new System.Windows.Forms.Label();
		this.lblCreate = new System.Windows.Forms.Label();
		this.lblRevisionHistory = new System.Windows.Forms.Label();
		this.lblReleaseDate = new System.Windows.Forms.Label();
		this.lblVersion = new System.Windows.Forms.Label();
		this.lblDescription = new System.Windows.Forms.Label();
		this.lblName = new System.Windows.Forms.Label();
		this.lblCode = new System.Windows.Forms.Label();
		this.btnConfirm = new System.Windows.Forms.Button();
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.lblChecksheet = new System.Windows.Forms.Label();
		this.panelChecksheet = new System.Windows.Forms.Panel();
		this.btnChecksheetDelete = new System.Windows.Forms.Button();
		this.txtChecksheet = new System.Windows.Forms.TextBox();
		this.btnChecksheetFolder = new System.Windows.Forms.Button();
		this.openFileDialogMain = new System.Windows.Forms.OpenFileDialog();
		this.panelMain.SuspendLayout();
		this.tableLayoutPanel2.SuspendLayout();
		this.panelChecksheet.SuspendLayout();
		base.SuspendLayout();
		this.panelMain.AutoScroll = true;
		this.panelMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panelMain.Controls.Add(this.tableLayoutPanel2);
		this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panelMain.Location = new System.Drawing.Point(20, 70);
		this.panelMain.Margin = new System.Windows.Forms.Padding(4);
		this.panelMain.Name = "panelMain";
		this.panelMain.Size = new System.Drawing.Size(500, 447);
		this.panelMain.TabIndex = 16;
		this.tableLayoutPanel2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.tableLayoutPanel2.AutoSize = true;
		this.tableLayoutPanel2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		this.tableLayoutPanel2.ColumnCount = 2;
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel2.Controls.Add(this.panelChecksheet, 1, 0);
		this.tableLayoutPanel2.Controls.Add(this.lblChecksheet, 0, 0);
		this.tableLayoutPanel2.Controls.Add(this.cbIsActivated, 1, 10);
		this.tableLayoutPanel2.Controls.Add(this.lblIsActivated, 0, 10);
		this.tableLayoutPanel2.Controls.Add(this.dtpReleaseDate, 1, 5);
		this.tableLayoutPanel2.Controls.Add(this.txtRevisionHistory, 1, 6);
		this.tableLayoutPanel2.Controls.Add(this.txtApprovedBy, 1, 9);
		this.tableLayoutPanel2.Controls.Add(this.txtCheckedBy, 1, 8);
		this.tableLayoutPanel2.Controls.Add(this.lblApprovedBy, 0, 9);
		this.tableLayoutPanel2.Controls.Add(this.lblCheckedBy, 0, 8);
		this.tableLayoutPanel2.Controls.Add(this.txtPreparedBy, 1, 7);
		this.tableLayoutPanel2.Controls.Add(this.lblPreparedBy, 0, 7);
		this.tableLayoutPanel2.Controls.Add(this.txtVersion, 1, 4);
		this.tableLayoutPanel2.Controls.Add(this.txtCode, 1, 1);
		this.tableLayoutPanel2.Controls.Add(this.txtDescription, 1, 3);
		this.tableLayoutPanel2.Controls.Add(this.lblModifiedBy, 1, 15);
		this.tableLayoutPanel2.Controls.Add(this.txtName, 1, 2);
		this.tableLayoutPanel2.Controls.Add(this.lblCreatedBy, 1, 14);
		this.tableLayoutPanel2.Controls.Add(this.lblModified, 1, 13);
		this.tableLayoutPanel2.Controls.Add(this.lblCreated, 1, 12);
		this.tableLayoutPanel2.Controls.Add(this.lblModifiBy, 0, 15);
		this.tableLayoutPanel2.Controls.Add(this.lblCreateBy, 0, 14);
		this.tableLayoutPanel2.Controls.Add(this.lblModifi, 0, 13);
		this.tableLayoutPanel2.Controls.Add(this.lblCreate, 0, 12);
		this.tableLayoutPanel2.Controls.Add(this.lblRevisionHistory, 0, 6);
		this.tableLayoutPanel2.Controls.Add(this.lblReleaseDate, 0, 5);
		this.tableLayoutPanel2.Controls.Add(this.lblVersion, 0, 4);
		this.tableLayoutPanel2.Controls.Add(this.lblDescription, 0, 3);
		this.tableLayoutPanel2.Controls.Add(this.lblName, 0, 2);
		this.tableLayoutPanel2.Controls.Add(this.lblCode, 0, 1);
		this.tableLayoutPanel2.Location = new System.Drawing.Point(4, 4);
		this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4);
		this.tableLayoutPanel2.Name = "tableLayoutPanel2";
		this.tableLayoutPanel2.RowCount = 16;
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.Size = new System.Drawing.Size(490, 437);
		this.tableLayoutPanel2.TabIndex = 15;
		this.cbIsActivated.AutoSize = true;
		this.cbIsActivated.Checked = true;
		this.cbIsActivated.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cbIsActivated.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbIsActivated.Dock = System.Windows.Forms.DockStyle.Fill;
		this.cbIsActivated.Enabled = false;
		this.cbIsActivated.Location = new System.Drawing.Point(113, 294);
		this.cbIsActivated.Name = "cbIsActivated";
		this.cbIsActivated.Size = new System.Drawing.Size(373, 22);
		this.cbIsActivated.TabIndex = 10;
		this.toolTipMain.SetToolTip(this.cbIsActivated, "Select to active");
		this.cbIsActivated.UseVisualStyleBackColor = true;
		this.lblIsActivated.AutoSize = true;
		this.lblIsActivated.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblIsActivated.Location = new System.Drawing.Point(4, 291);
		this.lblIsActivated.Name = "lblIsActivated";
		this.lblIsActivated.Size = new System.Drawing.Size(102, 28);
		this.lblIsActivated.TabIndex = 84;
		this.lblIsActivated.Text = "Is activated";
		this.lblIsActivated.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.dtpReleaseDate.Checked = false;
		this.dtpReleaseDate.Dock = System.Windows.Forms.DockStyle.Fill;
		this.dtpReleaseDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.dtpReleaseDate.Location = new System.Drawing.Point(113, 149);
		this.dtpReleaseDate.Name = "dtpReleaseDate";
		this.dtpReleaseDate.ShowCheckBox = true;
		this.dtpReleaseDate.Size = new System.Drawing.Size(373, 22);
		this.dtpReleaseDate.TabIndex = 5;
		this.toolTipMain.SetToolTip(this.dtpReleaseDate, "Select release date");
		this.txtRevisionHistory.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtRevisionHistory.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtRevisionHistory.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtRevisionHistory.Location = new System.Drawing.Point(113, 178);
		this.txtRevisionHistory.Name = "txtRevisionHistory";
		this.txtRevisionHistory.Size = new System.Drawing.Size(373, 22);
		this.txtRevisionHistory.TabIndex = 6;
		this.toolTipMain.SetToolTip(this.txtRevisionHistory, "Please enter revision history");
		this.txtRevisionHistory.Leave += new System.EventHandler(txtNormal_Leave);
		this.txtApprovedBy.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtApprovedBy.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtApprovedBy.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtApprovedBy.Location = new System.Drawing.Point(113, 265);
		this.txtApprovedBy.Name = "txtApprovedBy";
		this.txtApprovedBy.Size = new System.Drawing.Size(373, 22);
		this.txtApprovedBy.TabIndex = 9;
		this.toolTipMain.SetToolTip(this.txtApprovedBy, "Please enter approved by");
		this.txtApprovedBy.Leave += new System.EventHandler(txtNormal_Leave);
		this.txtCheckedBy.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtCheckedBy.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtCheckedBy.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtCheckedBy.Location = new System.Drawing.Point(113, 236);
		this.txtCheckedBy.Name = "txtCheckedBy";
		this.txtCheckedBy.Size = new System.Drawing.Size(373, 22);
		this.txtCheckedBy.TabIndex = 8;
		this.toolTipMain.SetToolTip(this.txtCheckedBy, "Please enter checked by");
		this.txtCheckedBy.Leave += new System.EventHandler(txtNormal_Leave);
		this.lblApprovedBy.AutoSize = true;
		this.lblApprovedBy.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblApprovedBy.Location = new System.Drawing.Point(4, 262);
		this.lblApprovedBy.Name = "lblApprovedBy";
		this.lblApprovedBy.Size = new System.Drawing.Size(102, 28);
		this.lblApprovedBy.TabIndex = 83;
		this.lblApprovedBy.Text = "Approved by";
		this.lblApprovedBy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblCheckedBy.AutoSize = true;
		this.lblCheckedBy.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCheckedBy.Location = new System.Drawing.Point(4, 233);
		this.lblCheckedBy.Name = "lblCheckedBy";
		this.lblCheckedBy.Size = new System.Drawing.Size(102, 28);
		this.lblCheckedBy.TabIndex = 82;
		this.lblCheckedBy.Text = "Checked by";
		this.lblCheckedBy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtPreparedBy.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtPreparedBy.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtPreparedBy.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtPreparedBy.Location = new System.Drawing.Point(113, 207);
		this.txtPreparedBy.Name = "txtPreparedBy";
		this.txtPreparedBy.Size = new System.Drawing.Size(373, 22);
		this.txtPreparedBy.TabIndex = 7;
		this.toolTipMain.SetToolTip(this.txtPreparedBy, "Please enter prepared by");
		this.txtPreparedBy.Leave += new System.EventHandler(txtNormal_Leave);
		this.lblPreparedBy.AutoSize = true;
		this.lblPreparedBy.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblPreparedBy.Location = new System.Drawing.Point(4, 204);
		this.lblPreparedBy.Name = "lblPreparedBy";
		this.lblPreparedBy.Size = new System.Drawing.Size(102, 28);
		this.lblPreparedBy.TabIndex = 81;
		this.lblPreparedBy.Text = "Prepared by";
		this.lblPreparedBy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtVersion.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtVersion.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtVersion.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtVersion.Location = new System.Drawing.Point(113, 120);
		this.txtVersion.Name = "txtVersion";
		this.txtVersion.Size = new System.Drawing.Size(373, 22);
		this.txtVersion.TabIndex = 4;
		this.toolTipMain.SetToolTip(this.txtVersion, "Please enter version");
		this.txtVersion.Leave += new System.EventHandler(txtNormal_Leave);
		this.txtCode.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtCode.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtCode.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtCode.Location = new System.Drawing.Point(113, 33);
		this.txtCode.Name = "txtCode";
		this.txtCode.Size = new System.Drawing.Size(373, 22);
		this.txtCode.TabIndex = 1;
		this.toolTipMain.SetToolTip(this.txtCode, "Please enter code");
		this.txtCode.Leave += new System.EventHandler(txtNormal_Leave);
		this.txtDescription.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtDescription.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtDescription.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtDescription.Location = new System.Drawing.Point(113, 91);
		this.txtDescription.Name = "txtDescription";
		this.txtDescription.Size = new System.Drawing.Size(373, 22);
		this.txtDescription.TabIndex = 3;
		this.toolTipMain.SetToolTip(this.txtDescription, "Please enter description");
		this.txtDescription.Leave += new System.EventHandler(txtNormal_Leave);
		this.lblModifiedBy.AutoSize = true;
		this.lblModifiedBy.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblModifiedBy.Location = new System.Drawing.Point(113, 408);
		this.lblModifiedBy.Name = "lblModifiedBy";
		this.lblModifiedBy.Size = new System.Drawing.Size(373, 28);
		this.lblModifiedBy.TabIndex = 80;
		this.lblModifiedBy.Text = "...";
		this.lblModifiedBy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.txtName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtName.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtName.Location = new System.Drawing.Point(113, 62);
		this.txtName.Name = "txtName";
		this.txtName.Size = new System.Drawing.Size(373, 22);
		this.txtName.TabIndex = 2;
		this.toolTipMain.SetToolTip(this.txtName, "Please enter name");
		this.txtName.Leave += new System.EventHandler(txtNormal_Leave);
		this.lblCreatedBy.AutoSize = true;
		this.lblCreatedBy.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCreatedBy.Location = new System.Drawing.Point(113, 379);
		this.lblCreatedBy.Name = "lblCreatedBy";
		this.lblCreatedBy.Size = new System.Drawing.Size(373, 28);
		this.lblCreatedBy.TabIndex = 79;
		this.lblCreatedBy.Text = "...";
		this.lblCreatedBy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblModified.AutoSize = true;
		this.lblModified.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblModified.Location = new System.Drawing.Point(113, 350);
		this.lblModified.Name = "lblModified";
		this.lblModified.Size = new System.Drawing.Size(373, 28);
		this.lblModified.TabIndex = 78;
		this.lblModified.Text = "...";
		this.lblModified.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblCreated.AutoSize = true;
		this.lblCreated.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCreated.Location = new System.Drawing.Point(113, 321);
		this.lblCreated.Name = "lblCreated";
		this.lblCreated.Size = new System.Drawing.Size(373, 28);
		this.lblCreated.TabIndex = 77;
		this.lblCreated.Text = "...";
		this.lblCreated.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblModifiBy.AutoSize = true;
		this.lblModifiBy.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblModifiBy.Location = new System.Drawing.Point(4, 408);
		this.lblModifiBy.Name = "lblModifiBy";
		this.lblModifiBy.Size = new System.Drawing.Size(102, 28);
		this.lblModifiBy.TabIndex = 76;
		this.lblModifiBy.Text = "Edited by";
		this.lblModifiBy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblCreateBy.AutoSize = true;
		this.lblCreateBy.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCreateBy.Location = new System.Drawing.Point(4, 379);
		this.lblCreateBy.Name = "lblCreateBy";
		this.lblCreateBy.Size = new System.Drawing.Size(102, 28);
		this.lblCreateBy.TabIndex = 75;
		this.lblCreateBy.Text = "Created by";
		this.lblCreateBy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblModifi.AutoSize = true;
		this.lblModifi.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblModifi.Location = new System.Drawing.Point(4, 350);
		this.lblModifi.Name = "lblModifi";
		this.lblModifi.Size = new System.Drawing.Size(102, 28);
		this.lblModifi.TabIndex = 74;
		this.lblModifi.Text = "Edited date";
		this.lblModifi.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblCreate.AutoSize = true;
		this.lblCreate.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCreate.Location = new System.Drawing.Point(4, 321);
		this.lblCreate.Name = "lblCreate";
		this.lblCreate.Size = new System.Drawing.Size(102, 28);
		this.lblCreate.TabIndex = 73;
		this.lblCreate.Text = "Create date";
		this.lblCreate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblRevisionHistory.AutoSize = true;
		this.lblRevisionHistory.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblRevisionHistory.Location = new System.Drawing.Point(4, 175);
		this.lblRevisionHistory.Name = "lblRevisionHistory";
		this.lblRevisionHistory.Size = new System.Drawing.Size(102, 28);
		this.lblRevisionHistory.TabIndex = 25;
		this.lblRevisionHistory.Text = "Revision history";
		this.lblRevisionHistory.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblReleaseDate.AutoSize = true;
		this.lblReleaseDate.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblReleaseDate.Location = new System.Drawing.Point(4, 146);
		this.lblReleaseDate.Name = "lblReleaseDate";
		this.lblReleaseDate.Size = new System.Drawing.Size(102, 28);
		this.lblReleaseDate.TabIndex = 24;
		this.lblReleaseDate.Text = "Release date";
		this.lblReleaseDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblVersion.AutoSize = true;
		this.lblVersion.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblVersion.Location = new System.Drawing.Point(4, 117);
		this.lblVersion.Name = "lblVersion";
		this.lblVersion.Size = new System.Drawing.Size(102, 28);
		this.lblVersion.TabIndex = 23;
		this.lblVersion.Text = "Version";
		this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblDescription.AutoSize = true;
		this.lblDescription.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblDescription.Location = new System.Drawing.Point(4, 88);
		this.lblDescription.Name = "lblDescription";
		this.lblDescription.Size = new System.Drawing.Size(102, 28);
		this.lblDescription.TabIndex = 22;
		this.lblDescription.Text = "Description";
		this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblName.AutoSize = true;
		this.lblName.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblName.Location = new System.Drawing.Point(4, 59);
		this.lblName.Name = "lblName";
		this.lblName.Size = new System.Drawing.Size(102, 28);
		this.lblName.TabIndex = 21;
		this.lblName.Text = "Name";
		this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblCode.AutoSize = true;
		this.lblCode.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCode.Location = new System.Drawing.Point(4, 30);
		this.lblCode.Name = "lblCode";
		this.lblCode.Size = new System.Drawing.Size(102, 28);
		this.lblCode.TabIndex = 20;
		this.lblCode.Text = "Code";
		this.lblCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.btnConfirm.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnConfirm.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.btnConfirm.FlatAppearance.BorderSize = 0;
		this.btnConfirm.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
		this.btnConfirm.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
		this.btnConfirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnConfirm.Location = new System.Drawing.Point(20, 517);
		this.btnConfirm.Name = "btnConfirm";
		this.btnConfirm.Size = new System.Drawing.Size(500, 28);
		this.btnConfirm.TabIndex = 1;
		this.btnConfirm.Text = "Confirm";
		this.toolTipMain.SetToolTip(this.btnConfirm, "Confirm");
		this.btnConfirm.UseVisualStyleBackColor = true;
		this.btnConfirm.Click += new System.EventHandler(btnComfirm_Click);
		this.lblChecksheet.AutoSize = true;
		this.lblChecksheet.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblChecksheet.Location = new System.Drawing.Point(4, 1);
		this.lblChecksheet.Name = "lblChecksheet";
		this.lblChecksheet.Size = new System.Drawing.Size(102, 28);
		this.lblChecksheet.TabIndex = 85;
		this.lblChecksheet.Text = "Checksheet";
		this.lblChecksheet.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.panelChecksheet.Controls.Add(this.btnChecksheetDelete);
		this.panelChecksheet.Controls.Add(this.txtChecksheet);
		this.panelChecksheet.Controls.Add(this.btnChecksheetFolder);
		this.panelChecksheet.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panelChecksheet.Location = new System.Drawing.Point(113, 4);
		this.panelChecksheet.Name = "panelChecksheet";
		this.panelChecksheet.Size = new System.Drawing.Size(373, 22);
		this.panelChecksheet.TabIndex = 0;
		this.panelChecksheet.TabStop = true;
		this.btnChecksheetDelete.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnChecksheetDelete.BackColor = System.Drawing.SystemColors.Control;
		this.btnChecksheetDelete.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnChecksheetDelete.FlatAppearance.BorderSize = 0;
		this.btnChecksheetDelete.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
		this.btnChecksheetDelete.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.AppWorkspace;
		this.btnChecksheetDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnChecksheetDelete.Image = _5S_QA_System.Properties.Resources.cancel;
		this.btnChecksheetDelete.Location = new System.Drawing.Point(329, 1);
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
		this.txtChecksheet.Size = new System.Drawing.Size(351, 22);
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
		this.btnChecksheetFolder.Location = new System.Drawing.Point(351, 0);
		this.btnChecksheetFolder.Name = "btnChecksheetFolder";
		this.btnChecksheetFolder.Size = new System.Drawing.Size(22, 22);
		this.btnChecksheetFolder.TabIndex = 1;
		this.toolTipMain.SetToolTip(this.btnChecksheetFolder, "Open folder to select checksheet");
		this.btnChecksheetFolder.UseVisualStyleBackColor = false;
		this.btnChecksheetFolder.Click += new System.EventHandler(btnChecksheetFolder_Click);
		this.openFileDialogMain.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png, *.bmp)| *.jpg; *.jpeg; *.jpe; *.jfif; *.png; *.bmp";
		this.openFileDialogMain.Title = "Please select a picture";
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(540, 565);
		base.Controls.Add(this.panelMain);
		base.Controls.Add(this.btnConfirm);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "frmProductGroupAdd";
		base.Padding = new System.Windows.Forms.Padding(20, 70, 20, 20);
		this.Text = "5S QA System * PRODUCT";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmProductGroupAdd_FormClosing);
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmProductGroupAdd_FormClosed);
		base.Load += new System.EventHandler(frmProductGroupAdd_Load);
		base.Shown += new System.EventHandler(frmProductGroupAdd_Shown);
		this.panelMain.ResumeLayout(false);
		this.panelMain.PerformLayout();
		this.tableLayoutPanel2.ResumeLayout(false);
		this.tableLayoutPanel2.PerformLayout();
		this.panelChecksheet.ResumeLayout(false);
		this.panelChecksheet.PerformLayout();
		base.ResumeLayout(false);
	}
}
