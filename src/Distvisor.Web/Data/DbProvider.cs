using LiteDB;
using Microsoft.Extensions.Configuration;

namespace Distvisor.Web.Data
{
    public interface IDbProvider
    {
        ILiteDatabase ReadStoreDatabase { get; }
    }

    public class DbProvider : IDbProvider
    {
        public DbProvider(IConfiguration configuration)
        {
            var readDbPath = configuration.GetConnectionString("ReadStore");
            ReadStoreDatabase = new LiteDatabase(readDbPath);
        }
        public ILiteDatabase ReadStoreDatabase { get; }
    }
}
