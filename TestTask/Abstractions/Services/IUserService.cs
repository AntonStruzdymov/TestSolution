using TestDB.Entities;
using TestTask.DTOs;

namespace TestTask.Abstractions
{
    public interface IUserService
    {
        Task<List<UserDTO>> GetAllUsersWithPag(int pagesize, int pagenumber);
        Task<UserDTO> GetUserById(int id);
        Task AddRoleToUser(RoleDTO role, int id);
        Task AddUser (UserDTO user);
        Task DeleteUser (int id);
        Task<bool> HasRole (int id, RoleDTO role);
        Task UpdateUser (int id, List<PatchDTO> dtos);
        Task<bool> IsEmailExist (string email);
        Task<bool> IsUserExists (int id);
        Task<UserDTO> GetByEmail (string email);
    }
}
