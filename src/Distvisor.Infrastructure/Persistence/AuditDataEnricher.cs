using Distvisor.App.Core.Entities;
using Distvisor.App.Core.Services;
using Microsoft.EntityFrameworkCore;
using System;

namespace Distvisor.Infrastructure.Persistence
{
    public class AuditDataEnricher : IAuditDataEnricher
    {
        private readonly ICorrelationIdProvider _correlationIdProvider;

        public AuditDataEnricher(ICorrelationIdProvider correlationIdProvider)
        {
            _correlationIdProvider = correlationIdProvider;
        }

        public void Enrich(DbContext context)
        {
            foreach (var entry in context.ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _correlationIdProvider.GetCorrelationId().ToString();
                        entry.Entity.Created = DateTimeOffset.Now;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = _correlationIdProvider.GetCorrelationId().ToString();
                        entry.Entity.LastModified = DateTimeOffset.Now;
                        break;
                }
            }
        }
    }
}
