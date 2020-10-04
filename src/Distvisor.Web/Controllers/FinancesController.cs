using Distvisor.Web.BackgroundServices;
using Distvisor.Web.Data.Events;
using Distvisor.Web.Data.Events.Core;
using Distvisor.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Distvisor.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FinancesController : ControllerBase
    {
        private readonly IMailgunClient _mailgun;
        private readonly IEventStore _eventStore;
        private readonly IEmailReceivedNotifier _emailReceivedNotifier;

        public FinancesController(IMailgunClient mailgun, IEventStore eventStore, IEmailReceivedNotifier emailReceivedNotifier)
        {
            _mailgun = mailgun;
            _eventStore = eventStore;
            _emailReceivedNotifier = emailReceivedNotifier;
        }

        [HttpGet]
        public async Task<object> GetEmail()
        {
            var l = await _mailgun.ListStoredEmailsAsync();
            var sl = l.ToArray().Last();
            var r = await _mailgun.GetStoredEmailAsync(sl.Url);

            await _eventStore.Publish(new EmailReceivedEvent
            {
                StorageKey = sl.StorageKey,
                Timestamp = sl.Timestamp,
                Content = r.RootElement
            });

            return r;
        }

        [HttpPost("notify")]
        public void Notify()
        {
            _emailReceivedNotifier.Notify();
        }
    }
}
