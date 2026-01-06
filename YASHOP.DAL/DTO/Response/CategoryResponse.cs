using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YASHOP.DAL.Models;

namespace YASHOP.DAL.DTO.Response
{
    public class CategoryResponse
    {
        public Status Status { get; set; }
        public List<CategoryTranslationResponse> Translations { get; set; }
    }
}
