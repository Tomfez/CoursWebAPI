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

            // Récupère la chaîne de connexion à la base dans les paramètres
            string? connect = builder.Configuration.GetConnectionString("Northwind2Connect");

            // Enregistre la classe de contexte de données comme service
            // en lui indiquant la connexion à utiliser
            builder.Services.AddDbContext<ContexteNorthwind>(opt => opt.UseSqlServer(connect));
            builder.Services.AddScoped<IServiceEmployes, ServiceEmploye>();

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
