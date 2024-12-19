namespace fournisseurIdentite.Services;

public class PINService
{
    public string CreatePIN (int length)
    {
        Random rnd = new();
        string pin = "";
        for (int i = 0; i < length; i++)
        { 
            pin += rnd.Next(0, 10).ToString();
        }
        return pin;
    }   
}