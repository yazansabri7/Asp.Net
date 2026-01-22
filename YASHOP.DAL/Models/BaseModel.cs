using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YASHOP.DAL.Models
{
    public class BaseModel
    {
        public int Id { get; set; }
        public Status Status { get; set; } = Status.active;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
