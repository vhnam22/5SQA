using System.Text.RegularExpressions;

namespace _5S_QA_Client.Controls;

internal class MachineFilter
{
	public static string GetResult_HR530(string rxString)
	{
		string value = Regex.Match(rxString, "\\d+\\.\\d+").Value;
		if (double.TryParse(value, out var result))
		{
			return result.ToString();
		}
		return "";
	}
}
