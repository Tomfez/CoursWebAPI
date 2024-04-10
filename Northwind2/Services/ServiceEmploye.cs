using Microsoft.EntityFrameworkCore;
using Northwind2.Data;
using Northwind2.Entities;

namespace Northwind2.Services
{
    public interface IServiceEmployes
    {
        Task<List<Employe>> ObtenirEmployes();
        Task<Employe?> ObtenirEmploye(int id);
        Task<Region?> ObtenirRegion(int id);
    }

    public class ServiceEmploye : IServiceEmployes
    {
        private readonly ContexteNorthwind _context;

        public ServiceEmploye(ContexteNorthwind context)
        {
            _context = context;
        }

        public async Task<List<Employe>> ObtenirEmployes()
        {
            return await _context.Employes.ToListAsync();
        }

        public async Task<Employe?> ObtenirEmploye(int id)
        {
            return await _context.Employes.FindAsync(id);
        }

        public async Task<Region?> ObtenirRegion(int id)
        {
            var req = from r in _context.Regions.Include(r => r.Territoires)
                      where r.Id == id
                      select r;

            return await req.FirstOrDefaultAsync();
        }
    }
}
