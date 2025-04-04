﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinLY.Models
{
    public class UserTransaction
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string TransactionTitle { get; set; } = string.Empty;
        public string TransactionType { get; set; } = string.Empty;
        public decimal Amounts { get; set; }
        public string Tag { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public DateTime? TransactionDate { get; set; } = DateTime.Now;
    }
}
