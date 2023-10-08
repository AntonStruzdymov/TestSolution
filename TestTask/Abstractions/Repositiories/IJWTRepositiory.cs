using TestDB.Entities;
using TestTask.DTOs;

namespace TestTask.Abstractions.Repositiories
{
    public interface IJWTRepositiory
    {
        Task AddRefreshToken(int id, Guid token);
        Task<UserDTO> GetUserByRefreshToken(Guid token);
        Task RemoveRefreshToken(Guid token);
    }
}
