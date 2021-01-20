using Distvisor.Web.BackgroundServices;
using Distvisor.Web.Data.Events;
using Distvisor.Web.Data.Events.Core;
using Distvisor.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distvisor.Web.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class FinancesController : ControllerBase
    {
        private readonly IMailgunClient _mailgun;
        private readonly IEventStore _eventStore;
        private readonly IEmailReceivedNotifier _emailReceivedNotifier;
        private readonly IFinancialFileImportService _emailFileImportService;
        private readonly IFinancialAccountsService _financialAccountsService;

        public FinancesController(IMailgunClient mailgun,
            IEventStore eventStore,
            IEmailReceivedNotifier emailReceivedNotifier,
            IFinancialFileImportService emailFileImportService,
            IFinancialAccountsService financialAccountsService)
        {
            _mailgun = mailgun;
            _eventStore = eventStore;
            _emailReceivedNotifier = emailReceivedNotifier;
            _emailFileImportService = emailFileImportService;
            _financialAccountsService = financialAccountsService;
        }

        [HttpGet]
        public async Task<string> GetEmail()
        {
            var l = await _mailgun.ListStoredEmailsAsync();
            var sl = l.ToArray().Last();
            var r = await _mailgun.GetStoredEmailContentAsync(sl.Url);

            await _eventStore.Publish(new EmailReceivedEvent
            {
                MimeMessageId = sl.MimeMessageId,
                BodyMime = r
            });

            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(r));
            var msg = await MimeMessage.LoadAsync(stream);

            return r;
        }

        [HttpPost("notify")]
        [AllowAnonymous]
        public async Task Notify()
        {
            await _emailReceivedNotifier.NotifyAsync(new EmailReceivedNotification { Key = "notify" });
        }

        [HttpPost("import-file")]
        public async Task ImportFile(IEnumerable<IFormFile> files)
        {
            await _emailFileImportService.ImportFilesAsync(files);
        }

        [HttpPost("accounts/add")]
        public async Task AddAccount([FromBody] AddFinancialAccountDto dto)
        {
            await _financialAccountsService.AddAccountAsync(dto);
        }

        [HttpGet("accounts/list")]
        public async Task<List<FinancialAccountDto>> ListAccounts()
        {
            return await _financialAccountsService.ListAccountsAsync();
        }

        [HttpPost("accounts/transactions/add")]
        public async Task AddAccountTransaction([FromBody] AddFinancialAccountTransactionDto dto)
        {
            await _financialAccountsService.AddAccountTransactionAsync(dto);
        }

        [HttpGet("accounts/transactions/list")]
        public async Task<List<FinancialAccountTransactionDto>> ListTransactions(Guid accountId)
        {
            return await _financialAccountsService.ListAccountTransactionsAsync(accountId);
        }
    }
}
