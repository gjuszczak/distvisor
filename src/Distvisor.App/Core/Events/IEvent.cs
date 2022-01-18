﻿using MediatR;
using System;

namespace Distvisor.App.Core.Events
{
    public interface IEvent : INotification
	{
		Guid EventId { get; set; }
		Guid AggregateId { get; set; }
		Guid CorrelationId { get; set; }
		int Version { get; set; }
		DateTimeOffset TimeStamp { get; set; }
	}
}
