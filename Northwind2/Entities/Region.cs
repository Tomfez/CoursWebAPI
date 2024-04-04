namespace Northwind2.Entities
{
    public class Region
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        // Propriété de navigation. Ici on récupère la liste des territoires reliés à une région.
        public virtual List<Territoire> Territoires { get; set; } = new();
    }
}
