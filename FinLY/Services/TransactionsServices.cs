using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using FinLY.Models;

namespace FinLY.Services
{
    public class TransactionsServices : ITransactionsServices
    {
        private readonly string FinLyFilePath = Path.Combine(AppContext.BaseDirectory, "FinLYDatabaseTransactions.json");

        public async Task AddTransactionAsync(Transactions transaction)
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
                Console.WriteLine($"Error adding transaction: {ex.Message}");
                throw; 
            }
        }

        private async Task<List<Transactions>> LoadTransactionsAsync()
        {
            try
            {
                if (!File.Exists(FinLyFilePath))
                {
                    return new List<Transactions>();
                }

                var json = await File.ReadAllTextAsync(FinLyFilePath);
                return JsonSerializer.Deserialize<List<Transactions>>(json) ?? new List<Transactions>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading transactions: {ex.Message}");
                throw;
            }
        }

        public async Task<List<Transactions>> GetTransactionsByUserIdAsync(Guid userId)
        {
            try
            {
                var transactions = await LoadTransactionsAsync();

                var userTransactions = transactions.Where(t => t.UserId == userId).ToList();

                return userTransactions;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting transactions: {ex.Message}");
                return new List<Transactions>();
            }
        }

        private async Task SaveTransactionsAsync(List<Transactions> transactions)
        {
            try
            {
                var json = JsonSerializer.Serialize(transactions, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(FinLyFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving transactions: {ex.Message}");
                throw;
            }
        }
    }
}
