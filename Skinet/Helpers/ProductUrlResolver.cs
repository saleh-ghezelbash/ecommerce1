using AutoMapper;
using Core.Entities;
using Microsoft.Extensions.Configuration;
using Skinet.Dtos;

namespace Skinet.Helpers
{
    public class ProductUrlResolver : IValueResolver<Product, ProductToReturn, string>
    {
        private readonly IConfiguration _configuration;

        public ProductUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(Product source, ProductToReturn destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
                return _configuration["ApiUrl"] + source.PictureUrl;
            }
            return null;
        }
    }
}
