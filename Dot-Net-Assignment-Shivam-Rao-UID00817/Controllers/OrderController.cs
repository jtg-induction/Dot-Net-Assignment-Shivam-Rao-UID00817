using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Services.Interfaces;
using Microsoft.Owin.Security.Provider;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Threading.Tasks;
using System.Net;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Controllers
{
    [RoutePrefix("api/orders")]
    public class OrderController: ApiController
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost, Route("")]
        public async Task<HttpResponseMessage> PlaceOrder([FromBody] OrderRequestDto model)
        {
            var claimsPrincipal = User as ClaimsPrincipal;

            long userId = Convert.ToInt64(claimsPrincipal.FindFirst("userId").Value);

            var response = await _orderService.PlaceOrderAsync(userId , model);

            return Request.CreateResponse(HttpStatusCode.Created, response);
        }
    }
}
