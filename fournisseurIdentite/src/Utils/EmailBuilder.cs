using System.Numerics;
using System.Text;

namespace fournisseurIdentite.src.Utils
{
    public class EmailBuilder {
        public static String buildValidationMail(int idUser, String nomUser){
            String header = "<h2>Hello, "+nomUser+"</h2>";
            String body = "<p>Votre compte a été créé avec succès, veuillez cliquer sur ce lien pour le valider</p>";
            String link = "<a href=http://localhost:5092/api/users/valider?id="+idUser+">Valider</a>";
            return header + body + link;
        }
    }
}