using TestDB.Entities;

namespace TestTask.Abstractions
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsersWithPag(int pagesize, int pagenumber);
        Task<User> GetUserById(int id);
        Task AddRoleToUser(Role role, int id);
        Task AddUser (User user);
        Task DeleteUser (int id);
        Task<bool> HasRole (int id, Role role);
        Task UpdateUser (User user);
        Task<bool> IsEmailExist (string email);
        Task<bool> IsUserExists (int id);
    }
}
