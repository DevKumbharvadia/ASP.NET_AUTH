using System;
using System.Collections.Generic;

namespace TodoAPI.Models
{
    public class Role
    {
        public Guid RoleId { get; set; } // Primary key
        public string RoleName { get; set; } // Name of the role

        // Navigation property for users (many-to-many relationship)
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
