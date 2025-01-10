# Use the official .NET image as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["fournisseurIdentite/fournisseurIdentite.csproj", "fournisseurIdentite/"]
RUN dotnet restore "fournisseurIdentite/fournisseurIdentite.csproj"
COPY . .
WORKDIR "/src/fournisseurIdentite"
RUN dotnet build "fournisseurIdentite.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "fournisseurIdentite.csproj" -c Release -o /app/publish

# Use the base image to run the app
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "fournisseurIdentite.dll"]