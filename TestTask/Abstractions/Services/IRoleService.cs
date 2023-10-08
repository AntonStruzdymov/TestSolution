using TestDB.Entities;
using TestTask.DTOs;

namespace TestTask.Abstractions
{
    public interface IRoleService
    {
        Task<RoleDTO> GetDefaultRole();
        Task<bool> IsRoleExists(string roleName);
        Task<RoleDTO> GetRoleByName(string roleName);
    }
}
