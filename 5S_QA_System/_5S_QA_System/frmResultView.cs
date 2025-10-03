using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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

public class frmResultView : MetroForm
{
	private Guid mIdParent;

	private int mSample;

	private int mRow;

	private int mCol;

	private bool isEdit;

	private IContainer components = null;

	private StatusStrip statusStripfrmMain;

	private ToolStripProgressBar sprogbarStatus;

	public ToolStripStatusLabel slblStatus;

	private ContextMenuStrip contextMenuStripMain;

	private ToolStripMenuItem main_refreshToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripMenuItem main_editToolStripMenuItem;

	private ToolTip toolTipMain;

	private mPanelOther mPanelViewMain;

	private DataGridView dgvMain;

	private mPanelSubRequest mPanelSubMain;

	private TableLayoutPanel tpanelMeasurement;

	private Label lblMeas;

	private Label lblOK;

	private Label lblNG;

	private Label lblEmpty;

	private Label lblJudgeOK;

	private Label lblJudgeNG;

	private Label lblJudgeEmpty;

	private Label lblEditResult;

	private Label lblEdit;

	private Label lblWarning;

	private Label lblWarn;

	public ComboBox cbbSample;

	private mSearch mSearchMain;

	private DataGridView dgvRequestDetail;

	private DataGridViewTextBoxColumn NoDetail;

	private DataGridViewTextBoxColumn CavityDetail;

	private DataGridViewTextBoxColumn PinDate;

	private DataGridViewTextBoxColumn ShotMold;

	private DataGridViewTextBoxColumn ProduceNo;

	private DataGridViewButtonColumn Action;

	private DataGridViewTextBoxColumn IdDetail;

	private Panel panelLogout;

	private Label lblFullname;

	private PictureBox ptbAvatar;

	private Button btnPrevious;

	private Button btnNext;

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

	public frmResultView(Guid id = default(Guid))
	{
		InitializeComponent();
		Common.setControls(this, toolTipMain, new List<ContextMenuStrip> { contextMenuStripMain });
		mIdParent = id;
	}

	private void frmResultView_Load(object sender, EventArgs e)
	{
		Init();
	}

	private void frmResultView_Shown(object sender, EventArgs e)
	{
		mPanelSubMain.Init();
		dgvMainSub_CurrentCellChanged(mPanelSubMain.dgvMain, null);
		mPanelSubMain.dgvMain.CurrentCellChanged += dgvMainSub_CurrentCellChanged;
	}

	private void frmResultView_FormClosed(object sender, FormClosedEventArgs e)
	{
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
			ResponseDto result = frmLogin.client.GetsAsync(mIdParent, sample, new List<string>(), "/api/RequestResult/Gets/{id}/{sample}").Result;
			mSearchMain.Init(Common.getDataTableNoType<RequestResultViewModel>(result), dgvMain);
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
		setSampleComplete();
	}

	private void setSampleComplete()
	{
		try
		{
			if (mIdParent.Equals(Guid.Empty))
			{
				throw new Exception();
			}
			ResponseDto result = frmLogin.client.GetsAsync(mIdParent, "/api/RequestResult/Gets/{id}").Result;
			cbbSample.Text = result.Count.ToString();
		}
		catch
		{
			cbbSample.SelectedIndex = 0;
		}
	}

	private void dgvMainSub_CurrentCellChanged(object sender, EventArgs e)
	{
		DataGridView dataGridView = sender as DataGridView;
		if (dataGridView.CurrentCell != null)
		{
			if (!mPanelSubMain.isEdit)
			{
				mPanelSubMain.mRow = dataGridView.CurrentCell.RowIndex;
				mPanelSubMain.mCol = dataGridView.CurrentCell.ColumnIndex;
			}
			mIdParent = Guid.Parse(dataGridView.CurrentRow.Cells["Id"].Value.ToString());
			mSample = int.Parse(dataGridView.CurrentRow.Cells["Sample"].Value.ToString());
		}
		else
		{
			mIdParent = Guid.Empty;
			mSample = 1;
		}
		load_cbbSample(mSample);
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
		if (mIdParent.Equals(Guid.Empty) || dgvMain.CurrentCell == null)
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
				item.Cells["Result"].Style.ForeColor = Color.Blue;
				item.Cells["Judge"].Style.ForeColor = Color.Blue;
				item.Cells["Result"].ToolTipText = "OK";
				num++;
				break;
			case "OK+":
				item.Cells["Result"].Style.ForeColor = Color.Blue;
				item.Cells["Judge"].Style.ForeColor = Color.Blue;
				item.Cells["Result"].ToolTipText = "OK+";
				num++;
				num4++;
				break;
			case "OK-":
				item.Cells["Result"].Style.ForeColor = Color.Blue;
				item.Cells["Judge"].Style.ForeColor = Color.Blue;
				item.Cells["Result"].ToolTipText = "OK-";
				num++;
				num4++;
				break;
			case "NG":
				item.Cells["Result"].Style.ForeColor = Color.Red;
				item.Cells["Judge"].Style.ForeColor = Color.Red;
				item.Cells["Result"].ToolTipText = "NG";
				num2++;
				break;
			case "NG+":
				item.Cells["Result"].Style.ForeColor = Color.Red;
				item.Cells["Judge"].Style.ForeColor = Color.Red;
				item.Cells["Result"].ToolTipText = "NG+";
				num2++;
				break;
			case "NG-":
				item.Cells["Result"].Style.ForeColor = Color.Red;
				item.Cells["Judge"].Style.ForeColor = Color.Red;
				item.Cells["Result"].ToolTipText = "NG-";
				num2++;
				break;
			}
			if (item.Cells["History"].Value != null && !string.IsNullOrEmpty(item.Cells["History"].Value.ToString()) && !item.Cells["History"].Value.ToString().Equals("[]"))
			{
				num3++;
				if (!item.Cells["MachineTypeName"].Value.ToString().StartsWith("* "))
				{
					item.Cells["MachineTypeName"].Value = string.Format("* {0}", item.Cells["MachineTypeName"].Value);
				}
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

	private void main_refreshToolStripMenuItem_Click(object sender, EventArgs e)
	{
		mSearchMain.btnSearch.Select();
		load_AllData();
		if (mIdParent.Equals(Guid.Empty))
		{
			dgvRequestDetail.Visible = false;
		}
	}

	private void main_editToolStripMenuItem_Click(object sender, EventArgs e)
	{
		mSearchMain.btnSearch.Select();
		Cursor.Current = Cursors.WaitCursor;
		if (mIdParent.Equals(Guid.Empty) || dgvMain.CurrentCell == null)
		{
			MessageBox.Show(Common.getTextLanguage(this, "wSelectRow"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		mPanelViewMain.setIdRequest(mIdParent);
		mPanelViewMain.Visible = true;
		mPanelViewMain.load_dgvContent((Enum)FormType.EDIT);
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

	private void dgvRequestDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
	{
		DataGridView dataGridView = sender as DataGridView;
		if (dataGridView.CurrentCell.Value != null)
		{
			dataGridView.CurrentCell.Value = Common.trimSpace(dataGridView.CurrentCell.Value.ToString());
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
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_5S_QA_System.frmResultView));
		this.statusStripfrmMain = new System.Windows.Forms.StatusStrip();
		this.sprogbarStatus = new System.Windows.Forms.ToolStripProgressBar();
		this.slblStatus = new System.Windows.Forms.ToolStripStatusLabel();
		this.contextMenuStripMain = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.main_refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.main_editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.cbbSample = new System.Windows.Forms.ComboBox();
		this.btnPrevious = new System.Windows.Forms.Button();
		this.btnNext = new System.Windows.Forms.Button();
		this.dgvMain = new System.Windows.Forms.DataGridView();
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
		this.mPanelViewMain = new _5S_QA_System.View.User_control.mPanelOther();
		this.mPanelSubMain = new _5S_QA_System.mPanelSubRequest();
		this.mSearchMain = new _5S_QA_System.View.User_control.mSearch();
		this.dgvRequestDetail = new System.Windows.Forms.DataGridView();
		this.NoDetail = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.CavityDetail = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.PinDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ShotMold = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ProduceNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Action = new System.Windows.Forms.DataGridViewButtonColumn();
		this.IdDetail = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.panelLogout = new System.Windows.Forms.Panel();
		this.lblFullname = new System.Windows.Forms.Label();
		this.ptbAvatar = new System.Windows.Forms.PictureBox();
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
		this.statusStripfrmMain.SuspendLayout();
		this.contextMenuStripMain.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvMain).BeginInit();
		this.tpanelMeasurement.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvRequestDetail).BeginInit();
		this.panelLogout.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.ptbAvatar).BeginInit();
		base.SuspendLayout();
		this.statusStripfrmMain.BackColor = System.Drawing.SystemColors.ControlLight;
		this.statusStripfrmMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.statusStripfrmMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.sprogbarStatus, this.slblStatus });
		this.statusStripfrmMain.Location = new System.Drawing.Point(20, 554);
		this.statusStripfrmMain.Name = "statusStripfrmMain";
		this.statusStripfrmMain.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
		this.statusStripfrmMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
		this.statusStripfrmMain.Size = new System.Drawing.Size(1160, 26);
		this.statusStripfrmMain.SizingGrip = false;
		this.statusStripfrmMain.Stretch = false;
		this.statusStripfrmMain.TabIndex = 27;
		this.sprogbarStatus.Name = "sprogbarStatus";
		this.sprogbarStatus.Size = new System.Drawing.Size(200, 20);
		this.slblStatus.Name = "slblStatus";
		this.slblStatus.Size = new System.Drawing.Size(16, 21);
		this.slblStatus.Text = "...";
		this.contextMenuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.main_refreshToolStripMenuItem, this.toolStripSeparator1, this.main_editToolStripMenuItem });
		this.contextMenuStripMain.Name = "contextMenuStripStaff";
		this.contextMenuStripMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
		this.contextMenuStripMain.Size = new System.Drawing.Size(114, 54);
		this.main_refreshToolStripMenuItem.Name = "main_refreshToolStripMenuItem";
		this.main_refreshToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
		this.main_refreshToolStripMenuItem.Text = "Refresh";
		this.main_refreshToolStripMenuItem.Click += new System.EventHandler(main_refreshToolStripMenuItem_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(110, 6);
		this.main_editToolStripMenuItem.Name = "main_editToolStripMenuItem";
		this.main_editToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
		this.main_editToolStripMenuItem.Text = "Edit";
		this.main_editToolStripMenuItem.Click += new System.EventHandler(main_editToolStripMenuItem_Click);
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
		this.btnPrevious.BackColor = System.Drawing.SystemColors.Control;
		this.btnPrevious.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnPrevious.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.btnPrevious.Location = new System.Drawing.Point(108, 2);
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
		this.btnNext.Location = new System.Drawing.Point(198, 2);
		this.btnNext.Margin = new System.Windows.Forms.Padding(0);
		this.btnNext.Name = "btnNext";
		this.btnNext.Size = new System.Drawing.Size(30, 25);
		this.btnNext.TabIndex = 3;
		this.btnNext.Text = ">";
		this.toolTipMain.SetToolTip(this.btnNext, "Goto next sample");
		this.btnNext.UseVisualStyleBackColor = false;
		this.btnNext.Click += new System.EventHandler(btnNext_Click);
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
		this.dgvMain.Location = new System.Drawing.Point(490, 209);
		this.dgvMain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.dgvMain.MultiSelect = false;
		this.dgvMain.Name = "dgvMain";
		this.dgvMain.ReadOnly = true;
		this.dgvMain.RowHeadersWidth = 25;
		this.dgvMain.Size = new System.Drawing.Size(690, 345);
		this.dgvMain.TabIndex = 36;
		this.dgvMain.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvMain_CellClick);
		this.dgvMain.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvMain_CellDoubleClick);
		this.dgvMain.CurrentCellChanged += new System.EventHandler(dgvMain_CurrentCellChanged);
		this.dgvMain.Sorted += new System.EventHandler(dgvMain_Sorted);
		this.tpanelMeasurement.AutoSize = true;
		this.tpanelMeasurement.ColumnCount = 15;
		this.tpanelMeasurement.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMeasurement.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30f));
		this.tpanelMeasurement.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60f));
		this.tpanelMeasurement.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30f));
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
		this.tpanelMeasurement.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tpanelMeasurement.Controls.Add(this.btnNext, 3, 0);
		this.tpanelMeasurement.Controls.Add(this.btnPrevious, 1, 0);
		this.tpanelMeasurement.Controls.Add(this.cbbSample, 2, 0);
		this.tpanelMeasurement.Controls.Add(this.lblWarning, 8, 0);
		this.tpanelMeasurement.Controls.Add(this.lblWarn, 7, 0);
		this.tpanelMeasurement.Controls.Add(this.lblEditResult, 6, 0);
		this.tpanelMeasurement.Controls.Add(this.lblEdit, 5, 0);
		this.tpanelMeasurement.Controls.Add(this.lblMeas, 0, 0);
		this.tpanelMeasurement.Controls.Add(this.lblOK, 9, 0);
		this.tpanelMeasurement.Controls.Add(this.lblNG, 11, 0);
		this.tpanelMeasurement.Controls.Add(this.lblEmpty, 13, 0);
		this.tpanelMeasurement.Controls.Add(this.lblJudgeOK, 10, 0);
		this.tpanelMeasurement.Controls.Add(this.lblJudgeNG, 12, 0);
		this.tpanelMeasurement.Controls.Add(this.lblJudgeEmpty, 14, 0);
		this.tpanelMeasurement.Dock = System.Windows.Forms.DockStyle.Top;
		this.tpanelMeasurement.Location = new System.Drawing.Point(490, 134);
		this.tpanelMeasurement.Name = "tpanelMeasurement";
		this.tpanelMeasurement.Padding = new System.Windows.Forms.Padding(2);
		this.tpanelMeasurement.RowCount = 1;
		this.tpanelMeasurement.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tpanelMeasurement.Size = new System.Drawing.Size(690, 29);
		this.tpanelMeasurement.TabIndex = 153;
		this.lblWarning.AutoSize = true;
		this.lblWarning.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblWarning.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblWarning.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lblWarning.Location = new System.Drawing.Point(483, 2);
		this.lblWarning.Name = "lblWarning";
		this.lblWarning.Size = new System.Drawing.Size(15, 25);
		this.lblWarning.TabIndex = 157;
		this.lblWarning.Text = "0";
		this.lblWarning.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblWarn.AutoSize = true;
		this.lblWarn.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblWarn.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lblWarn.Location = new System.Drawing.Point(417, 2);
		this.lblWarn.Name = "lblWarn";
		this.lblWarn.Size = new System.Drawing.Size(60, 25);
		this.lblWarn.TabIndex = 156;
		this.lblWarn.Text = "Warning:";
		this.lblWarn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblEditResult.AutoSize = true;
		this.lblEditResult.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblEditResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblEditResult.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lblEditResult.Location = new System.Drawing.Point(396, 2);
		this.lblEditResult.Name = "lblEditResult";
		this.lblEditResult.Size = new System.Drawing.Size(15, 25);
		this.lblEditResult.TabIndex = 155;
		this.lblEditResult.Text = "0";
		this.lblEditResult.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblEdit.AutoSize = true;
		this.lblEdit.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblEdit.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lblEdit.Location = new System.Drawing.Point(341, 2);
		this.lblEdit.Name = "lblEdit";
		this.lblEdit.Size = new System.Drawing.Size(49, 25);
		this.lblEdit.TabIndex = 154;
		this.lblEdit.Text = "Edited:";
		this.lblEdit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblMeas.AutoSize = true;
		this.lblMeas.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblMeas.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblMeas.Location = new System.Drawing.Point(5, 2);
		this.lblMeas.Name = "lblMeas";
		this.lblMeas.Size = new System.Drawing.Size(100, 25);
		this.lblMeas.TabIndex = 146;
		this.lblMeas.Text = "Measurement";
		this.lblMeas.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblOK.AutoSize = true;
		this.lblOK.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblOK.Location = new System.Drawing.Point(504, 2);
		this.lblOK.Name = "lblOK";
		this.lblOK.Size = new System.Drawing.Size(28, 25);
		this.lblOK.TabIndex = 147;
		this.lblOK.Text = "OK:";
		this.lblOK.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblNG.AutoSize = true;
		this.lblNG.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblNG.Location = new System.Drawing.Point(559, 2);
		this.lblNG.Name = "lblNG";
		this.lblNG.Size = new System.Drawing.Size(30, 25);
		this.lblNG.TabIndex = 148;
		this.lblNG.Text = "NG:";
		this.lblNG.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblEmpty.AutoSize = true;
		this.lblEmpty.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblEmpty.Location = new System.Drawing.Point(616, 2);
		this.lblEmpty.Name = "lblEmpty";
		this.lblEmpty.Size = new System.Drawing.Size(48, 25);
		this.lblEmpty.TabIndex = 149;
		this.lblEmpty.Text = "Empty:";
		this.lblEmpty.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblJudgeOK.AutoSize = true;
		this.lblJudgeOK.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblJudgeOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblJudgeOK.ForeColor = System.Drawing.Color.Blue;
		this.lblJudgeOK.Location = new System.Drawing.Point(538, 2);
		this.lblJudgeOK.Name = "lblJudgeOK";
		this.lblJudgeOK.Size = new System.Drawing.Size(15, 25);
		this.lblJudgeOK.TabIndex = 150;
		this.lblJudgeOK.Text = "0";
		this.lblJudgeOK.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblJudgeNG.AutoSize = true;
		this.lblJudgeNG.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblJudgeNG.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblJudgeNG.ForeColor = System.Drawing.Color.Crimson;
		this.lblJudgeNG.Location = new System.Drawing.Point(595, 2);
		this.lblJudgeNG.Name = "lblJudgeNG";
		this.lblJudgeNG.Size = new System.Drawing.Size(15, 25);
		this.lblJudgeNG.TabIndex = 151;
		this.lblJudgeNG.Text = "0";
		this.lblJudgeNG.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblJudgeEmpty.AutoSize = true;
		this.lblJudgeEmpty.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblJudgeEmpty.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblJudgeEmpty.Location = new System.Drawing.Point(670, 2);
		this.lblJudgeEmpty.Name = "lblJudgeEmpty";
		this.lblJudgeEmpty.Size = new System.Drawing.Size(15, 25);
		this.lblJudgeEmpty.TabIndex = 152;
		this.lblJudgeEmpty.Text = "0";
		this.lblJudgeEmpty.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.mPanelViewMain.AutoScroll = true;
		this.mPanelViewMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.mPanelViewMain.Dock = System.Windows.Forms.DockStyle.Right;
		this.mPanelViewMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.mPanelViewMain.Location = new System.Drawing.Point(880, 209);
		this.mPanelViewMain.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
		this.mPanelViewMain.Name = "mPanelViewMain";
		this.mPanelViewMain.Size = new System.Drawing.Size(300, 345);
		this.mPanelViewMain.TabIndex = 33;
		this.mPanelSubMain.BackColor = System.Drawing.SystemColors.Control;
		this.mPanelSubMain.Dock = System.Windows.Forms.DockStyle.Left;
		this.mPanelSubMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.mPanelSubMain.isEdit = false;
		this.mPanelSubMain.Location = new System.Drawing.Point(20, 70);
		this.mPanelSubMain.Margin = new System.Windows.Forms.Padding(4);
		this.mPanelSubMain.mCol = 0;
		this.mPanelSubMain.mRow = 0;
		this.mPanelSubMain.Name = "mPanelSubMain";
		this.mPanelSubMain.Size = new System.Drawing.Size(470, 484);
		this.mPanelSubMain.TabIndex = 37;
		this.mSearchMain.AutoSize = true;
		this.mSearchMain.BackColor = System.Drawing.SystemColors.Control;
		this.mSearchMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.mSearchMain.dataTable = null;
		this.mSearchMain.Dock = System.Windows.Forms.DockStyle.Top;
		this.mSearchMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.mSearchMain.Location = new System.Drawing.Point(490, 70);
		this.mSearchMain.Margin = new System.Windows.Forms.Padding(4);
		this.mSearchMain.MaximumSize = new System.Drawing.Size(5000, 64);
		this.mSearchMain.MinimumSize = new System.Drawing.Size(25, 64);
		this.mSearchMain.Name = "mSearchMain";
		this.mSearchMain.Padding = new System.Windows.Forms.Padding(3);
		this.mSearchMain.Size = new System.Drawing.Size(690, 64);
		this.mSearchMain.TabIndex = 162;
		this.dgvRequestDetail.AllowUserToAddRows = false;
		this.dgvRequestDetail.AllowUserToDeleteRows = false;
		this.dgvRequestDetail.AllowUserToOrderColumns = true;
		this.dgvRequestDetail.AllowUserToResizeRows = false;
		dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
		this.dgvRequestDetail.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
		this.dgvRequestDetail.BackgroundColor = System.Drawing.SystemColors.Window;
		dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.AppWorkspace;
		dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
		dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
		dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
		dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.dgvRequestDetail.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
		this.dgvRequestDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
		this.dgvRequestDetail.Columns.AddRange(this.NoDetail, this.CavityDetail, this.PinDate, this.ShotMold, this.ProduceNo, this.Action, this.IdDetail);
		this.dgvRequestDetail.Dock = System.Windows.Forms.DockStyle.Top;
		this.dgvRequestDetail.EnableHeadersVisualStyles = false;
		this.dgvRequestDetail.Location = new System.Drawing.Point(490, 163);
		this.dgvRequestDetail.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.dgvRequestDetail.MultiSelect = false;
		this.dgvRequestDetail.Name = "dgvRequestDetail";
		this.dgvRequestDetail.RowHeadersVisible = false;
		this.dgvRequestDetail.RowHeadersWidth = 25;
		this.dgvRequestDetail.Size = new System.Drawing.Size(690, 46);
		this.dgvRequestDetail.TabIndex = 163;
		this.dgvRequestDetail.Visible = false;
		this.dgvRequestDetail.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(dgvRequestDetail_CellEndEdit);
		this.NoDetail.DataPropertyName = "No";
		dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		this.NoDetail.DefaultCellStyle = dataGridViewCellStyle5;
		this.NoDetail.HeaderText = "No.";
		this.NoDetail.Name = "NoDetail";
		this.NoDetail.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.NoDetail.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.NoDetail.Visible = false;
		this.NoDetail.Width = 40;
		this.CavityDetail.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.CavityDetail.DataPropertyName = "Cavity";
		this.CavityDetail.FillWeight = 25f;
		this.CavityDetail.HeaderText = "Cavity";
		this.CavityDetail.Name = "CavityDetail";
		this.PinDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.PinDate.DataPropertyName = "PinDate";
		this.PinDate.FillWeight = 25f;
		this.PinDate.HeaderText = "Pindate";
		this.PinDate.Name = "PinDate";
		this.ShotMold.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.ShotMold.DataPropertyName = "ShotMold";
		this.ShotMold.FillWeight = 25f;
		this.ShotMold.HeaderText = "Shot mold";
		this.ShotMold.Name = "ShotMold";
		this.ProduceNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.ProduceNo.DataPropertyName = "ProduceNo";
		this.ProduceNo.FillWeight = 25f;
		this.ProduceNo.HeaderText = "Produce no.";
		this.ProduceNo.Name = "ProduceNo";
		this.Action.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Action.FillWeight = 10f;
		this.Action.HeaderText = "Action";
		this.Action.Name = "Action";
		this.Action.Text = "Save";
		this.Action.UseColumnTextForButtonValue = true;
		this.IdDetail.DataPropertyName = "Id";
		this.IdDetail.HeaderText = "Id";
		this.IdDetail.Name = "IdDetail";
		this.IdDetail.Visible = false;
		this.panelLogout.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.panelLogout.BackColor = System.Drawing.Color.Transparent;
		this.panelLogout.Controls.Add(this.lblFullname);
		this.panelLogout.Controls.Add(this.ptbAvatar);
		this.panelLogout.Location = new System.Drawing.Point(830, 27);
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
		this.No.DataPropertyName = "No";
		dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		this.No.DefaultCellStyle = dataGridViewCellStyle6;
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
		this.MachinetypeName.FillWeight = 14f;
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
		this.name.FillWeight = 30f;
		this.name.HeaderText = "Measurement";
		this.name.Name = "name";
		this.name.ReadOnly = true;
		this.Value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Value.DataPropertyName = "Value";
		dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.Value.DefaultCellStyle = dataGridViewCellStyle7;
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
		dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.Upper.DefaultCellStyle = dataGridViewCellStyle8;
		this.Upper.FillWeight = 20f;
		this.Upper.HeaderText = "Upper ";
		this.Upper.Name = "Upper";
		this.Upper.ReadOnly = true;
		this.Upper.Visible = false;
		this.Lower.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Lower.DataPropertyName = "Lower";
		dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.Lower.DefaultCellStyle = dataGridViewCellStyle9;
		this.Lower.FillWeight = 20f;
		this.Lower.HeaderText = "Lower";
		this.Lower.Name = "Lower";
		this.Lower.ReadOnly = true;
		this.Lower.Visible = false;
		this.LSL.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.LSL.DataPropertyName = "LSL";
		dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.LSL.DefaultCellStyle = dataGridViewCellStyle10;
		this.LSL.FillWeight = 20f;
		this.LSL.HeaderText = "LSL";
		this.LSL.Name = "LSL";
		this.LSL.ReadOnly = true;
		this.USL.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.USL.DataPropertyName = "USL";
		dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.USL.DefaultCellStyle = dataGridViewCellStyle11;
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
		this.Cavity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Cavity.DataPropertyName = "Cavity";
		this.Cavity.FillWeight = 15f;
		this.Cavity.HeaderText = "Cavity";
		this.Cavity.Name = "Cavity";
		this.Cavity.ReadOnly = true;
		this.Result.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Result.DataPropertyName = "Result";
		dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.Result.DefaultCellStyle = dataGridViewCellStyle12;
		this.Result.FillWeight = 20f;
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
		base.ClientSize = new System.Drawing.Size(1200, 600);
		base.Controls.Add(this.panelLogout);
		base.Controls.Add(this.mPanelViewMain);
		base.Controls.Add(this.dgvMain);
		base.Controls.Add(this.dgvRequestDetail);
		base.Controls.Add(this.tpanelMeasurement);
		base.Controls.Add(this.mSearchMain);
		base.Controls.Add(this.mPanelSubMain);
		base.Controls.Add(this.statusStripfrmMain);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "frmResultView";
		base.Padding = new System.Windows.Forms.Padding(20, 70, 20, 20);
		this.Text = "5S QA System * RESULT";
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmResultView_FormClosed);
		base.Load += new System.EventHandler(frmResultView_Load);
		base.Shown += new System.EventHandler(frmResultView_Shown);
		this.statusStripfrmMain.ResumeLayout(false);
		this.statusStripfrmMain.PerformLayout();
		this.contextMenuStripMain.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.dgvMain).EndInit();
		this.tpanelMeasurement.ResumeLayout(false);
		this.tpanelMeasurement.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvRequestDetail).EndInit();
		this.panelLogout.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.ptbAvatar).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
