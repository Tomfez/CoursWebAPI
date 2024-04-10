namespace Northwind2.Entities
{
    public class Region
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public virtual List<Territoire> Territoires { get; set; } = new();
    }
}
