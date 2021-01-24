using Microsoft.AspNetCore.Http;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IFinancialDataExtractor<TData>
    {
        bool CanExtract(TData data);
        Task<IEnumerable<FinacialExtractedData>> ExtractAsync(TData data);
    }

    public interface IFinancialDataExtractor : IFinancialDataExtractor<IFormFile>
    {
    }

    public class FinancialEmailDataExtractor : IFinancialDataExtractor
    {
        private readonly IEnumerable<IFinancialEmailDataExtractor> _emailExtractors;

        public FinancialEmailDataExtractor(IEnumerable<IFinancialEmailDataExtractor> emailExtractors)
        {
            _emailExtractors = emailExtractors;
        }

        public bool CanExtract(IFormFile data)
        {
            return data.FileName.Contains(".eml");
        }

        public async Task<IEnumerable<FinacialExtractedData>> ExtractAsync(IFormFile data)
        {
            using var ds = data.OpenReadStream();
            var emailBody = await MimeMessage.LoadAsync(ds);
            var matchAnalyzer = _emailExtractors.FirstOrDefault(a => a.CanExtract(emailBody));
            if (matchAnalyzer == null)
            {
                return Array.Empty<FinacialExtractedData>();
            }
            return await matchAnalyzer.ExtractAsync(emailBody);
        }
    }

    public class FinancialCsvDataExtractor : IFinancialDataExtractor
    {
        private readonly IEnumerable<IFinancialCsvDataExtractor> _csvExtractors;

        public FinancialCsvDataExtractor(IEnumerable<IFinancialCsvDataExtractor> csvExtractors)
        {
            _csvExtractors = csvExtractors;
        }

        public bool CanExtract(IFormFile data)
        {
            return data.FileName.Contains(".csv");
        }

        public async Task<IEnumerable<FinacialExtractedData>> ExtractAsync(IFormFile data)
        {
            using var ds = data.OpenReadStream();
            using var mds = new MemoryStream();
            await ds.CopyToAsync(mds);
            var dataContent = mds.ToArray();
            var matchAnalyzer = _csvExtractors.FirstOrDefault(a => a.CanExtract(dataContent));
            if (matchAnalyzer == null)
            {
                return Array.Empty<FinacialExtractedData>(); ;
            }
            return await matchAnalyzer.ExtractAsync(dataContent);
        }
    }

    public class FinacialExtractedData
    {
        public string AccountNumber { get; set; }
        public DateTime PostingDate { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Title { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
    }
}
