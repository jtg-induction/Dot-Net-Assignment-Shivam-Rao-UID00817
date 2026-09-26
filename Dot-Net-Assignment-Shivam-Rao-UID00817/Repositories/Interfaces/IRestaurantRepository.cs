using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces
{
    public interface IRestaurantRepository
    {
        void Add(Restaurants restaurant);
        Task<Restaurants> GetRestaurantAsync(string restaurantName , bool enableTracking);
    }
}
