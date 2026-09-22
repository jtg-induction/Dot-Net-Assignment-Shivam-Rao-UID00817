using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Services.Interfaces
{
    public interface IBrowseService
    {
        Task<List<BrowseRestaurantsResponseDto>> GetRestaurantsAsync();

        Task<BrowseMenuResponseDto> GetItemsAsync(long restaurant_id);
    }
}
