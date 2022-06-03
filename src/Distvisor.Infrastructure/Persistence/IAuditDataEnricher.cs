using Microsoft.EntityFrameworkCore;

namespace Distvisor.Infrastructure.Persistence
{
    public interface IAuditDataEnricher
    {
        void Enrich(DbContext context);
    }
}