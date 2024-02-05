using Microsoft.AspNetCore.SignalR;

namespace campground_api.Utils
{
    public class MessageHub : Hub
    {
        public override async Task OnConnectedAsync()
        {

            await Clients.All.SendAsync("ReceiveMessage", $"{Context.ConnectionId} has joined");
        }
    }
}
