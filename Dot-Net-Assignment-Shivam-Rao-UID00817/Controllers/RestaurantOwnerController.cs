using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Exceptions;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Services.Interfaces;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using Telerik.Reporting;
using Telerik.Reporting.Processing;
using ValidationException = Dot_Net_Assignment_Shivam_Rao_UID00817.Exceptions.ValidationException;



namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Controllers
{
    [Authorize(Roles = "Owner")]
    [RoutePrefix("api/restaurants")]
    public class RestaurantOwnerController : ApiController
    {
        private readonly IOrderService _orderService;
        private readonly IOwnerManagesRestaurantsRepository _ownerManagesRestaurantsRepository;
        public RestaurantOwnerController(IOrderService orderService, IOwnerManagesRestaurantsRepository ownerManagesRestaurantsRepository)
        {
            _orderService = orderService;
            _ownerManagesRestaurantsRepository = ownerManagesRestaurantsRepository;
        }

        public RestaurantOwnerController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet, Route("{restaurantId}/orders")]
        public async Task<HttpResponseMessage> GetRestaurantOrders(long restaurantId, int pageNumber = 1, string search = "", Enums.SortBy sortBy = Enums.SortBy.OrderDateLatest, Enums.FilterBy filterBy = Enums.FilterBy.Default, string filterByCity = "")
        {
            var claimsPrincipal = User as ClaimsPrincipal;

            long userId = Convert.ToInt64(claimsPrincipal.FindFirst("userId").Value);

            var result = await _orderService.GetAllOrdersAsync(userId, restaurantId, pageNumber, search, sortBy, filterBy, filterByCity);

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpGet, Route("{restaurantId}/orders/order/{orderId}")]
        public async Task<HttpResponseMessage> GetOrderDetails(long restaurantId, long orderId)
        {
            var claimsPrincipal = User as ClaimsPrincipal;

            long userId = Convert.ToInt64(claimsPrincipal.FindFirst("userId").Value);

            var result = await _orderService.GetOrderDetailsAsync(userId, restaurantId, orderId);

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPatch, Route("{restaurantId}/orders/order/{orderId}")]
        public async Task<HttpResponseMessage> ManageOrder(long restaurantId, long orderId, [FromBody] OrderManagementRequestDto model)
        {
            if (model is null) throw new ValidationException(ErrorMessages.INVALID_OPERATION);
            var claimsPrincipal = User as ClaimsPrincipal;
            long ownerId = Convert.ToInt64(claimsPrincipal.FindFirst("userId").Value);
            await _orderService.ManageOrderAsync(restaurantId, ownerId, orderId, model);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        [HttpGet, Route("{restaurantId}/reports/frequently-bought-together")]
        public async Task<HttpResponseMessage> GetBoughtTogetherItems(long restaurantId, int NumberOfPairs)
        {
            var claimsPrincipal = User as ClaimsPrincipal;

            long userId = Convert.ToInt64(claimsPrincipal.FindFirst("userId").Value);

            if (await _ownerManagesRestaurantsRepository.GetOwnerIfExistsAsync(userId, restaurantId, false) == null)
            {
                throw new UnauthorizedException();
            }
            var reportPath = HostingEnvironment.MapPath("~/Reports/FrequentlyBoughtTogetherItems.trdp");

            if (string.IsNullOrEmpty(reportPath) || !System.IO.File.Exists(reportPath))
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Report definition not found.");
            }

            var reportSource = new UriReportSource
            {
                Uri = reportPath
            };

            reportSource.Parameters.Add("RestaurantId", restaurantId);
            reportSource.Parameters.Add("NumberOfPairs", NumberOfPairs);

            var reportProcessor = new ReportProcessor();

            var result = reportProcessor.RenderReport("PDF", reportSource, null);

            if (result.HasErrors)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Failed to generate report.");
            }

            var response = Request.CreateResponse(HttpStatusCode.OK);

            response.Content = new ByteArrayContent(result.DocumentBytes);

            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = $"Restaurant-{restaurantId}-Frequently-Bought-Together-Items.pdf"
            };

            return response;
        }

        [HttpGet, Route("{restaurantId}/reports/top-10-items")]
        public async Task<HttpResponseMessage> GetTop10Items(long restaurantId, string excludeItemIds = "")
        {
            var claimsPrincipal = User as ClaimsPrincipal;

            long userId = Convert.ToInt64(claimsPrincipal.FindFirst("userId").Value);

            if (await _ownerManagesRestaurantsRepository.GetOwnerIfExistsAsync(userId, restaurantId, false) == null)
            {
                throw new UnauthorizedException();
            }
            var reportPath = HostingEnvironment.MapPath("~/Reports/Top10Items.trdp");

            if (string.IsNullOrEmpty(reportPath) || !System.IO.File.Exists(reportPath))
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Report definition not found.");
            }

            var excludedIds = excludeItemIds ?? string.Empty;

            var reportSource = new UriReportSource
            {
                Uri = reportPath
            };

            reportSource.Parameters.Add("RestaurantId", restaurantId);

            reportSource.Parameters.Add("ExcludedItemIds", excludedIds);

            var reportProcessor = new ReportProcessor();

            var result = reportProcessor.RenderReport("PDF", reportSource, null);

            if (result.HasErrors)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Failed to generate report.");
            }

            var response = Request.CreateResponse(HttpStatusCode.OK);

            response.Content = new ByteArrayContent(result.DocumentBytes);

            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

            response.Content.Headers.ContentDisposition =
                new ContentDispositionHeaderValue("attachment")
                {
                    FileName =
                        $"Restaurant-{restaurantId}-Top-10-Items.pdf"
                };

            return response;
        }


    }
}
