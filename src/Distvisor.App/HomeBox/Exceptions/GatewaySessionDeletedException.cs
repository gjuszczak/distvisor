using System;

namespace Distvisor.App.HomeBox.Exceptions
{
    public class GatewaySessionDeletedException : Exception
    {
        public GatewaySessionDeletedException() :
            base("Gateway session is deleted")
        {
        }
    }
}
