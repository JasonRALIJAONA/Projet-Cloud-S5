using System;
using System.Collections.Generic;

namespace fournisseurIdentite.Models;

public partial class Utilisateur
{
    public int Id { get; set; }

    public string? NomUtilisateur { get; set; }

    public string? Email { get; set; }

    public string MotDePasse { get; set; } = null!;

    public bool? EstValide { get; set; }

    public int? NbTentative { get; set; }
}
