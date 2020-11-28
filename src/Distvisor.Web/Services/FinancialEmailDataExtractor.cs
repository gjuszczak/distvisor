using MimeKit;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IFinancialEmailDataExtractor
    {
        Task ExtractAsync(string emailBodyMime);
    }

    public class FinancialEmailDataExtractor : IFinancialEmailDataExtractor
    {
        private readonly IEnumerable<IFinancialEmailAnalyzer> _analyzers;

        public FinancialEmailDataExtractor(IEnumerable<IFinancialEmailAnalyzer> analyzers)
        {
            _analyzers = analyzers;
        }

        public async Task ExtractAsync(string emailBodyMime)
        {
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(emailBodyMime));
            var emailBody = await MimeMessage.LoadAsync(stream);
            var matchAnalyzer = _analyzers.FirstOrDefault(a => a.CanAnalyze(emailBody));
            if (matchAnalyzer == null)
            {
                return;
            }
            var analysis = matchAnalyzer.Analyze(emailBody);
        }
    }
}
