using Distvisor.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Distvisor.Web.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/sec/[controller]")]
    public class FinancesController : ControllerBase
    {
        private readonly IFinancialService _financialService;

        public FinancesController(IFinancialService financialService)
        {
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

        [HttpGet("summary")]
        public async Task<FinancialSummaryDto> GetSummary()
        {
            return await _financialService.GetSummaryAsync();
        }
    }
}
