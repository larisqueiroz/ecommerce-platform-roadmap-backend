namespace CatalogService.Models.DTO
{
    public class ItemDto: BaseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string ImageUrl { get; set; }
        public string CategoryId { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string Brand { get; set; }
        public double? Discount { get; set; }
    }
}
