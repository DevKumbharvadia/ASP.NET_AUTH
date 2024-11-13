using Microsoft.AspNetCore.Mvc;
using TodoAPI.Data;
using TodoAPI.Models.Entity;
using System.Linq;
using System;
using TodoAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;

namespace TodoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuditController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public AuditController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // GET: api/audits
        [HttpGet("GetAllAudits")]
        public IActionResult GetUserAudits()
        {
            try
            {
                // Include the User data in the result
                var audits = _context.UserAudits
            .Include(a => a.User)
            .Select(a => new AuditWithUserDto
            {
                UserAuditId = a.UserAuditId,
                UserId = a.UserId,
                Username = a.User.Username, // Make sure to include this property
                LoginTime = a.LoginTime,
                LogoutTime = a.LogoutTime
            })
            .ToList();

                if (audits == null)
                {
                    return NotFound("No audits found.");
                }

                return Ok(audits);
            }
            catch (Exception ex)
            {
                // Log the exception (consider using a logging framework)
                Console.WriteLine($"Error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }

        // GET: api/audits/{userId}
        [HttpGet("GetUserAuditsByUserID")]
        public async Task<ActionResult<IEnumerable<UserAudit>>> GetUserAuditsByUserId(Guid userId)
        {
            try
            {
                var audits = await _context.UserAudits
                    .Where(a => a.UserId == userId)
                    .ToListAsync();

                if (audits == null || !audits.Any())
                {
                    return NotFound($"No audits found for user with ID: {userId}");
                }

                return Ok(audits);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        // POST: api/audit/logout/{userId}
        [HttpPost("Logout")]
        public IActionResult Logout(Guid userId)
        {
            try
            {
                // Find the last audit entry for the user (the most recent login without a logout time)
                var lastAudit = _context.UserAudits
                    .Where(ua => ua.UserId == userId && ua.LogoutTime == null)
                    .OrderByDescending(ua => ua.LoginTime)
                    .FirstOrDefault();

                if (lastAudit == null)
                {
                    return BadRequest(new ApiResponse<UserAudit>
                    {
                        Message = "No active login session found for this user",
                        Success = false,
                    });
                }

                // Set the logout time to the current time
                lastAudit.LogoutTime = DateTime.UtcNow;
                _context.UserAudits.Update(lastAudit); // Update the audit record in the database
                _context.SaveChanges();

                return Ok(new ApiResponse<UserAudit>
                {
                    Message = "Logout recorded successfully",
                    Success = true,
                    Data = lastAudit
                });
            }
            catch (Exception ex)
            {
                // Log the exception (consider using a logging framework)
                return StatusCode(500, new ApiResponse<UserAudit>
                {
                    Message = $"An error occurred while processing your request: {ex.Message}",
                    Success = false,
                });
            }
        }


        // POST: api/user/register
        [HttpPost("RegisterUser")]
        public ActionResult<ApiResponse<User>> Register([FromBody] UserRegisterModel model)
        {

            if (_context.Users.Any(u => u.Username == model.Username))
            {
                return BadRequest(new ApiResponse<User>
                {
                    Message = "Username already exists",
                    Success = false,
                });
            }

            var user = new User
            {
                UserId = Guid.NewGuid(),
                Username = model.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                Email = model.Email,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<User>
                {
                    Message = $"Error registering user:{ex.Message}",
                    Success = false,
                });
            }

            return CreatedAtAction(nameof(Register), new { id = user.UserId }, new ApiResponse<User>
            {
                Message = "User registered successfully",
                Success = true,
                Data = user
            });
        }

        // POST: api/user/login
        [HttpPost("LoginUser")]
        public ActionResult<ApiResponse<object>> Login([FromBody] UserLoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Message = "Invalid input data",
                    Success = false,
                });
            }

            var user = _context.Users.Include(u => u.RefreshTokens)
                .SingleOrDefault(u => u.Username == model.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                return Unauthorized(new ApiResponse<object>
                {
                    Message = "Invalid credentials",
                    Success = false,
                });
            }

            Logout(user.UserId);

            var jwtToken = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();

            user.RefreshTokens.Add(new RefreshToken
            {
                Token = refreshToken,
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                UserId = user.UserId
            });

            _context.UserAudits.Add(new UserAudit { LoginTime = DateTime.UtcNow, UserId = user.UserId });

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Message = $"Error logging in: {ex.Message}",
                    Success = false,
                });
            }

            return Ok(new ApiResponse<object>
            {
                Message = "Login successful",
                Success = true,
                Data = new
                {
                    JwtToken = jwtToken,
                    RefreshToken = refreshToken,
                    UserId = user.UserId
                }
            });
        }

        // POST: api/user/refresh-token
        [HttpPost("refresh-token")]
        public ActionResult<ApiResponse<string>> RefreshToken([FromBody] string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Refresh token is required"
                });
            }

            var user = _context.Users.Include(u => u.RefreshTokens)
                .SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == refreshToken));

            if (user == null)
            {
                return Unauthorized(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Invalid refresh token"
                });
            }

            var storedToken = user.RefreshTokens.Single(t => t.Token == refreshToken);

            if (!storedToken.IsActive)
            {
                return Unauthorized(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Token expired or revoked"
                });
            }

            // Generate new JWT and refresh token
            var newJwtToken = GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken();

            // Revoke the old refresh token
            storedToken.Revoked = DateTime.UtcNow;

            user.RefreshTokens.Add(new RefreshToken
            {
                Token = newRefreshToken,
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                UserId = user.UserId
            });

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>
                {
                    Success = false,
                    Message = $"Error refreshing token: {ex.Message}"
                });
            }

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Token refreshed successfully",
                Data = newJwtToken
            });
        }

        // Generate JWT token
        private string GenerateJwtToken(User user)
        {
            // Retrieve the JWT key from configuration
            var jwtKey = _config["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new InvalidOperationException("JWT key not configured.");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            // Retrieve and validate configuration values
            string issuer = _config["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT issuer not configured.");
            string audience = _config["Jwt:Audience"] ?? throw new InvalidOperationException("JWT audience not configured.");

            double expiresInMinutes = 60; // Default to 60 minutes if not configured or parsing fails
            if (!string.IsNullOrEmpty(_config["Jwt:ExpiresInMinutes"]) &&
                double.TryParse(_config["Jwt:ExpiresInMinutes"], out var parsedExpiresInMinutes))
            {
                expiresInMinutes = parsedExpiresInMinutes;
            }

            // Create the token
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiresInMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Generate refresh token
        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            return Convert.ToBase64String(randomBytes);
        }

    }
}
