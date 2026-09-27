using System.Collections.Generic;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs
{
    public class OnboardOwnersIdDto
    {
        public long RestaurantId { get; set; }

        public List<long> UserIds { get; set; } = new List<long>();

        public OnboardOwnersIdDto(long restaurantId, List<long> userIds)
        {
            RestaurantId = restaurantId;

            UserIds.AddRange(userIds);

        }
    }
}
