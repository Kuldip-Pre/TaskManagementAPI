using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.Dtos
{
    public class CreateTaskDto
    {
        [Required]
        [MinLength(3)]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}
