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
using _5S_QA_Entities.Models;
using _5S_QA_System.Controls;
using _5S_QA_System.Properties;
using MetroFramework.Forms;
using Newtonsoft.Json;
using OfficeOpenXml;

namespace _5S_QA_System;

public class frmTemplateAdd : MetroForm
{
	private readonly Form mForm;

	private string mRemember;

	public bool isClose;

	private IContainer components = null;

	private OpenFileDialog openFileDialogMain;

	private ToolTip toolTipMain;

	private TableLayoutPanel tableLayoutPanel2;

	private ComboBox cbbLimit;

	private Panel panelMain;

	private Button btnConfirm;

	private Label lblCode;

	private Label lblTemplate;

	private Label lblLimit;

	private Label lblName;

	private Label lblDescription;

	private TextBox txtCode;

	private TextBox txtName;

	private TextBox txtDescription;

	private Panel panel1;

	private Button btnTemplateDelete;

	private TextBox txtTemplate;

	private Button btnTemplateFolder;

	public frmTemplateAdd(Form frm, string product, string description, string limit)
	{
		InitializeComponent();
		Common.setControls(this, toolTipMain);
		mForm = frm;
		isClose = true;
		txtName.Text = product;
		txtDescription.Text = description;
		cbbLimit.Text = limit;
	}

	private void frmTemplateAdd_Load(object sender, EventArgs e)
	{
		Init();
	}

	private void frmTemplateAdd_Shown(object sender, EventArgs e)
	{
		load_Data();
		txtCode.Select();
	}

	private void frmTemplateAdd_FormClosing(object sender, FormClosingEventArgs e)
	{
		GC.Collect();
	}

	private void frmTemplateAdd_FormClosed(object sender, FormClosedEventArgs e)
	{
		if (!isClose && mForm != null)
		{
			((frmProductAdd)mForm).load_cbbTemplate();
		}
	}

	private void Init()
	{
		btnTemplateDelete.Visible = false;
	}

	private void load_Data()
	{
		txtCode.Text = set_Code();
	}

	private string set_Code()
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
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_0365: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Expected O, but got Unknown
		//IL_037a: Expected O, but got Unknown
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Expected O, but got Unknown
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
			if (string.IsNullOrEmpty(cbbLimit.Text))
			{
				MessageBox.Show(Common.getTextLanguage(this, "wLimit"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				cbbLimit.Focus();
				return;
			}
			if (string.IsNullOrEmpty(txtTemplate.Text))
			{
				MessageBox.Show(Common.getTextLanguage(this, "wTemplateUrl"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				btnTemplateFolder.Focus();
				return;
			}
			Cursor.Current = Cursors.WaitCursor;
			if (File.Exists(txtTemplate.Text))
			{
				using FileStream newStream = File.Open(txtTemplate.Text, FileMode.Open);
				List<ExportMappingDto> list = new List<ExportMappingDto>();
				using (ExcelPackage excelPackage = new ExcelPackage(newStream))
				{
					if (excelPackage.Workbook.Worksheets.Count < 1)
					{
						throw new Exception(Common.getTextLanguage("frmCreateFile", "IncorrectFormat"));
					}
					foreach (ExcelWorksheet worksheet in excelPackage.Workbook.Worksheets)
					{
						if (worksheet.Dimension == null)
						{
							throw new Exception(Common.getTextLanguage("frmCreateFile", "SheetNull"));
						}
						ExcelRange excelRange = worksheet.Cells[1, 1, worksheet.Dimension.End.Row, worksheet.Dimension.End.Column];
						foreach (ExcelRangeBase item in excelRange)
						{
							if (item.Value != null)
							{
								string text = item.Value.ToString().Trim().ToUpper();
								if (text.StartsWith("[[") && text.EndsWith("]]"))
								{
									list.Add(new ExportMappingDto(item.Address, text, worksheet.Name));
								}
							}
						}
					}
				}
				if (list.Count < 1)
				{
					throw new Exception(Common.getTextLanguage("frmCreateFile", "IncorrectFormat"));
				}
			}
			int result;
			TemplateViewModel val = new TemplateViewModel
			{
				Name = txtName.Text,
				Code = set_Code(),
				Limit = ((!int.TryParse(cbbLimit.Text, out result)) ? 1 : result),
				Type = "Detail"
			};
			((AuditableEntity)val).IsActivated = true;
			TemplateViewModel val2 = val;
			if (!string.IsNullOrEmpty(txtDescription.Text))
			{
				val2.Description = txtDescription.Text;
			}
			ResponseDto result2 = frmLogin.client.SaveAsync(val2, "/api/Template/Save").Result;
			if (!result2.Success)
			{
				throw new Exception(result2.Messages.ElementAt(0).Message);
			}
			TemplateViewModel val3 = JsonConvert.DeserializeObject<TemplateViewModel>(result2.Data.ToString());
			string text2 = txtTemplate.Text;
			if (File.Exists(text2))
			{
				using FileStream data = File.OpenRead(text2);
				FileParameter file = new FileParameter(data, text2);
				ResponseDto result3 = frmLogin.client.ImportAsync(((AuditableEntity)(object)val3).Id, file, "/api/Template/UpdateExcel/{id}").Result;
				if (!result3.Success)
				{
					MessageBox.Show(result3.Messages.First().Message, result3.Messages.First().Code, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
			isClose = false;
			Close();
		}
		catch (Exception ex)
		{
			string text3 = ex.Message;
			string name = Settings.Default.Language.Replace("rb", "Name");
			List<Language> list2 = Common.ReadLanguages("Error");
			foreach (Language item2 in list2)
			{
				object value = ((object)item2).GetType().GetProperty(name).GetValue(item2, null);
				if (value != null)
				{
					string newValue = value.ToString();
					text3 = text3.Replace(item2.Code, newValue);
				}
			}
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
					MessageBox.Show(string.IsNullOrEmpty(text3) ? ex.Message : text3, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			else
			{
				MessageBox.Show(string.IsNullOrEmpty(text3) ? ex.Message : text3, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
	}

	private void txtNormal_Leave(object sender, EventArgs e)
	{
		TextBox textBox = sender as TextBox;
		textBox.Text = Common.trimSpace(textBox.Text);
	}

	private void btnTemplateDelete_Click(object sender, EventArgs e)
	{
		txtTemplate.Text = "";
	}

	private void btnTemplateFolder_Click(object sender, EventArgs e)
	{
		if (openFileDialogMain.ShowDialog().Equals(DialogResult.OK))
		{
			txtTemplate.Text = openFileDialogMain.FileName;
		}
	}

	private void txtTemplateUrl_TextChanged(object sender, EventArgs e)
	{
		TextBox textBox = sender as TextBox;
		if (string.IsNullOrEmpty(textBox.Text))
		{
			btnTemplateDelete.Visible = false;
		}
		else
		{
			btnTemplateDelete.Visible = true;
		}
	}

	private void cbbNormal_Enter(object sender, EventArgs e)
	{
		ComboBox comboBox = sender as ComboBox;
		mRemember = comboBox.Text;
	}

	private void cbbNormalNotNull_Leave(object sender, EventArgs e)
	{
		ComboBox comboBox = sender as ComboBox;
		if (string.IsNullOrEmpty(comboBox.Text))
		{
			comboBox.Text = mRemember;
		}
		comboBox.Text = ((!int.TryParse(comboBox.Text, out var result)) ? "1" : (result.Equals(0) ? "1" : result.ToString()));
		if (int.Parse(comboBox.Text) > int.Parse(string.IsNullOrEmpty(cbbLimit.Text) ? "1" : cbbLimit.Text))
		{
			comboBox.Text = cbbLimit.Text;
		}
	}

	private void cbbLimit_KeyPress(object sender, KeyPressEventArgs e)
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_5S_QA_System.frmTemplateAdd));
		this.openFileDialogMain = new System.Windows.Forms.OpenFileDialog();
		this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
		this.cbbLimit = new System.Windows.Forms.ComboBox();
		this.panelMain = new System.Windows.Forms.Panel();
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.btnConfirm = new System.Windows.Forms.Button();
		this.lblCode = new System.Windows.Forms.Label();
		this.lblDescription = new System.Windows.Forms.Label();
		this.lblName = new System.Windows.Forms.Label();
		this.lblLimit = new System.Windows.Forms.Label();
		this.lblTemplate = new System.Windows.Forms.Label();
		this.txtCode = new System.Windows.Forms.TextBox();
		this.txtName = new System.Windows.Forms.TextBox();
		this.txtDescription = new System.Windows.Forms.TextBox();
		this.panel1 = new System.Windows.Forms.Panel();
		this.btnTemplateDelete = new System.Windows.Forms.Button();
		this.txtTemplate = new System.Windows.Forms.TextBox();
		this.btnTemplateFolder = new System.Windows.Forms.Button();
		this.tableLayoutPanel2.SuspendLayout();
		this.panelMain.SuspendLayout();
		this.panel1.SuspendLayout();
		base.SuspendLayout();
		this.openFileDialogMain.FileName = "Template";
		this.openFileDialogMain.Filter = "Excel files (*.xls, *.xlsx, *.xlsm)| *.xls; *.xlsx; *.xlsm;";
		this.openFileDialogMain.Title = "Please select a template excel";
		this.tableLayoutPanel2.AutoSize = true;
		this.tableLayoutPanel2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		this.tableLayoutPanel2.ColumnCount = 2;
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel2.Controls.Add(this.panel1, 1, 4);
		this.tableLayoutPanel2.Controls.Add(this.txtDescription, 1, 2);
		this.tableLayoutPanel2.Controls.Add(this.txtName, 1, 1);
		this.tableLayoutPanel2.Controls.Add(this.txtCode, 1, 0);
		this.tableLayoutPanel2.Controls.Add(this.lblTemplate, 0, 4);
		this.tableLayoutPanel2.Controls.Add(this.cbbLimit, 1, 3);
		this.tableLayoutPanel2.Controls.Add(this.lblLimit, 0, 3);
		this.tableLayoutPanel2.Controls.Add(this.lblDescription, 0, 2);
		this.tableLayoutPanel2.Controls.Add(this.lblName, 0, 1);
		this.tableLayoutPanel2.Controls.Add(this.lblCode, 0, 0);
		this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
		this.tableLayoutPanel2.Name = "tableLayoutPanel2";
		this.tableLayoutPanel2.RowCount = 5;
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel2.Size = new System.Drawing.Size(492, 146);
		this.tableLayoutPanel2.TabIndex = 1;
		this.cbbLimit.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cbbLimit.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cbbLimit.Dock = System.Windows.Forms.DockStyle.Fill;
		this.cbbLimit.FormattingEnabled = true;
		this.cbbLimit.Items.AddRange(new object[10] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" });
		this.cbbLimit.Location = new System.Drawing.Point(93, 90);
		this.cbbLimit.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.cbbLimit.Name = "cbbLimit";
		this.cbbLimit.Size = new System.Drawing.Size(395, 24);
		this.cbbLimit.TabIndex = 4;
		this.toolTipMain.SetToolTip(this.cbbLimit, "Please select or enterlimit");
		this.cbbLimit.Enter += new System.EventHandler(cbbNormal_Enter);
		this.cbbLimit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(cbbLimit_KeyPress);
		this.cbbLimit.Leave += new System.EventHandler(cbbNormalNotNull_Leave);
		this.panelMain.AutoScroll = true;
		this.panelMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panelMain.Controls.Add(this.tableLayoutPanel2);
		this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panelMain.Location = new System.Drawing.Point(20, 70);
		this.panelMain.Name = "panelMain";
		this.panelMain.Padding = new System.Windows.Forms.Padding(3);
		this.panelMain.Size = new System.Drawing.Size(500, 154);
		this.panelMain.TabIndex = 1;
		this.btnConfirm.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnConfirm.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.btnConfirm.FlatAppearance.BorderSize = 0;
		this.btnConfirm.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
		this.btnConfirm.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
		this.btnConfirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnConfirm.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnConfirm.Location = new System.Drawing.Point(20, 224);
		this.btnConfirm.Name = "btnConfirm";
		this.btnConfirm.Size = new System.Drawing.Size(500, 28);
		this.btnConfirm.TabIndex = 1;
		this.btnConfirm.Text = "Confirm";
		this.toolTipMain.SetToolTip(this.btnConfirm, "Confirm");
		this.btnConfirm.UseVisualStyleBackColor = true;
		this.btnConfirm.Click += new System.EventHandler(btnComfirm_Click);
		this.lblCode.AutoSize = true;
		this.lblCode.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCode.Location = new System.Drawing.Point(4, 1);
		this.lblCode.Name = "lblCode";
		this.lblCode.Size = new System.Drawing.Size(82, 28);
		this.lblCode.TabIndex = 20;
		this.lblCode.Text = "Code";
		this.lblCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblDescription.AutoSize = true;
		this.lblDescription.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblDescription.Location = new System.Drawing.Point(4, 59);
		this.lblDescription.Name = "lblDescription";
		this.lblDescription.Size = new System.Drawing.Size(82, 28);
		this.lblDescription.TabIndex = 21;
		this.lblDescription.Text = "Description";
		this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblName.AutoSize = true;
		this.lblName.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblName.Location = new System.Drawing.Point(4, 30);
		this.lblName.Name = "lblName";
		this.lblName.Size = new System.Drawing.Size(82, 28);
		this.lblName.TabIndex = 22;
		this.lblName.Text = "Name";
		this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblLimit.AutoSize = true;
		this.lblLimit.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblLimit.Location = new System.Drawing.Point(4, 88);
		this.lblLimit.Name = "lblLimit";
		this.lblLimit.Size = new System.Drawing.Size(82, 28);
		this.lblLimit.TabIndex = 23;
		this.lblLimit.Text = "Limit";
		this.lblLimit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblTemplate.AutoSize = true;
		this.lblTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTemplate.Location = new System.Drawing.Point(4, 117);
		this.lblTemplate.Name = "lblTemplate";
		this.lblTemplate.Size = new System.Drawing.Size(82, 28);
		this.lblTemplate.TabIndex = 24;
		this.lblTemplate.Text = "Template url";
		this.lblTemplate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtCode.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtCode.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtCode.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtCode.Enabled = false;
		this.txtCode.Location = new System.Drawing.Point(93, 4);
		this.txtCode.Name = "txtCode";
		this.txtCode.Size = new System.Drawing.Size(395, 22);
		this.txtCode.TabIndex = 1;
		this.toolTipMain.SetToolTip(this.txtCode, "Please enter code");
		this.txtName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtName.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtName.Location = new System.Drawing.Point(93, 33);
		this.txtName.Name = "txtName";
		this.txtName.Size = new System.Drawing.Size(395, 22);
		this.txtName.TabIndex = 2;
		this.toolTipMain.SetToolTip(this.txtName, "Please enter name");
		this.txtName.Leave += new System.EventHandler(txtNormal_Leave);
		this.txtDescription.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtDescription.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtDescription.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtDescription.Location = new System.Drawing.Point(93, 62);
		this.txtDescription.Name = "txtDescription";
		this.txtDescription.Size = new System.Drawing.Size(395, 22);
		this.txtDescription.TabIndex = 3;
		this.toolTipMain.SetToolTip(this.txtDescription, "Please enter description");
		this.txtDescription.Leave += new System.EventHandler(txtNormal_Leave);
		this.panel1.Controls.Add(this.btnTemplateDelete);
		this.panel1.Controls.Add(this.txtTemplate);
		this.panel1.Controls.Add(this.btnTemplateFolder);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(93, 120);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(395, 22);
		this.panel1.TabIndex = 5;
		this.panel1.TabStop = true;
		this.btnTemplateDelete.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnTemplateDelete.BackColor = System.Drawing.SystemColors.Control;
		this.btnTemplateDelete.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnTemplateDelete.FlatAppearance.BorderSize = 0;
		this.btnTemplateDelete.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
		this.btnTemplateDelete.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.AppWorkspace;
		this.btnTemplateDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnTemplateDelete.Image = _5S_QA_System.Properties.Resources.cancel;
		this.btnTemplateDelete.Location = new System.Drawing.Point(351, 1);
		this.btnTemplateDelete.Margin = new System.Windows.Forms.Padding(0);
		this.btnTemplateDelete.Name = "btnTemplateDelete";
		this.btnTemplateDelete.Size = new System.Drawing.Size(20, 20);
		this.btnTemplateDelete.TabIndex = 3;
		this.btnTemplateDelete.TabStop = false;
		this.toolTipMain.SetToolTip(this.btnTemplateDelete, "Clear template url");
		this.btnTemplateDelete.UseVisualStyleBackColor = false;
		this.btnTemplateDelete.Visible = false;
		this.btnTemplateDelete.Click += new System.EventHandler(btnTemplateDelete_Click);
		this.txtTemplate.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtTemplate.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtTemplate.Enabled = false;
		this.txtTemplate.Location = new System.Drawing.Point(0, 0);
		this.txtTemplate.Name = "txtTemplate";
		this.txtTemplate.Size = new System.Drawing.Size(373, 22);
		this.txtTemplate.TabIndex = 1;
		this.txtTemplate.TabStop = false;
		this.toolTipMain.SetToolTip(this.txtTemplate, "Please enter template url");
		this.txtTemplate.TextChanged += new System.EventHandler(txtTemplateUrl_TextChanged);
		this.btnTemplateFolder.BackColor = System.Drawing.Color.Gainsboro;
		this.btnTemplateFolder.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnTemplateFolder.Dock = System.Windows.Forms.DockStyle.Right;
		this.btnTemplateFolder.FlatAppearance.BorderSize = 0;
		this.btnTemplateFolder.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
		this.btnTemplateFolder.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SkyBlue;
		this.btnTemplateFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnTemplateFolder.Image = _5S_QA_System.Properties.Resources.folder;
		this.btnTemplateFolder.Location = new System.Drawing.Point(373, 0);
		this.btnTemplateFolder.Name = "btnTemplateFolder";
		this.btnTemplateFolder.Size = new System.Drawing.Size(22, 22);
		this.btnTemplateFolder.TabIndex = 1;
		this.toolTipMain.SetToolTip(this.btnTemplateFolder, "Open folder to select detail file");
		this.btnTemplateFolder.UseVisualStyleBackColor = false;
		this.btnTemplateFolder.Click += new System.EventHandler(btnTemplateFolder_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(540, 272);
		base.Controls.Add(this.panelMain);
		base.Controls.Add(this.btnConfirm);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		base.Name = "frmTemplateAdd";
		base.Padding = new System.Windows.Forms.Padding(20, 70, 20, 20);
		this.Text = "5S QA System * TEMPLATE";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmTemplateAdd_FormClosing);
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmTemplateAdd_FormClosed);
		base.Load += new System.EventHandler(frmTemplateAdd_Load);
		base.Shown += new System.EventHandler(frmTemplateAdd_Shown);
		this.tableLayoutPanel2.ResumeLayout(false);
		this.tableLayoutPanel2.PerformLayout();
		this.panelMain.ResumeLayout(false);
		this.panelMain.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		base.ResumeLayout(false);
	}
}
