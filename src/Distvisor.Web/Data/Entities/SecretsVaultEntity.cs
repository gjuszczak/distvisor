using System.ComponentModel.DataAnnotations;

namespace Distvisor.Web.Data.Entities
{
    public class SecretsVaultEntity
    {
        [Key]
        public SecretKey Key { get; set; }
        public string Value { get; set; }
    }

    public enum SecretKey
    {
        AccountingInvoicesApiKey,
        AccountingSubscriberApiKey,
        AccountingUser,
        MailgunApiKey,
        MailgunDomain,
        MailgunToAddress,
        GithubApiKey,
        GithubRepoOwner,
        GithubRepoName,
    }
}
