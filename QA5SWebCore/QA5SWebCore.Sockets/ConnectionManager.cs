using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace QA5SWebCore.Sockets;

public class ConnectionManager
{
	private ConcurrentDictionary<string, WebSocket> _connection = new ConcurrentDictionary<string, WebSocket>();

	public WebSocket GetSocketById(string id)
	{
		return _connection.FirstOrDefault((KeyValuePair<string, WebSocket> x) => x.Key == id).Value;
	}

	public ConcurrentDictionary<string, WebSocket> GetAllConnections()
	{
		return _connection;
	}

	public string GetId(WebSocket socket)
	{
		return _connection.FirstOrDefault((KeyValuePair<string, WebSocket> x) => x.Value == socket).Key;
	}

	public async Task RemoveSocketAsync(string id)
	{
		_connection.TryRemove(id, out var value);
		await value.CloseAsync(WebSocketCloseStatus.NormalClosure, "Socket connection closed", CancellationToken.None);
	}

	public void AddSocket(WebSocket socket)
	{
		_connection.TryAdd(GetConnectionId(), socket);
	}

	public async Task<bool> UpdateIdSocketAsync(string oldid, string newid)
	{
		WebSocket socketById = GetSocketById(newid);
		if (socketById != null)
		{
			if (socketById.State == WebSocketState.Open)
			{
				return false;
			}
			await RemoveSocketAsync(newid);
		}
		_connection.TryRemove(oldid, out var value);
		_connection.TryAdd(newid, value);
		return true;
	}

	private string GetConnectionId()
	{
		return Guid.NewGuid().ToString("N");
	}
}
