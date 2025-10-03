using System.Management;

namespace _5SQA_Config_UWave.Funtions;

internal class ProcessConnection
{
	public static ConnectionOptions ProcessConnectionOptions()
	{
		ConnectionOptions connectionOptions = new ConnectionOptions();
		connectionOptions.Impersonation = ImpersonationLevel.Impersonate;
		connectionOptions.Authentication = AuthenticationLevel.Default;
		connectionOptions.EnablePrivileges = true;
		return connectionOptions;
	}

	public static ManagementScope ConnectionScope(string machineName, ConnectionOptions options, string path)
	{
		ManagementScope managementScope = new ManagementScope();
		managementScope.Path = new ManagementPath("\\\\" + machineName + path);
		managementScope.Options = options;
		managementScope.Connect();
		return managementScope;
	}
}
