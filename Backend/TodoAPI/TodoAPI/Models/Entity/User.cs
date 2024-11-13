    using System;
using System.Collections.Generic;
using TodoAPI.Models.Entity;

namespace TodoAPI.Models
{
    public class User
    {
        public Guid UserId { get; set; } // Primary key
        public string Username { get; set; } // Username
        public string PasswordHash { get; set; } // Hashed password
        public string Email { get; set; } // Email address
        public DateTime CreatedAt { get; set; } // Created timestamp
        public DateTime UpdatedAt { get; set; } // Updated timestamp

        // Navigation properties
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public ICollection<TodoItem> TodoItems { get; set; } = new List<TodoItem>();
        public ICollection<UserAudit> UserAudits { get; set; } = new List<UserAudit>();
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
