using fournisseurIdentite.Models;
using fournisseurIdentite.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuration des services
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<EmailService>(provider => new EmailService(
    "smtp.gmail.com",       // Serveur SMTP
    587,                    // Port SMTP
    "jasonralijaona@gmail.com", // Utilisateur SMTP
    "ngddkrpplobkmkzj"      // Mot de passe SMTP
));
    //"hajainaraz28@gmail.com", // Utilisateur SMTP
    //"thmc dxau fbtd jsoy"      // Mot de passe SMTP000.
builder.Services.AddScoped<IPasswordService, PasswordService>();

builder.Services.AddScoped<PINService>();

// Configuration de la gestion des sessions
builder.Services.AddDistributedMemoryCache(); // Nécessaire pour stocker les sessions en mémoire
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20); // Durée de validité des sessions
    options.Cookie.HttpOnly = true;                // Sécurise l'accès cookie côté serveur
    options.Cookie.IsEssential = true;             // Essentiel pour les fonctionnalités critiques
});

// Configuration de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuration de la base de données
builder.Services.AddDbContext<FournisseurIdentiteContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configuration du pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Activer la gestion des sessions
app.UseSession();

app.UseAuthorization();

app.MapControllers();

// Exemple d'API simple
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
