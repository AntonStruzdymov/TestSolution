using TestDB.Entities;

namespace TestTask.DTOs
{
    public class RoleDTO
    {
        public RoleDTO()
        {
            this.Users = new List<UserDTO>();
        }
        public int ID { get; set; }
        public string Name { get; set; }
        public virtual List<UserDTO> Users { get; set; }
    }
}
