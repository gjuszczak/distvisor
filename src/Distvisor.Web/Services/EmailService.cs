using System;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IEmailService
    {
        Task SendInvoicePdfAsync(byte[] invoicePdf);
    }

    public class EmailService : IEmailService
    {
        private readonly IMailgunClient _mailgunClient;
        private readonly MailgunSecrets _secrets;
        private readonly INotificationService _notifications;

        public EmailService(ISecretsVault keyVault, IMailgunClient mailgunClient, INotificationService notifications)
        {
            _secrets = keyVault.GetMailgunSecrets();
            _mailgunClient = mailgunClient;
            _mailgunClient.Configure(_secrets.Domain, _secrets.ApiKey);
            _notifications = notifications;
        }

        public async Task SendInvoicePdfAsync(byte[] invoicePdf)
        {
            try
            {
                await _mailgunClient.SendEmailAsync(new MailgunEmail
                {
                    From = $"Distvisor <noreply@{_secrets.Domain}>",
                    To = _secrets.ToAddress,
                    Subject = "Faktura od Distvisior",
                    Text = "Hejka.Faktura w zalaczniku.",
                    Attachments = new MailgunAttachment[]
                    {
                        new MailgunAttachment
                        {
                            FileName = "faktura.pdf",
                            ContentType = "application/pdf",
                            Bytes = invoicePdf
                        }
                    }
                });

                await _notifications.PushSuccessAsync("Email sent successfully");
            }
            catch (Exception exc)
            {
                await _notifications.PushErrorAsync("Unable to sent email", exc);
            }
        }
    }
}
