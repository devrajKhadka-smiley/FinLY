using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinLY.Models
{
    public class UserBalance
    {
        public Guid UserId { get; set; }
        public decimal AvailableBalance { get; set; } = 0;

        public decimal TotalCashInFlow { get; set; } = 0;

        public decimal TotalCashOutFlow { get; set; } = 0;

        public decimal TotalDebtAmount { get; set; } = 0;

        public decimal AvailableBalancewithDebt { get; set; } = 0;

    }
}
