using Dot_Net_Assignment_Shivam_Rao_UID00817.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Controllers
{
    [Authorize(Roles = "Owner")]
    [RoutePrefix("api/restaurants")]
    public class RestaurantOwnerController: ApiController
    {
        private readonly IOrderService _orderService;
        public RestaurantOwnerController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet, Route("{restaurantId}/orders")]
        public async Task<HttpResponseMessage> GetRestaurantOrders(long restaurantId, int pageNumber = 1)
        {
            var claimsPrincipal = User as ClaimsPrincipal;

            long userId = Convert.ToInt64(claimsPrincipal.FindFirst("userId").Value);

            var result = await _orderService.GetAllOrdersAsync(userId , restaurantId, pageNumber);

            return Request.CreateResponse(HttpStatusCode.OK , result);
        }

        [HttpGet, Route("{restaurantId}/orders/order/{orderId}")]
        public async Task<HttpResponseMessage> GetOrderDetails(long restaurantId, long orderId)
        {
            var claimsPrincipal = User as ClaimsPrincipal;

            long userId = Convert.ToInt64(claimsPrincipal.FindFirst("userId").Value);

            var result = await _orderService.GetOrderDetailsAsync(userId, restaurantId, orderId);

            return Request.CreateResponse(HttpStatusCode.OK , result);
        }
    }
}
