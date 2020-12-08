using Distvisor.Web.Data.Events;
using Distvisor.Web.Data.Events.Core;
using Distvisor.Web.Data.Reads.Core;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IFinancialAccountsService
    {
        Task AddAccountAsync(FinancialAccount account);
        Task<List<FinancialAccount>> ListAccountsAsync();
    }

    public class FinancialAccountsService : IFinancialAccountsService
    {
        private readonly IEventStore _eventStore;
        private readonly ReadStoreContext _context;

        public FinancialAccountsService(IEventStore eventStore, ReadStoreContext context)
        {
            _eventStore = eventStore;
            _context = context;
        }

        public async Task AddAccountAsync(FinancialAccount account)
        {
            await _eventStore.Publish(new AddFinancialAccountEvent
            {
                Name = account.Name,
                Number = account.Number,
                Paycards = account.Paycards
            });
        }

        public async Task<List<FinancialAccount>> ListAccountsAsync()
        {
            var entities = await _context.FinancialAccounts
                .Include(x => x.Paycards)
                .ToListAsync();

            return entities.Select(e => new FinancialAccount
            {
                Name = e.Name,
                Number = e.Number,
                Paycards = e.Paycards.Select(p => p.Name).ToArray()
            }).ToList();
        }
    }

    public class FinancialAccount
    {
        public string Name { get; set; }
        public string Number { get; set; }
        public string[] Paycards { get; set; }
    }
}
