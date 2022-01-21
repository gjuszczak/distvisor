using System;

namespace Distvisor.App.HomeBox.Exceptions
{
    public class GatewaySessionClosedException : Exception
    {
        public GatewaySessionClosedException() :
            base("Gateway session is closed")
        {
        }
    }
}
