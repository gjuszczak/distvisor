﻿using System;

namespace Distvisor.App.Core.Entities
{
    public abstract class AuditableEntity
    {
        public DateTimeOffset Created { get; set; }

        public string CreatedBy { get; set; }

        public DateTimeOffset? LastModified { get; set; }

        public string LastModifiedBy { get; set; }
    }
}
