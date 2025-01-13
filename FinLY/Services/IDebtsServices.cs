using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinLY.Models;

namespace FinLY.Services
{
    public  interface IDebtsServices
    {
        Task AddDebtAsync(UserDebt debt);
        Task<List<UserDebt>> GetDebtsByUserIdAsync(Guid userId);
        Task UpdateDebtAsync(UserDebt debt);
    }
}
