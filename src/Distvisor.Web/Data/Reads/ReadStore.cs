using Distvisor.Web.Data.Entities;
using LiteDB;
using System;

namespace Distvisor.Web.Data.Reads
{
    public class ReadStore
    {
        public ReadStore(IDbProvider dbProvider)
        {
            Database = dbProvider.ReadStoreDatabase;
        }

        public ILiteDatabase Database { get; }

        public ILiteCollection<UserEntity> Users => Database.GetCollection<UserEntity>();
        public ILiteCollection<OAuthTokenEntity> OAuthTokens => Database.GetCollection<OAuthTokenEntity>();
        public ILiteCollection<NotificationEntity> Notifications => Database.GetCollection<NotificationEntity>();
        public ILiteCollection<SecretsVaultEntity> SecretsVault => Database.GetCollection<SecretsVaultEntity>();
        public ILiteCollection<RedirectionEntity> Redirections => Database.GetCollection<RedirectionEntity>();
    }
}
