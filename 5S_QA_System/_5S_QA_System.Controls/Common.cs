using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using _5S_QA_DL.APIContext;
using _5S_QA_Entities.Abstracts;
using _5S_QA_Entities.Constants;
using _5S_QA_Entities.Dtos;
using _5S_QA_Entities.Models;
using _5S_QA_System.Properties;
using MetroFramework.Controls;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace _5S_QA_System.Controls;

public static class Common
{
	public static DataTable getDataTable<T>(object obj)
	{
		DataTable dataTable = new DataTable(typeof(T).Name);
		try
		{
			dataTable.Columns.Add("No", typeof(ulong));
			PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
			PropertyInfo[] array = properties;
			foreach (PropertyInfo propertyInfo in array)
			{
				Type type = ((propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) ? Nullable.GetUnderlyingType(propertyInfo.PropertyType) : propertyInfo.PropertyType);
				dataTable.Columns.Add(propertyInfo.Name, type);
			}
			ResponseDto responseDto = obj as ResponseDto;
			if (!responseDto.Success)
			{
				throw new Exception(responseDto.Messages.ElementAt(0).Message);
			}
			if (responseDto.Count > 0 && !responseDto.Data.ToString().Equals("[]"))
			{
				string value = responseDto.Data.ToString();
				IEnumerable<T> enumerable = JsonConvert.DeserializeObject<IEnumerable<T>>(value);
				ulong num = 0uL;
				foreach (T item in enumerable)
				{
					num++;
					object[] array2 = new object[properties.Length + 1];
					for (int j = 0; j < properties.Length + 1; j++)
					{
						if (j.Equals(0))
						{
							array2[j] = num;
						}
						else
						{
							array2[j] = properties[j - 1].GetValue(item, null);
						}
					}
					dataTable.Rows.Add(array2);
				}
			}
		}
		finally
		{
			dataTable.Dispose();
		}
		return dataTable;
	}

	public static DataTable getDataTableNoType<T>(object obj)
	{
		DataTable dataTable = new DataTable(typeof(T).Name);
		try
		{
			dataTable.Columns.Add("No", typeof(ulong));
			PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
			PropertyInfo[] array = properties;
			foreach (PropertyInfo propertyInfo in array)
			{
				Type type = ((propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) ? Nullable.GetUnderlyingType(propertyInfo.PropertyType) : propertyInfo.PropertyType);
				if (type == typeof(double?) || type == typeof(double))
				{
					type = typeof(string);
				}
				dataTable.Columns.Add(propertyInfo.Name, type);
			}
			ResponseDto responseDto = obj as ResponseDto;
			if (!responseDto.Success)
			{
				throw new Exception(responseDto.Messages.ElementAt(0).Message);
			}
			if (responseDto.Count > 0 && !responseDto.Data.ToString().Equals("[]"))
			{
				string value = responseDto.Data.ToString();
				IEnumerable<T> enumerable = JsonConvert.DeserializeObject<IEnumerable<T>>(value);
				ulong num = 0uL;
				foreach (T item in enumerable)
				{
					num++;
					object[] array2 = new object[properties.Length + 1];
					for (int j = 0; j < properties.Length + 1; j++)
					{
						if (j.Equals(0))
						{
							array2[j] = num;
						}
						else
						{
							array2[j] = properties[j - 1].GetValue(item, null);
						}
					}
					dataTable.Rows.Add(array2);
				}
			}
		}
		finally
		{
			dataTable.Dispose();
		}
		return dataTable;
	}

	public static DataTable getDataTableIsSelect<T>(object obj, bool select = true)
	{
		DataTable dataTable = new DataTable(typeof(T).Name);
		try
		{
			dataTable.Columns.Add("No", typeof(ulong));
			PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
			PropertyInfo[] array = properties;
			foreach (PropertyInfo propertyInfo in array)
			{
				Type type = ((propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) ? Nullable.GetUnderlyingType(propertyInfo.PropertyType) : propertyInfo.PropertyType);
				dataTable.Columns.Add(propertyInfo.Name, type);
			}
			ResponseDto responseDto = obj as ResponseDto;
			if (!responseDto.Success)
			{
				throw new Exception(responseDto.Messages.ElementAt(0).Message);
			}
			if (responseDto.Count > 0 && !responseDto.Data.ToString().Equals("[]"))
			{
				string value = responseDto.Data.ToString();
				IEnumerable<T> enumerable = JsonConvert.DeserializeObject<IEnumerable<T>>(value);
				ulong num = 0uL;
				foreach (T item in enumerable)
				{
					num++;
					object[] array2 = new object[properties.Length + 1];
					for (int j = 0; j < properties.Length + 1; j++)
					{
						if (j.Equals(0))
						{
							array2[j] = num;
						}
						else if (j.Equals(1))
						{
							array2[j] = select;
						}
						else
						{
							array2[j] = properties[j - 1].GetValue(item, null);
						}
					}
					dataTable.Rows.Add(array2);
				}
			}
		}
		finally
		{
			dataTable.Dispose();
		}
		return dataTable;
	}

	public static DataTable getDataTableIsSelectNoType<T>(object obj, bool select = true)
	{
		DataTable dataTable = new DataTable(typeof(T).Name);
		try
		{
			dataTable.Columns.Add("No", typeof(ulong));
			PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
			PropertyInfo[] array = properties;
			foreach (PropertyInfo propertyInfo in array)
			{
				Type type = ((propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) ? Nullable.GetUnderlyingType(propertyInfo.PropertyType) : propertyInfo.PropertyType);
				if (type == typeof(double?) || type == typeof(double))
				{
					type = typeof(string);
				}
				dataTable.Columns.Add(propertyInfo.Name, type);
			}
			ResponseDto responseDto = obj as ResponseDto;
			if (!responseDto.Success)
			{
				throw new Exception(responseDto.Messages.ElementAt(0).Message);
			}
			if (responseDto.Count > 0 && !responseDto.Data.ToString().Equals("[]"))
			{
				string value = responseDto.Data.ToString();
				IEnumerable<T> enumerable = JsonConvert.DeserializeObject<IEnumerable<T>>(value);
				ulong num = 0uL;
				foreach (T item in enumerable)
				{
					num++;
					object[] array2 = new object[properties.Length + 1];
					for (int j = 0; j < properties.Length + 1; j++)
					{
						if (j.Equals(0))
						{
							array2[j] = num;
						}
						else if (j.Equals(1))
						{
							array2[j] = select;
						}
						else
						{
							array2[j] = properties[j - 1].GetValue(item, null);
						}
					}
					dataTable.Rows.Add(array2);
				}
			}
		}
		finally
		{
			dataTable.Dispose();
		}
		return dataTable;
	}

	public static List<T> getObjectToList<T>(object obj)
	{
		List<T> result = new List<T>();
		try
		{
			ResponseDto responseDto = obj as ResponseDto;
			if (!responseDto.Success)
			{
				throw new Exception(responseDto.Messages.ElementAt(0).Message);
			}
			if (responseDto.Count > 0 && !responseDto.Data.ToString().Equals("[]"))
			{
				string value = responseDto.Data.ToString();
				result = JsonConvert.DeserializeObject<List<T>>(value);
			}
		}
		finally
		{
		}
		return result;
	}

	public static T getObject<T>(object obj) where T : new()
	{
		T result = new T();
		try
		{
			ResponseDto responseDto = obj as ResponseDto;
			if (!responseDto.Success)
			{
				throw new Exception(responseDto.Messages.ElementAt(0).Message);
			}
			if (responseDto.Count > 0 && !responseDto.Data.ToString().Equals("[]"))
			{
				string value = responseDto.Data.ToString();
				result = JsonConvert.DeserializeObject<T>(value);
			}
		}
		finally
		{
		}
		return result;
	}

	public static T Cast<T>(DataRowView dataRow) where T : new()
	{
		T val = new T();
		IEnumerable<PropertyInfo> source = from x in val.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
			where x.CanWrite
			select x;
		foreach (DataColumn column in dataRow.Row.Table.Columns)
		{
			if (dataRow[column.ColumnName] == DBNull.Value)
			{
				continue;
			}
			PropertyInfo propertyInfo = source.FirstOrDefault((PropertyInfo x) => column.ColumnName.Equals(x.Name, StringComparison.OrdinalIgnoreCase));
			if (!(propertyInfo == null))
			{
				try
				{
					Type conversionType = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
					object value = ((dataRow[column.ColumnName] == null) ? null : Convert.ChangeType(dataRow[column.ColumnName], conversionType));
					propertyInfo.SetValue(val, value, null);
				}
				catch
				{
				}
			}
		}
		return val;
	}

	public static T Cast<T>(DataRow dataRow) where T : new()
	{
		T val = new T();
		IEnumerable<PropertyInfo> source = from x in val.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
			where x.CanWrite
			select x;
		foreach (DataColumn column in dataRow.Table.Columns)
		{
			if (dataRow[column.ColumnName] == DBNull.Value)
			{
				continue;
			}
			PropertyInfo propertyInfo = source.FirstOrDefault((PropertyInfo x) => column.ColumnName.Equals(x.Name, StringComparison.OrdinalIgnoreCase));
			if (!(propertyInfo == null))
			{
				try
				{
					Type conversionType = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
					object value = ((dataRow[column.ColumnName] == null) ? null : Convert.ChangeType(dataRow[column.ColumnName], conversionType));
					propertyInfo.SetValue(val, value, null);
				}
				catch
				{
				}
			}
		}
		return val;
	}

	public static List<T> Casts<T>(DataTable dt) where T : new()
	{
		List<T> list = new List<T>();
		foreach (DataRow row in dt.Rows)
		{
			T val = Cast<T>(row);
			if (val != null)
			{
				list.Add(val);
			}
		}
		return list;
	}

	public static DataTable getDataTable(object obj)
	{
		DataTable dataTable = null;
		try
		{
			ResponseDto responseDto = obj as ResponseDto;
			if (!responseDto.Success)
			{
				throw new Exception(responseDto.Messages.ElementAt(0).Message);
			}
			if (responseDto.Count > 0 && !responseDto.Data.ToString().Equals("[]"))
			{
				string value = responseDto.Data.ToString();
				dataTable = JsonConvert.DeserializeObject<DataTable>(value);
			}
		}
		finally
		{
			dataTable?.Dispose();
		}
		return dataTable;
	}

	public static DataTable getDataTable<T>(string obj)
	{
		DataTable dataTable = new DataTable(typeof(T).Name);
		try
		{
			dataTable.Columns.Add("No", typeof(ulong));
			PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
			PropertyInfo[] array = properties;
			foreach (PropertyInfo propertyInfo in array)
			{
				Type type = ((propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) ? Nullable.GetUnderlyingType(propertyInfo.PropertyType) : propertyInfo.PropertyType);
				dataTable.Columns.Add(propertyInfo.Name, type);
			}
			if (!string.IsNullOrEmpty(obj) && !obj.Equals("[]"))
			{
				IEnumerable<T> enumerable = JsonConvert.DeserializeObject<IEnumerable<T>>(obj);
				ulong num = 0uL;
				foreach (T item in enumerable)
				{
					num++;
					object[] array2 = new object[properties.Length + 1];
					for (int j = 0; j < properties.Length + 1; j++)
					{
						if (j.Equals(0))
						{
							array2[j] = num;
						}
						else
						{
							array2[j] = properties[j - 1].GetValue(item, null);
						}
					}
					dataTable.Rows.Add(array2);
				}
			}
		}
		finally
		{
			dataTable.Dispose();
		}
		return dataTable;
	}

	public static DataTable reverseDatatable(DataTable dt)
	{
		DataTable dataTable = dt.Clone();
		for (int num = dt.Rows.Count - 1; num >= 0; num--)
		{
			dataTable.ImportRow(dt.Rows[num]);
		}
		return dataTable;
	}

	public static string trimSpace(string str)
	{
		str = str.Trim();
		Regex regex = new Regex("\\s+");
		return regex.Replace(str, " ");
	}

	public static string trimChar(string str)
	{
		str = str.Trim('-');
		Regex regex = new Regex("-+");
		return regex.Replace(str, "-");
	}

	public static AutoCompleteStringCollection getAutoComplete(DataTable dt, string title)
	{
		AutoCompleteStringCollection autoCompleteStringCollection = null;
		try
		{
			if (dt != null && dt.Rows.Count > 0)
			{
				string[] array = new string[0];
				array = ((!title.ToLower().Equals("limit") && !title.ToLower().Equals("sample") && !title.ToLower().Equals("orderquantity") && !title.ToLower().Equals("checkquantity") && !title.ToLower().Equals("quantity") && !title.ToLower().Equals("sort") && !title.ToLower().Equals("inputquantity") && !title.ToLower().Equals("outputquantity") && !title.ToLower().Equals("mark")) ? (from x in dt.AsEnumerable()
					select x.Field<string>(title)).ToArray() : (from x in dt.AsEnumerable()
					select x.Field<int?>(title).ToString()).ToArray());
				autoCompleteStringCollection = new AutoCompleteStringCollection();
				string[] array2 = array;
				foreach (string value in array2)
				{
					if (!string.IsNullOrEmpty(value))
					{
						autoCompleteStringCollection.Add(value);
					}
				}
			}
		}
		finally
		{
		}
		return autoCompleteStringCollection;
	}

	public static void ExecuteBatFile(string filename)
	{
		try
		{
			Process.Start(new ProcessStartInfo
			{
				FileName = filename,
				UseShellExecute = true
			});
		}
		catch
		{
		}
	}

	public static bool FileInUse(string filepath)
	{
		try
		{
			using (FileStream fileStream = new FileStream(filepath, FileMode.OpenOrCreate))
			{
				fileStream.Close();
			}
			return false;
		}
		catch
		{
			return true;
		}
	}

	public static void KillApp(string exeName)
	{
		Process[] processesByName = Process.GetProcessesByName(exeName);
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

	public static void closeAllForm()
	{
		Form[] array = Application.OpenForms.Cast<Form>().ToArray();
		Form[] array2 = array;
		foreach (Form form in array2)
		{
			if (!form.GetType().Name.Equals(typeof(frmLogin).Name) && form.Owner != null)
			{
				form.Close();
			}
		}
	}

	public static void closeForm(List<Type> types)
	{
		Form[] array = Application.OpenForms.Cast<Form>().ToArray();
		Form[] array2 = array;
		foreach (Form form in array2)
		{
			foreach (Type type in types)
			{
				if (form.GetType().Equals(type))
				{
					form.Close();
					break;
				}
			}
		}
	}

	public static bool activeForm(Type type)
	{
		foreach (Form openForm in Application.OpenForms)
		{
			if (openForm.GetType().Equals(type))
			{
				if (openForm.WindowState.Equals(FormWindowState.Minimized))
				{
					openForm.WindowState = FormWindowState.Normal;
				}
				openForm.Activate();
				return true;
			}
		}
		return false;
	}

	public static bool activeForm(Type type, string title)
	{
		foreach (Form openForm in Application.OpenForms)
		{
			if (openForm.GetType().Equals(type) && openForm.Text.Contains(title))
			{
				if (openForm.WindowState.Equals(FormWindowState.Minimized))
				{
					openForm.WindowState = FormWindowState.Normal;
				}
				openForm.Activate();
				return true;
			}
		}
		return false;
	}

	public static Form findForm(Type type)
	{
		foreach (Form openForm in Application.OpenForms)
		{
			if (openForm.GetType().Equals(type))
			{
				return openForm;
			}
		}
		return null;
	}

	public static string checkFormat(string[] header, ExcelRange cells, int row = 1)
	{
		if (cells.Columns < header.Length)
		{
			return "File template excel have column count incorrect.";
		}
		for (int i = 0; i < header.Length; i++)
		{
			string text = trimSpace((cells[row, i + 1].Value == null) ? string.Empty : cells[row, i + 1].Value.ToString()).ToLower();
			if (!text.Equals(header[i].ToLower()))
			{
				return $"File template excel have header: App->[{header[i]}], Excel->[{text}] at column [{i + 1}] incorrect.";
			}
		}
		return "Ok";
	}

	public static Bitmap MergeImages(List<Image> images)
	{
		int num = 0;
		int num2 = 0;
		foreach (Image image in images)
		{
			num = ((image.Width > num) ? image.Width : num);
			num2 += image.Height;
		}
		Bitmap bitmap = new Bitmap(num, num2);
		using (Graphics graphics = Graphics.FromImage(bitmap))
		{
			int num3 = 0;
			foreach (Image image2 in images)
			{
				graphics.DrawImage(new Bitmap(image2), 0, num3);
				num3 += image2.Height;
			}
		}
		return bitmap;
	}

	public static void setControls(Control control, ToolTip tooltip = null, List<ContextMenuStrip> contexts = null)
	{
		List<Language> list = ReadLanguages(control.GetType().Name);
		if (list == null)
		{
			return;
		}
		IEnumerable<Control> all = GetAll(control);
		string name = Settings.Default.Language.Replace("rb", "Name");
		string name2 = Settings.Default.Language.Replace("rb", "Tooltip");
		Language val = list.FirstOrDefault((Language x) => control.Name.Equals(x.Code));
		if (val != null)
		{
			object value = ((object)val).GetType().GetProperty(name).GetValue(val, null);
			if (value != null)
			{
				control.Text = $"{value}";
			}
		}
		foreach (Control ctrl in all)
		{
			val = list.FirstOrDefault((Language x) => ctrl.Name.Replace("lbl", "txt").Equals(x.Code));
			if (val != null)
			{
				object value2 = ((object)val).GetType().GetProperty(name).GetValue(val, null);
				if (value2 != null)
				{
					if (ctrl.GetType().Equals(typeof(MetroTextBox)))
					{
						((MetroTextBox)ctrl).WaterMark = value2.ToString();
					}
					else if (!ctrl.GetType().Equals(typeof(TextBox)) && !ctrl.GetType().Equals(typeof(DateTimePicker)))
					{
						ctrl.Text = value2.ToString();
					}
				}
				if (tooltip != null && !ctrl.GetType().Equals(typeof(Label)) && !ctrl.GetType().Equals(typeof(MetroLabel)))
				{
					value2 = ((object)val).GetType().GetProperty(name2).GetValue(val, null);
					if (value2 != null)
					{
						tooltip.SetToolTip(ctrl, value2.ToString());
					}
				}
			}
			if (!ctrl.GetType().Equals(typeof(DataGridView)))
			{
				continue;
			}
			DataGridView dataGridView = ctrl as DataGridView;
			foreach (DataGridViewColumn col in dataGridView.Columns)
			{
				val = list.FirstOrDefault((Language x) => col.Name.Equals(x.Code));
				if (val != null)
				{
					object value3 = ((object)val).GetType().GetProperty(name).GetValue(val, null);
					if (value3 != null)
					{
						col.HeaderText = value3.ToString();
					}
				}
			}
		}
		if (contexts == null)
		{
			return;
		}
		foreach (ContextMenuStrip context in contexts)
		{
			foreach (object item in context.Items)
			{
				if (!item.GetType().Equals(typeof(ToolStripMenuItem)))
				{
					continue;
				}
				ToolStripMenuItem _menu = item as ToolStripMenuItem;
				val = list.FirstOrDefault((Language x) => _menu.Name.StartsWith(x.Code));
				if (val != null)
				{
					object value4 = ((object)val).GetType().GetProperty(name).GetValue(val, null);
					if (value4 != null)
					{
						_menu.Text = value4.ToString();
					}
				}
			}
		}
	}

	public static string getTextLanguage(Control control, string code)
	{
		string result = string.Empty;
		List<Language> list = ReadLanguages(control.GetType().Name);
		if (list != null)
		{
			string name = Settings.Default.Language.Replace("rb", "Name");
			Language val = list.FirstOrDefault((Language x) => code.Equals(x.Code));
			if (val != null)
			{
				object value = ((object)val).GetType().GetProperty(name).GetValue(val, null);
				if (value != null)
				{
					result = value.ToString();
				}
			}
		}
		return result;
	}

	public static string getTextLanguage(string control, string code)
	{
		string result = string.Empty;
		List<Language> list = ReadLanguages(control);
		if (list != null)
		{
			string name = Settings.Default.Language.Replace("rb", "Name");
			Language val = list.FirstOrDefault((Language x) => code.Equals(x.Code));
			if (val != null)
			{
				object value = ((object)val).GetType().GetProperty(name).GetValue(val, null);
				if (value != null)
				{
					result = value.ToString();
				}
			}
		}
		return result;
	}

	private static IEnumerable<Control> GetAll(Control control)
	{
		List<Type> types = new List<Type>
		{
			typeof(Label),
			typeof(MetroLabel),
			typeof(RadioButton),
			typeof(MetroCheckBox),
			typeof(Button),
			typeof(MetroButton),
			typeof(MetroTextBox.MetroTextButton),
			typeof(MetroTextBox),
			typeof(DateTimePicker),
			typeof(RichTextBox),
			typeof(GroupBox),
			typeof(ComboBox),
			typeof(DataGridView),
			typeof(TextBox),
			typeof(CheckBox),
			typeof(PictureBox),
			typeof(TabPage)
		};
		IEnumerable<Control> enumerable = from Control x in control.Controls
			where !x.Name.StartsWith("m")
			select x;
		return from c in enumerable.SelectMany((Control ctrl) => GetAll(ctrl)).Concat(enumerable)
			where types.Contains(c.GetType())
			select c;
	}

	public static IEnumerable<Control> GetControlsOfType<T>(Control control) where T : Control
	{
		IEnumerable<Control> enumerable = control.Controls.Cast<Control>();
		return from c in enumerable.SelectMany((Control ctrl) => GetControlsOfType<T>(ctrl)).Concat(enumerable)
			where c.GetType() == typeof(T)
			select c;
	}

	public static Control GetParentOfType<T>(Control control) where T : Control
	{
		Control parent = control.Parent;
		if (parent == null || parent.GetType() == typeof(T))
		{
			return parent;
		}
		return GetParentOfType<T>(parent);
	}

	public static List<Language> ReadLanguages(string path)
	{
		List<Language> result = null;
		try
		{
			string path2 = ".\\Languages\\" + path + ".json";
			string value = File.ReadAllText(path2);
			result = JsonConvert.DeserializeObject<List<Language>>(value);
		}
		catch
		{
		}
		return result;
	}

	public static List<ConfigDto> ReadConfigs()
	{
		List<ConfigDto> result = null;
		try
		{
			string path = ".\\Config.json";
			string value = File.ReadAllText(path);
			result = JsonConvert.DeserializeObject<List<ConfigDto>>(value);
		}
		catch
		{
		}
		return result;
	}

	public static BitmapImage ToBitmapImage(Bitmap bitmap)
	{
		try
		{
			using MemoryStream memoryStream = new MemoryStream();
			bitmap.Save(memoryStream, ImageFormat.Png);
			memoryStream.Position = 0L;
			BitmapImage bitmapImage = new BitmapImage();
			bitmapImage.BeginInit();
			bitmapImage.StreamSource = memoryStream;
			bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
			bitmapImage.EndInit();
			bitmapImage.Freeze();
			return bitmapImage;
		}
		catch
		{
			return new BitmapImage();
		}
	}

	public static string ConvertNumberToStringExcelColumnName(int columnNumber)
	{
		string text = "";
		while (columnNumber > 0)
		{
			int num = (columnNumber - 1) % 26;
			text = Convert.ToChar(65 + num) + text;
			columnNumber = (columnNumber - num) / 26;
		}
		return text;
	}

	public static MeasurementDto ReadExcelForMapper(string filename)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Expected O, but got Unknown
		//IL_0496: Unknown result type (might be due to invalid IL or missing references)
		//IL_049b: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dd: Expected O, but got Unknown
		//IL_0ee4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a04: Expected O, but got Unknown
		//IL_0f2b: Expected O, but got Unknown
		MeasurementDto val = new MeasurementDto
		{
			Measurements = new List<Measurement>()
		};
		try
		{
			FileInfo newFile = new FileInfo(filename);
			using (ExcelPackage excelPackage = new ExcelPackage(newFile))
			{
				if (excelPackage.Workbook.Worksheets.Count < 1)
				{
					throw new Exception(getTextLanguage("frmCreateFile", "IncorrectFormat"));
				}
				foreach (ExcelWorksheet worksheet in excelPackage.Workbook.Worksheets)
				{
					if (worksheet.Dimension == null)
					{
						throw new Exception(getTextLanguage("frmCreateFile", "SheetNull"));
					}
					List<ConfigDto> source = ReadConfigs();
					Dictionary<string, string> value = source.FirstOrDefault((ConfigDto x) => x.Table == "Type").Value;
					Dictionary<string, string> value2 = source.FirstOrDefault((ConfigDto x) => x.Table == "Title").Value;
					ExcelRange excelRange = worksheet.Cells[1, 1, worksheet.Dimension.End.Row, worksheet.Dimension.End.Column];
					List<string> list = new List<string>();
					List<CellAddress> list2 = new List<CellAddress>();
					string text = "PROCESS DATA INSPECTION";
					foreach (ExcelRangeBase item2 in excelRange)
					{
						if (item2.Value == null)
						{
							continue;
						}
						ExcelColor backgroundColor = item2.Style.Fill.BackgroundColor;
						if (backgroundColor != null && backgroundColor.Rgb != null)
						{
							if (backgroundColor.Rgb.Equals("FFFF0000"))
							{
								list.Add(item2.Address);
							}
							else
							{
								if (!backgroundColor.Rgb.Equals("FFFFC000"))
								{
									continue;
								}
								string value3 = item2.Value.ToString().Replace("\r", "").Replace("\n", "")
									.Replace(" ", "")
									.Replace("\u3000", "")
									.ToLower();
								KeyValuePair<string, string> check = value2.Where((KeyValuePair<string, string> x) => x.Key.ToLower().Equals(value3)).FirstOrDefault();
								if (check.Key == null)
								{
									if (list2.Any((CellAddress x) => x.Value == check.Value))
									{
										continue;
									}
									check = new KeyValuePair<string, string>(value3, value3);
								}
								CellAddress item = new CellAddress
								{
									Value = check.Value,
									IsSet = false,
									Sheet = worksheet.Name,
									Address = item2.Address
								};
								list2.Add(item);
							}
						}
						else
						{
							string value4 = item2.Value.ToString().Replace("\r", "").Replace("\n", "")
								.Replace(" ", "")
								.Replace("\u3000", "")
								.ToLower();
							KeyValuePair<string, string> keyValuePair = value.Where((KeyValuePair<string, string> x) => x.Key.ToLower().Equals(value4)).FirstOrDefault();
							if (keyValuePair.Key != null)
							{
								text = keyValuePair.Value;
							}
						}
					}
					if (list.Count == 0 || list2.Count == 0)
					{
						break;
					}
					foreach (string item3 in list)
					{
						string mergedAddress = GetMergedAddress(worksheet.Cells[item3]);
						int row = worksheet.Cells[mergedAddress].Start.Row;
						int row2 = worksheet.Cells[mergedAddress].End.Row;
						int column = worksheet.Cells[mergedAddress].Start.Column;
						int column2 = worksheet.Cells[mergedAddress].End.Column;
						string text2 = text;
						string text3 = text2;
						if (!(text3 == "INCOMING INSPECTION"))
						{
							if (text3 == "Hình vẽ:")
							{
								for (int num = row; num <= row2; num += 2)
								{
									Measurement val2 = new Measurement
									{
										Name = ((worksheet != null && worksheet.Cells[item3].Value != null) ? worksheet.Cells[item3].Value.ToString().Trim() : null)
									};
									foreach (CellAddress item4 in list2)
									{
										string mergedAddress2 = GetMergedAddress(worksheet.Cells[item4.Address]);
										int row3 = worksheet.Cells[mergedAddress2].Start.Row;
										int row4 = worksheet.Cells[mergedAddress2].End.Row;
										int column3 = worksheet.Cells[mergedAddress2].Start.Column;
										int column4 = worksheet.Cells[mergedAddress2].End.Column;
										switch (item4.Value)
										{
										case "[[NO]]":
											val2.Name = ((worksheet != null && worksheet.Cells[row, column3].Value != null) ? worksheet.Cells[row, column3].Value.ToString().Replace("\n", " ").Trim() : null);
											break;
										case "[[IMPORTANT]]":
											val2.Important = ((worksheet != null && worksheet.Cells[num, column3].Value != null) ? worksheet.Cells[num, column3].Value.ToString().Replace("\n", " ").Trim() : null);
											break;
										case "[[NAME]]":
											val2.Temp = ((worksheet != null && worksheet.Cells[num, column3].Value != null) ? worksheet.Cells[num, column3].Value.ToString().Split('\n')[0].Trim() : null);
											break;
										case "[[MACHINE]]":
											val2.MachineType = ((worksheet != null && worksheet.Cells[num, column3].Value != null) ? worksheet.Cells[num, column3].Value.ToString().Replace("\n", " ").Trim() : null);
											if (val2.MachineType == "↑")
											{
												Measurement obj = val.Measurements.LastOrDefault();
												val2.MachineType = ((obj != null) ? obj.MachineType : null);
											}
											break;
										case "[[VALUE]]":
										{
											val2.Value = ((worksheet != null && worksheet.Cells[num, column3].Value != null) ? worksheet.Cells[num, column3].Value.ToString().Replace("\n", " ").Trim() : null);
											string text4 = ((worksheet != null && worksheet.Cells[num, column4].Value != null) ? worksheet.Cells[num, column4].Value.ToString().Trim().Trim('\n') : null);
											if (!string.IsNullOrEmpty(text4))
											{
												string[] array = text4.Split('\n');
												if (array.Length > 1)
												{
													val2.USL = text4.Split('\n').FirstOrDefault();
													val2.LSL = text4.Split('\n').LastOrDefault();
												}
												else
												{
													val2.USL = ((worksheet != null && worksheet.Cells[num, column4].Value != null) ? worksheet.Cells[num, column4].Value.ToString().Replace("\n", " ").Trim() : null);
													val2.LSL = ((worksheet != null && worksheet.Cells[num + 1, column4].Value != null) ? worksheet.Cells[num + 1, column4].Value.ToString().Replace("\n", " ").Trim() : null);
												}
											}
											break;
										}
										}
									}
									if (!string.IsNullOrEmpty(val2.Name) && !string.IsNullOrEmpty(val2.Value))
									{
										val.Measurements.Add(val2);
									}
								}
								continue;
							}
							for (int num2 = row; num2 <= row2; num2++)
							{
								Measurement val3 = new Measurement
								{
									Name = ((worksheet != null && worksheet.Cells[item3].Value != null) ? worksheet.Cells[item3].Value.ToString().Trim() : null)
								};
								foreach (CellAddress item5 in list2)
								{
									string mergedAddress3 = GetMergedAddress(worksheet.Cells[item5.Address]);
									int row5 = worksheet.Cells[mergedAddress3].Start.Row;
									int row6 = worksheet.Cells[mergedAddress3].End.Row;
									int column5 = worksheet.Cells[mergedAddress3].Start.Column;
									int column6 = worksheet.Cells[mergedAddress3].End.Column;
									switch (item5.Value)
									{
									case "[[NO]]":
										val3.Name = ((worksheet != null && worksheet.Cells[row, column5].Value != null) ? worksheet.Cells[row, column5].Value.ToString().Replace("\n", " ").Trim() : null);
										break;
									case "[[IMPORTANT]]":
										val3.Important = ((worksheet != null && worksheet.Cells[row, column5].Value != null) ? worksheet.Cells[row, column5].Value.ToString().Replace("\n", " ").Trim() : null);
										break;
									case "[[NAME]]":
										val3.Temp = ((worksheet != null && worksheet.Cells[row, column5].Value != null) ? worksheet.Cells[row, column5].Value.ToString().Split('\n')[0].Trim() : null);
										break;
									case "[[MACHINE]]":
										val3.MachineType = ((worksheet != null && worksheet.Cells[num2, column5].Value != null) ? worksheet.Cells[num2, column5].Value.ToString().Replace("\n", " ").Trim() : null);
										if (val3.MachineType == "↑")
										{
											Measurement obj2 = val.Measurements.LastOrDefault();
											val3.MachineType = ((obj2 != null) ? obj2.MachineType : null);
										}
										if (string.IsNullOrEmpty(val3.MachineType) && num2 > row)
										{
											val3.MachineType = ((worksheet != null && worksheet.Cells[row, column5].Value != null) ? worksheet.Cells[row, column5].Value.ToString().Replace("\n", " ").Trim() : null);
										}
										break;
									case "[[VALUE]]":
									{
										val3.Value = ((worksheet != null && worksheet.Cells[row, column5].Value != null) ? worksheet.Cells[row, column5].Value.ToString().Replace("\n", " ").Trim() : null);
										string text5 = ((worksheet != null && worksheet.Cells[row, column6].Value != null) ? worksheet.Cells[row, column6].Value.ToString().Trim().Trim('\n') : null);
										if (!string.IsNullOrEmpty(text5))
										{
											string[] array2 = text5.Split('\n');
											if (array2.Length > 1)
											{
												val3.USL = text5.Split('\n').FirstOrDefault();
												val3.LSL = text5.Split('\n').LastOrDefault();
											}
											else if (num2 == row)
											{
												val3.USL = ((worksheet != null && worksheet.Cells[num2, column6].Value != null) ? worksheet.Cells[num2, column6].Value.ToString().Replace("\n", " ").Trim() : null);
												val3.LSL = ((worksheet != null && worksheet.Cells[num2 + 1, column6].Value != null) ? worksheet.Cells[num2 + 1, column6].Value.ToString().Replace("\n", " ").Trim() : null);
											}
											else
											{
												val3.USL = ((worksheet != null && worksheet.Cells[num2 - 1, column6].Value != null) ? worksheet.Cells[num2 - 1, column6].Value.ToString().Replace("\n", " ").Trim() : null);
												val3.LSL = ((worksheet != null && worksheet.Cells[num2, column6].Value != null) ? worksheet.Cells[num2, column6].Value.ToString().Replace("\n", " ").Trim() : null);
											}
										}
										break;
									}
									}
								}
								if (!string.IsNullOrEmpty(val3.Name) && !string.IsNullOrEmpty(val3.Value))
								{
									val.Measurements.Add(val3);
								}
							}
							continue;
						}
						for (int num3 = row; num3 <= row2; num3 += 2)
						{
							Measurement val4 = new Measurement
							{
								Name = ((worksheet != null && worksheet.Cells[item3].Value != null) ? worksheet.Cells[item3].Value.ToString().Trim() : null)
							};
							foreach (CellAddress item6 in list2)
							{
								string mergedAddress4 = GetMergedAddress(worksheet.Cells[item6.Address]);
								int row7 = worksheet.Cells[mergedAddress4].Start.Row;
								int row8 = worksheet.Cells[mergedAddress4].End.Row;
								int column7 = worksheet.Cells[mergedAddress4].Start.Column;
								int column8 = worksheet.Cells[mergedAddress4].End.Column;
								switch (item6.Value)
								{
								case "[[NO]]":
									val4.Name = ((worksheet != null && worksheet.Cells[row, column7].Value != null) ? worksheet.Cells[row, column7].Value.ToString().Replace("\n", " ").Trim() : null);
									break;
								case "[[IMPORTANT]]":
									val4.Important = ((worksheet != null && worksheet.Cells[num3, column7].Value != null) ? worksheet.Cells[num3, column7].Value.ToString().Replace("\n", " ").Trim() : null);
									break;
								case "[[NAME]]":
									val4.Temp = ((worksheet != null && worksheet.Cells[num3, column7].Value != null) ? worksheet.Cells[num3, column7].Value.ToString().Split('\n')[0].Trim() : null);
									break;
								case "[[MACHINE]]":
									val4.MachineType = ((worksheet != null && worksheet.Cells[num3, column7].Value != null) ? worksheet.Cells[num3, column7].Value.ToString().Replace("\n", " ").Trim() : null);
									if (val4.MachineType == "↑")
									{
										Measurement obj3 = val.Measurements.LastOrDefault();
										val4.MachineType = ((obj3 != null) ? obj3.MachineType : null);
									}
									break;
								case "[[VALUE]]":
								{
									val4.Value = ((worksheet != null && worksheet.Cells[num3, column7].Value != null) ? worksheet.Cells[num3, column7].Value.ToString().Replace("\n", " ").Trim() : null);
									string text6 = ((worksheet != null && worksheet.Cells[num3, column8].Value != null) ? worksheet.Cells[num3, column8].Value.ToString().Trim().Trim('\n') : null);
									if (!string.IsNullOrEmpty(text6))
									{
										string[] array3 = text6.Split('\n');
										if (array3.Length > 1)
										{
											val4.USL = text6.Split('\n').FirstOrDefault();
											val4.LSL = text6.Split('\n').LastOrDefault();
										}
										else
										{
											val4.USL = ((worksheet != null && worksheet.Cells[num3, column8].Value != null) ? worksheet.Cells[num3, column8].Value.ToString().Replace("\n", " ").Trim() : null);
											val4.LSL = ((worksheet != null && worksheet.Cells[num3 + 1, column8].Value != null) ? worksheet.Cells[num3 + 1, column8].Value.ToString().Replace("\n", " ").Trim() : null);
										}
									}
									break;
								}
								}
							}
							if (!string.IsNullOrEmpty(val4.Name) && !string.IsNullOrEmpty(val4.Value))
							{
								val.Measurements.Add(val4);
							}
						}
					}
				}
			}
			if (val.Measurements.Count.Equals(0))
			{
				val.Message = getTextLanguage("frmCreateFile", "IncorrectFormat");
			}
			IEnumerable<IGrouping<string, Measurement>> enumerable = from x in val.Measurements
				group x by x.Name;
			foreach (IGrouping<string, Measurement> item7 in enumerable)
			{
				if (item7.ToList().Count <= 1)
				{
					continue;
				}
				foreach (var item8 in item7.Select((Measurement value5, int index) => new
				{
					index = index,
					value = value5
				}))
				{
					item8.value.Name = $"{item8.value.Name}-{item8.index + 1}";
				}
			}
		}
		catch (Exception ex)
		{
			val.Message = ex.Message;
		}
		return val;
	}

	public static List<Measurement> CalMeasLs(List<Measurement> models)
	{
		foreach (Measurement model in models)
		{
			if (string.IsNullOrEmpty(model.Important))
			{
				model.Important = Settings.Default.Important;
			}
			model.Unit = ((model.Value.Contains("°") || model.Value.Contains("'")) ? "°" : "mm");
			if (model.Value.Contains("HRA") || model.Value.Contains("HrA"))
			{
				model.Unit = "HRA";
			}
			else if (model.Value.Contains("HRB") || model.Value.Contains("HrB"))
			{
				model.Unit = "HRB";
			}
			else if (model.Value.Contains("HRC") || model.Value.Contains("HrC"))
			{
				model.Unit = "HRC";
			}
			else if (model.Value.Contains("µM") || model.Value.Contains("µm") || model.Value.Contains("Rz"))
			{
				model.Unit = "µm";
			}
			else if (model.Value.ToUpper().Contains("KN"))
			{
				model.Unit = "KN";
			}
			else if (model.Value.Contains("N.m"))
			{
				model.Unit = "N.m";
			}
			else if (model.Value.Contains("N"))
			{
				model.Unit = "N";
			}
			model.Name = model.Name + "-" + model.Temp;
			if (model.Value.Contains("±"))
			{
				MatchCollection matchCollection = Regex.Matches(model.Value, "-?\\d+(\\.\\d+)?");
				string value = "0";
				string value2 = matchCollection[0].Value;
				if (matchCollection.Count > 1)
				{
					value = matchCollection[0].Value;
					value2 = matchCollection[1].Value;
				}
				double result;
				if (model.Value.Contains("°") || model.Value.Contains("'"))
				{
					double? num = ConvertDegreesToDouble(model.Value.Split('±').LastOrDefault()?.Trim());
					model.Value = value;
					model.Upper = num;
					model.Lower = 0.0 - num;
				}
				else if (double.TryParse(value2, out result))
				{
					model.Value = value;
					model.Upper = result;
					model.Lower = 0.0 - result;
				}
			}
			else if (model.Value.Contains("~"))
			{
				MatchCollection matchCollection2 = Regex.Matches(model.Value, "-?\\d+(\\.\\d+)?");
				if (matchCollection2.Count > 1 && double.TryParse(matchCollection2[0].Value, out var result2) && double.TryParse(matchCollection2[1].Value, out var result3))
				{
					model.Value = ((result2 + result3) / 2.0).ToString("0.####");
					double num2 = Math.Round((result3 - result2) / 2.0, 4);
					model.Upper = num2;
					model.Lower = 0.0 - num2;
				}
			}
			else if (model.Value.Contains("-"))
			{
				MatchCollection matchCollection3 = Regex.Matches(model.Value, "\\d+(\\.\\d+)?");
				if (matchCollection3.Count > 1 && double.TryParse(matchCollection3[0].Value, out var result4) && double.TryParse(matchCollection3[1].Value, out var result5))
				{
					model.Value = ((result4 + result5) / 2.0).ToString("0.####");
					double num3 = Math.Round((result5 - result4) / 2.0, 4);
					model.Upper = num3;
					model.Lower = 0.0 - num3;
				}
			}
			else if (model.Value.ToUpper().Contains("MAX"))
			{
				string value3 = Regex.Match(model.Value, "-?\\d+(\\.\\d+)?").Value;
				if (double.TryParse(value3, out var result6))
				{
					model.Value = "0";
					model.Upper = result6;
					model.Lower = 0.0;
				}
			}
			else if (model.Value.ToUpper().Contains("MIN"))
			{
				string value4 = Regex.Match(model.Value, "-?\\d+(\\.\\d+)?").Value;
				if (double.TryParse(value4, out var result7))
				{
					model.Value = result7.ToString();
					model.Upper = 9999.0;
					model.Lower = 0.0;
				}
			}
			else
			{
				string value5 = Regex.Match(model.Value, "-?\\d+(\\.\\d+)?").Value;
				double result8;
				if (model.Value.Contains("°") || model.Value.Contains("'"))
				{
					double? num4 = ConvertDegreesToDouble(model.Value);
					double? num5 = ConvertDegreesToDouble(model.USL);
					double? num6 = ConvertDegreesToDouble(model.LSL);
					if (num5 != 0.0 || num6 != 0.0)
					{
						model.Value = num4.ToString();
						model.Upper = num5;
						model.Lower = num6;
					}
				}
				else if (double.TryParse(value5, out result8))
				{
					if (double.TryParse(model.USL, out var result9) && double.TryParse(model.LSL, out var result10))
					{
						model.Value = result8.ToString();
						model.Upper = result9;
						model.Lower = result10;
					}
					else if (model.Value.Contains("Rz") || model.Value.Contains("Ra"))
					{
						model.Value = "0";
						model.Upper = result8;
						model.Lower = 0.0;
						model.Unit = "µm";
					}
				}
			}
			if (!model.Upper.HasValue || !model.Lower.HasValue)
			{
				model.Upper = null;
				model.Lower = null;
				model.Important = null;
				model.Unit = null;
			}
		}
		IEnumerable<IGrouping<string, Measurement>> enumerable = from x in models
			group x by x.Name;
		foreach (IGrouping<string, Measurement> item in enumerable)
		{
			if (item.ToList().Count <= 1)
			{
				continue;
			}
			foreach (var item2 in item.Select((Measurement value6, int index) => new
			{
				index = index,
				value = value6
			}))
			{
				item2.value.Name = $"{item2.value.Name}-{item2.index + 1}";
			}
		}
		return models;
	}

	public static CellAddressDto ReadExcelForDetail(string filename)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Expected O, but got Unknown
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_0288: Expected O, but got Unknown
		//IL_0358: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0370: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_0388: Expected O, but got Unknown
		List<ConfigDto> list = ReadConfigs();
		CellAddressDto val = new CellAddressDto
		{
			CellAddresses = new List<CellAddress>()
		};
		MetaType.CAVITY = 1;
		try
		{
			FileInfo newFile = new FileInfo(filename);
			using (ExcelPackage excelPackage = new ExcelPackage(newFile))
			{
				if (excelPackage.Workbook.Worksheets.Count < 1)
				{
					throw new Exception(getTextLanguage("frmCreateFile", "IncorrectFormat"));
				}
				foreach (ExcelWorksheet worksheet in excelPackage.Workbook.Worksheets)
				{
					if (worksheet.Dimension == null)
					{
						throw new Exception(getTextLanguage("frmCreateFile", "SheetNull"));
					}
					ExcelRange excelRange = worksheet.Cells[1, 1, worksheet.Dimension.End.Row, worksheet.Dimension.End.Column];
					foreach (ExcelRangeBase item in excelRange)
					{
						if (item.Value == null)
						{
							continue;
						}
						string value = item.Value.ToString().Replace("\r", "").Replace("\n", "")
							.Replace(" ", "")
							.Replace("\u3000", "")
							.ToLower();
						ExcelColor backgroundColor = item.Style.Fill.BackgroundColor;
						if (backgroundColor != null && backgroundColor.Rgb != null)
						{
							int result;
							if (backgroundColor.Rgb.Equals("FFFF0000"))
							{
								CellAddress val2 = new CellAddress
								{
									Address = item.Address,
									IsSet = true,
									Value = item.Value?.ToString().Trim(),
									Sheet = worksheet.Name
								};
								if (string.IsNullOrEmpty(val2.Value))
								{
									continue;
								}
								val.CellAddresses.Add(val2);
							}
							else if (backgroundColor.Rgb.Equals("FFFFC000") && int.TryParse(value, out result))
							{
								CellAddress val3 = new CellAddress
								{
									Address = item.Address,
									IsSet = false,
									Value = $"[[SP#{result}]]",
									Sheet = worksheet.Name
								};
								if (string.IsNullOrEmpty(val3.Value))
								{
									continue;
								}
								val.CellAddresses.Add(val3);
							}
						}
						foreach (ConfigDto item2 in list)
						{
							KeyValuePair<string, string> check = item2.Value.Where((KeyValuePair<string, string> x) => x.Key.ToLower().Equals(value)).FirstOrDefault();
							if (check.Key != null && !val.CellAddresses.Any((CellAddress x) => x.Value == check.Value))
							{
								string mergedAddress = GetMergedAddress(worksheet.Cells[item.Address]);
								CellAddress val4 = new CellAddress
								{
									Value = check.Value,
									IsSet = false,
									Sheet = worksheet.Name
								};
								switch (item2.Table)
								{
								case "InforVertical":
								{
									int row = worksheet.Cells[mergedAddress].Start.Row;
									int column2 = worksheet.Cells[mergedAddress].End.Column;
									val4.Address = worksheet.Cells[row, column2 + 1].Address;
									break;
								}
								case "InforVerticalRevert":
								{
									int row = worksheet.Cells[mergedAddress].Start.Row;
									int column = worksheet.Cells[mergedAddress].Start.Column;
									val4.Address = worksheet.Cells[row, column - 1].Address;
									break;
								}
								case "InforHorizontal":
								{
									int row2 = worksheet.Cells[mergedAddress].End.Row;
									int column = worksheet.Cells[mergedAddress].Start.Column;
									val4.Address = worksheet.Cells[row2 + 1, column].Address;
									break;
								}
								case "InforHorizontalRevert":
								{
									int row = worksheet.Cells[mergedAddress].Start.Row;
									int column = worksheet.Cells[mergedAddress].Start.Column;
									val4.Address = worksheet.Cells[row - 1, column].Address;
									break;
								}
								case "InforCurrent":
								{
									int row = worksheet.Cells[mergedAddress].Start.Row;
									int column = worksheet.Cells[mergedAddress].Start.Column;
									val4.Address = worksheet.Cells[row, column].Address;
									break;
								}
								case "Type":
								{
									int row = worksheet.Cells[mergedAddress].Start.Row;
									int column = worksheet.Cells[mergedAddress].Start.Column;
									val4.Address = worksheet.Cells[row, column].Address;
									break;
								}
								}
								if (!string.IsNullOrEmpty(val4.Address))
								{
									val.CellAddresses.Add(val4);
								}
							}
						}
					}
				}
			}
			if (val.CellAddresses.Count.Equals(0))
			{
				val.Message = getTextLanguage("frmCreateFile", "IncorrectFormat");
			}
		}
		catch (Exception ex)
		{
			val.Message = ex.Message;
		}
		return val;
	}

	public static string SaveFileForDetail(List<CellAddress> models, string sourceFile, int offset, bool isOpen = true)
	{
		string text = Path.Combine("C:\\Windows\\Temp\\5SQA_System", "Views");
		Directory.CreateDirectory(text);
		string text2 = Path.Combine(text, Path.GetFileNameWithoutExtension(sourceFile) + "_DETAIL" + Path.GetExtension(sourceFile));
		Cursor.Current = Cursors.WaitCursor;
		try
		{
			MetaType.MERGE = 4;
			if (FileInUse(text2))
			{
				throw new Exception(getTextLanguage("frmCreateFile", "Openning"));
			}
			if (!isOpen)
			{
				File.Delete(text2);
			}
			File.Copy(sourceFile, text2, overwrite: true);
			FileInfo newFile = new FileInfo(text2);
			using ExcelPackage excelPackage = new ExcelPackage(newFile);
			if (excelPackage.Workbook.Worksheets.Count < 1)
			{
				throw new Exception(getTextLanguage("frmCreateFile", "IncorrectFormat"));
			}
			int num = 0;
			foreach (ExcelWorksheet ws in excelPackage.Workbook.Worksheets)
			{
				if (ws.Dimension == null)
				{
					throw new Exception(getTextLanguage("frmCreateFile", "SheetNull"));
				}
				IEnumerable<CellAddress> enumerable = models.Where((CellAddress x) => x.Sheet == ws.Name && x.Value.StartsWith("[[SP#"));
				CellAddress val = models.FirstOrDefault((CellAddress x) => x.Sheet == ws.Name && x.Value == "[[JUDGE]]");
				if (val != null)
				{
					models.Remove(val);
				}
				CellAddress val2 = models.FirstOrDefault((CellAddress x) => x.Sheet == ws.Name && x.Value == "[[JUDGESAMPLE]]");
				if (val2 != null)
				{
					models.Remove(val2);
				}
				CellAddress obj = models.FirstOrDefault((CellAddress x) => x.Sheet == ws.Name && x.Value == "PROCESS DATA INSPECTION");
				string value = ((obj != null) ? obj.Value : null);
				int num2 = ((!string.IsNullOrEmpty(value)) ? 1 : 2);
				foreach (CellAddress item in models.Where((CellAddress x) => x.Sheet == ws.Name))
				{
					if (item.IsSet)
					{
						ws.Cells[item.Address].Style.Fill.BackgroundColor.SetColor(Color.White);
						string mergedAddress = GetMergedAddress(ws.Cells[item.Address]);
						int row = ws.Cells[mergedAddress].Start.Row;
						int row2 = ws.Cells[mergedAddress].End.Row;
						int column = ws.Cells[mergedAddress].Start.Column;
						int column2 = ws.Cells[mergedAddress].End.Column;
						for (int num3 = row; num3 <= row2; num3 += num2)
						{
							num++;
							foreach (CellAddress item2 in enumerable)
							{
								string mergedAddress2 = GetMergedAddress(ws.Cells[item2.Address]);
								int row3 = ws.Cells[mergedAddress2].Start.Row;
								int row4 = ws.Cells[mergedAddress2].End.Row;
								int column3 = ws.Cells[mergedAddress2].Start.Column;
								int column4 = ws.Cells[mergedAddress2].End.Column;
								ws.Cells[row3 - 1, column, row4, column4].Style.Fill.BackgroundColor.SetColor(Color.White);
								ws.Cells[num3, column3].Value = string.Format("[[MEAS-{0}#1#{1}]]", num + offset, Regex.Match(item2.Value, "-?\\d+(\\.\\d+)?").Value);
								if (val2 != null)
								{
									ws.Cells[ws.Cells[val2.Address].Start.Row, column3].Value = "[[JUD#" + Regex.Match(item2.Value, "-?\\d+(\\.\\d+)?").Value + "]]";
								}
							}
							if (val != null)
							{
								ExcelColor backgroundColor = ws.Cells[val.Address].Style.Fill.BackgroundColor;
								if (backgroundColor != null && backgroundColor.Rgb != null)
								{
									ws.Cells[val.Address].Style.Fill.BackgroundColor.SetColor(Color.White);
								}
								ws.Cells[num3, ws.Cells[val.Address].Start.Column].Value = $"[[MEAS-{num + offset}#1#JUD]]";
							}
						}
					}
					else
					{
						ws.Cells[item.Address].Value = item.Value;
					}
				}
			}
			excelPackage.Save();
		}
		catch (Exception ex)
		{
			text2 = "ERROR: " + ex.Message;
		}
		return text2;
	}

	public static RequestViewModel GetRequest(List<CellAddress> models, string filename)
	{
		RequestViewModel requestViewModel = new RequestViewModel
		{
			ProductCavity = MetaType.CAVITY,
			Sample = 1
		};
		PropertyInfo[] properties = requestViewModel.GetType().GetProperties();
		try
		{
			FileInfo newFile = new FileInfo(filename);
			using ExcelPackage excelPackage = new ExcelPackage(newFile);
			foreach (CellAddress model in models)
			{
				if (model.Value.StartsWith("[[SP#"))
				{
					int.TryParse(Regex.Match(model.Value, "-?\\d+(\\.\\d+)?").Value, out var result);
					if (result > requestViewModel.Sample)
					{
						requestViewModel.Sample = result;
					}
					continue;
				}
				string name = model.Value.Replace("[[", "").Replace("]]", "");
				PropertyInfo propertyInfo = properties.FirstOrDefault((PropertyInfo x) => x.Name.ToUpper() == name);
				if (propertyInfo == null)
				{
					continue;
				}
				object value = excelPackage.Workbook.Worksheets[model.Sheet].Cells[model.Address].Value;
				if (value != null && !string.IsNullOrEmpty(value.ToString()))
				{
					value = value.ToString().Trim();
					int result3;
					int result4;
					if (MetaType.IsDate.Contains(propertyInfo.Name) && DateTime.TryParse(value.ToString(), out var result2))
					{
						value = (DateTimeOffset)DateTime.SpecifyKind(result2, DateTimeKind.Local);
					}
					else if (propertyInfo.Name == "Quantity" && int.TryParse(Regex.Match(value.ToString(), "\\d*").Value, out result3))
					{
						value = result3;
					}
					else if (int.TryParse(Regex.Match(value.ToString(), "\\d*").Value, out result4))
					{
						value = value.ToString();
					}
					propertyInfo.SetValue(requestViewModel, value, null);
				}
			}
		}
		catch
		{
		}
		if (string.IsNullOrEmpty(requestViewModel.ProductStage))
		{
			requestViewModel.ProductStage = "Kiểm tra đầu vào";
		}
		return requestViewModel;
	}

	public static List<RequestResultViewModel> GetResults(DataRow product, string filename)
	{
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Expected O, but got Unknown
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_0323: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Expected O, but got Unknown
		//IL_032d: Expected O, but got Unknown
		List<RequestResultViewModel> list = new List<RequestResultViewModel>();
		try
		{
			if (string.IsNullOrEmpty(product["TemplateId"].ToString()))
			{
				throw new Exception();
			}
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "Id=@0";
			queryArgs.PredicateParameters = new string[1] { product["TemplateId"].ToString() };
			queryArgs.Order = "Created DESC";
			queryArgs.Page = 1;
			queryArgs.Limit = 1;
			QueryArgs body = queryArgs;
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/Template/Gets").Result;
			TemplateViewModel val = getObjectToList<TemplateViewModel>(result).FirstOrDefault();
			if (string.IsNullOrEmpty(val.TemplateData))
			{
				throw new Exception();
			}
			List<ExportMappingDto> list2 = JsonConvert.DeserializeObject<List<ExportMappingDto>>(val.TemplateData);
			list2.RemoveAll((ExportMappingDto x) => !x.Value.StartsWith("[[MEAS-"));
			queryArgs = new QueryArgs();
			queryArgs.Predicate = "ProductId=@0";
			queryArgs.PredicateParameters = new string[1] { product["Id"].ToString() };
			queryArgs.Order = "Sort, Created";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			body = queryArgs;
			result = frmLogin.client.GetsAsync(body, "/api/Measurement/Gets").Result;
			List<MeasurementQuickViewModel> objectToList = getObjectToList<MeasurementQuickViewModel>(result);
			FileInfo newFile = new FileInfo(filename);
			using ExcelPackage excelPackage = new ExcelPackage(newFile);
			IEnumerable<IGrouping<string, ExportMappingDto>> enumerable = from x in list2
				group x by x.Sheet;
			foreach (IGrouping<string, ExportMappingDto> item in enumerable)
			{
				ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets[item.Key] ?? excelPackage.Workbook.Worksheets[0];
				foreach (ExportMappingDto item2 in item)
				{
					string[] values = item2.Value.Replace("[[", "").Replace("]]", "").Split('#');
					RequestResultViewModel val2 = new RequestResultViewModel();
					((AuditableEntity)val2).Id = Guid.Empty;
					val2.MeasurementId = objectToList.FirstOrDefault((MeasurementQuickViewModel x) => x.Code == values.First()).Id;
					val2.MachineName = "Manual Input";
					val2.StaffName = frmLogin.User.FullName;
					val2.Sample = int.Parse(values.Last());
					val2.Cavity = int.Parse(values[1]);
					val2.Result = ((excelWorksheet == null || excelWorksheet.Cells[item2.CellAddress].Value == null) ? null : excelWorksheet.Cells[item2.CellAddress].Value.ToString());
					((AuditableEntity)val2).IsActivated = true;
					RequestResultViewModel val3 = val2;
					if (!string.IsNullOrEmpty(val3.Result))
					{
						list.Add(val3);
					}
				}
			}
		}
		catch
		{
		}
		return list;
	}

	private static string GetMergedAddress(ExcelRange range)
	{
		if (range.Merge)
		{
			int mergeCellId = range.Worksheet.GetMergeCellId(range.Start.Row, range.Start.Column);
			return range.Worksheet.MergedCells[mergeCellId - 1];
		}
		return range.Address;
	}

	public static bool ByteArrayToFile(string fileName, byte[] byteArray)
	{
		try
		{
			using (FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
			{
				fileStream.Write(byteArray, 0, byteArray.Length);
			}
			return true;
		}
		catch
		{
			return false;
		}
	}

	public static string ToString(double value, string filter, string[] decimals)
	{
		string empty = string.Empty;
		string text = decimals.FirstOrDefault((string x) => x.Split('#').Contains(filter));
		int result = 4;
		if (text != null)
		{
			string[] array = text.Split('#');
			if (array.Length > 1)
			{
				int.TryParse(array[1], out result);
			}
		}
		return result switch
		{
			0 => value.ToString("0"), 
			1 => value.ToString("0.0"), 
			2 => value.ToString("0.00"), 
			3 => value.ToString("0.000"), 
			5 => value.ToString("0.00000"), 
			6 => value.ToString("0.000000"), 
			7 => value.ToString("0.0000000"), 
			8 => value.ToString("0.00000000"), 
			9 => value.ToString("0.000000000"), 
			_ => value.ToString("0.0000"), 
		};
	}

	public static string ToString(double value, string filter, List<string> decimals)
	{
		string empty = string.Empty;
		string text = decimals.FirstOrDefault((string x) => x.Split('#').Contains(filter));
		int result = 4;
		if (text != null)
		{
			string[] array = text.Split('#');
			if (array.Length > 1)
			{
				int.TryParse(array[1], out result);
			}
		}
		return result switch
		{
			0 => value.ToString("0"), 
			1 => value.ToString("0.0"), 
			2 => value.ToString("0.00"), 
			3 => value.ToString("0.000"), 
			5 => value.ToString("0.00000"), 
			6 => value.ToString("0.000000"), 
			7 => value.ToString("0.0000000"), 
			8 => value.ToString("0.00000000"), 
			9 => value.ToString("0.000000000"), 
			_ => value.ToString("0.0000"), 
		};
	}

	private static double ConvertAngleToDouble(string valuestr)
	{
		double result = 0.0;
		double result2 = 0.0;
		double result3 = 0.0;
		int num = valuestr.IndexOf('°');
		if (num != -1)
		{
			string s = valuestr.Substring(0, num);
			double.TryParse(s, out result);
		}
		int num2 = valuestr.IndexOf('\'');
		if (num2 != -1)
		{
			string s2 = valuestr.Substring(num + 1, num2 - num - 1);
			double.TryParse(s2, out result2);
		}
		else
		{
			num2 = num;
		}
		int num3 = valuestr.IndexOf('"');
		if (num3 != -1)
		{
			string s3 = valuestr.Substring(num2 + 1, num3 - num2 - 1);
			double.TryParse(s3, out result3);
		}
		return Math.Round(result + result2 / 60.0 + result3 / 3600.0, 4);
	}

	public static string ConvertDoubleToDegrees(double value, bool uniformity = false)
	{
		int num = (int)value;
		double num2 = Math.Abs(value - (double)num) * 60.0;
		int num3 = (int)num2;
		double a = (num2 - (double)num3) * 60.0;
		int num4 = (int)Math.Round(a);
		if (num4 == 60)
		{
			num4 = 0;
			num3++;
		}
		if (num3 == 60)
		{
			num3 = 0;
			num++;
		}
		if (uniformity)
		{
			return string.Format("{0}{1}{2}{3}{4}", (num == 0 && value < 0.0) ? "-" : "", num, "°", (num3 == 0) ? "00" : $"{num3:00}", "'") + ((num4 == 0) ? "00" : $"{num4:00}") + "\"";
		}
		return string.Format("{0}{1}{2}{3}{4}", (num == 0 && value < 0.0) ? "-" : "", num, "°", (num3 == 0) ? "" : string.Format("{0}{1}", num3, "'"), (num4 == 0) ? "" : string.Format("{0}{1}", num4, "\""));
	}

	public static double? ConvertDegreesToDouble(string valuestr)
	{
		try
		{
			bool flag = valuestr.Contains("-");
			valuestr = valuestr.Trim('-');
			double result = 0.0;
			double result2 = 0.0;
			double result3 = 0.0;
			int num = valuestr.IndexOf('°');
			if (num != -1)
			{
				string s = valuestr.Substring(0, num);
				double.TryParse(s, out result);
			}
			int num2 = valuestr.IndexOf('\'');
			if (num2 != -1)
			{
				string s2 = valuestr.Substring(num + 1, num2 - num - 1);
				double.TryParse(s2, out result2);
			}
			else
			{
				num2 = num;
			}
			int num3 = valuestr.IndexOf('"');
			if (num3 != -1)
			{
				string s3 = valuestr.Substring(num2 + 1, num3 - num2 - 1);
				double.TryParse(s3, out result3);
			}
			if (num == -1 && num2 == -1 && num3 == -1)
			{
				throw new Exception();
			}
			if (flag)
			{
				return 0.0 - Math.Round(result + result2 / 60.0 + result3 / 3600.0, 4);
			}
			return Math.Round(result + result2 / 60.0 + result3 / 3600.0, 4);
		}
		catch
		{
			return null;
		}
	}

	public static List<string> ConvertDegreesToList(string valuestr)
	{
		try
		{
			List<string> list = new List<string>();
			if (valuestr.Contains("-"))
			{
				list.Add("-");
			}
			else
			{
				list.Add("+");
			}
			bool flag = valuestr.Contains("-");
			valuestr = valuestr.Trim('-');
			int num = valuestr.IndexOf('°');
			if (num != -1)
			{
				list.Add(valuestr.Substring(0, num));
			}
			else
			{
				list.Add("");
				num = 0;
			}
			int num2 = valuestr.IndexOf('\'');
			if (num2 != -1)
			{
				list.Add(valuestr.Substring(num + 1, num2 - num - 1));
			}
			else
			{
				list.Add("");
				num2 = num;
			}
			int num3 = valuestr.IndexOf('"');
			if (num3 != -1)
			{
				list.Add(valuestr.Substring(num2 + 1, num3 - num2 - 1));
			}
			else
			{
				list.Add("");
			}
			return list;
		}
		catch
		{
			return null;
		}
	}
}
