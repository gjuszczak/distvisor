namespace Distvisor.Web.Services
{
    public interface ICryptoService
    {
        string GeneratePasswordHash(string plainPassword);
        string GenerateRandomSessionId();
        bool ValidatePasswordHash(string plainPassword, string passwordHash);
    }
}