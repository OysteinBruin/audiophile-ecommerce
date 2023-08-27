using Catalog.Core.Interfaces;
using Catalog.Core.Constants;
using Catalog.Core.Models;
using Catalog.Core.Result;
using Catalog.Infrastructure.Mappings;
using GraphQL;
using GraphQL.Client.Abstractions;
using Mapster;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Catalog.Infrastructure.Services;

public class StrapiCmsService : IStrapiCmsService
{
    //private readonly IGraphQLClient _graphQlClient;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly StrapiCmsOptions? _strapiCmsOptions;

    public StrapiCmsService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _strapiCmsOptions = configuration.GetSection(StrapiCmsOptions.SectionName).Get<StrapiCmsOptions>();
        
        if (_strapiCmsOptions is null)
        {
            throw new ArgumentNullException(nameof(_strapiCmsOptions), $"Configuration section {nameof(_strapiCmsOptions)} is missing.");
        }
    }

    public async Task<Result<DetailedProductDto>> GetProductByIdAsync(int id)
    {
        try
        {
            string requestUriEndpoint =
                $"{StrapiApiConstants.ApiRequest.ProductEndpointBaseUrl}{id}{StrapiApiConstants.ApiRequest.GetProductsDetailQuery}";
            var jsonObject = await GetJsonObjectFromResponse(requestUriEndpoint);
            var result = jsonObject.Adapt<Result<DetailedProductDto>>(MapConfig.SingleProductConfig());
            return result;
        }
        catch (Exception e)
        {
            return Result<DetailedProductDto>.Failure("Failed to retrieve product.");
        }
    }

    public async Task<PaginatedResult<ProductDto>> GetAllProductsAsync(string itemCategory, int pageSize = 10, int pageIndex = 0)
    {
        // ToDo: implement pagination

        try
        {
            string requestUriEndpoint =
                $"{StrapiApiConstants.ApiRequest.ProductEndpointBaseUrl}{StrapiApiConstants.ApiRequest.GetProductsListQuery}{itemCategory}";
            var jsonObject = await GetJsonObjectFromResponse(requestUriEndpoint);
            var result = jsonObject.Adapt<PaginatedResult<ProductDto>>(MapConfig.AllProductsPagedConfig());
            return result;
        }
        catch (Exception e)
        {
            return PaginatedResult<ProductDto>.Failure("Failed to retrieve products.");
        }
    }
    
    private async Task<JObject> GetJsonObjectFromResponse(string requestUriEndpoint)
    {
        var httpClient = _httpClientFactory.CreateClient(_strapiCmsOptions!.HttpClientName);
        var baseAddress = httpClient.BaseAddress;
        var requestUri = $"{httpClient.BaseAddress}{requestUriEndpoint}";
        var httpResponse = await httpClient.GetAsync(requestUri);

        var responseAsString = await httpResponse.Content.ReadAsStringAsync();
        var jsonObject = JObject.Parse(responseAsString);
        return jsonObject;
    }
}