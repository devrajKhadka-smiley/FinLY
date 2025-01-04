using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinLY.Models
{
    public class DebtsPayment
    {
        public Guid Id { get; set; }
        public Guid DebtId { get; set; }
        public Guid UserId { get; set; }
        public decimal AmountPaid { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public string Note { get; set; }
    }
}
