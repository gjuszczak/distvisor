using System;

namespace Distvisor.App.Features.HomeBox.Exceptions
{
    public class GatewaySessionIsNotRefreshingException : Exception
    {
        public GatewaySessionIsNotRefreshingException() :
            base("Session is already closed.")
        {
        }
    }
}
