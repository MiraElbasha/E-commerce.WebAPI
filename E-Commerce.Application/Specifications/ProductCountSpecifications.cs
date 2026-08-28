using E_Commerce.Application.Common;
using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Specifications
{
    public class ProductCountSpecifications: BaseSpecifications<Product , int>
    {
        public ProductCountSpecifications(ProductQueryParams QueryParams) : base(p => (!QueryParams.BrandId.HasValue || p.BrandId == QueryParams.BrandId.Value) && (!QueryParams.TypeId.HasValue || p.TypeId == QueryParams.TypeId.Value) && (string.IsNullOrEmpty(QueryParams.SearchValue) || p.Name.ToLower().Contains(QueryParams.SearchValue)))
        {
            
        }
    }
}
