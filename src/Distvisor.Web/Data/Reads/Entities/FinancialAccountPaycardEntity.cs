﻿using Distvisor.Web.Models;
using System;

namespace Distvisor.Web.Data.Reads.Entities
{
    public class FinancialAccountPaycardEntity : FinancialAccountPaycard
    {
        public Guid Id { get; set; }

        public FinancialAccountEntity Account { get; set; }
    }
}
