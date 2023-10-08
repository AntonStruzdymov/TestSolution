using TestDB.Entities;

namespace TestTask.DTOs
{
    public class UserDTO
    {
        public UserDTO()
        {
            this.Roles = new List<RoleDTO>();
        }
        public int ID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public virtual List<RoleDTO> Roles { get; set; }

    }
}
