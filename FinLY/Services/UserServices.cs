﻿using FinLY.Models;
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

        public async Task SaveUserAsync(Users user)
        {
            List<Users> users = await LoadUsersAsync();

            user.UserId = Guid.NewGuid();  
            user.Password = HashPassword(user.Password);  

            users.Add(user);

            var jsonString = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });

            await File.WriteAllTextAsync(FinLyFilePath, jsonString);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

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

        public bool VerifyPassword(string enteredPassword, string storedPasswordHash)
        {
            var enteredPasswordHash = HashPassword(enteredPassword);
            return enteredPasswordHash == storedPasswordHash;
        }

        public async Task<Users> GetUserByUsernameAsync(string username)
        {
            var users = await LoadUsersAsync();
            return users.FirstOrDefault(u => u.UserName == username);
        }

        public async Task UpdateUserAsync(Users user)
        {
            try
            {
                var users = await LoadUsersAsync();
                var existingUser = users.FirstOrDefault(u => u.UserId == user.UserId);

                if (existingUser != null)
                {
                    existingUser.UserName = user.UserName;
                    existingUser.Currency = user.Currency;
                    //existingUser.AvailableBalance = user.AvailableBalance;
                    //existingUser.TotalCashInFlow = user.TotalCashInFlow;
                    //existingUser.TotalCashOutFlow = user.TotalCashOutFlow;
                    //existingUser.TotalDebtAmount = user.TotalDebtAmount;
                    //existingUser.AvailableBalancewithDebt = user.AvailableBalancewithDebt;

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

        private async Task SaveUsersAsync(List<Users> users)
        {
            var jsonString = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(FinLyFilePath, jsonString);
        }

        public async Task<Users> GetUserByIdAsync(Guid userId)
        {
            var users = await LoadUsersAsync();
            return users.FirstOrDefault(u => u.UserId == userId);
        }

    }
}
