using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinLY.Models
{
    public class UserDebt
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string DebtTitle { get; set; }
        public decimal TotalDebtAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public DateTime DueDate { get; set; } = DateTime.Today;
        public string SourceFrom { get; set; }
        public string Note { get; set; }
        public string DebtStatus { get; set; }

    }
}
