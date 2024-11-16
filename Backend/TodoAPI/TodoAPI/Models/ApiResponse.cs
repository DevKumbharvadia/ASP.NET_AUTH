namespace TodoAPI.Models
{
    public class ApiResponse<T>
    {
        // The success or error message
        public string Message { get; set; } = string.Empty; // Initialize to avoid null reference issues

        // Indicates if the request was successful
        public bool Success { get; set; }

        // Generic data field that can hold any type (e.g., TodoItem, User, etc.)
        public T? Data { get; set; }  // Nullable reference type for T

    }
}
