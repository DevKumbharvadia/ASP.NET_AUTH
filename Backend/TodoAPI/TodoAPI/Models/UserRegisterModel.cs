using System;
using System.ComponentModel.DataAnnotations;

namespace TodoAPI.Models
{
    public class UserRegisterModel
    {
        [Required(ErrorMessage = "Username is required.")]
        public string? Username { get; set; } // Username for registration

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string? Password { get; set; } // Raw password

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please provide a valid email address.")]
        public string? Email { get; set; } // Email for registration

        [Required(ErrorMessage = "RoleId is required.")]
        public Guid RoleId { get; set; } // Role ID
    }
}
