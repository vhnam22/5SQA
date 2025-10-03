using System;
using System.Collections.Generic;
using System.Management;

namespace _5SQA_Read_UWave.Funtions;

public class COMPortInfo
{
	public string Name { get; set; }

	public string Description { get; set; }

	public static List<COMPortInfo> GetCOMPortsInfo()
	{
		List<COMPortInfo> list = new List<COMPortInfo>();
		ConnectionOptions options = ProcessConnection.ProcessConnectionOptions();
		ManagementScope scope = ProcessConnection.ConnectionScope(Environment.MachineName, options, "\\root\\CIMV2");
		ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_PnPEntity WHERE ConfigManagerErrorCode = 0");
		ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(scope, query);
		using (managementObjectSearcher)
		{
			string text = null;
			foreach (ManagementObject item in managementObjectSearcher.Get())
			{
				if (item == null)
				{
					continue;
				}
				object obj = item["Caption"];
				if (obj != null)
				{
					text = obj.ToString();
					if (text.Contains("(COM"))
					{
						COMPortInfo cOMPortInfo = new COMPortInfo();
						cOMPortInfo.Name = text.Substring(text.LastIndexOf("(COM")).Replace("(", string.Empty).Replace(")", string.Empty);
						cOMPortInfo.Description = text;
						list.Add(cOMPortInfo);
					}
				}
			}
		}
		return list;
	}
}
