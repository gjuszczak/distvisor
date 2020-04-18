using System;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IMailingService
    {
        Task SendInvoicePdfAsync(byte[] invoicePdf);
    }

    public class MailingService : IMailingService
    {
        private readonly IMailgunClient _client;
        private readonly INotificationService _notifications;

        public MailingService(IMailgunClient mailgunClient, INotificationService notifications)
        {
            _client = mailgunClient;
            _notifications = notifications;
        }

        public async Task SendInvoicePdfAsync(byte[] invoicePdf)
        {
            try
            {
                await _client.SendEmailAsync(new MailgunEmail
                {
                    From = $"Distvisor <noreply@{_client.Config.Domain}>",
                    To = _client.Config.ToAddress,
                    Subject = "Faktura od Distvisior",
                    Text = "Hejka. Faktura w zalaczniku.",
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
