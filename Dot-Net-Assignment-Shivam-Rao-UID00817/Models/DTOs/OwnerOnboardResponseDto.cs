using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs
{
    public class OwnerOnboardResponseDto
    {
        public List<EmailAndStatus> EmailStatus { get; set; } = new List<EmailAndStatus>();
    }

    public class EmailAndStatus
    {
        public string Email { get; set; }
        public bool IsOnBoarded { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        public EmailAndStatus(string email, bool isOnBoarded, string message)
        {
            this.Email = email;
            this.IsOnBoarded = isOnBoarded;
            this.Message = isOnBoarded ? null : message;
        }
    }

}
