using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models
{
    [Table("Refresh_Tokens")]
    public class Refresh_Tokens
    {

        [Key]
        [Column("token_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TokenId { get; set; }

        [Required]
        [Column("refresh_token")]
        public string RefreshToken { get; set; }

        [Required]
        [Column("user_id")]
        public long UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual Users Users { get; set; }

        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Required]
        [Column("expires_at")]
        public DateTime ExpiresAt { get; set; }
    public Refresh_Tokens(long userId, string refreshToken)
        {
            DateTime CurrentTime = DateTime.UtcNow;
            this.UserId = userId;
            this.RefreshToken = refreshToken;
            this.CreatedAt = CurrentTime;
            this.ExpiresAt = CurrentTime.AddDays(10);
        }

        public Refresh_Tokens()
        {

        }
    }

}
