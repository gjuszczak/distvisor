using System;
using System.Linq.Expressions;

namespace Distvisor.App.EventLog.Services.DetailsProviding
{
    public interface ISensitiveDataMaskConfigurationBuilder
    {
        ISensitiveDataMaskConfigurationBuilder MaskProperty<T, P>(Expression<Func<T, P>> propertySelector) where T : class;
        ISensitiveDataMaskConfiguration Build();
    }
}