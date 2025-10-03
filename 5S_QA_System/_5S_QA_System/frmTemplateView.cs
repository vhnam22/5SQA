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
using MetroFramework.Controls;
using MetroFramework.Forms;
using OfficeOpenXml;

namespace _5S_QA_System;

public class frmTemplateView : MetroForm
{
	private readonly Form mForm;

	private Guid mId;

	public bool isEdit;

	private int mRow;

	private int mCol;

	public bool isClose;

	private IContainer components = null;

	private ToolTip toolTipMain;

	private StatusStrip statusStripfrmMain;

	private ToolStripProgressBar sprogbarStatus;

	public ToolStripStatusLabel slblStatus;

	private ContextMenuStrip contextMenuStripMain;

	private ToolStripMenuItem main_refreshToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripMenuItem main_newToolStripMenuItem;

	private ToolStripMenuItem main_editToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator6;

	private ToolStripMenuItem main_deleteToolStripMenuItem;

	private DataGridView dgvMain;

	private mPanelGroup mPanelViewMain;

	private ToolStripSeparator toolStripSeparatorImport;

	private ToolStripMenuItem main_importToolStripMenuItem;

	private OpenFileDialog openFileDialogMain;

	private Panel panelmProgressBarMain;

	private MetroProgressBar mProgressBarMain;

	private PictureBox pictureBox1;

	private mSearch mSearchMain;

	private Panel panelLogout;

	private Label lblFullname;

	private PictureBox ptbAvatar;

	private DataGridViewTextBoxColumn No;

	private DataGridViewTextBoxColumn Code;

	private DataGridViewTextBoxColumn name;

	private DataGridViewTextBoxColumn Description;

	private DataGridViewTextBoxColumn Limit;

	private DataGridViewTextBoxColumn Type;

	private DataGridViewTextBoxColumn TemplateUrl;

	private DataGridViewTextBoxColumn TemplateData;

	private DataGridViewTextBoxColumn Id;

	private new DataGridViewTextBoxColumn Created;

	private DataGridViewTextBoxColumn Modified;

	private DataGridViewTextBoxColumn CreatedBy;

	private DataGridViewTextBoxColumn ModifiedBy;

	private DataGridViewTextBoxColumn IsActivated;

	public frmTemplateView(Form frm = null)
	{
		InitializeComponent();
		Common.setControls(this, toolTipMain, new List<ContextMenuStrip> { contextMenuStripMain });
		mForm = frm;
	}

	private void frmTemplateView_Load(object sender, EventArgs e)
	{
		Init();
	}

	private void frmTemplateView_Shown(object sender, EventArgs e)
	{
		load_AllData();
	}

	private void frmTemplateView_FormClosing(object sender, FormClosingEventArgs e)
	{
		GC.Collect();
	}

	private void frmTemplateView_FormClosed(object sender, FormClosedEventArgs e)
	{
		if (!isClose && mForm != null)
		{
			if (mForm is frmMeasurementAdd frmMeasurementAdd2)
			{
				frmMeasurementAdd2.load_cbbTemplate();
			}
			if (mForm is frmProductAdd frmProductAdd2)
			{
				frmProductAdd2.load_cbbTemplate();
			}
		}
	}

	private void Init()
	{
		panelmProgressBarMain.BringToFront();
		panelmProgressBarMain.Visible = false;
		mPanelViewMain.Visible = false;
		isClose = true;
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
		mSearchMain.btnSearch.Click += btnSearch_Click;
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
			en_disControl(enable: true);
			QueryArgs body = new QueryArgs
			{
				Order = "Created DESC",
				Page = 1,
				Limit = int.MaxValue
			};
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/Template/Gets").Result;
			mSearchMain.Init(Common.getDataTable<TemplateViewModel>(result), dgvMain);
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
		dgvMain.DataSource = mSearchMain.SearchInAllColums();
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

	private void en_disControl(bool enable)
	{
		mPanelViewMain.btnConfirm.Enabled = enable;
		main_newToolStripMenuItem.Enabled = enable;
		main_editToolStripMenuItem.Enabled = enable;
		main_deleteToolStripMenuItem.Enabled = enable;
		if (enable)
		{
			main_importToolStripMenuItem.Text = Common.getTextLanguage(this, "Import");
		}
		else
		{
			main_importToolStripMenuItem.Text = Common.getTextLanguage(this, "Confirm");
		}
		toolStripSeparatorImport.Visible = false;
		main_importToolStripMenuItem.Visible = false;
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
			Guid guid = Guid.Parse(dataGridView.CurrentRow.Cells["Id"].Value.ToString());
			if (!guid.Equals(mId))
			{
				mId = guid;
				mPanelViewMain.load_dgvContent(FormType.VIEW);
			}
		}
		else
		{
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
		foreach (DataGridViewRow item in (IEnumerable)dataGridView.Rows)
		{
			item.Cells["No"].Value = (result - 1) * result2 + item.Index + 1;
			if (main_importToolStripMenuItem.Text.Equals(Common.getTextLanguage(this, "Confirm")))
			{
				item.DefaultCellStyle.ForeColor = Color.Green;
			}
			else if (item.Cells["TemplateUrl"].Value == null || string.IsNullOrEmpty(item.Cells["TemplateUrl"].Value.ToString()))
			{
				item.DefaultCellStyle.ForeColor = SystemColors.GrayText;
			}
		}
	}

	private void btnSearch_Click(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		load_dgvMain();
	}

	private void main_refreshToolStripMenuItem_Click(object sender, EventArgs e)
	{
		mSearchMain.btnSearch.Select();
		load_AllData();
	}

	private void main_newToolStripMenuItem_Click(object sender, EventArgs e)
	{
		mSearchMain.btnSearch.Select();
		Cursor.Current = Cursors.WaitCursor;
		if (frmLogin.User.Role.Equals(RoleWeb.Administrator) && frmLogin.User.JobTitle != "Manager" && frmLogin.User.JobTitle != "Supervisor")
		{
			MessageBox.Show(Common.getTextLanguage(typeof(frmLogin).Name, "wNoAuthorization"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		mPanelViewMain.Visible = true;
		mPanelViewMain.load_dgvContent(FormType.ADD);
	}

	private void main_editToolStripMenuItem_Click(object sender, EventArgs e)
	{
		mSearchMain.btnSearch.Select();
		Cursor.Current = Cursors.WaitCursor;
		if (mId.Equals(Guid.Empty))
		{
			MessageBox.Show(Common.getTextLanguage(this, "wSelectRow"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		if (frmLogin.User.Role.Equals(RoleWeb.Administrator) && frmLogin.User.JobTitle != "Manager" && frmLogin.User.JobTitle != "Supervisor")
		{
			MessageBox.Show(Common.getTextLanguage(typeof(frmLogin).Name, "wNoAuthorization"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		mPanelViewMain.Visible = true;
		mPanelViewMain.load_dgvContent(FormType.EDIT);
	}

	private void main_deleteToolStripMenuItem_Click(object sender, EventArgs e)
	{
		mSearchMain.btnSearch.Select();
		if (mId.Equals(Guid.Empty))
		{
			MessageBox.Show(Common.getTextLanguage(this, "wSelectRow"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
		else if (frmLogin.User.Role.Equals(RoleWeb.Administrator) && frmLogin.User.JobTitle != "Manager" && frmLogin.User.JobTitle != "Supervisor")
		{
			MessageBox.Show(Common.getTextLanguage(typeof(frmLogin).Name, "wNoAuthorization"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
		else
		{
			if (!MessageBox.Show(string.Format("{0} {1}", Common.getTextLanguage(this, "wSureDelete"), dgvMain.CurrentRow.Cells["Name"].Value), Common.getTextLanguage(this, "QUESTION"), MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.Yes))
			{
				return;
			}
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				start_Proccessor();
				ResponseDto result = frmLogin.client.DeleteAsync(mId, "/api/Template/Delete/{id}").Result;
				if (!result.Success)
				{
					throw new Exception(result.Messages.ElementAt(0).Message);
				}
				isClose = false;
				main_refreshToolStripMenuItem.PerformClick();
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
						debugOutput("ERR: " + (string.IsNullOrEmpty(textLanguage) ? ex.Message.Replace("\n", "") : textLanguage));
						MessageBox.Show(string.IsNullOrEmpty(textLanguage) ? ex.Message : textLanguage, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
					}
				}
				else
				{
					debugOutput("ERR: " + (string.IsNullOrEmpty(textLanguage) ? ex.Message.Replace("\n", "") : textLanguage));
					MessageBox.Show(string.IsNullOrEmpty(textLanguage) ? ex.Message : textLanguage, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
		}
	}

	private void main_importToolStripMenuItem_Click(object sender, EventArgs e)
	{
		//IL_0573: Unknown result type (might be due to invalid IL or missing references)
		//IL_0578: Unknown result type (might be due to invalid IL or missing references)
		//IL_0590: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c2: Expected O, but got Unknown
		mSearchMain.btnSearch.Select();
		if (frmLogin.User.Role.Equals(RoleWeb.Administrator) && frmLogin.User.JobTitle != "Manager" && frmLogin.User.JobTitle != "Supervisor")
		{
			MessageBox.Show(Common.getTextLanguage(typeof(frmLogin).Name, "wNoAuthorization"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		panelmProgressBarMain.Visible = true;
		mProgressBarMain.Value = mProgressBarMain.Maximum;
		if (main_importToolStripMenuItem.Text.Equals(Common.getTextLanguage(this, "Import")))
		{
			string[] templateHeaders = MetaType.TemplateHeaders;
			MessageBox.Show(Common.getTextLanguage(this, "FormatHeader") + "\r\n    " + string.Join(" - ", templateHeaders) + "\r\n" + Common.getTextLanguage(this, "NextRow"), Common.getTextLanguage(this, "INFORMATION"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			openFileDialogMain.FileName = "Template";
			openFileDialogMain.Title = Common.getTextLanguage(this, "SelectTemplate");
			if (openFileDialogMain.ShowDialog().Equals(DialogResult.OK))
			{
				Cursor.Current = Cursors.WaitCursor;
				dgvMain_CellClick(this, null);
				try
				{
					DataTable dataTable = new DataTable();
					foreach (DataGridViewColumn column in dgvMain.Columns)
					{
						dataTable.Columns.Add(column.Name);
					}
					FileInfo newFile = new FileInfo(openFileDialogMain.FileName);
					using (ExcelPackage excelPackage = new ExcelPackage(newFile))
					{
						if (excelPackage.Workbook.Worksheets.Count < 1)
						{
							throw new Exception(Common.getTextLanguage(this, "IncorrectFormat"));
						}
						ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets["Data"] ?? throw new Exception(Common.getTextLanguage(this, "HasntSheet"));
						if (excelWorksheet.Dimension == null)
						{
							throw new Exception(Common.getTextLanguage(this, "SheetNull"));
						}
						ExcelRange excelRange = excelWorksheet.Cells[1, 1, excelWorksheet.Dimension.End.Row, excelWorksheet.Dimension.End.Column];
						if (!excelRange.Any())
						{
							throw new Exception(Common.getTextLanguage(this, "ContentNull"));
						}
						string text = Common.checkFormat(templateHeaders, excelRange);
						if (!text.Equals("Ok"))
						{
							throw new Exception(text);
						}
						mProgressBarMain.Maximum = excelWorksheet.Dimension.Rows;
						for (int i = 1; i < excelWorksheet.Dimension.Rows; i++)
						{
							mProgressBarMain.Value = i;
							DataRow dataRow = dataTable.NewRow();
							for (int j = 0; j < templateHeaders.Length; j++)
							{
								string text2 = Common.trimSpace(excelRange[1, j + 1].Value.ToString()).ToLower();
								if (text2.Equals(templateHeaders[j].ToLower()))
								{
									dataRow[text2] = Common.trimSpace((excelRange[i + 1, j + 1].Value == null) ? "" : excelRange[i + 1, j + 1].Value.ToString());
								}
							}
							dataRow["Id"] = Guid.Empty;
							dataTable.Rows.Add(dataRow);
							dataTable.AcceptChanges();
						}
					}
					dataTable.Dispose();
					en_disControl(enable: false);
					mProgressBarMain.Value = mProgressBarMain.Maximum;
					mSearchMain.Init(dataTable, dgvMain);
					load_dgvMain();
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
		}
		else if (MessageBox.Show(Common.getTextLanguage(this, "wSureImport"), Common.getTextLanguage(this, "QUESTION"), MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.Yes))
		{
			Cursor.Current = Cursors.WaitCursor;
			DataTable dataTable2 = mSearchMain.dataTable;
			dataTable2.Dispose();
			mProgressBarMain.Maximum = dataTable2.Rows.Count;
			try
			{
				List<TemplateViewModel> list = new List<TemplateViewModel>();
				foreach (DataRow row in dataTable2.Rows)
				{
					int value = dataTable2.Rows.IndexOf(row) + 1;
					mProgressBarMain.Value = value;
					TemplateViewModel val = new TemplateViewModel
					{
						Code = row["Code"].ToString(),
						Name = row["Name"].ToString(),
						Type = row["Type"].ToString()
					};
					if (!string.IsNullOrEmpty(row["Description"].ToString()))
					{
						val.Description = row["Description"].ToString();
					}
					if (int.TryParse(row["Limit"].ToString(), out var result))
					{
						val.Limit = result;
					}
					list.Add(val);
				}
				ResponseDto result2 = frmLogin.client.SaveAsync(list, "/api/Template/SaveList").Result;
				if (!result2.Success)
				{
					string text3 = string.Empty;
					foreach (ResponseMessage message in result2.Messages)
					{
						text3 = text3 + message.Message + "\r\n";
					}
					throw new Exception(text3);
				}
				main_refreshToolStripMenuItem.PerformClick();
			}
			catch (Exception ex2)
			{
				if (ex2.InnerException is ApiException { StatusCode: var statusCode } ex3)
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
						debugOutput("ERR: " + ex3.Message.Replace("\n", ""));
						MessageBox.Show(ex3.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
					}
				}
				else
				{
					debugOutput("ERR: " + ex2.Message);
					MessageBox.Show(ex2.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
		}
		panelmProgressBarMain.Visible = false;
	}

	private void ptbAvatar_DoubleClick(object sender, EventArgs e)
	{
		Common.activeForm(typeof(frmLogin));
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_5S_QA_System.frmTemplateView));
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.statusStripfrmMain = new System.Windows.Forms.StatusStrip();
		this.sprogbarStatus = new System.Windows.Forms.ToolStripProgressBar();
		this.slblStatus = new System.Windows.Forms.ToolStripStatusLabel();
		this.contextMenuStripMain = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.main_refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.main_newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.main_editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.main_deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparatorImport = new System.Windows.Forms.ToolStripSeparator();
		this.main_importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.dgvMain = new System.Windows.Forms.DataGridView();
		this.openFileDialogMain = new System.Windows.Forms.OpenFileDialog();
		this.panelmProgressBarMain = new System.Windows.Forms.Panel();
		this.mProgressBarMain = new MetroFramework.Controls.MetroProgressBar();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.panelLogout = new System.Windows.Forms.Panel();
		this.lblFullname = new System.Windows.Forms.Label();
		this.ptbAvatar = new System.Windows.Forms.PictureBox();
		this.mPanelViewMain = new _5S_QA_System.View.User_control.mPanelGroup();
		this.mSearchMain = new _5S_QA_System.View.User_control.mSearch();
		this.No = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Limit = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.TemplateUrl = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.TemplateData = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Created = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Modified = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.CreatedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ModifiedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.IsActivated = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.statusStripfrmMain.SuspendLayout();
		this.contextMenuStripMain.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvMain).BeginInit();
		this.panelmProgressBarMain.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		this.panelLogout.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.ptbAvatar).BeginInit();
		base.SuspendLayout();
		this.statusStripfrmMain.BackColor = System.Drawing.SystemColors.ControlLight;
		this.statusStripfrmMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f);
		this.statusStripfrmMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.sprogbarStatus, this.slblStatus });
		this.statusStripfrmMain.Location = new System.Drawing.Point(20, 554);
		this.statusStripfrmMain.Name = "statusStripfrmMain";
		this.statusStripfrmMain.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
		this.statusStripfrmMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
		this.statusStripfrmMain.Size = new System.Drawing.Size(1060, 26);
		this.statusStripfrmMain.SizingGrip = false;
		this.statusStripfrmMain.Stretch = false;
		this.statusStripfrmMain.TabIndex = 7;
		this.sprogbarStatus.Name = "sprogbarStatus";
		this.sprogbarStatus.Size = new System.Drawing.Size(200, 20);
		this.slblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.slblStatus.Name = "slblStatus";
		this.slblStatus.Size = new System.Drawing.Size(16, 21);
		this.slblStatus.Text = "...";
		this.contextMenuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.main_refreshToolStripMenuItem, this.toolStripSeparator1, this.main_newToolStripMenuItem, this.main_editToolStripMenuItem, this.toolStripSeparator6, this.main_deleteToolStripMenuItem, this.toolStripSeparatorImport, this.main_importToolStripMenuItem });
		this.contextMenuStripMain.Name = "contextMenuStripStaff";
		this.contextMenuStripMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
		this.contextMenuStripMain.Size = new System.Drawing.Size(114, 132);
		this.main_refreshToolStripMenuItem.Name = "main_refreshToolStripMenuItem";
		this.main_refreshToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
		this.main_refreshToolStripMenuItem.Text = "Refresh";
		this.main_refreshToolStripMenuItem.Click += new System.EventHandler(main_refreshToolStripMenuItem_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(110, 6);
		this.main_newToolStripMenuItem.Name = "main_newToolStripMenuItem";
		this.main_newToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
		this.main_newToolStripMenuItem.Text = "New";
		this.main_newToolStripMenuItem.Click += new System.EventHandler(main_newToolStripMenuItem_Click);
		this.main_editToolStripMenuItem.Name = "main_editToolStripMenuItem";
		this.main_editToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
		this.main_editToolStripMenuItem.Text = "Edit";
		this.main_editToolStripMenuItem.Click += new System.EventHandler(main_editToolStripMenuItem_Click);
		this.toolStripSeparator6.Name = "toolStripSeparator6";
		this.toolStripSeparator6.Size = new System.Drawing.Size(110, 6);
		this.main_deleteToolStripMenuItem.Name = "main_deleteToolStripMenuItem";
		this.main_deleteToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
		this.main_deleteToolStripMenuItem.Text = "Delete";
		this.main_deleteToolStripMenuItem.Click += new System.EventHandler(main_deleteToolStripMenuItem_Click);
		this.toolStripSeparatorImport.Name = "toolStripSeparatorImport";
		this.toolStripSeparatorImport.Size = new System.Drawing.Size(110, 6);
		this.main_importToolStripMenuItem.Name = "main_importToolStripMenuItem";
		this.main_importToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
		this.main_importToolStripMenuItem.Text = "Import";
		this.main_importToolStripMenuItem.Click += new System.EventHandler(main_importToolStripMenuItem_Click);
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
		this.dgvMain.Columns.AddRange(this.No, this.Code, this.name, this.Description, this.Limit, this.Type, this.TemplateUrl, this.TemplateData, this.Id, this.Created, this.Modified, this.CreatedBy, this.ModifiedBy, this.IsActivated);
		this.dgvMain.ContextMenuStrip = this.contextMenuStripMain;
		this.dgvMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.dgvMain.EnableHeadersVisualStyles = false;
		this.dgvMain.Location = new System.Drawing.Point(20, 134);
		this.dgvMain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.dgvMain.MultiSelect = false;
		this.dgvMain.Name = "dgvMain";
		this.dgvMain.ReadOnly = true;
		this.dgvMain.RowHeadersWidth = 25;
		this.dgvMain.Size = new System.Drawing.Size(1060, 420);
		this.dgvMain.TabIndex = 8;
		this.dgvMain.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvMain_CellClick);
		this.dgvMain.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvMain_CellDoubleClick);
		this.dgvMain.CurrentCellChanged += new System.EventHandler(dgvMain_CurrentCellChanged);
		this.dgvMain.Sorted += new System.EventHandler(dgvMain_Sorted);
		this.openFileDialogMain.FileName = "Template";
		this.openFileDialogMain.Filter = "File excel (*.xls, *.xlsx)| *.xls; *.xlsx";
		this.openFileDialogMain.Title = "Select template excel";
		this.panelmProgressBarMain.Anchor = System.Windows.Forms.AnchorStyles.None;
		this.panelmProgressBarMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panelmProgressBarMain.Controls.Add(this.mProgressBarMain);
		this.panelmProgressBarMain.Controls.Add(this.pictureBox1);
		this.panelmProgressBarMain.Location = new System.Drawing.Point(489, 237);
		this.panelmProgressBarMain.Name = "panelmProgressBarMain";
		this.panelmProgressBarMain.Size = new System.Drawing.Size(122, 128);
		this.panelmProgressBarMain.TabIndex = 30;
		this.mProgressBarMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.mProgressBarMain.Location = new System.Drawing.Point(0, 120);
		this.mProgressBarMain.Name = "mProgressBarMain";
		this.mProgressBarMain.Size = new System.Drawing.Size(120, 6);
		this.mProgressBarMain.TabIndex = 18;
		this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
		this.pictureBox1.Image = _5S_QA_System.Properties.Resources._5S_QA_S;
		this.pictureBox1.Location = new System.Drawing.Point(0, 0);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(120, 120);
		this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.pictureBox1.TabIndex = 17;
		this.pictureBox1.TabStop = false;
		this.panelLogout.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.panelLogout.BackColor = System.Drawing.Color.Transparent;
		this.panelLogout.Controls.Add(this.lblFullname);
		this.panelLogout.Controls.Add(this.ptbAvatar);
		this.panelLogout.Location = new System.Drawing.Point(730, 27);
		this.panelLogout.Name = "panelLogout";
		this.panelLogout.Size = new System.Drawing.Size(350, 42);
		this.panelLogout.TabIndex = 175;
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
		this.mPanelViewMain.Location = new System.Drawing.Point(630, 134);
		this.mPanelViewMain.Margin = new System.Windows.Forms.Padding(4);
		this.mPanelViewMain.Name = "mPanelViewMain";
		this.mPanelViewMain.Size = new System.Drawing.Size(450, 420);
		this.mPanelViewMain.TabIndex = 9;
		this.mSearchMain.AutoSize = true;
		this.mSearchMain.BackColor = System.Drawing.SystemColors.Control;
		this.mSearchMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.mSearchMain.dataTable = null;
		this.mSearchMain.Dock = System.Windows.Forms.DockStyle.Top;
		this.mSearchMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.mSearchMain.Location = new System.Drawing.Point(20, 70);
		this.mSearchMain.Margin = new System.Windows.Forms.Padding(4);
		this.mSearchMain.MaximumSize = new System.Drawing.Size(5000, 64);
		this.mSearchMain.MinimumSize = new System.Drawing.Size(25, 64);
		this.mSearchMain.Name = "mSearchMain";
		this.mSearchMain.Padding = new System.Windows.Forms.Padding(3);
		this.mSearchMain.Size = new System.Drawing.Size(1060, 64);
		this.mSearchMain.TabIndex = 31;
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
		this.Code.FillWeight = 25f;
		this.Code.HeaderText = "Code";
		this.Code.Name = "Code";
		this.Code.ReadOnly = true;
		this.name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.name.DataPropertyName = "Name";
		this.name.FillWeight = 40f;
		this.name.HeaderText = "Name";
		this.name.Name = "name";
		this.name.ReadOnly = true;
		this.Description.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Description.DataPropertyName = "Description";
		this.Description.FillWeight = 40f;
		this.Description.HeaderText = "Description";
		this.Description.Name = "Description";
		this.Description.ReadOnly = true;
		this.Limit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Limit.DataPropertyName = "Limit";
		this.Limit.FillWeight = 25f;
		this.Limit.HeaderText = "Limit";
		this.Limit.Name = "Limit";
		this.Limit.ReadOnly = true;
		this.Type.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Type.DataPropertyName = "Type";
		this.Type.FillWeight = 25f;
		this.Type.HeaderText = "Type";
		this.Type.Name = "Type";
		this.Type.ReadOnly = true;
		this.TemplateUrl.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.TemplateUrl.DataPropertyName = "TemplateUrl";
		this.TemplateUrl.FillWeight = 50f;
		this.TemplateUrl.HeaderText = "Template url";
		this.TemplateUrl.Name = "TemplateUrl";
		this.TemplateUrl.ReadOnly = true;
		this.TemplateData.DataPropertyName = "TemplateData";
		this.TemplateData.HeaderText = "Template data";
		this.TemplateData.Name = "TemplateData";
		this.TemplateData.ReadOnly = true;
		this.TemplateData.Visible = false;
		this.Id.DataPropertyName = "Id";
		this.Id.HeaderText = "Id";
		this.Id.Name = "Id";
		this.Id.ReadOnly = true;
		this.Id.Visible = false;
		this.Created.DataPropertyName = "Created";
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
		base.Controls.Add(this.panelmProgressBarMain);
		base.Controls.Add(this.mPanelViewMain);
		base.Controls.Add(this.dgvMain);
		base.Controls.Add(this.statusStripfrmMain);
		base.Controls.Add(this.mSearchMain);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "frmTemplateView";
		base.Padding = new System.Windows.Forms.Padding(20, 70, 20, 20);
		this.Text = "5S QA System * TEMPLATE";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmTemplateView_FormClosing);
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmTemplateView_FormClosed);
		base.Load += new System.EventHandler(frmTemplateView_Load);
		base.Shown += new System.EventHandler(frmTemplateView_Shown);
		this.statusStripfrmMain.ResumeLayout(false);
		this.statusStripfrmMain.PerformLayout();
		this.contextMenuStripMain.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.dgvMain).EndInit();
		this.panelmProgressBarMain.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		this.panelLogout.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.ptbAvatar).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
