using JournalAPI.Models.ViewModels;

namespace JournalAPI.Services.Interfaces
{
    public interface IUserService
    {
        public Task<User> GetUserAsync(string username);
        public Task<User> GetUserAsync(int userId);
        public Task<List<UserViewModel>> GetUsersInfo();
        public Task<string> GetUserRoleAsync(string username);
        public Task<string> GetUserRoleAsync(int userId);
        public Task UpdateUserAsync(UserViewModel model);
        public Task AddUserAsync(string username, string password, string role, string comment = "");
        public Task<List<User>> GetUsersAsync();
        public Task<List<User>> GetUsersWithRoleAsync(string role);
        public Task AddUsersFromFileAsync(IFormFile file);
        public Task CreateDefaultUsers();
    }
}
