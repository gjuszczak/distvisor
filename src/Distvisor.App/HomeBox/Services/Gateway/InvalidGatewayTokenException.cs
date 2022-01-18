using System;

namespace Distvisor.App.HomeBox.Services.Gateway
{
    public class InvalidGatewayTokenException : Exception
    {
        public InvalidGatewayTokenException()
            : base("Provided gateway token is invalid.")
        { }
    }
}
