using FinLY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinLY.Services
{
    public interface ITagsServices
    {
        Task AddCustomTagAsync(Tags tag);
        Task<List<Tags>> GetTagsByUserIdAsync(Guid UserId);
    }
}
