using System;
using System.Security.Cryptography;
using System.Text;

namespace Distvisor.Web.Services
{
    public interface ICryptoService
    {
        void FillWithRandomNumbers(Span<byte> data);
        string GetHashString(string input);
        string HmacSha256Base64(string message, string key);
    }

    public class CryptoService : ICryptoService
    {
        public void FillWithRandomNumbers(Span<byte> data)
        {
            RandomNumberGenerator.Fill(data);
        }

        public string GetHashString(string input)
        {
            using var hashAlg = SHA256.Create();
            var bytes = hashAlg.ComputeHash(Encoding.UTF8.GetBytes(input));
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                sb.Append(b.ToString("X2"));
            }
            return sb.ToString();
        }

        public string HmacSha256Base64(string message, string key)
        {
            var encoding = new UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(key);

            byte[] messageBytes = encoding.GetBytes(message);
            using var hmacsha256 = new HMACSHA256(keyByte);
            byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
            return Convert.ToBase64String(hashmessage);
        }
    }
}
