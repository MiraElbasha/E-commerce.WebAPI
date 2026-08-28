using E_Commerce.Application.Common;
using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Specifications
{
    public class ProductWithBrandAndTypeSpecifications : BaseSpecifications<Product , int>
    {
        //Get All
        public ProductWithBrandAndTypeSpecifications(ProductQueryParams QueryParams) : base(p => (!QueryParams.BrandId.HasValue || p.BrandId == QueryParams.BrandId.Value) && (!QueryParams.TypeId.HasValue || p.TypeId == QueryParams.TypeId.Value) && (string.IsNullOrEmpty(QueryParams.SearchValue) || p.Name.ToLower().Contains(QueryParams.SearchValue)) )
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
            switch (QueryParams.Sort)
            {
                case ProductSortingOptions
                    .NameAsc: AddOrderBy(P => P.Name);
                    break;
                case ProductSortingOptions
                    .NameDesc: AddOrderByDesc(P => P.Name);
                    break;
                case ProductSortingOptions
                    .PriceAsc: AddOrderBy(P => P.Price);
                    break;
                case ProductSortingOptions
                    .PricerDesc: AddOrderBy(P => P.Price);
                    break;
                default:
                    AddOrderBy(P => P.Id);
                    break;
            }
            ApplyPagination(QueryParams.PageSize ,  QueryParams.PageIndex);
        }
        //Get By Id
        public ProductWithBrandAndTypeSpecifications(int id ) : base(x => x.Id == id)
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
        }
    }
}
