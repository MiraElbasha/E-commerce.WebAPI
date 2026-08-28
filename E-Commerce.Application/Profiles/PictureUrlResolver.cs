using AutoMapper;
using E_Commerce.Application.DTOs.Products;
using E_Commerce.Domain.Entities.Products;
using Microsoft.Extensions.Options;

namespace E_Commerce.Application.Profiles
{
    internal class PictureUrlResolver(IOptions<UrlSettings> options) : IValueResolver<Product, ProductDTO, string?>
    {
        private readonly UrlSettings _urlSettings = options.Value;

        public string? Resolve(Product source, ProductDTO destination, string? destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.PictureUrl))
            {
                return null;
            }
            var baseUrl = _urlSettings.BaseUrl.TrimEnd('/');
            var path = source.PictureUrl.TrimStart('/');
            return $"{baseUrl}/Files/{path}";
        }

       
    }

    public class UrlSettings 
    {
        public string BaseUrl { get; set; } = string.Empty;
    }
}
