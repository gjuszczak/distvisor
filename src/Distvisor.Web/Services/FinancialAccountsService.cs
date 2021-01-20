using Distvisor.Web.Data;
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
        Task AddAccountAsync(AddFinancialAccountDto account);
        Task AddAccountTransactionAsync(AddFinancialAccountTransactionDto transaction);
        Task<List<FinancialAccountDto>> ListAccountsAsync();
        Task<List<FinancialAccountTransactionDto>> ListAccountTransactionsAsync(Guid accountId);
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

            async Task<long> GetNextSeqNo(ReadStoreContext ctx, Guid accountId)
            {
                var maxSeqNo = await ctx.FinancialAccountTransactions
                    .Where(x => x.AccountId == accountId)
                    .MaxAsync(x => (long?)x.SeqNo);
                return (maxSeqNo ?? 0) + 1;
            }

            transaction.Id = transaction.Id.GenerateIfEmpty();
            transaction.SeqNo = await GetNextSeqNo(_context, transaction.AccountId);
            transaction.TransactionDate = transaction.TransactionDate.Date;
            transaction.PostingDate = transaction.PostingDate.Date;

            await _eventStore.Publish<FinancialAccountTransactionAddedEvent>(transaction);
        }

        public async Task<List<FinancialAccountTransactionDto>> ListAccountTransactionsAsync(Guid accountId)
        {
            var entities = await _context.FinancialAccountTransactions
                .ToListAsync();

            return entities.Select(e => new FinancialAccountTransactionDto
            {
                Id = e.Id,
                AccountId = e.AccountId,
                SeqNo = e.SeqNo,
                TransactionDate = e.TransactionDate,
                PostingDate = e.PostingDate,
                Title = e.Title,
                Amount = e.Amount,
                Balance = e.Balance,
                Source = e.Source,
            })
            .Where(e => e.AccountId == accountId)
            .ToList();
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
