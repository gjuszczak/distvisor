using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Distvisor.App.EventLog.Services.DetailsProviding
{
    public class SensitiveDataMaskConfiguration : ISensitiveDataMaskConfigurationBuilder, ISensitiveDataMaskConfiguration
    {
        private readonly Dictionary<Type, HashSet<string>> _maskedProperties = new();

        private SensitiveDataMaskConfiguration() { }

        public static ISensitiveDataMaskConfigurationBuilder Create()
            => new SensitiveDataMaskConfiguration();

        public string MaskString { get; } = "*****";

        public ISensitiveDataMaskConfigurationBuilder MaskProperty<T, P>(Expression<Func<T, P>> propertySelector) where T : class
        {
            LambdaExpression lambda = propertySelector;
            var memberExpression = lambda.Body is UnaryExpression expression
                ? (MemberExpression)expression.Operand
                : (MemberExpression)lambda.Body;

            var propertyInfo = (PropertyInfo)memberExpression.Member;
            var propertyName = propertyInfo.Name;
            var camelCasePropertyName = propertyName.Length == 1
                ? propertyName[..1].ToLowerInvariant()
                : propertyName[..1].ToLowerInvariant() + propertyName[1..];
            if (!_maskedProperties.ContainsKey(typeof(T)))
            {
                _maskedProperties.Add(typeof(T), new HashSet<string>());
            }
            _maskedProperties[typeof(T)].Add(camelCasePropertyName);
            
            return this;
        }

        public ISensitiveDataMaskConfiguration Build() 
            => this;

        public bool IsTypeMasked(Type type)
            => _maskedProperties.ContainsKey(type);

        public IEnumerable<string> GetMaskedProperties(Type type)
            => _maskedProperties.GetValueOrDefault(type, null) ?? Enumerable.Empty<string>();


    }
}
