using System;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models
{
    public interface IAuditableEntity
    {
        DateTime UpdatedAt { get; set; }
    }
}
