using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace AuthDemo.Hubs;

[Authorize]
public class ChatHub : Hub
{
    public async Task SendMessage(string toWho, string user, string message)
    {
        await Clients.Groups(toWho).SendAsync("ReceiveMessage", user, message);
    }

    public override async Task OnConnectedAsync()
    {
        string name = Context.User!.Identity!.Name!;
        await Groups.AddToGroupAsync(Context.ConnectionId, name);
        await base.OnConnectedAsync();
    }
}
