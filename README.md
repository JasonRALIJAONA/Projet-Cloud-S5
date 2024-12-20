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
    dotnet ef dbcontext scaffold "Host=127.0.0.1;Database=fournisseur_identite;Username=postgres;Password=hajaina" Npgsql.EntityFrameworkCore.PostgreSQL -o Models --force

# Pour le deploiement docker
## build le conteneur
docker-compose build 
## activer le conteneur en arriere-plan
docker-compose up -d
## ouvrir l'invite de commande du projet dans le conteneur
docker exec -it fournisseuridentite-webapp-1 bash
## migrer la base 
cd /app
dotnet ef dbcontext scaffold "Host=postgres_db;Port=5432;Database=fournisseur_identite;Username=postgres;Password=Etu002610" Npgsql.EntityFrameworkCore.PostgreSQL -o Models --force --project fournisseurIdentite.csproj
## sortir de l'invite de commande
exit
## eteindre le conteneur
docker-compose down



