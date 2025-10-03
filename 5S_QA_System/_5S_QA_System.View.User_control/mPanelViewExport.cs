using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using _5S_QA_DL.APIContext;
using _5S_QA_Entities.Constants;
using _5S_QA_Entities.Dtos;
using _5S_QA_Entities.Models;
using _5S_QA_System.Controls;
using _5S_QA_System.Properties;

namespace _5S_QA_System.View.User_control;

public class mPanelViewExport : UserControl
{
	private DataGridView dgvMain;

	private Bitmap bitmap;

	private bool isNullBitmap;

	private int mSample;

	private Guid mId;

	private IContainer components = null;

	private ToolTip toolTipMain;

	private Panel panelResize;

	private Panel panelFooter;

	private Panel panelTitle;

	private TableLayoutPanel tblPanelFooter;

	public Button btnExport;

	private TableLayoutPanel tblPanelTitle;

	private Button btnDown;

	private Label lblValue;

	private Button btnUp;

	private Label lblTitle;

	private Panel panelView;

	private DataGridView dgvFooter;

	private DataGridViewTextBoxColumn footer_title;

	private DataGridViewTextBoxColumn footer_value;

	private DataGridView dgvContent;

	private DataGridViewTextBoxColumn content_title;

	private DataGridViewTextBoxColumn content_value;

	private DataGridView dgvOther;

	private DataGridViewTextBoxColumn other_title;

	private DataGridViewImageColumn other_value;

	private DataGridView dgvMeas;

	private TableLayoutPanel tpanelMeasurement;

	private Label lblMeas;

	private Label lblOK;

	private Label lblNG;

	private Label lblEmpty;

	private Label lblJudgeOK;

	private Label lblJudgeNG;

	private Label lblJudgeEmpty;

	public Button btnDelete;

	private DataGridView dgvComment;

	private TableLayoutPanel tableLayoutPanel1;

	private Label lblComment;

	private Label lblTotalRow;

	private Label lblCommentTotal;

	private Label lblEdit;

	private Label lblEditResult;

	private Label lblWarning;

	private Label lblWarn;

	public ComboBox cbbSample;

	private DataGridView dgvRequestDetail;

	private DataGridViewTextBoxColumn NoDetail;

	private DataGridViewTextBoxColumn CavityDetail;

	private DataGridViewTextBoxColumn PinDate;

	private DataGridViewTextBoxColumn ShotMold;

	private DataGridViewTextBoxColumn ProduceNo;

	private DataGridViewTextBoxColumn IdDetail;

	private Button btnPrevious;

	private Button btnNext;

	private ContextMenuStrip contextMenuStripMain;

	private ToolStripMenuItem main_fileToolStripMenuItem;

	private DataGridViewTextBoxColumn cNo;

	private DataGridViewTextBoxColumn Content;

	private DataGridViewTextBoxColumn Link;

	private DataGridViewTextBoxColumn Id;

	private DataGridViewTextBoxColumn No;

	private DataGridViewTextBoxColumn name;

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

	public mPanelViewExport()
	{
		InitializeComponent();
		Common.setControls(this, toolTipMain, new List<ContextMenuStrip> { contextMenuStripMain });
	}

	private void mPanelViewExport_Load(object sender, EventArgs e)
	{
		Init();
	}

	private void Init()
	{
		ControlResize.Init(panelResize, this, ControlResize.Direction.Horizontal, Cursors.SizeWE);
		dgvMain = base.Parent.Controls["dgvMain"] as DataGridView;
	}

	private void load_dgvMeas(int sample)
	{
		try
		{
			Guid id = ((dgvMain.CurrentRow.Cells["Id"].Value == null) ? Guid.Empty : Guid.Parse(dgvMain.CurrentRow.Cells["Id"].Value.ToString()));
			ResponseDto result = frmLogin.client.GetsAsync(id, sample, new List<string>(), "/api/RequestResult/Gets/{id}/{sample}").Result;
			dgvMeas.DataSource = Common.getDataTableNoType<RequestResultQuickViewModel>(result);
			dgvMeas.Refresh();
			dgvMeas.CurrentCell = null;
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

	public void Display()
	{
		try
		{
			lblValue.Text = dgvMain.CurrentRow.Cells["Id"].Value.ToString();
			mSample = int.Parse(dgvMain.CurrentRow.Cells["Sample"].Value.ToString());
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
		catch
		{
			mSample = 0;
		}
		finally
		{
			load_cbbSample(mSample);
			load_dgvComment();
			dgvContent.Size = new Size(panelView.Width, dgvContent.Rows.Count * 22 + 3);
			dgvOther.Size = new Size(panelView.Width, dgvOther.Rows.Count * 100 + 3);
			dgvFooter.Size = new Size(panelView.Width, dgvFooter.Rows.Count * 22 + 3);
			dgvMeas.Size = new Size(panelView.Width, 22 + dgvMeas.Rows.Count * 22 + 3);
			dgvComment.Size = new Size(panelView.Width, 22 + dgvComment.Rows.Count * 22 + 3);
			dgvContent.CurrentCell = null;
			dgvContent.Refresh();
			dgvOther.CurrentCell = null;
			dgvOther.Refresh();
			dgvFooter.CurrentCell = null;
			dgvFooter.Refresh();
			dgvMeas.CurrentCell = null;
			dgvMeas.Refresh();
			dgvComment.CurrentCell = null;
			dgvComment.Refresh();
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

	private void load_cbbSample(int sample)
	{
		cbbSample.Items.Clear();
		for (int i = 0; i < sample; i++)
		{
			cbbSample.Items.Add(i + 1);
		}
		if (cbbSample.Items.Count.Equals(0))
		{
			cbbSample.Items.Add(1);
		}
		cbbSample.SelectedIndex = 0;
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
			frmZoomView frmZoomView = new frmZoomView(bitmap, lblValue.Text);
			frmZoomView.Show();
		}
	}

	private void dgvMeas_Sorted(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		DataGridView dataGridView = sender as DataGridView;
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		foreach (DataGridViewRow item in (IEnumerable)dataGridView.Rows)
		{
			item.Cells["No"].Value = item.Index + 1;
			object value = item.Cells["Judge"].Value;
			object obj = value;
			switch (obj as string)
			{
			case "OK":
				item.Cells["Result"].Style.ForeColor = Color.Blue;
				item.Cells["Judge"].Style.ForeColor = Color.Blue;
				num++;
				break;
			case "OK+":
				item.Cells["Result"].Style.ForeColor = Color.Blue;
				item.Cells["Judge"].Style.ForeColor = Color.Blue;
				num++;
				num4++;
				break;
			case "OK-":
				item.Cells["Result"].Style.ForeColor = Color.Blue;
				item.Cells["Judge"].Style.ForeColor = Color.Blue;
				num++;
				num4++;
				break;
			case "NG":
				item.Cells["Result"].Style.ForeColor = Color.Red;
				item.Cells["Judge"].Style.ForeColor = Color.Red;
				num2++;
				break;
			case "NG+":
				item.Cells["Result"].Style.ForeColor = Color.Red;
				item.Cells["Judge"].Style.ForeColor = Color.Red;
				num2++;
				break;
			case "NG-":
				item.Cells["Result"].Style.ForeColor = Color.Red;
				item.Cells["Judge"].Style.ForeColor = Color.Red;
				num2++;
				break;
			}
			if (item.Cells["History"].Value != null && !string.IsNullOrEmpty(item.Cells["History"].Value.ToString()) && !item.Cells["History"].Value.ToString().Equals("[]"))
			{
				num3++;
				if (!item.Cells["Name"].Value.ToString().StartsWith("* "))
				{
					item.Cells["Name"].Value = string.Format("* {0}", item.Cells["Name"].Value);
				}
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
		int num5 = dataGridView.Rows.Count - num - num2;
		lblJudgeOK.Text = num.ToString();
		lblJudgeNG.Text = num2.ToString();
		lblJudgeEmpty.Text = num5.ToString();
		lblEditResult.Text = num3.ToString();
		lblWarning.Text = num4.ToString();
	}

	private void cbbSample_SelectedIndexChanged(object sender, EventArgs e)
	{
		int sample = 1;
		if (!cbbSample.SelectedIndex.Equals(-1))
		{
			sample = int.Parse(cbbSample.Text);
		}
		load_dgvMeas(sample);
	}

	private void btnPrevious_Click(object sender, EventArgs e)
	{
		if (cbbSample.SelectedIndex > 0)
		{
			cbbSample.SelectedIndex--;
		}
	}

	private void btnNext_Click(object sender, EventArgs e)
	{
		if (cbbSample.SelectedIndex < cbbSample.Items.Count - 1)
		{
			cbbSample.SelectedIndex++;
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
			Cursor.Current = Cursors.WaitCursor;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_5S_QA_System.View.User_control.mPanelViewExport));
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.btnExport = new System.Windows.Forms.Button();
		this.btnDown = new System.Windows.Forms.Button();
		this.btnUp = new System.Windows.Forms.Button();
		this.btnDelete = new System.Windows.Forms.Button();
		this.cbbSample = new System.Windows.Forms.ComboBox();
		this.btnPrevious = new System.Windows.Forms.Button();
		this.btnNext = new System.Windows.Forms.Button();
		this.panelResize = new System.Windows.Forms.Panel();
		this.panelFooter = new System.Windows.Forms.Panel();
		this.panelTitle = new System.Windows.Forms.Panel();
		this.tblPanelFooter = new System.Windows.Forms.TableLayoutPanel();
		this.tblPanelTitle = new System.Windows.Forms.TableLayoutPanel();
		this.lblValue = new System.Windows.Forms.Label();
		this.lblTitle = new System.Windows.Forms.Label();
		this.dgvMeas = new System.Windows.Forms.DataGridView();
		this.tpanelMeasurement = new System.Windows.Forms.TableLayoutPanel();
		this.lblWarning = new System.Windows.Forms.Label();
		this.lblWarn = new System.Windows.Forms.Label();
		this.lblEditResult = new System.Windows.Forms.Label();
		this.lblEdit = new System.Windows.Forms.Label();
		this.lblMeas = new System.Windows.Forms.Label();
		this.lblOK = new System.Windows.Forms.Label();
		this.lblNG = new System.Windows.Forms.Label();
		this.lblEmpty = new System.Windows.Forms.Label();
		this.lblJudgeOK = new System.Windows.Forms.Label();
		this.lblJudgeNG = new System.Windows.Forms.Label();
		this.lblJudgeEmpty = new System.Windows.Forms.Label();
		this.panelView = new System.Windows.Forms.Panel();
		this.dgvRequestDetail = new System.Windows.Forms.DataGridView();
		this.NoDetail = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.CavityDetail = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.PinDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ShotMold = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ProduceNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.IdDetail = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dgvComment = new System.Windows.Forms.DataGridView();
		this.cNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Content = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Link = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.contextMenuStripMain = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.main_fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
		this.lblComment = new System.Windows.Forms.Label();
		this.lblTotalRow = new System.Windows.Forms.Label();
		this.lblCommentTotal = new System.Windows.Forms.Label();
		this.dgvFooter = new System.Windows.Forms.DataGridView();
		this.footer_title = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.footer_value = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dgvOther = new System.Windows.Forms.DataGridView();
		this.other_title = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.other_value = new System.Windows.Forms.DataGridViewImageColumn();
		this.dgvContent = new System.Windows.Forms.DataGridView();
		this.content_title = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.content_value = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.No = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
		this.tblPanelFooter.SuspendLayout();
		this.tblPanelTitle.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvMeas).BeginInit();
		this.tpanelMeasurement.SuspendLayout();
		this.panelView.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvRequestDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.dgvComment).BeginInit();
		this.contextMenuStripMain.SuspendLayout();
		this.tableLayoutPanel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvFooter).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.dgvOther).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.dgvContent).BeginInit();
		base.SuspendLayout();
		this.btnExport.AutoSize = true;
		this.btnExport.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnExport.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnExport.FlatAppearance.BorderSize = 0;
		this.btnExport.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
		this.btnExport.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
		this.btnExport.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnExport.Location = new System.Drawing.Point(3, 3);
		this.btnExport.Name = "btnExport";
		this.btnExport.Size = new System.Drawing.Size(83, 26);
		this.btnExport.TabIndex = 1;
		this.btnExport.Text = "Complete";
		this.toolTipMain.SetToolTip(this.btnExport, "Confirm complete");
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
		this.btnDown.UseVisualStyleBackColor = false;
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
		this.btnUp.UseVisualStyleBackColor = false;
		this.btnUp.Click += new System.EventHandler(btnUp_Click);
		this.btnDelete.AutoSize = true;
		this.btnDelete.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnDelete.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnDelete.FlatAppearance.BorderSize = 0;
		this.btnDelete.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
		this.btnDelete.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
		this.btnDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDelete.Location = new System.Drawing.Point(92, 3);
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(63, 26);
		this.btnDelete.TabIndex = 2;
		this.btnDelete.Text = "Delete";
		this.toolTipMain.SetToolTip(this.btnDelete, "Confirm delete");
		this.btnDelete.UseVisualStyleBackColor = true;
		this.cbbSample.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cbbSample.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cbbSample.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbbSample.Dock = System.Windows.Forms.DockStyle.Top;
		this.cbbSample.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbbSample.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cbbSample.FormattingEnabled = true;
		this.cbbSample.Location = new System.Drawing.Point(138, 2);
		this.cbbSample.Margin = new System.Windows.Forms.Padding(0);
		this.cbbSample.MaxLength = 250;
		this.cbbSample.Name = "cbbSample";
		this.cbbSample.Size = new System.Drawing.Size(60, 24);
		this.cbbSample.TabIndex = 2;
		this.toolTipMain.SetToolTip(this.cbbSample, "Select sample no.");
		this.cbbSample.SelectedIndexChanged += new System.EventHandler(cbbSample_SelectedIndexChanged);
		this.btnPrevious.BackColor = System.Drawing.SystemColors.Control;
		this.btnPrevious.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnPrevious.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.btnPrevious.Location = new System.Drawing.Point(108, 2);
		this.btnPrevious.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
		this.btnPrevious.Name = "btnPrevious";
		this.btnPrevious.Size = new System.Drawing.Size(30, 25);
		this.btnPrevious.TabIndex = 1;
		this.btnPrevious.Text = "<";
		this.toolTipMain.SetToolTip(this.btnPrevious, "Goto previous sample");
		this.btnPrevious.UseVisualStyleBackColor = false;
		this.btnPrevious.Click += new System.EventHandler(btnPrevious_Click);
		this.btnNext.BackColor = System.Drawing.SystemColors.Control;
		this.btnNext.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnNext.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.btnNext.Location = new System.Drawing.Point(198, 2);
		this.btnNext.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
		this.btnNext.Name = "btnNext";
		this.btnNext.Size = new System.Drawing.Size(30, 25);
		this.btnNext.TabIndex = 3;
		this.btnNext.Text = ">";
		this.toolTipMain.SetToolTip(this.btnNext, "Goto next sample");
		this.btnNext.UseVisualStyleBackColor = false;
		this.btnNext.Click += new System.EventHandler(btnNext_Click);
		this.panelResize.Dock = System.Windows.Forms.DockStyle.Left;
		this.panelResize.Location = new System.Drawing.Point(0, 28);
		this.panelResize.Margin = new System.Windows.Forms.Padding(0);
		this.panelResize.Name = "panelResize";
		this.panelResize.Size = new System.Drawing.Size(4, 604);
		this.panelResize.TabIndex = 144;
		this.panelFooter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panelFooter.Location = new System.Drawing.Point(0, 632);
		this.panelFooter.Name = "panelFooter";
		this.panelFooter.Size = new System.Drawing.Size(700, 1);
		this.panelFooter.TabIndex = 143;
		this.panelTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panelTitle.Dock = System.Windows.Forms.DockStyle.Top;
		this.panelTitle.Location = new System.Drawing.Point(0, 27);
		this.panelTitle.Name = "panelTitle";
		this.panelTitle.Size = new System.Drawing.Size(700, 1);
		this.panelTitle.TabIndex = 142;
		this.tblPanelFooter.AutoSize = true;
		this.tblPanelFooter.ColumnCount = 3;
		this.tblPanelFooter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tblPanelFooter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tblPanelFooter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tblPanelFooter.Controls.Add(this.btnDelete, 0, 0);
		this.tblPanelFooter.Controls.Add(this.btnExport, 0, 0);
		this.tblPanelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.tblPanelFooter.Location = new System.Drawing.Point(0, 633);
		this.tblPanelFooter.Margin = new System.Windows.Forms.Padding(1);
		this.tblPanelFooter.Name = "tblPanelFooter";
		this.tblPanelFooter.RowCount = 1;
		this.tblPanelFooter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32f));
		this.tblPanelFooter.Size = new System.Drawing.Size(700, 32);
		this.tblPanelFooter.TabIndex = 141;
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
		this.tblPanelTitle.TabIndex = 140;
		this.lblValue.AutoSize = true;
		this.lblValue.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblValue.ForeColor = System.Drawing.Color.Red;
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
		this.dgvMeas.AllowUserToAddRows = false;
		this.dgvMeas.AllowUserToDeleteRows = false;
		this.dgvMeas.AllowUserToOrderColumns = true;
		this.dgvMeas.AllowUserToResizeRows = false;
		dataGridViewCellStyle.BackColor = System.Drawing.SystemColors.Control;
		this.dgvMeas.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle;
		this.dgvMeas.BackgroundColor = System.Drawing.SystemColors.Window;
		dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.AppWorkspace;
		dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
		dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
		dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
		dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.dgvMeas.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
		this.dgvMeas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
		this.dgvMeas.Columns.AddRange(this.No, this.name, this.Value, this.Unit, this.Upper, this.Lower, this.LSL, this.USL, this.Cavity, this.Result, this.Judge, this.History, this.MeasurementUnit);
		this.dgvMeas.Dock = System.Windows.Forms.DockStyle.Top;
		this.dgvMeas.EnableHeadersVisualStyles = false;
		this.dgvMeas.Location = new System.Drawing.Point(0, 238);
		this.dgvMeas.Margin = new System.Windows.Forms.Padding(1);
		this.dgvMeas.MultiSelect = false;
		this.dgvMeas.Name = "dgvMeas";
		this.dgvMeas.ReadOnly = true;
		this.dgvMeas.RowHeadersVisible = false;
		this.dgvMeas.RowHeadersWidth = 25;
		this.dgvMeas.Size = new System.Drawing.Size(696, 70);
		this.dgvMeas.TabIndex = 146;
		this.dgvMeas.Sorted += new System.EventHandler(dgvMeas_Sorted);
		this.tpanelMeasurement.AutoSize = true;
		this.tpanelMeasurement.ColumnCount = 15;
		this.tpanelMeasurement.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMeasurement.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMeasurement.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMeasurement.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMeasurement.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tpanelMeasurement.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMeasurement.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMeasurement.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMeasurement.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMeasurement.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMeasurement.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMeasurement.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMeasurement.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMeasurement.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMeasurement.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMeasurement.Controls.Add(this.btnNext, 3, 0);
		this.tpanelMeasurement.Controls.Add(this.btnPrevious, 1, 0);
		this.tpanelMeasurement.Controls.Add(this.cbbSample, 2, 0);
		this.tpanelMeasurement.Controls.Add(this.lblWarning, 8, 0);
		this.tpanelMeasurement.Controls.Add(this.lblWarn, 7, 0);
		this.tpanelMeasurement.Controls.Add(this.lblEditResult, 6, 0);
		this.tpanelMeasurement.Controls.Add(this.lblEdit, 5, 0);
		this.tpanelMeasurement.Controls.Add(this.lblMeas, 0, 0);
		this.tpanelMeasurement.Controls.Add(this.lblOK, 9, 0);
		this.tpanelMeasurement.Controls.Add(this.lblNG, 11, 0);
		this.tpanelMeasurement.Controls.Add(this.lblEmpty, 13, 0);
		this.tpanelMeasurement.Controls.Add(this.lblJudgeOK, 10, 0);
		this.tpanelMeasurement.Controls.Add(this.lblJudgeNG, 12, 0);
		this.tpanelMeasurement.Controls.Add(this.lblJudgeEmpty, 14, 0);
		this.tpanelMeasurement.Dock = System.Windows.Forms.DockStyle.Top;
		this.tpanelMeasurement.Location = new System.Drawing.Point(0, 162);
		this.tpanelMeasurement.Name = "tpanelMeasurement";
		this.tpanelMeasurement.Padding = new System.Windows.Forms.Padding(2);
		this.tpanelMeasurement.RowCount = 1;
		this.tpanelMeasurement.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tpanelMeasurement.Size = new System.Drawing.Size(696, 30);
		this.tpanelMeasurement.TabIndex = 152;
		this.lblWarning.AutoSize = true;
		this.lblWarning.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblWarning.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblWarning.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lblWarning.Location = new System.Drawing.Point(489, 2);
		this.lblWarning.Name = "lblWarning";
		this.lblWarning.Size = new System.Drawing.Size(15, 26);
		this.lblWarning.TabIndex = 158;
		this.lblWarning.Text = "0";
		this.lblWarning.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblWarn.AutoSize = true;
		this.lblWarn.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblWarn.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lblWarn.Location = new System.Drawing.Point(423, 2);
		this.lblWarn.Name = "lblWarn";
		this.lblWarn.Size = new System.Drawing.Size(60, 26);
		this.lblWarn.TabIndex = 157;
		this.lblWarn.Text = "Warning:";
		this.lblWarn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblEditResult.AutoSize = true;
		this.lblEditResult.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblEditResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblEditResult.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lblEditResult.Location = new System.Drawing.Point(402, 2);
		this.lblEditResult.Name = "lblEditResult";
		this.lblEditResult.Size = new System.Drawing.Size(15, 26);
		this.lblEditResult.TabIndex = 156;
		this.lblEditResult.Text = "0";
		this.lblEditResult.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblEdit.AutoSize = true;
		this.lblEdit.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblEdit.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lblEdit.Location = new System.Drawing.Point(347, 2);
		this.lblEdit.Name = "lblEdit";
		this.lblEdit.Size = new System.Drawing.Size(49, 26);
		this.lblEdit.TabIndex = 155;
		this.lblEdit.Text = "Edited:";
		this.lblEdit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblMeas.AutoSize = true;
		this.lblMeas.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblMeas.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblMeas.Location = new System.Drawing.Point(5, 2);
		this.lblMeas.Name = "lblMeas";
		this.lblMeas.Size = new System.Drawing.Size(100, 26);
		this.lblMeas.TabIndex = 146;
		this.lblMeas.Text = "Measurement";
		this.lblMeas.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblOK.AutoSize = true;
		this.lblOK.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblOK.Location = new System.Drawing.Point(510, 2);
		this.lblOK.Name = "lblOK";
		this.lblOK.Size = new System.Drawing.Size(28, 26);
		this.lblOK.TabIndex = 147;
		this.lblOK.Text = "OK:";
		this.lblOK.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblNG.AutoSize = true;
		this.lblNG.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblNG.Location = new System.Drawing.Point(565, 2);
		this.lblNG.Name = "lblNG";
		this.lblNG.Size = new System.Drawing.Size(30, 26);
		this.lblNG.TabIndex = 148;
		this.lblNG.Text = "NG:";
		this.lblNG.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblEmpty.AutoSize = true;
		this.lblEmpty.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblEmpty.Location = new System.Drawing.Point(622, 2);
		this.lblEmpty.Name = "lblEmpty";
		this.lblEmpty.Size = new System.Drawing.Size(48, 26);
		this.lblEmpty.TabIndex = 149;
		this.lblEmpty.Text = "Empty:";
		this.lblEmpty.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblJudgeOK.AutoSize = true;
		this.lblJudgeOK.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblJudgeOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblJudgeOK.ForeColor = System.Drawing.Color.Blue;
		this.lblJudgeOK.Location = new System.Drawing.Point(544, 2);
		this.lblJudgeOK.Name = "lblJudgeOK";
		this.lblJudgeOK.Size = new System.Drawing.Size(15, 26);
		this.lblJudgeOK.TabIndex = 150;
		this.lblJudgeOK.Text = "0";
		this.lblJudgeOK.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblJudgeNG.AutoSize = true;
		this.lblJudgeNG.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblJudgeNG.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblJudgeNG.ForeColor = System.Drawing.Color.Crimson;
		this.lblJudgeNG.Location = new System.Drawing.Point(601, 2);
		this.lblJudgeNG.Name = "lblJudgeNG";
		this.lblJudgeNG.Size = new System.Drawing.Size(15, 26);
		this.lblJudgeNG.TabIndex = 151;
		this.lblJudgeNG.Text = "0";
		this.lblJudgeNG.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblJudgeEmpty.AutoSize = true;
		this.lblJudgeEmpty.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblJudgeEmpty.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblJudgeEmpty.Location = new System.Drawing.Point(676, 2);
		this.lblJudgeEmpty.Name = "lblJudgeEmpty";
		this.lblJudgeEmpty.Size = new System.Drawing.Size(15, 26);
		this.lblJudgeEmpty.TabIndex = 152;
		this.lblJudgeEmpty.Text = "0";
		this.lblJudgeEmpty.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panelView.AutoScroll = true;
		this.panelView.Controls.Add(this.dgvMeas);
		this.panelView.Controls.Add(this.dgvRequestDetail);
		this.panelView.Controls.Add(this.tpanelMeasurement);
		this.panelView.Controls.Add(this.dgvComment);
		this.panelView.Controls.Add(this.tableLayoutPanel1);
		this.panelView.Controls.Add(this.dgvFooter);
		this.panelView.Controls.Add(this.dgvOther);
		this.panelView.Controls.Add(this.dgvContent);
		this.panelView.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panelView.Location = new System.Drawing.Point(4, 28);
		this.panelView.Margin = new System.Windows.Forms.Padding(0);
		this.panelView.Name = "panelView";
		this.panelView.Size = new System.Drawing.Size(696, 604);
		this.panelView.TabIndex = 150;
		this.dgvRequestDetail.AllowUserToAddRows = false;
		this.dgvRequestDetail.AllowUserToDeleteRows = false;
		this.dgvRequestDetail.AllowUserToOrderColumns = true;
		this.dgvRequestDetail.AllowUserToResizeRows = false;
		dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
		this.dgvRequestDetail.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
		this.dgvRequestDetail.BackgroundColor = System.Drawing.SystemColors.Window;
		dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.AppWorkspace;
		dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
		dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
		dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
		dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.dgvRequestDetail.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
		this.dgvRequestDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
		this.dgvRequestDetail.Columns.AddRange(this.NoDetail, this.CavityDetail, this.PinDate, this.ShotMold, this.ProduceNo, this.IdDetail);
		this.dgvRequestDetail.Dock = System.Windows.Forms.DockStyle.Top;
		this.dgvRequestDetail.EnableHeadersVisualStyles = false;
		this.dgvRequestDetail.Location = new System.Drawing.Point(0, 192);
		this.dgvRequestDetail.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.dgvRequestDetail.MultiSelect = false;
		this.dgvRequestDetail.Name = "dgvRequestDetail";
		this.dgvRequestDetail.ReadOnly = true;
		this.dgvRequestDetail.RowHeadersVisible = false;
		this.dgvRequestDetail.RowHeadersWidth = 25;
		this.dgvRequestDetail.Size = new System.Drawing.Size(696, 46);
		this.dgvRequestDetail.TabIndex = 164;
		this.dgvRequestDetail.Visible = false;
		this.NoDetail.DataPropertyName = "No";
		dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		this.NoDetail.DefaultCellStyle = dataGridViewCellStyle5;
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
		this.dgvComment.AllowUserToAddRows = false;
		this.dgvComment.AllowUserToDeleteRows = false;
		this.dgvComment.AllowUserToOrderColumns = true;
		this.dgvComment.AllowUserToResizeRows = false;
		dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
		this.dgvComment.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
		this.dgvComment.BackgroundColor = System.Drawing.SystemColors.Window;
		dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.AppWorkspace;
		dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
		dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
		dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
		dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.dgvComment.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
		this.dgvComment.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
		this.dgvComment.Columns.AddRange(this.cNo, this.Content, this.Link, this.Id);
		this.dgvComment.ContextMenuStrip = this.contextMenuStripMain;
		this.dgvComment.Dock = System.Windows.Forms.DockStyle.Top;
		this.dgvComment.EnableHeadersVisualStyles = false;
		this.dgvComment.Location = new System.Drawing.Point(0, 84);
		this.dgvComment.Margin = new System.Windows.Forms.Padding(1);
		this.dgvComment.MultiSelect = false;
		this.dgvComment.Name = "dgvComment";
		this.dgvComment.ReadOnly = true;
		this.dgvComment.RowHeadersVisible = false;
		this.dgvComment.RowHeadersWidth = 25;
		this.dgvComment.Size = new System.Drawing.Size(696, 78);
		this.dgvComment.TabIndex = 146;
		this.dgvComment.CurrentCellChanged += new System.EventHandler(dgvComment_CurrentCellChanged);
		this.cNo.DataPropertyName = "No";
		dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		this.cNo.DefaultCellStyle = dataGridViewCellStyle8;
		this.cNo.HeaderText = "No";
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
		this.tableLayoutPanel1.AutoSize = true;
		this.tableLayoutPanel1.ColumnCount = 4;
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel1.Controls.Add(this.lblComment, 0, 0);
		this.tableLayoutPanel1.Controls.Add(this.lblTotalRow, 2, 0);
		this.tableLayoutPanel1.Controls.Add(this.lblCommentTotal, 3, 0);
		this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 66);
		this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(1);
		this.tableLayoutPanel1.Name = "tableLayoutPanel1";
		this.tableLayoutPanel1.RowCount = 1;
		this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel1.Size = new System.Drawing.Size(696, 18);
		this.tableLayoutPanel1.TabIndex = 153;
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
		this.lblTotalRow.AutoSize = true;
		this.lblTotalRow.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTotalRow.Location = new System.Drawing.Point(606, 1);
		this.lblTotalRow.Margin = new System.Windows.Forms.Padding(1);
		this.lblTotalRow.Name = "lblTotalRow";
		this.lblTotalRow.Size = new System.Drawing.Size(72, 16);
		this.lblTotalRow.TabIndex = 149;
		this.lblTotalRow.Text = "Total rows:";
		this.lblCommentTotal.AutoSize = true;
		this.lblCommentTotal.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCommentTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCommentTotal.Location = new System.Drawing.Point(680, 1);
		this.lblCommentTotal.Margin = new System.Windows.Forms.Padding(1);
		this.lblCommentTotal.Name = "lblCommentTotal";
		this.lblCommentTotal.Size = new System.Drawing.Size(15, 16);
		this.lblCommentTotal.TabIndex = 152;
		this.lblCommentTotal.Text = "0";
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
		this.dgvFooter.Size = new System.Drawing.Size(696, 22);
		this.dgvFooter.TabIndex = 3;
		this.dgvFooter.Visible = false;
		dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.footer_title.DefaultCellStyle = dataGridViewCellStyle9;
		this.footer_title.HeaderText = "Title";
		this.footer_title.Name = "footer_title";
		this.footer_title.ReadOnly = true;
		this.footer_title.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.footer_title.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.footer_title.Width = 140;
		this.footer_value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		this.footer_value.DefaultCellStyle = dataGridViewCellStyle10;
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
		this.dgvOther.Size = new System.Drawing.Size(696, 22);
		this.dgvOther.TabIndex = 4;
		this.dgvOther.Visible = false;
		this.dgvOther.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvOther_CellDoubleClick);
		dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
		this.other_title.DefaultCellStyle = dataGridViewCellStyle11;
		this.other_title.HeaderText = "Title";
		this.other_title.Name = "other_title";
		this.other_title.ReadOnly = true;
		this.other_title.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.other_title.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.other_title.Width = 140;
		this.other_value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle12.NullValue = resources.GetObject("dataGridViewCellStyle19.NullValue");
		this.other_value.DefaultCellStyle = dataGridViewCellStyle12;
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
		this.dgvContent.Size = new System.Drawing.Size(696, 22);
		this.dgvContent.TabIndex = 1;
		this.dgvContent.Visible = false;
		dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.content_title.DefaultCellStyle = dataGridViewCellStyle13;
		this.content_title.HeaderText = "Title";
		this.content_title.Name = "content_title";
		this.content_title.ReadOnly = true;
		this.content_title.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.content_title.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.content_title.Width = 140;
		this.content_value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		this.content_value.DefaultCellStyle = dataGridViewCellStyle14;
		this.content_value.HeaderText = "Value";
		this.content_value.Name = "content_value";
		this.content_value.ReadOnly = true;
		this.content_value.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.content_value.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.No.DataPropertyName = "No";
		dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		this.No.DefaultCellStyle = dataGridViewCellStyle15;
		this.No.HeaderText = "No.";
		this.No.Name = "No";
		this.No.ReadOnly = true;
		this.No.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.No.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.No.Width = 40;
		this.name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.name.DataPropertyName = "Name";
		this.name.FillWeight = 30f;
		this.name.HeaderText = "Measurement";
		this.name.Name = "name";
		this.name.ReadOnly = true;
		this.Value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Value.DataPropertyName = "Value";
		dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.Value.DefaultCellStyle = dataGridViewCellStyle16;
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
		dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.Upper.DefaultCellStyle = dataGridViewCellStyle17;
		this.Upper.FillWeight = 15f;
		this.Upper.HeaderText = "Upper ";
		this.Upper.Name = "Upper";
		this.Upper.ReadOnly = true;
		this.Lower.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Lower.DataPropertyName = "Lower";
		dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.Lower.DefaultCellStyle = dataGridViewCellStyle18;
		this.Lower.FillWeight = 15f;
		this.Lower.HeaderText = "Lower";
		this.Lower.Name = "Lower";
		this.Lower.ReadOnly = true;
		this.LSL.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.LSL.DataPropertyName = "LSL";
		dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.LSL.DefaultCellStyle = dataGridViewCellStyle19;
		this.LSL.FillWeight = 20f;
		this.LSL.HeaderText = "LSL";
		this.LSL.Name = "LSL";
		this.LSL.ReadOnly = true;
		this.USL.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.USL.DataPropertyName = "USL";
		dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.USL.DefaultCellStyle = dataGridViewCellStyle20;
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
		dataGridViewCellStyle21.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.Result.DefaultCellStyle = dataGridViewCellStyle21;
		this.Result.FillWeight = 20f;
		this.Result.HeaderText = "Result";
		this.Result.Name = "Result";
		this.Result.ReadOnly = true;
		this.Judge.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Judge.DataPropertyName = "Judge";
		this.Judge.FillWeight = 15f;
		this.Judge.HeaderText = "Judge";
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
		base.Controls.Add(this.panelResize);
		base.Controls.Add(this.panelFooter);
		base.Controls.Add(this.panelTitle);
		base.Controls.Add(this.tblPanelFooter);
		base.Controls.Add(this.tblPanelTitle);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "mPanelViewExport";
		base.Size = new System.Drawing.Size(700, 665);
		base.Load += new System.EventHandler(mPanelViewExport_Load);
		this.tblPanelFooter.ResumeLayout(false);
		this.tblPanelFooter.PerformLayout();
		this.tblPanelTitle.ResumeLayout(false);
		this.tblPanelTitle.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvMeas).EndInit();
		this.tpanelMeasurement.ResumeLayout(false);
		this.tpanelMeasurement.PerformLayout();
		this.panelView.ResumeLayout(false);
		this.panelView.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvRequestDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.dgvComment).EndInit();
		this.contextMenuStripMain.ResumeLayout(false);
		this.tableLayoutPanel1.ResumeLayout(false);
		this.tableLayoutPanel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvFooter).EndInit();
		((System.ComponentModel.ISupportInitialize)this.dgvOther).EndInit();
		((System.ComponentModel.ISupportInitialize)this.dgvContent).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
