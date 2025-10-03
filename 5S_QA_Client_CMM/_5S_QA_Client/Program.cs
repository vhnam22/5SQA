using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using _5S_QA_Client.Controls;

namespace _5S_QA_Client;

internal static class Program
{
	[STAThread]
	private static void Main()
	{
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(defaultValue: false);
		if (CountAppRunning() > 0)
		{
			MessageBox.Show(Assembly.GetEntryAssembly().GetName().Name + " " + Common.getTextLanguage("frmLogin", "wIsOpen"), Common.getTextLanguage("frmLogin", "INFORMATION"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			Environment.Exit(0);
		}
		if (!CheckVersion())
		{
			Application.Run(new frmLogin());
		}
		else
		{
			Application.Restart();
		}
	}

	private static int CountAppRunning()
	{
		string name = Assembly.GetEntryAssembly().GetName().Name;
		int num = 0;
		Process[] processes = Process.GetProcesses();
		foreach (Process process in processes)
		{
			if (process.ProcessName.ToLower().Contains(name.ToLower()))
			{
				num++;
			}
		}
		if (num > 0)
		{
			num--;
		}
		return num;
	}

	private static bool CheckVersion()
	{
		string connectionString = ConfigurationManager.ConnectionStrings["DirUpdate"].ConnectionString;
		if (Directory.Exists(connectionString))
		{
			Directory.CreateDirectory(".\\Olds");
			string[] directories = Directory.GetDirectories(connectionString);
			string name = Assembly.GetEntryAssembly().GetName().Name;
			int.TryParse(Assembly.GetEntryAssembly().GetName().Version.ToString(3).Replace(".", ""), out var result);
			string[] array = directories;
			foreach (string text in array)
			{
				if (!text.Contains(name))
				{
					continue;
				}
				string text2 = text.Split(' ').LastOrDefault();
				int.TryParse(text2.Replace(".", ""), out var result2);
				if (result >= result2)
				{
					continue;
				}
				DialogResult dialogResult = MessageBox.Show(Common.getTextLanguage("frmLogin", "wHasNewVersion") + " " + name + " v" + text2 + "!\r\n" + Common.getTextLanguage("frmLogin", "wHasUpdate"), Common.getTextLanguage("frmLogin", "QUESTION"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if (dialogResult != DialogResult.Yes)
				{
					break;
				}
				string[] files = Directory.GetFiles(text);
				string[] array2 = files;
				foreach (string text3 in array2)
				{
					string fileName = Path.GetFileName(text3);
					string text4 = Path.Combine(".\\Olds", fileName);
					string text5 = Path.Combine(".\\", fileName);
					if (File.Exists(text4))
					{
						File.Delete(text4);
					}
					if (File.Exists(text5))
					{
						File.Move(text5, text4);
					}
					File.Copy(text3, text5, overwrite: true);
				}
				string[] directories2 = Directory.GetDirectories(text);
				string[] array3 = directories2;
				foreach (string path in array3)
				{
					files = Directory.GetFiles(path);
					string[] array4 = files;
					foreach (string text6 in array4)
					{
						string path2 = text6.Replace(text + "\\", "");
						string text7 = Path.Combine(".\\Olds", path2);
						string text8 = Path.Combine(".\\", path2);
						if (File.Exists(text7))
						{
							File.Delete(text7);
						}
						Directory.CreateDirectory(Path.GetDirectoryName(text7));
						if (File.Exists(text8))
						{
							File.Move(text8, text7);
						}
						File.Copy(text6, text8, overwrite: true);
					}
				}
				MessageBox.Show(Common.getTextLanguage("frmLogin", "wUpdate") + " " + name + " v" + text2 + " " + Common.getTextLanguage("frmLogin", "wSuccessful"), Common.getTextLanguage("frmLogin", "INFORMATION"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return true;
			}
		}
		return false;
	}
}
