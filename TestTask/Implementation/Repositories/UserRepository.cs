using Microsoft.EntityFrameworkCore;
using TestDB.Entities;
using TestDB;
using TestTask.Abstractions.Repositiories;
using AutoMapper;
using TestTask.DTOs;

namespace TestTask.Implementation.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly Context _context;
        private readonly DbSet<User> _users;
        private readonly IMapper _mapper;
        public UserRepository(Context context, IMapper mapper)
        {
            _context = context;
            _users = _context.Set<User>();
            _mapper = mapper;
        }
        public async Task AddRoleToUser(RoleDTO role, int id)
        {
            var user = await _users.FirstOrDefaultAsync(u => u.ID == id);
            user.Roles.Add(_mapper.Map<Role>(role));
            await SaveChangesAsync();
        }

        public async Task AddUser(UserDTO user)
        {
            var userToAdd = _mapper.Map<User>(user);
            await _context.Users.AddAsync(userToAdd);
            await SaveChangesAsync();

        }

        public async Task DeleteUser(int id)
        {
            var user = await _users.FirstOrDefaultAsync(a => a.ID == id);
            _users.Remove(user);
            await SaveChangesAsync();
        }

        public async Task<List<UserDTO>> GetAllUsersWithPag(int pagesize, int pagenumber)
        {
            var users = await _context.Users.Include(u => u.Roles).Skip(pagesize * (pagenumber - 1)).Take(pagesize).ToListAsync();
            return users.Select(u=> _mapper.Map<UserDTO>(u)).ToList();
        }

        public async Task<UserDTO> GetByEmail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u=> u.Email.Equals(email));
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> GetUserById(int id)
        {
            var user = await _context.Users.AsNoTracking().Include(u => u.Roles).FirstOrDefaultAsync(u => u.ID == id);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<bool> HasRole(int id, RoleDTO role)
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
            var isExist = await _context.Users.Where(a=>a.Email.Equals(email)).AnyAsync();
            return isExist;
        }

        public async Task<bool> IsUserExists(int id)
        {
            return await _users.Select(u => u.ID == id).AnyAsync();
        }

        public async Task UpdateUser(int id, List<PatchDTO> patchDtos)
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.ID == id);
            var nameValuePairProperties = patchDtos.ToDictionary(k=>k.Name, v=>v.Value);
            var dbEntityEntry = _context.Entry(user);
            dbEntityEntry.CurrentValues.SetValues(nameValuePairProperties);
            dbEntityEntry.State = EntityState.Modified;
            await SaveChangesAsync();
        }
        private async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}

