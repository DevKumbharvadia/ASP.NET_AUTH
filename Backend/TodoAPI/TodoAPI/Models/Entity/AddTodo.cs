using System;
using System.ComponentModel.DataAnnotations;

namespace TodoAPI.Models.Entity
{
    public class AddTodo
    {
        [Required] // Ensure Title is provided
        public string Title { get; set; }

        public string Description { get; set; } // Optional description
        public bool IsCompleted { get; set; } // Indicates if the item is completed
        public DateTime? DueDate { get; set; } // Optional due date

        [Required] // Ensure UserId is provided
        public Guid UserId { get; set; } // Foreign key for User
    }
}
