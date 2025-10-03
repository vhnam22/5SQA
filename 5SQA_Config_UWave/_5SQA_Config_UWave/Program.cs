using System;
using System.Windows.Forms;

namespace _5SQA_Config_UWave;

internal static class Program
{
	[STAThread]
	private static void Main()
	{
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(defaultValue: false);
		Application.Run(new frmMain());
	}
}
