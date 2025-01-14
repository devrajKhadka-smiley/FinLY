using FinLY.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FinLY.Services
{
    public class UserServices : IUserServices
    {
        private readonly string FinLyFilePath = Path.Combine(AppContext.BaseDirectory, "FinLYDatabase.json");

        // Method to save a single user to the database
        public async Task SaveUserAsync(Users user)
        {
            List<Users> users = await LoadUsersAsync();

            user.UserId = Guid.NewGuid();  // Assign a new GUID if this is a new user
            user.Password = HashPassword(user.Password);  // Hash the password

            users.Add(user);

            var jsonString = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });

            await File.WriteAllTextAsync(FinLyFilePath, jsonString);
        }

        // Method to hash a user's password
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        // Method to load users from the JSON file
        public async Task<List<Users>> LoadUsersAsync()
        {
            if (!File.Exists(FinLyFilePath))
            {
                return new List<Users>();
            }

            var jsonString = await File.ReadAllTextAsync(FinLyFilePath);
            var users = JsonSerializer.Deserialize<List<Users>>(jsonString);

            return users ?? new List<Users>();
        }

        // Method to verify the user's password
        public bool VerifyPassword(string enteredPassword, string storedPasswordHash)
        {
            var enteredPasswordHash = HashPassword(enteredPassword);
            return enteredPasswordHash == storedPasswordHash;
        }

        // Method to get a user by their username
        public async Task<Users> GetUserByUsernameAsync(string username)
        {
            var users = await LoadUsersAsync();
            return users.FirstOrDefault(u => u.UserName == username);
        }

        // Method to update an existing user's information
        public async Task UpdateUserAsync(Users user)
        {
            try
            {
                var users = await LoadUsersAsync();
                var existingUser = users.FirstOrDefault(u => u.UserId == user.UserId);

                if (existingUser != null)
                {
                    // Update the user's properties
                    existingUser.UserName = user.UserName;
                    existingUser.Currency = user.Currency;
                    existingUser.AvailableBalance = user.AvailableBalance;
                    existingUser.TotalCashInFlow = user.TotalCashInFlow;
                    existingUser.TotalCashOutFlow = user.TotalCashOutFlow;
                    existingUser.TotalDebtAmount = user.TotalDebtAmount;
                    existingUser.AvailableBalancewithDebt = user.AvailableBalancewithDebt;

                    // Save the updated users list
                    await SaveUsersAsync(users);
                }
                else
                {
                    Console.WriteLine("User not found for update.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating user: {ex.Message}");
                throw;
            }
        }

        // Method to save the updated list of users
        private async Task SaveUsersAsync(List<Users> users)
        {
            var jsonString = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(FinLyFilePath, jsonString);
        }

        // Method to get a user by their UserId
        public async Task<Users> GetUserByIdAsync(Guid userId)
        {
            var users = await LoadUsersAsync();
            return users.FirstOrDefault(u => u.UserId == userId);
        }

    }
}
