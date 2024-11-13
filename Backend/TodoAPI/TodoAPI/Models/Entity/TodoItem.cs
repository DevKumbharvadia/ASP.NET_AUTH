using System;

namespace TodoAPI.Models
{
    public class TodoItem
    {
        public Guid Id { get; set; } // Primary key
        public string Title { get; set; } // Title of the todo item
        public string Description { get; set; } // Description of the todo item
        public bool IsCompleted { get; set; } // Indicates if the item is completed
        public DateTime? DueDate { get; set; } // Due date for the item
        public DateTime CreatedAt { get; set; } // Created timestamp
        public DateTime UpdatedAt { get; set; } // Updated timestamp

        // Foreign key for User
        public Guid? UserId { get; set; } // Nullable Foreign key
        public User User { get; set; } // Navigation property to User
    }
}
