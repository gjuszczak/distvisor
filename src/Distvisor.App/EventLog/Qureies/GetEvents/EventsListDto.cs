﻿using Distvisor.App.Common.Models;
using System;
using System.Collections.Generic;

namespace Distvisor.App.EventLog.Qureies.GetEvents
{
    public class EventsListDto : PaginatedList<EventDto>
    {
        public Guid? AggregateId { get; }

        public EventsListDto(List<EventDto> items, int totalRecords, int first, int rows, Guid? aggregateId)
            : base(items, totalRecords, first, rows) 
        {
            AggregateId = aggregateId;
        }
    }
}
