using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using SignalRChat.Hubs;

namespace SignalRChat
{
    [AllowAnonymous]
    public class MessageController : Controller
    {
        public MessageController(Microsoft.AspNetCore.SignalR.IHubContext<ChatHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        public IHubContext<ChatHub> hubContext { get; }

        public IActionResult Index()
        {
            return View();
        }

        // postMessage method
        [HttpPost]
        public IActionResult postMessage(string userName, string message, string groupName = "SGroup")
        {
            this.hubContext.Clients.Group(groupName).SendAsync("ReceiveMessage", userName, message);
            return Ok();
        }
    }
}
