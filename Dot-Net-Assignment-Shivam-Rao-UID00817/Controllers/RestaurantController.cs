using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Services.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Controllers
{
    [RoutePrefix("api/public/restaurants")]
    public class RestaurantController : ApiController
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpGet, Route("")]
        public async Task<HttpResponseMessage> Restaurant(int pageNumber = 1, int pageSize = NumberConstants.PAGE_SIZE)
        {
            if (pageNumber < 1 || pageSize < 1) return Request.CreateResponse(HttpStatusCode.NotFound);
            var result = await _restaurantService.GetRestaurantsAsync(pageNumber, pageSize);

            if (result.Restaurants.Count == 0) return Request.CreateResponse(HttpStatusCode.NotFound);

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpGet, Route("{restaurantId}")]
        public async Task<HttpResponseMessage> Menu(long restaurantId, int pageNumber = 1, int pageSize = NumberConstants.PAGE_SIZE)
        {
            if (pageNumber < 1 || pageSize < 1) return Request.CreateResponse(HttpStatusCode.NotFound);
            var result = await _restaurantService.GetItemsAsync(restaurantId, pageNumber, pageSize);

            if (result.items.Count == 0) return Request.CreateResponse(HttpStatusCode.NotFound);

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
