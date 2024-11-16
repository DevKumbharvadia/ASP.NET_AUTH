using System;
using System.ComponentModel.DataAnnotations;

namespace TodoAPI.Models.DTO
{
    public class AuditWithUserDTO
    {
        [Required(ErrorMessage = "UserAuditId is required.")]
        public Guid UserAuditId { get; set; }

        [Required(ErrorMessage = "UserId is required.")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(100, ErrorMessage = "Username cannot exceed 100 characters.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "LoginTime is required.")]
        public DateTime LoginTime { get; set; }

        public DateTime? LogoutTime { get; set; } // Optional LogoutTime
    }
}
