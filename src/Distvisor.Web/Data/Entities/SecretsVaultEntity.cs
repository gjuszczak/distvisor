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
        GithubApiKey,
        GithubRepoName,
        GithubRepoOwner,
        MailgunApiKey,
        MailgunDomain,
        MailgunToAddress,
        MicrosoftAppClientId,
        MicrosoftAppSecret,
        MicrosoftAuthRedirectUri,
    }
}
