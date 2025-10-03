using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace QA5SWebCore.Sockets;

public class WebSocketMessageHandler : SocketHandler
{
	public WebSocketMessageHandler(ConnectionManager connections)
		: base(connections)
	{
	}

	public override async Task OnConnected(WebSocket socket)
	{
		await base.OnConnected(socket);
		await SendMessageToAll($"Socket count: {base.Connections.GetAllConnections().Count}");
		await SendMessage(socket, "GetId@");
	}

	public override async Task OnDisconnected(WebSocket socket)
	{
		await base.OnDisconnected(socket);
		await SendMessageToAll($"{base.Connections.GetId(socket)} closed. Socket count: {base.Connections.GetAllConnections().Count}");
	}

	public override async Task Receive(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
	{
		string id = base.Connections.GetId(socket);
		string text = Encoding.UTF8.GetString(buffer, 0, result.Count);
		if (!text.Contains("SetId@"))
		{
			return;
		}
		string[] array = text.Split('@');
		if (array.Length.Equals(2) && !string.IsNullOrEmpty(array[1]))
		{
			if (await base.Connections.UpdateIdSocketAsync(id, array[1]))
			{
				await SendMessage(socket, "Successful");
			}
			else
			{
				await SendMessage(socket, "Unsuccessful");
			}
		}
	}
}
