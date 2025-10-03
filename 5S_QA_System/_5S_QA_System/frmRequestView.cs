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
using _5S_QA_System.View.User_control;
using MetroFramework.Controls;
using MetroFramework.Forms;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace _5S_QA_System;

public class frmRequestView : MetroForm
{
	private Guid mId;

	private bool isEdit;

	private int mRow;

	private int mCol;

	private readonly List<string> importantControl = new List<string> { "TypeName", "ProductCode", "LineName", "Date" };

	public bool isThis = false;

	private IContainer components = null;

	private SaveFileDialog saveFileDialogExportExcel;

	private mSearch mSearchMain;

	private ContextMenuStrip contextMenuStripMain;

	private ToolStripMenuItem main_refreshToolStripMenuItem;

	private ToolStripSeparator toolStripSeparatorNew;

	private ToolStripMenuItem main_newToolStripMenuItem;

	private ToolStripMenuItem main_editToolStripMenuItem;

	private ToolStripMenuItem main_completeToolStripMenuItem;

	private ToolStripSeparator toolStripSeparatorDelete;

	private ToolStripMenuItem main_deleteToolStripMenuItem;

	private ToolStripSeparator toolStripSeparatorExport;

	private ToolStripMenuItem main_exportToolStripMenuItem;

	private StatusStrip statusStripfrmMain;

	private ToolStripProgressBar sprogbarStatus;

	public ToolStripStatusLabel slblStatus;

	private ToolTip toolTipMain;

	private DataGridView dgvMain;

	private Panel panelmProgressBarMain;

	private MetroProgressBar mProgressBarMain;

	private PictureBox pictureBox1;

	private mPanelViewExport mPanelViewMain;

	private OpenFileDialog openFileDialogMain;

	private Panel panelLogout;

	private Label lblFullname;

	private PictureBox ptbAvatar;

	private ToolStripMenuItem main_fileToolStripMenuItem;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn16;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn17;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn18;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn19;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn20;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn21;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn22;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn23;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn24;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn25;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn26;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn27;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn28;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn29;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn30;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn31;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn32;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn33;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn34;

	private DataGridViewTextBoxColumn No;

	private DataGridViewTextBoxColumn Code;

	private DataGridViewTextBoxColumn name;

	private DataGridViewTextBoxColumn GroupId;

	private DataGridViewTextBoxColumn ProductId;

	private DataGridViewTextBoxColumn ProductStage;

	private DataGridViewTextBoxColumn ProductCode;

	private new DataGridViewTextBoxColumn ProductName;

	private DataGridViewTextBoxColumn ProductDescription;

	private DataGridViewTextBoxColumn ProductDepartment;

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

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn35;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn36;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn37;

	public frmRequestView()
	{
		InitializeComponent();
		Common.setControls(this, toolTipMain, new List<ContextMenuStrip> { contextMenuStripMain });
		mId = Guid.Empty;
		isEdit = false;
		mRow = 0;
		mCol = 0;
	}

	private void frmRequestView_Load(object sender, EventArgs e)
	{
		Init();
	}

	private void frmRequestView_Shown(object sender, EventArgs e)
	{
		load_AllData(mSearchMain.dtpMain.Value);
		mSearchMain.dtpMain.ValueChanged += dtpMain_ValueChanged;
	}

	private void frmRequestView_FormClosing(object sender, FormClosingEventArgs e)
	{
		GC.Collect();
	}

	private void frmRequestView_FormClosed(object sender, FormClosedEventArgs e)
	{
		mPanelViewMain.mDispose();
		List<Type> list = new List<Type>();
		list.Add(typeof(frmRequestAdd));
		Common.closeForm(list);
	}

	private void Init()
	{
		panelmProgressBarMain.BringToFront();
		panelmProgressBarMain.Visible = false;
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
		mSearchMain.btnSearch.Click += btnSearch_Click;
		mPanelViewMain.btnExport.Click += main_completeToolStripMenuItem_Click;
		mPanelViewMain.btnDelete.Click += main_deleteToolStripMenuItem_Click;
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

	public void load_AllData(DateTime dateTime)
	{
		Cursor.Current = Cursors.WaitCursor;
		try
		{
			start_Proccessor();
			isEdit = true;
			en_disControl(enable: false);
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "Date=@0 && (Status.Contains(@1) || Status.Contains(@2))";
			queryArgs.PredicateParameters = new string[3]
			{
				dateTime.Date.ToString("MM/dd/yyyy"),
				"Activated",
				"Rejected"
			};
			queryArgs.Order = "Created DESC";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs body = queryArgs;
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/Request/Gets").Result;
			mSearchMain.Init(Common.getDataTable<RequestViewModel>(result), dgvMain);
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
		mPanelViewMain.btnExport.Enabled = enable;
		mPanelViewMain.btnDelete.Enabled = enable;
		main_completeToolStripMenuItem.Visible = enable;
		main_deleteToolStripMenuItem.Visible = enable;
		main_editToolStripMenuItem.Visible = enable;
		main_exportToolStripMenuItem.Visible = enable;
		toolStripSeparatorDelete.Visible = enable;
		toolStripSeparatorExport.Visible = enable;
	}

	private int get_Code()
	{
		int result = 1;
		try
		{
			QueryArgs body = new QueryArgs
			{
				Order = "Created DESC",
				Page = 1,
				Limit = 1
			};
			ResponseDto result2 = frmLogin.client.GetsAsync(body, "/api/Request/Gets").Result;
			DataTable dataTable = Common.getDataTable<RequestViewModel>(result2);
			if (dataTable.Rows.Count > 0)
			{
				string[] array = dataTable.Rows[0]["Code"].ToString().Split('-');
				if (array.Length > 1 && int.TryParse(array[1], out var result3))
				{
					result = result3 + 1;
				}
			}
		}
		catch
		{
		}
		return result;
	}

	private string created_Name(DataRow dr)
	{
		string text = string.Format("{0}", get_PositionType(dr["TypeName"].ToString()));
		foreach (string item in importantControl)
		{
			text += (string.IsNullOrEmpty(dr[item].ToString()) ? string.Empty : ("_" + (item.Equals("Date") ? DateTime.Parse(dr[item].ToString()).ToString("yyMMdd") : dr[item].ToString())));
		}
		return Common.trimSpace(text);
	}

	private int get_PositionType(string type)
	{
		int result = 0;
		try
		{
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "TypeId=@0";
			queryArgs.PredicateParameters = new string[1] { "AC5FA813-C9EE-4805-A850-30A5EA5AB0A1" };
			queryArgs.Order = "Created";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs body = queryArgs;
			ResponseDto result2 = frmLogin.client.GetsAsync(body, "/api/MetadataValue/Gets").Result;
			DataTable dataTable = Common.getDataTable<RequestViewModel>(result2);
			foreach (DataRow row in dataTable.Rows)
			{
				if (row["Name"].ToString().Equals(type))
				{
					result = dataTable.Rows.IndexOf(row);
					break;
				}
			}
		}
		catch
		{
		}
		return result;
	}

	public void RefreshFromSocket(RequestViewModel request)
	{
		if (isThis)
		{
			isThis = false;
			return;
		}
		Invoke((MethodInvoker)delegate
		{
			string text = "yyyy/MM";
			if (!mSearchMain.cbSearchAll.Checked)
			{
				text += "/dd";
			}
			if (request.Date.Value.ToString(text) == mSearchMain.dtpMain.Value.ToString(text))
			{
				main_refreshToolStripMenuItem.PerformClick();
			}
		});
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
			MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			return false;
		}
	}

	private void dtpMain_ValueChanged(object sender, EventArgs e)
	{
		main_refreshToolStripMenuItem.PerformClick();
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
			en_disControl(enable: true);
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
			en_disControl(enable: false);
			main_fileToolStripMenuItem.Visible = false;
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
			string text = item.Cells["Status"].Value.ToString();
			string text2 = text;
			if (text2 == "Rejected")
			{
				item.DefaultCellStyle.ForeColor = Color.DarkRed;
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
		load_AllData(mSearchMain.dtpMain.Value);
	}

	private void main_newToolStripMenuItem_Click(object sender, EventArgs e)
	{
		mSearchMain.btnSearch.Select();
		Cursor.Current = Cursors.WaitCursor;
		List<Type> list = new List<Type>();
		list.Add(typeof(frmRequestAdd));
		Common.closeForm(list);
		new frmRequestAdd(this, (DataTable)dgvMain.DataSource, mId, mSearchMain.dtpMain.Value).Show();
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
		List<Type> list = new List<Type>();
		list.Add(typeof(frmRequestAdd));
		Common.closeForm(list);
		new frmRequestAdd(this, (DataTable)dgvMain.DataSource, mId, mSearchMain.dtpMain.Value, isadd: false).Show();
	}

	private void main_completeToolStripMenuItem_Click(object sender, EventArgs e)
	{
		mSearchMain.btnSearch.Select();
		if (mId.Equals(Guid.Empty))
		{
			MessageBox.Show(Common.getTextLanguage(this, "wSelectRow"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		try
		{
			if (MessageBox.Show(string.Format("{0} {1}", Common.getTextLanguage(this, "wSureComplete"), dgvMain.CurrentRow.Cells["Name"].Value), Common.getTextLanguage(this, "QUESTION"), MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.Yes))
			{
				Cursor.Current = Cursors.WaitCursor;
				isThis = true;
				ActiveRequestDto body = new ActiveRequestDto
				{
					Id = mId,
					Status = "Completed"
				};
				ResponseDto result = frmLogin.client.SaveAsync(body, "/api/Request/Active").Result;
				if (!result.Success)
				{
					throw new Exception(result.Messages.ElementAt(0).Message);
				}
				List<Type> list = new List<Type>();
				list.Add(typeof(frmRequestAdd));
				Common.closeForm(list);
				main_refreshToolStripMenuItem.PerformClick();
			}
		}
		catch (Exception ex)
		{
			isThis = false;
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

	private void main_deleteToolStripMenuItem_Click(object sender, EventArgs e)
	{
		mSearchMain.btnSearch.Select();
		if (mId.Equals(Guid.Empty))
		{
			MessageBox.Show(Common.getTextLanguage(this, "wSelectRow"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
				isThis = true;
				start_Proccessor();
				ResponseDto result = frmLogin.client.DeleteAsync(mId, "/api/Request/Delete/{id}").Result;
				if (!result.Success)
				{
					throw new Exception(result.Messages.ElementAt(0).Message);
				}
				List<Type> list = new List<Type>();
				list.Add(typeof(frmRequestAdd));
				Common.closeForm(list);
				main_refreshToolStripMenuItem.PerformClick();
			}
			catch (Exception ex)
			{
				isThis = false;
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

	private void main_exportToolStripMenuItem_Click(object sender, EventArgs e)
	{
		mSearchMain.btnSearch.Select();
		if (dgvMain.Rows.Count.Equals(0))
		{
			MessageBox.Show(Common.getTextLanguage(this, "wHasntRequest"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		string path = Path.Combine(Directory.GetCurrentDirectory(), "Templates");
		Directory.CreateDirectory(path);
		string[] files = Directory.GetFiles(path, "Daily_Request.xlsx");
		if (files.Length.Equals(0))
		{
			MessageBox.Show(Common.getTextLanguage(this, "wHasntTemplate"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		panelmProgressBarMain.Visible = true;
		mProgressBarMain.Maximum = dgvMain.Rows.Count;
		mProgressBarMain.Value = mProgressBarMain.Maximum;
		Application.DoEvents();
		saveFileDialogExportExcel.Title = Common.getTextLanguage(this, "SaveFile");
		saveFileDialogExportExcel.FileName = "Daily_Request_" + mSearchMain.dtpMain.Value.ToString("yyyyMMdd") + ".xlsx";
		if (saveFileDialogExportExcel.ShowDialog().Equals(DialogResult.OK))
		{
			string sourceFileName = files[0];
			string fileName = saveFileDialogExportExcel.FileName;
			try
			{
				if (Common.FileInUse(saveFileDialogExportExcel.FileName))
				{
					throw new Exception(Common.getTextLanguage(this, "Openning"));
				}
				File.Copy(sourceFileName, fileName, overwrite: true);
				FileInfo newFile = new FileInfo(fileName);
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
					excelRange["C3"].Value = DateTime.Parse(mSearchMain.dtpMain.Value.ToShortDateString());
					excelRange["C3"].Style.Numberformat.Format = "dd-mm-yyyy";
					int num = 0;
					foreach (DataGridViewColumn column in dgvMain.Columns)
					{
						if (column.Visible)
						{
							num++;
							excelRange[5, num].Value = column.HeaderText;
						}
					}
					int num2 = 0;
					foreach (DataGridViewRow item in (IEnumerable)dgvMain.Rows)
					{
						if (!item.Cells["Status"].Value.ToString().Equals("Activated") && !item.Cells["Status"].Value.ToString().Equals("Rejected"))
						{
							continue;
						}
						num2++;
						num = 0;
						foreach (DataGridViewColumn column2 in dgvMain.Columns)
						{
							if (column2.Visible)
							{
								num++;
								if (column2.Name.Contains("Date"))
								{
									excelRange[num2 + 5, num].Value = DateTime.Parse(item.Cells[column2.Name].Value.ToString());
									excelRange[num2 + 5, num].Style.Numberformat.Format = "dd-mm-yyyy";
								}
								else if (column2.Name.Contains("Special"))
								{
									excelRange[num2 + 5, num].Value = (bool.Parse(item.Cells[column2.Name].Value.ToString()) ? "√" : null);
									excelRange[num2 + 5, num].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
								}
								else
								{
									excelRange[num2 + 5, num].Value = item.Cells[column2.Name].Value;
								}
							}
						}
					}
					excelRange = excelWorksheet.Cells[5, 1, 5 + num2, num];
					excelRange.AutoFitColumns(5.0, 100.0);
					excelRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
					excelRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
					excelRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
					excelRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					excelPackage.Save();
				}
				Common.ExecuteBatFile(fileName);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
		panelmProgressBarMain.Visible = false;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_5S_QA_System.frmRequestView));
		this.saveFileDialogExportExcel = new System.Windows.Forms.SaveFileDialog();
		this.contextMenuStripMain = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.main_refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparatorNew = new System.Windows.Forms.ToolStripSeparator();
		this.main_newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.main_editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.main_completeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparatorDelete = new System.Windows.Forms.ToolStripSeparator();
		this.main_deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparatorExport = new System.Windows.Forms.ToolStripSeparator();
		this.main_exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.main_fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.statusStripfrmMain = new System.Windows.Forms.StatusStrip();
		this.sprogbarStatus = new System.Windows.Forms.ToolStripProgressBar();
		this.slblStatus = new System.Windows.Forms.ToolStripStatusLabel();
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.dgvMain = new System.Windows.Forms.DataGridView();
		this.panelmProgressBarMain = new System.Windows.Forms.Panel();
		this.mProgressBarMain = new MetroFramework.Controls.MetroProgressBar();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.openFileDialogMain = new System.Windows.Forms.OpenFileDialog();
		this.panelLogout = new System.Windows.Forms.Panel();
		this.lblFullname = new System.Windows.Forms.Label();
		this.ptbAvatar = new System.Windows.Forms.PictureBox();
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
		this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn15 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn16 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn17 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn18 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn19 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn20 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn21 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn22 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn23 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn24 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn25 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn26 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn27 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn28 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn29 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn30 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn31 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn32 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn33 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn34 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn35 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn36 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn37 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.mPanelViewMain = new _5S_QA_System.View.User_control.mPanelViewExport();
		this.mSearchMain = new _5S_QA_System.View.User_control.mSearch();
		this.No = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.GroupId = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ProductId = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ProductStage = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ProductCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ProductDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ProductDepartment = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
		this.panelmProgressBarMain.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		this.panelLogout.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.ptbAvatar).BeginInit();
		base.SuspendLayout();
		this.saveFileDialogExportExcel.Filter = "File excel (*.xls, *.xlsx)| *.xls; *.xlsx";
		this.saveFileDialogExportExcel.Title = "Select the path to save the file";
		this.contextMenuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[10] { this.main_refreshToolStripMenuItem, this.toolStripSeparatorNew, this.main_newToolStripMenuItem, this.main_editToolStripMenuItem, this.main_completeToolStripMenuItem, this.toolStripSeparatorDelete, this.main_deleteToolStripMenuItem, this.toolStripSeparatorExport, this.main_exportToolStripMenuItem, this.main_fileToolStripMenuItem });
		this.contextMenuStripMain.Name = "contextMenuStripStaff";
		this.contextMenuStripMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
		this.contextMenuStripMain.Size = new System.Drawing.Size(127, 176);
		this.main_refreshToolStripMenuItem.Name = "main_refreshToolStripMenuItem";
		this.main_refreshToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
		this.main_refreshToolStripMenuItem.Text = "Refresh";
		this.main_refreshToolStripMenuItem.Click += new System.EventHandler(main_refreshToolStripMenuItem_Click);
		this.toolStripSeparatorNew.Name = "toolStripSeparatorNew";
		this.toolStripSeparatorNew.Size = new System.Drawing.Size(123, 6);
		this.main_newToolStripMenuItem.Name = "main_newToolStripMenuItem";
		this.main_newToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
		this.main_newToolStripMenuItem.Text = "New";
		this.main_newToolStripMenuItem.Click += new System.EventHandler(main_newToolStripMenuItem_Click);
		this.main_editToolStripMenuItem.Name = "main_editToolStripMenuItem";
		this.main_editToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
		this.main_editToolStripMenuItem.Text = "Edit";
		this.main_editToolStripMenuItem.Visible = false;
		this.main_editToolStripMenuItem.Click += new System.EventHandler(main_editToolStripMenuItem_Click);
		this.main_completeToolStripMenuItem.Name = "main_completeToolStripMenuItem";
		this.main_completeToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
		this.main_completeToolStripMenuItem.Text = "Complete";
		this.main_completeToolStripMenuItem.Visible = false;
		this.main_completeToolStripMenuItem.Click += new System.EventHandler(main_completeToolStripMenuItem_Click);
		this.toolStripSeparatorDelete.Name = "toolStripSeparatorDelete";
		this.toolStripSeparatorDelete.Size = new System.Drawing.Size(123, 6);
		this.toolStripSeparatorDelete.Visible = false;
		this.main_deleteToolStripMenuItem.Name = "main_deleteToolStripMenuItem";
		this.main_deleteToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
		this.main_deleteToolStripMenuItem.Text = "Delete";
		this.main_deleteToolStripMenuItem.Visible = false;
		this.main_deleteToolStripMenuItem.Click += new System.EventHandler(main_deleteToolStripMenuItem_Click);
		this.toolStripSeparatorExport.Name = "toolStripSeparatorExport";
		this.toolStripSeparatorExport.Size = new System.Drawing.Size(123, 6);
		this.toolStripSeparatorExport.Visible = false;
		this.main_exportToolStripMenuItem.Name = "main_exportToolStripMenuItem";
		this.main_exportToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
		this.main_exportToolStripMenuItem.Text = "Export";
		this.main_exportToolStripMenuItem.Visible = false;
		this.main_exportToolStripMenuItem.Click += new System.EventHandler(main_exportToolStripMenuItem_Click);
		this.main_fileToolStripMenuItem.Name = "main_fileToolStripMenuItem";
		this.main_fileToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
		this.main_fileToolStripMenuItem.Text = "Result file";
		this.main_fileToolStripMenuItem.Visible = false;
		this.main_fileToolStripMenuItem.Click += new System.EventHandler(main_fileToolStripMenuItem_Click);
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
		this.statusStripfrmMain.TabIndex = 31;
		this.sprogbarStatus.Name = "sprogbarStatus";
		this.sprogbarStatus.Size = new System.Drawing.Size(200, 20);
		this.slblStatus.Name = "slblStatus";
		this.slblStatus.Size = new System.Drawing.Size(16, 21);
		this.slblStatus.Text = "...";
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
		this.dgvMain.Columns.AddRange(this.No, this.Code, this.name, this.GroupId, this.ProductId, this.ProductStage, this.ProductCode, this.ProductName, this.ProductDescription, this.ProductDepartment, this.ProductImageUrl, this.ProductCavity, this.Date, this.Quantity, this.Lot, this.Line, this.Intention, this.InputDate, this.Supplier, this.Sample, this.Type, this.Status, this.Judgement, this.Link, this.Completed, this.CompletedBy, this.Checked, this.CheckedBy, this.Approved, this.ApprovedBy, this.TotalComment, this.Id, this.Created, this.Modified, this.CreatedBy, this.ModifiedBy, this.IsActivated);
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
		this.dgvMain.TabIndex = 30;
		this.dgvMain.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvMain_CellClick);
		this.dgvMain.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvMain_CellDoubleClick);
		this.dgvMain.CurrentCellChanged += new System.EventHandler(dgvMain_CurrentCellChanged);
		this.dgvMain.Sorted += new System.EventHandler(dgvMain_Sorted);
		this.panelmProgressBarMain.Anchor = System.Windows.Forms.AnchorStyles.None;
		this.panelmProgressBarMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panelmProgressBarMain.Controls.Add(this.mProgressBarMain);
		this.panelmProgressBarMain.Controls.Add(this.pictureBox1);
		this.panelmProgressBarMain.Location = new System.Drawing.Point(489, 237);
		this.panelmProgressBarMain.Name = "panelmProgressBarMain";
		this.panelmProgressBarMain.Size = new System.Drawing.Size(122, 128);
		this.panelmProgressBarMain.TabIndex = 37;
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
		this.openFileDialogMain.FileName = "Request";
		this.openFileDialogMain.Filter = "File excel (*.xls, *.xlsx)| *.xls; *.xlsx";
		this.openFileDialogMain.Title = "Select template excel of request";
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
		this.dataGridViewTextBoxColumn1.DataPropertyName = "No";
		dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle3;
		this.dataGridViewTextBoxColumn1.HeaderText = "No.";
		this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
		this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.dataGridViewTextBoxColumn1.Width = 40;
		this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn2.DataPropertyName = "Code";
		this.dataGridViewTextBoxColumn2.FillWeight = 20f;
		this.dataGridViewTextBoxColumn2.HeaderText = "Code";
		this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
		this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn3.DataPropertyName = "Name";
		this.dataGridViewTextBoxColumn3.FillWeight = 40f;
		this.dataGridViewTextBoxColumn3.HeaderText = "Name";
		this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
		this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn4.DataPropertyName = "ProductId";
		this.dataGridViewTextBoxColumn4.FillWeight = 40f;
		this.dataGridViewTextBoxColumn4.HeaderText = "Product id";
		this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
		this.dataGridViewTextBoxColumn4.Visible = false;
		this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn5.DataPropertyName = "ProductCode";
		this.dataGridViewTextBoxColumn5.FillWeight = 30f;
		this.dataGridViewTextBoxColumn5.HeaderText = "Product code";
		this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
		this.dataGridViewTextBoxColumn5.Visible = false;
		this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn6.DataPropertyName = "ProductName";
		this.dataGridViewTextBoxColumn6.FillWeight = 30f;
		this.dataGridViewTextBoxColumn6.HeaderText = "Product name";
		this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
		this.dataGridViewTextBoxColumn7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn7.DataPropertyName = "ProductImageUrl";
		this.dataGridViewTextBoxColumn7.FillWeight = 30f;
		this.dataGridViewTextBoxColumn7.HeaderText = "Product image";
		this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
		this.dataGridViewTextBoxColumn7.Visible = false;
		this.dataGridViewTextBoxColumn8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn8.DataPropertyName = "ProductCavity";
		this.dataGridViewTextBoxColumn8.FillWeight = 20f;
		this.dataGridViewTextBoxColumn8.HeaderText = "Product cavity";
		this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
		this.dataGridViewTextBoxColumn8.Visible = false;
		this.dataGridViewTextBoxColumn9.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn9.DataPropertyName = "Date";
		this.dataGridViewTextBoxColumn9.FillWeight = 20f;
		this.dataGridViewTextBoxColumn9.HeaderText = "Date";
		this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
		this.dataGridViewTextBoxColumn9.Visible = false;
		this.dataGridViewTextBoxColumn10.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn10.DataPropertyName = "Quantity";
		this.dataGridViewTextBoxColumn10.FillWeight = 15f;
		this.dataGridViewTextBoxColumn10.HeaderText = "Q.ty";
		this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
		this.dataGridViewTextBoxColumn10.Visible = false;
		this.dataGridViewTextBoxColumn11.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn11.DataPropertyName = "Lot";
		this.dataGridViewTextBoxColumn11.FillWeight = 20f;
		this.dataGridViewTextBoxColumn11.HeaderText = "Lot";
		this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
		this.dataGridViewTextBoxColumn11.Visible = false;
		this.dataGridViewTextBoxColumn12.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn12.DataPropertyName = "Customer";
		this.dataGridViewTextBoxColumn12.FillWeight = 20f;
		this.dataGridViewTextBoxColumn12.HeaderText = "Customer";
		this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
		this.dataGridViewTextBoxColumn12.Visible = false;
		this.dataGridViewTextBoxColumn13.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn13.DataPropertyName = "DeliveryDate";
		this.dataGridViewTextBoxColumn13.FillWeight = 15f;
		this.dataGridViewTextBoxColumn13.HeaderText = "Del. date";
		this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
		this.dataGridViewTextBoxColumn13.ReadOnly = true;
		this.dataGridViewTextBoxColumn14.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn14.DataPropertyName = "Detail";
		this.dataGridViewTextBoxColumn14.FillWeight = 20f;
		this.dataGridViewTextBoxColumn14.HeaderText = "Detail";
		this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
		this.dataGridViewTextBoxColumn15.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn15.DataPropertyName = "Sample";
		this.dataGridViewTextBoxColumn15.FillWeight = 20f;
		this.dataGridViewTextBoxColumn15.HeaderText = "Sample";
		this.dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
		this.dataGridViewTextBoxColumn16.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn16.DataPropertyName = "Type";
		this.dataGridViewTextBoxColumn16.FillWeight = 20f;
		this.dataGridViewTextBoxColumn16.HeaderText = "Type";
		this.dataGridViewTextBoxColumn16.Name = "dataGridViewTextBoxColumn16";
		this.dataGridViewTextBoxColumn17.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn17.DataPropertyName = "Status";
		this.dataGridViewTextBoxColumn17.FillWeight = 20f;
		this.dataGridViewTextBoxColumn17.HeaderText = "Status";
		this.dataGridViewTextBoxColumn17.Name = "dataGridViewTextBoxColumn17";
		this.dataGridViewTextBoxColumn18.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn18.DataPropertyName = "Judgement";
		this.dataGridViewTextBoxColumn18.FillWeight = 20f;
		this.dataGridViewTextBoxColumn18.HeaderText = "Judgement";
		this.dataGridViewTextBoxColumn18.Name = "dataGridViewTextBoxColumn18";
		this.dataGridViewTextBoxColumn18.Visible = false;
		this.dataGridViewTextBoxColumn19.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn19.DataPropertyName = "Link";
		this.dataGridViewTextBoxColumn19.FillWeight = 20f;
		this.dataGridViewTextBoxColumn19.HeaderText = "Link";
		this.dataGridViewTextBoxColumn19.Name = "dataGridViewTextBoxColumn19";
		this.dataGridViewTextBoxColumn19.Visible = false;
		this.dataGridViewTextBoxColumn20.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn20.DataPropertyName = "Completed";
		this.dataGridViewTextBoxColumn20.FillWeight = 20f;
		this.dataGridViewTextBoxColumn20.HeaderText = "Prepared";
		this.dataGridViewTextBoxColumn20.Name = "dataGridViewTextBoxColumn20";
		this.dataGridViewTextBoxColumn20.Visible = false;
		this.dataGridViewTextBoxColumn21.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn21.DataPropertyName = "CompletedBy";
		this.dataGridViewTextBoxColumn21.FillWeight = 20f;
		this.dataGridViewTextBoxColumn21.HeaderText = "Prepared by";
		this.dataGridViewTextBoxColumn21.Name = "dataGridViewTextBoxColumn21";
		this.dataGridViewTextBoxColumn21.Visible = false;
		this.dataGridViewTextBoxColumn22.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn22.DataPropertyName = "Checked";
		this.dataGridViewTextBoxColumn22.FillWeight = 20f;
		this.dataGridViewTextBoxColumn22.HeaderText = "Checked";
		this.dataGridViewTextBoxColumn22.Name = "dataGridViewTextBoxColumn22";
		this.dataGridViewTextBoxColumn22.Visible = false;
		this.dataGridViewTextBoxColumn23.DataPropertyName = "CheckedBy";
		this.dataGridViewTextBoxColumn23.HeaderText = "Checked by";
		this.dataGridViewTextBoxColumn23.Name = "dataGridViewTextBoxColumn23";
		this.dataGridViewTextBoxColumn23.Visible = false;
		this.dataGridViewTextBoxColumn24.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn24.DataPropertyName = "Approved";
		this.dataGridViewTextBoxColumn24.FillWeight = 20f;
		this.dataGridViewTextBoxColumn24.HeaderText = "Approved";
		this.dataGridViewTextBoxColumn24.Name = "dataGridViewTextBoxColumn24";
		this.dataGridViewTextBoxColumn24.Visible = false;
		this.dataGridViewTextBoxColumn25.DataPropertyName = "ApprovedBy";
		this.dataGridViewTextBoxColumn25.HeaderText = "Approved by";
		this.dataGridViewTextBoxColumn25.Name = "dataGridViewTextBoxColumn25";
		this.dataGridViewTextBoxColumn25.Visible = false;
		this.dataGridViewTextBoxColumn26.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn26.DataPropertyName = "TotalComment";
		this.dataGridViewTextBoxColumn26.FillWeight = 15f;
		this.dataGridViewTextBoxColumn26.HeaderText = "Comment";
		this.dataGridViewTextBoxColumn26.Name = "dataGridViewTextBoxColumn26";
		this.dataGridViewTextBoxColumn26.Visible = false;
		this.dataGridViewTextBoxColumn27.DataPropertyName = "Id";
		this.dataGridViewTextBoxColumn27.HeaderText = "Id";
		this.dataGridViewTextBoxColumn27.Name = "dataGridViewTextBoxColumn27";
		this.dataGridViewTextBoxColumn27.Visible = false;
		this.dataGridViewTextBoxColumn28.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn28.DataPropertyName = "Created";
		this.dataGridViewTextBoxColumn28.FillWeight = 30f;
		this.dataGridViewTextBoxColumn28.HeaderText = "Created";
		this.dataGridViewTextBoxColumn28.Name = "dataGridViewTextBoxColumn28";
		this.dataGridViewTextBoxColumn28.Visible = false;
		this.dataGridViewTextBoxColumn29.DataPropertyName = "Modified";
		this.dataGridViewTextBoxColumn29.HeaderText = "Modified";
		this.dataGridViewTextBoxColumn29.Name = "dataGridViewTextBoxColumn29";
		this.dataGridViewTextBoxColumn29.Visible = false;
		this.dataGridViewTextBoxColumn30.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn30.DataPropertyName = "CreatedBy";
		this.dataGridViewTextBoxColumn30.FillWeight = 30f;
		this.dataGridViewTextBoxColumn30.HeaderText = "Created by";
		this.dataGridViewTextBoxColumn30.Name = "dataGridViewTextBoxColumn30";
		this.dataGridViewTextBoxColumn30.Visible = false;
		this.dataGridViewTextBoxColumn31.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn31.DataPropertyName = "modifiedby";
		this.dataGridViewTextBoxColumn31.FillWeight = 15f;
		this.dataGridViewTextBoxColumn31.HeaderText = "Modified by";
		this.dataGridViewTextBoxColumn31.Name = "dataGridViewTextBoxColumn31";
		this.dataGridViewTextBoxColumn31.Visible = false;
		this.dataGridViewTextBoxColumn32.DataPropertyName = "IsActivated";
		this.dataGridViewTextBoxColumn32.HeaderText = "Is activated";
		this.dataGridViewTextBoxColumn32.Name = "dataGridViewTextBoxColumn32";
		this.dataGridViewTextBoxColumn32.Visible = false;
		this.dataGridViewTextBoxColumn33.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn33.DataPropertyName = "modifiedby";
		this.dataGridViewTextBoxColumn33.FillWeight = 30f;
		this.dataGridViewTextBoxColumn33.HeaderText = "Modified by";
		this.dataGridViewTextBoxColumn33.Name = "dataGridViewTextBoxColumn33";
		this.dataGridViewTextBoxColumn33.Visible = false;
		this.dataGridViewTextBoxColumn34.DataPropertyName = "IsActivated";
		this.dataGridViewTextBoxColumn34.HeaderText = "Is activated";
		this.dataGridViewTextBoxColumn34.Name = "dataGridViewTextBoxColumn34";
		this.dataGridViewTextBoxColumn34.Visible = false;
		this.dataGridViewTextBoxColumn35.DataPropertyName = "CreatedBy";
		this.dataGridViewTextBoxColumn35.HeaderText = "Created by";
		this.dataGridViewTextBoxColumn35.Name = "dataGridViewTextBoxColumn35";
		this.dataGridViewTextBoxColumn35.Visible = false;
		this.dataGridViewTextBoxColumn36.DataPropertyName = "modifiedby";
		this.dataGridViewTextBoxColumn36.HeaderText = "Modified by";
		this.dataGridViewTextBoxColumn36.Name = "dataGridViewTextBoxColumn36";
		this.dataGridViewTextBoxColumn36.Visible = false;
		this.dataGridViewTextBoxColumn37.DataPropertyName = "IsActivated";
		this.dataGridViewTextBoxColumn37.HeaderText = "Is activated";
		this.dataGridViewTextBoxColumn37.Name = "dataGridViewTextBoxColumn37";
		this.dataGridViewTextBoxColumn37.Visible = false;
		this.mPanelViewMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.mPanelViewMain.Dock = System.Windows.Forms.DockStyle.Right;
		this.mPanelViewMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.mPanelViewMain.Location = new System.Drawing.Point(430, 134);
		this.mPanelViewMain.Margin = new System.Windows.Forms.Padding(4);
		this.mPanelViewMain.Name = "mPanelViewMain";
		this.mPanelViewMain.Size = new System.Drawing.Size(650, 420);
		this.mPanelViewMain.TabIndex = 38;
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
		this.mSearchMain.TabIndex = 34;
		this.No.DataPropertyName = "No";
		dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		this.No.DefaultCellStyle = dataGridViewCellStyle4;
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
		this.name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.name.DataPropertyName = "Name";
		this.name.FillWeight = 30f;
		this.name.HeaderText = "Name";
		this.name.Name = "name";
		this.name.ReadOnly = true;
		this.GroupId.DataPropertyName = "GroupId";
		this.GroupId.HeaderText = "Group id";
		this.GroupId.Name = "GroupId";
		this.GroupId.ReadOnly = true;
		this.GroupId.Visible = false;
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
		this.ProductCode.FillWeight = 30f;
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
		this.ProductDescription.DataPropertyName = "ProductDescription";
		this.ProductDescription.HeaderText = "Product description";
		this.ProductDescription.Name = "ProductDescription";
		this.ProductDescription.ReadOnly = true;
		this.ProductDescription.Visible = false;
		this.ProductDepartment.DataPropertyName = "ProductDepartment";
		this.ProductDepartment.HeaderText = "Product department";
		this.ProductDepartment.Name = "ProductDepartment";
		this.ProductDepartment.ReadOnly = true;
		this.ProductDepartment.Visible = false;
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
		this.Intention.Visible = false;
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
		this.Type.FillWeight = 20f;
		this.Type.HeaderText = "Type";
		this.Type.Name = "Type";
		this.Type.ReadOnly = true;
		this.Status.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Status.DataPropertyName = "Status";
		this.Status.FillWeight = 20f;
		this.Status.HeaderText = "Status";
		this.Status.Name = "Status";
		this.Status.ReadOnly = true;
		this.Judgement.DataPropertyName = "Judgement";
		this.Judgement.HeaderText = "Judgement";
		this.Judgement.Name = "Judgement";
		this.Judgement.ReadOnly = true;
		this.Judgement.Visible = false;
		this.Link.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Link.DataPropertyName = "Link";
		this.Link.FillWeight = 20f;
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
		base.Controls.Add(this.panelmProgressBarMain);
		base.Controls.Add(this.dgvMain);
		base.Controls.Add(this.mSearchMain);
		base.Controls.Add(this.statusStripfrmMain);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "frmRequestView";
		base.Padding = new System.Windows.Forms.Padding(20, 70, 20, 20);
		this.Text = "5S QA System * REQUEST";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmRequestView_FormClosing);
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmRequestView_FormClosed);
		base.Load += new System.EventHandler(frmRequestView_Load);
		base.Shown += new System.EventHandler(frmRequestView_Shown);
		this.contextMenuStripMain.ResumeLayout(false);
		this.statusStripfrmMain.ResumeLayout(false);
		this.statusStripfrmMain.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvMain).EndInit();
		this.panelmProgressBarMain.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		this.panelLogout.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.ptbAvatar).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
