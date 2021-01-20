using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public class CsvSVariantDataExtractor : IFinancialDataExtractor<string>
    {
        public bool CanExtract(string data)
        {
            return false;
        }

        public Task ExtractAsync(string data)
        {
            return Task.CompletedTask;
        }
    }

    public class CsvIVariantDataExtractor : IFinancialDataExtractor<string>
    {
        public bool CanExtract(string data)
        {
            return false;
        }

        public Task ExtractAsync(string data)
        {
            return Task.CompletedTask;
        }
    }
}
