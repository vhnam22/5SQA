using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using _5S_QA_DL.APIContext;
using _5S_QA_Entities.Abstracts;
using _5S_QA_Entities.Constants;
using _5S_QA_Entities.Dtos;
using _5S_QA_Entities.Enums;
using _5S_QA_Entities.Models;
using _5S_QA_System.Controls;
using _5S_QA_System.Properties;
using Newtonsoft.Json;
using OfficeOpenXml;

namespace _5S_QA_System.View.User_control;

public class mPanelGroup : UserControl
{
	private DataGridView mDataGridView;

	private Guid mId;

	private Form mForm;

	private string mFileName;

	private readonly ComboBox cbb;

	private IContainer components = null;

	private Panel panelResize;

	public Button btnConfirm;

	private Panel panel1;

	private TableLayoutPanel tpanelTitle;

	private Button btnDown;

	private Button btnUp;

	private Label lblTittle;

	private DataGridView dgvFooter;

	private DataGridView dgvContent;

	private OpenFileDialog openFileDialogMain;

	private DataGridView dgvOther;

	private DataGridViewTextBoxColumn FooterName;

	private DataGridViewTextBoxColumn FooterTitle;

	private DataGridViewTextBoxColumn FooterValue;

	private DataGridViewTextBoxColumn ContentName;

	private DataGridViewTextBoxColumn ContentTitle;

	private DataGridViewTextBoxColumn ContentValue;

	private DataGridViewTextBoxColumn OtherName;

	private DataGridViewTextBoxColumn OtherTitle;

	private DataGridViewTextBoxColumn OtherValue;

	private DataGridViewButtonColumn OtherClear;

	private DataGridViewButtonColumn OtherFolder;

	private ToolTip toolTipMain;

	private Label lblValue;

	public mPanelGroup()
	{
		InitializeComponent();
		Common.setControls(this, toolTipMain);
		mId = Guid.Empty;
		cbb = new ComboBox();
		dgvContent.Controls.Add(cbb);
		cbb.Visible = false;
		cbb.DropDownStyle = ComboBoxStyle.DropDownList;
		cbb.Cursor = Cursors.Hand;
		cbb.Items.AddRange(new object[3] { "Detail", "Chart", "Special" });
	}

	private void mPanelGroup_Load(object sender, EventArgs e)
	{
		ControlResize.Init(panelResize, this, ControlResize.Direction.Horizontal, Cursors.SizeWE);
		mDataGridView = base.Parent.Controls["dgvMain"] as DataGridView;
		mForm = base.ParentForm;
	}

	public void load_dgvContent(Enum type)
	{
		dgvContent.Rows.Clear();
		dgvOther.Rows.Clear();
		dgvFooter.Rows.Clear();
		try
		{
			List<string> list = MetaType.mPanelGroup;
			foreach (DataGridViewColumn col in mDataGridView.Columns)
			{
				if (list.Find((string x) => x.Equals(col.Name)) == null)
				{
					continue;
				}
				if (type is FormType formType)
				{
					switch (formType)
					{
					case FormType.VIEW:
						lblTittle.Text = Common.getTextLanguage(this, "VIEW");
						lblValue.Text = (string.IsNullOrEmpty(mDataGridView.CurrentRow.Cells["Id"].Value.ToString()) ? string.Empty : mDataGridView.CurrentRow.Cells["Id"].Value.ToString());
						btnConfirm.Visible = false;
						dgvOther.Columns["OtherClear"].Visible = false;
						dgvOther.Columns["OtherFolder"].Visible = false;
						if (MetaType.dgvFooter.Find((string x) => x.Equals(col.Name)) != null)
						{
							dgvFooter.Rows.Add(col.Name, col.HeaderText, mDataGridView.CurrentRow.Cells[col.Name].Value);
						}
						else if (MetaType.dgvOther.Find((string x) => x.Equals(col.Name)) != null)
						{
							dgvOther.Rows.Add(col.Name, col.HeaderText, mDataGridView.CurrentRow.Cells[col.Name].Value);
						}
						else
						{
							dgvContent.MultiSelect = true;
							dgvContent.Rows.Add(col.Name, col.HeaderText, mDataGridView.CurrentRow.Cells[col.Name].Value);
							dgvContent.Rows[dgvContent.RowCount - 1].Cells["ContentValue"].ReadOnly = true;
						}
						continue;
					case FormType.ADD:
					{
						lblTittle.Text = Common.getTextLanguage(this, "ADD");
						lblValue.Text = string.Empty;
						btnConfirm.Visible = true;
						mId = Guid.Empty;
						dgvOther.Columns["OtherClear"].Visible = false;
						dgvOther.Columns["OtherFolder"].Visible = true;
						if (MetaType.dgvFooter.Find((string x) => x.Equals(col.Name)) != null)
						{
							dgvFooter.Rows.Add(col.Name, col.HeaderText, "");
							continue;
						}
						if (MetaType.dgvOther.Find((string x) => x.Equals(col.Name)) != null)
						{
							dgvOther.Rows.Add(col.Name, col.HeaderText, "");
							continue;
						}
						dgvContent.MultiSelect = false;
						string name = col.Name;
						string text = name;
						if (text == "Code")
						{
							dgvContent.Rows.Add(col.Name, col.HeaderText, set_Code());
							dgvContent.Rows[dgvContent.RowCount - 1].Cells["ContentValue"].ReadOnly = true;
						}
						else
						{
							dgvContent.Rows.Add(col.Name, col.HeaderText, (mDataGridView.CurrentCell == null) ? string.Empty : mDataGridView.CurrentRow.Cells[col.Name].Value);
							dgvContent.Rows[dgvContent.RowCount - 1].Cells["ContentValue"].ReadOnly = false;
						}
						continue;
					}
					}
				}
				lblTittle.Text = Common.getTextLanguage(this, "EDIT");
				btnConfirm.Visible = true;
				mId = (string.IsNullOrEmpty(mDataGridView.CurrentRow.Cells["Id"].Value.ToString()) ? Guid.Empty : Guid.Parse(mDataGridView.CurrentRow.Cells["Id"].Value.ToString()));
				lblValue.Text = mId.ToString();
				dgvOther.Columns["OtherFolder"].Visible = true;
				if (MetaType.dgvFooter.Find((string x) => x.Equals(col.Name)) != null)
				{
					dgvFooter.Rows.Add(col.Name, col.HeaderText, mDataGridView.CurrentRow.Cells[col.Name].Value);
				}
				else if (MetaType.dgvOther.Find((string x) => x.Equals(col.Name)) != null)
				{
					mFileName = "";
					if (mDataGridView.CurrentRow.Cells[col.Name].Value != null)
					{
						mFileName = mDataGridView.CurrentRow.Cells[col.Name].Value.ToString();
					}
					dgvOther.Rows.Add(col.Name, col.HeaderText, mDataGridView.CurrentRow.Cells[col.Name].Value);
					if (mDataGridView.CurrentRow.Cells[col.Name].Value == null || string.IsNullOrEmpty(mDataGridView.CurrentRow.Cells[col.Name].Value.ToString()))
					{
						dgvOther.Columns["OtherClear"].Visible = false;
					}
					else
					{
						dgvOther.Columns["OtherClear"].Visible = true;
					}
				}
				else
				{
					dgvContent.MultiSelect = false;
					dgvContent.Rows.Add(col.Name, col.HeaderText, mDataGridView.CurrentRow.Cells[col.Name].Value);
					string name2 = col.Name;
					string text2 = name2;
					if (text2 == "Code")
					{
						dgvContent.Rows[dgvContent.RowCount - 1].Cells["ContentValue"].ReadOnly = true;
					}
					else
					{
						dgvContent.Rows[dgvContent.RowCount - 1].Cells["ContentValue"].ReadOnly = false;
					}
				}
			}
		}
		finally
		{
			dgvContent.Size = new Size(base.Width, dgvContent.Rows.Count * 22 + 3);
			dgvOther.Size = new Size(base.Width, dgvOther.Rows.Count * 22 + 3);
			dgvFooter.Size = new Size(base.Width, dgvFooter.Rows.Count * 22 + 3);
			dgvContent.CurrentCell = null;
			dgvContent.Refresh();
			dgvOther.CurrentCell = null;
			dgvOther.Refresh();
			dgvFooter.CurrentCell = null;
			dgvFooter.Refresh();
		}
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
						mForm.Close();
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

	private void btnUp_Click(object sender, EventArgs e)
	{
		mDataGridView.Select();
		SendKeys.SendWait("{up}");
	}

	private void btnDown_Click(object sender, EventArgs e)
	{
		mDataGridView.Select();
		SendKeys.SendWait("{down}");
	}

	private void dgvContent_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
	{
		DataGridView dataGridView = sender as DataGridView;
		DataGridViewTextBoxEditingControl dataGridViewTextBoxEditingControl = e.Control as DataGridViewTextBoxEditingControl;
		dataGridViewTextBoxEditingControl.AutoCompleteMode = AutoCompleteMode.Suggest;
		dataGridViewTextBoxEditingControl.AutoCompleteSource = AutoCompleteSource.CustomSource;
		dataGridViewTextBoxEditingControl.KeyPress -= txtTemp_KeyPress;
		dataGridViewTextBoxEditingControl.KeyUp -= txtTemp_KeyUp;
		dataGridViewTextBoxEditingControl.Leave -= txtTemp_Leave;
		dataGridViewTextBoxEditingControl.MaxLength = int.MaxValue;
		object value = dataGridView.CurrentRow.Cells["ContentName"].Value;
		object obj = value;
		switch (obj as string)
		{
		case "name":
			dataGridViewTextBoxEditingControl.AutoCompleteCustomSource = Common.getAutoComplete((DataTable)mDataGridView.DataSource, "Name");
			break;
		case "Description":
			dataGridViewTextBoxEditingControl.AutoCompleteCustomSource = Common.getAutoComplete((DataTable)mDataGridView.DataSource, "Description");
			break;
		case "Limit":
			dataGridViewTextBoxEditingControl.KeyPress += txtTemp_KeyPress;
			dataGridViewTextBoxEditingControl.KeyUp += txtTemp_KeyUp;
			dataGridViewTextBoxEditingControl.Leave += txtTemp_Leave;
			dataGridViewTextBoxEditingControl.AutoCompleteCustomSource = Common.getAutoComplete((DataTable)mDataGridView.DataSource, "Limit");
			break;
		case "Type":
		{
			Rectangle cellDisplayRectangle = dataGridView.GetCellDisplayRectangle(dataGridView.CurrentCell.ColumnIndex, dataGridView.CurrentCell.RowIndex, cutOverflow: true);
			cbb.Size = new Size(cellDisplayRectangle.Width, cellDisplayRectangle.Height);
			cbb.Location = new Point(cellDisplayRectangle.X, cellDisplayRectangle.Y);
			cbb.Text = dataGridView.CurrentCell.Value.ToString();
			cbb.Visible = true;
			cbb.BringToFront();
			break;
		}
		}
	}

	private void txtTemp_KeyPress(object sender, KeyPressEventArgs e)
	{
		if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b')
		{
			e.Handled = true;
		}
	}

	private void txtTemp_KeyUp(object sender, KeyEventArgs e)
	{
		DataGridViewTextBoxEditingControl dataGridViewTextBoxEditingControl = sender as DataGridViewTextBoxEditingControl;
		dataGridViewTextBoxEditingControl.Text = (int.TryParse(dataGridViewTextBoxEditingControl.Text.Replace(",", ""), out var result) ? result.ToString("#,##0") : "7");
		dataGridViewTextBoxEditingControl.Select(dataGridViewTextBoxEditingControl.Text.Length, 0);
	}

	private void txtTemp_Leave(object sender, EventArgs e)
	{
		DataGridViewTextBoxEditingControl dataGridViewTextBoxEditingControl = sender as DataGridViewTextBoxEditingControl;
		dataGridViewTextBoxEditingControl.Text = (int.TryParse(dataGridViewTextBoxEditingControl.Text.Replace(",", ""), out var result) ? result.ToString("#,##0") : "7");
		if (result.Equals(0))
		{
			dataGridViewTextBoxEditingControl.Text = "7";
		}
	}

	private void dgvContent_CellEndEdit(object sender, DataGridViewCellEventArgs e)
	{
		DataGridView dataGridView = sender as DataGridView;
		if (dataGridView.CurrentCell.Value != null)
		{
			dataGridView.CurrentCell.Value = Common.trimSpace(dataGridView.CurrentCell.Value.ToString());
		}
		object value = dataGridView.CurrentRow.Cells["ContentName"].Value;
		object obj = value;
		if (obj is string text && text == "Type")
		{
			dataGridView.CurrentCell.Value = cbb.Text;
			cbb.Visible = false;
		}
	}

	private void btnConfirm_Click(object sender, EventArgs e)
	{
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Expected O, but got Unknown
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Expected O, but got Unknown
		//IL_0228: Expected O, but got Unknown
		//IL_064b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0655: Expected O, but got Unknown
		Cursor.Current = Cursors.WaitCursor;
		try
		{
			foreach (DataGridViewRow item in (IEnumerable)dgvContent.Rows)
			{
				if (item.Cells["ContentValue"].Value == null || string.IsNullOrEmpty(item.Cells["ContentValue"].Value.ToString()))
				{
					object value = item.Cells["ContentName"].Value;
					object obj = value;
					switch (obj as string)
					{
					case "Code":
						MessageBox.Show(Common.getTextLanguage(this, "wCode"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					case "name":
						MessageBox.Show(Common.getTextLanguage(this, "wName"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						item.Cells["ContentValue"].Selected = true;
						dgvContent.BeginEdit(selectAll: true);
						return;
					case "Limit":
						MessageBox.Show(Common.getTextLanguage(this, "wLimit"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						item.Cells["ContentValue"].Selected = true;
						dgvContent.BeginEdit(selectAll: true);
						return;
					case "Type":
						MessageBox.Show(Common.getTextLanguage(this, "wType"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						item.Cells["ContentValue"].Selected = true;
						dgvContent.BeginEdit(selectAll: true);
						return;
					}
				}
			}
			TemplateViewModel val = new TemplateViewModel();
			((AuditableEntity)val).Id = mId;
			((AuditableEntity)val).IsActivated = true;
			TemplateViewModel val2 = val;
			foreach (DataGridViewRow item2 in (IEnumerable)dgvContent.Rows)
			{
				if (item2.Cells["ContentValue"].Value != null && !string.IsNullOrEmpty(item2.Cells["ContentValue"].Value.ToString()))
				{
					object value2 = item2.Cells["ContentName"].Value;
					object obj2 = value2;
					switch (obj2 as string)
					{
					case "Code":
						val2.Code = item2.Cells["ContentValue"].Value.ToString();
						break;
					case "name":
						val2.Name = item2.Cells["ContentValue"].Value.ToString();
						break;
					case "Description":
						val2.Description = item2.Cells["ContentValue"].Value.ToString();
						break;
					case "Limit":
						val2.Limit = int.Parse(item2.Cells["ContentValue"].Value.ToString().Replace(",", ""));
						break;
					case "Type":
						val2.Type = item2.Cells["ContentValue"].Value.ToString();
						break;
					}
				}
			}
			if (!mId.Equals(Guid.Empty) && MessageBox.Show(Common.getTextLanguage(this, "wSureEdit"), Common.getTextLanguage(this, "QUESTION"), MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.No))
			{
				return;
			}
			Cursor.Current = Cursors.WaitCursor;
			string text = string.Empty;
			if (dgvOther.Rows[0].Cells["OtherValue"].Value != null && !string.IsNullOrEmpty(dgvOther.Rows[0].Cells["OtherValue"].Value.ToString()))
			{
				text = dgvOther.Rows[0].Cells["OtherValue"].Value.ToString();
			}
			if (File.Exists(text))
			{
				using FileStream newStream = File.Open(text, FileMode.Open);
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
						foreach (ExcelRangeBase item3 in excelRange)
						{
							if (item3.Value != null)
							{
								string text2 = item3.Value.ToString().Trim().ToUpper();
								if (text2.StartsWith("[[") && text2.EndsWith("]]"))
								{
									list.Add(new ExportMappingDto(item3.Address, text2, worksheet.Name));
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
			if (!mId.Equals(Guid.Empty))
			{
				val2.TemplateUrl = mFileName;
			}
			ResponseDto result = frmLogin.client.SaveAsync(val2, "/api/Template/Save").Result;
			if (!result.Success)
			{
				throw new Exception(result.Messages.ElementAt(0).Message);
			}
			if (mId.Equals(Guid.Empty))
			{
				TemplateViewModel val3 = JsonConvert.DeserializeObject<TemplateViewModel>(result.Data.ToString());
				if (File.Exists(text))
				{
					using FileStream data = File.OpenRead(text);
					FileParameter file = new FileParameter(data, text);
					ResponseDto result2 = frmLogin.client.ImportAsync(((AuditableEntity)(object)val3).Id, file, "/api/Template/UpdateExcel/{id}").Result;
					if (!result2.Success)
					{
						MessageBox.Show(result2.Messages.First().Message, result2.Messages.First().Code, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
				}
			}
			else if (File.Exists(text))
			{
				using FileStream data2 = File.OpenRead(text);
				FileParameter file2 = new FileParameter(data2, text);
				ResponseDto result3 = frmLogin.client.ImportAsync(mId, file2, "/api/Template/UpdateExcel/{id}").Result;
				if (!result3.Success)
				{
					MessageBox.Show(result3.Messages.First().Message, result3.Messages.First().Code, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
			else if (string.IsNullOrEmpty(text))
			{
				Stream data3 = new MemoryStream();
				FileParameter file3 = new FileParameter(data3);
				ResponseDto result4 = frmLogin.client.ImportAsync(mId, file3, "/api/Template/UpdateExcel/{id}").Result;
				if (!result4.Success)
				{
					MessageBox.Show(result4.Messages.First().Message, result4.Messages.First().Code, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
			frmTemplateView frmTemplateView = mForm as frmTemplateView;
			frmTemplateView.isClose = false;
			frmTemplateView.load_AllData();
		}
		catch (Exception ex)
		{
			string text3 = ex.Message;
			string name = Settings.Default.Language.Replace("rb", "Name");
			List<Language> list2 = Common.ReadLanguages("Error");
			foreach (Language item4 in list2)
			{
				object value3 = ((object)item4).GetType().GetProperty(name).GetValue(item4, null);
				if (value3 != null)
				{
					string newValue = value3.ToString();
					text3 = text3.Replace(item4.Code, newValue);
				}
			}
			if (ex.InnerException is ApiException { StatusCode: var statusCode })
			{
				if (statusCode.Equals(401))
				{
					if (MessageBox.Show(Common.getTextLanguage(this, "LoginAgain"), Common.getTextLanguage(this, "INFORMATION"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk).Equals(DialogResult.OK))
					{
						mForm.Close();
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

	private void dgvNormal_Leave(object sender, EventArgs e)
	{
		DataGridView dataGridView = sender as DataGridView;
		dataGridView.CurrentCell = null;
	}

	private void dgvOther_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		DataGridView dataGridView = sender as DataGridView;
		string name = dataGridView.CurrentCell.OwningColumn.Name;
		string text = name;
		if (!(text == "OtherClear"))
		{
			if (text == "OtherFolder")
			{
				openFileDialogMain.Title = Common.getTextLanguage(this, "SelectTemplate");
				if (openFileDialogMain.ShowDialog().Equals(DialogResult.OK))
				{
					dataGridView.CurrentRow.Cells["OtherValue"].Value = openFileDialogMain.FileName;
					dataGridView.Columns["OtherClear"].Visible = true;
				}
			}
		}
		else
		{
			dataGridView.CurrentRow.Cells["OtherValue"].Value = null;
			dataGridView.Columns["OtherClear"].Visible = false;
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
		this.panelResize = new System.Windows.Forms.Panel();
		this.btnConfirm = new System.Windows.Forms.Button();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tpanelTitle = new System.Windows.Forms.TableLayoutPanel();
		this.lblValue = new System.Windows.Forms.Label();
		this.btnDown = new System.Windows.Forms.Button();
		this.btnUp = new System.Windows.Forms.Button();
		this.lblTittle = new System.Windows.Forms.Label();
		this.dgvFooter = new System.Windows.Forms.DataGridView();
		this.FooterName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.FooterTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.FooterValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dgvContent = new System.Windows.Forms.DataGridView();
		this.ContentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ContentTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ContentValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.openFileDialogMain = new System.Windows.Forms.OpenFileDialog();
		this.dgvOther = new System.Windows.Forms.DataGridView();
		this.OtherName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.OtherTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.OtherValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.OtherClear = new System.Windows.Forms.DataGridViewButtonColumn();
		this.OtherFolder = new System.Windows.Forms.DataGridViewButtonColumn();
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.tpanelTitle.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvFooter).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.dgvContent).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.dgvOther).BeginInit();
		base.SuspendLayout();
		this.panelResize.Dock = System.Windows.Forms.DockStyle.Left;
		this.panelResize.Location = new System.Drawing.Point(0, 28);
		this.panelResize.Margin = new System.Windows.Forms.Padding(1);
		this.panelResize.Name = "panelResize";
		this.panelResize.Size = new System.Drawing.Size(4, 294);
		this.panelResize.TabIndex = 144;
		this.btnConfirm.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnConfirm.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.btnConfirm.FlatAppearance.BorderSize = 0;
		this.btnConfirm.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
		this.btnConfirm.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
		this.btnConfirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnConfirm.Location = new System.Drawing.Point(0, 322);
		this.btnConfirm.Name = "btnConfirm";
		this.btnConfirm.Size = new System.Drawing.Size(400, 28);
		this.btnConfirm.TabIndex = 1;
		this.btnConfirm.Text = "Confirm";
		this.toolTipMain.SetToolTip(this.btnConfirm, "Confirm");
		this.btnConfirm.UseVisualStyleBackColor = true;
		this.btnConfirm.Click += new System.EventHandler(btnConfirm_Click);
		this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 27);
		this.panel1.Margin = new System.Windows.Forms.Padding(0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(400, 1);
		this.panel1.TabIndex = 142;
		this.tpanelTitle.AutoSize = true;
		this.tpanelTitle.ColumnCount = 4;
		this.tpanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tpanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelTitle.Controls.Add(this.lblValue, 1, 0);
		this.tpanelTitle.Controls.Add(this.btnDown, 3, 0);
		this.tpanelTitle.Controls.Add(this.btnUp, 2, 0);
		this.tpanelTitle.Controls.Add(this.lblTittle, 0, 0);
		this.tpanelTitle.Dock = System.Windows.Forms.DockStyle.Top;
		this.tpanelTitle.Location = new System.Drawing.Point(0, 0);
		this.tpanelTitle.Margin = new System.Windows.Forms.Padding(0);
		this.tpanelTitle.Name = "tpanelTitle";
		this.tpanelTitle.RowCount = 1;
		this.tpanelTitle.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tpanelTitle.Size = new System.Drawing.Size(400, 27);
		this.tpanelTitle.TabIndex = 141;
		this.lblValue.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblValue.ForeColor = System.Drawing.Color.Red;
		this.lblValue.Location = new System.Drawing.Point(43, 1);
		this.lblValue.Margin = new System.Windows.Forms.Padding(1);
		this.lblValue.Name = "lblValue";
		this.lblValue.Size = new System.Drawing.Size(308, 25);
		this.lblValue.TabIndex = 133;
		this.lblValue.Text = "...";
		this.lblValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnDown.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnDown.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnDown.FlatAppearance.BorderSize = 0;
		this.btnDown.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.AppWorkspace;
		this.btnDown.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
		this.btnDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnDown.Image = _5S_QA_System.Properties.Resources.arrow_down;
		this.btnDown.Location = new System.Drawing.Point(377, 1);
		this.btnDown.Margin = new System.Windows.Forms.Padding(1);
		this.btnDown.Name = "btnDown";
		this.btnDown.Size = new System.Drawing.Size(22, 25);
		this.btnDown.TabIndex = 2;
		this.btnDown.TabStop = false;
		this.toolTipMain.SetToolTip(this.btnDown, "Select lower row item");
		this.btnDown.Click += new System.EventHandler(btnDown_Click);
		this.btnUp.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnUp.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnUp.FlatAppearance.BorderSize = 0;
		this.btnUp.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.AppWorkspace;
		this.btnUp.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
		this.btnUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUp.Image = _5S_QA_System.Properties.Resources.arrow_up;
		this.btnUp.Location = new System.Drawing.Point(353, 1);
		this.btnUp.Margin = new System.Windows.Forms.Padding(1);
		this.btnUp.Name = "btnUp";
		this.btnUp.Size = new System.Drawing.Size(22, 25);
		this.btnUp.TabIndex = 1;
		this.btnUp.TabStop = false;
		this.toolTipMain.SetToolTip(this.btnUp, "Select upper row item");
		this.btnUp.Click += new System.EventHandler(btnUp_Click);
		this.lblTittle.AutoSize = true;
		this.lblTittle.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTittle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTittle.Location = new System.Drawing.Point(1, 1);
		this.lblTittle.Margin = new System.Windows.Forms.Padding(1);
		this.lblTittle.Name = "lblTittle";
		this.lblTittle.Size = new System.Drawing.Size(40, 25);
		this.lblTittle.TabIndex = 0;
		this.lblTittle.Text = "View";
		this.lblTittle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.dgvFooter.AllowUserToAddRows = false;
		this.dgvFooter.AllowUserToDeleteRows = false;
		this.dgvFooter.AllowUserToResizeColumns = false;
		this.dgvFooter.AllowUserToResizeRows = false;
		this.dgvFooter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		this.dgvFooter.ColumnHeadersVisible = false;
		this.dgvFooter.Columns.AddRange(this.FooterName, this.FooterTitle, this.FooterValue);
		this.dgvFooter.Dock = System.Windows.Forms.DockStyle.Top;
		this.dgvFooter.Location = new System.Drawing.Point(4, 72);
		this.dgvFooter.Margin = new System.Windows.Forms.Padding(1);
		this.dgvFooter.Name = "dgvFooter";
		this.dgvFooter.ReadOnly = true;
		this.dgvFooter.RowHeadersVisible = false;
		this.dgvFooter.ScrollBars = System.Windows.Forms.ScrollBars.None;
		this.dgvFooter.Size = new System.Drawing.Size(396, 22);
		this.dgvFooter.TabIndex = 3;
		this.dgvFooter.Leave += new System.EventHandler(dgvNormal_Leave);
		this.FooterName.HeaderText = "Name";
		this.FooterName.Name = "FooterName";
		this.FooterName.ReadOnly = true;
		this.FooterName.Visible = false;
		dataGridViewCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.FooterTitle.DefaultCellStyle = dataGridViewCellStyle;
		this.FooterTitle.HeaderText = "Title";
		this.FooterTitle.Name = "FooterTitle";
		this.FooterTitle.ReadOnly = true;
		this.FooterTitle.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.FooterTitle.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.FooterTitle.Width = 120;
		this.FooterValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		this.FooterValue.DefaultCellStyle = dataGridViewCellStyle2;
		this.FooterValue.HeaderText = "Value";
		this.FooterValue.Name = "FooterValue";
		this.FooterValue.ReadOnly = true;
		this.FooterValue.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.FooterValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.dgvContent.AllowUserToAddRows = false;
		this.dgvContent.AllowUserToDeleteRows = false;
		this.dgvContent.AllowUserToResizeColumns = false;
		this.dgvContent.AllowUserToResizeRows = false;
		this.dgvContent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		this.dgvContent.ColumnHeadersVisible = false;
		this.dgvContent.Columns.AddRange(this.ContentName, this.ContentTitle, this.ContentValue);
		this.dgvContent.Dock = System.Windows.Forms.DockStyle.Top;
		this.dgvContent.Location = new System.Drawing.Point(4, 28);
		this.dgvContent.Margin = new System.Windows.Forms.Padding(1);
		this.dgvContent.Name = "dgvContent";
		this.dgvContent.RowHeadersVisible = false;
		this.dgvContent.ScrollBars = System.Windows.Forms.ScrollBars.None;
		this.dgvContent.Size = new System.Drawing.Size(396, 22);
		this.dgvContent.TabIndex = 1;
		this.dgvContent.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(dgvContent_CellEndEdit);
		this.dgvContent.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(dgvContent_EditingControlShowing);
		this.ContentName.HeaderText = "Name";
		this.ContentName.Name = "ContentName";
		this.ContentName.Visible = false;
		dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.ContentTitle.DefaultCellStyle = dataGridViewCellStyle3;
		this.ContentTitle.HeaderText = "Title";
		this.ContentTitle.Name = "ContentTitle";
		this.ContentTitle.ReadOnly = true;
		this.ContentTitle.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.ContentTitle.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.ContentTitle.Width = 120;
		this.ContentValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		this.ContentValue.DefaultCellStyle = dataGridViewCellStyle4;
		this.ContentValue.HeaderText = "Value";
		this.ContentValue.Name = "ContentValue";
		this.ContentValue.ReadOnly = true;
		this.ContentValue.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.ContentValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.openFileDialogMain.FileName = "Template";
		this.openFileDialogMain.Filter = "Excel files (*.xls, *.xlsx, *.xlsm)| *.xls; *.xlsx; *.xlsm;";
		this.openFileDialogMain.Title = "Select template";
		this.dgvOther.AllowUserToAddRows = false;
		this.dgvOther.AllowUserToDeleteRows = false;
		this.dgvOther.AllowUserToResizeColumns = false;
		this.dgvOther.AllowUserToResizeRows = false;
		this.dgvOther.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		this.dgvOther.ColumnHeadersVisible = false;
		this.dgvOther.Columns.AddRange(this.OtherName, this.OtherTitle, this.OtherValue, this.OtherClear, this.OtherFolder);
		this.dgvOther.Dock = System.Windows.Forms.DockStyle.Top;
		this.dgvOther.Location = new System.Drawing.Point(4, 50);
		this.dgvOther.Margin = new System.Windows.Forms.Padding(1);
		this.dgvOther.Name = "dgvOther";
		this.dgvOther.ReadOnly = true;
		this.dgvOther.RowHeadersVisible = false;
		this.dgvOther.ScrollBars = System.Windows.Forms.ScrollBars.None;
		this.dgvOther.Size = new System.Drawing.Size(396, 22);
		this.dgvOther.TabIndex = 2;
		this.dgvOther.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvOther_CellClick);
		this.dgvOther.Leave += new System.EventHandler(dgvNormal_Leave);
		this.OtherName.HeaderText = "Name";
		this.OtherName.Name = "OtherName";
		this.OtherName.ReadOnly = true;
		this.OtherName.Visible = false;
		dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.OtherTitle.DefaultCellStyle = dataGridViewCellStyle5;
		this.OtherTitle.HeaderText = "Title";
		this.OtherTitle.Name = "OtherTitle";
		this.OtherTitle.ReadOnly = true;
		this.OtherTitle.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.OtherTitle.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.OtherTitle.Width = 120;
		this.OtherValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		this.OtherValue.DefaultCellStyle = dataGridViewCellStyle6;
		this.OtherValue.HeaderText = "Value";
		this.OtherValue.Name = "OtherValue";
		this.OtherValue.ReadOnly = true;
		this.OtherValue.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.OtherValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.OtherClear.HeaderText = "Clear";
		this.OtherClear.Name = "OtherClear";
		this.OtherClear.ReadOnly = true;
		this.OtherClear.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		this.OtherClear.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
		this.OtherClear.Text = "X";
		this.OtherClear.ToolTipText = "Clear template";
		this.OtherClear.UseColumnTextForButtonValue = true;
		this.OtherClear.Width = 25;
		this.OtherFolder.HeaderText = "Folder";
		this.OtherFolder.Name = "OtherFolder";
		this.OtherFolder.ReadOnly = true;
		this.OtherFolder.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		this.OtherFolder.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
		this.OtherFolder.Text = "...";
		this.OtherFolder.ToolTipText = "Select a template";
		this.OtherFolder.UseColumnTextForButtonValue = true;
		this.OtherFolder.Width = 30;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		base.Controls.Add(this.dgvFooter);
		base.Controls.Add(this.dgvOther);
		base.Controls.Add(this.dgvContent);
		base.Controls.Add(this.panelResize);
		base.Controls.Add(this.btnConfirm);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.tpanelTitle);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "mPanelGroup";
		base.Size = new System.Drawing.Size(400, 350);
		base.Load += new System.EventHandler(mPanelGroup_Load);
		this.tpanelTitle.ResumeLayout(false);
		this.tpanelTitle.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvFooter).EndInit();
		((System.ComponentModel.ISupportInitialize)this.dgvContent).EndInit();
		((System.ComponentModel.ISupportInitialize)this.dgvOther).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
