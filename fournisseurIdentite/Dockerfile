# Étape 1 : Construire l'application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copier tous les fichiers du projet
COPY . .

# Restaurer les dépendances
RUN dotnet restore

# Publier l'application
RUN dotnet publish -c Release -o /app/out

# Étape 2 : Utiliser l'image SDK pour inclure les outils de développement
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS runtime
WORKDIR /app

# Copier tous les fichiers source (y compris .csproj) pour les outils comme dotnet ef
COPY . .

# Copier les fichiers publiés
COPY --from=build /app/out ./out

# Installer dotnet-ef
RUN dotnet tool install --global dotnet-ef

# Ajouter dotnet-ef au PATH
ENV PATH="$PATH:/root/.dotnet/tools"

# Définir le répertoire de travail pour le runtime
WORKDIR /app/out

# Exposer le port utilisé par l'application
EXPOSE 5092

# Commande pour démarrer l'application
ENTRYPOINT ["dotnet", "fournisseurIdentite.dll"]
