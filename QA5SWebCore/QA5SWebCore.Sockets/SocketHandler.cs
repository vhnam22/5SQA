using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QA5SWebCore.Sockets;

public abstract class SocketHandler
{
	public ConnectionManager Connections { get; set; }

	public SocketHandler(ConnectionManager connections)
	{
		Connections = connections;
	}

	public virtual async Task OnConnected(WebSocket socket)
	{
		await Task.Run(delegate
		{
			Connections.AddSocket(socket);
		});
	}

	public virtual async Task OnDisconnected(WebSocket socket)
	{
		await Connections.RemoveSocketAsync(Connections.GetId(socket));
	}

	public async Task SendMessage(WebSocket socket, string message)
	{
		if (socket != null && socket.State.Equals(WebSocketState.Open))
		{
			await socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(message)), WebSocketMessageType.Text, endOfMessage: true, CancellationToken.None);
		}
	}

	public async Task SendMessage(string id, string message)
	{
		await SendMessage(Connections.GetSocketById(id), message);
	}

	public async Task SendMessageToAll(string message)
	{
		foreach (KeyValuePair<string, WebSocket> allConnection in Connections.GetAllConnections())
		{
			await SendMessage(allConnection.Value, message);
		}
	}

	public abstract Task Receive(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);
}
