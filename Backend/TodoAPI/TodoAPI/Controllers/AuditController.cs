using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using TodoAPI.Models.Domain;
using TodoAPI.Models.DTO;
using TodoAPI.Data;

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
                var audits = _context.UserAudits
                    .Include(a => a.User)
                    .Select(a => new AuditWithUserDTO
                    {
                        UserAuditId = a.UserAuditId,
                        UserId = a.UserId,
                        Username = a.User!.Username,
                        LoginTime = a.LoginTime,
                        LogoutTime = a.LogoutTime
                    })
                    .ToList();

                if (!audits.Any())
                {
                    return NotFound("No audits found.");
                }

                return Ok(audits);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }

        // GET: api/audits/{userId}
        [HttpGet("GetUserAuditsByUserID")]
        public async Task<IActionResult> GetUserAuditsByUserId(Guid userId)
        {
            try
            {
                var audits = await _context.UserAudits
                    .Where(a => a.UserId == userId)
                    .ToListAsync();

                if (!audits.Any())
                {
                    return NotFound($"No audits found for user with ID: {userId}");
                }

                return Ok(audits);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }

        // POST: api/audits
        [HttpPost("AddAudit")]
        public async Task<IActionResult> AddAudit([FromBody] UserAuditDTO newAuditDto)
        {
            if (newAuditDto == null)
            {
                return BadRequest("Audit data is null.");
            }

            try
            {
                var newAudit = new UserAudit
                {
                    UserId = newAuditDto.UserId,
                    LoginTime = DateTime.Now,
                };

                await _context.UserAudits.AddAsync(newAudit);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetUserAuditsByUserId), new { userId = newAudit.UserId }, newAudit);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }


        // PUT: api/audits/{id}
        [HttpPut("UpdateAudit/{id}")]
        public async Task<IActionResult> UpdateAudit(Guid id, [FromBody] UserAudit updatedAudit)
        {
            if (updatedAudit == null || id != updatedAudit.UserAuditId)
            {
                return BadRequest("Invalid audit data.");
            }

            try
            {
                var existingAudit = await _context.UserAudits.FindAsync(id);
                if (existingAudit == null)
                {
                    return NotFound($"Audit with ID: {id} not found.");
                }

                // Update fields
                existingAudit.LoginTime = updatedAudit.LoginTime;
                existingAudit.LogoutTime = updatedAudit.LogoutTime;

                _context.UserAudits.Update(existingAudit);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }

        // DELETE: api/audits/{id}
        [HttpDelete("DeleteAudit/{id}")]
        public async Task<IActionResult> DeleteAudit(Guid id)
        {
            try
            {
                var audit = await _context.UserAudits.FindAsync(id);
                if (audit == null)
                {
                    return NotFound($"Audit with ID: {id} not found.");
                }

                _context.UserAudits.Remove(audit);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
