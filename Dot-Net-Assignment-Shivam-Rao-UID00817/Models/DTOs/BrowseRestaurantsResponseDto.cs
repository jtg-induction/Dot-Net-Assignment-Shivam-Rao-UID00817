using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs
{
    public class BrowseRestaurantsResponseDto
    {
        public long RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State {  get; set; }
        public string Pincode { get; set; }
        public string Country { get; set; }

        public BrowseRestaurantsResponseDto(long restaurantId, string name, string addressLine1, string city, string state, string pincode, string country, string addressLine2  = "")
        {
            this.RestaurantId = restaurantId;
            this.RestaurantName = name;
            this.AddressLine1 = addressLine1;
            this.AddressLine2 = addressLine2;
            this.City = city;
            this.State = state;
            this.Pincode = pincode;
            this.Country = country;
        }

        public BrowseRestaurantsResponseDto()
        {

        }
    }

}
