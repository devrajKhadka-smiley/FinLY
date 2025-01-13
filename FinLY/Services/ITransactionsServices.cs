using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinLY.Models;

namespace FinLY.Services
{
    public interface ITransactionsServices
    {
        Task AddTransactionAsync(UserTransaction transaction);
        Task<List<UserTransaction>> GetTransactionsByUserIdAsync(Guid userId);
    }
}
