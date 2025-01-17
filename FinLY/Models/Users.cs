using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinLY.Models
{
    public class Users
    {
        public Guid UserId { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Currency { get; set; } = string.Empty;


        //public decimal AvailableBalance { get; set; } = 0;
        //public decimal TotalCashInFlow { get; set; } = 0;
        //public decimal TotalCashOutFlow { get; set; } = 0;
        //public decimal TotalDebtAmount { get; set; } = 0;
        //public decimal AvailableBalancewithDebt { get; set; } = 0;
    }
}
