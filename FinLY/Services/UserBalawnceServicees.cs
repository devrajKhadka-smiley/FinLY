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
        private static string GetTagFilePath()
        {
            string FinLYDocumentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // Define the folder where the file will be stored
            string FinLYDatabaseFolder = Path.Combine(FinLYDocumentPath, "FinLY Database");

            // Create the directory if it does not exist
            if (!Directory.Exists(FinLYDatabaseFolder))
            {
                Directory.CreateDirectory(FinLYDatabaseFolder);
            }

            // Return the full file path for the JSON file
            //return Path.Combine(FinLYDatabaseFolder, "FinLYDatabaseUserBalance.json");
            return Path.Combine(FinLYDatabaseFolder, "User Balance Database.json");
        }


        //private readonly string FinLyFilePath = Path.Combine(AppContext.BaseDirectory, "FinLYDatabaseUserBalance.json");
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

            if (transactionType == "InFlow")
            {
                userBalance.TotalCashInFlow += amount;
            }
            else if (transactionType == "OutFlow")
            {
                userBalance.TotalCashOutFlow += amount;
            }

            await UpdateDebtRemainingAmountAsync(userId, userBalance);

            userBalance.AvailableBalancewithDebt = userBalance.TotalCashInFlow + userBalance.DebtRemainingAmount - userBalance.TotalCashOutFlow;

            userBalance.AvailableBalance = userBalance.TotalCashInFlow - userBalance.TotalCashOutFlow;

            await SaveAllBalancesAsync(balances);
        }

        public async Task UpdateDebtRemainingAmountAsync(Guid userId, UserBalance userBalance)
        {
            var totalDebtRemaining = await CalculateDebtRemainingAmount(userId);

            userBalance.DebtRemainingAmount = totalDebtRemaining;

            userBalance.TotalDebtAmount = await CalculateTotalDebtAmount(userId);

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
            var debts = await debtsServices.GetDebtsByUserIdAsync(userId);
            if (debts == null || !debts.Any())
                return 0;

            return debts.Sum(debt => debt.RemainingAmount);
        }

        private async Task<decimal> CalculateTotalDebtAmount(Guid userId)
        {
            var debts = await debtsServices.GetDebtsByUserIdAsync(userId);
            if (debts == null || !debts.Any())
                return 0;

            //return debts.Where(d => d.DebtStatus != "Paid").Sum(d => d.TotalDebtAmount);
            return debts.Where(d => d.DebtStatus == "Pending" || d.DebtStatus == "Paid")
               .Sum(d => d.TotalDebtAmount);
        }

        private async Task<List<UserBalance>> LoadAllBalancesAsync()
        {
            string userBalanceFilePath = GetTagFilePath();

            if (!File.Exists(userBalanceFilePath))
            {
                return new List<UserBalance>();
            }

            var json = await File.ReadAllTextAsync(userBalanceFilePath);
            return JsonSerializer.Deserialize<List<UserBalance>>(json) ?? new List<UserBalance>();
        }

        private async Task SaveAllBalancesAsync(List<UserBalance> balances)
        {
            string userBalanceFilePath = GetTagFilePath();


            var json = JsonSerializer.Serialize(balances, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(userBalanceFilePath, json);
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
