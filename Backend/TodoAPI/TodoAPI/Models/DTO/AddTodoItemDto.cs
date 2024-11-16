using System;

namespace TodoAPI.Models.DTOs
{
    public class AddTodoItemDto
    {
        public string Title { get; set; } = string.Empty; // Title of the TodoItem (required)

        public string Description { get; set; } = string.Empty; // Description of the TodoItem (required)

        public bool IsCompleted { get; set; } // Whether the TodoItem is completed (optional)

        public DateTime? DueDate { get; set; } // Optional due date for the TodoItem (nullable)

        public Guid? UserId { get; set; } // Nullable UserId for assigning a user to the TodoItem
    }
}
