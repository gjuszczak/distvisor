namespace Distvisor.Web.Configuration
{
    public class FinancesConfiguration
    {
        public bool IsEmailPoolingEnabled { get; set; }
        public int EmailPoolingMinutesInterval { get; set; }
        public RegexAccountEmailAnalyzerConfig AccountIncomeEmailAnalyzer { get; set; }
        public RegexAccountEmailAnalyzerConfig AccountDebtEmailAnalyzer { get; set; }
        public RegexAccountEmailAnalyzerConfig CardPaymentEmailAnalyzer { get; set; }
        public RegexAccountEmailAnalyzerConfig CardPaymentSettledEmailAnalyzer { get; set; }
    }

    public class RegexAccountEmailAnalyzerConfig
    {
        public string RegexSubjectPattern { get; set; }
        public string RegexBodyPattern { get; set; }
    }
}
