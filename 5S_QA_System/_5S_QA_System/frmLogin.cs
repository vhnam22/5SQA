using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Timers;
using System.Windows.Forms;
using _5S_QA_DL.APIContext;
using _5S_QA_Entities.Constants;
using _5S_QA_Entities.Dtos;
using _5S_QA_Entities.Enums;
using _5S_QA_Entities.Models;
using _5S_QA_System.Controls;
using _5S_QA_System.Properties;
using MetroFramework;
using MetroFramework.Controls;
using MetroFramework.Forms;
using Newtonsoft.Json;
using WebSocketSharp;

namespace _5S_QA_System;

public class frmLogin : MetroForm
{
	public static APIClient client;

	public static string mFuntion = string.Empty;

	private WebSocket mWs;

	private System.Timers.Timer mTimer;

	private IContainer components = null;

	private ToolTip toolTipMain;

	private Panel panelUIMain;

	private Panel panelUI;

	private Button btnExport;

	private Button btnResult;

	private Button btnProduct;

	private Button btnMachine;

	private Button btnStaff;

	private ContextMenuStrip contextMenuStripMain;

	private ToolStripMenuItem changePasswordToolStripMenuItem;

	private Button btnRequest;

	private Button btnManager;

	private Button btnComment;

	private Button btnChart;

	private Button btnTemplate;

	private Button btnStatistic;

	private Panel panelLogin;

	private TableLayoutPanel tableLayoutPanel1;

	private MetroTextBox txtUsername;

	private MetroTextBox txtPassword;

	private Panel panel3;

	private MetroCheckBox cbRemember;

	private MetroTextBox.MetroTextButton btnLogin;

	private PictureBox ptbLogo;

	private MetroLabel lblVersion;

	private TableLayoutPanel tpanelLanguage;

	private RadioButton rbJP;

	private RadioButton rbEN;

	private RadioButton rbVN;

	private Panel panelLogout;

	public Button btnLogout;

	private Label lblFullname;

	private PictureBox ptbAvatar;

	private Label lblReport;

	private Label lblRequest;

	private Label lblProduct;

	private Label lblGeneral;

	private RadioButton rbTW;

	private Button btnHelp;

	private Label lblRealtime;

	private Button btnAlert;

	private TransparentLabel lblAlert;

	private Button btnEmail;

	private Button btnExportProduct;

	public static AuthUserViewModel User { get; set; }

	public frmLogin()
	{
		InitializeComponent();
		Common.setControls(this, toolTipMain, new List<ContextMenuStrip> { contextMenuStripMain });
		client = new APIClient();
	}

	private void frmLogin_Load(object sender, EventArgs e)
	{
		Version version = Assembly.GetEntryAssembly().GetName().Version;
		lblVersion.Text = version.ToString(3);
		loadSettings();
		cbRemember.CheckedChanged += cbRememberPassword_CheckedChanged;
	}

	private void frmLogin_Shown(object sender, EventArgs e)
	{
		Init();
	}

	private void frmLogin_FormClosing(object sender, FormClosingEventArgs e)
	{
		GC.Collect();
	}

	private void frmLogin_FormClosed(object sender, FormClosedEventArgs e)
	{
		StopSocket();
		if (client == null)
		{
			return;
		}
		try
		{
			if (panelLogout.Visible)
			{
				client.LogoutAsync(User.Id);
			}
		}
		finally
		{
			client.CloseAPI();
		}
	}

	private void Init()
	{
		txtUsername.Select();
		Init_Timer();
		Init_WebSocket();
	}

	private void Init_Timer()
	{
		mTimer = new System.Timers.Timer
		{
			Interval = 5000.0
		};
		mTimer.Elapsed += time_Elapsed;
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
		RefreshFromSocket(e.Data);
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

	private void en_disControl()
	{
		bool flag = true;
		if (User.Role.Equals(RoleWeb.Member) || User.Username.ToLower().Equals("Admin".ToLower()))
		{
			flag = false;
		}
		btnStaff.Enabled = flag;
		btnMachine.Enabled = flag;
		btnEmail.Enabled = false;
		btnProduct.Enabled = flag;
		btnTemplate.Enabled = flag;
		btnRequest.Enabled = flag;
		btnComment.Enabled = flag;
		btnResult.Enabled = flag;
		btnManager.Enabled = flag;
		btnExport.Enabled = flag;
		btnExportProduct.Enabled = flag;
		btnChart.Enabled = flag && mFuntion.Contains("QC2108");
		btnStatistic.Enabled = flag && mFuntion.Contains("QC2201");
		if (User.Role.Equals(RoleWeb.Member))
		{
			btnComment.Enabled = true;
			btnResult.Enabled = true;
		}
		if (User.Username.ToLower().Equals("Admin".ToLower()))
		{
			btnStaff.Enabled = true;
			btnEmail.Enabled = true;
		}
	}

	public void SetbtnAlert()
	{
		Cursor.Current = Cursors.WaitCursor;
		try
		{
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
			if (User.Role == RoleWeb.Administrator)
			{
				queryArgs.PredicateParameters.Add("Completed");
			}
			else
			{
				queryArgs.PredicateParameters.Add("Checked");
			}
			if (User.JobTitle == "Supervisor" || User.JobTitle == "Manager")
			{
				queryArgs.Predicate += " && Product.Department.Name.StartsWith(@3)";
			}
			else
			{
				queryArgs.Predicate += " && Product.Department.Name=@3";
			}
			queryArgs.PredicateParameters.Add(User.DepartmentName);
			ResponseDto result = client.GetsAsync(queryArgs, "/api/Request/Gets").Result;
			if (!result.Success)
			{
				throw new Exception(result.Messages.ElementAt(0).Message);
			}
			if (result.Count > 0 && lblAlert.Text != result.Count.ToString())
			{
				Console.Beep();
			}
			lblAlert.Text = result.Count.ToString();
		}
		catch
		{
			lblAlert.Text = "0";
		}
	}

	private void StartSocket()
	{
		WebSocket webSocket = mWs;
		if (webSocket == null || webSocket.ReadyState != WebSocketState.Open)
		{
			mWs.Connect();
		}
	}

	private void StopSocket()
	{
		WebSocket webSocket = mWs;
		if (webSocket != null && webSocket.ReadyState == WebSocketState.Open)
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
			if (Common.findForm(typeof(frmRequestManagerView)) is frmRequestManagerView frmRequestManagerView2)
			{
				frmRequestManagerView2.RefreshFromSocket(item);
			}
			if (Common.findForm(typeof(frmRequestView)) is frmRequestView frmRequestView2 && item.Status != "Approved" && item.Status != "Checked")
			{
				frmRequestView2.RefreshFromSocket(item);
			}
			SetbtnAlert();
		});
	}

	private void time_Elapsed(object sender, ElapsedEventArgs e)
	{
		StartSocket();
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
				ptbAvatar.Image = Resources._5S_QA_S;
			}
			SetbtnAlert();
			panelUIMain.Visible = true;
			panelLogout.Visible = true;
			panelLogin.Visible = false;
			StartSocket();
			result = client.GetsAsync("/api/AuthUser/GetFuntions").Result;
			if (result.Success)
			{
				mFuntion = result.Data.ToString();
			}
			else
			{
				mFuntion = string.Empty;
			}
			en_disControl();
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
		mFuntion = string.Empty;
		try
		{
			client.LogoutAsync(User.Id);
		}
		catch
		{
		}
		panelUIMain.Visible = false;
		panelLogout.Visible = false;
		panelLogin.Visible = true;
		client.Token = null;
		Common.closeAllForm();
		StopSocket();
		GC.Collect();
	}

	private void btnStaff_Click(object sender, EventArgs e)
	{
		if (!Common.activeForm(typeof(frmStaffView)))
		{
			frmStaffView frmStaffView2 = new frmStaffView();
			frmStaffView2.Show();
		}
	}

	private void btnMachine_Click(object sender, EventArgs e)
	{
		if (!Common.activeForm(typeof(frmMachineView)))
		{
			new frmMachineView().Show();
		}
	}

	private void btnProduct_Click(object sender, EventArgs e)
	{
		if (!Common.activeForm(typeof(frmProductGroupView)))
		{
			new frmProductGroupView().Show();
		}
	}

	private void btnRequest_Click(object sender, EventArgs e)
	{
		if (!Common.activeForm(typeof(frmRequestView)))
		{
			new frmRequestView().Show();
		}
	}

	private void btnResult_Click(object sender, EventArgs e)
	{
		if (!Common.activeForm(typeof(frmResultView)))
		{
			new frmResultView().Show();
		}
	}

	private void btnExport_Click(object sender, EventArgs e)
	{
		if (!Common.activeForm(typeof(frmMonthView)))
		{
			new frmMonthView().Show();
		}
	}

	private void btnComment_Click(object sender, EventArgs e)
	{
		if (!Common.activeForm(typeof(frmCommentView)))
		{
			new frmCommentView().Show();
		}
	}

	private void btnChart_Click(object sender, EventArgs e)
	{
		if (!Common.activeForm(typeof(frmChartView)))
		{
			new frmChartView().Show();
		}
	}

	private void btnTemplate_Click(object sender, EventArgs e)
	{
		if (!Common.activeForm(typeof(frmTemplateView)))
		{
			new frmTemplateView().Show();
		}
	}

	private void btnRequestManager_Click(object sender, EventArgs e)
	{
		if (Common.findForm(typeof(frmRequestManagerView)) is frmRequestManagerView frmRequestManagerView2)
		{
			frmRequestManagerView2.load_AllData();
			Common.activeForm(typeof(frmRequestManagerView));
		}
		else
		{
			new frmRequestManagerView().Show();
		}
	}

	private void btnStatistic_Click(object sender, EventArgs e)
	{
		if (!Common.activeForm(typeof(frmStatisticView)))
		{
			new frmStatisticView().Show();
		}
	}

	private void btnScatter_Click(object sender, EventArgs e)
	{
		if (!Common.activeForm(typeof(frmScatterView)))
		{
			new frmScatterView().Show();
		}
	}

	private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
	{
		new frmChangePassword().ShowDialog();
	}

	private void rbLanguage_CheckedChanged(object sender, EventArgs e)
	{
		RadioButton radioButton = sender as RadioButton;
		if (!(Settings.Default.Language == radioButton.Name))
		{
			Settings.Default.Language = radioButton.Name;
			Settings.Default.Save();
			Common.setControls(this, toolTipMain, new List<ContextMenuStrip> { contextMenuStripMain });
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

	private void btnAlert_Click(object sender, EventArgs e)
	{
		if (Common.findForm(typeof(frmRequestManagerView)) is frmRequestManagerView frmRequestManagerView2)
		{
			frmRequestManagerView2.LoadDataByAlert();
			Common.activeForm(typeof(frmRequestManagerView));
		}
		else
		{
			new frmRequestManagerView(isalert: true).Show();
		}
	}

	private void btnSetting_Click(object sender, EventArgs e)
	{
		if (!Common.activeForm(typeof(frmSettingView)))
		{
			new frmSettingView().Show();
		}
	}

	private void btnEmail_Click(object sender, EventArgs e)
	{
		if (!Common.activeForm(typeof(frmSettingView)))
		{
			new frmSettingView().Show();
		}
	}

	private void btnExportProduct_Click(object sender, EventArgs e)
	{
		if (!Common.activeForm(typeof(frmExportProduct)))
		{
			new frmExportProduct().Show();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_5S_QA_System.frmLogin));
		this.toolTipMain = new System.Windows.Forms.ToolTip();
		this.txtUsername = new MetroFramework.Controls.MetroTextBox();
		this.txtPassword = new MetroFramework.Controls.MetroTextBox();
		this.cbRemember = new MetroFramework.Controls.MetroCheckBox();
		this.btnLogout = new System.Windows.Forms.Button();
		this.btnHelp = new System.Windows.Forms.Button();
		this.btnEmail = new System.Windows.Forms.Button();
		this.btnAlert = new System.Windows.Forms.Button();
		this.btnStatistic = new System.Windows.Forms.Button();
		this.btnTemplate = new System.Windows.Forms.Button();
		this.btnChart = new System.Windows.Forms.Button();
		this.btnComment = new System.Windows.Forms.Button();
		this.btnManager = new System.Windows.Forms.Button();
		this.btnRequest = new System.Windows.Forms.Button();
		this.btnExport = new System.Windows.Forms.Button();
		this.btnResult = new System.Windows.Forms.Button();
		this.btnProduct = new System.Windows.Forms.Button();
		this.btnMachine = new System.Windows.Forms.Button();
		this.btnStaff = new System.Windows.Forms.Button();
		this.btnLogin = new MetroFramework.Controls.MetroTextBox.MetroTextButton();
		this.rbJP = new System.Windows.Forms.RadioButton();
		this.rbEN = new System.Windows.Forms.RadioButton();
		this.rbVN = new System.Windows.Forms.RadioButton();
		this.rbTW = new System.Windows.Forms.RadioButton();
		this.btnExportProduct = new System.Windows.Forms.Button();
		this.contextMenuStripMain = new System.Windows.Forms.ContextMenuStrip();
		this.changePasswordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.panelUIMain = new System.Windows.Forms.Panel();
		this.panelUI = new System.Windows.Forms.Panel();
		this.lblAlert = new _5S_QA_System.TransparentLabel();
		this.lblGeneral = new System.Windows.Forms.Label();
		this.lblRequest = new System.Windows.Forms.Label();
		this.lblProduct = new System.Windows.Forms.Label();
		this.lblReport = new System.Windows.Forms.Label();
		this.panelLogin = new System.Windows.Forms.Panel();
		this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ptbLogo = new System.Windows.Forms.PictureBox();
		this.lblVersion = new MetroFramework.Controls.MetroLabel();
		this.tpanelLanguage = new System.Windows.Forms.TableLayoutPanel();
		this.panelLogout = new System.Windows.Forms.Panel();
		this.lblRealtime = new System.Windows.Forms.Label();
		this.lblFullname = new System.Windows.Forms.Label();
		this.ptbAvatar = new System.Windows.Forms.PictureBox();
		this.contextMenuStripMain.SuspendLayout();
		this.panelUIMain.SuspendLayout();
		this.panelUI.SuspendLayout();
		this.panelLogin.SuspendLayout();
		this.tableLayoutPanel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.ptbLogo).BeginInit();
		this.tpanelLanguage.SuspendLayout();
		this.panelLogout.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.ptbAvatar).BeginInit();
		base.SuspendLayout();
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
		this.txtUsername.Lines = new string[0];
		this.txtUsername.MaxLength = 32767;
		this.txtUsername.Name = "txtUsername";
		this.txtUsername.PasswordChar = '\0';
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
		this.txtPassword.Lines = new string[0];
		this.txtPassword.MaxLength = 32767;
		this.txtPassword.Name = "txtPassword";
		this.txtPassword.PasswordChar = '●';
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
		resources.ApplyResources(this.btnHelp, "btnHelp");
		this.btnHelp.BackColor = System.Drawing.Color.Transparent;
		this.btnHelp.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnHelp.FlatAppearance.BorderSize = 0;
		this.btnHelp.Name = "btnHelp";
		this.btnHelp.TabStop = false;
		this.toolTipMain.SetToolTip(this.btnHelp, resources.GetString("btnHelp.ToolTip"));
		this.btnHelp.UseVisualStyleBackColor = false;
		this.btnHelp.Click += new System.EventHandler(btnHelp_Click);
		this.btnEmail.BackColor = System.Drawing.Color.SaddleBrown;
		this.btnEmail.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnEmail.FlatAppearance.BorderSize = 0;
		this.btnEmail.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGray;
		resources.ApplyResources(this.btnEmail, "btnEmail");
		this.btnEmail.ForeColor = System.Drawing.Color.Transparent;
		this.btnEmail.Image = _5S_QA_System.Properties.Resources.email_marketing;
		this.btnEmail.Name = "btnEmail";
		this.toolTipMain.SetToolTip(this.btnEmail, resources.GetString("btnEmail.ToolTip"));
		this.btnEmail.UseVisualStyleBackColor = false;
		this.btnEmail.Click += new System.EventHandler(btnEmail_Click);
		this.btnAlert.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnAlert.FlatAppearance.BorderSize = 0;
		this.btnAlert.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGray;
		resources.ApplyResources(this.btnAlert, "btnAlert");
		this.btnAlert.Image = _5S_QA_System.Properties.Resources.bell_orange;
		this.btnAlert.Name = "btnAlert";
		this.toolTipMain.SetToolTip(this.btnAlert, resources.GetString("btnAlert.ToolTip"));
		this.btnAlert.UseVisualStyleBackColor = false;
		this.btnAlert.Click += new System.EventHandler(btnAlert_Click);
		this.btnStatistic.BackColor = System.Drawing.Color.Orange;
		this.btnStatistic.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnStatistic.FlatAppearance.BorderSize = 0;
		this.btnStatistic.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGray;
		resources.ApplyResources(this.btnStatistic, "btnStatistic");
		this.btnStatistic.ForeColor = System.Drawing.Color.Transparent;
		this.btnStatistic.Image = _5S_QA_System.Properties.Resources.monitor;
		this.btnStatistic.Name = "btnStatistic";
		this.toolTipMain.SetToolTip(this.btnStatistic, resources.GetString("btnStatistic.ToolTip"));
		this.btnStatistic.UseVisualStyleBackColor = false;
		this.btnStatistic.Click += new System.EventHandler(btnStatistic_Click);
		this.btnTemplate.BackColor = System.Drawing.Color.BlueViolet;
		this.btnTemplate.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnTemplate.FlatAppearance.BorderSize = 0;
		this.btnTemplate.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGray;
		resources.ApplyResources(this.btnTemplate, "btnTemplate");
		this.btnTemplate.ForeColor = System.Drawing.Color.Transparent;
		this.btnTemplate.Image = _5S_QA_System.Properties.Resources.seo_report;
		this.btnTemplate.Name = "btnTemplate";
		this.toolTipMain.SetToolTip(this.btnTemplate, resources.GetString("btnTemplate.ToolTip"));
		this.btnTemplate.UseVisualStyleBackColor = false;
		this.btnTemplate.Click += new System.EventHandler(btnTemplate_Click);
		this.btnChart.BackColor = System.Drawing.Color.Green;
		this.btnChart.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnChart.FlatAppearance.BorderSize = 0;
		this.btnChart.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGray;
		resources.ApplyResources(this.btnChart, "btnChart");
		this.btnChart.ForeColor = System.Drawing.Color.Transparent;
		this.btnChart.Image = _5S_QA_System.Properties.Resources.bar_chart;
		this.btnChart.Name = "btnChart";
		this.toolTipMain.SetToolTip(this.btnChart, resources.GetString("btnChart.ToolTip"));
		this.btnChart.UseVisualStyleBackColor = false;
		this.btnChart.Click += new System.EventHandler(btnChart_Click);
		this.btnComment.BackColor = System.Drawing.Color.Fuchsia;
		this.btnComment.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnComment.FlatAppearance.BorderSize = 0;
		this.btnComment.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGray;
		resources.ApplyResources(this.btnComment, "btnComment");
		this.btnComment.ForeColor = System.Drawing.Color.Transparent;
		this.btnComment.Image = _5S_QA_System.Properties.Resources.blog;
		this.btnComment.Name = "btnComment";
		this.toolTipMain.SetToolTip(this.btnComment, resources.GetString("btnComment.ToolTip"));
		this.btnComment.UseVisualStyleBackColor = false;
		this.btnComment.Click += new System.EventHandler(btnComment_Click);
		this.btnManager.BackColor = System.Drawing.Color.MidnightBlue;
		this.btnManager.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnManager.FlatAppearance.BorderSize = 0;
		this.btnManager.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGray;
		resources.ApplyResources(this.btnManager, "btnManager");
		this.btnManager.ForeColor = System.Drawing.Color.Transparent;
		this.btnManager.Image = _5S_QA_System.Properties.Resources.contract;
		this.btnManager.Name = "btnManager";
		this.toolTipMain.SetToolTip(this.btnManager, resources.GetString("btnManager.ToolTip"));
		this.btnManager.UseVisualStyleBackColor = false;
		this.btnManager.Click += new System.EventHandler(btnRequestManager_Click);
		this.btnRequest.BackColor = System.Drawing.Color.Gold;
		this.btnRequest.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnRequest.FlatAppearance.BorderSize = 0;
		this.btnRequest.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGray;
		resources.ApplyResources(this.btnRequest, "btnRequest");
		this.btnRequest.ForeColor = System.Drawing.Color.Transparent;
		this.btnRequest.Image = _5S_QA_System.Properties.Resources.social_media;
		this.btnRequest.Name = "btnRequest";
		this.toolTipMain.SetToolTip(this.btnRequest, resources.GetString("btnRequest.ToolTip"));
		this.btnRequest.UseVisualStyleBackColor = false;
		this.btnRequest.Click += new System.EventHandler(btnRequest_Click);
		this.btnExport.BackColor = System.Drawing.Color.Blue;
		this.btnExport.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnExport.FlatAppearance.BorderSize = 0;
		this.btnExport.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGray;
		resources.ApplyResources(this.btnExport, "btnExport");
		this.btnExport.ForeColor = System.Drawing.Color.Transparent;
		this.btnExport.Image = _5S_QA_System.Properties.Resources.export_file;
		this.btnExport.Name = "btnExport";
		this.toolTipMain.SetToolTip(this.btnExport, resources.GetString("btnExport.ToolTip"));
		this.btnExport.UseVisualStyleBackColor = false;
		this.btnExport.Click += new System.EventHandler(btnExport_Click);
		this.btnResult.BackColor = System.Drawing.Color.Teal;
		this.btnResult.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnResult.FlatAppearance.BorderSize = 0;
		this.btnResult.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGray;
		resources.ApplyResources(this.btnResult, "btnResult");
		this.btnResult.ForeColor = System.Drawing.Color.Transparent;
		this.btnResult.Image = _5S_QA_System.Properties.Resources.medical_result;
		this.btnResult.Name = "btnResult";
		this.toolTipMain.SetToolTip(this.btnResult, resources.GetString("btnResult.ToolTip"));
		this.btnResult.UseVisualStyleBackColor = false;
		this.btnResult.Click += new System.EventHandler(btnResult_Click);
		this.btnProduct.BackColor = System.Drawing.Color.PaleVioletRed;
		this.btnProduct.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnProduct.FlatAppearance.BorderSize = 0;
		this.btnProduct.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGray;
		resources.ApplyResources(this.btnProduct, "btnProduct");
		this.btnProduct.ForeColor = System.Drawing.Color.Transparent;
		this.btnProduct.Image = _5S_QA_System.Properties.Resources.industry;
		this.btnProduct.Name = "btnProduct";
		this.toolTipMain.SetToolTip(this.btnProduct, resources.GetString("btnProduct.ToolTip"));
		this.btnProduct.UseVisualStyleBackColor = false;
		this.btnProduct.Click += new System.EventHandler(btnProduct_Click);
		this.btnMachine.BackColor = System.Drawing.Color.Lime;
		this.btnMachine.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnMachine.FlatAppearance.BorderSize = 0;
		this.btnMachine.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGray;
		resources.ApplyResources(this.btnMachine, "btnMachine");
		this.btnMachine.ForeColor = System.Drawing.Color.Transparent;
		this.btnMachine.Image = _5S_QA_System.Properties.Resources.microscope1;
		this.btnMachine.Name = "btnMachine";
		this.toolTipMain.SetToolTip(this.btnMachine, resources.GetString("btnMachine.ToolTip"));
		this.btnMachine.UseVisualStyleBackColor = false;
		this.btnMachine.Click += new System.EventHandler(btnMachine_Click);
		this.btnStaff.BackColor = System.Drawing.Color.Indigo;
		this.btnStaff.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnStaff.FlatAppearance.BorderSize = 0;
		this.btnStaff.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGray;
		resources.ApplyResources(this.btnStaff, "btnStaff");
		this.btnStaff.ForeColor = System.Drawing.Color.Transparent;
		this.btnStaff.Image = _5S_QA_System.Properties.Resources.user_groups;
		this.btnStaff.Name = "btnStaff";
		this.toolTipMain.SetToolTip(this.btnStaff, resources.GetString("btnStaff.ToolTip"));
		this.btnStaff.UseVisualStyleBackColor = false;
		this.btnStaff.Click += new System.EventHandler(btnStaff_Click);
		this.btnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
		resources.ApplyResources(this.btnLogin, "btnLogin");
		this.btnLogin.Name = "btnLogin";
		this.btnLogin.Style = MetroFramework.MetroColorStyle.Blue;
		this.toolTipMain.SetToolTip(this.btnLogin, resources.GetString("btnLogin.ToolTip"));
		this.btnLogin.UseSelectable = true;
		this.btnLogin.UseVisualStyleBackColor = true;
		this.btnLogin.Click += new System.EventHandler(btnLogin_Click);
		this.rbJP.Cursor = System.Windows.Forms.Cursors.Hand;
		resources.ApplyResources(this.rbJP, "rbJP");
		this.rbJP.Image = _5S_QA_System.Properties.Resources.japan;
		this.rbJP.Name = "rbJP";
		this.toolTipMain.SetToolTip(this.rbJP, resources.GetString("rbJP.ToolTip"));
		this.rbJP.UseVisualStyleBackColor = true;
		this.rbJP.CheckedChanged += new System.EventHandler(rbLanguage_CheckedChanged);
		this.rbEN.Cursor = System.Windows.Forms.Cursors.Hand;
		resources.ApplyResources(this.rbEN, "rbEN");
		this.rbEN.Image = _5S_QA_System.Properties.Resources.united_kingdom;
		this.rbEN.Name = "rbEN";
		this.toolTipMain.SetToolTip(this.rbEN, resources.GetString("rbEN.ToolTip"));
		this.rbEN.UseVisualStyleBackColor = true;
		this.rbEN.CheckedChanged += new System.EventHandler(rbLanguage_CheckedChanged);
		this.rbVN.Cursor = System.Windows.Forms.Cursors.Hand;
		resources.ApplyResources(this.rbVN, "rbVN");
		this.rbVN.Image = _5S_QA_System.Properties.Resources.vietnam;
		this.rbVN.Name = "rbVN";
		this.toolTipMain.SetToolTip(this.rbVN, resources.GetString("rbVN.ToolTip"));
		this.rbVN.UseVisualStyleBackColor = true;
		this.rbVN.CheckedChanged += new System.EventHandler(rbLanguage_CheckedChanged);
		this.rbTW.Cursor = System.Windows.Forms.Cursors.Hand;
		resources.ApplyResources(this.rbTW, "rbTW");
		this.rbTW.Image = _5S_QA_System.Properties.Resources.taiwan;
		this.rbTW.Name = "rbTW";
		this.toolTipMain.SetToolTip(this.rbTW, resources.GetString("rbTW.ToolTip"));
		this.rbTW.UseVisualStyleBackColor = true;
		this.rbTW.CheckedChanged += new System.EventHandler(rbLanguage_CheckedChanged);
		this.btnExportProduct.BackColor = System.Drawing.Color.DeepSkyBlue;
		this.btnExportProduct.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnExportProduct.FlatAppearance.BorderSize = 0;
		this.btnExportProduct.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGray;
		resources.ApplyResources(this.btnExportProduct, "btnExportProduct");
		this.btnExportProduct.ForeColor = System.Drawing.Color.Transparent;
		this.btnExportProduct.Image = _5S_QA_System.Properties.Resources.excel;
		this.btnExportProduct.Name = "btnExportProduct";
		this.toolTipMain.SetToolTip(this.btnExportProduct, resources.GetString("btnExportProduct.ToolTip"));
		this.btnExportProduct.UseVisualStyleBackColor = false;
		this.btnExportProduct.Click += new System.EventHandler(btnExportProduct_Click);
		this.contextMenuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.changePasswordToolStripMenuItem });
		this.contextMenuStripMain.Name = "contextMenuStripMain";
		resources.ApplyResources(this.contextMenuStripMain, "contextMenuStripMain");
		this.changePasswordToolStripMenuItem.Name = "changePasswordToolStripMenuItem";
		resources.ApplyResources(this.changePasswordToolStripMenuItem, "changePasswordToolStripMenuItem");
		this.changePasswordToolStripMenuItem.Click += new System.EventHandler(changePasswordToolStripMenuItem_Click);
		this.panelUIMain.Controls.Add(this.panelUI);
		resources.ApplyResources(this.panelUIMain, "panelUIMain");
		this.panelUIMain.Name = "panelUIMain";
		resources.ApplyResources(this.panelUI, "panelUI");
		this.panelUI.Controls.Add(this.btnExportProduct);
		this.panelUI.Controls.Add(this.btnEmail);
		this.panelUI.Controls.Add(this.btnAlert);
		this.panelUI.Controls.Add(this.lblAlert);
		this.panelUI.Controls.Add(this.lblGeneral);
		this.panelUI.Controls.Add(this.lblRequest);
		this.panelUI.Controls.Add(this.lblProduct);
		this.panelUI.Controls.Add(this.lblReport);
		this.panelUI.Controls.Add(this.btnStatistic);
		this.panelUI.Controls.Add(this.btnTemplate);
		this.panelUI.Controls.Add(this.btnChart);
		this.panelUI.Controls.Add(this.btnComment);
		this.panelUI.Controls.Add(this.btnManager);
		this.panelUI.Controls.Add(this.btnRequest);
		this.panelUI.Controls.Add(this.btnExport);
		this.panelUI.Controls.Add(this.btnResult);
		this.panelUI.Controls.Add(this.btnProduct);
		this.panelUI.Controls.Add(this.btnMachine);
		this.panelUI.Controls.Add(this.btnStaff);
		this.panelUI.Name = "panelUI";
		this.panelUI.TabStop = true;
		resources.ApplyResources(this.lblAlert, "lblAlert");
		this.lblAlert.BackColor = System.Drawing.Color.Transparent;
		this.lblAlert.ForeColor = System.Drawing.Color.FromArgb(255, 128, 0);
		this.lblAlert.Name = "lblAlert";
		this.lblAlert.Opacity = 0;
		this.lblAlert.TransparentBackColor = System.Drawing.Color.Transparent;
		resources.ApplyResources(this.lblGeneral, "lblGeneral");
		this.lblGeneral.Name = "lblGeneral";
		resources.ApplyResources(this.lblRequest, "lblRequest");
		this.lblRequest.Name = "lblRequest";
		resources.ApplyResources(this.lblProduct, "lblProduct");
		this.lblProduct.Name = "lblProduct";
		resources.ApplyResources(this.lblReport, "lblReport");
		this.lblReport.Name = "lblReport";
		resources.ApplyResources(this.panelLogin, "panelLogin");
		this.panelLogin.Controls.Add(this.tableLayoutPanel1);
		this.panelLogin.Controls.Add(this.ptbLogo);
		this.panelLogin.Controls.Add(this.lblVersion);
		this.panelLogin.Controls.Add(this.tpanelLanguage);
		this.panelLogin.Name = "panelLogin";
		resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
		this.tableLayoutPanel1.Controls.Add(this.txtUsername, 0, 0);
		this.tableLayoutPanel1.Controls.Add(this.txtPassword, 0, 1);
		this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 2);
		this.tableLayoutPanel1.Controls.Add(this.cbRemember, 0, 3);
		this.tableLayoutPanel1.Controls.Add(this.btnLogin, 0, 4);
		this.tableLayoutPanel1.Name = "tableLayoutPanel1";
		this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		resources.ApplyResources(this.panel3, "panel3");
		this.panel3.Name = "panel3";
		this.ptbLogo.ContextMenuStrip = this.contextMenuStripMain;
		resources.ApplyResources(this.ptbLogo, "ptbLogo");
		this.ptbLogo.Image = _5S_QA_System.Properties.Resources._5S_QA_S;
		this.ptbLogo.Name = "ptbLogo";
		this.ptbLogo.TabStop = false;
		this.lblVersion.BackColor = System.Drawing.Color.FromArgb(0, 174, 219);
		resources.ApplyResources(this.lblVersion, "lblVersion");
		this.lblVersion.ForeColor = System.Drawing.Color.White;
		this.lblVersion.Name = "lblVersion";
		this.lblVersion.Style = MetroFramework.MetroColorStyle.Blue;
		this.lblVersion.UseCustomBackColor = true;
		this.lblVersion.UseCustomForeColor = true;
		this.lblVersion.UseStyleColors = true;
		resources.ApplyResources(this.tpanelLanguage, "tpanelLanguage");
		this.tpanelLanguage.Controls.Add(this.rbTW, 3, 0);
		this.tpanelLanguage.Controls.Add(this.rbJP, 2, 0);
		this.tpanelLanguage.Controls.Add(this.rbEN, 1, 0);
		this.tpanelLanguage.Controls.Add(this.rbVN, 0, 0);
		this.tpanelLanguage.Name = "tpanelLanguage";
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
		this.ptbAvatar.Image = _5S_QA_System.Properties.Resources._5S_QA_S;
		this.ptbAvatar.Name = "ptbAvatar";
		this.ptbAvatar.TabStop = false;
		resources.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
		base.Controls.Add(this.btnHelp);
		base.Controls.Add(this.panelLogout);
		base.Controls.Add(this.panelUIMain);
		base.Controls.Add(this.panelLogin);
		base.Name = "frmLogin";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmLogin_FormClosing);
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmLogin_FormClosed);
		base.Load += new System.EventHandler(frmLogin_Load);
		base.Shown += new System.EventHandler(frmLogin_Shown);
		this.contextMenuStripMain.ResumeLayout(false);
		this.panelUIMain.ResumeLayout(false);
		this.panelUI.ResumeLayout(false);
		this.panelUI.PerformLayout();
		this.panelLogin.ResumeLayout(false);
		this.panelLogin.PerformLayout();
		this.tableLayoutPanel1.ResumeLayout(false);
		this.tableLayoutPanel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.ptbLogo).EndInit();
		this.tpanelLanguage.ResumeLayout(false);
		this.panelLogout.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.ptbAvatar).EndInit();
		base.ResumeLayout(false);
	}
}
