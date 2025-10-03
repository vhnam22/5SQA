using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using _5S_QA_DL.APIContext;
using _5S_QA_Entities.Abstracts;
using _5S_QA_Entities.Constants;
using _5S_QA_Entities.Dtos;
using _5S_QA_Entities.Models;
using _5S_QA_System.Controls;
using _5S_QA_System.Properties;
using MetroFramework.Forms;

namespace _5S_QA_System;

public class frmSettingView : MetroForm
{
	private Guid mIdEmail;

	private MetadataValueViewModel mDirectory;

	private IContainer components = null;

	private ToolTip toolTipMain;

	private StatusStrip statusStripMain;

	private ToolStripProgressBar sprogbarStatus;

	public ToolStripStatusLabel slblStatus;

	private Panel panelLogout;

	private Label lblFullname;

	private PictureBox ptbAvatar;

	private TabControl tabControlMain;

	private TabPage tabPageEmailSend;

	private TabPage tabPageWarning;

	private TableLayoutPanel tpanelEmailSend;

	private Label lblModifiedBy;

	private Label lblCreatedBy;

	private Label lblModified;

	private Label lblCreated;

	private Label lblModifiBy;

	private Label lblCreateBy;

	private Label lblModifi;

	private Label lblCreate;

	private Label lblPassword;

	private Label lblEmail;

	private Button btnConfirm;

	private Label lblFooter;

	private Label lblHeader;

	private TextBox txtEmail;

	private Panel panelPassword;

	private Button btnEye;

	private TextBox txtPassword;

	private RichTextBox rtbHeader;

	private RichTextBox rtbFooter;

	private GroupBox gbAbnormal;

	private TableLayoutPanel tpanelAbnormal;

	private CheckBox cbR4;

	private CheckBox cbR3;

	private CheckBox cbR2;

	private CheckBox cbR1;

	private CheckBox cbXtb8;

	private CheckBox cbXtb7;

	private CheckBox cbXtb6;

	private CheckBox cbXtb5;

	private CheckBox cbXtb4;

	private CheckBox cbXtb3;

	private CheckBox cbXtb2;

	private Label label4;

	private Label label8;

	private CheckBox cbXtb1;

	private TableLayoutPanel tpanelAbnormalActive;

	private Label lblAbnormalActive;

	private CheckBox cbAbnormalActive;

	private GroupBox gbOKNG;

	private TableLayoutPanel tpanelOKNG;

	private Label lblOKNGActive;

	private CheckBox cbOKNGActive;

	private GroupBox gbPosition;

	private TableLayoutPanel tpanelPosition;

	private CheckBox cbStaff;

	private CheckBox cbLeader;

	private Label label3;

	private Label label2;

	private Label label1;

	private CheckBox cbManager;

	private ContextMenuStrip contextMenuStripWarning;

	private ToolStripMenuItem main_refreshToolStripMenuItem;

	private CheckBox cbDepartment;

	private Label lblDepartment;

	private TabPage tabPageRank;

	private TableLayoutPanel tpanelRank;

	private TextBox txtRank3;

	private Label label6;

	private Label label16;

	private Label label17;

	private TextBox txtRank5;

	private TextBox txtRank4;

	private TabPage tabPageDirectory;

	private TableLayoutPanel tpanelDirectory;

	private Label lblIsActivated;

	private Panel panelFolder;

	private Button btnFolder;

	private TextBox txtFolder;

	private Panel panel1;

	private Button btnProtectEye;

	private TextBox txtProtect;

	private Label lblDirModifiedBy;

	private Label lblDirCreatedBy;

	private Label lblDirModified;

	private Label lblDirCreated;

	private Label lblDirModifiBy;

	private Label lblDirCreateBy;

	private Label lblDirModifi;

	private Label lblDirCreate;

	private Label lblProtect;

	private Label lblFolder;

	private CheckBox cbIsActivated;

	private FolderBrowserDialog folderBrowserDialogMain;

	private TabPage tabPageNotification;

	private GroupBox gbRequestStatus;

	private TableLayoutPanel tableLayoutPanel1;

	private CheckBox cbCompleted;

	private CheckBox cbChecked;

	private Label lblCompleted;

	private Label lblChecked;

	public frmSettingView()
	{
		InitializeComponent();
		Common.setControls(this, toolTipMain, new List<ContextMenuStrip> { contextMenuStripWarning });
		mIdEmail = Guid.Empty;
	}

	private void frmSettingView_Load(object sender, EventArgs e)
	{
		Init();
	}

	private void frmSettingView_Shown(object sender, EventArgs e)
	{
		LoadEmailSend();
		LoadWarning();
		LoadRank();
		LoadDirectory();
		LoadNotification();
	}

	private void frmSettingView_FormClosing(object sender, FormClosingEventArgs e)
	{
		GC.Collect();
	}

	private void frmSettingView_FormClosed(object sender, FormClosedEventArgs e)
	{
	}

	private void Init()
	{
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

	private void LoadEmailSend()
	{
		Cursor.Current = Cursors.WaitCursor;
		try
		{
			start_Proccessor();
			QueryArgs body = new QueryArgs
			{
				Order = "Created DESC",
				Page = 1,
				Limit = 1
			};
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/Email/Gets").Result;
			EmailViewModel val = Common.getObjectToList<EmailViewModel>(result).FirstOrDefault();
			mIdEmail = ((AuditableEntity)(object)val).Id;
			txtEmail.Text = val.Name;
			txtPassword.Text = val.Password;
			rtbHeader.Text = val.Header;
			rtbFooter.Text = val.Footer;
			lblCreated.Text = ((AuditableEntity)(object)val).Created.ToString();
			lblCreatedBy.Text = ((AuditableEntity)(object)val).CreatedBy.ToString();
			lblModified.Text = ((AuditableEntity)(object)val).Modified.ToString();
			lblModifiedBy.Text = ((AuditableEntity)(object)val).ModifiedBy.ToString();
			debugOutput(Common.getTextLanguage(this, "Successful"));
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

	private void LoadWarning()
	{
		Cursor.Current = Cursors.WaitCursor;
		IEnumerable<Control> controlsOfType = Common.GetControlsOfType<CheckBox>(tabPageWarning);
		try
		{
			start_Proccessor();
			QueryArgs body = new QueryArgs
			{
				Order = "Created DESC",
				Page = 1,
				Limit = int.MaxValue
			};
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/Setting/Gets").Result;
			List<SettingViewModel> objectToList = Common.getObjectToList<SettingViewModel>(result);
			debugOutput(Common.getTextLanguage(this, "Successful"));
			if (objectToList.Count > 0)
			{
				foreach (CheckBox cb in controlsOfType.Cast<CheckBox>())
				{
					SettingViewModel val = objectToList.FirstOrDefault((SettingViewModel x) => x.Name == cb.Name);
					if (val == null)
					{
						cb.Checked = false;
					}
					else
					{
						cb.Checked = val.IsChecked.Value;
					}
				}
				return;
			}
			SetWarningDefault(controlsOfType);
		}
		catch (Exception ex)
		{
			SetWarningDefault(controlsOfType);
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

	private void LoadRank()
	{
		Cursor.Current = Cursors.WaitCursor;
		IEnumerable<Control> controlsOfType = Common.GetControlsOfType<TextBox>(tabPageRank);
		try
		{
			start_Proccessor();
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "Code.Contains(@0)";
			queryArgs.PredicateParameters = new string[1] { "RANK-" };
			queryArgs.Order = "Created DESC";
			queryArgs.Page = 1;
			queryArgs.Limit = 3;
			QueryArgs body = queryArgs;
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/MetadataValue/Gets").Result;
			List<MetadataValueViewModel> objectToList = Common.getObjectToList<MetadataValueViewModel>(result);
			debugOutput(Common.getTextLanguage(this, "Successful"));
			foreach (TextBox txt in controlsOfType.Cast<TextBox>())
			{
				MetadataValueViewModel metadataValueViewModel = objectToList.FirstOrDefault((MetadataValueViewModel x) => x.Name == txt.Name);
				if (metadataValueViewModel == null)
				{
					txt.Text = "";
				}
				else
				{
					txt.Text = metadataValueViewModel.Value;
				}
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

	private void LoadDirectory()
	{
		Cursor.Current = Cursors.WaitCursor;
		try
		{
			start_Proccessor();
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "TypeId=@0 && Name=@1";
			queryArgs.PredicateParameters = new string[2] { "3FCB0099-A290-46A6-A2C4-1934C6328B9D", "DirFile" };
			queryArgs.Order = "Created DESC";
			queryArgs.Page = 1;
			queryArgs.Limit = 1;
			QueryArgs body = queryArgs;
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/MetadataValue/Gets").Result;
			mDirectory = Common.getObjectToList<MetadataValueViewModel>(result).FirstOrDefault();
			debugOutput(Common.getTextLanguage(this, "Successful"));
			if (mDirectory != null)
			{
				txtFolder.Text = mDirectory.Value;
				txtProtect.Text = mDirectory.Description;
				cbIsActivated.Checked = mDirectory.IsActivated;
				lblDirCreated.Text = mDirectory.Created.ToString();
				lblDirCreatedBy.Text = mDirectory.CreatedBy.ToString();
				lblDirModified.Text = mDirectory.Modified.ToString();
				lblDirModifiedBy.Text = mDirectory.ModifiedBy.ToString();
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

	private void LoadNotification()
	{
		Cursor.Current = Cursors.WaitCursor;
		IEnumerable<Control> controlsOfType = Common.GetControlsOfType<CheckBox>(tabPageNotification);
		try
		{
			start_Proccessor();
			QueryArgs body = new QueryArgs
			{
				Order = "Created DESC",
				Page = 1,
				Limit = int.MaxValue
			};
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/Setting/Gets").Result;
			List<SettingViewModel> objectToList = Common.getObjectToList<SettingViewModel>(result);
			debugOutput(Common.getTextLanguage(this, "Successful"));
			if (objectToList.Count > 0)
			{
				foreach (CheckBox cb in controlsOfType.Cast<CheckBox>())
				{
					SettingViewModel val = objectToList.FirstOrDefault((SettingViewModel x) => x.Name == cb.Name);
					if (val == null)
					{
						cb.Checked = false;
					}
					else
					{
						cb.Checked = val.IsChecked.Value;
					}
				}
				return;
			}
			SetWarningDefault(controlsOfType);
		}
		catch (Exception ex)
		{
			SetWarningDefault(controlsOfType);
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

	private void SetWarningDefault(IEnumerable<Control> controls)
	{
		foreach (CheckBox item in controls.Cast<CheckBox>())
		{
			item.Checked = false;
		}
	}

	private string set_Code()
	{
		string text = "CFG-";
		try
		{
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "TypeId=@0";
			queryArgs.PredicateParameters = new string[1] { "3FCB0099-A290-46A6-A2C4-1934C6328B9D" };
			queryArgs.Order = "Created DESC";
			queryArgs.Page = 1;
			queryArgs.Limit = 1;
			QueryArgs body = queryArgs;
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/MetadataValue/Gets").Result;
			DataTable dataTable = Common.getDataTable<MetadataValueViewModel>(result);
			if (dataTable.Rows.Count > 0)
			{
				string[] array = dataTable.Rows[0]["Code"].ToString().Split('-');
				if (array.Length > 1 && int.TryParse(array[1], out var result2))
				{
					text += result2 + 1;
				}
			}
			else
			{
				text += "1";
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
		return text;
	}

	private void txtPassword_TextChanged(object sender, EventArgs e)
	{
		TextBox textBox = sender as TextBox;
		btnEye.Visible = !string.IsNullOrEmpty(textBox.Text);
	}

	private void btnEye_MouseDown(object sender, MouseEventArgs e)
	{
		txtPassword.UseSystemPasswordChar = false;
	}

	private void btnEye_MouseUp(object sender, MouseEventArgs e)
	{
		txtPassword.UseSystemPasswordChar = true;
	}

	private void btnConfirm_Click(object sender, EventArgs e)
	{
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Expected O, but got Unknown
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		//IL_00c1: Expected O, but got Unknown
		//IL_05bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cb: Expected O, but got Unknown
		//IL_05cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05da: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0602: Expected O, but got Unknown
		//IL_0608: Expected O, but got Unknown
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Expected O, but got Unknown
		//IL_021c: Expected O, but got Unknown
		Cursor.Current = Cursors.WaitCursor;
		try
		{
			switch (tabControlMain.SelectedIndex)
			{
			case 0:
			{
				List<SettingViewModel> list = new List<SettingViewModel>();
				IEnumerable<Control> controlsOfType = Common.GetControlsOfType<CheckBox>(tabPageWarning);
				foreach (CheckBox item in controlsOfType.Cast<CheckBox>())
				{
					Control parentOfType2 = Common.GetParentOfType<GroupBox>(item);
					List<SettingViewModel> list4 = list;
					SettingViewModel val3 = new SettingViewModel();
					((AuditableEntity)val3).Id = Guid.Empty;
					val3.Group = parentOfType2.Name;
					val3.Name = item.Name;
					val3.IsChecked = item.Checked;
					((AuditableEntity)val3).IsActivated = true;
					list4.Add(val3);
				}
				if (MessageBox.Show(Common.getTextLanguage(this, "wSureEdit"), Common.getTextLanguage(this, "QUESTION"), MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.No))
				{
					return;
				}
				Cursor.Current = Cursors.WaitCursor;
				ResponseDto result = frmLogin.client.SaveAsync(list, "/api/Setting/SaveList").Result;
				if (!result.Success)
				{
					throw new Exception(result.Messages.ElementAt(0).Message);
				}
				break;
			}
			case 1:
			{
				EmailViewModel val2 = new EmailViewModel();
				((AuditableEntity)val2).Id = mIdEmail;
				val2.Name = (string.IsNullOrEmpty(txtEmail.Text) ? null : txtEmail.Text);
				val2.Password = (string.IsNullOrEmpty(txtPassword.Text) ? null : txtPassword.Text);
				val2.Header = (string.IsNullOrEmpty(rtbHeader.Text) ? null : rtbHeader.Text);
				val2.Footer = (string.IsNullOrEmpty(rtbFooter.Text) ? null : rtbFooter.Text);
				((AuditableEntity)val2).IsActivated = true;
				EmailViewModel body = val2;
				if (mIdEmail != Guid.Empty && MessageBox.Show(Common.getTextLanguage(this, "wSureEdit"), Common.getTextLanguage(this, "QUESTION"), MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.No))
				{
					return;
				}
				Cursor.Current = Cursors.WaitCursor;
				ResponseDto result = frmLogin.client.SaveAsync(body, "/api/Email/Save").Result;
				if (!result.Success)
				{
					throw new Exception(result.Messages.ElementAt(0).Message);
				}
				break;
			}
			case 2:
			{
				List<MetadataValueViewModel> list3 = new List<MetadataValueViewModel>();
				IEnumerable<Control> controlsOfType2 = Common.GetControlsOfType<TextBox>(tabPageRank);
				foreach (TextBox item2 in controlsOfType2.Cast<TextBox>())
				{
					list3.Add(new MetadataValueViewModel
					{
						Id = Guid.Empty,
						Code = "RANK-" + item2.Name.Replace("txtRank", ""),
						Name = item2.Name,
						Value = item2.Text,
						TypeId = new Guid("EEA65E86-D458-4919-82F7-3DCA0475695D"),
						IsActivated = true
					});
				}
				if (MessageBox.Show(Common.getTextLanguage(this, "wSureEdit"), Common.getTextLanguage(this, "QUESTION"), MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.No))
				{
					return;
				}
				Cursor.Current = Cursors.WaitCursor;
				if (list3.Count > 0)
				{
					ResponseDto result = frmLogin.client.SaveAsync(list3, "/api/MetadataValue/SaveRanks").Result;
					if (!result.Success)
					{
						throw new Exception(result.Messages.ElementAt(0).Message);
					}
				}
				break;
			}
			case 3:
			{
				if (mDirectory == null)
				{
					mDirectory = new MetadataValueViewModel
					{
						Id = Guid.Empty,
						Code = set_Code(),
						Name = "DirFile",
						TypeId = new Guid("3FCB0099-A290-46A6-A2C4-1934C6328B9D")
					};
				}
				mDirectory.Value = (string.IsNullOrEmpty(txtFolder.Text) ? null : txtFolder.Text);
				mDirectory.Description = (string.IsNullOrEmpty(txtProtect.Text) ? null : txtProtect.Text);
				mDirectory.IsActivated = cbIsActivated.Checked;
				if (mDirectory.Id != Guid.Empty && MessageBox.Show(Common.getTextLanguage(this, "wSureEdit"), Common.getTextLanguage(this, "QUESTION"), MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.No))
				{
					return;
				}
				Cursor.Current = Cursors.WaitCursor;
				ResponseDto result = frmLogin.client.SaveAsync(mDirectory, "/api/MetadataValue/Save").Result;
				if (!result.Success)
				{
					throw new Exception(result.Messages.ElementAt(0).Message);
				}
				break;
			}
			case 4:
			{
				List<SettingViewModel> list = new List<SettingViewModel>();
				IEnumerable<Control> controlsOfType = Common.GetControlsOfType<CheckBox>(tabPageNotification);
				foreach (CheckBox item3 in controlsOfType.Cast<CheckBox>())
				{
					Control parentOfType = Common.GetParentOfType<GroupBox>(item3);
					List<SettingViewModel> list2 = list;
					SettingViewModel val = new SettingViewModel();
					((AuditableEntity)val).Id = Guid.Empty;
					val.Group = parentOfType.Name;
					val.Name = item3.Name;
					val.IsChecked = item3.Checked;
					((AuditableEntity)val).IsActivated = true;
					list2.Add(val);
				}
				if (MessageBox.Show(Common.getTextLanguage(this, "wSureEdit"), Common.getTextLanguage(this, "QUESTION"), MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.No))
				{
					return;
				}
				Cursor.Current = Cursors.WaitCursor;
				ResponseDto result = frmLogin.client.SaveAsync(list, "/api/Setting/SaveList").Result;
				if (!result.Success)
				{
					throw new Exception(result.Messages.ElementAt(0).Message);
				}
				break;
			}
			}
			debugOutput(Common.getTextLanguage(this, "Successful"));
			MessageBox.Show(Common.getTextLanguage(this, "Successful"), Common.getTextLanguage(this, "INFORMATION"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
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

	private void ptbAvatar_DoubleClick(object sender, EventArgs e)
	{
		Common.activeForm(typeof(frmLogin));
	}

	private void main_refreshToolStripMenuItem_Click(object sender, EventArgs e)
	{
		switch (tabControlMain.SelectedIndex)
		{
		case 0:
			LoadWarning();
			break;
		case 1:
			LoadEmailSend();
			break;
		case 2:
			LoadRank();
			break;
		case 3:
			LoadDirectory();
			break;
		case 4:
			LoadNotification();
			break;
		}
	}

	private void txtRank_KeyPress(object sender, KeyPressEventArgs e)
	{
		if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b')
		{
			e.Handled = true;
		}
	}

	private void txtRank_Leave(object sender, EventArgs e)
	{
		TextBox textBox = sender as TextBox;
		if (int.TryParse(textBox.Text, out var result))
		{
			textBox.Text = result.ToString();
		}
		else
		{
			textBox.Text = string.Empty;
		}
	}

	private void btnFolder_Click(object sender, EventArgs e)
	{
		if (folderBrowserDialogMain.ShowDialog() == DialogResult.OK)
		{
			txtFolder.Text = folderBrowserDialogMain.SelectedPath;
		}
	}

	private void txtProtect_TextChanged(object sender, EventArgs e)
	{
		TextBox textBox = sender as TextBox;
		btnProtectEye.Visible = !string.IsNullOrEmpty(textBox.Text);
	}

	private void btnProtectEye_MouseDown(object sender, MouseEventArgs e)
	{
		txtProtect.UseSystemPasswordChar = false;
	}

	private void btnProtectEye_MouseUp(object sender, MouseEventArgs e)
	{
		txtProtect.UseSystemPasswordChar = true;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_5S_QA_System.frmSettingView));
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.btnConfirm = new System.Windows.Forms.Button();
		this.txtEmail = new System.Windows.Forms.TextBox();
		this.btnEye = new System.Windows.Forms.Button();
		this.txtPassword = new System.Windows.Forms.TextBox();
		this.rtbHeader = new System.Windows.Forms.RichTextBox();
		this.rtbFooter = new System.Windows.Forms.RichTextBox();
		this.cbR4 = new System.Windows.Forms.CheckBox();
		this.cbR3 = new System.Windows.Forms.CheckBox();
		this.cbR2 = new System.Windows.Forms.CheckBox();
		this.cbR1 = new System.Windows.Forms.CheckBox();
		this.cbXtb8 = new System.Windows.Forms.CheckBox();
		this.cbXtb7 = new System.Windows.Forms.CheckBox();
		this.cbXtb6 = new System.Windows.Forms.CheckBox();
		this.cbXtb5 = new System.Windows.Forms.CheckBox();
		this.cbXtb4 = new System.Windows.Forms.CheckBox();
		this.cbXtb3 = new System.Windows.Forms.CheckBox();
		this.cbXtb2 = new System.Windows.Forms.CheckBox();
		this.cbXtb1 = new System.Windows.Forms.CheckBox();
		this.cbAbnormalActive = new System.Windows.Forms.CheckBox();
		this.cbOKNGActive = new System.Windows.Forms.CheckBox();
		this.cbManager = new System.Windows.Forms.CheckBox();
		this.cbLeader = new System.Windows.Forms.CheckBox();
		this.cbStaff = new System.Windows.Forms.CheckBox();
		this.cbDepartment = new System.Windows.Forms.CheckBox();
		this.txtRank3 = new System.Windows.Forms.TextBox();
		this.txtRank4 = new System.Windows.Forms.TextBox();
		this.txtRank5 = new System.Windows.Forms.TextBox();
		this.btnFolder = new System.Windows.Forms.Button();
		this.txtFolder = new System.Windows.Forms.TextBox();
		this.btnProtectEye = new System.Windows.Forms.Button();
		this.txtProtect = new System.Windows.Forms.TextBox();
		this.cbIsActivated = new System.Windows.Forms.CheckBox();
		this.cbCompleted = new System.Windows.Forms.CheckBox();
		this.cbChecked = new System.Windows.Forms.CheckBox();
		this.statusStripMain = new System.Windows.Forms.StatusStrip();
		this.sprogbarStatus = new System.Windows.Forms.ToolStripProgressBar();
		this.slblStatus = new System.Windows.Forms.ToolStripStatusLabel();
		this.panelLogout = new System.Windows.Forms.Panel();
		this.lblFullname = new System.Windows.Forms.Label();
		this.ptbAvatar = new System.Windows.Forms.PictureBox();
		this.tabControlMain = new System.Windows.Forms.TabControl();
		this.tabPageWarning = new System.Windows.Forms.TabPage();
		this.contextMenuStripWarning = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.main_refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.gbOKNG = new System.Windows.Forms.GroupBox();
		this.tpanelOKNG = new System.Windows.Forms.TableLayoutPanel();
		this.lblOKNGActive = new System.Windows.Forms.Label();
		this.gbAbnormal = new System.Windows.Forms.GroupBox();
		this.tpanelAbnormal = new System.Windows.Forms.TableLayoutPanel();
		this.label4 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.tpanelAbnormalActive = new System.Windows.Forms.TableLayoutPanel();
		this.lblAbnormalActive = new System.Windows.Forms.Label();
		this.gbPosition = new System.Windows.Forms.GroupBox();
		this.tpanelPosition = new System.Windows.Forms.TableLayoutPanel();
		this.lblDepartment = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.tabPageEmailSend = new System.Windows.Forms.TabPage();
		this.tpanelEmailSend = new System.Windows.Forms.TableLayoutPanel();
		this.panelPassword = new System.Windows.Forms.Panel();
		this.lblFooter = new System.Windows.Forms.Label();
		this.lblHeader = new System.Windows.Forms.Label();
		this.lblModifiedBy = new System.Windows.Forms.Label();
		this.lblCreatedBy = new System.Windows.Forms.Label();
		this.lblModified = new System.Windows.Forms.Label();
		this.lblCreated = new System.Windows.Forms.Label();
		this.lblModifiBy = new System.Windows.Forms.Label();
		this.lblCreateBy = new System.Windows.Forms.Label();
		this.lblModifi = new System.Windows.Forms.Label();
		this.lblCreate = new System.Windows.Forms.Label();
		this.lblPassword = new System.Windows.Forms.Label();
		this.lblEmail = new System.Windows.Forms.Label();
		this.tabPageRank = new System.Windows.Forms.TabPage();
		this.tpanelRank = new System.Windows.Forms.TableLayoutPanel();
		this.label6 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.tabPageDirectory = new System.Windows.Forms.TabPage();
		this.tpanelDirectory = new System.Windows.Forms.TableLayoutPanel();
		this.lblIsActivated = new System.Windows.Forms.Label();
		this.panelFolder = new System.Windows.Forms.Panel();
		this.panel1 = new System.Windows.Forms.Panel();
		this.lblDirModifiedBy = new System.Windows.Forms.Label();
		this.lblDirCreatedBy = new System.Windows.Forms.Label();
		this.lblDirModified = new System.Windows.Forms.Label();
		this.lblDirCreated = new System.Windows.Forms.Label();
		this.lblDirModifiBy = new System.Windows.Forms.Label();
		this.lblDirCreateBy = new System.Windows.Forms.Label();
		this.lblDirModifi = new System.Windows.Forms.Label();
		this.lblDirCreate = new System.Windows.Forms.Label();
		this.lblProtect = new System.Windows.Forms.Label();
		this.lblFolder = new System.Windows.Forms.Label();
		this.tabPageNotification = new System.Windows.Forms.TabPage();
		this.gbRequestStatus = new System.Windows.Forms.GroupBox();
		this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
		this.lblCompleted = new System.Windows.Forms.Label();
		this.lblChecked = new System.Windows.Forms.Label();
		this.folderBrowserDialogMain = new System.Windows.Forms.FolderBrowserDialog();
		this.statusStripMain.SuspendLayout();
		this.panelLogout.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.ptbAvatar).BeginInit();
		this.tabControlMain.SuspendLayout();
		this.tabPageWarning.SuspendLayout();
		this.contextMenuStripWarning.SuspendLayout();
		this.gbOKNG.SuspendLayout();
		this.tpanelOKNG.SuspendLayout();
		this.gbAbnormal.SuspendLayout();
		this.tpanelAbnormal.SuspendLayout();
		this.tpanelAbnormalActive.SuspendLayout();
		this.gbPosition.SuspendLayout();
		this.tpanelPosition.SuspendLayout();
		this.tabPageEmailSend.SuspendLayout();
		this.tpanelEmailSend.SuspendLayout();
		this.panelPassword.SuspendLayout();
		this.tabPageRank.SuspendLayout();
		this.tpanelRank.SuspendLayout();
		this.tabPageDirectory.SuspendLayout();
		this.tpanelDirectory.SuspendLayout();
		this.panelFolder.SuspendLayout();
		this.panel1.SuspendLayout();
		this.tabPageNotification.SuspendLayout();
		this.gbRequestStatus.SuspendLayout();
		this.tableLayoutPanel1.SuspendLayout();
		base.SuspendLayout();
		this.btnConfirm.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnConfirm.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.btnConfirm.FlatAppearance.BorderSize = 0;
		this.btnConfirm.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
		this.btnConfirm.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
		this.btnConfirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnConfirm.Location = new System.Drawing.Point(20, 476);
		this.btnConfirm.Name = "btnConfirm";
		this.btnConfirm.Size = new System.Drawing.Size(610, 28);
		this.btnConfirm.TabIndex = 176;
		this.btnConfirm.Text = "Confirm";
		this.toolTipMain.SetToolTip(this.btnConfirm, "Confirm");
		this.btnConfirm.UseVisualStyleBackColor = true;
		this.btnConfirm.Click += new System.EventHandler(btnConfirm_Click);
		this.txtEmail.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtEmail.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtEmail.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtEmail.Location = new System.Drawing.Point(88, 4);
		this.txtEmail.Name = "txtEmail";
		this.txtEmail.Size = new System.Drawing.Size(504, 22);
		this.txtEmail.TabIndex = 1;
		this.toolTipMain.SetToolTip(this.txtEmail, "Please enter email");
		this.btnEye.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnEye.BackColor = System.Drawing.SystemColors.Control;
		this.btnEye.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnEye.FlatAppearance.BorderSize = 0;
		this.btnEye.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
		this.btnEye.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.AppWorkspace;
		this.btnEye.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnEye.Image = _5S_QA_System.Properties.Resources.eye_black;
		this.btnEye.Location = new System.Drawing.Point(481, 1);
		this.btnEye.Margin = new System.Windows.Forms.Padding(0);
		this.btnEye.Name = "btnEye";
		this.btnEye.Size = new System.Drawing.Size(20, 20);
		this.btnEye.TabIndex = 2;
		this.toolTipMain.SetToolTip(this.btnEye, "View password");
		this.btnEye.UseVisualStyleBackColor = false;
		this.btnEye.Visible = false;
		this.btnEye.MouseDown += new System.Windows.Forms.MouseEventHandler(btnEye_MouseDown);
		this.btnEye.MouseUp += new System.Windows.Forms.MouseEventHandler(btnEye_MouseUp);
		this.txtPassword.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtPassword.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtPassword.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtPassword.Location = new System.Drawing.Point(0, 0);
		this.txtPassword.Name = "txtPassword";
		this.txtPassword.Size = new System.Drawing.Size(504, 22);
		this.txtPassword.TabIndex = 1;
		this.toolTipMain.SetToolTip(this.txtPassword, "Please enter password");
		this.txtPassword.UseSystemPasswordChar = true;
		this.txtPassword.TextChanged += new System.EventHandler(txtPassword_TextChanged);
		this.rtbHeader.Dock = System.Windows.Forms.DockStyle.Fill;
		this.rtbHeader.Location = new System.Drawing.Point(88, 62);
		this.rtbHeader.Name = "rtbHeader";
		this.rtbHeader.Size = new System.Drawing.Size(504, 54);
		this.rtbHeader.TabIndex = 3;
		this.rtbHeader.Text = "";
		this.toolTipMain.SetToolTip(this.rtbHeader, "Please enter header");
		this.rtbFooter.Dock = System.Windows.Forms.DockStyle.Fill;
		this.rtbFooter.Location = new System.Drawing.Point(88, 123);
		this.rtbFooter.Name = "rtbFooter";
		this.rtbFooter.Size = new System.Drawing.Size(504, 54);
		this.rtbFooter.TabIndex = 4;
		this.rtbFooter.Text = "";
		this.toolTipMain.SetToolTip(this.rtbFooter, "Please enter footer");
		this.cbR4.AutoSize = true;
		this.cbR4.Checked = true;
		this.cbR4.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cbR4.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbR4.Location = new System.Drawing.Point(553, 4);
		this.cbR4.Name = "cbR4";
		this.cbR4.Size = new System.Drawing.Size(33, 20);
		this.cbR4.TabIndex = 12;
		this.cbR4.Text = "4";
		this.toolTipMain.SetToolTip(this.cbR4, "Select abnormal R4");
		this.cbR4.UseVisualStyleBackColor = true;
		this.cbR3.AutoSize = true;
		this.cbR3.Checked = true;
		this.cbR3.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cbR3.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbR3.Location = new System.Drawing.Point(513, 4);
		this.cbR3.Name = "cbR3";
		this.cbR3.Size = new System.Drawing.Size(33, 20);
		this.cbR3.TabIndex = 11;
		this.cbR3.Text = "3";
		this.toolTipMain.SetToolTip(this.cbR3, "Select abnormal R3");
		this.cbR3.UseVisualStyleBackColor = true;
		this.cbR2.AutoSize = true;
		this.cbR2.Checked = true;
		this.cbR2.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cbR2.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbR2.Location = new System.Drawing.Point(473, 4);
		this.cbR2.Name = "cbR2";
		this.cbR2.Size = new System.Drawing.Size(33, 20);
		this.cbR2.TabIndex = 10;
		this.cbR2.Text = "2";
		this.toolTipMain.SetToolTip(this.cbR2, "Select abnormal R2");
		this.cbR2.UseVisualStyleBackColor = true;
		this.cbR1.AutoSize = true;
		this.cbR1.Checked = true;
		this.cbR1.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cbR1.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbR1.Location = new System.Drawing.Point(433, 4);
		this.cbR1.Name = "cbR1";
		this.cbR1.Size = new System.Drawing.Size(33, 20);
		this.cbR1.TabIndex = 9;
		this.cbR1.Text = "1";
		this.toolTipMain.SetToolTip(this.cbR1, "Select abnormal R1");
		this.cbR1.UseVisualStyleBackColor = true;
		this.cbXtb8.AutoSize = true;
		this.cbXtb8.Checked = true;
		this.cbXtb8.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cbXtb8.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbXtb8.Location = new System.Drawing.Point(313, 4);
		this.cbXtb8.Name = "cbXtb8";
		this.cbXtb8.Size = new System.Drawing.Size(33, 20);
		this.cbXtb8.TabIndex = 8;
		this.cbXtb8.Text = "8";
		this.toolTipMain.SetToolTip(this.cbXtb8, "Select abnormal X\u00af 8");
		this.cbXtb8.UseVisualStyleBackColor = true;
		this.cbXtb7.AutoSize = true;
		this.cbXtb7.Checked = true;
		this.cbXtb7.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cbXtb7.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbXtb7.Location = new System.Drawing.Point(273, 4);
		this.cbXtb7.Name = "cbXtb7";
		this.cbXtb7.Size = new System.Drawing.Size(33, 20);
		this.cbXtb7.TabIndex = 7;
		this.cbXtb7.Text = "7";
		this.toolTipMain.SetToolTip(this.cbXtb7, "Select abnormal X\u00af 7");
		this.cbXtb7.UseVisualStyleBackColor = true;
		this.cbXtb6.AutoSize = true;
		this.cbXtb6.Checked = true;
		this.cbXtb6.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cbXtb6.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbXtb6.Location = new System.Drawing.Point(233, 4);
		this.cbXtb6.Name = "cbXtb6";
		this.cbXtb6.Size = new System.Drawing.Size(33, 20);
		this.cbXtb6.TabIndex = 6;
		this.cbXtb6.Text = "6";
		this.toolTipMain.SetToolTip(this.cbXtb6, "Select abnormal X\u00af 6");
		this.cbXtb6.UseVisualStyleBackColor = true;
		this.cbXtb5.AutoSize = true;
		this.cbXtb5.Checked = true;
		this.cbXtb5.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cbXtb5.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbXtb5.Location = new System.Drawing.Point(193, 4);
		this.cbXtb5.Name = "cbXtb5";
		this.cbXtb5.Size = new System.Drawing.Size(33, 20);
		this.cbXtb5.TabIndex = 5;
		this.cbXtb5.Text = "5";
		this.toolTipMain.SetToolTip(this.cbXtb5, "Select abnormal X\u00af 5");
		this.cbXtb5.UseVisualStyleBackColor = true;
		this.cbXtb4.AutoSize = true;
		this.cbXtb4.Checked = true;
		this.cbXtb4.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cbXtb4.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbXtb4.Location = new System.Drawing.Point(153, 4);
		this.cbXtb4.Name = "cbXtb4";
		this.cbXtb4.Size = new System.Drawing.Size(33, 20);
		this.cbXtb4.TabIndex = 4;
		this.cbXtb4.Text = "4";
		this.toolTipMain.SetToolTip(this.cbXtb4, "Select abnormal X\u00af 4");
		this.cbXtb4.UseVisualStyleBackColor = true;
		this.cbXtb3.AutoSize = true;
		this.cbXtb3.Checked = true;
		this.cbXtb3.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cbXtb3.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbXtb3.Location = new System.Drawing.Point(113, 4);
		this.cbXtb3.Name = "cbXtb3";
		this.cbXtb3.Size = new System.Drawing.Size(33, 20);
		this.cbXtb3.TabIndex = 3;
		this.cbXtb3.Text = "3";
		this.toolTipMain.SetToolTip(this.cbXtb3, "Select abnormal X\u00af 3");
		this.cbXtb3.UseVisualStyleBackColor = true;
		this.cbXtb2.AutoSize = true;
		this.cbXtb2.Checked = true;
		this.cbXtb2.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cbXtb2.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbXtb2.Location = new System.Drawing.Point(73, 4);
		this.cbXtb2.Name = "cbXtb2";
		this.cbXtb2.Size = new System.Drawing.Size(33, 20);
		this.cbXtb2.TabIndex = 2;
		this.cbXtb2.Text = "2";
		this.toolTipMain.SetToolTip(this.cbXtb2, "Select abnormal X\u00af 2");
		this.cbXtb2.UseVisualStyleBackColor = true;
		this.cbXtb1.AutoSize = true;
		this.cbXtb1.Checked = true;
		this.cbXtb1.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cbXtb1.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbXtb1.Location = new System.Drawing.Point(33, 4);
		this.cbXtb1.Name = "cbXtb1";
		this.cbXtb1.Size = new System.Drawing.Size(33, 20);
		this.cbXtb1.TabIndex = 1;
		this.cbXtb1.Text = "1";
		this.toolTipMain.SetToolTip(this.cbXtb1, "Select abnormal X\u00af 1");
		this.cbXtb1.UseVisualStyleBackColor = true;
		this.cbAbnormalActive.AutoSize = true;
		this.cbAbnormalActive.Checked = true;
		this.cbAbnormalActive.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cbAbnormalActive.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbAbnormalActive.Location = new System.Drawing.Point(74, 4);
		this.cbAbnormalActive.Name = "cbAbnormalActive";
		this.cbAbnormalActive.Size = new System.Drawing.Size(15, 14);
		this.cbAbnormalActive.TabIndex = 1;
		this.toolTipMain.SetToolTip(this.cbAbnormalActive, "Select send mail for abnormal");
		this.cbAbnormalActive.UseVisualStyleBackColor = true;
		this.cbOKNGActive.AutoSize = true;
		this.cbOKNGActive.Checked = true;
		this.cbOKNGActive.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cbOKNGActive.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbOKNGActive.Location = new System.Drawing.Point(74, 4);
		this.cbOKNGActive.Name = "cbOKNGActive";
		this.cbOKNGActive.Size = new System.Drawing.Size(15, 14);
		this.cbOKNGActive.TabIndex = 1;
		this.toolTipMain.SetToolTip(this.cbOKNGActive, "Select send mail for OK/NG");
		this.cbOKNGActive.UseVisualStyleBackColor = true;
		this.cbManager.AutoSize = true;
		this.cbManager.Checked = true;
		this.cbManager.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cbManager.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbManager.Location = new System.Drawing.Point(72, 4);
		this.cbManager.Name = "cbManager";
		this.cbManager.Size = new System.Drawing.Size(15, 14);
		this.cbManager.TabIndex = 1;
		this.toolTipMain.SetToolTip(this.cbManager, "Select send mail for manager");
		this.cbManager.UseVisualStyleBackColor = true;
		this.cbLeader.AutoSize = true;
		this.cbLeader.Checked = true;
		this.cbLeader.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cbLeader.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbLeader.Location = new System.Drawing.Point(151, 4);
		this.cbLeader.Name = "cbLeader";
		this.cbLeader.Size = new System.Drawing.Size(15, 14);
		this.cbLeader.TabIndex = 2;
		this.toolTipMain.SetToolTip(this.cbLeader, "Select send mail for leader");
		this.cbLeader.UseVisualStyleBackColor = true;
		this.cbStaff.AutoSize = true;
		this.cbStaff.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbStaff.Location = new System.Drawing.Point(213, 4);
		this.cbStaff.Name = "cbStaff";
		this.cbStaff.Size = new System.Drawing.Size(15, 14);
		this.cbStaff.TabIndex = 3;
		this.toolTipMain.SetToolTip(this.cbStaff, "Select send mail for staff");
		this.cbStaff.UseVisualStyleBackColor = true;
		this.cbDepartment.AutoSize = true;
		this.cbDepartment.Checked = true;
		this.cbDepartment.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cbDepartment.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbDepartment.Location = new System.Drawing.Point(347, 4);
		this.cbDepartment.Name = "cbDepartment";
		this.cbDepartment.Size = new System.Drawing.Size(15, 14);
		this.cbDepartment.TabIndex = 4;
		this.toolTipMain.SetToolTip(this.cbDepartment, "Select send mail for department only");
		this.cbDepartment.UseVisualStyleBackColor = true;
		this.txtRank3.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtRank3.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtRank3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtRank3.Location = new System.Drawing.Point(85, 4);
		this.txtRank3.Name = "txtRank3";
		this.txtRank3.Size = new System.Drawing.Size(507, 22);
		this.txtRank3.TabIndex = 1;
		this.toolTipMain.SetToolTip(this.txtRank3, "Please enter percentage sample increase of RANK3");
		this.txtRank3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtRank_KeyPress);
		this.txtRank3.Leave += new System.EventHandler(txtRank_Leave);
		this.txtRank4.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtRank4.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtRank4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtRank4.Location = new System.Drawing.Point(85, 33);
		this.txtRank4.Name = "txtRank4";
		this.txtRank4.Size = new System.Drawing.Size(507, 22);
		this.txtRank4.TabIndex = 2;
		this.toolTipMain.SetToolTip(this.txtRank4, "Please enter percentage sample increase of RANK4");
		this.txtRank4.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtRank_KeyPress);
		this.txtRank4.Leave += new System.EventHandler(txtRank_Leave);
		this.txtRank5.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtRank5.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtRank5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtRank5.Location = new System.Drawing.Point(85, 62);
		this.txtRank5.Name = "txtRank5";
		this.txtRank5.Size = new System.Drawing.Size(507, 22);
		this.txtRank5.TabIndex = 3;
		this.toolTipMain.SetToolTip(this.txtRank5, "Please enter percentage sample increase of RANK5");
		this.txtRank5.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtRank_KeyPress);
		this.txtRank5.Leave += new System.EventHandler(txtRank_Leave);
		this.btnFolder.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnFolder.BackColor = System.Drawing.SystemColors.Control;
		this.btnFolder.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnFolder.FlatAppearance.BorderSize = 0;
		this.btnFolder.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
		this.btnFolder.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.AppWorkspace;
		this.btnFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnFolder.Image = _5S_QA_System.Properties.Resources.folder;
		this.btnFolder.Location = new System.Drawing.Point(473, 1);
		this.btnFolder.Margin = new System.Windows.Forms.Padding(0);
		this.btnFolder.Name = "btnFolder";
		this.btnFolder.Size = new System.Drawing.Size(20, 20);
		this.btnFolder.TabIndex = 2;
		this.toolTipMain.SetToolTip(this.btnFolder, "Open to select a folder");
		this.btnFolder.UseVisualStyleBackColor = false;
		this.btnFolder.Click += new System.EventHandler(btnFolder_Click);
		this.txtFolder.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtFolder.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtFolder.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtFolder.Location = new System.Drawing.Point(0, 0);
		this.txtFolder.Name = "txtFolder";
		this.txtFolder.Size = new System.Drawing.Size(496, 22);
		this.txtFolder.TabIndex = 1;
		this.toolTipMain.SetToolTip(this.txtFolder, "Please enter password");
		this.btnProtectEye.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnProtectEye.BackColor = System.Drawing.SystemColors.Control;
		this.btnProtectEye.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnProtectEye.FlatAppearance.BorderSize = 0;
		this.btnProtectEye.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
		this.btnProtectEye.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.AppWorkspace;
		this.btnProtectEye.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnProtectEye.Image = _5S_QA_System.Properties.Resources.eye_black;
		this.btnProtectEye.Location = new System.Drawing.Point(473, 1);
		this.btnProtectEye.Margin = new System.Windows.Forms.Padding(0);
		this.btnProtectEye.Name = "btnProtectEye";
		this.btnProtectEye.Size = new System.Drawing.Size(20, 20);
		this.btnProtectEye.TabIndex = 2;
		this.toolTipMain.SetToolTip(this.btnProtectEye, "View password");
		this.btnProtectEye.UseVisualStyleBackColor = false;
		this.btnProtectEye.Visible = false;
		this.btnProtectEye.MouseDown += new System.Windows.Forms.MouseEventHandler(btnProtectEye_MouseDown);
		this.btnProtectEye.MouseUp += new System.Windows.Forms.MouseEventHandler(btnProtectEye_MouseUp);
		this.txtProtect.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtProtect.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtProtect.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtProtect.Location = new System.Drawing.Point(0, 0);
		this.txtProtect.Name = "txtProtect";
		this.txtProtect.Size = new System.Drawing.Size(496, 22);
		this.txtProtect.TabIndex = 1;
		this.toolTipMain.SetToolTip(this.txtProtect, "Please enter password protect sheet");
		this.txtProtect.UseSystemPasswordChar = true;
		this.txtProtect.TextChanged += new System.EventHandler(txtProtect_TextChanged);
		this.cbIsActivated.AutoSize = true;
		this.cbIsActivated.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbIsActivated.Dock = System.Windows.Forms.DockStyle.Left;
		this.cbIsActivated.Location = new System.Drawing.Point(96, 62);
		this.cbIsActivated.Name = "cbIsActivated";
		this.cbIsActivated.Padding = new System.Windows.Forms.Padding(0, 3, 3, 3);
		this.cbIsActivated.Size = new System.Drawing.Size(18, 20);
		this.cbIsActivated.TabIndex = 3;
		this.toolTipMain.SetToolTip(this.cbIsActivated, "Select is activated");
		this.cbIsActivated.UseVisualStyleBackColor = true;
		this.cbCompleted.AutoSize = true;
		this.cbCompleted.Checked = true;
		this.cbCompleted.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cbCompleted.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbCompleted.Location = new System.Drawing.Point(174, 4);
		this.cbCompleted.Name = "cbCompleted";
		this.cbCompleted.Size = new System.Drawing.Size(15, 14);
		this.cbCompleted.TabIndex = 3;
		this.toolTipMain.SetToolTip(this.cbCompleted, "Select send mail for leader");
		this.cbCompleted.UseVisualStyleBackColor = true;
		this.cbChecked.AutoSize = true;
		this.cbChecked.Checked = true;
		this.cbChecked.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cbChecked.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbChecked.Location = new System.Drawing.Point(72, 4);
		this.cbChecked.Name = "cbChecked";
		this.cbChecked.Size = new System.Drawing.Size(15, 14);
		this.cbChecked.TabIndex = 2;
		this.toolTipMain.SetToolTip(this.cbChecked, "Select send mail for manager");
		this.cbChecked.UseVisualStyleBackColor = true;
		this.statusStripMain.BackColor = System.Drawing.SystemColors.ControlLight;
		this.statusStripMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.statusStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.sprogbarStatus, this.slblStatus });
		this.statusStripMain.Location = new System.Drawing.Point(20, 504);
		this.statusStripMain.Name = "statusStripMain";
		this.statusStripMain.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
		this.statusStripMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
		this.statusStripMain.Size = new System.Drawing.Size(610, 26);
		this.statusStripMain.SizingGrip = false;
		this.statusStripMain.Stretch = false;
		this.statusStripMain.TabIndex = 5;
		this.sprogbarStatus.Name = "sprogbarStatus";
		this.sprogbarStatus.Size = new System.Drawing.Size(200, 20);
		this.slblStatus.Name = "slblStatus";
		this.slblStatus.Size = new System.Drawing.Size(16, 21);
		this.slblStatus.Text = "...";
		this.panelLogout.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.panelLogout.BackColor = System.Drawing.Color.Transparent;
		this.panelLogout.Controls.Add(this.lblFullname);
		this.panelLogout.Controls.Add(this.ptbAvatar);
		this.panelLogout.Location = new System.Drawing.Point(280, 27);
		this.panelLogout.Name = "panelLogout";
		this.panelLogout.Size = new System.Drawing.Size(350, 42);
		this.panelLogout.TabIndex = 174;
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
		this.tabControlMain.Controls.Add(this.tabPageWarning);
		this.tabControlMain.Controls.Add(this.tabPageEmailSend);
		this.tabControlMain.Controls.Add(this.tabPageRank);
		this.tabControlMain.Controls.Add(this.tabPageDirectory);
		this.tabControlMain.Controls.Add(this.tabPageNotification);
		this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControlMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControlMain.Location = new System.Drawing.Point(20, 70);
		this.tabControlMain.Name = "tabControlMain";
		this.tabControlMain.SelectedIndex = 0;
		this.tabControlMain.Size = new System.Drawing.Size(610, 406);
		this.tabControlMain.TabIndex = 175;
		this.tabPageWarning.ContextMenuStrip = this.contextMenuStripWarning;
		this.tabPageWarning.Controls.Add(this.gbOKNG);
		this.tabPageWarning.Controls.Add(this.gbAbnormal);
		this.tabPageWarning.Controls.Add(this.gbPosition);
		this.tabPageWarning.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabPageWarning.Location = new System.Drawing.Point(4, 25);
		this.tabPageWarning.Name = "tabPageWarning";
		this.tabPageWarning.Padding = new System.Windows.Forms.Padding(3);
		this.tabPageWarning.Size = new System.Drawing.Size(602, 377);
		this.tabPageWarning.TabIndex = 0;
		this.tabPageWarning.Text = "Warning";
		this.tabPageWarning.UseVisualStyleBackColor = true;
		this.contextMenuStripWarning.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.main_refreshToolStripMenuItem });
		this.contextMenuStripWarning.Name = "contextMenuStripWarning";
		this.contextMenuStripWarning.Size = new System.Drawing.Size(114, 26);
		this.main_refreshToolStripMenuItem.Name = "main_refreshToolStripMenuItem";
		this.main_refreshToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
		this.main_refreshToolStripMenuItem.Text = "Refresh";
		this.main_refreshToolStripMenuItem.Click += new System.EventHandler(main_refreshToolStripMenuItem_Click);
		this.gbOKNG.AutoSize = true;
		this.gbOKNG.Controls.Add(this.tpanelOKNG);
		this.gbOKNG.Dock = System.Windows.Forms.DockStyle.Top;
		this.gbOKNG.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
		this.gbOKNG.Location = new System.Drawing.Point(3, 117);
		this.gbOKNG.Name = "gbOKNG";
		this.gbOKNG.Size = new System.Drawing.Size(596, 43);
		this.gbOKNG.TabIndex = 3;
		this.gbOKNG.TabStop = false;
		this.gbOKNG.Text = "OK/NG";
		this.tpanelOKNG.AutoSize = true;
		this.tpanelOKNG.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		this.tpanelOKNG.ColumnCount = 3;
		this.tpanelOKNG.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelOKNG.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelOKNG.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tpanelOKNG.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tpanelOKNG.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tpanelOKNG.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tpanelOKNG.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tpanelOKNG.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tpanelOKNG.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tpanelOKNG.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tpanelOKNG.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tpanelOKNG.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tpanelOKNG.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tpanelOKNG.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tpanelOKNG.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tpanelOKNG.Controls.Add(this.lblOKNGActive, 0, 0);
		this.tpanelOKNG.Controls.Add(this.cbOKNGActive, 1, 0);
		this.tpanelOKNG.Dock = System.Windows.Forms.DockStyle.Top;
		this.tpanelOKNG.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tpanelOKNG.Location = new System.Drawing.Point(3, 18);
		this.tpanelOKNG.Name = "tpanelOKNG";
		this.tpanelOKNG.RowCount = 1;
		this.tpanelOKNG.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tpanelOKNG.Size = new System.Drawing.Size(590, 22);
		this.tpanelOKNG.TabIndex = 179;
		this.lblOKNGActive.AutoSize = true;
		this.lblOKNGActive.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblOKNGActive.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblOKNGActive.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lblOKNGActive.Location = new System.Drawing.Point(4, 1);
		this.lblOKNGActive.Name = "lblOKNGActive";
		this.lblOKNGActive.Size = new System.Drawing.Size(63, 20);
		this.lblOKNGActive.TabIndex = 154;
		this.lblOKNGActive.Text = "Activated";
		this.lblOKNGActive.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.gbAbnormal.AutoSize = true;
		this.gbAbnormal.Controls.Add(this.tpanelAbnormal);
		this.gbAbnormal.Controls.Add(this.tpanelAbnormalActive);
		this.gbAbnormal.Dock = System.Windows.Forms.DockStyle.Top;
		this.gbAbnormal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
		this.gbAbnormal.Location = new System.Drawing.Point(3, 46);
		this.gbAbnormal.Name = "gbAbnormal";
		this.gbAbnormal.Size = new System.Drawing.Size(596, 71);
		this.gbAbnormal.TabIndex = 2;
		this.gbAbnormal.TabStop = false;
		this.gbAbnormal.Text = "Abnormal";
		this.tpanelAbnormal.AutoSize = true;
		this.tpanelAbnormal.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		this.tpanelAbnormal.ColumnCount = 15;
		this.tpanelAbnormal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelAbnormal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelAbnormal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelAbnormal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelAbnormal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelAbnormal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelAbnormal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelAbnormal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelAbnormal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelAbnormal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tpanelAbnormal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelAbnormal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelAbnormal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelAbnormal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelAbnormal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelAbnormal.Controls.Add(this.cbR4, 14, 0);
		this.tpanelAbnormal.Controls.Add(this.cbR3, 13, 0);
		this.tpanelAbnormal.Controls.Add(this.cbR2, 12, 0);
		this.tpanelAbnormal.Controls.Add(this.cbR1, 11, 0);
		this.tpanelAbnormal.Controls.Add(this.cbXtb8, 8, 0);
		this.tpanelAbnormal.Controls.Add(this.cbXtb7, 7, 0);
		this.tpanelAbnormal.Controls.Add(this.cbXtb6, 6, 0);
		this.tpanelAbnormal.Controls.Add(this.cbXtb5, 5, 0);
		this.tpanelAbnormal.Controls.Add(this.cbXtb4, 4, 0);
		this.tpanelAbnormal.Controls.Add(this.cbXtb3, 3, 0);
		this.tpanelAbnormal.Controls.Add(this.cbXtb2, 2, 0);
		this.tpanelAbnormal.Controls.Add(this.label4, 0, 0);
		this.tpanelAbnormal.Controls.Add(this.label8, 10, 0);
		this.tpanelAbnormal.Controls.Add(this.cbXtb1, 1, 0);
		this.tpanelAbnormal.Dock = System.Windows.Forms.DockStyle.Top;
		this.tpanelAbnormal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tpanelAbnormal.Location = new System.Drawing.Point(3, 40);
		this.tpanelAbnormal.Name = "tpanelAbnormal";
		this.tpanelAbnormal.RowCount = 1;
		this.tpanelAbnormal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tpanelAbnormal.Size = new System.Drawing.Size(590, 28);
		this.tpanelAbnormal.TabIndex = 2;
		this.label4.AutoSize = true;
		this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label4.Location = new System.Drawing.Point(4, 1);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(22, 26);
		this.label4.TabIndex = 154;
		this.label4.Text = "X\u00af";
		this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label8.AutoSize = true;
		this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(409, 1);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(17, 26);
		this.label8.TabIndex = 149;
		this.label8.Text = "R";
		this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.tpanelAbnormalActive.AutoSize = true;
		this.tpanelAbnormalActive.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		this.tpanelAbnormalActive.ColumnCount = 3;
		this.tpanelAbnormalActive.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelAbnormalActive.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelAbnormalActive.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tpanelAbnormalActive.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tpanelAbnormalActive.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tpanelAbnormalActive.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tpanelAbnormalActive.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tpanelAbnormalActive.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tpanelAbnormalActive.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tpanelAbnormalActive.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tpanelAbnormalActive.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tpanelAbnormalActive.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tpanelAbnormalActive.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tpanelAbnormalActive.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tpanelAbnormalActive.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tpanelAbnormalActive.Controls.Add(this.lblAbnormalActive, 0, 0);
		this.tpanelAbnormalActive.Controls.Add(this.cbAbnormalActive, 1, 0);
		this.tpanelAbnormalActive.Dock = System.Windows.Forms.DockStyle.Top;
		this.tpanelAbnormalActive.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tpanelAbnormalActive.Location = new System.Drawing.Point(3, 18);
		this.tpanelAbnormalActive.Name = "tpanelAbnormalActive";
		this.tpanelAbnormalActive.RowCount = 1;
		this.tpanelAbnormalActive.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tpanelAbnormalActive.Size = new System.Drawing.Size(590, 22);
		this.tpanelAbnormalActive.TabIndex = 1;
		this.lblAbnormalActive.AutoSize = true;
		this.lblAbnormalActive.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblAbnormalActive.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblAbnormalActive.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lblAbnormalActive.Location = new System.Drawing.Point(4, 1);
		this.lblAbnormalActive.Name = "lblAbnormalActive";
		this.lblAbnormalActive.Size = new System.Drawing.Size(63, 20);
		this.lblAbnormalActive.TabIndex = 154;
		this.lblAbnormalActive.Text = "Activated";
		this.lblAbnormalActive.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.gbPosition.AutoSize = true;
		this.gbPosition.Controls.Add(this.tpanelPosition);
		this.gbPosition.Dock = System.Windows.Forms.DockStyle.Top;
		this.gbPosition.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
		this.gbPosition.Location = new System.Drawing.Point(3, 3);
		this.gbPosition.Name = "gbPosition";
		this.gbPosition.Size = new System.Drawing.Size(596, 43);
		this.gbPosition.TabIndex = 1;
		this.gbPosition.TabStop = false;
		this.gbPosition.Text = "Position";
		this.tpanelPosition.AutoSize = true;
		this.tpanelPosition.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		this.tpanelPosition.ColumnCount = 9;
		this.tpanelPosition.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelPosition.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelPosition.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelPosition.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelPosition.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelPosition.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelPosition.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelPosition.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelPosition.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tpanelPosition.Controls.Add(this.cbDepartment, 7, 0);
		this.tpanelPosition.Controls.Add(this.lblDepartment, 6, 0);
		this.tpanelPosition.Controls.Add(this.cbStaff, 5, 0);
		this.tpanelPosition.Controls.Add(this.cbLeader, 3, 0);
		this.tpanelPosition.Controls.Add(this.label3, 4, 0);
		this.tpanelPosition.Controls.Add(this.label2, 2, 0);
		this.tpanelPosition.Controls.Add(this.label1, 0, 0);
		this.tpanelPosition.Controls.Add(this.cbManager, 1, 0);
		this.tpanelPosition.Dock = System.Windows.Forms.DockStyle.Top;
		this.tpanelPosition.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tpanelPosition.Location = new System.Drawing.Point(3, 18);
		this.tpanelPosition.Name = "tpanelPosition";
		this.tpanelPosition.RowCount = 1;
		this.tpanelPosition.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tpanelPosition.Size = new System.Drawing.Size(590, 22);
		this.tpanelPosition.TabIndex = 179;
		this.lblDepartment.AutoSize = true;
		this.lblDepartment.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblDepartment.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblDepartment.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lblDepartment.Location = new System.Drawing.Point(235, 1);
		this.lblDepartment.Name = "lblDepartment";
		this.lblDepartment.Size = new System.Drawing.Size(105, 20);
		this.lblDepartment.TabIndex = 157;
		this.lblDepartment.Text = "Department only";
		this.lblDepartment.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label3.AutoSize = true;
		this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label3.Location = new System.Drawing.Point(173, 1);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(33, 20);
		this.label3.TabIndex = 156;
		this.label3.Text = "Staff";
		this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label2.AutoSize = true;
		this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label2.Location = new System.Drawing.Point(94, 1);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(50, 20);
		this.label2.TabIndex = 155;
		this.label2.Text = "Leader";
		this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label1.AutoSize = true;
		this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label1.Location = new System.Drawing.Point(4, 1);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(61, 20);
		this.label1.TabIndex = 154;
		this.label1.Text = "Manager";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.tabPageEmailSend.AutoScroll = true;
		this.tabPageEmailSend.ContextMenuStrip = this.contextMenuStripWarning;
		this.tabPageEmailSend.Controls.Add(this.tpanelEmailSend);
		this.tabPageEmailSend.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabPageEmailSend.Location = new System.Drawing.Point(4, 25);
		this.tabPageEmailSend.Name = "tabPageEmailSend";
		this.tabPageEmailSend.Padding = new System.Windows.Forms.Padding(3);
		this.tabPageEmailSend.Size = new System.Drawing.Size(602, 377);
		this.tabPageEmailSend.TabIndex = 1;
		this.tabPageEmailSend.Text = "Email send";
		this.tabPageEmailSend.UseVisualStyleBackColor = true;
		this.tpanelEmailSend.AutoSize = true;
		this.tpanelEmailSend.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		this.tpanelEmailSend.ColumnCount = 2;
		this.tpanelEmailSend.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelEmailSend.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tpanelEmailSend.Controls.Add(this.rtbFooter, 1, 3);
		this.tpanelEmailSend.Controls.Add(this.panelPassword, 1, 1);
		this.tpanelEmailSend.Controls.Add(this.txtEmail, 1, 0);
		this.tpanelEmailSend.Controls.Add(this.lblFooter, 0, 3);
		this.tpanelEmailSend.Controls.Add(this.lblHeader, 0, 2);
		this.tpanelEmailSend.Controls.Add(this.lblModifiedBy, 1, 8);
		this.tpanelEmailSend.Controls.Add(this.lblCreatedBy, 1, 7);
		this.tpanelEmailSend.Controls.Add(this.lblModified, 1, 6);
		this.tpanelEmailSend.Controls.Add(this.lblCreated, 1, 5);
		this.tpanelEmailSend.Controls.Add(this.lblModifiBy, 0, 8);
		this.tpanelEmailSend.Controls.Add(this.lblCreateBy, 0, 7);
		this.tpanelEmailSend.Controls.Add(this.lblModifi, 0, 6);
		this.tpanelEmailSend.Controls.Add(this.lblCreate, 0, 5);
		this.tpanelEmailSend.Controls.Add(this.lblPassword, 0, 1);
		this.tpanelEmailSend.Controls.Add(this.lblEmail, 0, 0);
		this.tpanelEmailSend.Controls.Add(this.rtbHeader, 1, 2);
		this.tpanelEmailSend.Dock = System.Windows.Forms.DockStyle.Top;
		this.tpanelEmailSend.Location = new System.Drawing.Point(3, 3);
		this.tpanelEmailSend.Margin = new System.Windows.Forms.Padding(4);
		this.tpanelEmailSend.Name = "tpanelEmailSend";
		this.tpanelEmailSend.RowCount = 9;
		this.tpanelEmailSend.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tpanelEmailSend.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tpanelEmailSend.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60f));
		this.tpanelEmailSend.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60f));
		this.tpanelEmailSend.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0f));
		this.tpanelEmailSend.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tpanelEmailSend.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tpanelEmailSend.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tpanelEmailSend.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tpanelEmailSend.Size = new System.Drawing.Size(596, 298);
		this.tpanelEmailSend.TabIndex = 18;
		this.panelPassword.Controls.Add(this.btnEye);
		this.panelPassword.Controls.Add(this.txtPassword);
		this.panelPassword.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panelPassword.Location = new System.Drawing.Point(88, 33);
		this.panelPassword.Name = "panelPassword";
		this.panelPassword.Size = new System.Drawing.Size(504, 22);
		this.panelPassword.TabIndex = 2;
		this.panelPassword.TabStop = true;
		this.lblFooter.AutoSize = true;
		this.lblFooter.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblFooter.Location = new System.Drawing.Point(4, 120);
		this.lblFooter.Name = "lblFooter";
		this.lblFooter.Size = new System.Drawing.Size(77, 60);
		this.lblFooter.TabIndex = 85;
		this.lblFooter.Text = "Footer";
		this.lblFooter.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblHeader.AutoSize = true;
		this.lblHeader.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblHeader.Location = new System.Drawing.Point(4, 59);
		this.lblHeader.Name = "lblHeader";
		this.lblHeader.Size = new System.Drawing.Size(77, 60);
		this.lblHeader.TabIndex = 84;
		this.lblHeader.Text = "Header";
		this.lblHeader.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblModifiedBy.AutoSize = true;
		this.lblModifiedBy.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblModifiedBy.Location = new System.Drawing.Point(88, 269);
		this.lblModifiedBy.Name = "lblModifiedBy";
		this.lblModifiedBy.Size = new System.Drawing.Size(504, 28);
		this.lblModifiedBy.TabIndex = 83;
		this.lblModifiedBy.Text = "...";
		this.lblModifiedBy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblCreatedBy.AutoSize = true;
		this.lblCreatedBy.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCreatedBy.Location = new System.Drawing.Point(88, 240);
		this.lblCreatedBy.Name = "lblCreatedBy";
		this.lblCreatedBy.Size = new System.Drawing.Size(504, 28);
		this.lblCreatedBy.TabIndex = 82;
		this.lblCreatedBy.Text = "...";
		this.lblCreatedBy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblModified.AutoSize = true;
		this.lblModified.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblModified.Location = new System.Drawing.Point(88, 211);
		this.lblModified.Name = "lblModified";
		this.lblModified.Size = new System.Drawing.Size(504, 28);
		this.lblModified.TabIndex = 81;
		this.lblModified.Text = "...";
		this.lblModified.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblCreated.AutoSize = true;
		this.lblCreated.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCreated.Location = new System.Drawing.Point(88, 182);
		this.lblCreated.Name = "lblCreated";
		this.lblCreated.Size = new System.Drawing.Size(504, 28);
		this.lblCreated.TabIndex = 80;
		this.lblCreated.Text = "...";
		this.lblCreated.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblModifiBy.AutoSize = true;
		this.lblModifiBy.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblModifiBy.Location = new System.Drawing.Point(4, 269);
		this.lblModifiBy.Name = "lblModifiBy";
		this.lblModifiBy.Size = new System.Drawing.Size(77, 28);
		this.lblModifiBy.TabIndex = 79;
		this.lblModifiBy.Text = "Edited by";
		this.lblModifiBy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblCreateBy.AutoSize = true;
		this.lblCreateBy.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCreateBy.Location = new System.Drawing.Point(4, 240);
		this.lblCreateBy.Name = "lblCreateBy";
		this.lblCreateBy.Size = new System.Drawing.Size(77, 28);
		this.lblCreateBy.TabIndex = 78;
		this.lblCreateBy.Text = "Created by";
		this.lblCreateBy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblModifi.AutoSize = true;
		this.lblModifi.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblModifi.Location = new System.Drawing.Point(4, 211);
		this.lblModifi.Name = "lblModifi";
		this.lblModifi.Size = new System.Drawing.Size(77, 28);
		this.lblModifi.TabIndex = 77;
		this.lblModifi.Text = "Edited date";
		this.lblModifi.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblCreate.AutoSize = true;
		this.lblCreate.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCreate.Location = new System.Drawing.Point(4, 182);
		this.lblCreate.Name = "lblCreate";
		this.lblCreate.Size = new System.Drawing.Size(77, 28);
		this.lblCreate.TabIndex = 76;
		this.lblCreate.Text = "Create date";
		this.lblCreate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblPassword.AutoSize = true;
		this.lblPassword.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblPassword.Location = new System.Drawing.Point(4, 30);
		this.lblPassword.Name = "lblPassword";
		this.lblPassword.Size = new System.Drawing.Size(77, 28);
		this.lblPassword.TabIndex = 21;
		this.lblPassword.Text = "Password";
		this.lblPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblEmail.AutoSize = true;
		this.lblEmail.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblEmail.Location = new System.Drawing.Point(4, 1);
		this.lblEmail.Name = "lblEmail";
		this.lblEmail.Size = new System.Drawing.Size(77, 28);
		this.lblEmail.TabIndex = 20;
		this.lblEmail.Text = "Email";
		this.lblEmail.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.tabPageRank.ContextMenuStrip = this.contextMenuStripWarning;
		this.tabPageRank.Controls.Add(this.tpanelRank);
		this.tabPageRank.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabPageRank.Location = new System.Drawing.Point(4, 25);
		this.tabPageRank.Name = "tabPageRank";
		this.tabPageRank.Padding = new System.Windows.Forms.Padding(3);
		this.tabPageRank.Size = new System.Drawing.Size(602, 377);
		this.tabPageRank.TabIndex = 2;
		this.tabPageRank.Text = "Rank";
		this.tabPageRank.UseVisualStyleBackColor = true;
		this.tpanelRank.AutoSize = true;
		this.tpanelRank.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		this.tpanelRank.ColumnCount = 2;
		this.tpanelRank.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelRank.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tpanelRank.Controls.Add(this.txtRank5, 1, 2);
		this.tpanelRank.Controls.Add(this.txtRank4, 1, 1);
		this.tpanelRank.Controls.Add(this.txtRank3, 1, 0);
		this.tpanelRank.Controls.Add(this.label6, 0, 2);
		this.tpanelRank.Controls.Add(this.label16, 0, 1);
		this.tpanelRank.Controls.Add(this.label17, 0, 0);
		this.tpanelRank.Dock = System.Windows.Forms.DockStyle.Top;
		this.tpanelRank.Location = new System.Drawing.Point(3, 3);
		this.tpanelRank.Margin = new System.Windows.Forms.Padding(4);
		this.tpanelRank.Name = "tpanelRank";
		this.tpanelRank.RowCount = 3;
		this.tpanelRank.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tpanelRank.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tpanelRank.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tpanelRank.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tpanelRank.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tpanelRank.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tpanelRank.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tpanelRank.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tpanelRank.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tpanelRank.Size = new System.Drawing.Size(596, 88);
		this.tpanelRank.TabIndex = 19;
		this.label6.AutoSize = true;
		this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label6.Location = new System.Drawing.Point(4, 59);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(74, 28);
		this.label6.TabIndex = 84;
		this.label6.Text = "RANK5 (%)";
		this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label16.AutoSize = true;
		this.label16.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label16.Location = new System.Drawing.Point(4, 30);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(74, 28);
		this.label16.TabIndex = 21;
		this.label16.Text = "RANK4 (%)";
		this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label17.AutoSize = true;
		this.label17.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label17.Location = new System.Drawing.Point(4, 1);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(74, 28);
		this.label17.TabIndex = 20;
		this.label17.Text = "RANK3 (%)";
		this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.tabPageDirectory.ContextMenuStrip = this.contextMenuStripWarning;
		this.tabPageDirectory.Controls.Add(this.tpanelDirectory);
		this.tabPageDirectory.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabPageDirectory.Location = new System.Drawing.Point(4, 25);
		this.tabPageDirectory.Name = "tabPageDirectory";
		this.tabPageDirectory.Padding = new System.Windows.Forms.Padding(3);
		this.tabPageDirectory.Size = new System.Drawing.Size(602, 377);
		this.tabPageDirectory.TabIndex = 3;
		this.tabPageDirectory.Text = "Directory";
		this.tabPageDirectory.UseVisualStyleBackColor = true;
		this.tpanelDirectory.AutoSize = true;
		this.tpanelDirectory.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		this.tpanelDirectory.ColumnCount = 2;
		this.tpanelDirectory.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelDirectory.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tpanelDirectory.Controls.Add(this.lblIsActivated, 0, 2);
		this.tpanelDirectory.Controls.Add(this.panelFolder, 1, 0);
		this.tpanelDirectory.Controls.Add(this.panel1, 1, 1);
		this.tpanelDirectory.Controls.Add(this.lblDirModifiedBy, 1, 7);
		this.tpanelDirectory.Controls.Add(this.lblDirCreatedBy, 1, 6);
		this.tpanelDirectory.Controls.Add(this.lblDirModified, 1, 5);
		this.tpanelDirectory.Controls.Add(this.lblDirCreated, 1, 4);
		this.tpanelDirectory.Controls.Add(this.lblDirModifiBy, 0, 7);
		this.tpanelDirectory.Controls.Add(this.lblDirCreateBy, 0, 6);
		this.tpanelDirectory.Controls.Add(this.lblDirModifi, 0, 5);
		this.tpanelDirectory.Controls.Add(this.lblDirCreate, 0, 4);
		this.tpanelDirectory.Controls.Add(this.lblProtect, 0, 1);
		this.tpanelDirectory.Controls.Add(this.lblFolder, 0, 0);
		this.tpanelDirectory.Controls.Add(this.cbIsActivated, 1, 2);
		this.tpanelDirectory.Dock = System.Windows.Forms.DockStyle.Top;
		this.tpanelDirectory.Location = new System.Drawing.Point(3, 3);
		this.tpanelDirectory.Margin = new System.Windows.Forms.Padding(4);
		this.tpanelDirectory.Name = "tpanelDirectory";
		this.tpanelDirectory.RowCount = 8;
		this.tpanelDirectory.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tpanelDirectory.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tpanelDirectory.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tpanelDirectory.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0f));
		this.tpanelDirectory.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tpanelDirectory.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tpanelDirectory.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tpanelDirectory.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tpanelDirectory.Size = new System.Drawing.Size(596, 203);
		this.tpanelDirectory.TabIndex = 3;
		this.lblIsActivated.AutoSize = true;
		this.lblIsActivated.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblIsActivated.Location = new System.Drawing.Point(4, 59);
		this.lblIsActivated.Name = "lblIsActivated";
		this.lblIsActivated.Size = new System.Drawing.Size(85, 26);
		this.lblIsActivated.TabIndex = 84;
		this.lblIsActivated.Text = "Is activated";
		this.lblIsActivated.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.panelFolder.Controls.Add(this.btnFolder);
		this.panelFolder.Controls.Add(this.txtFolder);
		this.panelFolder.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panelFolder.Location = new System.Drawing.Point(96, 4);
		this.panelFolder.Name = "panelFolder";
		this.panelFolder.Size = new System.Drawing.Size(496, 22);
		this.panelFolder.TabIndex = 1;
		this.panelFolder.TabStop = true;
		this.panel1.Controls.Add(this.btnProtectEye);
		this.panel1.Controls.Add(this.txtProtect);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(96, 33);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(496, 22);
		this.panel1.TabIndex = 2;
		this.panel1.TabStop = true;
		this.lblDirModifiedBy.AutoSize = true;
		this.lblDirModifiedBy.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblDirModifiedBy.Location = new System.Drawing.Point(96, 174);
		this.lblDirModifiedBy.Name = "lblDirModifiedBy";
		this.lblDirModifiedBy.Size = new System.Drawing.Size(496, 28);
		this.lblDirModifiedBy.TabIndex = 83;
		this.lblDirModifiedBy.Text = "...";
		this.lblDirModifiedBy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblDirCreatedBy.AutoSize = true;
		this.lblDirCreatedBy.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblDirCreatedBy.Location = new System.Drawing.Point(96, 145);
		this.lblDirCreatedBy.Name = "lblDirCreatedBy";
		this.lblDirCreatedBy.Size = new System.Drawing.Size(496, 28);
		this.lblDirCreatedBy.TabIndex = 82;
		this.lblDirCreatedBy.Text = "...";
		this.lblDirCreatedBy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblDirModified.AutoSize = true;
		this.lblDirModified.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblDirModified.Location = new System.Drawing.Point(96, 116);
		this.lblDirModified.Name = "lblDirModified";
		this.lblDirModified.Size = new System.Drawing.Size(496, 28);
		this.lblDirModified.TabIndex = 81;
		this.lblDirModified.Text = "...";
		this.lblDirModified.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblDirCreated.AutoSize = true;
		this.lblDirCreated.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblDirCreated.Location = new System.Drawing.Point(96, 87);
		this.lblDirCreated.Name = "lblDirCreated";
		this.lblDirCreated.Size = new System.Drawing.Size(496, 28);
		this.lblDirCreated.TabIndex = 80;
		this.lblDirCreated.Text = "...";
		this.lblDirCreated.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblDirModifiBy.AutoSize = true;
		this.lblDirModifiBy.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblDirModifiBy.Location = new System.Drawing.Point(4, 174);
		this.lblDirModifiBy.Name = "lblDirModifiBy";
		this.lblDirModifiBy.Size = new System.Drawing.Size(85, 28);
		this.lblDirModifiBy.TabIndex = 79;
		this.lblDirModifiBy.Text = "Edited by";
		this.lblDirModifiBy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblDirCreateBy.AutoSize = true;
		this.lblDirCreateBy.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblDirCreateBy.Location = new System.Drawing.Point(4, 145);
		this.lblDirCreateBy.Name = "lblDirCreateBy";
		this.lblDirCreateBy.Size = new System.Drawing.Size(85, 28);
		this.lblDirCreateBy.TabIndex = 78;
		this.lblDirCreateBy.Text = "Created by";
		this.lblDirCreateBy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblDirModifi.AutoSize = true;
		this.lblDirModifi.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblDirModifi.Location = new System.Drawing.Point(4, 116);
		this.lblDirModifi.Name = "lblDirModifi";
		this.lblDirModifi.Size = new System.Drawing.Size(85, 28);
		this.lblDirModifi.TabIndex = 77;
		this.lblDirModifi.Text = "Edited date";
		this.lblDirModifi.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblDirCreate.AutoSize = true;
		this.lblDirCreate.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblDirCreate.Location = new System.Drawing.Point(4, 87);
		this.lblDirCreate.Name = "lblDirCreate";
		this.lblDirCreate.Size = new System.Drawing.Size(85, 28);
		this.lblDirCreate.TabIndex = 76;
		this.lblDirCreate.Text = "Create date";
		this.lblDirCreate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblProtect.AutoSize = true;
		this.lblProtect.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblProtect.Location = new System.Drawing.Point(4, 30);
		this.lblProtect.Name = "lblProtect";
		this.lblProtect.Size = new System.Drawing.Size(85, 28);
		this.lblProtect.TabIndex = 21;
		this.lblProtect.Text = "Protect sheet";
		this.lblProtect.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblFolder.AutoSize = true;
		this.lblFolder.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblFolder.Location = new System.Drawing.Point(4, 1);
		this.lblFolder.Name = "lblFolder";
		this.lblFolder.Size = new System.Drawing.Size(85, 28);
		this.lblFolder.TabIndex = 20;
		this.lblFolder.Text = "Folder";
		this.lblFolder.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.tabPageNotification.ContextMenuStrip = this.contextMenuStripWarning;
		this.tabPageNotification.Controls.Add(this.gbRequestStatus);
		this.tabPageNotification.Location = new System.Drawing.Point(4, 25);
		this.tabPageNotification.Name = "tabPageNotification";
		this.tabPageNotification.Padding = new System.Windows.Forms.Padding(3);
		this.tabPageNotification.Size = new System.Drawing.Size(602, 377);
		this.tabPageNotification.TabIndex = 4;
		this.tabPageNotification.Text = "Notification";
		this.tabPageNotification.UseVisualStyleBackColor = true;
		this.gbRequestStatus.AutoSize = true;
		this.gbRequestStatus.Controls.Add(this.tableLayoutPanel1);
		this.gbRequestStatus.Dock = System.Windows.Forms.DockStyle.Top;
		this.gbRequestStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
		this.gbRequestStatus.Location = new System.Drawing.Point(3, 3);
		this.gbRequestStatus.Name = "gbRequestStatus";
		this.gbRequestStatus.Size = new System.Drawing.Size(596, 43);
		this.gbRequestStatus.TabIndex = 2;
		this.gbRequestStatus.TabStop = false;
		this.gbRequestStatus.Text = "Request status";
		this.tableLayoutPanel1.AutoSize = true;
		this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		this.tableLayoutPanel1.ColumnCount = 5;
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel1.Controls.Add(this.cbCompleted, 3, 0);
		this.tableLayoutPanel1.Controls.Add(this.cbChecked, 1, 0);
		this.tableLayoutPanel1.Controls.Add(this.lblCompleted, 2, 0);
		this.tableLayoutPanel1.Controls.Add(this.lblChecked, 0, 0);
		this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.tableLayoutPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 18);
		this.tableLayoutPanel1.Name = "tableLayoutPanel1";
		this.tableLayoutPanel1.RowCount = 1;
		this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel1.Size = new System.Drawing.Size(590, 22);
		this.tableLayoutPanel1.TabIndex = 179;
		this.lblCompleted.AutoSize = true;
		this.lblCompleted.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCompleted.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCompleted.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lblCompleted.Location = new System.Drawing.Point(94, 1);
		this.lblCompleted.Name = "lblCompleted";
		this.lblCompleted.Size = new System.Drawing.Size(73, 20);
		this.lblCompleted.TabIndex = 156;
		this.lblCompleted.Text = "Completed";
		this.lblCompleted.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblChecked.AutoSize = true;
		this.lblChecked.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblChecked.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblChecked.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lblChecked.Location = new System.Drawing.Point(4, 1);
		this.lblChecked.Name = "lblChecked";
		this.lblChecked.Size = new System.Drawing.Size(61, 20);
		this.lblChecked.TabIndex = 155;
		this.lblChecked.Text = "Checked";
		this.lblChecked.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(650, 550);
		base.Controls.Add(this.tabControlMain);
		base.Controls.Add(this.btnConfirm);
		base.Controls.Add(this.panelLogout);
		base.Controls.Add(this.statusStripMain);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "frmSettingView";
		base.Padding = new System.Windows.Forms.Padding(20, 70, 20, 20);
		this.Text = "5S QA System * SETTING";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmSettingView_FormClosing);
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmSettingView_FormClosed);
		base.Load += new System.EventHandler(frmSettingView_Load);
		base.Shown += new System.EventHandler(frmSettingView_Shown);
		this.statusStripMain.ResumeLayout(false);
		this.statusStripMain.PerformLayout();
		this.panelLogout.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.ptbAvatar).EndInit();
		this.tabControlMain.ResumeLayout(false);
		this.tabPageWarning.ResumeLayout(false);
		this.tabPageWarning.PerformLayout();
		this.contextMenuStripWarning.ResumeLayout(false);
		this.gbOKNG.ResumeLayout(false);
		this.gbOKNG.PerformLayout();
		this.tpanelOKNG.ResumeLayout(false);
		this.tpanelOKNG.PerformLayout();
		this.gbAbnormal.ResumeLayout(false);
		this.gbAbnormal.PerformLayout();
		this.tpanelAbnormal.ResumeLayout(false);
		this.tpanelAbnormal.PerformLayout();
		this.tpanelAbnormalActive.ResumeLayout(false);
		this.tpanelAbnormalActive.PerformLayout();
		this.gbPosition.ResumeLayout(false);
		this.gbPosition.PerformLayout();
		this.tpanelPosition.ResumeLayout(false);
		this.tpanelPosition.PerformLayout();
		this.tabPageEmailSend.ResumeLayout(false);
		this.tabPageEmailSend.PerformLayout();
		this.tpanelEmailSend.ResumeLayout(false);
		this.tpanelEmailSend.PerformLayout();
		this.panelPassword.ResumeLayout(false);
		this.panelPassword.PerformLayout();
		this.tabPageRank.ResumeLayout(false);
		this.tabPageRank.PerformLayout();
		this.tpanelRank.ResumeLayout(false);
		this.tpanelRank.PerformLayout();
		this.tabPageDirectory.ResumeLayout(false);
		this.tabPageDirectory.PerformLayout();
		this.tpanelDirectory.ResumeLayout(false);
		this.tpanelDirectory.PerformLayout();
		this.panelFolder.ResumeLayout(false);
		this.panelFolder.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.tabPageNotification.ResumeLayout(false);
		this.tabPageNotification.PerformLayout();
		this.gbRequestStatus.ResumeLayout(false);
		this.gbRequestStatus.PerformLayout();
		this.tableLayoutPanel1.ResumeLayout(false);
		this.tableLayoutPanel1.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
