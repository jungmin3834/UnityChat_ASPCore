using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ServerASP.Hubs
{
    public class ChatHub : Hub
    {

        public  Task SendMessage(string user, string message)
        {
            Clients.Group("_aid").SendAsync("broadcastMessage", "System", $"Group call {message}");
            return Clients.All.SendAsync("broadcastMessage", user, message);
        }

       public async Task JoinGroup(string groupName,string message)
       {
          await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
          await Clients.Group(groupName).SendAsync("broadcastMessage","System", $"{Context.ConnectionId} joined {groupName}");
       }

        public async Task Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            await Clients.All.SendAsync("broadcastMessage", name, message);
        }
    }
}