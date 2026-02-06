using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YASHOP.DAL.DTO.Request
{
    public class CreateReviewRequest
    {
        [Range(1,5)]
        [Required]
        public int Rating { get; set; }
        [MinLength(5)]
        [Required]
        public string Comment { get; set; } 
    }
}
