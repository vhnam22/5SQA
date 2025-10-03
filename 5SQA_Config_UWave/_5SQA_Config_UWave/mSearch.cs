using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using _5SQA_Config_UWave.Properties;
using MetroFramework;
using MetroFramework.Controls;

namespace _5SQA_Config_UWave;

public class mSearch : UserControl
{
	private int totalPage;

	private string oldcbbLimit;

	private string oldtxtPage;

	private DataTable dataTable;

	private DataGridView dataGridView;

	private IContainer components = null;

	private TableLayoutPanel tableLayoutPanel1;

	private Panel panel3;

	private MetroTextBox.MetroTextButton btnSearchThreeDelete;

	private MetroTextBox txtSearchThree;

	private Panel panel2;

	private MetroTextBox.MetroTextButton btnSearchTwoDelete;

	private MetroTextBox txtSearchTwo;

	public MetroTextBox.MetroTextButton btnSearch;

	private Panel panel1;

	private MetroTextBox.MetroTextButton btnSearchOneDelete;

	private MetroTextBox txtSearchOne;

	private TableLayoutPanel tableLayoutPanel2;

	public MetroTextBox txtPage;

	private Button btnLastData;

	private Button btnNextData;

	public MetroLabel lblTotalRows;

	private MetroLabel metroLabel147;

	private Button btnPreData;

	private MetroLabel metroLabel2;

	private MetroLabel metroLabel1;

	public ComboBox cbbLimit;

	private Button btnFirstData;

	public mSearch()
	{
		InitializeComponent();
		totalPage = 1;
		oldcbbLimit = cbbLimit.Text;
		oldtxtPage = txtPage.Text;
	}

	public DataTable getDataTable()
	{
		return dataTable;
	}

	public void Init(DataTable dt, DataGridView dgv)
	{
		dataTable = dt;
		dataGridView = dgv;
		getSetting();
		load_AutoComplete();
	}

	private void getSetting()
	{
		cbbLimit.Text = Settings.Default.Limit.ToString();
	}

	private void setSetting()
	{
		Settings.Default.Limit = int.Parse(cbbLimit.Text);
		Settings.Default.Save();
	}

	private void load_AutoComplete()
	{
		if (dataTable == null || dataTable.Rows.Count <= 0)
		{
			return;
		}
		AutoCompleteStringCollection autoCompleteStringCollection = new AutoCompleteStringCollection();
		foreach (DataRow row in dataTable.Rows)
		{
			foreach (DataGridViewColumn column in dataGridView.Columns)
			{
				if (column.Visible && !column.Name.Equals("No") && !string.IsNullOrEmpty(row[column.Index].ToString()))
				{
					autoCompleteStringCollection.Add(row[column.DataPropertyName].ToString());
				}
			}
		}
		txtSearchOne.AutoCompleteCustomSource = autoCompleteStringCollection;
		txtSearchTwo.AutoCompleteCustomSource = autoCompleteStringCollection;
		txtSearchThree.AutoCompleteCustomSource = autoCompleteStringCollection;
	}

	public DataTable SearchInAllColums()
	{
		try
		{
			if (this.dataTable != null)
			{
				StringComparison comparison = StringComparison.OrdinalIgnoreCase;
				DataRow[] array = (from DataRow r in this.dataTable.Rows
					where r.ItemArray.Any((object c) => c.ToString().IndexOf(txtSearchOne.Text, comparison) >= 0) && r.ItemArray.Any((object c) => c.ToString().IndexOf(txtSearchTwo.Text, comparison) >= 0) && r.ItemArray.Any((object c) => c.ToString().IndexOf(txtSearchThree.Text, comparison) >= 0)
					select r).ToArray();
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

	private void txtSearchOne_KeyUp(object sender, KeyEventArgs e)
	{
		if (string.IsNullOrEmpty(txtSearchOne.Text))
		{
			btnSearchOneDelete.Visible = false;
		}
		else
		{
			btnSearchOneDelete.Visible = true;
		}
		btnSearch.PerformClick();
	}

	private void btnSearchOneDelete_Click(object sender, EventArgs e)
	{
		txtSearchOne.Clear();
		txtSearchOne_KeyUp(this, null);
		txtSearchOne.Focus();
	}

	private void txtSearchTwo_KeyUp(object sender, KeyEventArgs e)
	{
		if (string.IsNullOrEmpty(txtSearchTwo.Text))
		{
			btnSearchTwoDelete.Visible = false;
		}
		else
		{
			btnSearchTwoDelete.Visible = true;
		}
		btnSearch.PerformClick();
	}

	private void btnSearchTwoDelete_Click(object sender, EventArgs e)
	{
		txtSearchTwo.Clear();
		txtSearchTwo_KeyUp(this, null);
		txtSearchTwo.Focus();
	}

	private void txtSearchThree_KeyUp(object sender, KeyEventArgs e)
	{
		if (string.IsNullOrEmpty(txtSearchThree.Text))
		{
			btnSearchThreeDelete.Visible = false;
		}
		else
		{
			btnSearchThreeDelete.Visible = true;
		}
		btnSearch.PerformClick();
	}

	private void btnSearchThreeDelete_Click(object sender, EventArgs e)
	{
		txtSearchThree.Clear();
		txtSearchThree_KeyUp(this, null);
		txtSearchThree.Focus();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_5SQA_Config_UWave.mSearch));
		this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.btnSearchThreeDelete = new MetroFramework.Controls.MetroTextBox.MetroTextButton();
		this.txtSearchThree = new MetroFramework.Controls.MetroTextBox();
		this.panel2 = new System.Windows.Forms.Panel();
		this.btnSearchTwoDelete = new MetroFramework.Controls.MetroTextBox.MetroTextButton();
		this.txtSearchTwo = new MetroFramework.Controls.MetroTextBox();
		this.btnSearch = new MetroFramework.Controls.MetroTextBox.MetroTextButton();
		this.panel1 = new System.Windows.Forms.Panel();
		this.btnSearchOneDelete = new MetroFramework.Controls.MetroTextBox.MetroTextButton();
		this.txtSearchOne = new MetroFramework.Controls.MetroTextBox();
		this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
		this.txtPage = new MetroFramework.Controls.MetroTextBox();
		this.btnLastData = new System.Windows.Forms.Button();
		this.btnNextData = new System.Windows.Forms.Button();
		this.lblTotalRows = new MetroFramework.Controls.MetroLabel();
		this.metroLabel147 = new MetroFramework.Controls.MetroLabel();
		this.btnPreData = new System.Windows.Forms.Button();
		this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
		this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
		this.cbbLimit = new System.Windows.Forms.ComboBox();
		this.btnFirstData = new System.Windows.Forms.Button();
		this.tableLayoutPanel1.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel1.SuspendLayout();
		this.tableLayoutPanel2.SuspendLayout();
		base.SuspendLayout();
		this.tableLayoutPanel1.AutoSize = true;
		this.tableLayoutPanel1.ColumnCount = 4;
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33332f));
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334f));
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334f));
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 0);
		this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 0);
		this.tableLayoutPanel1.Controls.Add(this.btnSearch, 3, 0);
		this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
		this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.tableLayoutPanel1.Location = new System.Drawing.Point(4, 4);
		this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
		this.tableLayoutPanel1.Name = "tableLayoutPanel1";
		this.tableLayoutPanel1.RowCount = 1;
		this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25f));
		this.tableLayoutPanel1.Size = new System.Drawing.Size(847, 25);
		this.tableLayoutPanel1.TabIndex = 1;
		this.panel3.Controls.Add(this.btnSearchThreeDelete);
		this.panel3.Controls.Add(this.txtSearchThree);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(543, 0);
		this.panel3.Margin = new System.Windows.Forms.Padding(0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(272, 25);
		this.panel3.TabIndex = 69;
		this.btnSearchThreeDelete.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnSearchThreeDelete.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnSearchThreeDelete.FlatAppearance.BorderSize = 0;
		this.btnSearchThreeDelete.Image = (System.Drawing.Image)resources.GetObject("btnSearchThreeDelete.Image");
		this.btnSearchThreeDelete.Location = new System.Drawing.Point(248, 2);
		this.btnSearchThreeDelete.Margin = new System.Windows.Forms.Padding(4);
		this.btnSearchThreeDelete.Name = "btnSearchThreeDelete";
		this.btnSearchThreeDelete.Size = new System.Drawing.Size(21, 21);
		this.btnSearchThreeDelete.Style = MetroFramework.MetroColorStyle.Orange;
		this.btnSearchThreeDelete.TabIndex = 61;
		this.btnSearchThreeDelete.TabStop = false;
		this.btnSearchThreeDelete.UseSelectable = true;
		this.btnSearchThreeDelete.UseVisualStyleBackColor = true;
		this.btnSearchThreeDelete.Visible = false;
		this.btnSearchThreeDelete.Click += new System.EventHandler(btnSearchThreeDelete_Click);
		this.txtSearchThree.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtSearchThree.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtSearchThree.CustomButton.Image = null;
		this.txtSearchThree.CustomButton.Location = new System.Drawing.Point(248, 1);
		this.txtSearchThree.CustomButton.Name = "";
		this.txtSearchThree.CustomButton.Size = new System.Drawing.Size(23, 23);
		this.txtSearchThree.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
		this.txtSearchThree.CustomButton.TabIndex = 1;
		this.txtSearchThree.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
		this.txtSearchThree.CustomButton.UseSelectable = true;
		this.txtSearchThree.CustomButton.Visible = false;
		this.txtSearchThree.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtSearchThree.Lines = new string[0];
		this.txtSearchThree.Location = new System.Drawing.Point(0, 0);
		this.txtSearchThree.MaxLength = 32767;
		this.txtSearchThree.Name = "txtSearchThree";
		this.txtSearchThree.PasswordChar = '\0';
		this.txtSearchThree.PromptText = "Enter a search term three";
		this.txtSearchThree.ScrollBars = System.Windows.Forms.ScrollBars.None;
		this.txtSearchThree.SelectedText = "";
		this.txtSearchThree.SelectionLength = 0;
		this.txtSearchThree.SelectionStart = 0;
		this.txtSearchThree.ShortcutsEnabled = true;
		this.txtSearchThree.Size = new System.Drawing.Size(272, 25);
		this.txtSearchThree.Style = MetroFramework.MetroColorStyle.Orange;
		this.txtSearchThree.TabIndex = 3;
		this.txtSearchThree.UseSelectable = true;
		this.txtSearchThree.WaterMark = "Enter a search term three";
		this.txtSearchThree.WaterMarkColor = System.Drawing.Color.FromArgb(109, 109, 109);
		this.txtSearchThree.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
		this.txtSearchThree.KeyUp += new System.Windows.Forms.KeyEventHandler(txtSearchThree_KeyUp);
		this.panel2.Controls.Add(this.btnSearchTwoDelete);
		this.panel2.Controls.Add(this.txtSearchTwo);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(271, 0);
		this.panel2.Margin = new System.Windows.Forms.Padding(0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(272, 25);
		this.panel2.TabIndex = 68;
		this.btnSearchTwoDelete.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnSearchTwoDelete.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnSearchTwoDelete.FlatAppearance.BorderSize = 0;
		this.btnSearchTwoDelete.Image = (System.Drawing.Image)resources.GetObject("btnSearchTwoDelete.Image");
		this.btnSearchTwoDelete.Location = new System.Drawing.Point(248, 2);
		this.btnSearchTwoDelete.Margin = new System.Windows.Forms.Padding(4);
		this.btnSearchTwoDelete.Name = "btnSearchTwoDelete";
		this.btnSearchTwoDelete.Size = new System.Drawing.Size(21, 21);
		this.btnSearchTwoDelete.Style = MetroFramework.MetroColorStyle.Orange;
		this.btnSearchTwoDelete.TabIndex = 61;
		this.btnSearchTwoDelete.TabStop = false;
		this.btnSearchTwoDelete.UseSelectable = true;
		this.btnSearchTwoDelete.UseVisualStyleBackColor = true;
		this.btnSearchTwoDelete.Visible = false;
		this.btnSearchTwoDelete.Click += new System.EventHandler(btnSearchTwoDelete_Click);
		this.txtSearchTwo.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtSearchTwo.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtSearchTwo.CustomButton.Image = null;
		this.txtSearchTwo.CustomButton.Location = new System.Drawing.Point(248, 1);
		this.txtSearchTwo.CustomButton.Name = "";
		this.txtSearchTwo.CustomButton.Size = new System.Drawing.Size(23, 23);
		this.txtSearchTwo.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
		this.txtSearchTwo.CustomButton.TabIndex = 1;
		this.txtSearchTwo.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
		this.txtSearchTwo.CustomButton.UseSelectable = true;
		this.txtSearchTwo.CustomButton.Visible = false;
		this.txtSearchTwo.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtSearchTwo.Lines = new string[0];
		this.txtSearchTwo.Location = new System.Drawing.Point(0, 0);
		this.txtSearchTwo.MaxLength = 32767;
		this.txtSearchTwo.Name = "txtSearchTwo";
		this.txtSearchTwo.PasswordChar = '\0';
		this.txtSearchTwo.PromptText = "Enter a search term two";
		this.txtSearchTwo.ScrollBars = System.Windows.Forms.ScrollBars.None;
		this.txtSearchTwo.SelectedText = "";
		this.txtSearchTwo.SelectionLength = 0;
		this.txtSearchTwo.SelectionStart = 0;
		this.txtSearchTwo.ShortcutsEnabled = true;
		this.txtSearchTwo.Size = new System.Drawing.Size(272, 25);
		this.txtSearchTwo.Style = MetroFramework.MetroColorStyle.Orange;
		this.txtSearchTwo.TabIndex = 2;
		this.txtSearchTwo.UseSelectable = true;
		this.txtSearchTwo.WaterMark = "Enter a search term two";
		this.txtSearchTwo.WaterMarkColor = System.Drawing.Color.FromArgb(109, 109, 109);
		this.txtSearchTwo.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
		this.txtSearchTwo.KeyUp += new System.Windows.Forms.KeyEventHandler(txtSearchTwo_KeyUp);
		this.btnSearch.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnSearch.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnSearch.FlatAppearance.BorderSize = 0;
		this.btnSearch.Image = (System.Drawing.Image)resources.GetObject("btnSearch.Image");
		this.btnSearch.Location = new System.Drawing.Point(815, 0);
		this.btnSearch.Margin = new System.Windows.Forms.Padding(0);
		this.btnSearch.Name = "btnSearch";
		this.btnSearch.Size = new System.Drawing.Size(32, 25);
		this.btnSearch.Style = MetroFramework.MetroColorStyle.Orange;
		this.btnSearch.TabIndex = 4;
		this.btnSearch.UseSelectable = true;
		this.btnSearch.UseVisualStyleBackColor = true;
		this.panel1.Controls.Add(this.btnSearchOneDelete);
		this.panel1.Controls.Add(this.txtSearchOne);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(271, 25);
		this.panel1.TabIndex = 64;
		this.btnSearchOneDelete.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnSearchOneDelete.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnSearchOneDelete.FlatAppearance.BorderSize = 0;
		this.btnSearchOneDelete.Image = (System.Drawing.Image)resources.GetObject("btnSearchOneDelete.Image");
		this.btnSearchOneDelete.Location = new System.Drawing.Point(248, 2);
		this.btnSearchOneDelete.Margin = new System.Windows.Forms.Padding(0);
		this.btnSearchOneDelete.Name = "btnSearchOneDelete";
		this.btnSearchOneDelete.Size = new System.Drawing.Size(21, 21);
		this.btnSearchOneDelete.Style = MetroFramework.MetroColorStyle.Orange;
		this.btnSearchOneDelete.TabIndex = 61;
		this.btnSearchOneDelete.TabStop = false;
		this.btnSearchOneDelete.UseSelectable = true;
		this.btnSearchOneDelete.UseVisualStyleBackColor = true;
		this.btnSearchOneDelete.Visible = false;
		this.btnSearchOneDelete.Click += new System.EventHandler(btnSearchOneDelete_Click);
		this.txtSearchOne.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtSearchOne.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtSearchOne.CustomButton.Image = null;
		this.txtSearchOne.CustomButton.Location = new System.Drawing.Point(247, 1);
		this.txtSearchOne.CustomButton.Name = "";
		this.txtSearchOne.CustomButton.Size = new System.Drawing.Size(23, 23);
		this.txtSearchOne.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
		this.txtSearchOne.CustomButton.TabIndex = 1;
		this.txtSearchOne.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
		this.txtSearchOne.CustomButton.UseSelectable = true;
		this.txtSearchOne.CustomButton.Visible = false;
		this.txtSearchOne.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtSearchOne.Lines = new string[0];
		this.txtSearchOne.Location = new System.Drawing.Point(0, 0);
		this.txtSearchOne.MaxLength = 32767;
		this.txtSearchOne.Name = "txtSearchOne";
		this.txtSearchOne.PasswordChar = '\0';
		this.txtSearchOne.PromptText = "Enter a search term one";
		this.txtSearchOne.ScrollBars = System.Windows.Forms.ScrollBars.None;
		this.txtSearchOne.SelectedText = "";
		this.txtSearchOne.SelectionLength = 0;
		this.txtSearchOne.SelectionStart = 0;
		this.txtSearchOne.ShortcutsEnabled = true;
		this.txtSearchOne.Size = new System.Drawing.Size(271, 25);
		this.txtSearchOne.Style = MetroFramework.MetroColorStyle.Orange;
		this.txtSearchOne.TabIndex = 1;
		this.txtSearchOne.UseSelectable = true;
		this.txtSearchOne.WaterMark = "Enter a search term one";
		this.txtSearchOne.WaterMarkColor = System.Drawing.Color.FromArgb(109, 109, 109);
		this.txtSearchOne.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
		this.txtSearchOne.KeyUp += new System.Windows.Forms.KeyEventHandler(txtSearchOne_KeyUp);
		this.tableLayoutPanel2.AutoSize = true;
		this.tableLayoutPanel2.ColumnCount = 11;
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 55f));
		this.tableLayoutPanel2.Controls.Add(this.txtPage, 4, 0);
		this.tableLayoutPanel2.Controls.Add(this.btnLastData, 8, 0);
		this.tableLayoutPanel2.Controls.Add(this.btnNextData, 7, 0);
		this.tableLayoutPanel2.Controls.Add(this.lblTotalRows, 10, 0);
		this.tableLayoutPanel2.Controls.Add(this.metroLabel147, 9, 0);
		this.tableLayoutPanel2.Controls.Add(this.btnPreData, 6, 0);
		this.tableLayoutPanel2.Controls.Add(this.metroLabel2, 3, 0);
		this.tableLayoutPanel2.Controls.Add(this.metroLabel1, 1, 0);
		this.tableLayoutPanel2.Controls.Add(this.cbbLimit, 2, 0);
		this.tableLayoutPanel2.Controls.Add(this.btnFirstData, 5, 0);
		this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.tableLayoutPanel2.Location = new System.Drawing.Point(4, 31);
		this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
		this.tableLayoutPanel2.Name = "tableLayoutPanel2";
		this.tableLayoutPanel2.RowCount = 1;
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel2.Size = new System.Drawing.Size(847, 25);
		this.tableLayoutPanel2.TabIndex = 2;
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
		this.txtPage.Location = new System.Drawing.Point(565, 1);
		this.txtPage.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
		this.txtPage.MaxLength = 3;
		this.txtPage.Name = "txtPage";
		this.txtPage.PasswordChar = '\0';
		this.txtPage.PromptText = "Page";
		this.txtPage.ScrollBars = System.Windows.Forms.ScrollBars.None;
		this.txtPage.SelectedText = "";
		this.txtPage.SelectionLength = 0;
		this.txtPage.SelectionStart = 0;
		this.txtPage.ShortcutsEnabled = true;
		this.txtPage.Size = new System.Drawing.Size(35, 23);
		this.txtPage.Style = MetroFramework.MetroColorStyle.Orange;
		this.txtPage.TabIndex = 2;
		this.txtPage.Text = "1";
		this.txtPage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.txtPage.UseSelectable = true;
		this.txtPage.WaterMark = "Page";
		this.txtPage.WaterMarkColor = System.Drawing.Color.FromArgb(109, 109, 109);
		this.txtPage.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
		this.txtPage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtPage_KeyPress);
		this.txtPage.Leave += new System.EventHandler(txtPage_Leave);
		this.btnLastData.BackColor = System.Drawing.SystemColors.Control;
		this.btnLastData.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnLastData.Location = new System.Drawing.Point(690, 0);
		this.btnLastData.Margin = new System.Windows.Forms.Padding(0);
		this.btnLastData.Name = "btnLastData";
		this.btnLastData.Size = new System.Drawing.Size(30, 25);
		this.btnLastData.TabIndex = 6;
		this.btnLastData.Text = ">>";
		this.btnLastData.UseVisualStyleBackColor = false;
		this.btnLastData.Click += new System.EventHandler(btnLastData_Click);
		this.btnNextData.BackColor = System.Drawing.SystemColors.Control;
		this.btnNextData.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnNextData.Location = new System.Drawing.Point(660, 0);
		this.btnNextData.Margin = new System.Windows.Forms.Padding(0);
		this.btnNextData.Name = "btnNextData";
		this.btnNextData.Size = new System.Drawing.Size(30, 25);
		this.btnNextData.TabIndex = 5;
		this.btnNextData.Text = ">";
		this.btnNextData.UseVisualStyleBackColor = false;
		this.btnNextData.Click += new System.EventHandler(btnNextData_Click);
		this.lblTotalRows.AutoSize = true;
		this.lblTotalRows.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTotalRows.ForeColor = System.Drawing.Color.Red;
		this.lblTotalRows.Location = new System.Drawing.Point(793, 1);
		this.lblTotalRows.Margin = new System.Windows.Forms.Padding(1);
		this.lblTotalRows.Name = "lblTotalRows";
		this.lblTotalRows.Size = new System.Drawing.Size(53, 23);
		this.lblTotalRows.TabIndex = 42;
		this.lblTotalRows.Text = "0";
		this.lblTotalRows.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblTotalRows.UseCustomBackColor = true;
		this.lblTotalRows.UseCustomForeColor = true;
		this.lblTotalRows.TextChanged += new System.EventHandler(lblTotalRows_TextChanged);
		this.metroLabel147.AutoSize = true;
		this.metroLabel147.Dock = System.Windows.Forms.DockStyle.Fill;
		this.metroLabel147.Location = new System.Drawing.Point(721, 1);
		this.metroLabel147.Margin = new System.Windows.Forms.Padding(1);
		this.metroLabel147.Name = "metroLabel147";
		this.metroLabel147.Size = new System.Drawing.Size(70, 23);
		this.metroLabel147.TabIndex = 43;
		this.metroLabel147.Text = "Total rows:";
		this.metroLabel147.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.metroLabel147.UseCustomBackColor = true;
		this.btnPreData.BackColor = System.Drawing.SystemColors.Control;
		this.btnPreData.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnPreData.Location = new System.Drawing.Point(630, 0);
		this.btnPreData.Margin = new System.Windows.Forms.Padding(0);
		this.btnPreData.Name = "btnPreData";
		this.btnPreData.Size = new System.Drawing.Size(30, 25);
		this.btnPreData.TabIndex = 4;
		this.btnPreData.Text = "<";
		this.btnPreData.UseVisualStyleBackColor = false;
		this.btnPreData.Click += new System.EventHandler(btnPreData_Click);
		this.metroLabel2.AutoSize = true;
		this.metroLabel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.metroLabel2.Location = new System.Drawing.Point(523, 1);
		this.metroLabel2.Margin = new System.Windows.Forms.Padding(1);
		this.metroLabel2.Name = "metroLabel2";
		this.metroLabel2.Size = new System.Drawing.Size(41, 23);
		this.metroLabel2.TabIndex = 64;
		this.metroLabel2.Text = "Page:";
		this.metroLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.metroLabel2.UseCustomBackColor = true;
		this.metroLabel1.AutoSize = true;
		this.metroLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.metroLabel1.Location = new System.Drawing.Point(431, 1);
		this.metroLabel1.Margin = new System.Windows.Forms.Padding(1);
		this.metroLabel1.Name = "metroLabel1";
		this.metroLabel1.Size = new System.Drawing.Size(40, 23);
		this.metroLabel1.TabIndex = 43;
		this.metroLabel1.Text = "Limit:";
		this.metroLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.metroLabel1.UseCustomBackColor = true;
		this.cbbLimit.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cbbLimit.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cbbLimit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cbbLimit.FormattingEnabled = true;
		this.cbbLimit.Items.AddRange(new object[19]
		{
			"50", "100", "150", "200", "250", "300", "350", "400", "450", "500",
			"550", "600", "650", "700", "750", "800", "850", "900", "950"
		});
		this.cbbLimit.Location = new System.Drawing.Point(472, 1);
		this.cbbLimit.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
		this.cbbLimit.MaxLength = 3;
		this.cbbLimit.Name = "cbbLimit";
		this.cbbLimit.Size = new System.Drawing.Size(50, 23);
		this.cbbLimit.TabIndex = 1;
		this.cbbLimit.Text = "50";
		this.cbbLimit.SelectedIndexChanged += new System.EventHandler(cbbLimit_SelectedIndexChanged);
		this.cbbLimit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(cbbLimit_KeyPress);
		this.cbbLimit.Leave += new System.EventHandler(cbbLimit_Leave);
		this.btnFirstData.BackColor = System.Drawing.SystemColors.Control;
		this.btnFirstData.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnFirstData.Location = new System.Drawing.Point(600, 0);
		this.btnFirstData.Margin = new System.Windows.Forms.Padding(0);
		this.btnFirstData.Name = "btnFirstData";
		this.btnFirstData.Size = new System.Drawing.Size(30, 25);
		this.btnFirstData.TabIndex = 3;
		this.btnFirstData.Text = "<<";
		this.btnFirstData.UseVisualStyleBackColor = false;
		this.btnFirstData.Click += new System.EventHandler(btnFirstData_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		base.Controls.Add(this.tableLayoutPanel2);
		base.Controls.Add(this.tableLayoutPanel1);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "mSearch";
		base.Padding = new System.Windows.Forms.Padding(4);
		base.Size = new System.Drawing.Size(855, 60);
		this.tableLayoutPanel1.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		this.tableLayoutPanel2.ResumeLayout(false);
		this.tableLayoutPanel2.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
