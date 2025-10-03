using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace QA5SWebCore.Sockets;

public class SocketMiddeware
{
	private readonly RequestDelegate _next;

	private SocketHandler Handler { get; set; }

	public SocketMiddeware(RequestDelegate next, SocketHandler handler)
	{
		_next = next;
		Handler = handler;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		if (!context.WebSockets.IsWebSocketRequest)
		{
			return;
		}
		WebSocket socket = await context.WebSockets.AcceptWebSocketAsync();
		await Handler.OnConnected(socket);
		await Receive(socket, async delegate(WebSocketReceiveResult result, byte[] buffer)
		{
			if (result.MessageType == WebSocketMessageType.Text)
			{
				await Handler.Receive(socket, result, buffer);
			}
			else if (result.MessageType == WebSocketMessageType.Close)
			{
				await Handler.OnDisconnected(socket);
			}
		});
	}

	private async Task Receive(WebSocket socket, Action<WebSocketReceiveResult, byte[]> messageHandler)
	{
		byte[] buffer = new byte[4096];
		while (socket.State == WebSocketState.Open)
		{
			try
			{
				messageHandler(await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None), buffer);
			}
			catch (WebSocketException ex)
			{
				if (ex.WebSocketErrorCode == WebSocketError.ConnectionClosedPrematurely)
				{
					await Handler.OnDisconnected(socket);
				}
			}
		}
	}
}
