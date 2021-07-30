using System;
using System.Globalization;
using System.Security.Cryptography;

namespace Distvisor.Web.Services
{
    public static class EwelinkHelper
    {
        public static string GenerateNonce()
        {
            var nonce = new byte[15];
            RandomNumberGenerator.Fill(nonce);
            return Convert.ToBase64String(nonce);
        }

        public static string GenerateTimestamp()
        {
            var seed = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            var timestamp = Math.Floor(seed / 1000);
            return timestamp.ToString(CultureInfo.InvariantCulture);
        }

        public static (string timestamp, string sequence) GenerateSequence()
        {
            var seed = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            var timestamp = Math.Floor(seed / 1000);
            var sequence = Math.Floor(timestamp);
            return (timestamp.ToString(CultureInfo.InvariantCulture), sequence.ToString(CultureInfo.InvariantCulture));
        }

        public static string GenerateFakeImei()
        {
            var random = new Random();
            var num1 = random.Next(1000, 9999);
            var num2 = random.Next(1000, 9999);

            return $"DF7425A0-{num1}-{num2}-9F5E-3BC9179E48FB";
        }

        public static class Constants
        {
            public const string VERSION = "8";
            public const string OS = "android";
            public const string USER_AGENT = "app";
            public const string MODEL = "";
            public const string ROM_VERSION = "";
            public const string APP_VERSION = "3.14.1";
            public const string APK_VERSION = "1.8";
            public const string GRANT_TYPE_REFRESH = "refresh";
            public const string LANG_EN = "en";
            public const string GET_TAGS_OFF = "0";
        }
    }
}
