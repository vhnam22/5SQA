using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace QA5SWebCore.Sockets;

public static class SocketsExtension
{
	public static IServiceCollection AddWebSocketManager(this IServiceCollection services)
	{
		services.AddTransient<ConnectionManager>();
		foreach (Type exportedType in Assembly.GetExecutingAssembly().ExportedTypes)
		{
			if (exportedType.GetTypeInfo().BaseType == typeof(SocketHandler))
			{
				services.AddSingleton(exportedType);
			}
		}
		return services;
	}

	public static IApplicationBuilder MapSockets(this IApplicationBuilder app, PathString path, SocketHandler socket)
	{
		return app.Map(path, delegate(IApplicationBuilder x)
		{
			x.UseMiddleware<SocketMiddeware>(new object[1] { socket });
		});
	}
}
