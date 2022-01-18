using Distvisor.App.Core.Aggregates;
using Distvisor.App.Core.Events;
using Distvisor.App.HomeBox.Enums;
using Distvisor.App.HomeBox.Events;
using Distvisor.App.HomeBox.Exceptions;
using Distvisor.App.HomeBox.ValueObjects;
using System;

namespace Distvisor.App.HomeBox.Aggregates
{
    public class GatewaySession : AggregateRoot
    {
        public string Username { get; private set; }
        public GatewayToken Token { get; private set; }
        public GatewaySessionStatus Status { get; private set; }
        public DateTimeOffset RefreshingReservationTimeout { get; private set; }

        public GatewaySession(Guid aggregateId, string username, GatewayToken token)
        {
            AggregateId = aggregateId;
            ApplyChange(new GatewaySessionOpened(username, token));
        }

        public void BeginRefresh(DateTimeOffset? optionalNow = null)
        {
            if (Status == GatewaySessionStatus.Closed)
            {
                throw new GatewaySessionClosedException();
            }

            var now = optionalNow ?? DateTime.Now;
            if (Status == GatewaySessionStatus.Refreshing && now < RefreshingReservationTimeout)
            {
                throw new GatewaySessionRefreshingReservedException(RefreshingReservationTimeout);
            };

            if (Status == GatewaySessionStatus.Open && (Token?.GeneratedAt ?? DateTimeOffset.MinValue) < now.AddMinutes(5))
            {
                throw new GatewaySessionTokenIsFreshException();
            }

            ApplyChange(new GatewaySessionRefreshStarted(now.AddSeconds(30)));
        }

        public void RefreshFailed()
        {
            if (Status == GatewaySessionStatus.Closed)
            {
                throw new GatewaySessionClosedException();
            }
            if (Status != GatewaySessionStatus.Refreshing)
            {
                throw new GatewaySessionIsNotRefreshingException();
            };

            ApplyChange(new GatewaySessionRefreshFailed());
        }

        public void RefreshSucceed(GatewayToken token)
        {
            if (Status == GatewaySessionStatus.Closed)
            {
                throw new GatewaySessionClosedException();
            }
            if (Status != GatewaySessionStatus.Refreshing)
            {
                throw new GatewaySessionIsNotRefreshingException();
            };

            ApplyChange(new GatewaySessionRefreshSucceeded(token));
        }

        protected override void Apply(IEvent @event)
        {
            switch (@event)
            {
                case GatewaySessionOpened sessionOpened:
                    Apply(sessionOpened);
                    break;

                case GatewaySessionRefreshStarted refreshStarted:
                    Apply(refreshStarted);
                    break;

                case GatewaySessionRefreshFailed refreshFailed:
                    Apply(refreshFailed);
                    break;

                case GatewaySessionRefreshSucceeded refreshSucceeded:
                    Apply(refreshSucceeded);
                    break;
            }
        }

        private void Apply(GatewaySessionOpened @event)
        {
            Token = @event.Token;
            Status = GatewaySessionStatus.Open;
            RefreshingReservationTimeout = DateTimeOffset.MinValue;
        }

        private void Apply(GatewaySessionRefreshStarted @event)
        {
            Token = null;
            Status = GatewaySessionStatus.Refreshing;
            RefreshingReservationTimeout = @event.Timeout;
        }

        private void Apply(GatewaySessionRefreshFailed @event)
        {
            Token = null;
            Status = GatewaySessionStatus.Closed;
            RefreshingReservationTimeout = DateTimeOffset.MinValue;
        }

        private void Apply(GatewaySessionRefreshSucceeded @event)
        {
            Token = @event.Token;
            Status = GatewaySessionStatus.Open;
            RefreshingReservationTimeout = DateTimeOffset.MinValue;
        }
    }
}
