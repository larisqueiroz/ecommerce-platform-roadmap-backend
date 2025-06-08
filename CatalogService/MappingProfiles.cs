using AutoMapper;
using CatalogService.Models.DAO;
using CatalogService.Models.DTO;

namespace CatalogService
{
    public class MappingProfiles: Profile
    {
        public MappingProfiles()
        {
            CreateMap<Item, ItemDto>();
            CreateMap<ItemDto, Item>();

            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();
        }
    }
}
