using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IInvoicesService
    {
        Task GenerateInvoiceAsync(string templateInvoiceId, DateTime issueDate, int quantity);
        Task<Invoice> GetInvoiceAsync(string invoiceId);
        Task<byte[]> GetInvoicePdfAsync(string invoiceId);
        Task<IEnumerable<Invoice>> GetInvoicesAsync();
    }

    public class InvoicesService : IInvoicesService
    {
        private readonly AccountingSecrets _secrets;
        private readonly IIFirmaClient _client;
        private readonly INotificationService _notifications;

        public InvoicesService(ISecretsVault keyVault, IIFirmaClient client, INotificationService notifications)
        {
            _secrets = keyVault.GetAccountingSecrets();
            _notifications = notifications;
            _client = client;
            _client.Configure(_secrets.InvoicesApiKey, _secrets.SubscriberApiKey, _secrets.User);
        }

        public async Task<IEnumerable<Invoice>> GetInvoicesAsync()
        {
            try
            {
                return await _client.GetInvoicesAsync();
            }
            catch (Exception exc)
            {
                await _notifications.PushErrorAsync("Unable to get invoices from remote api", exc);
                throw;
            }
        }

        public async Task<Invoice> GetInvoiceAsync(string invoiceId)
        {
            try
            {
                return await _client.GetInvoiceAsync(invoiceId);
            }
            catch (Exception exc)
            {
                await _notifications.PushErrorAsync("Unable to get invoice details from remote api", exc);
                throw;
            }
        }

        public async Task<byte[]> GetInvoicePdfAsync(string invoiceId)
        {
            try
            {
                return await _client.GetInvoicePdfAsync(invoiceId);
            }
            catch (Exception exc)
            {
                await _notifications.PushErrorAsync("Unable to get invoice pdf from remote api", exc);
                throw;
            }
        }

        public async Task GenerateInvoiceAsync(string templateInvoiceId, DateTime issueDate, int quantity)
        {
            try
            {
                await _client.GenerateInvoiceAsync(templateInvoiceId, issueDate, quantity);
            }
            catch (Exception exc)
            {
                await _notifications.PushErrorAsync("Unable to generate invoice", exc);
                throw;
            }
        }
    }

    public class Invoice
    {
        public string Id { get; set; }
        public string Number { get; set; }
        public string Customer { get; set; }
        public DateTime IssueDate { get; set; }
        public int WorkDays { get; set; }
        public decimal Amount { get; set; }
    }
}
