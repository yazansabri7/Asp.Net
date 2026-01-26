using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YASHOP.DAL.Models;

namespace YASHOP.DAL.Repository
{
    public interface IProductRepository
    {
        Task<Product> AddAsync(Product request);
    }
}
