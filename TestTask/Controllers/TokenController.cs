using Microsoft.AspNetCore.Mvc;
using TestTask.Abstractions;
using TestTask.Abstractions.Services;
using TestTask.Requests;
using TestTask.Responses;

namespace TestTask.Controllers
{
    public class TokenController : Controller
    {
        private readonly IUserService _userService;
        private readonly IJWTService _jwtService;
        private readonly ILogger<TokenController> _logger;

        public TokenController(IUserService userService, IJWTService jwtService, ILogger<TokenController> logger)
        {
            _userService = userService;
            _jwtService = jwtService;
            _logger = logger;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromQuery] LoginRequest request)
        {
            try
            {
                if (await _userService.IsEmailExist(request.Login))
                {
                    var user = await _userService.GetByEmail(request.Login);
                    var jwtTokenString = await _jwtService.GetJwtToken(user);
                    var refreshToken = Guid.NewGuid();
                    await _jwtService.AddRefreshToken(user.ID, refreshToken);
                    return Ok(new TokenResponse()
                    {
                        Token = jwtTokenString,
                        Email = request.Login,
                        RefreshToken = refreshToken.ToString()
                    });
                }
                else
                {
                    return BadRequest(new ErrorResponse() { Message = "Invalid User" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(new ErrorResponse() { Message= ex.Message });
            }
        }
        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> Refresh([FromQuery] RefreshTokenRequest refreshTokenRequest)
        {
            try
            {
                var token = await _jwtService.RefreshToken(refreshTokenRequest.RefreshToken);
                return Ok(new TokenResponse()
                {
                    RefreshToken = token.Value.ToString(),
                    Email = token.User.Email,
                    Token = token.Token
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(new ErrorResponse() { Message = ex.Message });
            }
        }
    }
}
