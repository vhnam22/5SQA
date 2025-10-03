using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using _5S_QA_Entities.Constants;
using _5S_QA_System.Controls;
using _5S_QA_System.Properties;
using MetroFramework;
using MetroFramework.Controls;

namespace _5S_QA_System.View.User_control;

public class mSearch : UserControl
{
	private int totalPage;

	private string oldcbbLimit;

	private string oldtxtPage;

	private DataGridView dataGridView;

	private IContainer components = null;

	private ToolTip toolTipMain;

	private TableLayoutPanel tpanelSearch;

	private Panel panel3;

	private Panel panel2;

	private Panel panel1;

	private MetroTextBox txtSearchOne;

	private MetroTextBox txtSearchTwo;

	private MetroTextBox txtSearchThree;

	public ComboBox cbbRequestStatus;

	private Button btnSearchOneDelete;

	private Button btnSearchTwoDelete;

	private Button btnSearchThreeDelete;

	public Button btnSearch;

	private Panel panel4;

	private Button btnMain;

	private Button btnNext;

	public DateTimePicker dtpMain;

	private Button btnPrevious;

	private TableLayoutPanel tpanelPage;

	private Label lblTotalRows;

	private Label txtTotalRow;

	private Label lblPage;

	private Label lblLimit;

	private Button btnLastData;

	private Button btnNextData;

	private Button btnPreData;

	private Button btnFirstData;

	public MetroTextBox txtPage;

	public ComboBox cbbLimit;

	public CheckBox cbSearchAll;

	public TableLayoutPanel tpanelLimit;

	public TableLayoutPanel tpanelMain;

	private TableLayoutPanel tpanelOKNG;

	public CheckBox cbOK;

	public CheckBox cbNG;

	public DataTable dataTable { get; set; }

	public mSearch()
	{
		InitializeComponent();
		Common.setControls(this, toolTipMain);
		totalPage = 1;
		oldcbbLimit = cbbLimit.Text;
		oldtxtPage = txtPage.Text;
		load_cbbStatus();
	}

	public void Init(DataTable dt, DataGridView dgv)
	{
		dataTable = dt;
		dataGridView = dgv;
		getSetting();
		load_AutoComplete();
		Type type = base.ParentForm.GetType();
		if (type.Equals(typeof(frmRequestView)))
		{
			tpanelMain.Visible = true;
		}
		else if (type.Equals(typeof(frmRequestManagerView)))
		{
			tpanelOKNG.Visible = true;
			tpanelLimit.Visible = true;
			tpanelMain.Visible = true;
		}
	}

	private void getSetting()
	{
		Type type = base.ParentForm.GetType();
		if (type.Equals(typeof(frmStaffView)))
		{
			cbbLimit.Text = Settings.Default.limitStaff.ToString();
		}
		else if (type.Equals(typeof(frmMachineView)))
		{
			cbbLimit.Text = Settings.Default.limitMachine.ToString();
		}
		else if (type.Equals(typeof(frmProductView)))
		{
			cbbLimit.Text = Settings.Default.limitProduct.ToString();
		}
		else if (type.Equals(typeof(frmRequestView)))
		{
			cbbLimit.Text = Settings.Default.limitRequest.ToString();
		}
		else if (type.Equals(typeof(frmMeasurementView)))
		{
			cbbLimit.Text = Settings.Default.limitMeasurement.ToString();
		}
		else if (type.Equals(typeof(frmRequestManagerView)))
		{
			cbbLimit.Text = Settings.Default.limitRequestManager.ToString();
		}
		else if (type.Equals(typeof(frmHistoryView)))
		{
			cbbLimit.Text = Settings.Default.limitHistory.ToString();
		}
		else if (type.Equals(typeof(frmCommentView)))
		{
			cbbLimit.Text = Settings.Default.limitComment.ToString();
		}
		else if (type.Equals(typeof(frmTemplateView)))
		{
			cbbLimit.Text = Settings.Default.limitTemplate.ToString();
		}
		else if (type.Equals(typeof(frmResultView)))
		{
			cbbLimit.Text = Settings.Default.limitResult.ToString();
		}
		else if (type.Equals(typeof(frmPlanView)))
		{
			cbbLimit.Text = Settings.Default.limitPlan.ToString();
		}
		else if (type.Equals(typeof(frmPlanDetailView)))
		{
			cbbLimit.Text = Settings.Default.limitPlanDetail.ToString();
		}
	}

	private void setSetting()
	{
		Type type = base.ParentForm.GetType();
		if (type.Equals(typeof(frmStaffView)))
		{
			Settings.Default.limitStaff = int.Parse(cbbLimit.Text);
		}
		else if (type.Equals(typeof(frmMachineView)))
		{
			Settings.Default.limitMachine = int.Parse(cbbLimit.Text);
		}
		else if (type.Equals(typeof(frmProductView)))
		{
			Settings.Default.limitProduct = int.Parse(cbbLimit.Text);
		}
		else if (type.Equals(typeof(frmRequestView)))
		{
			Settings.Default.limitRequest = int.Parse(cbbLimit.Text);
		}
		else if (type.Equals(typeof(frmMeasurementView)))
		{
			Settings.Default.limitMeasurement = int.Parse(cbbLimit.Text);
		}
		else if (type.Equals(typeof(frmRequestManagerView)))
		{
			Settings.Default.limitRequestManager = int.Parse(cbbLimit.Text);
		}
		else if (type.Equals(typeof(frmHistoryView)))
		{
			Settings.Default.limitHistory = int.Parse(cbbLimit.Text);
		}
		else if (type.Equals(typeof(frmCommentView)))
		{
			Settings.Default.limitComment = int.Parse(cbbLimit.Text);
		}
		else if (type.Equals(typeof(frmTemplateView)))
		{
			Settings.Default.limitTemplate = int.Parse(cbbLimit.Text);
		}
		else if (type.Equals(typeof(frmResultView)))
		{
			Settings.Default.limitResult = int.Parse(cbbLimit.Text);
		}
		else if (type.Equals(typeof(frmPlanView)))
		{
			Settings.Default.limitPlan = int.Parse(cbbLimit.Text);
		}
		else if (type.Equals(typeof(frmPlanDetailView)))
		{
			Settings.Default.limitPlanDetail = int.Parse(cbbLimit.Text);
		}
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

	public DataTable SearchInAllColums(string name)
	{
		try
		{
			if (this.dataTable == null)
			{
				throw new NotImplementedException();
			}
			DataRow[] array = name switch
			{
				"btnNG" => (from DataRow r in this.dataTable.Rows
					where r.Field<string>("Judge") != null && r.Field<string>("Judge").Contains("NG")
					select r).ToArray(), 
				"btnEdit" => (from DataRow r in this.dataTable.Rows
					where r.Field<string>("History") != null && !r.Field<string>("History").Contains("[]")
					select r).ToArray(), 
				"btnOK" => (from DataRow r in this.dataTable.Rows
					where r.Field<string>("Judge") != null && r.Field<string>("Judge").Contains("OK")
					select r).ToArray(), 
				"btnWarning" => (from DataRow r in this.dataTable.Rows
					where r.Field<string>("Judge") != null && (r.Field<string>("Judge").Equals("OK+") || r.Field<string>("Judge").Equals("OK-"))
					select r).ToArray(), 
				_ => (from DataRow r in this.dataTable.Rows
					where string.IsNullOrEmpty(r.Field<string>("Result"))
					select r).ToArray(), 
			};
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

	private void load_cbbStatus()
	{
		cbbRequestStatus.Items.AddRange(MetaType.lstStatus);
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

	private void btnMain_Click(object sender, EventArgs e)
	{
		dtpMain.Value = DateTime.Now;
	}

	private void btnPrevious_Click(object sender, EventArgs e)
	{
		if (cbSearchAll.Checked)
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
		if (cbSearchAll.Checked)
		{
			dtpMain.Value = dtpMain.Value.AddMonths(1);
		}
		else
		{
			dtpMain.Value = dtpMain.Value.AddDays(1.0);
		}
	}

	private void cbSearchAll_CheckedChanged(object sender, EventArgs e)
	{
		cbbRequestStatus.Enabled = !cbSearchAll.Checked;
		if (cbSearchAll.Checked)
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_5S_QA_System.View.User_control.mSearch));
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.txtSearchOne = new MetroFramework.Controls.MetroTextBox();
		this.txtSearchTwo = new MetroFramework.Controls.MetroTextBox();
		this.txtSearchThree = new MetroFramework.Controls.MetroTextBox();
		this.cbbRequestStatus = new System.Windows.Forms.ComboBox();
		this.btnSearchOneDelete = new System.Windows.Forms.Button();
		this.btnSearchTwoDelete = new System.Windows.Forms.Button();
		this.btnSearchThreeDelete = new System.Windows.Forms.Button();
		this.btnSearch = new System.Windows.Forms.Button();
		this.btnMain = new System.Windows.Forms.Button();
		this.btnNext = new System.Windows.Forms.Button();
		this.dtpMain = new System.Windows.Forms.DateTimePicker();
		this.btnPrevious = new System.Windows.Forms.Button();
		this.txtPage = new MetroFramework.Controls.MetroTextBox();
		this.btnLastData = new System.Windows.Forms.Button();
		this.btnNextData = new System.Windows.Forms.Button();
		this.btnPreData = new System.Windows.Forms.Button();
		this.cbbLimit = new System.Windows.Forms.ComboBox();
		this.btnFirstData = new System.Windows.Forms.Button();
		this.cbSearchAll = new System.Windows.Forms.CheckBox();
		this.tpanelSearch = new System.Windows.Forms.TableLayoutPanel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tpanelLimit = new System.Windows.Forms.TableLayoutPanel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.tpanelMain = new System.Windows.Forms.TableLayoutPanel();
		this.tpanelPage = new System.Windows.Forms.TableLayoutPanel();
		this.lblTotalRows = new System.Windows.Forms.Label();
		this.txtTotalRow = new System.Windows.Forms.Label();
		this.lblPage = new System.Windows.Forms.Label();
		this.lblLimit = new System.Windows.Forms.Label();
		this.tpanelOKNG = new System.Windows.Forms.TableLayoutPanel();
		this.cbOK = new System.Windows.Forms.CheckBox();
		this.cbNG = new System.Windows.Forms.CheckBox();
		this.tpanelSearch.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel1.SuspendLayout();
		this.tpanelLimit.SuspendLayout();
		this.panel4.SuspendLayout();
		this.tpanelMain.SuspendLayout();
		this.tpanelPage.SuspendLayout();
		this.tpanelOKNG.SuspendLayout();
		base.SuspendLayout();
		this.txtSearchOne.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtSearchOne.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtSearchOne.CustomButton.Image = null;
		this.txtSearchOne.CustomButton.Location = new System.Drawing.Point(309, 1);
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
		this.txtSearchOne.Size = new System.Drawing.Size(333, 25);
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
		this.txtSearchTwo.CustomButton.Location = new System.Drawing.Point(309, 1);
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
		this.txtSearchTwo.Size = new System.Drawing.Size(333, 25);
		this.txtSearchTwo.TabIndex = 2;
		this.toolTipMain.SetToolTip(this.txtSearchTwo, "Enter a search term two");
		this.txtSearchTwo.UseSelectable = true;
		this.txtSearchTwo.WaterMark = "Enter a search term two";
		this.txtSearchTwo.WaterMarkColor = System.Drawing.Color.Silver;
		this.txtSearchTwo.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSearchTwo.KeyUp += new System.Windows.Forms.KeyEventHandler(txtSearchTwo_KeyUp);
		this.txtSearchThree.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtSearchThree.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtSearchThree.CustomButton.Image = null;
		this.txtSearchThree.CustomButton.Location = new System.Drawing.Point(309, 1);
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
		this.txtSearchThree.Size = new System.Drawing.Size(333, 25);
		this.txtSearchThree.TabIndex = 3;
		this.toolTipMain.SetToolTip(this.txtSearchThree, "Enter a search term three");
		this.txtSearchThree.UseSelectable = true;
		this.txtSearchThree.WaterMark = "Enter a search term three";
		this.txtSearchThree.WaterMarkColor = System.Drawing.Color.Silver;
		this.txtSearchThree.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSearchThree.KeyUp += new System.Windows.Forms.KeyEventHandler(txtSearchThree_KeyUp);
		this.cbbRequestStatus.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cbbRequestStatus.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cbbRequestStatus.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbbRequestStatus.Dock = System.Windows.Forms.DockStyle.Fill;
		this.cbbRequestStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbbRequestStatus.FormattingEnabled = true;
		this.cbbRequestStatus.Location = new System.Drawing.Point(3, 3);
		this.cbbRequestStatus.Name = "cbbRequestStatus";
		this.cbbRequestStatus.Size = new System.Drawing.Size(100, 24);
		this.cbbRequestStatus.TabIndex = 1;
		this.toolTipMain.SetToolTip(this.cbbRequestStatus, "Select sample");
		this.btnSearchOneDelete.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnSearchOneDelete.BackColor = System.Drawing.SystemColors.Window;
		this.btnSearchOneDelete.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnSearchOneDelete.FlatAppearance.BorderSize = 0;
		this.btnSearchOneDelete.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.AppWorkspace;
		this.btnSearchOneDelete.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
		this.btnSearchOneDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnSearchOneDelete.Image = _5S_QA_System.Properties.Resources.cancel;
		this.btnSearchOneDelete.Location = new System.Drawing.Point(309, 2);
		this.btnSearchOneDelete.Margin = new System.Windows.Forms.Padding(0);
		this.btnSearchOneDelete.Name = "btnSearchOneDelete";
		this.btnSearchOneDelete.Size = new System.Drawing.Size(21, 21);
		this.btnSearchOneDelete.TabIndex = 53;
		this.btnSearchOneDelete.TabStop = false;
		this.toolTipMain.SetToolTip(this.btnSearchOneDelete, "Clear text of search term one");
		this.btnSearchOneDelete.UseVisualStyleBackColor = false;
		this.btnSearchOneDelete.Visible = false;
		this.btnSearchOneDelete.Click += new System.EventHandler(btnSearchOneDelete_Click);
		this.btnSearchTwoDelete.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnSearchTwoDelete.BackColor = System.Drawing.SystemColors.Window;
		this.btnSearchTwoDelete.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnSearchTwoDelete.FlatAppearance.BorderSize = 0;
		this.btnSearchTwoDelete.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.AppWorkspace;
		this.btnSearchTwoDelete.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
		this.btnSearchTwoDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnSearchTwoDelete.Image = _5S_QA_System.Properties.Resources.cancel;
		this.btnSearchTwoDelete.Location = new System.Drawing.Point(309, 2);
		this.btnSearchTwoDelete.Margin = new System.Windows.Forms.Padding(0);
		this.btnSearchTwoDelete.Name = "btnSearchTwoDelete";
		this.btnSearchTwoDelete.Size = new System.Drawing.Size(21, 21);
		this.btnSearchTwoDelete.TabIndex = 54;
		this.btnSearchTwoDelete.TabStop = false;
		this.toolTipMain.SetToolTip(this.btnSearchTwoDelete, "Clear text of search term one");
		this.btnSearchTwoDelete.UseVisualStyleBackColor = false;
		this.btnSearchTwoDelete.Visible = false;
		this.btnSearchTwoDelete.Click += new System.EventHandler(btnSearchTwoDelete_Click);
		this.btnSearchThreeDelete.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnSearchThreeDelete.BackColor = System.Drawing.SystemColors.Window;
		this.btnSearchThreeDelete.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnSearchThreeDelete.FlatAppearance.BorderSize = 0;
		this.btnSearchThreeDelete.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.AppWorkspace;
		this.btnSearchThreeDelete.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
		this.btnSearchThreeDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnSearchThreeDelete.Image = _5S_QA_System.Properties.Resources.cancel;
		this.btnSearchThreeDelete.Location = new System.Drawing.Point(309, 2);
		this.btnSearchThreeDelete.Margin = new System.Windows.Forms.Padding(0);
		this.btnSearchThreeDelete.Name = "btnSearchThreeDelete";
		this.btnSearchThreeDelete.Size = new System.Drawing.Size(21, 21);
		this.btnSearchThreeDelete.TabIndex = 55;
		this.btnSearchThreeDelete.TabStop = false;
		this.toolTipMain.SetToolTip(this.btnSearchThreeDelete, "Clear text of search term one");
		this.btnSearchThreeDelete.UseVisualStyleBackColor = false;
		this.btnSearchThreeDelete.Visible = false;
		this.btnSearchThreeDelete.Click += new System.EventHandler(btnSearchThreeDelete_Click);
		this.btnSearch.BackColor = System.Drawing.SystemColors.Control;
		this.btnSearch.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnSearch.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnSearch.FlatAppearance.BorderSize = 0;
		this.btnSearch.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.AppWorkspace;
		this.btnSearch.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLight;
		this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnSearch.Image = (System.Drawing.Image)resources.GetObject("btnSearch.Image");
		this.btnSearch.Location = new System.Drawing.Point(999, 0);
		this.btnSearch.Margin = new System.Windows.Forms.Padding(0);
		this.btnSearch.Name = "btnSearch";
		this.btnSearch.Size = new System.Drawing.Size(41, 25);
		this.btnSearch.TabIndex = 70;
		this.btnSearch.TabStop = false;
		this.toolTipMain.SetToolTip(this.btnSearch, "Search");
		this.btnSearch.UseVisualStyleBackColor = false;
		this.btnMain.AutoSize = true;
		this.btnMain.BackColor = System.Drawing.SystemColors.Control;
		this.btnMain.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnMain.Dock = System.Windows.Forms.DockStyle.Top;
		this.btnMain.Location = new System.Drawing.Point(150, 1);
		this.btnMain.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
		this.btnMain.Name = "btnMain";
		this.btnMain.Size = new System.Drawing.Size(44, 26);
		this.btnMain.TabIndex = 3;
		this.btnMain.Text = "Now";
		this.toolTipMain.SetToolTip(this.btnMain, "Goto current date");
		this.btnMain.UseVisualStyleBackColor = false;
		this.btnMain.Click += new System.EventHandler(btnMain_Click);
		this.btnNext.BackColor = System.Drawing.SystemColors.Control;
		this.btnNext.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnNext.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.btnNext.Location = new System.Drawing.Point(194, 4);
		this.btnNext.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
		this.btnNext.Name = "btnNext";
		this.btnNext.Size = new System.Drawing.Size(30, 25);
		this.btnNext.TabIndex = 4;
		this.btnNext.Text = ">";
		this.toolTipMain.SetToolTip(this.btnNext, "Goto next date");
		this.btnNext.UseVisualStyleBackColor = false;
		this.btnNext.Click += new System.EventHandler(btnNext_Click);
		this.dtpMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.dtpMain.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.dtpMain.Location = new System.Drawing.Point(0, 5);
		this.dtpMain.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
		this.dtpMain.Name = "dtpMain";
		this.dtpMain.Size = new System.Drawing.Size(120, 22);
		this.dtpMain.TabIndex = 1;
		this.toolTipMain.SetToolTip(this.dtpMain, "Select or enter date want search");
		this.btnPrevious.BackColor = System.Drawing.SystemColors.Control;
		this.btnPrevious.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnPrevious.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.btnPrevious.Location = new System.Drawing.Point(120, 4);
		this.btnPrevious.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
		this.btnPrevious.Name = "btnPrevious";
		this.btnPrevious.Size = new System.Drawing.Size(30, 25);
		this.btnPrevious.TabIndex = 2;
		this.btnPrevious.Text = "<";
		this.toolTipMain.SetToolTip(this.btnPrevious, "Goto previous date");
		this.btnPrevious.UseVisualStyleBackColor = false;
		this.btnPrevious.Click += new System.EventHandler(btnPrevious_Click);
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
		this.txtPage.WaterMarkColor = System.Drawing.Color.Silver;
		this.txtPage.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtPage_KeyPress);
		this.txtPage.Leave += new System.EventHandler(txtPage_Leave);
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
		this.cbbLimit.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cbbLimit.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cbbLimit.Dock = System.Windows.Forms.DockStyle.Top;
		this.cbbLimit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cbbLimit.FormattingEnabled = true;
		this.cbbLimit.Items.AddRange(new object[19]
		{
			"50", "100", "150", "200", "250", "300", "350", "400", "450", "500",
			"550", "600", "650", "700", "750", "800", "850", "900", "950"
		});
		this.cbbLimit.Location = new System.Drawing.Point(43, 3);
		this.cbbLimit.Margin = new System.Windows.Forms.Padding(0);
		this.cbbLimit.MaxLength = 3;
		this.cbbLimit.Name = "cbbLimit";
		this.cbbLimit.Size = new System.Drawing.Size(50, 24);
		this.cbbLimit.TabIndex = 1;
		this.cbbLimit.Text = "50";
		this.toolTipMain.SetToolTip(this.cbbLimit, "Select or enter limit a page");
		this.cbbLimit.SelectedIndexChanged += new System.EventHandler(cbbLimit_SelectedIndexChanged);
		this.cbbLimit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(cbbLimit_KeyPress);
		this.cbbLimit.Leave += new System.EventHandler(cbbLimit_Leave);
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
		this.cbSearchAll.AutoSize = true;
		this.cbSearchAll.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbSearchAll.Dock = System.Windows.Forms.DockStyle.Fill;
		this.cbSearchAll.Location = new System.Drawing.Point(109, 3);
		this.cbSearchAll.Name = "cbSearchAll";
		this.cbSearchAll.Size = new System.Drawing.Size(41, 24);
		this.cbSearchAll.TabIndex = 2;
		this.cbSearchAll.Text = "All";
		this.toolTipMain.SetToolTip(this.cbSearchAll, "Select to show all requests");
		this.cbSearchAll.UseVisualStyleBackColor = true;
		this.cbSearchAll.CheckedChanged += new System.EventHandler(cbSearchAll_CheckedChanged);
		this.tpanelSearch.AutoSize = true;
		this.tpanelSearch.ColumnCount = 4;
		this.tpanelSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33332f));
		this.tpanelSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334f));
		this.tpanelSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334f));
		this.tpanelSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelSearch.Controls.Add(this.btnSearch, 3, 0);
		this.tpanelSearch.Controls.Add(this.panel3, 2, 0);
		this.tpanelSearch.Controls.Add(this.panel2, 1, 0);
		this.tpanelSearch.Controls.Add(this.panel1, 0, 0);
		this.tpanelSearch.Dock = System.Windows.Forms.DockStyle.Top;
		this.tpanelSearch.Location = new System.Drawing.Point(3, 3);
		this.tpanelSearch.Margin = new System.Windows.Forms.Padding(0);
		this.tpanelSearch.Name = "tpanelSearch";
		this.tpanelSearch.RowCount = 1;
		this.tpanelSearch.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25f));
		this.tpanelSearch.Size = new System.Drawing.Size(1040, 25);
		this.tpanelSearch.TabIndex = 1;
		this.panel3.Controls.Add(this.btnSearchThreeDelete);
		this.panel3.Controls.Add(this.txtSearchThree);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(666, 0);
		this.panel3.Margin = new System.Windows.Forms.Padding(0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(333, 25);
		this.panel3.TabIndex = 69;
		this.panel2.Controls.Add(this.btnSearchTwoDelete);
		this.panel2.Controls.Add(this.txtSearchTwo);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(333, 0);
		this.panel2.Margin = new System.Windows.Forms.Padding(0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(333, 25);
		this.panel2.TabIndex = 68;
		this.panel1.Controls.Add(this.btnSearchOneDelete);
		this.panel1.Controls.Add(this.txtSearchOne);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(333, 25);
		this.panel1.TabIndex = 64;
		this.tpanelLimit.AutoSize = true;
		this.tpanelLimit.ColumnCount = 2;
		this.tpanelLimit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelLimit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelLimit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tpanelLimit.Controls.Add(this.cbSearchAll, 1, 0);
		this.tpanelLimit.Controls.Add(this.cbbRequestStatus, 0, 0);
		this.tpanelLimit.Dock = System.Windows.Forms.DockStyle.Left;
		this.tpanelLimit.Location = new System.Drawing.Point(224, 3);
		this.tpanelLimit.Name = "tpanelLimit";
		this.tpanelLimit.RowCount = 1;
		this.tpanelLimit.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tpanelLimit.Size = new System.Drawing.Size(153, 30);
		this.tpanelLimit.TabIndex = 2;
		this.tpanelLimit.TabStop = true;
		this.tpanelLimit.Visible = false;
		this.panel4.Controls.Add(this.tpanelOKNG);
		this.panel4.Controls.Add(this.tpanelLimit);
		this.panel4.Controls.Add(this.tpanelMain);
		this.panel4.Controls.Add(this.tpanelPage);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 28);
		this.panel4.Name = "panel4";
		this.panel4.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
		this.panel4.Size = new System.Drawing.Size(1040, 33);
		this.panel4.TabIndex = 53;
		this.tpanelMain.AutoSize = true;
		this.tpanelMain.ColumnCount = 4;
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMain.Controls.Add(this.btnMain, 2, 0);
		this.tpanelMain.Controls.Add(this.btnNext, 3, 0);
		this.tpanelMain.Controls.Add(this.dtpMain, 0, 0);
		this.tpanelMain.Controls.Add(this.btnPrevious, 1, 0);
		this.tpanelMain.Dock = System.Windows.Forms.DockStyle.Left;
		this.tpanelMain.Location = new System.Drawing.Point(0, 3);
		this.tpanelMain.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
		this.tpanelMain.Name = "tpanelMain";
		this.tpanelMain.RowCount = 1;
		this.tpanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tpanelMain.Size = new System.Drawing.Size(224, 30);
		this.tpanelMain.TabIndex = 1;
		this.tpanelMain.TabStop = true;
		this.tpanelMain.Visible = false;
		this.tpanelPage.AutoSize = true;
		this.tpanelPage.ColumnCount = 10;
		this.tpanelPage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelPage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelPage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelPage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelPage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelPage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelPage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelPage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelPage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelPage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70f));
		this.tpanelPage.Controls.Add(this.lblTotalRows, 9, 0);
		this.tpanelPage.Controls.Add(this.txtTotalRow, 8, 0);
		this.tpanelPage.Controls.Add(this.lblPage, 2, 0);
		this.tpanelPage.Controls.Add(this.lblLimit, 0, 0);
		this.tpanelPage.Controls.Add(this.txtPage, 3, 0);
		this.tpanelPage.Controls.Add(this.btnLastData, 7, 0);
		this.tpanelPage.Controls.Add(this.btnNextData, 6, 0);
		this.tpanelPage.Controls.Add(this.btnPreData, 5, 0);
		this.tpanelPage.Controls.Add(this.cbbLimit, 1, 0);
		this.tpanelPage.Controls.Add(this.btnFirstData, 4, 0);
		this.tpanelPage.Dock = System.Windows.Forms.DockStyle.Right;
		this.tpanelPage.Location = new System.Drawing.Point(595, 3);
		this.tpanelPage.Margin = new System.Windows.Forms.Padding(0);
		this.tpanelPage.Name = "tpanelPage";
		this.tpanelPage.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
		this.tpanelPage.RowCount = 1;
		this.tpanelPage.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tpanelPage.Size = new System.Drawing.Size(445, 30);
		this.tpanelPage.TabIndex = 3;
		this.tpanelPage.TabStop = true;
		this.lblTotalRows.AutoSize = true;
		this.lblTotalRows.Dock = System.Windows.Forms.DockStyle.Top;
		this.lblTotalRows.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotalRows.Location = new System.Drawing.Point(378, 3);
		this.lblTotalRows.Name = "lblTotalRows";
		this.lblTotalRows.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
		this.lblTotalRows.Size = new System.Drawing.Size(64, 20);
		this.lblTotalRows.TabIndex = 69;
		this.lblTotalRows.Text = "0";
		this.lblTotalRows.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblTotalRows.TextChanged += new System.EventHandler(lblTotalRows_TextChanged);
		this.txtTotalRow.AutoSize = true;
		this.txtTotalRow.Dock = System.Windows.Forms.DockStyle.Top;
		this.txtTotalRow.Location = new System.Drawing.Point(300, 3);
		this.txtTotalRow.Name = "txtTotalRow";
		this.txtTotalRow.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
		this.txtTotalRow.Size = new System.Drawing.Size(72, 20);
		this.txtTotalRow.TabIndex = 68;
		this.txtTotalRow.Text = "Total rows:";
		this.txtTotalRow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
		this.tpanelOKNG.AutoSize = true;
		this.tpanelOKNG.ColumnCount = 4;
		this.tpanelOKNG.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tpanelOKNG.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelOKNG.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelOKNG.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tpanelOKNG.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tpanelOKNG.Controls.Add(this.cbOK, 1, 0);
		this.tpanelOKNG.Controls.Add(this.cbNG, 2, 0);
		this.tpanelOKNG.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tpanelOKNG.Location = new System.Drawing.Point(377, 3);
		this.tpanelOKNG.Name = "tpanelOKNG";
		this.tpanelOKNG.RowCount = 1;
		this.tpanelOKNG.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tpanelOKNG.Size = new System.Drawing.Size(218, 30);
		this.tpanelOKNG.TabIndex = 4;
		this.tpanelOKNG.TabStop = true;
		this.tpanelOKNG.Visible = false;
		this.cbOK.AutoSize = true;
		this.cbOK.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbOK.Dock = System.Windows.Forms.DockStyle.Fill;
		this.cbOK.ForeColor = System.Drawing.Color.Blue;
		this.cbOK.Location = new System.Drawing.Point(61, 3);
		this.cbOK.Name = "cbOK";
		this.cbOK.Size = new System.Drawing.Size(44, 24);
		this.cbOK.TabIndex = 1;
		this.cbOK.Text = "OK";
		this.toolTipMain.SetToolTip(this.cbOK, "Select to show requests is OK");
		this.cbOK.UseVisualStyleBackColor = true;
		this.cbNG.AutoSize = true;
		this.cbNG.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbNG.Dock = System.Windows.Forms.DockStyle.Fill;
		this.cbNG.ForeColor = System.Drawing.Color.Red;
		this.cbNG.Location = new System.Drawing.Point(111, 3);
		this.cbNG.Name = "cbNG";
		this.cbNG.Size = new System.Drawing.Size(46, 24);
		this.cbNG.TabIndex = 2;
		this.cbNG.Text = "NG";
		this.toolTipMain.SetToolTip(this.cbNG, "Select to show requests is NG");
		this.cbNG.UseVisualStyleBackColor = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoSize = true;
		this.BackColor = System.Drawing.SystemColors.Control;
		base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		base.Controls.Add(this.panel4);
		base.Controls.Add(this.tpanelSearch);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Name = "mSearch";
		base.Padding = new System.Windows.Forms.Padding(3);
		base.Size = new System.Drawing.Size(1046, 64);
		this.tpanelSearch.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		this.tpanelLimit.ResumeLayout(false);
		this.tpanelLimit.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.tpanelMain.ResumeLayout(false);
		this.tpanelMain.PerformLayout();
		this.tpanelPage.ResumeLayout(false);
		this.tpanelPage.PerformLayout();
		this.tpanelOKNG.ResumeLayout(false);
		this.tpanelOKNG.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
