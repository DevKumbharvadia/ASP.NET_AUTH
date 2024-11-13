using System;

namespace TodoAPI.Models.Entity
{
    public class UserAudit
    {
        public Guid UserAuditId { get; set; } // Primary key
        public Guid UserId { get; set; } // Foreign key for the User
        public DateTime LoginTime { get; set; } // Time when the user logs in
        public DateTime? LogoutTime { get; set; } // Nullable logout time

        // Navigation property to associate with the User
        public User User { get; set; }
    }
}
