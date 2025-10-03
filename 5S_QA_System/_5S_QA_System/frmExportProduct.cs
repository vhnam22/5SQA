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
using _5S_QA_Entities.Models;
using _5S_QA_System.Controls;
using _5S_QA_System.Properties;
using MetroFramework.Forms;

namespace _5S_QA_System;

public class frmExportProduct : MetroForm
{
	private Guid mId;

	private bool isEdit;

	private int mRow;

	private int mCol;

	private IContainer components = null;

	private SaveFileDialog saveFileDialogMain;

	private ToolTip toolTipMain;

	private ToolStripProgressBar sprogbarStatus;

	public ToolStripStatusLabel slblStatus;

	private ContextMenuStrip contextMenuStripMain;

	private ToolStripMenuItem main_refreshToolStripMenuItem;

	private ToolStripSeparator toolStripSeparatorView;

	private ToolStripMenuItem main_viewToolStripMenuItem;

	private StatusStrip statusStripfrmMain;

	private mSearchExport mSearchMain;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripMenuItem enall_pageToolStripMenuItem;

	private ToolStripMenuItem unall_pageToolStripMenuItem;

	private mPanelViewManager mPanelViewMain;

	private mPanelButtonFilter mPanelButtonFilterMain;

	private DataGridView dgvMain;

	private Panel panelLogout;

	private Label lblFullname;

	private PictureBox ptbAvatar;

	private ToolStripMenuItem main_fileToolStripMenuItem;

	private DataGridViewCheckBoxColumn IsSelect;

	private DataGridViewTextBoxColumn No;

	private DataGridViewTextBoxColumn Code;

	private DataGridViewTextBoxColumn name;

	private DataGridViewTextBoxColumn ProductId;

	private DataGridViewTextBoxColumn ProductStage;

	private DataGridViewTextBoxColumn ProductCode;

	private new DataGridViewTextBoxColumn ProductName;

	private DataGridViewTextBoxColumn ProductImageUrl;

	private DataGridViewTextBoxColumn ProductCavity;

	private DataGridViewTextBoxColumn Date;

	private DataGridViewTextBoxColumn Quantity;

	private DataGridViewTextBoxColumn Lot;

	private DataGridViewTextBoxColumn Line;

	private DataGridViewTextBoxColumn Intention;

	private DataGridViewTextBoxColumn InputDate;

	private DataGridViewTextBoxColumn Supplier;

	private DataGridViewTextBoxColumn Sample;

	private DataGridViewTextBoxColumn Type;

	private DataGridViewTextBoxColumn Status;

	private DataGridViewTextBoxColumn Judgement;

	private DataGridViewTextBoxColumn Link;

	private DataGridViewTextBoxColumn Completed;

	private DataGridViewTextBoxColumn CompletedBy;

	private DataGridViewTextBoxColumn Checked;

	private DataGridViewTextBoxColumn CheckedBy;

	private DataGridViewTextBoxColumn Approved;

	private DataGridViewTextBoxColumn ApprovedBy;

	private DataGridViewTextBoxColumn TotalComment;

	private DataGridViewTextBoxColumn Id;

	private new DataGridViewTextBoxColumn Created;

	private DataGridViewTextBoxColumn Modified;

	private DataGridViewTextBoxColumn CreatedBy;

	private DataGridViewTextBoxColumn ModifiedBy;

	private DataGridViewTextBoxColumn IsActivated;

	public frmExportProduct()
	{
		InitializeComponent();
		Common.setControls(this, toolTipMain, new List<ContextMenuStrip> { contextMenuStripMain });
		mId = Guid.Empty;
		isEdit = false;
		mRow = 0;
		mCol = 0;
	}

	private void frmExportProduct_Load(object sender, EventArgs e)
	{
		Init();
	}

	private void frmExportProduct_Shown(object sender, EventArgs e)
	{
		mSearchMain.Init();
		mSearchMain.btnSearch.Click += btnSearch_Click;
		mSearchMain.btnRefresh.Click += btnRefresh_Click;
		dgvMain.SizeChanged += dgvMain_SizeChanged;
		load_AllData();
	}

	private void frmExportProduct_FormClosing(object sender, FormClosingEventArgs e)
	{
		GC.Collect();
	}

	private void frmExportProduct_FormClosed(object sender, FormClosedEventArgs e)
	{
		mPanelViewMain.mDispose();
		List<Type> list = new List<Type>();
		list.Add(typeof(frmHistoryView));
		Common.closeForm(list);
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
		mPanelViewMain.btnExport.Click += main_exportToolStripMenuItem_Click;
		mPanelViewMain.btnHistory.Click += main_viewToolStripMenuItem_Click;
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
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "Date>=@0 && Date<=@1 && Product.GroupId=@2 && !Status.Contains(@3) && !Status.Contains(@4)";
			queryArgs.PredicateParameters = new string[5]
			{
				mSearchMain.dtpDateFrom.Value.ToString("MM/dd/yyyy"),
				mSearchMain.dtpDateTo.Value.ToString("MM/dd/yyyy"),
				(mSearchMain.cbbProduct.SelectedIndex == -1) ? Guid.Empty.ToString() : mSearchMain.cbbProduct.SelectedValue.ToString(),
				"Activated",
				"Rejected"
			};
			queryArgs.Order = "Product.Sort, Date, Created";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs body = queryArgs;
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/Request/Gets").Result;
			mSearchMain.dataTable = Common.getDataTableIsSelect<RequestExportViewModel>(result);
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

	private void dgvMain_CurrentCellChanged(object sender, EventArgs e)
	{
		DataGridView dataGridView = sender as DataGridView;
		if (dataGridView.CurrentCell != null)
		{
			if (!isEdit)
			{
				mRow = dataGridView.CurrentCell.RowIndex;
				mCol = dataGridView.CurrentCell.ColumnIndex;
			}
			main_fileToolStripMenuItem.Visible = !string.IsNullOrEmpty(dataGridView.CurrentRow.Cells["Link"].Value.ToString());
			Guid guid = Guid.Parse(dataGridView.CurrentRow.Cells["Id"].Value.ToString());
			if (!guid.Equals(mId))
			{
				mId = guid;
				mPanelViewMain.Display();
			}
		}
		else
		{
			main_fileToolStripMenuItem.Visible = false;
			mId = Guid.Empty;
		}
		base.Controls.RemoveByKey("mPanelSelectFile");
		GC.Collect();
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
		foreach (DataGridViewRow item in (IEnumerable)dataGridView.Rows)
		{
			item.Cells["No"].Value = (result - 1) * result2 + item.Index + 1;
			switch (item.Cells["Judgement"].Value.ToString())
			{
			case "ACCEPTABLE":
				item.Cells["Judgement"].Style.ForeColor = Color.Green;
				break;
			case "NG":
				item.Cells["Judgement"].Style.ForeColor = Color.Red;
				break;
			case "RANK5":
				item.Cells["Judgement"].Style.ForeColor = Color.Red;
				break;
			default:
				item.Cells["Judgement"].Style.ForeColor = Color.Blue;
				break;
			}
		}
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
		if (mId.Equals(Guid.Empty))
		{
			MessageBox.Show(Common.getTextLanguage(this, "wSelectRow"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		List<Type> list = new List<Type>();
		list.Add(typeof(frmHistoryView));
		Common.closeForm(list);
		new frmHistoryView(dgvMain.CurrentRow).Show();
	}

	private void main_exportToolStripMenuItem_Click(object sender, EventArgs e)
	{
		mSearchMain.btnSearch.Select();
		base.Controls.RemoveByKey("mPanelSelectFile");
		Cursor.Current = Cursors.WaitCursor;
		if (mId.Equals(Guid.Empty))
		{
			MessageBox.Show(Common.getTextLanguage(this, "wSelectRow"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		mPanelSelectFile mPanelSelectFile2 = new mPanelSelectFile();
		base.Controls.Add(mPanelSelectFile2);
		mPanelSelectFile2.BringToFront();
	}

	private void main_viewallToolStripMenuItem_Click(object sender, EventArgs e)
	{
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Expected O, but got Unknown
		if (mSearchMain.dataTableFilter == null)
		{
			return;
		}
		mSearchMain.btnSearch.Select();
		Cursor.Current = Cursors.WaitCursor;
		List<ExportDto> list = new List<ExportDto>();
		foreach (DataRow row in mSearchMain.dataTableFilter.Rows)
		{
			if (bool.Parse(row["IsSelect"].ToString()))
			{
				list.Add(new ExportDto
				{
					Id = new Guid(row["Id"].ToString()),
					Name = $"{mSearchMain.cbbProduct.Text}_{mSearchMain.dtpDateFrom.Value:yyMMdd}_{mSearchMain.dtpDateFrom.Value:yyMMdd}",
					Type = "Product",
					Page = "All"
				});
			}
		}
		if (list.Count.Equals(0))
		{
			MessageBox.Show(Common.getTextLanguage(this, "wRequestNull"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			dgvMain.Focus();
			return;
		}
		try
		{
			ExportExcelDto exportExcelDto = frmLogin.client.ExportAsync((object)list, "/api/Template/ExportForProduct").Result ?? throw new Exception(Common.getTextLanguage(this, "wHasntFile"));
			string text = exportExcelDto.FileName.Replace("\"", "").Replace("\\", "").Replace("/", "");
			if (Path.GetExtension(text).Equals(".zip"))
			{
				saveFileDialogMain.FileName = text;
				if (saveFileDialogMain.ShowDialog().Equals(DialogResult.OK))
				{
					text = saveFileDialogMain.FileName;
					ByteArrayToFile(text, exportExcelDto.Value);
					Common.ExecuteBatFile(text);
				}
				return;
			}
			string text2 = Path.Combine("C:\\Windows\\Temp\\5SQA_System", "VIEWS");
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
			string text3 = ex.Message;
			string text4 = Settings.Default.Language.Replace("rb", "Name");
			List<Language> list2 = Common.ReadLanguages("Error");
			foreach (Language item in list2)
			{
				object value = ((object)item).GetType().GetProperty(text4).GetValue(item, null);
				if (value != null)
				{
					string newValue = value.ToString();
					text3 = text3.Replace(item.Code, newValue);
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
					debugOutput("ERR: " + (string.IsNullOrEmpty(text3) ? ex.Message.Replace("\n", "") : text3));
					MessageBox.Show(string.IsNullOrEmpty(text3) ? ex.Message : text3, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			else
			{
				debugOutput("ERR: " + (string.IsNullOrEmpty(text3) ? ex.Message.Replace("\n", "") : text3));
				MessageBox.Show(string.IsNullOrEmpty(text3) ? ex.Message : text3, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
	}

	private void main_fileToolStripMenuItem_Click(object sender, EventArgs e)
	{
		mSearchMain.btnSearch.Select();
		if (mId.Equals(Guid.Empty))
		{
			MessageBox.Show(Common.getTextLanguage(this, "wSelectRow"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		if (string.IsNullOrEmpty(dgvMain.CurrentRow.Cells["Link"].Value.ToString()))
		{
			MessageBox.Show(Common.getTextLanguage(this, "wNoFile"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		try
		{
			ExportExcelDto exportExcelDto = frmLogin.client.DownloadAsync(mId, "/api/Request/DownloadFile/{id}").Result ?? throw new Exception(Common.getTextLanguage(this, "wHasntFile"));
			string fileName = exportExcelDto.FileName;
			string text = Path.Combine("C:\\Windows\\Temp\\5SQA_System", "VIEWS");
			Directory.CreateDirectory(text);
			fileName = Path.Combine(text, fileName);
			if (File.Exists(fileName))
			{
				File.Delete(fileName);
			}
			Cursor.Current = Cursors.WaitCursor;
			if (ByteArrayToFile(fileName, exportExcelDto.Value))
			{
				WebBrowser webBrowser = new WebBrowser();
				webBrowser.Navigate(fileName, newWindow: false);
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

	private void dgvMain_MouseClick(object sender, MouseEventArgs e)
	{
		mPanelButtonFilterMain.CloseFormFilter();
		base.Controls.RemoveByKey("mPanelSelectFile");
	}

	private void dgvMain_SizeChanged(object sender, EventArgs e)
	{
		mPanelButtonFilterMain.SetFilterControls();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_5S_QA_System.frmExportProduct));
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
		this.saveFileDialogMain = new System.Windows.Forms.SaveFileDialog();
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.sprogbarStatus = new System.Windows.Forms.ToolStripProgressBar();
		this.slblStatus = new System.Windows.Forms.ToolStripStatusLabel();
		this.contextMenuStripMain = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.main_refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparatorView = new System.Windows.Forms.ToolStripSeparator();
		this.main_viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.main_fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.enall_pageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.unall_pageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.statusStripfrmMain = new System.Windows.Forms.StatusStrip();
		this.dgvMain = new System.Windows.Forms.DataGridView();
		this.panelLogout = new System.Windows.Forms.Panel();
		this.lblFullname = new System.Windows.Forms.Label();
		this.ptbAvatar = new System.Windows.Forms.PictureBox();
		this.mPanelViewMain = new _5S_QA_System.mPanelViewManager();
		this.mPanelButtonFilterMain = new _5S_QA_System.mPanelButtonFilter();
		this.mSearchMain = new _5S_QA_System.mSearchExport();
		this.IsSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
		this.No = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ProductId = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ProductStage = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ProductCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ProductImageUrl = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ProductCavity = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Lot = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Line = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Intention = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.InputDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Supplier = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Sample = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Judgement = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Link = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Completed = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.CompletedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Checked = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.CheckedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Approved = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ApprovedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.TotalComment = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Created = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Modified = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.CreatedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ModifiedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.IsActivated = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.contextMenuStripMain.SuspendLayout();
		this.statusStripfrmMain.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvMain).BeginInit();
		this.panelLogout.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.ptbAvatar).BeginInit();
		base.SuspendLayout();
		this.saveFileDialogMain.Filter = "File zip (*.zip)| *.zip";
		this.saveFileDialogMain.Title = "Select the path to save the file";
		this.sprogbarStatus.Name = "sprogbarStatus";
		this.sprogbarStatus.Size = new System.Drawing.Size(200, 20);
		this.slblStatus.Name = "slblStatus";
		this.slblStatus.Size = new System.Drawing.Size(16, 21);
		this.slblStatus.Text = "...";
		this.contextMenuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.main_refreshToolStripMenuItem, this.toolStripSeparatorView, this.main_viewToolStripMenuItem, this.main_fileToolStripMenuItem, this.toolStripSeparator1, this.enall_pageToolStripMenuItem, this.unall_pageToolStripMenuItem });
		this.contextMenuStripMain.Name = "contextMenuStripStaff";
		this.contextMenuStripMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
		this.contextMenuStripMain.Size = new System.Drawing.Size(197, 126);
		this.main_refreshToolStripMenuItem.Name = "main_refreshToolStripMenuItem";
		this.main_refreshToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
		this.main_refreshToolStripMenuItem.Text = "Refresh";
		this.main_refreshToolStripMenuItem.Click += new System.EventHandler(main_refreshToolStripMenuItem_Click);
		this.toolStripSeparatorView.Name = "toolStripSeparatorView";
		this.toolStripSeparatorView.Size = new System.Drawing.Size(193, 6);
		this.main_viewToolStripMenuItem.Name = "main_viewToolStripMenuItem";
		this.main_viewToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
		this.main_viewToolStripMenuItem.Text = "Export all";
		this.main_viewToolStripMenuItem.Click += new System.EventHandler(main_viewallToolStripMenuItem_Click);
		this.main_fileToolStripMenuItem.Name = "main_fileToolStripMenuItem";
		this.main_fileToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
		this.main_fileToolStripMenuItem.Text = "Result file";
		this.main_fileToolStripMenuItem.Visible = false;
		this.main_fileToolStripMenuItem.Click += new System.EventHandler(main_fileToolStripMenuItem_Click);
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
		this.statusStripfrmMain.TabIndex = 23;
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
		this.dgvMain.Columns.AddRange(this.IsSelect, this.No, this.Code, this.name, this.ProductId, this.ProductStage, this.ProductCode, this.ProductName, this.ProductImageUrl, this.ProductCavity, this.Date, this.Quantity, this.Lot, this.Line, this.Intention, this.InputDate, this.Supplier, this.Sample, this.Type, this.Status, this.Judgement, this.Link, this.Completed, this.CompletedBy, this.Checked, this.CheckedBy, this.Approved, this.ApprovedBy, this.TotalComment, this.Id, this.Created, this.Modified, this.CreatedBy, this.ModifiedBy, this.IsActivated);
		this.dgvMain.ContextMenuStrip = this.contextMenuStripMain;
		this.dgvMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.dgvMain.EnableHeadersVisualStyles = false;
		this.dgvMain.Location = new System.Drawing.Point(20, 155);
		this.dgvMain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.dgvMain.MultiSelect = false;
		this.dgvMain.Name = "dgvMain";
		this.dgvMain.RowHeadersWidth = 25;
		this.dgvMain.Size = new System.Drawing.Size(1060, 399);
		this.dgvMain.TabIndex = 34;
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
		this.mPanelViewMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.mPanelViewMain.Dock = System.Windows.Forms.DockStyle.Right;
		this.mPanelViewMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.mPanelViewMain.Location = new System.Drawing.Point(380, 155);
		this.mPanelViewMain.Margin = new System.Windows.Forms.Padding(4);
		this.mPanelViewMain.Name = "mPanelViewMain";
		this.mPanelViewMain.Size = new System.Drawing.Size(700, 399);
		this.mPanelViewMain.TabIndex = 32;
		this.mPanelButtonFilterMain.AutoSize = true;
		this.mPanelButtonFilterMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.mPanelButtonFilterMain.Dock = System.Windows.Forms.DockStyle.Top;
		this.mPanelButtonFilterMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.mPanelButtonFilterMain.Location = new System.Drawing.Point(20, 134);
		this.mPanelButtonFilterMain.Margin = new System.Windows.Forms.Padding(0);
		this.mPanelButtonFilterMain.mFilters = (System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>)resources.GetObject("mPanelButtonFilterMain.mFilters");
		this.mPanelButtonFilterMain.Name = "mPanelButtonFilterMain";
		this.mPanelButtonFilterMain.Size = new System.Drawing.Size(1060, 21);
		this.mPanelButtonFilterMain.TabIndex = 33;
		this.mSearchMain.AutoSize = true;
		this.mSearchMain.BackColor = System.Drawing.SystemColors.Control;
		this.mSearchMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.mSearchMain.dataTable = null;
		this.mSearchMain.dataTableFilter = null;
		this.mSearchMain.Dock = System.Windows.Forms.DockStyle.Top;
		this.mSearchMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.mSearchMain.Location = new System.Drawing.Point(20, 70);
		this.mSearchMain.Name = "mSearchMain";
		this.mSearchMain.Padding = new System.Windows.Forms.Padding(3);
		this.mSearchMain.Size = new System.Drawing.Size(1060, 64);
		this.mSearchMain.TabIndex = 30;
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
		this.Code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Code.DataPropertyName = "Code";
		this.Code.FillWeight = 20f;
		this.Code.HeaderText = "Code";
		this.Code.Name = "Code";
		this.Code.ReadOnly = true;
		this.Code.Visible = false;
		this.name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.name.DataPropertyName = "Name";
		this.name.FillWeight = 30f;
		this.name.HeaderText = "Name";
		this.name.Name = "name";
		this.name.ReadOnly = true;
		this.name.Visible = false;
		this.ProductId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.ProductId.DataPropertyName = "ProductId";
		this.ProductId.FillWeight = 40f;
		this.ProductId.HeaderText = "Product id";
		this.ProductId.Name = "ProductId";
		this.ProductId.ReadOnly = true;
		this.ProductId.Visible = false;
		this.ProductStage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.ProductStage.DataPropertyName = "ProductStage";
		this.ProductStage.FillWeight = 20f;
		this.ProductStage.HeaderText = "Stage name";
		this.ProductStage.Name = "ProductStage";
		this.ProductStage.ReadOnly = true;
		this.ProductCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.ProductCode.DataPropertyName = "ProductCode";
		this.ProductCode.FillWeight = 20f;
		this.ProductCode.HeaderText = "Product code";
		this.ProductCode.Name = "ProductCode";
		this.ProductCode.ReadOnly = true;
		this.ProductCode.Visible = false;
		this.ProductName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.ProductName.DataPropertyName = "ProductName";
		this.ProductName.FillWeight = 20f;
		this.ProductName.HeaderText = "Product name";
		this.ProductName.Name = "ProductName";
		this.ProductName.ReadOnly = true;
		this.ProductName.Visible = false;
		this.ProductImageUrl.DataPropertyName = "ProductImageUrl";
		this.ProductImageUrl.HeaderText = "Product image";
		this.ProductImageUrl.Name = "ProductImageUrl";
		this.ProductImageUrl.ReadOnly = true;
		this.ProductImageUrl.Visible = false;
		this.ProductCavity.DataPropertyName = "ProductCavity";
		this.ProductCavity.HeaderText = "Product cavity";
		this.ProductCavity.Name = "ProductCavity";
		this.ProductCavity.ReadOnly = true;
		this.ProductCavity.Visible = false;
		this.Date.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Date.DataPropertyName = "Date";
		this.Date.FillWeight = 20f;
		this.Date.HeaderText = "Date";
		this.Date.Name = "Date";
		this.Date.ReadOnly = true;
		this.Quantity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Quantity.DataPropertyName = "Quantity";
		this.Quantity.FillWeight = 20f;
		this.Quantity.HeaderText = "Q.ty";
		this.Quantity.Name = "Quantity";
		this.Quantity.ReadOnly = true;
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
		this.Intention.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Intention.DataPropertyName = "Intention";
		this.Intention.FillWeight = 20f;
		this.Intention.HeaderText = "Intention";
		this.Intention.Name = "Intention";
		this.Intention.ReadOnly = true;
		this.InputDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.InputDate.DataPropertyName = "InputDate";
		this.InputDate.FillWeight = 20f;
		this.InputDate.HeaderText = "Input date";
		this.InputDate.Name = "InputDate";
		this.InputDate.ReadOnly = true;
		this.InputDate.Visible = false;
		this.Supplier.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Supplier.DataPropertyName = "Supplier";
		this.Supplier.FillWeight = 20f;
		this.Supplier.HeaderText = "Supplier";
		this.Supplier.Name = "Supplier";
		this.Supplier.ReadOnly = true;
		this.Sample.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Sample.DataPropertyName = "Sample";
		this.Sample.FillWeight = 20f;
		this.Sample.HeaderText = "Sample";
		this.Sample.Name = "Sample";
		this.Sample.ReadOnly = true;
		this.Type.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Type.DataPropertyName = "Type";
		this.Type.FillWeight = 15f;
		this.Type.HeaderText = "Type";
		this.Type.Name = "Type";
		this.Type.ReadOnly = true;
		this.Status.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Status.DataPropertyName = "Status";
		this.Status.FillWeight = 20f;
		this.Status.HeaderText = "Status";
		this.Status.Name = "Status";
		this.Status.ReadOnly = true;
		this.Judgement.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Judgement.DataPropertyName = "Judgement";
		this.Judgement.FillWeight = 20f;
		this.Judgement.HeaderText = "Judgement";
		this.Judgement.Name = "Judgement";
		this.Judgement.ReadOnly = true;
		this.Link.DataPropertyName = "Link";
		this.Link.HeaderText = "Link";
		this.Link.Name = "Link";
		this.Link.ReadOnly = true;
		this.Link.Visible = false;
		this.Completed.DataPropertyName = "Completed";
		this.Completed.HeaderText = "Prepared";
		this.Completed.Name = "Completed";
		this.Completed.ReadOnly = true;
		this.Completed.Visible = false;
		this.CompletedBy.DataPropertyName = "CompletedBy";
		this.CompletedBy.HeaderText = "Prepared by";
		this.CompletedBy.Name = "CompletedBy";
		this.CompletedBy.ReadOnly = true;
		this.CompletedBy.Visible = false;
		this.Checked.DataPropertyName = "Checked";
		this.Checked.HeaderText = "Checked";
		this.Checked.Name = "Checked";
		this.Checked.ReadOnly = true;
		this.Checked.Visible = false;
		this.CheckedBy.DataPropertyName = "CheckedBy";
		this.CheckedBy.HeaderText = "Checked by";
		this.CheckedBy.Name = "CheckedBy";
		this.CheckedBy.ReadOnly = true;
		this.CheckedBy.Visible = false;
		this.Approved.DataPropertyName = "Approved";
		this.Approved.HeaderText = "Approved";
		this.Approved.Name = "Approved";
		this.Approved.ReadOnly = true;
		this.Approved.Visible = false;
		this.ApprovedBy.DataPropertyName = "ApprovedBy";
		this.ApprovedBy.HeaderText = "Approved by";
		this.ApprovedBy.Name = "ApprovedBy";
		this.ApprovedBy.ReadOnly = true;
		this.ApprovedBy.Visible = false;
		this.TotalComment.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.TotalComment.DataPropertyName = "TotalComment";
		this.TotalComment.FillWeight = 15f;
		this.TotalComment.HeaderText = "Comment";
		this.TotalComment.Name = "TotalComment";
		this.TotalComment.ReadOnly = true;
		this.TotalComment.Visible = false;
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
		this.ModifiedBy.DataPropertyName = "modifiedby";
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
		base.Controls.Add(this.statusStripfrmMain);
		base.Controls.Add(this.mPanelButtonFilterMain);
		base.Controls.Add(this.mSearchMain);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "frmExportProduct";
		base.Padding = new System.Windows.Forms.Padding(20, 70, 20, 20);
		this.Text = "5S QA System * EXPORT";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmExportProduct_FormClosing);
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmExportProduct_FormClosed);
		base.Load += new System.EventHandler(frmExportProduct_Load);
		base.Shown += new System.EventHandler(frmExportProduct_Shown);
		this.contextMenuStripMain.ResumeLayout(false);
		this.statusStripfrmMain.ResumeLayout(false);
		this.statusStripfrmMain.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvMain).EndInit();
		this.panelLogout.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.ptbAvatar).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
