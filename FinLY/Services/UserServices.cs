using FinLY.Models;
using System;
using System.Collections.Generic;
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

            var jsonString = JsonSerializer.Serialize(users);

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
    }
}
