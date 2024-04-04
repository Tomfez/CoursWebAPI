using Microsoft.EntityFrameworkCore;
using Northwind2.Data;
using Northwind2.Entities;

namespace Northwind2.Services
{
    public interface IServiceEmployes
    {
        Task<List<Employe>> ObtenirEmployes(string? nomEmploye);
        Task<Employe?> ObtenirEmploye(int id);
        Task<Region?> GetRegion(int id);
        Task<Employe> AjouterEmploye(Employe employe);
    }

    public class ServiceEmploye : IServiceEmployes
    {
        private readonly ContexteNorthwind _context;

        public ServiceEmploye(ContexteNorthwind context)
        {
            _context = context;
        }

        public async Task<List<Employe>> ObtenirEmployes(string? nomEmploye)
        {
            // Syntaxe de méthode
            //DbSet<Employe> employes = _context.Employes;

            // Syntaxe de requête
            IQueryable<Employe> req = from e in _context.Employes
                                      where nomEmploye == null || e.Nom.Contains(nomEmploye)
                                      select new Employe
                                      {
                                          Id = e.Id,
                                          Civilite = e.Civilite,
                                          Nom = e.Nom,
                                          Prenom = e.Prenom,
                                          Fonction = e.Fonction,
                                          DateEmbauche = e.DateEmbauche
                                      };

            return await req.ToListAsync();
        }

        public async Task<Employe?> ObtenirEmploye(int id)
        {
            //Syntaxe de méthode
            //ValueTask<Employe?> req = _context.Employes.FindAsync(id);

            // Syntaxe de requête
            IQueryable<Employe?> req = from e in _context.Employes
                                       where e.Id == id
                                       select e;

            return await req.FirstOrDefaultAsync();
        }

        public async Task<Region?> GetRegion(int id)
        {
            var req = from r in _context.Regions.Include(r => r.Territoires)
                      where r.Id == id
                      select r;

            return await req.FirstOrDefaultAsync();

        }

        public async Task<Employe> AjouterEmploye(Employe employe)
        {
            // Ajoute l'employé dans le DbSet
            _context.Add(employe);

            // Enregistre l'employé dans la BDD et affecte l'ID
            await _context.SaveChangesAsync();

            // Renvoie l'employé avec son ID généré
            return employe;
        }
    }
}
