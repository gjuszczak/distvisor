using Distvisor.Web.Data;
using Distvisor.Web.Data.Events;
using Distvisor.Web.Data.Events.Core;
using Distvisor.Web.Data.Reads.Core;
using Distvisor.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IFinancialService
    {
        Task AddAccountAsync(AddFinancialAccountDto account);
        Task AddAccountTransactionAsync(AddFinancialAccountTransactionDto transaction);
        Task<List<FinancialAccountDto>> ListAccountsAsync();
        Task<List<FinancialAccountTransactionDto>> ListAccountTransactionsAsync(Guid accountId);
        Task ImportFilesAsync(IEnumerable<IFormFile> files);
        Task<FinancialSummaryDto> GetSummaryAsync();
    }

    public class FinancialService : IFinancialService
    {
        private readonly IEventStore _eventStore;
        private readonly ReadStoreContext _context;
        private readonly INotificationService _notifications;
        private readonly IEnumerable<IFinancialDataExtractor> _dataExtractors;
        private readonly ICryptoService _cryptoService;

        public FinancialService(
            IEventStore eventStore,
            ReadStoreContext context,
            INotificationService notifiactions,
            IEnumerable<IFinancialDataExtractor> dataExtractors,
            ICryptoService cryptoService)
        {
            _eventStore = eventStore;
            _context = context;
            _notifications = notifiactions;
            _dataExtractors = dataExtractors;
            _cryptoService = cryptoService;
        }

        public async Task AddAccountAsync(AddFinancialAccountDto account)
        {
            await _notifications.PushSuccessAsync("Account added successfully.");

            account.Id = account.Id.GenerateIfEmpty();
            account.Number = Regex.Replace(account.Number, @"\s+", "");
            account.CreatedDateTimeUtc = DateTime.UtcNow;

            await _eventStore.Publish<FinancialAccountAddedEvent>(account);
        }

        public async Task<List<FinancialAccountDto>> ListAccountsAsync()
        {
            var entities = await _context.FinancialAccounts
                .OrderBy(e => e.CreatedDateTimeUtc)
                .ToListAsync();

            return entities.Select(e => new FinancialAccountDto
            {
                Id = e.Id,
                Name = e.Name,
                Number = e.Number,
            }).ToList();
        }

        public async Task AddAccountTransactionAsync(AddFinancialAccountTransactionDto transaction)
        {
            await _notifications.PushSuccessAsync("Transaction added successfully.");

            transaction.Id = transaction.Id.GenerateIfEmpty();
            transaction.SeqNo = await GetAccountNextSeqNo(transaction.AccountId);
            transaction.TransactionDate = transaction.TransactionDate.Date;
            transaction.PostingDate = transaction.PostingDate.Date;

            transaction.TransactionHash = GetTransactionHash(transaction);

            await _eventStore.Publish<FinancialAccountTransactionAddedEvent>(transaction);
        }

        public async Task<List<FinancialAccountTransactionDto>> ListAccountTransactionsAsync(Guid accountId)
        {
            var entities = await _context.FinancialAccountTransactions
                .Where(e => e.AccountId == accountId)
                .OrderByDescending(e => e.TransactionDate).ThenByDescending(e => e.SeqNo)
                .ToListAsync();

            return entities.Select(e => new FinancialAccountTransactionDto
            {
                Id = e.Id,
                AccountId = e.AccountId,
                SeqNo = e.SeqNo,
                TransactionDate = e.TransactionDate,
                PostingDate = e.PostingDate,
                Title = e.Title,
                Amount = e.Amount,
                Balance = e.Balance,
                Source = e.Source,
            })
            .ToList();
        }

        public async Task ImportFilesAsync(IEnumerable<IFormFile> files)
        {
            var importedFiles = new List<IFormFile>();

            foreach (var f in files)
            {
                try
                {
                    var extractor = _dataExtractors.FirstOrDefault(a => a.CanExtract(f));
                    if (extractor == null)
                    {
                        throw new InvalidOperationException("File not supporeted");
                    }
                    var data = await extractor.ExtractAsync(f);
                    var dataGroupped = data.GroupBy(x => x.AccountNumber);

                    var transactions = new List<FinancialAccountTransaction>();
                    foreach (var g in dataGroupped)
                    {
                        var account = await _context.FinancialAccounts.FirstOrDefaultAsync(x => x.Number == g.Key);

                        if (account == null)
                        {
                            throw new InvalidOperationException($"Account number: {g.Key} not found.");
                        }

                        var nextSeq = await GetAccountNextSeqNo(account.Id);

                        var trans = g.Select((tran, i) => new FinancialAccountTransaction
                        {
                            Id = Guid.NewGuid(),
                            AccountId = account.Id,
                            SeqNo = nextSeq + i,
                            TransactionDate = tran.TransactionDate,
                            PostingDate = tran.PostingDate,
                            Source = FinancialAccountTransactionSource.UserFileImport,
                            Title = tran.Title,
                            Amount = tran.Amount,
                            Balance = tran.Balance,
                        }).ToList();

                        trans.ForEach(t => t.TransactionHash = GetTransactionHash(t));

                        transactions.AddRange(trans);
                    }

                    await _eventStore.Publish(new FinancialDataImportedEvent
                    {
                        Transactions = transactions.ToArray()
                    });

                    importedFiles.Add(f);
                }
                catch (Exception exc)
                {
                    await _notifications.PushErrorAsync($"Unable to import file {f?.Name}.", exc?.InnerException ?? exc);
                }
            }

            if (importedFiles.Any())
            {
                await _notifications.PushSuccessAsync($"{importedFiles.Count}/{files.Count()} files imported successfully.");
            }
        }

        public async Task<FinancialSummaryDto> GetSummaryAsync()
        {
            var startDate = new DateTime(2019, 1, 1);
            var endDate = DateTime.Now; //new DateTime(2021, 1, 1);

            var lastTranPerDay = _context.FinancialAccountTransactions
                .Where(tran => tran.PostingDate >= startDate && tran.PostingDate < endDate)
                .GroupBy(tran => new { tran.AccountId, tran.PostingDate })
                .Select(g => new { g.Key.AccountId, SeqNo = g.Max(tran => tran.SeqNo) });

            var balancePerDay = _context.FinancialAccountTransactions
                .Join(lastTranPerDay,
                t1 => new { t1.AccountId, t1.SeqNo },
                t2 => new { t2.AccountId, t2.SeqNo },
                (t1, t2) => new { t1.Account.CreatedDateTimeUtc, t1.Account.Name, t1.PostingDate, t1.Balance })
                .OrderBy(t => t.CreatedDateTimeUtc).ThenBy(t => t.PostingDate);

            var result = await balancePerDay.ToListAsync();

            var dateSeries = Enumerable.Range(0, 1 + endDate.Subtract(startDate).Days)
                .Select(offset => startDate.AddDays(offset))
                .ToList();

            var dataSets = result.GroupBy(r => r.Name).Select(g =>
            {
                var dataValues = new List<decimal?>();
                var gEnumerator = g.GetEnumerator();
                var isEnd = !gEnumerator.MoveNext();
                decimal? previous = null;
                foreach (var d in dateSeries)
                {
                    if (gEnumerator.Current?.PostingDate.Date == d.Date)
                    {
                        dataValues.Add(gEnumerator.Current.Balance);
                        previous = gEnumerator.Current.Balance;
                        isEnd = !gEnumerator.MoveNext();
                    }
                    else
                    {
                        dataValues.Add(isEnd && previous == 0 ? null : previous);
                    }
                }

                return new FinancialSummaryDataSetDto
                {
                    Label = g.Key,
                    Data = dataValues.ToArray(),
                };
            }).ToArray();

            var summary = new FinancialSummaryDto
            {
                LineChart = new FinancialSummaryLineChartDto
                {
                    Labels = dateSeries.Select(d => d.ToString("d")).ToArray(),
                    DataSets = dataSets,
                }
            };

            return summary;
        }

        private async Task<long> GetAccountNextSeqNo(Guid accountId)
        {
            var maxSeqNo = await _context.FinancialAccountTransactions
                .Where(x => x.AccountId == accountId)
                .MaxAsync(x => (long?)x.SeqNo);
            return (maxSeqNo ?? 0) + 1;
        }

        private string GetTransactionHash(FinancialAccountTransaction tran)
        {
            var tranString = FormattableString.Invariant($"{tran.TransactionDate:dd.MM.yyyy}{tran.PostingDate:dd.MM.yyyy}{tran.Title}{tran.Amount}{tran.Balance}");
            return _cryptoService.GetHashString(tranString);
        }
    }

    public class AddFinancialAccountDto : FinancialAccountAddedEvent
    {
    }

    public class FinancialAccountDto : FinancialAccount
    {
    }

    public class AddFinancialAccountTransactionDto : FinancialAccountTransactionAddedEvent
    {
    }

    public class FinancialAccountTransactionDto : FinancialAccountTransaction
    {
    }

    public class FinancialSummaryDto
    {
        public FinancialSummaryLineChartDto LineChart { get; set; }
    }

    public class FinancialSummaryLineChartDto
    {
        public IEnumerable<string> Labels { get; set; }
        public IEnumerable<FinancialSummaryDataSetDto> DataSets { get; set; }
    }

    public class FinancialSummaryDataSetDto
    {
        public string Label { get; set; }
        public IEnumerable<decimal?> Data { get; set; }
    }
}
