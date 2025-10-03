using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using _5S_QA_Entities.Models;
using _5S_QA_System.Controls;

namespace _5S_QA_System;

public class mInformation : UserControl
{
	private readonly ProductViewModel mProduct;

	private readonly List<object> mDates;

	private readonly string mTitle;

	private IContainer components = null;

	private Label lblDate;

	private TransparentLabel lblTitle;

	private TableLayoutPanel tpanelInformation;

	private Label lblTitleCustomer;

	private Label lblTitleProduct;

	private Label lblTitleStage;

	private Label lblTitleTimeStart;

	private Label lblTitleDateStart;

	private Label lblTitleLotStart;

	private Label lblTitleTimeEnd;

	private Label lblTitleDateEnd;

	private Label lblTitleLotEnd;

	private Label lblTimeEnd;

	private Label lblTimeStart;

	private Label lblProduct;

	private Label lblDateEnd;

	private Label lblDateStart;

	private Label lblStage;

	private Label lblLotStart;

	private Label lblLotEnd;

	private Label lblCustomer;

	public mInformation(string title, ProductViewModel product, List<object> dates)
	{
		InitializeComponent();
		mProduct = product;
		mDates = dates;
		mTitle = title;
		Common.setControls(this);
	}

	private void mInformation_Load(object sender, EventArgs e)
	{
		Init();
	}

	private void Init()
	{
		lblDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
		lblTitle.Text = mTitle.ToUpper();
		lblStage.Text = mProduct.Name;
		lblProduct.Text = mProduct.GroupCodeName;
		lblDateStart.Text = ((DateTimeOffset)mDates.First()).ToString("yyyy/MM/dd");
		lblDateEnd.Text = ((DateTimeOffset)mDates[1]).ToString("yyyy/MM/dd");
		lblLotStart.Text = mDates[2]?.ToString();
		lblLotEnd.Text = mDates[3]?.ToString();
		lblTimeStart.Text = ((DateTimeOffset)mDates[4]).ToString("HH:mm:ss");
		lblTimeEnd.Text = ((DateTimeOffset)mDates[5]).ToString("HH:mm:ss");
		lblCustomer.Text = $"{mDates[6]} - {mDates.Last()}";
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
		this.lblDate = new System.Windows.Forms.Label();
		this.lblTitle = new _5S_QA_System.TransparentLabel();
		this.tpanelInformation = new System.Windows.Forms.TableLayoutPanel();
		this.lblTimeEnd = new System.Windows.Forms.Label();
		this.lblTimeStart = new System.Windows.Forms.Label();
		this.lblProduct = new System.Windows.Forms.Label();
		this.lblDateEnd = new System.Windows.Forms.Label();
		this.lblDateStart = new System.Windows.Forms.Label();
		this.lblStage = new System.Windows.Forms.Label();
		this.lblLotStart = new System.Windows.Forms.Label();
		this.lblLotEnd = new System.Windows.Forms.Label();
		this.lblCustomer = new System.Windows.Forms.Label();
		this.lblTitleTimeEnd = new System.Windows.Forms.Label();
		this.lblTitleDateEnd = new System.Windows.Forms.Label();
		this.lblTitleLotEnd = new System.Windows.Forms.Label();
		this.lblTitleProduct = new System.Windows.Forms.Label();
		this.lblTitleStage = new System.Windows.Forms.Label();
		this.lblTitleTimeStart = new System.Windows.Forms.Label();
		this.lblTitleDateStart = new System.Windows.Forms.Label();
		this.lblTitleLotStart = new System.Windows.Forms.Label();
		this.lblTitleCustomer = new System.Windows.Forms.Label();
		this.tpanelInformation.SuspendLayout();
		base.SuspendLayout();
		this.lblDate.AutoSize = true;
		this.lblDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblDate.ForeColor = System.Drawing.Color.Blue;
		this.lblDate.Location = new System.Drawing.Point(0, 0);
		this.lblDate.Name = "lblDate";
		this.lblDate.Size = new System.Drawing.Size(19, 16);
		this.lblDate.TabIndex = 0;
		this.lblDate.Text = "...";
		this.lblTitle.BackColor = System.Drawing.Color.Transparent;
		this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
		this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25f, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTitle.Location = new System.Drawing.Point(0, 0);
		this.lblTitle.Name = "lblTitle";
		this.lblTitle.Opacity = 50;
		this.lblTitle.Size = new System.Drawing.Size(1000, 40);
		this.lblTitle.TabIndex = 1;
		this.lblTitle.Text = "...";
		this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblTitle.TransparentBackColor = System.Drawing.Color.Transparent;
		this.tpanelInformation.AutoSize = true;
		this.tpanelInformation.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		this.tpanelInformation.ColumnCount = 6;
		this.tpanelInformation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelInformation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333f));
		this.tpanelInformation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelInformation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333f));
		this.tpanelInformation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelInformation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333f));
		this.tpanelInformation.Controls.Add(this.lblTimeEnd, 5, 2);
		this.tpanelInformation.Controls.Add(this.lblTimeStart, 5, 1);
		this.tpanelInformation.Controls.Add(this.lblProduct, 5, 0);
		this.tpanelInformation.Controls.Add(this.lblDateEnd, 3, 2);
		this.tpanelInformation.Controls.Add(this.lblDateStart, 3, 1);
		this.tpanelInformation.Controls.Add(this.lblStage, 3, 0);
		this.tpanelInformation.Controls.Add(this.lblLotStart, 1, 1);
		this.tpanelInformation.Controls.Add(this.lblLotEnd, 1, 2);
		this.tpanelInformation.Controls.Add(this.lblCustomer, 1, 0);
		this.tpanelInformation.Controls.Add(this.lblTitleTimeEnd, 4, 2);
		this.tpanelInformation.Controls.Add(this.lblTitleDateEnd, 2, 2);
		this.tpanelInformation.Controls.Add(this.lblTitleLotEnd, 0, 2);
		this.tpanelInformation.Controls.Add(this.lblTitleProduct, 4, 0);
		this.tpanelInformation.Controls.Add(this.lblTitleStage, 2, 0);
		this.tpanelInformation.Controls.Add(this.lblTitleTimeStart, 4, 1);
		this.tpanelInformation.Controls.Add(this.lblTitleDateStart, 2, 1);
		this.tpanelInformation.Controls.Add(this.lblTitleLotStart, 0, 1);
		this.tpanelInformation.Controls.Add(this.lblTitleCustomer, 0, 0);
		this.tpanelInformation.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tpanelInformation.Location = new System.Drawing.Point(0, 40);
		this.tpanelInformation.Name = "tpanelInformation";
		this.tpanelInformation.RowCount = 3;
		this.tpanelInformation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22f));
		this.tpanelInformation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22f));
		this.tpanelInformation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22f));
		this.tpanelInformation.Size = new System.Drawing.Size(1000, 70);
		this.tpanelInformation.TabIndex = 1;
		this.lblTimeEnd.AutoSize = true;
		this.lblTimeEnd.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTimeEnd.Location = new System.Drawing.Point(760, 50);
		this.lblTimeEnd.Margin = new System.Windows.Forms.Padding(3);
		this.lblTimeEnd.Name = "lblTimeEnd";
		this.lblTimeEnd.Size = new System.Drawing.Size(236, 16);
		this.lblTimeEnd.TabIndex = 18;
		this.lblTimeEnd.Text = "...";
		this.lblTimeEnd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblTimeStart.AutoSize = true;
		this.lblTimeStart.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTimeStart.Location = new System.Drawing.Point(760, 27);
		this.lblTimeStart.Margin = new System.Windows.Forms.Padding(3);
		this.lblTimeStart.Name = "lblTimeStart";
		this.lblTimeStart.Size = new System.Drawing.Size(236, 16);
		this.lblTimeStart.TabIndex = 17;
		this.lblTimeStart.Text = "...";
		this.lblTimeStart.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblProduct.AutoSize = true;
		this.lblProduct.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblProduct.Location = new System.Drawing.Point(760, 4);
		this.lblProduct.Margin = new System.Windows.Forms.Padding(3);
		this.lblProduct.Name = "lblProduct";
		this.lblProduct.Size = new System.Drawing.Size(236, 16);
		this.lblProduct.TabIndex = 16;
		this.lblProduct.Text = "...";
		this.lblProduct.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblDateEnd.AutoSize = true;
		this.lblDateEnd.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblDateEnd.Location = new System.Drawing.Point(438, 50);
		this.lblDateEnd.Margin = new System.Windows.Forms.Padding(3);
		this.lblDateEnd.Name = "lblDateEnd";
		this.lblDateEnd.Size = new System.Drawing.Size(234, 16);
		this.lblDateEnd.TabIndex = 15;
		this.lblDateEnd.Text = "...";
		this.lblDateEnd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblDateStart.AutoSize = true;
		this.lblDateStart.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblDateStart.Location = new System.Drawing.Point(438, 27);
		this.lblDateStart.Margin = new System.Windows.Forms.Padding(3);
		this.lblDateStart.Name = "lblDateStart";
		this.lblDateStart.Size = new System.Drawing.Size(234, 16);
		this.lblDateStart.TabIndex = 14;
		this.lblDateStart.Text = "...";
		this.lblDateStart.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblStage.AutoSize = true;
		this.lblStage.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblStage.Location = new System.Drawing.Point(438, 4);
		this.lblStage.Margin = new System.Windows.Forms.Padding(3);
		this.lblStage.Name = "lblStage";
		this.lblStage.Size = new System.Drawing.Size(234, 16);
		this.lblStage.TabIndex = 13;
		this.lblStage.Text = "...";
		this.lblStage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblLotStart.AutoSize = true;
		this.lblLotStart.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblLotStart.Location = new System.Drawing.Point(110, 27);
		this.lblLotStart.Margin = new System.Windows.Forms.Padding(3);
		this.lblLotStart.Name = "lblLotStart";
		this.lblLotStart.Size = new System.Drawing.Size(234, 16);
		this.lblLotStart.TabIndex = 12;
		this.lblLotStart.Text = "...";
		this.lblLotStart.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblLotEnd.AutoSize = true;
		this.lblLotEnd.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblLotEnd.Location = new System.Drawing.Point(110, 50);
		this.lblLotEnd.Margin = new System.Windows.Forms.Padding(3);
		this.lblLotEnd.Name = "lblLotEnd";
		this.lblLotEnd.Size = new System.Drawing.Size(234, 16);
		this.lblLotEnd.TabIndex = 11;
		this.lblLotEnd.Text = "...";
		this.lblLotEnd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblCustomer.AutoSize = true;
		this.lblCustomer.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCustomer.Location = new System.Drawing.Point(110, 4);
		this.lblCustomer.Margin = new System.Windows.Forms.Padding(3);
		this.lblCustomer.Name = "lblCustomer";
		this.lblCustomer.Size = new System.Drawing.Size(234, 16);
		this.lblCustomer.TabIndex = 10;
		this.lblCustomer.Text = "...";
		this.lblCustomer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblTitleTimeEnd.AutoSize = true;
		this.lblTitleTimeEnd.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitleTimeEnd.Location = new System.Drawing.Point(679, 50);
		this.lblTitleTimeEnd.Margin = new System.Windows.Forms.Padding(3);
		this.lblTitleTimeEnd.Name = "lblTitleTimeEnd";
		this.lblTitleTimeEnd.Size = new System.Drawing.Size(74, 16);
		this.lblTitleTimeEnd.TabIndex = 9;
		this.lblTitleTimeEnd.Text = "Time (end)";
		this.lblTitleTimeEnd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblTitleDateEnd.AutoSize = true;
		this.lblTitleDateEnd.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitleDateEnd.Location = new System.Drawing.Point(351, 50);
		this.lblTitleDateEnd.Margin = new System.Windows.Forms.Padding(3);
		this.lblTitleDateEnd.Name = "lblTitleDateEnd";
		this.lblTitleDateEnd.Size = new System.Drawing.Size(80, 16);
		this.lblTitleDateEnd.TabIndex = 8;
		this.lblTitleDateEnd.Text = "Date (end)";
		this.lblTitleDateEnd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblTitleLotEnd.AutoSize = true;
		this.lblTitleLotEnd.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitleLotEnd.Location = new System.Drawing.Point(4, 50);
		this.lblTitleLotEnd.Margin = new System.Windows.Forms.Padding(3);
		this.lblTitleLotEnd.Name = "lblTitleLotEnd";
		this.lblTitleLotEnd.Size = new System.Drawing.Size(99, 16);
		this.lblTitleLotEnd.TabIndex = 7;
		this.lblTitleLotEnd.Text = "To (end)";
		this.lblTitleLotEnd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblTitleProduct.AutoSize = true;
		this.lblTitleProduct.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitleProduct.Location = new System.Drawing.Point(679, 4);
		this.lblTitleProduct.Margin = new System.Windows.Forms.Padding(3);
		this.lblTitleProduct.Name = "lblTitleProduct";
		this.lblTitleProduct.Size = new System.Drawing.Size(74, 16);
		this.lblTitleProduct.TabIndex = 6;
		this.lblTitleProduct.Text = "Product";
		this.lblTitleProduct.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblTitleStage.AutoSize = true;
		this.lblTitleStage.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitleStage.Location = new System.Drawing.Point(351, 4);
		this.lblTitleStage.Margin = new System.Windows.Forms.Padding(3);
		this.lblTitleStage.Name = "lblTitleStage";
		this.lblTitleStage.Size = new System.Drawing.Size(80, 16);
		this.lblTitleStage.TabIndex = 5;
		this.lblTitleStage.Text = "Stage name";
		this.lblTitleStage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblTitleTimeStart.AutoSize = true;
		this.lblTitleTimeStart.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitleTimeStart.Location = new System.Drawing.Point(679, 27);
		this.lblTitleTimeStart.Margin = new System.Windows.Forms.Padding(3);
		this.lblTitleTimeStart.Name = "lblTitleTimeStart";
		this.lblTitleTimeStart.Size = new System.Drawing.Size(74, 16);
		this.lblTitleTimeStart.TabIndex = 4;
		this.lblTitleTimeStart.Text = "Time (start)";
		this.lblTitleTimeStart.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblTitleDateStart.AutoSize = true;
		this.lblTitleDateStart.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitleDateStart.Location = new System.Drawing.Point(351, 27);
		this.lblTitleDateStart.Margin = new System.Windows.Forms.Padding(3);
		this.lblTitleDateStart.Name = "lblTitleDateStart";
		this.lblTitleDateStart.Size = new System.Drawing.Size(80, 16);
		this.lblTitleDateStart.TabIndex = 3;
		this.lblTitleDateStart.Text = "Date (start)";
		this.lblTitleDateStart.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblTitleLotStart.AutoSize = true;
		this.lblTitleLotStart.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitleLotStart.Location = new System.Drawing.Point(4, 27);
		this.lblTitleLotStart.Margin = new System.Windows.Forms.Padding(3);
		this.lblTitleLotStart.Name = "lblTitleLotStart";
		this.lblTitleLotStart.Size = new System.Drawing.Size(99, 16);
		this.lblTitleLotStart.TabIndex = 2;
		this.lblTitleLotStart.Text = "Lot range (start)";
		this.lblTitleLotStart.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblTitleCustomer.AutoSize = true;
		this.lblTitleCustomer.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitleCustomer.Location = new System.Drawing.Point(4, 4);
		this.lblTitleCustomer.Margin = new System.Windows.Forms.Padding(3);
		this.lblTitleCustomer.Name = "lblTitleCustomer";
		this.lblTitleCustomer.Size = new System.Drawing.Size(99, 16);
		this.lblTitleCustomer.TabIndex = 1;
		this.lblTitleCustomer.Text = "Customer";
		this.lblTitleCustomer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoSize = true;
		base.Controls.Add(this.tpanelInformation);
		base.Controls.Add(this.lblTitle);
		base.Controls.Add(this.lblDate);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "mInformation";
		base.Size = new System.Drawing.Size(1000, 110);
		base.Load += new System.EventHandler(mInformation_Load);
		this.tpanelInformation.ResumeLayout(false);
		this.tpanelInformation.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
