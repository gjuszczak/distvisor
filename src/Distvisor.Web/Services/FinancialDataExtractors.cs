using Microsoft.AspNetCore.Http;
using MimeKit;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IFinancialDataExtractor<TData>
    {
        bool CanExtract(TData data);
        Task ExtractAsync(TData data);
    }

    public class FinancialEmailDataExtractor : IFinancialDataExtractor<IFormFile>
    {
        private readonly IEnumerable<IFinancialDataExtractor<MimeMessage>> _emailExtractors;

        public FinancialEmailDataExtractor(IEnumerable<IFinancialDataExtractor<MimeMessage>> emailExtractors)
        {
            _emailExtractors = emailExtractors;
        }

        public bool CanExtract(IFormFile data)
        {
            return data.FileName.Contains(".eml");
        }

        public async Task ExtractAsync(IFormFile data)
        {
            using var ds = data.OpenReadStream();
            var emailBody = await MimeMessage.LoadAsync(ds);
            var matchAnalyzer = _emailExtractors.FirstOrDefault(a => a.CanExtract(emailBody));
            if (matchAnalyzer == null)
            {
                return;
            }
            await matchAnalyzer.ExtractAsync(emailBody);
        }
    }

    public class FinancialCsvDataExtractor : IFinancialDataExtractor<IFormFile>
    {
        private readonly IEnumerable<IFinancialDataExtractor<string>> _csvExtractors;

        public FinancialCsvDataExtractor(IEnumerable<IFinancialDataExtractor<string>> csvExtractors)
        {
            _csvExtractors = csvExtractors;
        }

        public bool CanExtract(IFormFile data)
        {
            return data.FileName.Contains(".csv");
        }

        public async Task ExtractAsync(IFormFile data)
        {
            using var ds = data.OpenReadStream();
            using var dsr = new StreamReader(ds);
            
            var dataContent = await dsr.ReadToEndAsync();            
            var matchAnalyzer = _csvExtractors.FirstOrDefault(a => a.CanExtract(dataContent));
            if (matchAnalyzer == null)
            {
                return;
            }
            await matchAnalyzer.ExtractAsync(dataContent);
        }
    }
}
