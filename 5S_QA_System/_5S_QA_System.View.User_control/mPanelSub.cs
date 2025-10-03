using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using _5S_QA_DL.APIContext;
using _5S_QA_Entities.Dtos;
using _5S_QA_Entities.Models;
using _5S_QA_System.Controls;
using _5S_QA_System.Properties;
using MetroFramework;
using MetroFramework.Controls;

namespace _5S_QA_System.View.User_control;

public class mPanelSub : UserControl
{
	private int totalPage;

	private string oldcbbLimit;

	private string oldtxtPage;

	private DataTable dataTable;

	private IContainer components = null;

	private ToolTip toolTipMain;

	private Panel panelResize;

	private Panel panelSearch;

	private MetroTextBox txtSearch;

	private TableLayoutPanel tableLayoutPanel2;

	public MetroTextBox txtPage;

	private Button btnLastData;

	private Button btnNextData;

	private Button btnPreData;

	public ComboBox cbbLimit;

	private Button btnFirstData;

	private ContextMenuStrip contextMenuStripMain;

	private ToolStripMenuItem main_refreshToolStripMenuItem;

	public DataGridView dgvMain;

	private Panel panel1;

	public Button btnSearch;

	private Button btnSearchDelete;

	private Label lblTotalRows;

	private DataGridViewTextBoxColumn No;

	private DataGridViewTextBoxColumn Code;

	private DataGridViewTextBoxColumn name;

	private DataGridViewTextBoxColumn GroupCodeName;

	private DataGridViewTextBoxColumn ImageUrl;

	private DataGridViewTextBoxColumn TotalMeas;

	private DataGridViewTextBoxColumn Id;

	public bool isEdit { get; set; }

	public int mRow { get; set; }

	public int mCol { get; set; }

	public mPanelSub()
	{
		InitializeComponent();
		Common.setControls(this, toolTipMain, new List<ContextMenuStrip> { contextMenuStripMain });
		totalPage = 1;
		oldcbbLimit = cbbLimit.Text;
		oldtxtPage = txtPage.Text;
		ControlResize.Init(panelResize, this);
		isEdit = false;
		mRow = 0;
		mCol = 0;
		getSetting();
	}

	public void Init(Guid id)
	{
		load_AllData(id);
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
			foreach (DataGridViewColumn column in dgvMain.Columns)
			{
				if (column.Visible && !column.Name.Equals("No") && !string.IsNullOrEmpty(row[column.Index].ToString()))
				{
					autoCompleteStringCollection.Add(row[column.DataPropertyName].ToString());
				}
			}
		}
		txtSearch.AutoCompleteCustomSource = autoCompleteStringCollection;
	}

	private DataTable SearchInAllColums()
	{
		try
		{
			if (this.dataTable != null)
			{
				StringComparison comparison = StringComparison.OrdinalIgnoreCase;
				DataRow[] array = (from DataRow r in this.dataTable.Rows
					where r.ItemArray.Any((object c) => c.ToString().IndexOf(txtSearch.Text, comparison) >= 0)
					select r).ToArray();
				lblTotalRows.Text = array.Length.ToString();
				int num = int.Parse(cbbLimit.Text);
				int count = (int.Parse(txtPage.Text) - 1) * num;
				array = array.Skip(count).Take(num).ToArray();
				if (array.Length == 0)
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

	private void load_AllData(Guid id = default(Guid))
	{
		Cursor.Current = Cursors.WaitCursor;
		try
		{
			isEdit = true;
			QueryArgs queryArgs = new QueryArgs
			{
				Order = "Created DESC",
				Page = 1,
				Limit = int.MaxValue
			};
			if (!id.Equals(Guid.Empty))
			{
				queryArgs.Predicate = "Id=@0";
				queryArgs.PredicateParameters = new string[1] { id.ToString() };
			}
			ResponseDto result = frmLogin.client.GetsAsync(queryArgs, "/api/Product/Gets").Result;
			dataTable = Common.getDataTable<ProductQuickViewModel>(result);
			load_AutoComplete();
			load_dgvMain();
		}
		catch (Exception ex)
		{
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

	private void load_dgvMain()
	{
		dgvMain.DataSource = SearchInAllColums();
		dgvMain.Refresh();
		if (isEdit)
		{
			try
			{
				dgvMain.Rows[mRow].Cells[mCol].Selected = true;
			}
			catch
			{
			}
		}
		dgvMain_Sorted(dgvMain, null);
		isEdit = false;
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
		cbbLimit.Text = Settings.Default.limitProductSub.ToString();
	}

	private void setSetting()
	{
		Settings.Default.limitProductSub = int.Parse(cbbLimit.Text);
		Settings.Default.Save();
	}

	private void btnSearch_Click(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		load_dgvMain();
	}

	private void txtSearch_KeyUp(object sender, KeyEventArgs e)
	{
		if (string.IsNullOrEmpty(txtSearch.Text))
		{
			btnSearchDelete.Visible = false;
		}
		else
		{
			btnSearchDelete.Visible = true;
		}
		btnSearch.PerformClick();
	}

	private void btnSearchDelete_Click(object sender, EventArgs e)
	{
		txtSearch.Clear();
		txtSearch_KeyUp(this, null);
		txtSearch.Focus();
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
			if (string.IsNullOrEmpty(comboBox.Text))
			{
				comboBox.SelectedIndex = 0;
			}
			else if (int.Parse(comboBox.Text).Equals(0))
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
			if (string.IsNullOrEmpty(metroTextBox.Text))
			{
				metroTextBox.Text = "1";
			}
			else if (int.Parse(metroTextBox.Text).Equals(0))
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

	private void main_refreshToolStripMenuItem_Click(object sender, EventArgs e)
	{
		btnSearch.Select();
		load_AllData();
	}

	private void dgvMain_Sorted(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		DataGridView dataGridView = sender as DataGridView;
		int.TryParse(txtPage.Text, out var result);
		int.TryParse(cbbLimit.Text, out var result2);
		foreach (DataGridViewRow item in (IEnumerable)dataGridView.Rows)
		{
			item.Cells["No"].Value = (result - 1) * result2 + item.Index + 1;
			if (int.Parse(item.Cells["TotalMeas"].Value.ToString()) == 0)
			{
				item.DefaultCellStyle.ForeColor = SystemColors.GrayText;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_5S_QA_System.View.User_control.mPanelSub));
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.txtSearch = new MetroFramework.Controls.MetroTextBox();
		this.txtPage = new MetroFramework.Controls.MetroTextBox();
		this.btnLastData = new System.Windows.Forms.Button();
		this.btnNextData = new System.Windows.Forms.Button();
		this.btnPreData = new System.Windows.Forms.Button();
		this.cbbLimit = new System.Windows.Forms.ComboBox();
		this.btnFirstData = new System.Windows.Forms.Button();
		this.btnSearch = new System.Windows.Forms.Button();
		this.btnSearchDelete = new System.Windows.Forms.Button();
		this.panelResize = new System.Windows.Forms.Panel();
		this.panelSearch = new System.Windows.Forms.Panel();
		this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
		this.lblTotalRows = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.dgvMain = new System.Windows.Forms.DataGridView();
		this.contextMenuStripMain = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.main_refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.No = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.GroupCodeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ImageUrl = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.TotalMeas = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.panelSearch.SuspendLayout();
		this.tableLayoutPanel2.SuspendLayout();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvMain).BeginInit();
		this.contextMenuStripMain.SuspendLayout();
		base.SuspendLayout();
		this.txtSearch.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtSearch.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtSearch.CustomButton.Image = null;
		this.txtSearch.CustomButton.Location = new System.Drawing.Point(214, 1);
		this.txtSearch.CustomButton.Name = "";
		this.txtSearch.CustomButton.Size = new System.Drawing.Size(23, 23);
		this.txtSearch.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
		this.txtSearch.CustomButton.TabIndex = 1;
		this.txtSearch.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
		this.txtSearch.CustomButton.UseSelectable = true;
		this.txtSearch.CustomButton.Visible = false;
		this.txtSearch.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtSearch.Lines = new string[0];
		this.txtSearch.Location = new System.Drawing.Point(0, 0);
		this.txtSearch.MaxLength = 32767;
		this.txtSearch.Name = "txtSearch";
		this.txtSearch.PasswordChar = '\0';
		this.txtSearch.ScrollBars = System.Windows.Forms.ScrollBars.None;
		this.txtSearch.SelectedText = "";
		this.txtSearch.SelectionLength = 0;
		this.txtSearch.SelectionStart = 0;
		this.txtSearch.ShortcutsEnabled = true;
		this.txtSearch.Size = new System.Drawing.Size(238, 25);
		this.txtSearch.TabIndex = 62;
		this.toolTipMain.SetToolTip(this.txtSearch, "Enter a search term");
		this.txtSearch.UseSelectable = true;
		this.txtSearch.WaterMark = "Enter a search term";
		this.txtSearch.WaterMarkColor = System.Drawing.Color.Silver;
		this.txtSearch.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(txtSearch_KeyUp);
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
		this.txtPage.Location = new System.Drawing.Point(53, 4);
		this.txtPage.Margin = new System.Windows.Forms.Padding(2, 1, 0, 0);
		this.txtPage.MaxLength = 3;
		this.txtPage.Name = "txtPage";
		this.txtPage.PasswordChar = '\0';
		this.txtPage.ScrollBars = System.Windows.Forms.ScrollBars.None;
		this.txtPage.SelectedText = "";
		this.txtPage.SelectionLength = 0;
		this.txtPage.SelectionStart = 0;
		this.txtPage.ShortcutsEnabled = true;
		this.txtPage.Size = new System.Drawing.Size(35, 23);
		this.txtPage.TabIndex = 6;
		this.txtPage.Text = "1";
		this.txtPage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.toolTipMain.SetToolTip(this.txtPage, "Enter current page");
		this.txtPage.UseSelectable = true;
		this.txtPage.WaterMark = "Page";
		this.txtPage.WaterMarkColor = System.Drawing.Color.FromArgb(109, 109, 109);
		this.txtPage.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
		this.txtPage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtPage_KeyPress);
		this.txtPage.Leave += new System.EventHandler(txtPage_Leave);
		this.btnLastData.BackColor = System.Drawing.SystemColors.Control;
		this.btnLastData.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnLastData.Location = new System.Drawing.Point(178, 3);
		this.btnLastData.Margin = new System.Windows.Forms.Padding(0);
		this.btnLastData.Name = "btnLastData";
		this.btnLastData.Size = new System.Drawing.Size(30, 25);
		this.btnLastData.TabIndex = 10;
		this.btnLastData.Text = ">>";
		this.toolTipMain.SetToolTip(this.btnLastData, "Goto last page");
		this.btnLastData.UseVisualStyleBackColor = false;
		this.btnLastData.Click += new System.EventHandler(btnLastData_Click);
		this.btnNextData.BackColor = System.Drawing.SystemColors.Control;
		this.btnNextData.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnNextData.Location = new System.Drawing.Point(148, 3);
		this.btnNextData.Margin = new System.Windows.Forms.Padding(0);
		this.btnNextData.Name = "btnNextData";
		this.btnNextData.Size = new System.Drawing.Size(30, 25);
		this.btnNextData.TabIndex = 9;
		this.btnNextData.Text = ">";
		this.toolTipMain.SetToolTip(this.btnNextData, "Goto next page");
		this.btnNextData.UseVisualStyleBackColor = false;
		this.btnNextData.Click += new System.EventHandler(btnNextData_Click);
		this.btnPreData.BackColor = System.Drawing.SystemColors.Control;
		this.btnPreData.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnPreData.Location = new System.Drawing.Point(118, 3);
		this.btnPreData.Margin = new System.Windows.Forms.Padding(0);
		this.btnPreData.Name = "btnPreData";
		this.btnPreData.Size = new System.Drawing.Size(30, 25);
		this.btnPreData.TabIndex = 8;
		this.btnPreData.Text = "<";
		this.toolTipMain.SetToolTip(this.btnPreData, "Goto previous page");
		this.btnPreData.UseVisualStyleBackColor = false;
		this.btnPreData.Click += new System.EventHandler(btnPreData_Click);
		this.cbbLimit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cbbLimit.FormattingEnabled = true;
		this.cbbLimit.Items.AddRange(new object[19]
		{
			"50", "100", "150", "200", "250", "300", "350", "400", "450", "500",
			"550", "600", "650", "700", "750", "800", "850", "900", "950"
		});
		this.cbbLimit.Location = new System.Drawing.Point(1, 4);
		this.cbbLimit.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
		this.cbbLimit.MaxLength = 3;
		this.cbbLimit.Name = "cbbLimit";
		this.cbbLimit.Size = new System.Drawing.Size(50, 23);
		this.cbbLimit.TabIndex = 5;
		this.cbbLimit.Text = "50";
		this.toolTipMain.SetToolTip(this.cbbLimit, "Select or enter limit a page");
		this.cbbLimit.SelectedIndexChanged += new System.EventHandler(cbbLimit_SelectedIndexChanged);
		this.cbbLimit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(cbbLimit_KeyPress);
		this.cbbLimit.Leave += new System.EventHandler(cbbLimit_Leave);
		this.btnFirstData.BackColor = System.Drawing.SystemColors.Control;
		this.btnFirstData.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnFirstData.Location = new System.Drawing.Point(88, 3);
		this.btnFirstData.Margin = new System.Windows.Forms.Padding(0);
		this.btnFirstData.Name = "btnFirstData";
		this.btnFirstData.Size = new System.Drawing.Size(30, 25);
		this.btnFirstData.TabIndex = 7;
		this.btnFirstData.Text = "<<";
		this.toolTipMain.SetToolTip(this.btnFirstData, "Goto first page");
		this.btnFirstData.UseVisualStyleBackColor = false;
		this.btnFirstData.Click += new System.EventHandler(btnFirstData_Click);
		this.btnSearch.BackColor = System.Drawing.SystemColors.Control;
		this.btnSearch.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnSearch.Dock = System.Windows.Forms.DockStyle.Right;
		this.btnSearch.FlatAppearance.BorderSize = 0;
		this.btnSearch.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.AppWorkspace;
		this.btnSearch.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLight;
		this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnSearch.Image = (System.Drawing.Image)resources.GetObject("btnSearch.Image");
		this.btnSearch.Location = new System.Drawing.Point(238, 0);
		this.btnSearch.Margin = new System.Windows.Forms.Padding(0);
		this.btnSearch.Name = "btnSearch";
		this.btnSearch.Size = new System.Drawing.Size(25, 25);
		this.btnSearch.TabIndex = 71;
		this.btnSearch.TabStop = false;
		this.toolTipMain.SetToolTip(this.btnSearch, "Search");
		this.btnSearch.UseVisualStyleBackColor = false;
		this.btnSearch.Click += new System.EventHandler(btnSearch_Click);
		this.btnSearchDelete.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnSearchDelete.BackColor = System.Drawing.SystemColors.Window;
		this.btnSearchDelete.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnSearchDelete.FlatAppearance.BorderSize = 0;
		this.btnSearchDelete.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.AppWorkspace;
		this.btnSearchDelete.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
		this.btnSearchDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnSearchDelete.Image = _5S_QA_System.Properties.Resources.cancel;
		this.btnSearchDelete.Location = new System.Drawing.Point(214, 2);
		this.btnSearchDelete.Margin = new System.Windows.Forms.Padding(0);
		this.btnSearchDelete.Name = "btnSearchDelete";
		this.btnSearchDelete.Size = new System.Drawing.Size(21, 21);
		this.btnSearchDelete.TabIndex = 72;
		this.btnSearchDelete.TabStop = false;
		this.toolTipMain.SetToolTip(this.btnSearchDelete, "Clear text of search term");
		this.btnSearchDelete.UseVisualStyleBackColor = false;
		this.btnSearchDelete.Visible = false;
		this.btnSearchDelete.Click += new System.EventHandler(btnSearchDelete_Click);
		this.panelResize.Dock = System.Windows.Forms.DockStyle.Right;
		this.panelResize.Location = new System.Drawing.Point(271, 0);
		this.panelResize.Margin = new System.Windows.Forms.Padding(0);
		this.panelResize.Name = "panelResize";
		this.panelResize.Size = new System.Drawing.Size(3, 475);
		this.panelResize.TabIndex = 0;
		this.panelSearch.AutoSize = true;
		this.panelSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panelSearch.Controls.Add(this.tableLayoutPanel2);
		this.panelSearch.Controls.Add(this.panel1);
		this.panelSearch.Dock = System.Windows.Forms.DockStyle.Top;
		this.panelSearch.Location = new System.Drawing.Point(0, 0);
		this.panelSearch.Name = "panelSearch";
		this.panelSearch.Padding = new System.Windows.Forms.Padding(3);
		this.panelSearch.Size = new System.Drawing.Size(271, 61);
		this.panelSearch.TabIndex = 1;
		this.tableLayoutPanel2.AutoSize = true;
		this.tableLayoutPanel2.ColumnCount = 8;
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 55f));
		this.tableLayoutPanel2.Controls.Add(this.lblTotalRows, 7, 0);
		this.tableLayoutPanel2.Controls.Add(this.txtPage, 2, 0);
		this.tableLayoutPanel2.Controls.Add(this.btnLastData, 6, 0);
		this.tableLayoutPanel2.Controls.Add(this.btnNextData, 5, 0);
		this.tableLayoutPanel2.Controls.Add(this.btnPreData, 4, 0);
		this.tableLayoutPanel2.Controls.Add(this.cbbLimit, 1, 0);
		this.tableLayoutPanel2.Controls.Add(this.btnFirstData, 3, 0);
		this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 28);
		this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
		this.tableLayoutPanel2.Name = "tableLayoutPanel2";
		this.tableLayoutPanel2.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
		this.tableLayoutPanel2.RowCount = 1;
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanel2.Size = new System.Drawing.Size(263, 28);
		this.tableLayoutPanel2.TabIndex = 67;
		this.lblTotalRows.AutoSize = true;
		this.lblTotalRows.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTotalRows.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotalRows.Location = new System.Drawing.Point(211, 3);
		this.lblTotalRows.Name = "lblTotalRows";
		this.lblTotalRows.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
		this.lblTotalRows.Size = new System.Drawing.Size(49, 25);
		this.lblTotalRows.TabIndex = 70;
		this.lblTotalRows.Text = "0";
		this.lblTotalRows.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblTotalRows.TextChanged += new System.EventHandler(lblTotalRows_TextChanged);
		this.panel1.Controls.Add(this.btnSearchDelete);
		this.panel1.Controls.Add(this.txtSearch);
		this.panel1.Controls.Add(this.btnSearch);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(3, 3);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(263, 25);
		this.panel1.TabIndex = 68;
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
		this.dgvMain.Columns.AddRange(this.No, this.Code, this.name, this.GroupCodeName, this.ImageUrl, this.TotalMeas, this.Id);
		this.dgvMain.ContextMenuStrip = this.contextMenuStripMain;
		this.dgvMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.dgvMain.EnableHeadersVisualStyles = false;
		this.dgvMain.Location = new System.Drawing.Point(0, 61);
		this.dgvMain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.dgvMain.MultiSelect = false;
		this.dgvMain.Name = "dgvMain";
		this.dgvMain.ReadOnly = true;
		this.dgvMain.RowHeadersWidth = 25;
		this.dgvMain.Size = new System.Drawing.Size(271, 414);
		this.dgvMain.TabIndex = 3;
		this.dgvMain.Sorted += new System.EventHandler(dgvMain_Sorted);
		this.contextMenuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.main_refreshToolStripMenuItem });
		this.contextMenuStripMain.Name = "contextMenuStripStaff";
		this.contextMenuStripMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
		this.contextMenuStripMain.Size = new System.Drawing.Size(114, 26);
		this.main_refreshToolStripMenuItem.Name = "main_refreshToolStripMenuItem";
		this.main_refreshToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
		this.main_refreshToolStripMenuItem.Text = "Refresh";
		this.main_refreshToolStripMenuItem.Click += new System.EventHandler(main_refreshToolStripMenuItem_Click);
		this.No.DataPropertyName = "No";
		dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		this.No.DefaultCellStyle = dataGridViewCellStyle3;
		this.No.HeaderText = "No";
		this.No.Name = "No";
		this.No.ReadOnly = true;
		this.No.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.No.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.No.Width = 40;
		this.Code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Code.DataPropertyName = "Code";
		this.Code.FillWeight = 50f;
		this.Code.HeaderText = "Code";
		this.Code.Name = "Code";
		this.Code.ReadOnly = true;
		this.Code.Visible = false;
		this.name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.name.DataPropertyName = "Name";
		this.name.FillWeight = 50f;
		this.name.HeaderText = "Name";
		this.name.Name = "name";
		this.name.ReadOnly = true;
		this.GroupCodeName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.GroupCodeName.DataPropertyName = "GroupCodeName";
		this.GroupCodeName.FillWeight = 50f;
		this.GroupCodeName.HeaderText = "Product";
		this.GroupCodeName.Name = "GroupCodeName";
		this.GroupCodeName.ReadOnly = true;
		this.ImageUrl.DataPropertyName = "ImageUrl";
		this.ImageUrl.HeaderText = "Image";
		this.ImageUrl.Name = "ImageUrl";
		this.ImageUrl.ReadOnly = true;
		this.ImageUrl.Visible = false;
		this.TotalMeas.DataPropertyName = "TotalMeas";
		this.TotalMeas.HeaderText = "Meas. q.ty";
		this.TotalMeas.Name = "TotalMeas";
		this.TotalMeas.ReadOnly = true;
		this.TotalMeas.Visible = false;
		this.Id.DataPropertyName = "Id";
		this.Id.HeaderText = "Id";
		this.Id.Name = "Id";
		this.Id.ReadOnly = true;
		this.Id.Visible = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		base.Controls.Add(this.dgvMain);
		base.Controls.Add(this.panelSearch);
		base.Controls.Add(this.panelResize);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Name = "mPanelSub";
		base.Size = new System.Drawing.Size(274, 475);
		this.panelSearch.ResumeLayout(false);
		this.panelSearch.PerformLayout();
		this.tableLayoutPanel2.ResumeLayout(false);
		this.tableLayoutPanel2.PerformLayout();
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.dgvMain).EndInit();
		this.contextMenuStripMain.ResumeLayout(false);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
