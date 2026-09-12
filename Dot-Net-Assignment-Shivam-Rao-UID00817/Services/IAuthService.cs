using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Services
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterDto model);
    }
}
