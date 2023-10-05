using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using TestDB.Entities;
using TestTask.Abstractions;
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
        public UserController(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        /// <summary>
        /// Получение списка всех пользователей
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromBody] GetAllUsersWithPagRequest request)
        {
            var users = await _userService.GetAllUsersWithPag(request.PageSize, request.PageNumber);
            return Ok(users);
        }

        /// <summary>
        ///Получение пользователя по Id и всех его ролей
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        /// <summary>
        /// Добавление пользователя
        /// </summary>
        /// <param name="addUserRequest"></param>
        /// <returns></returns>
        [HttpPost]       
        public async Task<IActionResult> AddUser([BindRequired, FromQuery] AddUserRequest addUserRequest)
        {
            if(await _userService.IsEmailExist(addUserRequest.Email))
            {
                var user = new User
                {
                    Name = addUserRequest.UserName,
                    Age = addUserRequest.Age,
                    Email = addUserRequest.Email,
                };
                var role = await _roleService.GetDefaultRole();
                user.Roles.Add(role);
                await _userService.AddUser(user);
                return Ok();
            }
            return BadRequest(new ErrorResponse() { Message = "Email already exist" });
        }

        /// <summary>
        /// Добавление пользователю роли
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPatch("{id}, {RoleName}")]
        public async Task<IActionResult> AddRoleToUser([FromBody] AddRoleRequest request)
        {
            var role = await _roleService.GetRoleByName(request.RoleName);
            if(await _roleService.IsRoleExists(request.RoleName) && !await _userService.HasRole(request.UserId, role))
            {
                await _userService.AddRoleToUser(role, request.UserId);
            }
            return BadRequest(new ErrorResponse() { Message = "Role doesn't exist or User already have this role" });
        }

        /// <summary>
        /// Обновление информации о пользователе по Id
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("{id},{Name}, {Age}, {Email}")]
        public async Task<IActionResult> UpdateUserInfo([FromBody] UpdateUserRequest request)
        {
            var user = await _userService.GetUserById(request.UserId);
            user.Name = request.UserName;
            user.Age = request.Age;
            user.Email = request.Email;
            await _userService.UpdateUser(user);
            return Ok();
        }
        /// <summary>
        /// Удалиние пользователя по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if(!await _userService.IsUserExists(id))
            {
                await _userService.DeleteUser(id);
                return Ok();
            }
            return BadRequest(new ErrorResponse() { Message = "User doesn't exist" });
            
        }

    }
}
