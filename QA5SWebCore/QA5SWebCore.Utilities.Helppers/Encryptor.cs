using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace QA5SWebCore.Utilities.Helppers;

public class Encryptor
{
	public static string SHA256encrypt(string phrase)
	{
		UTF8Encoding uTF8Encoding = new UTF8Encoding();
		SHA256Managed sHA256Managed = new SHA256Managed();
		byte[] bytes = sHA256Managed.ComputeHash(uTF8Encoding.GetBytes(phrase));
		return HexStringFromBytes(bytes);
	}

	public static string GenerateSaltedSHA1(string source, string salt = "")
	{
		HashAlgorithm hashAlgorithm = new SHA1Managed();
		byte[] bytes = Encoding.ASCII.GetBytes(salt);
		byte[] bytes2 = Encoding.ASCII.GetBytes(source);
		byte[] buffer = AppendByteArrays(bytes, bytes2);
		byte[] bytes3 = hashAlgorithm.ComputeHash(buffer);
		return HexStringFromBytes(bytes3);
	}

	public static string GenerateRandomCode(PasswordOptions opts = null)
	{
		if (opts == null)
		{
			opts = new PasswordOptions
			{
				RequiredLength = 8,
				RequiredUniqueChars = 4,
				RequireDigit = true,
				RequireLowercase = false,
				RequireNonAlphanumeric = false,
				RequireUppercase = true
			};
		}
		string[] array = new string[2] { "ABCDEFGHJKLMNOPQRSTUVWXYZ", "0123456789" };
		Random random = new Random();
		List<char> list = new List<char>();
		if (opts.RequireUppercase)
		{
			list.Insert(random.Next(0, list.Count), array[0][random.Next(0, array[0].Length)]);
		}
		if (opts.RequireDigit)
		{
			list.Insert(random.Next(0, list.Count), array[1][random.Next(0, array[1].Length)]);
		}
		for (int i = list.Count; i < opts.RequiredLength || list.Distinct().Count() < opts.RequiredUniqueChars; i++)
		{
			string text = array[random.Next(0, array.Length)];
			list.Insert(random.Next(0, list.Count), text[random.Next(0, text.Length)]);
		}
		return new string(list.ToArray());
	}

	public static string GenerateRandomPassword(PasswordOptions opts = null)
	{
		if (opts == null)
		{
			opts = new PasswordOptions
			{
				RequiredLength = 8,
				RequiredUniqueChars = 4,
				RequireDigit = true,
				RequireLowercase = true,
				RequireNonAlphanumeric = true,
				RequireUppercase = true
			};
		}
		string[] array = new string[4] { "ABCDEFGHJKLMNOPQRSTUVWXYZ", "abcdefghijkmnopqrstuvwxyz", "0123456789", "!@$?_-" };
		Random random = new Random(Environment.TickCount);
		List<char> list = new List<char>();
		if (opts.RequireUppercase)
		{
			list.Insert(random.Next(0, list.Count), array[0][random.Next(0, array[0].Length)]);
		}
		if (opts.RequireLowercase)
		{
			list.Insert(random.Next(0, list.Count), array[1][random.Next(0, array[1].Length)]);
		}
		if (opts.RequireDigit)
		{
			list.Insert(random.Next(0, list.Count), array[2][random.Next(0, array[2].Length)]);
		}
		if (opts.RequireNonAlphanumeric)
		{
			list.Insert(random.Next(0, list.Count), array[3][random.Next(0, array[3].Length)]);
		}
		for (int i = list.Count; i < opts.RequiredLength || list.Distinct().Count() < opts.RequiredUniqueChars; i++)
		{
			string text = array[random.Next(0, array.Length)];
			list.Insert(random.Next(0, list.Count), text[random.Next(0, text.Length)]);
		}
		return new string(list.ToArray());
	}

	private static string HexStringFromBytes(byte[] bytes)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (byte b in bytes)
		{
			string value = b.ToString("x2");
			stringBuilder.Append(value);
		}
		return stringBuilder.ToString();
	}

	private static byte[] AppendByteArrays(byte[] byteArrayA, byte[] byteArrayB)
	{
		byte[] array = new byte[byteArrayA.Length + byteArrayB.Length];
		for (int i = 0; i < byteArrayA.Length; i++)
		{
			array[i] = byteArrayA[i];
		}
		for (int j = 0; j < byteArrayB.Length; j++)
		{
			array[byteArrayA.Length + j] = byteArrayB[j];
		}
		return array;
	}
}
