using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

namespace Distvisor.Web.Data.Reads.Core
{
    public interface IReadStoreTransactionProvider
    {
        Task<IDbContextTransaction> BeginTransactionAsync();
    }

    public class ReadStoreTransactionProvider : IReadStoreTransactionProvider
    {
        private readonly ReadStoreContext _db;

        public ReadStoreTransactionProvider(ReadStoreContext db)
        {
            _db = db;
        }

        public Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return _db.Database.BeginTransactionAsync();
        }
    }
}
