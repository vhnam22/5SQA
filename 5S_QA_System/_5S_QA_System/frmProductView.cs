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
using _5S_QA_Entities.Abstracts;
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

public class frmProductView : MetroForm
{
	private Guid mId;

	private Guid mIdCopy;

	private Guid IdFrom;

	private Guid IdTo;

	private bool isEdit;

	private int mRow;

	private int mCol;

	private readonly Form mForm;

	private readonly ProductGroupViewModel mGroup;

	public bool isClose;

	private IContainer components = null;

	private ContextMenuStrip contextMenuStripMain;

	private ToolStripMenuItem main_refreshToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripMenuItem main_newToolStripMenuItem;

	private ToolStripMenuItem main_editToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator6;

	private ToolStripMenuItem main_deleteToolStripMenuItem;

	private StatusStrip statusStripfrmMain;

	private ToolStripProgressBar sprogbarStatus;

	public ToolStripStatusLabel slblStatus;

	private ToolTip toolTipMain;

	private DataGridView dgvMain;

	private mSearch mSearchMain;

	private mPanelView mPanelViewMain;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripMenuItem main_importToolStripMenuItem;

	private OpenFileDialog openFileDialogMain;

	private ToolStripMenuItem main_viewToolStripMenuItem;

	private Panel panelmProgressBarMain;

	private MetroProgressBar mProgressBarMain;

	private PictureBox pictureBox1;

	private ToolStripMenuItem main_planToolStripMenuItem;

	private ToolStripMenuItem main_autoToolStripMenuItem;

	private Panel panelLogout;

	private Label lblFullname;

	private PictureBox ptbAvatar;

	private ToolStripMenuItem main_similarToolStripMenuItem;

	private ToolStripMenuItem main_templateotherToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripMenuItem main_copyToolStripMenuItem;

	private ToolStripMenuItem main_aqlToolStripMenuItem;

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

	private ToolStripMenuItem main_moveToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator4;

	private DataGridViewTextBoxColumn No;

	private DataGridViewTextBoxColumn Code;

	private DataGridViewTextBoxColumn name;

	private DataGridViewTextBoxColumn Description;

	private DataGridViewTextBoxColumn ImageUrl;

	private DataGridViewTextBoxColumn Cavity;

	private DataGridViewTextBoxColumn SampleMax;

	private DataGridViewTextBoxColumn TemplateId;

	private DataGridViewTextBoxColumn TemplateName;

	private DataGridViewTextBoxColumn DepartmentId;

	private DataGridViewTextBoxColumn DepartmentName;

	private DataGridViewTextBoxColumn TotalMeas;

	private DataGridViewCheckBoxColumn IsAQL;

	private DataGridViewTextBoxColumn GroupId;

	private DataGridViewTextBoxColumn GroupCodeName;

	private DataGridViewTextBoxColumn Id;

	private new DataGridViewTextBoxColumn Created;

	private DataGridViewTextBoxColumn Modified;

	private DataGridViewTextBoxColumn CreatedBy;

	private DataGridViewTextBoxColumn ModifiedBy;

	private DataGridViewTextBoxColumn IsActivated;

	public frmProductView(Form frm, ProductGroupViewModel group)
	{
		InitializeComponent();
		Common.setControls(this, toolTipMain, new List<ContextMenuStrip> { contextMenuStripMain });
		mForm = frm;
		mGroup = group;
		isClose = true;
		mId = Guid.Empty;
		isEdit = false;
		mRow = 0;
		mCol = 0;
		Text = Text + " (" + mGroup.Code + "#" + mGroup.Name + ")";
	}

	private void frmProductView_Load(object sender, EventArgs e)
	{
		Init();
	}

	private void frmProductView_Shown(object sender, EventArgs e)
	{
		load_AllData();
	}

	private void frmProductView_FormClosing(object sender, FormClosingEventArgs e)
	{
		GC.Collect();
	}

	private void frmProductView_FormClosed(object sender, FormClosedEventArgs e)
	{
		mPanelViewMain.mDispose();
		List<Type> list = new List<Type>();
		list.Add(typeof(frmProductAdd));
		list.Add(typeof(frmMeasurementView));
		list.Add(typeof(frmAQLView));
		list.Add(typeof(frmPlanView));
		list.Add(typeof(frmCreateFile));
		list.Add(typeof(frmSimilarView));
		list.Add(typeof(frmTemplateOtherView));
		Common.closeForm(list);
		if (!isClose)
		{
			((frmProductGroupView)mForm).load_AllData();
		}
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
		mPanelViewMain.btnEdit.Click += main_editToolStripMenuItem_Click;
		mPanelViewMain.btnDelete.Click += main_deleteToolStripMenuItem_Click;
		mPanelViewMain.btnView.Click += main_viewToolStripMenuItem_Click;
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
			queryArgs.Predicate = "GroupId=@0";
			queryArgs.PredicateParameters = new string[1] { ((AuditableEntity)(object)mGroup).Id.ToString() };
			queryArgs.Order = "Sort, Created";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs body = queryArgs;
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/Product/Gets").Result;
			mSearchMain.Init(Common.getDataTable<ProductViewModel>(result), dgvMain);
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
		mPanelViewMain.btnEdit.Enabled = enable;
		mPanelViewMain.btnDelete.Enabled = enable;
		mPanelViewMain.btnView.Enabled = enable;
		main_newToolStripMenuItem.Enabled = enable;
		main_editToolStripMenuItem.Enabled = enable;
		main_deleteToolStripMenuItem.Enabled = enable;
		main_viewToolStripMenuItem.Enabled = enable;
		main_planToolStripMenuItem.Visible = false;
		main_similarToolStripMenuItem.Visible = false;
		main_templateotherToolStripMenuItem.Enabled = enable;
		main_aqlToolStripMenuItem.Enabled = enable;
		main_templateotherToolStripMenuItem.Enabled = enable;
		if (enable)
		{
			main_copyToolStripMenuItem.Enabled = enable;
			main_copyToolStripMenuItem.Text = Common.getTextLanguage(this, "Copy");
			main_importToolStripMenuItem.Text = Common.getTextLanguage(this, "Import");
		}
		else
		{
			main_copyToolStripMenuItem.Text = Common.getTextLanguage(this, "Paste");
			main_importToolStripMenuItem.Text = Common.getTextLanguage(this, "Confirm");
		}
		main_importToolStripMenuItem.Visible = false;
	}

	public void OpenMeasurement(Guid id, string filename = null)
	{
		Cursor.Current = Cursors.WaitCursor;
		if (id.Equals(Guid.Empty))
		{
			MessageBox.Show(Common.getTextLanguage(this, "wSelectRow"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		List<Type> list = new List<Type>();
		list.Add(typeof(frmMeasurementView));
		Common.closeForm(list);
		new frmMeasurementView(this, id, filename).Show();
	}

	private void disControlMove()
	{
		mPanelViewMain.btnEdit.Enabled = false;
		mPanelViewMain.btnDelete.Enabled = false;
		mPanelViewMain.btnView.Enabled = false;
		main_newToolStripMenuItem.Enabled = false;
		main_editToolStripMenuItem.Enabled = false;
		main_viewToolStripMenuItem.Enabled = false;
		main_deleteToolStripMenuItem.Enabled = false;
		main_importToolStripMenuItem.Enabled = false;
		main_aqlToolStripMenuItem.Enabled = false;
		main_copyToolStripMenuItem.Enabled = false;
		main_templateotherToolStripMenuItem.Enabled = false;
		main_similarToolStripMenuItem.Enabled = false;
		main_planToolStripMenuItem.Enabled = false;
		mSearchMain.Enabled = false;
	}

	private void updateMove()
	{
		Cursor.Current = Cursors.WaitCursor;
		try
		{
			start_Proccessor();
			ResponseDto result = frmLogin.client.MoveAsync(IdFrom, IdTo, "/api/Product/Move/{idfrom}/{idto}").Result;
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
				mPanelViewMain.Display();
			}
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
		int.TryParse(mSearchMain.txtPage.Text, out var result);
		int.TryParse(mSearchMain.cbbLimit.Text, out var result2);
		foreach (DataGridViewRow item in (IEnumerable)dataGridView.Rows)
		{
			item.Cells["No"].Value = (result - 1) * result2 + item.Index + 1;
			if (main_importToolStripMenuItem.Text.Equals(Common.getTextLanguage(this, "Confirm")))
			{
				item.DefaultCellStyle.ForeColor = Color.Green;
			}
			else if (int.Parse(item.Cells["TotalMeas"].Value.ToString()) == 0 || !(bool)item.Cells["IsActivated"].Value)
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
		List<Type> list = new List<Type>();
		list.Add(typeof(frmProductAdd));
		Common.closeForm(list);
		new frmProductAdd(this, (DataTable)dgvMain.DataSource, mGroup, mId).Show();
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
		List<Type> list = new List<Type>();
		list.Add(typeof(frmProductAdd));
		Common.closeForm(list);
		new frmProductAdd(this, (DataTable)dgvMain.DataSource, mGroup, mId, isadd: false).Show();
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
			if (!MessageBox.Show(string.Format("{0} {1}", Common.getTextLanguage(this, "wSureDelete"), dgvMain.CurrentRow.Cells["Code"].Value), Common.getTextLanguage(this, "QUESTION"), MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.Yes))
			{
				return;
			}
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				start_Proccessor();
				ResponseDto result = frmLogin.client.DeleteAsync(mId, "/api/Product/Delete/{id}").Result;
				if (!result.Success)
				{
					throw new Exception(result.Messages.ElementAt(0).Message);
				}
				isClose = false;
				List<Type> list = new List<Type>();
				list.Add(typeof(frmProductAdd));
				list.Add(typeof(frmMeasurementView));
				list.Add(typeof(frmAQLView));
				list.Add(typeof(frmPlanView));
				list.Add(typeof(frmCreateFile));
				list.Add(typeof(frmSimilarView));
				list.Add(typeof(frmTemplateOtherView));
				Common.closeForm(list);
				if (dgvMain.CurrentCell != null && dgvMain.CurrentRow.Cells["TemplateId"].Value != null && Guid.TryParse(dgvMain.CurrentRow.Cells["TemplateId"].Value.ToString(), out var result2))
				{
					result = frmLogin.client.DeleteAsync(result2, "/api/Template/Delete/{id}").Result;
					if (!result.Success)
					{
						throw new Exception(result.Messages.ElementAt(0).Message);
					}
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
			main_refreshToolStripMenuItem.PerformClick();
		}
	}

	private void main_importToolStripMenuItem_Click(object sender, EventArgs e)
	{
		//IL_0573: Unknown result type (might be due to invalid IL or missing references)
		//IL_0578: Unknown result type (might be due to invalid IL or missing references)
		//IL_0590: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0600: Expected O, but got Unknown
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
			string[] productHeaders = MetaType.ProductHeaders;
			MessageBox.Show(Common.getTextLanguage(this, "FormatHeader") + "\r\n    " + string.Join(" - ", productHeaders) + "\r\n" + Common.getTextLanguage(this, "NextRow"), Common.getTextLanguage(this, "INFORMATION"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
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
						string text = Common.checkFormat(productHeaders, excelRange);
						if (!text.Equals("Ok"))
						{
							throw new Exception(text);
						}
						mProgressBarMain.Maximum = excelWorksheet.Dimension.Rows;
						for (int i = 1; i < excelWorksheet.Dimension.Rows; i++)
						{
							mProgressBarMain.Value = i;
							DataRow dataRow = dataTable.NewRow();
							for (int j = 0; j < productHeaders.Length; j++)
							{
								string text2 = Common.trimSpace(excelRange[1, j + 1].Value.ToString()).ToLower();
								if (text2.Equals(productHeaders[j].ToLower()))
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
				List<ProductViewModel> list = new List<ProductViewModel>();
				foreach (DataRow row in dataTable2.Rows)
				{
					int value = dataTable2.Rows.IndexOf(row) + 1;
					mProgressBarMain.Value = value;
					int result;
					ProductViewModel val = new ProductViewModel
					{
						Code = row["Code"].ToString(),
						Name = row["Name"].ToString(),
						Cavity = ((!int.TryParse(row["Cavity"].ToString(), out result)) ? 1 : result),
						SampleMax = ((!int.TryParse(row["SampleMax"].ToString(), out result)) ? 1 : result)
					};
					if (!string.IsNullOrEmpty(row["Description"].ToString()))
					{
						val.Description = row["Description"].ToString();
					}
					list.Add(val);
				}
				ResponseDto result2 = frmLogin.client.SaveAsync(list, "/api/Product/SaveList").Result;
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
		list.Add(typeof(frmMeasurementView));
		Common.closeForm(list);
		new frmMeasurementView(this, mId).Show();
	}

	private void main_planToolStripMenuItem_Click(object sender, EventArgs e)
	{
		mSearchMain.btnSearch.Select();
		Cursor.Current = Cursors.WaitCursor;
		if (mId.Equals(Guid.Empty))
		{
			MessageBox.Show(Common.getTextLanguage(this, "wSelectRow"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		List<Type> list = new List<Type>();
		list.Add(typeof(frmPlanView));
		Common.closeForm(list);
		new frmPlanView(this, mId).Show();
	}

	private void main_autoToolStripMenuItem_Click(object sender, EventArgs e)
	{
		if (!Common.activeForm(typeof(frmCreateFile)))
		{
			new frmCreateFile().Show();
		}
	}

	private void main_similarToolStripMenuItem_Click(object sender, EventArgs e)
	{
		mSearchMain.btnSearch.Select();
		Cursor.Current = Cursors.WaitCursor;
		if (mId.Equals(Guid.Empty))
		{
			MessageBox.Show(Common.getTextLanguage(this, "wSelectRow"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		List<Type> list = new List<Type>();
		list.Add(typeof(frmSimilarView));
		Common.closeForm(list);
		new frmSimilarView(this, mId).Show();
	}

	private void main_templateotherToolStripMenuItem_Click(object sender, EventArgs e)
	{
		mSearchMain.btnSearch.Select();
		Cursor.Current = Cursors.WaitCursor;
		if (mId.Equals(Guid.Empty))
		{
			MessageBox.Show(Common.getTextLanguage(this, "wSelectRow"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		List<Type> list = new List<Type>();
		list.Add(typeof(frmTemplateOtherView));
		Common.closeForm(list);
		new frmTemplateOtherView(this, mId).Show();
	}

	private void main_copyToolStripMenuItem_Click(object sender, EventArgs e)
	{
		mSearchMain.btnSearch.Select();
		Cursor.Current = Cursors.WaitCursor;
		if (frmLogin.User.Role.Equals(RoleWeb.Administrator) && frmLogin.User.JobTitle != "Manager" && frmLogin.User.JobTitle != "Supervisor")
		{
			MessageBox.Show(Common.getTextLanguage(typeof(frmLogin).Name, "wNoAuthorization"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		if (main_copyToolStripMenuItem.Text.Equals(Common.getTextLanguage(this, "Copy")))
		{
			if (mId.Equals(Guid.Empty))
			{
				MessageBox.Show(Common.getTextLanguage(this, "wSelectRow"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			mIdCopy = mId;
			en_disControl(enable: false);
			dgvMain_Sorted(dgvMain, null);
			return;
		}
		try
		{
			ResponseDto result = frmLogin.client.CopyAsync(mIdCopy, "/api/Product/Copy/{id}").Result;
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

	private void ptbAvatar_DoubleClick(object sender, EventArgs e)
	{
		Common.activeForm(typeof(frmLogin));
	}

	private void main_aqlToolStripMenuItem_Click(object sender, EventArgs e)
	{
		mSearchMain.btnSearch.Select();
		Cursor.Current = Cursors.WaitCursor;
		if (mId.Equals(Guid.Empty))
		{
			MessageBox.Show(Common.getTextLanguage(this, "wSelectRow"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		List<Type> list = new List<Type>();
		list.Add(typeof(frmAQLView));
		Common.closeForm(list);
		new frmAQLView(this, mId).Show();
	}

	private void main_moveToolStripMenuItem_Click(object sender, EventArgs e)
	{
		mSearchMain.btnSearch.Select();
		Cursor.Current = Cursors.WaitCursor;
		if (((AuditableEntity)(object)mGroup).Id.Equals(Guid.Empty) || mId.Equals(Guid.Empty))
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_5S_QA_System.frmProductView));
		this.contextMenuStripMain = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.main_refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.main_newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.main_editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.main_viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.main_planToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.main_similarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.main_aqlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.main_templateotherToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.main_deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.main_importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.main_copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.main_moveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.main_autoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.statusStripfrmMain = new System.Windows.Forms.StatusStrip();
		this.sprogbarStatus = new System.Windows.Forms.ToolStripProgressBar();
		this.slblStatus = new System.Windows.Forms.ToolStripStatusLabel();
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.dgvMain = new System.Windows.Forms.DataGridView();
		this.openFileDialogMain = new System.Windows.Forms.OpenFileDialog();
		this.panelmProgressBarMain = new System.Windows.Forms.Panel();
		this.mProgressBarMain = new MetroFramework.Controls.MetroProgressBar();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
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
		this.mPanelViewMain = new _5S_QA_System.View.User_control.mPanelView();
		this.mSearchMain = new _5S_QA_System.View.User_control.mSearch();
		this.No = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ImageUrl = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Cavity = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.SampleMax = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.TemplateId = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.TemplateName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.DepartmentId = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.DepartmentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.TotalMeas = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.IsAQL = new System.Windows.Forms.DataGridViewCheckBoxColumn();
		this.GroupId = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.GroupCodeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
		this.contextMenuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[18]
		{
			this.main_refreshToolStripMenuItem, this.toolStripSeparator1, this.main_newToolStripMenuItem, this.main_editToolStripMenuItem, this.main_viewToolStripMenuItem, this.toolStripSeparator3, this.main_planToolStripMenuItem, this.main_similarToolStripMenuItem, this.main_aqlToolStripMenuItem, this.main_templateotherToolStripMenuItem,
			this.toolStripSeparator6, this.main_deleteToolStripMenuItem, this.toolStripSeparator2, this.main_importToolStripMenuItem, this.main_copyToolStripMenuItem, this.main_moveToolStripMenuItem, this.toolStripSeparator4, this.main_autoToolStripMenuItem
		});
		this.contextMenuStripMain.Name = "contextMenuStripStaff";
		this.contextMenuStripMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
		this.contextMenuStripMain.Size = new System.Drawing.Size(189, 320);
		this.main_refreshToolStripMenuItem.Name = "main_refreshToolStripMenuItem";
		this.main_refreshToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
		this.main_refreshToolStripMenuItem.Text = "Refresh";
		this.main_refreshToolStripMenuItem.Click += new System.EventHandler(main_refreshToolStripMenuItem_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(185, 6);
		this.main_newToolStripMenuItem.Name = "main_newToolStripMenuItem";
		this.main_newToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
		this.main_newToolStripMenuItem.Text = "New";
		this.main_newToolStripMenuItem.Click += new System.EventHandler(main_newToolStripMenuItem_Click);
		this.main_editToolStripMenuItem.Name = "main_editToolStripMenuItem";
		this.main_editToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
		this.main_editToolStripMenuItem.Text = "Edit";
		this.main_editToolStripMenuItem.Click += new System.EventHandler(main_editToolStripMenuItem_Click);
		this.main_viewToolStripMenuItem.Name = "main_viewToolStripMenuItem";
		this.main_viewToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
		this.main_viewToolStripMenuItem.Text = "Measurement";
		this.main_viewToolStripMenuItem.Click += new System.EventHandler(main_viewToolStripMenuItem_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(185, 6);
		this.main_planToolStripMenuItem.Name = "main_planToolStripMenuItem";
		this.main_planToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
		this.main_planToolStripMenuItem.Text = "View plan";
		this.main_planToolStripMenuItem.Click += new System.EventHandler(main_planToolStripMenuItem_Click);
		this.main_similarToolStripMenuItem.Name = "main_similarToolStripMenuItem";
		this.main_similarToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
		this.main_similarToolStripMenuItem.Text = "Similar";
		this.main_similarToolStripMenuItem.Click += new System.EventHandler(main_similarToolStripMenuItem_Click);
		this.main_aqlToolStripMenuItem.Name = "main_aqlToolStripMenuItem";
		this.main_aqlToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
		this.main_aqlToolStripMenuItem.Text = "Master sampling data";
		this.main_aqlToolStripMenuItem.Click += new System.EventHandler(main_aqlToolStripMenuItem_Click);
		this.main_templateotherToolStripMenuItem.Name = "main_templateotherToolStripMenuItem";
		this.main_templateotherToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
		this.main_templateotherToolStripMenuItem.Text = "Template other";
		this.main_templateotherToolStripMenuItem.Click += new System.EventHandler(main_templateotherToolStripMenuItem_Click);
		this.toolStripSeparator6.Name = "toolStripSeparator6";
		this.toolStripSeparator6.Size = new System.Drawing.Size(185, 6);
		this.main_deleteToolStripMenuItem.Name = "main_deleteToolStripMenuItem";
		this.main_deleteToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
		this.main_deleteToolStripMenuItem.Text = "Delete";
		this.main_deleteToolStripMenuItem.Click += new System.EventHandler(main_deleteToolStripMenuItem_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(185, 6);
		this.main_importToolStripMenuItem.Name = "main_importToolStripMenuItem";
		this.main_importToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
		this.main_importToolStripMenuItem.Text = "Import from excel";
		this.main_importToolStripMenuItem.Click += new System.EventHandler(main_importToolStripMenuItem_Click);
		this.main_copyToolStripMenuItem.Name = "main_copyToolStripMenuItem";
		this.main_copyToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
		this.main_copyToolStripMenuItem.Text = "Copy";
		this.main_copyToolStripMenuItem.Click += new System.EventHandler(main_copyToolStripMenuItem_Click);
		this.main_moveToolStripMenuItem.Name = "main_moveToolStripMenuItem";
		this.main_moveToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
		this.main_moveToolStripMenuItem.Text = "Move";
		this.main_moveToolStripMenuItem.Click += new System.EventHandler(main_moveToolStripMenuItem_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(185, 6);
		this.main_autoToolStripMenuItem.Name = "main_autoToolStripMenuItem";
		this.main_autoToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
		this.main_autoToolStripMenuItem.Text = "Auto create file";
		this.main_autoToolStripMenuItem.Click += new System.EventHandler(main_autoToolStripMenuItem_Click);
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
		this.statusStripfrmMain.TabIndex = 7;
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
		this.dgvMain.Columns.AddRange(this.No, this.Code, this.name, this.Description, this.ImageUrl, this.Cavity, this.SampleMax, this.TemplateId, this.TemplateName, this.DepartmentId, this.DepartmentName, this.TotalMeas, this.IsAQL, this.GroupId, this.GroupCodeName, this.Id, this.Created, this.Modified, this.CreatedBy, this.ModifiedBy, this.IsActivated);
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
		this.dgvMain.TabIndex = 2;
		this.dgvMain.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvMain_CellClick);
		this.dgvMain.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvMain_CellDoubleClick);
		this.dgvMain.CurrentCellChanged += new System.EventHandler(dgvMain_CurrentCellChanged);
		this.dgvMain.Sorted += new System.EventHandler(dgvMain_Sorted);
		this.openFileDialogMain.FileName = "product";
		this.openFileDialogMain.Filter = "File excel (*.xls, *.xlsx)| *.xls; *.xlsx";
		this.openFileDialogMain.Title = "Select template excel of product";
		this.panelmProgressBarMain.Anchor = System.Windows.Forms.AnchorStyles.None;
		this.panelmProgressBarMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panelmProgressBarMain.Controls.Add(this.mProgressBarMain);
		this.panelmProgressBarMain.Controls.Add(this.pictureBox1);
		this.panelmProgressBarMain.Location = new System.Drawing.Point(489, 237);
		this.panelmProgressBarMain.Name = "panelmProgressBarMain";
		this.panelmProgressBarMain.Size = new System.Drawing.Size(122, 128);
		this.panelmProgressBarMain.TabIndex = 29;
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
		this.dataGridViewTextBoxColumn3.FillWeight = 30f;
		this.dataGridViewTextBoxColumn3.HeaderText = "Name";
		this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
		this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn4.DataPropertyName = "Description";
		this.dataGridViewTextBoxColumn4.FillWeight = 30f;
		this.dataGridViewTextBoxColumn4.HeaderText = "Description";
		this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
		this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn5.DataPropertyName = "ImageUrl";
		this.dataGridViewTextBoxColumn5.FillWeight = 30f;
		this.dataGridViewTextBoxColumn5.HeaderText = "Image url";
		this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
		this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn6.DataPropertyName = "Cavity";
		this.dataGridViewTextBoxColumn6.FillWeight = 20f;
		this.dataGridViewTextBoxColumn6.HeaderText = "Cavity q.ty";
		this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
		this.dataGridViewTextBoxColumn7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn7.DataPropertyName = "SampleMax";
		this.dataGridViewTextBoxColumn7.FillWeight = 20f;
		this.dataGridViewTextBoxColumn7.HeaderText = "Max. sample";
		this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
		this.dataGridViewTextBoxColumn8.DataPropertyName = "TemplateId";
		this.dataGridViewTextBoxColumn8.HeaderText = "Template id";
		this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
		this.dataGridViewTextBoxColumn8.Visible = false;
		this.dataGridViewTextBoxColumn9.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn9.DataPropertyName = "TemplateName";
		this.dataGridViewTextBoxColumn9.FillWeight = 30f;
		this.dataGridViewTextBoxColumn9.HeaderText = "Template name";
		this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
		this.dataGridViewTextBoxColumn10.DataPropertyName = "DepartmentId";
		this.dataGridViewTextBoxColumn10.HeaderText = "Department id";
		this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
		this.dataGridViewTextBoxColumn10.Visible = false;
		this.dataGridViewTextBoxColumn11.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn11.DataPropertyName = "DepartmentName";
		this.dataGridViewTextBoxColumn11.FillWeight = 30f;
		this.dataGridViewTextBoxColumn11.HeaderText = "Department name";
		this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
		this.dataGridViewTextBoxColumn12.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn12.DataPropertyName = "TotalMeas";
		this.dataGridViewTextBoxColumn12.FillWeight = 20f;
		this.dataGridViewTextBoxColumn12.HeaderText = "Meas. q.ty";
		this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
		this.dataGridViewTextBoxColumn13.DataPropertyName = "Id";
		this.dataGridViewTextBoxColumn13.HeaderText = "Id";
		this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
		this.dataGridViewTextBoxColumn13.Visible = false;
		this.dataGridViewTextBoxColumn14.DataPropertyName = "Created";
		this.dataGridViewTextBoxColumn14.HeaderText = "Created";
		this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
		this.dataGridViewTextBoxColumn14.Visible = false;
		this.dataGridViewTextBoxColumn15.DataPropertyName = "Modified";
		this.dataGridViewTextBoxColumn15.HeaderText = "Modified";
		this.dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
		this.dataGridViewTextBoxColumn15.Visible = false;
		this.dataGridViewTextBoxColumn16.DataPropertyName = "CreatedBy";
		this.dataGridViewTextBoxColumn16.HeaderText = "Created by";
		this.dataGridViewTextBoxColumn16.Name = "dataGridViewTextBoxColumn16";
		this.dataGridViewTextBoxColumn16.Visible = false;
		this.dataGridViewTextBoxColumn17.DataPropertyName = "ModifiedBy";
		this.dataGridViewTextBoxColumn17.HeaderText = "Modified by";
		this.dataGridViewTextBoxColumn17.Name = "dataGridViewTextBoxColumn17";
		this.dataGridViewTextBoxColumn17.Visible = false;
		this.dataGridViewTextBoxColumn18.DataPropertyName = "IsActivated";
		this.dataGridViewTextBoxColumn18.HeaderText = "Is activated";
		this.dataGridViewTextBoxColumn18.Name = "dataGridViewTextBoxColumn18";
		this.dataGridViewTextBoxColumn18.Visible = false;
		this.mPanelViewMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.mPanelViewMain.Dock = System.Windows.Forms.DockStyle.Right;
		this.mPanelViewMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.mPanelViewMain.Location = new System.Drawing.Point(580, 134);
		this.mPanelViewMain.Margin = new System.Windows.Forms.Padding(0);
		this.mPanelViewMain.Name = "mPanelViewMain";
		this.mPanelViewMain.Size = new System.Drawing.Size(500, 420);
		this.mPanelViewMain.TabIndex = 21;
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
		this.mSearchMain.TabIndex = 20;
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
		this.Description.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Description.DataPropertyName = "Description";
		this.Description.FillWeight = 30f;
		this.Description.HeaderText = "Description";
		this.Description.Name = "Description";
		this.Description.ReadOnly = true;
		this.ImageUrl.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.ImageUrl.DataPropertyName = "ImageUrl";
		this.ImageUrl.FillWeight = 30f;
		this.ImageUrl.HeaderText = "Image url";
		this.ImageUrl.Name = "ImageUrl";
		this.ImageUrl.ReadOnly = true;
		this.Cavity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Cavity.DataPropertyName = "Cavity";
		this.Cavity.FillWeight = 20f;
		this.Cavity.HeaderText = "Cavity q.ty";
		this.Cavity.Name = "Cavity";
		this.Cavity.ReadOnly = true;
		this.SampleMax.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.SampleMax.DataPropertyName = "SampleMax";
		this.SampleMax.FillWeight = 20f;
		this.SampleMax.HeaderText = "Max. sample";
		this.SampleMax.Name = "SampleMax";
		this.SampleMax.ReadOnly = true;
		this.TemplateId.DataPropertyName = "TemplateId";
		this.TemplateId.HeaderText = "Template id";
		this.TemplateId.Name = "TemplateId";
		this.TemplateId.ReadOnly = true;
		this.TemplateId.Visible = false;
		this.TemplateName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.TemplateName.DataPropertyName = "TemplateName";
		this.TemplateName.FillWeight = 30f;
		this.TemplateName.HeaderText = "Template name";
		this.TemplateName.Name = "TemplateName";
		this.TemplateName.ReadOnly = true;
		this.DepartmentId.DataPropertyName = "DepartmentId";
		this.DepartmentId.HeaderText = "Department id";
		this.DepartmentId.Name = "DepartmentId";
		this.DepartmentId.ReadOnly = true;
		this.DepartmentId.Visible = false;
		this.DepartmentName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.DepartmentName.DataPropertyName = "DepartmentName";
		this.DepartmentName.FillWeight = 30f;
		this.DepartmentName.HeaderText = "Department name";
		this.DepartmentName.Name = "DepartmentName";
		this.DepartmentName.ReadOnly = true;
		this.TotalMeas.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.TotalMeas.DataPropertyName = "TotalMeas";
		this.TotalMeas.FillWeight = 20f;
		this.TotalMeas.HeaderText = "Meas. q.ty";
		this.TotalMeas.Name = "TotalMeas";
		this.TotalMeas.ReadOnly = true;
		this.IsAQL.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.IsAQL.DataPropertyName = "IsAQL";
		this.IsAQL.FalseValue = "false";
		this.IsAQL.FillWeight = 15f;
		this.IsAQL.HeaderText = "Has AQL";
		this.IsAQL.Name = "IsAQL";
		this.IsAQL.ReadOnly = true;
		this.IsAQL.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		this.IsAQL.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
		this.IsAQL.TrueValue = "true";
		this.GroupId.DataPropertyName = "GroupId";
		this.GroupId.HeaderText = "Group id";
		this.GroupId.Name = "GroupId";
		this.GroupId.ReadOnly = true;
		this.GroupId.Visible = false;
		this.GroupCodeName.DataPropertyName = "GroupCodeName";
		this.GroupCodeName.HeaderText = "Product";
		this.GroupCodeName.Name = "GroupCodeName";
		this.GroupCodeName.ReadOnly = true;
		this.GroupCodeName.Visible = false;
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
		base.Controls.Add(this.mSearchMain);
		base.Controls.Add(this.statusStripfrmMain);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "frmProductView";
		base.Padding = new System.Windows.Forms.Padding(20, 70, 20, 20);
		this.Text = "5S QA System * STAGE";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmProductView_FormClosing);
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmProductView_FormClosed);
		base.Load += new System.EventHandler(frmProductView_Load);
		base.Shown += new System.EventHandler(frmProductView_Shown);
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
