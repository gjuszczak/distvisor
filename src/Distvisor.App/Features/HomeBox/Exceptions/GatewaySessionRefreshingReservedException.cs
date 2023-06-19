using System;

namespace Distvisor.App.Features.HomeBox.Exceptions
{
    public class GatewaySessionRefreshingReservedException : Exception
    {
        public GatewaySessionRefreshingReservedException(DateTimeOffset reservationTimeout) :
            base("Session refresh is currently reserved by another instance or thread.")
        {
            ReservationTimeout = reservationTimeout;
        }

        public DateTimeOffset ReservationTimeout { get; init; }
    }
}
