using System;

namespace Distvisor.App.Features.HomeBox.Services.Gateway
{
    public class InvalidGatewayTokenException : Exception
    {
        public InvalidGatewayTokenException()
            : base("Provided gateway token is invalid.")
        { }
    }
}
