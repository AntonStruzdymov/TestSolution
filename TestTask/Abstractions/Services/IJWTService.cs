using TestDB.Entities;
using TestTask.DTOs;

namespace TestTask.Abstractions.Services
{
    public interface IJWTService
    {
        Task<string> GetJwtToken(UserDTO user);
        Task AddRefreshToken(int id, Guid token);
        Task<TokenDTO> RefreshToken (Guid refreshToken);
    }
}
