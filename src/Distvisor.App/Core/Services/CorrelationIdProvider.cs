using System;

namespace Distvisor.App.Core.Services
{
    public class CorrelationIdProvider : ICorrelationIdProvider
    {
        private Guid _correlationId = Guid.NewGuid();

        public Guid GetCorrelationId()
        {
            return _correlationId;
        }

        public void SetCorrelationId(Guid correlationId)
        {
            _correlationId = correlationId;
        }
    }
}
