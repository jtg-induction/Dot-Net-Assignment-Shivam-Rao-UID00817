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

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Controllers
{
    [Authorize]
    [RoutePrefix("api/account")]
    public class AccountController: ApiController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPatch, Route("deactivate")]
        public async Task Deactivate()
        {
            var claimsPrincipal = User as ClaimsPrincipal;

            long userId = Convert.ToInt64(claimsPrincipal.FindFirst("userId").Value);

            await _accountService.DeactivateAccountAsync(userId);
        }
    }
}
