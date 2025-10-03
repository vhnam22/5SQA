using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using _5S_QA_Client.Controls;
using _5S_QA_Client.Properties;
using _5S_QA_Client.View.User_control;
using _5S_QA_DL.APIContext;
using _5S_QA_Entities.Constants;
using _5S_QA_Entities.Dtos;
using _5S_QA_Entities.Models;
using MetroFramework;
using MetroFramework.Controls;
using MetroFramework.Forms;
using Newtonsoft.Json;
using WebSocketSharp;

namespace _5S_QA_Client;

public class frmLogin : MetroForm
{
	public static APIClient client;

	public static string mMachineName = ConfigurationManager.ConnectionStrings["MachineName"].ConnectionString;

	public static string mTimeSample = ConfigurationManager.ConnectionStrings["TimeSample"].ConnectionString;

	private Guid mId;

	public static Guid mIdOpen = Guid.Empty;

	public static bool isEdit;

	private int mRow;

	private int mCol;

	private WebSocket mWs;

	private System.Timers.Timer mTimer;

	public bool isThis = false;

	public List<MachineLienceViewModel> mMachines;

	private IContainer components = null;

	private ToolTip toolTipMain;

	private Panel panel2;

	private MetroCheckBox cbRemember;

	private PictureBox pictureBox1;

	private MetroLabel lblVersion;

	private MetroTextBox.MetroTextButton btnLogin;

	private MetroTextBox txtPassword;

	private MetroTextBox txtUsername;

	private Panel panelLogin;

	private Panel panelUIMain;

	private ContextMenuStrip contextMenuStripMain;

	private ToolStripMenuItem changePasswordToolStripMenuItem;

	private StatusStrip statusStripfrmMain;

	private ToolStripProgressBar sprogbarStatus;

	public ToolStripStatusLabel slblStatus;

	private ContextMenuStrip contextMenuStripRequest;

	private ToolStripMenuItem main_refreshToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripMenuItem main_resultToolStripMenuItem;

	private ToolStripMenuItem main_completeToolStripMenuItem;

	private mSearch mSearchMain;

	private mPanelViewExport mPanelViewMain;

	private PictureBox ptbAvatar;

	private Panel panelLogout;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripMenuItem getIMEToolStripMenuItem;

	private DataGridView dgvMain;

	public Button btnLogout;

	private Label lblFullname;

	private TableLayoutPanel tableLayoutPanel1;

	private TableLayoutPanel tpanelButton;

	private Button btnView;

	private Button btnRefresh;

	private Button btnComplete;

	private Button btnResult;

	private ToolStripMenuItem main_viewToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator3;

	private TableLayoutPanel tpanelLanguage;

	private RadioButton rbJP;

	private RadioButton rbEN;

	private RadioButton rbVN;

	private RadioButton rbTW;

	private Button btnHelp;

	private Label lblRealtime;

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

	public static AuthUserViewModel User { get; set; }

	public frmLogin()
	{
		InitializeComponent();
		Common.setControls(this, toolTipMain, new List<ContextMenuStrip> { contextMenuStripMain, contextMenuStripRequest });
		client = new APIClient
		{
			IME = (string.IsNullOrEmpty(GetIME().Trim()) ? "null#UWave" : (GetIME() + "#UWave"))
		};
	}

	private void frmLogin_Load(object sender, EventArgs e)
	{
		Version version = Assembly.GetEntryAssembly().GetName().Version;
		lblVersion.Text = "(IME: " + client.IME + ")               " + version.ToString(3);
		loadSettings();
	}

	private void frmLogin_Shown(object sender, EventArgs e)
	{
		Init();
	}

	private void frmLogin_FormClosed(object sender, FormClosedEventArgs e)
	{
		StopSocket();
		mPanelViewMain.mDispose();
		if (client != null)
		{
			try
			{
				if (panelLogout.Visible)
				{
					client.LogoutAsync(User.Id);
				}
			}
			catch
			{
			}
			client.CloseAPI();
		}
		GC.Collect();
	}

	private void Init()
	{
		Text = Text + " (" + mMachineName + ")";
		enPanelUIView(en: false);
		mSearchMain.btnSearch.Click += btnSearch_Click;
		mSearchMain.btnAdd.Click += btnAdd_Click;
		mSearchMain.btnBarcode.Click += btnBarcode_Click;
		mPanelViewMain.btnComplete.Click += main_completeToolStripMenuItem_Click;
		mPanelViewMain.btnResult.Click += main_resultToolStripMenuItem_Click;
		mSearchMain.dtpMain.ValueChanged += dtpMain_ValueChanged;
		cbRemember.CheckedChanged += cbRememberPassword_CheckedChanged;
		mSearchMain.cbRejected.CheckedChanged += dtpMain_ValueChanged;
		txtUsername.Select();
		mMachines = new List<MachineLienceViewModel>();
		Init_Timer();
		Init_WebSocket();
	}

	private void Init_Timer()
	{
		mTimer = new System.Timers.Timer
		{
			Interval = 5000.0
		};
		mTimer.Elapsed += timer_Elapsed;
	}

	private void Init_WebSocket()
	{
		mWs = new WebSocket(APIUrl.APIHost.Replace("http", "ws") + "/ws");
		mWs.OnMessage += Ws_OnMessage;
		mWs.OnClose += Ws_OnClose;
		mWs.OnOpen += Ws_OnOpen;
	}

	private void Ws_OnOpen(object sender, EventArgs e)
	{
		Set_lblRealtime();
	}

	private void Ws_OnClose(object sender, CloseEventArgs e)
	{
		Set_lblRealtime(isconnect: false);
	}

	private void Ws_OnMessage(object sender, MessageEventArgs e)
	{
		if (isThis)
		{
			isThis = false;
		}
		else
		{
			RefreshFromSocket(e.Data);
		}
	}

	private void StartSocket()
	{
		if (mWs != null && mWs.ReadyState != WebSocketState.Open)
		{
			mWs.Connect();
		}
	}

	private void StopSocket()
	{
		if (mWs != null && mWs.ReadyState == WebSocketState.Open)
		{
			mWs.Close();
		}
	}

	private void RefreshFromSocket(string json)
	{
		if (!json.StartsWith("{") || !json.EndsWith("}"))
		{
			return;
		}
		RequestViewModel item = JsonConvert.DeserializeObject<RequestViewModel>(json);
		if (item == null)
		{
			return;
		}
		_ = item.Id;
		
		Invoke((MethodInvoker)delegate
		{
			if (item.Id == mIdOpen)
			{
				Common.closeForm(new List<Type>
				{
					typeof(frmResultView),
					typeof(frmCompleteView)
				});
				if ((item.Status.Contains("Activated") || item.Status.Contains("Rejected")) && item.ProductCavity.HasValue)
				{
					mIdOpen = item.Id;
					new frmResultView(this, item).Show();
				}
			}
			string text = "yyyy/MM";
			if (!mSearchMain.cbRejected.Checked)
			{
				text += "/dd";
			}
			if (item.Date.Value.ToString(text) == mSearchMain.dtpMain.Value.ToString(text))
			{
				main_refreshToolStripMenuItem.PerformClick();
			}
		});
	}

	private void Set_lblRealtime(bool isconnect = true)
	{
		Invoke((MethodInvoker)delegate
		{
			if (isconnect)
			{
				mTimer.Stop();
				lblRealtime.Text = "Realtime";
			}
			else
			{
				if (panelLogout.Visible)
				{
					mTimer.Start();
				}
				lblRealtime.Text = "";
			}
		});
	}

	private void loadSettings()
	{
		Control control = tpanelLanguage.Controls[Settings.Default.Language];
		if (control != null)
		{
			((RadioButton)control).Checked = true;
		}
		else
		{
			rbEN.Checked = true;
		}
		cbRemember.Checked = Settings.Default.RememberPassword;
		if (cbRemember.Checked)
		{
			txtUsername.Text = Settings.Default.Username;
			txtPassword.Text = Settings.Default.Password;
		}
	}

	private void saveSettings()
	{
		Settings.Default.RememberPassword = cbRemember.Checked;
		if (cbRemember.Checked)
		{
			Settings.Default.Username = txtUsername.Text;
			Settings.Default.Password = txtPassword.Text;
		}
		Settings.Default.Save();
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
				dgvMain_CellDoubleClick(dgvMain, null);
				return true;
			}
			if (keyData.Equals(Keys.Escape))
			{
				dgvMain_CellClick(dgvMain, null);
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
			queryArgs.Predicate = "Date=@0 && (Status=@1 || Status.Contains(@2))";
			queryArgs.PredicateParameters = new string[3]
			{
				mSearchMain.dtpMain.Value.ToString("MM/dd/yyyy"),
				"Activated",
				"Rejected"
			};
			queryArgs.Order = "Created DESC";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs queryArgs2 = queryArgs;
			if (mSearchMain.cbRejected.Checked)
			{
				queryArgs2.Predicate = "Date>=@0 && Date<@1 && Status.Contains(@2)";
				queryArgs2.PredicateParameters = new List<object>
				{
					mSearchMain.dtpMain.Value.ToString("yyyy/MM/01"),
					mSearchMain.dtpMain.Value.AddMonths(1).ToString("yyyy/MM/01"),
					"Rejected"
				};
			}
			ResponseDto result = client.GetsAsync(queryArgs2, "/api/Request/Gets").Result;
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
						btnLogout.PerformClick();
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

	private void enPanelUIView(bool en)
	{
		panelLogin.Visible = !en;
		panelLogout.Visible = en;
		panelUIMain.Visible = en;
	}

	private string GetIME()
	{
		try
		{
			ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("Select * From Win32_BIOS");
			using (IEnumerator<ManagementObject> enumerator = managementObjectSearcher.Get().Cast<ManagementObject>().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					ManagementObject current = enumerator.Current;
					return current["SerialNumber"].ToString();
				}
			}
			return "ERROR";
		}
		catch
		{
			return "ERROR";
		}
	}

	private void LoadLiences()
	{
		new Task(delegate
		{
			Cursor.Current = Cursors.WaitCursor;
			try
			{
				ResponseDto result = client.GetsAsync(client.IME, "/api/Machine/GetLiences/{id}").Result;
				mMachines = Common.getObjects<MachineLienceViewModel>(result);
			}
			catch
			{
			}
		}).Start();
	}

	private void timer_Elapsed(object sender, ElapsedEventArgs e)
	{
		StartSocket();
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
			Guid guid = Guid.Parse(dataGridView.CurrentRow.Cells["Id"].Value.ToString());
			if (!guid.Equals(mId))
			{
				mId = guid;
				mPanelViewMain.Display();
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
		main_resultToolStripMenuItem.PerformClick();
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

	private void main_refreshToolStripMenuItem_Click(object sender, EventArgs e)
	{
		mSearchMain.btnSearch.Select();
		load_AllData();
	}

	private void main_viewToolStripMenuItem_Click(object sender, EventArgs e)
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

	private void main_resultToolStripMenuItem_Click(object sender, EventArgs e)
	{
		mSearchMain.btnSearch.Select();
		Cursor.Current = Cursors.WaitCursor;
		if (mId.Equals(Guid.Empty))
		{
			MessageBox.Show(Common.getTextLanguage(this, "wSelectRow"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		RequestViewModel requestViewModel = (from DataRow x in mSearchMain.dataTable.Rows
			where x.Field<Guid>("Id") == mId
			select new RequestViewModel
			{
				Id = x.Field<Guid>("Id"),
				Date = x.Field<DateTimeOffset>("Date"),
				ProductId = x.Field<Guid>("ProductId"),
				ProductImageUrl = x.Field<string>("ProductImageUrl"),
				Code = x.Field<string>("Code"),
				Name = x.Field<string>("Name"),
				Sample = x.Field<int>("Sample"),
				ProductCavity = x.Field<int>("ProductCavity"),
				Status = x.Field<string>("Status"),
				Quantity = x.Field<int>("Quantity"),
				Type = x.Field<string>("Type")
			}).FirstOrDefault();
		mIdOpen = requestViewModel.Id;
		new frmResultView(this, requestViewModel).Show();
	}

	private void main_completeToolStripMenuItem_Click(object sender, EventArgs e)
	{
		mSearchMain.btnSearch.Select();
		if (mId.Equals(Guid.Empty))
		{
			MessageBox.Show(Common.getTextLanguage(this, "wSelectRow"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		RequestViewModel request = (from DataRow x in mSearchMain.dataTable.Rows
			where x.Field<Guid>("Id") == mId
			select new RequestViewModel
			{
				Id = x.Field<Guid>("Id"),
				Date = x.Field<DateTimeOffset>("Date"),
				ProductId = x.Field<Guid>("ProductId"),
				ProductImageUrl = x.Field<string>("ProductImageUrl"),
				Code = x.Field<string>("Code"),
				Name = x.Field<string>("Name"),
				Sample = x.Field<int>("Sample"),
				ProductCavity = x.Field<int>("ProductCavity"),
				Quantity = x.Field<int>("Quantity")
			}).FirstOrDefault();
		new frmCompleteView(this, request).ShowDialog();
	}

	private void cbRememberPassword_CheckedChanged(object sender, EventArgs e)
	{
		saveSettings();
	}

	private void btnLogin_Click(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		try
		{
			LoginDto body = new LoginDto
			{
				Username = txtUsername.Text,
				Password = txtPassword.Text
			};
			ResponseDto result = client.LoginAsync(body).Result;
			if (!result.Success)
			{
				throw new Exception(result.Messages.ElementAt(0).Message);
			}
			string value = result.Data.ToString();
			User = JsonConvert.DeserializeObject<AuthUserViewModel>(value);
			client.Token = User.Token;
			saveSettings();
			lblFullname.Text = User.FullName;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(APIUrl.APIHost + "/AuthUserImage/").Append(User.ImageUrl);
			try
			{
				ptbAvatar.Load(stringBuilder.ToString());
			}
			catch
			{
				ptbAvatar.Image = Resources._5S_QA_C;
			}
			enPanelUIView(en: true);
			mSearchMain.dtpMain.Value = DateTime.Now;
			StartSocket();
			LoadLiences();
		}
		catch (Exception ex)
		{
			string textLanguage = Common.getTextLanguage("Error", ex.Message);
			MessageBox.Show(string.IsNullOrEmpty(textLanguage) ? ex.Message : textLanguage, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			txtPassword.Focus();
		}
	}

	private void txtUsername_KeyPress(object sender, KeyPressEventArgs e)
	{
		if (e.KeyChar == '\r')
		{
			btnLogin.PerformClick();
		}
	}

	private void btnLogout_Click(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		try
		{
			client.LogoutAsync(User.Id);
		}
		catch
		{
		}
		mPanelViewMain.mDispose();
		enPanelUIView(en: false);
		client.Token = "";
		StopSocket();
		GC.Collect();
	}

	private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		new frmChangePassword().ShowDialog();
	}

	private void getIMEToolStripMenuItem_Click(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		new frmIME().ShowDialog();
	}

	private void btnAdd_Click(object sender, EventArgs e)
	{
		mSearchMain.btnSearch.Select();
		Cursor.Current = Cursors.WaitCursor;
		new frmRequestAdd(this, (DataTable)dgvMain.DataSource, mId).ShowDialog();
	}

	private void btnBarcode_Click(object sender, EventArgs e)
	{
		mSearchMain.btnSearch.Select();
		Cursor.Current = Cursors.WaitCursor;
		new frmBarcodeView(this, mSearchMain.dtpMain.Value).ShowDialog();
	}

	private void btnView_Click(object sender, EventArgs e)
	{
		main_viewToolStripMenuItem.PerformClick();
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		main_refreshToolStripMenuItem.PerformClick();
	}

	private void btnResult_Click(object sender, EventArgs e)
	{
		main_resultToolStripMenuItem.PerformClick();
	}

	private void btnComplete_Click(object sender, EventArgs e)
	{
		main_completeToolStripMenuItem.PerformClick();
	}

	private void rbLanguage_CheckedChanged(object sender, EventArgs e)
	{
		RadioButton radioButton = sender as RadioButton;
		if (!(Settings.Default.Language == radioButton.Name))
		{
			Settings.Default.Language = radioButton.Name;
			Settings.Default.Save();
			Common.setControls(this, toolTipMain, new List<ContextMenuStrip> { contextMenuStripMain, contextMenuStripRequest });
			mSearchMain.Update_Language();
			mPanelViewMain.Update_Language();
		}
	}

	private void btnHelp_Click(object sender, EventArgs e)
	{
		Directory.CreateDirectory(".\\Manuals");
		string path = Assembly.GetEntryAssembly().GetName().Name + "_" + Settings.Default.Language.Replace("rb", "") + ".pdf";
		path = Path.Combine(".\\Manuals", path);
		if (File.Exists(path))
		{
			Common.ExecuteBatFile(path);
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_5S_QA_Client.frmLogin));
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.cbRemember = new MetroFramework.Controls.MetroCheckBox();
		this.btnLogout = new System.Windows.Forms.Button();
		this.btnView = new System.Windows.Forms.Button();
		this.btnResult = new System.Windows.Forms.Button();
		this.btnComplete = new System.Windows.Forms.Button();
		this.btnRefresh = new System.Windows.Forms.Button();
		this.txtUsername = new MetroFramework.Controls.MetroTextBox();
		this.rbEN = new System.Windows.Forms.RadioButton();
		this.btnLogin = new MetroFramework.Controls.MetroTextBox.MetroTextButton();
		this.txtPassword = new MetroFramework.Controls.MetroTextBox();
		this.rbJP = new System.Windows.Forms.RadioButton();
		this.rbVN = new System.Windows.Forms.RadioButton();
		this.rbTW = new System.Windows.Forms.RadioButton();
		this.btnHelp = new System.Windows.Forms.Button();
		this.panel2 = new System.Windows.Forms.Panel();
		this.lblVersion = new MetroFramework.Controls.MetroLabel();
		this.panelLogin = new System.Windows.Forms.Panel();
		this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.contextMenuStripMain = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.changePasswordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.getIMEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.tpanelLanguage = new System.Windows.Forms.TableLayoutPanel();
		this.panelUIMain = new System.Windows.Forms.Panel();
		this.mPanelViewMain = new _5S_QA_Client.View.User_control.mPanelViewExport();
		this.dgvMain = new System.Windows.Forms.DataGridView();
		this.contextMenuStripRequest = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.main_refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.main_viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.main_resultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.main_completeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.statusStripfrmMain = new System.Windows.Forms.StatusStrip();
		this.sprogbarStatus = new System.Windows.Forms.ToolStripProgressBar();
		this.slblStatus = new System.Windows.Forms.ToolStripStatusLabel();
		this.tpanelButton = new System.Windows.Forms.TableLayoutPanel();
		this.mSearchMain = new _5S_QA_Client.View.User_control.mSearch();
		this.panelLogout = new System.Windows.Forms.Panel();
		this.lblRealtime = new System.Windows.Forms.Label();
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
		this.panelLogin.SuspendLayout();
		this.tableLayoutPanel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		this.contextMenuStripMain.SuspendLayout();
		this.tpanelLanguage.SuspendLayout();
		this.panelUIMain.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvMain).BeginInit();
		this.contextMenuStripRequest.SuspendLayout();
		this.statusStripfrmMain.SuspendLayout();
		this.tpanelButton.SuspendLayout();
		this.panelLogout.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.ptbAvatar).BeginInit();
		base.SuspendLayout();
		resources.ApplyResources(this.cbRemember, "cbRemember");
		this.cbRemember.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbRemember.Name = "cbRemember";
		this.cbRemember.Style = MetroFramework.MetroColorStyle.Blue;
		this.toolTipMain.SetToolTip(this.cbRemember, resources.GetString("cbRemember.ToolTip"));
		this.cbRemember.UseSelectable = true;
		this.btnLogout.BackColor = System.Drawing.SystemColors.Control;
		this.btnLogout.Cursor = System.Windows.Forms.Cursors.Hand;
		resources.ApplyResources(this.btnLogout, "btnLogout");
		this.btnLogout.Name = "btnLogout";
		this.toolTipMain.SetToolTip(this.btnLogout, resources.GetString("btnLogout.ToolTip"));
		this.btnLogout.UseVisualStyleBackColor = true;
		this.btnLogout.Click += new System.EventHandler(btnLogout_Click);
		resources.ApplyResources(this.btnView, "btnView");
		this.btnView.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnView.Name = "btnView";
		this.toolTipMain.SetToolTip(this.btnView, resources.GetString("btnView.ToolTip"));
		this.btnView.UseVisualStyleBackColor = true;
		this.btnView.Click += new System.EventHandler(btnView_Click);
		resources.ApplyResources(this.btnResult, "btnResult");
		this.btnResult.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnResult.Name = "btnResult";
		this.toolTipMain.SetToolTip(this.btnResult, resources.GetString("btnResult.ToolTip"));
		this.btnResult.UseVisualStyleBackColor = true;
		this.btnResult.Click += new System.EventHandler(btnResult_Click);
		resources.ApplyResources(this.btnComplete, "btnComplete");
		this.btnComplete.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnComplete.Name = "btnComplete";
		this.toolTipMain.SetToolTip(this.btnComplete, resources.GetString("btnComplete.ToolTip"));
		this.btnComplete.UseVisualStyleBackColor = true;
		this.btnComplete.Click += new System.EventHandler(btnComplete_Click);
		resources.ApplyResources(this.btnRefresh, "btnRefresh");
		this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnRefresh.Name = "btnRefresh";
		this.toolTipMain.SetToolTip(this.btnRefresh, resources.GetString("btnRefresh.ToolTip"));
		this.btnRefresh.UseVisualStyleBackColor = true;
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.txtUsername.CustomButton.Image = (System.Drawing.Image)resources.GetObject("resource.Image");
		this.txtUsername.CustomButton.ImeMode = (System.Windows.Forms.ImeMode)resources.GetObject("resource.ImeMode");
		this.txtUsername.CustomButton.Location = (System.Drawing.Point)resources.GetObject("resource.Location");
		this.txtUsername.CustomButton.Margin = (System.Windows.Forms.Padding)resources.GetObject("resource.Margin");
		this.txtUsername.CustomButton.Name = "";
		this.txtUsername.CustomButton.Size = (System.Drawing.Size)resources.GetObject("resource.Size");
		this.txtUsername.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
		this.txtUsername.CustomButton.TabIndex = (int)resources.GetObject("resource.TabIndex");
		this.txtUsername.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
		this.txtUsername.CustomButton.UseSelectable = true;
		this.txtUsername.CustomButton.Visible = (bool)resources.GetObject("resource.Visible");
		this.txtUsername.DisplayIcon = true;
		resources.ApplyResources(this.txtUsername, "txtUsername");
		this.txtUsername.Icon = _5S_QA_Client.Properties.Resources.user;
		this.txtUsername.Lines = new string[0];
		this.txtUsername.MaxLength = 32767;
		this.txtUsername.Name = "txtUsername";
		this.txtUsername.PasswordChar = '\0';
		this.txtUsername.PromptText = "Username";
		this.txtUsername.ScrollBars = System.Windows.Forms.ScrollBars.None;
		this.txtUsername.SelectedText = "";
		this.txtUsername.SelectionLength = 0;
		this.txtUsername.SelectionStart = 0;
		this.txtUsername.ShortcutsEnabled = true;
		this.txtUsername.ShowClearButton = true;
		this.toolTipMain.SetToolTip(this.txtUsername, resources.GetString("txtUsername.ToolTip"));
		this.txtUsername.UseSelectable = true;
		this.txtUsername.WaterMark = "Username";
		this.txtUsername.WaterMarkColor = System.Drawing.Color.Silver;
		this.txtUsername.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
		this.txtUsername.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtUsername_KeyPress);
		this.rbEN.Cursor = System.Windows.Forms.Cursors.Hand;
		resources.ApplyResources(this.rbEN, "rbEN");
		this.rbEN.Image = _5S_QA_Client.Properties.Resources.united_kingdom;
		this.rbEN.Name = "rbEN";
		this.toolTipMain.SetToolTip(this.rbEN, resources.GetString("rbEN.ToolTip"));
		this.rbEN.UseVisualStyleBackColor = true;
		this.rbEN.CheckedChanged += new System.EventHandler(rbLanguage_CheckedChanged);
		this.btnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
		resources.ApplyResources(this.btnLogin, "btnLogin");
		this.btnLogin.Name = "btnLogin";
		this.btnLogin.Style = MetroFramework.MetroColorStyle.Blue;
		this.toolTipMain.SetToolTip(this.btnLogin, resources.GetString("btnLogin.ToolTip"));
		this.btnLogin.UseSelectable = true;
		this.btnLogin.UseVisualStyleBackColor = true;
		this.btnLogin.Click += new System.EventHandler(btnLogin_Click);
		this.txtPassword.CustomButton.Image = (System.Drawing.Image)resources.GetObject("resource.Image1");
		this.txtPassword.CustomButton.ImeMode = (System.Windows.Forms.ImeMode)resources.GetObject("resource.ImeMode1");
		this.txtPassword.CustomButton.Location = (System.Drawing.Point)resources.GetObject("resource.Location1");
		this.txtPassword.CustomButton.Margin = (System.Windows.Forms.Padding)resources.GetObject("resource.Margin1");
		this.txtPassword.CustomButton.Name = "";
		this.txtPassword.CustomButton.Size = (System.Drawing.Size)resources.GetObject("resource.Size1");
		this.txtPassword.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
		this.txtPassword.CustomButton.TabIndex = (int)resources.GetObject("resource.TabIndex1");
		this.txtPassword.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
		this.txtPassword.CustomButton.UseSelectable = true;
		this.txtPassword.CustomButton.Visible = (bool)resources.GetObject("resource.Visible1");
		this.txtPassword.DisplayIcon = true;
		resources.ApplyResources(this.txtPassword, "txtPassword");
		this.txtPassword.Icon = _5S_QA_Client.Properties.Resources.key;
		this.txtPassword.Lines = new string[0];
		this.txtPassword.MaxLength = 32767;
		this.txtPassword.Name = "txtPassword";
		this.txtPassword.PasswordChar = '●';
		this.txtPassword.PromptText = "Password";
		this.txtPassword.ScrollBars = System.Windows.Forms.ScrollBars.None;
		this.txtPassword.SelectedText = "";
		this.txtPassword.SelectionLength = 0;
		this.txtPassword.SelectionStart = 0;
		this.txtPassword.ShortcutsEnabled = true;
		this.txtPassword.ShowClearButton = true;
		this.toolTipMain.SetToolTip(this.txtPassword, resources.GetString("txtPassword.ToolTip"));
		this.txtPassword.UseSelectable = true;
		this.txtPassword.UseSystemPasswordChar = true;
		this.txtPassword.WaterMark = "Password";
		this.txtPassword.WaterMarkColor = System.Drawing.Color.Silver;
		this.txtPassword.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtUsername_KeyPress);
		this.rbJP.Cursor = System.Windows.Forms.Cursors.Hand;
		resources.ApplyResources(this.rbJP, "rbJP");
		this.rbJP.Image = _5S_QA_Client.Properties.Resources.japan;
		this.rbJP.Name = "rbJP";
		this.toolTipMain.SetToolTip(this.rbJP, resources.GetString("rbJP.ToolTip"));
		this.rbJP.UseVisualStyleBackColor = true;
		this.rbJP.CheckedChanged += new System.EventHandler(rbLanguage_CheckedChanged);
		this.rbVN.Cursor = System.Windows.Forms.Cursors.Hand;
		resources.ApplyResources(this.rbVN, "rbVN");
		this.rbVN.Image = _5S_QA_Client.Properties.Resources.vietnam;
		this.rbVN.Name = "rbVN";
		this.toolTipMain.SetToolTip(this.rbVN, resources.GetString("rbVN.ToolTip"));
		this.rbVN.UseVisualStyleBackColor = true;
		this.rbVN.CheckedChanged += new System.EventHandler(rbLanguage_CheckedChanged);
		this.rbTW.Cursor = System.Windows.Forms.Cursors.Hand;
		resources.ApplyResources(this.rbTW, "rbTW");
		this.rbTW.Image = _5S_QA_Client.Properties.Resources.taiwan;
		this.rbTW.Name = "rbTW";
		this.toolTipMain.SetToolTip(this.rbTW, resources.GetString("rbTW.ToolTip"));
		this.rbTW.UseVisualStyleBackColor = true;
		this.rbTW.CheckedChanged += new System.EventHandler(rbLanguage_CheckedChanged);
		resources.ApplyResources(this.btnHelp, "btnHelp");
		this.btnHelp.BackColor = System.Drawing.Color.Transparent;
		this.btnHelp.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnHelp.FlatAppearance.BorderSize = 0;
		this.btnHelp.Name = "btnHelp";
		this.btnHelp.TabStop = false;
		this.toolTipMain.SetToolTip(this.btnHelp, resources.GetString("btnHelp.ToolTip"));
		this.btnHelp.UseVisualStyleBackColor = false;
		this.btnHelp.Click += new System.EventHandler(btnHelp_Click);
		this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		resources.ApplyResources(this.panel2, "panel2");
		this.panel2.Name = "panel2";
		this.lblVersion.BackColor = System.Drawing.Color.FromArgb(0, 174, 219);
		resources.ApplyResources(this.lblVersion, "lblVersion");
		this.lblVersion.ForeColor = System.Drawing.Color.White;
		this.lblVersion.Name = "lblVersion";
		this.lblVersion.Style = MetroFramework.MetroColorStyle.Blue;
		this.lblVersion.UseCustomBackColor = true;
		this.lblVersion.UseCustomForeColor = true;
		this.lblVersion.UseStyleColors = true;
		resources.ApplyResources(this.panelLogin, "panelLogin");
		this.panelLogin.Controls.Add(this.tableLayoutPanel1);
		this.panelLogin.Controls.Add(this.pictureBox1);
		this.panelLogin.Controls.Add(this.lblVersion);
		this.panelLogin.Controls.Add(this.tpanelLanguage);
		this.panelLogin.Name = "panelLogin";
		resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
		this.tableLayoutPanel1.Controls.Add(this.txtUsername, 0, 0);
		this.tableLayoutPanel1.Controls.Add(this.txtPassword, 0, 1);
		this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 2);
		this.tableLayoutPanel1.Controls.Add(this.cbRemember, 0, 3);
		this.tableLayoutPanel1.Controls.Add(this.btnLogin, 0, 4);
		this.tableLayoutPanel1.Name = "tableLayoutPanel1";
		this.pictureBox1.ContextMenuStrip = this.contextMenuStripMain;
		resources.ApplyResources(this.pictureBox1, "pictureBox1");
		this.pictureBox1.Image = _5S_QA_Client.Properties.Resources._5S_QA_C;
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.TabStop = false;
		this.contextMenuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.changePasswordToolStripMenuItem, this.toolStripSeparator2, this.getIMEToolStripMenuItem });
		this.contextMenuStripMain.Name = "contextMenuStripMain";
		resources.ApplyResources(this.contextMenuStripMain, "contextMenuStripMain");
		this.changePasswordToolStripMenuItem.Name = "changePasswordToolStripMenuItem";
		resources.ApplyResources(this.changePasswordToolStripMenuItem, "changePasswordToolStripMenuItem");
		this.changePasswordToolStripMenuItem.Click += new System.EventHandler(changePasswordToolStripMenuItem_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
		this.getIMEToolStripMenuItem.Name = "getIMEToolStripMenuItem";
		resources.ApplyResources(this.getIMEToolStripMenuItem, "getIMEToolStripMenuItem");
		this.getIMEToolStripMenuItem.Click += new System.EventHandler(getIMEToolStripMenuItem_Click);
		resources.ApplyResources(this.tpanelLanguage, "tpanelLanguage");
		this.tpanelLanguage.Controls.Add(this.rbTW, 3, 0);
		this.tpanelLanguage.Controls.Add(this.rbJP, 2, 0);
		this.tpanelLanguage.Controls.Add(this.rbEN, 1, 0);
		this.tpanelLanguage.Controls.Add(this.rbVN, 0, 0);
		this.tpanelLanguage.Name = "tpanelLanguage";
		this.panelUIMain.Controls.Add(this.mPanelViewMain);
		this.panelUIMain.Controls.Add(this.dgvMain);
		this.panelUIMain.Controls.Add(this.statusStripfrmMain);
		this.panelUIMain.Controls.Add(this.tpanelButton);
		this.panelUIMain.Controls.Add(this.mSearchMain);
		resources.ApplyResources(this.panelUIMain, "panelUIMain");
		this.panelUIMain.Name = "panelUIMain";
		this.panelUIMain.TabStop = true;
		this.mPanelViewMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		resources.ApplyResources(this.mPanelViewMain, "mPanelViewMain");
		this.mPanelViewMain.Name = "mPanelViewMain";
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
		resources.ApplyResources(this.dgvMain, "dgvMain");
		this.dgvMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
		this.dgvMain.Columns.AddRange(this.No, this.Code, this.name, this.GroupId, this.ProductId, this.ProductStage, this.ProductCode, this.ProductName, this.ProductDescription, this.ProductDepartment, this.ProductImageUrl, this.ProductCavity, this.Date, this.Quantity, this.Lot, this.Line, this.Intention, this.Sample, this.Type, this.Status, this.Judgement, this.Link, this.Completed, this.CompletedBy, this.Checked, this.CheckedBy, this.Approved, this.ApprovedBy, this.TotalComment, this.Id, this.Created, this.Modified, this.CreatedBy, this.ModifiedBy, this.IsActivated);
		this.dgvMain.ContextMenuStrip = this.contextMenuStripRequest;
		this.dgvMain.EnableHeadersVisualStyles = false;
		this.dgvMain.MultiSelect = false;
		this.dgvMain.Name = "dgvMain";
		this.dgvMain.ReadOnly = true;
		this.dgvMain.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvMain_CellClick);
		this.dgvMain.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvMain_CellDoubleClick);
		this.dgvMain.CurrentCellChanged += new System.EventHandler(dgvMain_CurrentCellChanged);
		this.dgvMain.Sorted += new System.EventHandler(dgvMain_Sorted);
		this.contextMenuStripRequest.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.main_refreshToolStripMenuItem, this.toolStripSeparator1, this.main_viewToolStripMenuItem, this.toolStripSeparator3, this.main_resultToolStripMenuItem, this.main_completeToolStripMenuItem });
		this.contextMenuStripRequest.Name = "contextMenuStripStaff";
		this.contextMenuStripRequest.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
		resources.ApplyResources(this.contextMenuStripRequest, "contextMenuStripRequest");
		this.main_refreshToolStripMenuItem.Name = "main_refreshToolStripMenuItem";
		resources.ApplyResources(this.main_refreshToolStripMenuItem, "main_refreshToolStripMenuItem");
		this.main_refreshToolStripMenuItem.Click += new System.EventHandler(main_refreshToolStripMenuItem_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
		this.main_viewToolStripMenuItem.Name = "main_viewToolStripMenuItem";
		resources.ApplyResources(this.main_viewToolStripMenuItem, "main_viewToolStripMenuItem");
		this.main_viewToolStripMenuItem.Click += new System.EventHandler(main_viewToolStripMenuItem_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
		this.main_resultToolStripMenuItem.Name = "main_resultToolStripMenuItem";
		resources.ApplyResources(this.main_resultToolStripMenuItem, "main_resultToolStripMenuItem");
		this.main_resultToolStripMenuItem.Click += new System.EventHandler(main_resultToolStripMenuItem_Click);
		this.main_completeToolStripMenuItem.Name = "main_completeToolStripMenuItem";
		resources.ApplyResources(this.main_completeToolStripMenuItem, "main_completeToolStripMenuItem");
		this.main_completeToolStripMenuItem.Click += new System.EventHandler(main_completeToolStripMenuItem_Click);
		this.statusStripfrmMain.BackColor = System.Drawing.SystemColors.ControlLight;
		this.statusStripfrmMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.sprogbarStatus, this.slblStatus });
		resources.ApplyResources(this.statusStripfrmMain, "statusStripfrmMain");
		this.statusStripfrmMain.Name = "statusStripfrmMain";
		this.statusStripfrmMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
		this.statusStripfrmMain.SizingGrip = false;
		this.statusStripfrmMain.Stretch = false;
		this.sprogbarStatus.Name = "sprogbarStatus";
		resources.ApplyResources(this.sprogbarStatus, "sprogbarStatus");
		this.slblStatus.Name = "slblStatus";
		resources.ApplyResources(this.slblStatus, "slblStatus");
		resources.ApplyResources(this.tpanelButton, "tpanelButton");
		this.tpanelButton.Controls.Add(this.btnRefresh, 3, 0);
		this.tpanelButton.Controls.Add(this.btnComplete, 2, 0);
		this.tpanelButton.Controls.Add(this.btnResult, 1, 0);
		this.tpanelButton.Controls.Add(this.btnView, 0, 0);
		this.tpanelButton.Name = "tpanelButton";
		this.tpanelButton.TabStop = true;
		resources.ApplyResources(this.mSearchMain, "mSearchMain");
		this.mSearchMain.BackColor = System.Drawing.SystemColors.Control;
		this.mSearchMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.mSearchMain.dataTable = null;
		this.mSearchMain.Name = "mSearchMain";
		resources.ApplyResources(this.panelLogout, "panelLogout");
		this.panelLogout.BackColor = System.Drawing.Color.Transparent;
		this.panelLogout.Controls.Add(this.lblRealtime);
		this.panelLogout.Controls.Add(this.btnLogout);
		this.panelLogout.Controls.Add(this.lblFullname);
		this.panelLogout.Controls.Add(this.ptbAvatar);
		this.panelLogout.Name = "panelLogout";
		this.panelLogout.TabStop = true;
		resources.ApplyResources(this.lblRealtime, "lblRealtime");
		this.lblRealtime.ForeColor = System.Drawing.Color.Blue;
		this.lblRealtime.Name = "lblRealtime";
		resources.ApplyResources(this.lblFullname, "lblFullname");
		this.lblFullname.Name = "lblFullname";
		resources.ApplyResources(this.ptbAvatar, "ptbAvatar");
		this.ptbAvatar.ErrorImage = _5S_QA_Client.Properties.Resources._5S_QA_C;
		this.ptbAvatar.Image = _5S_QA_Client.Properties.Resources._5S_QA_C;
		this.ptbAvatar.InitialImage = _5S_QA_Client.Properties.Resources._5S_QA_C;
		this.ptbAvatar.Name = "ptbAvatar";
		this.ptbAvatar.TabStop = false;
		this.dataGridViewTextBoxColumn1.DataPropertyName = "No";
		dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle3;
		resources.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
		this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
		this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn2.DataPropertyName = "Code";
		this.dataGridViewTextBoxColumn2.FillWeight = 20f;
		resources.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
		this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
		this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn3.DataPropertyName = "Name";
		this.dataGridViewTextBoxColumn3.FillWeight = 40f;
		resources.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
		this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
		this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn4.DataPropertyName = "ProductId";
		this.dataGridViewTextBoxColumn4.FillWeight = 40f;
		resources.ApplyResources(this.dataGridViewTextBoxColumn4, "dataGridViewTextBoxColumn4");
		this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
		this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn5.DataPropertyName = "ProductCode";
		this.dataGridViewTextBoxColumn5.FillWeight = 30f;
		resources.ApplyResources(this.dataGridViewTextBoxColumn5, "dataGridViewTextBoxColumn5");
		this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
		this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn6.DataPropertyName = "ProductName";
		this.dataGridViewTextBoxColumn6.FillWeight = 30f;
		resources.ApplyResources(this.dataGridViewTextBoxColumn6, "dataGridViewTextBoxColumn6");
		this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
		this.dataGridViewTextBoxColumn7.DataPropertyName = "ProductImageUrl";
		resources.ApplyResources(this.dataGridViewTextBoxColumn7, "dataGridViewTextBoxColumn7");
		this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
		this.dataGridViewTextBoxColumn8.DataPropertyName = "ProductCavity";
		resources.ApplyResources(this.dataGridViewTextBoxColumn8, "dataGridViewTextBoxColumn8");
		this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
		this.dataGridViewTextBoxColumn9.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn9.DataPropertyName = "Date";
		this.dataGridViewTextBoxColumn9.FillWeight = 20f;
		resources.ApplyResources(this.dataGridViewTextBoxColumn9, "dataGridViewTextBoxColumn9");
		this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
		this.dataGridViewTextBoxColumn10.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn10.DataPropertyName = "Lot";
		this.dataGridViewTextBoxColumn10.FillWeight = 20f;
		resources.ApplyResources(this.dataGridViewTextBoxColumn10, "dataGridViewTextBoxColumn10");
		this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
		this.dataGridViewTextBoxColumn11.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn11.DataPropertyName = "Line";
		this.dataGridViewTextBoxColumn11.FillWeight = 20f;
		resources.ApplyResources(this.dataGridViewTextBoxColumn11, "dataGridViewTextBoxColumn11");
		this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
		this.dataGridViewTextBoxColumn12.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn12.DataPropertyName = "Quantity";
		this.dataGridViewTextBoxColumn12.FillWeight = 20f;
		resources.ApplyResources(this.dataGridViewTextBoxColumn12, "dataGridViewTextBoxColumn12");
		this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
		this.dataGridViewTextBoxColumn13.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn13.DataPropertyName = "CheckQuantity";
		this.dataGridViewTextBoxColumn13.FillWeight = 20f;
		resources.ApplyResources(this.dataGridViewTextBoxColumn13, "dataGridViewTextBoxColumn13");
		this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
		this.dataGridViewTextBoxColumn14.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn14.DataPropertyName = "Sample";
		this.dataGridViewTextBoxColumn14.FillWeight = 20f;
		resources.ApplyResources(this.dataGridViewTextBoxColumn14, "dataGridViewTextBoxColumn14");
		this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
		this.dataGridViewTextBoxColumn15.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn15.DataPropertyName = "Type";
		this.dataGridViewTextBoxColumn15.FillWeight = 20f;
		resources.ApplyResources(this.dataGridViewTextBoxColumn15, "dataGridViewTextBoxColumn15");
		this.dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
		this.dataGridViewTextBoxColumn16.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn16.DataPropertyName = "Status";
		this.dataGridViewTextBoxColumn16.FillWeight = 20f;
		resources.ApplyResources(this.dataGridViewTextBoxColumn16, "dataGridViewTextBoxColumn16");
		this.dataGridViewTextBoxColumn16.Name = "dataGridViewTextBoxColumn16";
		this.dataGridViewTextBoxColumn17.DataPropertyName = "Judgement";
		resources.ApplyResources(this.dataGridViewTextBoxColumn17, "dataGridViewTextBoxColumn17");
		this.dataGridViewTextBoxColumn17.Name = "dataGridViewTextBoxColumn17";
		this.dataGridViewTextBoxColumn18.DataPropertyName = "Link";
		resources.ApplyResources(this.dataGridViewTextBoxColumn18, "dataGridViewTextBoxColumn18");
		this.dataGridViewTextBoxColumn18.Name = "dataGridViewTextBoxColumn18";
		this.dataGridViewTextBoxColumn19.DataPropertyName = "Completed";
		resources.ApplyResources(this.dataGridViewTextBoxColumn19, "dataGridViewTextBoxColumn19");
		this.dataGridViewTextBoxColumn19.Name = "dataGridViewTextBoxColumn19";
		this.dataGridViewTextBoxColumn20.DataPropertyName = "CompletedBy";
		resources.ApplyResources(this.dataGridViewTextBoxColumn20, "dataGridViewTextBoxColumn20");
		this.dataGridViewTextBoxColumn20.Name = "dataGridViewTextBoxColumn20";
		this.dataGridViewTextBoxColumn21.DataPropertyName = "Checked";
		resources.ApplyResources(this.dataGridViewTextBoxColumn21, "dataGridViewTextBoxColumn21");
		this.dataGridViewTextBoxColumn21.Name = "dataGridViewTextBoxColumn21";
		this.dataGridViewTextBoxColumn22.DataPropertyName = "CheckedBy";
		resources.ApplyResources(this.dataGridViewTextBoxColumn22, "dataGridViewTextBoxColumn22");
		this.dataGridViewTextBoxColumn22.Name = "dataGridViewTextBoxColumn22";
		this.dataGridViewTextBoxColumn23.DataPropertyName = "Approved";
		resources.ApplyResources(this.dataGridViewTextBoxColumn23, "dataGridViewTextBoxColumn23");
		this.dataGridViewTextBoxColumn23.Name = "dataGridViewTextBoxColumn23";
		this.dataGridViewTextBoxColumn24.DataPropertyName = "ApprovedBy";
		resources.ApplyResources(this.dataGridViewTextBoxColumn24, "dataGridViewTextBoxColumn24");
		this.dataGridViewTextBoxColumn24.Name = "dataGridViewTextBoxColumn24";
		this.dataGridViewTextBoxColumn25.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn25.DataPropertyName = "TotalComment";
		this.dataGridViewTextBoxColumn25.FillWeight = 15f;
		resources.ApplyResources(this.dataGridViewTextBoxColumn25, "dataGridViewTextBoxColumn25");
		this.dataGridViewTextBoxColumn25.Name = "dataGridViewTextBoxColumn25";
		this.dataGridViewTextBoxColumn26.DataPropertyName = "Id";
		resources.ApplyResources(this.dataGridViewTextBoxColumn26, "dataGridViewTextBoxColumn26");
		this.dataGridViewTextBoxColumn26.Name = "dataGridViewTextBoxColumn26";
		this.dataGridViewTextBoxColumn27.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.dataGridViewTextBoxColumn27.DataPropertyName = "Created";
		this.dataGridViewTextBoxColumn27.FillWeight = 30f;
		resources.ApplyResources(this.dataGridViewTextBoxColumn27, "dataGridViewTextBoxColumn27");
		this.dataGridViewTextBoxColumn27.Name = "dataGridViewTextBoxColumn27";
		this.dataGridViewTextBoxColumn28.DataPropertyName = "Modified";
		resources.ApplyResources(this.dataGridViewTextBoxColumn28, "dataGridViewTextBoxColumn28");
		this.dataGridViewTextBoxColumn28.Name = "dataGridViewTextBoxColumn28";
		this.dataGridViewTextBoxColumn29.DataPropertyName = "CreatedBy";
		resources.ApplyResources(this.dataGridViewTextBoxColumn29, "dataGridViewTextBoxColumn29");
		this.dataGridViewTextBoxColumn29.Name = "dataGridViewTextBoxColumn29";
		this.dataGridViewTextBoxColumn30.DataPropertyName = "modifiedby";
		resources.ApplyResources(this.dataGridViewTextBoxColumn30, "dataGridViewTextBoxColumn30");
		this.dataGridViewTextBoxColumn30.Name = "dataGridViewTextBoxColumn30";
		this.dataGridViewTextBoxColumn31.DataPropertyName = "IsActivated";
		resources.ApplyResources(this.dataGridViewTextBoxColumn31, "dataGridViewTextBoxColumn31");
		this.dataGridViewTextBoxColumn31.Name = "dataGridViewTextBoxColumn31";
		this.No.DataPropertyName = "No";
		dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		this.No.DefaultCellStyle = dataGridViewCellStyle4;
		resources.ApplyResources(this.No, "No");
		this.No.Name = "No";
		this.No.ReadOnly = true;
		this.No.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.No.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.Code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Code.DataPropertyName = "Code";
		this.Code.FillWeight = 20f;
		resources.ApplyResources(this.Code, "Code");
		this.Code.Name = "Code";
		this.Code.ReadOnly = true;
		this.name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.name.DataPropertyName = "Name";
		this.name.FillWeight = 30f;
		resources.ApplyResources(this.name, "name");
		this.name.Name = "name";
		this.name.ReadOnly = true;
		this.GroupId.DataPropertyName = "GroupId";
		resources.ApplyResources(this.GroupId, "GroupId");
		this.GroupId.Name = "GroupId";
		this.GroupId.ReadOnly = true;
		this.ProductId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.ProductId.DataPropertyName = "ProductId";
		this.ProductId.FillWeight = 40f;
		resources.ApplyResources(this.ProductId, "ProductId");
		this.ProductId.Name = "ProductId";
		this.ProductId.ReadOnly = true;
		this.ProductStage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.ProductStage.DataPropertyName = "ProductStage";
		this.ProductStage.FillWeight = 20f;
		resources.ApplyResources(this.ProductStage, "ProductStage");
		this.ProductStage.Name = "ProductStage";
		this.ProductStage.ReadOnly = true;
		this.ProductCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.ProductCode.DataPropertyName = "ProductCode";
		this.ProductCode.FillWeight = 30f;
		resources.ApplyResources(this.ProductCode, "ProductCode");
		this.ProductCode.Name = "ProductCode";
		this.ProductCode.ReadOnly = true;
		this.ProductName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.ProductName.DataPropertyName = "ProductName";
		this.ProductName.FillWeight = 20f;
		resources.ApplyResources(this.ProductName, "ProductName");
		this.ProductName.Name = "ProductName";
		this.ProductName.ReadOnly = true;
		this.ProductDescription.DataPropertyName = "ProductDescription";
		resources.ApplyResources(this.ProductDescription, "ProductDescription");
		this.ProductDescription.Name = "ProductDescription";
		this.ProductDescription.ReadOnly = true;
		this.ProductDepartment.DataPropertyName = "ProductDepartment";
		resources.ApplyResources(this.ProductDepartment, "ProductDepartment");
		this.ProductDepartment.Name = "ProductDepartment";
		this.ProductDepartment.ReadOnly = true;
		this.ProductImageUrl.DataPropertyName = "ProductImageUrl";
		resources.ApplyResources(this.ProductImageUrl, "ProductImageUrl");
		this.ProductImageUrl.Name = "ProductImageUrl";
		this.ProductImageUrl.ReadOnly = true;
		this.ProductCavity.DataPropertyName = "ProductCavity";
		resources.ApplyResources(this.ProductCavity, "ProductCavity");
		this.ProductCavity.Name = "ProductCavity";
		this.ProductCavity.ReadOnly = true;
		this.Date.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Date.DataPropertyName = "Date";
		this.Date.FillWeight = 20f;
		resources.ApplyResources(this.Date, "Date");
		this.Date.Name = "Date";
		this.Date.ReadOnly = true;
		this.Quantity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Quantity.DataPropertyName = "Quantity";
		this.Quantity.FillWeight = 20f;
		resources.ApplyResources(this.Quantity, "Quantity");
		this.Quantity.Name = "Quantity";
		this.Quantity.ReadOnly = true;
		this.Lot.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Lot.DataPropertyName = "Lot";
		this.Lot.FillWeight = 20f;
		resources.ApplyResources(this.Lot, "Lot");
		this.Lot.Name = "Lot";
		this.Lot.ReadOnly = true;
		this.Line.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Line.DataPropertyName = "Line";
		this.Line.FillWeight = 20f;
		resources.ApplyResources(this.Line, "Line");
		this.Line.Name = "Line";
		this.Line.ReadOnly = true;
		this.Intention.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Intention.DataPropertyName = "Intention";
		this.Intention.FillWeight = 20f;
		resources.ApplyResources(this.Intention, "Intention");
		this.Intention.Name = "Intention";
		this.Intention.ReadOnly = true;
		this.Sample.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Sample.DataPropertyName = "Sample";
		this.Sample.FillWeight = 20f;
		resources.ApplyResources(this.Sample, "Sample");
		this.Sample.Name = "Sample";
		this.Sample.ReadOnly = true;
		this.Type.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Type.DataPropertyName = "Type";
		this.Type.FillWeight = 20f;
		resources.ApplyResources(this.Type, "Type");
		this.Type.Name = "Type";
		this.Type.ReadOnly = true;
		this.Status.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Status.DataPropertyName = "Status";
		this.Status.FillWeight = 20f;
		resources.ApplyResources(this.Status, "Status");
		this.Status.Name = "Status";
		this.Status.ReadOnly = true;
		this.Judgement.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Judgement.DataPropertyName = "Judgement";
		this.Judgement.FillWeight = 20f;
		resources.ApplyResources(this.Judgement, "Judgement");
		this.Judgement.Name = "Judgement";
		this.Judgement.ReadOnly = true;
		this.Link.DataPropertyName = "Link";
		resources.ApplyResources(this.Link, "Link");
		this.Link.Name = "Link";
		this.Link.ReadOnly = true;
		this.Completed.DataPropertyName = "Completed";
		resources.ApplyResources(this.Completed, "Completed");
		this.Completed.Name = "Completed";
		this.Completed.ReadOnly = true;
		this.CompletedBy.DataPropertyName = "CompletedBy";
		resources.ApplyResources(this.CompletedBy, "CompletedBy");
		this.CompletedBy.Name = "CompletedBy";
		this.CompletedBy.ReadOnly = true;
		this.Checked.DataPropertyName = "Checked";
		resources.ApplyResources(this.Checked, "Checked");
		this.Checked.Name = "Checked";
		this.Checked.ReadOnly = true;
		this.CheckedBy.DataPropertyName = "CheckedBy";
		resources.ApplyResources(this.CheckedBy, "CheckedBy");
		this.CheckedBy.Name = "CheckedBy";
		this.CheckedBy.ReadOnly = true;
		this.Approved.DataPropertyName = "Approved";
		resources.ApplyResources(this.Approved, "Approved");
		this.Approved.Name = "Approved";
		this.Approved.ReadOnly = true;
		this.ApprovedBy.DataPropertyName = "ApprovedBy";
		resources.ApplyResources(this.ApprovedBy, "ApprovedBy");
		this.ApprovedBy.Name = "ApprovedBy";
		this.ApprovedBy.ReadOnly = true;
		this.TotalComment.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.TotalComment.DataPropertyName = "TotalComment";
		this.TotalComment.FillWeight = 15f;
		resources.ApplyResources(this.TotalComment, "TotalComment");
		this.TotalComment.Name = "TotalComment";
		this.TotalComment.ReadOnly = true;
		this.Id.DataPropertyName = "Id";
		resources.ApplyResources(this.Id, "Id");
		this.Id.Name = "Id";
		this.Id.ReadOnly = true;
		this.Created.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Created.DataPropertyName = "Created";
		this.Created.FillWeight = 30f;
		resources.ApplyResources(this.Created, "Created");
		this.Created.Name = "Created";
		this.Created.ReadOnly = true;
		this.Modified.DataPropertyName = "Modified";
		resources.ApplyResources(this.Modified, "Modified");
		this.Modified.Name = "Modified";
		this.Modified.ReadOnly = true;
		this.CreatedBy.DataPropertyName = "CreatedBy";
		resources.ApplyResources(this.CreatedBy, "CreatedBy");
		this.CreatedBy.Name = "CreatedBy";
		this.CreatedBy.ReadOnly = true;
		this.ModifiedBy.DataPropertyName = "modifiedby";
		resources.ApplyResources(this.ModifiedBy, "ModifiedBy");
		this.ModifiedBy.Name = "ModifiedBy";
		this.ModifiedBy.ReadOnly = true;
		this.IsActivated.DataPropertyName = "IsActivated";
		resources.ApplyResources(this.IsActivated, "IsActivated");
		this.IsActivated.Name = "IsActivated";
		this.IsActivated.ReadOnly = true;
		resources.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
		base.Controls.Add(this.btnHelp);
		base.Controls.Add(this.panelLogin);
		base.Controls.Add(this.panelLogout);
		base.Controls.Add(this.panelUIMain);
		base.Name = "frmLogin";
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmLogin_FormClosed);
		base.Load += new System.EventHandler(frmLogin_Load);
		base.Shown += new System.EventHandler(frmLogin_Shown);
		this.panelLogin.ResumeLayout(false);
		this.panelLogin.PerformLayout();
		this.tableLayoutPanel1.ResumeLayout(false);
		this.tableLayoutPanel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		this.contextMenuStripMain.ResumeLayout(false);
		this.tpanelLanguage.ResumeLayout(false);
		this.panelUIMain.ResumeLayout(false);
		this.panelUIMain.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvMain).EndInit();
		this.contextMenuStripRequest.ResumeLayout(false);
		this.statusStripfrmMain.ResumeLayout(false);
		this.statusStripfrmMain.PerformLayout();
		this.tpanelButton.ResumeLayout(false);
		this.tpanelButton.PerformLayout();
		this.panelLogout.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.ptbAvatar).EndInit();
		base.ResumeLayout(false);
	}
}
