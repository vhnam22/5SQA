using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using _5S_QA_DL.APIContext;
using _5S_QA_Entities.Constants;
using _5S_QA_Entities.Enums;
using _5S_QA_Entities.Models;
using _5S_QA_System.Controls;
using _5S_QA_System.Properties;
using _5S_QA_System.View.User_control;
using MetroFramework.Forms;

namespace _5S_QA_System;

public class frmHistoryView : MetroForm
{
	private bool isEdit;

	private int mRow;

	private int mCol;

	private readonly RequestViewModel mRequest;

	private IContainer components = null;

	private mSearch mSearchMain;

	private mPanelOther mPanelViewMain;

	private StatusStrip statusStripfrmMain;

	private ToolStripProgressBar sprogbarStatus;

	public ToolStripStatusLabel slblStatus;

	private ToolTip toolTipMain;

	private ContextMenuStrip contextMenuStripMain;

	private ToolStripMenuItem main_refreshToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripMenuItem main_viewToolStripMenuItem;

	private TableLayoutPanel tpanelMeasurement;

	public ComboBox cbbSample;

	private Label lblWarning;

	private Label lblEditResult;

	private Label lblMeas;

	private Label lblJudgeOK;

	private Label lblJudgeNG;

	private Label lblJudgeEmpty;

	private DataGridView dgvMain;

	private Button btnEmpty;

	private Button btnNG;

	private Button btnOK;

	private Button btnWarning;

	private Button btnEdit;

	private Panel panelLogout;

	private Label lblFullname;

	private PictureBox ptbAvatar;

	private Button btnPrevious;

	private Button btnNext;

	private Button btnViewAll;

	private DataGridViewTextBoxColumn No;

	private DataGridViewTextBoxColumn RequestId;

	private DataGridViewTextBoxColumn MeasurementId;

	private DataGridViewTextBoxColumn MachinetypeName;

	private DataGridViewTextBoxColumn MeasurementCode;

	private DataGridViewTextBoxColumn MeasurementUnit;

	private DataGridViewTextBoxColumn name;

	private DataGridViewTextBoxColumn Value;

	private DataGridViewTextBoxColumn Unit;

	private DataGridViewTextBoxColumn Upper;

	private DataGridViewTextBoxColumn Lower;

	private DataGridViewTextBoxColumn LSL;

	private DataGridViewTextBoxColumn USL;

	private DataGridViewTextBoxColumn Formula;

	private DataGridViewTextBoxColumn Cavity;

	private DataGridViewTextBoxColumn Result;

	private DataGridViewTextBoxColumn Judge;

	private DataGridViewTextBoxColumn MachineName;

	private DataGridViewTextBoxColumn StaffName;

	private DataGridViewTextBoxColumn History;

	private DataGridViewTextBoxColumn Sample;

	private DataGridViewTextBoxColumn Id;

	private new DataGridViewTextBoxColumn Created;

	private DataGridViewTextBoxColumn Modified;

	private DataGridViewTextBoxColumn CreatedBy;

	private DataGridViewTextBoxColumn ModifiedBy;

	private DataGridViewTextBoxColumn IsActivated;

	public frmHistoryView(DataGridViewRow row)
	{
		InitializeComponent();
		Common.setControls(this, toolTipMain, new List<ContextMenuStrip> { contextMenuStripMain });
		mRequest = new RequestViewModel
		{
			Id = new Guid(row.Cells["Id"].Value.ToString()),
			ProductId = new Guid(row.Cells["ProductId"].Value.ToString()),
			Name = row.Cells["Name"].Value.ToString(),
			Sample = int.Parse(row.Cells["Sample"].Value.ToString()),
			ProductCavity = int.Parse(row.Cells["ProductCavity"].Value.ToString()),
			Quantity = int.Parse(row.Cells["Quantity"].Value.ToString())
		};
		Text = Text + " (" + mRequest.Name + ")";
	}

	private void frmHistoryView_Load(object sender, EventArgs e)
	{
		Init();
	}

	private void frmHistoryView_Shown(object sender, EventArgs e)
	{
		load_cbbSample(mRequest.Sample.Value);
	}

	private void frmHistoryView_FormClosed(object sender, FormClosedEventArgs e)
	{
		GC.Collect();
		List<Type> list = new List<Type>();
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
			int sample = 1;
			if (!cbbSample.SelectedIndex.Equals(-1))
			{
				sample = int.Parse(cbbSample.Text);
			}
			ResponseDto result = frmLogin.client.GetsAsync(mRequest.Id, sample, new List<string>(), "/api/RequestResult/Gets/{id}/{sample}").Result;
			mSearchMain.Init(Common.getDataTableNoType<RequestResultViewModel>(result), dgvMain);
			Cal_Judgement();
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

	private void load_dgvMain(string judge = "")
	{
		start_Proccessor();
		if (string.IsNullOrEmpty(judge))
		{
			dgvMain.DataSource = mSearchMain.SearchInAllColums();
		}
		else
		{
			dgvMain.DataSource = mSearchMain.SearchInAllColums(judge);
		}
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

	private void load_cbbSample(int sample)
	{
		cbbSample.Items.Clear();
		for (int i = 0; i < sample; i++)
		{
			cbbSample.Items.Add(i + 1);
		}
		if (cbbSample.Items.Count.Equals(0))
		{
			cbbSample.Items.Add(1);
		}
		cbbSample.SelectedIndex = 0;
	}

	private void Cal_Judgement()
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		foreach (DataRow row in mSearchMain.dataTable.Rows)
		{
			switch (row["Judge"].ToString())
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
				num2++;
				break;
			case "NG+":
				num2++;
				break;
			case "NG-":
				num2++;
				break;
			}
			if (row["History"] != null && !string.IsNullOrEmpty(row["History"].ToString()) && !row["History"].ToString().Equals("[]"))
			{
				num3++;
			}
		}
		int num5 = mSearchMain.dataTable.Rows.Count - num - num2;
		lblJudgeOK.Text = num.ToString();
		lblJudgeNG.Text = num2.ToString();
		lblJudgeEmpty.Text = num5.ToString();
		lblEditResult.Text = num3.ToString();
		lblWarning.Text = num4.ToString();
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
			mPanelViewMain.load_dgvContent((Enum)FormType.VIEW);
		}
	}

	private void dgvMain_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		mPanelViewMain.Visible = false;
	}

	private void dgvMain_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
	{
		if (mRequest.Id.Equals(Guid.Empty) || dgvMain.CurrentCell == null)
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
			item.Cells["no"].Value = (result - 1) * result2 + item.Index + 1;
			object value = item.Cells["Judge"].Value;
			object obj = value;
			switch (obj as string)
			{
			case "OK":
				item.Cells["Result"].Style.ForeColor = Color.Blue;
				item.Cells["Judge"].Style.ForeColor = Color.Blue;
				break;
			case "OK+":
				item.Cells["Result"].Style.ForeColor = Color.Blue;
				item.Cells["Judge"].Style.ForeColor = Color.Blue;
				break;
			case "OK-":
				item.Cells["Result"].Style.ForeColor = Color.Blue;
				item.Cells["Judge"].Style.ForeColor = Color.Blue;
				break;
			case "NG":
				item.Cells["Result"].Style.ForeColor = Color.Red;
				item.Cells["Judge"].Style.ForeColor = Color.Red;
				break;
			case "NG+":
				item.Cells["Result"].Style.ForeColor = Color.Red;
				item.Cells["Judge"].Style.ForeColor = Color.Red;
				break;
			case "NG-":
				item.Cells["Result"].Style.ForeColor = Color.Red;
				item.Cells["Judge"].Style.ForeColor = Color.Red;
				break;
			}
			if (item.Cells["History"].Value != null && !string.IsNullOrEmpty(item.Cells["History"].Value.ToString()) && !item.Cells["History"].Value.ToString().Equals("[]") && !item.Cells["Name"].Value.ToString().StartsWith("* "))
			{
				item.Cells["Name"].Value = string.Format("* {0}", item.Cells["Name"].Value);
			}
			if (item.Cells["MeasurementUnit"].Value?.ToString() == "°")
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

	private void main_viewToolStripMenuItem_Click(object sender, EventArgs e)
	{
		mSearchMain.btnSearch.Select();
		dgvMain_CellDoubleClick(this, null);
	}

	private void ptbAvatar_DoubleClick(object sender, EventArgs e)
	{
		Common.activeForm(typeof(frmLogin));
	}

	private void cbbSample_SelectedIndexChanged(object sender, EventArgs e)
	{
		main_refreshToolStripMenuItem.PerformClick();
	}

	private void btnPrevious_Click(object sender, EventArgs e)
	{
		if (cbbSample.SelectedIndex > 0)
		{
			cbbSample.SelectedIndex--;
		}
	}

	private void btnNext_Click(object sender, EventArgs e)
	{
		if (cbbSample.SelectedIndex < cbbSample.Items.Count - 1)
		{
			cbbSample.SelectedIndex++;
		}
	}

	private void btnJudge_Click(object sender, EventArgs e)
	{
		Button button = sender as Button;
		load_dgvMain(button.Name);
	}

	private void btnViewAll_Click(object sender, EventArgs e)
	{
		List<Type> list = new List<Type>();
		list.Add(typeof(frmCompleteView));
		Common.closeForm(list);
		new frmCompleteView(mRequest).Show();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_5S_QA_System.frmHistoryView));
		this.contextMenuStripMain = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.main_refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.main_viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.statusStripfrmMain = new System.Windows.Forms.StatusStrip();
		this.sprogbarStatus = new System.Windows.Forms.ToolStripProgressBar();
		this.slblStatus = new System.Windows.Forms.ToolStripStatusLabel();
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.cbbSample = new System.Windows.Forms.ComboBox();
		this.btnEmpty = new System.Windows.Forms.Button();
		this.btnNG = new System.Windows.Forms.Button();
		this.btnOK = new System.Windows.Forms.Button();
		this.btnWarning = new System.Windows.Forms.Button();
		this.btnEdit = new System.Windows.Forms.Button();
		this.btnPrevious = new System.Windows.Forms.Button();
		this.btnNext = new System.Windows.Forms.Button();
		this.btnViewAll = new System.Windows.Forms.Button();
		this.tpanelMeasurement = new System.Windows.Forms.TableLayoutPanel();
		this.lblWarning = new System.Windows.Forms.Label();
		this.lblEditResult = new System.Windows.Forms.Label();
		this.lblMeas = new System.Windows.Forms.Label();
		this.lblJudgeOK = new System.Windows.Forms.Label();
		this.lblJudgeNG = new System.Windows.Forms.Label();
		this.lblJudgeEmpty = new System.Windows.Forms.Label();
		this.dgvMain = new System.Windows.Forms.DataGridView();
		this.panelLogout = new System.Windows.Forms.Panel();
		this.lblFullname = new System.Windows.Forms.Label();
		this.ptbAvatar = new System.Windows.Forms.PictureBox();
		this.mPanelViewMain = new _5S_QA_System.View.User_control.mPanelOther();
		this.mSearchMain = new _5S_QA_System.View.User_control.mSearch();
		this.No = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.RequestId = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.MeasurementId = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.MachinetypeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.MeasurementCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.MeasurementUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Upper = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Lower = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.LSL = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.USL = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Formula = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Cavity = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Result = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Judge = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.MachineName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.StaffName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.History = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Sample = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Created = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Modified = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.CreatedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ModifiedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.IsActivated = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.contextMenuStripMain.SuspendLayout();
		this.statusStripfrmMain.SuspendLayout();
		this.tpanelMeasurement.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvMain).BeginInit();
		this.panelLogout.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.ptbAvatar).BeginInit();
		base.SuspendLayout();
		this.contextMenuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.main_refreshToolStripMenuItem, this.toolStripSeparator1, this.main_viewToolStripMenuItem });
		this.contextMenuStripMain.Name = "contextMenuStripStaff";
		this.contextMenuStripMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
		this.contextMenuStripMain.Size = new System.Drawing.Size(114, 54);
		this.main_refreshToolStripMenuItem.Name = "main_refreshToolStripMenuItem";
		this.main_refreshToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
		this.main_refreshToolStripMenuItem.Text = "Refresh";
		this.main_refreshToolStripMenuItem.Click += new System.EventHandler(main_refreshToolStripMenuItem_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(110, 6);
		this.main_viewToolStripMenuItem.Name = "main_viewToolStripMenuItem";
		this.main_viewToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
		this.main_viewToolStripMenuItem.Text = "View";
		this.main_viewToolStripMenuItem.Click += new System.EventHandler(main_viewToolStripMenuItem_Click);
		this.statusStripfrmMain.BackColor = System.Drawing.SystemColors.ControlLight;
		this.statusStripfrmMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.statusStripfrmMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.sprogbarStatus, this.slblStatus });
		this.statusStripfrmMain.Location = new System.Drawing.Point(20, 554);
		this.statusStripfrmMain.Name = "statusStripfrmMain";
		this.statusStripfrmMain.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
		this.statusStripfrmMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
		this.statusStripfrmMain.Size = new System.Drawing.Size(860, 26);
		this.statusStripfrmMain.SizingGrip = false;
		this.statusStripfrmMain.Stretch = false;
		this.statusStripfrmMain.TabIndex = 154;
		this.sprogbarStatus.Name = "sprogbarStatus";
		this.sprogbarStatus.Size = new System.Drawing.Size(200, 20);
		this.slblStatus.Name = "slblStatus";
		this.slblStatus.Size = new System.Drawing.Size(16, 21);
		this.slblStatus.Text = "...";
		this.cbbSample.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cbbSample.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cbbSample.Dock = System.Windows.Forms.DockStyle.Top;
		this.cbbSample.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbbSample.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cbbSample.FormattingEnabled = true;
		this.cbbSample.Location = new System.Drawing.Point(138, 2);
		this.cbbSample.Margin = new System.Windows.Forms.Padding(0);
		this.cbbSample.MaxLength = 250;
		this.cbbSample.Name = "cbbSample";
		this.cbbSample.Size = new System.Drawing.Size(60, 23);
		this.cbbSample.TabIndex = 2;
		this.toolTipMain.SetToolTip(this.cbbSample, "Select sample no.");
		this.cbbSample.SelectedIndexChanged += new System.EventHandler(cbbSample_SelectedIndexChanged);
		this.btnEmpty.AutoSize = true;
		this.btnEmpty.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnEmpty.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnEmpty.Location = new System.Drawing.Point(782, 2);
		this.btnEmpty.Margin = new System.Windows.Forms.Padding(0);
		this.btnEmpty.Name = "btnEmpty";
		this.btnEmpty.Size = new System.Drawing.Size(55, 26);
		this.btnEmpty.TabIndex = 9;
		this.btnEmpty.Text = "Empty";
		this.toolTipMain.SetToolTip(this.btnEmpty, "Select show empty result");
		this.btnEmpty.UseVisualStyleBackColor = true;
		this.btnEmpty.Click += new System.EventHandler(btnJudge_Click);
		this.btnNG.AutoSize = true;
		this.btnNG.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnNG.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnNG.Location = new System.Drawing.Point(724, 2);
		this.btnNG.Margin = new System.Windows.Forms.Padding(0);
		this.btnNG.Name = "btnNG";
		this.btnNG.Size = new System.Drawing.Size(37, 26);
		this.btnNG.TabIndex = 8;
		this.btnNG.Text = "NG";
		this.toolTipMain.SetToolTip(this.btnNG, "Select show NG result");
		this.btnNG.UseVisualStyleBackColor = true;
		this.btnNG.Click += new System.EventHandler(btnJudge_Click);
		this.btnOK.AutoSize = true;
		this.btnOK.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnOK.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnOK.Location = new System.Drawing.Point(668, 2);
		this.btnOK.Margin = new System.Windows.Forms.Padding(0);
		this.btnOK.Name = "btnOK";
		this.btnOK.Size = new System.Drawing.Size(35, 26);
		this.btnOK.TabIndex = 7;
		this.btnOK.Text = "OK";
		this.toolTipMain.SetToolTip(this.btnOK, "Select show OK result");
		this.btnOK.UseVisualStyleBackColor = true;
		this.btnOK.Click += new System.EventHandler(btnJudge_Click);
		this.btnWarning.AutoSize = true;
		this.btnWarning.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnWarning.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnWarning.Location = new System.Drawing.Point(580, 2);
		this.btnWarning.Margin = new System.Windows.Forms.Padding(0);
		this.btnWarning.Name = "btnWarning";
		this.btnWarning.Size = new System.Drawing.Size(67, 26);
		this.btnWarning.TabIndex = 6;
		this.btnWarning.Text = "Warning";
		this.toolTipMain.SetToolTip(this.btnWarning, "Select show warning result");
		this.btnWarning.UseVisualStyleBackColor = true;
		this.btnWarning.Click += new System.EventHandler(btnJudge_Click);
		this.btnEdit.AutoSize = true;
		this.btnEdit.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnEdit.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnEdit.Location = new System.Drawing.Point(503, 2);
		this.btnEdit.Margin = new System.Windows.Forms.Padding(0);
		this.btnEdit.Name = "btnEdit";
		this.btnEdit.Size = new System.Drawing.Size(56, 26);
		this.btnEdit.TabIndex = 5;
		this.btnEdit.Text = "Edited";
		this.toolTipMain.SetToolTip(this.btnEdit, "Select show edited result");
		this.btnEdit.UseVisualStyleBackColor = true;
		this.btnEdit.Click += new System.EventHandler(btnJudge_Click);
		this.btnPrevious.BackColor = System.Drawing.SystemColors.Control;
		this.btnPrevious.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnPrevious.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.btnPrevious.Location = new System.Drawing.Point(108, 3);
		this.btnPrevious.Margin = new System.Windows.Forms.Padding(0);
		this.btnPrevious.Name = "btnPrevious";
		this.btnPrevious.Size = new System.Drawing.Size(30, 25);
		this.btnPrevious.TabIndex = 1;
		this.btnPrevious.Text = "<";
		this.toolTipMain.SetToolTip(this.btnPrevious, "Goto previous sample");
		this.btnPrevious.UseVisualStyleBackColor = false;
		this.btnPrevious.Click += new System.EventHandler(btnPrevious_Click);
		this.btnNext.BackColor = System.Drawing.SystemColors.Control;
		this.btnNext.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnNext.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.btnNext.Location = new System.Drawing.Point(198, 3);
		this.btnNext.Margin = new System.Windows.Forms.Padding(0);
		this.btnNext.Name = "btnNext";
		this.btnNext.Size = new System.Drawing.Size(30, 25);
		this.btnNext.TabIndex = 3;
		this.btnNext.Text = ">";
		this.toolTipMain.SetToolTip(this.btnNext, "Goto next sample");
		this.btnNext.UseVisualStyleBackColor = false;
		this.btnNext.Click += new System.EventHandler(btnNext_Click);
		this.btnViewAll.AutoSize = true;
		this.btnViewAll.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnViewAll.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnViewAll.Location = new System.Drawing.Point(228, 2);
		this.btnViewAll.Margin = new System.Windows.Forms.Padding(0);
		this.btnViewAll.Name = "btnViewAll";
		this.btnViewAll.Size = new System.Drawing.Size(63, 26);
		this.btnViewAll.TabIndex = 4;
		this.btnViewAll.Text = "View all";
		this.toolTipMain.SetToolTip(this.btnViewAll, "Select show all result");
		this.btnViewAll.UseVisualStyleBackColor = true;
		this.btnViewAll.Click += new System.EventHandler(btnViewAll_Click);
		this.tpanelMeasurement.AutoSize = true;
		this.tpanelMeasurement.ColumnCount = 16;
		this.tpanelMeasurement.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMeasurement.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30f));
		this.tpanelMeasurement.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60f));
		this.tpanelMeasurement.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30f));
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
		this.tpanelMeasurement.Controls.Add(this.btnViewAll, 4, 0);
		this.tpanelMeasurement.Controls.Add(this.btnNext, 3, 0);
		this.tpanelMeasurement.Controls.Add(this.btnPrevious, 1, 0);
		this.tpanelMeasurement.Controls.Add(this.btnEdit, 6, 0);
		this.tpanelMeasurement.Controls.Add(this.btnWarning, 8, 0);
		this.tpanelMeasurement.Controls.Add(this.btnOK, 10, 0);
		this.tpanelMeasurement.Controls.Add(this.btnNG, 12, 0);
		this.tpanelMeasurement.Controls.Add(this.btnEmpty, 14, 0);
		this.tpanelMeasurement.Controls.Add(this.cbbSample, 2, 0);
		this.tpanelMeasurement.Controls.Add(this.lblWarning, 9, 0);
		this.tpanelMeasurement.Controls.Add(this.lblEditResult, 7, 0);
		this.tpanelMeasurement.Controls.Add(this.lblMeas, 0, 0);
		this.tpanelMeasurement.Controls.Add(this.lblJudgeOK, 11, 0);
		this.tpanelMeasurement.Controls.Add(this.lblJudgeNG, 13, 0);
		this.tpanelMeasurement.Controls.Add(this.lblJudgeEmpty, 15, 0);
		this.tpanelMeasurement.Dock = System.Windows.Forms.DockStyle.Top;
		this.tpanelMeasurement.Location = new System.Drawing.Point(20, 134);
		this.tpanelMeasurement.Name = "tpanelMeasurement";
		this.tpanelMeasurement.Padding = new System.Windows.Forms.Padding(2);
		this.tpanelMeasurement.RowCount = 1;
		this.tpanelMeasurement.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26f));
		this.tpanelMeasurement.Size = new System.Drawing.Size(860, 30);
		this.tpanelMeasurement.TabIndex = 157;
		this.lblWarning.AutoSize = true;
		this.lblWarning.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblWarning.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblWarning.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lblWarning.Location = new System.Drawing.Point(650, 2);
		this.lblWarning.Name = "lblWarning";
		this.lblWarning.Size = new System.Drawing.Size(15, 26);
		this.lblWarning.TabIndex = 157;
		this.lblWarning.Text = "0";
		this.lblWarning.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblEditResult.AutoSize = true;
		this.lblEditResult.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblEditResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblEditResult.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lblEditResult.Location = new System.Drawing.Point(562, 2);
		this.lblEditResult.Name = "lblEditResult";
		this.lblEditResult.Size = new System.Drawing.Size(15, 26);
		this.lblEditResult.TabIndex = 155;
		this.lblEditResult.Text = "0";
		this.lblEditResult.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblMeas.AutoSize = true;
		this.lblMeas.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblMeas.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblMeas.Location = new System.Drawing.Point(5, 2);
		this.lblMeas.Name = "lblMeas";
		this.lblMeas.Size = new System.Drawing.Size(100, 26);
		this.lblMeas.TabIndex = 146;
		this.lblMeas.Text = "Measurement";
		this.lblMeas.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblJudgeOK.AutoSize = true;
		this.lblJudgeOK.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblJudgeOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblJudgeOK.ForeColor = System.Drawing.Color.Blue;
		this.lblJudgeOK.Location = new System.Drawing.Point(706, 2);
		this.lblJudgeOK.Name = "lblJudgeOK";
		this.lblJudgeOK.Size = new System.Drawing.Size(15, 26);
		this.lblJudgeOK.TabIndex = 150;
		this.lblJudgeOK.Text = "0";
		this.lblJudgeOK.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblJudgeNG.AutoSize = true;
		this.lblJudgeNG.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblJudgeNG.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblJudgeNG.ForeColor = System.Drawing.Color.Red;
		this.lblJudgeNG.Location = new System.Drawing.Point(764, 2);
		this.lblJudgeNG.Name = "lblJudgeNG";
		this.lblJudgeNG.Size = new System.Drawing.Size(15, 26);
		this.lblJudgeNG.TabIndex = 151;
		this.lblJudgeNG.Text = "0";
		this.lblJudgeNG.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblJudgeEmpty.AutoSize = true;
		this.lblJudgeEmpty.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblJudgeEmpty.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblJudgeEmpty.Location = new System.Drawing.Point(840, 2);
		this.lblJudgeEmpty.Name = "lblJudgeEmpty";
		this.lblJudgeEmpty.Size = new System.Drawing.Size(15, 26);
		this.lblJudgeEmpty.TabIndex = 152;
		this.lblJudgeEmpty.Text = "0";
		this.lblJudgeEmpty.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
		this.dgvMain.Columns.AddRange(this.No, this.RequestId, this.MeasurementId, this.MachinetypeName, this.MeasurementCode, this.MeasurementUnit, this.name, this.Value, this.Unit, this.Upper, this.Lower, this.LSL, this.USL, this.Formula, this.Cavity, this.Result, this.Judge, this.MachineName, this.StaffName, this.History, this.Sample, this.Id, this.Created, this.Modified, this.CreatedBy, this.ModifiedBy, this.IsActivated);
		this.dgvMain.ContextMenuStrip = this.contextMenuStripMain;
		this.dgvMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.dgvMain.EnableHeadersVisualStyles = false;
		this.dgvMain.Location = new System.Drawing.Point(20, 164);
		this.dgvMain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.dgvMain.MultiSelect = false;
		this.dgvMain.Name = "dgvMain";
		this.dgvMain.ReadOnly = true;
		this.dgvMain.RowHeadersWidth = 25;
		this.dgvMain.Size = new System.Drawing.Size(860, 390);
		this.dgvMain.TabIndex = 158;
		this.dgvMain.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvMain_CellClick);
		this.dgvMain.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvMain_CellDoubleClick);
		this.dgvMain.CurrentCellChanged += new System.EventHandler(dgvMain_CurrentCellChanged);
		this.dgvMain.Sorted += new System.EventHandler(dgvMain_Sorted);
		this.panelLogout.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.panelLogout.BackColor = System.Drawing.Color.Transparent;
		this.panelLogout.Controls.Add(this.lblFullname);
		this.panelLogout.Controls.Add(this.ptbAvatar);
		this.panelLogout.Location = new System.Drawing.Point(530, 27);
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
		this.mPanelViewMain.AutoScroll = true;
		this.mPanelViewMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.mPanelViewMain.Dock = System.Windows.Forms.DockStyle.Right;
		this.mPanelViewMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.mPanelViewMain.Location = new System.Drawing.Point(530, 164);
		this.mPanelViewMain.Margin = new System.Windows.Forms.Padding(9, 7, 9, 7);
		this.mPanelViewMain.Name = "mPanelViewMain";
		this.mPanelViewMain.Size = new System.Drawing.Size(350, 390);
		this.mPanelViewMain.TabIndex = 155;
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
		this.mSearchMain.Size = new System.Drawing.Size(860, 64);
		this.mSearchMain.TabIndex = 21;
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
		this.RequestId.HeaderText = "Request id";
		this.RequestId.Name = "RequestId";
		this.RequestId.ReadOnly = true;
		this.RequestId.Visible = false;
		this.MeasurementId.DataPropertyName = "MeasurementId";
		this.MeasurementId.HeaderText = "Measurement id";
		this.MeasurementId.Name = "MeasurementId";
		this.MeasurementId.ReadOnly = true;
		this.MeasurementId.Visible = false;
		this.MachinetypeName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.MachinetypeName.DataPropertyName = "MachinetypeName";
		this.MachinetypeName.FillWeight = 20f;
		this.MachinetypeName.HeaderText = "Mac.";
		this.MachinetypeName.Name = "MachinetypeName";
		this.MachinetypeName.ReadOnly = true;
		this.MeasurementCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.MeasurementCode.DataPropertyName = "MeasurementCode";
		this.MeasurementCode.FillWeight = 14f;
		this.MeasurementCode.HeaderText = "Code";
		this.MeasurementCode.Name = "MeasurementCode";
		this.MeasurementCode.ReadOnly = true;
		this.MeasurementCode.Visible = false;
		this.MeasurementUnit.DataPropertyName = "MeasurementUnit";
		this.MeasurementUnit.HeaderText = "Meas. unit";
		this.MeasurementUnit.Name = "MeasurementUnit";
		this.MeasurementUnit.ReadOnly = true;
		this.MeasurementUnit.Visible = false;
		this.name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.name.DataPropertyName = "Name";
		this.name.FillWeight = 25f;
		this.name.HeaderText = "Measurement";
		this.name.Name = "name";
		this.name.ReadOnly = true;
		this.Value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Value.DataPropertyName = "Value";
		dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.Value.DefaultCellStyle = dataGridViewCellStyle4;
		this.Value.FillWeight = 20f;
		this.Value.HeaderText = "Value";
		this.Value.Name = "Value";
		this.Value.ReadOnly = true;
		this.Unit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Unit.DataPropertyName = "Unit";
		this.Unit.FillWeight = 10f;
		this.Unit.HeaderText = "Unit";
		this.Unit.Name = "Unit";
		this.Unit.ReadOnly = true;
		this.Upper.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Upper.DataPropertyName = "Upper";
		dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.Upper.DefaultCellStyle = dataGridViewCellStyle5;
		this.Upper.FillWeight = 15f;
		this.Upper.HeaderText = "Upper ";
		this.Upper.Name = "Upper";
		this.Upper.ReadOnly = true;
		this.Lower.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Lower.DataPropertyName = "Lower";
		dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.Lower.DefaultCellStyle = dataGridViewCellStyle6;
		this.Lower.FillWeight = 15f;
		this.Lower.HeaderText = "Lower";
		this.Lower.Name = "Lower";
		this.Lower.ReadOnly = true;
		this.LSL.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.LSL.DataPropertyName = "LSL";
		dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.LSL.DefaultCellStyle = dataGridViewCellStyle7;
		this.LSL.FillWeight = 20f;
		this.LSL.HeaderText = "LSL";
		this.LSL.Name = "LSL";
		this.LSL.ReadOnly = true;
		this.USL.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.USL.DataPropertyName = "USL";
		dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.USL.DefaultCellStyle = dataGridViewCellStyle8;
		this.USL.FillWeight = 20f;
		this.USL.HeaderText = "USL";
		this.USL.Name = "USL";
		this.USL.ReadOnly = true;
		this.Formula.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Formula.DataPropertyName = "Formula";
		this.Formula.FillWeight = 20f;
		this.Formula.HeaderText = "Formula";
		this.Formula.Name = "Formula";
		this.Formula.ReadOnly = true;
		this.Formula.Visible = false;
		this.Cavity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Cavity.DataPropertyName = "Cavity";
		this.Cavity.FillWeight = 15f;
		this.Cavity.HeaderText = "Cavity";
		this.Cavity.Name = "Cavity";
		this.Cavity.ReadOnly = true;
		this.Result.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Result.DataPropertyName = "Result";
		dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.Result.DefaultCellStyle = dataGridViewCellStyle9;
		this.Result.FillWeight = 20f;
		this.Result.HeaderText = "Result";
		this.Result.Name = "Result";
		this.Result.ReadOnly = true;
		this.Judge.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Judge.DataPropertyName = "Judge";
		this.Judge.FillWeight = 12f;
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
		this.History.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.History.DataPropertyName = "History";
		this.History.FillWeight = 25f;
		this.History.HeaderText = "History";
		this.History.Name = "History";
		this.History.ReadOnly = true;
		this.History.Visible = false;
		this.Sample.DataPropertyName = "Sample";
		this.Sample.HeaderText = "Sample";
		this.Sample.Name = "Sample";
		this.Sample.ReadOnly = true;
		this.Sample.Visible = false;
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
		base.ClientSize = new System.Drawing.Size(900, 600);
		base.Controls.Add(this.panelLogout);
		base.Controls.Add(this.mPanelViewMain);
		base.Controls.Add(this.dgvMain);
		base.Controls.Add(this.statusStripfrmMain);
		base.Controls.Add(this.tpanelMeasurement);
		base.Controls.Add(this.mSearchMain);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "frmHistoryView";
		base.Padding = new System.Windows.Forms.Padding(20, 70, 20, 20);
		base.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
		this.Text = "5S QA System * HISTORY";
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmHistoryView_FormClosed);
		base.Load += new System.EventHandler(frmHistoryView_Load);
		base.Shown += new System.EventHandler(frmHistoryView_Shown);
		this.contextMenuStripMain.ResumeLayout(false);
		this.statusStripfrmMain.ResumeLayout(false);
		this.statusStripfrmMain.PerformLayout();
		this.tpanelMeasurement.ResumeLayout(false);
		this.tpanelMeasurement.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvMain).EndInit();
		this.panelLogout.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.ptbAvatar).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
