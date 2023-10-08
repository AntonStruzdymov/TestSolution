using System.ComponentModel.DataAnnotations;

namespace TestTask.Requests
{
    public class UpdateUserRequest
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public int Age { get; set; }
    }
}
