using System;
using fournisseurIdentite.Services;
using fournisseurIdentite.Models;
using fournisseurIdentite.src.DTO;
using fournisseurIdentite.src.Utils;

public class UtilisateurService
{
    private readonly FournisseurIdentiteContext _context;

    private readonly IPasswordService _passwordService;

    private readonly EmailService _emailService;

    public UtilisateurService(FournisseurIdentiteContext context, IPasswordService passwordService, EmailService emailService)
    {
        _context = context;
        _passwordService = passwordService;
        _emailService = emailService;
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

    public async Task<Utilisateur> UpdateUtilisateur(UpdateUtilisateurDto updatedUtilisateur)
    {
        if (updatedUtilisateur == null)
        {
            throw new ArgumentNullException(nameof(updatedUtilisateur), "The utilisateur object cannot be null.");
        }

        var existingUtilisateur = _context.Utilisateurs.FirstOrDefault(u => u.Email == updatedUtilisateur.email);
        if (existingUtilisateur == null)
        {
            throw new InvalidOperationException($"Utilisateur with email {updatedUtilisateur.email} does not exist.");
        }

        existingUtilisateur.NomUtilisateur = updatedUtilisateur.NomUtilisateur ?? existingUtilisateur.NomUtilisateur;

        if (!string.IsNullOrEmpty(updatedUtilisateur.MotDePasse))
        {
            string mdphash = _passwordService.HashPassword(updatedUtilisateur.MotDePasse);
            existingUtilisateur.MotDePasse = mdphash;

            // Réinitialiser le nombre de tentatives si le mot de passe est changé
            ReinitializeTentative(updatedUtilisateur.email ?? "");

            await _emailService.SendEmailAsync(updatedUtilisateur.email ?? "", "Reinitialisation Mot De Passe", EmailBuilder.buildMessageMotDePasseReinitialiser(updatedUtilisateur.NomUtilisateur ?? ""));
        }
        
        _context.Utilisateurs.Update(existingUtilisateur);
        _context.SaveChanges();

        return existingUtilisateur;
    }

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
