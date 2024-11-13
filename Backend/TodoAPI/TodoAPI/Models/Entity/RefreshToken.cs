using System;

namespace TodoAPI.Models.Entity
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Revoked { get; set; }

        public bool IsExpired => DateTime.UtcNow >= Expires;
        public bool IsActive => Revoked == null && !IsExpired;

        // Foreign key to User
        public Guid UserId { get; set; }
        public User User { get; set; } // Navigation property to User
    }
}
