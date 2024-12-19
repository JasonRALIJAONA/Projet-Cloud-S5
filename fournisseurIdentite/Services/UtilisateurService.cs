using System;
using fournisseurIdentite.Services;
using fournisseurIdentite.Models;

public class UtilisateurService
{
    private readonly FournisseurIdentiteContext _context;

    public UtilisateurService(FournisseurIdentiteContext context)
    {
        _context = context;
    }

    public Utilisateur CreateUtilisateur(Utilisateur utilisateur)
    {
        if (utilisateur == null)
        {
            throw new ArgumentNullException(nameof(utilisateur), "The utilisateur object cannot be null.");
        }

        _context.Utilisateurs.Add(utilisateur);
        _context.SaveChanges();
        return utilisateur;
    }
}
