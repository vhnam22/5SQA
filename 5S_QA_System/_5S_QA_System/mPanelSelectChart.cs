using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using _5S_QA_DL.APIContext;
using _5S_QA_Entities.Dtos;
using _5S_QA_System.Controls;

namespace _5S_QA_System;

public class mPanelSelectChart : UserControl
{
	private mSearchChart mSearchMain;

	private IContainer components = null;

	private TableLayoutPanel tableLayoutPanelMain;

	private ComboBox cbbChart;

	private ToolTip toolTipMain;

	private Button btnView;

	public mPanelSelectChart()
	{
		InitializeComponent();
	}

	private void mPanelSelectChart_Load(object sender, EventArgs e)
	{
		mSearchMain = base.ParentForm.Controls["mSearchMain"] as mSearchChart;
		Init();
	}

	private void Init()
	{
		base.Location = base.ParentForm.PointToClient(Cursor.Position);
		load_cbbChart();
		Guid idGroup = getIdGroup(mSearchMain.cbbMeas.SelectedValue);
		if (idGroup != default(Guid) && cbbChart.Items.Count > 0)
		{
			cbbChart.SelectedValue = idGroup;
		}
	}

	private void load_cbbChart()
	{
		try
		{
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "type=@0";
			queryArgs.PredicateParameters = new string[1] { "Chart" };
			queryArgs.Order = "Created DESC";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs body = queryArgs;
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/Template/Gets").Result;
			DataTable dataTable = Common.getDataTable(result);
			if (dataTable == null)
			{
				dataTable = new DataTable();
				dataTable.Columns.Add("id");
				dataTable.Columns.Add("name");
				cbbChart.SelectedIndex = -1;
			}
			dataTable.Dispose();
			cbbChart.DataSource = dataTable;
			cbbChart.ValueMember = "id";
			cbbChart.DisplayMember = "name";
			cbbChart.Refresh();
		}
		catch (Exception ex)
		{
			cbbChart.DataSource = null;
			cbbChart.Refresh();
			if (ex.InnerException is ApiException ex2)
			{
				if (ex2.StatusCode == 401)
				{
					DialogResult dialogResult = MessageBox.Show("This account is already login elsewhere.\r\nPlease login again.", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					if (dialogResult == DialogResult.OK)
					{
						base.ParentForm.Close();
					}
				}
				else
				{
					MessageBox.Show(ex2.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			else
			{
				MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
	}

	private Guid getIdGroup(object measid)
	{
		Guid result = default(Guid);
		try
		{
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "Id=@0";
			queryArgs.PredicateParameters = new string[1] { measid.ToString() };
			queryArgs.Order = "Created DESC";
			queryArgs.Page = 1;
			queryArgs.Limit = 1;
			QueryArgs body = queryArgs;
			ResponseDto result2 = frmLogin.client.GetsAsync(body, "/api/Measurement/Gets").Result;
			DataTable dataTable = Common.getDataTable(result2);
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				Guid.TryParse(dataTable.Rows[0]["productgroupid"].ToString(), out result);
			}
		}
		catch
		{
		}
		return result;
	}

	private bool ByteArrayToFile(string fileName, byte[] byteArray)
	{
		try
		{
			using FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
			fileStream.Write(byteArray, 0, byteArray.Length);
			return true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			return false;
		}
	}

	private void btnView_Click(object sender, EventArgs e)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Expected O, but got Unknown
		Cursor.Current = Cursors.WaitCursor;
		if (string.IsNullOrEmpty(cbbChart.Text))
		{
			MessageBox.Show("Please select template chart.", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			cbbChart.Focus();
			return;
		}
		try
		{
			ChartDto val = new ChartDto
			{
				ProductId = Guid.Parse(mSearchMain.cbbStage.SelectedValue.ToString()),
				StartDate = DateTime.Parse(mSearchMain.dtpDateFrom.Value.ToShortDateString()),
				EndDate = DateTime.Parse(mSearchMain.dtpDateTo.Value.ToShortDateString()),
				MeasurementId = Guid.Parse(mSearchMain.cbbMeas.SelectedValue.ToString()),
				ProductGroupId = Guid.Parse(cbbChart.SelectedValue.ToString())
			};
			string text = "/api/Template/ExportExcel";
			ExportExcelDto result = frmLogin.client.ExportAsync((object)val, text).Result;
			if (result == null)
			{
				throw new Exception("Haven't file template");
			}
			string text2 = Path.Combine("C:\\Windows\\Temp", "5S_QA", "VIEW");
			if (!Directory.Exists(text2))
			{
				Directory.CreateDirectory(text2);
			}
			string text3 = Path.Combine(text2, result.FileName.Replace("\"", ""));
			if (File.Exists(text3))
			{
				File.Delete(text3);
			}
			Cursor.Current = Cursors.WaitCursor;
			if (ByteArrayToFile(text3, result.Value))
			{
				WebBrowser webBrowser = new WebBrowser();
				webBrowser.Navigate(text3, newWindow: false);
			}
		}
		catch (Exception ex)
		{
			if (ex.InnerException is ApiException ex2)
			{
				if (ex2.StatusCode == 401)
				{
					DialogResult dialogResult = MessageBox.Show("This account is already login elsewhere.\r\nPlease login again.", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					if (dialogResult == DialogResult.OK)
					{
						base.ParentForm.Close();
					}
				}
				else
				{
					MessageBox.Show(ex2.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			else
			{
				MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
		this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
		this.cbbChart = new System.Windows.Forms.ComboBox();
		this.btnView = new System.Windows.Forms.Button();
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.tableLayoutPanelMain.SuspendLayout();
		base.SuspendLayout();
		this.tableLayoutPanelMain.AutoSize = true;
		this.tableLayoutPanelMain.ColumnCount = 1;
		this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanelMain.Controls.Add(this.cbbChart, 0, 0);
		this.tableLayoutPanelMain.Controls.Add(this.btnView, 0, 1);
		this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
		this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
		this.tableLayoutPanelMain.RowCount = 2;
		this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tableLayoutPanelMain.Size = new System.Drawing.Size(306, 63);
		this.tableLayoutPanelMain.TabIndex = 0;
		this.cbbChart.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbbChart.Dock = System.Windows.Forms.DockStyle.Fill;
		this.cbbChart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbbChart.FormattingEnabled = true;
		this.cbbChart.Location = new System.Drawing.Point(3, 3);
		this.cbbChart.Name = "cbbChart";
		this.cbbChart.Size = new System.Drawing.Size(300, 21);
		this.cbbChart.TabIndex = 0;
		this.toolTipMain.SetToolTip(this.cbbChart, "Please select template chart");
		this.btnView.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnView.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnView.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnView.Location = new System.Drawing.Point(3, 30);
		this.btnView.Name = "btnView";
		this.btnView.Size = new System.Drawing.Size(300, 30);
		this.btnView.TabIndex = 1;
		this.btnView.Text = "View chart";
		this.toolTipMain.SetToolTip(this.btnView, "Select view file chart");
		this.btnView.UseVisualStyleBackColor = true;
		this.btnView.Click += new System.EventHandler(btnView_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoSize = true;
		base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		base.Controls.Add(this.tableLayoutPanelMain);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Name = "mPanelSelectChart";
		base.Size = new System.Drawing.Size(306, 63);
		base.Load += new System.EventHandler(mPanelSelectChart_Load);
		this.tableLayoutPanelMain.ResumeLayout(false);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
