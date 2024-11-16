using System;
using System.ComponentModel.DataAnnotations;

namespace TodoAPI.Models.DTO
{
    public class TodoDTO
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
        public string? Title { get; set; } // Ensure Title is provided and doesn't exceed 200 chars

        public string? Description { get; set; } // Optional description, nullable

        public bool IsCompleted { get; set; } = false; // Default to false if not provided, for better clarity

        public DateTime? DueDate { get; set; } // Optional due date

        [Required(ErrorMessage = "UserId is required.")]
        public Guid UserId { get; set; } // Foreign key for User
    }
}
