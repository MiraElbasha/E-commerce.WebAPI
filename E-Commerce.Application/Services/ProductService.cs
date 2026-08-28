using AutoMapper;
using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Products;
using E_Commerce.Application.Specifications;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services
{
    internal class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IReadOnlyList<BrandDTO>>> GetAllBrandsAsync(CancellationToken ct = default)
        {
            var Brands = await _unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync(ct);
            return Result<IReadOnlyList<BrandDTO>>.Ok(_mapper.Map<IReadOnlyList<BrandDTO>>(Brands));
        }

        public async Task<Result<PaginatedResult<ProductDTO>>> GetAllProductsAcync(ProductQueryParams QueryParams, CancellationToken ct = default)
        {
            var Spec = new ProductWithBrandAndTypeSpecifications(QueryParams);
            var Repo = _unitOfWork.GetRepository<Product, int>();
            var Products = await Repo.GetAllAsync(Spec ,ct);
            var Data = _mapper.Map<IReadOnlyList<ProductDTO>>(Products);
            var CountSpec = new ProductCountSpecifications(QueryParams);
            var CountOfAllProducts = await _unitOfWork.GetRepository<Product , int>().CountAsync(CountSpec);
            var Result = new PaginatedResult<ProductDTO>(QueryParams.PageIndex, QueryParams.PageSize , CountOfAllProducts , Data);
            return Result<PaginatedResult<ProductDTO>>.Ok(Result);
        }

        public async Task<Result<IReadOnlyList<TypeDTO>>> GetAllTypesAsync(CancellationToken ct = default)
        {
            var Types = await _unitOfWork.GetRepository<ProductType,int>().GetAllAsync(ct);
            return Result<IReadOnlyList<TypeDTO>>.Ok(_mapper.Map<IReadOnlyList<TypeDTO>>(Types));
        }

        public async Task<Result<ProductDTO>> GetProductByIdAsync(int id, CancellationToken ct = default)
        {
            var Spec = new ProductWithBrandAndTypeSpecifications(id);
            var Product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(Spec , ct);
            if (Product is null)
            {
                return Result<ProductDTO>.Fail(Error.NotFound($"Product Not Found , Product With ID {id} Was Not Found"));
            }
            return _mapper.Map<ProductDTO>(Product);
        }
    }
}
