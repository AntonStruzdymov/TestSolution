using TestDB.Entities;
using TestTask.DTOs;

namespace TestTask.Responses
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public List<RoleDTO> Roles { get; set; }
    }
}
