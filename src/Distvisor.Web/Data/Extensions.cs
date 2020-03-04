using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Distvisor.Web.Data
{
    public static class Extensions
    {
        public static PropertyBuilder<TProperty> HasEnumConversion<TProperty>(this PropertyBuilder<TProperty> builder) {
            return builder.HasConversion(
                    v => v.ToString(),
                    v => (TProperty)Enum.Parse(typeof(TProperty), v));
        }

        public static PropertyBuilder<DateTime> HasUtcDateTimeConversion(this PropertyBuilder<DateTime> builder)
        {
            return builder.HasConversion(
                    v => v.ToUniversalTime(),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
        }
    }
}
