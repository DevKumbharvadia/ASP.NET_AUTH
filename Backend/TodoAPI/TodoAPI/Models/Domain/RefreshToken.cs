using System;

namespace TodoAPI.Models.Domain
{
    public class RefreshToken
    {
        public Guid Id { get; set; } // Primary key

        public string Token { get; set; } = string.Empty; // Ensure non-nullable with default value

        public DateTime Expires { get; set; } // Token expiration date

        public DateTime Created { get; set; } // Token creation date

        public DateTime? Revoked { get; set; } // Nullable, indicates when the token was revoked

        // Read-only property to check if the token is expired
        public bool IsExpired => DateTime.UtcNow >= Expires;

        // Read-only property to check if the token is active
        public bool IsActive => Revoked == null && !IsExpired;

        // Foreign key to User
        public Guid UserId { get; set; } // Ensures the relationship with User

        public User? User { get; set; } // Navigation property (nullable to avoid issues during lazy loading)
    }
}
