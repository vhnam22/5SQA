using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using _5S_QA_DL.APIContext;
using _5S_QA_Entities.Dtos;
using _5S_QA_System.Controls;

namespace _5S_QA_System;

public class mSearchScatter : UserControl
{
	private Form mForm;

	private string mRemember;

	private IContainer components = null;

	private TableLayoutPanel tableLayoutPanel1;

	private ToolTip toolTipMain;

	public ComboBox cbbProduct;

	public ComboBox cbbAxisX;

	private TableLayoutPanel tpanelMain;

	public DateTimePicker dtpDateFrom;

	public DateTimePicker dtpDateTo;

	public ComboBox cbbAxisY;

	private Label lblProduct;

	private Label lblAxisX;

	private Label lblFrom;

	private Label lblAxisY;

	private Label lblTo;

	private Button btnPreviousDateFrom;

	private Button btnMainDateFrom;

	private Button btnNextDateFrom;

	private Button btnPreviousDateTo;

	private Button btnMainDateTo;

	private Button btnNextDateTo;

	private Button btnSearch;

	public mSearchScatter()
	{
		InitializeComponent();
		Common.setControls(this, toolTipMain);
	}

	public void Init()
	{
		mForm = base.ParentForm;
		load_cbbProduct();
		cbbNormal_Enter(cbbProduct, null);
		cbbAxisX.SelectedIndexChanged += cbbProduct_SelectedIndexChanged;
		cbbAxisY.SelectedIndexChanged += cbbProduct_SelectedIndexChanged;
		cbbProduct.SelectedIndexChanged += cbbProduct_SelectedIndexChanged;
		dtpDateFrom.ValueChanged += cbbProduct_SelectedIndexChanged;
		dtpDateTo.ValueChanged += cbbProduct_SelectedIndexChanged;
		cbbProduct_SelectedIndexChanged(cbbProduct, null);
	}

	private void load_cbbProduct()
	{
		try
		{
			QueryArgs body = new QueryArgs
			{
				Predicate = "Measurements.Any()",
				Order = "Name",
				Page = 1,
				Limit = int.MaxValue
			};
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/Product/Gets").Result;
			DataTable dataTable = Common.getDataTable(result);
			if (dataTable != null)
			{
				dataTable.Dispose();
				cbbProduct.ValueMember = "Id";
				cbbProduct.DisplayMember = "CodeName";
				cbbProduct.DataSource = dataTable;
			}
		}
		catch (Exception ex)
		{
			cbbProduct.DataSource = null;
			if (ex.InnerException is ApiException { StatusCode: var statusCode } ex2)
			{
				if (statusCode.Equals(401))
				{
					if (MessageBox.Show(Common.getTextLanguage(this, "LoginAgain"), Common.getTextLanguage(this, "INFORMATION"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk).Equals(DialogResult.OK))
					{
						base.ParentForm.Close();
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

	private void load_cbbMeas()
	{
		try
		{
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "ProductId=@0 && ImportantId!=null";
			queryArgs.PredicateParameters = new string[1] { string.IsNullOrEmpty(cbbProduct.Text) ? Guid.Empty.ToString() : cbbProduct.SelectedValue.ToString() };
			queryArgs.Order = "Sort, Created";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs body = queryArgs;
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/Measurement/Gets").Result;
			DataTable dataTable = Common.getDataTable(result);
			if (dataTable == null)
			{
				dataTable = new DataTable();
				dataTable.Columns.Add("Id");
				dataTable.Columns.Add("Name");
				cbbAxisX.SelectedIndex = -1;
				cbbAxisY.SelectedIndex = -1;
			}
			dataTable.Dispose();
			cbbAxisX.ValueMember = "Id";
			cbbAxisX.DisplayMember = "Name";
			cbbAxisY.ValueMember = "Id";
			cbbAxisY.DisplayMember = "Name";
			cbbAxisX.DataSource = dataTable;
			cbbAxisY.DataSource = dataTable.Copy();
		}
		catch (Exception ex)
		{
			cbbAxisX.DataSource = null;
			if (ex.InnerException is ApiException { StatusCode: var statusCode } ex2)
			{
				if (statusCode.Equals(401))
				{
					if (MessageBox.Show(Common.getTextLanguage(this, "LoginAgain"), Common.getTextLanguage(this, "INFORMATION"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk).Equals(DialogResult.OK))
					{
						base.ParentForm.Close();
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

	private void cbbProduct_SelectedIndexChanged(object sender, EventArgs e)
	{
		Control control = sender as Control;
		if (control.Name.Equals(cbbProduct.Name))
		{
			load_cbbMeas();
		}
		btnSearch.PerformClick();
	}

	private void cbbNormal_Leave(object sender, EventArgs e)
	{
		ComboBox comboBox = sender as ComboBox;
		if (comboBox.SelectedIndex.Equals(-1))
		{
			comboBox.Text = mRemember;
		}
	}

	private void cbbNormal_Enter(object sender, EventArgs e)
	{
		ComboBox comboBox = sender as ComboBox;
		mRemember = comboBox.Text;
	}

	private void btnPreviousDateFrom_Click(object sender, EventArgs e)
	{
		dtpDateFrom.Value = dtpDateFrom.Value.AddDays(-1.0);
	}

	private void btnMainDateFrom_Click(object sender, EventArgs e)
	{
		dtpDateFrom.Value = DateTime.Now;
	}

	private void btnNextDateFrom_Click(object sender, EventArgs e)
	{
		dtpDateFrom.Value = dtpDateFrom.Value.AddDays(1.0);
	}

	private void btnPreviousDateTo_Click(object sender, EventArgs e)
	{
		dtpDateTo.Value = dtpDateTo.Value.AddDays(-1.0);
	}

	private void btnMainDateTo_Click(object sender, EventArgs e)
	{
		dtpDateTo.Value = DateTime.Now;
	}

	private void btnNextDateTo_Click(object sender, EventArgs e)
	{
		dtpDateTo.Value = dtpDateTo.Value.AddDays(1.0);
	}

	private void btnSearch_Click(object sender, EventArgs e)
	{
		OnClick(e);
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_5S_QA_System.mSearchChart));
		this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
		this.btnSearch = new System.Windows.Forms.Button();
		this.lblAxisY = new System.Windows.Forms.Label();
		this.lblAxisX = new System.Windows.Forms.Label();
		this.lblProduct = new System.Windows.Forms.Label();
		this.cbbAxisY = new System.Windows.Forms.ComboBox();
		this.cbbAxisX = new System.Windows.Forms.ComboBox();
		this.cbbProduct = new System.Windows.Forms.ComboBox();
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.dtpDateFrom = new System.Windows.Forms.DateTimePicker();
		this.dtpDateTo = new System.Windows.Forms.DateTimePicker();
		this.btnPreviousDateFrom = new System.Windows.Forms.Button();
		this.btnMainDateFrom = new System.Windows.Forms.Button();
		this.btnNextDateFrom = new System.Windows.Forms.Button();
		this.btnPreviousDateTo = new System.Windows.Forms.Button();
		this.btnMainDateTo = new System.Windows.Forms.Button();
		this.btnNextDateTo = new System.Windows.Forms.Button();
		this.tpanelMain = new System.Windows.Forms.TableLayoutPanel();
		this.lblTo = new System.Windows.Forms.Label();
		this.lblFrom = new System.Windows.Forms.Label();
		this.tableLayoutPanel1.SuspendLayout();
		this.tpanelMain.SuspendLayout();
		base.SuspendLayout();
		this.tableLayoutPanel1.AutoSize = true;
		this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		this.tableLayoutPanel1.ColumnCount = 7;
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25f));
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25f));
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel1.Controls.Add(this.btnSearch, 6, 0);
		this.tableLayoutPanel1.Controls.Add(this.lblAxisY, 4, 0);
		this.tableLayoutPanel1.Controls.Add(this.lblAxisX, 2, 0);
		this.tableLayoutPanel1.Controls.Add(this.lblProduct, 0, 0);
		this.tableLayoutPanel1.Controls.Add(this.cbbAxisY, 5, 0);
		this.tableLayoutPanel1.Controls.Add(this.cbbAxisX, 3, 0);
		this.tableLayoutPanel1.Controls.Add(this.cbbProduct, 1, 0);
		this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
		this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
		this.tableLayoutPanel1.Name = "tableLayoutPanel1";
		this.tableLayoutPanel1.RowCount = 1;
		this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel1.Size = new System.Drawing.Size(1036, 28);
		this.tableLayoutPanel1.TabIndex = 1;
		this.tableLayoutPanel1.TabStop = true;
		this.btnSearch.BackColor = System.Drawing.SystemColors.Control;
		this.btnSearch.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnSearch.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnSearch.FlatAppearance.BorderSize = 0;
		this.btnSearch.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.AppWorkspace;
		this.btnSearch.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLight;
		this.btnSearch.Image = (System.Drawing.Image)resources.GetObject("btnSearch.Image");
		this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnSearch.Location = new System.Drawing.Point(1009, 1);
		this.btnSearch.Margin = new System.Windows.Forms.Padding(0);
		this.btnSearch.Name = "btnSearch";
		this.btnSearch.Size = new System.Drawing.Size(26, 26);
		this.btnSearch.TabIndex = 4;
		this.toolTipMain.SetToolTip(this.btnSearch, "Search");
		this.btnSearch.UseVisualStyleBackColor = false;
		this.btnSearch.Click += new System.EventHandler(btnSearch_Click);
		this.lblAxisY.AutoSize = true;
		this.lblAxisY.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblAxisY.Location = new System.Drawing.Point(749, 1);
		this.lblAxisY.Name = "lblAxisY";
		this.lblAxisY.Size = new System.Drawing.Size(44, 26);
		this.lblAxisY.TabIndex = 59;
		this.lblAxisY.Text = "Axis Y";
		this.lblAxisY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblAxisX.AutoSize = true;
		this.lblAxisX.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblAxisX.Location = new System.Drawing.Point(487, 1);
		this.lblAxisX.Name = "lblAxisX";
		this.lblAxisX.Size = new System.Drawing.Size(43, 26);
		this.lblAxisX.TabIndex = 58;
		this.lblAxisX.Text = "Axis X";
		this.lblAxisX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblProduct.AutoSize = true;
		this.lblProduct.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblProduct.Location = new System.Drawing.Point(4, 1);
		this.lblProduct.Name = "lblProduct";
		this.lblProduct.Size = new System.Drawing.Size(53, 26);
		this.lblProduct.TabIndex = 56;
		this.lblProduct.Text = "Product";
		this.lblProduct.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.cbbAxisY.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cbbAxisY.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cbbAxisY.Dock = System.Windows.Forms.DockStyle.Fill;
		this.cbbAxisY.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cbbAxisY.FormattingEnabled = true;
		this.cbbAxisY.Location = new System.Drawing.Point(798, 2);
		this.cbbAxisY.Margin = new System.Windows.Forms.Padding(1);
		this.cbbAxisY.MaxLength = 250;
		this.cbbAxisY.Name = "cbbAxisY";
		this.cbbAxisY.Size = new System.Drawing.Size(209, 24);
		this.cbbAxisY.TabIndex = 3;
		this.toolTipMain.SetToolTip(this.cbbAxisY, "Select or enter template");
		this.cbbAxisY.Enter += new System.EventHandler(cbbNormal_Enter);
		this.cbbAxisY.Leave += new System.EventHandler(cbbNormal_Leave);
		this.cbbAxisX.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cbbAxisX.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cbbAxisX.Dock = System.Windows.Forms.DockStyle.Fill;
		this.cbbAxisX.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cbbAxisX.FormattingEnabled = true;
		this.cbbAxisX.Location = new System.Drawing.Point(535, 2);
		this.cbbAxisX.Margin = new System.Windows.Forms.Padding(1);
		this.cbbAxisX.MaxLength = 250;
		this.cbbAxisX.Name = "cbbAxisX";
		this.cbbAxisX.Size = new System.Drawing.Size(209, 24);
		this.cbbAxisX.TabIndex = 2;
		this.toolTipMain.SetToolTip(this.cbbAxisX, "Select or enter measurement");
		this.cbbAxisX.Enter += new System.EventHandler(cbbNormal_Enter);
		this.cbbAxisX.Leave += new System.EventHandler(cbbNormal_Leave);
		this.cbbProduct.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cbbProduct.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cbbProduct.Dock = System.Windows.Forms.DockStyle.Fill;
		this.cbbProduct.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cbbProduct.FormattingEnabled = true;
		this.cbbProduct.Location = new System.Drawing.Point(62, 2);
		this.cbbProduct.Margin = new System.Windows.Forms.Padding(1);
		this.cbbProduct.MaxLength = 250;
		this.cbbProduct.Name = "cbbProduct";
		this.cbbProduct.Size = new System.Drawing.Size(420, 24);
		this.cbbProduct.TabIndex = 1;
		this.toolTipMain.SetToolTip(this.cbbProduct, "Select or enter product");
		this.cbbProduct.Enter += new System.EventHandler(cbbNormal_Enter);
		this.cbbProduct.Leave += new System.EventHandler(cbbNormal_Leave);
		this.dtpDateFrom.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.dtpDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.dtpDateFrom.Location = new System.Drawing.Point(139, 7);
		this.dtpDateFrom.Margin = new System.Windows.Forms.Padding(0);
		this.dtpDateFrom.Name = "dtpDateFrom";
		this.dtpDateFrom.Size = new System.Drawing.Size(120, 22);
		this.dtpDateFrom.TabIndex = 4;
		this.toolTipMain.SetToolTip(this.dtpDateFrom, "Select or enter from date want search");
		this.dtpDateTo.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.dtpDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.dtpDateTo.Location = new System.Drawing.Point(292, 7);
		this.dtpDateTo.Margin = new System.Windows.Forms.Padding(0);
		this.dtpDateTo.Name = "dtpDateTo";
		this.dtpDateTo.Size = new System.Drawing.Size(120, 22);
		this.dtpDateTo.TabIndex = 5;
		this.toolTipMain.SetToolTip(this.dtpDateTo, "Select or enter to date want search");
		this.btnPreviousDateFrom.BackColor = System.Drawing.SystemColors.Control;
		this.btnPreviousDateFrom.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnPreviousDateFrom.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.btnPreviousDateFrom.Location = new System.Drawing.Point(47, 5);
		this.btnPreviousDateFrom.Margin = new System.Windows.Forms.Padding(0);
		this.btnPreviousDateFrom.Name = "btnPreviousDateFrom";
		this.btnPreviousDateFrom.Size = new System.Drawing.Size(24, 24);
		this.btnPreviousDateFrom.TabIndex = 1;
		this.btnPreviousDateFrom.Text = "<";
		this.toolTipMain.SetToolTip(this.btnPreviousDateFrom, "Goto previous date from");
		this.btnPreviousDateFrom.UseVisualStyleBackColor = false;
		this.btnPreviousDateFrom.Click += new System.EventHandler(btnPreviousDateFrom_Click);
		this.btnMainDateFrom.AutoSize = true;
		this.btnMainDateFrom.BackColor = System.Drawing.SystemColors.Control;
		this.btnMainDateFrom.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnMainDateFrom.Dock = System.Windows.Forms.DockStyle.Top;
		this.btnMainDateFrom.Location = new System.Drawing.Point(71, 3);
		this.btnMainDateFrom.Margin = new System.Windows.Forms.Padding(0);
		this.btnMainDateFrom.Name = "btnMainDateFrom";
		this.btnMainDateFrom.Size = new System.Drawing.Size(44, 26);
		this.btnMainDateFrom.TabIndex = 2;
		this.btnMainDateFrom.Text = "Now";
		this.toolTipMain.SetToolTip(this.btnMainDateFrom, "Goto current date from");
		this.btnMainDateFrom.UseVisualStyleBackColor = false;
		this.btnMainDateFrom.Click += new System.EventHandler(btnMainDateFrom_Click);
		this.btnNextDateFrom.BackColor = System.Drawing.SystemColors.Control;
		this.btnNextDateFrom.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnNextDateFrom.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.btnNextDateFrom.Location = new System.Drawing.Point(115, 5);
		this.btnNextDateFrom.Margin = new System.Windows.Forms.Padding(0);
		this.btnNextDateFrom.Name = "btnNextDateFrom";
		this.btnNextDateFrom.Size = new System.Drawing.Size(24, 24);
		this.btnNextDateFrom.TabIndex = 3;
		this.btnNextDateFrom.Text = ">";
		this.toolTipMain.SetToolTip(this.btnNextDateFrom, "Goto next date from");
		this.btnNextDateFrom.UseVisualStyleBackColor = false;
		this.btnNextDateFrom.Click += new System.EventHandler(btnNextDateFrom_Click);
		this.btnPreviousDateTo.BackColor = System.Drawing.SystemColors.Control;
		this.btnPreviousDateTo.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnPreviousDateTo.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.btnPreviousDateTo.Location = new System.Drawing.Point(412, 5);
		this.btnPreviousDateTo.Margin = new System.Windows.Forms.Padding(0);
		this.btnPreviousDateTo.Name = "btnPreviousDateTo";
		this.btnPreviousDateTo.Size = new System.Drawing.Size(24, 24);
		this.btnPreviousDateTo.TabIndex = 6;
		this.btnPreviousDateTo.Text = "<";
		this.toolTipMain.SetToolTip(this.btnPreviousDateTo, "Goto previous date to");
		this.btnPreviousDateTo.UseVisualStyleBackColor = false;
		this.btnPreviousDateTo.Click += new System.EventHandler(btnPreviousDateTo_Click);
		this.btnMainDateTo.AutoSize = true;
		this.btnMainDateTo.BackColor = System.Drawing.SystemColors.Control;
		this.btnMainDateTo.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnMainDateTo.Dock = System.Windows.Forms.DockStyle.Top;
		this.btnMainDateTo.Location = new System.Drawing.Point(436, 3);
		this.btnMainDateTo.Margin = new System.Windows.Forms.Padding(0);
		this.btnMainDateTo.Name = "btnMainDateTo";
		this.btnMainDateTo.Size = new System.Drawing.Size(44, 26);
		this.btnMainDateTo.TabIndex = 7;
		this.btnMainDateTo.Text = "Now";
		this.toolTipMain.SetToolTip(this.btnMainDateTo, "Goto current date to");
		this.btnMainDateTo.UseVisualStyleBackColor = false;
		this.btnMainDateTo.Click += new System.EventHandler(btnMainDateTo_Click);
		this.btnNextDateTo.BackColor = System.Drawing.SystemColors.Control;
		this.btnNextDateTo.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnNextDateTo.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.btnNextDateTo.Location = new System.Drawing.Point(480, 5);
		this.btnNextDateTo.Margin = new System.Windows.Forms.Padding(0);
		this.btnNextDateTo.Name = "btnNextDateTo";
		this.btnNextDateTo.Size = new System.Drawing.Size(24, 24);
		this.btnNextDateTo.TabIndex = 8;
		this.btnNextDateTo.Text = ">";
		this.toolTipMain.SetToolTip(this.btnNextDateTo, "Goto next date to");
		this.btnNextDateTo.UseVisualStyleBackColor = false;
		this.btnNextDateTo.Click += new System.EventHandler(btnNextDateTo_Click);
		this.tpanelMain.AutoSize = true;
		this.tpanelMain.ColumnCount = 11;
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tpanelMain.Controls.Add(this.btnNextDateTo, 9, 0);
		this.tpanelMain.Controls.Add(this.btnMainDateTo, 8, 0);
		this.tpanelMain.Controls.Add(this.btnPreviousDateTo, 7, 0);
		this.tpanelMain.Controls.Add(this.btnNextDateFrom, 3, 0);
		this.tpanelMain.Controls.Add(this.btnMainDateFrom, 2, 0);
		this.tpanelMain.Controls.Add(this.btnPreviousDateFrom, 1, 0);
		this.tpanelMain.Controls.Add(this.lblTo, 5, 0);
		this.tpanelMain.Controls.Add(this.lblFrom, 0, 0);
		this.tpanelMain.Controls.Add(this.dtpDateFrom, 4, 0);
		this.tpanelMain.Controls.Add(this.dtpDateTo, 6, 0);
		this.tpanelMain.Dock = System.Windows.Forms.DockStyle.Top;
		this.tpanelMain.Location = new System.Drawing.Point(3, 31);
		this.tpanelMain.Margin = new System.Windows.Forms.Padding(0);
		this.tpanelMain.Name = "tpanelMain";
		this.tpanelMain.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
		this.tpanelMain.RowCount = 1;
		this.tpanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tpanelMain.Size = new System.Drawing.Size(1036, 29);
		this.tpanelMain.TabIndex = 2;
		this.tpanelMain.TabStop = true;
		this.lblTo.AutoSize = true;
		this.lblTo.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTo.Location = new System.Drawing.Point(262, 4);
		this.lblTo.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
		this.lblTo.Name = "lblTo";
		this.lblTo.Size = new System.Drawing.Size(27, 24);
		this.lblTo.TabIndex = 61;
		this.lblTo.Text = "To:";
		this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblFrom.AutoSize = true;
		this.lblFrom.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblFrom.Location = new System.Drawing.Point(3, 4);
		this.lblFrom.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
		this.lblFrom.Name = "lblFrom";
		this.lblFrom.Size = new System.Drawing.Size(41, 24);
		this.lblFrom.TabIndex = 59;
		this.lblFrom.Text = "From:";
		this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoSize = true;
		base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		base.Controls.Add(this.tpanelMain);
		base.Controls.Add(this.tableLayoutPanel1);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Name = "mSearchScatter";
		base.Padding = new System.Windows.Forms.Padding(3);
		base.Size = new System.Drawing.Size(1042, 63);
		this.tableLayoutPanel1.ResumeLayout(false);
		this.tableLayoutPanel1.PerformLayout();
		this.tpanelMain.ResumeLayout(false);
		this.tpanelMain.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
