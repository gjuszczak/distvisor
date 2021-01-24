using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using Distvisor.Web.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IFinancialCsvDataExtractor : IFinancialDataExtractor<byte[]>
    {
    }

    public class CsvSVariantDataExtractor : IFinancialCsvDataExtractor
    {
        private readonly Lazy<byte[]> _discriminator;
        private readonly Encoding _encoding;

        public CsvSVariantDataExtractor(IOptions<FinancesConfiguration> config)
        {
            _encoding = Encoding.UTF8;
            _discriminator = new Lazy<byte[]>(() => _encoding.GetBytes(config.Value.CsvIVariantDiscriminator));
        }

        public bool CanExtract(byte[] data)
        {
            return !_discriminator.Value.SequenceEqual(data.Take(_discriminator.Value.Length));
        }

        public async Task<IEnumerable<FinacialExtractedData>> ExtractAsync(byte[] data)
        {
            var csvConfig = new CsvConfiguration(CultureInfo.GetCultureInfo("pl-PL"))
            {
                HasHeaderRecord = false,
                Delimiter = ",",
            };

            var csvContent = _encoding.GetString(data);
            using var reader = new StringReader(csvContent);
            using var csv = new CsvReader(reader, csvConfig);

            await csv.ReadAsync();
            var accNum = csv.GetField(2);
            var transactions = await csv.GetRecordsAsync<CsvSVariantTransactionRecord>().ToListAsync();
            transactions.Reverse();

            return transactions.Select(t => new FinacialExtractedData
            {
                AccountNumber = accNum.Trim().Replace(" ", "").Replace("'", ""),
                TransactionDate = t.TransactionDate,
                PostingDate = t.PostingDate,
                Title = t.Title,
                Amount = t.Amount,
                Balance = t.Balance,
            }); ;
        }

        public class CsvSVariantTransactionRecord
        {
            [Index(0)]
            public DateTime PostingDate { get; set; }

            [Index(1)]
            public DateTime TransactionDate { get; set; }

            [Index(2)]
            public string Title { get; set; }

            [Index(5)]
            public decimal Amount { get; set; }

            [Index(6)]
            public decimal Balance { get; set; }
        }
    }

    public class CsvIVariantDataExtractor : IFinancialCsvDataExtractor
    {
        private readonly Lazy<byte[]> _discriminator;
        private readonly Encoding _encoding;

        public CsvIVariantDataExtractor(IOptions<FinancesConfiguration> config)
        {
            _encoding = Encoding.GetEncoding("windows-1250");
            _discriminator = new Lazy<byte[]>(() => _encoding.GetBytes(config.Value.CsvIVariantDiscriminator));
        }

        public bool CanExtract(byte[] data)
        {
            return _discriminator.Value.SequenceEqual(data.Take(_discriminator.Value.Length));
        }

        public async Task<IEnumerable<FinacialExtractedData>> ExtractAsync(byte[] data)
        {
            var csvConfig = new CsvConfiguration(CultureInfo.GetCultureInfo("pl-PL"))
            {
                HasHeaderRecord = false,
                Delimiter = ";",
                BadDataFound = null,
                MissingFieldFound = null,
                ReadingExceptionOccurred = _ => false,
            };

            var csvContent = _encoding.GetString(data);

            async Task<List<CsvIVariantAccountRecord>> ReadAccountsAsync()
            {
                using var reader = new StringReader(csvContent);
                using var csv = new CsvReader(reader, csvConfig);
                csv.Context.RegisterClassMap<CsvIVariantAccountRecordMap>();
                return await csv.GetRecordsAsync<CsvIVariantAccountRecord>().ToListAsync();
            }

            async Task<List<CsvIVariantTransactionRecord>> ReadTransactionsAsync()
            {
                using var reader = new StringReader(csvContent);
                using var csv = new CsvReader(reader, csvConfig);
                return await csv.GetRecordsAsync<CsvIVariantTransactionRecord>().ToListAsync();
            }

            var accounts = await ReadAccountsAsync();
            var transactions = await ReadTransactionsAsync();
            transactions.Reverse();

            return transactions.Select(t => new FinacialExtractedData
            {
                AccountNumber = (accounts.Find(acc => acc.Name.Contains(t.AccountName))?.Number ?? "").Trim().Replace(" ", ""),
                TransactionDate = t.TransactionDate,
                PostingDate = t.PostingDate,
                Title = t.Title.Trim(),
                Amount = t.Amount,
                Balance = t.Balance,
            });
        }

        public class CsvIVariantAccountRecord
        {
            public string Name { get; set; }
            public string Number { get; set; }
        }

        public class CsvIVariantAccountRecordMap : ClassMap<CsvIVariantAccountRecord>
        {
            public CsvIVariantAccountRecordMap()
            {
                Map(m => m.Name).Index(0).Validate(v => !string.IsNullOrWhiteSpace(v));
                Map(m => m.Number).Index(2).Validate(v => v != null && Regex.IsMatch(v, @"^\d{2}( \d{4}){6}$"));
            }
        }

        public class CsvIVariantTransactionRecord
        {
            [Index(0)]
            [Format("yyyy-MM-dd")]
            public DateTime TransactionDate { get; set; }

            [Index(1)]
            [Format("yyyy-MM-dd")]
            public DateTime PostingDate { get; set; }

            [Index(3)]
            public string Title { get; set; }

            [Index(8)]
            public decimal Amount { get; set; }

            [Index(14)]
            public string AccountName { get; set; }

            [Index(15)]
            public decimal Balance { get; set; }
        }
    }
}
