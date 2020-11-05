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
        public decimal Balance { get; set; }
        public DateTime Date { get; set; }
    }

    public class AccountIncomeEmailAnalyzer : IFinancialEmailAnalyzer
    {
        public bool CanAnalyze(MimeMessage emailBody)
        {
            return false;
        }

        public FinancialEmailAnalysis Analyze(MimeMessage emailBody)
        {
            throw new NotImplementedException();
        }
    }

    public class AccountDebtEmailAnalyzer : IFinancialEmailAnalyzer
    {
        private readonly string _regexSubjectPattern = @"Alerty24: rachunek (?<accnum>[\d ]*) - obciążenie rachunku";
        private readonly string _regexBodyPattern = @"Stan Twojego konta zmniejszył się(.|\n)*Z konta:\n\* (?<accnum>[\d ]*) \*(.|\n)*Ile:\n\* (?<amount>[\d,]*) PLN \*(.|\n)*Kiedy:\n\* (?<date>[\d-]*) \*(.|\n)*Saldo:\n\* (?<balance>[\d, ]*) PLN \*";

        public bool CanAnalyze(MimeMessage emailBody)
        {
            return Regex.IsMatch(emailBody.Subject, _regexSubjectPattern) &&
                Regex.IsMatch(emailBody.TextBody, _regexBodyPattern);
        }

        public FinancialEmailAnalysis Analyze(MimeMessage emailBody)
        {
            var bodyMatch = Regex.Match(emailBody.TextBody, _regexBodyPattern);

            return new FinancialEmailAnalysis
            {
                AccountNumber = bodyMatch.Groups["accnum"].Value.Replace(" ", string.Empty),
                Amount = decimal.Parse(bodyMatch.Groups["amount"].Value.Replace(" ", string.Empty).Replace(",", "."), CultureInfo.InvariantCulture),
                Balance = decimal.Parse(bodyMatch.Groups["balance"].Value.Replace(" ", string.Empty).Replace(",", "."), CultureInfo.InvariantCulture),
                Date = DateTime.ParseExact(bodyMatch.Groups["date"].Value, "dd-MM-yyyy", CultureInfo.InvariantCulture),
            };
        }
    }

    public class CardPaymentEmailAnalyzer : IFinancialEmailAnalyzer
    {
        public bool CanAnalyze(MimeMessage emailBody)
        {
            return false;
        }

        public FinancialEmailAnalysis Analyze(MimeMessage emailBody)
        {
            throw new NotImplementedException();
        }
    }
}
