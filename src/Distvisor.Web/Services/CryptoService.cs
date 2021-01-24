using System.Security.Cryptography;
using System.Text;

namespace Distvisor.Web.Services
{
    public interface ICryptoService
    {
        string GetHashString(string input);
    }

    public class CryptoService : ICryptoService
    {
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
    }
}
