namespace TaskManagementAPI.Dtos
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public int UserId { get; set; }
        public UserDto User { get; set; }

        public List<CommentDto> Comments { get; set; }
    }
}
