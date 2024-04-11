﻿using Duende.Bff;
using Duende.Bff.Yarp;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

/**
 * 
 * Enregistrer le service BFF
 * 
 **/
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddBff().AddRemoteApis();


/**
 * 
 * Configurer les schémas d'authentification
 * 
 **/
builder.Services.AddAuthentication(options =>
{
    // Schémas d'authentification par défaut
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = "oidc";
    //options.DefaultSignOutScheme = "oidc";
})
    .AddCookie()
    // Authentification par cookie
    //.AddCookie("cookie", options =>
    //{
    //    options.Cookie.Name = "__Host-blazor";
    //    options.Cookie.SameSite = SameSiteMode.Strict;
    //})

    // Authentification OIDC
    .AddOpenIdConnect("oidc", options =>
    {
        // Url d'accès au fournisseur d'identité OIDC 
        options.Authority = builder.Configuration["IdentityServerUrl"];

        // Id et code secret de l'appli cliente
        options.ClientId = "Client1";
        options.ClientSecret = "Secret1";

        // Type de flux de communication avec le serveur
        options.ResponseType = "code";
        options.ResponseMode = "query";

        options.Scope.Clear();
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("entreprise"); // Pour pouvoir récupérer les revendications associées
        options.Scope.Add("offline_access"); // pour utiliser le jeton d'actualisation

        // Active la récupération des revendications sur le point de terminaison
        // des infos utilisateur après obtention d'un jeton d'identité
        options.MapInboundClaims = false;
        options.GetClaimsFromUserInfoEndpoint = true;

        // Enregistre les jetons d'accès et d'actualisation dans le cookie d'authentification
        options.SaveTokens = true;

        // Paramètres nécessaires à OIDC pour valider les jetons
        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = "name",
            RoleClaimType = "role"
        };
    });

/**
 * 
 * Gérer les jetons
 * 
 **/
// Enregistre le service de gestion de jetons pour appli clientes avec utilisateurs
builder.Services.AddUserAccessTokenManagement();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

// Ajoute les middlewares d'authentification, de BFF et d'autorisation
app.UseAuthentication();
app.UseBff();
app.UseAuthorization();

// Ajoute les points de terminaison pour la connexion / déconnexion
app.MapBffManagementEndpoints();


app.MapRazorPages();

// Ajoute les points de terminaison correspondants aux contrôleurs de l'API locale (du backend)
// avec la politique d'autorisation par défaut
// et indique qu'il s'agit de points de terminaison locaux du backend
app.MapControllers()
    .RequireAuthorization()
    .AsBffApiEndpoint();

// Mappe les points de terminaison de l'API externe sur l'API locale
app.MapRemoteBffApiEndpoint("/Regions", builder.Configuration["ApiUrl"] + "Regions")
       .RequireAccessToken(TokenType.User);

app.MapFallbackToFile("index.html");

app.Run();