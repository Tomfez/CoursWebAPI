namespace Northwind2.Entities
{
    public class Employe
    {
        public int Id { get; set; }
        public Guid IdAdresse { get; set; }
        public int? IdManager { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public string? Fonction { get; set; }
        public string? Civilite { get; set; }
        public DateOnly? DateNaissance { get; set; }
        public DateOnly? DateEmbauche { get; set; }
        public byte[]? Photo { get; set; }
        public string? Notes { get; set; }
        public virtual Adresse Adresse { get; set; } = null!;
    }
}
