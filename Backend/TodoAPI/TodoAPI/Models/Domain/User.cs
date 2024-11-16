using System;
using System.Collections.Generic;

namespace TodoAPI.Models.Domain
{
    public class User
    {
        public Guid UserId { get; set; } // Primary key

        public string Username { get; set; } = string.Empty; // Ensure non-nullable with default value

        public string PasswordHash { get; set; } = string.Empty; // Ensure non-nullable with default value

        public string Email { get; set; } = string.Empty; // Ensure non-nullable with default value

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Default to current UTC time

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow; // Default to current UTC time

        // Navigation properties
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>(); // Many-to-many relationship with roles

        public ICollection<TodoItem> TodoItems { get; set; } = new List<TodoItem>(); // One-to-many relationship with todo items

        public ICollection<UserAudit> UserAudits { get; set; } = new List<UserAudit>(); // One-to-many relationship with user audits

        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>(); // One-to-many relationship with refresh tokens
    }
}
