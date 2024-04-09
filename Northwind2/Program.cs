using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Northwind2.Data;
using Northwind2.Services;

namespace Northwind2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // R�cup�re la cha�ne de connexion � la base dans les param�tres
            string? connect = builder.Configuration.GetConnectionString("Northwind2Connect");

            // Enregistre la classe de contexte de donn�es comme service
            // en lui indiquant la connexion � utiliser
            builder.Services.AddDbContext<ContexteNorthwind>(opt => opt.UseSqlServer(connect));
            builder.Services.AddScoped<IServiceEmployes, ServiceEmploye>();


            // Ajoute le service d'authentification par porteur de jetons JWT
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   // url d'acc�s au serveur d'identit�
                   options.Authority = builder.Configuration["IdentityServerUrl"];
                   options.TokenValidationParameters.ValidateAudience = false;

                   // Tol�rance sur la dur�e de validit� du jeton
                   options.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
               });

            // Ajoute le service d'autorisation
            builder.Services.AddAuthorization(options =>
            {
                // Sp�cifie que tout utilisateur de l'API doit �tre authentifi�
                options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
