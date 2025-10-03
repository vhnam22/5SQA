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

namespace _5S_QA_System;

public class mPanelConfirm : UserControl
{
	private DataGridView dgvRequest;

	private Guid IdRequest;

	private readonly DataTable mData;

	private int mRowMax;

	private IContainer components = null;

	private ToolTip toolTipMain;

	private TableLayoutPanel tableLayoutPanel1;

	private Label lblSelects;

	private Label lblSelect;

	private Label lblAcceptable;

	private Label lblTotal;

	private Label lblTotals;

	private DataGridView dgvMain;

	private Button btnConfirm;

	private ContextMenuStrip contextMenuStripMain;

	private ToolStripMenuItem main_allToolStripMenuItem;

	private ToolStripMenuItem main_unallToolStripMenuItem;

	private DataGridView dgvMeas;

	private DataGridViewCheckBoxColumn IsSelect;

	private DataGridViewTextBoxColumn No;

	private DataGridViewTextBoxColumn RequestName;

	private DataGridViewTextBoxColumn Sample;

	private DataGridViewTextBoxColumn PlanName;

	private DataGridViewTextBoxColumn OK;

	private DataGridViewTextBoxColumn NG;

	private DataGridViewTextBoxColumn Empty;

	private DataGridViewTextBoxColumn Status;

	private DataGridViewTextBoxColumn Id;

	private DataGridViewTextBoxColumn MNo;

	private DataGridViewTextBoxColumn MName;

	private DataGridViewTextBoxColumn Value;

	private DataGridViewTextBoxColumn Unit;

	private DataGridViewTextBoxColumn Upper;

	private DataGridViewTextBoxColumn Lower;

	private DataGridViewTextBoxColumn LSL;

	private DataGridViewTextBoxColumn USL;

	private DataGridViewTextBoxColumn Cavity;

	private DataGridViewTextBoxColumn Result;

	private DataGridViewTextBoxColumn Judge;

	private DataGridViewTextBoxColumn History;

	private DataGridViewTextBoxColumn MeasurementUnit;

	public mPanelConfirm(DataTable dt)
	{
		InitializeComponent();
		Common.setControls(this, toolTipMain, new List<ContextMenuStrip> { contextMenuStripMain });
		mData = dt;
	}

	private void mPanelConfirm_Load(object sender, EventArgs e)
	{
		dgvRequest = base.ParentForm.Controls["dgvMain"] as DataGridView;
		Guid.TryParse(dgvRequest.CurrentRow.Cells["Id"].Value.ToString(), out IdRequest);
		dgvMeas.Visible = false;
		Init();
	}

	private void Init()
	{
		load_dgvMain();
		base.Size = new Size(base.Width, 76 + ((mRowMax < 10) ? mRowMax : 10) * 22);
		base.Location = new Point((base.ParentForm.Width - base.Width) / 2, (base.ParentForm.Height - base.Height) / 2);
	}

	public void load_dgvMain()
	{
		dgvMain.DataSource = mData;
		dgvMain.Refresh();
		mRowMax = dgvMain.RowCount;
		lblTotals.Text = mRowMax.ToString();
		dgvMain_Sorted(dgvMain, null);
	}

	private void load_dgvMeas()
	{
		try
		{
			int.TryParse(dgvMain.CurrentRow.Cells["Sample"].Value.ToString(), out var result);
			string name = dgvMain.CurrentCell.OwningColumn.Name;
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "RequestId=@0 && Sample=@1 && Judge.Contains(@2) && !string.IsNullOrEmpty(Result)";
			queryArgs.PredicateParameters = new string[3]
			{
				IdRequest.ToString(),
				result.ToString(),
				name
			};
			queryArgs.Order = "Created";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs body = queryArgs;
			ResponseDto result2 = frmLogin.client.GetsAsync(body, "/api/RequestResult/Gets").Result;
			dgvMeas.DataSource = Common.getDataTableNoType<RequestResultQuickViewModel>(result2);
			dgvMeas.CurrentCell = null;
			dgvMeas.Refresh();
			dgvMeas_Sorted(dgvMeas, null);
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

	private void dgvMain_CurrentCellChanged(object sender, EventArgs e)
	{
		DataGridView dataGridView = sender as DataGridView;
		if (dataGridView.CurrentCell != null)
		{
			load_dgvMeas();
		}
	}

	private void dgvMain_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		dgvMeas.Visible = false;
	}

	private void dgvMain_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
	{
		DataGridView dataGridView = sender as DataGridView;
		if (dataGridView.CurrentCell == null)
		{
			dgvMeas.Visible = false;
		}
		else if (dataGridView.CurrentCell.OwningColumn.Name.Equals("NG"))
		{
			int.TryParse(dataGridView.CurrentCell.Value.ToString(), out var result);
			if (!result.Equals(0))
			{
				dgvMeas.Visible = true;
			}
		}
	}

	private void dgvMain_Sorted(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		DataGridView dataGridView = sender as DataGridView;
		foreach (DataGridViewRow item in (IEnumerable)dataGridView.Rows)
		{
			item.Cells["No"].Value = item.Index + 1;
			int.TryParse(item.Cells["NG"].Value.ToString(), out var result);
			if (mRowMax < result)
			{
				mRowMax = result;
			}
		}
	}

	private void main_allToolStripMenuItem_Click(object sender, EventArgs e)
	{
		foreach (DataRow row in mData.Rows)
		{
			row["IsSelect"] = true;
		}
		lblSelects.Text = lblTotals.Text;
	}

	private void main_unallToolStripMenuItem_Click(object sender, EventArgs e)
	{
		foreach (DataRow row in mData.Rows)
		{
			row["IsSelect"] = false;
		}
		lblSelects.Text = "0";
	}

	private void dgvMain_CurrentCellDirtyStateChanged(object sender, EventArgs e)
	{
		dgvMain.CurrentCellDirtyStateChanged -= dgvMain_CurrentCellDirtyStateChanged;
		DataGridView dataGridView = sender as DataGridView;
		if (dataGridView.CurrentCell is DataGridViewCheckBoxCell dataGridViewCheckBoxCell)
		{
			int num = int.Parse(lblSelects.Text);
			num = ((!(bool)dataGridViewCheckBoxCell.Value) ? (num + 1) : (num - 1));
			lblSelects.Text = num.ToString();
		}
		dataGridView.EndEdit();
		dgvMain.CurrentCellDirtyStateChanged += dgvMain_CurrentCellDirtyStateChanged;
	}

	private void dgvMeas_Sorted(object sender, EventArgs e)
	{
		DataGridView dataGridView = sender as DataGridView;
		foreach (DataGridViewRow item in (IEnumerable)dataGridView.Rows)
		{
			item.Cells["MNo"].Value = item.Index + 1;
			object value = item.Cells["Judge"].Value;
			object obj = value;
			switch (obj as string)
			{
			case "OK":
				item.DefaultCellStyle.ForeColor = Color.Blue;
				item.Cells["Result"].ToolTipText = "OK";
				break;
			case "OK+":
				item.DefaultCellStyle.ForeColor = Color.Blue;
				item.Cells["Result"].ToolTipText = "OK+";
				break;
			case "OK-":
				item.DefaultCellStyle.ForeColor = Color.Blue;
				item.Cells["Result"].ToolTipText = "OK-";
				break;
			case "NG":
				item.DefaultCellStyle.ForeColor = Color.Red;
				item.Cells["Result"].ToolTipText = "NG";
				break;
			case "NG+":
				item.DefaultCellStyle.ForeColor = Color.Red;
				item.Cells["Result"].ToolTipText = "NG+";
				break;
			case "NG-":
				item.DefaultCellStyle.ForeColor = Color.Red;
				item.Cells["Result"].ToolTipText = "NG-";
				break;
			}
			if (item.Cells["History"].Value != null && !string.IsNullOrEmpty(item.Cells["History"].Value.ToString()) && !item.Cells["History"].Value.ToString().Equals("[]") && !item.Cells["MName"].Value.ToString().StartsWith("* "))
			{
				item.Cells["MName"].Value = string.Format("* {0}", item.Cells["MName"].Value);
			}
			if (item.Cells["MeasurementUnit"].Value?.ToString() == "°")
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
				if (double.TryParse(item.Cells["LSL"].Value?.ToString(), out var result4))
				{
					item.Cells["LSL"].Value = Common.ConvertDoubleToDegrees(result4);
				}
				if (double.TryParse(item.Cells["USL"].Value?.ToString(), out var result5))
				{
					item.Cells["USL"].Value = Common.ConvertDoubleToDegrees(result5);
				}
				if (double.TryParse(item.Cells["Result"].Value?.ToString(), out var result6))
				{
					item.Cells["Result"].Value = Common.ConvertDoubleToDegrees(result6);
				}
			}
		}
	}

	private void btnConfirm_Click(object sender, EventArgs e)
	{
		try
		{
			if (!MessageBox.Show(Common.getTextLanguage(this, "wSureConfirm"), Common.getTextLanguage(this, "QUESTION"), MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.Yes))
			{
				return;
			}
			Cursor.Current = Cursors.WaitCursor;
			DataTable dataTable = dgvMain.DataSource as DataTable;
			List<Guid> list = new List<Guid>();
			foreach (DataRow row in dataTable.Rows)
			{
				if (bool.Parse(row["IsSelect"].ToString()))
				{
					Guid item = Guid.Parse(row["Id"].ToString());
					list.Add(item);
				}
			}
			ActiveRequestDto activeRequestDto = new ActiveRequestDto
			{
				Id = IdRequest,
				Judgement = ((list.Count > 0) ? "ACCEPTABLE" : dgvRequest.CurrentRow.Cells["Judgement"].Value.ToString())
			};
			if (dgvRequest.CurrentRow.Cells["Status"].Value.Equals("Completed"))
			{
				activeRequestDto.Status = "Checked";
			}
			else
			{
				activeRequestDto.Status = "Approved";
			}
			ResponseDto result = frmLogin.client.SaveAsync(activeRequestDto, "/api/Request/Active").Result;
			if (!result.Success)
			{
				throw new Exception(result.Messages.ElementAt(0).Message);
			}
			result = frmLogin.client.SaveAsync(list, "/api/RequestStatus/SaveListWithId").Result;
			if (!result.Success)
			{
				throw new Exception(result.Messages.ElementAt(0).Message);
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
		finally
		{
			frmRequestManagerView frmRequestManagerView2 = base.ParentForm as frmRequestManagerView;
			frmRequestManagerView2.main_refreshToolStripMenuItem_Click(frmRequestManagerView2, null);
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
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.btnConfirm = new System.Windows.Forms.Button();
		this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
		this.lblSelects = new System.Windows.Forms.Label();
		this.lblSelect = new System.Windows.Forms.Label();
		this.lblAcceptable = new System.Windows.Forms.Label();
		this.lblTotal = new System.Windows.Forms.Label();
		this.lblTotals = new System.Windows.Forms.Label();
		this.dgvMain = new System.Windows.Forms.DataGridView();
		this.IsSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
		this.No = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.RequestName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Sample = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.PlanName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.OK = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.NG = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Empty = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.contextMenuStripMain = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.main_allToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.main_unallToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.dgvMeas = new System.Windows.Forms.DataGridView();
		this.MNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.MName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Upper = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Lower = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.LSL = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.USL = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Cavity = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Result = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Judge = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.History = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.MeasurementUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.tableLayoutPanel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvMain).BeginInit();
		this.contextMenuStripMain.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvMeas).BeginInit();
		base.SuspendLayout();
		this.btnConfirm.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnConfirm.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.btnConfirm.FlatAppearance.BorderSize = 0;
		this.btnConfirm.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
		this.btnConfirm.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
		this.btnConfirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnConfirm.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnConfirm.Location = new System.Drawing.Point(0, 274);
		this.btnConfirm.Name = "btnConfirm";
		this.btnConfirm.Size = new System.Drawing.Size(650, 26);
		this.btnConfirm.TabIndex = 3;
		this.btnConfirm.Text = "Confirm";
		this.toolTipMain.SetToolTip(this.btnConfirm, "Confirm");
		this.btnConfirm.UseVisualStyleBackColor = true;
		this.btnConfirm.Click += new System.EventHandler(btnConfirm_Click);
		this.tableLayoutPanel1.AutoSize = true;
		this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
		this.tableLayoutPanel1.ColumnCount = 6;
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel1.Controls.Add(this.lblSelects, 3, 0);
		this.tableLayoutPanel1.Controls.Add(this.lblSelect, 2, 0);
		this.tableLayoutPanel1.Controls.Add(this.lblAcceptable, 0, 0);
		this.tableLayoutPanel1.Controls.Add(this.lblTotal, 4, 0);
		this.tableLayoutPanel1.Controls.Add(this.lblTotals, 5, 0);
		this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
		this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(1);
		this.tableLayoutPanel1.Name = "tableLayoutPanel1";
		this.tableLayoutPanel1.RowCount = 1;
		this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24f));
		this.tableLayoutPanel1.Size = new System.Drawing.Size(650, 24);
		this.tableLayoutPanel1.TabIndex = 1;
		this.lblSelects.AutoSize = true;
		this.lblSelects.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblSelects.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSelects.ForeColor = System.Drawing.Color.White;
		this.lblSelects.Location = new System.Drawing.Point(543, 1);
		this.lblSelects.Margin = new System.Windows.Forms.Padding(1);
		this.lblSelects.Name = "lblSelects";
		this.lblSelects.Size = new System.Drawing.Size(15, 22);
		this.lblSelects.TabIndex = 154;
		this.lblSelects.Text = "0";
		this.lblSelects.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblSelect.AutoSize = true;
		this.lblSelect.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSelect.ForeColor = System.Drawing.Color.White;
		this.lblSelect.Location = new System.Drawing.Point(493, 1);
		this.lblSelect.Margin = new System.Windows.Forms.Padding(1);
		this.lblSelect.Name = "lblSelect";
		this.lblSelect.Size = new System.Drawing.Size(48, 22);
		this.lblSelect.TabIndex = 153;
		this.lblSelect.Text = "Select:";
		this.lblSelect.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblAcceptable.AutoSize = true;
		this.lblAcceptable.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblAcceptable.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblAcceptable.ForeColor = System.Drawing.Color.White;
		this.lblAcceptable.Location = new System.Drawing.Point(1, 1);
		this.lblAcceptable.Margin = new System.Windows.Forms.Padding(1);
		this.lblAcceptable.Name = "lblAcceptable";
		this.lblAcceptable.Size = new System.Drawing.Size(141, 22);
		this.lblAcceptable.TabIndex = 146;
		this.lblAcceptable.Text = "Acceptable sample";
		this.lblAcceptable.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblTotal.AutoSize = true;
		this.lblTotal.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotal.ForeColor = System.Drawing.Color.White;
		this.lblTotal.Location = new System.Drawing.Point(560, 1);
		this.lblTotal.Margin = new System.Windows.Forms.Padding(1);
		this.lblTotal.Name = "lblTotal";
		this.lblTotal.Size = new System.Drawing.Size(72, 22);
		this.lblTotal.TabIndex = 149;
		this.lblTotal.Text = "Total rows:";
		this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblTotals.AutoSize = true;
		this.lblTotals.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTotals.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotals.ForeColor = System.Drawing.Color.White;
		this.lblTotals.Location = new System.Drawing.Point(634, 1);
		this.lblTotals.Margin = new System.Windows.Forms.Padding(1);
		this.lblTotals.Name = "lblTotals";
		this.lblTotals.Size = new System.Drawing.Size(15, 22);
		this.lblTotals.TabIndex = 152;
		this.lblTotals.Text = "0";
		this.lblTotals.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
		this.dgvMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
		this.dgvMain.Columns.AddRange(this.IsSelect, this.No, this.RequestName, this.Sample, this.PlanName, this.OK, this.NG, this.Empty, this.Status, this.Id);
		this.dgvMain.ContextMenuStrip = this.contextMenuStripMain;
		this.dgvMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.dgvMain.EnableHeadersVisualStyles = false;
		this.dgvMain.Location = new System.Drawing.Point(0, 24);
		this.dgvMain.Margin = new System.Windows.Forms.Padding(1);
		this.dgvMain.Name = "dgvMain";
		this.dgvMain.RowHeadersVisible = false;
		this.dgvMain.Size = new System.Drawing.Size(650, 250);
		this.dgvMain.TabIndex = 2;
		this.dgvMain.TabStop = false;
		this.dgvMain.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvMain_CellClick);
		this.dgvMain.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvMain_CellDoubleClick);
		this.dgvMain.CurrentCellChanged += new System.EventHandler(dgvMain_CurrentCellChanged);
		this.dgvMain.CurrentCellDirtyStateChanged += new System.EventHandler(dgvMain_CurrentCellDirtyStateChanged);
		this.dgvMain.Sorted += new System.EventHandler(dgvMain_Sorted);
		this.IsSelect.DataPropertyName = "IsSelect";
		this.IsSelect.FalseValue = "False";
		this.IsSelect.HeaderText = "";
		this.IsSelect.Name = "IsSelect";
		this.IsSelect.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		this.IsSelect.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
		this.IsSelect.TrueValue = "True";
		this.IsSelect.Width = 25;
		this.No.DataPropertyName = "No";
		dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		this.No.DefaultCellStyle = dataGridViewCellStyle3;
		this.No.HeaderText = "No";
		this.No.Name = "No";
		this.No.ReadOnly = true;
		this.No.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.No.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.No.Width = 35;
		this.RequestName.DataPropertyName = "RequestName";
		this.RequestName.HeaderText = "Request";
		this.RequestName.Name = "RequestName";
		this.RequestName.Visible = false;
		this.Sample.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Sample.DataPropertyName = "Sample";
		this.Sample.FillWeight = 20f;
		this.Sample.HeaderText = "Sample";
		this.Sample.Name = "Sample";
		this.Sample.ReadOnly = true;
		this.PlanName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.PlanName.DataPropertyName = "PlanName";
		this.PlanName.FillWeight = 20f;
		this.PlanName.HeaderText = "Plan";
		this.PlanName.Name = "PlanName";
		this.PlanName.ReadOnly = true;
		this.OK.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.OK.DataPropertyName = "OK";
		this.OK.FillWeight = 20f;
		this.OK.HeaderText = "OK";
		this.OK.Name = "OK";
		this.OK.ReadOnly = true;
		this.NG.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.NG.DataPropertyName = "NG";
		dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Red;
		this.NG.DefaultCellStyle = dataGridViewCellStyle4;
		this.NG.FillWeight = 20f;
		this.NG.HeaderText = "NG";
		this.NG.Name = "NG";
		this.NG.ReadOnly = true;
		this.Empty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Empty.DataPropertyName = "Empty";
		this.Empty.FillWeight = 20f;
		this.Empty.HeaderText = "Empty";
		this.Empty.Name = "Empty";
		this.Empty.ReadOnly = true;
		this.Status.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Status.DataPropertyName = "Status";
		dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Red;
		this.Status.DefaultCellStyle = dataGridViewCellStyle5;
		this.Status.FillWeight = 20f;
		this.Status.HeaderText = "Status";
		this.Status.Name = "Status";
		this.Status.ReadOnly = true;
		this.Id.DataPropertyName = "Id";
		dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.Id.DefaultCellStyle = dataGridViewCellStyle6;
		this.Id.HeaderText = "Id";
		this.Id.Name = "Id";
		this.Id.ReadOnly = true;
		this.Id.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.Id.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.Id.Visible = false;
		this.Id.Width = 140;
		this.contextMenuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.main_allToolStripMenuItem, this.main_unallToolStripMenuItem });
		this.contextMenuStripMain.Name = "contextMenuStripRequest";
		this.contextMenuStripMain.Size = new System.Drawing.Size(135, 48);
		this.main_allToolStripMenuItem.Name = "main_allToolStripMenuItem";
		this.main_allToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
		this.main_allToolStripMenuItem.Text = "Select all";
		this.main_allToolStripMenuItem.Click += new System.EventHandler(main_allToolStripMenuItem_Click);
		this.main_unallToolStripMenuItem.Name = "main_unallToolStripMenuItem";
		this.main_unallToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
		this.main_unallToolStripMenuItem.Text = "Unselect all";
		this.main_unallToolStripMenuItem.Click += new System.EventHandler(main_unallToolStripMenuItem_Click);
		this.dgvMeas.AllowUserToAddRows = false;
		this.dgvMeas.AllowUserToDeleteRows = false;
		this.dgvMeas.AllowUserToOrderColumns = true;
		this.dgvMeas.AllowUserToResizeRows = false;
		dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
		this.dgvMeas.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle7;
		this.dgvMeas.BackgroundColor = System.Drawing.SystemColors.Window;
		dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.AppWorkspace;
		dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
		dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
		dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
		dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.dgvMeas.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle8;
		this.dgvMeas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
		this.dgvMeas.Columns.AddRange(this.MNo, this.MName, this.Value, this.Unit, this.Upper, this.Lower, this.LSL, this.USL, this.Cavity, this.Result, this.Judge, this.History, this.MeasurementUnit);
		this.dgvMeas.Dock = System.Windows.Forms.DockStyle.Right;
		this.dgvMeas.EnableHeadersVisualStyles = false;
		this.dgvMeas.Location = new System.Drawing.Point(100, 24);
		this.dgvMeas.Margin = new System.Windows.Forms.Padding(1);
		this.dgvMeas.MultiSelect = false;
		this.dgvMeas.Name = "dgvMeas";
		this.dgvMeas.ReadOnly = true;
		this.dgvMeas.RowHeadersVisible = false;
		this.dgvMeas.RowHeadersWidth = 25;
		this.dgvMeas.Size = new System.Drawing.Size(550, 250);
		this.dgvMeas.TabIndex = 157;
		this.dgvMeas.Sorted += new System.EventHandler(dgvMeas_Sorted);
		this.MNo.DataPropertyName = "No";
		dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		this.MNo.DefaultCellStyle = dataGridViewCellStyle9;
		this.MNo.HeaderText = "No.";
		this.MNo.Name = "MNo";
		this.MNo.ReadOnly = true;
		this.MNo.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.MNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.MNo.Width = 40;
		this.MName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.MName.DataPropertyName = "Name";
		this.MName.FillWeight = 30f;
		this.MName.HeaderText = "Measurement";
		this.MName.Name = "MName";
		this.MName.ReadOnly = true;
		this.Value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Value.DataPropertyName = "Value";
		dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.Value.DefaultCellStyle = dataGridViewCellStyle10;
		this.Value.FillWeight = 20f;
		this.Value.HeaderText = "Value";
		this.Value.Name = "Value";
		this.Value.ReadOnly = true;
		this.Unit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Unit.DataPropertyName = "Unit";
		this.Unit.FillWeight = 10f;
		this.Unit.HeaderText = "Unit";
		this.Unit.Name = "Unit";
		this.Unit.ReadOnly = true;
		this.Upper.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Upper.DataPropertyName = "Upper";
		dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.Upper.DefaultCellStyle = dataGridViewCellStyle11;
		this.Upper.FillWeight = 20f;
		this.Upper.HeaderText = "Upper ";
		this.Upper.Name = "Upper";
		this.Upper.ReadOnly = true;
		this.Lower.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Lower.DataPropertyName = "Lower";
		dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.Lower.DefaultCellStyle = dataGridViewCellStyle12;
		this.Lower.FillWeight = 20f;
		this.Lower.HeaderText = "Lower";
		this.Lower.Name = "Lower";
		this.Lower.ReadOnly = true;
		this.LSL.DataPropertyName = "LSL";
		this.LSL.HeaderText = "LSL";
		this.LSL.Name = "LSL";
		this.LSL.ReadOnly = true;
		this.LSL.Visible = false;
		this.USL.DataPropertyName = "USL";
		this.USL.HeaderText = "USL";
		this.USL.Name = "USL";
		this.USL.ReadOnly = true;
		this.USL.Visible = false;
		this.Cavity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Cavity.DataPropertyName = "Cavity";
		this.Cavity.FillWeight = 15f;
		this.Cavity.HeaderText = "Cavity";
		this.Cavity.Name = "Cavity";
		this.Cavity.ReadOnly = true;
		this.Result.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Result.DataPropertyName = "Result";
		dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.Result.DefaultCellStyle = dataGridViewCellStyle13;
		this.Result.FillWeight = 20f;
		this.Result.HeaderText = "Result";
		this.Result.Name = "Result";
		this.Result.ReadOnly = true;
		this.Judge.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Judge.DataPropertyName = "Judge";
		this.Judge.FillWeight = 10f;
		this.Judge.HeaderText = "Jud.";
		this.Judge.Name = "Judge";
		this.Judge.ReadOnly = true;
		this.History.DataPropertyName = "History";
		this.History.HeaderText = "History";
		this.History.Name = "History";
		this.History.ReadOnly = true;
		this.History.Visible = false;
		this.MeasurementUnit.DataPropertyName = "MeasurementUnit";
		this.MeasurementUnit.HeaderText = "Meas. unit";
		this.MeasurementUnit.Name = "MeasurementUnit";
		this.MeasurementUnit.ReadOnly = true;
		this.MeasurementUnit.Visible = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		base.Controls.Add(this.dgvMeas);
		base.Controls.Add(this.dgvMain);
		base.Controls.Add(this.btnConfirm);
		base.Controls.Add(this.tableLayoutPanel1);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "mPanelConfirm";
		base.Size = new System.Drawing.Size(650, 300);
		base.Load += new System.EventHandler(mPanelConfirm_Load);
		this.tableLayoutPanel1.ResumeLayout(false);
		this.tableLayoutPanel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvMain).EndInit();
		this.contextMenuStripMain.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.dgvMeas).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
