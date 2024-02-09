using Microsoft.AspNetCore.SignalR;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            // Send message only to users with name starting with 'user'
            // await Clients.User(user).SendAsync("ReceiveMessage", user, message);

            // await Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", user, message);

            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}