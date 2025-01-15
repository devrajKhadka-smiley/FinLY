using FinLY.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using FinLY.Services;

namespace FinLY.Services
{
    public class UserBalawnceServicees : IUserBalanceServicees
    {
        private readonly string FinLyFilePath = Path.Combine(AppContext.BaseDirectory, "FinLYDatabaseUserBalance.json");
        private readonly IDebtsServices debtsServices;
        private readonly IUserBalanceServicees userBalanceServicees;

        public UserBalawnceServicees(IDebtsServices debtsServices)
        {
            this.debtsServices = debtsServices;
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

            // **Ensure the debt amount is updated whenever the balance is updated**
            await UpdateDebtRemainingAmountAsync(userId, userBalance);

            // Recalculate Available Balance with Debts:
            userBalance.AvailableBalancewithDebt = userBalance.TotalCashInFlow + userBalance.DebtRemainingAmount - userBalance.TotalCashOutFlow;

            // Recalculate Available Balance (Cash Inflow - Cash Outflow)
            userBalance.AvailableBalance = userBalance.TotalCashInFlow - userBalance.TotalCashOutFlow;

            // Save updated balances back to file
            await SaveAllBalancesAsync(balances);
        }

        public async Task UpdateDebtRemainingAmountAsync(Guid userId, UserBalance userBalance)
        {
            // Calculate the total remaining amount from debts
            var totalDebtRemaining = await CalculateDebtRemainingAmount(userId);

            // Update the DebtRemainingAmount field
            userBalance.DebtRemainingAmount = totalDebtRemaining;

            // **Update the TotalDebtAmount here as well**
            userBalance.TotalDebtAmount = await CalculateTotalDebtAmount(userId);

            // Save the updated user balance back to the file
            var balances = await LoadAllBalancesAsync();
            var existingBalance = balances.FirstOrDefault(b => b.UserId == userId);

            if (existingBalance == null)
            {
                balances.Add(userBalance);
            }

            await SaveAllBalancesAsync(balances);
        }

        private async Task<decimal> CalculateDebtRemainingAmount(Guid userId)
        {
            // Fetch debts for the user
            var debts = await debtsServices.GetDebtsByUserIdAsync(userId);
            if (debts == null || !debts.Any())
                return 0;

            // Sum up the remaining amounts of all the debts
            return debts.Sum(debt => debt.RemainingAmount);
        }

        private async Task<decimal> CalculateTotalDebtAmount(Guid userId)
        {
            var debts = await debtsServices.GetDebtsByUserIdAsync(userId);
            if (debts == null || !debts.Any())
                return 0;

            // Sum up the total debt amount (considering unpaid debts)
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

        public async Task UpdateTotalClearedDebtAmountAsync(Guid userId, decimal totalClearedDebtAmount)
        {
            var balances = await LoadAllBalancesAsync();
            var userBalance = balances.FirstOrDefault(b => b.UserId == userId);

            if (userBalance != null)
            {
                userBalance.DebtClearedAmount = totalClearedDebtAmount;
                await SaveAllBalancesAsync(balances);
            }
        }


    }
}
