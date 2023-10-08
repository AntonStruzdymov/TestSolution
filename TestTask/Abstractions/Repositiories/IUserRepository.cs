using TestDB.Entities;
using TestTask.DTOs;

namespace TestTask.Abstractions.Repositiories
{
    public interface IUserRepository
    {
        Task AddRoleToUser(RoleDTO role, int id);
        Task AddUser(UserDTO user);
        Task DeleteUser(int id);
        Task<List<UserDTO>> GetAllUsersWithPag(int pagesize, int pagenumber);
        Task<UserDTO> GetUserById(int id);
        Task<bool> HasRole(int id, RoleDTO role);
        Task<bool> IsEmailExist(string email);
        Task<bool> IsUserExists(int id);
        Task UpdateUser(int id, List<PatchDTO> dtos);
        Task<UserDTO> GetByEmail(string email);


    }
}
