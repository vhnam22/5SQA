using System.Management;

namespace _5S_QA_Client.Controls;

internal class ProcessConnection
{
	public static ConnectionOptions ProcessConnectionOptions()
	{
		return new ConnectionOptions
		{
			Impersonation = ImpersonationLevel.Impersonate,
			Authentication = AuthenticationLevel.Default,
			EnablePrivileges = true
		};
	}

	public static ManagementScope ConnectionScope(string machineName, ConnectionOptions options, string path)
	{
		ManagementScope managementScope = new ManagementScope
		{
			Path = new ManagementPath("\\\\" + machineName + path),
			Options = options
		};
		managementScope.Connect();
		return managementScope;
	}
}
