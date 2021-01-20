using Distvisor.Web.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public class FinancialEmailAnalysis
    {
        public string AccountNumber { get; set; }
        public decimal Amount { get; set; }
        public decimal? Balance { get; set; }
        public DateTimeOffset MessageUtcDateTime { get; set; }
        public DateTimeOffset TransactionUtcDate { get; set; }
        public string Paycard { get; set; }
        public string Details { get; set; }

    }

    public class RegexAccountEmailAnalyzer : IFinancialDataExtractor<MimeMessage>
    {
        protected virtual RegexAccountEmailAnalyzerConfig Config { get; }

        public RegexAccountEmailAnalyzer(RegexAccountEmailAnalyzerConfig config)
        {
            Config = config;
        }

        public bool CanExtract(MimeMessage data)
        {
            return Regex.IsMatch(data.Subject ?? "", Config.RegexSubjectPattern) &&
                Regex.IsMatch(data.HtmlBody ?? "", Config.RegexBodyPattern);
        }

        public Task ExtractAsync(MimeMessage data)
        {
            var bodyMatch = new Lazy<Match>(() => Regex.Match(data.HtmlBody, Config.RegexBodyPattern));
            var subjectMatch = new Lazy<Match>(() => Regex.Match(data.Subject, Config.RegexSubjectPattern));

            new FinancialEmailAnalysis
            {
                AccountNumber = GetAccountNumber(data, bodyMatch, subjectMatch),
                Amount = GetAmount(data, bodyMatch, subjectMatch),
                Balance = GetBalance(data, bodyMatch, subjectMatch),
                TransactionUtcDate = GetTransactionUtcDate(data, bodyMatch, subjectMatch),
                MessageUtcDateTime = GetMessageUtcDateTime(data, bodyMatch, subjectMatch),
                Paycard = GetPaycard(data, bodyMatch, subjectMatch),
                Details = GetDetails(data, bodyMatch, subjectMatch),
            };

            return Task.CompletedTask;
        }

        protected virtual string GetAccountNumber(MimeMessage data, Lazy<Match> bodyMatch, Lazy<Match> subjectMatch) =>
            bodyMatch.Value.Groups["accnum"].Value.Trim().Replace(" ", string.Empty);

        protected virtual decimal GetAmount(MimeMessage data, Lazy<Match> bodyMatch, Lazy<Match> subjectMatch) =>
            decimal.Parse(bodyMatch.Value.Groups["amount"].Value.Replace(" ", string.Empty).Replace(",", "."), CultureInfo.InvariantCulture);

        protected virtual decimal? GetBalance(MimeMessage data, Lazy<Match> bodyMatch, Lazy<Match> subjectMatch) =>
            decimal.Parse(bodyMatch.Value.Groups["balance"].Value.Replace(" ", string.Empty).Replace(",", "."), CultureInfo.InvariantCulture);

        protected virtual DateTimeOffset GetTransactionUtcDate(MimeMessage data, Lazy<Match> bodyMatch, Lazy<Match> subjectMatch) =>
            DateTimeOffset.ParseExact(bodyMatch.Value.Groups["date"].Value, "dd-MM-yyyy", CultureInfo.InvariantCulture);

        protected virtual DateTimeOffset GetMessageUtcDateTime(MimeMessage data, Lazy<Match> bodyMatch, Lazy<Match> subjectMatch) =>
            data.Date;

        protected virtual string GetPaycard(MimeMessage data, Lazy<Match> bodyMatch, Lazy<Match> subjectMatch) =>
            bodyMatch.Value.Groups["paycard"].Value.Trim();

        protected virtual string GetDetails(MimeMessage data, Lazy<Match> bodyMatch, Lazy<Match> subjectMatch) =>
            bodyMatch.Value.Groups["details"].Value.Trim();
    }

    public class AccountIncomeEmailAnalyzer : RegexAccountEmailAnalyzer
    {
        public AccountIncomeEmailAnalyzer(IOptions<FinancesConfiguration> config) 
            : base(config.Value.AccountIncomeEmailAnalyzer) { }

        protected override DateTimeOffset GetTransactionUtcDate(MimeMessage data, Lazy<Match> bodyMatch, Lazy<Match> subjectMatch) =>
            GetMessageUtcDateTime(data, bodyMatch, subjectMatch).Date;

        protected override string GetPaycard(MimeMessage data, Lazy<Match> bodyMatch, Lazy<Match> subjectMatch) => 
            null;
    }

    public class AccountDebtEmailAnalyzer : RegexAccountEmailAnalyzer
    {
        public AccountDebtEmailAnalyzer(IOptions<FinancesConfiguration> config)
            : base(config.Value.AccountDebtEmailAnalyzer) { }

        protected override decimal GetAmount(MimeMessage data, Lazy<Match> bodyMatch, Lazy<Match> subjectMatch) =>
            base.GetAmount(data, bodyMatch, subjectMatch) * -1;

        protected override string GetPaycard(MimeMessage data, Lazy<Match> bodyMatch, Lazy<Match> subjectMatch) =>
            null;
    }

    public class CardPaymentEmailAnalyzer : RegexAccountEmailAnalyzer
    {
        public CardPaymentEmailAnalyzer(IOptions<FinancesConfiguration> config)
            : base(config.Value.CardPaymentEmailAnalyzer) { }

        protected override string GetAccountNumber(MimeMessage data, Lazy<Match> bodyMatch, Lazy<Match> subjectMatch) =>
            null;

        protected override decimal? GetBalance(MimeMessage data, Lazy<Match> bodyMatch, Lazy<Match> subjectMatch) =>
            null;
    }

    public class CardPaymentSettledEmailAnalyzer : RegexAccountEmailAnalyzer
    {
        public CardPaymentSettledEmailAnalyzer(IOptions<FinancesConfiguration> config)
            : base(config.Value.CardPaymentSettledEmailAnalyzer) { }

        protected override string GetAccountNumber(MimeMessage data, Lazy<Match> bodyMatch, Lazy<Match> subjectMatch) =>
            null;

        protected override decimal? GetBalance(MimeMessage data, Lazy<Match> bodyMatch, Lazy<Match> subjectMatch) =>
            null;
    }
}
