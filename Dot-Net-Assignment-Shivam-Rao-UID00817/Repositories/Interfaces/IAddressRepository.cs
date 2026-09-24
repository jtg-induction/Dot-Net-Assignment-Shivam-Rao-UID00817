using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces
{
    public interface IAddressRepository
    {
        void Add(Addresses address);
        Task<Addresses> GetAddressAsync(long addressId, bool enableTracking, CancellationToken cancellationToken = default);
        Task RemoveAsync(long addressId, CancellationToken cancellationToken = default);
    }
}
