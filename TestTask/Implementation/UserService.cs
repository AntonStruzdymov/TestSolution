using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Globalization;
using TestDB;
using TestDB.Entities;
using TestTask.Abstractions;

namespace TestTask.Implementation
{
    public class UserService : IUserService
    {
        private readonly Context _context;
        private readonly DbSet<User> _users;
        public UserService(Context context)
        {
            _context = context;
            _users = _context.Set<User>();
        }
        public async Task AddRoleToUser(Role role, int id)
        {
            var user = await _users.FirstOrDefaultAsync(u => u.ID == id);
            user.Roles.Add(role);
            await _context.SaveChangesAsync();
        }

        public async Task AddUser(User user)
        {
            await _users.AddAsync(user);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteUser(int id)
        {
            var user = await _users.FirstOrDefaultAsync(a=> a.ID == id);
            _users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<List<User>> GetAllUsersWithPag(int pagesize, int pagenumber)
        {
            var users = await _users.AsQueryable().Skip(pagesize*(pagenumber-1)).Take(pagesize).ToListAsync();
            return users;
        }

        public Task<User> GetUserById(int id)
        {
            var user = _users.FirstOrDefaultAsync(u=>u.ID==id);
            return user;
        }

        public async Task<bool> HasRole(int id, Role role)
        {
            var user = await GetUserById(id);
            if (user.Roles.Contains(role))
            {
                return true;
            }
            return false;
        }

        public async Task<bool> IsEmailExist(string email)
        {
            return await _users.Select(u=>u.Email==email).AnyAsync();
        }

        public async Task<bool> IsUserExists(int id)
        {
            return await _users.Select(u=>u.ID == id).AnyAsync();
        }

        public async Task UpdateUser(User user)
        {
            _users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
