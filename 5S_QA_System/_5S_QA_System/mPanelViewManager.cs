using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using _5S_QA_DL.APIContext;
using _5S_QA_Entities.Constants;
using _5S_QA_Entities.Dtos;
using _5S_QA_Entities.Models;
using _5S_QA_System.Controls;
using _5S_QA_System.Properties;
using MetroFramework;
using MetroFramework.Controls;

namespace _5S_QA_System;

public class mPanelViewManager : UserControl
{
	private DataGridView dgvMain;

	private Bitmap bitmap;

	private bool isNullBitmap;

	private int mRowMax;

	private Guid mId;

	private IContainer components = null;

	private MetroTabPage tbpInformation;

	private DataGridView dgvFooter;

	private DataGridViewTextBoxColumn footer_title;

	private DataGridViewTextBoxColumn footer_value;

	private DataGridView dgvOther;

	private DataGridViewTextBoxColumn other_title;

	private DataGridViewImageColumn other_value;

	private DataGridView dgvContent;

	private DataGridViewTextBoxColumn content_title;

	private DataGridViewTextBoxColumn content_value;

	private Label lblTitleProgress;

	private MetroTabPage tbpStatistic;

	private MetroTabControl tbcView;

	private ToolTip toolTipMain;

	private Button btnDown;

	private Button btnUp;

	public Button btnHistory;

	public Button btnExport;

	private TableLayoutPanel tblPanelTitle;

	private Label lblValue;

	private Label lblTitle;

	private Panel panelFooter;

	private Panel panelTitle;

	private TableLayoutPanel tblPanelFooter;

	private Panel panelResize;

	private DataGridView dgvComment;

	private Label lblComment;

	private Label txtTotalRow;

	private Label lblCommentTotal;

	private TableLayoutPanel tableLayoutPanel1;

	private Panel panelView;

	private Chart chartProcess;

	private Chart chartMain;

	private Panel panelSample;

	private DataGridView dgvSample;

	private TableLayoutPanel tableLayoutPanel2;

	private Label lblStatus;

	private Label lblTotalRow;

	private Label lblSampleTotal;

	private DataGridView dgvMeas;

	private DataGridView dgvRequestDetail;

	private DataGridView dgvRequestStatus;

	private DataGridViewTextBoxColumn StatusCompletedBy;

	private DataGridViewTextBoxColumn StatusCompleted;

	private DataGridViewTextBoxColumn StatusCheckedBy;

	private DataGridViewTextBoxColumn StatusChecked;

	private DataGridViewTextBoxColumn StatusApprovedBy;

	private DataGridViewTextBoxColumn StatusApproved;

	private DataGridViewTextBoxColumn NoDetail;

	private DataGridViewTextBoxColumn CavityDetail;

	private DataGridViewTextBoxColumn PinDate;

	private DataGridViewTextBoxColumn ShotMold;

	private DataGridViewTextBoxColumn ProduceNo;

	private DataGridViewTextBoxColumn IdDetail;

	private DataGridViewTextBoxColumn No;

	private DataGridViewTextBoxColumn Sample;

	private DataGridViewTextBoxColumn PlanId;

	private DataGridViewTextBoxColumn PlanName;

	private DataGridViewTextBoxColumn OK;

	private DataGridViewTextBoxColumn NG;

	private DataGridViewTextBoxColumn Empty;

	private DataGridViewTextBoxColumn Status;

	private DataGridViewTextBoxColumn CompletedBy;

	private DataGridViewTextBoxColumn Completed;

	private DataGridViewTextBoxColumn CheckedBy;

	private DataGridViewTextBoxColumn Checked;

	private DataGridViewTextBoxColumn ApprovedBy;

	private DataGridViewTextBoxColumn Approved;

	private DataGridViewTextBoxColumn cNo;

	private DataGridViewTextBoxColumn Content;

	private DataGridViewTextBoxColumn Link;

	private DataGridViewTextBoxColumn Id;

	private ContextMenuStrip contextMenuStripMain;

	private ToolStripMenuItem main_fileToolStripMenuItem;

	private DataGridView dgvCpk;

	private TableLayoutPanel tpanelCpk;

	private Label lblCpk;

	private Label lblCpkTitleTotal;

	private Label lblCpkTotal;

	private DataGridViewTextBoxColumn cpkNo;

	private DataGridViewTextBoxColumn cpkName;

	private DataGridViewTextBoxColumn cpkCpk;

	private DataGridViewTextBoxColumn cpkRank;

	private DataGridViewTextBoxColumn cpkId;

	private DataGridView dgvRank;

	private TableLayoutPanel tableLayoutPanel3;

	private Label lblRank;

	private Label lblRankTotalRow;

	private Label lblRankTotal;

	private DataGridViewTextBoxColumn rNo;

	private DataGridViewTextBoxColumn MeasurementName;

	private DataGridViewTextBoxColumn Rank;

	private DataGridViewTextBoxColumn Percentage;

	private DataGridViewTextBoxColumn rSample;

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

	public mPanelViewManager()
	{
		InitializeComponent();
		Common.setControls(this, toolTipMain, new List<ContextMenuStrip> { contextMenuStripMain });
		foreach (TabPage tabPage in tbcView.TabPages)
		{
			tabPage.Text = Common.getTextLanguage(this, tabPage.Name);
		}
	}

	private void mPanelViewManager_Load(object sender, EventArgs e)
	{
		Init();
	}

	private void Init()
	{
		ControlResize.Init(panelResize, this, ControlResize.Direction.Horizontal, Cursors.SizeWE);
		dgvMain = base.Parent.Controls["dgvMain"] as DataGridView;
		dgvMeas.Visible = false;
	}

	private void load_dgvComment()
	{
		try
		{
			Guid guid = ((dgvMain.CurrentRow.Cells["Id"].Value == null) ? Guid.Empty : Guid.Parse(dgvMain.CurrentRow.Cells["Id"].Value.ToString()));
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "RequestId=@0";
			queryArgs.PredicateParameters = new string[1] { guid.ToString() };
			queryArgs.Order = "Created";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs body = queryArgs;
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/Comment/Gets").Result;
			dgvComment.DataSource = Common.getDataTable<CommentQuickViewModel>(result);
			dgvComment.Refresh();
			lblCommentTotal.Text = dgvComment.Rows.Count.ToString();
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

	private void load_ChartProcess()
	{
		chartProcess.Series["OK"].Points.Clear();
		chartProcess.Series["NG"].Points.Clear();
		chartProcess.Series["Empty"].Points.Clear();
		DataTable dataTable = load_Process();
		foreach (DataRow row in dataTable.Rows)
		{
			string xValue = string.Format("{0}{1}", row.Field<int?>("Sample"), string.IsNullOrEmpty(row.Field<string>("PlanName")) ? string.Empty : ("_" + row.Field<string>("PlanName")));
			chartProcess.Series["OK"].Points.AddXY(xValue, row.Field<int?>("OK"));
			chartProcess.Series["NG"].Points.AddXY(xValue, row.Field<int?>("NG"));
			chartProcess.Series["Empty"].Points.AddXY(xValue, row.Field<int?>("Empty"));
		}
		switch (dgvMain.CurrentRow.Cells["Status"].Value.ToString())
		{
		case "Rejected":
			add_StatusCharProcess("Rejected", 0, 1, 0);
			add_StatusCharProcess("Completed", 0, 0, 0);
			add_StatusCharProcess("Checked", 0, 0, 0);
			add_StatusCharProcess("Approved", 0, 0, 0);
			break;
		case "Completed":
			add_StatusCharProcess("Rejected", 0, 0, 0);
			add_StatusCharProcess("Completed", 1, 0, 0);
			add_StatusCharProcess("Checked", 0, 0, 0);
			add_StatusCharProcess("Approved", 0, 0, 0);
			break;
		case "Checked":
			add_StatusCharProcess("Rejected", 0, 0, 0);
			add_StatusCharProcess("Completed", 1, 0, 0);
			add_StatusCharProcess("Checked", 1, 0, 0);
			add_StatusCharProcess("Approved", 0, 0, 0);
			break;
		case "Approved":
			add_StatusCharProcess("Rejected", 0, 0, 0);
			add_StatusCharProcess("Completed", 1, 0, 0);
			add_StatusCharProcess("Checked", 1, 0, 0);
			add_StatusCharProcess("Approved", 1, 0, 0);
			break;
		default:
			add_StatusCharProcess("Rejected", 0, 0, 0);
			add_StatusCharProcess("Completed", 0, 0, 0);
			add_StatusCharProcess("Checked", 0, 0, 0);
			add_StatusCharProcess("Approved", 0, 0, 0);
			break;
		}
	}

	private void add_StatusCharProcess(string name, int ok, int ng, int empty)
	{
		chartProcess.Series["OK"].Points.AddXY(name, ok);
		chartProcess.Series["NG"].Points.AddXY(name, ng);
		chartProcess.Series["Empty"].Points.AddXY(name, empty);
	}

	private void load_ChartPie()
	{
		chartMain.Series["SeriesMain"].Points.Clear();
		int index = chartMain.Series["SeriesMain"].Points.AddXY("NG", dgvSample.CurrentRow.Cells["NG"].Value);
		int index2 = chartMain.Series["SeriesMain"].Points.AddXY("OK", dgvSample.CurrentRow.Cells["OK"].Value);
		int index3 = chartMain.Series["SeriesMain"].Points.AddXY("Empty", dgvSample.CurrentRow.Cells["Empty"].Value);
		chartMain.Series["SeriesMain"].Points[index].Color = Color.Red;
		chartMain.Series["SeriesMain"].Points[index2].Color = Color.Blue;
		chartMain.Series["SeriesMain"].Points[index3].Color = Color.Gray;
		foreach (DataPoint point in chartMain.Series["SeriesMain"].Points)
		{
			point.Label = "#VALX #PERCENT";
		}
	}

	public void Display()
	{
		try
		{
			lblValue.Text = dgvMain.CurrentRow.Cells["Id"].Value.ToString();
			dgvContent.Rows.Clear();
			dgvOther.Rows.Clear();
			dgvFooter.Rows.Clear();
			foreach (DataGridViewColumn col in dgvMain.Columns)
			{
				if (MetaType.dgvContent.Find((string x) => x.Equals(col.Name)) != null)
				{
					dgvContent.Visible = true;
					dgvContent.Rows.Add(col.HeaderText, dgvMain.CurrentRow.Cells[col.Name].Value);
				}
				else if (MetaType.dgvOther.Find((string x) => x.Equals(col.Name)) != null)
				{
					dgvOther.Visible = true;
					try
					{
						string value = dgvMain.CurrentRow.Cells[col.Name].Value.ToString();
						StringBuilder stringBuilder = new StringBuilder();
						stringBuilder.Append(APIUrl.APIHost + "/ProductImage/").Append(value);
						PictureBox pictureBox = new PictureBox();
						pictureBox.Load(stringBuilder.ToString());
						bitmap = (Bitmap)pictureBox.Image;
						pictureBox.Dispose();
						isNullBitmap = false;
					}
					catch
					{
						bitmap = Resources._5S_QA_S;
						isNullBitmap = true;
					}
					dgvOther.Rows.Add(col.HeaderText, bitmap);
				}
				else if (MetaType.dgvFooter.Find((string x) => x.Equals(col.Name)) != null)
				{
					dgvFooter.Visible = true;
					dgvFooter.Rows.Add(col.HeaderText, dgvMain.CurrentRow.Cells[col.Name].Value);
				}
			}
		}
		finally
		{
			load_ChartProcess();
			load_dgvComment();
			load_dgvMeasCpk();
			load_dgvRank();
			dgvContent.Size = new Size(panelView.Width, dgvContent.Rows.Count * 22 + 3);
			dgvOther.Size = new Size(panelView.Width, dgvOther.Rows.Count * 100 + 3);
			dgvFooter.Size = new Size(panelView.Width, dgvFooter.Rows.Count * 22 + 3);
			panelSample.Size = new Size(panelView.Width, 22 + ((mRowMax < 100) ? mRowMax : 100) * 22 + 3);
			dgvComment.Size = new Size(panelView.Width, 22 + dgvComment.Rows.Count * 22 + 3);
			dgvCpk.Size = new Size(panelView.Width, 22 + dgvCpk.Rows.Count * 22 + 3);
			dgvRank.Size = new Size(panelView.Width, 22 + dgvRank.Rows.Count * 22 + 3);
			tbcView_SelectedIndexChanged(tbcView, null);
			dgvContent.CurrentCell = null;
			dgvContent.Refresh();
			dgvOther.CurrentCell = null;
			dgvOther.Refresh();
			dgvFooter.CurrentCell = null;
			dgvFooter.Refresh();
			dgvCpk.CurrentCell = null;
			dgvCpk.Refresh();
			dgvSample.Refresh();
			dgvComment.CurrentCell = null;
			dgvComment.Refresh();
			dgvRank.CurrentCell = null;
			dgvRank.Refresh();
			dgvFooter.SendToBack();
			dgvOther.SendToBack();
			dgvContent.SendToBack();
		}
	}

	public void mDispose()
	{
		List<Type> list = new List<Type>();
		list.Add(typeof(frmZoomView));
		Common.closeForm(list);
	}

	private DataTable load_Process()
	{
		DataTable dataTable = new DataTable();
		try
		{
			Guid id = ((dgvMain.CurrentRow.Cells["Id"].Value == null) ? Guid.Empty : Guid.Parse(dgvMain.CurrentRow.Cells["Id"].Value.ToString()));
			ResponseDto result = frmLogin.client.GetsAsync(id, "/api/RequestStatus/Gets/{id}").Result;
			dataTable = Common.getDataTable<RequestStatusViewModel>(result);
			dgvMeas.Visible = false;
			load_dgvSample(dataTable);
			dgvSample_Sorted(dgvSample, null);
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
		return dataTable;
	}

	private void load_dgvSample(DataTable dt)
	{
		dgvSample.DataSource = dt;
		mRowMax = dgvSample.Rows.Count;
		lblSampleTotal.Text = mRowMax.ToString();
	}

	private void load_dgvMeas()
	{
		try
		{
			Guid guid = ((dgvMain.CurrentRow.Cells["Id"].Value == null) ? Guid.Empty : Guid.Parse(dgvMain.CurrentRow.Cells["Id"].Value.ToString()));
			int.TryParse(dgvSample.CurrentRow.Cells["Sample"].Value.ToString(), out var result);
			string name = dgvSample.CurrentCell.OwningColumn.Name;
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "RequestId=@0 && Sample=@1 && Judge.Contains(@2) && !string.IsNullOrEmpty(Result)";
			queryArgs.PredicateParameters = new string[3]
			{
				guid.ToString(),
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

	private void load_dgvRequestStatus()
	{
		dgvRequestStatus.Rows.Clear();
		dgvRequestStatus.Rows.Add();
		dgvRequestStatus.Rows[0].Cells["StatusCompletedBy"].Value = dgvSample.CurrentRow.Cells["CompletedBy"].Value;
		dgvRequestStatus.Rows[0].Cells["StatusCompleted"].Value = dgvSample.CurrentRow.Cells["Completed"].Value;
		dgvRequestStatus.Rows[0].Cells["StatusCheckedBy"].Value = dgvSample.CurrentRow.Cells["CheckedBy"].Value;
		dgvRequestStatus.Rows[0].Cells["StatusChecked"].Value = dgvSample.CurrentRow.Cells["Checked"].Value;
		dgvRequestStatus.Rows[0].Cells["StatusApprovedBy"].Value = dgvSample.CurrentRow.Cells["ApprovedBy"].Value;
		dgvRequestStatus.Rows[0].Cells["StatusApproved"].Value = dgvSample.CurrentRow.Cells["Approved"].Value;
		dgvRequestStatus.Refresh();
		dgvRequestStatus.CurrentCell = null;
	}

	private void load_dgvMeasCpk()
	{
		try
		{
			Guid id = ((dgvMain.CurrentRow.Cells["Id"].Value == null) ? Guid.Empty : Guid.Parse(dgvMain.CurrentRow.Cells["Id"].Value.ToString()));
			ResponseDto result = frmLogin.client.GetsAsync(id, "/api/Request/GetCpks/{id}").Result;
			dgvCpk.DataSource = Common.getDataTable<CpkViewModel>(result);
			dgvCpk.CurrentCell = null;
			dgvCpk.Refresh();
			lblCpkTotal.Text = dgvCpk.Rows.Count.ToString();
			dgvCpk_Sorted(dgvCpk, null);
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

	private void load_dgvRank()
	{
		try
		{
			Guid guid = ((dgvMain.CurrentRow.Cells["Id"].Value == null) ? Guid.Empty : Guid.Parse(dgvMain.CurrentRow.Cells["Id"].Value.ToString()));
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "RequestId=@0";
			queryArgs.PredicateParameters = new string[1] { guid.ToString() };
			queryArgs.Order = "Created";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs body = queryArgs;
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/ResultRank/Gets").Result;
			dgvRank.DataSource = Common.getDataTable<ResultRankQuickViewModel>(result);
			dgvRank.CurrentCell = null;
			dgvRank.Refresh();
			lblRankTotal.Text = dgvRank.Rows.Count.ToString();
			dgvRank_Sorted(dgvRank, null);
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

	private void btnUp_Click(object sender, EventArgs e)
	{
		dgvMain.Select();
		SendKeys.SendWait("{up}");
	}

	private void btnDown_Click(object sender, EventArgs e)
	{
		dgvMain.Select();
		SendKeys.SendWait("{down}");
	}

	private void dgvOther_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
	{
		if (!isNullBitmap && !Common.activeForm(typeof(frmZoomView)))
		{
			frmZoomView frmZoomView2 = new frmZoomView(bitmap, lblValue.Text);
			frmZoomView2.Show();
		}
	}

	private void tbcView_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tbcView.SelectedIndex.Equals(1))
		{
			tbcView.Size = new Size(panelView.Width, dgvContent.Size.Height + dgvOther.Size.Height + dgvFooter.Size.Height + 42);
		}
		else
		{
			tbcView.Size = new Size(panelView.Width, 150);
		}
	}

	private void dgvSample_Sorted(object sender, EventArgs e)
	{
		System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
		DataGridView dataGridView = sender as DataGridView;
		foreach (DataGridViewRow item in (IEnumerable)dataGridView.Rows)
		{
			item.Cells["No"].Value = item.Index + 1;
			int.TryParse(item.Cells["NG"].Value.ToString(), out var result);
			if (mRowMax < result)
			{
				mRowMax = result;
			}
			if (result > 0)
			{
				item.Cells["NG"].Style.ForeColor = Color.Red;
			}
			if (item.Cells["Status"].Value != null)
			{
				if (item.Cells["Status"].Value.ToString() == "NG")
				{
					item.Cells["Status"].Style.ForeColor = Color.Red;
				}
				else if (item.Cells["Status"].Value.ToString() == "ACCEPTABLE")
				{
					item.Cells["Status"].Style.ForeColor = Color.Green;
				}
				else if (item.Cells["Status"].Value.ToString() == "Rejected")
				{
					item.Cells["Status"].Style.ForeColor = Color.DarkRed;
				}
				else
				{
					item.Cells["Status"].Style.ForeColor = Color.Blue;
				}
			}
		}
	}

	private void dgvSample_CurrentCellChanged(object sender, EventArgs e)
	{
		DataGridView dataGridView = sender as DataGridView;
		if (dataGridView.CurrentCell != null)
		{
			load_ChartPie();
			load_dgvMeas();
			load_dgvRequestStatus();
		}
	}

	private void dgvSample_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		dgvMeas.Visible = false;
	}

	private void dgvSample_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
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
				item.Cells["Result"].Style.ForeColor = Color.Blue;
				item.Cells["Judge"].Style.ForeColor = Color.Blue;
				break;
			case "OK+":
				item.Cells["Result"].Style.ForeColor = Color.Blue;
				item.Cells["Judge"].Style.ForeColor = Color.Blue;
				break;
			case "OK-":
				item.Cells["Result"].Style.ForeColor = Color.Blue;
				item.Cells["Judge"].Style.ForeColor = Color.Blue;
				break;
			case "NG":
				item.Cells["Result"].Style.ForeColor = Color.Red;
				item.Cells["Judge"].Style.ForeColor = Color.Red;
				break;
			case "NG+":
				item.Cells["Result"].Style.ForeColor = Color.Red;
				item.Cells["Judge"].Style.ForeColor = Color.Red;
				break;
			case "NG-":
				item.Cells["Result"].Style.ForeColor = Color.Red;
				item.Cells["Judge"].Style.ForeColor = Color.Red;
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

	private void dgvComment_CurrentCellChanged(object sender, EventArgs e)
	{
		DataGridView dataGridView = sender as DataGridView;
		if (dataGridView.CurrentCell != null)
		{
			main_fileToolStripMenuItem.Visible = !string.IsNullOrEmpty(dataGridView.CurrentRow.Cells["Link"].Value.ToString());
			Guid guid = Guid.Parse(dataGridView.CurrentRow.Cells["Id"].Value.ToString());
			if (!guid.Equals(mId))
			{
				mId = guid;
			}
		}
		else
		{
			main_fileToolStripMenuItem.Visible = false;
			mId = Guid.Empty;
		}
	}

	private void main_fileToolStripMenuItem_Click(object sender, EventArgs e)
	{
		if (mId.Equals(Guid.Empty))
		{
			MessageBox.Show(Common.getTextLanguage(this, "wSelectRow"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		if (string.IsNullOrEmpty(dgvComment.CurrentRow.Cells["Link"].Value.ToString()))
		{
			MessageBox.Show(Common.getTextLanguage(this, "wNoFile"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		try
		{
			ExportExcelDto exportExcelDto = frmLogin.client.DownloadAsync(mId, "/api/Comment/DownloadFile/{id}").Result ?? throw new Exception(Common.getTextLanguage(this, "wHasntFile"));
			string path = exportExcelDto.FileName.Replace("\"", "");
			string text = Path.Combine("C:\\Windows\\Temp\\5SQA_System", "VIEWS");
			Directory.CreateDirectory(text);
			path = Path.Combine(text, path);
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
			if (Common.ByteArrayToFile(path, exportExcelDto.Value))
			{
				Common.ExecuteBatFile(path);
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
	}

	private void dgvCpk_Sorted(object sender, EventArgs e)
	{
		DataGridView dataGridView = sender as DataGridView;
		foreach (DataGridViewRow item in (IEnumerable)dataGridView.Rows)
		{
			item.Cells["cpkNo"].Value = item.Index + 1;
			if (item.Cells["cpkRank"].Value != null)
			{
				switch (item.Cells["cpkRank"].Value.ToString())
				{
				case "RANK3":
					item.Cells["cpkRank"].Style.ForeColor = Color.Orange;
					break;
				case "RANK4":
					item.Cells["cpkRank"].Style.ForeColor = Color.Orange;
					break;
				case "RANK5":
					item.Cells["cpkRank"].Style.ForeColor = Color.Red;
					break;
				default:
					item.Cells["cpkRank"].Style.ForeColor = Color.Black;
					break;
				}
			}
		}
	}

	private void dgvRank_Sorted(object sender, EventArgs e)
	{
		DataGridView dataGridView = sender as DataGridView;
		foreach (DataGridViewRow item in (IEnumerable)dataGridView.Rows)
		{
			item.Cells["rNo"].Value = item.Index + 1;
			if (item.Cells["Rank"].Value != null)
			{
				switch (item.Cells["Rank"].Value.ToString())
				{
				case "RANK3":
					item.Cells["Rank"].Style.ForeColor = Color.Orange;
					break;
				case "RANK4":
					item.Cells["Rank"].Style.ForeColor = Color.Orange;
					break;
				case "RANK5":
					item.Cells["Rank"].Style.ForeColor = Color.Red;
					break;
				default:
					item.Cells["Rank"].Style.ForeColor = Color.Black;
					break;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_5S_QA_System.mPanelViewManager));
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
		System.Windows.Forms.DataVisualization.Charting.Legend legend = new System.Windows.Forms.DataVisualization.Charting.Legend();
		System.Windows.Forms.DataVisualization.Charting.Series series = new System.Windows.Forms.DataVisualization.Charting.Series();
		System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
		System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
		System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
		System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
		System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle22 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle23 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle24 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle25 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle26 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle27 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle28 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle29 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle30 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle31 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle32 = new System.Windows.Forms.DataGridViewCellStyle();
		this.tbpInformation = new MetroFramework.Controls.MetroTabPage();
		this.dgvFooter = new System.Windows.Forms.DataGridView();
		this.footer_title = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.footer_value = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dgvOther = new System.Windows.Forms.DataGridView();
		this.other_title = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.other_value = new System.Windows.Forms.DataGridViewImageColumn();
		this.dgvContent = new System.Windows.Forms.DataGridView();
		this.content_title = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.content_value = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.lblTitleProgress = new System.Windows.Forms.Label();
		this.tbpStatistic = new MetroFramework.Controls.MetroTabPage();
		this.chartProcess = new System.Windows.Forms.DataVisualization.Charting.Chart();
		this.chartMain = new System.Windows.Forms.DataVisualization.Charting.Chart();
		this.tbcView = new MetroFramework.Controls.MetroTabControl();
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.btnHistory = new System.Windows.Forms.Button();
		this.btnExport = new System.Windows.Forms.Button();
		this.btnDown = new System.Windows.Forms.Button();
		this.btnUp = new System.Windows.Forms.Button();
		this.tblPanelTitle = new System.Windows.Forms.TableLayoutPanel();
		this.lblValue = new System.Windows.Forms.Label();
		this.lblTitle = new System.Windows.Forms.Label();
		this.panelFooter = new System.Windows.Forms.Panel();
		this.panelTitle = new System.Windows.Forms.Panel();
		this.tblPanelFooter = new System.Windows.Forms.TableLayoutPanel();
		this.panelResize = new System.Windows.Forms.Panel();
		this.dgvComment = new System.Windows.Forms.DataGridView();
		this.cNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Content = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Link = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.contextMenuStripMain = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.main_fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.lblComment = new System.Windows.Forms.Label();
		this.txtTotalRow = new System.Windows.Forms.Label();
		this.lblCommentTotal = new System.Windows.Forms.Label();
		this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
		this.panelView = new System.Windows.Forms.Panel();
		this.dgvRank = new System.Windows.Forms.DataGridView();
		this.rNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.MeasurementName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Rank = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Percentage = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.rSample = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
		this.lblRank = new System.Windows.Forms.Label();
		this.lblRankTotalRow = new System.Windows.Forms.Label();
		this.lblRankTotal = new System.Windows.Forms.Label();
		this.panelSample = new System.Windows.Forms.Panel();
		this.dgvMeas = new System.Windows.Forms.DataGridView();
		this.dgvSample = new System.Windows.Forms.DataGridView();
		this.No = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Sample = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.PlanId = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.PlanName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.OK = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.NG = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Empty = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.CompletedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Completed = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.CheckedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Checked = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ApprovedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Approved = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dgvRequestStatus = new System.Windows.Forms.DataGridView();
		this.StatusCompletedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.StatusCompleted = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.StatusCheckedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.StatusChecked = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.StatusApprovedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.StatusApproved = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dgvRequestDetail = new System.Windows.Forms.DataGridView();
		this.NoDetail = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.CavityDetail = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.PinDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ShotMold = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ProduceNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.IdDetail = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
		this.lblStatus = new System.Windows.Forms.Label();
		this.lblTotalRow = new System.Windows.Forms.Label();
		this.lblSampleTotal = new System.Windows.Forms.Label();
		this.dgvCpk = new System.Windows.Forms.DataGridView();
		this.cpkNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.cpkName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.cpkCpk = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.cpkRank = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.cpkId = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.tpanelCpk = new System.Windows.Forms.TableLayoutPanel();
		this.lblCpk = new System.Windows.Forms.Label();
		this.lblCpkTitleTotal = new System.Windows.Forms.Label();
		this.lblCpkTotal = new System.Windows.Forms.Label();
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
		this.tbpInformation.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvFooter).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.dgvOther).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.dgvContent).BeginInit();
		this.tbpStatistic.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.chartProcess).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.chartMain).BeginInit();
		this.tbcView.SuspendLayout();
		this.tblPanelTitle.SuspendLayout();
		this.tblPanelFooter.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvComment).BeginInit();
		this.contextMenuStripMain.SuspendLayout();
		this.tableLayoutPanel1.SuspendLayout();
		this.panelView.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvRank).BeginInit();
		this.tableLayoutPanel3.SuspendLayout();
		this.panelSample.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvMeas).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.dgvSample).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.dgvRequestStatus).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.dgvRequestDetail).BeginInit();
		this.tableLayoutPanel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvCpk).BeginInit();
		this.tpanelCpk.SuspendLayout();
		base.SuspendLayout();
		this.tbpInformation.Controls.Add(this.dgvFooter);
		this.tbpInformation.Controls.Add(this.dgvOther);
		this.tbpInformation.Controls.Add(this.dgvContent);
		this.tbpInformation.HorizontalScrollbarBarColor = false;
		this.tbpInformation.HorizontalScrollbarHighlightOnWheel = false;
		this.tbpInformation.HorizontalScrollbarSize = 0;
		this.tbpInformation.Location = new System.Drawing.Point(4, 38);
		this.tbpInformation.Name = "tbpInformation";
		this.tbpInformation.Size = new System.Drawing.Size(689, 254);
		this.tbpInformation.TabIndex = 1;
		this.tbpInformation.Text = "Information";
		this.tbpInformation.VerticalScrollbarBarColor = false;
		this.tbpInformation.VerticalScrollbarHighlightOnWheel = false;
		this.tbpInformation.VerticalScrollbarSize = 0;
		this.dgvFooter.AllowUserToAddRows = false;
		this.dgvFooter.AllowUserToDeleteRows = false;
		this.dgvFooter.AllowUserToResizeColumns = false;
		this.dgvFooter.AllowUserToResizeRows = false;
		this.dgvFooter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		this.dgvFooter.ColumnHeadersVisible = false;
		this.dgvFooter.Columns.AddRange(this.footer_title, this.footer_value);
		this.dgvFooter.Dock = System.Windows.Forms.DockStyle.Top;
		this.dgvFooter.Location = new System.Drawing.Point(0, 44);
		this.dgvFooter.Margin = new System.Windows.Forms.Padding(1);
		this.dgvFooter.Name = "dgvFooter";
		this.dgvFooter.ReadOnly = true;
		this.dgvFooter.RowHeadersVisible = false;
		this.dgvFooter.ScrollBars = System.Windows.Forms.ScrollBars.None;
		this.dgvFooter.Size = new System.Drawing.Size(689, 22);
		this.dgvFooter.TabIndex = 158;
		this.dgvFooter.Visible = false;
		dataGridViewCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.footer_title.DefaultCellStyle = dataGridViewCellStyle;
		this.footer_title.HeaderText = "Title";
		this.footer_title.Name = "footer_title";
		this.footer_title.ReadOnly = true;
		this.footer_title.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.footer_title.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.footer_title.Width = 140;
		this.footer_value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		this.footer_value.DefaultCellStyle = dataGridViewCellStyle2;
		this.footer_value.HeaderText = "Value";
		this.footer_value.Name = "footer_value";
		this.footer_value.ReadOnly = true;
		this.footer_value.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.footer_value.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.dgvOther.AllowUserToAddRows = false;
		this.dgvOther.AllowUserToDeleteRows = false;
		this.dgvOther.AllowUserToResizeColumns = false;
		this.dgvOther.AllowUserToResizeRows = false;
		this.dgvOther.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		this.dgvOther.ColumnHeadersVisible = false;
		this.dgvOther.Columns.AddRange(this.other_title, this.other_value);
		this.dgvOther.Dock = System.Windows.Forms.DockStyle.Top;
		this.dgvOther.Location = new System.Drawing.Point(0, 22);
		this.dgvOther.Margin = new System.Windows.Forms.Padding(1);
		this.dgvOther.Name = "dgvOther";
		this.dgvOther.ReadOnly = true;
		this.dgvOther.RowHeadersVisible = false;
		this.dgvOther.RowTemplate.Height = 100;
		this.dgvOther.ScrollBars = System.Windows.Forms.ScrollBars.None;
		this.dgvOther.Size = new System.Drawing.Size(689, 22);
		this.dgvOther.TabIndex = 159;
		this.dgvOther.Visible = false;
		this.dgvOther.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvOther_CellDoubleClick);
		dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
		this.other_title.DefaultCellStyle = dataGridViewCellStyle3;
		this.other_title.HeaderText = "Title";
		this.other_title.Name = "other_title";
		this.other_title.ReadOnly = true;
		this.other_title.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.other_title.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.other_title.Width = 140;
		this.other_value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle4.NullValue = resources.GetObject("dataGridViewCellStyle4.NullValue");
		this.other_value.DefaultCellStyle = dataGridViewCellStyle4;
		this.other_value.HeaderText = "Value";
		this.other_value.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
		this.other_value.Name = "other_value";
		this.other_value.ReadOnly = true;
		this.other_value.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.dgvContent.AllowUserToAddRows = false;
		this.dgvContent.AllowUserToDeleteRows = false;
		this.dgvContent.AllowUserToResizeColumns = false;
		this.dgvContent.AllowUserToResizeRows = false;
		this.dgvContent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		this.dgvContent.ColumnHeadersVisible = false;
		this.dgvContent.Columns.AddRange(this.content_title, this.content_value);
		this.dgvContent.Dock = System.Windows.Forms.DockStyle.Top;
		this.dgvContent.Location = new System.Drawing.Point(0, 0);
		this.dgvContent.Margin = new System.Windows.Forms.Padding(1);
		this.dgvContent.Name = "dgvContent";
		this.dgvContent.ReadOnly = true;
		this.dgvContent.RowHeadersVisible = false;
		this.dgvContent.ScrollBars = System.Windows.Forms.ScrollBars.None;
		this.dgvContent.Size = new System.Drawing.Size(689, 22);
		this.dgvContent.TabIndex = 157;
		this.dgvContent.Visible = false;
		dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.content_title.DefaultCellStyle = dataGridViewCellStyle5;
		this.content_title.HeaderText = "Title";
		this.content_title.Name = "content_title";
		this.content_title.ReadOnly = true;
		this.content_title.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.content_title.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.content_title.Width = 140;
		this.content_value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		this.content_value.DefaultCellStyle = dataGridViewCellStyle6;
		this.content_value.HeaderText = "Value";
		this.content_value.Name = "content_value";
		this.content_value.ReadOnly = true;
		this.content_value.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.content_value.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.lblTitleProgress.Dock = System.Windows.Forms.DockStyle.Top;
		this.lblTitleProgress.Location = new System.Drawing.Point(221, 0);
		this.lblTitleProgress.Name = "lblTitleProgress";
		this.lblTitleProgress.Size = new System.Drawing.Size(466, 25);
		this.lblTitleProgress.TabIndex = 148;
		this.lblTitleProgress.Text = "* Progress of the request *";
		this.lblTitleProgress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.tbpStatistic.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.tbpStatistic.Controls.Add(this.chartProcess);
		this.tbpStatistic.Controls.Add(this.lblTitleProgress);
		this.tbpStatistic.Controls.Add(this.chartMain);
		this.tbpStatistic.HorizontalScrollbarBarColor = false;
		this.tbpStatistic.HorizontalScrollbarHighlightOnWheel = false;
		this.tbpStatistic.HorizontalScrollbarSize = 0;
		this.tbpStatistic.Location = new System.Drawing.Point(4, 38);
		this.tbpStatistic.Name = "tbpStatistic";
		this.tbpStatistic.Size = new System.Drawing.Size(689, 254);
		this.tbpStatistic.TabIndex = 0;
		this.tbpStatistic.Text = "Statistic";
		this.tbpStatistic.VerticalScrollbarBarColor = false;
		this.tbpStatistic.VerticalScrollbarHighlightOnWheel = false;
		this.tbpStatistic.VerticalScrollbarSize = 0;
		chartArea.AxisX.MajorGrid.Enabled = false;
		chartArea.AxisY.LabelStyle.Enabled = false;
		chartArea.AxisY.LineWidth = 0;
		chartArea.AxisY.MajorGrid.Enabled = false;
		chartArea.AxisY.MajorTickMark.Enabled = false;
		chartArea.Name = "ChartArea1";
		this.chartProcess.ChartAreas.Add(chartArea);
		this.chartProcess.Dock = System.Windows.Forms.DockStyle.Fill;
		legend.Enabled = false;
		legend.Name = "Legend1";
		this.chartProcess.Legends.Add(legend);
		this.chartProcess.Location = new System.Drawing.Point(221, 25);
		this.chartProcess.Name = "chartProcess";
		series.ChartArea = "ChartArea1";
		series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedColumn100;
		series.Color = System.Drawing.Color.Blue;
		series.Legend = "Legend1";
		series.Name = "OK";
		series2.ChartArea = "ChartArea1";
		series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedColumn100;
		series2.Color = System.Drawing.Color.Red;
		series2.Legend = "Legend1";
		series2.Name = "NG";
		series3.ChartArea = "ChartArea1";
		series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedColumn100;
		series3.Color = System.Drawing.Color.Gray;
		series3.Legend = "Legend1";
		series3.Name = "Empty";
		this.chartProcess.Series.Add(series);
		this.chartProcess.Series.Add(series2);
		this.chartProcess.Series.Add(series3);
		this.chartProcess.Size = new System.Drawing.Size(466, 227);
		this.chartProcess.TabIndex = 150;
		this.chartProcess.Text = "chart2";
		chartArea2.Name = "ChartArea1";
		this.chartMain.ChartAreas.Add(chartArea2);
		this.chartMain.Dock = System.Windows.Forms.DockStyle.Left;
		legend2.Name = "Legend1";
		this.chartMain.Legends.Add(legend2);
		this.chartMain.Location = new System.Drawing.Point(0, 0);
		this.chartMain.Name = "chartMain";
		series4.ChartArea = "ChartArea1";
		series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
		series4.CustomProperties = "PieLabelStyle=Disabled";
		series4.Legend = "Legend1";
		series4.Name = "SeriesMain";
		this.chartMain.Series.Add(series4);
		this.chartMain.Size = new System.Drawing.Size(221, 252);
		this.chartMain.TabIndex = 149;
		this.chartMain.Text = "chart1";
		this.tbcView.Controls.Add(this.tbpStatistic);
		this.tbcView.Controls.Add(this.tbpInformation);
		this.tbcView.Dock = System.Windows.Forms.DockStyle.Top;
		this.tbcView.FontWeight = MetroFramework.MetroTabControlWeight.Bold;
		this.tbcView.Location = new System.Drawing.Point(0, 64);
		this.tbcView.Name = "tbcView";
		this.tbcView.SelectedIndex = 0;
		this.tbcView.Size = new System.Drawing.Size(697, 296);
		this.tbcView.TabIndex = 157;
		this.tbcView.UseSelectable = true;
		this.tbcView.SelectedIndexChanged += new System.EventHandler(tbcView_SelectedIndexChanged);
		this.btnHistory.AutoSize = true;
		this.btnHistory.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnHistory.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnHistory.FlatAppearance.BorderSize = 0;
		this.btnHistory.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
		this.btnHistory.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
		this.btnHistory.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnHistory.Location = new System.Drawing.Point(70, 3);
		this.btnHistory.Name = "btnHistory";
		this.btnHistory.Size = new System.Drawing.Size(66, 26);
		this.btnHistory.TabIndex = 4;
		this.btnHistory.Text = "History";
		this.toolTipMain.SetToolTip(this.btnHistory, "Goto form history");
		this.btnHistory.UseVisualStyleBackColor = true;
		this.btnExport.AutoSize = true;
		this.btnExport.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnExport.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnExport.FlatAppearance.BorderSize = 0;
		this.btnExport.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
		this.btnExport.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
		this.btnExport.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnExport.Location = new System.Drawing.Point(3, 3);
		this.btnExport.Name = "btnExport";
		this.btnExport.Size = new System.Drawing.Size(61, 26);
		this.btnExport.TabIndex = 1;
		this.btnExport.Text = "Export";
		this.toolTipMain.SetToolTip(this.btnExport, "Confirm export to excel");
		this.btnExport.UseVisualStyleBackColor = true;
		this.btnDown.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnDown.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnDown.FlatAppearance.BorderSize = 0;
		this.btnDown.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.AppWorkspace;
		this.btnDown.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
		this.btnDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnDown.Image = _5S_QA_System.Properties.Resources.arrow_down;
		this.btnDown.Location = new System.Drawing.Point(677, 1);
		this.btnDown.Margin = new System.Windows.Forms.Padding(1);
		this.btnDown.Name = "btnDown";
		this.btnDown.Size = new System.Drawing.Size(22, 25);
		this.btnDown.TabIndex = 129;
		this.btnDown.TabStop = false;
		this.toolTipMain.SetToolTip(this.btnDown, "Display lower row item");
		this.btnDown.Click += new System.EventHandler(btnDown_Click);
		this.btnUp.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnUp.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnUp.FlatAppearance.BorderSize = 0;
		this.btnUp.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.AppWorkspace;
		this.btnUp.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
		this.btnUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUp.Image = _5S_QA_System.Properties.Resources.arrow_up;
		this.btnUp.Location = new System.Drawing.Point(653, 1);
		this.btnUp.Margin = new System.Windows.Forms.Padding(1);
		this.btnUp.Name = "btnUp";
		this.btnUp.Size = new System.Drawing.Size(22, 25);
		this.btnUp.TabIndex = 128;
		this.btnUp.TabStop = false;
		this.toolTipMain.SetToolTip(this.btnUp, "Display upper row item");
		this.btnUp.Click += new System.EventHandler(btnUp_Click);
		this.tblPanelTitle.AutoSize = true;
		this.tblPanelTitle.ColumnCount = 4;
		this.tblPanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tblPanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tblPanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tblPanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tblPanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tblPanelTitle.Controls.Add(this.btnDown, 3, 0);
		this.tblPanelTitle.Controls.Add(this.lblValue, 1, 0);
		this.tblPanelTitle.Controls.Add(this.btnUp, 2, 0);
		this.tblPanelTitle.Controls.Add(this.lblTitle, 0, 0);
		this.tblPanelTitle.Dock = System.Windows.Forms.DockStyle.Top;
		this.tblPanelTitle.Location = new System.Drawing.Point(0, 0);
		this.tblPanelTitle.Margin = new System.Windows.Forms.Padding(0);
		this.tblPanelTitle.Name = "tblPanelTitle";
		this.tblPanelTitle.RowCount = 1;
		this.tblPanelTitle.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tblPanelTitle.Size = new System.Drawing.Size(700, 27);
		this.tblPanelTitle.TabIndex = 157;
		this.lblValue.AutoSize = true;
		this.lblValue.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblValue.ForeColor = System.Drawing.Color.Crimson;
		this.lblValue.Location = new System.Drawing.Point(48, 1);
		this.lblValue.Margin = new System.Windows.Forms.Padding(1);
		this.lblValue.Name = "lblValue";
		this.lblValue.Size = new System.Drawing.Size(603, 25);
		this.lblValue.TabIndex = 131;
		this.lblValue.Text = "...";
		this.lblValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblTitle.AutoSize = true;
		this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTitle.Location = new System.Drawing.Point(1, 1);
		this.lblTitle.Margin = new System.Windows.Forms.Padding(1);
		this.lblTitle.Name = "lblTitle";
		this.lblTitle.Size = new System.Drawing.Size(45, 25);
		this.lblTitle.TabIndex = 0;
		this.lblTitle.Text = "VIEW";
		this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.panelFooter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panelFooter.Location = new System.Drawing.Point(3, 716);
		this.panelFooter.Name = "panelFooter";
		this.panelFooter.Size = new System.Drawing.Size(697, 1);
		this.panelFooter.TabIndex = 160;
		this.panelTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panelTitle.Dock = System.Windows.Forms.DockStyle.Top;
		this.panelTitle.Location = new System.Drawing.Point(3, 27);
		this.panelTitle.Name = "panelTitle";
		this.panelTitle.Size = new System.Drawing.Size(697, 1);
		this.panelTitle.TabIndex = 159;
		this.tblPanelFooter.AutoSize = true;
		this.tblPanelFooter.ColumnCount = 3;
		this.tblPanelFooter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tblPanelFooter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tblPanelFooter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tblPanelFooter.Controls.Add(this.btnHistory, 1, 0);
		this.tblPanelFooter.Controls.Add(this.btnExport, 0, 0);
		this.tblPanelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.tblPanelFooter.Location = new System.Drawing.Point(0, 717);
		this.tblPanelFooter.Margin = new System.Windows.Forms.Padding(1);
		this.tblPanelFooter.Name = "tblPanelFooter";
		this.tblPanelFooter.RowCount = 1;
		this.tblPanelFooter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32f));
		this.tblPanelFooter.Size = new System.Drawing.Size(700, 32);
		this.tblPanelFooter.TabIndex = 158;
		this.panelResize.Dock = System.Windows.Forms.DockStyle.Left;
		this.panelResize.Location = new System.Drawing.Point(0, 27);
		this.panelResize.Margin = new System.Windows.Forms.Padding(0);
		this.panelResize.Name = "panelResize";
		this.panelResize.Size = new System.Drawing.Size(3, 690);
		this.panelResize.TabIndex = 161;
		this.dgvComment.AllowUserToAddRows = false;
		this.dgvComment.AllowUserToDeleteRows = false;
		this.dgvComment.AllowUserToOrderColumns = true;
		this.dgvComment.AllowUserToResizeRows = false;
		dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
		this.dgvComment.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle7;
		this.dgvComment.BackgroundColor = System.Drawing.SystemColors.Window;
		dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.AppWorkspace;
		dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
		dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
		dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
		dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.dgvComment.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle8;
		this.dgvComment.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
		this.dgvComment.Columns.AddRange(this.cNo, this.Content, this.Link, this.Id);
		this.dgvComment.ContextMenuStrip = this.contextMenuStripMain;
		this.dgvComment.Dock = System.Windows.Forms.DockStyle.Top;
		this.dgvComment.EnableHeadersVisualStyles = false;
		this.dgvComment.Location = new System.Drawing.Point(0, 613);
		this.dgvComment.Margin = new System.Windows.Forms.Padding(1);
		this.dgvComment.MultiSelect = false;
		this.dgvComment.Name = "dgvComment";
		this.dgvComment.ReadOnly = true;
		this.dgvComment.RowHeadersVisible = false;
		this.dgvComment.RowHeadersWidth = 25;
		this.dgvComment.Size = new System.Drawing.Size(697, 52);
		this.dgvComment.TabIndex = 146;
		this.dgvComment.CurrentCellChanged += new System.EventHandler(dgvComment_CurrentCellChanged);
		this.cNo.DataPropertyName = "No";
		dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		this.cNo.DefaultCellStyle = dataGridViewCellStyle9;
		this.cNo.HeaderText = "No.";
		this.cNo.Name = "cNo";
		this.cNo.ReadOnly = true;
		this.cNo.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.cNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.cNo.Width = 40;
		this.Content.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Content.DataPropertyName = "Content";
		this.Content.FillWeight = 50f;
		this.Content.HeaderText = "Content";
		this.Content.Name = "Content";
		this.Content.ReadOnly = true;
		this.Link.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Link.DataPropertyName = "Link";
		this.Link.FillWeight = 50f;
		this.Link.HeaderText = "File";
		this.Link.Name = "Link";
		this.Link.ReadOnly = true;
		this.Id.DataPropertyName = "Id";
		this.Id.HeaderText = "Id";
		this.Id.Name = "Id";
		this.Id.ReadOnly = true;
		this.Id.Visible = false;
		this.contextMenuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.main_fileToolStripMenuItem });
		this.contextMenuStripMain.Name = "contextMenuStripStaff";
		this.contextMenuStripMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
		this.contextMenuStripMain.Size = new System.Drawing.Size(119, 26);
		this.main_fileToolStripMenuItem.Name = "main_fileToolStripMenuItem";
		this.main_fileToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
		this.main_fileToolStripMenuItem.Text = "View file";
		this.main_fileToolStripMenuItem.Visible = false;
		this.main_fileToolStripMenuItem.Click += new System.EventHandler(main_fileToolStripMenuItem_Click);
		this.lblComment.AutoSize = true;
		this.lblComment.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblComment.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblComment.Location = new System.Drawing.Point(1, 1);
		this.lblComment.Margin = new System.Windows.Forms.Padding(1);
		this.lblComment.Name = "lblComment";
		this.lblComment.Size = new System.Drawing.Size(71, 16);
		this.lblComment.TabIndex = 146;
		this.lblComment.Text = "Comment";
		this.lblComment.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.txtTotalRow.AutoSize = true;
		this.txtTotalRow.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtTotalRow.Location = new System.Drawing.Point(607, 1);
		this.txtTotalRow.Margin = new System.Windows.Forms.Padding(1);
		this.txtTotalRow.Name = "txtTotalRow";
		this.txtTotalRow.Size = new System.Drawing.Size(72, 16);
		this.txtTotalRow.TabIndex = 149;
		this.txtTotalRow.Text = "Total rows:";
		this.lblCommentTotal.AutoSize = true;
		this.lblCommentTotal.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCommentTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCommentTotal.Location = new System.Drawing.Point(681, 1);
		this.lblCommentTotal.Margin = new System.Windows.Forms.Padding(1);
		this.lblCommentTotal.Name = "lblCommentTotal";
		this.lblCommentTotal.Size = new System.Drawing.Size(15, 16);
		this.lblCommentTotal.TabIndex = 152;
		this.lblCommentTotal.Text = "0";
		this.tableLayoutPanel1.AutoSize = true;
		this.tableLayoutPanel1.ColumnCount = 4;
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel1.Controls.Add(this.lblComment, 0, 0);
		this.tableLayoutPanel1.Controls.Add(this.txtTotalRow, 2, 0);
		this.tableLayoutPanel1.Controls.Add(this.lblCommentTotal, 3, 0);
		this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 595);
		this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(1);
		this.tableLayoutPanel1.Name = "tableLayoutPanel1";
		this.tableLayoutPanel1.RowCount = 1;
		this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel1.Size = new System.Drawing.Size(697, 18);
		this.tableLayoutPanel1.TabIndex = 153;
		this.panelView.AutoScroll = true;
		this.panelView.Controls.Add(this.dgvComment);
		this.panelView.Controls.Add(this.tableLayoutPanel1);
		this.panelView.Controls.Add(this.dgvRank);
		this.panelView.Controls.Add(this.tableLayoutPanel3);
		this.panelView.Controls.Add(this.panelSample);
		this.panelView.Controls.Add(this.dgvRequestStatus);
		this.panelView.Controls.Add(this.dgvRequestDetail);
		this.panelView.Controls.Add(this.tableLayoutPanel2);
		this.panelView.Controls.Add(this.tbcView);
		this.panelView.Controls.Add(this.dgvCpk);
		this.panelView.Controls.Add(this.tpanelCpk);
		this.panelView.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panelView.Location = new System.Drawing.Point(3, 28);
		this.panelView.Margin = new System.Windows.Forms.Padding(0);
		this.panelView.Name = "panelView";
		this.panelView.Size = new System.Drawing.Size(697, 688);
		this.panelView.TabIndex = 162;
		this.dgvRank.AllowUserToAddRows = false;
		this.dgvRank.AllowUserToDeleteRows = false;
		this.dgvRank.AllowUserToOrderColumns = true;
		this.dgvRank.AllowUserToResizeRows = false;
		dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
		this.dgvRank.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle10;
		this.dgvRank.BackgroundColor = System.Drawing.SystemColors.Window;
		dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.AppWorkspace;
		dataGridViewCellStyle11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.WindowText;
		dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
		dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
		dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.dgvRank.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle11;
		this.dgvRank.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
		this.dgvRank.Columns.AddRange(this.rNo, this.MeasurementName, this.Rank, this.Percentage, this.rSample);
		this.dgvRank.ContextMenuStrip = this.contextMenuStripMain;
		this.dgvRank.Dock = System.Windows.Forms.DockStyle.Top;
		this.dgvRank.EnableHeadersVisualStyles = false;
		this.dgvRank.Location = new System.Drawing.Point(0, 543);
		this.dgvRank.Margin = new System.Windows.Forms.Padding(1);
		this.dgvRank.MultiSelect = false;
		this.dgvRank.Name = "dgvRank";
		this.dgvRank.ReadOnly = true;
		this.dgvRank.RowHeadersVisible = false;
		this.dgvRank.RowHeadersWidth = 25;
		this.dgvRank.Size = new System.Drawing.Size(697, 52);
		this.dgvRank.TabIndex = 169;
		this.dgvRank.Sorted += new System.EventHandler(dgvRank_Sorted);
		this.rNo.DataPropertyName = "No";
		dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		this.rNo.DefaultCellStyle = dataGridViewCellStyle12;
		this.rNo.HeaderText = "No.";
		this.rNo.Name = "rNo";
		this.rNo.ReadOnly = true;
		this.rNo.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.rNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.rNo.Width = 40;
		this.MeasurementName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.MeasurementName.DataPropertyName = "MeasurementName";
		this.MeasurementName.FillWeight = 40f;
		this.MeasurementName.HeaderText = "Measurement";
		this.MeasurementName.Name = "MeasurementName";
		this.MeasurementName.ReadOnly = true;
		this.Rank.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Rank.DataPropertyName = "Rank";
		this.Rank.FillWeight = 20f;
		this.Rank.HeaderText = "Rank";
		this.Rank.Name = "Rank";
		this.Rank.ReadOnly = true;
		this.Percentage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Percentage.DataPropertyName = "Percentage";
		this.Percentage.FillWeight = 20f;
		this.Percentage.HeaderText = "Percentage (%)";
		this.Percentage.Name = "Percentage";
		this.Percentage.ReadOnly = true;
		this.rSample.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.rSample.DataPropertyName = "Sample";
		this.rSample.FillWeight = 20f;
		this.rSample.HeaderText = "Sample";
		this.rSample.Name = "rSample";
		this.rSample.ReadOnly = true;
		this.tableLayoutPanel3.AutoSize = true;
		this.tableLayoutPanel3.ColumnCount = 4;
		this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel3.Controls.Add(this.lblRank, 0, 0);
		this.tableLayoutPanel3.Controls.Add(this.lblRankTotalRow, 2, 0);
		this.tableLayoutPanel3.Controls.Add(this.lblRankTotal, 3, 0);
		this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 525);
		this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(1);
		this.tableLayoutPanel3.Name = "tableLayoutPanel3";
		this.tableLayoutPanel3.RowCount = 1;
		this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel3.Size = new System.Drawing.Size(697, 18);
		this.tableLayoutPanel3.TabIndex = 170;
		this.lblRank.AutoSize = true;
		this.lblRank.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblRank.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblRank.Location = new System.Drawing.Point(1, 1);
		this.lblRank.Margin = new System.Windows.Forms.Padding(1);
		this.lblRank.Name = "lblRank";
		this.lblRank.Size = new System.Drawing.Size(122, 16);
		this.lblRank.TabIndex = 146;
		this.lblRank.Text = "Increase sample";
		this.lblRank.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblRankTotalRow.AutoSize = true;
		this.lblRankTotalRow.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblRankTotalRow.Location = new System.Drawing.Point(607, 1);
		this.lblRankTotalRow.Margin = new System.Windows.Forms.Padding(1);
		this.lblRankTotalRow.Name = "lblRankTotalRow";
		this.lblRankTotalRow.Size = new System.Drawing.Size(72, 16);
		this.lblRankTotalRow.TabIndex = 149;
		this.lblRankTotalRow.Text = "Total rows:";
		this.lblRankTotal.AutoSize = true;
		this.lblRankTotal.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblRankTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblRankTotal.Location = new System.Drawing.Point(681, 1);
		this.lblRankTotal.Margin = new System.Windows.Forms.Padding(1);
		this.lblRankTotal.Name = "lblRankTotal";
		this.lblRankTotal.Size = new System.Drawing.Size(15, 16);
		this.lblRankTotal.TabIndex = 152;
		this.lblRankTotal.Text = "0";
		this.panelSample.Controls.Add(this.dgvMeas);
		this.panelSample.Controls.Add(this.dgvSample);
		this.panelSample.Dock = System.Windows.Forms.DockStyle.Top;
		this.panelSample.Location = new System.Drawing.Point(0, 470);
		this.panelSample.Name = "panelSample";
		this.panelSample.Size = new System.Drawing.Size(697, 55);
		this.panelSample.TabIndex = 158;
		this.dgvMeas.AllowUserToAddRows = false;
		this.dgvMeas.AllowUserToDeleteRows = false;
		this.dgvMeas.AllowUserToOrderColumns = true;
		this.dgvMeas.AllowUserToResizeRows = false;
		dataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Control;
		this.dgvMeas.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle13;
		this.dgvMeas.BackgroundColor = System.Drawing.SystemColors.Window;
		dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.AppWorkspace;
		dataGridViewCellStyle14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.WindowText;
		dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
		dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
		dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.dgvMeas.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle14;
		this.dgvMeas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
		this.dgvMeas.Columns.AddRange(this.MNo, this.MName, this.Value, this.Unit, this.Upper, this.Lower, this.LSL, this.USL, this.Cavity, this.Result, this.Judge, this.History, this.MeasurementUnit);
		this.dgvMeas.Dock = System.Windows.Forms.DockStyle.Right;
		this.dgvMeas.EnableHeadersVisualStyles = false;
		this.dgvMeas.Location = new System.Drawing.Point(117, 0);
		this.dgvMeas.Margin = new System.Windows.Forms.Padding(1);
		this.dgvMeas.MultiSelect = false;
		this.dgvMeas.Name = "dgvMeas";
		this.dgvMeas.ReadOnly = true;
		this.dgvMeas.RowHeadersVisible = false;
		this.dgvMeas.RowHeadersWidth = 25;
		this.dgvMeas.Size = new System.Drawing.Size(580, 55);
		this.dgvMeas.TabIndex = 156;
		this.dgvMeas.Sorted += new System.EventHandler(dgvMeas_Sorted);
		this.dgvSample.AllowUserToAddRows = false;
		this.dgvSample.AllowUserToDeleteRows = false;
		this.dgvSample.AllowUserToOrderColumns = true;
		this.dgvSample.AllowUserToResizeRows = false;
		dataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.Control;
		this.dgvSample.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle15;
		this.dgvSample.BackgroundColor = System.Drawing.SystemColors.Window;
		dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle16.BackColor = System.Drawing.SystemColors.AppWorkspace;
		dataGridViewCellStyle16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		dataGridViewCellStyle16.ForeColor = System.Drawing.SystemColors.WindowText;
		dataGridViewCellStyle16.SelectionBackColor = System.Drawing.SystemColors.Highlight;
		dataGridViewCellStyle16.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
		dataGridViewCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.dgvSample.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle16;
		this.dgvSample.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
		this.dgvSample.Columns.AddRange(this.No, this.Sample, this.PlanId, this.PlanName, this.OK, this.NG, this.Empty, this.Status, this.CompletedBy, this.Completed, this.CheckedBy, this.Checked, this.ApprovedBy, this.Approved);
		this.dgvSample.Dock = System.Windows.Forms.DockStyle.Fill;
		this.dgvSample.EnableHeadersVisualStyles = false;
		this.dgvSample.Location = new System.Drawing.Point(0, 0);
		this.dgvSample.Margin = new System.Windows.Forms.Padding(1);
		this.dgvSample.MultiSelect = false;
		this.dgvSample.Name = "dgvSample";
		this.dgvSample.ReadOnly = true;
		this.dgvSample.RowHeadersVisible = false;
		this.dgvSample.RowHeadersWidth = 25;
		this.dgvSample.Size = new System.Drawing.Size(697, 55);
		this.dgvSample.TabIndex = 155;
		this.dgvSample.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvSample_CellClick);
		this.dgvSample.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvSample_CellDoubleClick);
		this.dgvSample.CurrentCellChanged += new System.EventHandler(dgvSample_CurrentCellChanged);
		this.dgvSample.Sorted += new System.EventHandler(dgvSample_Sorted);
		this.No.DataPropertyName = "No";
		dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		this.No.DefaultCellStyle = dataGridViewCellStyle17;
		this.No.HeaderText = "No.";
		this.No.Name = "No";
		this.No.ReadOnly = true;
		this.No.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.No.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.No.Width = 40;
		this.Sample.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Sample.DataPropertyName = "Sample";
		this.Sample.FillWeight = 20f;
		this.Sample.HeaderText = "Sample";
		this.Sample.Name = "Sample";
		this.Sample.ReadOnly = true;
		this.PlanId.DataPropertyName = "PlanId";
		this.PlanId.HeaderText = "Plan id";
		this.PlanId.Name = "PlanId";
		this.PlanId.ReadOnly = true;
		this.PlanId.Visible = false;
		this.PlanName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.PlanName.DataPropertyName = "PlanName";
		this.PlanName.FillWeight = 30f;
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
		this.Status.FillWeight = 20f;
		this.Status.HeaderText = "Status";
		this.Status.Name = "Status";
		this.Status.ReadOnly = true;
		this.CompletedBy.DataPropertyName = "CompletedBy";
		this.CompletedBy.HeaderText = "Completed by";
		this.CompletedBy.Name = "CompletedBy";
		this.CompletedBy.ReadOnly = true;
		this.CompletedBy.Visible = false;
		this.Completed.DataPropertyName = "Completed";
		this.Completed.HeaderText = "Completed";
		this.Completed.Name = "Completed";
		this.Completed.ReadOnly = true;
		this.Completed.Visible = false;
		this.CheckedBy.DataPropertyName = "CheckedBy";
		this.CheckedBy.HeaderText = "Checked by";
		this.CheckedBy.Name = "CheckedBy";
		this.CheckedBy.ReadOnly = true;
		this.CheckedBy.Visible = false;
		this.Checked.DataPropertyName = "Checked";
		this.Checked.HeaderText = "Checked";
		this.Checked.Name = "Checked";
		this.Checked.ReadOnly = true;
		this.Checked.Visible = false;
		this.ApprovedBy.DataPropertyName = "ApprovedBy";
		this.ApprovedBy.HeaderText = "Approved by";
		this.ApprovedBy.Name = "ApprovedBy";
		this.ApprovedBy.ReadOnly = true;
		this.ApprovedBy.Visible = false;
		this.Approved.DataPropertyName = "Approved";
		this.Approved.HeaderText = "Approved";
		this.Approved.Name = "Approved";
		this.Approved.ReadOnly = true;
		this.Approved.Visible = false;
		this.dgvRequestStatus.AllowUserToAddRows = false;
		this.dgvRequestStatus.AllowUserToDeleteRows = false;
		this.dgvRequestStatus.AllowUserToOrderColumns = true;
		this.dgvRequestStatus.AllowUserToResizeRows = false;
		dataGridViewCellStyle18.BackColor = System.Drawing.SystemColors.Control;
		this.dgvRequestStatus.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle18;
		this.dgvRequestStatus.BackgroundColor = System.Drawing.SystemColors.Window;
		dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle19.BackColor = System.Drawing.SystemColors.AppWorkspace;
		dataGridViewCellStyle19.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		dataGridViewCellStyle19.ForeColor = System.Drawing.SystemColors.WindowText;
		dataGridViewCellStyle19.SelectionBackColor = System.Drawing.SystemColors.Highlight;
		dataGridViewCellStyle19.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
		dataGridViewCellStyle19.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.dgvRequestStatus.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle19;
		this.dgvRequestStatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
		this.dgvRequestStatus.Columns.AddRange(this.StatusCompletedBy, this.StatusCompleted, this.StatusCheckedBy, this.StatusChecked, this.StatusApprovedBy, this.StatusApproved);
		this.dgvRequestStatus.Dock = System.Windows.Forms.DockStyle.Top;
		this.dgvRequestStatus.EnableHeadersVisualStyles = false;
		this.dgvRequestStatus.Location = new System.Drawing.Point(0, 424);
		this.dgvRequestStatus.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.dgvRequestStatus.MultiSelect = false;
		this.dgvRequestStatus.Name = "dgvRequestStatus";
		this.dgvRequestStatus.ReadOnly = true;
		this.dgvRequestStatus.RowHeadersVisible = false;
		this.dgvRequestStatus.RowHeadersWidth = 25;
		this.dgvRequestStatus.Size = new System.Drawing.Size(697, 46);
		this.dgvRequestStatus.TabIndex = 166;
		this.dgvRequestStatus.Visible = false;
		this.StatusCompletedBy.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.StatusCompletedBy.DataPropertyName = "CompletedBy";
		this.StatusCompletedBy.FillWeight = 25f;
		this.StatusCompletedBy.HeaderText = "Completed by";
		this.StatusCompletedBy.Name = "StatusCompletedBy";
		this.StatusCompletedBy.ReadOnly = true;
		this.StatusCompleted.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.StatusCompleted.DataPropertyName = "Completed";
		this.StatusCompleted.FillWeight = 25f;
		this.StatusCompleted.HeaderText = "Completed";
		this.StatusCompleted.Name = "StatusCompleted";
		this.StatusCompleted.ReadOnly = true;
		this.StatusCheckedBy.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.StatusCheckedBy.DataPropertyName = "CheckedBy";
		this.StatusCheckedBy.FillWeight = 25f;
		this.StatusCheckedBy.HeaderText = "Checked by";
		this.StatusCheckedBy.Name = "StatusCheckedBy";
		this.StatusCheckedBy.ReadOnly = true;
		this.StatusChecked.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.StatusChecked.DataPropertyName = "Checked";
		this.StatusChecked.FillWeight = 25f;
		this.StatusChecked.HeaderText = "Checked";
		this.StatusChecked.Name = "StatusChecked";
		this.StatusChecked.ReadOnly = true;
		this.StatusApprovedBy.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.StatusApprovedBy.DataPropertyName = "ApprovedBy";
		this.StatusApprovedBy.FillWeight = 25f;
		this.StatusApprovedBy.HeaderText = "Approved by";
		this.StatusApprovedBy.Name = "StatusApprovedBy";
		this.StatusApprovedBy.ReadOnly = true;
		this.StatusApproved.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.StatusApproved.DataPropertyName = "Approved";
		this.StatusApproved.FillWeight = 25f;
		this.StatusApproved.HeaderText = "Approved";
		this.StatusApproved.Name = "StatusApproved";
		this.StatusApproved.ReadOnly = true;
		this.dgvRequestDetail.AllowUserToAddRows = false;
		this.dgvRequestDetail.AllowUserToDeleteRows = false;
		this.dgvRequestDetail.AllowUserToOrderColumns = true;
		this.dgvRequestDetail.AllowUserToResizeRows = false;
		dataGridViewCellStyle20.BackColor = System.Drawing.SystemColors.Control;
		this.dgvRequestDetail.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle20;
		this.dgvRequestDetail.BackgroundColor = System.Drawing.SystemColors.Window;
		dataGridViewCellStyle21.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle21.BackColor = System.Drawing.SystemColors.AppWorkspace;
		dataGridViewCellStyle21.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		dataGridViewCellStyle21.ForeColor = System.Drawing.SystemColors.WindowText;
		dataGridViewCellStyle21.SelectionBackColor = System.Drawing.SystemColors.Highlight;
		dataGridViewCellStyle21.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
		dataGridViewCellStyle21.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.dgvRequestDetail.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle21;
		this.dgvRequestDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
		this.dgvRequestDetail.Columns.AddRange(this.NoDetail, this.CavityDetail, this.PinDate, this.ShotMold, this.ProduceNo, this.IdDetail);
		this.dgvRequestDetail.Dock = System.Windows.Forms.DockStyle.Top;
		this.dgvRequestDetail.EnableHeadersVisualStyles = false;
		this.dgvRequestDetail.Location = new System.Drawing.Point(0, 378);
		this.dgvRequestDetail.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.dgvRequestDetail.MultiSelect = false;
		this.dgvRequestDetail.Name = "dgvRequestDetail";
		this.dgvRequestDetail.ReadOnly = true;
		this.dgvRequestDetail.RowHeadersVisible = false;
		this.dgvRequestDetail.RowHeadersWidth = 25;
		this.dgvRequestDetail.Size = new System.Drawing.Size(697, 46);
		this.dgvRequestDetail.TabIndex = 165;
		this.dgvRequestDetail.Visible = false;
		this.NoDetail.DataPropertyName = "No";
		dataGridViewCellStyle22.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		this.NoDetail.DefaultCellStyle = dataGridViewCellStyle22;
		this.NoDetail.HeaderText = "No.";
		this.NoDetail.Name = "NoDetail";
		this.NoDetail.ReadOnly = true;
		this.NoDetail.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.NoDetail.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.NoDetail.Visible = false;
		this.NoDetail.Width = 40;
		this.CavityDetail.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.CavityDetail.DataPropertyName = "Cavity";
		this.CavityDetail.FillWeight = 25f;
		this.CavityDetail.HeaderText = "Cavity";
		this.CavityDetail.Name = "CavityDetail";
		this.CavityDetail.ReadOnly = true;
		this.PinDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.PinDate.DataPropertyName = "PinDate";
		this.PinDate.FillWeight = 25f;
		this.PinDate.HeaderText = "Pindate";
		this.PinDate.Name = "PinDate";
		this.PinDate.ReadOnly = true;
		this.ShotMold.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.ShotMold.DataPropertyName = "ShotMold";
		this.ShotMold.FillWeight = 25f;
		this.ShotMold.HeaderText = "Shot mold";
		this.ShotMold.Name = "ShotMold";
		this.ShotMold.ReadOnly = true;
		this.ProduceNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.ProduceNo.DataPropertyName = "ProduceNo";
		this.ProduceNo.FillWeight = 25f;
		this.ProduceNo.HeaderText = "Produce no.";
		this.ProduceNo.Name = "ProduceNo";
		this.ProduceNo.ReadOnly = true;
		this.IdDetail.DataPropertyName = "Id";
		this.IdDetail.HeaderText = "Id";
		this.IdDetail.Name = "IdDetail";
		this.IdDetail.ReadOnly = true;
		this.IdDetail.Visible = false;
		this.tableLayoutPanel2.AutoSize = true;
		this.tableLayoutPanel2.ColumnCount = 4;
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel2.Controls.Add(this.lblStatus, 0, 0);
		this.tableLayoutPanel2.Controls.Add(this.lblTotalRow, 2, 0);
		this.tableLayoutPanel2.Controls.Add(this.lblSampleTotal, 3, 0);
		this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 360);
		this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(1);
		this.tableLayoutPanel2.Name = "tableLayoutPanel2";
		this.tableLayoutPanel2.RowCount = 1;
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel2.Size = new System.Drawing.Size(697, 18);
		this.tableLayoutPanel2.TabIndex = 159;
		this.lblStatus.AutoSize = true;
		this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblStatus.Location = new System.Drawing.Point(1, 1);
		this.lblStatus.Margin = new System.Windows.Forms.Padding(1);
		this.lblStatus.Name = "lblStatus";
		this.lblStatus.Size = new System.Drawing.Size(50, 16);
		this.lblStatus.TabIndex = 146;
		this.lblStatus.Text = "Status";
		this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblTotalRow.AutoSize = true;
		this.lblTotalRow.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTotalRow.Location = new System.Drawing.Point(607, 1);
		this.lblTotalRow.Margin = new System.Windows.Forms.Padding(1);
		this.lblTotalRow.Name = "lblTotalRow";
		this.lblTotalRow.Size = new System.Drawing.Size(72, 16);
		this.lblTotalRow.TabIndex = 149;
		this.lblTotalRow.Text = "Total rows:";
		this.lblSampleTotal.AutoSize = true;
		this.lblSampleTotal.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblSampleTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSampleTotal.Location = new System.Drawing.Point(681, 1);
		this.lblSampleTotal.Margin = new System.Windows.Forms.Padding(1);
		this.lblSampleTotal.Name = "lblSampleTotal";
		this.lblSampleTotal.Size = new System.Drawing.Size(15, 16);
		this.lblSampleTotal.TabIndex = 152;
		this.lblSampleTotal.Text = "0";
		this.dgvCpk.AllowUserToAddRows = false;
		this.dgvCpk.AllowUserToDeleteRows = false;
		this.dgvCpk.AllowUserToOrderColumns = true;
		this.dgvCpk.AllowUserToResizeRows = false;
		dataGridViewCellStyle23.BackColor = System.Drawing.SystemColors.Control;
		this.dgvCpk.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle23;
		this.dgvCpk.BackgroundColor = System.Drawing.SystemColors.Window;
		dataGridViewCellStyle24.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle24.BackColor = System.Drawing.SystemColors.AppWorkspace;
		dataGridViewCellStyle24.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		dataGridViewCellStyle24.ForeColor = System.Drawing.SystemColors.WindowText;
		dataGridViewCellStyle24.SelectionBackColor = System.Drawing.SystemColors.Highlight;
		dataGridViewCellStyle24.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
		dataGridViewCellStyle24.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.dgvCpk.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle24;
		this.dgvCpk.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
		this.dgvCpk.Columns.AddRange(this.cpkNo, this.cpkName, this.cpkCpk, this.cpkRank, this.cpkId);
		this.dgvCpk.Dock = System.Windows.Forms.DockStyle.Top;
		this.dgvCpk.EnableHeadersVisualStyles = false;
		this.dgvCpk.Location = new System.Drawing.Point(0, 18);
		this.dgvCpk.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.dgvCpk.MultiSelect = false;
		this.dgvCpk.Name = "dgvCpk";
		this.dgvCpk.ReadOnly = true;
		this.dgvCpk.RowHeadersVisible = false;
		this.dgvCpk.RowHeadersWidth = 25;
		this.dgvCpk.Size = new System.Drawing.Size(697, 46);
		this.dgvCpk.TabIndex = 168;
		this.dgvCpk.Sorted += new System.EventHandler(dgvCpk_Sorted);
		this.cpkNo.DataPropertyName = "No";
		dataGridViewCellStyle25.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		this.cpkNo.DefaultCellStyle = dataGridViewCellStyle25;
		this.cpkNo.HeaderText = "No.";
		this.cpkNo.Name = "cpkNo";
		this.cpkNo.ReadOnly = true;
		this.cpkNo.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.cpkNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.cpkNo.Width = 40;
		this.cpkName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.cpkName.DataPropertyName = "Name";
		this.cpkName.FillWeight = 50f;
		this.cpkName.HeaderText = "Name";
		this.cpkName.Name = "cpkName";
		this.cpkName.ReadOnly = true;
		this.cpkCpk.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.cpkCpk.DataPropertyName = "Cpk";
		this.cpkCpk.FillWeight = 25f;
		this.cpkCpk.HeaderText = "Cpk";
		this.cpkCpk.Name = "cpkCpk";
		this.cpkCpk.ReadOnly = true;
		this.cpkRank.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.cpkRank.DataPropertyName = "Rank";
		this.cpkRank.FillWeight = 25f;
		this.cpkRank.HeaderText = "Rank";
		this.cpkRank.Name = "cpkRank";
		this.cpkRank.ReadOnly = true;
		this.cpkId.DataPropertyName = "Id";
		this.cpkId.HeaderText = "Id";
		this.cpkId.Name = "cpkId";
		this.cpkId.ReadOnly = true;
		this.cpkId.Visible = false;
		this.tpanelCpk.AutoSize = true;
		this.tpanelCpk.ColumnCount = 4;
		this.tpanelCpk.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelCpk.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tpanelCpk.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelCpk.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelCpk.Controls.Add(this.lblCpk, 0, 0);
		this.tpanelCpk.Controls.Add(this.lblCpkTitleTotal, 2, 0);
		this.tpanelCpk.Controls.Add(this.lblCpkTotal, 3, 0);
		this.tpanelCpk.Dock = System.Windows.Forms.DockStyle.Top;
		this.tpanelCpk.Location = new System.Drawing.Point(0, 0);
		this.tpanelCpk.Margin = new System.Windows.Forms.Padding(1);
		this.tpanelCpk.Name = "tpanelCpk";
		this.tpanelCpk.RowCount = 1;
		this.tpanelCpk.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tpanelCpk.Size = new System.Drawing.Size(697, 18);
		this.tpanelCpk.TabIndex = 167;
		this.lblCpk.AutoSize = true;
		this.lblCpk.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCpk.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCpk.Location = new System.Drawing.Point(1, 1);
		this.lblCpk.Margin = new System.Windows.Forms.Padding(1);
		this.lblCpk.Name = "lblCpk";
		this.lblCpk.Size = new System.Drawing.Size(34, 16);
		this.lblCpk.TabIndex = 146;
		this.lblCpk.Text = "Cpk";
		this.lblCpk.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblCpkTitleTotal.AutoSize = true;
		this.lblCpkTitleTotal.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCpkTitleTotal.Location = new System.Drawing.Point(607, 1);
		this.lblCpkTitleTotal.Margin = new System.Windows.Forms.Padding(1);
		this.lblCpkTitleTotal.Name = "lblCpkTitleTotal";
		this.lblCpkTitleTotal.Size = new System.Drawing.Size(72, 16);
		this.lblCpkTitleTotal.TabIndex = 149;
		this.lblCpkTitleTotal.Text = "Total rows:";
		this.lblCpkTotal.AutoSize = true;
		this.lblCpkTotal.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCpkTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCpkTotal.Location = new System.Drawing.Point(681, 1);
		this.lblCpkTotal.Margin = new System.Windows.Forms.Padding(1);
		this.lblCpkTotal.Name = "lblCpkTotal";
		this.lblCpkTotal.Size = new System.Drawing.Size(15, 16);
		this.lblCpkTotal.TabIndex = 152;
		this.lblCpkTotal.Text = "0";
		this.MNo.DataPropertyName = "No";
		dataGridViewCellStyle26.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		this.MNo.DefaultCellStyle = dataGridViewCellStyle26;
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
		dataGridViewCellStyle27.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.Value.DefaultCellStyle = dataGridViewCellStyle27;
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
		dataGridViewCellStyle28.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.Upper.DefaultCellStyle = dataGridViewCellStyle28;
		this.Upper.FillWeight = 15f;
		this.Upper.HeaderText = "Upper ";
		this.Upper.Name = "Upper";
		this.Upper.ReadOnly = true;
		this.Lower.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Lower.DataPropertyName = "Lower";
		dataGridViewCellStyle29.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.Lower.DefaultCellStyle = dataGridViewCellStyle29;
		this.Lower.FillWeight = 15f;
		this.Lower.HeaderText = "Lower";
		this.Lower.Name = "Lower";
		this.Lower.ReadOnly = true;
		this.LSL.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.LSL.DataPropertyName = "LSL";
		dataGridViewCellStyle30.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.LSL.DefaultCellStyle = dataGridViewCellStyle30;
		this.LSL.FillWeight = 20f;
		this.LSL.HeaderText = "LSL";
		this.LSL.Name = "LSL";
		this.LSL.ReadOnly = true;
		this.USL.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.USL.DataPropertyName = "USL";
		dataGridViewCellStyle31.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.USL.DefaultCellStyle = dataGridViewCellStyle31;
		this.USL.FillWeight = 20f;
		this.USL.HeaderText = "USL";
		this.USL.Name = "USL";
		this.USL.ReadOnly = true;
		this.Cavity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Cavity.DataPropertyName = "Cavity";
		this.Cavity.FillWeight = 15f;
		this.Cavity.HeaderText = "Cavity";
		this.Cavity.Name = "Cavity";
		this.Cavity.ReadOnly = true;
		this.Result.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Result.DataPropertyName = "Result";
		dataGridViewCellStyle32.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.Result.DefaultCellStyle = dataGridViewCellStyle32;
		this.Result.FillWeight = 20f;
		this.Result.HeaderText = "Result";
		this.Result.Name = "Result";
		this.Result.ReadOnly = true;
		this.Judge.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Judge.DataPropertyName = "Judge";
		this.Judge.FillWeight = 13f;
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
		base.Controls.Add(this.panelView);
		base.Controls.Add(this.panelFooter);
		base.Controls.Add(this.panelTitle);
		base.Controls.Add(this.panelResize);
		base.Controls.Add(this.tblPanelFooter);
		base.Controls.Add(this.tblPanelTitle);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "mPanelViewManager";
		base.Size = new System.Drawing.Size(700, 749);
		base.Load += new System.EventHandler(mPanelViewManager_Load);
		this.tbpInformation.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.dgvFooter).EndInit();
		((System.ComponentModel.ISupportInitialize)this.dgvOther).EndInit();
		((System.ComponentModel.ISupportInitialize)this.dgvContent).EndInit();
		this.tbpStatistic.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.chartProcess).EndInit();
		((System.ComponentModel.ISupportInitialize)this.chartMain).EndInit();
		this.tbcView.ResumeLayout(false);
		this.tblPanelTitle.ResumeLayout(false);
		this.tblPanelTitle.PerformLayout();
		this.tblPanelFooter.ResumeLayout(false);
		this.tblPanelFooter.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvComment).EndInit();
		this.contextMenuStripMain.ResumeLayout(false);
		this.tableLayoutPanel1.ResumeLayout(false);
		this.tableLayoutPanel1.PerformLayout();
		this.panelView.ResumeLayout(false);
		this.panelView.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvRank).EndInit();
		this.tableLayoutPanel3.ResumeLayout(false);
		this.tableLayoutPanel3.PerformLayout();
		this.panelSample.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.dgvMeas).EndInit();
		((System.ComponentModel.ISupportInitialize)this.dgvSample).EndInit();
		((System.ComponentModel.ISupportInitialize)this.dgvRequestStatus).EndInit();
		((System.ComponentModel.ISupportInitialize)this.dgvRequestDetail).EndInit();
		this.tableLayoutPanel2.ResumeLayout(false);
		this.tableLayoutPanel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvCpk).EndInit();
		this.tpanelCpk.ResumeLayout(false);
		this.tpanelCpk.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
