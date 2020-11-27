namespace Distvisor.Web.Configuration
{
    public class FinancesConfiguration
    {
        public RegexAccountEmailAnalyzerConfig AccountIncomeEmailAnalyzer { get; set; }
        public RegexAccountEmailAnalyzerConfig AccountDebtEmailAnalyzer { get; set; }
        public RegexAccountEmailAnalyzerConfig CardPaymentEmailAnalyzer { get; set; }
    }

    public class RegexAccountEmailAnalyzerConfig
    {
        public string RegexSubjectPattern { get; set; }
        public string RegexBodyPattern { get; set; }
    }
}
