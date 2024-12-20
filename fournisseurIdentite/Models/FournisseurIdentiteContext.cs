using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace fournisseurIdentite.Models;

public partial class FournisseurIdentiteContext : DbContext
{
    public FournisseurIdentiteContext()
    {
    }

    public FournisseurIdentiteContext(DbContextOptions<FournisseurIdentiteContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Utilisateur> Utilisateurs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=127.0.0.1;Database=fournisseur_identite;Username=postgres;Password=admin");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Utilisateur>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("utilisateur_pkey");

            entity.ToTable("utilisateur");

            entity.HasIndex(e => e.Email, "utilisateur_email_key").IsUnique();

            entity.HasIndex(e => e.NomUtilisateur, "utilisateur_nom_utilisateur_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.EstValide)
                .HasDefaultValue(false)
                .HasColumnName("est_valide");
            entity.Property(e => e.MotDePasse)
                .HasMaxLength(100)
                .HasColumnName("mot_de_passe");
            entity.Property(e => e.NbTentative)
                .HasDefaultValue(0)
                .HasColumnName("nb_tentative");
            entity.Property(e => e.NomUtilisateur)
                .HasMaxLength(100)
                .HasColumnName("nom_utilisateur");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
