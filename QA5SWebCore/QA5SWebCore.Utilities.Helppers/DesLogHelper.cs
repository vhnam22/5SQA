using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace QA5SWebCore.Utilities.Helppers;

public static class DesLogHelper
{
	private static byte[] GetKey(string key)
	{
		return Encoding.ASCII.GetBytes(key);
	}

	public static string Encrypt(string originalString, string key)
	{
		if (string.IsNullOrEmpty(originalString))
		{
			return "";
		}
		DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
		dESCryptoServiceProvider.Mode = CipherMode.ECB;
		MemoryStream memoryStream = new MemoryStream();
		CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateEncryptor(GetKey(key), GetKey(key)), CryptoStreamMode.Write);
		StreamWriter streamWriter = new StreamWriter(cryptoStream);
		streamWriter.Write(originalString);
		streamWriter.Flush();
		cryptoStream.FlushFinalBlock();
		streamWriter.Flush();
		return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
	}

	public static string Decrypt(string cryptedString, string key)
	{
		if (string.IsNullOrEmpty(cryptedString))
		{
			return "";
		}
		DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
		dESCryptoServiceProvider.Mode = CipherMode.ECB;
		MemoryStream stream = new MemoryStream(Convert.FromBase64String(cryptedString));
		CryptoStream stream2 = new CryptoStream(stream, dESCryptoServiceProvider.CreateDecryptor(GetKey(key), GetKey(key)), CryptoStreamMode.Read);
		StreamReader streamReader = new StreamReader(stream2);
		return streamReader.ReadToEnd();
	}
}
