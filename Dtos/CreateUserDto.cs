using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.Dtos
{
    public class CreateUserDto
    {
        [Required]
        [MinLength(3)]
        public string Name { get; set; }
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
        public string Role { get; set; }  //  "Admin" or "User"
    }
}
