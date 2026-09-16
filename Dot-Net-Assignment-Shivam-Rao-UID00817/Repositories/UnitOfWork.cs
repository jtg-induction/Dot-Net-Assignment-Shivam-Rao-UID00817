using Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly Restaurant_ManagementContext _db;

        public UnitOfWork(Restaurant_ManagementContext db)
        {
            _db = db;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
