using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoAPI.Data;
using TodoAPI.Models;
using TodoAPI.Models.Domain;

namespace TodoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/User/assignRoles
        [HttpPost("assignRoles")]
        public async Task<ActionResult> UpdateUserRoles(Guid userId, List<Guid> roleIds)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound($"User with ID {userId} does not exist.");
            }

            // Retrieve the user's existing roles
            var existingUserRoles = _context.UserRoles.Where(ur => ur.UserId == userId).ToList();

            // Remove roles that are not in the provided roleIds list
            foreach (var userRole in existingUserRoles)
            {
                if (!roleIds.Contains(userRole.RoleId))
                {
                    _context.UserRoles.Remove(userRole);
                }
            }

            // Add new roles that the user doesn't already have
            foreach (var roleId in roleIds)
            {
                var role = await _context.Roles.FindAsync(roleId);
                if (role == null)
                {
                    return BadRequest($"Role with ID {roleId} does not exist.");
                }

                var userRoleExist = existingUserRoles.SingleOrDefault(ur => ur.RoleId == roleId);
                if (userRoleExist == null)
                {
                    _context.UserRoles.Add(new UserRole { RoleId = roleId, UserId = userId });
                }
            }

            // Save changes to the database
            await _context.SaveChangesAsync();

            return Ok(new { Message = "User roles updated successfully." });
        }

        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

    }
}
