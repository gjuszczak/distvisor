using Distvisor.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateInvoice([FromBody]GenerateInvoiceDto dto)
        {
            await _invoices.GenerateInvoiceAsync(dto.TemplateInvoiceId, dto.UtcIssueDate.ToLocalTime(), dto.Workdays);
            return Ok();
        }

        [HttpPost("{invoiceId}/send-mail")]
        public async Task SendMailInvoicePdf(string invoiceId)
        {
            var invoicePdf = await _invoices.GetInvoicePdfAsync(invoiceId);
            await _mailService.SendInvoicePdfAsync(invoicePdf);
        }
    }

    public class GenerateInvoiceDto
    {
        [Required]
        public string TemplateInvoiceId { get; set; }

        [Required]
        public DateTime UtcIssueDate { get; set; }

        [Required]
        [Range(1, 31)]
        public int Workdays { get; set; }
    }

}