using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories
{
    public class AddressRepository: IAddressRepository
    {
        private readonly Restaurant_ManagementContext _db;

        public AddressRepository(Restaurant_ManagementContext db)
        {
            _db = db;
        }

        public void Add(Addresses address)
        {
            _db.Addresses.Add(address);
        }

        public async Task<Addresses> GetAddressAsync(long addressId, bool enableTracking)
        {
            if (enableTracking)
            {
                return await _db.Addresses.FindAsync(addressId);
            }
            else
            {
                return await _db.Addresses.AsNoTracking().FirstOrDefaultAsync( u => u.AddressId == addressId);
            }
        }

        public async Task RemoveAsync(long addressId)
        {
            _db.Addresses.Remove(await GetAddressAsync(addressId, true));
        }
    }
}
