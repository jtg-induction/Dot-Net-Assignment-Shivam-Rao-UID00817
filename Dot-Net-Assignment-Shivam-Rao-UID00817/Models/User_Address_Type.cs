using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models
{
    [Table("User_Address_Type")]
    public class User_Address_Type
    {
        [Key]
        [Column("user_id", Order = 1)]
        public long UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual Users Users { get; set; }

        [Key]
        [Column("address_id", Order = 2)]
        public long AddressId { get; set; }

        [ForeignKey(nameof(AddressId))]
        public virtual Addresses Addresses { get; set; }

        [Required]
        [Column("address_type")]
        public string AddressType { get; set; } = "Home";
    }
}
