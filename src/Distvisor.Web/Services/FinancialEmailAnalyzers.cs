using Distvisor.Web.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Distvisor.Web.Services
{
    public interface IFinancialEmailAnalyzer
    {
        bool CanAnalyze(MimeMessage emailBody);
        FinancialEmailAnalysis Analyze(MimeMessage emailBody);
    }

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

    public class RegexAccountEmailAnalyzer : IFinancialEmailAnalyzer
    {
        protected virtual RegexAccountEmailAnalyzerConfig Config { get; }

        public RegexAccountEmailAnalyzer(RegexAccountEmailAnalyzerConfig config)
        {
            Config = config;
        }

        public bool CanAnalyze(MimeMessage emailBody)
        {
            return Regex.IsMatch(emailBody.Subject ?? "", Config.RegexSubjectPattern) &&
                Regex.IsMatch(emailBody.HtmlBody ?? "", Config.RegexBodyPattern);
        }

        public FinancialEmailAnalysis Analyze(MimeMessage emailBody)
        {
            var bodyMatch = new Lazy<Match>(() => Regex.Match(emailBody.HtmlBody, Config.RegexBodyPattern));
            var subjectMatch = new Lazy<Match>(() => Regex.Match(emailBody.Subject, Config.RegexSubjectPattern));

            return new FinancialEmailAnalysis
            {
                AccountNumber = GetAccountNumber(emailBody, bodyMatch, subjectMatch),
                Amount = GetAmount(emailBody, bodyMatch, subjectMatch),
                Balance = GetBalance(emailBody, bodyMatch, subjectMatch),
                TransactionUtcDate = GetTransactionUtcDate(emailBody, bodyMatch, subjectMatch),
                MessageUtcDateTime = GetMessageUtcDateTime(emailBody, bodyMatch, subjectMatch),
                Paycard = GetPaycard(emailBody, bodyMatch, subjectMatch),
                Details = GetDetails(emailBody, bodyMatch, subjectMatch),
            };
        }

        protected virtual string GetAccountNumber(MimeMessage emailBody, Lazy<Match> bodyMatch, Lazy<Match> subjectMatch) =>
            bodyMatch.Value.Groups["accnum"].Value.Trim().Replace(" ", string.Empty);

        protected virtual decimal GetAmount(MimeMessage emailBody, Lazy<Match> bodyMatch, Lazy<Match> subjectMatch) =>
            decimal.Parse(bodyMatch.Value.Groups["amount"].Value.Replace(" ", string.Empty).Replace(",", "."), CultureInfo.InvariantCulture);

        protected virtual decimal? GetBalance(MimeMessage emailBody, Lazy<Match> bodyMatch, Lazy<Match> subjectMatch) =>
            decimal.Parse(bodyMatch.Value.Groups["balance"].Value.Replace(" ", string.Empty).Replace(",", "."), CultureInfo.InvariantCulture);

        protected virtual DateTimeOffset GetTransactionUtcDate(MimeMessage emailBody, Lazy<Match> bodyMatch, Lazy<Match> subjectMatch) =>
            DateTimeOffset.ParseExact(bodyMatch.Value.Groups["date"].Value, "dd-MM-yyyy", CultureInfo.InvariantCulture);

        protected virtual DateTimeOffset GetMessageUtcDateTime(MimeMessage emailBody, Lazy<Match> bodyMatch, Lazy<Match> subjectMatch) =>
            emailBody.Date;

        protected virtual string GetPaycard(MimeMessage emailBody, Lazy<Match> bodyMatch, Lazy<Match> subjectMatch) =>
            bodyMatch.Value.Groups["paycard"].Value.Trim();

        protected virtual string GetDetails(MimeMessage emailBody, Lazy<Match> bodyMatch, Lazy<Match> subjectMatch) =>
            bodyMatch.Value.Groups["details"].Value.Trim();
    }

    public class AccountIncomeEmailAnalyzer : RegexAccountEmailAnalyzer
    {
        public AccountIncomeEmailAnalyzer(IOptions<FinancesConfiguration> config) 
            : base(config.Value.AccountIncomeEmailAnalyzer) { }

        protected override DateTimeOffset GetTransactionUtcDate(MimeMessage emailBody, Lazy<Match> bodyMatch, Lazy<Match> subjectMatch) =>
            GetMessageUtcDateTime(emailBody, bodyMatch, subjectMatch).Date;

        protected override string GetPaycard(MimeMessage emailBody, Lazy<Match> bodyMatch, Lazy<Match> subjectMatch) => 
            null;
    }

    public class AccountDebtEmailAnalyzer : RegexAccountEmailAnalyzer
    {
        public AccountDebtEmailAnalyzer(IOptions<FinancesConfiguration> config)
            : base(config.Value.AccountDebtEmailAnalyzer) { }

        protected override decimal GetAmount(MimeMessage emailBody, Lazy<Match> bodyMatch, Lazy<Match> subjectMatch) =>
            base.GetAmount(emailBody, bodyMatch, subjectMatch) * -1;

        protected override string GetPaycard(MimeMessage emailBody, Lazy<Match> bodyMatch, Lazy<Match> subjectMatch) =>
            null;
    }

    public class CardPaymentEmailAnalyzer : RegexAccountEmailAnalyzer
    {
        public CardPaymentEmailAnalyzer(IOptions<FinancesConfiguration> config)
            : base(config.Value.CardPaymentEmailAnalyzer) { }

        protected override string GetAccountNumber(MimeMessage emailBody, Lazy<Match> bodyMatch, Lazy<Match> subjectMatch) =>
            null;

        protected override decimal? GetBalance(MimeMessage emailBody, Lazy<Match> bodyMatch, Lazy<Match> subjectMatch) =>
            null;
    }

    public class CardPaymentSettledEmailAnalyzer : RegexAccountEmailAnalyzer
    {
        public CardPaymentSettledEmailAnalyzer(IOptions<FinancesConfiguration> config)
            : base(config.Value.CardPaymentSettledEmailAnalyzer) { }

        protected override string GetAccountNumber(MimeMessage emailBody, Lazy<Match> bodyMatch, Lazy<Match> subjectMatch) =>
            null;

        protected override decimal? GetBalance(MimeMessage emailBody, Lazy<Match> bodyMatch, Lazy<Match> subjectMatch) =>
            null;
    }
}
