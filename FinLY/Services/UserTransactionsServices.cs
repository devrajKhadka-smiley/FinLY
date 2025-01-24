using FinLY.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace FinLY.Services
{
    //here usertransactionserive is inherited from IUserTransactionServices
    //usertransactionserice hides the details of how transaction are stored and retrieved
    public class UserTransactionsServices : IUserTransactionServices
    {


        //private readonly string FinLyFilePath = Path.Combine(AppContext.BaseDirectory, "FinLYDatabaseUserTransactions.json");
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
            //return Path.Combine(FinLYDatabaseFolder, "FinLYDatabaseUserTransactions.json");
            return Path.Combine(FinLYDatabaseFolder, "Transaction Database.json");
        }


        public async Task AddTransactionAsync(UserTransaction transaction)
        {
            try
            {
                var transactions = await LoadTransactionsAsync();
                transaction.Id = Guid.NewGuid(); 

                transactions.Add(transaction);

                await SaveTransactionsAsync(transactions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Adding Transaction: {ex.Message}");
            }
        }

        private async Task<List<UserTransaction>> LoadTransactionsAsync()
        {
            try
            {
                string FinLYTransactionFilePath = GetTagFilePath();

                if (!File.Exists(FinLYTransactionFilePath))
                {
                    return new List<UserTransaction>();
                }

                var json = await File.ReadAllTextAsync(FinLYTransactionFilePath);
                return JsonSerializer.Deserialize<List<UserTransaction>>(json) ?? new List<UserTransaction>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading transactions: {ex.Message}");
                throw;
            }
        }

        public async Task<List<UserTransaction>> GetTransactionsByUserIdAsync(Guid userId)
        {
            try
            {
                var transactions = await LoadTransactionsAsync();
                return transactions.Where(t => t.UserId == userId).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting transactions: {ex.Message}");
                return new List<UserTransaction>();
            }
        }

        private async Task SaveTransactionsAsync(List<UserTransaction> transactions)
        {
            try
            {
                string FinLYTransactionFilePath = GetTagFilePath();


                var json = JsonSerializer.Serialize(transactions, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(FinLYTransactionFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving transactions: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateTransactionAsync(UserTransaction transaction)
        {
            try
            {
                if (transaction == null)
                {
                    throw new ArgumentNullException(nameof(transaction), "Transaction cannot be null");
                }

                var transactions = await LoadTransactionsAsync();
                var existingTransaction = transactions.FirstOrDefault(t => t.Id == transaction.Id);
                if (existingTransaction != null)
                {
                    existingTransaction.TransactionTitle = transaction.TransactionTitle;
                    existingTransaction.TransactionType = transaction.TransactionType;
                    existingTransaction.Amounts = transaction.Amounts;
                    existingTransaction.Tag = transaction.Tag;
                    existingTransaction.Note = transaction.Note;
                    existingTransaction.TransactionDate = transaction.TransactionDate;
                    await SaveTransactionsAsync(transactions);
                }
                else
                {
                    Console.WriteLine($"Transaction with ID {transaction.Id} not found for update");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating transaction: {ex.Message}");
                throw;
            }
        }
    }
}
