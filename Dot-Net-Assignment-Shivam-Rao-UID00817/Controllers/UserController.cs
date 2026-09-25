using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Services.Interfaces;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Controllers
{
    [Authorize]
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost, Route("deactivate")]
        public async Task Deactivate()
        {
            var claimsPrincipal = User as ClaimsPrincipal;

            long userId = Convert.ToInt64(claimsPrincipal.FindFirst("userId").Value);

            await _userService.DeactivateAccountAsync(userId);
        }

        [HttpPatch, Route("")]
        public async Task Update([FromBody] UpdateAccountDto model)
        {
            var claimsPrincipal = User as ClaimsPrincipal;

            long userId = Convert.ToInt64(claimsPrincipal.FindFirst("userId").Value);

            await _userService.UpdateAccountAsync(userId, model);
        }

    }
}
