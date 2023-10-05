using TestDB.Entities;

namespace TestTask.Abstractions
{
    public interface IRoleService
    {
        Task<Role> GetDefaultRole();
        Task<bool> IsRoleExists(string roleName);
        Task<Role> GetRoleByName(string roleName);
    }
}
