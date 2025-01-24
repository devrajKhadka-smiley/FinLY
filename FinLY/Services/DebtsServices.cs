using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinLY.Models;
using FinLY.Services;

namespace FinLY.Services
{
    //debt service hides the details of how debts are stored and retrieved
    public class DebtsServices : IDebtsServices
    {
        //Field Decelaration to save the file path

        private static string GetUserFilePath()
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
            //return Path.Combine(FinLYDatabaseFolder, "FinLYDatabaseDebts.json");
            return Path.Combine(FinLYDatabaseFolder, "Debts Database.json");
        }


        //private readonly string FinLyFilePath = Path.Combine(AppContext.BaseDirectory, "FinLYDatabaseDebts.json");
        private readonly IUserTransactionServices _transactionService;
        public DebtsServices(IUserTransactionServices transactionService)
        {
            _transactionService = transactionService;
        }

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
                string databaseFilePath = GetUserFilePath();

                if (!System.IO.File.Exists(databaseFilePath))
                {
                    return new List<UserDebt>();
                }

                var json = await System.IO.File.ReadAllTextAsync(databaseFilePath);
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
                string databaseFilePath = GetUserFilePath();

                // Serialization: Convert the list of UserDebt objects into JSON and save it to the file
                var json = System.Text.Json.JsonSerializer.Serialize(debts, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
                await System.IO.File.WriteAllTextAsync(databaseFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving debts to file: {ex.Message}");
            }
        }


        private async Task<decimal> CalculateTotalDebtAmount(Guid userId)
        {
            IUserTransactionServices transactionService = new UserTransactionsServices();

            var debtsServices = new DebtsServices(transactionService);

            var userDebts = await debtsServices.GetDebtsByUserIdAsync(userId);

            if (userDebts == null || !userDebts.Any())
            {
                return 0;
            }

            //decimal totalDebtAmount = userDebts.Sum(debt => debt.TotalDebtAmount);
            decimal totalDebtAmount = userDebts
        .Where(debt => debt.DebtStatus == "Pending" || debt.DebtStatus == "Paid")
        .Sum(debt => debt.TotalDebtAmount);


            return totalDebtAmount;
        }

    }
}
