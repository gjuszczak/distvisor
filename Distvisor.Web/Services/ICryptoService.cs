namespace Distvisor.Web.Services
{
    public interface ICryptoService
    {
        string GeneratePasswordHash(string plainPassword);
        bool ValidatePasswordHash(string plainPassword, string passwordHash);
    }
}