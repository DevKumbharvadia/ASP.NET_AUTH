namespace TodoAPI.Models
{
    public class TodoItemDTO
    {
        public required string Title { get; set; } // The title of the todo item
        public string? Description { get; set; } // Detailed description of the todo item (optional)
        public required bool IsCompleted { get; set; } // Indicates whether the task is completed
        public DateTime? DueDate { get; set; } // The due date for the task (optional)
        public required DateTime CreatedAt { get; set; } // Timestamp of when the task was created
        public DateTime UpdatedAt { get; set; } // Timestamp of when the task was last updated
        public int? UserId { get; set; } // (Optional) Foreign Key to link to a user
    }
}
