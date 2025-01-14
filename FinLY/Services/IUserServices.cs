using FinLY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinLY.Services
{
    public interface IUserServices
    {
        Task SaveUserAsync(Users user);

        Task<List<Users>> LoadUsersAsync();

        Task UpdateUserAsync(Users user);
    }
}
