namespace Northwind2.Entities
{
    public class Adresse
    {
        public Guid Id { get; set; }
        public string Rue { get; set; } = string.Empty;
        public string Ville { get; set; } = string.Empty;
        public string CodePostal { get; set; } = string.Empty;
        public string Pays { get; set; } = string.Empty;
        public string? Region { get; set; }
        public string? Tel { get; set; }
    }
}
