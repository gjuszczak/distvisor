using System;

namespace Distvisor.App.Core.Services
{
    public interface ICorrelationIdProvider
    {
        void SetCorrelationId(Guid correlationId);
        Guid GetCorrelationId();
    }
}
