namespace Distvisor.Web.Configuration
{
    public class FinancesConfiguration
    {
        public bool IsEmailPoolingEnabled { get; set; }
        public int EmailPoolingMinutesInterval { get; set; }
        public string CsvIVariantDiscriminator { get; set; }
        public RegexFinancialEmailDataExtractorConfig AccountIncomeEmailDataExtractor { get; set; }
        public RegexFinancialEmailDataExtractorConfig AccountDebtEmailDataExtractor { get; set; }
        public RegexFinancialEmailDataExtractorConfig CardPaymentEmailDataExtractor { get; set; }
        public RegexFinancialEmailDataExtractorConfig CardPaymentSettledEmailDataExtractor { get; set; }
    }

    public class RegexFinancialEmailDataExtractorConfig
    {
        public string RegexSubjectPattern { get; set; }
        public string RegexBodyPattern { get; set; }
    }
}
