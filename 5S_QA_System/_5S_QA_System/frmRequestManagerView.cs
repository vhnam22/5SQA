using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
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

public class frmRequestManagerView : MetroForm
{
	private Guid mId;

	private int mRow;

	private int mCol;

	public bool isThis = false;

	private bool isEdit;

	private bool isAlert = false;

	private bool isMulti = false;

	private IContainer components = null;

	private SaveFileDialog saveFileDialogMain;

	private OpenFileDialog openFileDialogMain;

	private ToolTip toolTipMain;

	private StatusStrip statusStripfrmMain;

	private ToolStripProgressBar sprogbarStatus;

	public ToolStripStatusLabel slblStatus;

	private ContextMenuStrip contextMenuStripMain;

	private ToolStripMenuItem main_refreshToolStripMenuItem;

	private ToolStripSeparator toolStripSeparatorConfirm;

	private ToolStripMenuItem main_rejectToolStripMenuItem;

	private ToolStripMenuItem main_confirmToolStripMenuItem;

	private mSearch mSearchMain;

	private ToolStripMenuItem main_viewToolStripMenuItem;

	private ToolStripSeparator toolStripSeparatorView;

	private mPanelViewManager mPanelViewMain;

	private DataGridView dgvMain;

	private Panel panelLogout;

	private Label lblFullname;

	private PictureBox ptbAvatar;

	private ToolStripMenuItem main_deleteToolStripMenuItem;

	private ToolStripMenuItem main_fileToolStripMenuItem;

	private ToolStripSeparator toolStripSeparatorSelect;

	private ToolStripMenuItem enall_pageToolStripMenuItem;

	private ToolStripMenuItem unall_pageToolStripMenuItem;

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

	public frmRequestManagerView(bool isalert = false)
	{
		InitializeComponent();
		Common.setControls(this, toolTipMain, new List<ContextMenuStrip> { contextMenuStripMain });
		isAlert = isalert;
	}

	private void frmRequestManagerView_Load(object sender, EventArgs e)
	{
		Init();
	}

	private void frmRequestManagerView_Shown(object sender, EventArgs e)
	{
		if (isAlert)
		{
			LoadDataByAlert();
		}
		else
		{
			load_AllData();
		}
		mSearchMain.dtpMain.ValueChanged += dtpMain_ValueChanged;
		mSearchMain.cbbRequestStatus.SelectedIndexChanged += dtpMain_ValueChanged;
		mSearchMain.cbSearchAll.CheckedChanged += dtpMain_ValueChanged;
		mSearchMain.cbOK.CheckedChanged += dtpMain_ValueChanged;
		mSearchMain.cbNG.CheckedChanged += dtpMain_ValueChanged;
	}

	private void frmRequestManagerView_FormClosing(object sender, FormClosingEventArgs e)
	{
		GC.Collect();
	}

	private void frmRequestManagerView_FormClosed(object sender, FormClosedEventArgs e)
	{
		mPanelViewMain.mDispose();
		List<Type> list = new List<Type>();
		list.Add(typeof(frmHistoryView));
		list.Add(typeof(frmCompleteView));
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
		mSearchMain.btnSearch.Click += btnSearch_Click;
		mPanelViewMain.btnHistory.Click += main_viewToolStripMenuItem_Click;
		mPanelViewMain.btnExport.Click += main_exportToolStripMenuItem_Click;
		if (frmLogin.User.Role.Equals(RoleWeb.SuperAdministrator))
		{
			mSearchMain.cbbRequestStatus.SelectedIndex = 3;
		}
		else
		{
			mSearchMain.cbbRequestStatus.SelectedIndex = 2;
		}
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
		DateTime value = mSearchMain.dtpMain.Value;
		isAlert = false;
		mSearchMain.tpanelLimit.Enabled = !isAlert;
		mSearchMain.tpanelMain.Enabled = !isAlert;
		try
		{
			start_Proccessor();
			isEdit = true;
			QueryArgs queryArgs = new QueryArgs
			{
				Order = "Created DESC",
				Page = 1,
				Limit = int.MaxValue
			};
			if (mSearchMain.cbSearchAll.Checked)
			{
				queryArgs.Predicate = "Date>=@0 && Date<@1";
				queryArgs.PredicateParameters = new List<object>
				{
					value.Date.ToString("yyyy/MM/01"),
					value.Date.AddMonths(1).ToString("yyyy/MM/01")
				};
			}
			else
			{
				queryArgs.Predicate = "Date=@0 && Status=@1";
				queryArgs.PredicateParameters = new List<object>
				{
					value.Date.ToString("MM/dd/yyyy"),
					mSearchMain.cbbRequestStatus.Text
				};
			}
			if (frmLogin.User.Role == RoleWeb.Administrator || frmLogin.User.Role == RoleWeb.SuperAdministrator)
			{
				if (frmLogin.User.JobTitle == "Supervisor" || frmLogin.User.JobTitle == "Manager")
				{
					queryArgs.Predicate += " && Product.Department.Name.StartsWith(@2)";
				}
				else
				{
					queryArgs.Predicate += " && Product.Department.Name=@2";
				}
				queryArgs.PredicateParameters.Add(frmLogin.User.DepartmentName);
			}
			if (mSearchMain.cbOK.Checked)
			{
				if (mSearchMain.cbNG.Checked)
				{
					queryArgs.Predicate += " && (Judgement=\"OK\" || Judgement=\"NG\")";
				}
				else
				{
					queryArgs.Predicate += " && Judgement=\"OK\"";
				}
			}
			else if (mSearchMain.cbNG.Checked)
			{
				queryArgs.Predicate += " && Judgement=\"NG\"";
			}
			ResponseDto result = frmLogin.client.GetsAsync(queryArgs, "/api/Request/Gets").Result;
			mSearchMain.Init(Common.getDataTableIsSelect<RequestExportViewModel>(result, select: false), dgvMain);
			load_dgvMain();
			SetIsSelect(!mSearchMain.cbSearchAll.Checked);
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
			MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			return false;
		}
	}

	private bool isRequestOK()
	{
		bool result = true;
		try
		{
			ResponseDto result2 = frmLogin.client.GetsAsync(mId, "/api/Request/Gets/{id}").Result;
			result = result2.Count.Equals(0);
		}
		catch
		{
		}
		return result;
	}

	private DataTable getRequestStatuss(string id)
	{
		DataTable result = new DataTable();
		try
		{
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "RequestId=@0 && Status!=@1 ";
			queryArgs.PredicateParameters = new string[2] { id, "OK" };
			queryArgs.Order = "Sample";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs body = queryArgs;
			ResponseDto result2 = frmLogin.client.GetsAsync(body, "/api/RequestStatus/Gets").Result;
			result = Common.getDataTableIsSelect<RequestStatusQuickViewModel>(result2, select: false);
		}
		catch
		{
		}
		return result;
	}

	public void RefreshFromSocket(RequestViewModel request)
	{
		if (isThis || isMulti)
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

	public void LoadDataByAlert()
	{
		Cursor.Current = Cursors.WaitCursor;
		isAlert = true;
		mSearchMain.tpanelLimit.Enabled = !isAlert;
		mSearchMain.tpanelMain.Enabled = !isAlert;
		try
		{
			start_Proccessor();
			isEdit = true;
			QueryArgs queryArgs = new QueryArgs
			{
				Order = "Created DESC",
				Page = 1,
				Limit = int.MaxValue,
				Predicate = "Date>=@0 && Date<=@1 && Status=@2",
				PredicateParameters = new List<object>
				{
					DateTime.Now.AddDays(-7.0).ToString("yyyy/MM/dd"),
					DateTime.Now.ToString("yyyy/MM/dd")
				}
			};
			if (frmLogin.User.Role == RoleWeb.Administrator)
			{
				queryArgs.PredicateParameters.Add("Completed");
			}
			else
			{
				queryArgs.PredicateParameters.Add("Checked");
			}
			if (frmLogin.User.JobTitle == "Supervisor" || frmLogin.User.JobTitle == "Manager")
			{
				queryArgs.Predicate += " && Product.Department.Name.StartsWith(@3)";
			}
			else
			{
				queryArgs.Predicate += " && Product.Department.Name=@3";
			}
			queryArgs.PredicateParameters.Add(frmLogin.User.DepartmentName);
			if (mSearchMain.cbOK.Checked)
			{
				if (mSearchMain.cbNG.Checked)
				{
					queryArgs.Predicate += " && (Judgement=\"OK\" || Judgement=\"NG\")";
				}
				else
				{
					queryArgs.Predicate += " && Judgement=\"OK\"";
				}
			}
			else if (mSearchMain.cbNG.Checked)
			{
				queryArgs.Predicate += " && Judgement=\"NG\"";
			}
			ResponseDto result = frmLogin.client.GetsAsync(queryArgs, "/api/Request/Gets").Result;
			mSearchMain.Init(Common.getDataTableIsSelect<RequestExportViewModel>(result, select: false), dgvMain);
			load_dgvMain();
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
	}

	private void SetIsSelect(bool en)
	{
		enall_pageToolStripMenuItem.Visible = en;
		unall_pageToolStripMenuItem.Visible = en;
		toolStripSeparatorSelect.Visible = en;
		dgvMain.Columns["IsSelect"].Visible = en;
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
			main_fileToolStripMenuItem.Visible = !string.IsNullOrEmpty(dataGridView.CurrentRow.Cells["Link"].Value.ToString());
			switch (dataGridView.CurrentRow.Cells["Status"].Value.ToString())
			{
			case "Completed":
				main_confirmToolStripMenuItem.Visible = true;
				main_rejectToolStripMenuItem.Visible = true;
				main_viewToolStripMenuItem.Visible = true;
				main_deleteToolStripMenuItem.Visible = false;
				toolStripSeparatorConfirm.Visible = true;
				toolStripSeparatorView.Visible = true;
				break;
			case "Checked":
				main_confirmToolStripMenuItem.Visible = true;
				main_rejectToolStripMenuItem.Visible = true;
				main_viewToolStripMenuItem.Visible = true;
				main_deleteToolStripMenuItem.Visible = false;
				toolStripSeparatorConfirm.Visible = true;
				toolStripSeparatorView.Visible = true;
				if (frmLogin.User.Role.Equals(RoleWeb.Administrator))
				{
					main_confirmToolStripMenuItem.Visible = false;
					main_rejectToolStripMenuItem.Visible = false;
					toolStripSeparatorConfirm.Visible = false;
				}
				break;
			case "Approved":
				main_confirmToolStripMenuItem.Visible = false;
				main_rejectToolStripMenuItem.Visible = false;
				main_viewToolStripMenuItem.Visible = true;
				main_deleteToolStripMenuItem.Visible = false;
				toolStripSeparatorConfirm.Visible = false;
				toolStripSeparatorView.Visible = true;
				break;
			default:
				main_confirmToolStripMenuItem.Visible = false;
				main_rejectToolStripMenuItem.Visible = false;
				main_viewToolStripMenuItem.Visible = true;
				main_deleteToolStripMenuItem.Visible = true;
				toolStripSeparatorConfirm.Visible = false;
				toolStripSeparatorView.Visible = true;
				break;
			}
			Guid guid = Guid.Parse(dataGridView.CurrentRow.Cells["Id"].Value.ToString());
			if (!guid.Equals(mId))
			{
				mId = guid;
				mPanelViewMain.Display();
			}
		}
		else
		{
			main_confirmToolStripMenuItem.Visible = false;
			main_rejectToolStripMenuItem.Visible = false;
			main_viewToolStripMenuItem.Visible = false;
			main_deleteToolStripMenuItem.Visible = false;
			main_fileToolStripMenuItem.Visible = false;
			toolStripSeparatorConfirm.Visible = false;
			toolStripSeparatorView.Visible = false;
			mId = Guid.Empty;
		}
		base.Controls.RemoveByKey("mPanelSelectFile");
		base.Controls.RemoveByKey("mPanelConfirm");
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
			string text = item.Cells["Status"].Value.ToString();
			string text2 = text;
			if (!(text2 == "Activated"))
			{
				if (text2 == "Rejected")
				{
					item.DefaultCellStyle.ForeColor = Color.DarkRed;
				}
				else
				{
					item.DefaultCellStyle.ForeColor = Color.Blue;
				}
			}
			else
			{
				item.DefaultCellStyle.ForeColor = Color.Black;
			}
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
				item.DefaultCellStyle.BackColor = Color.Yellow;
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

	public void main_refreshToolStripMenuItem_Click(object sender, EventArgs e)
	{
		mSearchMain.btnSearch.Select();
		if (isAlert)
		{
			LoadDataByAlert();
		}
		else
		{
			load_AllData();
		}
	}

	private void main_rejectToolStripMenuItem_Click(object sender, EventArgs e)
	{
		mSearchMain.btnSearch.Select();
		if (mId.Equals(Guid.Empty))
		{
			MessageBox.Show(Common.getTextLanguage(this, "wSelectRow"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
		else
		{
			if (!MessageBox.Show(string.Format("{0} {1}", Common.getTextLanguage(this, "wSureReject"), dgvMain.CurrentRow.Cells["Name"].Value), Common.getTextLanguage(this, "QUESTION"), MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.Yes))
			{
				return;
			}
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				isThis = true;
				ActiveRequestDto body = new ActiveRequestDto
				{
					Id = mId,
					Status = "Rejected"
				};
				ResponseDto result = frmLogin.client.SaveAsync(body, "/api/Request/Active").Result;
				if (!result.Success)
				{
					throw new Exception(result.Messages.ElementAt(0).Message);
				}
				frmCommentView frmCommentView2 = new frmCommentView(this, mId)
				{
					Size = new Size(600, 400)
				};
				frmCommentView2.ShowDialog();
				main_refreshToolStripMenuItem.PerformClick();
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
	}

	private void main_confirmToolStripMenuItem_Click(object sender, EventArgs e)
	{
		mSearchMain.btnSearch.Select();
		base.Controls.RemoveByKey("mPanelConfirm");
		if (mId.Equals(Guid.Empty))
		{
			MessageBox.Show(Common.getTextLanguage(this, "wSelectRow"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		List<ActiveRequestDto> list = new List<ActiveRequestDto>();
		Cursor.Current = Cursors.WaitCursor;
		string text = "";
		foreach (DataRow row in mSearchMain.dataTable.Rows)
		{
			if (bool.Parse(row["IsSelect"].ToString()) && (!(row["Status"].ToString() != "Completed") || !(row["Status"].ToString() != "Checked")))
			{
				DataTable requestStatuss = getRequestStatuss(row["Id"].ToString());
				if (requestStatuss.Rows.Count <= 0)
				{
					list.Add(new ActiveRequestDto
					{
						Id = new Guid(row["Id"].ToString()),
						Judgement = "OK",
						Status = (dgvMain.CurrentRow.Cells["Status"].Value.Equals("Completed") ? "Checked" : "Approved")
					});
					text += string.Format("\r\n     {0}", row["Name"]);
				}
			}
		}
		if (list.Count > 0)
		{
			if (!MessageBox.Show(Common.getTextLanguage(this, "wSureComplete") + " " + text.TrimEnd('\n'), Common.getTextLanguage(this, "QUESTION"), MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.Yes))
			{
				return;
			}
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				isMulti = true;
				ResponseDto result = frmLogin.client.SaveAsync(list, "/api/Request/ActiveList").Result;
				if (!result.Success)
				{
					throw new Exception(result.Messages.ElementAt(0).Message);
				}
				main_refreshToolStripMenuItem.PerformClick();
				isMulti = false;
				return;
			}
			catch (Exception ex)
			{
				isMulti = false;
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
				return;
			}
		}
		bool result2;
		bool flag = bool.TryParse(ConfigurationManager.ConnectionStrings["Acceptable"].ConnectionString, out result2) && result2;
		bool flag2 = true;
		if (flag)
		{
			Cursor.Current = Cursors.WaitCursor;
			DataTable requestStatuss2 = getRequestStatuss(mId.ToString());
			if (requestStatuss2.Rows.Count > 0)
			{
				flag2 = false;
				mPanelConfirm mPanelConfirm2 = new mPanelConfirm(requestStatuss2);
				base.Controls.Add(mPanelConfirm2);
				mPanelConfirm2.BringToFront();
			}
		}
		if (!flag2 || !MessageBox.Show(string.Format("{0} {1}", Common.getTextLanguage(this, "wSureComplete"), dgvMain.CurrentRow.Cells["Name"].Value), Common.getTextLanguage(this, "QUESTION"), MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.Yes))
		{
			return;
		}
		try
		{
			Cursor.Current = Cursors.WaitCursor;
			isThis = true;
			ActiveRequestDto activeRequestDto = new ActiveRequestDto
			{
				Id = mId,
				Judgement = dgvMain.CurrentRow.Cells["Judgement"].Value.ToString()
			};
			if (dgvMain.CurrentRow.Cells["Status"].Value.Equals("Completed"))
			{
				activeRequestDto.Status = "Checked";
			}
			else
			{
				activeRequestDto.Status = "Approved";
			}
			ResponseDto result3 = frmLogin.client.SaveAsync(activeRequestDto, "/api/Request/Active").Result;
			if (!result3.Success)
			{
				throw new Exception(result3.Messages.ElementAt(0).Message);
			}
			main_refreshToolStripMenuItem.PerformClick();
		}
		catch (Exception ex3)
		{
			isThis = false;
			if (ex3.InnerException is ApiException { StatusCode: var statusCode2 } ex4)
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
					debugOutput("ERR: " + ex4.Message.Replace("\n", ""));
					MessageBox.Show(ex4.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			else
			{
				debugOutput("ERR: " + ex3.Message);
				MessageBox.Show(ex3.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
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
		list.Add(typeof(frmCompleteView));
		Common.closeForm(list);
		RequestViewModel request = new RequestViewModel
		{
			Id = new Guid(dgvMain.CurrentRow.Cells["Id"].Value.ToString()),
			ProductId = new Guid(dgvMain.CurrentRow.Cells["ProductId"].Value.ToString()),
			Code = dgvMain.CurrentRow.Cells["Code"].Value.ToString(),
			Name = dgvMain.CurrentRow.Cells["Name"].Value.ToString(),
			Sample = int.Parse(dgvMain.CurrentRow.Cells["Sample"].Value.ToString()),
			ProductCavity = int.Parse(dgvMain.CurrentRow.Cells["ProductCavity"].Value.ToString()),
			Quantity = int.Parse(dgvMain.CurrentRow.Cells["Quantity"].Value.ToString())
		};
		new frmCompleteView(request).Show();
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
				list.Add(typeof(frmHistoryView));
				list.Add(typeof(frmCompleteView));
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

	private void dgvMain_MouseClick(object sender, MouseEventArgs e)
	{
		base.Controls.RemoveByKey("mPanelSelectFile");
		base.Controls.RemoveByKey("mPanelConfirm");
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_5S_QA_System.frmRequestManagerView));
		this.saveFileDialogMain = new System.Windows.Forms.SaveFileDialog();
		this.openFileDialogMain = new System.Windows.Forms.OpenFileDialog();
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.statusStripfrmMain = new System.Windows.Forms.StatusStrip();
		this.sprogbarStatus = new System.Windows.Forms.ToolStripProgressBar();
		this.slblStatus = new System.Windows.Forms.ToolStripStatusLabel();
		this.contextMenuStripMain = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.main_refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparatorView = new System.Windows.Forms.ToolStripSeparator();
		this.main_viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.main_fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.main_deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparatorConfirm = new System.Windows.Forms.ToolStripSeparator();
		this.main_rejectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.main_confirmToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparatorSelect = new System.Windows.Forms.ToolStripSeparator();
		this.enall_pageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.unall_pageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.dgvMain = new System.Windows.Forms.DataGridView();
		this.panelLogout = new System.Windows.Forms.Panel();
		this.lblFullname = new System.Windows.Forms.Label();
		this.ptbAvatar = new System.Windows.Forms.PictureBox();
		this.mPanelViewMain = new _5S_QA_System.mPanelViewManager();
		this.mSearchMain = new _5S_QA_System.View.User_control.mSearch();
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
		this.statusStripfrmMain.SuspendLayout();
		this.contextMenuStripMain.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvMain).BeginInit();
		this.panelLogout.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.ptbAvatar).BeginInit();
		base.SuspendLayout();
		this.saveFileDialogMain.Filter = "File excel (*.xls, *.xlsx)| *.xls; *.xlsx";
		this.saveFileDialogMain.Title = "Select the path to save the file";
		this.openFileDialogMain.Filter = "Files PDF(*.pdf)| *.pdf; ";
		this.openFileDialogMain.Title = "Please select a file pdf";
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
		this.statusStripfrmMain.TabIndex = 22;
		this.sprogbarStatus.Name = "sprogbarStatus";
		this.sprogbarStatus.Size = new System.Drawing.Size(200, 20);
		this.slblStatus.Name = "slblStatus";
		this.slblStatus.Size = new System.Drawing.Size(16, 21);
		this.slblStatus.Text = "...";
		this.contextMenuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[11]
		{
			this.main_refreshToolStripMenuItem, this.toolStripSeparatorView, this.main_viewToolStripMenuItem, this.main_fileToolStripMenuItem, this.main_deleteToolStripMenuItem, this.toolStripSeparatorConfirm, this.main_rejectToolStripMenuItem, this.main_confirmToolStripMenuItem, this.toolStripSeparatorSelect, this.enall_pageToolStripMenuItem,
			this.unall_pageToolStripMenuItem
		});
		this.contextMenuStripMain.Name = "contextMenuStripStaff";
		this.contextMenuStripMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
		this.contextMenuStripMain.Size = new System.Drawing.Size(197, 198);
		this.main_refreshToolStripMenuItem.Name = "main_refreshToolStripMenuItem";
		this.main_refreshToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
		this.main_refreshToolStripMenuItem.Text = "Refresh";
		this.main_refreshToolStripMenuItem.Click += new System.EventHandler(main_refreshToolStripMenuItem_Click);
		this.toolStripSeparatorView.Name = "toolStripSeparatorView";
		this.toolStripSeparatorView.Size = new System.Drawing.Size(193, 6);
		this.toolStripSeparatorView.Visible = false;
		this.main_viewToolStripMenuItem.Name = "main_viewToolStripMenuItem";
		this.main_viewToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
		this.main_viewToolStripMenuItem.Text = "History";
		this.main_viewToolStripMenuItem.Visible = false;
		this.main_viewToolStripMenuItem.Click += new System.EventHandler(main_viewToolStripMenuItem_Click);
		this.main_fileToolStripMenuItem.Name = "main_fileToolStripMenuItem";
		this.main_fileToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
		this.main_fileToolStripMenuItem.Text = "File result";
		this.main_fileToolStripMenuItem.Visible = false;
		this.main_fileToolStripMenuItem.Click += new System.EventHandler(main_fileToolStripMenuItem_Click);
		this.main_deleteToolStripMenuItem.Name = "main_deleteToolStripMenuItem";
		this.main_deleteToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
		this.main_deleteToolStripMenuItem.Text = "Delete";
		this.main_deleteToolStripMenuItem.Visible = false;
		this.main_deleteToolStripMenuItem.Click += new System.EventHandler(main_deleteToolStripMenuItem_Click);
		this.toolStripSeparatorConfirm.Name = "toolStripSeparatorConfirm";
		this.toolStripSeparatorConfirm.Size = new System.Drawing.Size(193, 6);
		this.toolStripSeparatorConfirm.Visible = false;
		this.main_rejectToolStripMenuItem.Name = "main_rejectToolStripMenuItem";
		this.main_rejectToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
		this.main_rejectToolStripMenuItem.Text = "Reject";
		this.main_rejectToolStripMenuItem.Visible = false;
		this.main_rejectToolStripMenuItem.Click += new System.EventHandler(main_rejectToolStripMenuItem_Click);
		this.main_confirmToolStripMenuItem.Name = "main_confirmToolStripMenuItem";
		this.main_confirmToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
		this.main_confirmToolStripMenuItem.Text = "Confirm";
		this.main_confirmToolStripMenuItem.Visible = false;
		this.main_confirmToolStripMenuItem.Click += new System.EventHandler(main_confirmToolStripMenuItem_Click);
		this.toolStripSeparatorSelect.Name = "toolStripSeparatorSelect";
		this.toolStripSeparatorSelect.Size = new System.Drawing.Size(193, 6);
		this.enall_pageToolStripMenuItem.Name = "enall_pageToolStripMenuItem";
		this.enall_pageToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
		this.enall_pageToolStripMenuItem.Text = "Enable all current page";
		this.enall_pageToolStripMenuItem.Click += new System.EventHandler(enall_pageToolStripMenuItem_Click);
		this.unall_pageToolStripMenuItem.Name = "unall_pageToolStripMenuItem";
		this.unall_pageToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
		this.unall_pageToolStripMenuItem.Text = "Unable all current page";
		this.unall_pageToolStripMenuItem.Click += new System.EventHandler(unall_pageToolStripMenuItem_Click);
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
		this.dgvMain.Location = new System.Drawing.Point(20, 134);
		this.dgvMain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.dgvMain.MultiSelect = false;
		this.dgvMain.Name = "dgvMain";
		this.dgvMain.RowHeadersWidth = 25;
		this.dgvMain.Size = new System.Drawing.Size(1060, 420);
		this.dgvMain.TabIndex = 33;
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
		this.mPanelViewMain.Location = new System.Drawing.Point(380, 134);
		this.mPanelViewMain.Margin = new System.Windows.Forms.Padding(4);
		this.mPanelViewMain.Name = "mPanelViewMain";
		this.mPanelViewMain.Size = new System.Drawing.Size(700, 420);
		this.mPanelViewMain.TabIndex = 32;
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
		this.mSearchMain.TabIndex = 24;
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
		this.name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.name.DataPropertyName = "Name";
		this.name.FillWeight = 30f;
		this.name.HeaderText = "Name";
		this.name.Name = "name";
		this.name.ReadOnly = true;
		this.ProductId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.ProductId.DataPropertyName = "ProductId";
		this.ProductId.FillWeight = 40f;
		this.ProductId.HeaderText = "Product id";
		this.ProductId.Name = "ProductId";
		this.ProductId.ReadOnly = true;
		this.ProductId.Visible = false;
		this.ProductStage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.ProductStage.DataPropertyName = "ProductStage";
		this.ProductStage.FillWeight = 30f;
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
		this.ProductName.FillWeight = 30f;
		this.ProductName.HeaderText = "Product name";
		this.ProductName.Name = "ProductName";
		this.ProductName.ReadOnly = true;
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
		base.Controls.Add(this.mSearchMain);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "frmRequestManagerView";
		base.Padding = new System.Windows.Forms.Padding(20, 70, 20, 20);
		this.Text = "5S QA System * REQUEST MANAGER";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmRequestManagerView_FormClosing);
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmRequestManagerView_FormClosed);
		base.Load += new System.EventHandler(frmRequestManagerView_Load);
		base.Shown += new System.EventHandler(frmRequestManagerView_Shown);
		this.statusStripfrmMain.ResumeLayout(false);
		this.statusStripfrmMain.PerformLayout();
		this.contextMenuStripMain.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.dgvMain).EndInit();
		this.panelLogout.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.ptbAvatar).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
