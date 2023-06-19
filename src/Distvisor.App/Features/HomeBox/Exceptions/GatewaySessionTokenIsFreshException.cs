using System;

namespace Distvisor.App.Features.HomeBox.Exceptions
{
    public class GatewaySessionTokenIsFreshException : Exception
    {
        public GatewaySessionTokenIsFreshException() :
            base("Session token was refreshed already in 5 minutes.")
        {
        }
    }
}
