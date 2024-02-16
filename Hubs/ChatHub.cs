using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        //static long counter=0;

        // static Dictionary<string, List<string>> ConnectedUserGroups = new Dictionary<string, List<string>>{
        //     {"SGroup", new List<string>()},
        //     {"JGroup", new List<string>()}
        // };

        // Add event handler for OnConnected
        public override async Task OnConnectedAsync()
        {
            Debug.WriteLine(" ---------------------- --------------------------- ");
            // Print all query string parameters recieved
            foreach (var query in Context.GetHttpContext().Request.Query)
            {
                Debug.WriteLine($"Query: {query.Key} = {query.Value}");
            }

            var userName = Context.GetHttpContext().Request.Query["userName"];
            var userConnectionId = Context.GetHttpContext().Request.Query["userConnectionId"];
            
            Debug.WriteLine(" ---------------------- --------------------------- ");
            Debug.WriteLine($" *** OnConnectedAsync: {Context.ConnectionId}:: {userName} :: {userConnectionId}");
            Debug.WriteLine(" ---------------------- --------------------------- ");

            // Read all custom headers and log them
            foreach (var header in Context.GetHttpContext().Request.Headers)
            {
                Debug.WriteLine($"Header: {header.Key} = {header.Value}");
            }

            Debug.WriteLine(" ---------------------- --------------------------- ");

            if(userName.ToString().Contains("shripad", StringComparison.InvariantCultureIgnoreCase))
            {
                var connId = Context.ConnectionId;
                Debug.WriteLine($"Adding connection [{connId}] to SGroup!");

                // create a signalr group named SGroup and add the connection to it
                await Groups.AddToGroupAsync(connId, "SGroup");
            }
            else if(userName.ToString().Contains("jatan", StringComparison.InvariantCultureIgnoreCase))
            {
                var connId = Context.ConnectionId;
                Debug.WriteLine($"Adding connection [{connId}] to JGroup!");

                // create a signalr group named JGroup and add the connection to it
                await Groups.AddToGroupAsync(connId, "JGroup");
            }

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
            Debug.WriteLine(" --- ");
            Debug.WriteLine($" *** SendMessage only to ShripadGroup users: {user} {message}");
            string connectionId = Context.ConnectionId;
            Debug.WriteLine(" --- ");

            Debug.WriteLine($"Context.UserIdentifier: {Context.UserIdentifier}");
            Debug.WriteLine($"Context.User: {Context.User}");
            Debug.WriteLine($"Context.Items: {Context.Items}");
            Debug.WriteLine($"Context.Items[arguments]: {Context.Items["arguments"]}");
            Debug.WriteLine(" --- ");

            // Get connection information
            var connectionInfo = Context.GetHttpContext().Connection;
            Debug.WriteLine($"ConnectionId: {connectionInfo.Id}");
            Debug.WriteLine($"RemoteIpAddress: {connectionInfo.RemoteIpAddress}");
            Debug.WriteLine($"RemotePort: {connectionInfo.RemotePort}");
            Debug.WriteLine($"LocalIpAddress: {connectionInfo.LocalIpAddress}");
            Debug.WriteLine($"LocalPort: {connectionInfo.LocalPort}");
            Debug.WriteLine(" --- ");

            // Send message only to users with name starting with 'user'
            // await Clients.User(user).SendAsync("ReceiveMessage", user, message);

            // await Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", user, message);

            // if username contains shripad, send message to SGroup
            if(user.Contains("shripad", StringComparison.InvariantCultureIgnoreCase))
            {
                Debug.WriteLine($"Sending message to SGroup: {user} {message}");
                await Clients.Group("SGroup").SendAsync("ReceiveMessage", user, message);
            }
            else if(user.Contains("jatan", StringComparison.InvariantCultureIgnoreCase))
            {
                Debug.WriteLine($"Sending message to JGroup: {user} {message}");
                await Clients.Group("JGroup").SendAsync("ReceiveMessage", user, message);
            }

            // await Clients.Group("ShripadGroup").SendAsync("ReceiveMessage", user, message);
            // await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        
        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }        
    }
}