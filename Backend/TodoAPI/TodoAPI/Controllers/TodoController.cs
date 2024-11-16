using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoAPI.Models.Domain;
using TodoAPI.Data; // Assuming the DbContext is in the Data namespace
using System.Threading.Tasks;
using System.Linq;
using TodoAPI.Models.DTO;
using TodoAPI.Models.DTOs;
using Sieve.Services;
using Sieve.Models;

namespace TodoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly SieveProcessor _sieveProcessor;

        public TodoController(ApplicationDbContext context, SieveProcessor sieveProcessor)
        {
            _context = context;
            _sieveProcessor = sieveProcessor;
        }
        // Create a new TodoItem
        [HttpPost]
        public async Task<IActionResult> CreateTodoItem([FromBody] AddTodoItemDto addTodoItemDto)
        {
            if (addTodoItemDto == null)
            {
                return BadRequest("Invalid data.");
            }

            // Map AddTodoItemDto to TodoItem model
            var todoItem = new TodoItem
            {
                Id = Guid.NewGuid(), // Generate a new Id
                Title = addTodoItemDto.Title,
                Description = addTodoItemDto.Description,
                IsCompleted = addTodoItemDto.IsCompleted,
                DueDate = addTodoItemDto.DueDate,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                UserId = addTodoItemDto.UserId
            };

            // Add the new TodoItem to the database
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            // Return the created TodoItem with a 201 status code
            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
        }

        // Read all
        [HttpGet]
        public async Task<IActionResult> GetTodoItems()
        {
            var todoItems = await _context.TodoItems.ToListAsync();
            return Ok(todoItems);
        }

        // Read Sort
        [HttpGet("Sorted")]
        public async Task<IActionResult> GetTodoSorted([FromQuery]SieveModel model)
        {
            var todoItemsQuery = _context.TodoItems.AsQueryable();

            todoItemsQuery = _sieveProcessor.Apply(model, todoItemsQuery);

            var todoItems = await todoItemsQuery.ToListAsync();

            return Ok(todoItems);
        }


        [HttpGet("Paged")]
        public async Task<IActionResult> GetTodoPaged(int page = 1, int pageSize = 10)
        {
            // Get the total number of TodoItems
            var totalCount = await _context.TodoItems.CountAsync();

            // Calculate the total number of pages
            var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);

            // Get the TodoItems for the current page
            var todoItems = await _context.TodoItems
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Return the result as a paged response
            return Ok(todoItems);
        }

        // Read by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodoItem(Guid id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return Ok(todoItem);
        }

        // Update
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodoItem(Guid id, [FromBody] TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return BadRequest("Todo ID mismatch.");
            }

            var existingTodoItem = await _context.TodoItems.FindAsync(id);
            if (existingTodoItem == null)
            {
                return NotFound();
            }

            // Update properties
            existingTodoItem.Title = todoItem.Title;
            existingTodoItem.Description = todoItem.Description;
            existingTodoItem.IsCompleted = todoItem.IsCompleted;
            existingTodoItem.DueDate = todoItem.DueDate;
            existingTodoItem.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent(); // 204 No Content (successful update)
        }

        // Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(Guid id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent(); // 204 No Content (successful delete)
        }
    }
}
