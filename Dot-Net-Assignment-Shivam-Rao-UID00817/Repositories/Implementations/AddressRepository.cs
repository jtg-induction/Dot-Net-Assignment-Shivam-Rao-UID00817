using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly Restaurant_ManagementContext _db;

        public AddressRepository(Restaurant_ManagementContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Adds an address to the database context.
        /// </summary>
        /// <param name="address">The address to add.</param>
        public void Add(Addresses address)
        {
            _db.Addresses.Add(address);
        }

        /// <summary>
        /// Retrieves an address by its ID, with optional change tracking.
        /// </summary>
        /// <param name="addressId">The ID of the address to retrieve.</param>
        /// <param name="enableTracking">Whether to enable entity tracking.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>The address if found; otherwise, null.</returns>
        public async Task<Addresses> GetAddressAsync(long addressId, bool enableTracking, CancellationToken cancellationToken = default)
        {
            if (enableTracking)
            {
                return await _db.Addresses.FindAsync(addressId);
            }
            else
            {
                return await _db.Addresses.AsNoTracking().FirstOrDefaultAsync(u => u.AddressId == addressId);
            }
        }

        /// <summary>
        /// Removes an address from the database context by its ID.
        /// </summary>
        /// <param name="addressId">The ID of the address to remove.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        public async Task RemoveAsync(long addressId, CancellationToken cancellationToken = default)
        {
            _db.Addresses.Remove(await GetAddressAsync(addressId, true));
        }
    }
}
