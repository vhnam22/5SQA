using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using _5S_QA_Client.Controls;
using _5S_QA_Client.Properties;
using _5S_QA_DL.APIContext;
using _5S_QA_Entities.Dtos;
using MetroFramework;
using MetroFramework.Controls;
using MetroFramework.Forms;

namespace _5S_QA_Client;

public class frmChangePassword : MetroForm
{
	private IContainer components = null;

	private ToolTip toolTipMain;

	private Panel panelMain;

	private TableLayoutPanel tableLayoutPanel2;

	private Button btnConfirm;

	private Label lblPasswordConfirmNew;

	private Label lblUsername;

	private Label lblPasswordNew;

	private Label lblPasswordOld;

	private MetroTextBox txtUsername;

	private MetroTextBox txtPasswordOld;

	private MetroTextBox txtPasswordNew;

	private MetroTextBox txtPasswordConfirmNew;

	public frmChangePassword()
	{
		InitializeComponent();
		Common.setControls(this, toolTipMain);
	}

	private void frmChangePassword_Load(object sender, EventArgs e)
	{
		txtUsername.Select();
	}

	private void frmChangePassword_FormClosing(object sender, FormClosingEventArgs e)
	{
		GC.Collect();
	}

	private void btnConfirm_Click(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		try
		{
			if (string.IsNullOrEmpty(txtUsername.Text))
			{
				MessageBox.Show(Common.getTextLanguage(this, "wUsername"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				txtUsername.Focus();
				return;
			}
			if (string.IsNullOrEmpty(txtPasswordOld.Text))
			{
				MessageBox.Show(Common.getTextLanguage(this, "wPasswordOld"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				txtPasswordOld.Focus();
				return;
			}
			if (string.IsNullOrEmpty(txtPasswordNew.Text))
			{
				MessageBox.Show(Common.getTextLanguage(this, "wPasswordNew"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				txtPasswordNew.Focus();
				return;
			}
			if (string.IsNullOrEmpty(txtPasswordConfirmNew.Text))
			{
				MessageBox.Show(Common.getTextLanguage(this, "wPasswordConfirmNew"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				txtPasswordConfirmNew.Focus();
				return;
			}
			ChangePasswordDto body = new ChangePasswordDto
			{
				Username = txtUsername.Text,
				OldPassword = txtPasswordOld.Text,
				NewPassword = txtPasswordNew.Text,
				ConfirmNewPassword = txtPasswordConfirmNew.Text
			};
			ResponseDto result = frmLogin.client.ChangePasswordAsync(body).Result;
			if (!result.Success)
			{
				throw new Exception(result.Messages.ElementAt(0).Message);
			}
			MessageBox.Show(Common.getTextLanguage(this, "ChangePasswordSuccessful"), Common.getTextLanguage(this, "SUCCESSFUL"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			Close();
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
					MessageBox.Show(string.IsNullOrEmpty(textLanguage) ? ex.Message : textLanguage, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			else
			{
				MessageBox.Show(string.IsNullOrEmpty(textLanguage) ? ex.Message : textLanguage, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_5S_QA_Client.frmChangePassword));
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.btnConfirm = new System.Windows.Forms.Button();
		this.txtUsername = new MetroFramework.Controls.MetroTextBox();
		this.txtPasswordOld = new MetroFramework.Controls.MetroTextBox();
		this.txtPasswordNew = new MetroFramework.Controls.MetroTextBox();
		this.txtPasswordConfirmNew = new MetroFramework.Controls.MetroTextBox();
		this.panelMain = new System.Windows.Forms.Panel();
		this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
		this.lblPasswordConfirmNew = new System.Windows.Forms.Label();
		this.lblUsername = new System.Windows.Forms.Label();
		this.lblPasswordNew = new System.Windows.Forms.Label();
		this.lblPasswordOld = new System.Windows.Forms.Label();
		this.panelMain.SuspendLayout();
		this.tableLayoutPanel2.SuspendLayout();
		base.SuspendLayout();
		this.btnConfirm.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnConfirm.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.btnConfirm.FlatAppearance.BorderSize = 0;
		this.btnConfirm.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
		this.btnConfirm.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
		this.btnConfirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnConfirm.Location = new System.Drawing.Point(20, 205);
		this.btnConfirm.Name = "btnConfirm";
		this.btnConfirm.Size = new System.Drawing.Size(460, 30);
		this.btnConfirm.TabIndex = 1;
		this.btnConfirm.Text = "Confirm";
		this.toolTipMain.SetToolTip(this.btnConfirm, "Confirm");
		this.btnConfirm.UseVisualStyleBackColor = true;
		this.btnConfirm.Click += new System.EventHandler(btnConfirm_Click);
		this.txtUsername.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtUsername.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtUsername.CustomButton.Image = null;
		this.txtUsername.CustomButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.txtUsername.CustomButton.Location = new System.Drawing.Point(272, 2);
		this.txtUsername.CustomButton.Name = "";
		this.txtUsername.CustomButton.Size = new System.Drawing.Size(19, 19);
		this.txtUsername.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
		this.txtUsername.CustomButton.TabIndex = 1;
		this.txtUsername.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
		this.txtUsername.CustomButton.UseSelectable = true;
		this.txtUsername.CustomButton.Visible = false;
		this.txtUsername.DisplayIcon = true;
		this.txtUsername.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtUsername.Icon = _5S_QA_Client.Properties.Resources.edit;
		this.txtUsername.Lines = new string[0];
		this.txtUsername.Location = new System.Drawing.Point(152, 4);
		this.txtUsername.MaxLength = 32767;
		this.txtUsername.Name = "txtUsername";
		this.txtUsername.PasswordChar = '\0';
		this.txtUsername.ScrollBars = System.Windows.Forms.ScrollBars.None;
		this.txtUsername.SelectedText = "";
		this.txtUsername.SelectionLength = 0;
		this.txtUsername.SelectionStart = 0;
		this.txtUsername.ShortcutsEnabled = true;
		this.txtUsername.ShowClearButton = true;
		this.txtUsername.Size = new System.Drawing.Size(294, 24);
		this.txtUsername.TabIndex = 1;
		this.toolTipMain.SetToolTip(this.txtUsername, "Please enter username");
		this.txtUsername.UseSelectable = true;
		this.txtUsername.WaterMark = "Username";
		this.txtUsername.WaterMarkColor = System.Drawing.Color.Silver;
		this.txtUsername.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPasswordOld.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtPasswordOld.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtPasswordOld.CustomButton.Image = null;
		this.txtPasswordOld.CustomButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.txtPasswordOld.CustomButton.Location = new System.Drawing.Point(272, 2);
		this.txtPasswordOld.CustomButton.Name = "";
		this.txtPasswordOld.CustomButton.Size = new System.Drawing.Size(19, 19);
		this.txtPasswordOld.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
		this.txtPasswordOld.CustomButton.TabIndex = 1;
		this.txtPasswordOld.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
		this.txtPasswordOld.CustomButton.UseSelectable = true;
		this.txtPasswordOld.CustomButton.Visible = false;
		this.txtPasswordOld.DisplayIcon = true;
		this.txtPasswordOld.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtPasswordOld.Icon = _5S_QA_Client.Properties.Resources.edit;
		this.txtPasswordOld.Lines = new string[0];
		this.txtPasswordOld.Location = new System.Drawing.Point(152, 35);
		this.txtPasswordOld.MaxLength = 32767;
		this.txtPasswordOld.Name = "txtPasswordOld";
		this.txtPasswordOld.PasswordChar = '●';
		this.txtPasswordOld.ScrollBars = System.Windows.Forms.ScrollBars.None;
		this.txtPasswordOld.SelectedText = "";
		this.txtPasswordOld.SelectionLength = 0;
		this.txtPasswordOld.SelectionStart = 0;
		this.txtPasswordOld.ShortcutsEnabled = true;
		this.txtPasswordOld.ShowClearButton = true;
		this.txtPasswordOld.Size = new System.Drawing.Size(294, 24);
		this.txtPasswordOld.TabIndex = 2;
		this.toolTipMain.SetToolTip(this.txtPasswordOld, "Please enter old password");
		this.txtPasswordOld.UseSelectable = true;
		this.txtPasswordOld.UseSystemPasswordChar = true;
		this.txtPasswordOld.WaterMark = "Old password";
		this.txtPasswordOld.WaterMarkColor = System.Drawing.Color.Silver;
		this.txtPasswordOld.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPasswordNew.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtPasswordNew.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtPasswordNew.CustomButton.Image = null;
		this.txtPasswordNew.CustomButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.txtPasswordNew.CustomButton.Location = new System.Drawing.Point(272, 2);
		this.txtPasswordNew.CustomButton.Name = "";
		this.txtPasswordNew.CustomButton.Size = new System.Drawing.Size(19, 19);
		this.txtPasswordNew.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
		this.txtPasswordNew.CustomButton.TabIndex = 1;
		this.txtPasswordNew.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
		this.txtPasswordNew.CustomButton.UseSelectable = true;
		this.txtPasswordNew.CustomButton.Visible = false;
		this.txtPasswordNew.DisplayIcon = true;
		this.txtPasswordNew.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtPasswordNew.Icon = _5S_QA_Client.Properties.Resources.edit;
		this.txtPasswordNew.Lines = new string[0];
		this.txtPasswordNew.Location = new System.Drawing.Point(152, 66);
		this.txtPasswordNew.MaxLength = 32767;
		this.txtPasswordNew.Name = "txtPasswordNew";
		this.txtPasswordNew.PasswordChar = '●';
		this.txtPasswordNew.ScrollBars = System.Windows.Forms.ScrollBars.None;
		this.txtPasswordNew.SelectedText = "";
		this.txtPasswordNew.SelectionLength = 0;
		this.txtPasswordNew.SelectionStart = 0;
		this.txtPasswordNew.ShortcutsEnabled = true;
		this.txtPasswordNew.ShowClearButton = true;
		this.txtPasswordNew.Size = new System.Drawing.Size(294, 24);
		this.txtPasswordNew.TabIndex = 3;
		this.toolTipMain.SetToolTip(this.txtPasswordNew, "Please enter new password");
		this.txtPasswordNew.UseSelectable = true;
		this.txtPasswordNew.UseSystemPasswordChar = true;
		this.txtPasswordNew.WaterMark = "New password";
		this.txtPasswordNew.WaterMarkColor = System.Drawing.Color.Silver;
		this.txtPasswordNew.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPasswordConfirmNew.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtPasswordConfirmNew.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtPasswordConfirmNew.CustomButton.Image = null;
		this.txtPasswordConfirmNew.CustomButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.txtPasswordConfirmNew.CustomButton.Location = new System.Drawing.Point(272, 2);
		this.txtPasswordConfirmNew.CustomButton.Name = "";
		this.txtPasswordConfirmNew.CustomButton.Size = new System.Drawing.Size(19, 19);
		this.txtPasswordConfirmNew.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
		this.txtPasswordConfirmNew.CustomButton.TabIndex = 1;
		this.txtPasswordConfirmNew.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
		this.txtPasswordConfirmNew.CustomButton.UseSelectable = true;
		this.txtPasswordConfirmNew.CustomButton.Visible = false;
		this.txtPasswordConfirmNew.DisplayIcon = true;
		this.txtPasswordConfirmNew.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtPasswordConfirmNew.Icon = _5S_QA_Client.Properties.Resources.edit;
		this.txtPasswordConfirmNew.Lines = new string[0];
		this.txtPasswordConfirmNew.Location = new System.Drawing.Point(152, 97);
		this.txtPasswordConfirmNew.MaxLength = 32767;
		this.txtPasswordConfirmNew.Name = "txtPasswordConfirmNew";
		this.txtPasswordConfirmNew.PasswordChar = '●';
		this.txtPasswordConfirmNew.ScrollBars = System.Windows.Forms.ScrollBars.None;
		this.txtPasswordConfirmNew.SelectedText = "";
		this.txtPasswordConfirmNew.SelectionLength = 0;
		this.txtPasswordConfirmNew.SelectionStart = 0;
		this.txtPasswordConfirmNew.ShortcutsEnabled = true;
		this.txtPasswordConfirmNew.ShowClearButton = true;
		this.txtPasswordConfirmNew.Size = new System.Drawing.Size(294, 24);
		this.txtPasswordConfirmNew.TabIndex = 4;
		this.toolTipMain.SetToolTip(this.txtPasswordConfirmNew, "Please enter confirm new password");
		this.txtPasswordConfirmNew.UseSelectable = true;
		this.txtPasswordConfirmNew.UseSystemPasswordChar = true;
		this.txtPasswordConfirmNew.WaterMark = "Confirm new password";
		this.txtPasswordConfirmNew.WaterMarkColor = System.Drawing.Color.Silver;
		this.txtPasswordConfirmNew.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
		this.panelMain.AutoScroll = true;
		this.panelMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panelMain.Controls.Add(this.tableLayoutPanel2);
		this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panelMain.Location = new System.Drawing.Point(20, 70);
		this.panelMain.Margin = new System.Windows.Forms.Padding(4);
		this.panelMain.Name = "panelMain";
		this.panelMain.Size = new System.Drawing.Size(460, 135);
		this.panelMain.TabIndex = 1;
		this.tableLayoutPanel2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.tableLayoutPanel2.AutoSize = true;
		this.tableLayoutPanel2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		this.tableLayoutPanel2.ColumnCount = 2;
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel2.Controls.Add(this.txtPasswordConfirmNew, 1, 3);
		this.tableLayoutPanel2.Controls.Add(this.txtPasswordNew, 1, 2);
		this.tableLayoutPanel2.Controls.Add(this.txtPasswordOld, 1, 1);
		this.tableLayoutPanel2.Controls.Add(this.txtUsername, 1, 0);
		this.tableLayoutPanel2.Controls.Add(this.lblPasswordConfirmNew, 0, 3);
		this.tableLayoutPanel2.Controls.Add(this.lblUsername, 0, 0);
		this.tableLayoutPanel2.Controls.Add(this.lblPasswordNew, 0, 2);
		this.tableLayoutPanel2.Controls.Add(this.lblPasswordOld, 0, 1);
		this.tableLayoutPanel2.Location = new System.Drawing.Point(4, 4);
		this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4);
		this.tableLayoutPanel2.Name = "tableLayoutPanel2";
		this.tableLayoutPanel2.RowCount = 4;
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30f));
		this.tableLayoutPanel2.Size = new System.Drawing.Size(450, 125);
		this.tableLayoutPanel2.TabIndex = 15;
		this.lblPasswordConfirmNew.AutoSize = true;
		this.lblPasswordConfirmNew.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblPasswordConfirmNew.Location = new System.Drawing.Point(4, 94);
		this.lblPasswordConfirmNew.Name = "lblPasswordConfirmNew";
		this.lblPasswordConfirmNew.Size = new System.Drawing.Size(141, 30);
		this.lblPasswordConfirmNew.TabIndex = 19;
		this.lblPasswordConfirmNew.Text = "Confirm new password";
		this.lblPasswordConfirmNew.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblUsername.AutoSize = true;
		this.lblUsername.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblUsername.Location = new System.Drawing.Point(4, 1);
		this.lblUsername.Name = "lblUsername";
		this.lblUsername.Size = new System.Drawing.Size(141, 30);
		this.lblUsername.TabIndex = 16;
		this.lblUsername.Text = "Username";
		this.lblUsername.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblPasswordNew.AutoSize = true;
		this.lblPasswordNew.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblPasswordNew.Location = new System.Drawing.Point(4, 63);
		this.lblPasswordNew.Name = "lblPasswordNew";
		this.lblPasswordNew.Size = new System.Drawing.Size(141, 30);
		this.lblPasswordNew.TabIndex = 18;
		this.lblPasswordNew.Text = "New password";
		this.lblPasswordNew.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblPasswordOld.AutoSize = true;
		this.lblPasswordOld.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblPasswordOld.Location = new System.Drawing.Point(4, 32);
		this.lblPasswordOld.Name = "lblPasswordOld";
		this.lblPasswordOld.Size = new System.Drawing.Size(141, 30);
		this.lblPasswordOld.TabIndex = 17;
		this.lblPasswordOld.Text = "Old password";
		this.lblPasswordOld.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(500, 255);
		base.Controls.Add(this.panelMain);
		base.Controls.Add(this.btnConfirm);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "frmChangePassword";
		base.Padding = new System.Windows.Forms.Padding(20, 70, 20, 20);
		this.Text = "5S QA Client * CHANGE PASSWORD";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmChangePassword_FormClosing);
		base.Load += new System.EventHandler(frmChangePassword_Load);
		this.panelMain.ResumeLayout(false);
		this.panelMain.PerformLayout();
		this.tableLayoutPanel2.ResumeLayout(false);
		this.tableLayoutPanel2.PerformLayout();
		base.ResumeLayout(false);
	}
}
