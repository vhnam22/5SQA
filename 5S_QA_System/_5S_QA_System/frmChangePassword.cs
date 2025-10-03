using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using _5S_QA_DL.APIContext;
using _5S_QA_Entities.Dtos;
using _5S_QA_System.Controls;
using MetroFramework.Forms;

namespace _5S_QA_System;

public class frmChangePassword : MetroForm
{
	private IContainer components = null;

	private ToolTip toolTipMain;

	private Panel panelMain;

	private TableLayoutPanel tableLayoutPanel2;

	private Label lblPasswordConfirmNew;

	private Label lblPasswordNew;

	private Label lblPasswordOld;

	private Label lblUsername;

	private TextBox txtUsername;

	private TextBox txtPasswordConfirmNew;

	private TextBox txtPasswordNew;

	private TextBox txtPasswordOld;

	private Button btnConfirm;

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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_5S_QA_System.frmChangePassword));
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.txtUsername = new System.Windows.Forms.TextBox();
		this.txtPasswordOld = new System.Windows.Forms.TextBox();
		this.txtPasswordNew = new System.Windows.Forms.TextBox();
		this.txtPasswordConfirmNew = new System.Windows.Forms.TextBox();
		this.btnConfirm = new System.Windows.Forms.Button();
		this.panelMain = new System.Windows.Forms.Panel();
		this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
		this.lblPasswordConfirmNew = new System.Windows.Forms.Label();
		this.lblPasswordNew = new System.Windows.Forms.Label();
		this.lblPasswordOld = new System.Windows.Forms.Label();
		this.lblUsername = new System.Windows.Forms.Label();
		this.panelMain.SuspendLayout();
		this.tableLayoutPanel2.SuspendLayout();
		base.SuspendLayout();
		this.txtUsername.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtUsername.Location = new System.Drawing.Point(152, 4);
		this.txtUsername.Name = "txtUsername";
		this.txtUsername.Size = new System.Drawing.Size(316, 22);
		this.txtUsername.TabIndex = 1;
		this.toolTipMain.SetToolTip(this.txtUsername, "Please enter username");
		this.txtPasswordOld.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtPasswordOld.Location = new System.Drawing.Point(152, 33);
		this.txtPasswordOld.Name = "txtPasswordOld";
		this.txtPasswordOld.PasswordChar = '*';
		this.txtPasswordOld.Size = new System.Drawing.Size(316, 22);
		this.txtPasswordOld.TabIndex = 2;
		this.toolTipMain.SetToolTip(this.txtPasswordOld, "Please enter old password");
		this.txtPasswordNew.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtPasswordNew.Location = new System.Drawing.Point(152, 62);
		this.txtPasswordNew.Name = "txtPasswordNew";
		this.txtPasswordNew.PasswordChar = '*';
		this.txtPasswordNew.Size = new System.Drawing.Size(316, 22);
		this.txtPasswordNew.TabIndex = 3;
		this.toolTipMain.SetToolTip(this.txtPasswordNew, "Please enter new password");
		this.txtPasswordConfirmNew.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtPasswordConfirmNew.Location = new System.Drawing.Point(152, 91);
		this.txtPasswordConfirmNew.Name = "txtPasswordConfirmNew";
		this.txtPasswordConfirmNew.PasswordChar = '*';
		this.txtPasswordConfirmNew.Size = new System.Drawing.Size(316, 22);
		this.txtPasswordConfirmNew.TabIndex = 4;
		this.toolTipMain.SetToolTip(this.txtPasswordConfirmNew, "Please enter confirm new password");
		this.btnConfirm.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnConfirm.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.btnConfirm.FlatAppearance.BorderSize = 0;
		this.btnConfirm.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
		this.btnConfirm.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
		this.btnConfirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnConfirm.Location = new System.Drawing.Point(20, 195);
		this.btnConfirm.Name = "btnConfirm";
		this.btnConfirm.Size = new System.Drawing.Size(480, 30);
		this.btnConfirm.TabIndex = 1;
		this.btnConfirm.Text = "Confirm";
		this.toolTipMain.SetToolTip(this.btnConfirm, "Confirm");
		this.btnConfirm.UseVisualStyleBackColor = true;
		this.btnConfirm.Click += new System.EventHandler(btnConfirm_Click);
		this.panelMain.AutoScroll = true;
		this.panelMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panelMain.Controls.Add(this.tableLayoutPanel2);
		this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panelMain.Location = new System.Drawing.Point(20, 70);
		this.panelMain.Name = "panelMain";
		this.panelMain.Padding = new System.Windows.Forms.Padding(3);
		this.panelMain.Size = new System.Drawing.Size(480, 125);
		this.panelMain.TabIndex = 1;
		this.tableLayoutPanel2.AutoSize = true;
		this.tableLayoutPanel2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		this.tableLayoutPanel2.ColumnCount = 2;
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel2.Controls.Add(this.txtPasswordConfirmNew, 1, 3);
		this.tableLayoutPanel2.Controls.Add(this.txtUsername, 1, 0);
		this.tableLayoutPanel2.Controls.Add(this.txtPasswordNew, 1, 2);
		this.tableLayoutPanel2.Controls.Add(this.lblPasswordConfirmNew, 0, 3);
		this.tableLayoutPanel2.Controls.Add(this.txtPasswordOld, 1, 1);
		this.tableLayoutPanel2.Controls.Add(this.lblPasswordNew, 0, 2);
		this.tableLayoutPanel2.Controls.Add(this.lblPasswordOld, 0, 1);
		this.tableLayoutPanel2.Controls.Add(this.lblUsername, 0, 0);
		this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
		this.tableLayoutPanel2.Name = "tableLayoutPanel2";
		this.tableLayoutPanel2.RowCount = 4;
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel2.Size = new System.Drawing.Size(472, 117);
		this.tableLayoutPanel2.TabIndex = 1;
		this.lblPasswordConfirmNew.AutoSize = true;
		this.lblPasswordConfirmNew.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblPasswordConfirmNew.Location = new System.Drawing.Point(4, 88);
		this.lblPasswordConfirmNew.Name = "lblPasswordConfirmNew";
		this.lblPasswordConfirmNew.Size = new System.Drawing.Size(141, 28);
		this.lblPasswordConfirmNew.TabIndex = 20;
		this.lblPasswordConfirmNew.Text = "Confirm new password";
		this.lblPasswordConfirmNew.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblPasswordNew.AutoSize = true;
		this.lblPasswordNew.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblPasswordNew.Location = new System.Drawing.Point(4, 59);
		this.lblPasswordNew.Name = "lblPasswordNew";
		this.lblPasswordNew.Size = new System.Drawing.Size(141, 28);
		this.lblPasswordNew.TabIndex = 19;
		this.lblPasswordNew.Text = "New password";
		this.lblPasswordNew.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblPasswordOld.AutoSize = true;
		this.lblPasswordOld.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblPasswordOld.Location = new System.Drawing.Point(4, 30);
		this.lblPasswordOld.Name = "lblPasswordOld";
		this.lblPasswordOld.Size = new System.Drawing.Size(141, 28);
		this.lblPasswordOld.TabIndex = 18;
		this.lblPasswordOld.Text = "Old password";
		this.lblPasswordOld.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblUsername.AutoSize = true;
		this.lblUsername.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblUsername.Location = new System.Drawing.Point(4, 1);
		this.lblUsername.Name = "lblUsername";
		this.lblUsername.Size = new System.Drawing.Size(141, 28);
		this.lblUsername.TabIndex = 17;
		this.lblUsername.Text = "Username";
		this.lblUsername.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(520, 245);
		base.Controls.Add(this.panelMain);
		base.Controls.Add(this.btnConfirm);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "frmChangePassword";
		base.Padding = new System.Windows.Forms.Padding(20, 70, 20, 20);
		this.Text = "5S QA System * CHANGE PASSWORD";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmChangePassword_FormClosing);
		base.Load += new System.EventHandler(frmChangePassword_Load);
		this.panelMain.ResumeLayout(false);
		this.panelMain.PerformLayout();
		this.tableLayoutPanel2.ResumeLayout(false);
		this.tableLayoutPanel2.PerformLayout();
		base.ResumeLayout(false);
	}
}
