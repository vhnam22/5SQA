using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

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

class Program
{
    static void Main(string[] args)
    {
        string originalString = @"{
  ""ActiveDate"": ""2025-03-11"",
  ""NumUserSystem"": 3,
  ""ImeList"": ""22HN522#CLP-35;GT8XTBX#SE3500;FT8XTBX#EF3000;JPH742N4D1#ROUNDNESS-ACCTee;S9PFWT01D175395#Rs232;3b3e44cbd6d4c272;JPH7204WZS#CONTOUR-ACCTee;JPH117W649#SUFRCOM-ACCTee;46M682S#CRYSTA;8V0W4Y1#Rs232;F0766V1#UWave"",
  ""Funtion"": ""QC2106;QC2107;QC2108;QC2202;""
}";
        string key = "GetBytes";
        
        try
        {
            Console.WriteLine("Starting encryption...");
            Console.WriteLine($"Original string:");
            Console.WriteLine(originalString);
            Console.WriteLine($"Key: {key}");
            Console.WriteLine();
            
            string encryptedResult = DesLogHelper.Encrypt(originalString, key);
            Console.WriteLine("Encryption successful!");
            Console.WriteLine("Encrypted result:");
            Console.WriteLine(encryptedResult);
            Console.WriteLine();
            
            // Test decryption to verify
            Console.WriteLine("Verifying with decryption...");
            string decryptedResult = DesLogHelper.Decrypt(encryptedResult, key);
            Console.WriteLine("Decrypted result:");
            Console.WriteLine(decryptedResult);
            
            Console.WriteLine();
            Console.WriteLine($"Verification: {(originalString == decryptedResult ? "SUCCESS" : "FAILED")}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during encryption/decryption: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
        }
        
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}