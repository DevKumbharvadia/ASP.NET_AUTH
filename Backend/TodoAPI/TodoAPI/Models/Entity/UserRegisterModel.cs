namespace TodoAPI.Models
{
    public class UserRegisterModel
    {
        public string Username { get; set; } // Username for registration
        public string Password { get; set; } // Raw password
        public string Email { get; set; } // Email for registration
        public Guid RoleId { get; set; }
    }
}
