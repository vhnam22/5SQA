using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace _5S_QA_System;

public class mPanelFileName : UserControl
{
	private readonly int No;

	private IContainer components = null;

	private Label lblFile;

	public Button btnDelete;

	private Label lblNo;

	public string FileName { get; set; }

	public mPanelFileName(string filename, int no)
	{
		InitializeComponent();
		FileName = filename;
		No = no;
	}

	private void mPanelFileName_Load(object sender, EventArgs e)
	{
		lblFile.Text = FileName;
		lblNo.Text = No.ToString();
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
		this.btnDelete = new System.Windows.Forms.Button();
		this.lblFile = new System.Windows.Forms.Label();
		this.lblNo = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.btnDelete.BackColor = System.Drawing.SystemColors.Control;
		this.btnDelete.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnDelete.Dock = System.Windows.Forms.DockStyle.Right;
		this.btnDelete.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
		this.btnDelete.FlatAppearance.BorderSize = 0;
		this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDelete.Location = new System.Drawing.Point(469, 0);
		this.btnDelete.Margin = new System.Windows.Forms.Padding(0);
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(31, 23);
		this.btnDelete.TabIndex = 5;
		this.btnDelete.Text = "X";
		this.btnDelete.UseVisualStyleBackColor = false;
		this.lblFile.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblFile.Location = new System.Drawing.Point(30, 0);
		this.lblFile.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblFile.Name = "lblFile";
		this.lblFile.Size = new System.Drawing.Size(470, 23);
		this.lblFile.TabIndex = 4;
		this.lblFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblNo.Dock = System.Windows.Forms.DockStyle.Left;
		this.lblNo.ForeColor = System.Drawing.Color.Blue;
		this.lblNo.Location = new System.Drawing.Point(0, 0);
		this.lblNo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblNo.Name = "lblNo";
		this.lblNo.Size = new System.Drawing.Size(30, 23);
		this.lblNo.TabIndex = 6;
		this.lblNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.btnDelete);
		base.Controls.Add(this.lblFile);
		base.Controls.Add(this.lblNo);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "mPanelFileName";
		base.Size = new System.Drawing.Size(500, 23);
		base.Load += new System.EventHandler(mPanelFileName_Load);
		base.ResumeLayout(false);
	}
}
