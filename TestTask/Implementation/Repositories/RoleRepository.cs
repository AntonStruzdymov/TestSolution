using Microsoft.EntityFrameworkCore;
using TestDB.Entities;
using TestDB;
using TestTask.Abstractions.Repositiories;
using AutoMapper;
using TestTask.DTOs;

namespace TestTask.Implementation.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly Context _context;
        private readonly DbSet<Role> _roles;
        private readonly IMapper _mapper;
        public RoleRepository(Context context, IMapper mapper)
        {
            _context = context;
            _roles = _context.Set<Role>();
            _mapper = mapper;
        }
        public async Task<RoleDTO> GetDefaultRole()
        {
            var defaultRole = await _roles.AsQueryable().AsNoTracking().FirstOrDefaultAsync(r => r.Name.Equals("User"));
            return _mapper.Map<RoleDTO>(defaultRole);
        }

        public async Task<RoleDTO> GetRoleByName(string roleName)
        {
            var role = await _roles.AsNoTracking().FirstOrDefaultAsync(r => r.Name == roleName);
            return _mapper.Map<RoleDTO>(role);
        }

        public async Task<bool> IsRoleExists(string roleName)
        {
            var isExist = await _roles.Select(r => r.Name == roleName).AnyAsync();
            return isExist;
        }
    }
}
