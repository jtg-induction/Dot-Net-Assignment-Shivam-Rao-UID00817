using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using System.Threading.Tasks;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> UserExistsAsync(string email , string phoneNumber);
        Task AddUserAsync(Users user);
    }
}
