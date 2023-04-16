using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Distvisor.Infrastructure.Services.HomeBox
{
    public static class GatewayHelper
    {
        public static string GenerateNonce()
        {
            var nonce = new byte[15];
            RandomNumberGenerator.Fill(nonce);
            return Convert.ToBase64String(nonce);
        }

        public static string GenerateTimestamp()
        {
            var seed = DateTimeOffset.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            var timestamp = Math.Floor(seed / 1000);
            return timestamp.ToString(CultureInfo.InvariantCulture);
        }

        public static (string timestamp, string sequence) GenerateSequence()
        {
            var seed = DateTimeOffset.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
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

        public static string HmacSha256Base64(string message, string key)
        {
            var encoding = new UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(key);

            byte[] messageBytes = encoding.GetBytes(message);
            using var hmacsha256 = new HMACSHA256(keyByte);
            byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
            return Convert.ToBase64String(hashmessage);
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
