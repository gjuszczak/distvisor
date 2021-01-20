using Distvisor.Web.Data.Events.Core;
using System.Threading.Tasks;

namespace Distvisor.Web.Data.Events
{
    public class EmailReceivedEvent
    {
        public string MimeMessageId { get; set; }
        public string BodyMime { get; set; }
    }

    public class EmailReceivedEventHandler : IEventHandler<EmailReceivedEvent>
    {
        public async Task Handle(EmailReceivedEvent payload)
        {
            //TODO: Implement or remove

            await Task.CompletedTask;
        }
    }
}
