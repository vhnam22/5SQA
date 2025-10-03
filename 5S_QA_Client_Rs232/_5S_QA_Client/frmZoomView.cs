using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using _5S_QA_Client.Controls;
using _5S_QA_Client.Properties;
using MetroFramework;
using MetroFramework.Controls;
using MetroFramework.Forms;
using WpfUserImage;

namespace _5S_QA_Client;

public class frmZoomView : MetroForm
{
	private UcWpf ucWpf;

	private readonly Bitmap mBitmap;

	private readonly string mName;

	private IContainer components = null;

	private Panel panel1;

	private MetroLabel lblName;

	private PictureBox ptbImage;

	private ToolTip toolTipMain;

	private ElementHost elementHostZoomImage;

	public frmZoomView(Bitmap bitmap, string name)
	{
		InitializeComponent();
		Common.setControls(this, toolTipMain);
		mBitmap = bitmap;
		mName = name;
	}

	private void frmZoomView_Load(object sender, EventArgs e)
	{
		Init();
	}

	private void frmZoomView_FormClosing(object sender, FormClosingEventArgs e)
	{
		GC.Collect();
	}

	private void Init()
	{
		lblName.Text = mName;
		ptbImage.Image = mBitmap;
		ptbImage_Click(ptbImage, null);
	}

	private void ptbImage_Click(object sender, EventArgs e)
	{
		elementHostZoomImage.Child = null;
		ucWpf = new UcWpf(Common.ToBitmapImage(mBitmap));
		elementHostZoomImage.Child = (UIElement)(object)ucWpf;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_5S_QA_Client.frmZoomView));
		this.panel1 = new System.Windows.Forms.Panel();
		this.lblName = new MetroFramework.Controls.MetroLabel();
		this.elementHostZoomImage = new System.Windows.Forms.Integration.ElementHost();
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.ptbImage = new System.Windows.Forms.PictureBox();
		((System.ComponentModel.ISupportInitialize)this.ptbImage).BeginInit();
		base.SuspendLayout();
		this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(8, 60);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(884, 1);
		this.panel1.TabIndex = 25;
		this.lblName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lblName.FontWeight = MetroFramework.MetroLabelWeight.Regular;
		this.lblName.ForeColor = System.Drawing.Color.Red;
		this.lblName.Location = new System.Drawing.Point(442, 38);
		this.lblName.Name = "lblName";
		this.lblName.Size = new System.Drawing.Size(450, 19);
		this.lblName.TabIndex = 24;
		this.lblName.Text = "...";
		this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblName.UseCustomForeColor = true;
		this.elementHostZoomImage.Dock = System.Windows.Forms.DockStyle.Fill;
		this.elementHostZoomImage.Location = new System.Drawing.Point(8, 60);
		this.elementHostZoomImage.Name = "elementHostZoomImage";
		this.elementHostZoomImage.Size = new System.Drawing.Size(884, 532);
		this.elementHostZoomImage.TabIndex = 22;
		this.elementHostZoomImage.Child = null;
		this.ptbImage.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ptbImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ptbImage.Cursor = System.Windows.Forms.Cursors.Hand;
		this.ptbImage.ErrorImage = _5S_QA_Client.Properties.Resources._5S_QA_C;
		this.ptbImage.Image = _5S_QA_Client.Properties.Resources._5S_QA_C;
		this.ptbImage.InitialImage = _5S_QA_Client.Properties.Resources._5S_QA_C;
		this.ptbImage.Location = new System.Drawing.Point(792, 60);
		this.ptbImage.Name = "ptbImage";
		this.ptbImage.Size = new System.Drawing.Size(100, 100);
		this.ptbImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.ptbImage.TabIndex = 23;
		this.ptbImage.TabStop = false;
		this.toolTipMain.SetToolTip(this.ptbImage, "Reset image");
		this.ptbImage.Click += new System.EventHandler(ptbImage_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(900, 600);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.lblName);
		base.Controls.Add(this.ptbImage);
		base.Controls.Add(this.elementHostZoomImage);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "frmZoomView";
		base.Padding = new System.Windows.Forms.Padding(8, 60, 8, 8);
		this.Text = "5S QA Client * ZOOM";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmZoomView_FormClosing);
		base.Load += new System.EventHandler(frmZoomView_Load);
		((System.ComponentModel.ISupportInitialize)this.ptbImage).EndInit();
		base.ResumeLayout(false);
	}
}
