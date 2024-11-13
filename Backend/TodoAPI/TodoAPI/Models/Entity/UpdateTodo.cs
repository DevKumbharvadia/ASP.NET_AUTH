using System;

namespace TodoAPI.Models.Entity
{
    public class UpdateTodo
    {
        public string Title { get; set; } // Title of the todo item
        public string Description { get; set; } // Description of the todo item
        public bool IsCompleted { get; set; } // Indicates if the item is completed
        public DateTime? DueDate { get; set; } // Due date for the item
    }
}
