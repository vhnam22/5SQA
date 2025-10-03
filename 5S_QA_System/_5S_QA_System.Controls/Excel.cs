using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;

namespace _5S_QA_System.Controls;

internal class Excel
{
	private string path = "";

	private _Application excel = (Microsoft.Office.Interop.Excel.Application)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("00024500-0000-0000-C000-000000000046")));

	private Workbook wb;

	private Worksheet ws;

	private Range range;

	public static object XlFileFormat { get; private set; }

	public static object XlSaveAsAccessMode { get; private set; }

	public Excel(string path)
	{
		try
		{
			this.path = path;
			excel.ScreenUpdating = false;
			excel.DisplayAlerts = false;
			wb = excel.Workbooks.Open(path, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
			ws = (dynamic)wb.Worksheets[1];
			range = ws.UsedRange;
		}
		catch
		{
		}
	}

	public Excel(string path, string sheet)
	{
		try
		{
			this.path = path;
			excel.ScreenUpdating = false;
			excel.DisplayAlerts = false;
			wb = excel.Workbooks.Open(path, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
			ws = (dynamic)wb.Worksheets[sheet];
			range = ws.UsedRange;
		}
		catch
		{
		}
	}

	public Excel()
	{
		try
		{
			if (excel == null)
			{
				MessageBox.Show("This device isn't installed EXCEL.", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			object value = Missing.Value;
			wb = excel.Workbooks.Add(value);
			ws = (dynamic)wb.Worksheets.get_Item((object)1);
		}
		catch
		{
		}
	}

	public void WriteToCell(int row, int col, string s)
	{
		try
		{
			((dynamic)ws.Cells[row, col]).Value2 = s;
		}
		catch
		{
		}
	}

	public void WriteToCell(string pos, string s)
	{
		try
		{
			((_Worksheet)ws).get_Range((object)pos, Type.Missing).Value2 = s;
		}
		catch
		{
		}
	}

	public void ExportPDF(string outputPath)
	{
		try
		{
			ws.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, outputPath, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
		}
		catch
		{
		}
	}

	public void AddComment(int row, int col, string s)
	{
		try
		{
			((dynamic)ws.Cells[row, col]).AddComment(s);
		}
		catch
		{
		}
	}

	public void AddComment(string pos, string s)
	{
		try
		{
			((_Worksheet)ws).get_Range((object)pos, Type.Missing).AddComment(s);
		}
		catch
		{
		}
	}

	public void FormatCell(int row, int col, Color color)
	{
		try
		{
			((dynamic)ws.Cells[row, col]).Interior.Color = color;
		}
		catch
		{
		}
	}

	public void FormatCell(string pos, string s)
	{
		try
		{
			switch (s)
			{
			case "Bold":
				((_Worksheet)ws).get_Range((object)pos, Type.Missing).Font.Bold = true;
				break;
			case "Italic":
				((_Worksheet)ws).get_Range((object)pos, Type.Missing).Font.Italic = true;
				break;
			case "Underline":
				((_Worksheet)ws).get_Range((object)pos, Type.Missing).Font.Underline = true;
				break;
			case "Red":
				((_Worksheet)ws).get_Range((object)pos, Type.Missing).Font.Color = Color.Red;
				break;
			case "AlignRight":
				((_Worksheet)ws).get_Range((object)pos, Type.Missing).HorizontalAlignment = XlHAlign.xlHAlignRight;
				break;
			case "AlignCenter":
				((_Worksheet)ws).get_Range((object)pos, Type.Missing).HorizontalAlignment = XlHAlign.xlHAlignCenter;
				break;
			case "Percentage":
				((_Worksheet)ws).get_Range((object)pos, Type.Missing).NumberFormat = "0%";
				break;
			case "VND":
				((_Worksheet)ws).get_Range((object)pos, Type.Missing).NumberFormat = "\"VND\" #,##0\". -\";-\"VND\" #,##0\". -\"";
				break;
			case "JPY":
				((_Worksheet)ws).get_Range((object)pos, Type.Missing).NumberFormat = "\"JPY\" #,##0\". -\";-\"JPY\" #,##0\". -\"";
				break;
			case "USD":
				((_Worksheet)ws).get_Range((object)pos, Type.Missing).NumberFormat = "\"USD\" #,##0\". -\";-\"USD\" #,##0\". -\"";
				break;
			}
		}
		catch
		{
		}
	}

	public Range ReadFromCell()
	{
		return range;
	}

	public void Save()
	{
		try
		{
			wb.Save();
		}
		catch
		{
		}
	}

	public void SaveAs(string path)
	{
		try
		{
			wb.SaveAs(path, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
		}
		catch
		{
		}
	}

	public void Close()
	{
		wb.Close(false, Type.Missing, Type.Missing);
		excel.Quit();
		Process[] processesByName = Process.GetProcessesByName("EXCEL");
		Process[] array = processesByName;
		foreach (Process process in array)
		{
			if (process.MainWindowTitle.Equals(""))
			{
				try
				{
					process.Kill();
				}
				catch
				{
				}
			}
		}
	}

	public string checkFormat(string[] header, int row = 1)
	{
		if (range.Columns.Count < header.Length)
		{
			return "File template excel have columns count incorrect.";
		}
		for (int i = 0; i < header.Length; i++)
		{
			string text = ((dynamic)range.Cells[row, i + 1]).Value;
			if (!Common.trimSpace(text.ToUpper()).Equals(header[i].ToUpper()))
			{
				return $"File template excel have header: App->[{header[i]}], Excel->[{text}] at column [{i + 1}] incorrect.";
			}
		}
		return "Ok";
	}
}
