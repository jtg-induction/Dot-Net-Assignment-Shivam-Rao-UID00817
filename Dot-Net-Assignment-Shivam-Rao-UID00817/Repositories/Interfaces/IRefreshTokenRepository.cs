using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces
{
    public interface IRefreshTokenRepository
    {
        void Add(Refresh_Tokens RefreshToken);

        Task<Refresh_Tokens> CheckIfRefreshTokenExistsAsync(string token);

        void DeleteRefreshToken(Refresh_Tokens TokenRecord);
    }
}
