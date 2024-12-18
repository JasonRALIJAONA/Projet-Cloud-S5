using fournisseurIdentite.Services;

namespace fournisseurIdentite.Models
{
    public partial class Users
    {
        public static User CreateInstance(int id, string username, string email, string rawPassword, IPasswordService passwordService)
        {
            string hashedPassword = passwordService.HashPassword(rawPassword);

            return new User
            {
                Id = id,
                Username = username,
                Email = email,
                Pass = hashedPassword
            };
        }
    }
}
