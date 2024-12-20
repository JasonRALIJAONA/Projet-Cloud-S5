using System;
using fournisseurIdentite.Services;
using fournisseurIdentite.Models;
using fournisseurIdentite.src.DTO;

public class UtilisateurService
{
    private readonly FournisseurIdentiteContext _context;

    private readonly IPasswordService _passwordService;

    public UtilisateurService(FournisseurIdentiteContext context, IPasswordService passwordService)
    {
        _context = context;
        _passwordService = passwordService;
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
    // public Utilisateur UpdateUtilisateur(Utilisateur updatedUtilisateur)
    // {
    //     if (updatedUtilisateur == null)
    //     {
    //         throw new ArgumentNullException(nameof(updatedUtilisateur), "The utilisateur object cannot be null.");
    //     }

    //     var existingUtilisateur = _context.Utilisateurs.Find(updatedUtilisateur.Id);
    //     if (existingUtilisateur == null)
    //     {
    //         throw new InvalidOperationException($"Utilisateur with ID {updatedUtilisateur.Id} does not exist.");
    //     }

    //     // Update fields
    //     existingUtilisateur.NomUtilisateur = updatedUtilisateur.NomUtilisateur ?? existingUtilisateur.NomUtilisateur;
    //     existingUtilisateur.MotDePasse = updatedUtilisateur.MotDePasse ?? existingUtilisateur.MotDePasse;
    //     // existingUtilisateur.Email = updatedUtilisateur.Email;
    //     // existingUtilisateur.EstValide = updatedUtilisateur.EstValide;
    //     // existingUtilisateur.NbTentative = updatedUtilisateur.NbTentative;

    //     _context.Utilisateurs.Update(existingUtilisateur);
    //     _context.SaveChanges();

    //     return existingUtilisateur;
    // }
    public Utilisateur UpdateUtilisateur(UpdateUtilisateurDto updatedUtilisateur)
    {
        if (updatedUtilisateur == null)
        {
            throw new ArgumentNullException(nameof(updatedUtilisateur), "The utilisateur object cannot be null.");
        }

        var existingUtilisateur = _context.Utilisateurs.Find(updatedUtilisateur.Id);
        if (existingUtilisateur == null)
        {
            throw new InvalidOperationException($"Utilisateur with ID {updatedUtilisateur.Id} does not exist.");
        }

        existingUtilisateur.NomUtilisateur = updatedUtilisateur.NomUtilisateur ?? existingUtilisateur.NomUtilisateur;
        string mdphash = _passwordService.HashPassword((updatedUtilisateur.MotDePasse) ?? "");
        existingUtilisateur.MotDePasse = string.IsNullOrEmpty(mdphash)
            ? existingUtilisateur.MotDePasse
            : mdphash;
        
        _context.Utilisateurs.Update(existingUtilisateur);
        _context.SaveChanges();

        return existingUtilisateur;
    }
//    public Utilisateur AddTentative(UtilisateurTentativeDto dto)
//     {
//         if (dto == null)
//         {
//             throw new ArgumentNullException(nameof(dto), "Le DTO ne peut pas être nul.");
//         }

//         // Rechercher l'utilisateur dans la base de données par ID
//         var utilisateur = _context.Utilisateurs.Find(dto.Id);
//         if (utilisateur == null)
//         {
//             throw new InvalidOperationException($"Utilisateur avec l'ID {dto.Id} n'existe pas.");
//         }

//         // Ajouter le nombre de tentatives et mettre à jour
//         utilisateur.NbTentative = (utilisateur.NbTentative ?? 0) + dto.NbTentativeAjouter;

//         // Sauvegarder les modifications
//         _context.Utilisateurs.Update(utilisateur);
//         _context.SaveChanges();

//         // Retourner le nouveau nombre de tentatives
//         return utilisateur;
//     }
   public Utilisateur AddTentative(string email)
    {
        if (email == null)
        {
            throw new ArgumentNullException(nameof(email), "Le email ne peut pas être nul.");
        }

        // Rechercher l'utilisateur dans la base de données par ID
        var utilisateur = _context.Utilisateurs.FirstOrDefault(u => u.Email == email);
        if (utilisateur == null)
        {
            throw new InvalidOperationException($"Utilisateur avec l'email {email} n'existe pas.");
        }

        // Ajouter le nombre de tentatives et mettre à jour
        utilisateur.NbTentative = (utilisateur.NbTentative ?? 0) + 1;

        // Sauvegarder les modifications
        _context.Utilisateurs.Update(utilisateur);
        _context.SaveChanges();

        // Retourner le nouveau nombre de tentatives
        return utilisateur;
    }

    public Utilisateur ReinitializeTentative(string email)
    {
        if (email == null)
        {
            throw new ArgumentNullException(nameof(email), "Le DTO ne peut pas être nul.");
        }

        // Rechercher l'utilisateur dans la base de données par ID
        var utilisateur = _context.Utilisateurs.FirstOrDefault(u => u.Email == email);
        if (utilisateur == null)
        {
            throw new InvalidOperationException($"Utilisateur avec l'email {email} n'existe pas.");
        }

        utilisateur.NbTentative = 0;
        _context.Utilisateurs.Update(utilisateur);
        _context.SaveChanges();

        // Retourner le nouveau nombre de tentatives
        return utilisateur;
    }

}
