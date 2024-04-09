using Duende.IdentityServer.Models;
using HoteIdentityServer.Data;
using HoteIdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HoteIdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Capture les exceptions de base de donn�es r�solvables par migration
            // et envoie une r�ponse HTML invitant � migrer la base (� utiliser en mode dev uniquement)
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            // Ajoute des services d'identit�s communs :
            // - une interface utilisateur par d�faut,
            // - des fournisseurs de jetons pour g�n�rer des jetons afin de r�initialiser les mots de passe,
            // modifier l'e-mail et modifier le N� de tel, et pour l'authentification � 2 facteurs.
            // - configure l'authentification pour utiliser les cookies d'identit�
            builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddRazorPages();

            // Ajoute et configure le service IdentityServer
            builder.Services.AddIdentityServer(options =>
                  options.Authentication.CoordinateClientLifetimesWithUserSession = true)

                // Cr�e des identit�s
                .AddInMemoryIdentityResources(new IdentityResource[] {
         new IdentityResources.OpenId(),
         new IdentityResources.Profile(),
                })

                // Configure une appli cliente
                .AddInMemoryClients(new Client[] {
         new Client
         {
            ClientId = "Client1",
            ClientSecrets = { new Secret("Secret1".Sha256()) },
            AllowedGrantTypes = GrantTypes.Code,

            // Urls auxquelles envoyer les jetons
            RedirectUris = { "https://localhost:6001/signin-oidc" },
            // Urls de redirection apr�s d�connexion
            PostLogoutRedirectUris = { "https://localhost:6001/signout-callback-oidc" },
            // Url pour envoyer une demande de d�connexion au serveur d'identit�
            FrontChannelLogoutUri = "https://localhost:6001/signout-oidc",

            // Etendues d'API autoris�es
            AllowedScopes = { "openid", "profile" },

            // Autorise le client � utiliser un jeton d'actualisation
            AllowOfflineAccess = true
         }
                })
                // Indique d'utiliser ASP.Net Core Identity pour la gestion des profils et revendications
                .AddAspNetIdentity<ApplicationUser>();

            // Ajoute la journalisation au niveau debug des �v�nements �mis par Duende
            builder.Services.AddLogging(options =>
            {
                options.AddFilter("Duende", LogLevel.Debug);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");

                // Ajoute un en-t�te de r�ponse qui informe les navigateurs que le site ne doit �tre accessible qu'en utilisant HTTPS, et que toute tentative future d'y acc�der via HTTP doit �tre automatiquement convertie en HTTPS.
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Ajoute le middleware d'authentification avec IdentityServer dans le pipeline
            app.UseIdentityServer();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
