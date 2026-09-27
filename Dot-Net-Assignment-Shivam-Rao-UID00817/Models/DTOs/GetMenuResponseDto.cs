using System.Collections.Generic;
using System.Security.RightsManagement;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs
{
    public class GetMenuResponseDto
    {
        public GetRestaurantsResponseDto Restaurant { get; set; }
        public List<ItemAndPrice> items = new List<ItemAndPrice>();
        public PaginationMetadata Meta { get; set; }

        public GetMenuResponseDto(GetRestaurantsResponseDto restaurant, List<ItemAndPrice> items, PaginationMetadata meta)
        {
            Restaurant = restaurant;
            this.items = items;
            this.Meta = meta;
        }
    }

    public class ItemAndPrice
    {
        public long ItemId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public ItemAndPrice(long itemId, string name, decimal price)
        {
            ItemId = itemId;
            Name = name;
            Price = price;
        }
    }
}
