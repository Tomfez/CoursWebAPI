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
