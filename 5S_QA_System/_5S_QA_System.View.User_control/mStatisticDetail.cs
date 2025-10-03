using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using _5S_QA_DL.APIContext;
using _5S_QA_Entities.Dtos;
using _5S_QA_Entities.Models;
using _5S_QA_System.Controls;

namespace _5S_QA_System.View.User_control;

public class mStatisticDetail : UserControl
{
	private readonly StatisticDetailDto mDto;

	private readonly bool isLeft;

	private IContainer components = null;

	private Button btnClose;

	private ToolTip toolTipMain;

	private TableLayoutPanel tpanelTitle;

	private Label lblTitleProduct;

	private Label lblTitleStage;

	private Label lblStage;

	private Label lblProduct;

	private Panel panelResize;

	private Label lblMeasurement;

	private Label lblMeas;

	private DataGridView dgvMain;

	private Label lblMeasValue;

	private Label lblMeasurementValue;

	private DataGridViewTextBoxColumn No;

	private DataGridViewTextBoxColumn RequestName;

	private DataGridViewTextBoxColumn Date;

	private DataGridViewTextBoxColumn Lot;

	private DataGridViewTextBoxColumn Line;

	private DataGridViewTextBoxColumn Type;

	private DataGridViewTextBoxColumn name;

	private DataGridViewTextBoxColumn Value;

	private DataGridViewTextBoxColumn Unit;

	private DataGridViewTextBoxColumn Upper;

	private DataGridViewTextBoxColumn Lower;

	private DataGridViewTextBoxColumn Cavity;

	private DataGridViewTextBoxColumn Sample;

	private DataGridViewTextBoxColumn Result;

	private DataGridViewTextBoxColumn Judge;

	public mStatisticDetail(StatisticDetailDto dto, bool left = true)
	{
		InitializeComponent();
		mDto = dto;
		isLeft = left;
	}

	private void mStatisticDetail_Load(object sender, EventArgs e)
	{
		Init();
	}

	private void Init()
	{
		if (isLeft)
		{
			Dock = DockStyle.Left;
			panelResize.Dock = DockStyle.Right;
			ControlResize.Init(panelResize, this, ControlResize.Direction.HorizontalRight, Cursors.SizeWE);
		}
		else
		{
			lblMeasurement.Name = "lblType";
			lblMeasurementValue.Visible = false;
			lblMeasValue.Visible = false;
			dgvMain.Columns["Type"].Visible = false;
			dgvMain.Columns["name"].Visible = true;
			Dock = DockStyle.Right;
			panelResize.Dock = DockStyle.Left;
			ControlResize.Init(panelResize, this, ControlResize.Direction.Horizontal, Cursors.SizeWE);
		}
		Common.setControls(this, toolTipMain);
		lblProduct.Text = mDto.ProductCode.Split('#').First();
		lblMeas.Text = mDto.MeasurementName;
		load_Title();
		load_AllData();
	}

	private void load_Title()
	{
		Cursor.Current = Cursors.WaitCursor;
		try
		{
			string[] source = mDto.ProductCode.Split('#');
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "Name=@0 && Product.Name=@1 && Product.Group.Code=@2";
			queryArgs.PredicateParameters = new string[3]
			{
				mDto.MeasurementName.Split('(').FirstOrDefault().Trim(),
				source.Last(),
				source.First()
			};
			queryArgs.Page = 1;
			queryArgs.Limit = 1;
			QueryArgs queryArgs2 = queryArgs;
			if (!isLeft)
			{
				queryArgs2.Predicate = "Product.Name=@0 && Product.Group.Code=@1";
				queryArgs2.PredicateParameters = new string[2]
				{
					source.Last(),
					source.First()
				};
			}
			ResponseDto result = frmLogin.client.GetsAsync(queryArgs2, "/api/Measurement/Gets").Result;
			DataTable dataTable = Common.getDataTable<MeasurementStatisticViewModel>(result);
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				if (isLeft)
				{
					lblMeas.Text = dataTable.Rows[0]["Name"].ToString();
					lblMeasValue.Text = string.Format("{0} {1}", dataTable.Rows[0]["Value"], string.IsNullOrEmpty(dataTable.Rows[0]["Upper"].ToString()) ? "" : string.Format("({0}/{1})", dataTable.Rows[0]["Upper"], dataTable.Rows[0]["Lower"]));
					mDto.MeasurementId = Guid.Parse(dataTable.Rows[0]["Id"].ToString());
				}
				else
				{
					mDto.MeasurementId = Guid.Empty;
				}
				mDto.ProductId = Guid.Parse(dataTable.Rows[0]["ProductId"].ToString());
				lblStage.Text = dataTable.Rows[0]["ProductName"].ToString();
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
						btnClose.PerformClick();
					}
				}
				else
				{
					MessageBox.Show(ex2.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			else
			{
				MessageBox.Show(ex.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
	}

	private void load_AllData()
	{
		Cursor.Current = Cursors.WaitCursor;
		try
		{
			ResponseDto result = frmLogin.client.GetsAsync(mDto, "/api/RequestResult/GetForStatistics").Result;
			dgvMain.DataSource = Common.getDataTableNoType<ResultStatisticViewModel>(result);
			dgvMain.Refresh();
			dgvMain_Sorted(dgvMain, null);
		}
		catch (Exception ex)
		{
			if (ex.InnerException is ApiException { StatusCode: var statusCode } ex2)
			{
				if (statusCode.Equals(401))
				{
					if (MessageBox.Show(Common.getTextLanguage(this, "LoginAgain"), Common.getTextLanguage(this, "INFORMATION"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk).Equals(DialogResult.OK))
					{
						btnClose.PerformClick();
					}
				}
				else
				{
					MessageBox.Show(ex2.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			else
			{
				MessageBox.Show(ex.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
	}

	private void btnClose_Click(object sender, EventArgs e)
	{
		base.Parent?.Controls.Remove(this);
	}

	private void dgvMain_Sorted(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		DataGridView dataGridView = sender as DataGridView;
		foreach (DataGridViewRow item in (IEnumerable)dataGridView.Rows)
		{
			item.Cells["No"].Value = item.Index + 1;
			if (item.Cells["Unit"].Value?.ToString() == "°")
			{
				if (double.TryParse(item.Cells["Value"].Value?.ToString(), out var result))
				{
					item.Cells["Value"].Value = Common.ConvertDoubleToDegrees(result);
				}
				if (double.TryParse(item.Cells["Upper"].Value?.ToString(), out var result2))
				{
					item.Cells["Upper"].Value = Common.ConvertDoubleToDegrees(result2);
				}
				if (double.TryParse(item.Cells["Lower"].Value?.ToString(), out var result3))
				{
					item.Cells["Lower"].Value = Common.ConvertDoubleToDegrees(result3);
				}
				if (double.TryParse(item.Cells["Result"].Value?.ToString(), out var result4))
				{
					item.Cells["Result"].Value = Common.ConvertDoubleToDegrees(result4);
				}
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
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
		this.btnClose = new System.Windows.Forms.Button();
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.tpanelTitle = new System.Windows.Forms.TableLayoutPanel();
		this.lblMeasValue = new System.Windows.Forms.Label();
		this.lblMeasurementValue = new System.Windows.Forms.Label();
		this.lblMeas = new System.Windows.Forms.Label();
		this.lblMeasurement = new System.Windows.Forms.Label();
		this.lblStage = new System.Windows.Forms.Label();
		this.lblProduct = new System.Windows.Forms.Label();
		this.lblTitleStage = new System.Windows.Forms.Label();
		this.lblTitleProduct = new System.Windows.Forms.Label();
		this.panelResize = new System.Windows.Forms.Panel();
		this.dgvMain = new System.Windows.Forms.DataGridView();
		this.No = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.RequestName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Lot = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Line = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Upper = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Lower = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Cavity = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Sample = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Result = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Judge = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.tpanelTitle.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvMain).BeginInit();
		base.SuspendLayout();
		this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnClose.Location = new System.Drawing.Point(1018, 1);
		this.btnClose.Margin = new System.Windows.Forms.Padding(0);
		this.btnClose.Name = "btnClose";
		this.btnClose.Size = new System.Drawing.Size(28, 28);
		this.btnClose.TabIndex = 1;
		this.btnClose.Text = "X";
		this.toolTipMain.SetToolTip(this.btnClose, "Close");
		this.btnClose.UseVisualStyleBackColor = true;
		this.btnClose.Click += new System.EventHandler(btnClose_Click);
		this.tpanelTitle.AutoSize = true;
		this.tpanelTitle.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		this.tpanelTitle.ColumnCount = 5;
		this.tpanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tpanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tpanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelTitle.Controls.Add(this.lblMeasValue, 3, 1);
		this.tpanelTitle.Controls.Add(this.lblMeasurementValue, 2, 1);
		this.tpanelTitle.Controls.Add(this.lblMeas, 1, 1);
		this.tpanelTitle.Controls.Add(this.lblMeasurement, 0, 1);
		this.tpanelTitle.Controls.Add(this.lblStage, 3, 0);
		this.tpanelTitle.Controls.Add(this.lblProduct, 1, 0);
		this.tpanelTitle.Controls.Add(this.lblTitleStage, 2, 0);
		this.tpanelTitle.Controls.Add(this.btnClose, 4, 0);
		this.tpanelTitle.Controls.Add(this.lblTitleProduct, 0, 0);
		this.tpanelTitle.Dock = System.Windows.Forms.DockStyle.Top;
		this.tpanelTitle.Location = new System.Drawing.Point(0, 0);
		this.tpanelTitle.Name = "tpanelTitle";
		this.tpanelTitle.RowCount = 2;
		this.tpanelTitle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tpanelTitle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28f));
		this.tpanelTitle.Size = new System.Drawing.Size(1047, 59);
		this.tpanelTitle.TabIndex = 1;
		this.lblMeasValue.AutoSize = true;
		this.lblMeasValue.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblMeasValue.Location = new System.Drawing.Point(604, 30);
		this.lblMeasValue.Name = "lblMeasValue";
		this.lblMeasValue.Size = new System.Drawing.Size(410, 28);
		this.lblMeasValue.TabIndex = 8;
		this.lblMeasValue.Text = "...";
		this.lblMeasValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblMeasurementValue.AutoSize = true;
		this.lblMeasurementValue.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblMeasurementValue.Location = new System.Drawing.Point(517, 30);
		this.lblMeasurementValue.Name = "lblMeasurementValue";
		this.lblMeasurementValue.Size = new System.Drawing.Size(80, 28);
		this.lblMeasurementValue.TabIndex = 7;
		this.lblMeasurementValue.Text = "Value";
		this.lblMeasurementValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblMeas.AutoSize = true;
		this.lblMeas.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblMeas.Location = new System.Drawing.Point(100, 30);
		this.lblMeas.Name = "lblMeas";
		this.lblMeas.Size = new System.Drawing.Size(410, 28);
		this.lblMeas.TabIndex = 6;
		this.lblMeas.Text = "...";
		this.lblMeas.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblMeasurement.AutoSize = true;
		this.lblMeasurement.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblMeasurement.Location = new System.Drawing.Point(4, 30);
		this.lblMeasurement.Name = "lblMeasurement";
		this.lblMeasurement.Size = new System.Drawing.Size(89, 28);
		this.lblMeasurement.TabIndex = 5;
		this.lblMeasurement.Text = "Measurement";
		this.lblMeasurement.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblStage.AutoSize = true;
		this.lblStage.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblStage.Location = new System.Drawing.Point(604, 1);
		this.lblStage.Name = "lblStage";
		this.lblStage.Size = new System.Drawing.Size(410, 28);
		this.lblStage.TabIndex = 4;
		this.lblStage.Text = "...";
		this.lblStage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblProduct.AutoSize = true;
		this.lblProduct.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblProduct.Location = new System.Drawing.Point(100, 1);
		this.lblProduct.Name = "lblProduct";
		this.lblProduct.Size = new System.Drawing.Size(410, 28);
		this.lblProduct.TabIndex = 3;
		this.lblProduct.Text = "...";
		this.lblProduct.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblTitleStage.AutoSize = true;
		this.lblTitleStage.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitleStage.Location = new System.Drawing.Point(517, 1);
		this.lblTitleStage.Name = "lblTitleStage";
		this.lblTitleStage.Size = new System.Drawing.Size(80, 28);
		this.lblTitleStage.TabIndex = 2;
		this.lblTitleStage.Text = "Stage name";
		this.lblTitleStage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblTitleProduct.AutoSize = true;
		this.lblTitleProduct.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitleProduct.Location = new System.Drawing.Point(4, 1);
		this.lblTitleProduct.Name = "lblTitleProduct";
		this.lblTitleProduct.Size = new System.Drawing.Size(89, 28);
		this.lblTitleProduct.TabIndex = 1;
		this.lblTitleProduct.Text = "Product";
		this.lblTitleProduct.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panelResize.Dock = System.Windows.Forms.DockStyle.Right;
		this.panelResize.Location = new System.Drawing.Point(1044, 59);
		this.panelResize.Margin = new System.Windows.Forms.Padding(1);
		this.panelResize.Name = "panelResize";
		this.panelResize.Size = new System.Drawing.Size(3, 199);
		this.panelResize.TabIndex = 139;
		this.dgvMain.AllowUserToAddRows = false;
		this.dgvMain.AllowUserToDeleteRows = false;
		this.dgvMain.AllowUserToOrderColumns = true;
		this.dgvMain.AllowUserToResizeRows = false;
		dataGridViewCellStyle.BackColor = System.Drawing.SystemColors.Control;
		this.dgvMain.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle;
		this.dgvMain.BackgroundColor = System.Drawing.SystemColors.Window;
		dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.AppWorkspace;
		dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
		dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
		dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
		dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.dgvMain.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
		this.dgvMain.ColumnHeadersHeight = 26;
		this.dgvMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
		this.dgvMain.Columns.AddRange(this.No, this.RequestName, this.Date, this.Lot, this.Line, this.Type, this.name, this.Value, this.Unit, this.Upper, this.Lower, this.Cavity, this.Sample, this.Result, this.Judge);
		this.dgvMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.dgvMain.EnableHeadersVisualStyles = false;
		this.dgvMain.Location = new System.Drawing.Point(0, 59);
		this.dgvMain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.dgvMain.MultiSelect = false;
		this.dgvMain.Name = "dgvMain";
		this.dgvMain.ReadOnly = true;
		this.dgvMain.RowHeadersWidth = 25;
		this.dgvMain.Size = new System.Drawing.Size(1044, 199);
		this.dgvMain.TabIndex = 164;
		this.dgvMain.Sorted += new System.EventHandler(dgvMain_Sorted);
		this.No.DataPropertyName = "No";
		dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		this.No.DefaultCellStyle = dataGridViewCellStyle3;
		this.No.HeaderText = "No.";
		this.No.Name = "No";
		this.No.ReadOnly = true;
		this.No.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.No.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.No.Width = 40;
		this.RequestName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.RequestName.DataPropertyName = "RequestName";
		this.RequestName.FillWeight = 20f;
		this.RequestName.HeaderText = "Req. name";
		this.RequestName.Name = "RequestName";
		this.RequestName.ReadOnly = true;
		this.RequestName.Visible = false;
		this.Date.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Date.DataPropertyName = "Date";
		this.Date.FillWeight = 15f;
		this.Date.HeaderText = "Date";
		this.Date.Name = "Date";
		this.Date.ReadOnly = true;
		this.Lot.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Lot.DataPropertyName = "Lot";
		this.Lot.FillWeight = 15f;
		this.Lot.HeaderText = "Lot no.";
		this.Lot.Name = "Lot";
		this.Lot.ReadOnly = true;
		this.Line.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Line.DataPropertyName = "Line";
		this.Line.FillWeight = 15f;
		this.Line.HeaderText = "Line";
		this.Line.Name = "Line";
		this.Line.ReadOnly = true;
		this.Type.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Type.DataPropertyName = "Type";
		this.Type.FillWeight = 15f;
		this.Type.HeaderText = "Type";
		this.Type.Name = "Type";
		this.Type.ReadOnly = true;
		this.name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.name.DataPropertyName = "Name";
		this.name.FillWeight = 15f;
		this.name.HeaderText = "Name";
		this.name.Name = "name";
		this.name.ReadOnly = true;
		this.name.Visible = false;
		this.Value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Value.DataPropertyName = "Value";
		dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.Value.DefaultCellStyle = dataGridViewCellStyle4;
		this.Value.FillWeight = 15f;
		this.Value.HeaderText = "Value";
		this.Value.Name = "Value";
		this.Value.ReadOnly = true;
		this.Unit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Unit.DataPropertyName = "Unit";
		this.Unit.FillWeight = 15f;
		this.Unit.HeaderText = "Unit";
		this.Unit.Name = "Unit";
		this.Unit.ReadOnly = true;
		this.Upper.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Upper.DataPropertyName = "Upper";
		dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.Upper.DefaultCellStyle = dataGridViewCellStyle5;
		this.Upper.FillWeight = 15f;
		this.Upper.HeaderText = "Upper";
		this.Upper.Name = "Upper";
		this.Upper.ReadOnly = true;
		this.Lower.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Lower.DataPropertyName = "Lower";
		dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.Lower.DefaultCellStyle = dataGridViewCellStyle6;
		this.Lower.FillWeight = 15f;
		this.Lower.HeaderText = "Lower";
		this.Lower.Name = "Lower";
		this.Lower.ReadOnly = true;
		this.Cavity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Cavity.DataPropertyName = "Cavity";
		this.Cavity.FillWeight = 15f;
		this.Cavity.HeaderText = "Cavity";
		this.Cavity.Name = "Cavity";
		this.Cavity.ReadOnly = true;
		this.Sample.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Sample.DataPropertyName = "Sample";
		this.Sample.FillWeight = 15f;
		this.Sample.HeaderText = "Sample";
		this.Sample.Name = "Sample";
		this.Sample.ReadOnly = true;
		this.Result.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Result.DataPropertyName = "Result";
		dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		dataGridViewCellStyle7.ForeColor = System.Drawing.Color.Red;
		this.Result.DefaultCellStyle = dataGridViewCellStyle7;
		this.Result.FillWeight = 15f;
		this.Result.HeaderText = "Result";
		this.Result.Name = "Result";
		this.Result.ReadOnly = true;
		this.Judge.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Judge.DataPropertyName = "Judge";
		dataGridViewCellStyle8.ForeColor = System.Drawing.Color.Red;
		this.Judge.DefaultCellStyle = dataGridViewCellStyle8;
		this.Judge.FillWeight = 15f;
		this.Judge.HeaderText = "Judge";
		this.Judge.Name = "Judge";
		this.Judge.ReadOnly = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		base.Controls.Add(this.dgvMain);
		base.Controls.Add(this.panelResize);
		base.Controls.Add(this.tpanelTitle);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Name = "mStatisticDetail";
		base.Size = new System.Drawing.Size(1047, 258);
		base.Load += new System.EventHandler(mStatisticDetail_Load);
		this.tpanelTitle.ResumeLayout(false);
		this.tpanelTitle.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvMain).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
