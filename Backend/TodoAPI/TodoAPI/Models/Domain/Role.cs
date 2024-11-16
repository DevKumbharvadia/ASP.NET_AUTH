using System;
using System.Collections.Generic;

namespace TodoAPI.Models.Domain
{
    public class Role
    {
        public Guid RoleId { get; set; } // Primary key

        public string RoleName { get; set; } = string.Empty; // Ensure non-nullable with default value

        // Navigation property for users (many-to-many relationship)
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>(); // Initialize to prevent null reference issues
    }
}
