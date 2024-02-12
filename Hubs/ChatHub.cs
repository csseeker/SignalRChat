using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        static long counter=0;
        static Dictionary<string, string> ConnectedUserGroups = new Dictionary<string, string>();

        // Add event handler for OnConnected
        public override async Task OnConnectedAsync()
        {
            Debug.WriteLine($" *** OnConnectedAsync: {Context.ConnectionId} {Context.User}:{Context.UserIdentifier}");

            // string? UserName = Context?.User?.Identity?.Name;

            // if (!string.IsNullOrWhiteSpace(UserName) && UserName.Contains("shripad", StringComparison.InvariantCultureIgnoreCase))
            // {

            ChatHub.counter++;

            if(ChatHub.counter %2 == 0)
            {
                var connId = Context.ConnectionId;
                Debug.WriteLine($"Adding connection [{connId}] to ShripadGroup!");
                await Groups.AddToGroupAsync(connId, "ShripadGroup");
            }
            // }
            await base.OnConnectedAsync();
            await Clients.All.SendAsync("UserConnected", Context.ConnectionId);
        }

        // Add event handler for OnDisconnected
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Debug.WriteLine($" *** OnDisconnectedAsync: {Context.ConnectionId} {Context.User}:{Context.UserIdentifier} ({exception?.Message})");
            await base.OnDisconnectedAsync(exception);
            await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId);
        }

        public async Task SendMessage(string user, string message)
        {
            Debug.WriteLine($" *** SendMessage only to ShripadGroup users: {user} {message}");
            // Send message only to users with name starting with 'user'
            // await Clients.User(user).SendAsync("ReceiveMessage", user, message);

            // await Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", user, message);

            await Clients.Group("ShripadGroup").SendAsync("ReceiveMessage", user, message);

            // await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}