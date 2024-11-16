using System;

namespace TodoAPI.Models.Domain
{
    public class TodoItem
    {
        public Guid Id { get; set; } // Primary key

        public string Title { get; set; } = string.Empty; // Ensure non-nullable with default value

        public string Description { get; set; } = string.Empty; // Ensure non-nullable with default value

        public bool IsCompleted { get; set; } // Indicates if the item is completed

        public DateTime? DueDate { get; set; } // Optional due date for the item

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Default to current UTC time

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow; // Default to current UTC time

        // Foreign key for User
        public Guid? UserId { get; set; } // Nullable foreign key to allow unassigned items

        public User? User { get; set; } // Nullable navigation property to avoid issues during lazy loading
    }
}
