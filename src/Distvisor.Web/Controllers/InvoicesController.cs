using Distvisor.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Distvisor.Web.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoicesService _invoices;
        private readonly IMailService _mailService;

        public InvoicesController(IInvoicesService invoices, IMailService mailService)
        {
            _invoices = invoices;
            _mailService = mailService;
        }

        [HttpGet("list")]
        public Task<IEnumerable<Invoice>> ListInvoices()
        {
            return _invoices.GetInvoicesAsync();
        }

        [HttpGet("{invoiceId}")]
        [ProducesResponseType(typeof(FileContentResult), 200)]
        [Produces("application/pdf")]
        public async Task<IActionResult> GetInvoicePdf(string invoiceId)
        {
            var pdf = await _invoices.GetInvoicePdfAsync(invoiceId);
            return File(pdf, "application/pdf");
        }

        [HttpPost("{invoiceId}/send-mail")]
        public async Task SendMailInvoicePdf(string invoiceId)
        {
            var invoicePdf = await _invoices.GetInvoicePdfAsync(invoiceId);
            await _mailService.SendInvoicePdfAsync(invoicePdf);
        }
    }
}