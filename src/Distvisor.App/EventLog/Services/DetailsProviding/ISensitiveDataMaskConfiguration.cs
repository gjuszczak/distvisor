using System;
using System.Collections.Generic;

namespace Distvisor.App.EventLog.Services.DetailsProviding
{
    public interface ISensitiveDataMaskConfiguration
    {
        string MaskString { get; }

        IEnumerable<string> GetMaskedProperties(Type type);
        bool IsTypeMasked(Type type);
    }
}