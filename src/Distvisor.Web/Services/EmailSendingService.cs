using System;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IEmailSendingService
    {
        Task SendInvoicePdfAsync(Invoice invoice, byte[] invoicePdf);
    }

    public class EmailSendingService : IEmailSendingService
    {
        private readonly IMailgunClient _client;
        private readonly INotificationService _notifications;

        public EmailSendingService(IMailgunClient mailgunClient, INotificationService notifications)
        {
            _client = mailgunClient;
            _notifications = notifications;
        }

        public async Task SendInvoicePdfAsync(Invoice invoice, byte[] invoicePdf)
        {
            try
            {
                await _client.SendEmailAsync(new MailgunTemplateEmail
                {
                    From = $"Distvisor <noreply@{_client.Config.Domain}>",
                    To = _client.Config.ToAddress,
                    Subject = $"{invoice.Number} za {invoice.IssueDate:MM.yyyy}",
                    Template = "distvisor-invoice",
                    Variables = new
                    {
                        invoice.Number,
                        IssueDate = invoice.IssueDate.ToString("MM.yyyy"),
                    },
                    Attachments = new MailgunAttachment[]
                    {
                        new MailgunAttachment
                        {
                            FileName = $"{invoice.Number.ToLower().Replace(' ','_').Replace('/','_')}_{invoice.IssueDate:dd-MM-yyyy}.pdf",
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
