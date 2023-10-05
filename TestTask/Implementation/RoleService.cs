using Microsoft.EntityFrameworkCore;
using TestDB;
using TestDB.Entities;
using TestTask.Abstractions;

namespace TestTask.Implementation
{
    public class RoleService : IRoleService
    {
        private readonly Context _context;
        private readonly DbSet<Role> _roles;
        public RoleService(Context context)
        {
            _context = context;
            _roles = _context.Set<Role>();
        }
        public async Task<Role> GetDefaultRole()
        {
            return await _roles.FirstOrDefaultAsync(r=>r.Name.Equals("User"));
        }

        public async Task<Role> GetRoleByName(string roleName)
        {
            return await _roles.FirstOrDefaultAsync(r=>r.Name == roleName);
        }

        public async Task<bool> IsRoleExists(string roleName)
        {
            var isExist = await _roles.Select(r => r.Name == roleName).AnyAsync();
            return isExist;
        }
    }
}
