using CatalogService.Models.DAO;

namespace CatalogService.Models.DTO
{
    public class CategoryDto: BaseDto
    {
        public string Name { get; set; }
        public string CategoryDiscount { get; set; }
        public List<Item> Items { get; set; } = new List<Item>();
    }
}
