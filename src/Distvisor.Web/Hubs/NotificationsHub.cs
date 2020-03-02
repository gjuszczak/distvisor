using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Distvisor.Web.Hubs
{
    public class NotificationsHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
