using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using _5S_QA_Client.Controls;
using _5S_QA_Client.Properties;
using MetroFramework;
using MetroFramework.Controls;

namespace _5S_QA_Client.View.User_control;

public class mSearch : UserControl
{
	private int totalPage;

	private string oldcbbLimit;

	private string oldtxtPage;

	public string oldcbbSample = "1";

	private DataGridView dataGridView;

	public bool isLoadFirst = true;

	private Form mForm;

	private IContainer components = null;

	private ToolTip toolTipMain;

	private TableLayoutPanel tableLayoutPanel1;

	private Panel panel3;

	private Panel panel2;

	private Panel panel1;

	private TableLayoutPanel tableLayoutPanel2;

	public MetroTextBox txtPage;

	private Button btnFirstData;

	private Button btnLastData;

	private Button btnNextData;

	private Button btnPreData;

	public ComboBox cbbLimit;

	private MetroTextBox txtSearchOne;

	private MetroTextBox txtSearchTwo;

	private MetroTextBox txtSearchThree;

	private MetroRadioButton rbAll;

	public DateTimePicker dtpMain;

	private TableLayoutPanel tpanelMain;

	public Button btnBarcode;

	private Panel panel4;

	private Button btnPrevious;

	private Button btnNext;

	private Button btnMain;

	public Button btnAdd;

	public Button btnSearch;

	public Button btnSearchOneDelete;

	public Button btnSearchTwoDelete;

	public Button btnSearchThreeDelete;

	private Label lblLimit;

	private Label lblPage;

	private Label lblTotalRow;

	private Label lblTotalRows;

	public CheckBox cbRejected;

	public DataTable dataTable { get; set; }

	public mSearch()
	{
		InitializeComponent();
		Update_Language();
		totalPage = 1;
		oldcbbLimit = cbbLimit.Text;
		oldtxtPage = txtPage.Text;
	}

	public void Update_Language()
	{
		Common.setControls(this, toolTipMain);
	}

	public void Init(DataTable dt, DataGridView dgv)
	{
		mForm = base.ParentForm;
		getSetting();
		if (!mForm.GetType().Equals(typeof(frmLogin)))
		{
			tpanelMain.Visible = false;
		}
		dataTable = dt;
		dataGridView = dgv;
		load_AutoComplete();
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

	private void getSetting()
	{
		Type type = base.ParentForm.GetType();
		if (type.Equals(typeof(frmLogin)))
		{
			cbbLimit.Text = Settings.Default.limitLogin.ToString();
		}
	}

	private void setSetting()
	{
		Type type = base.ParentForm.GetType();
		if (type.Equals(typeof(frmLogin)))
		{
			Settings.Default.limitLogin = int.Parse(cbbLimit.Text);
		}
		Settings.Default.Save();
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

	private void rbAll_CheckedChanged(object sender, EventArgs e)
	{
		load_AutoComplete();
	}

	private void btnMain_Click(object sender, EventArgs e)
	{
		dtpMain.Value = DateTime.Now;
	}

	private void btnPrevious_Click(object sender, EventArgs e)
	{
		if (cbRejected.Checked)
		{
			dtpMain.Value = dtpMain.Value.AddMonths(-1);
		}
		else
		{
			dtpMain.Value = dtpMain.Value.AddDays(-1.0);
		}
	}

	private void btnNext_Click(object sender, EventArgs e)
	{
		if (cbRejected.Checked)
		{
			dtpMain.Value = dtpMain.Value.AddMonths(1);
		}
		else
		{
			dtpMain.Value = dtpMain.Value.AddDays(1.0);
		}
	}

	private void cbRejected_CheckedChanged(object sender, EventArgs e)
	{
		if (cbRejected.Checked)
		{
			dtpMain.CustomFormat = "MM/yyyy";
			dtpMain.Format = DateTimePickerFormat.Custom;
		}
		else
		{
			dtpMain.Format = DateTimePickerFormat.Short;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_5S_QA_Client.View.User_control.mSearch));
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.txtSearchOne = new MetroFramework.Controls.MetroTextBox();
		this.txtSearchTwo = new MetroFramework.Controls.MetroTextBox();
		this.txtSearchThree = new MetroFramework.Controls.MetroTextBox();
		this.txtPage = new MetroFramework.Controls.MetroTextBox();
		this.rbAll = new MetroFramework.Controls.MetroRadioButton();
		this.cbbLimit = new System.Windows.Forms.ComboBox();
		this.btnLastData = new System.Windows.Forms.Button();
		this.btnNextData = new System.Windows.Forms.Button();
		this.btnPreData = new System.Windows.Forms.Button();
		this.btnFirstData = new System.Windows.Forms.Button();
		this.dtpMain = new System.Windows.Forms.DateTimePicker();
		this.btnPrevious = new System.Windows.Forms.Button();
		this.btnNext = new System.Windows.Forms.Button();
		this.btnMain = new System.Windows.Forms.Button();
		this.btnAdd = new System.Windows.Forms.Button();
		this.btnBarcode = new System.Windows.Forms.Button();
		this.btnSearch = new System.Windows.Forms.Button();
		this.btnSearchThreeDelete = new System.Windows.Forms.Button();
		this.btnSearchTwoDelete = new System.Windows.Forms.Button();
		this.btnSearchOneDelete = new System.Windows.Forms.Button();
		this.cbRejected = new System.Windows.Forms.CheckBox();
		this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
		this.lblTotalRows = new System.Windows.Forms.Label();
		this.lblTotalRow = new System.Windows.Forms.Label();
		this.lblPage = new System.Windows.Forms.Label();
		this.lblLimit = new System.Windows.Forms.Label();
		this.tpanelMain = new System.Windows.Forms.TableLayoutPanel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.tableLayoutPanel1.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel1.SuspendLayout();
		this.tableLayoutPanel2.SuspendLayout();
		this.tpanelMain.SuspendLayout();
		this.panel4.SuspendLayout();
		base.SuspendLayout();
		this.txtSearchOne.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtSearchOne.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtSearchOne.CustomButton.Image = null;
		this.txtSearchOne.CustomButton.Location = new System.Drawing.Point(307, 1);
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
		this.txtSearchOne.ScrollBars = System.Windows.Forms.ScrollBars.None;
		this.txtSearchOne.SelectedText = "";
		this.txtSearchOne.SelectionLength = 0;
		this.txtSearchOne.SelectionStart = 0;
		this.txtSearchOne.ShortcutsEnabled = true;
		this.txtSearchOne.Size = new System.Drawing.Size(331, 25);
		this.txtSearchOne.TabIndex = 1;
		this.toolTipMain.SetToolTip(this.txtSearchOne, "Enter a search term one");
		this.txtSearchOne.UseSelectable = true;
		this.txtSearchOne.WaterMark = "Enter a search term one";
		this.txtSearchOne.WaterMarkColor = System.Drawing.Color.Silver;
		this.txtSearchOne.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSearchOne.KeyUp += new System.Windows.Forms.KeyEventHandler(txtSearchOne_KeyUp);
		this.txtSearchTwo.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtSearchTwo.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtSearchTwo.CustomButton.Image = null;
		this.txtSearchTwo.CustomButton.Location = new System.Drawing.Point(307, 1);
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
		this.txtSearchTwo.ScrollBars = System.Windows.Forms.ScrollBars.None;
		this.txtSearchTwo.SelectedText = "";
		this.txtSearchTwo.SelectionLength = 0;
		this.txtSearchTwo.SelectionStart = 0;
		this.txtSearchTwo.ShortcutsEnabled = true;
		this.txtSearchTwo.Size = new System.Drawing.Size(331, 25);
		this.txtSearchTwo.TabIndex = 1;
		this.toolTipMain.SetToolTip(this.txtSearchTwo, "Enter a search term two");
		this.txtSearchTwo.UseSelectable = true;
		this.txtSearchTwo.WaterMark = "Enter a search term two";
		this.txtSearchTwo.WaterMarkColor = System.Drawing.Color.Silver;
		this.txtSearchTwo.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSearchTwo.KeyUp += new System.Windows.Forms.KeyEventHandler(txtSearchTwo_KeyUp);
		this.txtSearchThree.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtSearchThree.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtSearchThree.CustomButton.Image = null;
		this.txtSearchThree.CustomButton.Location = new System.Drawing.Point(307, 1);
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
		this.txtSearchThree.ScrollBars = System.Windows.Forms.ScrollBars.None;
		this.txtSearchThree.SelectedText = "";
		this.txtSearchThree.SelectionLength = 0;
		this.txtSearchThree.SelectionStart = 0;
		this.txtSearchThree.ShortcutsEnabled = true;
		this.txtSearchThree.Size = new System.Drawing.Size(331, 25);
		this.txtSearchThree.TabIndex = 1;
		this.toolTipMain.SetToolTip(this.txtSearchThree, "Enter a search term three");
		this.txtSearchThree.UseSelectable = true;
		this.txtSearchThree.WaterMark = "Enter a search term three";
		this.txtSearchThree.WaterMarkColor = System.Drawing.Color.Silver;
		this.txtSearchThree.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSearchThree.KeyUp += new System.Windows.Forms.KeyEventHandler(txtSearchThree_KeyUp);
		this.txtPage.CustomButton.Image = null;
		this.txtPage.CustomButton.Location = new System.Drawing.Point(13, 1);
		this.txtPage.CustomButton.Name = "";
		this.txtPage.CustomButton.Size = new System.Drawing.Size(21, 21);
		this.txtPage.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
		this.txtPage.CustomButton.TabIndex = 1;
		this.txtPage.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
		this.txtPage.CustomButton.UseSelectable = true;
		this.txtPage.CustomButton.Visible = false;
		this.txtPage.Dock = System.Windows.Forms.DockStyle.Top;
		this.txtPage.Lines = new string[1] { "1" };
		this.txtPage.Location = new System.Drawing.Point(142, 4);
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
		this.txtPage.TabIndex = 2;
		this.txtPage.Text = "1";
		this.txtPage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.toolTipMain.SetToolTip(this.txtPage, "Enter current page");
		this.txtPage.UseSelectable = true;
		this.txtPage.WaterMark = "Page";
		this.txtPage.WaterMarkColor = System.Drawing.Color.FromArgb(109, 109, 109);
		this.txtPage.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
		this.txtPage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtPage_KeyPress);
		this.txtPage.Leave += new System.EventHandler(txtPage_Leave);
		this.rbAll.AutoSize = true;
		this.rbAll.Checked = true;
		this.rbAll.Cursor = System.Windows.Forms.Cursors.Hand;
		this.rbAll.Dock = System.Windows.Forms.DockStyle.Fill;
		this.rbAll.Location = new System.Drawing.Point(3, 2);
		this.rbAll.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.rbAll.Name = "rbAll";
		this.rbAll.Size = new System.Drawing.Size(37, 26);
		this.rbAll.TabIndex = 1;
		this.rbAll.TabStop = true;
		this.rbAll.Text = "All";
		this.toolTipMain.SetToolTip(this.rbAll, "Select search for all condition");
		this.rbAll.UseCustomBackColor = true;
		this.rbAll.UseSelectable = true;
		this.rbAll.CheckedChanged += new System.EventHandler(rbAll_CheckedChanged);
		this.cbbLimit.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cbbLimit.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cbbLimit.Dock = System.Windows.Forms.DockStyle.Top;
		this.cbbLimit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cbbLimit.FormattingEnabled = true;
		this.cbbLimit.Items.AddRange(new object[19]
		{
			"50", "100", "150", "200", "250", "300", "350", "400", "450", "500",
			"550", "600", "650", "700", "750", "800", "850", "900", "950"
		});
		this.cbbLimit.Location = new System.Drawing.Point(43, 4);
		this.cbbLimit.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
		this.cbbLimit.MaxLength = 3;
		this.cbbLimit.Name = "cbbLimit";
		this.cbbLimit.Size = new System.Drawing.Size(50, 23);
		this.cbbLimit.TabIndex = 1;
		this.cbbLimit.Text = "50";
		this.toolTipMain.SetToolTip(this.cbbLimit, "Select or enter limit a page");
		this.cbbLimit.SelectedIndexChanged += new System.EventHandler(cbbLimit_SelectedIndexChanged);
		this.cbbLimit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(cbbLimit_KeyPress);
		this.cbbLimit.Leave += new System.EventHandler(cbbLimit_Leave);
		this.btnLastData.BackColor = System.Drawing.SystemColors.Control;
		this.btnLastData.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnLastData.Dock = System.Windows.Forms.DockStyle.Top;
		this.btnLastData.Location = new System.Drawing.Point(267, 3);
		this.btnLastData.Margin = new System.Windows.Forms.Padding(0);
		this.btnLastData.Name = "btnLastData";
		this.btnLastData.Size = new System.Drawing.Size(30, 25);
		this.btnLastData.TabIndex = 6;
		this.btnLastData.Text = ">>";
		this.toolTipMain.SetToolTip(this.btnLastData, "Goto last page");
		this.btnLastData.UseVisualStyleBackColor = false;
		this.btnLastData.Click += new System.EventHandler(btnLastData_Click);
		this.btnNextData.BackColor = System.Drawing.SystemColors.Control;
		this.btnNextData.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnNextData.Dock = System.Windows.Forms.DockStyle.Top;
		this.btnNextData.Location = new System.Drawing.Point(237, 3);
		this.btnNextData.Margin = new System.Windows.Forms.Padding(0);
		this.btnNextData.Name = "btnNextData";
		this.btnNextData.Size = new System.Drawing.Size(30, 25);
		this.btnNextData.TabIndex = 5;
		this.btnNextData.Text = ">";
		this.toolTipMain.SetToolTip(this.btnNextData, "Goto next page");
		this.btnNextData.UseVisualStyleBackColor = false;
		this.btnNextData.Click += new System.EventHandler(btnNextData_Click);
		this.btnPreData.BackColor = System.Drawing.SystemColors.Control;
		this.btnPreData.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnPreData.Dock = System.Windows.Forms.DockStyle.Top;
		this.btnPreData.Location = new System.Drawing.Point(207, 3);
		this.btnPreData.Margin = new System.Windows.Forms.Padding(0);
		this.btnPreData.Name = "btnPreData";
		this.btnPreData.Size = new System.Drawing.Size(30, 25);
		this.btnPreData.TabIndex = 4;
		this.btnPreData.Text = "<";
		this.toolTipMain.SetToolTip(this.btnPreData, "Goto previous page");
		this.btnPreData.UseVisualStyleBackColor = false;
		this.btnPreData.Click += new System.EventHandler(btnPreData_Click);
		this.btnFirstData.BackColor = System.Drawing.SystemColors.Control;
		this.btnFirstData.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnFirstData.Dock = System.Windows.Forms.DockStyle.Top;
		this.btnFirstData.Location = new System.Drawing.Point(177, 3);
		this.btnFirstData.Margin = new System.Windows.Forms.Padding(0);
		this.btnFirstData.Name = "btnFirstData";
		this.btnFirstData.Size = new System.Drawing.Size(30, 25);
		this.btnFirstData.TabIndex = 3;
		this.btnFirstData.Text = "<<";
		this.toolTipMain.SetToolTip(this.btnFirstData, "Goto first page");
		this.btnFirstData.UseVisualStyleBackColor = false;
		this.btnFirstData.Click += new System.EventHandler(btnFirstData_Click);
		this.dtpMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.dtpMain.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.dtpMain.Location = new System.Drawing.Point(43, 5);
		this.dtpMain.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
		this.dtpMain.Name = "dtpMain";
		this.dtpMain.Size = new System.Drawing.Size(120, 22);
		this.dtpMain.TabIndex = 2;
		this.toolTipMain.SetToolTip(this.dtpMain, "Select or enter date want search");
		this.btnPrevious.BackColor = System.Drawing.SystemColors.Control;
		this.btnPrevious.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnPrevious.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.btnPrevious.Location = new System.Drawing.Point(163, 4);
		this.btnPrevious.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
		this.btnPrevious.Name = "btnPrevious";
		this.btnPrevious.Size = new System.Drawing.Size(30, 25);
		this.btnPrevious.TabIndex = 3;
		this.btnPrevious.Text = "<";
		this.toolTipMain.SetToolTip(this.btnPrevious, "Goto previous date");
		this.btnPrevious.UseVisualStyleBackColor = false;
		this.btnPrevious.Click += new System.EventHandler(btnPrevious_Click);
		this.btnNext.BackColor = System.Drawing.SystemColors.Control;
		this.btnNext.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnNext.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.btnNext.Location = new System.Drawing.Point(237, 4);
		this.btnNext.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
		this.btnNext.Name = "btnNext";
		this.btnNext.Size = new System.Drawing.Size(30, 25);
		this.btnNext.TabIndex = 5;
		this.btnNext.Text = ">";
		this.toolTipMain.SetToolTip(this.btnNext, "Goto next date");
		this.btnNext.UseVisualStyleBackColor = false;
		this.btnNext.Click += new System.EventHandler(btnNext_Click);
		this.btnMain.AutoSize = true;
		this.btnMain.BackColor = System.Drawing.SystemColors.Control;
		this.btnMain.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnMain.Dock = System.Windows.Forms.DockStyle.Top;
		this.btnMain.Location = new System.Drawing.Point(193, 1);
		this.btnMain.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
		this.btnMain.Name = "btnMain";
		this.btnMain.Size = new System.Drawing.Size(44, 26);
		this.btnMain.TabIndex = 4;
		this.btnMain.Text = "Now";
		this.toolTipMain.SetToolTip(this.btnMain, "Goto current date");
		this.btnMain.UseVisualStyleBackColor = false;
		this.btnMain.Click += new System.EventHandler(btnMain_Click);
		this.btnAdd.AutoSize = true;
		this.btnAdd.BackColor = System.Drawing.SystemColors.Control;
		this.btnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnAdd.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnAdd.Location = new System.Drawing.Point(277, 0);
		this.btnAdd.Margin = new System.Windows.Forms.Padding(0);
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.Size = new System.Drawing.Size(100, 30);
		this.btnAdd.TabIndex = 6;
		this.btnAdd.Text = "New request";
		this.toolTipMain.SetToolTip(this.btnAdd, "Create new request");
		this.btnAdd.UseVisualStyleBackColor = true;
		this.btnBarcode.BackColor = System.Drawing.SystemColors.Control;
		this.btnBarcode.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnBarcode.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnBarcode.Enabled = false;
		this.btnBarcode.Image = _5S_QA_Client.Properties.Resources.qr_code;
		this.btnBarcode.Location = new System.Drawing.Point(377, 0);
		this.btnBarcode.Margin = new System.Windows.Forms.Padding(0);
		this.btnBarcode.Name = "btnBarcode";
		this.btnBarcode.Size = new System.Drawing.Size(30, 30);
		this.btnBarcode.TabIndex = 7;
		this.toolTipMain.SetToolTip(this.btnBarcode, "Create new request by qrcode");
		this.btnBarcode.UseVisualStyleBackColor = false;
		this.btnSearch.BackColor = System.Drawing.SystemColors.Control;
		this.btnSearch.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnSearch.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnSearch.FlatAppearance.BorderSize = 0;
		this.btnSearch.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.AppWorkspace;
		this.btnSearch.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLight;
		this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnSearch.Image = (System.Drawing.Image)resources.GetObject("btnSearch.Image");
		this.btnSearch.Location = new System.Drawing.Point(993, 0);
		this.btnSearch.Margin = new System.Windows.Forms.Padding(0);
		this.btnSearch.Name = "btnSearch";
		this.btnSearch.Size = new System.Drawing.Size(47, 25);
		this.btnSearch.TabIndex = 4;
		this.btnSearch.TabStop = false;
		this.toolTipMain.SetToolTip(this.btnSearch, "Search");
		this.btnSearch.UseVisualStyleBackColor = false;
		this.btnSearchThreeDelete.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnSearchThreeDelete.BackColor = System.Drawing.SystemColors.Window;
		this.btnSearchThreeDelete.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnSearchThreeDelete.FlatAppearance.BorderSize = 0;
		this.btnSearchThreeDelete.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.AppWorkspace;
		this.btnSearchThreeDelete.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
		this.btnSearchThreeDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnSearchThreeDelete.Image = _5S_QA_Client.Properties.Resources.cancel;
		this.btnSearchThreeDelete.Location = new System.Drawing.Point(307, 2);
		this.btnSearchThreeDelete.Margin = new System.Windows.Forms.Padding(0);
		this.btnSearchThreeDelete.Name = "btnSearchThreeDelete";
		this.btnSearchThreeDelete.Size = new System.Drawing.Size(21, 21);
		this.btnSearchThreeDelete.TabIndex = 7;
		this.btnSearchThreeDelete.TabStop = false;
		this.toolTipMain.SetToolTip(this.btnSearchThreeDelete, "Clear text of search term three");
		this.btnSearchThreeDelete.UseVisualStyleBackColor = false;
		this.btnSearchThreeDelete.Visible = false;
		this.btnSearchThreeDelete.Click += new System.EventHandler(btnSearchThreeDelete_Click);
		this.btnSearchTwoDelete.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnSearchTwoDelete.BackColor = System.Drawing.SystemColors.Window;
		this.btnSearchTwoDelete.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnSearchTwoDelete.FlatAppearance.BorderSize = 0;
		this.btnSearchTwoDelete.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.AppWorkspace;
		this.btnSearchTwoDelete.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
		this.btnSearchTwoDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnSearchTwoDelete.Image = _5S_QA_Client.Properties.Resources.cancel;
		this.btnSearchTwoDelete.Location = new System.Drawing.Point(307, 2);
		this.btnSearchTwoDelete.Margin = new System.Windows.Forms.Padding(0);
		this.btnSearchTwoDelete.Name = "btnSearchTwoDelete";
		this.btnSearchTwoDelete.Size = new System.Drawing.Size(21, 21);
		this.btnSearchTwoDelete.TabIndex = 6;
		this.btnSearchTwoDelete.TabStop = false;
		this.toolTipMain.SetToolTip(this.btnSearchTwoDelete, "Clear text of search term two");
		this.btnSearchTwoDelete.UseVisualStyleBackColor = false;
		this.btnSearchTwoDelete.Visible = false;
		this.btnSearchTwoDelete.Click += new System.EventHandler(btnSearchTwoDelete_Click);
		this.btnSearchOneDelete.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnSearchOneDelete.BackColor = System.Drawing.SystemColors.Window;
		this.btnSearchOneDelete.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnSearchOneDelete.FlatAppearance.BorderSize = 0;
		this.btnSearchOneDelete.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.AppWorkspace;
		this.btnSearchOneDelete.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
		this.btnSearchOneDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnSearchOneDelete.Image = _5S_QA_Client.Properties.Resources.cancel;
		this.btnSearchOneDelete.Location = new System.Drawing.Point(307, 2);
		this.btnSearchOneDelete.Margin = new System.Windows.Forms.Padding(0);
		this.btnSearchOneDelete.Name = "btnSearchOneDelete";
		this.btnSearchOneDelete.Size = new System.Drawing.Size(21, 21);
		this.btnSearchOneDelete.TabIndex = 5;
		this.btnSearchOneDelete.TabStop = false;
		this.toolTipMain.SetToolTip(this.btnSearchOneDelete, "Clear text of search term one");
		this.btnSearchOneDelete.UseVisualStyleBackColor = false;
		this.btnSearchOneDelete.Visible = false;
		this.btnSearchOneDelete.Click += new System.EventHandler(btnSearchOneDelete_Click);
		this.cbRejected.AutoSize = true;
		this.cbRejected.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbRejected.Dock = System.Windows.Forms.DockStyle.Left;
		this.cbRejected.Location = new System.Drawing.Point(410, 3);
		this.cbRejected.Name = "cbRejected";
		this.cbRejected.Size = new System.Drawing.Size(98, 24);
		this.cbRejected.TabIndex = 8;
		this.cbRejected.Text = "For rejected";
		this.toolTipMain.SetToolTip(this.cbRejected, "Select load all request with status is rejected");
		this.cbRejected.UseVisualStyleBackColor = true;
		this.cbRejected.CheckedChanged += new System.EventHandler(cbRejected_CheckedChanged);
		this.tableLayoutPanel1.AutoSize = true;
		this.tableLayoutPanel1.ColumnCount = 4;
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33332f));
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334f));
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334f));
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel1.Controls.Add(this.btnSearch, 3, 0);
		this.tableLayoutPanel1.Controls.Add(this.panel3, 2, 0);
		this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 0);
		this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
		this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
		this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
		this.tableLayoutPanel1.Name = "tableLayoutPanel1";
		this.tableLayoutPanel1.RowCount = 1;
		this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25f));
		this.tableLayoutPanel1.Size = new System.Drawing.Size(1040, 25);
		this.tableLayoutPanel1.TabIndex = 2;
		this.panel3.Controls.Add(this.btnSearchThreeDelete);
		this.panel3.Controls.Add(this.txtSearchThree);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(662, 0);
		this.panel3.Margin = new System.Windows.Forms.Padding(0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(331, 25);
		this.panel3.TabIndex = 3;
		this.panel2.Controls.Add(this.btnSearchTwoDelete);
		this.panel2.Controls.Add(this.txtSearchTwo);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(331, 0);
		this.panel2.Margin = new System.Windows.Forms.Padding(0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(331, 25);
		this.panel2.TabIndex = 2;
		this.panel1.Controls.Add(this.btnSearchOneDelete);
		this.panel1.Controls.Add(this.txtSearchOne);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(331, 25);
		this.panel1.TabIndex = 1;
		this.tableLayoutPanel2.AutoSize = true;
		this.tableLayoutPanel2.ColumnCount = 10;
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
		this.tableLayoutPanel2.Controls.Add(this.lblTotalRows, 9, 0);
		this.tableLayoutPanel2.Controls.Add(this.lblTotalRow, 8, 0);
		this.tableLayoutPanel2.Controls.Add(this.lblPage, 2, 0);
		this.tableLayoutPanel2.Controls.Add(this.lblLimit, 0, 0);
		this.tableLayoutPanel2.Controls.Add(this.txtPage, 3, 0);
		this.tableLayoutPanel2.Controls.Add(this.btnLastData, 7, 0);
		this.tableLayoutPanel2.Controls.Add(this.btnNextData, 6, 0);
		this.tableLayoutPanel2.Controls.Add(this.btnPreData, 5, 0);
		this.tableLayoutPanel2.Controls.Add(this.cbbLimit, 1, 0);
		this.tableLayoutPanel2.Controls.Add(this.btnFirstData, 4, 0);
		this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Right;
		this.tableLayoutPanel2.Location = new System.Drawing.Point(610, 3);
		this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
		this.tableLayoutPanel2.Name = "tableLayoutPanel2";
		this.tableLayoutPanel2.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
		this.tableLayoutPanel2.RowCount = 1;
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel2.Size = new System.Drawing.Size(430, 30);
		this.tableLayoutPanel2.TabIndex = 2;
		this.tableLayoutPanel2.TabStop = true;
		this.lblTotalRows.AutoSize = true;
		this.lblTotalRows.Dock = System.Windows.Forms.DockStyle.Top;
		this.lblTotalRows.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotalRows.Location = new System.Drawing.Point(378, 3);
		this.lblTotalRows.Name = "lblTotalRows";
		this.lblTotalRows.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
		this.lblTotalRows.Size = new System.Drawing.Size(49, 20);
		this.lblTotalRows.TabIndex = 69;
		this.lblTotalRows.Text = "0";
		this.lblTotalRows.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblTotalRows.TextChanged += new System.EventHandler(lblTotalRows_TextChanged);
		this.lblTotalRow.AutoSize = true;
		this.lblTotalRow.Dock = System.Windows.Forms.DockStyle.Top;
		this.lblTotalRow.Location = new System.Drawing.Point(300, 3);
		this.lblTotalRow.Name = "lblTotalRow";
		this.lblTotalRow.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
		this.lblTotalRow.Size = new System.Drawing.Size(72, 20);
		this.lblTotalRow.TabIndex = 68;
		this.lblTotalRow.Text = "Total rows:";
		this.lblTotalRow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblPage.AutoSize = true;
		this.lblPage.Dock = System.Windows.Forms.DockStyle.Top;
		this.lblPage.Location = new System.Drawing.Point(96, 3);
		this.lblPage.Name = "lblPage";
		this.lblPage.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
		this.lblPage.Size = new System.Drawing.Size(43, 20);
		this.lblPage.TabIndex = 67;
		this.lblPage.Text = "Page:";
		this.lblPage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblLimit.AutoSize = true;
		this.lblLimit.Dock = System.Windows.Forms.DockStyle.Top;
		this.lblLimit.Location = new System.Drawing.Point(3, 3);
		this.lblLimit.Name = "lblLimit";
		this.lblLimit.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
		this.lblLimit.Size = new System.Drawing.Size(37, 20);
		this.lblLimit.TabIndex = 65;
		this.lblLimit.Text = "Limit:";
		this.lblLimit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.tpanelMain.AutoSize = true;
		this.tpanelMain.ColumnCount = 10;
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10f));
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tpanelMain.Controls.Add(this.btnAdd, 6, 0);
		this.tpanelMain.Controls.Add(this.btnMain, 3, 0);
		this.tpanelMain.Controls.Add(this.btnNext, 4, 0);
		this.tpanelMain.Controls.Add(this.btnBarcode, 7, 0);
		this.tpanelMain.Controls.Add(this.rbAll, 0, 0);
		this.tpanelMain.Controls.Add(this.dtpMain, 1, 0);
		this.tpanelMain.Controls.Add(this.btnPrevious, 2, 0);
		this.tpanelMain.Controls.Add(this.cbRejected, 8, 0);
		this.tpanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tpanelMain.Location = new System.Drawing.Point(0, 3);
		this.tpanelMain.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
		this.tpanelMain.Name = "tpanelMain";
		this.tpanelMain.RowCount = 1;
		this.tpanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tpanelMain.Size = new System.Drawing.Size(610, 30);
		this.tpanelMain.TabIndex = 1;
		this.tpanelMain.TabStop = true;
		this.panel4.AutoSize = true;
		this.panel4.Controls.Add(this.tpanelMain);
		this.panel4.Controls.Add(this.tableLayoutPanel2);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 28);
		this.panel4.Name = "panel4";
		this.panel4.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
		this.panel4.Size = new System.Drawing.Size(1040, 33);
		this.panel4.TabIndex = 1;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoSize = true;
		this.BackColor = System.Drawing.SystemColors.Control;
		base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		base.Controls.Add(this.panel4);
		base.Controls.Add(this.tableLayoutPanel1);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Margin = new System.Windows.Forms.Padding(0);
		base.Name = "mSearch";
		base.Padding = new System.Windows.Forms.Padding(3);
		base.Size = new System.Drawing.Size(1046, 64);
		this.tableLayoutPanel1.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		this.tableLayoutPanel2.ResumeLayout(false);
		this.tableLayoutPanel2.PerformLayout();
		this.tpanelMain.ResumeLayout(false);
		this.tpanelMain.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
