namespace UserService.Models.Request
{
    public class TodoServiceRequest
    {
    }
    public class TodoDetails
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class NewTodo
    {
        public int UserId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
    }
    public class UpdateTodo
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public string? Status { get; set; }

    }
}
