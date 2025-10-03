using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using _5S_QA_Client.Controls;
using MetroFramework.Forms;

namespace _5S_QA_Client;

public class frmIME : MetroForm
{
	private IContainer components = null;

	private TableLayoutPanel tableLayoutPanel2;

	private ToolTip toolTipMain;

	private Label label1;

	private Button btnCopy;

	private TextBox txtIME;

	public frmIME()
	{
		InitializeComponent();
		Common.setControls(this, toolTipMain);
	}

	private void frmIME_Load(object sender, EventArgs e)
	{
		txtIME.Text = frmLogin.client.IME;
	}

	private void btnCopy_Click(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		Clipboard.SetText(txtIME.Text);
		toolTipMain.Show(btnCopy.Text, this, 0, 0, 1000);
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_5S_QA_Client.frmIME));
		this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
		this.label1 = new System.Windows.Forms.Label();
		this.btnCopy = new System.Windows.Forms.Button();
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.txtIME = new System.Windows.Forms.TextBox();
		this.tableLayoutPanel2.SuspendLayout();
		base.SuspendLayout();
		this.tableLayoutPanel2.AutoSize = true;
		this.tableLayoutPanel2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		this.tableLayoutPanel2.ColumnCount = 3;
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel2.Controls.Add(this.txtIME, 1, 0);
		this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
		this.tableLayoutPanel2.Controls.Add(this.btnCopy, 2, 0);
		this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.tableLayoutPanel2.Location = new System.Drawing.Point(20, 70);
		this.tableLayoutPanel2.Name = "tableLayoutPanel2";
		this.tableLayoutPanel2.RowCount = 1;
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel2.Size = new System.Drawing.Size(460, 30);
		this.tableLayoutPanel2.TabIndex = 15;
		this.label1.AutoSize = true;
		this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label1.Location = new System.Drawing.Point(5, 1);
		this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(30, 28);
		this.label1.TabIndex = 72;
		this.label1.Text = "IME";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.btnCopy.AutoSize = true;
		this.btnCopy.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnCopy.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnCopy.Location = new System.Drawing.Point(410, 1);
		this.btnCopy.Margin = new System.Windows.Forms.Padding(0);
		this.btnCopy.Name = "btnCopy";
		this.btnCopy.Size = new System.Drawing.Size(49, 28);
		this.btnCopy.TabIndex = 73;
		this.btnCopy.Text = "Copy";
		this.toolTipMain.SetToolTip(this.btnCopy, "Copy IME");
		this.btnCopy.UseVisualStyleBackColor = true;
		this.btnCopy.Click += new System.EventHandler(btnCopy_Click);
		this.txtIME.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtIME.Enabled = false;
		this.txtIME.Location = new System.Drawing.Point(43, 4);
		this.txtIME.Name = "txtIME";
		this.txtIME.Size = new System.Drawing.Size(363, 22);
		this.txtIME.TabIndex = 16;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(500, 120);
		base.Controls.Add(this.tableLayoutPanel2);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "frmIME";
		base.Padding = new System.Windows.Forms.Padding(20, 70, 20, 20);
		this.Text = "5S QA Client * IME";
		base.Load += new System.EventHandler(frmIME_Load);
		this.tableLayoutPanel2.ResumeLayout(false);
		this.tableLayoutPanel2.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
