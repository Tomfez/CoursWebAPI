using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Northwind2.Data;
using Northwind2.Services;
using System.Text.Json.Serialization;

namespace Northwind2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            // Enregistre les contrôleurs et ajoute une option de sérialisation
            // pour interrompre les références circulaires infinies
            builder.Services.AddControllers()
                .AddNewtonsoftJson(opt =>
                opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                .AddJsonOptions(opt =>
                opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            //builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddOpenApiDocument(options =>
            {
                options.Title = "API Northwind";
                options.Description = "<strong>API Northwind2 pour formation ASP.Net Core.<br/>Code dispo sur <a href='https://github.com/developpeur-pro/CoursWebAPI'>ce référentiel GitHub</a></strong>";
                options.Version = "v1";
            });

            // Récupère la chaîne de connexion à la base dans les paramètres
            string? connect = builder.Configuration.GetConnectionString("Northwind2Connect");

            // Enregistre la classe de contexte de données comme service
            // en lui indiquant la connexion à utiliser
            builder.Services.AddDbContext<ContexteNorthwind>(opt => opt.UseSqlServer(connect));
            builder.Services.AddScoped<IServiceEmployes, ServiceEmploye>();


            // Ajoute le service d'authentification par porteur de jetons JWT
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   // url d'accès au serveur d'identité
                   options.Authority = builder.Configuration["IdentityServerUrl"];
                   options.TokenValidationParameters.ValidateAudience = false;

                   // Tolérance sur la durée de validité du jeton
                   options.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
               });

            // Ajoute le service d'autorisation
            builder.Services.AddAuthorization(options =>
            {
                // Spécifie que tout utilisateur de l'API doit être authentifié
                options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                // Middleware serveur de définition d'API
                app.UseOpenApi();

                // Interface web pour la doc
                app.UseSwaggerUi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
