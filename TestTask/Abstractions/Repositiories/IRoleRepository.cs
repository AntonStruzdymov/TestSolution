using TestDB.Entities;
using TestTask.DTOs;

namespace TestTask.Abstractions.Repositiories
{
    public interface IRoleRepository
    {
        Task<RoleDTO> GetDefaultRole();
        Task<RoleDTO> GetRoleByName(string roleName);
        Task<bool> IsRoleExists(string roleName);
    }
}
