using FinLY.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FinLY.Services;


namespace FinLY.Services
{
    public class UserBalawnceServicees : IUserBalanceServicees
    {
        private readonly string FinLyFilePath = Path.Combine(AppContext.BaseDirectory, "FinLYDatabaseUserBalance.json");
        private readonly IDebtsServices debtsServices; // Add this field

        // Inject IDebtsServices via the constructor
        public UserBalawnceServicees(IDebtsServices debtsServices)
        {
            this.debtsServices = debtsServices; // Assign the injected service to the field
        }

        public async Task<UserBalance> GetUserBalanceAsync(Guid userId)
        {
            var balances = await LoadAllBalancesAsync();
            return balances.FirstOrDefault(b => b.UserId == userId) ?? new UserBalance { UserId = userId };
        }

        public async Task UpdateUserBalanceAsync(Guid userId, decimal amount, string transactionType)
        {
            var balances = await LoadAllBalancesAsync();
            var userBalance = balances.FirstOrDefault(b => b.UserId == userId);

            if (userBalance == null)
            {
                userBalance = new UserBalance { UserId = userId };
                balances.Add(userBalance);
            }

            // Handle Cash Inflow or Outflow and update totals
            if (transactionType == "InFlow")
            {
                userBalance.TotalCashInFlow += amount;
            }
            else if (transactionType == "OutFlow")
            {
                userBalance.TotalCashOutFlow += amount;
            }

            // If the transaction type is related to inflow or outflow, update the debt amount
            if (transactionType == "InFlow" || transactionType == "OutFlow")
            {
                // Recalculate total debt amount
                userBalance.TotalDebtAmount = await GetTotalDebtAmount(userId);
            }

            // Recalculate Available Balance with Debts: 
            // (Total Cash Inflow + Total Debt Amount - Total Cash Outflow)
            userBalance.AvailableBalancewithDebt = userBalance.TotalCashInFlow + userBalance.TotalDebtAmount - userBalance.TotalCashOutFlow;

            // Recalculate Available Balance (Cash Inflow - Cash Outflow)
            userBalance.AvailableBalance = userBalance.TotalCashInFlow - userBalance.TotalCashOutFlow;

            // Save updated balances back to file
            await SaveAllBalancesAsync(balances);
        }


        // Helper method to calculate total debt for a user using the debts service
        private async Task<decimal> GetTotalDebtAmount(Guid userId)
        {
            // Use the injected debtsServices instance to fetch debts
            var debts = await debtsServices.GetDebtsByUserIdAsync(userId);
            // Sum up the total unpaid debts
            return debts.Where(d => d.DebtStatus != "Paid").Sum(d => d.TotalDebtAmount);
        }

        private async Task<List<UserBalance>> LoadAllBalancesAsync()
        {
            if (!File.Exists(FinLyFilePath))
            {
                return new List<UserBalance>();
            }

            var json = await File.ReadAllTextAsync(FinLyFilePath);
            return JsonSerializer.Deserialize<List<UserBalance>>(json) ?? new List<UserBalance>();
        }

        private async Task SaveAllBalancesAsync(List<UserBalance> balances)
        {
            var json = JsonSerializer.Serialize(balances, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(FinLyFilePath, json);
        }
    }

}
