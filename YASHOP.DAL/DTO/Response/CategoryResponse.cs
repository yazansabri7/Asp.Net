using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using YASHOP.DAL.Models;

namespace YASHOP.DAL.DTO.Response
{
    public class CategoryResponse
    {
        public int Id { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Status Status { get; set; }
        public string CraetedBy { get; set; }
        public List<CategoryTranslationResponse> Translations { get; set; }
    }
}
