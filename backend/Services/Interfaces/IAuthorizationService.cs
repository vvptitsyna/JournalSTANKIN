namespace JournalAPI.Services.Interfaces
{
    public interface IAuthorizationService
    {
        public Task LoginAsync(string username, string password);
        public Task LogoutAsync();
    }
}
