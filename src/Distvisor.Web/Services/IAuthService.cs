using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IAuthService
    {
        Task<bool> AnyAsync();
        Task<AuthResult> AuthenticateAsync(string sessionId);
        Task CreateUserAsync(string username, string password);
        Task<AuthResult> LoginAsync(string username, string password);
        Task LogoutAsync(string username);
    }
}