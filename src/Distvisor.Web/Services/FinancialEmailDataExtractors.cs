using Distvisor.Web.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IFinancialEmailDataExtractor : IFinancialDataExtractor<MimeMessage>
    {
    }

    public class RegexFinancialEmailDataExtractor : IFinancialEmailDataExtractor
    {
        protected virtual RegexFinancialEmailDataExtractorConfig Config { get; }

        public RegexFinancialEmailDataExtractor(RegexFinancialEmailDataExtractorConfig config)
        {
            Config = config;
        }

        public bool CanExtract(MimeMessage data)
        {
            return Regex.IsMatch(data.Subject ?? "", Config.RegexSubjectPattern) &&
                Regex.IsMatch(data.HtmlBody ?? "", Config.RegexBodyPattern);
        }

        public Task<IEnumerable<FinacialExtractedData>> ExtractAsync(MimeMessage data)
        {
            var bodyMatch = new Lazy<Match>(() => Regex.Match(data.HtmlBody, Config.RegexBodyPattern));
            var subjectMatch = new Lazy<Match>(() => Regex.Match(data.Subject, Config.RegexSubjectPattern));

            var result = new FinacialExtractedData
            {
                AccountNumber = GetAccountNumber(data, bodyMatch, subjectMatch),
                TransactionDate = GetTransactionUtcDate(data, bodyMatch, subjectMatch).Date,
                PostingDate = GetMessageUtcDateTime(data, bodyMatch, subjectMatch).Date,
                Title = GetDetails(data, bodyMatch, subjectMatch),
                Amount = GetAmount(data, bodyMatch, subjectMatch),
                Balance = GetBalance(data, bodyMatch, subjectMatch) ?? 0,
                //Paycard = GetPaycard(data, bodyMatch, subjectMatch),
            };

            return Task.FromResult<IEnumerable<FinacialExtractedData>>(new[] { result });
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

    public class AccountIncomeEmailDataExtractor : RegexFinancialEmailDataExtractor
    {
        public AccountIncomeEmailDataExtractor(IOptions<FinancesConfiguration> config)
            : base(config.Value.AccountIncomeEmailDataExtractor) { }

        protected override DateTimeOffset GetTransactionUtcDate(MimeMessage data, Lazy<Match> bodyMatch, Lazy<Match> subjectMatch) =>
            GetMessageUtcDateTime(data, bodyMatch, subjectMatch).Date;

        protected override string GetPaycard(MimeMessage data, Lazy<Match> bodyMatch, Lazy<Match> subjectMatch) =>
            null;
    }

    public class AccountDebtEmailDataExtractor : RegexFinancialEmailDataExtractor
    {
        public AccountDebtEmailDataExtractor(IOptions<FinancesConfiguration> config)
            : base(config.Value.AccountDebtEmailDataExtractor) { }

        protected override decimal GetAmount(MimeMessage data, Lazy<Match> bodyMatch, Lazy<Match> subjectMatch) =>
            base.GetAmount(data, bodyMatch, subjectMatch) * -1;

        protected override string GetPaycard(MimeMessage data, Lazy<Match> bodyMatch, Lazy<Match> subjectMatch) =>
            null;
    }

    public class CardPaymentEmailDataExtractor : RegexFinancialEmailDataExtractor
    {
        public CardPaymentEmailDataExtractor(IOptions<FinancesConfiguration> config)
            : base(config.Value.CardPaymentEmailDataExtractor) { }

        protected override string GetAccountNumber(MimeMessage data, Lazy<Match> bodyMatch, Lazy<Match> subjectMatch) =>
            null;

        protected override decimal? GetBalance(MimeMessage data, Lazy<Match> bodyMatch, Lazy<Match> subjectMatch) =>
            null;
    }

    public class CardPaymentSettledEmailDataExtractor : RegexFinancialEmailDataExtractor
    {
        public CardPaymentSettledEmailDataExtractor(IOptions<FinancesConfiguration> config)
            : base(config.Value.CardPaymentSettledEmailDataExtractor) { }

        protected override string GetAccountNumber(MimeMessage data, Lazy<Match> bodyMatch, Lazy<Match> subjectMatch) =>
            null;

        protected override decimal? GetBalance(MimeMessage data, Lazy<Match> bodyMatch, Lazy<Match> subjectMatch) =>
            null;
    }
}
