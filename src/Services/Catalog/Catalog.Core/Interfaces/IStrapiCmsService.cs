using Catalog.Core.Models;
using Catalog.Core.Result;

namespace Catalog.Core.Interfaces;

public interface IStrapiCmsService
{
    Task<Result<DetailedProductDto>> GetProductByIdAsync(int id);
    Task<PaginatedResult<ProductDto>> GetAllProductsAsync(string itemCategory, int pageSize = 10, int pageIndex = 0);
}