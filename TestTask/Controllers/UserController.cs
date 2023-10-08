using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Runtime.CompilerServices;
using TestDB.Entities;
using TestTask.Abstractions;
using TestTask.DTOs;
using TestTask.Requests;
using TestTask.Responses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserService userService, IRoleService roleService, IMapper mapper, ILogger<UserController> logger)
        {
            _userService = userService;
            _roleService = roleService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Получение списка всех пользователей
        /// </summary>
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAll([FromQuery] GetAllUsersWithPagRequest getAllRequest)
        {
            try
            {
                var users = await _userService.GetAllUsersWithPag(getAllRequest.PageSize, getAllRequest.PageNumber);
                if (!getAllRequest.OrderBy.IsNullOrEmpty())
                {
                    switch (getAllRequest.OrderBy)
                    {
                        case "Name":
                            users = users.OrderBy(x => x.Name).ToList();
                            break;
                        case "Age":
                            users = users.OrderBy(x => x.Age).ToList();
                            break;
                        case "Email":
                            users = users.OrderBy(x => x.Email).ToList();
                            break;
                        case "RoleName":
                            users = users.OrderBy(x => x.Roles.OrderBy(x => x.Name)).ToList();
                            break;
                        default:
                            return BadRequest(new ErrorResponse() { Message = "Unknown sort parameter" });
                    }
                }
                if (!getAllRequest.FilterBy.IsNullOrEmpty() && !getAllRequest.FilterByValue.IsNullOrEmpty())
                {
                    switch (getAllRequest.FilterBy)
                    {
                        case "Name":
                            users = users.Where(a => a.Name.Equals(getAllRequest.FilterByValue)).ToList();
                            break;
                        case "Age":
                            users = users.Where(a => a.Age.Equals(getAllRequest.FilterByValue)).ToList();
                            break;
                        case "Email":
                            users = users.Where(a => a.Email.Equals(getAllRequest.FilterByValue)).ToList();
                            break;
                        case "RoleName":
                            users = users.Where(a => a.Roles.Any(b => b.Name.Equals(getAllRequest.FilterByValue))).ToList();
                            break;
                        default:
                            return BadRequest(new ErrorResponse() { Message = "Unknown filter parameters" });
                    }
                }
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(new ErrorResponse() { Message=ex.Message });
            }
        }

        /// <summary>
        ///Получение пользователя по Id и всех его ролей
        /// </summary>
        [HttpGet("GetUserById")]
        public async Task<IActionResult> Get([FromQuery, BindRequired]int id)
        {
            try
            {
                var user = await _userService.GetUserById(id);
                if (user == null)
                {
                    return NotFound(new ErrorResponse() { Message = "User doesn't exist" });
                }

                return Ok(_mapper.Map<UserResponse>(user));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(new ErrorResponse() { Message = ex.Message });
            }
        }

        /// <summary>
        /// Добавление пользователя
        /// </summary>
        /// <param name="addUserRequest"></param>
        /// <returns></returns>
        [HttpPost("AddUser")]       
        public async Task<IActionResult> AddUser([FromQuery] AddUserRequest addUserRequest)
        {
            try
            {
                if (!await _userService.IsEmailExist(addUserRequest.Email))
                {
                    var role = await _roleService.GetDefaultRole();
                    var user = new UserDTO
                    {
                        Name = addUserRequest.UserName,
                        Age = addUserRequest.Age,
                        Email = addUserRequest.Email                        
                    };                    
                    await _userService.AddUser(user);
                    var userRole = await _userService.GetByEmail(addUserRequest.Email);
                    await _userService.AddRoleToUser(role, userRole.ID);
                    return Ok();
                }
                return BadRequest(new ErrorResponse() { Message = "Email already exist" });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(new ErrorResponse() { Message= ex.Message });
            }
        }

        /// <summary>
        /// Добавление пользователю роли
        /// </summary>
        /// <param name="addRoleRequest"></param>
        /// <returns></returns>
        [HttpPatch("AddRoleToUser")]
        public async Task<IActionResult> AddRoleToUser([FromQuery] AddRoleRequest addRoleRequest)
        {
            try
            {
                var role = await _roleService.GetRoleByName(addRoleRequest.RoleName);
                if (await _roleService.IsRoleExists(addRoleRequest.RoleName) && !await _userService.HasRole(addRoleRequest.UserId, role))
                {
                    await _userService.AddRoleToUser(role, addRoleRequest.UserId);
                    return Ok();
                }
                return BadRequest(new ErrorResponse() { Message = "Role doesn't exist or User already have this role" });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(new ErrorResponse() { Message =  ex.Message });
            }
        }

        /// <summary>
        /// Обновление информации о пользователе по Id
        /// </summary>
        /// <param name="updateUserRequest"></param>
        /// <returns></returns>
        [HttpPost("UpdateUserInfo")]
        public async Task<IActionResult> UpdateUserInfo([FromQuery] UpdateUserRequest updateUserRequest)
        {
            try
            {
                var valuesToPatch = new List<PatchDTO>()
                {
                    new PatchDTO(){Name = "Name", Value = updateUserRequest.UserName},
                    new PatchDTO(){Name = "Age", Value = updateUserRequest.Age},
                    new PatchDTO(){Name = "Email", Value = updateUserRequest.Email}
                };
                await _userService.UpdateUser(updateUserRequest.UserId, valuesToPatch);
                
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(new ErrorResponse() { Message = ex.Message});
            }
        }
        /// <summary>
        /// Удаление пользователя по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteUserById")]
        public async Task<IActionResult> DeleteUser([FromQuery, BindRequired] int id)
        {
            try
            {
                if (await _userService.IsUserExists(id))
                {
                    await _userService.DeleteUser(id);
                    return Ok();
                }
                return BadRequest(new ErrorResponse() { Message = "User doesn't exist" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(new ErrorResponse() { Message=ex.Message});
            }
            
        }

    }
}
