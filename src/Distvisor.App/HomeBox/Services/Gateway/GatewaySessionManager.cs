using Distvisor.App.Common;
using Distvisor.App.Core.Aggregates;
using Distvisor.App.Core.Exceptions;
using Distvisor.App.HomeBox.Aggregates;
using Distvisor.App.HomeBox.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Distvisor.App.HomeBox.Services.Gateway
{
    public class GatewaySessionManager : IGatewaySessionManager
    {
        private readonly IAggregateContext _aggregateContext;
        private readonly IAppDbContext _appDbContext;
        private readonly IGatewayAuthenticationClient _gatewayClient;

        public GatewaySessionManager(IAggregateContext aggregateContext, IAppDbContext appDbContext, IGatewayAuthenticationClient gatewayClient)
        {
            _aggregateContext = aggregateContext;
            _appDbContext = appDbContext;
            _gatewayClient = gatewayClient;
        }

        public async Task<GatewayActiveSession> GetActiveSessionAsync()
        {
            var session = await GetSessionAggregateAsync();

            return session?.Token != null
                ? new GatewayActiveSession { SessionId = session.AggregateId, AccessToken = session.Token.AccessToken }
                : null;
        }

        public async Task RefreshSessionAsync(Guid sessionId)
        {
            var session = await BeginRefreshAsync(sessionId);
            try
            {
                var authResult = await _gatewayClient.RefreshSessionAsync(session.Token.RefreshToken);
                session.RefreshSucceed(authResult.Token);
            }
            catch
            {
                session.RefreshFailed();
            }
        }

        private async Task<GatewaySession> GetSessionAggregateAsync()
        {
            var session = await _appDbContext.HomeboxGatewaySessions.FirstOrDefaultAsync();
            if (session == null)
            {
                return null;
            }

            return _aggregateContext.Get<GatewaySession>(session.AggregateId);
        }

        private async Task<GatewaySession> BeginRefreshAsync(Guid sessionId)
        {
            for (int retryCount = 0; retryCount < 3; retryCount++)
            {
                try
                {
                    _aggregateContext.Deatach(sessionId);
                    var session = _aggregateContext.Get<GatewaySession>(sessionId);
                    session.BeginRefresh();
                    _aggregateContext.Commit();
                    return session;
                }
                catch (GatewaySessionRefreshingReservedException exc)
                {
                    await Task.Delay(exc.ReservationTimeout - DateTimeOffset.Now);
                }
                catch (ConcurrencyException)
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(500));
                }
            }

            throw new Exception("Unable to begin session refresh.");
        }
    }
}
