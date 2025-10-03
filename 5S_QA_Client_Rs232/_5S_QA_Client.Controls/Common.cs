using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using _5S_QA_Client.Properties;
using _5S_QA_DL.APIContext;
using _5S_QA_Entities.Dtos;
using MetroFramework.Controls;
using Newtonsoft.Json;
using OfficeOpenXml;

namespace _5S_QA_Client.Controls;

public static class Common
{
	public static List<T> getObjects<T>(object obj)
	{
		List<T> result = new List<T>();
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
		return result;
	}

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

	public static DataTable getDataTableIsSelect<T>(object obj)
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
							array2[j] = false;
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
				array = ((!title.ToLower().Equals("limit") && !title.ToLower().Equals("sample") && !title.ToLower().Equals("quantity") && !title.ToLower().Equals("checkquantity")) ? (from x in dt.AsEnumerable()
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
			Process process = new Process();
			process.StartInfo.FileName = filename;
			process.StartInfo.Arguments = $"10";
			process.StartInfo.CreateNoWindow = false;
			process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			process.Start();
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

	public static List<MapperMeasDto> getMappers(Guid? id)
	{
		string text = Path.Combine("C:\\Windows\\Temp", "5S_QA", "Mapper");
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		string path = Path.Combine(text, $"{id}.json");
		string value = "[]";
		if (File.Exists(path))
		{
			using FileStream stream = File.OpenRead(path);
			using StreamReader streamReader = new StreamReader(stream, Encoding.UTF8);
			value = streamReader.ReadToEnd();
		}
		return JsonConvert.DeserializeObject<List<MapperMeasDto>>(value);
	}

	public static bool addMappers(Guid productid, DataGridView dgvmain, DataGridView dgvcontent)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Expected O, but got Unknown
		try
		{
			List<MapperMeasDto> list = new List<MapperMeasDto>();
			foreach (DataGridViewRow item in (IEnumerable)dgvmain.Rows)
			{
				list.Add(new MapperMeasDto
				{
					MeasurementId = Guid.Parse(item.Cells["Id"].Value.ToString()),
					Mapper = item.Cells["Mapper"].Value.ToString()
				});
			}
			if (dgvcontent != null)
			{
				string mappername = ((dgvcontent.Rows[2].Cells["ContentValue"].Value == null) ? string.Empty : dgvcontent.Rows[2].Cells["ContentValue"].Value.ToString());
				MapperMeasDto val = list.Where((MapperMeasDto x) => x.Mapper.Equals(mappername) && !x.MeasurementId.Equals(Guid.Parse(dgvmain.CurrentRow.Cells["Id"].Value.ToString()))).FirstOrDefault();
				if (val != null && !string.IsNullOrEmpty(mappername))
				{
					MessageBox.Show("Mapper name: [" + mappername + "] already exit.", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return false;
				}
				val = list.Where((MapperMeasDto x) => x.MeasurementId.Equals(Guid.Parse(dgvmain.CurrentRow.Cells["Id"].Value.ToString()))).FirstOrDefault();
				val.Mapper = mappername;
			}
			else
			{
				foreach (DataGridViewRow row in (IEnumerable)dgvmain.Rows)
				{
					string mappername2 = ((row.Cells["Mapper"].Value == null) ? string.Empty : row.Cells["Mapper"].Value.ToString());
					MapperMeasDto val2 = list.Where((MapperMeasDto x) => x.Mapper.Equals(mappername2) && !x.MeasurementId.Equals(Guid.Parse(row.Cells["Id"].Value.ToString()))).FirstOrDefault();
					if (val2 != null && !string.IsNullOrEmpty(mappername2))
					{
						MessageBox.Show(string.Format("Mapper name: [{0}] at row [{1}] already exit.", mappername2, row.Cells["no"].Value), "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return false;
					}
				}
			}
			string text = Path.Combine("C:\\Windows\\Temp", "5S_QA", "Mapper");
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			string path = Path.Combine(text, $"{productid}.json");
			string value = JsonConvert.SerializeObject(list);
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			using (FileStream stream = File.Create(path))
			{
				using StreamWriter streamWriter = new StreamWriter(stream, Encoding.UTF8);
				streamWriter.WriteLine(value);
			}
			return true;
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error: " + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			return false;
		}
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

	public static void setControls(Control control, ToolTip tooltip = null, List<ContextMenuStrip> contexts = null)
	{
		List<LanguageDto> list = ReadLanguages(control.GetType().Name);
		if (list == null)
		{
			return;
		}
		IEnumerable<Control> all = GetAll(control);
		string name = Settings.Default.Language.Replace("rb", "Name");
		string name2 = Settings.Default.Language.Replace("rb", "Tooltip");
		LanguageDto val = list.FirstOrDefault((LanguageDto x) => control.Name.Equals(x.Code));
		if (val != null)
		{
			object value = ((object)val).GetType().GetProperty(name).GetValue(val, null);
			if (value != null)
			{
				control.Text = $"5S QA Client * {value}";
			}
		}
		foreach (Control ctrl in all)
		{
			val = list.FirstOrDefault((LanguageDto x) => ctrl.Name.Replace("lbl", "txt").Equals(x.Code));
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
				val = list.FirstOrDefault((LanguageDto x) => col.Name.Equals(x.Code));
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
				val = list.FirstOrDefault((LanguageDto x) => _menu.Name.StartsWith(x.Code));
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
		List<LanguageDto> list = ReadLanguages(control.GetType().Name);
		if (list != null)
		{
			string name = Settings.Default.Language.Replace("rb", "Name");
			LanguageDto val = list.FirstOrDefault((LanguageDto x) => code.Equals(x.Code));
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
		List<LanguageDto> list = ReadLanguages(control);
		if (list != null)
		{
			string name = Settings.Default.Language.Replace("rb", "Name");
			LanguageDto val = list.FirstOrDefault((LanguageDto x) => code.Equals(x.Code));
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
			typeof(MetroTextBox.MetroTextButton),
			typeof(MetroTextBox),
			typeof(DateTimePicker),
			typeof(ComboBox),
			typeof(DataGridView),
			typeof(TextBox),
			typeof(CheckBox),
			typeof(PictureBox)
		};
		IEnumerable<Control> enumerable = from Control x in control.Controls
			where !x.Name.StartsWith("m")
			select x;
		return from c in enumerable.SelectMany((Control ctrl) => GetAll(ctrl)).Concat(enumerable)
			where types.Contains(c.GetType())
			select c;
	}

	private static List<LanguageDto> ReadLanguages(string path)
	{
		List<LanguageDto> result = null;
		try
		{
			string path2 = ".\\Languages\\" + path + ".json";
			string value = File.ReadAllText(path2);
			result = JsonConvert.DeserializeObject<List<LanguageDto>>(value);
		}
		catch
		{
		}
		return result;
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

	public static string getRequestType()
	{
		try
		{
			MatchCollection matchCollection = Regex.Matches(frmLogin.mTimeSample, "\\d{1,2}:\\d{2}");
			if (matchCollection.Count < 2)
			{
				throw new Exception();
			}
			string[] source = matchCollection[0].Value.Split(':');
			string[] source2 = matchCollection[1].Value.Split(':');
			int.TryParse(source.First(), out var result);
			int.TryParse(source.Last(), out var result2);
			TimeSpan timeSpan = new TimeSpan(result, result2, 0);
			int.TryParse(source2.First(), out result);
			int.TryParse(source2.Last(), out result2);
			TimeSpan timeSpan2 = new TimeSpan(result, result2, 0);
			TimeSpan timeSpan3 = timeSpan2 - timeSpan;
			TimeSpan timeOfDay = DateTime.Now.TimeOfDay;
			string text = ((source.First().Length < 2) ? "0" : "00");
			string text2 = ((source.Last().Length < 2) ? "0" : "00");
			string text3 = frmLogin.mTimeSample.Replace(matchCollection[0].Value, "").Replace(matchCollection[1].Value, "");
			while (timeOfDay > timeSpan2)
			{
				timeSpan = timeSpan2;
				timeSpan2 = timeSpan + timeSpan3;
			}
			return timeSpan.Hours.ToString(text) + ":" + timeSpan.Minutes.ToString(text2) + text3 + timeSpan2.Hours.ToString(text) + ":" + timeSpan2.Minutes.ToString(text2);
		}
		catch
		{
			return null;
		}
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
			else
			{
				num = 0;
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
			if (num == 0 && num2 == 0 && num3 == -1)
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
