using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace QA5SWebCore;

public class Program
{
	public static void Main(string[] args)
	{
		CreateHostBuilder(args).Build().Run();
	}

	public static IHostBuilder CreateHostBuilder(string[] args)
	{
		return Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(delegate(IWebHostBuilder webBuilder)
		{
			webBuilder.UseStartup<Startup>();
		});
	}
}
