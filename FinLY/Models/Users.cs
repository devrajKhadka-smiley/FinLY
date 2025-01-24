using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinLY.Models
{
    public class Users
    {
        public Guid UserId { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Currency { get; set; } = string.Empty;
    }
}
