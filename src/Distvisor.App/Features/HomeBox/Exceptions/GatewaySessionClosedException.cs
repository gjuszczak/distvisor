using System;

namespace Distvisor.App.Features.HomeBox.Exceptions
{
    public class GatewaySessionClosedException : Exception
    {
        public GatewaySessionClosedException() :
            base("Gateway session is closed")
        {
        }
    }
}
