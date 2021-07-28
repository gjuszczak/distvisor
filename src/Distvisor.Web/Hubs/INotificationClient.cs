using System.Threading.Tasks;

namespace Distvisor.Web.Hubs
{
    public interface INotificationClient
    {
        Task PushNotification(string payload);
        Task PushRfCode(string code);
    }
}
