namespace TodoAPI.Models
{
    public class JwtSettings
    {
        public string Key { get; set; }              // JWT Secret key
        public string Issuer { get; set; }           // JWT Issuer
        public string Audience { get; set; }         // JWT Audience
        public int ExpiresInMinutes { get; set; }    // Expiration time in minutes
    }
}
