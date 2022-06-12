using Distvisor.App.Common.Interfaces;
using Distvisor.App.Core.Aggregates;
using Distvisor.App.Core.Exceptions;
using Distvisor.App.HomeBox.Aggregates;
using Distvisor.App.HomeBox.Enums;
using Distvisor.App.HomeBox.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
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

        public async Task OpenGatewaySessionAsync(Guid sessionId, string username, string password, CancellationToken cancellationToken = default)
        {
            var authResult = await _gatewayClient.LoginAsync(username, password);
            var gatewaySession = new GatewaySession(sessionId, username, authResult.Token);
            _aggregateContext.Add(gatewaySession);
            await _aggregateContext.CommitAsync(cancellationToken);
        }

        public async Task<GatewayActiveSession> GetActiveSessionAsync(CancellationToken cancellationToken = default)
        {
            var session = await _appDbContext.HomeboxGatewaySessions
                .FirstOrDefaultAsync(x => x.Status == GatewaySessionStatus.Open, cancellationToken);

            return session != null
                ? new GatewayActiveSession { SessionId = session.Id, AccessToken = session.Token.AccessToken }
                : null;
        }

        public async Task RefreshSessionAsync(Guid sessionId, CancellationToken cancellationToken = default)
        {
            var session = await BeginRefreshAsync(sessionId, cancellationToken);
            try
            {
                var authResult = await _gatewayClient.RefreshSessionAsync(session.Token.RefreshToken);
                session.RefreshSucceed(authResult.Token);
            }
            catch
            {
                session.RefreshFailed();
            }
            await _aggregateContext.CommitAsync(cancellationToken);
        }

        public async Task DeleteSessionAsync(Guid sessionId, CancellationToken cancellationToken = default)
        {
            var session = await _aggregateContext.GetAsync<GatewaySession>(sessionId, cancellationToken: cancellationToken);
            session.Delete();
            await _aggregateContext.CommitAsync(cancellationToken);
        }

        private async Task<GatewaySession> BeginRefreshAsync(Guid sessionId, CancellationToken cancellationToken)
        {
            for (int retryCount = 0; retryCount < 3; retryCount++)
            {
                try
                {
                    _aggregateContext.Deatach(sessionId);
                    var session = await _aggregateContext.GetAsync<GatewaySession>(sessionId, cancellationToken: cancellationToken);
                    session.BeginRefresh();
                    await _aggregateContext.CommitAsync(cancellationToken);
                    return await _aggregateContext.GetAsync<GatewaySession>(sessionId, cancellationToken: cancellationToken);
                }
                catch (GatewaySessionRefreshingReservedException exc)
                {
                    await Task.Delay(exc.ReservationTimeout - DateTimeOffset.Now, cancellationToken);
                }
                catch (ConcurrencyException)
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(500), cancellationToken);
                }
            }

            throw new Exception("Unable to begin session refresh.");
        }
    }
}
