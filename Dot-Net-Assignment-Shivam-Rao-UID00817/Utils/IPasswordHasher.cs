using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Utils
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);

        bool VerifyPassword(string p, string hash);
    }
}
