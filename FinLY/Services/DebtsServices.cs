using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinLY.Models;
using FinLY.Services;

namespace FinLY.Services
{
    public class DebtsServices : IDebtsServices
    {
        private readonly string FinLyFilePath = Path.Combine(AppContext.BaseDirectory, "FinLYDatabaseDebts.json");

        public async Task AddDebtAsync(UserDebt debt)
        {
            try
            {
                var debts = await LoadDebtsAsync();
                debt.Id = Guid.NewGuid();
                debt.RemainingAmount = debt.TotalDebtAmount - debt.PaidAmount;
                debts.Add(debt);
                await SaveDebtsAsync(debts);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding debt: {ex.Message}");
            }
        }

        public async Task<List<UserDebt>> GetDebtsByUserIdAsync(Guid userId)
        {
            try
            {
                var debts = await LoadDebtsAsync();
                return debts.Where(d => d.UserId == userId).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting debts for userId {userId}: {ex.Message}");
                return new List<UserDebt>();
            }
        }

        public async Task<UserDebt> GetDebtByIdAsync(Guid debtId)
        {
            try
            {
                var debts = await LoadDebtsAsync();
                return debts.FirstOrDefault(d => d.Id == debtId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting debt by ID {debtId}: {ex.Message}");
                return null;
            }
        }

        public async Task UpdateDebtAsync(UserDebt debt)
        {
            try
            {
                if (debt == null)
                {
                    Console.WriteLine("Attempted to update a null debt");
                    return;
                }

                var debts = await LoadDebtsAsync();
                var existingDebt = debts.FirstOrDefault(d => d.Id == debt.Id);
                if (existingDebt != null)
                {
                    existingDebt.DebtTitle = debt.DebtTitle;
                    existingDebt.TotalDebtAmount = debt.TotalDebtAmount;
                    existingDebt.RemainingAmount = debt.RemainingAmount;
                    existingDebt.PaidAmount = debt.PaidAmount;
                    existingDebt.DueDate = debt.DueDate;
                    existingDebt.SourceFrom = debt.SourceFrom;
                    existingDebt.Note = debt.Note;
                    existingDebt.DebtStatus = debt.DebtStatus;

                    await SaveDebtsAsync(debts);
                }
                else
                {
                    Console.WriteLine($"Debt with ID {debt.Id} not found for update");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating debt with ID {debt.Id}: {ex.Message}");
            }
        }

        private async Task<List<UserDebt>> LoadDebtsAsync()
        {
            try
            {
                if (!System.IO.File.Exists(FinLyFilePath))
                {
                    return new List<UserDebt>();
                }

                var json = await System.IO.File.ReadAllTextAsync(FinLyFilePath);
                return System.Text.Json.JsonSerializer.Deserialize<List<UserDebt>>(json) ?? new List<UserDebt>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading debts from file: {ex.Message}");
                return new List<UserDebt>();
            }
        }

        private async Task SaveDebtsAsync(List<UserDebt> debts)
        {
            try
            {
                var json = System.Text.Json.JsonSerializer.Serialize(debts, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
                await System.IO.File.WriteAllTextAsync(FinLyFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving debts to file: {ex.Message}");
            }
        }

        private async Task<decimal> CalculateTotalDebtAmount(Guid userId)
        {
            var debtsServices = new DebtsServices(); 

            var userDebts = await debtsServices.GetDebtsByUserIdAsync(userId);

            if (userDebts == null || !userDebts.Any())
            {
                return 0; 
            }

            decimal totalDebtAmount = userDebts.Sum(debt => debt.TotalDebtAmount);

            return totalDebtAmount;
        }

    }
}
