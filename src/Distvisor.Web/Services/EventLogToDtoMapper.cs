using Distvisor.Web.Controllers;
using Distvisor.Web.Data.Events;
using Distvisor.Web.Data.Events.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Distvisor.Web.Services
{
    public interface IEventLogToDtoMapper
    {
        EventLogDto Map(EventEntity eventEntity);
    }

    public class EventLogToDtoMapper : IEventLogToDtoMapper
    {
        private readonly Dictionary<string, IEventPayloadMapper> _mappers;

        public EventLogToDtoMapper()
        {
            var configs = new List<IEventPayloadMapper>
            {
                new EventPayloadMapper<SetRedirectionEvent>()
                    .ConfigurePayloadTypeDisplayName("Set Redirection"),

                new EventPayloadMapper<RemoveRedirectionEvent>()
                    .ConfigurePayloadTypeDisplayName("Remove Redirection"),

                new EventPayloadMapper<SetSecretEvent>()
                    .ConfigurePayloadTypeDisplayName("Set Secret")
                    .ConfigureMaskPayloadProperty(x=> x.Value),

                new EventPayloadMapper<RemoveSecretEvent>()
                    .ConfigurePayloadTypeDisplayName("Remove Secret"),

                new EventPayloadMapper<EmailReceivedEvent>()
                    .ConfigurePayloadTypeDisplayName("Email Received"),

                new EventPayloadMapper<FinancialAccountAddedEvent>()
                    .ConfigurePayloadTypeDisplayName("Financial Account Added"),

                new EventPayloadMapper<FinancialAccountTransactionAddedEvent>()
                    .ConfigurePayloadTypeDisplayName("Financial Account Transaction Added"),

                new EventPayloadMapper<FinancialDataImportedEvent>()
                    .ConfigurePayloadTypeDisplayName("Financial Data Imported"),
            };

            _mappers = configs.ToDictionary(x => x.GetPayloadType());
        }

        public EventLogDto Map(EventEntity eventEntity)
        {
            var mapper = _mappers[eventEntity.PayloadType];
            var result = new EventLogDto
            {
                Id = eventEntity.Id,
                PublishDateUtc = eventEntity.PublishDateUtc,
                Status = eventEntity.Success ? "Success" : "Failure",
                PayloadType = eventEntity.PayloadType,
                PayloadTypeDisplayName = mapper.GetPayloadTypeDisplayName(),
                MaskedPayload = mapper.GetMaskedPayload(eventEntity.ToPayload())
            };
            return result;
        }

        private interface IEventPayloadMapper
        {
            string GetPayloadType();
            string GetPayloadTypeDisplayName();
            object GetMaskedPayload(object payload);
        }

        private class EventPayloadMapper<TPayload> : IEventPayloadMapper
        {
            private readonly List<Action<object>> _payloadValuePropertyMasks = new List<Action<object>>();
            private string _payloadTypeDisplayName = null;

            public EventPayloadMapper<TPayload> ConfigurePayloadTypeDisplayName(string displayName)
            {
                _payloadTypeDisplayName = displayName;
                return this;
            }

            public EventPayloadMapper<TPayload> ConfigureMaskPayloadProperty<TProperty>(Expression<Func<TPayload, TProperty>> payloadProperty)
            {
                MemberExpression member = payloadProperty.Body as MemberExpression;
                PropertyInfo propInfo = member.Member as PropertyInfo;
                _payloadValuePropertyMasks.Add(entity =>
                {
                    propInfo.SetValue(entity, "*****");
                });
                return this;
            }

            public string GetPayloadType() => typeof(TPayload).ToString();

            public string GetPayloadTypeDisplayName()
            {
                return _payloadTypeDisplayName ?? GetPayloadType();
            }

            public object GetMaskedPayload(object payload)
            {
                foreach (var maskProperty in _payloadValuePropertyMasks)
                {
                    maskProperty(payload);
                }
                return payload;
            }
        }
    }
}
