namespace TaskManagementAPI.Dtos
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int TaskItemId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }

        public UserDto User { get; set; }
    }
}
