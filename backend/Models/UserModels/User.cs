using Microsoft.AspNetCore.Identity;

namespace JournalAPI.Models.UserModels
{

    public class User:IdentityUser<int>
    {
        public string? Comment { get; set; }

        /// <summary>
        /// Связи пользователя
        /// </summary>
        public List<UserRelation> Relations { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is User user)
                return user.UserName == UserName;

            return false;
        }

        public override int GetHashCode()
            => UserName.GetHashCode();

    }
}
