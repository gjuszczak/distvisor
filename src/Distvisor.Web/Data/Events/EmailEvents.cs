using Distvisor.Web.Data.Events.Core;
using Distvisor.Web.Data.Reads.Core;
using Distvisor.Web.Data.Reads.Entities;
using Distvisor.Web.Services;
using Microsoft.EntityFrameworkCore;
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
        private readonly ReadStoreContext _context;
        private readonly IFinancialEmailDataExtractor _financialEmailDataExtractor;

        public EmailReceivedEventHandler(ReadStoreContext context, IFinancialEmailDataExtractor financialEmailDataExtractor)
        {
            _context = context;
            _financialEmailDataExtractor = financialEmailDataExtractor;
        }

        public async Task Handle(EmailReceivedEvent payload)
        {
            await _financialEmailDataExtractor.ExtractAsync(payload.BodyMime);

            var entry = _context.ProcessedEmails.Add(new ProcessedEmailEntity
            {
                UniqueKey = payload.MimeMessageId,
                BodyMime = payload.BodyMime,
            });

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                entry.State = EntityState.Detached;
                throw;
            }
        }
    }
}
