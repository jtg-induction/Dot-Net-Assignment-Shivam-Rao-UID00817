using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Http;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Services;
using System.Web.UI;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Net;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Controllers
{
    [RoutePrefix("api/auth")]
    public class AuthController: ApiController
    {
        private readonly Restaurant_ManagementContext _db = new Restaurant_ManagementContext();

        [HttpPost, Route("register")]
        public async Task<IHttpActionResult> Register([FromBody] RegisterDto model)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            
            if(await _db.Users.AnyAsync(u => u.Email.ToLower() == model.Email.ToLower()
                                    || u.PhoneNumber == model.PhoneNumber))
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest,"Email / Phone Number already exists."));

            var newUser = new Users
            {
                Email = model.Email.Trim() ,
                Password = PasswordHasher.HashPassword(model.Password.Trim()) ,
                PhoneNumber = model.PhoneNumber.Trim(),
                Name = model.Name.Trim(),
                Role = "Customer",
                CreatedAt = DateTime.UtcNow,
                WalletBalance = 1000m,
                IsActive = true,
                UpdatedAt = DateTime.UtcNow
            };

            _db.Users.Add(newUser);
            await _db.SaveChangesAsync();
            return Ok(new { message = "Registration successful!" });
        }
    }
}
