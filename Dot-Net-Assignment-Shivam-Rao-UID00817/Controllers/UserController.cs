using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Services;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Security.Claims;
using ValidationException = Dot_Net_Assignment_Shivam_Rao_UID00817.Exceptions.ValidationException;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Controllers
{
    [Authorize]
    [RoutePrefix("api/user")]
    public class UserController: ApiController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost, Route("update/deactivate")]
        public async Task Deactivate()
        {
            var claimsPrincipal = User as ClaimsPrincipal;

            long userId = Convert.ToInt64(claimsPrincipal.FindFirst("userId").Value);

            await _userService.DeactivateAccountAsync(userId);
        }

        [HttpPatch]
        public async Task Update([FromBody] UpdateAccountDto model)
        {
            if (model is null) throw new ValidationException(Constants.ExceptionMessages.MODEL_WAS_NULL);
            var claimsPrincipal = User as ClaimsPrincipal;

            long userId = Convert.ToInt64(claimsPrincipal.FindFirst("userId").Value);

            await _userService.UpdateAccountAsync(userId, model);
        }

    }
}
