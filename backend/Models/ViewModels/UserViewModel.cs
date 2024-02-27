namespace JournalAPI.Models.ViewModels
{
    public class UserViewModel
    {
        public int? Id { get; set; }
        public bool? NeedChangePassword { get; set; }
        public string UserName { get; set; }
        public string? Password { get; set; }
        public string Role { get; set; }    
        public string? Comment { get; set; }
    }
}
