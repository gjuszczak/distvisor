using Distvisor.Web.Data;
using Distvisor.Web.Data.Events;
using Distvisor.Web.Data.Events.Core;
using Distvisor.Web.Data.Reads.Core;
using Distvisor.Web.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IFinancialAccountsService
    {
        Task AddAccountAsync(AddFinancialAccountDto account);
        Task AddAccountTransactionAsync(AddFinancialAccountTransactionDto transaction);
        Task<List<FinancialAccountDto>> ListAccountsAsync();
        Task<List<FinancialAccountTransactionDto>> ListAccountTransactionsAsync();
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

        public async Task AddAccountAsync(AddFinancialAccountDto account)
        {
            await _notifications.PushSuccessAsync("Account added successfully.");

            account.Id = account.Id.GenerateIfEmpty();

            await _eventStore.Publish<FinancialAccountAddedEvent>(account);
        }

        public async Task<List<FinancialAccountDto>> ListAccountsAsync()
        {
            var entities = await _context.FinancialAccounts
                .Include(x => x.Paycards)
                .ToListAsync();

            return entities.Select(e => new FinancialAccountDto
            {
                Id = e.Id,
                Name = e.Name,
                Number = e.Number,
                Paycards = e.Paycards.Select(p => p.Name).ToArray()
            }).ToList();
        }

        public async Task AddAccountTransactionAsync(AddFinancialAccountTransactionDto transaction)
        {
            await _notifications.PushSuccessAsync("Transaction added successfully.");

            transaction.Id = transaction.Id.GenerateIfEmpty();

            await _eventStore.Publish<FinancialAccountTransactionAddedEvent>(transaction);
        }

        public async Task<List<FinancialAccountTransactionDto>> ListAccountTransactionsAsync()
        {
            var entities = await _context.FinancialAccountTransactions
                .ToListAsync();

            return entities.Select(e => new FinancialAccountTransactionDto
            {
                Id = e.Id,
                AccountId = e.AccountId,
                Amount = e.Amount,
                Balance = e.Balance,
                DataSource = e.DataSource,
                Date = e.Date,
                Details = e.Details,
                IsBalanceEstimated = e.IsBalanceEstimated
            }).ToList();
        }
    }

    public class AddFinancialAccountDto : FinancialAccountAddedEvent
    {
    }

    public class FinancialAccountDto : FinancialAccount
    {
        public string[] Paycards { get; set; }
    }

    public class AddFinancialAccountTransactionDto : FinancialAccountTransactionAddedEvent
    {
    }

    public class FinancialAccountTransactionDto : FinancialAccountTransaction
    {
    }
}
