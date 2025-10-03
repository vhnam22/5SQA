using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
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

namespace _5S_QA_System.View.User_control;

public class mPanelOther : UserControl
{
	private DataGridView mDataGridView;

	private Guid mId;

	private Form mForm;

	private Type mType;

	private Guid mIdRequest;

	private string mFileName;

	private string mUnit;

	private IContainer components = null;

	private TableLayoutPanel tpanelTitle;

	private Button btnDown;

	private Button btnUp;

	private Panel panel1;

	private ToolTip toolTipMain;

	private Panel panelResize;

	private Label lblTitle;

	private Button btnConfirm;

	private Label lblValue;

	private Panel panelView;

	private DataGridView dgvHistory;

	private DataGridViewTextBoxColumn his_no;

	private DataGridViewTextBoxColumn his_value;

	private DataGridViewTextBoxColumn his_staff;

	private DataGridViewTextBoxColumn his_machine;

	private DataGridViewTextBoxColumn his_created;

	private Label lblHistory;

	private DataGridView dgvFooter;

	private DataGridViewTextBoxColumn FooterName;

	private DataGridViewTextBoxColumn FooterTitle;

	private DataGridViewTextBoxColumn FooterValue;

	private DataGridView dgvContent;

	private DataGridViewTextBoxColumn ContentName;

	private DataGridViewTextBoxColumn ContentTitle;

	private DataGridViewTextBoxColumn ContentValue;

	private DataGridView dgvOther;

	private DataGridViewTextBoxColumn OtherName;

	private DataGridViewTextBoxColumn OtherTitle;

	private DataGridViewTextBoxColumn OtherValue;

	private DataGridViewButtonColumn OtherClear;

	private DataGridViewButtonColumn OtherFolder;

	private OpenFileDialog openFileDialogMain;

	public mPanelOther()
	{
		InitializeComponent();
		Common.setControls(this, toolTipMain);
		mId = Guid.Empty;
		mIdRequest = Guid.Empty;
	}

	private void mPanelOther_Load(object sender, EventArgs e)
	{
		ControlResize.Init(panelResize, this, ControlResize.Direction.Horizontal, Cursors.SizeWE);
		mDataGridView = base.Parent.Controls["dgvMain"] as DataGridView;
		mForm = base.ParentForm;
		mType = mForm.GetType();
		if (mType.Equals(typeof(frmResultView)) || mType.Equals(typeof(frmHistoryView)) || mType.Equals(typeof(frmChartView)))
		{
			lblHistory.Visible = true;
			dgvHistory.Visible = true;
		}
		else if (mType.Equals(typeof(frmCommentView)))
		{
			dgvOther.Visible = true;
		}
		else if (mType.Equals(typeof(frmCompleteView)))
		{
			lblHistory.Visible = true;
			dgvHistory.Visible = true;
			btnUp.Visible = false;
			btnDown.Visible = false;
			btnConfirm.Visible = false;
		}
	}

	public void setIdRequest(Guid id)
	{
		mIdRequest = id;
	}

	private void load_dgvHistory()
	{
		if (mType.Equals(typeof(frmChartView)))
		{
			mUnit = mDataGridView.CurrentRow.Cells["Unit"].Value?.ToString();
		}
		else
		{
			mUnit = mDataGridView.CurrentRow.Cells["MeasurementUnit"].Value?.ToString();
		}
		if (mDataGridView.CurrentRow.Cells["History"].Value != null)
		{
			DataTable dataTable = Common.getDataTable<HistoryViewModel>(mDataGridView.CurrentRow.Cells["History"].Value.ToString());
			dgvHistory.DataSource = Common.reverseDatatable(dataTable);
		}
		dgvHistory.Refresh();
		dgvHistory_Sorted(dgvHistory, null);
	}

	private void load_dgvHistory(RequestResultViewModel item)
	{
		mUnit = item.MeasurementUnit;
		if (!string.IsNullOrEmpty(item.History))
		{
			DataTable dataTable = Common.getDataTable<HistoryViewModel>(item.History);
			dgvHistory.DataSource = Common.reverseDatatable(dataTable);
		}
		dgvHistory.Refresh();
		dgvHistory_Sorted(dgvHistory, null);
	}

	public void load_dgvContent(Enum type)
	{
		dgvContent.Rows.Clear();
		dgvOther.Rows.Clear();
		dgvFooter.Rows.Clear();
		try
		{
			List<string> list = MetaType.mPanelOther.Cast<string>().ToList();
			if (mType.Equals(typeof(frmResultView)))
			{
				list.RemoveAll((string x) => x.Equals("Code") || x.Equals("name"));
			}
			else if (mForm is frmSubView { mFrmType: (FormType)6 })
			{
				list.Add("Value");
			}
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
						lblTitle.Text = Common.getTextLanguage(this, "VIEW");
						mId = (string.IsNullOrEmpty(mDataGridView.CurrentRow.Cells["Id"].Value.ToString()) ? Guid.Empty : Guid.Parse(mDataGridView.CurrentRow.Cells["Id"].Value.ToString()));
						lblValue.Text = (mId.Equals(Guid.Empty) ? string.Empty : mId.ToString());
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
						lblTitle.Text = Common.getTextLanguage(this, "ADD");
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
						switch (col.Name)
						{
						case "Code":
							dgvContent.Rows.Add(col.Name, col.HeaderText, set_Code());
							dgvContent.Rows[dgvContent.RowCount - 1].Cells["ContentValue"].ReadOnly = true;
							break;
						case "MachineName":
							dgvContent.Rows.Add(col.Name, col.HeaderText, (mDataGridView.CurrentCell == null) ? string.Empty : mDataGridView.CurrentRow.Cells[col.Name].Value);
							dgvContent.Rows[dgvContent.RowCount - 1].Cells["ContentValue"].ReadOnly = true;
							break;
						case "StaffName":
							dgvContent.Rows.Add(col.Name, col.HeaderText, (mDataGridView.CurrentCell == null) ? string.Empty : mDataGridView.CurrentRow.Cells[col.Name].Value);
							dgvContent.Rows[dgvContent.RowCount - 1].Cells["ContentValue"].ReadOnly = true;
							break;
						case "Judge":
							dgvContent.Rows.Add(col.Name, col.HeaderText, (mDataGridView.CurrentCell == null) ? string.Empty : mDataGridView.CurrentRow.Cells[col.Name].Value);
							dgvContent.Rows[dgvContent.RowCount - 1].Cells["ContentValue"].ReadOnly = true;
							break;
						default:
							dgvContent.Rows.Add(col.Name, col.HeaderText, (mDataGridView.CurrentCell == null) ? string.Empty : mDataGridView.CurrentRow.Cells[col.Name].Value);
							dgvContent.Rows[dgvContent.RowCount - 1].Cells["ContentValue"].ReadOnly = false;
							break;
						}
						continue;
					}
				}
				lblTitle.Text = Common.getTextLanguage(this, "EDIT");
				btnConfirm.Visible = true;
				mId = (string.IsNullOrEmpty(mDataGridView.CurrentRow.Cells["Id"].Value.ToString()) ? Guid.Empty : Guid.Parse(mDataGridView.CurrentRow.Cells["Id"].Value.ToString()));
				lblValue.Text = (mId.Equals(Guid.Empty) ? string.Empty : mId.ToString());
				dgvOther.Columns["OtherFolder"].Visible = true;
				if (MetaType.dgvFooter.Find((string x) => x.Equals(col.Name)) != null)
				{
					dgvFooter.Rows.Add(col.Name, col.HeaderText, mDataGridView.CurrentRow.Cells[col.Name].Value);
					continue;
				}
				if (MetaType.dgvOther.Find((string x) => x.Equals(col.Name)) != null)
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
					continue;
				}
				dgvContent.MultiSelect = false;
				dgvContent.Rows.Add(col.Name, col.HeaderText, mDataGridView.CurrentRow.Cells[col.Name].Value);
				switch (col.Name)
				{
				case "Code":
					dgvContent.Rows[dgvContent.RowCount - 1].Cells["ContentValue"].ReadOnly = true;
					break;
				case "MachineName":
					dgvContent.Rows[dgvContent.RowCount - 1].Cells["ContentValue"].ReadOnly = true;
					break;
				case "StaffName":
					dgvContent.Rows[dgvContent.RowCount - 1].Cells["ContentValue"].ReadOnly = true;
					break;
				case "Judge":
					dgvContent.Rows[dgvContent.RowCount - 1].Cells["ContentValue"].ReadOnly = true;
					break;
				default:
					dgvContent.Rows[dgvContent.RowCount - 1].Cells["ContentValue"].ReadOnly = false;
					break;
				}
			}
			if (mType.Equals(typeof(frmResultView)) || mType.Equals(typeof(frmHistoryView)) || mType.Equals(typeof(frmChartView)))
			{
				load_dgvHistory();
			}
		}
		finally
		{
			dgvContent.Size = new Size(base.Width, dgvContent.Rows.Count * 22 + 3);
			dgvOther.Size = new Size(base.Width, dgvOther.Rows.Count * 22 + 3);
			dgvFooter.Size = new Size(base.Width, dgvFooter.Rows.Count * 22 + 3);
			dgvHistory.Size = new Size(base.Width, 22 + dgvHistory.Rows.Count * 22 + 3);
			dgvContent.CurrentCell = null;
			dgvContent.Refresh();
			dgvOther.CurrentCell = null;
			dgvOther.Refresh();
			dgvFooter.CurrentCell = null;
			dgvFooter.Refresh();
			dgvHistory.CurrentCell = null;
			dgvHistory.Refresh();
			dgvHistory.BringToFront();
		}
	}

	public void load_dgvContent(RequestResultViewModel item)
	{
		dgvContent.Rows.Clear();
		dgvFooter.Rows.Clear();
		try
		{
			lblValue.Text = ((AuditableEntity)(object)item).Id.ToString();
			List<string> list = MetaType.mPanelOther.Cast<string>().ToList();
			foreach (string colName in list)
			{
				PropertyInfo property = ((object)item).GetType().GetProperty(colName);
				if (!(property != null))
				{
					continue;
				}
				if (MetaType.dgvFooter.Find((string x) => x.Equals(colName)) != null)
				{
					dgvFooter.Rows.Add(colName, Common.getTextLanguage(this, colName), property.GetValue(item, null));
					continue;
				}
				object obj = property.GetValue(item, null);
				if (item.MeasurementUnit == "°" && colName == "Result" && obj != null)
				{
					obj = Common.ConvertDoubleToDegrees(double.Parse(obj.ToString()));
				}
				dgvContent.Rows.Add(colName, Common.getTextLanguage(this, colName), obj);
			}
			load_dgvHistory(item);
		}
		finally
		{
			dgvContent.Size = new Size(base.Width, dgvContent.Rows.Count * 22 + 3);
			dgvFooter.Size = new Size(base.Width, dgvFooter.Rows.Count * 22 + 3);
			dgvHistory.Size = new Size(base.Width, 22 + dgvHistory.Rows.Count * 22 + 3);
			dgvContent.CurrentCell = null;
			dgvContent.Refresh();
			dgvFooter.CurrentCell = null;
			dgvFooter.Refresh();
			dgvHistory.CurrentCell = null;
			dgvHistory.Refresh();
			dgvHistory.BringToFront();
		}
	}

	private string set_Code()
	{
		string text = string.Empty;
		try
		{
			frmSubView frmSubView = mForm as frmSubView;
			QueryArgs queryArgs = new QueryArgs
			{
				Predicate = "TypeId=@0",
				Order = "Created DESC",
				Page = 1,
				Limit = 1
			};
			switch (frmSubView.mFrmType)
			{
			case (FormType)4:
				queryArgs.PredicateParameters = new string[1] { "55630EBA-6A11-4001-B161-9AE77ACCA43D" };
				text = "DEP-";
				break;
			case (FormType)5:
				queryArgs.PredicateParameters = new string[1] { "5EB07BDB-9086-4BC5-A02B-5D6E9CFCD476" };
				text = "FAC-";
				break;
			case (FormType)6:
				queryArgs.PredicateParameters = new string[1] { "438D7052-25F3-4342-ED0C-08D7E9C5C77D" };
				text = "MTYPE-";
				break;
			case (FormType)7:
				queryArgs.PredicateParameters = new string[1] { "6042BF53-9411-47D4-9BD6-F8AB7BABB663" };
				text = "IMP-";
				break;
			case (FormType)8:
				queryArgs.PredicateParameters = new string[1] { "7CA6130A-00D1-40CE-ED0F-08D7E9C5C77D" };
				text = "UNIT-";
				break;
			case (FormType)9:
				queryArgs.PredicateParameters = new string[1] { "11C5FD56-AD45-4457-8DC9-6C8D9F6673A1" };
				text = "STA-";
				break;
			case (FormType)10:
				queryArgs.PredicateParameters = new string[1] { "AC5FA813-C9EE-4805-A850-30A5EA5AB0A1" };
				text = "TYPE-";
				break;
			case (FormType)11:
				queryArgs.PredicateParameters = new string[1] { "AC5FA814-C9EE-4807-A851-30A5EA5AB0A2" };
				text = "LINE-";
				break;
			case (FormType)12:
				queryArgs.PredicateParameters = new string[1] { "AC5FA815-C9EE-4807-A852-30A5EA5AB0A3" };
				text = "TYPECAL-";
				break;
			}
			ResponseDto result = frmLogin.client.GetsAsync(queryArgs, "/api/MetadataValue/Gets").Result;
			DataTable dataTable = Common.getDataTable<MetadataValueViewModel>(result);
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
		dataGridViewTextBoxEditingControl.KeyPress -= txtNormal_KeyPress;
		dataGridViewTextBoxEditingControl.KeyUp -= txtNormal_KeyUp;
		dataGridViewTextBoxEditingControl.Leave -= txtTemp_Leave;
		dataGridViewTextBoxEditingControl.MaxLength = int.MaxValue;
		dataGridViewTextBoxEditingControl.KeyPress -= txtDecimal_KeyPress;
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
		case "Content":
			dataGridViewTextBoxEditingControl.AutoCompleteCustomSource = Common.getAutoComplete((DataTable)mDataGridView.DataSource, "Content");
			break;
		case "Result":
		{
			if (double.TryParse(mDataGridView.CurrentRow.Cells["Value"].Value.ToString(), out var _))
			{
				dataGridViewTextBoxEditingControl.AutoCompleteCustomSource = Common.getAutoComplete((DataTable)mDataGridView.DataSource, "Result");
				dataGridViewTextBoxEditingControl.KeyPress += txtNormal_KeyPress;
				dataGridViewTextBoxEditingControl.KeyUp += txtNormal_KeyUp;
			}
			else
			{
				dataGridViewTextBoxEditingControl.AutoCompleteCustomSource = new AutoCompleteStringCollection { "OK", "IN", "GO", "NOGO", "NG", "STOP" };
			}
			dataGridViewTextBoxEditingControl.Leave += txtTemp_Leave;
			break;
		}
		case "Value":
			dataGridViewTextBoxEditingControl.AutoCompleteCustomSource = Common.getAutoComplete((DataTable)mDataGridView.DataSource, "Value");
			dataGridViewTextBoxEditingControl.KeyPress += txtDecimal_KeyPress;
			dataGridViewTextBoxEditingControl.MaxLength = 1;
			break;
		}
	}

	private void txtNormal_KeyPress(object sender, KeyPressEventArgs e)
	{
		DataGridViewTextBoxEditingControl dataGridViewTextBoxEditingControl = sender as DataGridViewTextBoxEditingControl;
		if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b' && e.KeyChar != '.' && e.KeyChar != '-')
		{
			e.Handled = true;
		}
		if (e.KeyChar != '.' && ((dataGridViewTextBoxEditingControl.Text.StartsWith("0") && dataGridViewTextBoxEditingControl.Text.Length.Equals(1)) || (dataGridViewTextBoxEditingControl.Text.StartsWith("-0") && dataGridViewTextBoxEditingControl.Text.Length.Equals(2))))
		{
			dataGridViewTextBoxEditingControl.Text = dataGridViewTextBoxEditingControl.Text.Replace("0", "");
			dataGridViewTextBoxEditingControl.Select(dataGridViewTextBoxEditingControl.Text.Length, 0);
		}
		if (e.KeyChar == '.' && dataGridViewTextBoxEditingControl.Text.Contains("."))
		{
			e.Handled = true;
		}
		if (e.KeyChar == '-' && (!dataGridViewTextBoxEditingControl.SelectionStart.Equals(0) || dataGridViewTextBoxEditingControl.Text.Contains("-")))
		{
			e.Handled = true;
		}
	}

	private void txtNormal_KeyUp(object sender, KeyEventArgs e)
	{
		DataGridViewTextBoxEditingControl dataGridViewTextBoxEditingControl = sender as DataGridViewTextBoxEditingControl;
		string text = dataGridViewTextBoxEditingControl.Text.Split('.')[0];
		if (text.Length > 3 && !dataGridViewTextBoxEditingControl.Text.Contains("."))
		{
			dataGridViewTextBoxEditingControl.Text = double.Parse(text).ToString("#,###");
		}
		dataGridViewTextBoxEditingControl.Select(dataGridViewTextBoxEditingControl.Text.Length, 0);
	}

	private void txtTemp_Leave(object sender, EventArgs e)
	{
		DataGridViewTextBoxEditingControl dataGridViewTextBoxEditingControl = sender as DataGridViewTextBoxEditingControl;
		if (double.TryParse(mDataGridView.CurrentRow.Cells["Value"].Value.ToString(), out var result))
		{
			dataGridViewTextBoxEditingControl.Text = (double.TryParse(dataGridViewTextBoxEditingControl.Text, out result) ? result.ToString("#,##0.##########") : null);
		}
		else if (dataGridViewTextBoxEditingControl.Text.ToUpper().Equals("OK") || dataGridViewTextBoxEditingControl.Text.ToUpper().Equals("NG"))
		{
			dataGridViewTextBoxEditingControl.Text = dataGridViewTextBoxEditingControl.Text.ToUpper();
		}
	}

	private void txtDecimal_KeyPress(object sender, KeyPressEventArgs e)
	{
		DataGridViewTextBoxEditingControl dataGridViewTextBoxEditingControl = sender as DataGridViewTextBoxEditingControl;
		if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b')
		{
			e.Handled = true;
		}
	}

	private void dgvContent_CellEndEdit(object sender, DataGridViewCellEventArgs e)
	{
		DataGridView dataGridView = sender as DataGridView;
		if (dataGridView.CurrentCell.Value != null)
		{
			dataGridView.CurrentCell.Value = Common.trimSpace(dataGridView.CurrentCell.Value.ToString());
		}
	}

	private void btnConfirm_Click(object sender, EventArgs e)
	{
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Expected O, but got Unknown
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_033c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0343: Expected O, but got Unknown
		//IL_0346: Expected O, but got Unknown
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
					case "Content":
						MessageBox.Show(Common.getTextLanguage(this, "wContent"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						item.Cells["ContentValue"].Selected = true;
						dgvContent.BeginEdit(selectAll: true);
						return;
					}
				}
			}
			if (mType.Equals(typeof(frmResultView)))
			{
				frmResultView frmResultView = mForm as frmResultView;
				RequestResultViewModel val = new RequestResultViewModel();
				((AuditableEntity)val).Id = mId;
				val.RequestId = mIdRequest;
				val.MeasurementId = (string.IsNullOrEmpty(mDataGridView.CurrentRow.Cells["MeasurementId"].Value.ToString()) ? Guid.Empty : Guid.Parse(mDataGridView.CurrentRow.Cells["MeasurementId"].Value.ToString()));
				val.MachineName = "Manual Input";
				val.StaffName = frmLogin.User.FullName;
				val.Sample = (string.IsNullOrEmpty(mDataGridView.CurrentRow.Cells["Sample"].Value.ToString()) ? 1 : int.Parse(mDataGridView.CurrentRow.Cells["Sample"].Value.ToString()));
				val.Cavity = (string.IsNullOrEmpty(mDataGridView.CurrentRow.Cells["Cavity"].Value.ToString()) ? 1 : int.Parse(mDataGridView.CurrentRow.Cells["Cavity"].Value.ToString()));
				((AuditableEntity)val).IsActivated = true;
				RequestResultViewModel val2 = val;
				foreach (DataGridViewRow item2 in (IEnumerable)dgvContent.Rows)
				{
					if (item2.Cells["ContentValue"].Value == null || string.IsNullOrEmpty(item2.Cells["ContentValue"].Value.ToString()))
					{
						continue;
					}
					object value2 = item2.Cells["ContentName"].Value;
					object obj2 = value2;
					if (!(obj2 is string text) || !(text == "Result"))
					{
						continue;
					}
					val2.Result = item2.Cells["ContentValue"].Value.ToString();
					if (mDataGridView.CurrentRow.Cells["MeasurementUnit"].Value.ToString() == "°")
					{
						double? num = Common.ConvertDegreesToDouble(val2.Result);
						if (num.HasValue)
						{
							val2.Result = num.ToString();
						}
					}
				}
				if (mId.Equals(Guid.Empty) || !MessageBox.Show(Common.getTextLanguage(this, "wSureEdit"), Common.getTextLanguage(this, "QUESTION"), MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.No))
				{
					Cursor.Current = Cursors.WaitCursor;
					ResponseDto result = frmLogin.client.SaveAsync(val2, "/api/RequestResult/Save").Result;
					if (!result.Success)
					{
						throw new Exception(result.Messages.ElementAt(0).Message);
					}
					frmCommentView frmCommentView = new frmCommentView(frmResultView, mIdRequest)
					{
						Size = new Size(600, 400)
					};
					frmCommentView.ShowDialog();
					frmResultView.load_AllData();
				}
				return;
			}
			if (mType.Equals(typeof(frmCommentView)))
			{
				frmCommentView frmCommentView2 = mForm as frmCommentView;
				CommentViewModel commentViewModel = new CommentViewModel
				{
					Id = mId,
					RequestId = mIdRequest,
					IsActivated = true
				};
				foreach (DataGridViewRow item3 in (IEnumerable)dgvContent.Rows)
				{
					if (item3.Cells["ContentValue"].Value != null && !string.IsNullOrEmpty(item3.Cells["ContentValue"].Value.ToString()))
					{
						object value3 = item3.Cells["ContentName"].Value;
						object obj3 = value3;
						if (obj3 is string text2 && text2 == "Content")
						{
							commentViewModel.Content = item3.Cells["ContentValue"].Value.ToString();
						}
					}
				}
				if (!mId.Equals(Guid.Empty) && MessageBox.Show(Common.getTextLanguage(this, "wSureEdit"), Common.getTextLanguage(this, "QUESTION"), MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.No))
				{
					return;
				}
				Cursor.Current = Cursors.WaitCursor;
				string text3 = string.Empty;
				if (dgvOther.Rows[0].Cells["OtherValue"].Value != null && !string.IsNullOrEmpty(dgvOther.Rows[0].Cells["OtherValue"].Value.ToString()))
				{
					text3 = dgvOther.Rows[0].Cells["OtherValue"].Value.ToString();
				}
				if (File.Exists(text3))
				{
					using (File.Open(text3, FileMode.Open))
					{
					}
				}
				if (!mId.Equals(Guid.Empty))
				{
					commentViewModel.Link = mFileName;
				}
				ResponseDto result2 = frmLogin.client.SaveAsync(commentViewModel, "/api/Comment/Save").Result;
				if (!result2.Success)
				{
					throw new Exception(result2.Messages.ElementAt(0).Message);
				}
				if (mId.Equals(Guid.Empty))
				{
					CommentViewModel commentViewModel2 = JsonConvert.DeserializeObject<CommentViewModel>(result2.Data.ToString());
					if (File.Exists(text3))
					{
						using FileStream data = File.OpenRead(text3);
						FileParameter file = new FileParameter(data, text3);
						ResponseDto result3 = frmLogin.client.ImportAsync(commentViewModel2.Id, file, "/api/Comment/UpdateFile/{id}").Result;
						if (!result3.Success)
						{
							MessageBox.Show(result3.Messages.First().Message, result3.Messages.First().Code, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						}
					}
				}
				else if (File.Exists(text3))
				{
					using FileStream data2 = File.OpenRead(text3);
					FileParameter file2 = new FileParameter(data2, text3);
					ResponseDto result4 = frmLogin.client.ImportAsync(mId, file2, "/api/Comment/UpdateFile/{id}").Result;
					if (!result4.Success)
					{
						MessageBox.Show(result4.Messages.First().Message, result4.Messages.First().Code, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
				}
				else if (string.IsNullOrEmpty(text3))
				{
					Stream data3 = new MemoryStream();
					FileParameter file3 = new FileParameter(data3);
					ResponseDto result5 = frmLogin.client.ImportAsync(mId, file3, "/api/Comment/UpdateFile/{id}").Result;
					if (!result5.Success)
					{
						MessageBox.Show(result5.Messages.First().Message, result5.Messages.First().Code, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
				}
				frmCommentView2.load_AllData();
				return;
			}
			frmSubView frmSubView = mForm as frmSubView;
			MetadataValueViewModel metadataValueViewModel = new MetadataValueViewModel
			{
				Id = mId,
				IsActivated = true
			};
			switch (frmSubView.mFrmType)
			{
			case (FormType)4:
				metadataValueViewModel.TypeId = new Guid("55630EBA-6A11-4001-B161-9AE77ACCA43D");
				break;
			case (FormType)5:
				metadataValueViewModel.TypeId = new Guid("5EB07BDB-9086-4BC5-A02B-5D6E9CFCD476");
				break;
			case (FormType)6:
				metadataValueViewModel.TypeId = new Guid("438D7052-25F3-4342-ED0C-08D7E9C5C77D");
				break;
			case (FormType)7:
				metadataValueViewModel.TypeId = new Guid("6042BF53-9411-47D4-9BD6-F8AB7BABB663");
				break;
			case (FormType)8:
				metadataValueViewModel.TypeId = new Guid("7CA6130A-00D1-40CE-ED0F-08D7E9C5C77D");
				break;
			case (FormType)9:
				metadataValueViewModel.TypeId = new Guid("11C5FD56-AD45-4457-8DC9-6C8D9F6673A1");
				break;
			case (FormType)10:
				metadataValueViewModel.TypeId = new Guid("AC5FA813-C9EE-4805-A850-30A5EA5AB0A1");
				break;
			case (FormType)11:
				metadataValueViewModel.TypeId = new Guid("AC5FA814-C9EE-4807-A851-30A5EA5AB0A2");
				break;
			case (FormType)12:
				metadataValueViewModel.TypeId = new Guid("AC5FA815-C9EE-4807-A852-30A5EA5AB0A3");
				break;
			}
			foreach (DataGridViewRow item4 in (IEnumerable)dgvContent.Rows)
			{
				if (item4.Cells["ContentValue"].Value != null && !string.IsNullOrEmpty(item4.Cells["ContentValue"].Value.ToString()))
				{
					object value4 = item4.Cells["ContentName"].Value;
					object obj4 = value4;
					switch (obj4 as string)
					{
					case "Code":
						metadataValueViewModel.Code = item4.Cells["ContentValue"].Value.ToString();
						break;
					case "name":
						metadataValueViewModel.Name = item4.Cells["ContentValue"].Value.ToString();
						break;
					case "Description":
						metadataValueViewModel.Description = item4.Cells["ContentValue"].Value.ToString();
						break;
					case "Value":
						metadataValueViewModel.Value = item4.Cells["ContentValue"].Value.ToString();
						break;
					}
				}
			}
			if (mId.Equals(Guid.Empty) || !MessageBox.Show(Common.getTextLanguage(this, "wSureEdit"), Common.getTextLanguage(this, "QUESTION"), MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.No))
			{
				Cursor.Current = Cursors.WaitCursor;
				ResponseDto result6 = frmLogin.client.SaveAsync(metadataValueViewModel, "/api/MetadataValue/Save").Result;
				if (!result6.Success)
				{
					throw new Exception(result6.Messages.ElementAt(0).Message);
				}
				frmSubView.isClose = false;
				frmSubView.load_AllData();
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
						mForm.Close();
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
			if (text == "OtherFolder" && openFileDialogMain.ShowDialog().Equals(DialogResult.OK))
			{
				dataGridView.CurrentRow.Cells["OtherValue"].Value = openFileDialogMain.FileName;
				dataGridView.Columns["OtherClear"].Visible = true;
			}
		}
		else
		{
			dataGridView.CurrentRow.Cells["OtherValue"].Value = null;
			dataGridView.Columns["OtherClear"].Visible = false;
		}
	}

	private void dgvHistory_Sorted(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		DataGridView dataGridView = sender as DataGridView;
		foreach (DataGridViewRow item in (IEnumerable)dataGridView.Rows)
		{
			item.Cells["his_no"].Value = item.Index + 1;
			if (mUnit == "°" && double.TryParse(item.Cells["his_value"].Value?.ToString(), out var result))
			{
				item.Cells["his_value"].Value = Common.ConvertDoubleToDegrees(result);
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
		this.tpanelTitle = new System.Windows.Forms.TableLayoutPanel();
		this.lblValue = new System.Windows.Forms.Label();
		this.btnDown = new System.Windows.Forms.Button();
		this.btnUp = new System.Windows.Forms.Button();
		this.lblTitle = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.btnConfirm = new System.Windows.Forms.Button();
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.panelResize = new System.Windows.Forms.Panel();
		this.panelView = new System.Windows.Forms.Panel();
		this.dgvHistory = new System.Windows.Forms.DataGridView();
		this.his_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.his_value = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.his_staff = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.his_machine = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.his_created = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.lblHistory = new System.Windows.Forms.Label();
		this.dgvFooter = new System.Windows.Forms.DataGridView();
		this.FooterName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.FooterTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.FooterValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dgvOther = new System.Windows.Forms.DataGridView();
		this.OtherName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.OtherTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.OtherValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.OtherClear = new System.Windows.Forms.DataGridViewButtonColumn();
		this.OtherFolder = new System.Windows.Forms.DataGridViewButtonColumn();
		this.dgvContent = new System.Windows.Forms.DataGridView();
		this.ContentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ContentTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ContentValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.openFileDialogMain = new System.Windows.Forms.OpenFileDialog();
		this.tpanelTitle.SuspendLayout();
		this.panelView.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.dgvFooter).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.dgvOther).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.dgvContent).BeginInit();
		base.SuspendLayout();
		this.tpanelTitle.AutoSize = true;
		this.tpanelTitle.ColumnCount = 4;
		this.tpanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tpanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelTitle.Controls.Add(this.lblValue, 1, 0);
		this.tpanelTitle.Controls.Add(this.btnDown, 3, 0);
		this.tpanelTitle.Controls.Add(this.btnUp, 2, 0);
		this.tpanelTitle.Controls.Add(this.lblTitle, 0, 0);
		this.tpanelTitle.Dock = System.Windows.Forms.DockStyle.Top;
		this.tpanelTitle.Location = new System.Drawing.Point(0, 0);
		this.tpanelTitle.Margin = new System.Windows.Forms.Padding(0);
		this.tpanelTitle.Name = "tpanelTitle";
		this.tpanelTitle.RowCount = 1;
		this.tpanelTitle.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tpanelTitle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27f));
		this.tpanelTitle.Size = new System.Drawing.Size(400, 27);
		this.tpanelTitle.TabIndex = 134;
		this.lblValue.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblValue.ForeColor = System.Drawing.Color.Red;
		this.lblValue.Location = new System.Drawing.Point(43, 1);
		this.lblValue.Margin = new System.Windows.Forms.Padding(1);
		this.lblValue.Name = "lblValue";
		this.lblValue.Size = new System.Drawing.Size(308, 25);
		this.lblValue.TabIndex = 132;
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
		this.btnDown.TabIndex = 129;
		this.btnDown.TabStop = false;
		this.toolTipMain.SetToolTip(this.btnDown, "Display lower row item");
		this.btnDown.UseVisualStyleBackColor = false;
		this.btnDown.Click += new System.EventHandler(btnDown_Click);
		this.btnUp.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnUp.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnUp.FlatAppearance.BorderSize = 0;
		this.btnUp.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.AppWorkspace;
		this.btnUp.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
		this.btnUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUp.Image = _5S_QA_System.Properties.Resources.arrow_up;
		this.btnUp.Location = new System.Drawing.Point(353, 1);
		this.btnUp.Margin = new System.Windows.Forms.Padding(1);
		this.btnUp.Name = "btnUp";
		this.btnUp.Size = new System.Drawing.Size(22, 25);
		this.btnUp.TabIndex = 128;
		this.btnUp.TabStop = false;
		this.toolTipMain.SetToolTip(this.btnUp, "Display upper row item");
		this.btnUp.UseVisualStyleBackColor = false;
		this.btnUp.Click += new System.EventHandler(btnUp_Click);
		this.lblTitle.AutoSize = true;
		this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTitle.Location = new System.Drawing.Point(1, 1);
		this.lblTitle.Margin = new System.Windows.Forms.Padding(1);
		this.lblTitle.Name = "lblTitle";
		this.lblTitle.Size = new System.Drawing.Size(40, 25);
		this.lblTitle.TabIndex = 0;
		this.lblTitle.Text = "View";
		this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 27);
		this.panel1.Margin = new System.Windows.Forms.Padding(0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(400, 1);
		this.panel1.TabIndex = 135;
		this.btnConfirm.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnConfirm.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.btnConfirm.FlatAppearance.BorderSize = 0;
		this.btnConfirm.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
		this.btnConfirm.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
		this.btnConfirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnConfirm.Location = new System.Drawing.Point(0, 310);
		this.btnConfirm.Margin = new System.Windows.Forms.Padding(1);
		this.btnConfirm.Name = "btnConfirm";
		this.btnConfirm.Size = new System.Drawing.Size(400, 28);
		this.btnConfirm.TabIndex = 4;
		this.btnConfirm.Text = "Confirm";
		this.toolTipMain.SetToolTip(this.btnConfirm, "Confirm");
		this.btnConfirm.UseVisualStyleBackColor = true;
		this.btnConfirm.Click += new System.EventHandler(btnConfirm_Click);
		this.panelResize.Dock = System.Windows.Forms.DockStyle.Left;
		this.panelResize.Location = new System.Drawing.Point(0, 28);
		this.panelResize.Margin = new System.Windows.Forms.Padding(1);
		this.panelResize.Name = "panelResize";
		this.panelResize.Size = new System.Drawing.Size(3, 282);
		this.panelResize.TabIndex = 137;
		this.panelView.AutoScroll = true;
		this.panelView.Controls.Add(this.dgvHistory);
		this.panelView.Controls.Add(this.lblHistory);
		this.panelView.Controls.Add(this.dgvFooter);
		this.panelView.Controls.Add(this.dgvOther);
		this.panelView.Controls.Add(this.dgvContent);
		this.panelView.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panelView.Location = new System.Drawing.Point(3, 28);
		this.panelView.Name = "panelView";
		this.panelView.Size = new System.Drawing.Size(397, 282);
		this.panelView.TabIndex = 142;
		this.dgvHistory.AllowUserToAddRows = false;
		this.dgvHistory.AllowUserToDeleteRows = false;
		this.dgvHistory.AllowUserToOrderColumns = true;
		this.dgvHistory.AllowUserToResizeRows = false;
		dataGridViewCellStyle.BackColor = System.Drawing.SystemColors.Control;
		this.dgvHistory.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle;
		this.dgvHistory.BackgroundColor = System.Drawing.SystemColors.Window;
		dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.AppWorkspace;
		dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
		dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
		dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
		dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.dgvHistory.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
		this.dgvHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
		this.dgvHistory.Columns.AddRange(this.his_no, this.his_value, this.his_staff, this.his_machine, this.his_created);
		this.dgvHistory.Dock = System.Windows.Forms.DockStyle.Top;
		this.dgvHistory.EnableHeadersVisualStyles = false;
		this.dgvHistory.Location = new System.Drawing.Point(0, 91);
		this.dgvHistory.Margin = new System.Windows.Forms.Padding(1);
		this.dgvHistory.Name = "dgvHistory";
		this.dgvHistory.ReadOnly = true;
		this.dgvHistory.RowHeadersVisible = false;
		this.dgvHistory.ScrollBars = System.Windows.Forms.ScrollBars.None;
		this.dgvHistory.Size = new System.Drawing.Size(397, 119);
		this.dgvHistory.TabIndex = 3;
		this.dgvHistory.Visible = false;
		this.dgvHistory.Sorted += new System.EventHandler(dgvHistory_Sorted);
		this.dgvHistory.Leave += new System.EventHandler(dgvNormal_Leave);
		this.his_no.DataPropertyName = "no";
		dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		this.his_no.DefaultCellStyle = dataGridViewCellStyle3;
		this.his_no.HeaderText = "No";
		this.his_no.Name = "his_no";
		this.his_no.ReadOnly = true;
		this.his_no.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.his_no.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.his_no.Width = 35;
		this.his_value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.his_value.DataPropertyName = "value";
		dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.his_value.DefaultCellStyle = dataGridViewCellStyle4;
		this.his_value.FillWeight = 20f;
		this.his_value.HeaderText = "Value";
		this.his_value.Name = "his_value";
		this.his_value.ReadOnly = true;
		this.his_staff.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.his_staff.DataPropertyName = "createdby";
		this.his_staff.FillWeight = 30f;
		this.his_staff.HeaderText = "Staff";
		this.his_staff.Name = "his_staff";
		this.his_staff.ReadOnly = true;
		this.his_machine.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.his_machine.DataPropertyName = "machinename";
		this.his_machine.FillWeight = 30f;
		this.his_machine.HeaderText = "Machine";
		this.his_machine.Name = "his_machine";
		this.his_machine.ReadOnly = true;
		this.his_created.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.his_created.DataPropertyName = "created";
		this.his_created.FillWeight = 40f;
		this.his_created.HeaderText = "Created";
		this.his_created.Name = "his_created";
		this.his_created.ReadOnly = true;
		this.lblHistory.Dock = System.Windows.Forms.DockStyle.Top;
		this.lblHistory.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblHistory.Location = new System.Drawing.Point(0, 66);
		this.lblHistory.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblHistory.Name = "lblHistory";
		this.lblHistory.Size = new System.Drawing.Size(397, 25);
		this.lblHistory.TabIndex = 144;
		this.lblHistory.Text = "History";
		this.lblHistory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblHistory.Visible = false;
		this.dgvFooter.AllowUserToAddRows = false;
		this.dgvFooter.AllowUserToDeleteRows = false;
		this.dgvFooter.AllowUserToResizeColumns = false;
		this.dgvFooter.AllowUserToResizeRows = false;
		this.dgvFooter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		this.dgvFooter.ColumnHeadersVisible = false;
		this.dgvFooter.Columns.AddRange(this.FooterName, this.FooterTitle, this.FooterValue);
		this.dgvFooter.Dock = System.Windows.Forms.DockStyle.Top;
		this.dgvFooter.Location = new System.Drawing.Point(0, 44);
		this.dgvFooter.Margin = new System.Windows.Forms.Padding(1);
		this.dgvFooter.Name = "dgvFooter";
		this.dgvFooter.ReadOnly = true;
		this.dgvFooter.RowHeadersVisible = false;
		this.dgvFooter.ScrollBars = System.Windows.Forms.ScrollBars.None;
		this.dgvFooter.Size = new System.Drawing.Size(397, 22);
		this.dgvFooter.TabIndex = 2;
		this.dgvFooter.Leave += new System.EventHandler(dgvNormal_Leave);
		this.FooterName.HeaderText = "Name";
		this.FooterName.Name = "FooterName";
		this.FooterName.ReadOnly = true;
		this.FooterName.Visible = false;
		dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.FooterTitle.DefaultCellStyle = dataGridViewCellStyle5;
		this.FooterTitle.HeaderText = "Title";
		this.FooterTitle.Name = "FooterTitle";
		this.FooterTitle.ReadOnly = true;
		this.FooterTitle.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.FooterTitle.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.FooterTitle.Width = 120;
		this.FooterValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		this.FooterValue.DefaultCellStyle = dataGridViewCellStyle6;
		this.FooterValue.HeaderText = "Value";
		this.FooterValue.Name = "FooterValue";
		this.FooterValue.ReadOnly = true;
		this.FooterValue.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.FooterValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.dgvOther.AllowUserToAddRows = false;
		this.dgvOther.AllowUserToDeleteRows = false;
		this.dgvOther.AllowUserToResizeColumns = false;
		this.dgvOther.AllowUserToResizeRows = false;
		this.dgvOther.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		this.dgvOther.ColumnHeadersVisible = false;
		this.dgvOther.Columns.AddRange(this.OtherName, this.OtherTitle, this.OtherValue, this.OtherClear, this.OtherFolder);
		this.dgvOther.Dock = System.Windows.Forms.DockStyle.Top;
		this.dgvOther.Location = new System.Drawing.Point(0, 22);
		this.dgvOther.Margin = new System.Windows.Forms.Padding(1);
		this.dgvOther.Name = "dgvOther";
		this.dgvOther.ReadOnly = true;
		this.dgvOther.RowHeadersVisible = false;
		this.dgvOther.ScrollBars = System.Windows.Forms.ScrollBars.None;
		this.dgvOther.Size = new System.Drawing.Size(397, 22);
		this.dgvOther.TabIndex = 145;
		this.dgvOther.Visible = false;
		this.dgvOther.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvOther_CellClick);
		this.OtherName.HeaderText = "Name";
		this.OtherName.Name = "OtherName";
		this.OtherName.ReadOnly = true;
		this.OtherName.Visible = false;
		dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.OtherTitle.DefaultCellStyle = dataGridViewCellStyle7;
		this.OtherTitle.HeaderText = "Title";
		this.OtherTitle.Name = "OtherTitle";
		this.OtherTitle.ReadOnly = true;
		this.OtherTitle.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.OtherTitle.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.OtherTitle.Width = 120;
		this.OtherValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		this.OtherValue.DefaultCellStyle = dataGridViewCellStyle8;
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
		this.dgvContent.AllowUserToAddRows = false;
		this.dgvContent.AllowUserToDeleteRows = false;
		this.dgvContent.AllowUserToResizeColumns = false;
		this.dgvContent.AllowUserToResizeRows = false;
		this.dgvContent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		this.dgvContent.ColumnHeadersVisible = false;
		this.dgvContent.Columns.AddRange(this.ContentName, this.ContentTitle, this.ContentValue);
		this.dgvContent.Dock = System.Windows.Forms.DockStyle.Top;
		this.dgvContent.Location = new System.Drawing.Point(0, 0);
		this.dgvContent.Margin = new System.Windows.Forms.Padding(1);
		this.dgvContent.Name = "dgvContent";
		this.dgvContent.RowHeadersVisible = false;
		this.dgvContent.Size = new System.Drawing.Size(397, 22);
		this.dgvContent.TabIndex = 1;
		this.dgvContent.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(dgvContent_CellEndEdit);
		this.dgvContent.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(dgvContent_EditingControlShowing);
		this.ContentName.HeaderText = "Name";
		this.ContentName.Name = "ContentName";
		this.ContentName.Visible = false;
		dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.ContentTitle.DefaultCellStyle = dataGridViewCellStyle9;
		this.ContentTitle.HeaderText = "Title";
		this.ContentTitle.Name = "ContentTitle";
		this.ContentTitle.ReadOnly = true;
		this.ContentTitle.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.ContentTitle.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.ContentTitle.Width = 120;
		this.ContentValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		this.ContentValue.DefaultCellStyle = dataGridViewCellStyle10;
		this.ContentValue.FillWeight = 70f;
		this.ContentValue.HeaderText = "Value";
		this.ContentValue.Name = "ContentValue";
		this.ContentValue.ReadOnly = true;
		this.ContentValue.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.ContentValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.openFileDialogMain.FileName = "Template";
		this.openFileDialogMain.Filter = "All files (*.*)| *.*";
		this.openFileDialogMain.Title = "Select file";
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		base.Controls.Add(this.panelView);
		base.Controls.Add(this.panelResize);
		base.Controls.Add(this.btnConfirm);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.tpanelTitle);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Name = "mPanelOther";
		base.Size = new System.Drawing.Size(400, 338);
		base.Load += new System.EventHandler(mPanelOther_Load);
		this.tpanelTitle.ResumeLayout(false);
		this.tpanelTitle.PerformLayout();
		this.panelView.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.dgvHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.dgvFooter).EndInit();
		((System.ComponentModel.ISupportInitialize)this.dgvOther).EndInit();
		((System.ComponentModel.ISupportInitialize)this.dgvContent).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
