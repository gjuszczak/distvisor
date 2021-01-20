using Distvisor.Web.Data.Events.Core;
using Distvisor.Web.Data.Reads.Core;
using Distvisor.Web.Data.Reads.Entities;
using Distvisor.Web.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Distvisor.Web.Data.Events
{
    public class FinancialAccountAddedEvent : FinancialAccount
    {
        public string[] Paycards { get; set; }
    }

    public class FinancialAccountTransactionAddedEvent : FinancialAccountTransaction
    {
    }

    public class FinancialEventHandlers : IEventHandler<FinancialAccountAddedEvent>, IEventHandler<FinancialAccountTransactionAddedEvent>
    {
        private readonly ReadStoreContext _context;

        public FinancialEventHandlers(ReadStoreContext context)
        {
            _context = context;
        }

        public async Task Handle(FinancialAccountAddedEvent payload)
        {
            _context.FinancialAccounts.Add(new FinancialAccountEntity
            {
                Name = payload.Name,
                Number = payload.Number,
                Paycards = payload.Paycards.Select(name => new FinancialAccountPaycardEntity
                {
                    Name = name
                }).ToList(),
                Type = payload.Type,
            });

            await _context.SaveChangesAsync();
        }

        public async Task Handle(FinancialAccountTransactionAddedEvent payload)
        {
            _context.FinancialAccountTransactions.Add(new FinancialAccountTransactionEntity
            {
                Id = payload.Id,
                AccountId = payload.AccountId,
                SeqNo = payload.SeqNo,
                TransactionDate = payload.TransactionDate,
                PostingDate = payload.PostingDate,
                Title = payload.Title,
                Amount = payload.Amount,
                Balance = payload.Balance,
                Source = FinancialAccountTransactionSource.UserInput,
            });

            await _context.SaveChangesAsync();
        }
    }
}
