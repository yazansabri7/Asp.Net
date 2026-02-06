using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YASHOP.DAL.DTO.Request;
using YASHOP.DAL.DTO.Response;

namespace YASHOP.BLL.Service.Interfaces
{
    public interface IReviewService
    {
        Task<BaseResponse> AddReviewAsync(string userId, int productId, CreateReviewRequest request);
    }
}
