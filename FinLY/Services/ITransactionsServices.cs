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
        Task AddTransactionAsync(Transactions transaction);
        Task<List<Transactions>> GetTransactionsByUserIdAsync(Guid userId);
    }
}
