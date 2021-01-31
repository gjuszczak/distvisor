using Distvisor.Web.Data.Events.Core;
using Distvisor.Web.Data.Reads.Core;
using Distvisor.Web.Data.Reads.Entities;
using Distvisor.Web.Models;
using System.Threading.Tasks;

namespace Distvisor.Web.Data.Events
{
    public class FinancialAccountAddedEvent : FinancialAccount
    {
    }

    public class FinancialAccountTransactionAddedEvent : FinancialAccountTransaction
    {
    }

    public class FinancialDataImportedEvent
    {
        public FinancialAccountTransaction[] Transactions { get; set; }
    }

    public class FinancialEventHandlers :
        IEventHandler<FinancialAccountAddedEvent>,
        IEventHandler<FinancialAccountTransactionAddedEvent>,
        IEventHandler<FinancialDataImportedEvent>
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
                Id = payload.Id,
                Name = payload.Name,
                Number = payload.Number,
                Type = payload.Type,
                CreatedDateTimeUtc = payload.CreatedDateTimeUtc,
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
                TransactionHash = payload.TransactionHash,
            });

            await _context.SaveChangesAsync();
        }

        public async Task Handle(FinancialDataImportedEvent payload)
        {
            foreach (var t in payload.Transactions)
            {
                _context.FinancialAccountTransactions.Add(new FinancialAccountTransactionEntity
                {
                    Id = t.Id,
                    AccountId = t.AccountId,
                    SeqNo = t.SeqNo,
                    TransactionDate = t.TransactionDate,
                    PostingDate = t.PostingDate,
                    Title = t.Title,
                    Amount = t.Amount,
                    Balance = t.Balance,
                    Source = t.Source,
                    TransactionHash = t.TransactionHash,
                });
            }

            await _context.SaveChangesAsync();
        }
    }
}
