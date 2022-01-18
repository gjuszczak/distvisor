using Distvisor.App.Core.ValueObjects;
using System;
using System.Collections.Generic;

namespace Distvisor.App.HomeBox.ValueObjects
{
    public class GatewayToken : ValueObject
    {
        public GatewayToken(string accessToken, string refreshToken, DateTimeOffset generatedAt)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            GeneratedAt = generatedAt;
        }

        public string AccessToken { get; init; }
        public string RefreshToken { get; init; }
        public DateTimeOffset GeneratedAt { get; init; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return AccessToken;
            yield return RefreshToken;
            yield return GeneratedAt;
        }
    }
}
