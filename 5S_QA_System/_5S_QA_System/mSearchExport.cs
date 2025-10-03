using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using _5S_QA_DL.APIContext;
using _5S_QA_Entities.Dtos;
using _5S_QA_System.Controls;
using _5S_QA_System.Properties;
using MetroFramework;
using MetroFramework.Controls;

namespace _5S_QA_System;

public class mSearchExport : UserControl
{
	private int totalPage;

	private string oldcbbLimit;

	private string oldtxtPage;

	private Form mForm;

	private string mRemember;

	public Button btnRefresh;

	public string mFilter;

	private IContainer components = null;

	private TableLayoutPanel tpanelMain;

	private ToolTip toolTipMain;

	public DateTimePicker dtpDateTo;

	public MetroTextBox txtPage;

	private Button btnLastData;

	private Button btnNextData;

	private Button btnPreData;

	public ComboBox cbbLimit;

	private Button btnFirstData;

	public DateTimePicker dtpDateFrom;

	private TableLayoutPanel tableLayoutPanel1;

	private Label lblProduct;

	public Button btnSearch;

	private Label lblFrom;

	private Label lblTo;

	private Label lblLimit;

	private Label lblPage;

	private Label lblTotalRow;

	private Label lblTotalRows;

	private Button btnPreviousDateFrom;

	private Button btnMainDateFrom;

	private Button btnNextDateFrom;

	private Button btnPreviousDateTo;

	private Button btnMainDateTo;

	private Button btnNextDateTo;

	public ComboBox cbbProduct;

	public DataTable dataTable { get; set; }

	public DataTable dataTableFilter { get; set; }

	public mSearchExport()
	{
		InitializeComponent();
		Common.setControls(this, toolTipMain);
		btnRefresh = new Button();
		totalPage = 1;
		oldcbbLimit = cbbLimit.Text;
		oldtxtPage = txtPage.Text;
		mFilter = string.Empty;
	}

	public void Init()
	{
		mForm = base.ParentForm;
		getSetting();
		load_cbbProduct();
		cbbNormal_Enter(cbbProduct, null);
		dtpDateFrom.ValueChanged += cbbProduct_SelectedIndexChanged;
		dtpDateTo.ValueChanged += cbbProduct_SelectedIndexChanged;
		cbbProduct.SelectedIndexChanged += cbbProduct_SelectedIndexChanged;
		cbbProduct_SelectedIndexChanged(cbbProduct, null);
	}

	private void load_cbbProduct()
	{
		try
		{
			QueryArgs body = new QueryArgs
			{
				Predicate = "Products.Any()",
				Order = "Name",
				Page = 1,
				Limit = int.MaxValue
			};
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/ProductGroup/Gets").Result;
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

	public DataTable SearchInAllColums()
	{
		try
		{
			if (this.dataTable != null)
			{
				DataView dataView = new DataView(this.dataTable)
				{
					RowFilter = mFilter
				};
				DataRow[] array = dataView.ToTable().Rows.Cast<DataRow>().ToArray();
				dataTableFilter = array.CopyToDataTable();
				lblTotalRows.Text = array.Length.ToString();
				int num = int.Parse(cbbLimit.Text);
				int count = (int.Parse(txtPage.Text) - 1) * num;
				array = array.Skip(count).Take(num).ToArray();
				if (array.Length.Equals(0))
				{
					DataTable dataTable = this.dataTable.Clone();
					dataTable.Clear();
					return dataTable;
				}
				return array.CopyToDataTable();
			}
			throw new NotImplementedException();
		}
		catch
		{
			dataTableFilter = new DataTable();
			return this.dataTable;
		}
	}

	private void calc_totalPage()
	{
		try
		{
			long num = long.Parse(lblTotalRows.Text);
			int num2 = int.Parse(cbbLimit.Text);
			if (num.Equals(0L) || !(num % num2).Equals(0L))
			{
				totalPage = (int)(num / num2) + 1;
			}
			else
			{
				totalPage = (int)(num / num2);
			}
			if (int.Parse(txtPage.Text) > totalPage)
			{
				txtPage.Text = totalPage.ToString();
				btnSearch.PerformClick();
			}
		}
		catch
		{
			totalPage = 1;
			txtPage.Text = "1";
		}
	}

	private void getSetting()
	{
		Type type = mForm.GetType();
		if (type.Equals(typeof(frmMonthView)))
		{
			cbbLimit.Text = Settings.Default.limitMonth.ToString();
		}
	}

	private void setSetting()
	{
		Type type = mForm.GetType();
		if (type.Equals(typeof(frmMonthView)))
		{
			Settings.Default.limitMonth = int.Parse(cbbLimit.Text);
		}
		Settings.Default.Save();
	}

	private void cbbLimit_KeyPress(object sender, KeyPressEventArgs e)
	{
		if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b' && e.KeyChar != '\r')
		{
			e.Handled = true;
		}
		if (e.KeyChar == '\r')
		{
			lblTotalRows.Select();
		}
	}

	private void txtPage_KeyPress(object sender, KeyPressEventArgs e)
	{
		if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b' && e.KeyChar != '\r')
		{
			e.Handled = true;
		}
		if (e.KeyChar == '\r')
		{
			lblTotalRows.Select();
		}
	}

	private void cbbLimit_Leave(object sender, EventArgs e)
	{
		ComboBox comboBox = sender as ComboBox;
		try
		{
			if (string.IsNullOrEmpty(comboBox.Text) || int.Parse(comboBox.Text).Equals(0))
			{
				comboBox.SelectedIndex = 0;
			}
			else
			{
				comboBox.Text = int.Parse(comboBox.Text).ToString();
			}
			if (!oldcbbLimit.Equals(comboBox.Text))
			{
				txtPage.Text = "1";
				btnSearch.PerformClick();
				calc_totalPage();
			}
		}
		catch
		{
			comboBox.SelectedIndex = 0;
			txtPage.Text = "1";
		}
		finally
		{
			oldcbbLimit = comboBox.Text;
			setSetting();
		}
	}

	private void txtPage_Leave(object sender, EventArgs e)
	{
		MetroTextBox metroTextBox = sender as MetroTextBox;
		try
		{
			if (string.IsNullOrEmpty(metroTextBox.Text) || int.Parse(metroTextBox.Text).Equals(0))
			{
				metroTextBox.Text = "1";
			}
			else if (int.Parse(metroTextBox.Text) > totalPage)
			{
				metroTextBox.Text = totalPage.ToString();
			}
			else
			{
				metroTextBox.Text = int.Parse(metroTextBox.Text).ToString();
			}
			if (!oldtxtPage.Equals(metroTextBox.Text))
			{
				btnSearch.PerformClick();
			}
		}
		catch
		{
			metroTextBox.Text = "1";
		}
		finally
		{
			oldtxtPage = metroTextBox.Text;
		}
	}

	private void cbbLimit_SelectedIndexChanged(object sender, EventArgs e)
	{
		cbbLimit_Leave(sender, null);
	}

	private void btnFirstData_Click(object sender, EventArgs e)
	{
		int.TryParse(txtPage.Text, out var result);
		if (result > 1)
		{
			txtPage.Text = "1";
			btnSearch.PerformClick();
		}
	}

	private void btnLastData_Click(object sender, EventArgs e)
	{
		int.TryParse(txtPage.Text, out var result);
		if (result < totalPage)
		{
			txtPage.Text = totalPage.ToString();
			btnSearch.PerformClick();
		}
	}

	private void btnPreData_Click(object sender, EventArgs e)
	{
		int.TryParse(txtPage.Text, out var result);
		if (result > 1)
		{
			result--;
			txtPage.Text = result.ToString();
			btnSearch.PerformClick();
		}
	}

	private void btnNextData_Click(object sender, EventArgs e)
	{
		int.TryParse(txtPage.Text, out var result);
		if (result < totalPage)
		{
			result++;
			txtPage.Text = result.ToString();
			btnSearch.PerformClick();
		}
	}

	private void lblTotalRows_TextChanged(object sender, EventArgs e)
	{
		calc_totalPage();
	}

	private void cbbProduct_SelectedIndexChanged(object sender, EventArgs e)
	{
		btnRefresh.PerformClick();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_5S_QA_System.mSearchExport));
		this.tpanelMain = new System.Windows.Forms.TableLayoutPanel();
		this.lblTotalRows = new System.Windows.Forms.Label();
		this.btnNextDateTo = new System.Windows.Forms.Button();
		this.lblTotalRow = new System.Windows.Forms.Label();
		this.btnMainDateTo = new System.Windows.Forms.Button();
		this.btnLastData = new System.Windows.Forms.Button();
		this.txtPage = new MetroFramework.Controls.MetroTextBox();
		this.btnNextData = new System.Windows.Forms.Button();
		this.lblPage = new System.Windows.Forms.Label();
		this.btnPreData = new System.Windows.Forms.Button();
		this.btnPreviousDateTo = new System.Windows.Forms.Button();
		this.btnFirstData = new System.Windows.Forms.Button();
		this.lblLimit = new System.Windows.Forms.Label();
		this.btnNextDateFrom = new System.Windows.Forms.Button();
		this.btnMainDateFrom = new System.Windows.Forms.Button();
		this.btnPreviousDateFrom = new System.Windows.Forms.Button();
		this.cbbLimit = new System.Windows.Forms.ComboBox();
		this.lblTo = new System.Windows.Forms.Label();
		this.lblFrom = new System.Windows.Forms.Label();
		this.dtpDateFrom = new System.Windows.Forms.DateTimePicker();
		this.dtpDateTo = new System.Windows.Forms.DateTimePicker();
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.cbbProduct = new System.Windows.Forms.ComboBox();
		this.btnSearch = new System.Windows.Forms.Button();
		this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
		this.lblProduct = new System.Windows.Forms.Label();
		this.tpanelMain.SuspendLayout();
		this.tableLayoutPanel1.SuspendLayout();
		base.SuspendLayout();
		this.tpanelMain.AutoSize = true;
		this.tpanelMain.ColumnCount = 21;
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
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70f));
		this.tpanelMain.Controls.Add(this.lblTotalRows, 20, 0);
		this.tpanelMain.Controls.Add(this.btnNextDateTo, 9, 0);
		this.tpanelMain.Controls.Add(this.lblTotalRow, 19, 0);
		this.tpanelMain.Controls.Add(this.btnMainDateTo, 8, 0);
		this.tpanelMain.Controls.Add(this.btnLastData, 18, 0);
		this.tpanelMain.Controls.Add(this.txtPage, 14, 0);
		this.tpanelMain.Controls.Add(this.btnNextData, 17, 0);
		this.tpanelMain.Controls.Add(this.lblPage, 13, 0);
		this.tpanelMain.Controls.Add(this.btnPreData, 16, 0);
		this.tpanelMain.Controls.Add(this.btnPreviousDateTo, 7, 0);
		this.tpanelMain.Controls.Add(this.btnFirstData, 15, 0);
		this.tpanelMain.Controls.Add(this.lblLimit, 11, 0);
		this.tpanelMain.Controls.Add(this.btnNextDateFrom, 3, 0);
		this.tpanelMain.Controls.Add(this.btnMainDateFrom, 2, 0);
		this.tpanelMain.Controls.Add(this.btnPreviousDateFrom, 1, 0);
		this.tpanelMain.Controls.Add(this.cbbLimit, 12, 0);
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
		this.tpanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25f));
		this.tpanelMain.Size = new System.Drawing.Size(994, 28);
		this.tpanelMain.TabIndex = 2;
		this.tpanelMain.TabStop = true;
		this.lblTotalRows.AutoSize = true;
		this.lblTotalRows.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTotalRows.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotalRows.Location = new System.Drawing.Point(927, 4);
		this.lblTotalRows.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
		this.lblTotalRows.Name = "lblTotalRows";
		this.lblTotalRows.Size = new System.Drawing.Size(64, 23);
		this.lblTotalRows.TabIndex = 68;
		this.lblTotalRows.Text = "0";
		this.lblTotalRows.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblTotalRows.TextChanged += new System.EventHandler(lblTotalRows_TextChanged);
		this.btnNextDateTo.BackColor = System.Drawing.SystemColors.Control;
		this.btnNextDateTo.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnNextDateTo.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.btnNextDateTo.Location = new System.Drawing.Point(480, 4);
		this.btnNextDateTo.Margin = new System.Windows.Forms.Padding(0);
		this.btnNextDateTo.Name = "btnNextDateTo";
		this.btnNextDateTo.Size = new System.Drawing.Size(24, 24);
		this.btnNextDateTo.TabIndex = 8;
		this.btnNextDateTo.Text = ">";
		this.toolTipMain.SetToolTip(this.btnNextDateTo, "Goto next date to");
		this.btnNextDateTo.UseVisualStyleBackColor = false;
		this.btnNextDateTo.Click += new System.EventHandler(btnNextDateTo_Click);
		this.lblTotalRow.AutoSize = true;
		this.lblTotalRow.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTotalRow.Location = new System.Drawing.Point(849, 4);
		this.lblTotalRow.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
		this.lblTotalRow.Name = "lblTotalRow";
		this.lblTotalRow.Size = new System.Drawing.Size(72, 23);
		this.lblTotalRow.TabIndex = 67;
		this.lblTotalRow.Text = "Total rows:";
		this.lblTotalRow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.btnMainDateTo.AutoSize = true;
		this.btnMainDateTo.BackColor = System.Drawing.SystemColors.Control;
		this.btnMainDateTo.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnMainDateTo.Dock = System.Windows.Forms.DockStyle.Top;
		this.btnMainDateTo.Location = new System.Drawing.Point(436, 3);
		this.btnMainDateTo.Margin = new System.Windows.Forms.Padding(0);
		this.btnMainDateTo.Name = "btnMainDateTo";
		this.btnMainDateTo.Size = new System.Drawing.Size(44, 25);
		this.btnMainDateTo.TabIndex = 7;
		this.btnMainDateTo.Text = "Now";
		this.toolTipMain.SetToolTip(this.btnMainDateTo, "Goto current date to");
		this.btnMainDateTo.UseVisualStyleBackColor = false;
		this.btnMainDateTo.Click += new System.EventHandler(btnMainDateTo_Click);
		this.btnLastData.BackColor = System.Drawing.SystemColors.Control;
		this.btnLastData.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnLastData.Location = new System.Drawing.Point(816, 3);
		this.btnLastData.Margin = new System.Windows.Forms.Padding(0);
		this.btnLastData.Name = "btnLastData";
		this.btnLastData.Size = new System.Drawing.Size(30, 25);
		this.btnLastData.TabIndex = 14;
		this.btnLastData.Text = ">>";
		this.toolTipMain.SetToolTip(this.btnLastData, "Goto last page");
		this.btnLastData.UseVisualStyleBackColor = false;
		this.btnLastData.Click += new System.EventHandler(btnLastData_Click);
		this.txtPage.CustomButton.Image = null;
		this.txtPage.CustomButton.Location = new System.Drawing.Point(13, 1);
		this.txtPage.CustomButton.Name = "";
		this.txtPage.CustomButton.Size = new System.Drawing.Size(21, 21);
		this.txtPage.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
		this.txtPage.CustomButton.TabIndex = 1;
		this.txtPage.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
		this.txtPage.CustomButton.UseSelectable = true;
		this.txtPage.CustomButton.Visible = false;
		this.txtPage.Lines = new string[1] { "1" };
		this.txtPage.Location = new System.Drawing.Point(691, 4);
		this.txtPage.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
		this.txtPage.MaxLength = 3;
		this.txtPage.Name = "txtPage";
		this.txtPage.PasswordChar = '\0';
		this.txtPage.ScrollBars = System.Windows.Forms.ScrollBars.None;
		this.txtPage.SelectedText = "";
		this.txtPage.SelectionLength = 0;
		this.txtPage.SelectionStart = 0;
		this.txtPage.ShortcutsEnabled = true;
		this.txtPage.Size = new System.Drawing.Size(35, 23);
		this.txtPage.TabIndex = 10;
		this.txtPage.Text = "1";
		this.txtPage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.toolTipMain.SetToolTip(this.txtPage, "Enter current page");
		this.txtPage.UseSelectable = true;
		this.txtPage.WaterMark = "Page";
		this.txtPage.WaterMarkColor = System.Drawing.Color.FromArgb(109, 109, 109);
		this.txtPage.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
		this.txtPage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtPage_KeyPress);
		this.txtPage.Leave += new System.EventHandler(txtPage_Leave);
		this.btnNextData.BackColor = System.Drawing.SystemColors.Control;
		this.btnNextData.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnNextData.Location = new System.Drawing.Point(786, 3);
		this.btnNextData.Margin = new System.Windows.Forms.Padding(0);
		this.btnNextData.Name = "btnNextData";
		this.btnNextData.Size = new System.Drawing.Size(30, 25);
		this.btnNextData.TabIndex = 13;
		this.btnNextData.Text = ">";
		this.toolTipMain.SetToolTip(this.btnNextData, "Goto next page");
		this.btnNextData.UseVisualStyleBackColor = false;
		this.btnNextData.Click += new System.EventHandler(btnNextData_Click);
		this.lblPage.AutoSize = true;
		this.lblPage.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblPage.Location = new System.Drawing.Point(645, 4);
		this.lblPage.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
		this.lblPage.Name = "lblPage";
		this.lblPage.Size = new System.Drawing.Size(43, 23);
		this.lblPage.TabIndex = 66;
		this.lblPage.Text = "Page:";
		this.lblPage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.btnPreData.BackColor = System.Drawing.SystemColors.Control;
		this.btnPreData.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnPreData.Location = new System.Drawing.Point(756, 3);
		this.btnPreData.Margin = new System.Windows.Forms.Padding(0);
		this.btnPreData.Name = "btnPreData";
		this.btnPreData.Size = new System.Drawing.Size(30, 25);
		this.btnPreData.TabIndex = 12;
		this.btnPreData.Text = "<";
		this.toolTipMain.SetToolTip(this.btnPreData, "Goto previous page");
		this.btnPreData.UseVisualStyleBackColor = false;
		this.btnPreData.Click += new System.EventHandler(btnPreData_Click);
		this.btnPreviousDateTo.BackColor = System.Drawing.SystemColors.Control;
		this.btnPreviousDateTo.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnPreviousDateTo.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.btnPreviousDateTo.Location = new System.Drawing.Point(412, 4);
		this.btnPreviousDateTo.Margin = new System.Windows.Forms.Padding(0);
		this.btnPreviousDateTo.Name = "btnPreviousDateTo";
		this.btnPreviousDateTo.Size = new System.Drawing.Size(24, 24);
		this.btnPreviousDateTo.TabIndex = 6;
		this.btnPreviousDateTo.Text = "<";
		this.toolTipMain.SetToolTip(this.btnPreviousDateTo, "Goto previous date to");
		this.btnPreviousDateTo.UseVisualStyleBackColor = false;
		this.btnPreviousDateTo.Click += new System.EventHandler(btnPreviousDateTo_Click);
		this.btnFirstData.BackColor = System.Drawing.SystemColors.Control;
		this.btnFirstData.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnFirstData.Location = new System.Drawing.Point(726, 3);
		this.btnFirstData.Margin = new System.Windows.Forms.Padding(0);
		this.btnFirstData.Name = "btnFirstData";
		this.btnFirstData.Size = new System.Drawing.Size(30, 25);
		this.btnFirstData.TabIndex = 11;
		this.btnFirstData.Text = "<<";
		this.toolTipMain.SetToolTip(this.btnFirstData, "Goto first page");
		this.btnFirstData.UseVisualStyleBackColor = false;
		this.btnFirstData.Click += new System.EventHandler(btnFirstData_Click);
		this.lblLimit.AutoSize = true;
		this.lblLimit.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblLimit.Location = new System.Drawing.Point(552, 4);
		this.lblLimit.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
		this.lblLimit.Name = "lblLimit";
		this.lblLimit.Size = new System.Drawing.Size(37, 23);
		this.lblLimit.TabIndex = 65;
		this.lblLimit.Text = "Limit:";
		this.lblLimit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.btnNextDateFrom.BackColor = System.Drawing.SystemColors.Control;
		this.btnNextDateFrom.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnNextDateFrom.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.btnNextDateFrom.Location = new System.Drawing.Point(115, 4);
		this.btnNextDateFrom.Margin = new System.Windows.Forms.Padding(0);
		this.btnNextDateFrom.Name = "btnNextDateFrom";
		this.btnNextDateFrom.Size = new System.Drawing.Size(24, 24);
		this.btnNextDateFrom.TabIndex = 3;
		this.btnNextDateFrom.Text = ">";
		this.toolTipMain.SetToolTip(this.btnNextDateFrom, "Goto next date from");
		this.btnNextDateFrom.UseVisualStyleBackColor = false;
		this.btnNextDateFrom.Click += new System.EventHandler(btnNextDateFrom_Click);
		this.btnMainDateFrom.AutoSize = true;
		this.btnMainDateFrom.BackColor = System.Drawing.SystemColors.Control;
		this.btnMainDateFrom.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnMainDateFrom.Dock = System.Windows.Forms.DockStyle.Top;
		this.btnMainDateFrom.Location = new System.Drawing.Point(71, 3);
		this.btnMainDateFrom.Margin = new System.Windows.Forms.Padding(0);
		this.btnMainDateFrom.Name = "btnMainDateFrom";
		this.btnMainDateFrom.Size = new System.Drawing.Size(44, 25);
		this.btnMainDateFrom.TabIndex = 2;
		this.btnMainDateFrom.Text = "Now";
		this.toolTipMain.SetToolTip(this.btnMainDateFrom, "Goto current date from");
		this.btnMainDateFrom.UseVisualStyleBackColor = false;
		this.btnMainDateFrom.Click += new System.EventHandler(btnMainDateFrom_Click);
		this.btnPreviousDateFrom.BackColor = System.Drawing.SystemColors.Control;
		this.btnPreviousDateFrom.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnPreviousDateFrom.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.btnPreviousDateFrom.Location = new System.Drawing.Point(47, 4);
		this.btnPreviousDateFrom.Margin = new System.Windows.Forms.Padding(0);
		this.btnPreviousDateFrom.Name = "btnPreviousDateFrom";
		this.btnPreviousDateFrom.Size = new System.Drawing.Size(24, 24);
		this.btnPreviousDateFrom.TabIndex = 1;
		this.btnPreviousDateFrom.Text = "<";
		this.toolTipMain.SetToolTip(this.btnPreviousDateFrom, "Goto previous date from");
		this.btnPreviousDateFrom.UseVisualStyleBackColor = false;
		this.btnPreviousDateFrom.Click += new System.EventHandler(btnPreviousDateFrom_Click);
		this.cbbLimit.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.cbbLimit.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cbbLimit.Dock = System.Windows.Forms.DockStyle.Fill;
		this.cbbLimit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cbbLimit.FormattingEnabled = true;
		this.cbbLimit.Items.AddRange(new object[19]
		{
			"50", "100", "150", "200", "250", "300", "350", "400", "450", "500",
			"550", "600", "650", "700", "750", "800", "850", "900", "950"
		});
		this.cbbLimit.Location = new System.Drawing.Point(592, 3);
		this.cbbLimit.Margin = new System.Windows.Forms.Padding(0);
		this.cbbLimit.MaxLength = 3;
		this.cbbLimit.Name = "cbbLimit";
		this.cbbLimit.Size = new System.Drawing.Size(50, 24);
		this.cbbLimit.TabIndex = 9;
		this.cbbLimit.Text = "50";
		this.toolTipMain.SetToolTip(this.cbbLimit, "Select or enter limit a page");
		this.cbbLimit.SelectedIndexChanged += new System.EventHandler(cbbLimit_SelectedIndexChanged);
		this.cbbLimit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(cbbLimit_KeyPress);
		this.cbbLimit.Leave += new System.EventHandler(cbbLimit_Leave);
		this.lblTo.AutoSize = true;
		this.lblTo.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTo.Location = new System.Drawing.Point(262, 4);
		this.lblTo.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
		this.lblTo.Name = "lblTo";
		this.lblTo.Size = new System.Drawing.Size(27, 23);
		this.lblTo.TabIndex = 57;
		this.lblTo.Text = "To:";
		this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblFrom.AutoSize = true;
		this.lblFrom.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblFrom.Location = new System.Drawing.Point(3, 4);
		this.lblFrom.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
		this.lblFrom.Name = "lblFrom";
		this.lblFrom.Size = new System.Drawing.Size(41, 23);
		this.lblFrom.TabIndex = 56;
		this.lblFrom.Text = "From:";
		this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.dtpDateFrom.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.dtpDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.dtpDateFrom.Location = new System.Drawing.Point(139, 6);
		this.dtpDateFrom.Margin = new System.Windows.Forms.Padding(0);
		this.dtpDateFrom.Name = "dtpDateFrom";
		this.dtpDateFrom.Size = new System.Drawing.Size(120, 22);
		this.dtpDateFrom.TabIndex = 4;
		this.toolTipMain.SetToolTip(this.dtpDateFrom, "Select or enter from date want search");
		this.dtpDateTo.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.dtpDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.dtpDateTo.Location = new System.Drawing.Point(292, 6);
		this.dtpDateTo.Margin = new System.Windows.Forms.Padding(0);
		this.dtpDateTo.Name = "dtpDateTo";
		this.dtpDateTo.Size = new System.Drawing.Size(120, 22);
		this.dtpDateTo.TabIndex = 5;
		this.toolTipMain.SetToolTip(this.dtpDateTo, "Select or enter to date want search");
		this.cbbProduct.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cbbProduct.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cbbProduct.Dock = System.Windows.Forms.DockStyle.Fill;
		this.cbbProduct.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cbbProduct.FormattingEnabled = true;
		this.cbbProduct.Location = new System.Drawing.Point(62, 2);
		this.cbbProduct.Margin = new System.Windows.Forms.Padding(1);
		this.cbbProduct.MaxLength = 250;
		this.cbbProduct.Name = "cbbProduct";
		this.cbbProduct.Size = new System.Drawing.Size(903, 24);
		this.cbbProduct.TabIndex = 1;
		this.toolTipMain.SetToolTip(this.cbbProduct, "Select or enter product");
		this.cbbProduct.Enter += new System.EventHandler(cbbNormal_Enter);
		this.cbbProduct.Leave += new System.EventHandler(cbbNormal_Leave);
		this.btnSearch.BackColor = System.Drawing.SystemColors.Control;
		this.btnSearch.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnSearch.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnSearch.FlatAppearance.BorderSize = 0;
		this.btnSearch.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.AppWorkspace;
		this.btnSearch.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLight;
		this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnSearch.Image = (System.Drawing.Image)resources.GetObject("btnSearch.Image");
		this.btnSearch.Location = new System.Drawing.Point(968, 1);
		this.btnSearch.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
		this.btnSearch.Name = "btnSearch";
		this.btnSearch.Size = new System.Drawing.Size(24, 26);
		this.btnSearch.TabIndex = 3;
		this.btnSearch.TabStop = false;
		this.toolTipMain.SetToolTip(this.btnSearch, "Search");
		this.btnSearch.UseVisualStyleBackColor = false;
		this.tableLayoutPanel1.AutoSize = true;
		this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		this.tableLayoutPanel1.ColumnCount = 3;
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel1.Controls.Add(this.btnSearch, 2, 0);
		this.tableLayoutPanel1.Controls.Add(this.cbbProduct, 1, 0);
		this.tableLayoutPanel1.Controls.Add(this.lblProduct, 0, 0);
		this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
		this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
		this.tableLayoutPanel1.Name = "tableLayoutPanel1";
		this.tableLayoutPanel1.RowCount = 1;
		this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel1.Size = new System.Drawing.Size(994, 28);
		this.tableLayoutPanel1.TabIndex = 1;
		this.tableLayoutPanel1.TabStop = true;
		this.lblProduct.AutoSize = true;
		this.lblProduct.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblProduct.Location = new System.Drawing.Point(4, 2);
		this.lblProduct.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
		this.lblProduct.Name = "lblProduct";
		this.lblProduct.Size = new System.Drawing.Size(53, 24);
		this.lblProduct.TabIndex = 4;
		this.lblProduct.Text = "Product";
		this.lblProduct.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoSize = true;
		base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		base.Controls.Add(this.tpanelMain);
		base.Controls.Add(this.tableLayoutPanel1);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Name = "mSearchExport";
		base.Padding = new System.Windows.Forms.Padding(3);
		base.Size = new System.Drawing.Size(1000, 63);
		this.tpanelMain.ResumeLayout(false);
		this.tpanelMain.PerformLayout();
		this.tableLayoutPanel1.ResumeLayout(false);
		this.tableLayoutPanel1.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
