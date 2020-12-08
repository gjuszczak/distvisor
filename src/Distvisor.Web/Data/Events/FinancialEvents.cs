using Distvisor.Web.Data.Events.Core;
using Distvisor.Web.Data.Reads.Core;
using Distvisor.Web.Data.Reads.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Distvisor.Web.Data.Events
{
    public class AddFinancialAccountEvent
    {
        public string Name { get; set; }
        public string Number { get; set; }
        public string[] Paycards { get; set; }
    }

    public class AddFinancialAccountEventHandler : IEventHandler<AddFinancialAccountEvent>
    {
        private readonly ReadStoreContext _context;

        public AddFinancialAccountEventHandler(ReadStoreContext context)
        {
            _context = context;
        }

        public async Task Handle(AddFinancialAccountEvent payload)
        {
            _context.FinancialAccounts.Add(new FinancialAccountEntity
            {
                Name = payload.Name,
                Number = payload.Number,
                Paycards = payload.Paycards.Select(name => new FinancialAccountPaycardEntity
                {
                    Name = name
                }).ToList()
            });

            await _context.SaveChangesAsync();
        }
    }
}
