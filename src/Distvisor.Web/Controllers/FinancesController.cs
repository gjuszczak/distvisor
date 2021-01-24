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
        private readonly IFinancialService _financialService;

        public FinancesController(IMailgunClient mailgun,
            IEventStore eventStore,
            IEmailReceivedNotifier emailReceivedNotifier,
            IFinancialService financialService)
        {
            _mailgun = mailgun;
            _eventStore = eventStore;
            _emailReceivedNotifier = emailReceivedNotifier;
            _financialService = financialService;
        }

        [HttpPost("import-files")]
        public async Task ImportFiles(IEnumerable<IFormFile> files)
        {
            await _financialService.ImportFilesAsync(files);
        }

        [HttpPost("accounts/add")]
        public async Task AddAccount([FromBody] AddFinancialAccountDto dto)
        {
            await _financialService.AddAccountAsync(dto);
        }

        [HttpGet("accounts/list")]
        public async Task<List<FinancialAccountDto>> ListAccounts()
        {
            return await _financialService.ListAccountsAsync();
        }

        [HttpPost("accounts/transactions/add")]
        public async Task AddAccountTransaction([FromBody] AddFinancialAccountTransactionDto dto)
        {
            await _financialService.AddAccountTransactionAsync(dto);
        }

        [HttpGet("accounts/transactions/list")]
        public async Task<List<FinancialAccountTransactionDto>> ListTransactions(Guid accountId)
        {
            return await _financialService.ListAccountTransactionsAsync(accountId);
        }
    }
}
