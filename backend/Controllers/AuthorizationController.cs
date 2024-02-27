using JournalAPI.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace JournalAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserService _userService;
        public AuthorizationController(IAuthorizationService authorizationService, IUserService userService)
        {
            _authorizationService = authorizationService;
            _userService = userService;
        }

        /// <summary>
        /// Получение  текущего пользователя
        /// </summary>
        /// <returns>Роль пользователя</returns>
        [HttpGet("getCurrentUser")]
        public async Task<User> GetCurrentUser()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var user = await _userService.GetUserAsync(User.Identity.Name);
                return user;
            }
            else
            {
                throw new Exception("Нет текущего пользователя");
            }
        }

        /// <summary>
        /// Получение роли текущего пользователя
        /// </summary>
        /// <returns>Роль пользователя</returns>
        [HttpGet("getCurrentUserRole")]
        public async Task<string> GetCurrentUserRole()
        {
            string role = "";
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                role = await _userService.GetUserRoleAsync(User.Identity.Name);
            }
            else
            {
                role = "No";
            }

            return role;
        }
        
        /// <summary>
        /// Создание пользователей по умолчанию
        /// </summary>
        [HttpGet("createDefaultUsers")]
        public async Task CreateDefaultUsers()
        {
            await _userService.CreateDefaultUsers();
        }

        /// <summary>
        /// Авторизация в системе
        /// </summary>
        /// <returns>Роль пользователя</returns>
        [HttpPost("login")]
        public async Task<string> Login([FromBody] LoginViewModel model)
        {
            await _authorizationService.LoginAsync(model.Username, model.Password);

            var role = await _userService.GetUserRoleAsync(model.Username);

            return role;
        }

        /// <summary>
        /// Выход из системы
        /// </summary>
        [HttpGet("logout")]
        public async Task Logout()
        {
            await _authorizationService.LogoutAsync();
        }
    }
}
