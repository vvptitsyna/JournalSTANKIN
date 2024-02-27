using Microsoft.AspNetCore.Identity;

namespace JournalAPI.Services.Implementations
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly SignInManager<User> _signInManager;
        public AuthorizationService(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task LoginAsync(string username, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(username, password, true, false);
            
            if(!result.Succeeded)
            {
                throw new Exception("Введен неправильный логин или пароль");
            }
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
