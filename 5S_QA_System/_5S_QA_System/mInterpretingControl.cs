using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using _5S_QA_Entities.Models;
using _5S_QA_System.Controls;

namespace _5S_QA_System;

public class mInterpretingControl : UserControl
{
	private CommonCpk mCommonCpk;

	private IContainer components = null;

	private Label label17;

	private Label label16;

	private Label label15;

	private Label lblTitleMinimun;

	private Label lblTitleOutLSL;

	private Label label12;

	private Label lblTitleStandard;

	private Label label10;

	private Label lblTitleMaximun;

	private Label label8;

	private Label lblTitleAverage;

	private Label lblTitleOutUSL;

	private Label lblTitleLower;

	private Label lblTitleUpper;

	private Label lblTool;

	private Label lblItemNo;

	private Label lblTitleTool;

	private Label lblTitleItemNo;

	private Label label6;

	private TableLayoutPanel tpanelHeader;

	private Label lblRLCL;

	private Label label20;

	private Label lblRUCL;

	private Label label19;

	private Label lblR;

	private Label lblXLCL;

	private Label lblXUCL;

	private Label lblCpk;

	private Label lblCp;

	private Label lblSD;

	private Label lblMinimun;

	private Label lblMaximun;

	private Label lblAverage;

	private Label lblOutLSL;

	private Label lblOK;

	private Label lblOutUSL;

	private Label lblN;

	private Label lblLower;

	private Label lblUpper;

	private Label lblStandard;

	private Label label18;

	public mInterpretingControl(CommonCpk commonCpk)
	{
		InitializeComponent();
		mCommonCpk = commonCpk;
		Common.setControls(this);
	}

	private void mInterpretingControl_Load(object sender, EventArgs e)
	{
		Init();
	}

	private void Init()
	{
		lblItemNo.Text = mCommonCpk.ItemNo;
		lblTool.Text = mCommonCpk.Tool;
		if (mCommonCpk.Unit == "°")
		{
			lblStandard.Text = Common.ConvertDoubleToDegrees(mCommonCpk.Standard);
			lblUpper.Text = Common.ConvertDoubleToDegrees(mCommonCpk.Upper);
			lblLower.Text = Common.ConvertDoubleToDegrees(mCommonCpk.Lower);
			lblAverage.Text = Common.ConvertDoubleToDegrees(mCommonCpk.Average);
			lblMaximun.Text = Common.ConvertDoubleToDegrees(mCommonCpk.Maximun);
			lblMinimun.Text = Common.ConvertDoubleToDegrees(mCommonCpk.Minimun);
			lblSD.Text = Common.ConvertDoubleToDegrees(mCommonCpk.SD);
			lblXUCL.Text = Common.ConvertDoubleToDegrees(mCommonCpk.XUCL);
			lblXLCL.Text = Common.ConvertDoubleToDegrees(mCommonCpk.XLCL);
			lblR.Text = Common.ConvertDoubleToDegrees(mCommonCpk.Rtb);
			lblRUCL.Text = Common.ConvertDoubleToDegrees(mCommonCpk.RUCL);
			lblRLCL.Text = Common.ConvertDoubleToDegrees(mCommonCpk.RLCL);
		}
		else
		{
			lblStandard.Text = mCommonCpk.Standard.ToString();
			lblUpper.Text = mCommonCpk.Upper.ToString();
			lblLower.Text = mCommonCpk.Lower.ToString();
			lblAverage.Text = mCommonCpk.Average.ToString("0.0000");
			lblMaximun.Text = mCommonCpk.Maximun.ToString("0.0000");
			lblMinimun.Text = mCommonCpk.Minimun.ToString("0.0000");
			lblSD.Text = mCommonCpk.SD.ToString("0.0000");
			lblXUCL.Text = mCommonCpk.XUCL.ToString("0.0000");
			lblXLCL.Text = mCommonCpk.XLCL.ToString("0.0000");
			lblR.Text = mCommonCpk.Rtb.ToString("0.0000");
			lblRUCL.Text = mCommonCpk.RUCL.ToString("0.0000");
			lblRLCL.Text = mCommonCpk.RLCL.ToString("0.0000");
		}
		lblN.Text = mCommonCpk.n.ToString();
		lblOutUSL.Text = mCommonCpk.OutUSL.ToString();
		lblOutLSL.Text = mCommonCpk.OutLSL.ToString();
		lblOK.Text = mCommonCpk.OK.ToString();
		lblCp.Text = mCommonCpk.Cp.ToString("0.0000");
		lblCpk.Text = mCommonCpk.Cpk.ToString("0.0000");
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
		this.label17 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.lblTitleMinimun = new System.Windows.Forms.Label();
		this.lblTitleOutLSL = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.lblTitleStandard = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.lblTitleMaximun = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.lblTitleAverage = new System.Windows.Forms.Label();
		this.lblTitleOutUSL = new System.Windows.Forms.Label();
		this.lblTitleLower = new System.Windows.Forms.Label();
		this.lblTitleUpper = new System.Windows.Forms.Label();
		this.lblTool = new System.Windows.Forms.Label();
		this.lblItemNo = new System.Windows.Forms.Label();
		this.lblTitleTool = new System.Windows.Forms.Label();
		this.lblTitleItemNo = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.tpanelHeader = new System.Windows.Forms.TableLayoutPanel();
		this.lblRLCL = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.lblRUCL = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.lblR = new System.Windows.Forms.Label();
		this.lblXLCL = new System.Windows.Forms.Label();
		this.lblXUCL = new System.Windows.Forms.Label();
		this.lblCpk = new System.Windows.Forms.Label();
		this.lblCp = new System.Windows.Forms.Label();
		this.lblSD = new System.Windows.Forms.Label();
		this.lblMinimun = new System.Windows.Forms.Label();
		this.lblMaximun = new System.Windows.Forms.Label();
		this.lblAverage = new System.Windows.Forms.Label();
		this.lblOutLSL = new System.Windows.Forms.Label();
		this.lblOK = new System.Windows.Forms.Label();
		this.lblOutUSL = new System.Windows.Forms.Label();
		this.lblN = new System.Windows.Forms.Label();
		this.lblLower = new System.Windows.Forms.Label();
		this.lblUpper = new System.Windows.Forms.Label();
		this.lblStandard = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.tpanelHeader.SuspendLayout();
		base.SuspendLayout();
		this.label17.AutoSize = true;
		this.label17.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label17.Location = new System.Drawing.Point(741, 27);
		this.label17.Margin = new System.Windows.Forms.Padding(3);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(51, 16);
		this.label17.TabIndex = 18;
		this.label17.Text = "X\u00af LCL";
		this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label16.AutoSize = true;
		this.label16.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label16.Location = new System.Drawing.Point(741, 4);
		this.label16.Margin = new System.Windows.Forms.Padding(3);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(51, 16);
		this.label16.TabIndex = 17;
		this.label16.Text = "X\u00af UCL";
		this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label15.AutoSize = true;
		this.label15.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label15.Location = new System.Drawing.Point(630, 50);
		this.label15.Margin = new System.Windows.Forms.Padding(3);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(31, 16);
		this.label15.TabIndex = 16;
		this.label15.Text = "Cpk";
		this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblTitleMinimun.AutoSize = true;
		this.lblTitleMinimun.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitleMinimun.Location = new System.Drawing.Point(490, 50);
		this.lblTitleMinimun.Margin = new System.Windows.Forms.Padding(3);
		this.lblTitleMinimun.Name = "lblTitleMinimun";
		this.lblTitleMinimun.Size = new System.Drawing.Size(60, 16);
		this.lblTitleMinimun.TabIndex = 15;
		this.lblTitleMinimun.Text = "Minimun";
		this.lblTitleMinimun.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblTitleOutLSL.AutoSize = true;
		this.lblTitleOutLSL.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitleOutLSL.Location = new System.Drawing.Point(346, 50);
		this.lblTitleOutLSL.Margin = new System.Windows.Forms.Padding(3);
		this.lblTitleOutLSL.Name = "lblTitleOutLSL";
		this.lblTitleOutLSL.Size = new System.Drawing.Size(64, 16);
		this.lblTitleOutLSL.TabIndex = 14;
		this.lblTitleOutLSL.Text = "Out (LSL)";
		this.lblTitleOutLSL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label12.AutoSize = true;
		this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label12.Location = new System.Drawing.Point(195, 50);
		this.label12.Margin = new System.Windows.Forms.Padding(3);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(71, 16);
		this.label12.TabIndex = 13;
		this.label12.Text = "(n)";
		this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblTitleStandard.AutoSize = true;
		this.lblTitleStandard.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitleStandard.Location = new System.Drawing.Point(4, 50);
		this.lblTitleStandard.Margin = new System.Windows.Forms.Padding(3);
		this.lblTitleStandard.Name = "lblTitleStandard";
		this.lblTitleStandard.Size = new System.Drawing.Size(62, 16);
		this.lblTitleStandard.TabIndex = 12;
		this.lblTitleStandard.Text = "Standard";
		this.lblTitleStandard.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label10.AutoSize = true;
		this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label10.Location = new System.Drawing.Point(630, 27);
		this.label10.Margin = new System.Windows.Forms.Padding(3);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(31, 16);
		this.label10.TabIndex = 11;
		this.label10.Text = "Cp";
		this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblTitleMaximun.AutoSize = true;
		this.lblTitleMaximun.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitleMaximun.Location = new System.Drawing.Point(490, 27);
		this.lblTitleMaximun.Margin = new System.Windows.Forms.Padding(3);
		this.lblTitleMaximun.Name = "lblTitleMaximun";
		this.lblTitleMaximun.Size = new System.Drawing.Size(60, 16);
		this.lblTitleMaximun.TabIndex = 10;
		this.lblTitleMaximun.Text = "Maximun";
		this.lblTitleMaximun.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label8.AutoSize = true;
		this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label8.Location = new System.Drawing.Point(630, 4);
		this.label8.Margin = new System.Windows.Forms.Padding(3);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(31, 16);
		this.label8.TabIndex = 9;
		this.label8.Text = "SD";
		this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblTitleAverage.AutoSize = true;
		this.lblTitleAverage.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitleAverage.Location = new System.Drawing.Point(490, 4);
		this.lblTitleAverage.Margin = new System.Windows.Forms.Padding(3);
		this.lblTitleAverage.Name = "lblTitleAverage";
		this.lblTitleAverage.Size = new System.Drawing.Size(60, 16);
		this.lblTitleAverage.TabIndex = 8;
		this.lblTitleAverage.Text = "Average";
		this.lblTitleAverage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblTitleOutUSL.AutoSize = true;
		this.lblTitleOutUSL.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitleOutUSL.Location = new System.Drawing.Point(346, 4);
		this.lblTitleOutUSL.Margin = new System.Windows.Forms.Padding(3);
		this.lblTitleOutUSL.Name = "lblTitleOutUSL";
		this.lblTitleOutUSL.Size = new System.Drawing.Size(64, 16);
		this.lblTitleOutUSL.TabIndex = 6;
		this.lblTitleOutUSL.Text = "Out (USL)";
		this.lblTitleOutUSL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblTitleLower.AutoSize = true;
		this.lblTitleLower.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitleLower.Location = new System.Drawing.Point(195, 27);
		this.lblTitleLower.Margin = new System.Windows.Forms.Padding(3);
		this.lblTitleLower.Name = "lblTitleLower";
		this.lblTitleLower.Size = new System.Drawing.Size(71, 16);
		this.lblTitleLower.TabIndex = 5;
		this.lblTitleLower.Text = "Lower limit";
		this.lblTitleLower.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblTitleUpper.AutoSize = true;
		this.lblTitleUpper.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitleUpper.Location = new System.Drawing.Point(195, 4);
		this.lblTitleUpper.Margin = new System.Windows.Forms.Padding(3);
		this.lblTitleUpper.Name = "lblTitleUpper";
		this.lblTitleUpper.Size = new System.Drawing.Size(71, 16);
		this.lblTitleUpper.TabIndex = 4;
		this.lblTitleUpper.Text = "Upper limit";
		this.lblTitleUpper.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblTool.AutoSize = true;
		this.lblTool.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTool.Location = new System.Drawing.Point(73, 27);
		this.lblTool.Margin = new System.Windows.Forms.Padding(3);
		this.lblTool.Name = "lblTool";
		this.lblTool.Size = new System.Drawing.Size(115, 16);
		this.lblTool.TabIndex = 3;
		this.lblTool.Text = "...";
		this.lblTool.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblItemNo.AutoSize = true;
		this.lblItemNo.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblItemNo.Location = new System.Drawing.Point(73, 4);
		this.lblItemNo.Margin = new System.Windows.Forms.Padding(3);
		this.lblItemNo.Name = "lblItemNo";
		this.lblItemNo.Size = new System.Drawing.Size(115, 16);
		this.lblItemNo.TabIndex = 2;
		this.lblItemNo.Text = "...";
		this.lblItemNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblTitleTool.AutoSize = true;
		this.lblTitleTool.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitleTool.Location = new System.Drawing.Point(4, 27);
		this.lblTitleTool.Margin = new System.Windows.Forms.Padding(3);
		this.lblTitleTool.Name = "lblTitleTool";
		this.lblTitleTool.Size = new System.Drawing.Size(62, 16);
		this.lblTitleTool.TabIndex = 1;
		this.lblTitleTool.Text = "Tool";
		this.lblTitleTool.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblTitleItemNo.AutoSize = true;
		this.lblTitleItemNo.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitleItemNo.Location = new System.Drawing.Point(4, 4);
		this.lblTitleItemNo.Margin = new System.Windows.Forms.Padding(3);
		this.lblTitleItemNo.Name = "lblTitleItemNo";
		this.lblTitleItemNo.Size = new System.Drawing.Size(62, 16);
		this.lblTitleItemNo.TabIndex = 0;
		this.lblTitleItemNo.Text = "Item no.";
		this.lblTitleItemNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label6.AutoSize = true;
		this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label6.Location = new System.Drawing.Point(346, 27);
		this.label6.Margin = new System.Windows.Forms.Padding(3);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(64, 16);
		this.label6.TabIndex = 7;
		this.label6.Text = "OK";
		this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.tpanelHeader.AutoSize = true;
		this.tpanelHeader.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		this.tpanelHeader.ColumnCount = 14;
		this.tpanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21.73913f));
		this.tpanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.04348f));
		this.tpanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.04348f));
		this.tpanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.04348f));
		this.tpanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.04348f));
		this.tpanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.04348f));
		this.tpanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.04348f));
		this.tpanelHeader.Controls.Add(this.lblRLCL, 13, 1);
		this.tpanelHeader.Controls.Add(this.label20, 12, 1);
		this.tpanelHeader.Controls.Add(this.lblRUCL, 13, 0);
		this.tpanelHeader.Controls.Add(this.label19, 12, 0);
		this.tpanelHeader.Controls.Add(this.lblR, 11, 2);
		this.tpanelHeader.Controls.Add(this.lblXLCL, 11, 1);
		this.tpanelHeader.Controls.Add(this.lblXUCL, 11, 0);
		this.tpanelHeader.Controls.Add(this.lblCpk, 9, 2);
		this.tpanelHeader.Controls.Add(this.lblCp, 9, 1);
		this.tpanelHeader.Controls.Add(this.lblSD, 9, 0);
		this.tpanelHeader.Controls.Add(this.lblMinimun, 7, 2);
		this.tpanelHeader.Controls.Add(this.lblMaximun, 7, 1);
		this.tpanelHeader.Controls.Add(this.lblAverage, 7, 0);
		this.tpanelHeader.Controls.Add(this.lblOutLSL, 5, 2);
		this.tpanelHeader.Controls.Add(this.lblOK, 5, 1);
		this.tpanelHeader.Controls.Add(this.lblOutUSL, 5, 0);
		this.tpanelHeader.Controls.Add(this.lblN, 3, 2);
		this.tpanelHeader.Controls.Add(this.lblLower, 3, 1);
		this.tpanelHeader.Controls.Add(this.lblUpper, 3, 0);
		this.tpanelHeader.Controls.Add(this.lblStandard, 1, 2);
		this.tpanelHeader.Controls.Add(this.label18, 10, 2);
		this.tpanelHeader.Controls.Add(this.label17, 10, 1);
		this.tpanelHeader.Controls.Add(this.label16, 10, 0);
		this.tpanelHeader.Controls.Add(this.label15, 8, 2);
		this.tpanelHeader.Controls.Add(this.lblTitleMinimun, 6, 2);
		this.tpanelHeader.Controls.Add(this.lblTitleOutLSL, 4, 2);
		this.tpanelHeader.Controls.Add(this.label12, 2, 2);
		this.tpanelHeader.Controls.Add(this.lblTitleStandard, 0, 2);
		this.tpanelHeader.Controls.Add(this.label10, 8, 1);
		this.tpanelHeader.Controls.Add(this.lblTitleMaximun, 6, 1);
		this.tpanelHeader.Controls.Add(this.label8, 8, 0);
		this.tpanelHeader.Controls.Add(this.lblTitleAverage, 6, 0);
		this.tpanelHeader.Controls.Add(this.label6, 4, 1);
		this.tpanelHeader.Controls.Add(this.lblTitleOutUSL, 4, 0);
		this.tpanelHeader.Controls.Add(this.lblTitleLower, 2, 1);
		this.tpanelHeader.Controls.Add(this.lblTitleUpper, 2, 0);
		this.tpanelHeader.Controls.Add(this.lblTool, 1, 1);
		this.tpanelHeader.Controls.Add(this.lblItemNo, 1, 0);
		this.tpanelHeader.Controls.Add(this.lblTitleTool, 0, 1);
		this.tpanelHeader.Controls.Add(this.lblTitleItemNo, 0, 0);
		this.tpanelHeader.Dock = System.Windows.Forms.DockStyle.Top;
		this.tpanelHeader.Location = new System.Drawing.Point(0, 0);
		this.tpanelHeader.Margin = new System.Windows.Forms.Padding(4);
		this.tpanelHeader.Name = "tpanelHeader";
		this.tpanelHeader.RowCount = 3;
		this.tpanelHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22f));
		this.tpanelHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22f));
		this.tpanelHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22f));
		this.tpanelHeader.Size = new System.Drawing.Size(1000, 70);
		this.tpanelHeader.TabIndex = 8;
		this.lblRLCL.AutoSize = true;
		this.lblRLCL.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblRLCL.Location = new System.Drawing.Point(925, 27);
		this.lblRLCL.Margin = new System.Windows.Forms.Padding(3);
		this.lblRLCL.Name = "lblRLCL";
		this.lblRLCL.Size = new System.Drawing.Size(71, 16);
		this.lblRLCL.TabIndex = 39;
		this.lblRLCL.Text = "...";
		this.lblRLCL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label20.AutoSize = true;
		this.label20.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label20.Location = new System.Drawing.Point(872, 27);
		this.label20.Margin = new System.Windows.Forms.Padding(3);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(46, 16);
		this.label20.TabIndex = 38;
		this.label20.Text = "R LCL";
		this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblRUCL.AutoSize = true;
		this.lblRUCL.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblRUCL.Location = new System.Drawing.Point(925, 4);
		this.lblRUCL.Margin = new System.Windows.Forms.Padding(3);
		this.lblRUCL.Name = "lblRUCL";
		this.lblRUCL.Size = new System.Drawing.Size(71, 16);
		this.lblRUCL.TabIndex = 37;
		this.lblRUCL.Text = "...";
		this.lblRUCL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label19.AutoSize = true;
		this.label19.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label19.Location = new System.Drawing.Point(872, 4);
		this.label19.Margin = new System.Windows.Forms.Padding(3);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(46, 16);
		this.label19.TabIndex = 36;
		this.label19.Text = "R UCL";
		this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblR.AutoSize = true;
		this.lblR.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblR.Location = new System.Drawing.Point(799, 50);
		this.lblR.Margin = new System.Windows.Forms.Padding(3);
		this.lblR.Name = "lblR";
		this.lblR.Size = new System.Drawing.Size(66, 16);
		this.lblR.TabIndex = 35;
		this.lblR.Text = "...";
		this.lblR.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblXLCL.AutoSize = true;
		this.lblXLCL.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblXLCL.Location = new System.Drawing.Point(799, 27);
		this.lblXLCL.Margin = new System.Windows.Forms.Padding(3);
		this.lblXLCL.Name = "lblXLCL";
		this.lblXLCL.Size = new System.Drawing.Size(66, 16);
		this.lblXLCL.TabIndex = 34;
		this.lblXLCL.Text = "...";
		this.lblXLCL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblXUCL.AutoSize = true;
		this.lblXUCL.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblXUCL.Location = new System.Drawing.Point(799, 4);
		this.lblXUCL.Margin = new System.Windows.Forms.Padding(3);
		this.lblXUCL.Name = "lblXUCL";
		this.lblXUCL.Size = new System.Drawing.Size(66, 16);
		this.lblXUCL.TabIndex = 33;
		this.lblXUCL.Text = "...";
		this.lblXUCL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblCpk.AutoSize = true;
		this.lblCpk.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCpk.Location = new System.Drawing.Point(668, 50);
		this.lblCpk.Margin = new System.Windows.Forms.Padding(3);
		this.lblCpk.Name = "lblCpk";
		this.lblCpk.Size = new System.Drawing.Size(66, 16);
		this.lblCpk.TabIndex = 32;
		this.lblCpk.Text = "...";
		this.lblCpk.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblCp.AutoSize = true;
		this.lblCp.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCp.Location = new System.Drawing.Point(668, 27);
		this.lblCp.Margin = new System.Windows.Forms.Padding(3);
		this.lblCp.Name = "lblCp";
		this.lblCp.Size = new System.Drawing.Size(66, 16);
		this.lblCp.TabIndex = 31;
		this.lblCp.Text = "...";
		this.lblCp.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblSD.AutoSize = true;
		this.lblSD.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblSD.Location = new System.Drawing.Point(668, 4);
		this.lblSD.Margin = new System.Windows.Forms.Padding(3);
		this.lblSD.Name = "lblSD";
		this.lblSD.Size = new System.Drawing.Size(66, 16);
		this.lblSD.TabIndex = 30;
		this.lblSD.Text = "...";
		this.lblSD.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblMinimun.AutoSize = true;
		this.lblMinimun.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblMinimun.Location = new System.Drawing.Point(557, 50);
		this.lblMinimun.Margin = new System.Windows.Forms.Padding(3);
		this.lblMinimun.Name = "lblMinimun";
		this.lblMinimun.Size = new System.Drawing.Size(66, 16);
		this.lblMinimun.TabIndex = 29;
		this.lblMinimun.Text = "...";
		this.lblMinimun.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblMaximun.AutoSize = true;
		this.lblMaximun.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblMaximun.Location = new System.Drawing.Point(557, 27);
		this.lblMaximun.Margin = new System.Windows.Forms.Padding(3);
		this.lblMaximun.Name = "lblMaximun";
		this.lblMaximun.Size = new System.Drawing.Size(66, 16);
		this.lblMaximun.TabIndex = 28;
		this.lblMaximun.Text = "...";
		this.lblMaximun.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblAverage.AutoSize = true;
		this.lblAverage.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblAverage.Location = new System.Drawing.Point(557, 4);
		this.lblAverage.Margin = new System.Windows.Forms.Padding(3);
		this.lblAverage.Name = "lblAverage";
		this.lblAverage.Size = new System.Drawing.Size(66, 16);
		this.lblAverage.TabIndex = 27;
		this.lblAverage.Text = "...";
		this.lblAverage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblOutLSL.AutoSize = true;
		this.lblOutLSL.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblOutLSL.ForeColor = System.Drawing.Color.Red;
		this.lblOutLSL.Location = new System.Drawing.Point(417, 50);
		this.lblOutLSL.Margin = new System.Windows.Forms.Padding(3);
		this.lblOutLSL.Name = "lblOutLSL";
		this.lblOutLSL.Size = new System.Drawing.Size(66, 16);
		this.lblOutLSL.TabIndex = 26;
		this.lblOutLSL.Text = "...";
		this.lblOutLSL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblOK.AutoSize = true;
		this.lblOK.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblOK.ForeColor = System.Drawing.Color.Blue;
		this.lblOK.Location = new System.Drawing.Point(417, 27);
		this.lblOK.Margin = new System.Windows.Forms.Padding(3);
		this.lblOK.Name = "lblOK";
		this.lblOK.Size = new System.Drawing.Size(66, 16);
		this.lblOK.TabIndex = 25;
		this.lblOK.Text = "...";
		this.lblOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblOutUSL.AutoSize = true;
		this.lblOutUSL.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblOutUSL.ForeColor = System.Drawing.Color.Red;
		this.lblOutUSL.Location = new System.Drawing.Point(417, 4);
		this.lblOutUSL.Margin = new System.Windows.Forms.Padding(3);
		this.lblOutUSL.Name = "lblOutUSL";
		this.lblOutUSL.Size = new System.Drawing.Size(66, 16);
		this.lblOutUSL.TabIndex = 24;
		this.lblOutUSL.Text = "...";
		this.lblOutUSL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblN.AutoSize = true;
		this.lblN.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblN.Location = new System.Drawing.Point(273, 50);
		this.lblN.Margin = new System.Windows.Forms.Padding(3);
		this.lblN.Name = "lblN";
		this.lblN.Size = new System.Drawing.Size(66, 16);
		this.lblN.TabIndex = 23;
		this.lblN.Text = "...";
		this.lblN.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblLower.AutoSize = true;
		this.lblLower.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblLower.Location = new System.Drawing.Point(273, 27);
		this.lblLower.Margin = new System.Windows.Forms.Padding(3);
		this.lblLower.Name = "lblLower";
		this.lblLower.Size = new System.Drawing.Size(66, 16);
		this.lblLower.TabIndex = 22;
		this.lblLower.Text = "...";
		this.lblLower.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblUpper.AutoSize = true;
		this.lblUpper.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblUpper.Location = new System.Drawing.Point(273, 4);
		this.lblUpper.Margin = new System.Windows.Forms.Padding(3);
		this.lblUpper.Name = "lblUpper";
		this.lblUpper.Size = new System.Drawing.Size(66, 16);
		this.lblUpper.TabIndex = 21;
		this.lblUpper.Text = "...";
		this.lblUpper.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblStandard.AutoSize = true;
		this.lblStandard.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblStandard.Location = new System.Drawing.Point(73, 50);
		this.lblStandard.Margin = new System.Windows.Forms.Padding(3);
		this.lblStandard.Name = "lblStandard";
		this.lblStandard.Size = new System.Drawing.Size(115, 16);
		this.lblStandard.TabIndex = 20;
		this.lblStandard.Text = "...";
		this.lblStandard.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label18.AutoSize = true;
		this.label18.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label18.Location = new System.Drawing.Point(741, 50);
		this.label18.Margin = new System.Windows.Forms.Padding(3);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(51, 16);
		this.label18.TabIndex = 19;
		this.label18.Text = "R\u00af";
		this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.tpanelHeader);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "mInterpretingControl";
		base.Size = new System.Drawing.Size(1000, 70);
		base.Load += new System.EventHandler(mInterpretingControl_Load);
		this.tpanelHeader.ResumeLayout(false);
		this.tpanelHeader.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
