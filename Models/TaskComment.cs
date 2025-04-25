using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.Models
{
    public class TaskComment
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public int TaskItemId { get; set; }
        public TaskItem TaskItem { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
