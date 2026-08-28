using AutoMapper;
using E_Commerce.Application.DTOs.Products;
using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Profiles
{
    internal class ProductProfile :Profile
    {
        public ProductProfile()
        {
            //trasnfer common properties values to the DTO
            CreateMap<ProductBrand, BrandDTO>();
            CreateMap<ProductType, TypeDTO>();
            CreateMap<Product, ProductDTO>()
                //dst(destnation) = DTO , src(source) = entity
                //get the ProductBrand property value in DTO from Source ProductBrand Entity -> name
                .ForMember(dst => dst.ProductBrand , opt => opt.MapFrom(src => src.ProductBrand.Name))
                //get the ProductType Property value in DTO from Source ProductType Entity -> name
                .ForMember(dst => dst.ProductType , opt => opt.MapFrom(src => src.ProductType.Name))
                .ForMember(d => d.PictureUrl , o =>  o.MapFrom<PictureUrlResolver>());
        }
    }
}
