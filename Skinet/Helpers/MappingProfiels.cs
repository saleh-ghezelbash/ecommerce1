using AutoMapper;
using Core.Entities;
using Skinet.Dtos;

namespace Skinet.Helpers
{
    public class MappingProfiels: Profile
    {
        public MappingProfiels()
        {
            CreateMap<Product, ProductToReturn>()
                .ForMember(x => x.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
                .ForMember(x => x.ProductType, o => o.MapFrom(s => s.ProductType.Name))
                .ForMember(x => x.PictureUrl, o => o.MapFrom<ProductUrlResolver>());
        }
    }
}
