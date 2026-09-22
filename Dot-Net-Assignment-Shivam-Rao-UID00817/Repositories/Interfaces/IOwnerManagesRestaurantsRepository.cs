using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces
{
    public interface IOwnerManagesRestaurantsRepository
    {
        void Add(List<Owner_Manages_Restaurants> Owners);

        Task<Owner_Manages_Restaurants> GetOwnerIfExistsAsync(long userId , long restaurantId , bool enableTracking);

        void Remove(Owner_Manages_Restaurants Entry);
    }
}
