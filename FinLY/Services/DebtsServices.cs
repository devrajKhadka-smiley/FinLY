using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinLY.Models;

namespace FinLY.Services
{
    public class DebtsServices : IDebtsServices
    {
        private readonly string FinLyFilePath = Path.Combine(AppContext.BaseDirectory, "FinLYDatabaseDebts.json");

        public async Task AddDebtAsync(Debts debt)
        {
            try
            {
                var debts = await LoadDebtsAsync();
                debt.Id = Guid.NewGuid();  
                debts.Add(debt);
                await SaveDebtsAsync(debts);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding debt: {ex.Message}");
            }
        }

        public async Task<List<Debts>> GetDebtsByUserIdAsync(Guid userId)
        {
            try
            {
                var debts = await LoadDebtsAsync();
                return debts.Where(d => d.UserId == userId).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting debts: {ex.Message}");
                return new List<Debts>();
            }
        }

        public async Task<Debts> GetDebtByIdAsync(Guid debtId)
        {
            try
            {
                var debts = await LoadDebtsAsync();
                return debts.FirstOrDefault(d => d.Id == debtId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting debt by ID: {ex.Message}");
                return null;
            }
        }

        public async Task UpdateDebtAsync(Debts debt)
        {
            try
            {
                var debts = await LoadDebtsAsync();
                var existingDebt = debts.FirstOrDefault(d => d.Id == debt.Id);
                if (existingDebt != null)
                {
                    existingDebt.DebtType = debt.DebtType;
                    existingDebt.Amount = debt.Amount;
                    existingDebt.RemainingAmount = debt.RemainingAmount;
                    existingDebt.DueDate = debt.DueDate;
                    existingDebt.DateIncurred = debt.DateIncurred;
                    existingDebt.Note = debt.Note;
                    await SaveDebtsAsync(debts);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating debt: {ex.Message}");
            }
        }

        public async Task DeleteDebtAsync(Guid debtId)
        {
            try
            {
                var debts = await LoadDebtsAsync();
                var debtToRemove = debts.FirstOrDefault(d => d.Id == debtId);
                if (debtToRemove != null)
                {
                    debts.Remove(debtToRemove);
                    await SaveDebtsAsync(debts);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting debt: {ex.Message}");
            }
        }

        private async Task<List<Debts>> LoadDebtsAsync()
        {
            try
            {
                if (!System.IO.File.Exists(FinLyFilePath))
                {
                    return new List<Debts>();
                }
                var json = await System.IO.File.ReadAllTextAsync(FinLyFilePath);
                return System.Text.Json.JsonSerializer.Deserialize<List<Debts>>(json) ?? new List<Debts>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading debts: {ex.Message}");
                return new List<Debts>();
            }
        }

        private async Task SaveDebtsAsync(List<Debts> debts)
        {
            try
            {
                var json = System.Text.Json.JsonSerializer.Serialize(debts, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
                await System.IO.File.WriteAllTextAsync(FinLyFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving debts: {ex.Message}");
            }
        }
    }
}
