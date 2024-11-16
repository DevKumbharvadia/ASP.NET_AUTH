using System;

namespace TodoAPI.Models.Domain
{
    public class UserRole
    {
        public Guid UserId { get; set; } // Foreign key for the User

        public User? User { get; set; } // Nullable navigation property to handle cases where related data may not be loaded

        public Guid RoleId { get; set; } // Foreign key for the Role

        public Role? Role { get; set; } // Nullable navigation property for flexibility in loading related data
    }
}
