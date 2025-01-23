using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace AuthDemo.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private static readonly ConnectionMapping<string> Connections = new();
    public async Task SendMessage(string user, string message)
    {
        foreach (string connection in Connections.GetConnections(user))
        {
            await Clients.Client(connection).SendAsync("ReceiveMessage", $"{user}: {message}");
        }
    }

    public override Task OnConnectedAsync()
    {
        string? name = Context.User?.Identity?.Name;
        if (string.IsNullOrEmpty(name))
        {
            return base.OnConnectedAsync();
        }
        
        Connections.Add(name, Context.ConnectionId);

        return base.OnConnectedAsync();
    }
    
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        string? name = Context.User?.Identity?.Name;
        if (string.IsNullOrEmpty(name))
        {
            return base.OnDisconnectedAsync(exception);
        }
        
        Connections.Remove(name, Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }
}
