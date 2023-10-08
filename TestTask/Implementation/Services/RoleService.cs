using Microsoft.EntityFrameworkCore;
using TestDB;
using TestDB.Entities;
using TestTask.Abstractions;
using TestTask.Abstractions.Repositiories;
using TestTask.DTOs;

namespace TestTask.Implementation
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _repository;
        public RoleService(IRoleRepository repository)
        {
            _repository = repository;
        }
        public async Task<RoleDTO> GetDefaultRole()
        {
            return await _repository.GetDefaultRole();
        }

        public async Task<RoleDTO> GetRoleByName(string roleName)
        {
            return await _repository.GetRoleByName(roleName);
        }

        public async Task<bool> IsRoleExists(string roleName)
        {
            return await _repository.IsRoleExists(roleName);
        }
    }
}
