using System.ComponentModel.DataAnnotations;

namespace CatalogService.Models.DAO
{
    public class Base
    {
        public Guid Id { get; set; } = new Guid();
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; }
        public bool Active { get; set; } = true;
    }
}
