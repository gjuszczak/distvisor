using System;

namespace Distvisor.App.HomeBox.Exceptions
{
    public class GatewaySessionIsNotRefreshingException : Exception
    {
        public GatewaySessionIsNotRefreshingException() :
            base("Session is already closed.")
        {
        }
    }
}
