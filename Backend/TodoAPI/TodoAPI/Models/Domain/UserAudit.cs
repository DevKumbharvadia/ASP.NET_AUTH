using System;

namespace TodoAPI.Models.Domain
{
    public class UserAudit
    {
        public Guid UserAuditId { get; set; } // Primary key

        public Guid UserId { get; set; } // Foreign key for the User

        public DateTime LoginTime { get; set; } = DateTime.UtcNow; // Default to current UTC time when a login occurs

        public DateTime? LogoutTime { get; set; } // Nullable logout time, indicating the user is logged in if null

        // Navigation property to associate with the User
        public User? User { get; set; } // Marked nullable for cases where related data may not be loaded
    }
}
