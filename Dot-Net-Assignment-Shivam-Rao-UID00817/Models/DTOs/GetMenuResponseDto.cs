using System.Collections.Generic;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs
{
    public class GetMenuResponseDto
    {
        public GetRestaurantsResponseDto Restaurant { get; set; }
        public List<ItemAndPrice> items = new List<ItemAndPrice>();

        public GetMenuResponseDto(GetRestaurantsResponseDto restaurant, List<ItemAndPrice> items)
        {
            Restaurant = restaurant;
            this.items = items;
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
