using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using _5S_QA_Client.Controls;
using _5S_QA_Client.Properties;
using _5S_QA_DL.APIContext;
using _5S_QA_Entities.Constants;
using _5S_QA_Entities.Dtos;
using _5S_QA_Entities.Enums;
using _5S_QA_Entities.Models;
using MetroFramework.Controls;
using MetroFramework.Forms;
using OfficeOpenXml;

namespace _5S_QA_Client;

public class frmMapperView : MetroForm
{
	private readonly RequestViewModel mRequest;

	private string mRemember;

	public bool isClose;

	public Guid mIdParent;

	private Guid mId;

	private bool isEdit;

	private bool isChange;

	private int mRow;

	private int mCol;

	private DataTable mData;

	private string[] mFilter;

	private IContainer components = null;

	private ToolTip toolTipMain;

	private StatusStrip statusStripfrmMain;

	private ToolStripProgressBar sprogbarStatus;

	private ContextMenuStrip contextMenuStripMain;

	private ToolStripMenuItem main_refreshToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripMenuItem main_editToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripMenuItem main_importToolStripMenuItem;

	private OpenFileDialog openFileDialogMain;

	private DataGridView dgvMain;

	private Panel panelProduct;

	private TableLayoutPanel tpanelProduct;

	private ComboBox cbbProduct;

	private mPanelMapper mPanelViewMain;

	private Panel panelmProgressBarMain;

	private MetroProgressBar mProgressBarMain;

	private PictureBox pictureBox1;

	private Panel panelLogout;

	private Label lblFullname;

	private PictureBox ptbAvatar;

	private Label lblProduct;

	private Label lblTotalRow;

	private Label lblMapper;

	private Label lblTotalRows;

	private Label lblMappers;

	private ToolStripStatusLabel slblStatus;

	private DataGridViewTextBoxColumn No;

	private DataGridViewTextBoxColumn MachineTypeName;

	private DataGridViewTextBoxColumn Code;

	private DataGridViewTextBoxColumn name;

	private DataGridViewTextBoxColumn Value;

	private DataGridViewTextBoxColumn UnitName;

	private DataGridViewTextBoxColumn Upper;

	private DataGridViewTextBoxColumn Lower;

	private DataGridViewTextBoxColumn Formula;

	private DataGridViewTextBoxColumn Mapper;

	private DataGridViewTextBoxColumn Id;

	private new DataGridViewTextBoxColumn Created;

	private DataGridViewTextBoxColumn Modified;

	private DataGridViewTextBoxColumn CreatedBy;

	private DataGridViewTextBoxColumn ModifiedBy;

	private DataGridViewTextBoxColumn IsActivated;

	private ToolStripMenuItem main_exportToolStripMenuItem;

	private FolderBrowserDialog folderDialogMain;

	private DataGridView dgvMapper;

	private ContextMenuStrip contextMenuStripMapper;

	private ToolStripMenuItem mapper_changeToolStripMenuItem;

	private Panel panelMapper;

	private Panel panelMapperResize;

	private DataGridViewTextBoxColumn NoMapper;

	private DataGridViewTextBoxColumn NameMapper;

	private DataGridViewTextBoxColumn Sample;

	private DataGridViewTextBoxColumn CavityMapper;

	private DataGridViewTextBoxColumn ActualMapper;

	private ComboBox cbbGroup;

	private Label lblGroup;

	public frmMapperView(RequestViewModel request)
	{
		InitializeComponent();
		Common.setControls(this, toolTipMain, new List<ContextMenuStrip> { contextMenuStripMain, contextMenuStripMapper });
		mRequest = request;
		isClose = true;
	}

	private void frmMapperView_Load(object sender, EventArgs e)
	{
		ControlResize.Init(panelMapperResize, panelMapper, ControlResize.Direction.Horizontal, Cursors.SizeWE);
		Init();
	}

	private void frmMapperView_Shown(object sender, EventArgs e)
	{
		load_cbbGroup();
		if (mRequest != null)
		{
			cbbProduct.SelectedValue = mRequest.ProductId;
		}
		cbbProduct.Select();
		cbbProduct.SelectedIndexChanged += cbbNormal_SelectedIndexChanged;
		cbbNormal_SelectedIndexChanged(cbbProduct, null);
	}

	private void frmMapperView_FormClosing(object sender, FormClosingEventArgs e)
	{
		GC.Collect();
	}

	private void frmMapperView_FormClosed(object sender, FormClosedEventArgs e)
	{
		if (!isClose)
		{
			frmLogin.isEdit = true;
		}
	}

	private void Init()
	{
		Load_Settings();
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
			ptbAvatar.Image = Resources._5S_QA_C;
		}
		switch (frmLogin.mMachineType)
		{
		case "CLP-35":
			openFileDialogMain.Filter = "File result (*.LN2)|*.LN2|" + openFileDialogMain.Filter;
			break;
		case "ADCOLE911":
			openFileDialogMain.Filter = "File result (*.txt)|*.txt|" + openFileDialogMain.Filter;
			break;
		case "ROUNDNESS-ACCTee":
			openFileDialogMain.Multiselect = true;
			openFileDialogMain.Filter = "File result (*.txt)|*.txt|" + openFileDialogMain.Filter;
			break;
		case "CONTOUR-ACCTee":
			openFileDialogMain.Multiselect = true;
			openFileDialogMain.Filter = "File result (*.txt)|*.txt|" + openFileDialogMain.Filter;
			break;
		case "SUFRCOM-ACCTee":
			openFileDialogMain.Multiselect = true;
			openFileDialogMain.Filter = "File result (*.txt)|*.txt|" + openFileDialogMain.Filter;
			break;
		case "CRYSTA":
			openFileDialogMain.Filter = "File result (*.txt)|*.txt|" + openFileDialogMain.Filter;
			break;
		case "SE3500":
			openFileDialogMain.Filter = "File result (*.txt)|*.txt|" + openFileDialogMain.Filter;
			break;
		case "EF3000":
			openFileDialogMain.Filter = "File result (*.csv)|*.csv|" + openFileDialogMain.Filter;
			break;
		}
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

	private void Load_Settings()
	{
		string connectionString = ConfigurationManager.ConnectionStrings["Filter"].ConnectionString;
		string[] array = connectionString.Split(new string[1] { ";" }, StringSplitOptions.RemoveEmptyEntries);
		mFilter = (string[])array.Clone();
		int num = 0;
		string[] array2 = array;
		foreach (string text in array2)
		{
			string[] array3 = text.Split('#');
			mFilter[num] = array3[0];
			num++;
		}
	}

	private void load_cbbGroup()
	{
		try
		{
			QueryArgs body = new QueryArgs
			{
				Predicate = "Products.Any() && IsActivated",
				Order = "Created DESC",
				Page = 1,
				Limit = int.MaxValue
			};
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/ProductGroup/Gets").Result;
			DataTable dataTable = Common.getDataTable(result);
			if (dataTable != null)
			{
				dataTable.Dispose();
				cbbGroup.ValueMember = "Id";
				cbbGroup.DisplayMember = "CodeName";
				cbbGroup.DataSource = dataTable;
			}
			else
			{
				cbbGroup.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			cbbGroup.DataSource = null;
			if (ex.InnerException is ApiException { StatusCode: var statusCode } ex2)
			{
				if (statusCode.Equals(401))
				{
					if (MessageBox.Show(Common.getTextLanguage(this, "LoginAgain"), Common.getTextLanguage(this, "INFORMATION"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk).Equals(DialogResult.OK))
					{
						Close();
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

	private void load_cbbProduct()
	{
		try
		{
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "Measurements.Any() && IsActivated && GroupId=@0";
			queryArgs.PredicateParameters = new string[1] { string.IsNullOrEmpty(cbbGroup.Text) ? Guid.Empty.ToString() : cbbGroup.SelectedValue.ToString() };
			queryArgs.Order = "Created DESC";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs body = queryArgs;
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/Product/Gets").Result;
			DataTable dataTable = Common.getDataTable<ProductForCbbViewModel>(result);
			if (dataTable != null)
			{
				dataTable.Dispose();
				cbbProduct.ValueMember = "Id";
				cbbProduct.DisplayMember = "Name";
				cbbProduct.DataSource = dataTable;
			}
		}
		catch (Exception ex)
		{
			cbbProduct.DataSource = null;
			if (ex.InnerException is ApiException { StatusCode: var statusCode } ex2)
			{
				if (statusCode.Equals(401))
				{
					if (MessageBox.Show(Common.getTextLanguage(this, "LoginAgain"), Common.getTextLanguage(this, "INFORMATION"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk).Equals(DialogResult.OK))
					{
						Close();
					}
				}
				else
				{
					debugOutput("ERR: " + ex2.Message.Replace("\n", ""));
					MessageBox.Show(ex2.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			else
			{
				debugOutput("ERR: " + ex.Message);
				MessageBox.Show(ex.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
	}

	private void load_AllData()
	{
		Cursor.Current = Cursors.WaitCursor;
		try
		{
			start_Proccessor();
			isEdit = true;
			en_disControl(enable: true);
			string text = string.Empty;
			string[] array = mFilter;
			foreach (string text2 in array)
			{
				text = text + "MachineType.Name=\"" + text2.Trim() + "\"||";
			}
			if (!string.IsNullOrEmpty(text))
			{
				text = "&&(" + text.TrimEnd('|') + ")";
			}
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "ProductId=@0" + text;
			queryArgs.PredicateParameters = new string[1] { mIdParent.ToString() };
			queryArgs.Order = "Sort, Created";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs body = queryArgs;
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/Measurement/Gets").Result;
			mData = Common.getDataTableNoType<MapperMeasViewModel>(result);
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
						Close();
					}
				}
				else
				{
					debugOutput("ERR: " + ex2.Message.Replace("\n", ""));
					MessageBox.Show(ex2.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			else
			{
				debugOutput("ERR: " + ex.Message);
				MessageBox.Show(ex.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
	}

	private void load_dgvMain()
	{
		start_Proccessor();
		dgvMain.DataSource = mData;
		dgvMain.Refresh();
		if (dgvMain.CurrentCell == null)
		{
			mPanelViewMain.Visible = false;
		}
		lblTotalRows.Text = dgvMain.RowCount.ToString();
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
		main_editToolStripMenuItem.Enabled = enable;
		main_exportToolStripMenuItem.Enabled = enable;
		if (enable)
		{
			main_importToolStripMenuItem.Text = Common.getTextLanguage(this, "Import");
		}
		else
		{
			main_importToolStripMenuItem.Text = Common.getTextLanguage(this, "Confirm");
		}
	}

	private void cbbNormal_Leave(object sender, EventArgs e)
	{
		ComboBox comboBox = sender as ComboBox;
		if (comboBox.SelectedIndex.Equals(-1))
		{
			comboBox.Text = mRemember;
		}
	}

	private void cbbNormal_Enter(object sender, EventArgs e)
	{
		ComboBox comboBox = sender as ComboBox;
		mRemember = comboBox.Text;
	}

	private void cbbNormal_SelectedIndexChanged(object sender, EventArgs e)
	{
		main_refreshToolStripMenuItem.PerformClick();
	}

	public void main_refreshToolStripMenuItem_Click(object sender, EventArgs e)
	{
		main_importToolStripMenuItem.Enabled = true;
		panelMapper.Visible = false;
		lblTotalRows.Select();
		if (!cbbProduct.SelectedIndex.Equals(-1))
		{
			mIdParent = (string.IsNullOrEmpty(cbbProduct.Text) ? Guid.Empty : Guid.Parse(cbbProduct.SelectedValue.ToString()));
			load_AllData();
		}
	}

	private void dgvMain_Sorted(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		List<MapperMeasDto> mappers = Common.getMappers(mIdParent);
		int num = 0;
		foreach (DataGridViewRow row in (IEnumerable)dgvMain.Rows)
		{
			row.Cells["no"].Value = row.Index + 1;
			row.DefaultCellStyle.ForeColor = Color.Black;
			if (main_importToolStripMenuItem.Text.Equals(Common.getTextLanguage(this, "Import")))
			{
				MapperMeasDto val = mappers.Where((MapperMeasDto x) => x.MeasurementId.Equals(Guid.Parse(row.Cells["Id"].Value.ToString()))).FirstOrDefault();
				if (val != null && !string.IsNullOrEmpty(val.Mapper))
				{
					row.Cells["Mapper"].Value = val.Mapper;
				}
				if (row.Cells["Mapper"].Value != null && !string.IsNullOrEmpty(row.Cells["Mapper"].Value.ToString()))
				{
					row.DefaultCellStyle.ForeColor = Color.Blue;
					num++;
				}
			}
			else if (row.Cells["Mapper"].Value != null && !string.IsNullOrEmpty(row.Cells["Mapper"].Value.ToString()))
			{
				row.DefaultCellStyle.ForeColor = Color.Green;
				num++;
			}
			if (row.Cells["UnitName"].Value?.ToString() == "°")
			{
				if (double.TryParse(row.Cells["Value"].Value?.ToString(), out var result))
				{
					row.Cells["Value"].Value = Common.ConvertDoubleToDegrees(result);
				}
				if (double.TryParse(row.Cells["Upper"].Value?.ToString(), out var result2))
				{
					row.Cells["Upper"].Value = Common.ConvertDoubleToDegrees(result2);
				}
				if (double.TryParse(row.Cells["Lower"].Value?.ToString(), out var result3))
				{
					row.Cells["Lower"].Value = Common.ConvertDoubleToDegrees(result3);
				}
			}
		}
		mPanelViewMain.load_dgvContent(FormType.VIEW);
		lblMappers.Text = num.ToString();
	}

	private void dgvMain_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		mPanelViewMain.Visible = false;
	}

	private void dgvMain_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
	{
		if (mId.Equals(Guid.Empty))
		{
			mPanelViewMain.Visible = false;
		}
		else
		{
			mPanelViewMain.Visible = true;
		}
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
			mId = Guid.Parse(dataGridView.CurrentRow.Cells["Id"].Value.ToString());
			mPanelViewMain.load_dgvContent(FormType.VIEW);
		}
		else
		{
			mId = Guid.Empty;
		}
	}

	private void main_editToolStripMenuItem_Click(object sender, EventArgs e)
	{
		lblTotalRows.Select();
		Cursor.Current = Cursors.WaitCursor;
		if (mId.Equals(Guid.Empty))
		{
			MessageBox.Show(Common.getTextLanguage(this, "wSelectRow"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		mPanelViewMain.Visible = true;
		mPanelViewMain.load_dgvContent(FormType.EDIT);
	}

	private void main_exportToolStripMenuItem_Click(object sender, EventArgs e)
	{
		lblTotalRows.Select();
		Cursor.Current = Cursors.WaitCursor;
		if (mIdParent.Equals(Guid.Empty))
		{
			MessageBox.Show(Common.getTextLanguage(this, "wProductHasnt"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		string text = Path.Combine(".\\", "Mappers", $"{mIdParent}.json");
		if (!File.Exists(text))
		{
			MessageBox.Show(Common.getTextLanguage(this, "wFileMapperHasnt"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		panelmProgressBarMain.Visible = true;
		mProgressBarMain.Value = mProgressBarMain.Maximum;
		if (folderDialogMain.ShowDialog().Equals(DialogResult.OK))
		{
			string selectedPath = folderDialogMain.SelectedPath;
			string destFileName = Path.Combine(selectedPath, $"{mIdParent}.json");
			File.Copy(text, destFileName, overwrite: true);
		}
		panelmProgressBarMain.Visible = false;
	}

	private void main_importToolStripMenuItem_Click(object sender, EventArgs e)
	{
		//IL_0668: Unknown result type (might be due to invalid IL or missing references)
		//IL_066d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0676: Unknown result type (might be due to invalid IL or missing references)
		//IL_0684: Expected O, but got Unknown
		//IL_08b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d3: Expected O, but got Unknown
		lblTotalRows.Select();
		Cursor.Current = Cursors.WaitCursor;
		panelmProgressBarMain.Visible = true;
		mProgressBarMain.Value = mProgressBarMain.Maximum;
		if (mIdParent.Equals(Guid.Empty))
		{
			MessageBox.Show(Common.getTextLanguage(this, "wProductHasnt"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		if (main_importToolStripMenuItem.Text.Equals(Common.getTextLanguage(this, "Import")))
		{
			string[] measurementHeaders = MetaType.MeasurementHeaders;
			openFileDialogMain.FileName = "Template";
			openFileDialogMain.Title = Common.getTextLanguage(this, "SelectTemplate");
			if (openFileDialogMain.ShowDialog().Equals(DialogResult.OK))
			{
				Cursor.Current = Cursors.WaitCursor;
				int filterIndex = openFileDialogMain.FilterIndex;
				dgvMain_CellClick(this, null);
				try
				{
					if (filterIndex.Equals(1))
					{
						List<Result> list = new List<Result>();
						list = frmLogin.mMachineType switch
						{
							"CLP-35" => MachineFilter.FilterResult_CLP_35(openFileDialogMain.FileName), 
							"ADCOLE911" => MachineFilter.FilterResult_ADCOLE911(openFileDialogMain.FileName), 
							"ROUNDNESS-ACCTee" => MachineFilter.FilterResult_ROUNDNESS_ACCTee(openFileDialogMain.FileNames.ToList()), 
							"CONTOUR-ACCTee" => MachineFilter.FilterResult_CONTOUR_ACCTee(openFileDialogMain.FileNames.ToList(), 1, 1, isConvert: false), 
							"SUFRCOM-ACCTee" => MachineFilter.FilterResult_SUFRCOM_ACCTee(openFileDialogMain.FileNames.ToList()), 
							"CRYSTA" => MachineFilter.FilterResult_CRYSTA(openFileDialogMain.FileName, 1, 1, isConvert: false), 
							"SE3500" => MachineFilter.FilterResult_SE3500(openFileDialogMain.FileName), 
							"EF3000" => MachineFilter.FilterResult_EF3000(openFileDialogMain.FileName, 1, 1, isConvert: false), 
							_ => throw new Exception(Common.getTextLanguage(this, "Unname")), 
						};
						if (list == null || list.Count.Equals(0))
						{
							throw new Exception(Common.getTextLanguage(this, "IncorrectFormat"));
						}
						List<Result> dataSource = (from x in list
							group x by x.Sample).FirstOrDefault().ToList();
						dgvMapper.DataSource = dataSource;
						panelMapper.Visible = true;
						dgvMapper_Sorted(dgvMapper, null);
						main_importToolStripMenuItem.Enabled = false;
					}
					else if (filterIndex.Equals(4))
					{
						string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(openFileDialogMain.FileName);
						if (!Guid.TryParse(fileNameWithoutExtension, out var _))
						{
							throw new Exception(Common.getTextLanguage(this, "IncorrectFormat"));
						}
						string destFileName = Path.Combine(".\\", "Mappers", fileNameWithoutExtension + ".json");
						string fileName = openFileDialogMain.FileName;
						File.Copy(fileName, destFileName, overwrite: true);
						main_refreshToolStripMenuItem.PerformClick();
					}
					else
					{
						List<MapperMeasViewModel> list2 = new List<MapperMeasViewModel>();
						if (Path.GetExtension(openFileDialogMain.FileName).ToLower().Contains(".txt"))
						{
							string[] array = File.ReadAllLines(openFileDialogMain.FileName);
							mProgressBarMain.Maximum = array.Length;
							int num = 0;
							int num2 = 0;
							foreach (var item in array.Select((string value3, int index) => new
							{
								value = value3,
								index = index
							}))
							{
								mProgressBarMain.Value = item.index + 1;
								string[] array2 = item.value.Split('\t');
								if (item.index == 0)
								{
									foreach (var item2 in array2.Select((string value3, int index) => new
									{
										value = value3,
										index = index
									}))
									{
										string value = item2.value;
										string text = value;
										if (!(text == "Code"))
										{
											if (text == "Mapper")
											{
												num2 = item2.index;
											}
										}
										else
										{
											num = item2.index;
										}
									}
									if (num == 0 && num2 == 0)
									{
										throw new Exception(Common.getTextLanguage(this, "IncorrectFormat"));
									}
								}
								else if (array2.Length > num2)
								{
									string str = array2[num].ToString();
									string text2 = Common.trimSpace(str);
									str = array2[num2].ToString();
									string text3 = Common.trimSpace(str);
									if (!string.IsNullOrEmpty(text2) && !string.IsNullOrEmpty(text3))
									{
										list2.Add(new MapperMeasViewModel
										{
											Code = text2,
											Mapper = text3
										});
									}
								}
							}
						}
						else
						{
							FileInfo newFile = new FileInfo(openFileDialogMain.FileName);
							using ExcelPackage excelPackage = new ExcelPackage(newFile);
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
							string text4 = Common.checkFormat(measurementHeaders, excelRange);
							if (!text4.Equals("Ok"))
							{
								throw new Exception(text4);
							}
							mProgressBarMain.Maximum = excelWorksheet.Dimension.Rows;
							for (int num3 = 1; num3 < excelWorksheet.Dimension.Rows; num3++)
							{
								mProgressBarMain.Value = num3;
								string text5 = string.Empty;
								string text6 = string.Empty;
								for (int num4 = 0; num4 < measurementHeaders.Length; num4++)
								{
									string text7 = ((excelRange[num3 + 1, num4 + 1].Value == null) ? string.Empty : Common.trimSpace(excelRange[num3 + 1, num4 + 1].Value.ToString()));
									if (string.IsNullOrEmpty(text7))
									{
										continue;
									}
									string text8 = measurementHeaders[num4];
									string text9 = text8;
									if (!(text9 == "Code"))
									{
										if (text9 == "Mapper")
										{
											text6 = text7;
										}
									}
									else
									{
										text5 = text7;
									}
								}
								if (!string.IsNullOrEmpty(text5) && !string.IsNullOrEmpty(text6))
								{
									list2.Add(new MapperMeasViewModel
									{
										Code = text5,
										Mapper = text6
									});
								}
							}
						}
						foreach (DataRow row in mData.Rows)
						{
							string codeData = row.Field<string>("Code");
							MapperMeasViewModel val = list2.Where((MapperMeasViewModel x) => x.Code.Equals(codeData)).FirstOrDefault();
							if (val != null)
							{
								row.SetField("Mapper", val.Mapper);
							}
						}
						en_disControl(enable: false);
						load_dgvMain();
					}
					mProgressBarMain.Value = mProgressBarMain.Maximum;
				}
				catch (Exception ex)
				{
					string text10 = ex.Message;
					string text11 = Settings.Default.Language.Replace("rb", "Name");
					List<LanguageDto> list3 = Common.ReadLanguages("Error");
					foreach (LanguageDto item3 in list3)
					{
						object value2 = ((object)item3).GetType().GetProperty(text11).GetValue(item3, null);
						if (value2 != null)
						{
							string newValue = value2.ToString();
							text10 = text10.Replace(item3.Code, newValue);
						}
					}
					MessageBox.Show(string.IsNullOrEmpty(text10) ? ex.Message : text10, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
		}
		else if (MessageBox.Show(Common.getTextLanguage(this, "wSureImport"), Common.getTextLanguage(this, "QUESTION"), MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.Yes))
		{
			Cursor.Current = Cursors.WaitCursor;
			if (Common.addMappers(this, mIdParent, dgvMain, null))
			{
				isClose = false;
				main_refreshToolStripMenuItem.PerformClick();
			}
		}
		panelmProgressBarMain.Visible = false;
	}

	private void dgvMain_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
	{
		DataGridView dataGridView = sender as DataGridView;
		if (dataGridView.CurrentCell is DataGridViewTextBoxCell)
		{
			mRemember = dgvMain.CurrentCell.Value.ToString();
			DataGridViewTextBoxEditingControl dataGridViewTextBoxEditingControl = e.Control as DataGridViewTextBoxEditingControl;
			dataGridViewTextBoxEditingControl.AutoCompleteMode = AutoCompleteMode.Suggest;
			dataGridViewTextBoxEditingControl.AutoCompleteSource = AutoCompleteSource.CustomSource;
			dataGridViewTextBoxEditingControl.Leave -= txtTemp_Leave;
			dataGridViewTextBoxEditingControl.MaxLength = int.MaxValue;
			dataGridViewTextBoxEditingControl.AutoCompleteCustomSource = Common.getAutoComplete(mData, "Mapper");
			dataGridViewTextBoxEditingControl.Leave += txtTemp_Leave;
		}
	}

	private void txtTemp_Leave(object sender, EventArgs e)
	{
		DataGridViewTextBoxEditingControl dataGridViewTextBoxEditingControl = sender as DataGridViewTextBoxEditingControl;
		dataGridViewTextBoxEditingControl.Text = Common.trimSpace(dataGridViewTextBoxEditingControl.Text);
	}

	private void dgvMain_SelectionChanged(object sender, EventArgs e)
	{
		if (isChange)
		{
			isChange = false;
			main_refreshToolStripMenuItem.PerformClick();
		}
	}

	private void dgvMain_CellEndEdit(object sender, DataGridViewCellEventArgs e)
	{
		DataGridView dataGridView = sender as DataGridView;
		if (dataGridView.CurrentCell is DataGridViewTextBoxCell && !dgvMain.CurrentCell.Value.ToString().Equals(mRemember))
		{
			Cursor.Current = Cursors.WaitCursor;
			Common.addMappers(this, mIdParent, dgvMain, null);
			isClose = false;
			isChange = true;
		}
	}

	private void dgvMapper_Sorted(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		foreach (DataGridViewRow item in (IEnumerable)dgvMapper.Rows)
		{
			item.Cells["NoMapper"].Value = item.Index + 1;
		}
	}

	private void mapper_changeToolStripMenuItem_Click(object sender, EventArgs e)
	{
		dgvMapper_CellDoubleClick(dgvMapper, null);
	}

	private void dgvMapper_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
	{
		if (dgvMapper.CurrentCell != null && dgvMain.CurrentCell != null)
		{
			dgvMain.CurrentRow.Cells["Mapper"].Value = dgvMapper.CurrentRow.Cells["NameMapper"].Value;
			if (Common.addMappers(this, mIdParent, dgvMain, null))
			{
				dgvMain_Sorted(dgvMain, null);
			}
			else
			{
				load_AllData();
			}
			isClose = false;
			dgvMain.Select();
			SendKeys.SendWait("{down}");
		}
	}

	private void cbbGroup_SelectedIndexChanged(object sender, EventArgs e)
	{
		load_cbbProduct();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_5S_QA_Client.frmMapperView));
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.cbbProduct = new System.Windows.Forms.ComboBox();
		this.cbbGroup = new System.Windows.Forms.ComboBox();
		this.statusStripfrmMain = new System.Windows.Forms.StatusStrip();
		this.sprogbarStatus = new System.Windows.Forms.ToolStripProgressBar();
		this.slblStatus = new System.Windows.Forms.ToolStripStatusLabel();
		this.contextMenuStripMain = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.main_refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.main_editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.main_exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.main_importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.openFileDialogMain = new System.Windows.Forms.OpenFileDialog();
		this.dgvMain = new System.Windows.Forms.DataGridView();
		this.No = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.MachineTypeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.UnitName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Upper = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Lower = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Formula = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Mapper = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Created = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Modified = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.CreatedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ModifiedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.IsActivated = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.panelProduct = new System.Windows.Forms.Panel();
		this.tpanelProduct = new System.Windows.Forms.TableLayoutPanel();
		this.lblGroup = new System.Windows.Forms.Label();
		this.lblMappers = new System.Windows.Forms.Label();
		this.lblTotalRows = new System.Windows.Forms.Label();
		this.lblMapper = new System.Windows.Forms.Label();
		this.lblTotalRow = new System.Windows.Forms.Label();
		this.lblProduct = new System.Windows.Forms.Label();
		this.panelmProgressBarMain = new System.Windows.Forms.Panel();
		this.mProgressBarMain = new MetroFramework.Controls.MetroProgressBar();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.panelLogout = new System.Windows.Forms.Panel();
		this.lblFullname = new System.Windows.Forms.Label();
		this.ptbAvatar = new System.Windows.Forms.PictureBox();
		this.folderDialogMain = new System.Windows.Forms.FolderBrowserDialog();
		this.dgvMapper = new System.Windows.Forms.DataGridView();
		this.NoMapper = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.NameMapper = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Sample = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.CavityMapper = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ActualMapper = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.contextMenuStripMapper = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.mapper_changeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.panelMapper = new System.Windows.Forms.Panel();
		this.panelMapperResize = new System.Windows.Forms.Panel();
		this.mPanelViewMain = new _5S_QA_Client.mPanelMapper();
		this.statusStripfrmMain.SuspendLayout();
		this.contextMenuStripMain.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvMain).BeginInit();
		this.panelProduct.SuspendLayout();
		this.tpanelProduct.SuspendLayout();
		this.panelmProgressBarMain.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		this.panelLogout.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.ptbAvatar).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.dgvMapper).BeginInit();
		this.contextMenuStripMapper.SuspendLayout();
		this.panelMapper.SuspendLayout();
		base.SuspendLayout();
		this.cbbProduct.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cbbProduct.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cbbProduct.Dock = System.Windows.Forms.DockStyle.Fill;
		this.cbbProduct.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cbbProduct.FormattingEnabled = true;
		this.cbbProduct.ItemHeight = 16;
		this.cbbProduct.Location = new System.Drawing.Point(101, 32);
		this.cbbProduct.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.cbbProduct.Name = "cbbProduct";
		this.cbbProduct.Size = new System.Drawing.Size(647, 24);
		this.cbbProduct.TabIndex = 1;
		this.toolTipMain.SetToolTip(this.cbbProduct, "Select or enter stage name");
		this.cbbProduct.Enter += new System.EventHandler(cbbNormal_Enter);
		this.cbbProduct.Leave += new System.EventHandler(cbbNormal_Leave);
		this.cbbGroup.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cbbGroup.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cbbGroup.Dock = System.Windows.Forms.DockStyle.Fill;
		this.cbbGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cbbGroup.FormattingEnabled = true;
		this.cbbGroup.ItemHeight = 16;
		this.cbbGroup.Location = new System.Drawing.Point(101, 3);
		this.cbbGroup.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.cbbGroup.Name = "cbbGroup";
		this.cbbGroup.Size = new System.Drawing.Size(647, 24);
		this.cbbGroup.TabIndex = 181;
		this.toolTipMain.SetToolTip(this.cbbGroup, "Select or enter product");
		this.cbbGroup.SelectedIndexChanged += new System.EventHandler(cbbGroup_SelectedIndexChanged);
		this.statusStripfrmMain.BackColor = System.Drawing.SystemColors.ControlLight;
		this.statusStripfrmMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.statusStripfrmMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.sprogbarStatus, this.slblStatus });
		this.statusStripfrmMain.Location = new System.Drawing.Point(20, 554);
		this.statusStripfrmMain.Name = "statusStripfrmMain";
		this.statusStripfrmMain.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
		this.statusStripfrmMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
		this.statusStripfrmMain.Size = new System.Drawing.Size(760, 26);
		this.statusStripfrmMain.SizingGrip = false;
		this.statusStripfrmMain.Stretch = false;
		this.statusStripfrmMain.TabIndex = 39;
		this.sprogbarStatus.Name = "sprogbarStatus";
		this.sprogbarStatus.Size = new System.Drawing.Size(200, 20);
		this.slblStatus.Name = "slblStatus";
		this.slblStatus.Size = new System.Drawing.Size(16, 21);
		this.slblStatus.Text = "...";
		this.contextMenuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.main_refreshToolStripMenuItem, this.toolStripSeparator1, this.main_editToolStripMenuItem, this.toolStripSeparator2, this.main_exportToolStripMenuItem, this.main_importToolStripMenuItem });
		this.contextMenuStripMain.Name = "contextMenuStripStaff";
		this.contextMenuStripMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
		this.contextMenuStripMain.Size = new System.Drawing.Size(114, 104);
		this.main_refreshToolStripMenuItem.Name = "main_refreshToolStripMenuItem";
		this.main_refreshToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
		this.main_refreshToolStripMenuItem.Text = "Refresh";
		this.main_refreshToolStripMenuItem.Click += new System.EventHandler(main_refreshToolStripMenuItem_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(110, 6);
		this.main_editToolStripMenuItem.Name = "main_editToolStripMenuItem";
		this.main_editToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
		this.main_editToolStripMenuItem.Text = "Edit";
		this.main_editToolStripMenuItem.Click += new System.EventHandler(main_editToolStripMenuItem_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(110, 6);
		this.main_exportToolStripMenuItem.Name = "main_exportToolStripMenuItem";
		this.main_exportToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
		this.main_exportToolStripMenuItem.Text = "Export";
		this.main_exportToolStripMenuItem.Click += new System.EventHandler(main_exportToolStripMenuItem_Click);
		this.main_importToolStripMenuItem.Name = "main_importToolStripMenuItem";
		this.main_importToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
		this.main_importToolStripMenuItem.Text = "Import";
		this.main_importToolStripMenuItem.Click += new System.EventHandler(main_importToolStripMenuItem_Click);
		this.openFileDialogMain.FileName = "Mapper";
		this.openFileDialogMain.Filter = "File excel (*.xls; *.xlsx)|*.xls; *.xlsx|File text (*.txt)|*.txt|File json (*.json)|*.json";
		this.openFileDialogMain.Title = "Select file mapper";
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
		this.dgvMain.Columns.AddRange(this.No, this.MachineTypeName, this.Code, this.name, this.Value, this.UnitName, this.Upper, this.Lower, this.Formula, this.Mapper, this.Id, this.Created, this.Modified, this.CreatedBy, this.ModifiedBy, this.IsActivated);
		this.dgvMain.ContextMenuStrip = this.contextMenuStripMain;
		this.dgvMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.dgvMain.EnableHeadersVisualStyles = false;
		this.dgvMain.Location = new System.Drawing.Point(20, 171);
		this.dgvMain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.dgvMain.MultiSelect = false;
		this.dgvMain.Name = "dgvMain";
		this.dgvMain.RowHeadersWidth = 25;
		this.dgvMain.Size = new System.Drawing.Size(560, 383);
		this.dgvMain.TabIndex = 44;
		this.dgvMain.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvMain_CellClick);
		this.dgvMain.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvMain_CellDoubleClick);
		this.dgvMain.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(dgvMain_CellEndEdit);
		this.dgvMain.CurrentCellChanged += new System.EventHandler(dgvMain_CurrentCellChanged);
		this.dgvMain.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(dgvMain_EditingControlShowing);
		this.dgvMain.SelectionChanged += new System.EventHandler(dgvMain_SelectionChanged);
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
		this.MachineTypeName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.MachineTypeName.DataPropertyName = "MachineTypeName";
		this.MachineTypeName.FillWeight = 20f;
		this.MachineTypeName.HeaderText = "Mac.";
		this.MachineTypeName.Name = "MachineTypeName";
		this.MachineTypeName.ReadOnly = true;
		this.Code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Code.DataPropertyName = "Code";
		this.Code.FillWeight = 15f;
		this.Code.HeaderText = "Code";
		this.Code.Name = "Code";
		this.Code.ReadOnly = true;
		this.name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.name.DataPropertyName = "Name";
		this.name.FillWeight = 20f;
		this.name.HeaderText = "Name";
		this.name.Name = "name";
		this.name.ReadOnly = true;
		this.Value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Value.DataPropertyName = "Value";
		dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.Value.DefaultCellStyle = dataGridViewCellStyle4;
		this.Value.FillWeight = 20f;
		this.Value.HeaderText = "Value";
		this.Value.Name = "Value";
		this.Value.ReadOnly = true;
		this.UnitName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.UnitName.DataPropertyName = "UnitName";
		this.UnitName.FillWeight = 10f;
		this.UnitName.HeaderText = "Unit";
		this.UnitName.Name = "UnitName";
		this.UnitName.ReadOnly = true;
		this.Upper.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Upper.DataPropertyName = "Upper";
		dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.Upper.DefaultCellStyle = dataGridViewCellStyle5;
		this.Upper.FillWeight = 15f;
		this.Upper.HeaderText = "Upper ";
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
		this.Formula.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Formula.DataPropertyName = "Formula";
		this.Formula.FillWeight = 20f;
		this.Formula.HeaderText = "Formula";
		this.Formula.Name = "Formula";
		this.Formula.ReadOnly = true;
		this.Formula.Visible = false;
		this.Mapper.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Mapper.DataPropertyName = "Mapper";
		this.Mapper.FillWeight = 20f;
		this.Mapper.HeaderText = "Mapper";
		this.Mapper.Name = "Mapper";
		this.Id.DataPropertyName = "Id";
		this.Id.HeaderText = "Id";
		this.Id.Name = "Id";
		this.Id.ReadOnly = true;
		this.Id.Visible = false;
		this.Created.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Created.DataPropertyName = "Created";
		this.Created.FillWeight = 30f;
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
		this.panelProduct.AutoSize = true;
		this.panelProduct.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panelProduct.Controls.Add(this.tpanelProduct);
		this.panelProduct.Dock = System.Windows.Forms.DockStyle.Top;
		this.panelProduct.Location = new System.Drawing.Point(20, 70);
		this.panelProduct.Name = "panelProduct";
		this.panelProduct.Padding = new System.Windows.Forms.Padding(3);
		this.panelProduct.Size = new System.Drawing.Size(760, 101);
		this.panelProduct.TabIndex = 45;
		this.tpanelProduct.AutoSize = true;
		this.tpanelProduct.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		this.tpanelProduct.ColumnCount = 2;
		this.tpanelProduct.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelProduct.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tpanelProduct.Controls.Add(this.cbbGroup, 1, 0);
		this.tpanelProduct.Controls.Add(this.lblGroup, 0, 0);
		this.tpanelProduct.Controls.Add(this.lblMappers, 1, 3);
		this.tpanelProduct.Controls.Add(this.lblTotalRows, 1, 2);
		this.tpanelProduct.Controls.Add(this.lblMapper, 0, 3);
		this.tpanelProduct.Controls.Add(this.lblTotalRow, 0, 2);
		this.tpanelProduct.Controls.Add(this.lblProduct, 0, 1);
		this.tpanelProduct.Controls.Add(this.cbbProduct, 1, 1);
		this.tpanelProduct.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tpanelProduct.Location = new System.Drawing.Point(3, 3);
		this.tpanelProduct.Name = "tpanelProduct";
		this.tpanelProduct.RowCount = 4;
		this.tpanelProduct.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tpanelProduct.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tpanelProduct.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tpanelProduct.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tpanelProduct.Size = new System.Drawing.Size(752, 93);
		this.tpanelProduct.TabIndex = 0;
		this.lblGroup.AutoSize = true;
		this.lblGroup.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblGroup.Location = new System.Drawing.Point(4, 1);
		this.lblGroup.Name = "lblGroup";
		this.lblGroup.Size = new System.Drawing.Size(90, 28);
		this.lblGroup.TabIndex = 180;
		this.lblGroup.Text = "Product";
		this.lblGroup.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblMappers.AutoSize = true;
		this.lblMappers.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblMappers.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblMappers.ForeColor = System.Drawing.Color.Blue;
		this.lblMappers.Location = new System.Drawing.Point(101, 76);
		this.lblMappers.Name = "lblMappers";
		this.lblMappers.Size = new System.Drawing.Size(647, 16);
		this.lblMappers.TabIndex = 179;
		this.lblMappers.Text = "0";
		this.lblMappers.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblTotalRows.AutoSize = true;
		this.lblTotalRows.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTotalRows.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotalRows.Location = new System.Drawing.Point(101, 59);
		this.lblTotalRows.Name = "lblTotalRows";
		this.lblTotalRows.Size = new System.Drawing.Size(647, 16);
		this.lblTotalRows.TabIndex = 178;
		this.lblTotalRows.Text = "0";
		this.lblTotalRows.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblMapper.AutoSize = true;
		this.lblMapper.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblMapper.Location = new System.Drawing.Point(4, 76);
		this.lblMapper.Name = "lblMapper";
		this.lblMapper.Size = new System.Drawing.Size(90, 16);
		this.lblMapper.TabIndex = 177;
		this.lblMapper.Text = "Mappers";
		this.lblMapper.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblTotalRow.AutoSize = true;
		this.lblTotalRow.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTotalRow.Location = new System.Drawing.Point(4, 59);
		this.lblTotalRow.Name = "lblTotalRow";
		this.lblTotalRow.Size = new System.Drawing.Size(90, 16);
		this.lblTotalRow.TabIndex = 176;
		this.lblTotalRow.Text = "Total rows";
		this.lblTotalRow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblProduct.AutoSize = true;
		this.lblProduct.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblProduct.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblProduct.Location = new System.Drawing.Point(4, 30);
		this.lblProduct.Name = "lblProduct";
		this.lblProduct.Size = new System.Drawing.Size(90, 28);
		this.lblProduct.TabIndex = 74;
		this.lblProduct.Text = "Stage name";
		this.lblProduct.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.panelmProgressBarMain.Anchor = System.Windows.Forms.AnchorStyles.None;
		this.panelmProgressBarMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panelmProgressBarMain.Controls.Add(this.mProgressBarMain);
		this.panelmProgressBarMain.Controls.Add(this.pictureBox1);
		this.panelmProgressBarMain.Location = new System.Drawing.Point(339, 237);
		this.panelmProgressBarMain.Name = "panelmProgressBarMain";
		this.panelmProgressBarMain.Size = new System.Drawing.Size(122, 128);
		this.panelmProgressBarMain.TabIndex = 47;
		this.mProgressBarMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.mProgressBarMain.Location = new System.Drawing.Point(0, 120);
		this.mProgressBarMain.Name = "mProgressBarMain";
		this.mProgressBarMain.Size = new System.Drawing.Size(120, 6);
		this.mProgressBarMain.TabIndex = 18;
		this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
		this.pictureBox1.ErrorImage = _5S_QA_Client.Properties.Resources._5S_QA_C;
		this.pictureBox1.Image = _5S_QA_Client.Properties.Resources._5S_QA_C;
		this.pictureBox1.InitialImage = _5S_QA_Client.Properties.Resources._5S_QA_C;
		this.pictureBox1.Location = new System.Drawing.Point(0, 0);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(120, 120);
		this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.pictureBox1.TabIndex = 17;
		this.pictureBox1.TabStop = false;
		this.panelLogout.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.panelLogout.BackColor = System.Drawing.Color.Transparent;
		this.panelLogout.Controls.Add(this.lblFullname);
		this.panelLogout.Controls.Add(this.ptbAvatar);
		this.panelLogout.Location = new System.Drawing.Point(430, 25);
		this.panelLogout.Margin = new System.Windows.Forms.Padding(4);
		this.panelLogout.Name = "panelLogout";
		this.panelLogout.Size = new System.Drawing.Size(350, 42);
		this.panelLogout.TabIndex = 174;
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
		this.ptbAvatar.ErrorImage = _5S_QA_Client.Properties.Resources._5S_QA_C;
		this.ptbAvatar.Image = _5S_QA_Client.Properties.Resources._5S_QA_C;
		this.ptbAvatar.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.ptbAvatar.InitialImage = _5S_QA_Client.Properties.Resources._5S_QA_C;
		this.ptbAvatar.Location = new System.Drawing.Point(308, 0);
		this.ptbAvatar.Margin = new System.Windows.Forms.Padding(4);
		this.ptbAvatar.Name = "ptbAvatar";
		this.ptbAvatar.Size = new System.Drawing.Size(42, 42);
		this.ptbAvatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.ptbAvatar.TabIndex = 14;
		this.ptbAvatar.TabStop = false;
		this.dgvMapper.AllowUserToAddRows = false;
		this.dgvMapper.AllowUserToDeleteRows = false;
		this.dgvMapper.AllowUserToOrderColumns = true;
		this.dgvMapper.AllowUserToResizeRows = false;
		dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
		this.dgvMapper.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle7;
		this.dgvMapper.BackgroundColor = System.Drawing.SystemColors.Window;
		dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.AppWorkspace;
		dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
		dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
		dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
		dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.dgvMapper.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle8;
		this.dgvMapper.ColumnHeadersHeight = 26;
		this.dgvMapper.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
		this.dgvMapper.Columns.AddRange(this.NoMapper, this.NameMapper, this.Sample, this.CavityMapper, this.ActualMapper);
		this.dgvMapper.ContextMenuStrip = this.contextMenuStripMapper;
		this.dgvMapper.Dock = System.Windows.Forms.DockStyle.Fill;
		this.dgvMapper.EnableHeadersVisualStyles = false;
		this.dgvMapper.Location = new System.Drawing.Point(3, 0);
		this.dgvMapper.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.dgvMapper.MultiSelect = false;
		this.dgvMapper.Name = "dgvMapper";
		this.dgvMapper.ReadOnly = true;
		this.dgvMapper.RowHeadersVisible = false;
		this.dgvMapper.RowHeadersWidth = 25;
		this.dgvMapper.Size = new System.Drawing.Size(197, 383);
		this.dgvMapper.TabIndex = 175;
		this.dgvMapper.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvMapper_CellDoubleClick);
		this.dgvMapper.Sorted += new System.EventHandler(dgvMapper_Sorted);
		this.NoMapper.DataPropertyName = "No";
		dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		this.NoMapper.DefaultCellStyle = dataGridViewCellStyle9;
		this.NoMapper.HeaderText = "No.";
		this.NoMapper.Name = "NoMapper";
		this.NoMapper.ReadOnly = true;
		this.NoMapper.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.NoMapper.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.NoMapper.Width = 40;
		this.NameMapper.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.NameMapper.DataPropertyName = "Name";
		this.NameMapper.FillWeight = 50f;
		this.NameMapper.HeaderText = "Name";
		this.NameMapper.Name = "NameMapper";
		this.NameMapper.ReadOnly = true;
		this.Sample.DataPropertyName = "Sample";
		this.Sample.HeaderText = "Sample";
		this.Sample.Name = "Sample";
		this.Sample.ReadOnly = true;
		this.Sample.Visible = false;
		this.CavityMapper.DataPropertyName = "Cavity";
		this.CavityMapper.HeaderText = "Cavity";
		this.CavityMapper.Name = "CavityMapper";
		this.CavityMapper.ReadOnly = true;
		this.CavityMapper.Visible = false;
		this.ActualMapper.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.ActualMapper.DataPropertyName = "Actual";
		this.ActualMapper.FillWeight = 50f;
		this.ActualMapper.HeaderText = "Actual";
		this.ActualMapper.Name = "ActualMapper";
		this.ActualMapper.ReadOnly = true;
		this.contextMenuStripMapper.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.mapper_changeToolStripMenuItem });
		this.contextMenuStripMapper.Name = "contextMenuStripStaff";
		this.contextMenuStripMapper.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
		this.contextMenuStripMapper.Size = new System.Drawing.Size(116, 26);
		this.mapper_changeToolStripMenuItem.Name = "mapper_changeToolStripMenuItem";
		this.mapper_changeToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
		this.mapper_changeToolStripMenuItem.Text = "Change";
		this.mapper_changeToolStripMenuItem.Click += new System.EventHandler(mapper_changeToolStripMenuItem_Click);
		this.panelMapper.Controls.Add(this.dgvMapper);
		this.panelMapper.Controls.Add(this.panelMapperResize);
		this.panelMapper.Dock = System.Windows.Forms.DockStyle.Right;
		this.panelMapper.Location = new System.Drawing.Point(580, 171);
		this.panelMapper.Name = "panelMapper";
		this.panelMapper.Size = new System.Drawing.Size(200, 383);
		this.panelMapper.TabIndex = 176;
		this.panelMapper.Visible = false;
		this.panelMapperResize.Dock = System.Windows.Forms.DockStyle.Left;
		this.panelMapperResize.Location = new System.Drawing.Point(0, 0);
		this.panelMapperResize.Name = "panelMapperResize";
		this.panelMapperResize.Size = new System.Drawing.Size(3, 383);
		this.panelMapperResize.TabIndex = 0;
		this.mPanelViewMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.mPanelViewMain.Dock = System.Windows.Forms.DockStyle.Right;
		this.mPanelViewMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.mPanelViewMain.Location = new System.Drawing.Point(380, 142);
		this.mPanelViewMain.Margin = new System.Windows.Forms.Padding(4);
		this.mPanelViewMain.Name = "mPanelViewMain";
		this.mPanelViewMain.Size = new System.Drawing.Size(400, 412);
		this.mPanelViewMain.TabIndex = 46;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(800, 600);
		base.Controls.Add(this.panelLogout);
		base.Controls.Add(this.panelmProgressBarMain);
		base.Controls.Add(this.mPanelViewMain);
		base.Controls.Add(this.dgvMain);
		base.Controls.Add(this.panelMapper);
		base.Controls.Add(this.statusStripfrmMain);
		base.Controls.Add(this.panelProduct);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "frmMapperView";
		base.Padding = new System.Windows.Forms.Padding(20, 70, 20, 20);
		this.Text = "5S QA Client * MAPPER";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmMapperView_FormClosing);
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmMapperView_FormClosed);
		base.Load += new System.EventHandler(frmMapperView_Load);
		base.Shown += new System.EventHandler(frmMapperView_Shown);
		this.statusStripfrmMain.ResumeLayout(false);
		this.statusStripfrmMain.PerformLayout();
		this.contextMenuStripMain.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.dgvMain).EndInit();
		this.panelProduct.ResumeLayout(false);
		this.panelProduct.PerformLayout();
		this.tpanelProduct.ResumeLayout(false);
		this.tpanelProduct.PerformLayout();
		this.panelmProgressBarMain.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		this.panelLogout.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.ptbAvatar).EndInit();
		((System.ComponentModel.ISupportInitialize)this.dgvMapper).EndInit();
		this.contextMenuStripMapper.ResumeLayout(false);
		this.panelMapper.ResumeLayout(false);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
