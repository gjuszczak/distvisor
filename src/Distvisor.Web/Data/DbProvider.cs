using LiteDB;
using Microsoft.Extensions.Configuration;

namespace Distvisor.Web.Data
{
    public interface IDbProvider
    {
        ILiteDatabase EventStoreDatabase { get; }
        ILiteDatabase ReadStoreDatabase { get; }
    }

    public class DbProvider : IDbProvider
    {
        public DbProvider(IConfiguration configuration)
        {
            var eventDbPath = configuration.GetConnectionString("EventStore");
            var readDbPath = configuration.GetConnectionString("ReadStore");
            EventStoreDatabase = new LiteDatabase(eventDbPath);
            ReadStoreDatabase = new LiteDatabase(readDbPath);
        }
        public ILiteDatabase EventStoreDatabase { get; }
        public ILiteDatabase ReadStoreDatabase { get; }
    }
}
