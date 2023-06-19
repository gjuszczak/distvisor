using System;
using System.Collections.Generic;

namespace Distvisor.App.Features.EventLog.Services.DetailsProviding
{
    public interface ISensitiveDataMaskConfiguration
    {
        string MaskString { get; }

        IEnumerable<string> GetMaskedProperties(Type type);
        bool IsTypeMasked(Type type);
    }
}