using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoAPI.Data;
using TodoAPI.Models;
using TodoAPI.Models.Entity;

namespace TodoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RoleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Role
        [HttpGet("GetAllRoles")]
        public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
        {
            var roles = await _context.Roles.ToListAsync();
            return Ok(roles);
        }

        [HttpGet("GetUserRoles")]
        public ActionResult<ApiResponse<List<string>>> GetUserRoles(Guid userId)
        {
            try
            {
                // Select role names for the user, joining UserRole and Role entities
                var roles = _context.UserRoles
                    .Where(ur => ur.UserId == userId)
                    .Select(ur => ur.Role.RoleName) // Select RoleName instead of RoleId
                    .ToList();

                if (!roles.Any())
                {
                    return NotFound(new ApiResponse<List<string>>
                    {
                        Success = false,
                        Message = "User has no roles assigned",
                    });
                }

                return Ok(new ApiResponse<List<string>>
                {
                    Success = true,
                    Message = "Roles retrieved successfully",
                    Data = roles
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<string>>
                {
                    Success = false,
                    Message = ex.Message,
                });
            }
        }


        // POST: api/Role
        [HttpPost("AddRole")]
        public async Task<ActionResult<ApiResponse<Role>>> AddRole(AddRole role)
        {
            var newRole = new Role
            {
                RoleId = Guid.NewGuid(),
                RoleName = role.RoleName,
            };

            _context.Roles.Add(newRole);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<Role>
            {
                Message = "Role added successfully",
                Success = true,
                Data = newRole
            });
        }

        // DELETE: api/Role/{id}
        [HttpDelete("RemoveRole")]
        public async Task<IActionResult> RemoveRole(Guid id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("UpdateRole")]
        public async Task<IActionResult> UpdateRole(Guid Id, UpdateRoleDto role)
        {
            if (Id == Guid.Empty)
            {
                return BadRequest("Invalid role ID.");
            }

            var existingRole = await _context.Roles.FindAsync(Id);
            if (existingRole == null)
            {
                return NotFound("Role not found.");
            }

            existingRole.RoleName = role.RoleName;

            await _context.SaveChangesAsync();

            return Ok("Role updated successfully.");
        }

    }
}
