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
        Task AddDebtAsync(Debts debt);
        Task<List<Debts>> GetDebtsByUserIdAsync(Guid userId);
        Task<Debts> GetDebtByIdAsync(Guid debtId);
        Task UpdateDebtAsync(Debts debt);
    }
}
