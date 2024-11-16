namespace TodoAPI.Models.DTO
{
    public class UserAuditDTO
    {
        public Guid UserId { get; set; } // Required: Foreign key for the User

        public DateTime? LogoutTime { get; set; } // Nullable logout time, indicating the user is logged in if null
    }
}
