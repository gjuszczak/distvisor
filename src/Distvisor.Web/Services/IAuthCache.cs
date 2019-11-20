namespace Distvisor.Web.Services
{
    public interface IAuthCache
    {
        AuthResult Get(string sessionId);
        void Remove(string sessionId);
        void Set(string sessionId, AuthResult value);
    }
}