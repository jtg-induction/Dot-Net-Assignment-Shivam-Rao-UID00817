using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using System.Threading.Tasks;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces
{
    public interface IAddressRepository
    {
        void Add(Addresses address);
        Task<Addresses> GetAddressAsync(long addressId, bool enableTracking);
        Task RemoveAsync(long addressId);
    }
}
