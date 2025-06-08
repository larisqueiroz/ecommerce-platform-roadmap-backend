namespace CatalogService.Models.DAO
{
    public class Category: Base
    {
        public string Name { get; set; }
        public string CategoryDiscount { get; set; }
        public List<Item> Items { get; set; } = new List<Item>();
    }
}
