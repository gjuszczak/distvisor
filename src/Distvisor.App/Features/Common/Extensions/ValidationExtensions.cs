using FluentValidation;
using System;

namespace Distvisor.App.Features.Common.Extensions
{
    public static class ValidationExtensions
    {
        public static IRuleBuilderOptionsConditions<T, string> IsValidUri<T>(this IRuleBuilder<T, string> ruleBuilder, UriKind uriKind = UriKind.Absolute)
        {
            return ruleBuilder.Custom((uri, context) => {
                if (!Uri.TryCreate(uri, uriKind, out _))
                {
                    context.AddFailure("The property must be a valid uri");
                }
            });
        }
    }
}
