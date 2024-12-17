using fournisseurIdentite.Services;

namespace fournisseurIdentite.src.DTO
{
    public class Users
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Pass { get; set; }

        public static Users CreateInstance(int id, string username, string email, string rawPassword, IPasswordService passwordService)
        {
            string hashedPassword = passwordService.HashPassword(rawPassword);

            return new Users
            {
                Id = id,
                Username = username,
                Email = email,
                Pass = hashedPassword
            };
        }
    }
}
