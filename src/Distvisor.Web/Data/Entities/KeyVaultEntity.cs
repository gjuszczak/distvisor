namespace Distvisor.Web.Data.Entities
{
    public class KeyVaultEntity
    {
        public KeyType Id { get; set; }
        public string KeyValue { get; set; }
    }

    public enum KeyType
    {
        GithubApiKey,
        IFirmaApiKey,
        MailgunApiKey,
    }
}
