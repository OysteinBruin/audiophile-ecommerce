using System.Net;
using Catalog.Tests.Integration;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace Catalog.API.Tests.Integration.CatalogController;

public class GetAllItemsPaginatedTests : IClassFixture<CatalogApplicationFactory>
{
    private readonly HttpClient _httpClient;

    public GetAllItemsPaginatedTests(CatalogApplicationFactory catalogFactory)
    {
        _httpClient = catalogFactory.CreateClient();
    }
    
    [Theory]
    [InlineData("speakers")]
    [InlineData("headphones")]
    [InlineData("")]
    public async Task GetAllItemsPaginated_ReturnsBadRequest_WhenQueryParamIsInvalid(string category)
    {
        // Arrange

        var request = new HttpRequestMessage(HttpMethod.Get, $"/Catalog/items?itemCategory={category}");
        
        // Act
        var response = await _httpClient.SendAsync(request);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Theory]
    [InlineData("speaker")]
    [InlineData("headphone")]
    [InlineData("earphone")]
    public async Task GetAllItemsPaginated_ReturnsOk_WhenQueryParamIsValid(string category)
    {
        // Arrange

        var request = new HttpRequestMessage(HttpMethod.Get, $"/Catalog/items?itemCategory={category}");
        
        // Act
        var response = await _httpClient.SendAsync(request);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
}