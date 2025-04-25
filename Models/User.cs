using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.Models
{
    public enum UserRole
    {
        Admin,
        User
    }
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public string Role { get; set; } 
        public ICollection<TaskItem> Tasks { get; set; }
        public ICollection<TaskComment> Comments { get; set; }
    }
}
