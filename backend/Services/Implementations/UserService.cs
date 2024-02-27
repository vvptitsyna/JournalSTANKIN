using JournalAPI.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Principal;

namespace JournalAPI.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        public UserService(AppDbContext context, UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<List<User>> GetUsersAsync()
            => await _context.Users.ToListAsync();

        public async Task<User> GetUserAsync(string username)
        {
            if (!UserExist(username))
                throw new Exception("Пользователь с таким логином не найден");

            return await _context.Users.FirstAsync(u => u.UserName == username);
        }

        public async Task<User> GetUserAsync(int userId)
        {
            if (!UserExist(userId))
                throw new Exception("Пользователь с таким Id не найден");

            return await _context.Users.FirstAsync(u => u.Id == userId);
        }

        public async Task<List<UserViewModel>> GetUsersInfo()
        {
            var users = await _context.Users.ToListAsync();

            var userInfos = new List<UserViewModel>();

            foreach (var user in users)
            {
                userInfos.Add(new UserViewModel
                {
                    Id = user.Id,
                    Comment = user.Comment,
                    UserName = user.UserName,
                    Role = await GetUserRoleAsync(user.UserName)
                });
            }

            return userInfos;
        }


        public async Task<List<User>> GetUsersWithRoleAsync(string role)
        {
            if (!CheckRole(role))
                throw new Exception($"Роль {role} не существует");

            var users = await GetUsersAsync();

            var userWithRole = new List<User>();

            foreach (var user in users)
            {
                var userRole = await GetUserRoleAsync(user.Id);

                if (userRole == role)
                    userWithRole.Add(user);
            }

            return userWithRole;
        }

        public async Task<string> GetUserRoleAsync(string username)
        {
            var user = await GetUserAsync(username);

            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Count > 1)
            {
                throw new Exception($"У пользователя {user.UserName} больше одной роли");
            }
            else
            {
                if (roles.Count == 0)
                    throw new Exception($"У пользователя {user.UserName} нет роли");

                return roles.First();
            }
        }

        public async Task<string> GetUserRoleAsync(int userId)
        {
            var user = await GetUserAsync(userId);

            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Count > 1)
            {
                throw new Exception($"У пользователя {user.UserName} больше одной роли");
            }
            else
            {
                if (roles.Count == 0)
                    throw new Exception($"У пользователя {user.UserName} нет роли");

                return roles.First();
            }
        }

        public async Task UpdateUserAsync(UserViewModel model)
        {
            if (!CheckRole(model.Role))
                throw new Exception($"Роль {model.Role} не существует");

            var user = await _context.Users.FirstAsync(u => u.Id == model.Id);

            if (user is null)
                throw new Exception($"Пользователь с Id {model.Id} не найден");

            if (user.UserName != model.UserName)
                user.UserName = model.UserName;

            if (user.Comment != model.Comment)
                user.Comment = model.Comment;

            if (!await _userManager.IsInRoleAsync(user, model.Role))
            {
                var roles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, roles);
                await _userManager.AddToRoleAsync(user, model.Role);
            }

            if (model.NeedChangePassword.HasValue && model.NeedChangePassword.Value && !string.IsNullOrWhiteSpace(model.Password))
            {
                await _userManager.RemovePasswordAsync(user);
                await _userManager.AddPasswordAsync(user, model.Password);
            }
        }

        public async Task AddUserAsync(string username, string password, string role, string comment = "")
        {
            if (UserExist(username))
                throw new Exception("Пользователь с таким логином уже существует");

            if (!CheckRole(role))
                throw new Exception($"Роль {role} не существует");

            User user = new User { UserName = username, Comment = comment };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
                throw new Exception("Ошибка при создании пользователя");

            await _userManager.AddToRoleAsync(user, role);

            await _context.SaveChangesAsync();
        }

        public async Task AddUsersFromFileAsync(IFormFile file)
        {
            var usersFromExcel = ExcelHelper.GetUsersFromExcel(file);

            IEnumerable<string> userNames = _context.Users.Select(u => u.UserName);

            foreach (var user in usersFromExcel)
            {
                if (userNames.Contains(user.UserName))
                {
                    throw new Exception($"Пользователь с логином \"{user.UserName}\" уже существует ");
                }
                else
                {
                    var newUser = new User();

                    newUser.UserName = user.UserName;
                    newUser.Comment = user.Comment;

                    var createResult = await _userManager.CreateAsync(newUser, user.Password);

                    if (!createResult.Succeeded)
                        throw new Exception(createResult.Errors.First().Description);

                    var addRoleResult = await _userManager.AddToRoleAsync(newUser, user.Role);

                    if (!addRoleResult.Succeeded)
                        throw new Exception(addRoleResult.Errors.First().Description);
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task CreateDefaultUsers()
        {
            await CreateDefaultRoles();

            if (!UserExist("admin"))
            {
                await AddUserAsync("admin", "123", "Administrator");
            }

            if (!UserExist("user"))
            {
                await AddUserAsync("user", "123", "Teacher");
            }

            if (!UserExist("support"))
            {
                await AddUserAsync("support", "123", "Support");
            }
        }

        public async Task CreateDefaultRoles()
        {
            await _roleManager.CreateAsync(new IdentityRole<int>("Administrator"));
            await _roleManager.CreateAsync(new IdentityRole<int>("Support"));
            await _roleManager.CreateAsync(new IdentityRole<int>("Teacher"));
            await _context.SaveChangesAsync();
        }

        private bool UserExist(string username)
            => _context.Users.Any(u => u.UserName == username);

        private bool UserExist(int userId)
            => _context.Users.Any(u => u.Id == userId);

        private bool CheckRole(string role) => role.ToLower() switch
        {
            "administrator" => true,
            "support" => true,
            "teacher" => true,
            _ => false
        };
    }
}
