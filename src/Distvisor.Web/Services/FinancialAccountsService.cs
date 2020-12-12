﻿using Distvisor.Web.Data.Events;
using Distvisor.Web.Data.Events.Core;
using Distvisor.Web.Data.Reads.Core;
using Distvisor.Web.Data.Reads.Entities;
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
        private readonly INotificationService _notifications;

        public FinancialAccountsService(IEventStore eventStore, ReadStoreContext context, INotificationService notifiactions)
        {
            _eventStore = eventStore;
            _context = context;
            _notifications = notifiactions;
        }

        public async Task AddAccountAsync(FinancialAccount account)
        {
            await _notifications.PushSuccessAsync("Account added successfully.");

            await _eventStore.Publish(new AddFinancialAccountEvent
            {
                Name = account.Name,
                Number = account.Number,
                Paycards = account.Paycards,
                Type = account.Type
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
        public FinancialAccountType Type { get; set; }
    }
}
