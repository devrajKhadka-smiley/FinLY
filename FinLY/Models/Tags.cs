using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinLY.Models
{
    public class Tags
    {
        public Guid UserId { get; set; } = Guid.Empty;
        public Guid TagId { get; set; }
        public string TagName { get; set; } = string.Empty;
        public bool IsDefault { get; set; }
        public string TagType { get; set; } = string.Empty;
    }
}
