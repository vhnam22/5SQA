using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.ServiceProcess;
using System.Windows.Forms;
using _5SQA_Config_UWave.Dtos;
using _5SQA_Config_UWave.Funtions;
using _5SQA_Config_UWave.ViewModels;
using MetroFramework;
using MetroFramework.Controls;

namespace _5SQA_Config_UWave;

public class frmMain : Form
{
	private APIClient mClient;

	private string mAPIUrl = ConfigurationManager.ConnectionStrings["APIUrl"].ConnectionString;

	private string mPortComName = ConfigurationManager.ConnectionStrings["PortComName"].ConnectionString;

	private Guid mId;

	private bool isEdit;

	private int mRow;

	private int mCol;

	public static Config mConfig;

	private ServiceController mService;

	private SafeSerialPort mSerialPort;

	private bool dataProcessing;

	private string mRemember;

	private IContainer components = null;

	private ToolTip toolTipfrmMain;

	private ContextMenuStrip contextMenuStripMain;

	private ToolStripMenuItem main_refreshToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator6;

	private ToolStripMenuItem main_deleteToolStripMenuItem;

	private StatusStrip statusStripfrmMain;

	private ToolStripProgressBar sprogbarStatus;

	public ToolStripStatusLabel slblStatus;

	private mSearch mSearchMain;

	private mPanelView mPanelViewMain;

	private DataGridView dgvMain;

	private DataGridViewTextBoxColumn No;

	private DataGridViewTextBoxColumn ChanelId;

	private DataGridViewTextBoxColumn Code;

	private DataGridViewTextBoxColumn name;

	private DataGridViewTextBoxColumn Model;

	private DataGridViewTextBoxColumn Serial;

	private DataGridViewTextBoxColumn FactoryName;

	private DataGridViewTextBoxColumn MachineTypeName;

	private DataGridViewTextBoxColumn Status;

	private DataGridViewTextBoxColumn Id;

	private new DataGridViewTextBoxColumn Created;

	private DataGridViewTextBoxColumn Modified;

	private DataGridViewTextBoxColumn CreatedBy;

	private DataGridViewTextBoxColumn ModifiedBy;

	private DataGridViewTextBoxColumn IsActivated;

	private TableLayoutPanel tableLayoutPanel1;

	public Button btnStart;

	public Button btnStop;

	private TableLayoutPanel tableLayoutPanel2;

	private MetroLabel metroLabel13;

	private ComboBox cbbChanel;

	private MetroLabel metroLabel1;

	private ComboBox cbbMachine;

	private Button btnComfirm;

	public frmMain()
	{
		InitializeComponent();
	}

	private void frmMain_Load(object sender, EventArgs e)
	{
		Init();
	}

	private void frmMain_Shown(object sender, EventArgs e)
	{
		main_refreshToolStripMenuItem.PerformClick();
		btnStop.PerformClick();
	}

	private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
	{
		if (mClient != null)
		{
			mClient.CloseAPI();
		}
		GC.Collect();
		btnStart.PerformClick();
	}

	private void Init()
	{
		mPanelViewMain.Visible = false;
		mService = new ServiceController("5SQAReadUWaveService");
		mClient = new APIClient(mAPIUrl);
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

	private void load_AllData()
	{
		Cursor.Current = Cursors.WaitCursor;
		try
		{
			start_Proccessor();
			isEdit = true;
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "MachineType.Name!=@0";
			queryArgs.PredicateParameters = new string[1] { "Tablet" };
			queryArgs.Order = "Created";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs body = queryArgs;
			ResponseDto result = mClient.GetsAsync(body, "/api/Machine/NoAuthorGets").Result;
			DataTable dataTable = Common.getDataTable<MachineViewModel>(result);
			SetChanelForDataTable(dataTable);
			mSearchMain.Init(dataTable, dgvMain);
			load_cbbMachine(dataTable);
			load_dgvMain();
		}
		catch (Exception ex)
		{
			if (ex.InnerException is ApiException { StatusCode: var statusCode } ex2)
			{
				if (statusCode.Equals(401))
				{
					if (MessageBox.Show("This account is already login elsewhere.\r\nPlease login again.", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Asterisk).Equals(DialogResult.OK))
					{
						Close();
					}
				}
				else
				{
					debugOutput("ERR: " + ex2.Message.Replace("\n", ""));
					MessageBox.Show(ex2.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			else
			{
				debugOutput("ERR: " + ex.Message);
				MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
	}

	private void SetChanelForDataTable(DataTable dt)
	{
		ReadConfig();
		if (mConfig.Mappers == null)
		{
			return;
		}
		foreach (DataRow row in dt.Rows)
		{
			Mapper mapper = mConfig.Mappers.Where((Mapper x) => x.Id.Equals(row.Field<Guid>("Id"))).FirstOrDefault();
			if (mapper != null)
			{
				row.SetField("ChanelId", mapper.ChanelId);
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
		debugOutput("Successful.");
		dgvMain_Sorted(dgvMain, null);
		isEdit = false;
	}

	private void ReadConfig()
	{
		mConfig = Common.ReadFromFileConfig("Config.ini");
	}

	private void WriteConfig()
	{
		Config config = new Config();
		config.APIUrl = mAPIUrl;
		config.ComName = mPortComName;
		config.IME = Common.GetIME();
		config.Mappers = new List<Mapper>();
		foreach (DataRow row in mSearchMain.getDataTable().Rows)
		{
			if (!string.IsNullOrEmpty(row.Field<string>("ChanelId")))
			{
				config.Mappers.Add(new Mapper
				{
					Id = row.Field<Guid>("Id"),
					ChanelId = row.Field<string>("ChanelId")
				});
			}
		}
		Common.WriteToFileConfig("Config.ini", config);
	}

	private void UpdateButton()
	{
		debugOutput(mService.Status.ToString());
		if (mService.Status == ServiceControllerStatus.Stopped)
		{
			btnStart.Enabled = true;
			btnStop.Enabled = false;
		}
		else
		{
			btnStart.Enabled = false;
			btnStop.Enabled = true;
		}
	}

	public void load_cbbMachine(DataTable dt)
	{
		if (dt != null)
		{
			dt.Dispose();
			cbbMachine.ValueMember = "Id";
			cbbMachine.DisplayMember = "Name";
			cbbMachine.DataSource = dt;
		}
		else
		{
			cbbMachine.DataSource = null;
		}
	}

	private SafeSerialPort GetPort()
	{
		SafeSerialPort result = null;
		foreach (COMPortInfo item in COMPortInfo.GetCOMPortsInfo())
		{
			if (!string.IsNullOrEmpty(mConfig.ComName) && item.Description.Contains(mConfig.ComName))
			{
				result = new SafeSerialPort
				{
					BaudRate = 57600,
					Handshake = Handshake.RequestToSend,
					PortName = item.Name
				};
				break;
			}
		}
		return result;
	}

	private void OpenPort()
	{
		mSerialPort = GetPort();
		if (mSerialPort != null)
		{
			if (!mSerialPort.IsOpen)
			{
				mSerialPort.Open();
			}
			mSerialPort.DataReceived += mSerialPort_DataReceived;
		}
	}

	private void ClosePort()
	{
		if (mSerialPort != null)
		{
			if (mSerialPort.IsOpen)
			{
				mSerialPort.Close();
			}
			mSerialPort = null;
		}
	}

	private void mSerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
	{
		if (!dataProcessing)
		{
			dataProcessing = true;
			string text = mSerialPort.ReadTo("\r");
			if (text.Length > 0)
			{
				processData(text);
			}
			dataProcessing = false;
		}
	}

	private void processData(string rxString)
	{
		UWave uWave = new UWave(rxString);
		string chanel = uWave.ChanelId;
		if (!string.IsNullOrEmpty(chanel))
		{
			cbbChanel.Invoke((MethodInvoker)delegate
			{
				cbbChanel.Text = chanel;
			});
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
				if (dataGridView.CurrentRow.Cells["ChanelId"].Value != null)
				{
					cbbChanel.Text = dataGridView.CurrentRow.Cells["ChanelId"].Value.ToString();
				}
				cbbMachine.SelectedValue = mId;
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
			if (item.Cells["ChanelId"].Value != null && !string.IsNullOrEmpty(item.Cells["ChanelId"].Value.ToString()))
			{
				item.DefaultCellStyle.ForeColor = Color.Blue;
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

	private void main_deleteToolStripMenuItem_Click(object sender, EventArgs e)
	{
		mSearchMain.btnSearch.Select();
		if (mId.Equals(Guid.Empty))
		{
			MessageBox.Show("Please select a row.", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
		else if (MessageBox.Show("You sure to delete machine:\r\n* Name: " + dgvMain.CurrentRow.Cells["Name"].Value, "QUESTION", MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.Yes))
		{
			Cursor.Current = Cursors.WaitCursor;
			start_Proccessor();
			btnStop.PerformClick();
			DataTable dataTable = mSearchMain.getDataTable();
			dataTable.Select($"Id='{mId}'").FirstOrDefault()?.SetField<string>("ChanelId", null);
			WriteConfig();
			main_refreshToolStripMenuItem.PerformClick();
		}
	}

	private void btnStart_Click(object sender, EventArgs e)
	{
		ClosePort();
		if (mService.Status == ServiceControllerStatus.Stopped)
		{
			mService.Start();
			TimeSpan timeout = new TimeSpan(0, 0, 5);
			mService.WaitForStatus(ServiceControllerStatus.Running, timeout);
		}
		UpdateButton();
	}

	private void btnStop_Click(object sender, EventArgs e)
	{
		if (mService.Status == ServiceControllerStatus.Running)
		{
			mService.Stop();
			TimeSpan timeout = new TimeSpan(0, 0, 5);
			mService.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
		}
		UpdateButton();
		OpenPort();
	}

	private void cbbNormal_Leave(object sender, EventArgs e)
	{
		ComboBox comboBox = sender as ComboBox;
		if (comboBox.SelectedIndex.Equals(-1) && !string.IsNullOrEmpty(comboBox.Text))
		{
			comboBox.Text = mRemember;
		}
	}

	private void cbbNormal_Enter(object sender, EventArgs e)
	{
		ComboBox comboBox = sender as ComboBox;
		mRemember = comboBox.Text;
	}

	private void btnComfirm_Click(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		if (cbbMachine.SelectedIndex.Equals(-1))
		{
			MessageBox.Show("Please select machine.", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			cbbMachine.Focus();
			return;
		}
		if (!string.IsNullOrEmpty(cbbChanel.Text))
		{
			foreach (DataRow row in mSearchMain.getDataTable().Rows)
			{
				Guid.TryParse(cbbMachine.SelectedValue.ToString(), out var result);
				if (row.Field<Guid>("Id").Equals(result))
				{
					row.SetField("ChanelId", cbbChanel.Text);
				}
				else if (!string.IsNullOrEmpty(row.Field<string>("ChanelId")) && row.Field<string>("ChanelId").Equals(cbbChanel.Text))
				{
					row.SetField<string>("ChanelId", null);
				}
			}
		}
		WriteConfig();
		main_refreshToolStripMenuItem.PerformClick();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_5SQA_Config_UWave.frmMain));
		this.toolTipfrmMain = new System.Windows.Forms.ToolTip(this.components);
		this.btnStart = new System.Windows.Forms.Button();
		this.btnStop = new System.Windows.Forms.Button();
		this.cbbChanel = new System.Windows.Forms.ComboBox();
		this.cbbMachine = new System.Windows.Forms.ComboBox();
		this.btnComfirm = new System.Windows.Forms.Button();
		this.contextMenuStripMain = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.main_refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.main_deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.statusStripfrmMain = new System.Windows.Forms.StatusStrip();
		this.sprogbarStatus = new System.Windows.Forms.ToolStripProgressBar();
		this.slblStatus = new System.Windows.Forms.ToolStripStatusLabel();
		this.dgvMain = new System.Windows.Forms.DataGridView();
		this.No = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ChanelId = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Model = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Serial = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.FactoryName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.MachineTypeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Created = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Modified = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.CreatedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ModifiedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.IsActivated = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
		this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
		this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
		this.metroLabel13 = new MetroFramework.Controls.MetroLabel();
		this.mPanelViewMain = new _5SQA_Config_UWave.mPanelView();
		this.mSearchMain = new _5SQA_Config_UWave.mSearch();
		this.contextMenuStripMain.SuspendLayout();
		this.statusStripfrmMain.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvMain).BeginInit();
		this.tableLayoutPanel1.SuspendLayout();
		this.tableLayoutPanel2.SuspendLayout();
		base.SuspendLayout();
		this.btnStart.BackColor = System.Drawing.Color.Green;
		this.btnStart.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnStart.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnStart.FlatAppearance.BorderSize = 0;
		this.btnStart.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
		this.btnStart.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
		this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnStart.ForeColor = System.Drawing.Color.White;
		this.btnStart.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnStart.Location = new System.Drawing.Point(0, 0);
		this.btnStart.Margin = new System.Windows.Forms.Padding(0);
		this.btnStart.Name = "btnStart";
		this.btnStart.Size = new System.Drawing.Size(438, 30);
		this.btnStart.TabIndex = 2;
		this.btnStart.Text = "Start connection";
		this.toolTipfrmMain.SetToolTip(this.btnStart, "Select start connection");
		this.btnStart.UseVisualStyleBackColor = false;
		this.btnStart.Click += new System.EventHandler(btnStart_Click);
		this.btnStop.BackColor = System.Drawing.Color.Red;
		this.btnStop.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnStop.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnStop.FlatAppearance.BorderSize = 0;
		this.btnStop.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
		this.btnStop.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
		this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnStop.ForeColor = System.Drawing.Color.White;
		this.btnStop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnStop.Location = new System.Drawing.Point(438, 0);
		this.btnStop.Margin = new System.Windows.Forms.Padding(0);
		this.btnStop.Name = "btnStop";
		this.btnStop.Size = new System.Drawing.Size(438, 30);
		this.btnStop.TabIndex = 3;
		this.btnStop.Text = "Stop connection";
		this.toolTipfrmMain.SetToolTip(this.btnStop, "Select stop connection");
		this.btnStop.UseVisualStyleBackColor = false;
		this.btnStop.Click += new System.EventHandler(btnStop_Click);
		this.cbbChanel.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cbbChanel.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cbbChanel.Dock = System.Windows.Forms.DockStyle.Fill;
		this.cbbChanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cbbChanel.FormattingEnabled = true;
		this.cbbChanel.ItemHeight = 18;
		this.cbbChanel.Items.AddRange(new object[100]
		{
			"00", "01", "02", "03", "04", "05", "06", "07", "08", "09",
			"10", "11", "12", "13", "14", "15", "16", "17", "18", "19",
			"20", "21", "22", "23", "24", "25", "26", "27", "28", "29",
			"30", "31", "32", "33", "34", "35", "36", "37", "38", "39",
			"40", "41", "42", "43", "44", "45", "46", "47", "48", "49",
			"50", "51", "52", "53", "54", "55", "56", "57", "58", "59",
			"60", "61", "62", "63", "64", "65", "66", "67", "68", "69",
			"70", "71", "72", "73", "74", "75", "76", "77", "78", "79",
			"80", "81", "82", "83", "84", "85", "86", "87", "88", "89",
			"90", "91", "92", "93", "94", "95", "96", "97", "98", "99"
		});
		this.cbbChanel.Location = new System.Drawing.Point(82, 3);
		this.cbbChanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.cbbChanel.Name = "cbbChanel";
		this.cbbChanel.Size = new System.Drawing.Size(302, 26);
		this.cbbChanel.TabIndex = 75;
		this.toolTipfrmMain.SetToolTip(this.cbbChanel, "Select or enter channel id");
		this.cbbChanel.Enter += new System.EventHandler(cbbNormal_Enter);
		this.cbbChanel.Leave += new System.EventHandler(cbbNormal_Leave);
		this.cbbMachine.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cbbMachine.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cbbMachine.Dock = System.Windows.Forms.DockStyle.Fill;
		this.cbbMachine.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cbbMachine.FormattingEnabled = true;
		this.cbbMachine.ItemHeight = 18;
		this.cbbMachine.Location = new System.Drawing.Point(463, 3);
		this.cbbMachine.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.cbbMachine.Name = "cbbMachine";
		this.cbbMachine.Size = new System.Drawing.Size(302, 26);
		this.cbbMachine.TabIndex = 77;
		this.toolTipfrmMain.SetToolTip(this.cbbMachine, "Select or enter machine");
		this.cbbMachine.Enter += new System.EventHandler(cbbNormal_Enter);
		this.cbbMachine.Leave += new System.EventHandler(cbbNormal_Leave);
		this.btnComfirm.BackColor = System.Drawing.Color.Green;
		this.btnComfirm.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnComfirm.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.btnComfirm.FlatAppearance.BorderSize = 0;
		this.btnComfirm.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
		this.btnComfirm.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
		this.btnComfirm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnComfirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnComfirm.ForeColor = System.Drawing.Color.White;
		this.btnComfirm.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnComfirm.Location = new System.Drawing.Point(772, 4);
		this.btnComfirm.Name = "btnComfirm";
		this.btnComfirm.Size = new System.Drawing.Size(100, 26);
		this.btnComfirm.TabIndex = 78;
		this.btnComfirm.Text = "Confirm";
		this.toolTipfrmMain.SetToolTip(this.btnComfirm, "Confirm");
		this.btnComfirm.UseVisualStyleBackColor = false;
		this.btnComfirm.Click += new System.EventHandler(btnComfirm_Click);
		this.contextMenuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.main_refreshToolStripMenuItem, this.toolStripSeparator6, this.main_deleteToolStripMenuItem });
		this.contextMenuStripMain.Name = "contextMenuStripStaff";
		this.contextMenuStripMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
		this.contextMenuStripMain.Size = new System.Drawing.Size(114, 54);
		this.main_refreshToolStripMenuItem.Name = "main_refreshToolStripMenuItem";
		this.main_refreshToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
		this.main_refreshToolStripMenuItem.Text = "Refresh";
		this.main_refreshToolStripMenuItem.Click += new System.EventHandler(main_refreshToolStripMenuItem_Click);
		this.toolStripSeparator6.Name = "toolStripSeparator6";
		this.toolStripSeparator6.Size = new System.Drawing.Size(110, 6);
		this.main_deleteToolStripMenuItem.Name = "main_deleteToolStripMenuItem";
		this.main_deleteToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
		this.main_deleteToolStripMenuItem.Text = "Delete";
		this.main_deleteToolStripMenuItem.Click += new System.EventHandler(main_deleteToolStripMenuItem_Click);
		this.statusStripfrmMain.BackColor = System.Drawing.SystemColors.ControlLight;
		this.statusStripfrmMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.sprogbarStatus, this.slblStatus });
		this.statusStripfrmMain.Location = new System.Drawing.Point(4, 431);
		this.statusStripfrmMain.Name = "statusStripfrmMain";
		this.statusStripfrmMain.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
		this.statusStripfrmMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
		this.statusStripfrmMain.Size = new System.Drawing.Size(876, 26);
		this.statusStripfrmMain.SizingGrip = false;
		this.statusStripfrmMain.Stretch = false;
		this.statusStripfrmMain.TabIndex = 7;
		this.sprogbarStatus.Name = "sprogbarStatus";
		this.sprogbarStatus.Size = new System.Drawing.Size(150, 20);
		this.slblStatus.Name = "slblStatus";
		this.slblStatus.Size = new System.Drawing.Size(16, 21);
		this.slblStatus.Text = "...";
		this.dgvMain.AllowUserToAddRows = false;
		this.dgvMain.AllowUserToDeleteRows = false;
		this.dgvMain.AllowUserToOrderColumns = true;
		this.dgvMain.AllowUserToResizeRows = false;
		dataGridViewCellStyle.BackColor = System.Drawing.Color.LightGray;
		this.dgvMain.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle;
		dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle2.BackColor = System.Drawing.Color.DimGray;
		dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
		dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
		dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
		dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.dgvMain.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
		this.dgvMain.ColumnHeadersHeight = 30;
		this.dgvMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
		this.dgvMain.Columns.AddRange(this.No, this.ChanelId, this.Code, this.name, this.Model, this.Serial, this.FactoryName, this.MachineTypeName, this.Status, this.Id, this.Created, this.Modified, this.CreatedBy, this.ModifiedBy, this.IsActivated);
		this.dgvMain.ContextMenuStrip = this.contextMenuStripMain;
		this.dgvMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.dgvMain.EnableHeadersVisualStyles = false;
		this.dgvMain.Location = new System.Drawing.Point(4, 94);
		this.dgvMain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.dgvMain.MultiSelect = false;
		this.dgvMain.Name = "dgvMain";
		this.dgvMain.ReadOnly = true;
		this.dgvMain.RowHeadersWidth = 25;
		this.dgvMain.Size = new System.Drawing.Size(876, 303);
		this.dgvMain.TabIndex = 11;
		this.dgvMain.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvMain_CellClick);
		this.dgvMain.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvMain_CellDoubleClick);
		this.dgvMain.CurrentCellChanged += new System.EventHandler(dgvMain_CurrentCellChanged);
		this.dgvMain.Sorted += new System.EventHandler(dgvMain_Sorted);
		this.No.DataPropertyName = "No";
		dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		this.No.DefaultCellStyle = dataGridViewCellStyle3;
		this.No.HeaderText = "No.";
		this.No.Name = "No";
		this.No.ReadOnly = true;
		this.No.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.No.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.No.Width = 40;
		this.ChanelId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.ChanelId.DataPropertyName = "ChanelId";
		this.ChanelId.FillWeight = 20f;
		this.ChanelId.HeaderText = "Chanel id";
		this.ChanelId.Name = "ChanelId";
		this.ChanelId.ReadOnly = true;
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
		this.Model.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Model.DataPropertyName = "Model";
		this.Model.FillWeight = 30f;
		this.Model.HeaderText = "Model";
		this.Model.Name = "Model";
		this.Model.ReadOnly = true;
		this.Serial.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Serial.DataPropertyName = "Serial";
		this.Serial.FillWeight = 30f;
		this.Serial.HeaderText = "Serial";
		this.Serial.Name = "Serial";
		this.Serial.ReadOnly = true;
		this.FactoryName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.FactoryName.DataPropertyName = "FactoryName";
		this.FactoryName.FillWeight = 30f;
		this.FactoryName.HeaderText = "Factory";
		this.FactoryName.Name = "FactoryName";
		this.FactoryName.ReadOnly = true;
		this.MachineTypeName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.MachineTypeName.DataPropertyName = "MachineTypeName";
		this.MachineTypeName.FillWeight = 30f;
		this.MachineTypeName.HeaderText = "Machine type";
		this.MachineTypeName.Name = "MachineTypeName";
		this.MachineTypeName.ReadOnly = true;
		this.Status.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Status.DataPropertyName = "Status";
		this.Status.FillWeight = 20f;
		this.Status.HeaderText = "Status";
		this.Status.Name = "Status";
		this.Status.ReadOnly = true;
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
		this.tableLayoutPanel1.AutoSize = true;
		this.tableLayoutPanel1.ColumnCount = 2;
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tableLayoutPanel1.Controls.Add(this.btnStop, 0, 0);
		this.tableLayoutPanel1.Controls.Add(this.btnStart, 0, 0);
		this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.tableLayoutPanel1.Location = new System.Drawing.Point(4, 4);
		this.tableLayoutPanel1.Name = "tableLayoutPanel1";
		this.tableLayoutPanel1.RowCount = 1;
		this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel1.Size = new System.Drawing.Size(876, 30);
		this.tableLayoutPanel1.TabIndex = 12;
		this.tableLayoutPanel2.AutoSize = true;
		this.tableLayoutPanel2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		this.tableLayoutPanel2.ColumnCount = 5;
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel2.Controls.Add(this.btnComfirm, 4, 0);
		this.tableLayoutPanel2.Controls.Add(this.cbbMachine, 3, 0);
		this.tableLayoutPanel2.Controls.Add(this.metroLabel1, 2, 0);
		this.tableLayoutPanel2.Controls.Add(this.cbbChanel, 1, 0);
		this.tableLayoutPanel2.Controls.Add(this.metroLabel13, 0, 0);
		this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.tableLayoutPanel2.Location = new System.Drawing.Point(4, 397);
		this.tableLayoutPanel2.Name = "tableLayoutPanel2";
		this.tableLayoutPanel2.RowCount = 1;
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel2.Size = new System.Drawing.Size(876, 34);
		this.tableLayoutPanel2.TabIndex = 13;
		this.metroLabel1.AutoSize = true;
		this.metroLabel1.BackColor = System.Drawing.SystemColors.Control;
		this.metroLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.metroLabel1.FontWeight = MetroFramework.MetroLabelWeight.Bold;
		this.metroLabel1.Location = new System.Drawing.Point(391, 1);
		this.metroLabel1.Name = "metroLabel1";
		this.metroLabel1.Size = new System.Drawing.Size(65, 32);
		this.metroLabel1.TabIndex = 76;
		this.metroLabel1.Text = "Machine";
		this.metroLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.metroLabel1.UseCustomBackColor = true;
		this.metroLabel13.AutoSize = true;
		this.metroLabel13.BackColor = System.Drawing.SystemColors.Control;
		this.metroLabel13.Dock = System.Windows.Forms.DockStyle.Fill;
		this.metroLabel13.FontWeight = MetroFramework.MetroLabelWeight.Bold;
		this.metroLabel13.Location = new System.Drawing.Point(4, 1);
		this.metroLabel13.Name = "metroLabel13";
		this.metroLabel13.Size = new System.Drawing.Size(71, 32);
		this.metroLabel13.TabIndex = 69;
		this.metroLabel13.Text = "Chanel id";
		this.metroLabel13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.metroLabel13.UseCustomBackColor = true;
		this.mPanelViewMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.mPanelViewMain.Dock = System.Windows.Forms.DockStyle.Right;
		this.mPanelViewMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.mPanelViewMain.Location = new System.Drawing.Point(480, 94);
		this.mPanelViewMain.Margin = new System.Windows.Forms.Padding(0);
		this.mPanelViewMain.Name = "mPanelViewMain";
		this.mPanelViewMain.Size = new System.Drawing.Size(400, 303);
		this.mPanelViewMain.TabIndex = 10;
		this.mSearchMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.mSearchMain.Dock = System.Windows.Forms.DockStyle.Top;
		this.mSearchMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.mSearchMain.Location = new System.Drawing.Point(4, 34);
		this.mSearchMain.Margin = new System.Windows.Forms.Padding(4);
		this.mSearchMain.Name = "mSearchMain";
		this.mSearchMain.Padding = new System.Windows.Forms.Padding(4);
		this.mSearchMain.Size = new System.Drawing.Size(876, 60);
		this.mSearchMain.TabIndex = 8;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(884, 461);
		base.Controls.Add(this.mPanelViewMain);
		base.Controls.Add(this.dgvMain);
		base.Controls.Add(this.mSearchMain);
		base.Controls.Add(this.tableLayoutPanel1);
		base.Controls.Add(this.tableLayoutPanel2);
		base.Controls.Add(this.statusStripfrmMain);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Margin = new System.Windows.Forms.Padding(4);
		this.MinimumSize = new System.Drawing.Size(500, 200);
		base.Name = "frmMain";
		base.Padding = new System.Windows.Forms.Padding(4);
		base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "5SQA Config U_Wave * Copyright @2022- v1.0.0";
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmMain_FormClosed);
		base.Load += new System.EventHandler(frmMain_Load);
		base.Shown += new System.EventHandler(frmMain_Shown);
		this.contextMenuStripMain.ResumeLayout(false);
		this.statusStripfrmMain.ResumeLayout(false);
		this.statusStripfrmMain.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvMain).EndInit();
		this.tableLayoutPanel1.ResumeLayout(false);
		this.tableLayoutPanel2.ResumeLayout(false);
		this.tableLayoutPanel2.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
