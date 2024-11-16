using System.ComponentModel.DataAnnotations;

namespace TodoAPI.Models.DTO
{
    public class RoleDTO
    {
        [Required(ErrorMessage = "Role name is required.")]
        [StringLength(100, ErrorMessage = "Role name cannot exceed 100 characters.")]
        public string? RoleName { get; set; } // Name of the role
    }
}
