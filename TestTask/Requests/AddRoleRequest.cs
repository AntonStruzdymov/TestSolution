using System.ComponentModel.DataAnnotations;

namespace TestTask.Requests
{
    public class AddRoleRequest
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public string RoleName { get; set; }
    }
}
