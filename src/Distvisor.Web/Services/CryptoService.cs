using System;
using System.Linq;
using System.Security.Cryptography;

namespace Distvisor.Web.Services
{
    public interface ICryptoService
    {
        string GeneratePasswordHash(string plainPassword);
        string GenerateRandomSessionId();
        bool ValidatePasswordHash(string plainPassword, string passwordHash);
    }

    public class CryptoService : ICryptoService
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 10000;
        private const int SessionIdSize = 64;

        public string GeneratePasswordHash(string plainPassword)
        {
            using var saltGen = new RNGCryptoServiceProvider();
            var salt = new byte[SaltSize];
            saltGen.GetBytes(salt);

            using var hashGen = new Rfc2898DeriveBytes(plainPassword, salt, Iterations, HashAlgorithmName.SHA256);
            var keyString = Convert.ToBase64String(hashGen.GetBytes(KeySize));
            var saltString = Convert.ToBase64String(salt);

            return $"{saltString}.{keyString}";
        }

        public bool ValidatePasswordHash(string plainPassword, string passwordHash)
        {
            var parts = passwordHash.Split('.', 2);
            var salt = Convert.FromBase64String(parts[0]);
            var key = Convert.FromBase64String(parts[1]);

            using var hashGen = new Rfc2898DeriveBytes(plainPassword, salt, Iterations, HashAlgorithmName.SHA256);
            var keyToVerify = hashGen.GetBytes(KeySize);
            return keyToVerify.SequenceEqual(key);
        }

        public string GenerateRandomSessionId()
        {
            using var saltGen = new RNGCryptoServiceProvider();
            var sessionId = new byte[SessionIdSize];
            saltGen.GetBytes(sessionId);
            return Convert.ToBase64String(sessionId);
        }
    }
}
