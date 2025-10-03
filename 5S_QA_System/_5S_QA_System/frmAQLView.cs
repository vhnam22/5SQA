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
using MetroFramework.Controls;
using MetroFramework.Forms;
using OfficeOpenXml;

namespace _5S_QA_System;

public class frmAQLView : MetroForm
{
	private readonly Form mForm;

	public Guid mIdParent = Guid.Empty;

	private Guid mId;

	private Guid IdFrom;

	private Guid IdTo;

	private bool isEdit;

	private int mRow;

	private int mCol;

	public bool isClose;

	private IContainer components = null;

	private ToolStripProgressBar sprogbarStatus;

	public ToolStripStatusLabel slblStatus;

	private ToolTip toolTipMain;

	private ToolStripMenuItem main_deleteToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator6;

	private ToolStripMenuItem main_newToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripMenuItem main_refreshToolStripMenuItem;

	private ContextMenuStrip contextMenuStripMain;

	private StatusStrip statusStripfrmMain;

	private DataGridView dgvMain;

	private ToolStripMenuItem main_editToolStripMenuItem;

	private mPanelAQL mPanelViewMain;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripMenuItem main_moveToolStripMenuItem;

	private ToolStripMenuItem main_importToolStripMenuItem;

	private Panel panelmProgressBarMain;

	private MetroProgressBar mProgressBarMain;

	private PictureBox pictureBox1;

	private OpenFileDialog openFileDialogMain;

	private DataGridViewCheckBoxColumn IsSelect;

	private DataGridViewTextBoxColumn No;

	private DataGridViewTextBoxColumn ProductId;

	private DataGridViewTextBoxColumn Type;

	private DataGridViewTextBoxColumn InputQuantity;

	private DataGridViewTextBoxColumn Sample;

	private DataGridViewTextBoxColumn Id;

	private new DataGridViewTextBoxColumn Created;

	private DataGridViewTextBoxColumn Modified;

	private DataGridViewTextBoxColumn CreatedBy;

	private DataGridViewTextBoxColumn ModifiedBy;

	private DataGridViewTextBoxColumn IsActivated;

	private ToolStripSeparator toolStripSeparatorSelect;

	private ToolStripMenuItem enall_pageToolStripMenuItem;

	private ToolStripMenuItem unall_pageToolStripMenuItem;

	public frmAQLView(Form frm, Guid id = default(Guid))
	{
		InitializeComponent();
		Common.setControls(this, toolTipMain, new List<ContextMenuStrip> { contextMenuStripMain });
		mForm = frm;
		mIdParent = id;
		Init_Title();
	}

	private void frmAQLView_Load(object sender, EventArgs e)
	{
		Init();
	}

	private void frmAQLView_Shown(object sender, EventArgs e)
	{
		load_AllData();
	}

	private void frmAQLView_FormClosing(object sender, FormClosingEventArgs e)
	{
		GC.Collect();
	}

	private void frmAQLView_FormClosed(object sender, FormClosedEventArgs e)
	{
		if (!isClose)
		{
			((frmProductView)mForm).isClose = false;
			((frmProductView)mForm).load_AllData();
		}
	}

	private void Init()
	{
		panelmProgressBarMain.BringToFront();
		panelmProgressBarMain.Visible = false;
		mPanelViewMain.Visible = false;
		isClose = true;
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
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "ProductId=@0";
			queryArgs.PredicateParameters = new string[1] { mIdParent.ToString() };
			queryArgs.Order = "Sort, Created";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs body = queryArgs;
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/AQL/Gets").Result;
			dgvMain.DataSource = Common.getDataTableIsSelect<AQLViewModel>(result, select: false);
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
			SetIsSelect(en: true);
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
		finally
		{
			isEdit = false;
		}
	}

	public void Init_Title()
	{
		DataGridView dataGridView = mForm.Controls["dgvMain"] as DataGridView;
		Text = string.Format("{0} ({1}#{2})", Text, dataGridView.CurrentRow.Cells["Code"].Value, dataGridView.CurrentRow.Cells["Name"].Value);
	}

	private void en_disControl(bool enable)
	{
		main_importToolStripMenuItem.Enabled = true;
		main_newToolStripMenuItem.Enabled = enable;
		main_editToolStripMenuItem.Enabled = enable;
		main_deleteToolStripMenuItem.Enabled = enable;
		main_moveToolStripMenuItem.Enabled = enable;
		enall_pageToolStripMenuItem.Enabled = enable;
		unall_pageToolStripMenuItem.Enabled = enable;
		if (enable)
		{
			main_importToolStripMenuItem.Text = Common.getTextLanguage(this, "Import");
		}
		else
		{
			main_importToolStripMenuItem.Text = Common.getTextLanguage(this, "Confirm");
		}
	}

	private void disControlMove()
	{
		main_newToolStripMenuItem.Enabled = false;
		main_editToolStripMenuItem.Enabled = false;
		main_deleteToolStripMenuItem.Enabled = false;
		main_importToolStripMenuItem.Enabled = false;
		enall_pageToolStripMenuItem.Enabled = false;
		unall_pageToolStripMenuItem.Enabled = false;
	}

	private void updateMove()
	{
		Cursor.Current = Cursors.WaitCursor;
		try
		{
			start_Proccessor();
			ResponseDto result = frmLogin.client.MoveAsync(IdFrom, IdTo, "/api/AQL/Move/{idfrom}/{idto}").Result;
			IdFrom = Guid.Empty;
			IdTo = Guid.Empty;
			if (!result.Success)
			{
				throw new Exception(result.Messages.ElementAt(0).Message);
			}
			isClose = false;
			main_refreshToolStripMenuItem.PerformClick();
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

	private void SetIsSelect(bool en)
	{
		enall_pageToolStripMenuItem.Visible = en;
		unall_pageToolStripMenuItem.Visible = en;
		toolStripSeparatorSelect.Visible = en;
		dgvMain.Columns["IsSelect"].Visible = en;
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
			mId = Guid.Parse(dataGridView.CurrentRow.Cells["Id"].Value.ToString());
			mPanelViewMain.load_dgvContent(FormType.VIEW);
			if (!IdFrom.Equals(Guid.Empty))
			{
				IdTo = mId;
			}
		}
		else
		{
			mId = Guid.Empty;
			IdFrom = Guid.Empty;
			IdTo = Guid.Empty;
			dgvMain.Cursor = Cursors.Default;
		}
	}

	private void dgvMain_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		mPanelViewMain.Visible = false;
		if (!IdTo.Equals(Guid.Empty) && !IdFrom.Equals(Guid.Empty) && !IdTo.Equals(IdFrom))
		{
			updateMove();
		}
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
		foreach (DataGridViewRow item in (IEnumerable)dataGridView.Rows)
		{
			item.Cells["No"].Value = item.Index + 1;
			if (main_importToolStripMenuItem.Text.Equals(Common.getTextLanguage(this, "Confirm")))
			{
				item.DefaultCellStyle.ForeColor = Color.Green;
			}
		}
	}

	private void main_refreshToolStripMenuItem_Click(object sender, EventArgs e)
	{
		load_AllData();
	}

	private void main_newToolStripMenuItem_Click(object sender, EventArgs e)
	{
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
		List<Guid> list = new List<Guid>();
		Cursor.Current = Cursors.WaitCursor;
		DataTable dataTable = dgvMain.DataSource as DataTable;
		string text = "";
		foreach (DataRow row in dataTable.Rows)
		{
			if (bool.Parse(row["IsSelect"].ToString()))
			{
				list.Add(new Guid(row["Id"].ToString()));
				text += string.Format("\r\n     {0}#{1}#{2}", row["Type"], row["InputQuantity"], row["Sample"]);
			}
		}
		if (list.Count > 1)
		{
			if (!MessageBox.Show(Common.getTextLanguage(this, "wSureDelete") + " " + text.TrimEnd('\n'), Common.getTextLanguage(this, "QUESTION"), MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.Yes))
			{
				return;
			}
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				isClose = true;
				ResponseDto result = frmLogin.client.SaveAsync(list, "/api/AQL/DeleteList").Result;
				if (!result.Success)
				{
					throw new Exception(result.Messages.ElementAt(0).Message);
				}
				main_refreshToolStripMenuItem.PerformClick();
				return;
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
				return;
			}
		}
		if (!MessageBox.Show(string.Format("{0} {1}#{2}", Common.getTextLanguage(this, "wSureDelete"), dgvMain.CurrentRow.Cells["Type"].Value, dgvMain.CurrentRow.Cells["InputQuantity"].Value) + string.Format("#{0}", dgvMain.CurrentRow.Cells["Sample"].Value), Common.getTextLanguage(this, "QUESTION"), MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.Yes))
		{
			return;
		}
		try
		{
			Cursor.Current = Cursors.WaitCursor;
			start_Proccessor();
			ResponseDto result2 = frmLogin.client.DeleteAsync(mId, "/api/AQL/Delete/{id}").Result;
			if (!result2.Success)
			{
				throw new Exception(result2.Messages.ElementAt(0).Message);
			}
			isClose = false;
			main_refreshToolStripMenuItem.PerformClick();
		}
		catch (Exception ex3)
		{
			string textLanguage2 = Common.getTextLanguage("Error", ex3.Message);
			if (ex3.InnerException is ApiException { StatusCode: var statusCode2 })
			{
				if (statusCode2.Equals(401))
				{
					if (MessageBox.Show(Common.getTextLanguage(this, "LoginAgain"), Common.getTextLanguage(this, "INFORMATION"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk).Equals(DialogResult.OK))
					{
						Close();
					}
				}
				else
				{
					debugOutput("ERR: " + (string.IsNullOrEmpty(textLanguage2) ? ex3.Message.Replace("\n", "") : textLanguage2));
					MessageBox.Show(string.IsNullOrEmpty(textLanguage2) ? ex3.Message : textLanguage2, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			else
			{
				debugOutput("ERR: " + (string.IsNullOrEmpty(textLanguage2) ? ex3.Message.Replace("\n", "") : textLanguage2));
				MessageBox.Show(string.IsNullOrEmpty(textLanguage2) ? ex3.Message : textLanguage2, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
	}

	private void main_moveToolStripMenuItem_Click(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		if (mIdParent.Equals(Guid.Empty) || mId.Equals(Guid.Empty))
		{
			MessageBox.Show(Common.getTextLanguage(this, "wSelectRow"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		if (frmLogin.User.Role.Equals(RoleWeb.Administrator) && frmLogin.User.JobTitle != "Manager" && frmLogin.User.JobTitle != "Supervisor")
		{
			MessageBox.Show(Common.getTextLanguage(typeof(frmLogin).Name, "wNoAuthorization"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		IdFrom = mId;
		dgvMain.Cursor = Cursors.NoMoveVert;
		disControlMove();
	}

	private void main_importToolStripMenuItem_Click(object sender, EventArgs e)
	{
		//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f0: Expected O, but got Unknown
		//IL_05f3: Expected O, but got Unknown
		if (mIdParent.Equals(Guid.Empty))
		{
			MessageBox.Show(Common.getTextLanguage(this, "wProductHasnt"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		if (frmLogin.User.Role.Equals(RoleWeb.Administrator) && frmLogin.User.JobTitle != "Manager" && frmLogin.User.JobTitle != "Supervisor")
		{
			MessageBox.Show(Common.getTextLanguage(typeof(frmLogin).Name, "wNoAuthorization"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		panelmProgressBarMain.Visible = true;
		mProgressBarMain.Value = mProgressBarMain.Maximum;
		if (main_importToolStripMenuItem.Text.Equals(Common.getTextLanguage(this, "Import")))
		{
			string[] aQLHeaders = MetaType.AQLHeaders;
			MessageBox.Show(Common.getTextLanguage(this, "FormatHeader") + "\r\n    " + string.Join(" - ", aQLHeaders) + "\r\n" + Common.getTextLanguage(this, "NextRow"), Common.getTextLanguage(this, "INFORMATION"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
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
						string text = Common.checkFormat(aQLHeaders, excelRange);
						if (!text.Equals("Ok"))
						{
							throw new Exception(text);
						}
						mProgressBarMain.Maximum = excelWorksheet.Dimension.Rows;
						for (int i = 1; i < excelWorksheet.Dimension.Rows; i++)
						{
							mProgressBarMain.Value = i;
							DataRow dataRow = dataTable.NewRow();
							for (int j = 0; j < aQLHeaders.Length; j++)
							{
								string text2 = Common.trimSpace(excelRange[1, j + 1].Value.ToString()).ToLower();
								if (text2.Equals(aQLHeaders[j].ToLower()))
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
					dgvMain.DataSource = dataTable;
					dgvMain.Refresh();
					dgvMain_Sorted(dgvMain, null);
					SetIsSelect(en: false);
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
			DataTable dataTable2 = dgvMain.DataSource as DataTable;
			dataTable2.Dispose();
			mProgressBarMain.Maximum = dataTable2.Rows.Count;
			try
			{
				List<AQLViewModel> list = new List<AQLViewModel>();
				foreach (DataRow row in dataTable2.Rows)
				{
					int value = dataTable2.Rows.IndexOf(row) + 1;
					mProgressBarMain.Value = value;
					AQLViewModel val = new AQLViewModel
					{
						Type = row["Type"].ToString(),
						ProductId = mIdParent
					};
					((AuditableEntity)val).IsActivated = true;
					AQLViewModel val2 = val;
					if (int.TryParse(row["InputQuantity"].ToString(), out var result))
					{
						val2.InputQuantity = result;
					}
					if (int.TryParse(row["Sample"].ToString(), out var result2))
					{
						val2.Sample = result2;
					}
					list.Add(val2);
				}
				ResponseDto result3 = frmLogin.client.SaveAsync(list, "/api/AQL/SaveList").Result;
				if (!result3.Success)
				{
					string text3 = string.Empty;
					foreach (ResponseMessage message in result3.Messages)
					{
						text3 = text3 + message.Message + "\r\n";
					}
					throw new Exception(text3);
				}
				isClose = false;
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

	private void enall_pageToolStripMenuItem_Click(object sender, EventArgs e)
	{
		DataTable dataTable = dgvMain.DataSource as DataTable;
		foreach (DataRow row in dataTable.Rows)
		{
			row["IsSelect"] = true;
		}
		dgvMain.Refresh();
	}

	private void unall_pageToolStripMenuItem_Click(object sender, EventArgs e)
	{
		DataTable dataTable = dgvMain.DataSource as DataTable;
		foreach (DataRow row in dataTable.Rows)
		{
			row["IsSelect"] = false;
		}
		dgvMain.Refresh();
	}

	private void dgvMain_CurrentCellDirtyStateChanged(object sender, EventArgs e)
	{
		DataGridView dataGridView = sender as DataGridView;
		dataGridView.CurrentCellDirtyStateChanged -= dgvMain_CurrentCellDirtyStateChanged;
		if (dataGridView.CurrentCell is DataGridViewCheckBoxCell dataGridViewCheckBoxCell)
		{
			dataGridView.CurrentRow.Cells["IsSelect"].Value = !(bool)dataGridViewCheckBoxCell.Value;
		}
		dataGridView.EndEdit();
		dataGridView.CurrentCellDirtyStateChanged += dgvMain_CurrentCellDirtyStateChanged;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_5S_QA_System.frmAQLView));
		this.sprogbarStatus = new System.Windows.Forms.ToolStripProgressBar();
		this.slblStatus = new System.Windows.Forms.ToolStripStatusLabel();
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.main_deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.main_newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.main_refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.contextMenuStripMain = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.main_editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.main_importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.main_moveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparatorSelect = new System.Windows.Forms.ToolStripSeparator();
		this.enall_pageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.unall_pageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.statusStripfrmMain = new System.Windows.Forms.StatusStrip();
		this.dgvMain = new System.Windows.Forms.DataGridView();
		this.IsSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
		this.No = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ProductId = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.InputQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Sample = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Created = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Modified = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.CreatedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ModifiedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.IsActivated = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.panelmProgressBarMain = new System.Windows.Forms.Panel();
		this.mProgressBarMain = new MetroFramework.Controls.MetroProgressBar();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.openFileDialogMain = new System.Windows.Forms.OpenFileDialog();
		this.mPanelViewMain = new _5S_QA_System.mPanelAQL();
		this.contextMenuStripMain.SuspendLayout();
		this.statusStripfrmMain.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvMain).BeginInit();
		this.panelmProgressBarMain.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		base.SuspendLayout();
		this.sprogbarStatus.Name = "sprogbarStatus";
		this.sprogbarStatus.Size = new System.Drawing.Size(100, 20);
		this.slblStatus.Name = "slblStatus";
		this.slblStatus.Size = new System.Drawing.Size(16, 21);
		this.slblStatus.Text = "...";
		this.main_deleteToolStripMenuItem.Name = "main_deleteToolStripMenuItem";
		this.main_deleteToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
		this.main_deleteToolStripMenuItem.Text = "Delete";
		this.main_deleteToolStripMenuItem.Click += new System.EventHandler(main_deleteToolStripMenuItem_Click);
		this.toolStripSeparator6.Name = "toolStripSeparator6";
		this.toolStripSeparator6.Size = new System.Drawing.Size(166, 6);
		this.main_newToolStripMenuItem.Name = "main_newToolStripMenuItem";
		this.main_newToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
		this.main_newToolStripMenuItem.Text = "New";
		this.main_newToolStripMenuItem.Click += new System.EventHandler(main_newToolStripMenuItem_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(166, 6);
		this.main_refreshToolStripMenuItem.Name = "main_refreshToolStripMenuItem";
		this.main_refreshToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
		this.main_refreshToolStripMenuItem.Text = "Refresh";
		this.main_refreshToolStripMenuItem.Click += new System.EventHandler(main_refreshToolStripMenuItem_Click);
		this.contextMenuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[12]
		{
			this.main_refreshToolStripMenuItem, this.toolStripSeparator1, this.main_newToolStripMenuItem, this.main_editToolStripMenuItem, this.toolStripSeparator6, this.main_deleteToolStripMenuItem, this.toolStripSeparator2, this.main_importToolStripMenuItem, this.main_moveToolStripMenuItem, this.toolStripSeparatorSelect,
			this.enall_pageToolStripMenuItem, this.unall_pageToolStripMenuItem
		});
		this.contextMenuStripMain.Name = "contextMenuStripStaff";
		this.contextMenuStripMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
		this.contextMenuStripMain.Size = new System.Drawing.Size(170, 204);
		this.main_editToolStripMenuItem.Name = "main_editToolStripMenuItem";
		this.main_editToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
		this.main_editToolStripMenuItem.Text = "Edit";
		this.main_editToolStripMenuItem.Click += new System.EventHandler(main_editToolStripMenuItem_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(166, 6);
		this.main_importToolStripMenuItem.Name = "main_importToolStripMenuItem";
		this.main_importToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
		this.main_importToolStripMenuItem.Text = "Import from excel";
		this.main_importToolStripMenuItem.Click += new System.EventHandler(main_importToolStripMenuItem_Click);
		this.main_moveToolStripMenuItem.Name = "main_moveToolStripMenuItem";
		this.main_moveToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
		this.main_moveToolStripMenuItem.Text = "Move";
		this.main_moveToolStripMenuItem.Click += new System.EventHandler(main_moveToolStripMenuItem_Click);
		this.toolStripSeparatorSelect.Name = "toolStripSeparatorSelect";
		this.toolStripSeparatorSelect.Size = new System.Drawing.Size(166, 6);
		this.enall_pageToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ControlText;
		this.enall_pageToolStripMenuItem.Name = "enall_pageToolStripMenuItem";
		this.enall_pageToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
		this.enall_pageToolStripMenuItem.Text = "Enable all";
		this.enall_pageToolStripMenuItem.Click += new System.EventHandler(enall_pageToolStripMenuItem_Click);
		this.unall_pageToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ControlText;
		this.unall_pageToolStripMenuItem.Name = "unall_pageToolStripMenuItem";
		this.unall_pageToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
		this.unall_pageToolStripMenuItem.Text = "Unable all";
		this.unall_pageToolStripMenuItem.Click += new System.EventHandler(unall_pageToolStripMenuItem_Click);
		this.statusStripfrmMain.BackColor = System.Drawing.SystemColors.ControlLight;
		this.statusStripfrmMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.statusStripfrmMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.sprogbarStatus, this.slblStatus });
		this.statusStripfrmMain.Location = new System.Drawing.Point(20, 454);
		this.statusStripfrmMain.Name = "statusStripfrmMain";
		this.statusStripfrmMain.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
		this.statusStripfrmMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
		this.statusStripfrmMain.Size = new System.Drawing.Size(760, 26);
		this.statusStripfrmMain.SizingGrip = false;
		this.statusStripfrmMain.Stretch = false;
		this.statusStripfrmMain.TabIndex = 12;
		this.dgvMain.AllowUserToAddRows = false;
		this.dgvMain.AllowUserToDeleteRows = false;
		this.dgvMain.AllowUserToOrderColumns = true;
		this.dgvMain.AllowUserToResizeRows = false;
		dataGridViewCellStyle.BackColor = System.Drawing.SystemColors.Control;
		this.dgvMain.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle;
		this.dgvMain.BackgroundColor = System.Drawing.SystemColors.Window;
		dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.AppWorkspace;
		dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f);
		dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
		dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
		dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
		dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.dgvMain.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
		this.dgvMain.ColumnHeadersHeight = 26;
		this.dgvMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
		this.dgvMain.Columns.AddRange(this.IsSelect, this.No, this.ProductId, this.Type, this.InputQuantity, this.Sample, this.Id, this.Created, this.Modified, this.CreatedBy, this.ModifiedBy, this.IsActivated);
		this.dgvMain.ContextMenuStrip = this.contextMenuStripMain;
		this.dgvMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.dgvMain.EnableHeadersVisualStyles = false;
		this.dgvMain.Location = new System.Drawing.Point(20, 70);
		this.dgvMain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.dgvMain.MultiSelect = false;
		this.dgvMain.Name = "dgvMain";
		this.dgvMain.RowHeadersWidth = 25;
		this.dgvMain.Size = new System.Drawing.Size(760, 384);
		this.dgvMain.TabIndex = 11;
		this.dgvMain.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvMain_CellClick);
		this.dgvMain.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvMain_CellDoubleClick);
		this.dgvMain.CurrentCellChanged += new System.EventHandler(dgvMain_CurrentCellChanged);
		this.dgvMain.CurrentCellDirtyStateChanged += new System.EventHandler(dgvMain_CurrentCellDirtyStateChanged);
		this.dgvMain.Sorted += new System.EventHandler(dgvMain_Sorted);
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
		this.No.Width = 40;
		this.ProductId.DataPropertyName = "ProductId";
		this.ProductId.HeaderText = "Product id";
		this.ProductId.Name = "ProductId";
		this.ProductId.ReadOnly = true;
		this.ProductId.Visible = false;
		this.Type.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Type.DataPropertyName = "Type";
		this.Type.FillWeight = 40f;
		this.Type.HeaderText = "Type";
		this.Type.Name = "Type";
		this.Type.ReadOnly = true;
		this.InputQuantity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.InputQuantity.DataPropertyName = "InputQuantity";
		this.InputQuantity.FillWeight = 40f;
		this.InputQuantity.HeaderText = "Input q.ty";
		this.InputQuantity.Name = "InputQuantity";
		this.InputQuantity.ReadOnly = true;
		this.Sample.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Sample.DataPropertyName = "Sample";
		this.Sample.FillWeight = 40f;
		this.Sample.HeaderText = "Sample";
		this.Sample.Name = "Sample";
		this.Sample.ReadOnly = true;
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
		this.panelmProgressBarMain.Anchor = System.Windows.Forms.AnchorStyles.None;
		this.panelmProgressBarMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panelmProgressBarMain.Controls.Add(this.mProgressBarMain);
		this.panelmProgressBarMain.Controls.Add(this.pictureBox1);
		this.panelmProgressBarMain.Location = new System.Drawing.Point(339, 186);
		this.panelmProgressBarMain.Name = "panelmProgressBarMain";
		this.panelmProgressBarMain.Size = new System.Drawing.Size(122, 128);
		this.panelmProgressBarMain.TabIndex = 32;
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
		this.openFileDialogMain.FileName = "AQL";
		this.openFileDialogMain.Filter = "File excel (*.xls, *.xlsx)| *.xls; *.xlsx";
		this.openFileDialogMain.Title = "Select template excel AQL for product";
		this.mPanelViewMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.mPanelViewMain.Dock = System.Windows.Forms.DockStyle.Right;
		this.mPanelViewMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.mPanelViewMain.Location = new System.Drawing.Point(480, 70);
		this.mPanelViewMain.Name = "mPanelViewMain";
		this.mPanelViewMain.Size = new System.Drawing.Size(300, 384);
		this.mPanelViewMain.TabIndex = 13;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(800, 500);
		base.Controls.Add(this.panelmProgressBarMain);
		base.Controls.Add(this.mPanelViewMain);
		base.Controls.Add(this.dgvMain);
		base.Controls.Add(this.statusStripfrmMain);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "frmAQLView";
		base.Padding = new System.Windows.Forms.Padding(20, 70, 20, 20);
		this.Text = "5S QA System * MASTER SAMPLING DATA";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmAQLView_FormClosing);
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmAQLView_FormClosed);
		base.Load += new System.EventHandler(frmAQLView_Load);
		base.Shown += new System.EventHandler(frmAQLView_Shown);
		this.contextMenuStripMain.ResumeLayout(false);
		this.statusStripfrmMain.ResumeLayout(false);
		this.statusStripfrmMain.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvMain).EndInit();
		this.panelmProgressBarMain.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
