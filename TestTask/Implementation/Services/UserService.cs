using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Globalization;
using TestDB;
using TestDB.Entities;
using TestTask.Abstractions;
using TestTask.Abstractions.Repositiories;
using TestTask.DTOs;

namespace TestTask.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }
        public async Task AddRoleToUser(RoleDTO role, int id)
        {
            await _repository.AddRoleToUser(role, id);
        }

        public async Task AddUser(UserDTO user)
        {
            await _repository.AddUser(user);
        }

        public async Task DeleteUser(int id)
        {
            await _repository.DeleteUser(id);
        }

        public async Task<List<UserDTO>> GetAllUsersWithPag(int pagesize, int pagenumber)
        {
            return await _repository.GetAllUsersWithPag(pagesize, pagenumber); 
        }

        public async Task<UserDTO> GetByEmail(string email)
        {
            return await _repository.GetByEmail(email);
        }

        public async Task<UserDTO> GetUserById(int id)
        {
            return await _repository.GetUserById(id);
        }

        public async Task<bool> HasRole(int id, RoleDTO role)
        {
            return await _repository.HasRole(id, role);
        }

        public async Task<bool> IsEmailExist(string email)
        {
            return await _repository.IsEmailExist(email);
        }

        public async Task<bool> IsUserExists(int id)
        {
            return await _repository.IsUserExists(id);
        }

        public async Task UpdateUser(int id, List<PatchDTO> dtos)
        {
            await _repository.UpdateUser(id, dtos);
        }
    }


}
