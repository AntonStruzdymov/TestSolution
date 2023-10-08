using System.ComponentModel.DataAnnotations;

namespace TestTask.Requests
{
    public class LoginRequest
    {
        [Required]
        public string Login { get; set; }
    }
}
