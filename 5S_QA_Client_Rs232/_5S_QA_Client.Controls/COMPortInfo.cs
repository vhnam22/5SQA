using System;
using System.Collections.Generic;
using System.Management;

namespace _5S_QA_Client.Controls;

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
			foreach (ManagementBaseObject item2 in managementObjectSearcher.Get())
			{
				if (item2 == null)
				{
					continue;
				}
				object obj = item2["Caption"];
				if (obj != null)
				{
					text = obj.ToString();
					if (text.Contains("(COM"))
					{
						COMPortInfo item = new COMPortInfo
						{
							Name = text.Substring(text.LastIndexOf("(COM")).Replace("(", string.Empty).Replace(")", string.Empty),
							Description = text
						};
						list.Add(item);
					}
				}
			}
		}
		return list;
	}
}
