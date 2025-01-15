using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinLY.Models;

namespace FinLY.Services
{
    public interface IUserBalanceServicees
    {
        Task<UserBalance> GetUserBalanceAsync(Guid userId);
        Task UpdateUserBalanceAsync(Guid userId, decimal amount, string transactionType);
        Task UpdateTotalClearedDebtAmountAsync(Guid userId, decimal totalClearedDebtAmount);  
    }
}
