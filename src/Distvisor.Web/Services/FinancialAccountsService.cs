using Distvisor.Web.Data.Events;
using Distvisor.Web.Data.Events.Core;
using Distvisor.Web.Data.Reads.Core;
using Distvisor.Web.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IFinancialAccountsService
    {
        Task AddAccountAsync(FinancialAccountDto account);
        Task AddAccountTransactionAsync(FinancialAccountTransactionDto transaction);
        Task<List<FinancialAccountDto>> ListAccountsAsync();
    }

    public class FinancialAccountsService : IFinancialAccountsService
    {
        private readonly IEventStore _eventStore;
        private readonly ReadStoreContext _context;
        private readonly INotificationService _notifications;

        public FinancialAccountsService(IEventStore eventStore, ReadStoreContext context, INotificationService notifiactions)
        {
            _eventStore = eventStore;
            _context = context;
            _notifications = notifiactions;
        }

        public async Task AddAccountAsync(FinancialAccountDto account)
        {
            await _notifications.PushSuccessAsync("Account added successfully.");

            await _eventStore.Publish(new FinancialAccountAddedEvent
            {
                Name = account.Name,
                Number = account.Number,
                Paycards = account.Paycards,
                Type = account.Type
            });
        }

        public async Task<List<FinancialAccountDto>> ListAccountsAsync()
        {
            var entities = await _context.FinancialAccounts
                .Include(x => x.Paycards)
                .ToListAsync();

            return entities.Select(e => new FinancialAccountDto
            {
                Name = e.Name,
                Number = e.Number,
                Paycards = e.Paycards.Select(p => p.Name).ToArray()
            }).ToList();
        }

        public async Task AddAccountTransactionAsync(FinancialAccountTransactionDto transaction)
        {
            await _notifications.PushSuccessAsync("Transaction added successfully.");

            await _eventStore.Publish(new FinancialAccountTransactionAddedEvent
            {
                AccountId = transaction.AccountId,
                Amount = transaction.Amount,
                Balance = transaction.Balance,
                DataSource = transaction.DataSource,
                Date = transaction.Date,
                Details = transaction.Details,
                IsBalanceEstimated = transaction.IsBalanceEstimated,
            });
        }
    }

    public class FinancialAccountDto : FinancialAccount
    {
        public Guid? Id { get; set; }
        public string[] Paycards { get; set; }
    }

    public class FinancialAccountTransactionDto : FinancialAccountTransaction
    {
        public Guid? Id { get; set; }
    }
}
