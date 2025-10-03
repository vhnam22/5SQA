using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using _5S_QA_DL.APIContext;
using _5S_QA_Entities.Abstracts;
using _5S_QA_Entities.Constants;
using _5S_QA_Entities.Dtos;
using _5S_QA_Entities.Enums;
using _5S_QA_Entities.Models;
using _5S_QA_System.Controls;
using _5S_QA_System.Properties;
using _5S_QA_System.View.User_control;
using KAutoHelper;
using MetroFramework.Controls;
using MetroFramework.Forms;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using WpfUserImage;

namespace _5S_QA_System;

public class frmMeasurementView : MetroForm
{
	private readonly Form mForm;

	private Guid mId;

	private Guid mIdParent;

	private Guid IdFrom;

	private Guid IdTo;

	private bool isEdit;

	private int mRow;

	private int mCol;

	public bool isClose;

	public bool isSaveCoordinate;

	private UcWpf ucWpf;

	private IContainer components = null;

	private StatusStrip statusStripfrmMain;

	private ToolStripProgressBar sprogbarStatus;

	public ToolStripStatusLabel slblStatus;

	private ToolTip toolTipMain;

	private mPanelView mPanelViewMain;

	private DataGridView dgvMain;

	private mSearch mSearchMain;

	private OpenFileDialog openFileDialogMain;

	private ContextMenuStrip contextMenuStripMain;

	private ToolStripMenuItem main_refreshToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripMenuItem main_newToolStripMenuItem;

	private ToolStripMenuItem main_editToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator6;

	private ToolStripMenuItem main_deleteToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripMenuItem main_importToolStripMenuItem;

	private mPanelSub mPanelSubMain;

	private ToolStripMenuItem main_moveToolStripMenuItem;

	private Panel panelmProgressBarMain;

	private MetroProgressBar mProgressBarMain;

	private PictureBox pictureBox1;

	private DataGridViewTextBoxColumn No;

	private DataGridViewTextBoxColumn Code;

	private DataGridViewTextBoxColumn name;

	private DataGridViewTextBoxColumn ImportantId;

	private DataGridViewTextBoxColumn ImportantName;

	private DataGridViewTextBoxColumn Value;

	private DataGridViewTextBoxColumn UnitId;

	private DataGridViewTextBoxColumn UnitName;

	private DataGridViewTextBoxColumn Upper;

	private DataGridViewTextBoxColumn Lower;

	private DataGridViewTextBoxColumn UCL;

	private DataGridViewTextBoxColumn LCL;

	private DataGridViewTextBoxColumn Formula;

	private DataGridViewTextBoxColumn MachineTypeId;

	private DataGridViewTextBoxColumn MachineTypeName;

	private DataGridViewTextBoxColumn TemplateId;

	private DataGridViewTextBoxColumn TemplateName;

	private DataGridViewTextBoxColumn ProductId;

	private DataGridViewTextBoxColumn Coordinate;

	private DataGridViewTextBoxColumn Id;

	private new DataGridViewTextBoxColumn Created;

	private DataGridViewTextBoxColumn Modified;

	private DataGridViewTextBoxColumn CreatedBy;

	private DataGridViewTextBoxColumn ModifiedBy;

	private DataGridViewTextBoxColumn IsActivated;

	private Panel panelViewImage;

	private Button btnSetCoordinate;

	private Panel panelLineV;

	private Panel panelLineH;

	private Button btnClearCoordinate;

	private PictureBox ptbImage;

	private Button btnClose;

	private ElementHost elementHostZoomImage;

	private Panel panelViewImageResize;

	private TableLayoutPanel tpanelImageViewClose;

	private Panel panelLogout;

	private Label lblFullname;

	private PictureBox ptbAvatar;

	public frmMeasurementView(Form frm, Guid id = default(Guid), string filename = null)
	{
		InitializeComponent();
		Common.setControls(this, toolTipMain, new List<ContextMenuStrip> { contextMenuStripMain });
		ControlResize.Init(panelViewImageResize, panelViewImage, ControlResize.Direction.Vertical, System.Windows.Forms.Cursors.SizeNS);
		elementHostZoomImage.HostContainer.MouseRightButtonUp += HostContainer_MouseRightButtonUp;
		panelViewImage.Visible = false;
		mForm = frm;
		mId = Guid.Empty;
		mIdParent = id;
		isEdit = false;
		isClose = true;
		mRow = 0;
		mCol = 0;
		openFileDialogMain.FileName = filename;
	}

	private void frmMeasurementView_Load(object sender, EventArgs e)
	{
		Init();
	}

	private void frmMeasurementView_Shown(object sender, EventArgs e)
	{
		mPanelSubMain.Init(mIdParent);
		dgvMainSub_CurrentCellChanged(mPanelSubMain.dgvMain, null);
		mPanelSubMain.dgvMain.CurrentCellChanged += dgvMainSub_CurrentCellChanged;
		if (!string.IsNullOrEmpty(openFileDialogMain.FileName))
		{
			AddMeasurement();
		}
	}

	private void frmMeasurementView_FormClosing(object sender, FormClosingEventArgs e)
	{
		GC.Collect();
	}

	private void frmMeasurementView_FormClosed(object sender, FormClosedEventArgs e)
	{
		mPanelViewMain.mDispose();
		List<Type> list = new List<Type>();
		list.Add(typeof(frmMeasurementAdd));
		Common.closeForm(list);
		if (!isClose)
		{
			((frmProductView)mForm).isClose = false;
			((frmProductView)mForm).load_AllData();
		}
	}

	private void Init()
	{
		panelmProgressBarMain.BringToFront();
		panelmProgressBarMain.Visible = false;
		mPanelViewMain.Visible = false;
		lblFullname.Text = frmLogin.User.FullName;
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(APIUrl.APIHost + "/AuthUserImage/").Append(frmLogin.User.ImageUrl);
		try
		{
			ptbAvatar.Load(stringBuilder.ToString());
		}
		catch
		{
			ptbAvatar.Image = Resources._5S_QA_S;
		}
		mSearchMain.btnSearch.Click += btnSearch_Click;
		mPanelViewMain.btnEdit.Click += main_editToolStripMenuItem_Click;
		mPanelViewMain.btnDelete.Click += main_deleteToolStripMenuItem_Click;
	}

	private void debugOutput(string strDebugText)
	{
		slblStatus.Text = strDebugText;
	}

	private void start_Proccessor()
	{
		sprogbarStatus.Maximum = 100;
		sprogbarStatus.Value = 0;
		sprogbarStatus.Value = 100;
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		if ((msg.Msg.Equals(256) || msg.Msg.Equals(260)) && base.ActiveControl.Name.Equals("dgvMain"))
		{
			if (keyData.Equals(Keys.Return))
			{
				dgvMain_CellDoubleClick(this, null);
				return true;
			}
			if (keyData.Equals(Keys.Escape))
			{
				dgvMain_CellClick(this, null);
				return true;
			}
		}
		return base.ProcessCmdKey(ref msg, keyData);
	}

	public void load_AllData()
	{
		System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
		try
		{
			start_Proccessor();
			isEdit = true;
			en_disControl(enable: true);
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "ProductId=@0";
			queryArgs.PredicateParameters = new string[1] { mIdParent.ToString() };
			queryArgs.Order = "Sort, Created";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs body = queryArgs;
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/Measurement/Gets").Result;
			mSearchMain.Init(Common.getDataTableNoType<MeasurementViewModel>(result), dgvMain);
			load_dgvMain();
		}
		catch (Exception ex)
		{
			if (ex.InnerException is ApiException { StatusCode: var statusCode } ex2)
			{
				if (statusCode.Equals(401))
				{
					if (System.Windows.Forms.MessageBox.Show(Common.getTextLanguage(this, "LoginAgain"), Common.getTextLanguage(this, "INFORMATION"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk).Equals(DialogResult.OK))
					{
						Close();
					}
				}
				else
				{
					debugOutput("ERR: " + ex2.Message.Replace("\n", ""));
					System.Windows.Forms.MessageBox.Show(ex2.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			else
			{
				debugOutput("ERR: " + ex.Message);
				System.Windows.Forms.MessageBox.Show(ex.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
	}

	private void load_dgvMain()
	{
		start_Proccessor();
		dgvMain.DataSource = mSearchMain.SearchInAllColums();
		dgvMain.Refresh();
		if (dgvMain.CurrentCell == null)
		{
			mPanelViewMain.Visible = false;
		}
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
		debugOutput(Common.getTextLanguage(this, "Successful"));
		dgvMain_Sorted(dgvMain, null);
		isEdit = false;
	}

	private void en_disControl(bool enable)
	{
		main_importToolStripMenuItem.Enabled = true;
		mPanelViewMain.btnEdit.Enabled = enable;
		mPanelViewMain.btnDelete.Enabled = enable;
		main_newToolStripMenuItem.Enabled = enable;
		main_editToolStripMenuItem.Enabled = enable;
		main_deleteToolStripMenuItem.Enabled = enable;
		main_moveToolStripMenuItem.Enabled = enable;
		if (enable)
		{
			main_importToolStripMenuItem.Text = Common.getTextLanguage(this, "Import");
		}
		else
		{
			main_importToolStripMenuItem.Text = Common.getTextLanguage(this, "Confirm");
		}
		mSearchMain.Enabled = true;
	}

	private void disControlMove()
	{
		mPanelViewMain.btnEdit.Enabled = false;
		mPanelViewMain.btnDelete.Enabled = false;
		main_newToolStripMenuItem.Enabled = false;
		main_editToolStripMenuItem.Enabled = false;
		main_deleteToolStripMenuItem.Enabled = false;
		main_importToolStripMenuItem.Enabled = false;
		mSearchMain.Enabled = false;
	}

	private void updateMove()
	{
		System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
		try
		{
			start_Proccessor();
			ResponseDto result = frmLogin.client.MoveAsync(IdFrom, IdTo, "/api/Measurement/Move/{idfrom}/{idto}").Result;
			IdFrom = Guid.Empty;
			IdTo = Guid.Empty;
			if (!result.Success)
			{
				throw new Exception(result.Messages.ElementAt(0).Message);
			}
			isClose = false;
			main_refreshToolStripMenuItem.PerformClick();
		}
		catch (Exception ex)
		{
			if (ex.InnerException is ApiException { StatusCode: var statusCode } ex2)
			{
				if (statusCode.Equals(401))
				{
					if (System.Windows.Forms.MessageBox.Show(Common.getTextLanguage(this, "LoginAgain"), Common.getTextLanguage(this, "INFORMATION"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk).Equals(DialogResult.OK))
					{
						Close();
					}
				}
				else
				{
					debugOutput("ERR: " + ex2.Message.Replace("\n", ""));
					System.Windows.Forms.MessageBox.Show(ex2.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			else
			{
				debugOutput("ERR: " + ex.Message);
				System.Windows.Forms.MessageBox.Show(ex.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
	}

	private void Load_ImageProduct(string url)
	{
		if (string.IsNullOrEmpty(url))
		{
			panelViewImage.Visible = false;
		}
		else
		{
			panelViewImage.Visible = true;
		}
		try
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(APIUrl.APIHost + "/ProductImage/").Append(url);
			ptbImage.Load(stringBuilder.ToString());
			ptbImage_Click(ptbImage, null);
		}
		catch
		{
			ptbImage.Image = Resources._5S_QA_S;
		}
	}

	private void Move_ImageWithMeas(string str)
	{
		if (panelViewImage.Visible && ucWpf != null && !string.IsNullOrEmpty(str))
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

	private void Next_Row()
	{
		mRow++;
		if (mRow < dgvMain.RowCount)
		{
			dgvMain.Rows[mRow].Cells[mCol].Selected = true;
		}
	}

	private void AddMeasurement()
	{
		panelmProgressBarMain.Visible = true;
		mProgressBarMain.Value = mProgressBarMain.Maximum;
		System.Windows.Forms.Application.DoEvents();
		if (mIdParent.Equals(Guid.Empty))
		{
			System.Windows.Forms.MessageBox.Show(Common.getTextLanguage(this, "wProductHasnt"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
		try
		{
			List<Measurement> list = new List<Measurement>();
			MeasurementDto val = Common.ReadExcelForMapper(openFileDialogMain.FileName);
			if (!string.IsNullOrEmpty(val.Message))
			{
				throw new Exception(val.Message);
			}
			list.AddRange(Common.CalMeasLs(val.Measurements));
			string text = SaveFileForMapper(list);
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
			dgvMain_CellClick(this, null);
			DataTable dataTable = new DataTable();
			foreach (DataGridViewColumn column in dgvMain.Columns)
			{
				dataTable.Columns.Add(column.Name);
			}
			FileInfo newFile = new FileInfo(text);
			using (ExcelPackage excelPackage = new ExcelPackage(newFile))
			{
				if (excelPackage.Workbook.Worksheets.Count < 1)
				{
					throw new Exception(Common.getTextLanguage(this, "IncorrectFormat"));
				}
				ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets["Data"] ?? throw new Exception(Common.getTextLanguage(this, "HasntSheet"));
				if (excelWorksheet.Dimension == null)
				{
					throw new Exception(Common.getTextLanguage(this, "SheetNull"));
				}
				ExcelRange excelRange = excelWorksheet.Cells[1, 1, excelWorksheet.Dimension.End.Row, excelWorksheet.Dimension.End.Column];
				if (!excelRange.Any())
				{
					throw new Exception(Common.getTextLanguage(this, "ContentNull"));
				}
				string[] measurementHeaders = MetaType.MeasurementHeaders;
				string text2 = Common.checkFormat(measurementHeaders, excelRange);
				if (!text2.Equals("Ok"))
				{
					throw new Exception(text2);
				}
				mProgressBarMain.Maximum = excelWorksheet.Dimension.Rows;
				for (int i = 1; i < excelWorksheet.Dimension.Rows; i++)
				{
					mProgressBarMain.Value = i;
					DataRow dataRow = dataTable.NewRow();
					for (int j = 0; j < measurementHeaders.Length; j++)
					{
						string text3 = Common.trimSpace(excelRange[1, j + 1].Value.ToString()).ToLower();
						if (text3.Equals(measurementHeaders[j].ToLower()))
						{
							dataRow[text3] = Common.trimSpace((excelRange[i + 1, j + 1].Value == null) ? "" : excelRange[i + 1, j + 1].Value.ToString());
						}
					}
					dataRow["Id"] = Guid.Empty;
					dataTable.Rows.Add(dataRow);
					dataTable.AcceptChanges();
				}
			}
			dataTable.Dispose();
			isSaveCoordinate = false;
			en_disControl(enable: false);
			mProgressBarMain.Value = mProgressBarMain.Maximum;
			mSearchMain.Init(dataTable, dgvMain);
			load_dgvMain();
		}
		catch (Exception ex)
		{
			System.Windows.Forms.MessageBox.Show(ex.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		finally
		{
			panelmProgressBarMain.Visible = false;
		}
	}

	private string SaveFileForMapper(List<Measurement> models)
	{
		string path = Path.Combine(Directory.GetCurrentDirectory(), "Templates");
		string text = Path.Combine("C:\\Windows\\Temp\\5SQA_System", "Views");
		Directory.CreateDirectory(path);
		Directory.CreateDirectory(text);
		string[] files = Directory.GetFiles(path, "MAPPER.xlsx");
		if (files.Length.Equals(0))
		{
			System.Windows.Forms.MessageBox.Show(Common.getTextLanguage("frmCreateFile", "MAPPER.xlsx"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return null;
		}
		string path2 = $"{Guid.NewGuid()}_MAPPER.xlsx";
		string text2 = Path.Combine(text, path2);
		System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
		string sourceFileName = files.First();
		try
		{
			if (Common.FileInUse(text2))
			{
				throw new Exception(Common.getTextLanguage("frmCreateFile", "Openning"));
			}
			File.Copy(sourceFileName, text2, overwrite: true);
			FileInfo newFile = new FileInfo(text2);
			using ExcelPackage excelPackage = new ExcelPackage(newFile);
			if (excelPackage.Workbook.Worksheets.Count < 1)
			{
				throw new Exception(Common.getTextLanguage("frmCreateFile", "IncorrectFormat"));
			}
			ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets["DATA"] ?? throw new Exception(Common.getTextLanguage("frmCreateFile", "HasntSheet"));
			int num = 2;
			foreach (Measurement model in models)
			{
				excelWorksheet.Cells[$"A{num}"].Value = num - 1;
				excelWorksheet.Cells[$"B{num}"].Value = $"MEAS-{num - 1}";
				excelWorksheet.Cells[$"C{num}"].Value = model.Name;
				excelWorksheet.Cells[$"D{num}"].Value = model.Important;
				excelWorksheet.Cells[$"E{num}"].Value = model.Value;
				excelWorksheet.Cells[$"F{num}"].Value = model.Unit;
				excelWorksheet.Cells[$"G{num}"].Value = model.Upper;
				excelWorksheet.Cells[$"H{num}"].Value = model.Lower;
				excelWorksheet.Cells[$"I{num}"].Value = model.Formula;
				excelWorksheet.Cells[$"J{num}"].Value = model.MachineType;
				excelWorksheet.Cells[$"K{num}"].Value = model.Mapper;
				num++;
			}
			ExcelRange excelRange = excelWorksheet.Cells[1, 1, num - 1, 11];
			excelRange.AutoFitColumns(5.0, 100.0);
			excelRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
			excelRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
			excelRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
			excelRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
			excelPackage.Save();
		}
		catch (Exception ex)
		{
			text2 = null;
			System.Windows.Forms.MessageBox.Show(ex.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		return text2;
	}

	private void dgvMainSub_CurrentCellChanged(object sender, EventArgs e)
	{
		DataGridView dataGridView = sender as DataGridView;
		if (dataGridView.CurrentCell != null)
		{
			if (!mPanelSubMain.isEdit)
			{
				mPanelSubMain.mRow = dataGridView.CurrentCell.RowIndex;
				mPanelSubMain.mCol = dataGridView.CurrentCell.ColumnIndex;
			}
			mIdParent = Guid.Parse(dataGridView.CurrentRow.Cells["Id"].Value.ToString());
			Load_ImageProduct(dataGridView.CurrentRow.Cells["ImageUrl"].Value.ToString());
		}
		else
		{
			panelViewImage.Visible = false;
			mIdParent = Guid.Empty;
		}
		load_AllData();
	}

	private void dgvMain_CurrentCellChanged(object sender, EventArgs e)
	{
		DataGridView dataGridView = sender as DataGridView;
		if (dataGridView.CurrentCell != null)
		{
			if (!isEdit)
			{
				mRow = dataGridView.CurrentCell.RowIndex;
				mCol = dataGridView.CurrentCell.ColumnIndex;
			}
			Guid guid = Guid.Parse(dataGridView.CurrentRow.Cells["Id"].Value.ToString());
			if (!guid.Equals(mId))
			{
				mId = guid;
				mPanelViewMain.Display();
				Move_ImageWithMeas(dataGridView.CurrentRow.Cells["Coordinate"].Value.ToString());
			}
			if (!IdFrom.Equals(Guid.Empty))
			{
				IdTo = mId;
			}
		}
		else
		{
			mId = Guid.Empty;
			IdFrom = Guid.Empty;
			IdTo = Guid.Empty;
			dgvMain.Cursor = System.Windows.Forms.Cursors.Default;
		}
	}

	private void dgvMain_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		mPanelViewMain.Visible = false;
		if (!IdTo.Equals(Guid.Empty) && !IdFrom.Equals(Guid.Empty) && !IdTo.Equals(IdFrom))
		{
			updateMove();
		}
	}

	private void dgvMain_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
	{
		if (mIdParent.Equals(Guid.Empty) || mId.Equals(Guid.Empty))
		{
			mPanelViewMain.Visible = false;
		}
		else
		{
			mPanelViewMain.Visible = true;
		}
	}

	private void dgvMain_Sorted(object sender, EventArgs e)
	{
		if (e != null)
		{
			System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
		}
		DataGridView dataGridView = sender as DataGridView;
		int.TryParse(mSearchMain.txtPage.Text, out var result);
		int.TryParse(mSearchMain.cbbLimit.Text, out var result2);
		foreach (DataGridViewRow item in (IEnumerable)dataGridView.Rows)
		{
			item.Cells["No"].Value = (result - 1) * result2 + item.Index + 1;
			if (main_importToolStripMenuItem.Text.Equals(Common.getTextLanguage(this, "Confirm")))
			{
				item.DefaultCellStyle.ForeColor = Color.Green;
			}
			if (item.Cells["UnitName"].Value?.ToString() == "°")
			{
				if (double.TryParse(item.Cells["Value"].Value?.ToString(), out var result3))
				{
					item.Cells["Value"].Value = Common.ConvertDoubleToDegrees(result3);
				}
				if (double.TryParse(item.Cells["Upper"].Value?.ToString(), out var result4))
				{
					item.Cells["Upper"].Value = Common.ConvertDoubleToDegrees(result4);
				}
				if (double.TryParse(item.Cells["Lower"].Value?.ToString(), out var result5))
				{
					item.Cells["Lower"].Value = Common.ConvertDoubleToDegrees(result5);
				}
				if (double.TryParse(item.Cells["LCL"].Value?.ToString(), out var result6))
				{
					item.Cells["LCL"].Value = Common.ConvertDoubleToDegrees(result6);
				}
				if (double.TryParse(item.Cells["UCL"].Value?.ToString(), out var result7))
				{
					item.Cells["UCL"].Value = Common.ConvertDoubleToDegrees(result7);
				}
			}
		}
	}

	private void btnSearch_Click(object sender, EventArgs e)
	{
		System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
		load_dgvMain();
	}

	private void main_refreshToolStripMenuItem_Click(object sender, EventArgs e)
	{
		mSearchMain.btnSearch.Select();
		load_AllData();
	}

	private void main_newToolStripMenuItem_Click(object sender, EventArgs e)
	{
		mSearchMain.btnSearch.Select();
		System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
		if (mIdParent.Equals(Guid.Empty))
		{
			System.Windows.Forms.MessageBox.Show(Common.getTextLanguage(this, "wProductHasnt"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		if (frmLogin.User.Role.Equals(RoleWeb.Administrator) && frmLogin.User.JobTitle != "Manager" && frmLogin.User.JobTitle != "Supervisor")
		{
			System.Windows.Forms.MessageBox.Show(Common.getTextLanguage(typeof(frmLogin).Name, "wNoAuthorization"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		List<Type> list = new List<Type>();
		list.Add(typeof(frmMeasurementAdd));
		Common.closeForm(list);
		new frmMeasurementAdd(this, (DataTable)dgvMain.DataSource, mIdParent, mId).Show();
	}

	private void main_editToolStripMenuItem_Click(object sender, EventArgs e)
	{
		mSearchMain.btnSearch.Select();
		System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
		if (mIdParent.Equals(Guid.Empty) || mId.Equals(Guid.Empty))
		{
			System.Windows.Forms.MessageBox.Show(Common.getTextLanguage(this, "wSelectRow"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		if (frmLogin.User.Role.Equals(RoleWeb.Administrator) && frmLogin.User.JobTitle != "Manager" && frmLogin.User.JobTitle != "Supervisor")
		{
			System.Windows.Forms.MessageBox.Show(Common.getTextLanguage(typeof(frmLogin).Name, "wNoAuthorization"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		List<Type> list = new List<Type>();
		list.Add(typeof(frmMeasurementAdd));
		Common.closeForm(list);
		new frmMeasurementAdd(this, (DataTable)dgvMain.DataSource, mIdParent, mId, isadd: false).Show();
	}

	private void main_deleteToolStripMenuItem_Click(object sender, EventArgs e)
	{
		mSearchMain.btnSearch.Select();
		if (mIdParent.Equals(Guid.Empty) || mId.Equals(Guid.Empty))
		{
			System.Windows.Forms.MessageBox.Show(Common.getTextLanguage(this, "wSelectRow"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
		else if (frmLogin.User.Role.Equals(RoleWeb.Administrator) && frmLogin.User.JobTitle != "Manager" && frmLogin.User.JobTitle != "Supervisor")
		{
			System.Windows.Forms.MessageBox.Show(Common.getTextLanguage(typeof(frmLogin).Name, "wNoAuthorization"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
		else
		{
			if (!System.Windows.Forms.MessageBox.Show(string.Format("{0} {1}", Common.getTextLanguage(this, "wSureDelete"), dgvMain.CurrentRow.Cells["Name"].Value), Common.getTextLanguage(this, "QUESTION"), MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.Yes))
			{
				return;
			}
			try
			{
				System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
				start_Proccessor();
				ResponseDto result = frmLogin.client.DeleteAsync(mId, "/api/Measurement/Delete/{id}").Result;
				if (!result.Success)
				{
					throw new Exception(result.Messages.ElementAt(0).Message);
				}
				isClose = false;
				List<Type> list = new List<Type>();
				list.Add(typeof(frmMeasurementAdd));
				Common.closeForm(list);
				main_refreshToolStripMenuItem.PerformClick();
			}
			catch (Exception ex)
			{
				string textLanguage = Common.getTextLanguage("Error", ex.Message);
				if (ex.InnerException is ApiException { StatusCode: var statusCode })
				{
					if (statusCode.Equals(401))
					{
						if (System.Windows.Forms.MessageBox.Show(Common.getTextLanguage(this, "LoginAgain"), Common.getTextLanguage(this, "INFORMATION"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk).Equals(DialogResult.OK))
						{
							Close();
						}
					}
					else
					{
						debugOutput("ERR: " + (string.IsNullOrEmpty(textLanguage) ? ex.Message.Replace("\n", "") : textLanguage));
						System.Windows.Forms.MessageBox.Show(string.IsNullOrEmpty(textLanguage) ? ex.Message : textLanguage, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
					}
				}
				else
				{
					debugOutput("ERR: " + (string.IsNullOrEmpty(textLanguage) ? ex.Message.Replace("\n", "") : textLanguage));
					System.Windows.Forms.MessageBox.Show(string.IsNullOrEmpty(textLanguage) ? ex.Message : textLanguage, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
		}
	}

	private void main_importToolStripMenuItem_Click(object sender, EventArgs e)
	{
		//IL_06c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f3: Expected O, but got Unknown
		//IL_06fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_072e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0761: Unknown result type (might be due to invalid IL or missing references)
		//IL_0794: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ad: Expected O, but got Unknown
		//IL_07b0: Expected O, but got Unknown
		mSearchMain.btnSearch.Select();
		if (mIdParent.Equals(Guid.Empty))
		{
			System.Windows.Forms.MessageBox.Show(Common.getTextLanguage(this, "wProductHasnt"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		if (frmLogin.User.Role.Equals(RoleWeb.Administrator) && frmLogin.User.JobTitle != "Manager" && frmLogin.User.JobTitle != "Supervisor")
		{
			System.Windows.Forms.MessageBox.Show(Common.getTextLanguage(typeof(frmLogin).Name, "wNoAuthorization"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		panelmProgressBarMain.Visible = true;
		mProgressBarMain.Value = mProgressBarMain.Maximum;
		if (main_importToolStripMenuItem.Text.Equals(Common.getTextLanguage(this, "Import")))
		{
			string[] measurementHeaders = MetaType.MeasurementHeaders;
			System.Windows.Forms.MessageBox.Show(Common.getTextLanguage(this, "FormatHeader") + "\r\n    " + string.Join(" - ", measurementHeaders) + "\r\n" + Common.getTextLanguage(this, "NextRow"), Common.getTextLanguage(this, "INFORMATION"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			openFileDialogMain.FileName = "Template";
			openFileDialogMain.Title = Common.getTextLanguage(this, "SelectTemplate");
			if (openFileDialogMain.ShowDialog().Equals(DialogResult.OK))
			{
				System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
				dgvMain_CellClick(this, null);
				try
				{
					DataTable dataTable = new DataTable();
					foreach (DataGridViewColumn column in dgvMain.Columns)
					{
						dataTable.Columns.Add(column.Name);
					}
					FileInfo newFile = new FileInfo(openFileDialogMain.FileName);
					using (ExcelPackage excelPackage = new ExcelPackage(newFile))
					{
						if (excelPackage.Workbook.Worksheets.Count < 1)
						{
							throw new Exception(Common.getTextLanguage(this, "IncorrectFormat"));
						}
						ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets["Data"] ?? throw new Exception(Common.getTextLanguage(this, "HasntSheet"));
						if (excelWorksheet.Dimension == null)
						{
							throw new Exception(Common.getTextLanguage(this, "SheetNull"));
						}
						ExcelRange excelRange = excelWorksheet.Cells[1, 1, excelWorksheet.Dimension.End.Row, excelWorksheet.Dimension.End.Column];
						if (!excelRange.Any())
						{
							throw new Exception(Common.getTextLanguage(this, "ContentNull"));
						}
						string text = Common.checkFormat(measurementHeaders, excelRange);
						if (!text.Equals("Ok"))
						{
							throw new Exception(text);
						}
						mProgressBarMain.Maximum = excelWorksheet.Dimension.Rows;
						for (int i = 1; i < excelWorksheet.Dimension.Rows; i++)
						{
							mProgressBarMain.Value = i;
							DataRow dataRow = dataTable.NewRow();
							for (int j = 0; j < measurementHeaders.Length; j++)
							{
								string text2 = Common.trimSpace(excelRange[1, j + 1].Value.ToString()).ToLower();
								if (text2.Equals(measurementHeaders[j].ToLower()))
								{
									dataRow[text2] = Common.trimSpace((excelRange[i + 1, j + 1].Value == null) ? "" : excelRange[i + 1, j + 1].Value.ToString());
								}
							}
							dataRow["Id"] = Guid.Empty;
							dataTable.Rows.Add(dataRow);
							dataTable.AcceptChanges();
						}
					}
					dataTable.Dispose();
					isSaveCoordinate = false;
					en_disControl(enable: false);
					mProgressBarMain.Value = mProgressBarMain.Maximum;
					mSearchMain.Init(dataTable, dgvMain);
					load_dgvMain();
				}
				catch (Exception ex)
				{
					System.Windows.Forms.MessageBox.Show(ex.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
		}
		else if (System.Windows.Forms.MessageBox.Show(Common.getTextLanguage(this, "wSureImport"), Common.getTextLanguage(this, "QUESTION"), MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.Yes))
		{
			System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
			DataTable dataTable2 = mSearchMain.dataTable;
			dataTable2.Dispose();
			mProgressBarMain.Maximum = dataTable2.Rows.Count;
			try
			{
				ResponseDto responseDto = new ResponseDto();
				if (isSaveCoordinate)
				{
					List<CoordinateDto> list = new List<CoordinateDto>();
					foreach (DataRow row in dataTable2.Rows)
					{
						int value = dataTable2.Rows.IndexOf(row) + 1;
						mProgressBarMain.Value = value;
						CoordinateDto val = new CoordinateDto
						{
							Id = Guid.Parse(row["Id"].ToString())
						};
						if (!string.IsNullOrEmpty(row["Coordinate"].ToString()))
						{
							val.Coordinate = row["Coordinate"].ToString();
						}
						list.Add(val);
					}
					responseDto = frmLogin.client.SaveAsync(list, "/api/Measurement/SaveCoordinates").Result;
				}
				else
				{
					List<MeasurementViewModel> list2 = new List<MeasurementViewModel>();
					foreach (DataRow row2 in dataTable2.Rows)
					{
						int value2 = dataTable2.Rows.IndexOf(row2) + 1;
						mProgressBarMain.Value = value2;
						MeasurementViewModel val2 = new MeasurementViewModel
						{
							MachineTypeName = (string.IsNullOrEmpty(row2["MachineTypeName"].ToString()) ? null : row2["MachineTypeName"].ToString()),
							Code = (string.IsNullOrEmpty(row2["Code"].ToString()) ? null : row2["Code"].ToString()),
							Name = (string.IsNullOrEmpty(row2["Name"].ToString()) ? null : row2["Name"].ToString()),
							Value = (string.IsNullOrEmpty(row2["Value"].ToString()) ? null : row2["Value"].ToString()),
							ProductId = mIdParent
						};
						((AuditableEntity)val2).IsActivated = true;
						MeasurementViewModel val3 = val2;
						if (double.TryParse(row2["Value"].ToString(), out var _))
						{
							if (double.TryParse(row2["Upper"].ToString(), out var result2))
							{
								val3.Upper = result2;
							}
							if (double.TryParse(row2["Lower"].ToString(), out result2))
							{
								val3.Lower = result2;
							}
							val3.ImportantName = (string.IsNullOrEmpty(row2["ImportantName"].ToString()) ? null : row2["ImportantName"].ToString());
							val3.UnitName = (string.IsNullOrEmpty(row2["UnitName"].ToString()) ? null : row2["UnitName"].ToString());
							val3.Formula = (string.IsNullOrEmpty(row2["Formula"].ToString()) ? null : row2["Formula"].ToString());
						}
						list2.Add(val3);
					}
					responseDto = frmLogin.client.SaveAsync(list2, "/api/Measurement/SaveList").Result;
				}
				if (!responseDto.Success)
				{
					string text3 = string.Empty;
					foreach (ResponseMessage message in responseDto.Messages)
					{
						text3 = text3 + message.Message + "\r\n";
					}
					throw new Exception(text3);
				}
				isClose = false;
				main_refreshToolStripMenuItem.PerformClick();
			}
			catch (Exception ex2)
			{
				string text4 = ex2.Message;
				string text5 = Settings.Default.Language.Replace("rb", "Name");
				List<Language> list3 = Common.ReadLanguages("Error");
				foreach (Language item in list3)
				{
					object value3 = ((object)item).GetType().GetProperty(text5).GetValue(item, null);
					if (value3 != null)
					{
						string newValue = value3.ToString();
						text4 = text4.Replace(item.Code, newValue);
					}
				}
				if (ex2.InnerException is ApiException { StatusCode: var statusCode })
				{
					if (statusCode.Equals(401))
					{
						if (System.Windows.Forms.MessageBox.Show(Common.getTextLanguage(this, "LoginAgain"), Common.getTextLanguage(this, "INFORMATION"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk).Equals(DialogResult.OK))
						{
							Close();
						}
					}
					else
					{
						debugOutput("ERR: " + (string.IsNullOrEmpty(text4) ? ex2.Message.Replace("\n", "") : text4));
						System.Windows.Forms.MessageBox.Show(string.IsNullOrEmpty(text4) ? ex2.Message : text4, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
					}
				}
				else
				{
					debugOutput("ERR: " + (string.IsNullOrEmpty(text4) ? ex2.Message.Replace("\n", "") : text4));
					System.Windows.Forms.MessageBox.Show(string.IsNullOrEmpty(text4) ? ex2.Message : text4, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
		}
		panelmProgressBarMain.Visible = false;
	}

	private void ptbAvatar_DoubleClick(object sender, EventArgs e)
	{
		Common.activeForm(typeof(frmLogin));
	}

	private void btnClearCoordinate_Click(object sender, EventArgs e)
	{
		if (dgvMain.CurrentCell != null)
		{
			dgvMain.CurrentRow.Cells["Coordinate"].Value = null;
			DataRow dataRow = mSearchMain.dataTable.Select($"Id='{mId}'").FirstOrDefault();
			if (dataRow != null)
			{
				dataRow["Coordinate"] = dgvMain.CurrentRow.Cells["Coordinate"].Value;
			}
			isSaveCoordinate = true;
			en_disControl(enable: false);
			dgvMain_Sorted(dgvMain, null);
			Next_Row();
		}
	}

	private void btnSetCoordinate_Click(object sender, EventArgs e)
	{
		System.Drawing.Point globalPoint = AutoControl.GetGlobalPoint(elementHostZoomImage.Handle, panelLineV.Location.X - 1, panelLineH.Location.Y - 1);
		AutoControl.MouseClick(globalPoint, (EMouseKey)1);
	}

	private void main_moveToolStripMenuItem_Click(object sender, EventArgs e)
	{
		mSearchMain.btnSearch.Select();
		System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
		if (mIdParent.Equals(Guid.Empty) || mId.Equals(Guid.Empty))
		{
			System.Windows.Forms.MessageBox.Show(Common.getTextLanguage(this, "wSelectRow"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		if (frmLogin.User.Role.Equals(RoleWeb.Administrator) && frmLogin.User.JobTitle != "Manager" && frmLogin.User.JobTitle != "Supervisor")
		{
			System.Windows.Forms.MessageBox.Show(Common.getTextLanguage(typeof(frmLogin).Name, "wNoAuthorization"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		IdFrom = mId;
		dgvMain.Cursor = System.Windows.Forms.Cursors.NoMoveVert;
		disControlMove();
	}

	private void btnClose_Click(object sender, EventArgs e)
	{
		panelViewImage.Visible = false;
	}

	private void ptbImage_Click(object sender, EventArgs e)
	{
		elementHostZoomImage.Child = null;
		ucWpf = new UcWpf(Common.ToBitmapImage((Bitmap)ptbImage.Image));
		elementHostZoomImage.Child = ucWpf;
	}

	private void HostContainer_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
	{
		if (dgvMain.CurrentCell != null)
		{
			dgvMain.CurrentRow.Cells["Coordinate"].Value = $"{ucWpf.Point.X}#{ucWpf.Point.Y}#{ucWpf.Scale}";
			DataRow dataRow = mSearchMain.dataTable.Select($"Id='{mId}'").FirstOrDefault();
			if (dataRow != null)
			{
				dataRow["Coordinate"] = dgvMain.CurrentRow.Cells["Coordinate"].Value;
			}
			isSaveCoordinate = true;
			en_disControl(enable: false);
			dgvMain_Sorted(dgvMain, null);
			Next_Row();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_5S_QA_System.frmMeasurementView));
		this.statusStripfrmMain = new System.Windows.Forms.StatusStrip();
		this.sprogbarStatus = new System.Windows.Forms.ToolStripProgressBar();
		this.slblStatus = new System.Windows.Forms.ToolStripStatusLabel();
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.btnSetCoordinate = new System.Windows.Forms.Button();
		this.btnClearCoordinate = new System.Windows.Forms.Button();
		this.ptbImage = new System.Windows.Forms.PictureBox();
		this.btnClose = new System.Windows.Forms.Button();
		this.dgvMain = new System.Windows.Forms.DataGridView();
		this.No = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ImportantId = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ImportantName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.UnitId = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.UnitName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Upper = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Lower = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.UCL = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.LCL = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Formula = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.MachineTypeId = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.MachineTypeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.TemplateId = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.TemplateName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ProductId = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Coordinate = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Created = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Modified = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.CreatedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ModifiedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.IsActivated = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.contextMenuStripMain = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.main_refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.main_newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.main_editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.main_deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.main_importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.main_moveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.openFileDialogMain = new System.Windows.Forms.OpenFileDialog();
		this.panelmProgressBarMain = new System.Windows.Forms.Panel();
		this.mProgressBarMain = new MetroFramework.Controls.MetroProgressBar();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.panelViewImage = new System.Windows.Forms.Panel();
		this.tpanelImageViewClose = new System.Windows.Forms.TableLayoutPanel();
		this.panelLineV = new System.Windows.Forms.Panel();
		this.panelLineH = new System.Windows.Forms.Panel();
		this.elementHostZoomImage = new System.Windows.Forms.Integration.ElementHost();
		this.panelViewImageResize = new System.Windows.Forms.Panel();
		this.mPanelViewMain = new _5S_QA_System.View.User_control.mPanelView();
		this.mSearchMain = new _5S_QA_System.View.User_control.mSearch();
		this.mPanelSubMain = new _5S_QA_System.View.User_control.mPanelSub();
		this.panelLogout = new System.Windows.Forms.Panel();
		this.lblFullname = new System.Windows.Forms.Label();
		this.ptbAvatar = new System.Windows.Forms.PictureBox();
		this.statusStripfrmMain.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.ptbImage).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.dgvMain).BeginInit();
		this.contextMenuStripMain.SuspendLayout();
		this.panelmProgressBarMain.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		this.panelViewImage.SuspendLayout();
		this.tpanelImageViewClose.SuspendLayout();
		this.panelLogout.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.ptbAvatar).BeginInit();
		base.SuspendLayout();
		this.statusStripfrmMain.BackColor = System.Drawing.SystemColors.ControlLight;
		this.statusStripfrmMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.statusStripfrmMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.sprogbarStatus, this.slblStatus });
		this.statusStripfrmMain.Location = new System.Drawing.Point(20, 554);
		this.statusStripfrmMain.Name = "statusStripfrmMain";
		this.statusStripfrmMain.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
		this.statusStripfrmMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
		this.statusStripfrmMain.Size = new System.Drawing.Size(1060, 26);
		this.statusStripfrmMain.SizingGrip = false;
		this.statusStripfrmMain.Stretch = false;
		this.statusStripfrmMain.TabIndex = 7;
		this.sprogbarStatus.Name = "sprogbarStatus";
		this.sprogbarStatus.Size = new System.Drawing.Size(200, 20);
		this.slblStatus.Name = "slblStatus";
		this.slblStatus.Size = new System.Drawing.Size(16, 21);
		this.slblStatus.Text = "...";
		this.btnSetCoordinate.AutoSize = true;
		this.btnSetCoordinate.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnSetCoordinate.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnSetCoordinate.Location = new System.Drawing.Point(142, 0);
		this.btnSetCoordinate.Margin = new System.Windows.Forms.Padding(0);
		this.btnSetCoordinate.Name = "btnSetCoordinate";
		this.btnSetCoordinate.Size = new System.Drawing.Size(104, 26);
		this.btnSetCoordinate.TabIndex = 158;
		this.btnSetCoordinate.Text = "Set coordinate";
		this.toolTipMain.SetToolTip(this.btnSetCoordinate, "Select set coordinate at this measurement");
		this.btnSetCoordinate.UseVisualStyleBackColor = true;
		this.btnSetCoordinate.Click += new System.EventHandler(btnSetCoordinate_Click);
		this.btnClearCoordinate.AutoSize = true;
		this.btnClearCoordinate.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnClearCoordinate.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnClearCoordinate.Location = new System.Drawing.Point(26, 0);
		this.btnClearCoordinate.Margin = new System.Windows.Forms.Padding(0);
		this.btnClearCoordinate.Name = "btnClearCoordinate";
		this.btnClearCoordinate.Size = new System.Drawing.Size(116, 26);
		this.btnClearCoordinate.TabIndex = 155;
		this.btnClearCoordinate.Text = "Clear coordinate";
		this.toolTipMain.SetToolTip(this.btnClearCoordinate, "Select clear coordinate at this measurement");
		this.btnClearCoordinate.UseVisualStyleBackColor = true;
		this.btnClearCoordinate.Click += new System.EventHandler(btnClearCoordinate_Click);
		this.ptbImage.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ptbImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ptbImage.Cursor = System.Windows.Forms.Cursors.Hand;
		this.ptbImage.ErrorImage = _5S_QA_System.Properties.Resources._5S_QA_S;
		this.ptbImage.Image = _5S_QA_System.Properties.Resources._5S_QA_S;
		this.ptbImage.InitialImage = _5S_QA_System.Properties.Resources._5S_QA_S;
		this.ptbImage.Location = new System.Drawing.Point(723, 0);
		this.ptbImage.Name = "ptbImage";
		this.ptbImage.Size = new System.Drawing.Size(60, 60);
		this.ptbImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.ptbImage.TabIndex = 154;
		this.ptbImage.TabStop = false;
		this.toolTipMain.SetToolTip(this.ptbImage, "Reset image");
		this.ptbImage.Click += new System.EventHandler(ptbImage_Click);
		this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnClose.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnClose.Location = new System.Drawing.Point(0, 0);
		this.btnClose.Margin = new System.Windows.Forms.Padding(0);
		this.btnClose.Name = "btnClose";
		this.btnClose.Size = new System.Drawing.Size(26, 26);
		this.btnClose.TabIndex = 152;
		this.btnClose.Text = "X";
		this.toolTipMain.SetToolTip(this.btnClose, "Close");
		this.btnClose.UseVisualStyleBackColor = true;
		this.btnClose.Click += new System.EventHandler(btnClose_Click);
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
		this.dgvMain.Columns.AddRange(this.No, this.Code, this.name, this.ImportantId, this.ImportantName, this.Value, this.UnitId, this.UnitName, this.Upper, this.Lower, this.UCL, this.LCL, this.Formula, this.MachineTypeId, this.MachineTypeName, this.TemplateId, this.TemplateName, this.ProductId, this.Coordinate, this.Id, this.Created, this.Modified, this.CreatedBy, this.ModifiedBy, this.IsActivated);
		this.dgvMain.ContextMenuStrip = this.contextMenuStripMain;
		this.dgvMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.dgvMain.EnableHeadersVisualStyles = false;
		this.dgvMain.Location = new System.Drawing.Point(295, 134);
		this.dgvMain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.dgvMain.MultiSelect = false;
		this.dgvMain.Name = "dgvMain";
		this.dgvMain.ReadOnly = true;
		this.dgvMain.RowHeadersWidth = 25;
		this.dgvMain.Size = new System.Drawing.Size(785, 200);
		this.dgvMain.TabIndex = 18;
		this.dgvMain.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvMain_CellClick);
		this.dgvMain.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvMain_CellDoubleClick);
		this.dgvMain.CurrentCellChanged += new System.EventHandler(dgvMain_CurrentCellChanged);
		this.dgvMain.Sorted += new System.EventHandler(dgvMain_Sorted);
		this.No.DataPropertyName = "No";
		dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		this.No.DefaultCellStyle = dataGridViewCellStyle3;
		this.No.HeaderText = "No.";
		this.No.Name = "No";
		this.No.ReadOnly = true;
		this.No.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.No.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.No.Width = 40;
		this.Code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Code.DataPropertyName = "Code";
		this.Code.FillWeight = 20f;
		this.Code.HeaderText = "Code";
		this.Code.Name = "Code";
		this.Code.ReadOnly = true;
		this.name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.name.DataPropertyName = "Name";
		this.name.FillWeight = 20f;
		this.name.HeaderText = "Name";
		this.name.Name = "name";
		this.name.ReadOnly = true;
		this.ImportantId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.ImportantId.DataPropertyName = "ImportantId";
		this.ImportantId.FillWeight = 30f;
		this.ImportantId.HeaderText = "Important id";
		this.ImportantId.Name = "ImportantId";
		this.ImportantId.ReadOnly = true;
		this.ImportantId.Visible = false;
		this.ImportantName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.ImportantName.DataPropertyName = "ImportantName";
		this.ImportantName.FillWeight = 20f;
		this.ImportantName.HeaderText = "Important";
		this.ImportantName.Name = "ImportantName";
		this.ImportantName.ReadOnly = true;
		this.Value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Value.DataPropertyName = "Value";
		dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.Value.DefaultCellStyle = dataGridViewCellStyle4;
		this.Value.FillWeight = 20f;
		this.Value.HeaderText = "Value";
		this.Value.Name = "Value";
		this.Value.ReadOnly = true;
		this.UnitId.DataPropertyName = "UnitId";
		this.UnitId.HeaderText = "Unit id";
		this.UnitId.Name = "UnitId";
		this.UnitId.ReadOnly = true;
		this.UnitId.Visible = false;
		this.UnitName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.UnitName.DataPropertyName = "UnitName";
		this.UnitName.FillWeight = 15f;
		this.UnitName.HeaderText = "Unit";
		this.UnitName.Name = "UnitName";
		this.UnitName.ReadOnly = true;
		this.Upper.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Upper.DataPropertyName = "Upper";
		dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.Upper.DefaultCellStyle = dataGridViewCellStyle5;
		this.Upper.FillWeight = 15f;
		this.Upper.HeaderText = "Upper";
		this.Upper.Name = "Upper";
		this.Upper.ReadOnly = true;
		this.Lower.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Lower.DataPropertyName = "Lower";
		dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.Lower.DefaultCellStyle = dataGridViewCellStyle6;
		this.Lower.FillWeight = 15f;
		this.Lower.HeaderText = "Lower";
		this.Lower.Name = "Lower";
		this.Lower.ReadOnly = true;
		this.UCL.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.UCL.DataPropertyName = "UCL";
		this.UCL.FillWeight = 15f;
		this.UCL.HeaderText = "UCL";
		this.UCL.Name = "UCL";
		this.UCL.ReadOnly = true;
		this.LCL.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.LCL.DataPropertyName = "LCL";
		this.LCL.FillWeight = 15f;
		this.LCL.HeaderText = "LCL";
		this.LCL.Name = "LCL";
		this.LCL.ReadOnly = true;
		this.Formula.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Formula.DataPropertyName = "Formula";
		this.Formula.FillWeight = 20f;
		this.Formula.HeaderText = "Formula";
		this.Formula.Name = "Formula";
		this.Formula.ReadOnly = true;
		this.MachineTypeId.DataPropertyName = "MachineTypeId";
		this.MachineTypeId.HeaderText = "Machine type id";
		this.MachineTypeId.Name = "MachineTypeId";
		this.MachineTypeId.ReadOnly = true;
		this.MachineTypeId.Visible = false;
		this.MachineTypeName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.MachineTypeName.DataPropertyName = "MachineTypeName";
		this.MachineTypeName.FillWeight = 20f;
		this.MachineTypeName.HeaderText = "M. type";
		this.MachineTypeName.Name = "MachineTypeName";
		this.MachineTypeName.ReadOnly = true;
		this.TemplateId.DataPropertyName = "TemplateId";
		this.TemplateId.HeaderText = "Template id";
		this.TemplateId.Name = "TemplateId";
		this.TemplateId.ReadOnly = true;
		this.TemplateId.Visible = false;
		this.TemplateName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.TemplateName.DataPropertyName = "TemplateName";
		this.TemplateName.FillWeight = 20f;
		this.TemplateName.HeaderText = "Template";
		this.TemplateName.Name = "TemplateName";
		this.TemplateName.ReadOnly = true;
		this.TemplateName.Visible = false;
		this.ProductId.DataPropertyName = "ProductId";
		this.ProductId.HeaderText = "Product id";
		this.ProductId.Name = "ProductId";
		this.ProductId.ReadOnly = true;
		this.ProductId.Visible = false;
		this.Coordinate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Coordinate.DataPropertyName = "Coordinate";
		this.Coordinate.FillWeight = 20f;
		this.Coordinate.HeaderText = "Coordinate";
		this.Coordinate.Name = "Coordinate";
		this.Coordinate.ReadOnly = true;
		this.Id.DataPropertyName = "Id";
		this.Id.HeaderText = "Id";
		this.Id.Name = "Id";
		this.Id.ReadOnly = true;
		this.Id.Visible = false;
		this.Created.DataPropertyName = "Created";
		this.Created.HeaderText = "Created";
		this.Created.Name = "Created";
		this.Created.ReadOnly = true;
		this.Created.Visible = false;
		this.Modified.DataPropertyName = "Modified";
		this.Modified.HeaderText = "Modified";
		this.Modified.Name = "Modified";
		this.Modified.ReadOnly = true;
		this.Modified.Visible = false;
		this.CreatedBy.DataPropertyName = "CreatedBy";
		this.CreatedBy.HeaderText = "Created by";
		this.CreatedBy.Name = "CreatedBy";
		this.CreatedBy.ReadOnly = true;
		this.CreatedBy.Visible = false;
		this.ModifiedBy.DataPropertyName = "ModifiedBy";
		this.ModifiedBy.HeaderText = "Modified by";
		this.ModifiedBy.Name = "ModifiedBy";
		this.ModifiedBy.ReadOnly = true;
		this.ModifiedBy.Visible = false;
		this.IsActivated.DataPropertyName = "IsActivated";
		this.IsActivated.HeaderText = "Is activated";
		this.IsActivated.Name = "IsActivated";
		this.IsActivated.ReadOnly = true;
		this.IsActivated.Visible = false;
		this.contextMenuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.main_refreshToolStripMenuItem, this.toolStripSeparator1, this.main_newToolStripMenuItem, this.main_editToolStripMenuItem, this.toolStripSeparator6, this.main_deleteToolStripMenuItem, this.toolStripSeparator2, this.main_importToolStripMenuItem, this.main_moveToolStripMenuItem });
		this.contextMenuStripMain.Name = "contextMenuStripStaff";
		this.contextMenuStripMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
		this.contextMenuStripMain.Size = new System.Drawing.Size(170, 154);
		this.main_refreshToolStripMenuItem.Name = "main_refreshToolStripMenuItem";
		this.main_refreshToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
		this.main_refreshToolStripMenuItem.Text = "Refresh";
		this.main_refreshToolStripMenuItem.Click += new System.EventHandler(main_refreshToolStripMenuItem_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(166, 6);
		this.main_newToolStripMenuItem.Name = "main_newToolStripMenuItem";
		this.main_newToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
		this.main_newToolStripMenuItem.Text = "New";
		this.main_newToolStripMenuItem.Click += new System.EventHandler(main_newToolStripMenuItem_Click);
		this.main_editToolStripMenuItem.Name = "main_editToolStripMenuItem";
		this.main_editToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
		this.main_editToolStripMenuItem.Text = "Edit";
		this.main_editToolStripMenuItem.Click += new System.EventHandler(main_editToolStripMenuItem_Click);
		this.toolStripSeparator6.Name = "toolStripSeparator6";
		this.toolStripSeparator6.Size = new System.Drawing.Size(166, 6);
		this.main_deleteToolStripMenuItem.Name = "main_deleteToolStripMenuItem";
		this.main_deleteToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
		this.main_deleteToolStripMenuItem.Text = "Delete";
		this.main_deleteToolStripMenuItem.Click += new System.EventHandler(main_deleteToolStripMenuItem_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(166, 6);
		this.main_importToolStripMenuItem.Name = "main_importToolStripMenuItem";
		this.main_importToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
		this.main_importToolStripMenuItem.Text = "Import from excel";
		this.main_importToolStripMenuItem.Click += new System.EventHandler(main_importToolStripMenuItem_Click);
		this.main_moveToolStripMenuItem.Name = "main_moveToolStripMenuItem";
		this.main_moveToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
		this.main_moveToolStripMenuItem.Text = "Move";
		this.main_moveToolStripMenuItem.Click += new System.EventHandler(main_moveToolStripMenuItem_Click);
		this.openFileDialogMain.FileName = "Measurement";
		this.openFileDialogMain.Filter = "File excel (*.xls, *.xlsx)| *.xls; *.xlsx";
		this.openFileDialogMain.Title = "Select template excel measurement for product";
		this.panelmProgressBarMain.Anchor = System.Windows.Forms.AnchorStyles.None;
		this.panelmProgressBarMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panelmProgressBarMain.Controls.Add(this.mProgressBarMain);
		this.panelmProgressBarMain.Controls.Add(this.pictureBox1);
		this.panelmProgressBarMain.Location = new System.Drawing.Point(489, 237);
		this.panelmProgressBarMain.Name = "panelmProgressBarMain";
		this.panelmProgressBarMain.Size = new System.Drawing.Size(122, 128);
		this.panelmProgressBarMain.TabIndex = 31;
		this.mProgressBarMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.mProgressBarMain.Location = new System.Drawing.Point(0, 120);
		this.mProgressBarMain.Name = "mProgressBarMain";
		this.mProgressBarMain.Size = new System.Drawing.Size(120, 6);
		this.mProgressBarMain.TabIndex = 18;
		this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
		this.pictureBox1.Image = _5S_QA_System.Properties.Resources._5S_QA_S;
		this.pictureBox1.Location = new System.Drawing.Point(0, 0);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(120, 120);
		this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.pictureBox1.TabIndex = 17;
		this.pictureBox1.TabStop = false;
		this.panelViewImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panelViewImage.Controls.Add(this.tpanelImageViewClose);
		this.panelViewImage.Controls.Add(this.panelLineV);
		this.panelViewImage.Controls.Add(this.panelLineH);
		this.panelViewImage.Controls.Add(this.ptbImage);
		this.panelViewImage.Controls.Add(this.elementHostZoomImage);
		this.panelViewImage.Controls.Add(this.panelViewImageResize);
		this.panelViewImage.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panelViewImage.Location = new System.Drawing.Point(295, 334);
		this.panelViewImage.Name = "panelViewImage";
		this.panelViewImage.Size = new System.Drawing.Size(785, 220);
		this.panelViewImage.TabIndex = 33;
		this.tpanelImageViewClose.AutoSize = true;
		this.tpanelImageViewClose.ColumnCount = 3;
		this.tpanelImageViewClose.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelImageViewClose.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelImageViewClose.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelImageViewClose.Controls.Add(this.btnClose, 0, 0);
		this.tpanelImageViewClose.Controls.Add(this.btnSetCoordinate, 2, 0);
		this.tpanelImageViewClose.Controls.Add(this.btnClearCoordinate, 1, 0);
		this.tpanelImageViewClose.Location = new System.Drawing.Point(0, 0);
		this.tpanelImageViewClose.Name = "tpanelImageViewClose";
		this.tpanelImageViewClose.RowCount = 1;
		this.tpanelImageViewClose.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tpanelImageViewClose.Size = new System.Drawing.Size(246, 26);
		this.tpanelImageViewClose.TabIndex = 159;
		this.panelLineV.Anchor = System.Windows.Forms.AnchorStyles.None;
		this.panelLineV.BackColor = System.Drawing.Color.Red;
		this.panelLineV.Location = new System.Drawing.Point(392, 85);
		this.panelLineV.Name = "panelLineV";
		this.panelLineV.Size = new System.Drawing.Size(1, 50);
		this.panelLineV.TabIndex = 157;
		this.panelLineH.Anchor = System.Windows.Forms.AnchorStyles.None;
		this.panelLineH.BackColor = System.Drawing.Color.Red;
		this.panelLineH.Location = new System.Drawing.Point(367, 110);
		this.panelLineH.Name = "panelLineH";
		this.panelLineH.Size = new System.Drawing.Size(50, 1);
		this.panelLineH.TabIndex = 156;
		this.elementHostZoomImage.BackColor = System.Drawing.SystemColors.Control;
		this.elementHostZoomImage.Dock = System.Windows.Forms.DockStyle.Fill;
		this.elementHostZoomImage.Location = new System.Drawing.Point(0, 3);
		this.elementHostZoomImage.Name = "elementHostZoomImage";
		this.elementHostZoomImage.Size = new System.Drawing.Size(783, 215);
		this.elementHostZoomImage.TabIndex = 153;
		this.elementHostZoomImage.Child = null;
		this.panelViewImageResize.Dock = System.Windows.Forms.DockStyle.Top;
		this.panelViewImageResize.Location = new System.Drawing.Point(0, 0);
		this.panelViewImageResize.Name = "panelViewImageResize";
		this.panelViewImageResize.Size = new System.Drawing.Size(783, 3);
		this.panelViewImageResize.TabIndex = 0;
		this.mPanelViewMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.mPanelViewMain.Dock = System.Windows.Forms.DockStyle.Right;
		this.mPanelViewMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.mPanelViewMain.Location = new System.Drawing.Point(730, 134);
		this.mPanelViewMain.Margin = new System.Windows.Forms.Padding(0);
		this.mPanelViewMain.Name = "mPanelViewMain";
		this.mPanelViewMain.Size = new System.Drawing.Size(350, 200);
		this.mPanelViewMain.TabIndex = 22;
		this.mSearchMain.AutoSize = true;
		this.mSearchMain.BackColor = System.Drawing.SystemColors.Control;
		this.mSearchMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.mSearchMain.dataTable = null;
		this.mSearchMain.Dock = System.Windows.Forms.DockStyle.Top;
		this.mSearchMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.mSearchMain.Location = new System.Drawing.Point(295, 70);
		this.mSearchMain.Margin = new System.Windows.Forms.Padding(4);
		this.mSearchMain.MaximumSize = new System.Drawing.Size(5000, 64);
		this.mSearchMain.MinimumSize = new System.Drawing.Size(25, 64);
		this.mSearchMain.Name = "mSearchMain";
		this.mSearchMain.Padding = new System.Windows.Forms.Padding(3);
		this.mSearchMain.Size = new System.Drawing.Size(785, 64);
		this.mSearchMain.TabIndex = 21;
		this.mPanelSubMain.BackColor = System.Drawing.SystemColors.Control;
		this.mPanelSubMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.mPanelSubMain.Dock = System.Windows.Forms.DockStyle.Left;
		this.mPanelSubMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.mPanelSubMain.isEdit = false;
		this.mPanelSubMain.Location = new System.Drawing.Point(20, 70);
		this.mPanelSubMain.Margin = new System.Windows.Forms.Padding(4);
		this.mPanelSubMain.mCol = 0;
		this.mPanelSubMain.mRow = 0;
		this.mPanelSubMain.Name = "mPanelSubMain";
		this.mPanelSubMain.Size = new System.Drawing.Size(275, 484);
		this.mPanelSubMain.TabIndex = 30;
		this.panelLogout.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.panelLogout.BackColor = System.Drawing.Color.Transparent;
		this.panelLogout.Controls.Add(this.lblFullname);
		this.panelLogout.Controls.Add(this.ptbAvatar);
		this.panelLogout.Location = new System.Drawing.Point(730, 27);
		this.panelLogout.Name = "panelLogout";
		this.panelLogout.Size = new System.Drawing.Size(350, 42);
		this.panelLogout.TabIndex = 176;
		this.panelLogout.TabStop = true;
		this.lblFullname.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.lblFullname.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f);
		this.lblFullname.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.lblFullname.Location = new System.Drawing.Point(0, 26);
		this.lblFullname.Name = "lblFullname";
		this.lblFullname.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
		this.lblFullname.Size = new System.Drawing.Size(308, 16);
		this.lblFullname.TabIndex = 34;
		this.lblFullname.Text = "...";
		this.lblFullname.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.ptbAvatar.Dock = System.Windows.Forms.DockStyle.Right;
		this.ptbAvatar.Image = _5S_QA_System.Properties.Resources._5S_QA_S;
		this.ptbAvatar.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.ptbAvatar.Location = new System.Drawing.Point(308, 0);
		this.ptbAvatar.Margin = new System.Windows.Forms.Padding(4);
		this.ptbAvatar.Name = "ptbAvatar";
		this.ptbAvatar.Size = new System.Drawing.Size(42, 42);
		this.ptbAvatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.ptbAvatar.TabIndex = 14;
		this.ptbAvatar.TabStop = false;
		this.ptbAvatar.DoubleClick += new System.EventHandler(ptbAvatar_DoubleClick);
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1100, 600);
		base.Controls.Add(this.panelLogout);
		base.Controls.Add(this.mPanelViewMain);
		base.Controls.Add(this.dgvMain);
		base.Controls.Add(this.panelViewImage);
		base.Controls.Add(this.panelmProgressBarMain);
		base.Controls.Add(this.mSearchMain);
		base.Controls.Add(this.mPanelSubMain);
		base.Controls.Add(this.statusStripfrmMain);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "frmMeasurementView";
		base.Padding = new System.Windows.Forms.Padding(20, 70, 20, 20);
		this.Text = "5S QA System * MEASUREMENT";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmMeasurementView_FormClosing);
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmMeasurementView_FormClosed);
		base.Load += new System.EventHandler(frmMeasurementView_Load);
		base.Shown += new System.EventHandler(frmMeasurementView_Shown);
		this.statusStripfrmMain.ResumeLayout(false);
		this.statusStripfrmMain.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.ptbImage).EndInit();
		((System.ComponentModel.ISupportInitialize)this.dgvMain).EndInit();
		this.contextMenuStripMain.ResumeLayout(false);
		this.panelmProgressBarMain.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		this.panelViewImage.ResumeLayout(false);
		this.panelViewImage.PerformLayout();
		this.tpanelImageViewClose.ResumeLayout(false);
		this.tpanelImageViewClose.PerformLayout();
		this.panelLogout.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.ptbAvatar).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
