﻿namespace fournisseurIdentite.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Username { get; set; }

    public string? Email { get; set; }

    public string Pass { get; set; } = null!;

    public int? NbTentative { get; set; }
}