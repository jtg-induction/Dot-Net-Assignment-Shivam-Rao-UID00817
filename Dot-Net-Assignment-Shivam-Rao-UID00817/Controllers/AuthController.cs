using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Http;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using System.Web.UI;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Net;
using System.Web.Http.Description;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Services;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Controllers
{       
    [RoutePrefix("api/auth")]
    public class AuthController: ApiController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost, Route("register")]
        public async Task<IHttpActionResult> Register([FromBody] RegisterDto model)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            bool registered = await _authService.RegisterAsync(model);

            if (!registered)
            {
                return BadRequest("Email / Phone Number already exists");
            }

            return Ok(new { message = "Registration Successful!" });
        }
    }
}
