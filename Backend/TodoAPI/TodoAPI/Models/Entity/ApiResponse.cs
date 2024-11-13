namespace TodoAPI.Models
{
    public class ApiResponse<T>
    {
        public string Message { get; set; } // Success or error message
        public bool Success { get; set; }   // Indicates if the request was successful
        public T Data { get; set; }         // Generic data field that can hold any type (e.g., TodoItem, User, etc.)
    }
}
