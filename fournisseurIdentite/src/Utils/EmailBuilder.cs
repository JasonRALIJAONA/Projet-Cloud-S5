using System.Numerics;
using System.Text;

namespace fournisseurIdentite.src.Utils
{
    public class EmailBuilder {
        private static String _footer="<footer>ETU2529-ETU2610-ETU2669-ETU2752</footer>";
        public static String buildValidationMail(int? idUser, String nomUser){
            String header = "<h2>Bonjour, "+nomUser+"</h2>";
            String body = "<p>Votre compte a été créé avec succès, veuillez cliquer sur ce lien pour le valider</p>";
            String link = "<a href=http://localhost:5092/api/users/valider?id="+idUser+">Valider</a>";
            return header + body + link+ _footer;
        }
        public static String buildPINMail(String pin, String username){
            string emailBody = $@"
                <!DOCTYPE html>
                <html lang='en'>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Votre Code PIN</title>
                </head>
                <body>
                    <p>Bonjour {username},</p>
                    <p>Voici votre code PIN pour accéder à votre compte :</p>
                    <p><strong style='font-size: 20px;'>{pin}</strong></p>
                    <p>Ce code est valable pendant 90 secondes.</p>
                    <p>Si vous n'avez pas demandé ce code, veuillez ignorer cet email.</p>
                    <p>Cordialement,</p>
                </body>
                </html>"; 
            return emailBody+_footer;
        }

        public static String buildMessageMotDePasseReinitialiser(String username){
            string emailBody = $@"
                <!DOCTYPE html>
                <html lang='en'>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Votre Code PIN</title>
                </head>
                <body>
                    <p>Bonjour {username},</p>
                    <p>Votre mot de passe a bien ete reinitialiser</p>
                    <p>Cordialement,</p>
                </body>
                </html>"; 
            return emailBody+_footer;
        }
    }
}