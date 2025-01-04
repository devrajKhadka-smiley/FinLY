using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinLY.Models
{
    public class DebtsProvider
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string DebtType { get; set; }
        public decimal Amount { get; set; }
        public decimal RemainingAmount { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime DateIncurred { get; set; }
        public string Note { get; set; }
    }
}
