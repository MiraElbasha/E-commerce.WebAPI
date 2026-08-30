using E_Commerce.API.Attributes;
using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductService productService) : APIBaseController
    {
        #region Get All Products
        [RedisCashe(100)]
        [HttpGet]
        [ProducesResponseType(typeof(ProductDTO) , StatusCodes.Status200OK)]
        public async Task<ActionResult<PaginatedResult<ProductDTO>>>GetAllProducts([FromQuery]ProductQueryParams QueryParams, CancellationToken ct = default)
        {
            var products = await productService.GetAllProductsAcync(QueryParams, ct);
            return ToActionResult(products);
        }
        #endregion

        #region Get Product By Id
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDTO>> GetProductById(int id, CancellationToken ct = default)
        {

            var product = await productService.GetProductByIdAsync(id, ct);
            return ToActionResult(product);
        }
        #endregion

        #region Get All Brands
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<BrandDTO>>> GetAllBrands(CancellationToken ct = default)
              => ToActionResult(await productService.GetAllBrandsAsync(ct));
        #endregion

        #region gettAll Types
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<TypeDTO>>> GetAllTypes(CancellationToken ct = default)
              => ToActionResult(await productService.GetAllTypesAsync(ct)); 
        #endregion
    }
}
