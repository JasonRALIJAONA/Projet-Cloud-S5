using fournisseurIdentite.Services;

namespace fournisseurIdentite.Models
{
    public partial class Utilisateur
    {
        public static Utilisateur CreateInstance(int id, string username, string email, string rawPassword, IPasswordService passwordService)
        {
            string hashedPassword = passwordService.HashPassword(rawPassword);

            return new Utilisateur
            {
                Id = id,
                NomUtilisateur = username,
                Email = email,
                MotDePasse = hashedPassword
            };
        }
    }
}
