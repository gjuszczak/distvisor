using Distvisor.App.HomeBox.Aggregates;
using Distvisor.App.HomeBox.Enums;
using Distvisor.App.HomeBox.Events;
using Distvisor.App.HomeBox.Exceptions;
using Distvisor.App.HomeBox.ValueObjects;
using NUnit.Framework;
using System;
using System.Linq;

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
        public void CanOpenGatewaySession()
        {
            var token = new GatewayToken(accessToken, refreshToken, generatedAt);
            var session = new GatewaySession(sessionId, username, token);

            var events = session.GetUncommittedChanges();
            Assert.IsTrue(events.Count() == 1);

            var gatewaySessionOpened = events.OfType<GatewaySessionOpened>().FirstOrDefault();
            Assert.IsNotNull(gatewaySessionOpened);
            Assert.AreEqual(token, gatewaySessionOpened.Token);
            Assert.AreEqual(username, gatewaySessionOpened.Username);

            Assert.AreEqual(sessionId, session.AggregateId);
            Assert.AreEqual(token, session.Token);
            Assert.AreEqual(GatewaySessionStatus.Open, session.Status);
            Assert.AreEqual(DateTimeOffset.MinValue, session.RefreshingReservationTimeout);
        }

        [Test]
        public void CanBeginRefreshGatewaySession()
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

            Assert.AreEqual(sessionId, session.AggregateId);
            Assert.IsNull(session.Token.AccessToken);
            Assert.AreEqual(refreshToken, session.Token.RefreshToken);
            Assert.AreEqual(generatedAt, session.Token.GeneratedAt);
            Assert.AreEqual(GatewaySessionStatus.Refreshing, session.Status);
            Assert.AreEqual(gatewaySessionRefreshStarted.Timeout, session.RefreshingReservationTimeout);
        }

        [Test]
        public void CanCompleteRefreshGatewaySession()
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
            Assert.AreEqual(refreshedToken, gatewaySessionRefreshSucceeded.Token);

            Assert.AreEqual(sessionId, session.AggregateId);
            Assert.AreEqual(refreshedToken, session.Token);
            Assert.AreEqual(GatewaySessionStatus.Open, session.Status);
            Assert.AreEqual(DateTimeOffset.MinValue, session.RefreshingReservationTimeout);
        }

        [Test]
        public void CanFailRefreshGatewaySession()
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

            Assert.AreEqual(sessionId, session.AggregateId);
            Assert.IsNull(session.Token);
            Assert.AreEqual(GatewaySessionStatus.Closed, session.Status);
            Assert.AreEqual(DateTimeOffset.MinValue, session.RefreshingReservationTimeout);
        }

        [Test]
        public void CanDeleteGatewaySession()
        {
            var token = new GatewayToken(accessToken, refreshToken, generatedAt);
            var session = new GatewaySession(sessionId, username, token);
            session.Delete();

            var events = session.GetUncommittedChanges();
            Assert.IsTrue(events.Count() == 2);

            var gatewaySessionRefreshFailed = events.ElementAt(1) as GatewaySessionDeleted;
            Assert.IsNotNull(gatewaySessionRefreshFailed);

            Assert.IsNull(session.Token);
            Assert.AreEqual(GatewaySessionStatus.Deleted, session.Status);
        }

        [Test]
        public void CanNotBeginRefreshGatewaySessionWhenTokenIsFresh()
        {            
            var token = new GatewayToken(accessToken, refreshToken, generatedAt);
            var session = new GatewaySession(sessionId, username, token);

            var now = DateTimeOffset.Now.AddMinutes(4.9);
            Assert.Throws<GatewaySessionTokenIsFreshException>(() => session.BeginRefresh(now));
        }

        [Test]
        public void CanNotBeginRefreshGatewaySessionWhenSessionIsReserveredForRefreshing()
        {
            var token = new GatewayToken(accessToken, refreshToken, generatedAt);
            var session = new GatewaySession(sessionId, username, token);

            var firstNow = DateTimeOffset.Now.AddMinutes(5.1);
            session.BeginRefresh(firstNow);

            var nextNow = DateTimeOffset.Now.AddMinutes(5.2);
            Assert.Throws<GatewaySessionRefreshingReservedException>(() => session.BeginRefresh(nextNow));
        }

        [Test]
        public void CanNotDeleteWhenSessionIsReserveredForRefreshing()
        {
            var token = new GatewayToken(accessToken, refreshToken, generatedAt);
            var session = new GatewaySession(sessionId, username, token);

            var firstNow = DateTimeOffset.Now.AddMinutes(5.1);
            session.BeginRefresh(firstNow);

            var nextNow = DateTimeOffset.Now.AddMinutes(5.2);
            Assert.Throws<GatewaySessionRefreshingReservedException>(() => session.BeginRefresh(nextNow));
        }

        [Test]
        public void CanNotUseRefreshGatewaySessionWhenClosed()
        {
            var token = new GatewayToken(accessToken, refreshToken, generatedAt);
            var session = new GatewaySession(sessionId, username, token);

            var firstNow = DateTimeOffset.Now.AddMinutes(5.1);
            session.BeginRefresh(firstNow);

            session.RefreshFailed();

            var nextNow = DateTimeOffset.Now.AddMinutes(5.2);
            var refreshedToken = new GatewayToken($"{accessToken}_fresh", $"{refreshToken}_fresh", nextNow);

            Assert.Throws<GatewaySessionClosedException>(() => session.BeginRefresh(nextNow));
            Assert.Throws<GatewaySessionClosedException>(() => session.RefreshSucceed(refreshedToken));
            Assert.Throws<GatewaySessionClosedException>(() => session.RefreshFailed());
        }

        [Test]
        public void CanNotUseRefreshGatewaySessionWhenDeleted()
        {
            var token = new GatewayToken(accessToken, refreshToken, generatedAt);
            var session = new GatewaySession(sessionId, username, token);
            session.Delete();

            Assert.Throws<GatewaySessionDeletedException>(() => session.BeginRefresh(DateTimeOffset.Now));
            Assert.Throws<GatewaySessionDeletedException>(() => session.RefreshSucceed(token));
            Assert.Throws<GatewaySessionDeletedException>(() => session.RefreshFailed());
        }
    }
}