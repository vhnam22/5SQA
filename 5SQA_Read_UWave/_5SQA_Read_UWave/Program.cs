using System.ServiceProcess;

namespace _5SQA_Read_UWave;

internal static class Program
{
	private static void Main()
	{
		ServiceBase[] services = new ServiceBase[1]
		{
			new ServiceMain()
		};
		ServiceBase.Run(services);
	}
}
