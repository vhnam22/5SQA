using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using _5S_QA_Entities.Models;
using _5S_QA_System.Controls;
using _5S_QA_System.Properties;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using MetroFramework.Forms;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace _5S_QA_System;

public class frmCreateFile : MetroForm
{
	private bool IsOpen = true;

	private List<string> srcFileNames;

	private List<string> listFileName;

	private IContainer components = null;

	private SaveFileDialog saveFileDialogMain;

	private Label lblImportant;

	private Panel panelFile;

	private ToolTip toolTipMain;

	private TableLayoutPanel tableLayoutPanel2;

	private Button btnAll;

	private Button btnDetail;

	private Button btnMapper;

	private Label lblFile;

	private Button btnFile;

	private OpenFileDialog openFileDialogMain;

	private TableLayoutPanel tpanelMain;

	private Panel panel1;

	private TextBox txtImportant;

	private TextBox txtStart;

	private Label lblStart;

	public frmCreateFile()
	{
		InitializeComponent();
		Common.setControls(this, toolTipMain);
	}

	private void frmCreateFile_Load(object sender, EventArgs e)
	{
		btnMapper.Enabled = false;
		btnDetail.Enabled = false;
		btnAll.Enabled = false;
		listFileName = new List<string>();
		srcFileNames = new List<string>();
		LoadSetting();
	}

	private void frmCreateFile_FormClosed(object sender, FormClosedEventArgs e)
	{
		SaveSetting();
		GC.Collect();
	}

	private void UpdateControlVisible(bool en)
	{
		btnMapper.Enabled = en;
		btnDetail.Enabled = en;
		btnAll.Enabled = en;
	}

	private void CreatedControl()
	{
		int num = tpanelMain.Height;
		panelFile.Controls.Clear();
		foreach (string srcFileName in srcFileNames)
		{
			int no = srcFileNames.IndexOf(srcFileName) + 1;
			mPanelFileName mPanelFileName2 = new mPanelFileName(srcFileName, no)
			{
				Dock = DockStyle.Top
			};
			mPanelFileName2.btnDelete.Click += btnDelete_Click;
			panelFile.Controls.Add(mPanelFileName2);
			mPanelFileName2.BringToFront();
		}
		int num2 = tpanelMain.Height;
		base.Height += num2 - num;
		UpdateControlVisible(srcFileNames.Count > 0);
	}

	private void LoadSetting()
	{
		txtImportant.Text = Settings.Default.Important;
	}

	private void SaveSetting()
	{
		Settings.Default.Important = txtImportant.Text;
		Settings.Default.Save();
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
			MessageBox.Show(Common.getTextLanguage(this, "MAPPER.xlsx"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return null;
		}
		string text2 = Path.GetFileNameWithoutExtension(srcFileNames.First()) + "_MAPPER.xlsx";
		string text3 = Path.Combine(text, text2);
		if (IsOpen)
		{
			saveFileDialogMain.FileName = text2;
			saveFileDialogMain.Filter = "File excel (*.xls, *.xlsx)| *.xls; *.xlsx";
			if (!saveFileDialogMain.ShowDialog().Equals(DialogResult.OK))
			{
				return null;
			}
			text3 = saveFileDialogMain.FileName;
		}
		Cursor.Current = Cursors.WaitCursor;
		string sourceFileName = files[0];
		try
		{
			if (Common.FileInUse(text3))
			{
				throw new Exception(Common.getTextLanguage(this, "Openning"));
			}
			if (!IsOpen)
			{
				File.Delete(text3);
			}
			File.Copy(sourceFileName, text3, overwrite: true);
			FileInfo newFile = new FileInfo(text3);
			using (ExcelPackage excelPackage = new ExcelPackage(newFile))
			{
				if (excelPackage.Workbook.Worksheets.Count < 1)
				{
					throw new Exception(Common.getTextLanguage(this, "IncorrectFormat"));
				}
				ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets["DATA"] ?? throw new Exception(Common.getTextLanguage(this, "HasntSheet"));
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
			if (IsOpen)
			{
				Common.ExecuteBatFile(text3);
			}
		}
		catch (Exception ex)
		{
			text3 = null;
			MessageBox.Show(ex.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		return text3;
	}

	private void SaveFileForAll()
	{
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Expected O, but got Unknown
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Expected O, but got Unknown
		string fileName;
		if (listFileName.Count > 1)
		{
			saveFileDialogMain.FileName = Path.GetFileNameWithoutExtension(srcFileNames.First()) + ".zip";
			saveFileDialogMain.Filter = "File zip (*.zip)| *.zip";
			if (!saveFileDialogMain.ShowDialog().Equals(DialogResult.OK))
			{
				return;
			}
			Cursor.Current = Cursors.WaitCursor;
			fileName = saveFileDialogMain.FileName;
			ZipOutputStream val = new ZipOutputStream((Stream)File.Create(fileName));
			try
			{
				val.SetLevel(9);
				byte[] array = new byte[16384];
				for (int i = 0; i < listFileName.Count; i++)
				{
					ZipEntry val2 = new ZipEntry(Path.GetFileName(listFileName[i]))
					{
						DateTime = DateTime.Now,
						IsUnicodeText = true
					};
					val.PutNextEntry(val2);
					using FileStream fileStream = File.OpenRead(listFileName[i]);
					int num;
					do
					{
						num = fileStream.Read(array, 0, array.Length);
						((Stream)(object)val).Write(array, 0, num);
					}
					while (num > 0);
				}
				((DeflaterOutputStream)val).Finish();
				((Stream)(object)val).Flush();
				((Stream)(object)val).Close();
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		else
		{
			saveFileDialogMain.FileName = Path.GetFileName(listFileName.First()) ?? "";
			saveFileDialogMain.Filter = "File excel (*.xls, *.xlsx)| *.xls; *.xlsx";
			if (!saveFileDialogMain.ShowDialog().Equals(DialogResult.OK))
			{
				return;
			}
			Cursor.Current = Cursors.WaitCursor;
			fileName = saveFileDialogMain.FileName;
			if (Common.FileInUse(fileName))
			{
				MessageBox.Show(Common.getTextLanguage(this, "Openning"), Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			File.Copy(listFileName.First(), fileName, overwrite: true);
		}
		Common.ExecuteBatFile(fileName);
	}

	private void txtLimit_KeyPress(object sender, KeyPressEventArgs e)
	{
		if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b')
		{
			e.Handled = true;
		}
	}

	private void txtLimit_Leave(object sender, EventArgs e)
	{
		TextBox textBox = sender as TextBox;
		textBox.Text = ((!int.TryParse(textBox.Text, out var result)) ? "5" : (result.Equals(0) ? "5" : result.ToString()));
		SaveSetting();
	}

	private void txtStart_Leave(object sender, EventArgs e)
	{
		TextBox textBox = sender as TextBox;
		textBox.Text = ((!int.TryParse(textBox.Text, out var result)) ? "" : (result.Equals(0) ? "" : result.ToString()));
	}

	private void txtImportant_Leave(object sender, EventArgs e)
	{
		SaveSetting();
	}

	private void btnFile_Click(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		if (openFileDialogMain.ShowDialog().Equals(DialogResult.OK))
		{
			srcFileNames = openFileDialogMain.FileNames.ToList();
			CreatedControl();
		}
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		Button button = sender as Button;
		mPanelFileName mPanelFileName2 = button.Parent as mPanelFileName;
		srcFileNames.Remove(mPanelFileName2.FileName);
		CreatedControl();
	}

	private void btnMapper_Click(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		try
		{
			if (srcFileNames.Count.Equals(0))
			{
				throw new Exception(Common.getTextLanguage(this, "FileHasnt"));
			}
			List<Measurement> list = new List<Measurement>();
			List<Measurement> list2 = new List<Measurement>();
			string text = string.Empty;
			foreach (string srcFileName in srcFileNames)
			{
				int num = srcFileNames.IndexOf(srcFileName) + 1;
				MeasurementDto val = Common.ReadExcelForMapper(srcFileName);
				if (!string.IsNullOrEmpty(val.Message))
				{
					text += (string.IsNullOrEmpty(text) ? string.Format("{0} {1}: {2}", Common.getTextLanguage(this, "FileNo"), num, val.Message) : string.Format("\n{0} {1}: {2}", Common.getTextLanguage(this, "FileNo"), num, val.Message));
				}
				else
				{
					list2.AddRange(val.Measurements);
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				throw new Exception(text);
			}
			list.AddRange(Common.CalMeasLs(list2));
			IsOpen = sender != null;
			string text2 = SaveFileForMapper(list);
			if (!string.IsNullOrEmpty(text2))
			{
				listFileName.Add(text2);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnDetail_Click(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		IsOpen = sender != null;
		if (IsOpen)
		{
			listFileName.Clear();
		}
		try
		{
			if (srcFileNames.Count.Equals(0))
			{
				throw new Exception(Common.getTextLanguage(this, "FileHasnt"));
			}
			string text = string.Empty;
			int result = 0;
			if (!string.IsNullOrEmpty(txtStart.Text))
			{
				int.TryParse(txtStart.Text, out result);
				result--;
			}
			foreach (string srcFileName in srcFileNames)
			{
				int num = srcFileNames.IndexOf(srcFileName) + 1;
				CellAddressDto val = Common.ReadExcelForDetail(srcFileName);
				if (!string.IsNullOrEmpty(val.Message))
				{
					text += (string.IsNullOrEmpty(text) ? string.Format("{0} {1}: {2}", Common.getTextLanguage(this, "FileNo"), num, val.Message) : string.Format("\n{0} {1}: {2}", Common.getTextLanguage(this, "FileNo"), num, val.Message));
					continue;
				}
				string text2 = Common.SaveFileForDetail(val.CellAddresses, srcFileName, result, IsOpen);
				if (text2.Contains("ERROR: "))
				{
					text += (string.IsNullOrEmpty(text) ? string.Format("{0} {1}: {2}", Common.getTextLanguage(this, "FileNo"), num, text2.Replace("ERROR: ", "")) : string.Format("\n{0} {1}: {2}", Common.getTextLanguage(this, "FileNo"), num, text2.Replace("ERROR: ", "")));
					continue;
				}
				if (!string.IsNullOrEmpty(text2))
				{
					listFileName.Add(text2);
				}
				result += val.CellAddresses.Count((CellAddress x) => x.IsSet);
			}
			if (!string.IsNullOrEmpty(text))
			{
				throw new Exception(text);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			return;
		}
		if (IsOpen && listFileName.Count > 0)
		{
			SaveFileForAll();
		}
	}

	private void btnAll_Click(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		try
		{
			if (srcFileNames.Count.Equals(0))
			{
				throw new Exception(Common.getTextLanguage(this, "FileHasnt"));
			}
			listFileName.Clear();
			btnMapper_Click(null, null);
			if (listFileName.Count > 0)
			{
				btnDetail_Click(null, null);
			}
			if (listFileName.Count > 0)
			{
				SaveFileForAll();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_5S_QA_System.frmCreateFile));
		this.saveFileDialogMain = new System.Windows.Forms.SaveFileDialog();
		this.lblImportant = new System.Windows.Forms.Label();
		this.panelFile = new System.Windows.Forms.Panel();
		this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
		this.btnAll = new System.Windows.Forms.Button();
		this.btnDetail = new System.Windows.Forms.Button();
		this.btnMapper = new System.Windows.Forms.Button();
		this.lblFile = new System.Windows.Forms.Label();
		this.btnFile = new System.Windows.Forms.Button();
		this.openFileDialogMain = new System.Windows.Forms.OpenFileDialog();
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.txtImportant = new System.Windows.Forms.TextBox();
		this.txtStart = new System.Windows.Forms.TextBox();
		this.tpanelMain = new System.Windows.Forms.TableLayoutPanel();
		this.lblStart = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tableLayoutPanel2.SuspendLayout();
		this.tpanelMain.SuspendLayout();
		this.panel1.SuspendLayout();
		base.SuspendLayout();
		this.saveFileDialogMain.Filter = "File excel (*.xls, *.xlsx, *.xlsm)| *.xls; *.xlsx; *.xlsm";
		this.saveFileDialogMain.Title = "Select the path to save the file";
		this.lblImportant.AutoSize = true;
		this.lblImportant.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblImportant.Location = new System.Drawing.Point(4, 25);
		this.lblImportant.Name = "lblImportant";
		this.lblImportant.Size = new System.Drawing.Size(82, 28);
		this.lblImportant.TabIndex = 6;
		this.lblImportant.Text = "Important";
		this.lblImportant.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.panelFile.AutoSize = true;
		this.panelFile.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panelFile.Location = new System.Drawing.Point(90, 1);
		this.panelFile.Margin = new System.Windows.Forms.Padding(0);
		this.panelFile.Name = "panelFile";
		this.panelFile.Size = new System.Drawing.Size(360, 23);
		this.panelFile.TabIndex = 3;
		this.tableLayoutPanel2.ColumnCount = 3;
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333f));
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333f));
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333f));
		this.tableLayoutPanel2.Controls.Add(this.btnAll, 2, 0);
		this.tableLayoutPanel2.Controls.Add(this.btnDetail, 1, 0);
		this.tableLayoutPanel2.Controls.Add(this.btnMapper, 0, 0);
		this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.tableLayoutPanel2.Location = new System.Drawing.Point(20, 161);
		this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
		this.tableLayoutPanel2.Name = "tableLayoutPanel2";
		this.tableLayoutPanel2.RowCount = 1;
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel2.Size = new System.Drawing.Size(500, 28);
		this.tableLayoutPanel2.TabIndex = 2;
		this.btnAll.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnAll.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAll.Location = new System.Drawing.Point(332, 0);
		this.btnAll.Margin = new System.Windows.Forms.Padding(0);
		this.btnAll.Name = "btnAll";
		this.btnAll.Size = new System.Drawing.Size(168, 28);
		this.btnAll.TabIndex = 3;
		this.btnAll.Text = "Created file all";
		this.toolTipMain.SetToolTip(this.btnAll, "Created file all (mapper and detail)");
		this.btnAll.UseVisualStyleBackColor = true;
		this.btnAll.Click += new System.EventHandler(btnAll_Click);
		this.btnDetail.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnDetail.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDetail.Location = new System.Drawing.Point(166, 0);
		this.btnDetail.Margin = new System.Windows.Forms.Padding(0);
		this.btnDetail.Name = "btnDetail";
		this.btnDetail.Size = new System.Drawing.Size(166, 28);
		this.btnDetail.TabIndex = 2;
		this.btnDetail.Text = "Created file detail";
		this.toolTipMain.SetToolTip(this.btnDetail, "Created file detail");
		this.btnDetail.UseVisualStyleBackColor = true;
		this.btnDetail.Click += new System.EventHandler(btnDetail_Click);
		this.btnMapper.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnMapper.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnMapper.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnMapper.Location = new System.Drawing.Point(0, 0);
		this.btnMapper.Margin = new System.Windows.Forms.Padding(0);
		this.btnMapper.Name = "btnMapper";
		this.btnMapper.Size = new System.Drawing.Size(166, 28);
		this.btnMapper.TabIndex = 1;
		this.btnMapper.Text = "Created file mapper";
		this.toolTipMain.SetToolTip(this.btnMapper, "Created file mapper");
		this.btnMapper.UseVisualStyleBackColor = true;
		this.btnMapper.Click += new System.EventHandler(btnMapper_Click);
		this.lblFile.AutoSize = true;
		this.lblFile.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblFile.Location = new System.Drawing.Point(4, 1);
		this.lblFile.Name = "lblFile";
		this.lblFile.Size = new System.Drawing.Size(82, 23);
		this.lblFile.TabIndex = 0;
		this.lblFile.Text = "File";
		this.lblFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.btnFile.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnFile.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnFile.Location = new System.Drawing.Point(451, 1);
		this.btnFile.Margin = new System.Windows.Forms.Padding(0);
		this.btnFile.Name = "btnFile";
		this.btnFile.Size = new System.Drawing.Size(40, 23);
		this.btnFile.TabIndex = 1;
		this.btnFile.Text = "...";
		this.toolTipMain.SetToolTip(this.btnFile, "Select file");
		this.btnFile.UseVisualStyleBackColor = true;
		this.btnFile.Click += new System.EventHandler(btnFile_Click);
		this.openFileDialogMain.Filter = "File excel (*.xls, *.xlsx, *xlsm)| *.xls; *.xlsx; *.xlsm";
		this.openFileDialogMain.Multiselect = true;
		this.openFileDialogMain.Title = "Select file checksheet excel";
		this.txtImportant.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtImportant.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtImportant.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtImportant.Location = new System.Drawing.Point(93, 28);
		this.txtImportant.Name = "txtImportant";
		this.txtImportant.Size = new System.Drawing.Size(354, 22);
		this.txtImportant.TabIndex = 3;
		this.toolTipMain.SetToolTip(this.txtImportant, "Please enter important");
		this.txtImportant.Leave += new System.EventHandler(txtImportant_Leave);
		this.txtStart.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
		this.txtStart.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtStart.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtStart.Location = new System.Drawing.Point(93, 57);
		this.txtStart.Name = "txtStart";
		this.txtStart.Size = new System.Drawing.Size(354, 22);
		this.txtStart.TabIndex = 5;
		this.toolTipMain.SetToolTip(this.txtStart, "Please enter start number for file detail");
		this.txtStart.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtLimit_KeyPress);
		this.txtStart.Leave += new System.EventHandler(txtStart_Leave);
		this.tpanelMain.AutoSize = true;
		this.tpanelMain.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		this.tpanelMain.ColumnCount = 3;
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tpanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMain.Controls.Add(this.txtStart, 1, 2);
		this.tpanelMain.Controls.Add(this.lblStart, 0, 2);
		this.tpanelMain.Controls.Add(this.txtImportant, 1, 1);
		this.tpanelMain.Controls.Add(this.lblImportant, 0, 1);
		this.tpanelMain.Controls.Add(this.panelFile, 1, 0);
		this.tpanelMain.Controls.Add(this.lblFile, 0, 0);
		this.tpanelMain.Controls.Add(this.btnFile, 2, 0);
		this.tpanelMain.Dock = System.Windows.Forms.DockStyle.Top;
		this.tpanelMain.Location = new System.Drawing.Point(3, 3);
		this.tpanelMain.Margin = new System.Windows.Forms.Padding(0);
		this.tpanelMain.Name = "tpanelMain";
		this.tpanelMain.RowCount = 3;
		this.tpanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tpanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tpanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tpanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tpanelMain.Size = new System.Drawing.Size(492, 83);
		this.tpanelMain.TabIndex = 1;
		this.lblStart.AutoSize = true;
		this.lblStart.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblStart.Location = new System.Drawing.Point(4, 54);
		this.lblStart.Name = "lblStart";
		this.lblStart.Size = new System.Drawing.Size(82, 28);
		this.lblStart.TabIndex = 8;
		this.lblStart.Text = "Start number";
		this.lblStart.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.panel1.AutoScroll = true;
		this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel1.Controls.Add(this.tpanelMain);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(20, 70);
		this.panel1.Name = "panel1";
		this.panel1.Padding = new System.Windows.Forms.Padding(3);
		this.panel1.Size = new System.Drawing.Size(500, 91);
		this.panel1.TabIndex = 1;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(540, 209);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.tableLayoutPanel2);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		base.Name = "frmCreateFile";
		base.Padding = new System.Windows.Forms.Padding(20, 70, 20, 20);
		this.Text = "5S QA System * AUTO MAPPER FILE";
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmCreateFile_FormClosed);
		base.Load += new System.EventHandler(frmCreateFile_Load);
		this.tableLayoutPanel2.ResumeLayout(false);
		this.tpanelMain.ResumeLayout(false);
		this.tpanelMain.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		base.ResumeLayout(false);
	}
}
