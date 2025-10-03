using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using _5S_QA_Client.Controls;
using _5S_QA_Client.Properties;
using _5S_QA_DL.APIContext;
using _5S_QA_Entities.Dtos;
using _5S_QA_Entities.Models;
using Newtonsoft.Json;
using WpfUserImage;
using MessageBox = System.Windows.Forms.MessageBox;

namespace _5S_QA_Client;

public class mPanelImage : UserControl
{
	private UcWpf ucWpf;

	private Guid mId;

	private frmResultView mForm;

	private string mRemember;

	private bool isChange;

	private IContainer components = null;

	private Panel panelResize;

	private Button btnClose;

	private ContextMenuStrip contextMenuStripComment;

	private ToolStripMenuItem main_refreshToolStripMenuItem;

	private Panel panelDisplayDraw;

	private Panel panelComment;

	private TableLayoutPanel tableLayoutPanel1;

	private Label lblcTitle;

	private Label lblcTotal;

	private Label lblCommentTotal;

	private Panel panelCommentResize;

	private Panel panelImage;

	private PictureBox ptbImage;

	private ElementHost elementHostZoomImage;

	private ToolTip toolTipMain;

	public Button btnCommentRefresh;

	private Panel panelLineV;

	private Panel panelLineH;

	private ToolStripSeparator toolStripSeparatorView;

	private ToolStripMenuItem main_fileToolStripMenuItem;

	private OpenFileDialog openFileDialogMain;

	private DataGridView dgvComment;

	private DataGridViewTextBoxColumn cNo;

	private DataGridViewTextBoxColumn cRequestId;

	private DataGridViewTextBoxColumn cContent;

	private DataGridViewTextBoxColumn cLink;

	private DataGridViewButtonColumn cDelete;

	private DataGridViewButtonColumn cFolder;

	private DataGridViewTextBoxColumn cId;

	private DataGridViewTextBoxColumn cCreated;

	private DataGridViewTextBoxColumn cModified;

	private DataGridViewTextBoxColumn cCreatedBy;

	private DataGridViewTextBoxColumn cModifiedBy;

	private DataGridViewTextBoxColumn cIsActivated;

	public mPanelImage()
	{
		InitializeComponent();
		Common.setControls(this, toolTipMain, new List<ContextMenuStrip> { contextMenuStripComment });
	}

	private void mPanelImage_Load(object sender, EventArgs e)
	{
		ControlResize.Init(panelResize, this, ControlResize.Direction.Horizontal, Cursors.SizeWE);
		mForm = base.ParentForm as frmResultView;
		base.Visible = false;
	}

	public void Init(Image img, Guid id, bool istool = false)
	{
		mId = id;
		Invoke((MethodInvoker)delegate
		{
			Display(istool);
			if (img == null)
			{
				panelCommentResize.Height = btnClose.Height;
				panelComment.Dock = DockStyle.Fill;
				panelImage.Visible = false;
			}
			else
			{
				ControlResize.Init(panelCommentResize, panelComment, ControlResize.Direction.Vertical, Cursors.SizeNS);
				ptbImage.Image = img;
				ptbImage_Click(ptbImage, null);
				base.Visible = true;
			}
			Load_dgvComment();
		});
	}

	public void Display(bool istool = false)
	{
		panelDisplayDraw.Visible = !istool;
	}

	public void Move_ImageWithMeas(string str)
	{
		if (panelImage.Visible && ucWpf != null && !string.IsNullOrEmpty(str))
		{
			System.Windows.Point pCenter = new System.Windows.Point(elementHostZoomImage.Width / 2, elementHostZoomImage.Height / 2);
			string[] array = str.Split('#');
			if (array.Length >= 3)
			{
				double.TryParse(array[0], out var result);
				double.TryParse(array[1], out var result2);
				double.TryParse(array[2], out var result3);
				System.Windows.Point pMove = new System.Windows.Point(result, result2);
				ucWpf.Move_ImageAtPoint(pCenter, pMove, result3);
			}
		}
	}

	private void Load_dgvComment()
	{
		Cursor.Current = Cursors.WaitCursor;
		try
		{
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "RequestId=@0";
			queryArgs.PredicateParameters = new string[1] { mId.ToString() };
			queryArgs.Order = "Created";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs body = queryArgs;
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/Comment/Gets").Result;
			dgvComment.DataSource = Common.getDataTable<CommentViewModel>(result);
			dgvComment.Refresh();
			lblCommentTotal.Text = (dgvComment.RowCount - 1).ToString();
			dgvComment_Sorted(dgvComment, null);
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

	private void Save_Comment()
	{
		try
		{
			Guid.TryParse(dgvComment.CurrentRow.Cells["cId"].Value.ToString(), out var result);
			CommentViewModel commentViewModel = new CommentViewModel
			{
				Id = result,
				RequestId = mId,
				Content = dgvComment.CurrentRow.Cells["cContent"].Value.ToString()
			};
			Cursor.Current = Cursors.WaitCursor;
			string text = string.Empty;
			if (dgvComment.CurrentRow.Cells["cLink"].Value != null && !string.IsNullOrEmpty(dgvComment.CurrentRow.Cells["cLink"].Value.ToString()))
			{
				text = dgvComment.CurrentRow.Cells["cLink"].Value.ToString();
			}
			if (File.Exists(text))
			{
				using (File.Open(text, FileMode.Open))
				{
				}
			}
			if (!result.Equals(Guid.Empty))
			{
				commentViewModel.Link = (string.IsNullOrEmpty(text) ? null : Path.GetFileName(text));
			}
			ResponseDto result2 = frmLogin.client.SaveAsync(commentViewModel, "/api/Comment/Save").Result;
			if (!result2.Success)
			{
				throw new Exception(result2.Messages.ElementAt(0).Message);
			}
			if (result.Equals(Guid.Empty))
			{
				CommentViewModel commentViewModel2 = JsonConvert.DeserializeObject<CommentViewModel>(result2.Data.ToString());
				if (File.Exists(text))
				{
					using FileStream data = File.OpenRead(text);
					FileParameter file = new FileParameter(data, text);
					ResponseDto result3 = frmLogin.client.ImportAsync(commentViewModel2.Id, file, "/api/Comment/UpdateFile/{id}").Result;
					if (!result3.Success)
					{
						MessageBox.Show(result3.Messages.First().Message, result3.Messages.First().Code, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
				}
			}
			else if (File.Exists(text))
			{
				using FileStream data2 = File.OpenRead(text);
				FileParameter file2 = new FileParameter(data2, text);
				ResponseDto result4 = frmLogin.client.ImportAsync(result, file2, "/api/Comment/UpdateFile/{id}").Result;
				if (!result4.Success)
				{
					MessageBox.Show(result4.Messages.First().Message, result4.Messages.First().Code, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
			else if (string.IsNullOrEmpty(text))
			{
				Stream data3 = new MemoryStream();
				FileParameter file3 = new FileParameter(data3);
				ResponseDto result5 = frmLogin.client.ImportAsync(result, file3, "/api/Comment/UpdateFile/{id}").Result;
				if (!result5.Success)
				{
					MessageBox.Show(result5.Messages.First().Message, result5.Messages.First().Code, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
			mForm.isClose = false;
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
			isChange = true;
		}
	}

	private void main_refreshToolStripMenuItem_Click(object sender, EventArgs e)
	{
		Load_dgvComment();
	}

	private void btnClose_Click(object sender, EventArgs e)
	{
		base.Visible = false;
	}

	private void ptbImage_Click(object sender, EventArgs e)
	{
		elementHostZoomImage.Child = null;
		ucWpf = new UcWpf(Common.ToBitmapImage((Bitmap)ptbImage.Image));
		elementHostZoomImage.Child = (UIElement)(object)ucWpf;
	}

	private void dgvComment_Sorted(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		DataGridView dataGridView = sender as DataGridView;
		foreach (DataGridViewRow item in (IEnumerable)dataGridView.Rows)
		{
			if (!item.IsNewRow)
			{
				item.Cells["cNo"].Value = item.Index + 1;
				item.Cells["cNo"].ToolTipText = string.Format("{0}: {1}\n", item.Cells["cCreated"].OwningColumn.HeaderText, item.Cells["cCreated"].Value) + string.Format("{0}: {1}\n", item.Cells["cModified"].OwningColumn.HeaderText, item.Cells["cModified"].Value) + string.Format("{0}: {1}\n", item.Cells["cCreatedBy"].OwningColumn.HeaderText, item.Cells["cCreatedBy"].Value) + string.Format("{0}: {1}\n", item.Cells["cModifiedBy"].OwningColumn.HeaderText, item.Cells["cModifiedBy"].Value) + string.Format("{0}: {1}", item.Cells["cIsActivated"].OwningColumn.HeaderText, item.Cells["cIsActivated"].Value);
			}
		}
	}

	private void btnCommentRefresh_Click(object sender, EventArgs e)
	{
		main_refreshToolStripMenuItem.PerformClick();
	}

	private void dgvComment_CurrentCellChanged(object sender, EventArgs e)
	{
		DataGridView dataGridView = sender as DataGridView;
		if (dataGridView.CurrentCell != null)
		{
			main_fileToolStripMenuItem.Visible = !string.IsNullOrEmpty(dataGridView.CurrentRow.Cells["cLink"].Value.ToString());
			toolStripSeparatorView.Visible = !string.IsNullOrEmpty(dataGridView.CurrentRow.Cells["cLink"].Value.ToString());
			mRemember = ((dataGridView.CurrentCell.Value == null) ? string.Empty : dataGridView.CurrentCell.Value.ToString());
		}
		else
		{
			main_fileToolStripMenuItem.Visible = false;
			toolStripSeparatorView.Visible = false;
		}
	}

	private void dgvComment_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
	{
		Guid.TryParse(dgvComment.CurrentRow.Cells["cId"].Value.ToString(), out var result);
		if (result == Guid.Empty)
		{
			MessageBox.Show(Common.getTextLanguage(this, "wSelectRow"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			e.Cancel = true;
			return;
		}
		if (MessageBox.Show(string.Format("{0}{1}", Common.getTextLanguage(this, "wSureDelete"), dgvComment.CurrentRow.Cells["cContent"].Value), Common.getTextLanguage(this, "QUESTION"), MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.Yes))
		{
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				ResponseDto result2 = frmLogin.client.DeleteAsync(result, "/api/Comment/Delete/{id}").Result;
				if (!result2.Success)
				{
					throw new Exception(result2.Messages.ElementAt(0).Message);
				}
				mForm.isClose = false;
				isChange = true;
				return;
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
				e.Cancel = true;
				return;
			}
		}
		e.Cancel = true;
	}

	private void dgvComment_CellEndEdit(object sender, DataGridViewCellEventArgs e)
	{
		DataGridView dataGridView = sender as DataGridView;
		if (dataGridView.CurrentCell.Value != null && mRemember == dataGridView.CurrentCell.Value.ToString())
		{
			return;
		}
		if (dataGridView.CurrentCell.Value == null || string.IsNullOrEmpty(dataGridView.CurrentCell.Value.ToString()))
		{
			if (!dataGridView.CurrentRow.IsNewRow)
			{
				isChange = true;
			}
		}
		else
		{
			Save_Comment();
		}
	}

	private void dgvComment_SelectionChanged(object sender, EventArgs e)
	{
		if (isChange)
		{
			isChange = false;
			main_refreshToolStripMenuItem.PerformClick();
		}
	}

	private void dgvComment_CellContentClick(object sender, DataGridViewCellEventArgs e)
	{
		DataGridView dataGridView = sender as DataGridView;
		if (!(dataGridView.Columns[e.ColumnIndex] is DataGridViewButtonColumn) || e.RowIndex < 0)
		{
			return;
		}
		string name = dataGridView.CurrentCell.OwningColumn.Name;
		string text = name;
		if (!(text == "cDelete"))
		{
			if (text == "cFolder" && openFileDialogMain.ShowDialog().Equals(DialogResult.OK))
			{
				dataGridView.CurrentRow.Cells["cLink"].Value = openFileDialogMain.FileName;
			}
		}
		else
		{
			dataGridView.CurrentRow.Cells["cLink"].Value = null;
		}
		Save_Comment();
		main_refreshToolStripMenuItem.PerformClick();
	}

	private void main_fileToolStripMenuItem_Click(object sender, EventArgs e)
	{
		Guid.TryParse(dgvComment.CurrentRow.Cells["cId"].Value.ToString(), out var result);
		if (result.Equals(Guid.Empty))
		{
			MessageBox.Show(Common.getTextLanguage(this, "wSelectRow"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		if (string.IsNullOrEmpty(dgvComment.CurrentRow.Cells["cLink"].Value.ToString()))
		{
			MessageBox.Show(Common.getTextLanguage(this, "wNoFile"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		try
		{
			ExportExcelDto exportExcelDto = frmLogin.client.DownloadAsync(result, "/api/Comment/DownloadFile/{id}").Result ?? throw new Exception(Common.getTextLanguage(this, "wHasntFile"));
			string path = exportExcelDto.FileName.Replace("\"", "");
			string text = Path.Combine("C:\\Windows\\Temp\\5SQA_Client", "VIEWS");
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
		this.panelResize = new System.Windows.Forms.Panel();
		this.contextMenuStripComment = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.main_refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparatorView = new System.Windows.Forms.ToolStripSeparator();
		this.main_fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.btnClose = new System.Windows.Forms.Button();
		this.panelDisplayDraw = new System.Windows.Forms.Panel();
		this.panelImage = new System.Windows.Forms.Panel();
		this.panelLineH = new System.Windows.Forms.Panel();
		this.panelLineV = new System.Windows.Forms.Panel();
		this.ptbImage = new System.Windows.Forms.PictureBox();
		this.elementHostZoomImage = new System.Windows.Forms.Integration.ElementHost();
		this.panelComment = new System.Windows.Forms.Panel();
		this.dgvComment = new System.Windows.Forms.DataGridView();
		this.cNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.cRequestId = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.cContent = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.cLink = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.cDelete = new System.Windows.Forms.DataGridViewButtonColumn();
		this.cFolder = new System.Windows.Forms.DataGridViewButtonColumn();
		this.cId = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.cCreated = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.cModified = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.cCreatedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.cModifiedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.cIsActivated = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
		this.btnCommentRefresh = new System.Windows.Forms.Button();
		this.lblcTitle = new System.Windows.Forms.Label();
		this.lblcTotal = new System.Windows.Forms.Label();
		this.lblCommentTotal = new System.Windows.Forms.Label();
		this.panelCommentResize = new System.Windows.Forms.Panel();
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.openFileDialogMain = new System.Windows.Forms.OpenFileDialog();
		this.contextMenuStripComment.SuspendLayout();
		this.panelDisplayDraw.SuspendLayout();
		this.panelImage.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.ptbImage).BeginInit();
		this.panelComment.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvComment).BeginInit();
		this.tableLayoutPanel1.SuspendLayout();
		base.SuspendLayout();
		this.panelResize.Dock = System.Windows.Forms.DockStyle.Left;
		this.panelResize.Location = new System.Drawing.Point(0, 0);
		this.panelResize.Margin = new System.Windows.Forms.Padding(1);
		this.panelResize.Name = "panelResize";
		this.panelResize.Size = new System.Drawing.Size(3, 500);
		this.panelResize.TabIndex = 147;
		this.contextMenuStripComment.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.main_refreshToolStripMenuItem, this.toolStripSeparatorView, this.main_fileToolStripMenuItem });
		this.contextMenuStripComment.Name = "contextMenuStripStaff";
		this.contextMenuStripComment.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
		this.contextMenuStripComment.Size = new System.Drawing.Size(119, 54);
		this.main_refreshToolStripMenuItem.Name = "main_refreshToolStripMenuItem";
		this.main_refreshToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
		this.main_refreshToolStripMenuItem.Text = "Refresh";
		this.main_refreshToolStripMenuItem.Click += new System.EventHandler(main_refreshToolStripMenuItem_Click);
		this.toolStripSeparatorView.Name = "toolStripSeparatorView";
		this.toolStripSeparatorView.Size = new System.Drawing.Size(115, 6);
		this.main_fileToolStripMenuItem.Name = "main_fileToolStripMenuItem";
		this.main_fileToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
		this.main_fileToolStripMenuItem.Text = "View file";
		this.main_fileToolStripMenuItem.Click += new System.EventHandler(main_fileToolStripMenuItem_Click);
		this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnClose.Location = new System.Drawing.Point(0, 0);
		this.btnClose.Margin = new System.Windows.Forms.Padding(0);
		this.btnClose.Name = "btnClose";
		this.btnClose.Size = new System.Drawing.Size(26, 26);
		this.btnClose.TabIndex = 151;
		this.btnClose.Text = "X";
		this.toolTipMain.SetToolTip(this.btnClose, "Close");
		this.btnClose.UseVisualStyleBackColor = true;
		this.btnClose.Click += new System.EventHandler(btnClose_Click);
		this.panelDisplayDraw.Controls.Add(this.panelImage);
		this.panelDisplayDraw.Controls.Add(this.panelComment);
		this.panelDisplayDraw.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panelDisplayDraw.Location = new System.Drawing.Point(0, 0);
		this.panelDisplayDraw.Name = "panelDisplayDraw";
		this.panelDisplayDraw.Size = new System.Drawing.Size(351, 500);
		this.panelDisplayDraw.TabIndex = 152;
		this.panelImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panelImage.Controls.Add(this.panelLineH);
		this.panelImage.Controls.Add(this.panelLineV);
		this.panelImage.Controls.Add(this.ptbImage);
		this.panelImage.Controls.Add(this.elementHostZoomImage);
		this.panelImage.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panelImage.Location = new System.Drawing.Point(0, 0);
		this.panelImage.Name = "panelImage";
		this.panelImage.Size = new System.Drawing.Size(351, 250);
		this.panelImage.TabIndex = 150;
		this.panelLineH.Anchor = System.Windows.Forms.AnchorStyles.None;
		this.panelLineH.BackColor = System.Drawing.Color.Red;
		this.panelLineH.Location = new System.Drawing.Point(175, 100);
		this.panelLineH.Name = "panelLineH";
		this.panelLineH.Size = new System.Drawing.Size(1, 50);
		this.panelLineH.TabIndex = 151;
		this.panelLineV.Anchor = System.Windows.Forms.AnchorStyles.None;
		this.panelLineV.BackColor = System.Drawing.Color.Red;
		this.panelLineV.Location = new System.Drawing.Point(150, 125);
		this.panelLineV.Name = "panelLineV";
		this.panelLineV.Size = new System.Drawing.Size(50, 1);
		this.panelLineV.TabIndex = 150;
		this.ptbImage.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ptbImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ptbImage.Cursor = System.Windows.Forms.Cursors.Hand;
		this.ptbImage.ErrorImage = _5S_QA_Client.Properties.Resources._5S_QA_C;
		this.ptbImage.Image = _5S_QA_Client.Properties.Resources._5S_QA_C;
		this.ptbImage.InitialImage = _5S_QA_Client.Properties.Resources._5S_QA_C;
		this.ptbImage.Location = new System.Drawing.Point(299, 0);
		this.ptbImage.Name = "ptbImage";
		this.ptbImage.Size = new System.Drawing.Size(50, 50);
		this.ptbImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.ptbImage.TabIndex = 149;
		this.ptbImage.TabStop = false;
		this.toolTipMain.SetToolTip(this.ptbImage, "Reset image");
		this.ptbImage.Click += new System.EventHandler(ptbImage_Click);
		this.elementHostZoomImage.Dock = System.Windows.Forms.DockStyle.Fill;
		this.elementHostZoomImage.Location = new System.Drawing.Point(0, 0);
		this.elementHostZoomImage.Name = "elementHostZoomImage";
		this.elementHostZoomImage.Size = new System.Drawing.Size(349, 248);
		this.elementHostZoomImage.TabIndex = 148;
		this.elementHostZoomImage.Child = null;
		this.panelComment.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panelComment.Controls.Add(this.dgvComment);
		this.panelComment.Controls.Add(this.tableLayoutPanel1);
		this.panelComment.Controls.Add(this.panelCommentResize);
		this.panelComment.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panelComment.Location = new System.Drawing.Point(0, 250);
		this.panelComment.Name = "panelComment";
		this.panelComment.Size = new System.Drawing.Size(351, 250);
		this.panelComment.TabIndex = 151;
		this.dgvComment.AllowUserToOrderColumns = true;
		this.dgvComment.AllowUserToResizeRows = false;
		dataGridViewCellStyle.BackColor = System.Drawing.SystemColors.Control;
		this.dgvComment.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle;
		this.dgvComment.BackgroundColor = System.Drawing.SystemColors.Window;
		dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.AppWorkspace;
		dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
		dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
		dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
		dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.dgvComment.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
		this.dgvComment.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
		this.dgvComment.Columns.AddRange(this.cNo, this.cRequestId, this.cContent, this.cLink, this.cDelete, this.cFolder, this.cId, this.cCreated, this.cModified, this.cCreatedBy, this.cModifiedBy, this.cIsActivated);
		this.dgvComment.ContextMenuStrip = this.contextMenuStripComment;
		this.dgvComment.Dock = System.Windows.Forms.DockStyle.Fill;
		this.dgvComment.EnableHeadersVisualStyles = false;
		this.dgvComment.Location = new System.Drawing.Point(0, 29);
		this.dgvComment.Margin = new System.Windows.Forms.Padding(1);
		this.dgvComment.MultiSelect = false;
		this.dgvComment.Name = "dgvComment";
		this.dgvComment.RowHeadersWidth = 25;
		this.dgvComment.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
		this.dgvComment.Size = new System.Drawing.Size(349, 219);
		this.dgvComment.TabIndex = 157;
		this.dgvComment.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvComment_CellContentClick);
		this.dgvComment.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(dgvComment_CellEndEdit);
		this.dgvComment.CurrentCellChanged += new System.EventHandler(dgvComment_CurrentCellChanged);
		this.dgvComment.SelectionChanged += new System.EventHandler(dgvComment_SelectionChanged);
		this.dgvComment.Sorted += new System.EventHandler(dgvComment_Sorted);
		this.dgvComment.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(dgvComment_UserDeletingRow);
		this.cNo.DataPropertyName = "No";
		dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		this.cNo.DefaultCellStyle = dataGridViewCellStyle3;
		this.cNo.HeaderText = "No.";
		this.cNo.Name = "cNo";
		this.cNo.ReadOnly = true;
		this.cNo.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.cNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.cNo.Width = 40;
		this.cRequestId.DataPropertyName = "RequestId";
		this.cRequestId.HeaderText = "Request id";
		this.cRequestId.Name = "cRequestId";
		this.cRequestId.Visible = false;
		this.cContent.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.cContent.DataPropertyName = "Content";
		this.cContent.FillWeight = 50f;
		this.cContent.HeaderText = "Content";
		this.cContent.Name = "cContent";
		this.cLink.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.cLink.DataPropertyName = "Link";
		this.cLink.FillWeight = 50f;
		this.cLink.HeaderText = "File";
		this.cLink.Name = "cLink";
		this.cLink.ReadOnly = true;
		this.cDelete.HeaderText = "";
		this.cDelete.Name = "cDelete";
		this.cDelete.Text = "X";
		this.cDelete.UseColumnTextForButtonValue = true;
		this.cDelete.Width = 22;
		this.cFolder.HeaderText = "";
		this.cFolder.Name = "cFolder";
		this.cFolder.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		this.cFolder.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
		this.cFolder.Text = "...";
		this.cFolder.UseColumnTextForButtonValue = true;
		this.cFolder.Width = 30;
		this.cId.DataPropertyName = "Id";
		this.cId.HeaderText = "Id";
		this.cId.Name = "cId";
		this.cId.Visible = false;
		this.cCreated.DataPropertyName = "Created";
		this.cCreated.HeaderText = "Created";
		this.cCreated.Name = "cCreated";
		this.cCreated.Visible = false;
		this.cModified.DataPropertyName = "Modified";
		this.cModified.HeaderText = "Modified";
		this.cModified.Name = "cModified";
		this.cModified.Visible = false;
		this.cCreatedBy.DataPropertyName = "CreatedBy";
		this.cCreatedBy.HeaderText = "Created by";
		this.cCreatedBy.Name = "cCreatedBy";
		this.cCreatedBy.Visible = false;
		this.cModifiedBy.DataPropertyName = "ModifiedBy";
		this.cModifiedBy.HeaderText = "Modified by";
		this.cModifiedBy.Name = "cModifiedBy";
		this.cModifiedBy.Visible = false;
		this.cIsActivated.DataPropertyName = "IsActivated";
		this.cIsActivated.HeaderText = "Is activated";
		this.cIsActivated.Name = "cIsActivated";
		this.cIsActivated.Visible = false;
		this.tableLayoutPanel1.AutoSize = true;
		this.tableLayoutPanel1.ColumnCount = 4;
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel1.Controls.Add(this.btnCommentRefresh, 1, 0);
		this.tableLayoutPanel1.Controls.Add(this.lblcTitle, 0, 0);
		this.tableLayoutPanel1.Controls.Add(this.lblcTotal, 2, 0);
		this.tableLayoutPanel1.Controls.Add(this.lblCommentTotal, 3, 0);
		this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 3);
		this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(1);
		this.tableLayoutPanel1.Name = "tableLayoutPanel1";
		this.tableLayoutPanel1.RowCount = 1;
		this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel1.Size = new System.Drawing.Size(349, 26);
		this.tableLayoutPanel1.TabIndex = 155;
		this.btnCommentRefresh.AutoSize = true;
		this.btnCommentRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnCommentRefresh.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnCommentRefresh.Location = new System.Drawing.Point(73, 0);
		this.btnCommentRefresh.Margin = new System.Windows.Forms.Padding(0);
		this.btnCommentRefresh.Name = "btnCommentRefresh";
		this.btnCommentRefresh.Size = new System.Drawing.Size(185, 26);
		this.btnCommentRefresh.TabIndex = 1;
		this.btnCommentRefresh.Text = "Refresh";
		this.toolTipMain.SetToolTip(this.btnCommentRefresh, "Select refresh data");
		this.btnCommentRefresh.UseVisualStyleBackColor = true;
		this.btnCommentRefresh.Click += new System.EventHandler(btnCommentRefresh_Click);
		this.lblcTitle.AutoSize = true;
		this.lblcTitle.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblcTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblcTitle.Location = new System.Drawing.Point(1, 1);
		this.lblcTitle.Margin = new System.Windows.Forms.Padding(1);
		this.lblcTitle.Name = "lblcTitle";
		this.lblcTitle.Size = new System.Drawing.Size(71, 24);
		this.lblcTitle.TabIndex = 146;
		this.lblcTitle.Text = "Comment";
		this.lblcTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblcTotal.AutoSize = true;
		this.lblcTotal.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblcTotal.Location = new System.Drawing.Point(259, 1);
		this.lblcTotal.Margin = new System.Windows.Forms.Padding(1);
		this.lblcTotal.Name = "lblcTotal";
		this.lblcTotal.Size = new System.Drawing.Size(72, 24);
		this.lblcTotal.TabIndex = 149;
		this.lblcTotal.Text = "Total rows:";
		this.lblcTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblCommentTotal.AutoSize = true;
		this.lblCommentTotal.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCommentTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCommentTotal.Location = new System.Drawing.Point(333, 1);
		this.lblCommentTotal.Margin = new System.Windows.Forms.Padding(1);
		this.lblCommentTotal.Name = "lblCommentTotal";
		this.lblCommentTotal.Size = new System.Drawing.Size(15, 24);
		this.lblCommentTotal.TabIndex = 152;
		this.lblCommentTotal.Text = "0";
		this.lblCommentTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panelCommentResize.Dock = System.Windows.Forms.DockStyle.Top;
		this.panelCommentResize.Location = new System.Drawing.Point(0, 0);
		this.panelCommentResize.Margin = new System.Windows.Forms.Padding(1);
		this.panelCommentResize.Name = "panelCommentResize";
		this.panelCommentResize.Size = new System.Drawing.Size(349, 3);
		this.panelCommentResize.TabIndex = 156;
		this.openFileDialogMain.FileName = "Template";
		this.openFileDialogMain.Filter = "All files (*.*)| *.*";
		this.openFileDialogMain.Title = "Select file";
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		base.Controls.Add(this.btnClose);
		base.Controls.Add(this.panelResize);
		base.Controls.Add(this.panelDisplayDraw);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "mPanelImage";
		base.Size = new System.Drawing.Size(351, 500);
		base.Load += new System.EventHandler(mPanelImage_Load);
		this.contextMenuStripComment.ResumeLayout(false);
		this.panelDisplayDraw.ResumeLayout(false);
		this.panelImage.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.ptbImage).EndInit();
		this.panelComment.ResumeLayout(false);
		this.panelComment.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvComment).EndInit();
		this.tableLayoutPanel1.ResumeLayout(false);
		this.tableLayoutPanel1.PerformLayout();
		base.ResumeLayout(false);
	}
}
