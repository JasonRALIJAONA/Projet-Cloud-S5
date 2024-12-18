# Projet-Cloud-S5

## note pour le groupe
* A changer dans appsettings.json config connexion base de donnee

* ne pas oublier:
    git rm -r --cached .
    git add .
    git commit -m "Mettre à jour .gitignore pour ignorer les fichiers appropriés"

    apres avoir change le gitignore

* A faire dans le terminal si ce n'est pas encore fait
    dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
    dotnet add package Microsoft.EntityFrameworkCore.Design
    dotnet add package Microsoft.EntityFrameworkCore.Tools

* pour mapper la base vers le code
    dotnet ef dbcontext scaffold "Host=127.0.0.1;Database=fournisseur_identite;Username=postgres;Password=Etu002610" Npgsql.EntityFrameworkCore.PostgreSQL -o Models --force


