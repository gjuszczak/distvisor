using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Distvisor.Web.Hubs
{
    [Authorize]
    public class NotificationsHub : Hub<INotificationClient>
    {
    }
}
