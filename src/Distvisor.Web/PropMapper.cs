﻿using System.Linq.Expressions;
using System.Reflection;

namespace Distvisor.Web
{
    //clones object public properties to another object
    //uses Expressions (compiled and saved to static) - faster than Reflection
    //(compilation happens with every new generic type call cause it's a new static class each time)
    public static class PropMapper<TInput, TOutput>
    {
        private static readonly Func<TInput, TOutput> _cloner;
        private static readonly Action<TInput, TOutput> _copier;

        private static readonly IEnumerable<PropertyInfo> _sourceProperties;
        private static readonly IEnumerable<PropertyInfo> _destinationProperties;

        static PropMapper()
        {
            _destinationProperties = typeof(TOutput)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(prop => prop.CanWrite);
            _sourceProperties = typeof(TInput)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(prop => prop.CanRead);

            _cloner = CreateCloner();
            _copier = CreateCopier();
        }

        private static Func<TInput, TOutput> CreateCloner()
        {
            //check if type has parameterless constructor - just in case
            if (typeof(TOutput).GetConstructor(Type.EmptyTypes) == null) return (x) => default;

            var input = Expression.Parameter(typeof(TInput), "input");

            // For each property that exists in the destination object, is there a property with the same name in the source object?
            var memberBindings = _sourceProperties.Join(_destinationProperties,
                sourceProperty => sourceProperty.Name,
                destinationProperty => destinationProperty.Name,
                (sourceProperty, destinationProperty) =>
                {
                    if (sourceProperty.PropertyType.Equals(destinationProperty.PropertyType))
                    {
                        return Expression.Bind(destinationProperty, Expression.Property(input, sourceProperty));
                    }
                    return null;
                })
                .Where(x => x != null);

            var body = Expression.MemberInit(Expression.New(typeof(TOutput)), memberBindings);
            var lambda = Expression.Lambda<Func<TInput, TOutput>>(body, input);
            return lambda.Compile();
        }

        private static Action<TInput, TOutput> CreateCopier()
        {
            var input = Expression.Parameter(typeof(TInput), "input");
            var output = Expression.Parameter(typeof(TOutput), "output");

            // For each property that exists in the destination object, is there a property with the same name in the source object?
            var memberAssignments = _sourceProperties.Join(_destinationProperties,
                sourceProperty => sourceProperty.Name,
                destinationProperty => destinationProperty.Name,
                (sourceProperty, destinationProperty) =>
                {
                    if (sourceProperty.PropertyType.Equals(destinationProperty.PropertyType))
                    {
                        return Expression.Assign(Expression.Property(output, destinationProperty), Expression.Property(input, sourceProperty));
                    }
                    return null;
                })
                .Where(mb => mb != null);

            var body = Expression.Block(memberAssignments);
            var lambda = Expression.Lambda<Action<TInput, TOutput>>(body, input, output);
            return lambda.Compile();
        }

        public static TOutput From(TInput input)
        {
            if (input == null) return default;
            return _cloner(input);
        }

        public static void CopyTo(TInput input, TOutput output)
        {
            _copier(input, output);
        }

    }
}
