namespace TodoAPI.Models.Entity
{
    public class AuditWithUserDto
    {
        public Guid UserAuditId { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; } // Add properties you want from User
        public DateTime LoginTime { get; set; }
        public DateTime? LogoutTime { get; set; }
    }

}
