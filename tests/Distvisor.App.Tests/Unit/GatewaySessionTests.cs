using Distvisor.App.HomeBox.Aggregates;
using Distvisor.App.HomeBox.Enums;
using Distvisor.App.HomeBox.Events;
using Distvisor.App.HomeBox.Exceptions;
using Distvisor.App.HomeBox.ValueObjects;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Distvisor.App.Tests.Unit
{
    public class GatewaySessionTests
    {
        private readonly Guid sessionId = Guid.NewGuid();
        private readonly string username = "user";
        private readonly string accessToken = "at";
        private readonly string refreshToken = "rt";
        private readonly DateTimeOffset generatedAt = DateTimeOffset.Now;

        [Test]
        public async Task CanOpenGatewaySession()
        {
            var token = new GatewayToken(accessToken, refreshToken, generatedAt);
            var session = new GatewaySession(sessionId, username, token);

            var events = session.GetUncommittedChanges();
            Assert.IsTrue(events.Count() == 1);

            var gatewaySessionOpened = events.OfType<GatewaySessionOpened>().FirstOrDefault();
            Assert.IsNotNull(gatewaySessionOpened);
            Assert.AreEqual(gatewaySessionOpened.Token, token);
            Assert.AreEqual(gatewaySessionOpened.Username, username);

            Assert.AreEqual(session.AggregateId, sessionId);
            Assert.AreEqual(session.Token, token);
            Assert.AreEqual(session.Status, GatewaySessionStatus.Open);
            Assert.AreEqual(session.RefreshingReservationTimeout, DateTimeOffset.MinValue);
        }

        [Test]
        public async Task CanBeginRefreshGatewaySession()
        {
            var token = new GatewayToken(accessToken, refreshToken, generatedAt);
            var session = new GatewaySession(sessionId, username, token);
            
            var now = DateTimeOffset.Now.AddMinutes(5.1);
            session.BeginRefresh(now);

            var events = session.GetUncommittedChanges();
            Assert.IsTrue(events.Count() == 2);

            var gatewaySessionRefreshStarted = events.ElementAt(1) as GatewaySessionRefreshStarted;
            Assert.IsNotNull(gatewaySessionRefreshStarted);
            Assert.IsTrue(gatewaySessionRefreshStarted.Timeout > now);

            Assert.AreEqual(session.AggregateId, sessionId);
            Assert.IsNull(session.Token.AccessToken);
            Assert.AreEqual(session.Token.RefreshToken, refreshToken);
            Assert.AreEqual(session.Token.GeneratedAt, generatedAt);
            Assert.AreEqual(session.Status, GatewaySessionStatus.Refreshing);
            Assert.AreEqual(session.RefreshingReservationTimeout, gatewaySessionRefreshStarted.Timeout);
        }

        [Test]
        public async Task CanCompleteRefreshGatewaySession()
        {
            var originalToken = new GatewayToken(accessToken, refreshToken, generatedAt);
            var session = new GatewaySession(sessionId, username, originalToken);

            var now = DateTimeOffset.Now.AddMinutes(5.1);
            session.BeginRefresh(now);

            var refreshedToken = new GatewayToken($"{accessToken}_fresh", $"{refreshToken}_fresh", now);
            session.RefreshSucceed(refreshedToken);

            var events = session.GetUncommittedChanges();
            Assert.IsTrue(events.Count() == 3);

            var gatewaySessionRefreshSucceeded = events.ElementAt(2) as GatewaySessionRefreshSucceeded;
            Assert.IsNotNull(gatewaySessionRefreshSucceeded);
            Assert.AreEqual(gatewaySessionRefreshSucceeded.Token, refreshedToken);

            Assert.AreEqual(session.AggregateId, sessionId);
            Assert.AreEqual(session.Token, refreshedToken);
            Assert.AreEqual(session.Status, GatewaySessionStatus.Open);
            Assert.AreEqual(session.RefreshingReservationTimeout, DateTimeOffset.MinValue);
        }

        [Test]
        public async Task CanFailRefreshGatewaySession()
        {
            var token = new GatewayToken(accessToken, refreshToken, generatedAt);
            var session = new GatewaySession(sessionId, username, token);

            var now = DateTimeOffset.Now.AddMinutes(5.1);
            session.BeginRefresh(now);

            session.RefreshFailed();

            var events = session.GetUncommittedChanges();
            Assert.IsTrue(events.Count() == 3);

            var gatewaySessionRefreshFailed = events.ElementAt(2) as GatewaySessionRefreshFailed;
            Assert.IsNotNull(gatewaySessionRefreshFailed);

            Assert.AreEqual(session.AggregateId, sessionId);
            Assert.IsNull(session.Token);
            Assert.AreEqual(session.Status, GatewaySessionStatus.Closed);
            Assert.AreEqual(session.RefreshingReservationTimeout, DateTimeOffset.MinValue);
        }

        [Test]
        public async Task CanNotBeginRefreshGatewaySessionWhenTokenIsFresh()
        {            
            var token = new GatewayToken(accessToken, refreshToken, generatedAt);
            var session = new GatewaySession(sessionId, username, token);

            var now = DateTimeOffset.Now.AddMinutes(4.9);
            Assert.Throws<GatewaySessionTokenIsFreshException>(() => session.BeginRefresh(now));
        }

        [Test]
        public async Task CanNotBeginRefreshGatewaySessionWhenRefreshIsAlreadyReserved()
        {
            var token = new GatewayToken(accessToken, refreshToken, generatedAt);
            var session = new GatewaySession(sessionId, username, token);

            var firstNow = DateTimeOffset.Now.AddMinutes(5.1);
            session.BeginRefresh(firstNow);

            var nextNow = DateTimeOffset.Now.AddMinutes(5.2);
            Assert.Throws<GatewaySessionRefreshingReservedException>(() => session.BeginRefresh(nextNow));
        }
    }
}