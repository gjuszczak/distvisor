﻿using System;

namespace Distvisor.Web.Data.Reads.Entities
{
    public class ProcessedEmailEntity
    {
        public Guid Id { get; set; }
        public string UniqueKey { get; set; }
        public string BodyMime { get; set; }
    }
}
