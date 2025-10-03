using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using _5SQA_Config_UWave.Constants;
using _5SQA_Config_UWave.ViewModels;
using Newtonsoft.Json;

namespace _5SQA_Config_UWave.Funtions;

public class Common
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

	public static void WriteToFileConfig(string filename, Config config)
	{
		string text = Path.Combine(Constant.PATHCONFIG);
		Directory.CreateDirectory(text);
		string path = Path.Combine(text, filename);
		if (File.Exists(path))
		{
			File.Delete(path);
		}
		string plainText = JsonConvert.SerializeObject(config);
		string value = Encrypt(Base64Encode(plainText));
		using FileStream stream = File.Create(path);
		using StreamWriter streamWriter = new StreamWriter(stream, Encoding.UTF8);
		streamWriter.WriteLine(value);
	}

	public static Config ReadFromFileConfig(string filename)
	{
		string path = Path.Combine(Constant.PATHCONFIG, filename);
		string value = "{}";
		if (File.Exists(path))
		{
			using FileStream stream = File.OpenRead(path);
			using StreamReader streamReader = new StreamReader(stream, Encoding.UTF8);
			string toDecrypt = streamReader.ReadToEnd();
			value = Base64Decode(Decrypt(toDecrypt));
		}
		return JsonConvert.DeserializeObject<Config>(value);
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

	private static string Base64Encode(string plainText)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(plainText);
		return Convert.ToBase64String(bytes);
	}

	private static string Base64Decode(string base64EncodedData)
	{
		byte[] bytes = Convert.FromBase64String(base64EncodedData);
		return Encoding.UTF8.GetString(bytes);
	}

	private static string Decrypt(string toDecrypt)
	{
		if (string.IsNullOrEmpty(toDecrypt))
		{
			return toDecrypt;
		}
		bool flag = true;
		byte[] array = Convert.FromBase64String(toDecrypt);
		byte[] key;
		if (flag)
		{
			MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
			key = mD5CryptoServiceProvider.ComputeHash(Encoding.UTF8.GetBytes("A&A Technology"));
		}
		else
		{
			key = Encoding.UTF8.GetBytes("A&A Technology");
		}
		TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider();
		tripleDESCryptoServiceProvider.Key = key;
		tripleDESCryptoServiceProvider.Mode = CipherMode.ECB;
		tripleDESCryptoServiceProvider.Padding = PaddingMode.PKCS7;
		ICryptoTransform cryptoTransform = tripleDESCryptoServiceProvider.CreateDecryptor();
		byte[] bytes = cryptoTransform.TransformFinalBlock(array, 0, array.Length);
		return Encoding.UTF8.GetString(bytes);
	}

	private static string Encrypt(string toEncrypt)
	{
		bool flag = true;
		byte[] bytes = Encoding.UTF8.GetBytes(toEncrypt);
		byte[] key;
		if (flag)
		{
			MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
			key = mD5CryptoServiceProvider.ComputeHash(Encoding.UTF8.GetBytes("A&A Technology"));
		}
		else
		{
			key = Encoding.UTF8.GetBytes("A&A Technology");
		}
		TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider();
		tripleDESCryptoServiceProvider.Key = key;
		tripleDESCryptoServiceProvider.Mode = CipherMode.ECB;
		tripleDESCryptoServiceProvider.Padding = PaddingMode.PKCS7;
		ICryptoTransform cryptoTransform = tripleDESCryptoServiceProvider.CreateEncryptor();
		byte[] array = cryptoTransform.TransformFinalBlock(bytes, 0, bytes.Length);
		return Convert.ToBase64String(array, 0, array.Length);
	}

	public static string GetIME()
	{
		try
		{
			ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("Select * From Win32_BaseBoard");
			using (ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = managementObjectSearcher.Get().GetEnumerator())
			{
				if (managementObjectEnumerator.MoveNext())
				{
					ManagementObject managementObject = (ManagementObject)managementObjectEnumerator.Current;
					return managementObject["SerialNumber"].ToString();
				}
			}
			return "ERROR";
		}
		catch
		{
			return "ERROR";
		}
	}
}
