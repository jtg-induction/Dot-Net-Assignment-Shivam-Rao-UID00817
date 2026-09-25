using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Net;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories;
using Swashbuckle.Swagger;
using System.Net.Http;
using ValidationException = Dot_Net_Assignment_Shivam_Rao_UID00817.Exceptions.ValidationException;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/admin/restaurants")]
    public class AdminRestaurantController: ApiController
    {
        private readonly IAdminRestaurantService _adminService;

        public AdminRestaurantController(IAdminRestaurantService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost, Route("")]
        public async Task<HttpResponseMessage> Restaurant([FromBody] RestaurantOnboardDto model)
        {
            if (model is null) throw new ValidationException(ErrorMessages.INVALID_OPERATION);
            var result = await _adminService.OnboardRestaurantAsync(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost, Route("onboarding")]
        public async Task<HttpResponseMessage> Onboard([FromBody] OwnerOnboardRequestDto model)
        {
            if (model is null) throw new ValidationException(ErrorMessages.INVALID_OPERATION);
            var result = await _adminService.AssignOwnerToRestaurantAsync(model);
            return Request.CreateResponse(HttpStatusCode.OK , result);
        }

        [HttpPatch, Route("deactivate")]
        public async Task<IHttpActionResult> Deactivate([FromBody] RestaurantActivateDeactivateDto model)
        {
            if (model is null) throw new ValidationException(ErrorMessages.INVALID_OPERATION);
            await _adminService.DeactivateRestaurant(model.Name);
            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPatch, Route("activate")]
        public async Task<IHttpActionResult> Activate([FromBody] RestaurantActivateDeactivateDto model)
        {
            if (model is null) throw new ValidationException(ErrorMessages.INVALID_OPERATION);
            await _adminService.ActivateRestaurant(model.Name);
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
