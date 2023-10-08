using Microsoft.EntityFrameworkCore;
using TestDB.Entities;
using TestDB;
using TestTask.Abstractions.Repositiories;
using TestTask.DTOs;
using AutoMapper;

namespace TestTask.Implementation.Repositories
{
    public class JWTRepository : IJWTRepositiory
    {
        private readonly Context _context;
        private readonly DbSet<RefreshToken> _tokens;
        private readonly IMapper _mapper;
        public JWTRepository(Context context, IMapper mapper)
        {
            _context = context;
            _tokens = _context.Set<RefreshToken>();
            _mapper = mapper;
        }

        public async Task AddRefreshToken(int id, Guid token)
        {
            await _tokens.AddAsync(new RefreshToken()
            {
                UserId = id,
                Value = token
            });
            await _context.SaveChangesAsync();
        }

        public async Task<UserDTO> GetUserByRefreshToken(Guid token)
        {
            var user = (await _tokens.Include(u => u.User).FirstOrDefaultAsync(t=>t.Value.Equals(token))).User;
            return _mapper.Map<UserDTO>(user);

        }

        public async Task RemoveRefreshToken(Guid token)
        {
            var tokenToRemove = await _tokens.FirstOrDefaultAsync(t => t.Value.Equals(token));
            _tokens.Remove(tokenToRemove);
            await _context.SaveChangesAsync();
        }
    }
}
