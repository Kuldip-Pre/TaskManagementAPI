using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string? Description { get; set; }
        [Required]
        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<TaskComment> Comments { get; set; }
    }
}
