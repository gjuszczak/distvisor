using System;

namespace Distvisor.Web.Data.Entities
{
    public class SecretsVaultEntity
    {
        public Guid Id { get; set; }
        public SecretKey Key { get; set; }
        public string Value { get; set; }
    }

    public enum SecretKey
    {
        AccountingInvoicesApiKey,
        AccountingSubscriberApiKey,
        AccountingUser,
    }
}
