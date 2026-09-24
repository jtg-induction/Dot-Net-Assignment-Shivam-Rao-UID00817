using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Security;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Controllers
{
    [RoutePrefix("api/public/restaurants")]
    public class RestaurantController: ApiController
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpGet, Route("")]
        public async Task<HttpResponseMessage> Restaurant(int pageNumber = 1)
        {
            var result = await _restaurantService.GetRestaurantsAsync(pageNumber);

            return Request.CreateResponse(HttpStatusCode.OK , result);
        }

        [HttpGet, Route("{restaurantId}")]
        public async Task<HttpResponseMessage> Menu(long restaurantId,int pageNumber = 1)
        {
            var result = await _restaurantService.GetItemsAsync(pageNumber, restaurantId);

            return Request.CreateResponse(HttpStatusCode.OK ,result);
        }
    }
}
