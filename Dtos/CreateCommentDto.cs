namespace TaskManagementAPI.Dtos
{
    public class CreateCommentDto
    {
        public string Content { get; set; }
        public int TaskItemId { get; set; }
        public int UserId { get; set; }
    }
}
