using Distvisor.Web.Data.Events.Core;
using Distvisor.Web.Data.Reads.Core;
using System.Text.Json;
using System.Threading.Tasks;

namespace Distvisor.Web.Data.Events
{
    public class EmailReceivedEvent
    {
        public decimal Timestamp { get; set; }
        public string StorageKey { get; set; }
        public JsonElement Content { get; set; }
    }

    public class EmailReceivedEventHandler : IEventHandler<EmailReceivedEvent>
    {
        private readonly ReadStoreContext _context;

        public EmailReceivedEventHandler(ReadStoreContext context)
        {
            _context = context;
        }

        public async Task Handle(EmailReceivedEvent payload)
        {
           
        }
    }
}
