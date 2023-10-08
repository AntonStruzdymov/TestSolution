using TestDB.Entities;

namespace TestTask.DTOs
{
    public class TokenDTO
    {
        public int Id { get; set; }
        public Guid Value { get; set; }
        public int UserId { get; set; }
        public UserDTO User { get; set; }
        public string Token { get; set; }
    }
}
