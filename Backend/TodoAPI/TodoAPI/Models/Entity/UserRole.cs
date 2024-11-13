using System;

namespace TodoAPI.Models
{
    public class UserRole
    {
        public Guid UserId { get; set; } // Foreign key for User
        public User User { get; set; } // Navigation property to User

        public Guid RoleId { get; set; } // Foreign key for Role
        public Role Role { get; set; } // Navigation property to Role
    }
}
