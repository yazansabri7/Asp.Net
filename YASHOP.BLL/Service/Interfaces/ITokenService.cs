using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using YASHOP.DAL.Models;

namespace YASHOP.BLL.Service.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateAccessToken(ApplicationUser user);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
