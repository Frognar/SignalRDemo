using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace AuthDemo.Hubs;

[Authorize]
public class ChatHub : Hub
{
    public async Task SendMessage(string who, string message)
    {
        string? name = Context.User?.Identity?.Name;
        if (string.IsNullOrEmpty(name))
        {
            return;
        }

        await Clients.Groups(who).SendAsync("ReceiveMessage", $"{name}: {message}");
    }

    public override async Task OnConnectedAsync()
    {
        string? name = Context.User?.Identity?.Name;
        if (string.IsNullOrEmpty(name))
        {
            await base.OnConnectedAsync();
            return;
        }
        
        await Groups.AddToGroupAsync(Context.ConnectionId, name);
        await base.OnConnectedAsync();
    }
}
