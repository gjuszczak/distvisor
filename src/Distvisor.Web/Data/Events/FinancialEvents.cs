using Distvisor.Web.Data.Events.Core;
using Distvisor.Web.Data.Reads.Core;
using Distvisor.Web.Data.Reads.Entities;
using Distvisor.Web.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Distvisor.Web.Data.Events
{
    public class FinancialAccountAddedEvent : FinancialAccount
    {
        public string[] Paycards { get; set; }
    }

    public class FinancialAccountTransactionAddedEvent
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Details { get; set; }
        public decimal Amount { get; set; }
        public decimal? Balance { get; set; }
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
            async Task<decimal> CalculateBalance()
            {
                var lastTransaction = await _context.FinancialAccountTransactions
                    .Where(x => x.AccountId == payload.AccountId)
                    .OrderByDescending(x => x.Date)
                    .FirstOrDefaultAsync();
                return (lastTransaction?.Balance ?? 0m) + payload.Amount;
            }

            _context.FinancialAccountTransactions.Add(new FinancialAccountTransactionEntity
            {
                Id = payload.Id,
                AccountId = payload.AccountId,
                Amount = payload.Amount,
                Balance = payload.Balance ?? await CalculateBalance(),
                DataSource = FinancialAccountTransactionDataSource.UserInput,
                Date = payload.Date,
                Details = payload.Details,
                IsBalanceEstimated = !payload.Balance.HasValue
            });

            await _context.SaveChangesAsync();
        }
    }
}
