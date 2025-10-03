using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using _5S_QA_DL.APIContext;
using _5S_QA_Entities.Constants;
using _5S_QA_Entities.Dtos;
using _5S_QA_Entities.Enums;
using _5S_QA_Entities.Models;
using _5S_QA_System.Controls;
using _5S_QA_System.Properties;
using _5S_QA_System.View.User_control;
using MetroFramework.Forms;

namespace _5S_QA_System;

public class frmChartView : MetroForm
{
	private Guid mId;

	public bool isEdit;

	private int mRow;

	private int mCol;

	private IContainer components = null;

	private SaveFileDialog saveFileDialogMain;

	private OpenFileDialog openFileDialogMain;

	private ToolTip toolTipMain;

	private ToolStripProgressBar sprogbarStatus;

	public ToolStripStatusLabel slblStatus;

	private ContextMenuStrip contextMenuStripMain;

	private ToolStripMenuItem main_refreshToolStripMenuItem;

	private ToolStripSeparator toolStripSeparatorView;

	private ToolStripMenuItem main_viewToolStripMenuItem;

	private StatusStrip statusStripfrmMain;

	private mSearchChart mSearchMain;

	private mPanelOther mPanelViewMain;

	private TableLayoutPanel tpanelMeasurement;

	private Label lblWarning;

	private Label lblWarn;

	private Label lblEditResult;

	private Label lblEdit;

	private Label lblMeas;

	private Label lblOK;

	private Label lblNG;

	private Label lblEmpty;

	private Label lblJudgeOK;

	private Label lblJudgeNG;

	private Label lblJudgeEmpty;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripMenuItem enall_pageToolStripMenuItem;

	private ToolStripMenuItem unall_pageToolStripMenuItem;

	private TableLayoutPanel tpanelButtonView;

	public Button btnQualityReport;

	public Button btnHistogram;

	public Button btnXControlN1;

	public Button btnXtbControl;

	public Button btnXtbRControl;

	public Button btnXtbSControl;

	private mPanelButtonFilter mPanelButtonFilterMain;

	private DataGridView dgvMain;

	public Button btnInterpreting;

	private Panel panelLogout;

	private Label lblFullname;

	private PictureBox ptbAvatar;

	private TableLayoutPanel tpanelInterpreting;

	private Label label4;

	private Label lblInterpreting;

	private Label label8;

	private CheckBox cbXtb3;

	private CheckBox cbXtb2;

	private CheckBox cbXtb1;

	private CheckBox cbXtb4;

	private CheckBox cbXtb8;

	private CheckBox cbXtb7;

	private CheckBox cbXtb6;

	private CheckBox cbXtb5;

	private CheckBox cbR4;

	private CheckBox cbR3;

	private CheckBox cbR2;

	private CheckBox cbR1;

	private DataGridViewCheckBoxColumn IsSelect;

	private DataGridViewTextBoxColumn No;

	private DataGridViewTextBoxColumn RequestId;

	private DataGridViewTextBoxColumn RequestName;

	private DataGridViewTextBoxColumn RequestDate;

	private DataGridViewTextBoxColumn RequestSample;

	private DataGridViewTextBoxColumn Lot;

	private DataGridViewTextBoxColumn Line;

	private DataGridViewTextBoxColumn ProductId;

	private DataGridViewTextBoxColumn MeasurementId;

	private DataGridViewTextBoxColumn MeasurementCode;

	private DataGridViewTextBoxColumn MachineTypeName;

	private DataGridViewTextBoxColumn name;

	private DataGridViewTextBoxColumn Value;

	private DataGridViewTextBoxColumn Unit;

	private DataGridViewTextBoxColumn Upper;

	private DataGridViewTextBoxColumn Lower;

	private DataGridViewTextBoxColumn LSL;

	private DataGridViewTextBoxColumn USL;

	private DataGridViewTextBoxColumn UCL;

	private DataGridViewTextBoxColumn LCL;

	private DataGridViewTextBoxColumn Sample;

	private DataGridViewTextBoxColumn Cavity;

	private DataGridViewTextBoxColumn Result;

	private DataGridViewTextBoxColumn Judge;

	private DataGridViewTextBoxColumn MachineName;

	private DataGridViewTextBoxColumn StaffName;

	private DataGridViewTextBoxColumn History;

	private DataGridViewTextBoxColumn Id;

	private new DataGridViewTextBoxColumn Created;

	private DataGridViewTextBoxColumn Modified;

	private DataGridViewTextBoxColumn CreatedBy;

	private DataGridViewTextBoxColumn ModifiedBy;

	private DataGridViewTextBoxColumn IsActivated;

	public frmChartView()
	{
		InitializeComponent();
		Common.setControls(this, toolTipMain, new List<ContextMenuStrip> { contextMenuStripMain });
	}

	private void frmChartView_Load(object sender, EventArgs e)
	{
		Init();
	}

	private void frmChartView_Shown(object sender, EventArgs e)
	{
		mSearchMain.Init();
		mSearchMain.btnSearch.Click += btnSearch_Click;
		mSearchMain.btnRefresh.Click += btnRefresh_Click;
		dgvMain.SizeChanged += dgvMain_SizeChanged;
		load_AllData();
	}

	private void frmChartView_FormClosed(object sender, FormClosedEventArgs e)
	{
		List<Type> list = new List<Type>();
		list.Add(typeof(frmCpkView));
		Common.closeForm(list);
		GC.Collect();
	}

	private void Init()
	{
		mPanelViewMain.Visible = false;
		lblFullname.Text = frmLogin.User.FullName;
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(APIUrl.APIHost + "/AuthUserImage/").Append(frmLogin.User.ImageUrl);
		try
		{
			ptbAvatar.Load(stringBuilder.ToString());
		}
		catch
		{
			ptbAvatar.Image = Resources._5S_QA_S;
		}
		LoadSetting();
	}

	private void debugOutput(string strDebugText)
	{
		slblStatus.Text = strDebugText;
	}

	private void start_Proccessor()
	{
		sprogbarStatus.Maximum = 100;
		sprogbarStatus.Value = 0;
		sprogbarStatus.Value = 100;
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		if ((msg.Msg.Equals(256) || msg.Msg.Equals(260)) && base.ActiveControl.Name.Equals("dgvMain"))
		{
			if (keyData.Equals(Keys.Return))
			{
				dgvMain_CellDoubleClick(this, null);
				return true;
			}
			if (keyData.Equals(Keys.Escape))
			{
				dgvMain_CellClick(this, null);
				return true;
			}
		}
		return base.ProcessCmdKey(ref msg, keyData);
	}

	public void load_AllData()
	{
		Cursor.Current = Cursors.WaitCursor;
		try
		{
			start_Proccessor();
			isEdit = true;
			QueryArgs queryArgs = new QueryArgs
			{
				Predicate = "Request.ProductId=@0 && Request.Date>=@1 && Request.Date<=@2 && !Request.Status.Contains(@3) && !Request.Status.Contains(@4) && !string.IsNullOrEmpty(Result)",
				PredicateParameters = new List<object>
				{
					(mSearchMain.cbbStage.SelectedIndex == -1) ? Guid.Empty.ToString() : mSearchMain.cbbStage.SelectedValue.ToString(),
					mSearchMain.dtpDateFrom.Value.ToString("MM/dd/yyyy"),
					mSearchMain.dtpDateTo.Value.ToString("MM/dd/yyyy"),
					"Activated",
					"Rejected"
				},
				Order = "Request.Date, Request.Created, Request.Name, Measurement.Sort, Measurement.Created, Sample, Cavity",
				Page = 1,
				Limit = int.MaxValue
			};
			if (mSearchMain.cbAll.Checked)
			{
				queryArgs.Predicate += " && Measurement.ImportantId!=null";
			}
			else
			{
				queryArgs.Predicate += " && MeasurementId=@5";
				queryArgs.PredicateParameters.Add((mSearchMain.cbbMeas.SelectedIndex == -1) ? Guid.Empty.ToString() : mSearchMain.cbbMeas.SelectedValue.ToString());
			}
			ResponseDto result = frmLogin.client.GetsAsync(queryArgs, "/api/RequestResult/Gets").Result;
			mSearchMain.dataTable = Common.getDataTableIsSelectNoType<ChartViewModel>(result);
			mSearchMain.mFilter = string.Empty;
			mPanelButtonFilterMain.mFilters.Clear();
			TableLayoutPanel tableLayoutPanel = mPanelButtonFilterMain.Controls["tpanelMain"] as TableLayoutPanel;
			foreach (Control control in tableLayoutPanel.Controls)
			{
				control.Text = control.Text.TrimEnd(' ', '*');
			}
			load_dgvMain();
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
					debugOutput("ERR: " + ex2.Message.Replace("\n", ""));
					MessageBox.Show(ex2.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			else
			{
				debugOutput("ERR: " + ex.Message);
				MessageBox.Show(ex.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
	}

	private void load_dgvMain()
	{
		start_Proccessor();
		dgvMain.DataSource = new DataView(mSearchMain.SearchInAllColums());
		dgvMain.Refresh();
		if (dgvMain.CurrentCell == null)
		{
			mPanelViewMain.Visible = false;
		}
		if (isEdit)
		{
			try
			{
				dgvMain.Rows[mRow].Cells[mCol].Selected = true;
			}
			catch
			{
			}
		}
		debugOutput(Common.getTextLanguage(this, "Successful"));
		dgvMain_Sorted(dgvMain, null);
		isEdit = false;
	}

	private bool ByteArrayToFile(string fileName, byte[] byteArray)
	{
		try
		{
			using (FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
			{
				fileStream.Write(byteArray, 0, byteArray.Length);
			}
			return true;
		}
		catch (Exception ex)
		{
			debugOutput("ERR: " + ex.Message);
			MessageBox.Show(ex.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			return false;
		}
	}

	private void LoadSetting()
	{
		string interpreting = Settings.Default.Interpreting;
		cbXtb1.Checked = interpreting.Contains(cbXtb1.Name);
		cbXtb2.Checked = interpreting.Contains(cbXtb2.Name);
		cbXtb3.Checked = interpreting.Contains(cbXtb3.Name);
		cbXtb4.Checked = interpreting.Contains(cbXtb4.Name);
		cbXtb5.Checked = interpreting.Contains(cbXtb5.Name);
		cbXtb6.Checked = interpreting.Contains(cbXtb6.Name);
		cbXtb7.Checked = interpreting.Contains(cbXtb7.Name);
		cbXtb8.Checked = interpreting.Contains(cbXtb8.Name);
		cbR1.Checked = interpreting.Contains(cbR1.Name);
		cbR2.Checked = interpreting.Contains(cbR2.Name);
		cbR3.Checked = interpreting.Contains(cbR3.Name);
		cbR4.Checked = interpreting.Contains(cbR4.Name);
	}

	private void SaveSetting()
	{
		string text = string.Empty;
		if (cbXtb1.Checked)
		{
			text = text + cbXtb1.Name + ";";
		}
		if (cbXtb2.Checked)
		{
			text = text + cbXtb2.Name + ";";
		}
		if (cbXtb3.Checked)
		{
			text = text + cbXtb3.Name + ";";
		}
		if (cbXtb4.Checked)
		{
			text = text + cbXtb4.Name + ";";
		}
		if (cbXtb5.Checked)
		{
			text = text + cbXtb5.Name + ";";
		}
		if (cbXtb6.Checked)
		{
			text = text + cbXtb6.Name + ";";
		}
		if (cbXtb7.Checked)
		{
			text = text + cbXtb7.Name + ";";
		}
		if (cbXtb8.Checked)
		{
			text = text + cbXtb8.Name + ";";
		}
		if (cbR1.Checked)
		{
			text = text + cbR1.Name + ";";
		}
		if (cbR2.Checked)
		{
			text = text + cbR2.Name + ";";
		}
		if (cbR3.Checked)
		{
			text = text + cbR3.Name + ";";
		}
		if (cbR4.Checked)
		{
			text = text + cbR4.Name + ";";
		}
		Settings.Default.Interpreting = text;
		Settings.Default.Save();
	}

	private void dgvMain_CurrentCellChanged(object sender, EventArgs e)
	{
		DataGridView dataGridView = sender as DataGridView;
		if (dataGridView.CurrentCell != null)
		{
			tpanelButtonView.Enabled = true;
			if (!isEdit)
			{
				mRow = dataGridView.CurrentCell.RowIndex;
				mCol = dataGridView.CurrentCell.ColumnIndex;
			}
			Guid guid = Guid.Parse(dataGridView.CurrentRow.Cells["Id"].Value.ToString());
			if (!guid.Equals(mId))
			{
				mId = guid;
				mPanelViewMain.load_dgvContent((Enum)FormType.VIEW);
				if (int.Parse(dataGridView.CurrentRow.Cells["RequestSample"].Value.ToString()) == 1)
				{
					btnXControlN1.Enabled = true;
					btnXtbControl.Enabled = false;
					btnXtbRControl.Enabled = false;
					btnXtbSControl.Enabled = false;
				}
				else
				{
					btnXControlN1.Enabled = false;
					btnXtbControl.Enabled = true;
					btnXtbRControl.Enabled = true;
					btnXtbSControl.Enabled = true;
				}
			}
		}
		else
		{
			tpanelButtonView.Enabled = false;
			mId = Guid.Empty;
		}
	}

	private void dgvMain_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		mPanelViewMain.Visible = false;
	}

	private void dgvMain_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
	{
		if (mId.Equals(Guid.Empty))
		{
			mPanelViewMain.Visible = false;
		}
		else
		{
			mPanelViewMain.Visible = true;
		}
	}

	private void dgvMain_Sorted(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		DataGridView dataGridView = sender as DataGridView;
		int.TryParse(mSearchMain.txtPage.Text, out var result);
		int.TryParse(mSearchMain.cbbLimit.Text, out var result2);
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		foreach (DataGridViewRow item in (IEnumerable)dataGridView.Rows)
		{
			item.Cells["No"].Value = (result - 1) * result2 + item.Index + 1;
			object value = item.Cells["Judge"].Value;
			object obj = value;
			switch (obj as string)
			{
			case "OK":
				num++;
				break;
			case "OK+":
				num++;
				num4++;
				break;
			case "OK-":
				num++;
				num4++;
				break;
			case "NG":
				item.Cells["Result"].Style.ForeColor = Color.Red;
				item.Cells["Judge"].Style.ForeColor = Color.Red;
				num2++;
				break;
			case "NG+":
				item.Cells["Result"].Style.ForeColor = Color.Red;
				item.Cells["Judge"].Style.ForeColor = Color.Red;
				num2++;
				break;
			case "NG-":
				item.Cells["Result"].Style.ForeColor = Color.Red;
				item.Cells["Judge"].Style.ForeColor = Color.Red;
				num2++;
				break;
			}
			if (item.Cells["History"].Value != null && !string.IsNullOrEmpty(item.Cells["History"].Value.ToString()) && !item.Cells["History"].Value.ToString().Equals("[]"))
			{
				num3++;
				if (!item.Cells["RequestName"].Value.ToString().StartsWith("* "))
				{
					item.Cells["RequestName"].Value = string.Format("* {0}", item.Cells["RequestName"].Value);
				}
			}
			if (item.Cells["Unit"].Value?.ToString() == "°")
			{
				if (double.TryParse(item.Cells["Value"].Value?.ToString(), out var result3))
				{
					item.Cells["Value"].Value = Common.ConvertDoubleToDegrees(result3);
				}
				if (double.TryParse(item.Cells["Upper"].Value?.ToString(), out var result4))
				{
					item.Cells["Upper"].Value = Common.ConvertDoubleToDegrees(result4);
				}
				if (double.TryParse(item.Cells["Lower"].Value?.ToString(), out var result5))
				{
					item.Cells["Lower"].Value = Common.ConvertDoubleToDegrees(result5);
				}
				if (double.TryParse(item.Cells["LSL"].Value?.ToString(), out var result6))
				{
					item.Cells["LSL"].Value = Common.ConvertDoubleToDegrees(result6);
				}
				if (double.TryParse(item.Cells["USL"].Value?.ToString(), out var result7))
				{
					item.Cells["USL"].Value = Common.ConvertDoubleToDegrees(result7);
				}
				if (double.TryParse(item.Cells["Result"].Value?.ToString(), out var result8))
				{
					item.Cells["Result"].Value = Common.ConvertDoubleToDegrees(result8);
				}
			}
		}
		int num5 = dataGridView.Rows.Count - num - num2;
		lblJudgeOK.Text = num.ToString();
		lblJudgeNG.Text = num2.ToString();
		lblJudgeEmpty.Text = num5.ToString();
		lblEditResult.Text = num3.ToString();
		lblWarning.Text = num4.ToString();
	}

	private void btnSearch_Click(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		load_dgvMain();
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		main_refreshToolStripMenuItem.PerformClick();
	}

	private void main_refreshToolStripMenuItem_Click(object sender, EventArgs e)
	{
		mSearchMain.btnSearch.Select();
		load_AllData();
	}

	private void main_viewToolStripMenuItem_Click(object sender, EventArgs e)
	{
		mSearchMain.btnSearch.Select();
		Cursor.Current = Cursors.WaitCursor;
		List<ChartViewModel> list = new List<ChartViewModel>();
		foreach (DataRow row in mSearchMain.dataTable.Rows)
		{
			if (bool.Parse(row["IsSelect"].ToString()))
			{
				list.Add(Common.Cast<ChartViewModel>(row));
			}
		}
		if (list.Count.Equals(0))
		{
			MessageBox.Show(Common.getTextLanguage(this, "wListNull"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			dgvMain.Focus();
			return;
		}
		Guid guid = (string.IsNullOrEmpty(mSearchMain.cbbTemplate.Text) ? Guid.Empty : new Guid(mSearchMain.cbbTemplate.SelectedValue.ToString()));
		try
		{
			ExportExcelDto exportExcelDto = frmLogin.client.ExportAsync(guid, (object)list, "/api/Template/ExportExcelChart/{id}").Result ?? throw new Exception(Common.getTextLanguage(this, "wHasntFile"));
			string text = exportExcelDto.FileName.Replace("\"", "");
			if (Path.GetExtension(text).Equals(".zip"))
			{
				saveFileDialogMain.FileName = text;
				if (saveFileDialogMain.ShowDialog().Equals(DialogResult.OK))
				{
					text = saveFileDialogMain.FileName;
					ByteArrayToFile(text, exportExcelDto.Value);
				}
				return;
			}
			string text2 = Path.Combine("C:\\Windows\\Temp", "5S_QA", "VIEW");
			Directory.CreateDirectory(text2);
			text = Path.Combine(text2, text);
			if (File.Exists(text))
			{
				File.Delete(text);
			}
			Cursor.Current = Cursors.WaitCursor;
			if (ByteArrayToFile(text, exportExcelDto.Value))
			{
				WebBrowser webBrowser = new WebBrowser();
				webBrowser.Navigate(text, newWindow: false);
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
					debugOutput("ERR: " + ex2.Message.Replace("\n", ""));
					MessageBox.Show(ex2.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			else
			{
				debugOutput("ERR: " + ex.Message);
				MessageBox.Show(ex.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
	}

	private void ptbAvatar_DoubleClick(object sender, EventArgs e)
	{
		Common.activeForm(typeof(frmLogin));
	}

	private void enall_pageToolStripMenuItem_Click(object sender, EventArgs e)
	{
		DataView dataView = dgvMain.DataSource as DataView;
		foreach (DataRowView item in dataView)
		{
			item.Row["IsSelect"] = true;
			DataRow dataRow = mSearchMain.dataTable.Select(string.Format("Id='{0}'", item.Row["Id"])).FirstOrDefault();
			if (dataRow != null)
			{
				dataRow["IsSelect"] = true;
			}
			dataRow = mSearchMain.dataTableFilter.Select(string.Format("Id='{0}'", item.Row["Id"])).FirstOrDefault();
			if (dataRow != null)
			{
				dataRow["IsSelect"] = true;
			}
		}
		mSearchMain.dataTable.AcceptChanges();
		dgvMain.Refresh();
	}

	private void unall_pageToolStripMenuItem_Click(object sender, EventArgs e)
	{
		DataView dataView = dgvMain.DataSource as DataView;
		foreach (DataRowView item in dataView)
		{
			item.Row["IsSelect"] = false;
			DataRow dataRow = mSearchMain.dataTable.Select(string.Format("Id='{0}'", item.Row["Id"])).FirstOrDefault();
			if (dataRow != null)
			{
				dataRow["IsSelect"] = false;
			}
			dataRow = mSearchMain.dataTableFilter.Select(string.Format("Id='{0}'", item.Row["Id"])).FirstOrDefault();
			if (dataRow != null)
			{
				dataRow["IsSelect"] = false;
			}
		}
		mSearchMain.dataTable.AcceptChanges();
		dgvMain.Refresh();
	}

	private void dgvMain_CurrentCellDirtyStateChanged(object sender, EventArgs e)
	{
		DataGridView dataGridView = sender as DataGridView;
		dataGridView.CurrentCellDirtyStateChanged -= dgvMain_CurrentCellDirtyStateChanged;
		if (dataGridView.CurrentCell is DataGridViewCheckBoxCell dataGridViewCheckBoxCell)
		{
			dataGridView.CurrentRow.Cells["IsSelect"].Value = !(bool)dataGridViewCheckBoxCell.Value;
			DataRow dataRow = mSearchMain.dataTable.Select(string.Format("Id='{0}'", dataGridView.CurrentRow.Cells["Id"].Value)).FirstOrDefault();
			if (dataRow != null)
			{
				dataRow["IsSelect"] = dataGridView.CurrentRow.Cells["IsSelect"].Value;
			}
			dataRow = mSearchMain.dataTableFilter.Select(string.Format("Id='{0}'", dataGridView.CurrentRow.Cells["Id"].Value)).FirstOrDefault();
			if (dataRow != null)
			{
				dataRow["IsSelect"] = dataGridView.CurrentRow.Cells["IsSelect"].Value;
			}
		}
		dataGridView.EndEdit();
		dataGridView.CurrentCellDirtyStateChanged += dgvMain_CurrentCellDirtyStateChanged;
	}

	private void btnExport_Click(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		List<ChartViewModel> list = new List<ChartViewModel>();
		foreach (DataRow row in mSearchMain.dataTableFilter.Rows)
		{
			if (bool.Parse(row["IsSelect"].ToString()))
			{
				list.Add(Common.Cast<ChartViewModel>(row));
			}
		}
		if (list.Count.Equals(0))
		{
			MessageBox.Show(Common.getTextLanguage(this, "wListNull"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			dgvMain.Focus();
			return;
		}
		Button button = sender as Button;
		DataTable dataTable = mSearchMain.cbbStage.DataSource as DataTable;
		DataRow dataRow2 = dataTable.Select($"Id='{mSearchMain.cbbStage.SelectedValue}'").FirstOrDefault();
		ProductViewModel product = Common.Cast<ProductViewModel>(dataRow2);
		string title = button.Text + " (" + mSearchMain.cbbStage.Text + (mSearchMain.cbbMeas.Visible ? ("#" + mSearchMain.cbbMeas.Text) : "") + ")";
		List<Type> list2 = new List<Type>();
		list2.Add(typeof(frmCpkView));
		Common.closeForm(list2);
		new frmCpkView(title, button, product, list).Show();
	}

	private void dgvMain_MouseClick(object sender, MouseEventArgs e)
	{
		mPanelButtonFilterMain.CloseFormFilter();
	}

	private void dgvMain_SizeChanged(object sender, EventArgs e)
	{
		mPanelButtonFilterMain.SetFilterControls();
	}

	private void cbNormal_CheckedChanged(object sender, EventArgs e)
	{
		SaveSetting();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_5S_QA_System.frmChartView));
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
		this.saveFileDialogMain = new System.Windows.Forms.SaveFileDialog();
		this.openFileDialogMain = new System.Windows.Forms.OpenFileDialog();
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.btnQualityReport = new System.Windows.Forms.Button();
		this.btnHistogram = new System.Windows.Forms.Button();
		this.btnXControlN1 = new System.Windows.Forms.Button();
		this.btnXtbControl = new System.Windows.Forms.Button();
		this.btnXtbRControl = new System.Windows.Forms.Button();
		this.btnXtbSControl = new System.Windows.Forms.Button();
		this.btnInterpreting = new System.Windows.Forms.Button();
		this.cbXtb1 = new System.Windows.Forms.CheckBox();
		this.cbXtb2 = new System.Windows.Forms.CheckBox();
		this.cbXtb3 = new System.Windows.Forms.CheckBox();
		this.cbXtb4 = new System.Windows.Forms.CheckBox();
		this.cbXtb5 = new System.Windows.Forms.CheckBox();
		this.cbXtb6 = new System.Windows.Forms.CheckBox();
		this.cbXtb7 = new System.Windows.Forms.CheckBox();
		this.cbXtb8 = new System.Windows.Forms.CheckBox();
		this.cbR1 = new System.Windows.Forms.CheckBox();
		this.cbR2 = new System.Windows.Forms.CheckBox();
		this.cbR3 = new System.Windows.Forms.CheckBox();
		this.cbR4 = new System.Windows.Forms.CheckBox();
		this.sprogbarStatus = new System.Windows.Forms.ToolStripProgressBar();
		this.slblStatus = new System.Windows.Forms.ToolStripStatusLabel();
		this.contextMenuStripMain = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.main_refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparatorView = new System.Windows.Forms.ToolStripSeparator();
		this.main_viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.enall_pageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.unall_pageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.statusStripfrmMain = new System.Windows.Forms.StatusStrip();
		this.tpanelMeasurement = new System.Windows.Forms.TableLayoutPanel();
		this.lblWarning = new System.Windows.Forms.Label();
		this.lblWarn = new System.Windows.Forms.Label();
		this.lblEditResult = new System.Windows.Forms.Label();
		this.lblEdit = new System.Windows.Forms.Label();
		this.lblMeas = new System.Windows.Forms.Label();
		this.lblOK = new System.Windows.Forms.Label();
		this.lblNG = new System.Windows.Forms.Label();
		this.lblEmpty = new System.Windows.Forms.Label();
		this.lblJudgeOK = new System.Windows.Forms.Label();
		this.lblJudgeNG = new System.Windows.Forms.Label();
		this.lblJudgeEmpty = new System.Windows.Forms.Label();
		this.tpanelButtonView = new System.Windows.Forms.TableLayoutPanel();
		this.dgvMain = new System.Windows.Forms.DataGridView();
		this.panelLogout = new System.Windows.Forms.Panel();
		this.lblFullname = new System.Windows.Forms.Label();
		this.ptbAvatar = new System.Windows.Forms.PictureBox();
		this.tpanelInterpreting = new System.Windows.Forms.TableLayoutPanel();
		this.label4 = new System.Windows.Forms.Label();
		this.lblInterpreting = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.mPanelViewMain = new _5S_QA_System.View.User_control.mPanelOther();
		this.mPanelButtonFilterMain = new _5S_QA_System.mPanelButtonFilter();
		this.mSearchMain = new _5S_QA_System.mSearchChart();
		this.IsSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
		this.No = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.RequestId = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.RequestName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.RequestDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.RequestSample = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Lot = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Line = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ProductId = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.MeasurementId = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.MeasurementCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.MachineTypeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Upper = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Lower = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.LSL = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.USL = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.UCL = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.LCL = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Sample = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Cavity = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Result = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Judge = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.MachineName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.StaffName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.History = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Created = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Modified = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.CreatedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ModifiedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.IsActivated = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.contextMenuStripMain.SuspendLayout();
		this.statusStripfrmMain.SuspendLayout();
		this.tpanelMeasurement.SuspendLayout();
		this.tpanelButtonView.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvMain).BeginInit();
		this.panelLogout.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.ptbAvatar).BeginInit();
		this.tpanelInterpreting.SuspendLayout();
		base.SuspendLayout();
		this.saveFileDialogMain.Filter = "File zip (*.zip)|*.zip";
		this.saveFileDialogMain.Title = "Select folder and enter file name";
		this.openFileDialogMain.Filter = "Files PDF(*.pdf)| *.pdf; ";
		this.openFileDialogMain.Title = "Please select a file pdf";
		this.btnQualityReport.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnQualityReport.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnQualityReport.FlatAppearance.BorderSize = 0;
		this.btnQualityReport.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.btnQualityReport.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnQualityReport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnQualityReport.Location = new System.Drawing.Point(1, 1);
		this.btnQualityReport.Margin = new System.Windows.Forms.Padding(1);
		this.btnQualityReport.Name = "btnQualityReport";
		this.btnQualityReport.Size = new System.Drawing.Size(149, 24);
		this.btnQualityReport.TabIndex = 2;
		this.btnQualityReport.Text = "Quality report";
		this.toolTipMain.SetToolTip(this.btnQualityReport, "Select view Quality report");
		this.btnQualityReport.UseVisualStyleBackColor = false;
		this.btnQualityReport.Click += new System.EventHandler(btnExport_Click);
		this.btnHistogram.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnHistogram.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnHistogram.FlatAppearance.BorderSize = 0;
		this.btnHistogram.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.btnHistogram.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnHistogram.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnHistogram.Location = new System.Drawing.Point(152, 1);
		this.btnHistogram.Margin = new System.Windows.Forms.Padding(1);
		this.btnHistogram.Name = "btnHistogram";
		this.btnHistogram.Size = new System.Drawing.Size(149, 24);
		this.btnHistogram.TabIndex = 3;
		this.btnHistogram.Text = "Histogram";
		this.toolTipMain.SetToolTip(this.btnHistogram, "Select view Histogram");
		this.btnHistogram.UseVisualStyleBackColor = false;
		this.btnHistogram.Click += new System.EventHandler(btnExport_Click);
		this.btnXControlN1.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnXControlN1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnXControlN1.FlatAppearance.BorderSize = 0;
		this.btnXControlN1.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.btnXControlN1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnXControlN1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnXControlN1.Location = new System.Drawing.Point(303, 1);
		this.btnXControlN1.Margin = new System.Windows.Forms.Padding(1);
		this.btnXControlN1.Name = "btnXControlN1";
		this.btnXControlN1.Size = new System.Drawing.Size(149, 24);
		this.btnXControlN1.TabIndex = 4;
		this.btnXControlN1.Text = "X Control (n=1)";
		this.toolTipMain.SetToolTip(this.btnXControlN1, "Select view X Control (n=1)");
		this.btnXControlN1.UseVisualStyleBackColor = false;
		this.btnXControlN1.Click += new System.EventHandler(btnExport_Click);
		this.btnXtbControl.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnXtbControl.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnXtbControl.FlatAppearance.BorderSize = 0;
		this.btnXtbControl.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.btnXtbControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnXtbControl.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnXtbControl.Location = new System.Drawing.Point(454, 1);
		this.btnXtbControl.Margin = new System.Windows.Forms.Padding(1);
		this.btnXtbControl.Name = "btnXtbControl";
		this.btnXtbControl.Size = new System.Drawing.Size(149, 24);
		this.btnXtbControl.TabIndex = 5;
		this.btnXtbControl.Text = "X\u00af Control";
		this.toolTipMain.SetToolTip(this.btnXtbControl, "Select view Xtb Control");
		this.btnXtbControl.UseVisualStyleBackColor = false;
		this.btnXtbControl.Click += new System.EventHandler(btnExport_Click);
		this.btnXtbRControl.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnXtbRControl.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnXtbRControl.FlatAppearance.BorderSize = 0;
		this.btnXtbRControl.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.btnXtbRControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnXtbRControl.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnXtbRControl.Location = new System.Drawing.Point(605, 1);
		this.btnXtbRControl.Margin = new System.Windows.Forms.Padding(1);
		this.btnXtbRControl.Name = "btnXtbRControl";
		this.btnXtbRControl.Size = new System.Drawing.Size(149, 24);
		this.btnXtbRControl.TabIndex = 6;
		this.btnXtbRControl.Text = "X\u00af R Control";
		this.toolTipMain.SetToolTip(this.btnXtbRControl, "Select view Xtb-R Control");
		this.btnXtbRControl.UseVisualStyleBackColor = false;
		this.btnXtbRControl.Click += new System.EventHandler(btnExport_Click);
		this.btnXtbSControl.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnXtbSControl.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnXtbSControl.FlatAppearance.BorderSize = 0;
		this.btnXtbSControl.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.btnXtbSControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnXtbSControl.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnXtbSControl.Location = new System.Drawing.Point(756, 1);
		this.btnXtbSControl.Margin = new System.Windows.Forms.Padding(1);
		this.btnXtbSControl.Name = "btnXtbSControl";
		this.btnXtbSControl.Size = new System.Drawing.Size(149, 24);
		this.btnXtbSControl.TabIndex = 7;
		this.btnXtbSControl.Text = "X\u00af S Control";
		this.toolTipMain.SetToolTip(this.btnXtbSControl, "Select view Xtb-S Control");
		this.btnXtbSControl.UseVisualStyleBackColor = false;
		this.btnXtbSControl.Click += new System.EventHandler(btnExport_Click);
		this.btnInterpreting.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnInterpreting.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnInterpreting.FlatAppearance.BorderSize = 0;
		this.btnInterpreting.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.btnInterpreting.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnInterpreting.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnInterpreting.Location = new System.Drawing.Point(907, 1);
		this.btnInterpreting.Margin = new System.Windows.Forms.Padding(1);
		this.btnInterpreting.Name = "btnInterpreting";
		this.btnInterpreting.Size = new System.Drawing.Size(152, 24);
		this.btnInterpreting.TabIndex = 9;
		this.btnInterpreting.Text = "Interpreting";
		this.toolTipMain.SetToolTip(this.btnInterpreting, "Select view interpreting");
		this.btnInterpreting.UseVisualStyleBackColor = false;
		this.btnInterpreting.Click += new System.EventHandler(btnExport_Click);
		this.cbXtb1.AutoSize = true;
		this.cbXtb1.Checked = true;
		this.cbXtb1.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cbXtb1.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbXtb1.Location = new System.Drawing.Point(127, 4);
		this.cbXtb1.Name = "cbXtb1";
		this.cbXtb1.Size = new System.Drawing.Size(33, 20);
		this.cbXtb1.TabIndex = 1;
		this.cbXtb1.Text = "1";
		this.toolTipMain.SetToolTip(this.cbXtb1, "Select abnormal X\u00af 1");
		this.cbXtb1.UseVisualStyleBackColor = true;
		this.cbXtb1.CheckedChanged += new System.EventHandler(cbNormal_CheckedChanged);
		this.cbXtb2.AutoSize = true;
		this.cbXtb2.Checked = true;
		this.cbXtb2.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cbXtb2.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbXtb2.Location = new System.Drawing.Point(167, 4);
		this.cbXtb2.Name = "cbXtb2";
		this.cbXtb2.Size = new System.Drawing.Size(33, 20);
		this.cbXtb2.TabIndex = 2;
		this.cbXtb2.Text = "2";
		this.toolTipMain.SetToolTip(this.cbXtb2, "Select abnormal X\u00af 2");
		this.cbXtb2.UseVisualStyleBackColor = true;
		this.cbXtb2.CheckedChanged += new System.EventHandler(cbNormal_CheckedChanged);
		this.cbXtb3.AutoSize = true;
		this.cbXtb3.Checked = true;
		this.cbXtb3.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cbXtb3.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbXtb3.Location = new System.Drawing.Point(207, 4);
		this.cbXtb3.Name = "cbXtb3";
		this.cbXtb3.Size = new System.Drawing.Size(33, 20);
		this.cbXtb3.TabIndex = 3;
		this.cbXtb3.Text = "3";
		this.toolTipMain.SetToolTip(this.cbXtb3, "Select abnormal X\u00af 3");
		this.cbXtb3.UseVisualStyleBackColor = true;
		this.cbXtb3.CheckedChanged += new System.EventHandler(cbNormal_CheckedChanged);
		this.cbXtb4.AutoSize = true;
		this.cbXtb4.Checked = true;
		this.cbXtb4.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cbXtb4.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbXtb4.Location = new System.Drawing.Point(247, 4);
		this.cbXtb4.Name = "cbXtb4";
		this.cbXtb4.Size = new System.Drawing.Size(33, 20);
		this.cbXtb4.TabIndex = 4;
		this.cbXtb4.Text = "4";
		this.toolTipMain.SetToolTip(this.cbXtb4, "Select abnormal X\u00af 4");
		this.cbXtb4.UseVisualStyleBackColor = true;
		this.cbXtb4.CheckedChanged += new System.EventHandler(cbNormal_CheckedChanged);
		this.cbXtb5.AutoSize = true;
		this.cbXtb5.Checked = true;
		this.cbXtb5.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cbXtb5.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbXtb5.Location = new System.Drawing.Point(287, 4);
		this.cbXtb5.Name = "cbXtb5";
		this.cbXtb5.Size = new System.Drawing.Size(33, 20);
		this.cbXtb5.TabIndex = 5;
		this.cbXtb5.Text = "5";
		this.toolTipMain.SetToolTip(this.cbXtb5, "Select abnormal X\u00af 5");
		this.cbXtb5.UseVisualStyleBackColor = true;
		this.cbXtb5.CheckedChanged += new System.EventHandler(cbNormal_CheckedChanged);
		this.cbXtb6.AutoSize = true;
		this.cbXtb6.Checked = true;
		this.cbXtb6.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cbXtb6.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbXtb6.Location = new System.Drawing.Point(327, 4);
		this.cbXtb6.Name = "cbXtb6";
		this.cbXtb6.Size = new System.Drawing.Size(33, 20);
		this.cbXtb6.TabIndex = 6;
		this.cbXtb6.Text = "6";
		this.toolTipMain.SetToolTip(this.cbXtb6, "Select abnormal X\u00af 6");
		this.cbXtb6.UseVisualStyleBackColor = true;
		this.cbXtb6.CheckedChanged += new System.EventHandler(cbNormal_CheckedChanged);
		this.cbXtb7.AutoSize = true;
		this.cbXtb7.Checked = true;
		this.cbXtb7.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cbXtb7.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbXtb7.Location = new System.Drawing.Point(367, 4);
		this.cbXtb7.Name = "cbXtb7";
		this.cbXtb7.Size = new System.Drawing.Size(33, 20);
		this.cbXtb7.TabIndex = 7;
		this.cbXtb7.Text = "7";
		this.toolTipMain.SetToolTip(this.cbXtb7, "Select abnormal X\u00af 7");
		this.cbXtb7.UseVisualStyleBackColor = true;
		this.cbXtb7.CheckedChanged += new System.EventHandler(cbNormal_CheckedChanged);
		this.cbXtb8.AutoSize = true;
		this.cbXtb8.Checked = true;
		this.cbXtb8.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cbXtb8.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbXtb8.Location = new System.Drawing.Point(407, 4);
		this.cbXtb8.Name = "cbXtb8";
		this.cbXtb8.Size = new System.Drawing.Size(33, 20);
		this.cbXtb8.TabIndex = 8;
		this.cbXtb8.Text = "8";
		this.toolTipMain.SetToolTip(this.cbXtb8, "Select abnormal X\u00af 8");
		this.cbXtb8.UseVisualStyleBackColor = true;
		this.cbXtb8.CheckedChanged += new System.EventHandler(cbNormal_CheckedChanged);
		this.cbR1.AutoSize = true;
		this.cbR1.Checked = true;
		this.cbR1.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cbR1.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbR1.Location = new System.Drawing.Point(472, 4);
		this.cbR1.Name = "cbR1";
		this.cbR1.Size = new System.Drawing.Size(33, 20);
		this.cbR1.TabIndex = 9;
		this.cbR1.Text = "1";
		this.toolTipMain.SetToolTip(this.cbR1, "Select abnormal R1");
		this.cbR1.UseVisualStyleBackColor = true;
		this.cbR1.CheckedChanged += new System.EventHandler(cbNormal_CheckedChanged);
		this.cbR2.AutoSize = true;
		this.cbR2.Checked = true;
		this.cbR2.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cbR2.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbR2.Location = new System.Drawing.Point(512, 4);
		this.cbR2.Name = "cbR2";
		this.cbR2.Size = new System.Drawing.Size(33, 20);
		this.cbR2.TabIndex = 10;
		this.cbR2.Text = "2";
		this.toolTipMain.SetToolTip(this.cbR2, "Select abnormal R2");
		this.cbR2.UseVisualStyleBackColor = true;
		this.cbR2.CheckedChanged += new System.EventHandler(cbNormal_CheckedChanged);
		this.cbR3.AutoSize = true;
		this.cbR3.Checked = true;
		this.cbR3.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cbR3.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbR3.Location = new System.Drawing.Point(552, 4);
		this.cbR3.Name = "cbR3";
		this.cbR3.Size = new System.Drawing.Size(33, 20);
		this.cbR3.TabIndex = 11;
		this.cbR3.Text = "3";
		this.toolTipMain.SetToolTip(this.cbR3, "Select abnormal R3");
		this.cbR3.UseVisualStyleBackColor = true;
		this.cbR3.CheckedChanged += new System.EventHandler(cbNormal_CheckedChanged);
		this.cbR4.AutoSize = true;
		this.cbR4.Checked = true;
		this.cbR4.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cbR4.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbR4.Location = new System.Drawing.Point(592, 4);
		this.cbR4.Name = "cbR4";
		this.cbR4.Size = new System.Drawing.Size(33, 20);
		this.cbR4.TabIndex = 12;
		this.cbR4.Text = "4";
		this.toolTipMain.SetToolTip(this.cbR4, "Select abnormal R4");
		this.cbR4.UseVisualStyleBackColor = true;
		this.cbR4.CheckedChanged += new System.EventHandler(cbNormal_CheckedChanged);
		this.sprogbarStatus.Name = "sprogbarStatus";
		this.sprogbarStatus.Size = new System.Drawing.Size(200, 20);
		this.slblStatus.Name = "slblStatus";
		this.slblStatus.Size = new System.Drawing.Size(16, 21);
		this.slblStatus.Text = "...";
		this.contextMenuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.main_refreshToolStripMenuItem, this.toolStripSeparatorView, this.main_viewToolStripMenuItem, this.toolStripSeparator1, this.enall_pageToolStripMenuItem, this.unall_pageToolStripMenuItem });
		this.contextMenuStripMain.Name = "contextMenuStripStaff";
		this.contextMenuStripMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
		this.contextMenuStripMain.Size = new System.Drawing.Size(197, 104);
		this.main_refreshToolStripMenuItem.Name = "main_refreshToolStripMenuItem";
		this.main_refreshToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
		this.main_refreshToolStripMenuItem.Text = "Refresh";
		this.main_refreshToolStripMenuItem.Click += new System.EventHandler(main_refreshToolStripMenuItem_Click);
		this.toolStripSeparatorView.Name = "toolStripSeparatorView";
		this.toolStripSeparatorView.Size = new System.Drawing.Size(193, 6);
		this.toolStripSeparatorView.Visible = false;
		this.main_viewToolStripMenuItem.Name = "main_viewToolStripMenuItem";
		this.main_viewToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
		this.main_viewToolStripMenuItem.Text = "View";
		this.main_viewToolStripMenuItem.Visible = false;
		this.main_viewToolStripMenuItem.Click += new System.EventHandler(main_viewToolStripMenuItem_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(193, 6);
		this.enall_pageToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ControlText;
		this.enall_pageToolStripMenuItem.Name = "enall_pageToolStripMenuItem";
		this.enall_pageToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
		this.enall_pageToolStripMenuItem.Text = "Enable all current page";
		this.enall_pageToolStripMenuItem.Click += new System.EventHandler(enall_pageToolStripMenuItem_Click);
		this.unall_pageToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ControlText;
		this.unall_pageToolStripMenuItem.Name = "unall_pageToolStripMenuItem";
		this.unall_pageToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
		this.unall_pageToolStripMenuItem.Text = "Unable all current page";
		this.unall_pageToolStripMenuItem.Click += new System.EventHandler(unall_pageToolStripMenuItem_Click);
		this.statusStripfrmMain.BackColor = System.Drawing.SystemColors.ControlLight;
		this.statusStripfrmMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.statusStripfrmMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.sprogbarStatus, this.slblStatus });
		this.statusStripfrmMain.Location = new System.Drawing.Point(20, 554);
		this.statusStripfrmMain.Name = "statusStripfrmMain";
		this.statusStripfrmMain.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
		this.statusStripfrmMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
		this.statusStripfrmMain.Size = new System.Drawing.Size(1060, 26);
		this.statusStripfrmMain.SizingGrip = false;
		this.statusStripfrmMain.Stretch = false;
		this.statusStripfrmMain.TabIndex = 32;
		this.tpanelMeasurement.AutoSize = true;
		this.tpanelMeasurement.ColumnCount = 14;
		this.tpanelMeasurement.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMeasurement.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMeasurement.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMeasurement.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tpanelMeasurement.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMeasurement.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMeasurement.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMeasurement.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMeasurement.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMeasurement.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMeasurement.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMeasurement.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMeasurement.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMeasurement.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMeasurement.Controls.Add(this.lblWarning, 7, 0);
		this.tpanelMeasurement.Controls.Add(this.lblWarn, 6, 0);
		this.tpanelMeasurement.Controls.Add(this.lblEditResult, 5, 0);
		this.tpanelMeasurement.Controls.Add(this.lblEdit, 4, 0);
		this.tpanelMeasurement.Controls.Add(this.lblMeas, 0, 0);
		this.tpanelMeasurement.Controls.Add(this.lblOK, 8, 0);
		this.tpanelMeasurement.Controls.Add(this.lblNG, 10, 0);
		this.tpanelMeasurement.Controls.Add(this.lblEmpty, 12, 0);
		this.tpanelMeasurement.Controls.Add(this.lblJudgeOK, 9, 0);
		this.tpanelMeasurement.Controls.Add(this.lblJudgeNG, 11, 0);
		this.tpanelMeasurement.Controls.Add(this.lblJudgeEmpty, 13, 0);
		this.tpanelMeasurement.Dock = System.Windows.Forms.DockStyle.Top;
		this.tpanelMeasurement.Location = new System.Drawing.Point(20, 162);
		this.tpanelMeasurement.Name = "tpanelMeasurement";
		this.tpanelMeasurement.Padding = new System.Windows.Forms.Padding(2);
		this.tpanelMeasurement.RowCount = 1;
		this.tpanelMeasurement.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tpanelMeasurement.Size = new System.Drawing.Size(1060, 20);
		this.tpanelMeasurement.TabIndex = 159;
		this.lblWarning.AutoSize = true;
		this.lblWarning.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblWarning.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblWarning.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lblWarning.Location = new System.Drawing.Point(853, 2);
		this.lblWarning.Name = "lblWarning";
		this.lblWarning.Size = new System.Drawing.Size(15, 16);
		this.lblWarning.TabIndex = 157;
		this.lblWarning.Text = "0";
		this.lblWarning.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblWarn.AutoSize = true;
		this.lblWarn.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblWarn.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lblWarn.Location = new System.Drawing.Point(787, 2);
		this.lblWarn.Name = "lblWarn";
		this.lblWarn.Size = new System.Drawing.Size(60, 16);
		this.lblWarn.TabIndex = 156;
		this.lblWarn.Text = "Warning:";
		this.lblWarn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblEditResult.AutoSize = true;
		this.lblEditResult.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblEditResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblEditResult.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lblEditResult.Location = new System.Drawing.Point(766, 2);
		this.lblEditResult.Name = "lblEditResult";
		this.lblEditResult.Size = new System.Drawing.Size(15, 16);
		this.lblEditResult.TabIndex = 155;
		this.lblEditResult.Text = "0";
		this.lblEditResult.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblEdit.AutoSize = true;
		this.lblEdit.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblEdit.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lblEdit.Location = new System.Drawing.Point(692, 2);
		this.lblEdit.Name = "lblEdit";
		this.lblEdit.Size = new System.Drawing.Size(68, 16);
		this.lblEdit.TabIndex = 154;
		this.lblEdit.Text = "Edit result:";
		this.lblEdit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblMeas.AutoSize = true;
		this.lblMeas.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblMeas.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblMeas.Location = new System.Drawing.Point(5, 2);
		this.lblMeas.Name = "lblMeas";
		this.lblMeas.Size = new System.Drawing.Size(100, 16);
		this.lblMeas.TabIndex = 146;
		this.lblMeas.Text = "Measurement";
		this.lblMeas.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblOK.AutoSize = true;
		this.lblOK.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblOK.Location = new System.Drawing.Point(874, 2);
		this.lblOK.Name = "lblOK";
		this.lblOK.Size = new System.Drawing.Size(28, 16);
		this.lblOK.TabIndex = 147;
		this.lblOK.Text = "OK:";
		this.lblOK.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblNG.AutoSize = true;
		this.lblNG.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblNG.Location = new System.Drawing.Point(929, 2);
		this.lblNG.Name = "lblNG";
		this.lblNG.Size = new System.Drawing.Size(30, 16);
		this.lblNG.TabIndex = 148;
		this.lblNG.Text = "NG:";
		this.lblNG.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblEmpty.AutoSize = true;
		this.lblEmpty.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblEmpty.Location = new System.Drawing.Point(986, 2);
		this.lblEmpty.Name = "lblEmpty";
		this.lblEmpty.Size = new System.Drawing.Size(48, 16);
		this.lblEmpty.TabIndex = 149;
		this.lblEmpty.Text = "Empty:";
		this.lblEmpty.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblJudgeOK.AutoSize = true;
		this.lblJudgeOK.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblJudgeOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblJudgeOK.ForeColor = System.Drawing.Color.Blue;
		this.lblJudgeOK.Location = new System.Drawing.Point(908, 2);
		this.lblJudgeOK.Name = "lblJudgeOK";
		this.lblJudgeOK.Size = new System.Drawing.Size(15, 16);
		this.lblJudgeOK.TabIndex = 150;
		this.lblJudgeOK.Text = "0";
		this.lblJudgeOK.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblJudgeNG.AutoSize = true;
		this.lblJudgeNG.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblJudgeNG.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblJudgeNG.ForeColor = System.Drawing.Color.Red;
		this.lblJudgeNG.Location = new System.Drawing.Point(965, 2);
		this.lblJudgeNG.Name = "lblJudgeNG";
		this.lblJudgeNG.Size = new System.Drawing.Size(15, 16);
		this.lblJudgeNG.TabIndex = 151;
		this.lblJudgeNG.Text = "0";
		this.lblJudgeNG.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblJudgeEmpty.AutoSize = true;
		this.lblJudgeEmpty.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblJudgeEmpty.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblJudgeEmpty.Location = new System.Drawing.Point(1040, 2);
		this.lblJudgeEmpty.Name = "lblJudgeEmpty";
		this.lblJudgeEmpty.Size = new System.Drawing.Size(15, 16);
		this.lblJudgeEmpty.TabIndex = 152;
		this.lblJudgeEmpty.Text = "0";
		this.lblJudgeEmpty.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.tpanelButtonView.AutoSize = true;
		this.tpanelButtonView.ColumnCount = 7;
		this.tpanelButtonView.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28531f));
		this.tpanelButtonView.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28531f));
		this.tpanelButtonView.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28531f));
		this.tpanelButtonView.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28531f));
		this.tpanelButtonView.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28531f));
		this.tpanelButtonView.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28531f));
		this.tpanelButtonView.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28815f));
		this.tpanelButtonView.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tpanelButtonView.Controls.Add(this.btnInterpreting, 6, 0);
		this.tpanelButtonView.Controls.Add(this.btnXtbSControl, 5, 0);
		this.tpanelButtonView.Controls.Add(this.btnXtbRControl, 4, 0);
		this.tpanelButtonView.Controls.Add(this.btnXtbControl, 3, 0);
		this.tpanelButtonView.Controls.Add(this.btnXControlN1, 2, 0);
		this.tpanelButtonView.Controls.Add(this.btnHistogram, 1, 0);
		this.tpanelButtonView.Controls.Add(this.btnQualityReport, 0, 0);
		this.tpanelButtonView.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.tpanelButtonView.Enabled = false;
		this.tpanelButtonView.Location = new System.Drawing.Point(20, 528);
		this.tpanelButtonView.Margin = new System.Windows.Forms.Padding(0);
		this.tpanelButtonView.Name = "tpanelButtonView";
		this.tpanelButtonView.RowCount = 1;
		this.tpanelButtonView.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tpanelButtonView.Size = new System.Drawing.Size(1060, 26);
		this.tpanelButtonView.TabIndex = 161;
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
		this.dgvMain.ColumnHeadersHeight = 26;
		this.dgvMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
		this.dgvMain.Columns.AddRange(this.IsSelect, this.No, this.RequestId, this.RequestName, this.RequestDate, this.RequestSample, this.Lot, this.Line, this.ProductId, this.MeasurementId, this.MeasurementCode, this.MachineTypeName, this.name, this.Value, this.Unit, this.Upper, this.Lower, this.LSL, this.USL, this.UCL, this.LCL, this.Sample, this.Cavity, this.Result, this.Judge, this.MachineName, this.StaffName, this.History, this.Id, this.Created, this.Modified, this.CreatedBy, this.ModifiedBy, this.IsActivated);
		this.dgvMain.ContextMenuStrip = this.contextMenuStripMain;
		this.dgvMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.dgvMain.EnableHeadersVisualStyles = false;
		this.dgvMain.Location = new System.Drawing.Point(20, 203);
		this.dgvMain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.dgvMain.MultiSelect = false;
		this.dgvMain.Name = "dgvMain";
		this.dgvMain.RowHeadersWidth = 25;
		this.dgvMain.Size = new System.Drawing.Size(1060, 325);
		this.dgvMain.TabIndex = 163;
		this.dgvMain.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvMain_CellClick);
		this.dgvMain.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvMain_CellDoubleClick);
		this.dgvMain.CurrentCellChanged += new System.EventHandler(dgvMain_CurrentCellChanged);
		this.dgvMain.CurrentCellDirtyStateChanged += new System.EventHandler(dgvMain_CurrentCellDirtyStateChanged);
		this.dgvMain.Sorted += new System.EventHandler(dgvMain_Sorted);
		this.dgvMain.MouseClick += new System.Windows.Forms.MouseEventHandler(dgvMain_MouseClick);
		this.panelLogout.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.panelLogout.BackColor = System.Drawing.Color.Transparent;
		this.panelLogout.Controls.Add(this.lblFullname);
		this.panelLogout.Controls.Add(this.ptbAvatar);
		this.panelLogout.Location = new System.Drawing.Point(730, 27);
		this.panelLogout.Name = "panelLogout";
		this.panelLogout.Size = new System.Drawing.Size(350, 42);
		this.panelLogout.TabIndex = 176;
		this.panelLogout.TabStop = true;
		this.lblFullname.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.lblFullname.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f);
		this.lblFullname.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.lblFullname.Location = new System.Drawing.Point(0, 26);
		this.lblFullname.Name = "lblFullname";
		this.lblFullname.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
		this.lblFullname.Size = new System.Drawing.Size(308, 16);
		this.lblFullname.TabIndex = 34;
		this.lblFullname.Text = "...";
		this.lblFullname.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.ptbAvatar.Dock = System.Windows.Forms.DockStyle.Right;
		this.ptbAvatar.Image = _5S_QA_System.Properties.Resources._5S_QA_S;
		this.ptbAvatar.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.ptbAvatar.Location = new System.Drawing.Point(308, 0);
		this.ptbAvatar.Margin = new System.Windows.Forms.Padding(4);
		this.ptbAvatar.Name = "ptbAvatar";
		this.ptbAvatar.Size = new System.Drawing.Size(42, 42);
		this.ptbAvatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.ptbAvatar.TabIndex = 14;
		this.ptbAvatar.TabStop = false;
		this.ptbAvatar.DoubleClick += new System.EventHandler(ptbAvatar_DoubleClick);
		this.tpanelInterpreting.AutoSize = true;
		this.tpanelInterpreting.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		this.tpanelInterpreting.ColumnCount = 16;
		this.tpanelInterpreting.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelInterpreting.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelInterpreting.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelInterpreting.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelInterpreting.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelInterpreting.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelInterpreting.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelInterpreting.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelInterpreting.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelInterpreting.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelInterpreting.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelInterpreting.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelInterpreting.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelInterpreting.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelInterpreting.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelInterpreting.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tpanelInterpreting.Controls.Add(this.cbR4, 14, 0);
		this.tpanelInterpreting.Controls.Add(this.cbR3, 13, 0);
		this.tpanelInterpreting.Controls.Add(this.cbR2, 12, 0);
		this.tpanelInterpreting.Controls.Add(this.cbR1, 11, 0);
		this.tpanelInterpreting.Controls.Add(this.cbXtb8, 9, 0);
		this.tpanelInterpreting.Controls.Add(this.cbXtb7, 8, 0);
		this.tpanelInterpreting.Controls.Add(this.cbXtb6, 7, 0);
		this.tpanelInterpreting.Controls.Add(this.cbXtb5, 6, 0);
		this.tpanelInterpreting.Controls.Add(this.cbXtb4, 5, 0);
		this.tpanelInterpreting.Controls.Add(this.cbXtb3, 4, 0);
		this.tpanelInterpreting.Controls.Add(this.cbXtb2, 3, 0);
		this.tpanelInterpreting.Controls.Add(this.label4, 1, 0);
		this.tpanelInterpreting.Controls.Add(this.lblInterpreting, 0, 0);
		this.tpanelInterpreting.Controls.Add(this.label8, 10, 0);
		this.tpanelInterpreting.Controls.Add(this.cbXtb1, 2, 0);
		this.tpanelInterpreting.Dock = System.Windows.Forms.DockStyle.Top;
		this.tpanelInterpreting.Location = new System.Drawing.Point(20, 134);
		this.tpanelInterpreting.Name = "tpanelInterpreting";
		this.tpanelInterpreting.RowCount = 1;
		this.tpanelInterpreting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tpanelInterpreting.Size = new System.Drawing.Size(1060, 28);
		this.tpanelInterpreting.TabIndex = 177;
		this.label4.AutoSize = true;
		this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label4.Location = new System.Drawing.Point(96, 1);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(24, 26);
		this.label4.TabIndex = 154;
		this.label4.Text = "X\u00af";
		this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblInterpreting.AutoSize = true;
		this.lblInterpreting.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblInterpreting.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblInterpreting.Location = new System.Drawing.Point(4, 1);
		this.lblInterpreting.Name = "lblInterpreting";
		this.lblInterpreting.Size = new System.Drawing.Size(85, 26);
		this.lblInterpreting.TabIndex = 146;
		this.lblInterpreting.Text = "Interpreting";
		this.lblInterpreting.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label8.AutoSize = true;
		this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(447, 1);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(18, 26);
		this.label8.TabIndex = 149;
		this.label8.Text = "R";
		this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.mPanelViewMain.AutoScroll = true;
		this.mPanelViewMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.mPanelViewMain.Dock = System.Windows.Forms.DockStyle.Right;
		this.mPanelViewMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.mPanelViewMain.Location = new System.Drawing.Point(630, 203);
		this.mPanelViewMain.Margin = new System.Windows.Forms.Padding(12, 9, 12, 9);
		this.mPanelViewMain.Name = "mPanelViewMain";
		this.mPanelViewMain.Size = new System.Drawing.Size(450, 325);
		this.mPanelViewMain.TabIndex = 158;
		this.mPanelButtonFilterMain.AutoSize = true;
		this.mPanelButtonFilterMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.mPanelButtonFilterMain.Dock = System.Windows.Forms.DockStyle.Top;
		this.mPanelButtonFilterMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.mPanelButtonFilterMain.Location = new System.Drawing.Point(20, 182);
		this.mPanelButtonFilterMain.Margin = new System.Windows.Forms.Padding(0);
		this.mPanelButtonFilterMain.mFilters = (System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>)resources.GetObject("mPanelButtonFilterMain.mFilters");
		this.mPanelButtonFilterMain.Name = "mPanelButtonFilterMain";
		this.mPanelButtonFilterMain.Size = new System.Drawing.Size(1060, 21);
		this.mPanelButtonFilterMain.TabIndex = 162;
		this.mSearchMain.AutoSize = true;
		this.mSearchMain.BackColor = System.Drawing.SystemColors.Control;
		this.mSearchMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.mSearchMain.dataTable = null;
		this.mSearchMain.Dock = System.Windows.Forms.DockStyle.Top;
		this.mSearchMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.mSearchMain.Location = new System.Drawing.Point(20, 70);
		this.mSearchMain.Margin = new System.Windows.Forms.Padding(4);
		this.mSearchMain.MaximumSize = new System.Drawing.Size(5000, 64);
		this.mSearchMain.MinimumSize = new System.Drawing.Size(100, 64);
		this.mSearchMain.Name = "mSearchMain";
		this.mSearchMain.Padding = new System.Windows.Forms.Padding(3);
		this.mSearchMain.Size = new System.Drawing.Size(1060, 64);
		this.mSearchMain.TabIndex = 38;
		this.IsSelect.DataPropertyName = "IsSelect";
		this.IsSelect.FalseValue = "False";
		this.IsSelect.HeaderText = "";
		this.IsSelect.Name = "IsSelect";
		this.IsSelect.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		this.IsSelect.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
		this.IsSelect.TrueValue = "True";
		this.IsSelect.Width = 22;
		this.No.DataPropertyName = "No";
		dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		this.No.DefaultCellStyle = dataGridViewCellStyle3;
		this.No.HeaderText = "No.";
		this.No.Name = "No";
		this.No.ReadOnly = true;
		this.No.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.No.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.No.Width = 40;
		this.RequestId.DataPropertyName = "RequestId";
		this.RequestId.HeaderText = "Req. id";
		this.RequestId.Name = "RequestId";
		this.RequestId.ReadOnly = true;
		this.RequestId.Visible = false;
		this.RequestName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.RequestName.DataPropertyName = "RequestName";
		this.RequestName.FillWeight = 20f;
		this.RequestName.HeaderText = "Req. name";
		this.RequestName.Name = "RequestName";
		this.RequestName.ReadOnly = true;
		this.RequestDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.RequestDate.DataPropertyName = "RequestDate";
		this.RequestDate.FillWeight = 20f;
		this.RequestDate.HeaderText = "Req. date";
		this.RequestDate.Name = "RequestDate";
		this.RequestDate.ReadOnly = true;
		this.RequestSample.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.RequestSample.DataPropertyName = "RequestSample";
		this.RequestSample.FillWeight = 20f;
		this.RequestSample.HeaderText = "Req. sample";
		this.RequestSample.Name = "RequestSample";
		this.RequestSample.ReadOnly = true;
		this.RequestSample.Visible = false;
		this.Lot.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Lot.DataPropertyName = "Lot";
		this.Lot.FillWeight = 20f;
		this.Lot.HeaderText = "Lot";
		this.Lot.Name = "Lot";
		this.Lot.ReadOnly = true;
		this.Line.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Line.DataPropertyName = "Line";
		this.Line.FillWeight = 20f;
		this.Line.HeaderText = "Line";
		this.Line.Name = "Line";
		this.Line.ReadOnly = true;
		this.ProductId.DataPropertyName = "ProductId";
		this.ProductId.HeaderText = "Pro. id";
		this.ProductId.Name = "ProductId";
		this.ProductId.ReadOnly = true;
		this.ProductId.Visible = false;
		this.MeasurementId.DataPropertyName = "MeasurementId";
		this.MeasurementId.HeaderText = "Meas. id";
		this.MeasurementId.Name = "MeasurementId";
		this.MeasurementId.ReadOnly = true;
		this.MeasurementId.Visible = false;
		this.MeasurementCode.DataPropertyName = "MeasurementCode";
		this.MeasurementCode.HeaderText = "Code";
		this.MeasurementCode.Name = "MeasurementCode";
		this.MeasurementCode.ReadOnly = true;
		this.MeasurementCode.Visible = false;
		this.MachineTypeName.DataPropertyName = "MachineTypeName";
		this.MachineTypeName.HeaderText = "Tool";
		this.MachineTypeName.Name = "MachineTypeName";
		this.MachineTypeName.ReadOnly = true;
		this.MachineTypeName.Visible = false;
		this.name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.name.DataPropertyName = "Name";
		this.name.FillWeight = 20f;
		this.name.HeaderText = "Name";
		this.name.Name = "name";
		this.name.ReadOnly = true;
		this.Value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Value.DataPropertyName = "Value";
		dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.Value.DefaultCellStyle = dataGridViewCellStyle4;
		this.Value.FillWeight = 15f;
		this.Value.HeaderText = "Value";
		this.Value.Name = "Value";
		this.Value.ReadOnly = true;
		this.Unit.DataPropertyName = "Unit";
		this.Unit.HeaderText = "Unit";
		this.Unit.Name = "Unit";
		this.Unit.ReadOnly = true;
		this.Unit.Visible = false;
		this.Upper.DataPropertyName = "Upper";
		dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.Upper.DefaultCellStyle = dataGridViewCellStyle5;
		this.Upper.HeaderText = "Upper";
		this.Upper.Name = "Upper";
		this.Upper.ReadOnly = true;
		this.Upper.Visible = false;
		this.Lower.DataPropertyName = "Lower";
		dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.Lower.DefaultCellStyle = dataGridViewCellStyle6;
		this.Lower.HeaderText = "Lower";
		this.Lower.Name = "Lower";
		this.Lower.ReadOnly = true;
		this.Lower.Visible = false;
		this.LSL.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.LSL.DataPropertyName = "LSL";
		dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.LSL.DefaultCellStyle = dataGridViewCellStyle7;
		this.LSL.FillWeight = 15f;
		this.LSL.HeaderText = "LSL";
		this.LSL.Name = "LSL";
		this.LSL.ReadOnly = true;
		this.USL.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.USL.DataPropertyName = "USL";
		dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.USL.DefaultCellStyle = dataGridViewCellStyle8;
		this.USL.FillWeight = 15f;
		this.USL.HeaderText = "USL";
		this.USL.Name = "USL";
		this.USL.ReadOnly = true;
		this.UCL.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.UCL.DataPropertyName = "UCL";
		dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.UCL.DefaultCellStyle = dataGridViewCellStyle9;
		this.UCL.FillWeight = 15f;
		this.UCL.HeaderText = "UCL";
		this.UCL.Name = "UCL";
		this.UCL.ReadOnly = true;
		this.LCL.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.LCL.DataPropertyName = "LCL";
		dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.LCL.DefaultCellStyle = dataGridViewCellStyle10;
		this.LCL.FillWeight = 15f;
		this.LCL.HeaderText = "LCL";
		this.LCL.Name = "LCL";
		this.LCL.ReadOnly = true;
		this.Sample.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Sample.DataPropertyName = "Sample";
		this.Sample.FillWeight = 15f;
		this.Sample.HeaderText = "Sample";
		this.Sample.Name = "Sample";
		this.Sample.ReadOnly = true;
		this.Cavity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Cavity.DataPropertyName = "Cavity";
		this.Cavity.FillWeight = 15f;
		this.Cavity.HeaderText = "Cavity";
		this.Cavity.Name = "Cavity";
		this.Cavity.ReadOnly = true;
		this.Result.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Result.DataPropertyName = "Result";
		dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.Result.DefaultCellStyle = dataGridViewCellStyle11;
		this.Result.FillWeight = 15f;
		this.Result.HeaderText = "Result";
		this.Result.Name = "Result";
		this.Result.ReadOnly = true;
		this.Judge.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Judge.DataPropertyName = "Judge";
		this.Judge.FillWeight = 15f;
		this.Judge.HeaderText = "Judge";
		this.Judge.Name = "Judge";
		this.Judge.ReadOnly = true;
		this.MachineName.DataPropertyName = "MachineName";
		this.MachineName.HeaderText = "Machine name";
		this.MachineName.Name = "MachineName";
		this.MachineName.ReadOnly = true;
		this.MachineName.Visible = false;
		this.StaffName.DataPropertyName = "StaffName";
		this.StaffName.HeaderText = "Staff name";
		this.StaffName.Name = "StaffName";
		this.StaffName.ReadOnly = true;
		this.StaffName.Visible = false;
		this.History.DataPropertyName = "History";
		this.History.HeaderText = "History";
		this.History.Name = "History";
		this.History.Visible = false;
		this.Id.DataPropertyName = "Id";
		this.Id.HeaderText = "Id";
		this.Id.Name = "Id";
		this.Id.ReadOnly = true;
		this.Id.Visible = false;
		this.Created.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Created.DataPropertyName = "Created";
		this.Created.FillWeight = 30f;
		this.Created.HeaderText = "Created";
		this.Created.Name = "Created";
		this.Created.ReadOnly = true;
		this.Created.Visible = false;
		this.Modified.DataPropertyName = "Modified";
		this.Modified.HeaderText = "Modified";
		this.Modified.Name = "Modified";
		this.Modified.ReadOnly = true;
		this.Modified.Visible = false;
		this.CreatedBy.DataPropertyName = "CreatedBy";
		this.CreatedBy.HeaderText = "Created by";
		this.CreatedBy.Name = "CreatedBy";
		this.CreatedBy.ReadOnly = true;
		this.CreatedBy.Visible = false;
		this.ModifiedBy.DataPropertyName = "ModifiedBy";
		this.ModifiedBy.HeaderText = "Modified by";
		this.ModifiedBy.Name = "ModifiedBy";
		this.ModifiedBy.ReadOnly = true;
		this.ModifiedBy.Visible = false;
		this.IsActivated.DataPropertyName = "IsActivated";
		this.IsActivated.HeaderText = "Is activated";
		this.IsActivated.Name = "IsActivated";
		this.IsActivated.ReadOnly = true;
		this.IsActivated.Visible = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1100, 600);
		base.Controls.Add(this.panelLogout);
		base.Controls.Add(this.mPanelViewMain);
		base.Controls.Add(this.dgvMain);
		base.Controls.Add(this.tpanelButtonView);
		base.Controls.Add(this.statusStripfrmMain);
		base.Controls.Add(this.mPanelButtonFilterMain);
		base.Controls.Add(this.tpanelMeasurement);
		base.Controls.Add(this.tpanelInterpreting);
		base.Controls.Add(this.mSearchMain);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "frmChartView";
		base.Padding = new System.Windows.Forms.Padding(20, 70, 20, 20);
		this.Text = "5S QA System * CHART";
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmChartView_FormClosed);
		base.Load += new System.EventHandler(frmChartView_Load);
		base.Shown += new System.EventHandler(frmChartView_Shown);
		this.contextMenuStripMain.ResumeLayout(false);
		this.statusStripfrmMain.ResumeLayout(false);
		this.statusStripfrmMain.PerformLayout();
		this.tpanelMeasurement.ResumeLayout(false);
		this.tpanelMeasurement.PerformLayout();
		this.tpanelButtonView.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.dgvMain).EndInit();
		this.panelLogout.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.ptbAvatar).EndInit();
		this.tpanelInterpreting.ResumeLayout(false);
		this.tpanelInterpreting.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
