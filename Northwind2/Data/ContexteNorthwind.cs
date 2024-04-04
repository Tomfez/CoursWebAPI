using Microsoft.EntityFrameworkCore;
using Northwind2.Entities;

namespace Northwind2.Data
{
    public class ContexteNorthwind : DbContext
    {
        public ContexteNorthwind(DbContextOptions<ContexteNorthwind> options)
            : base(options)
        {
        }

        public virtual DbSet<Adresse> Adresses { get; set; }
        public virtual DbSet<Employe> Employes { get; set; }
        public virtual DbSet<Region> Regions { get; set; }
        public virtual DbSet<Territoire> Territoires { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Adresse>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Ville).HasMaxLength(40);
                entity.Property(e => e.Pays).HasMaxLength(40);
                entity.Property(e => e.Tel).HasMaxLength(20).IsUnicode(false);
                entity.Property(e => e.CodePostal).HasMaxLength(20).IsUnicode(false);
                entity.Property(e => e.Region).HasMaxLength(40);
                entity.Property(e => e.Rue).HasMaxLength(100);
            });

            modelBuilder.Entity<Employe>(entity =>
            {
                entity.HasKey(e => e.Id);
                // Relation de la table Employe sur elle-même 
                entity.HasOne<Employe>().WithMany().HasForeignKey(d => d.IdManager);

                // Relation Employe - Adresse de cardinalités 0,1 - 1,1
                entity.HasOne<Adresse>().WithOne().HasForeignKey<Employe>(d => d.IdAdresse)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.Property(e => e.Prenom).HasMaxLength(40);
                entity.Property(e => e.Nom).HasMaxLength(40);
                entity.Property(e => e.Notes).HasMaxLength(1000);
                entity.Property(e => e.Photo).HasColumnType("image");
                entity.Property(e => e.Fonction).HasMaxLength(40);
                entity.Property(e => e.Civilite).HasMaxLength(40);
            });

            modelBuilder.Entity<Affectation>(entity =>
            {
                entity.ToTable("Affectations");
                entity.HasKey(e => new { e.IdEmploye, e.IdTerritoire });
                //entity.Property(e => e.IdTerritoire).HasMaxLength(20).IsUnicode(false);
            });

            modelBuilder.Entity<Territoire>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne<Region>().WithMany(t => t.Territoires).HasForeignKey(f => f.IdRegion)
                        .OnDelete(DeleteBehavior.NoAction);

                // Crée la relation N-N avec Employe en utilisant l'entité Affectation comme entité d'association
                entity.HasMany<Employe>().WithMany().UsingEntity<Affectation>(
                    l => l.HasOne<Employe>().WithMany().HasForeignKey(a => a.IdEmploye),
                    r => r.HasOne<Territoire>().WithMany().HasForeignKey(a => a.IdTerritoire));

                entity.Property(e => e.Id).HasMaxLength(20).IsUnicode(false);
                entity.Property(e => e.Nom).HasMaxLength(40);
            });

            modelBuilder.Entity<Region>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Nom).HasMaxLength(40);
            });
        }
    }
}
