using System;
using System.IO;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using _5SQA_Read_UWave.Models;
using Newtonsoft.Json;

namespace _5SQA_Read_UWave.Funtions;

public class Common
{
	public static void WriteToFile(string filename, string message)
	{
		string text = Path.Combine("C:\\5SQA_UWave_Config");
		Directory.CreateDirectory(text);
		string path = Path.Combine(text, filename);
		if (!File.Exists(path))
		{
			File.Create(path);
		}
		using StreamWriter streamWriter = File.AppendText(path);
		streamWriter.WriteLine($"{DateTime.Now}: {message}");
	}

	public static Config ReadFromFileConfig(string filename)
	{
		string path = Path.Combine("C:\\5SQA_UWave_Config", filename);
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
