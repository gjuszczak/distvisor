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

        public GatewaySession() { }

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
            if (Status == GatewaySessionStatus.Deleted)
            {
                throw new GatewaySessionDeletedException();
            }

            var now = optionalNow ?? DateTimeOffset.Now;
            if (IsReservedForRefreshing(now))
            {
                throw new GatewaySessionRefreshingReservedException(RefreshingReservationTimeout);
            };

            if (Status == GatewaySessionStatus.Open && (Token?.GeneratedAt ?? DateTimeOffset.MinValue) > now.AddMinutes(-5))
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
            if (Status == GatewaySessionStatus.Deleted)
            {
                throw new GatewaySessionDeletedException();
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
            if (Status == GatewaySessionStatus.Deleted)
            {
                throw new GatewaySessionDeletedException();
            }
            if (Status != GatewaySessionStatus.Refreshing)
            {
                throw new GatewaySessionIsNotRefreshingException();
            };

            ApplyChange(new GatewaySessionRefreshSucceeded(token));
        }

        public void Delete()
        {
            if (Status == GatewaySessionStatus.Deleted)
            {
                return;
            }
            if (IsReservedForRefreshing(DateTimeOffset.UtcNow))
            {
                throw new GatewaySessionRefreshingReservedException(RefreshingReservationTimeout);
            };

            ApplyChange(new GatewaySessionDeleted());
        }

        private bool IsReservedForRefreshing(DateTimeOffset now)
        {
            return Status == GatewaySessionStatus.Refreshing && now < RefreshingReservationTimeout;
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
                
                case GatewaySessionDeleted sessionDeleted:
                    Apply(sessionDeleted);
                    break;
            }
        }

        private void Apply(GatewaySessionOpened @event)
        {
            Username = @event.Username;
            Token = @event.Token;
            Status = GatewaySessionStatus.Open;
            RefreshingReservationTimeout = DateTimeOffset.MinValue;
        }

        private void Apply(GatewaySessionRefreshStarted @event)
        {
            Token = new GatewayToken(null, Token.RefreshToken, Token.GeneratedAt);
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

        private void Apply(GatewaySessionDeleted @event)
        {
            Token = null;
            Status = GatewaySessionStatus.Deleted;
        }
    }
}
