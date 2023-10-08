using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestDB.Entities;
using TestTask.Abstractions.Repositiories;
using TestTask.Abstractions.Services;
using TestTask.DTOs;

namespace TestTask.Implementation.Services
{
    public class JWTService : IJWTService
    {
        private readonly IJWTRepositiory _jWTRepositiory;
        private readonly IConfiguration _configuration;

        public JWTService(IJWTRepositiory jWTRepositiory, IConfiguration configuration)
        {
            _jWTRepositiory = jWTRepositiory;
            _configuration = configuration;
        }

        public async Task AddRefreshToken(int id, Guid token)
        {
            await _jWTRepositiory.AddRefreshToken(id,token);
        }

        public async Task<string> GetJwtToken(UserDTO user)
        {
            return await GenerateJwtTokenAsync(user);
        }

        public async Task<TokenDTO> RefreshToken(Guid refreshToken)
        {
            var user = await _jWTRepositiory.GetUserByRefreshToken(refreshToken);
            if (user != null)
            {
                var token = await GetJwtToken(user);
                await _jWTRepositiory.RemoveRefreshToken(refreshToken);
                var newToken = Guid.NewGuid();
                await _jWTRepositiory.AddRefreshToken(user.ID, newToken);
                return new TokenDTO()
                {
                    User = user,
                    Token = token,
                    Value = newToken,
                    UserId = user.ID
                };
            }
            throw new ArgumentException("Token not connected with user", nameof(refreshToken));
        }

        private async Task<string> GenerateJwtTokenAsync(UserDTO user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),                
                new Claim(JwtRegisteredClaimNames.Sub, user.ID.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("D")),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString("R"))
            };
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration["Jwt:SecurityKey"]));

            var signIn = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["Jwt:ExpireInMinutes"])),
                signingCredentials: signIn);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
