using Catalog.Core.Interfaces;
using Catalog.Core.Constants;
using Catalog.Core.Models;
using Catalog.Core.Result;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CatalogController : ControllerBase
{
    private readonly ILogger<CatalogController> _logger;
    private readonly IStrapiCmsService _strapiCmsService;

    public CatalogController(ILogger<CatalogController> logger, IStrapiCmsService strapiCmsService)
    {
        _logger = logger;
        _strapiCmsService = strapiCmsService;
    }

    [HttpGet]
    [Route("items/{id:int}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductByIdAsync(int id)
    {
        if (id <= 0)
            return BadRequest();
        
        var response = await _strapiCmsService.GetProductByIdAsync(id);
        await Task.Delay(1000);
        return response.Succeeded
            ? Ok(response)
            : NotFound(response);
    }
    
    [HttpGet]
    [Route("items")]
    [ProducesResponseType(typeof(PaginatedResult<ProductDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProductsAsync([FromQuery] string itemCategory ,[FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
    {
        if (StrapiApiConstants.ApiRequest.ProductCategoryTypes.All(x => x != itemCategory))
            return BadRequest($"\"{itemCategory}\" is not a supported category. Supported categories are: {string.Join(", ", StrapiApiConstants.ApiRequest.ProductCategoryTypes)}");

        var response = await _strapiCmsService.GetAllProductsAsync(itemCategory, pageSize, pageIndex);
        await Task.Delay(1000);
        return response.Succeeded
            ? Ok(response)
            : StatusCode(500);
    }
}