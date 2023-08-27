using Catalog.Tests.Integration.Mocks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Catalog.Tests.Integration;

public class CatalogApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MockedStrapiApiServer _mockedStrapiApi = new ();
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        
        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
        });

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(IHostedService));
            services.AddHttpClient("StrapiCms", httpClient =>
            {
                httpClient.BaseAddress = new Uri(_mockedStrapiApi.Url);
            });
            
            var address = _mockedStrapiApi.Url;
        });
    }

    public async Task InitializeAsync()
    {
        _mockedStrapiApi.Start();
        await Task.Delay(100);
    }

    public async Task DisposeAsync()
    {
        _mockedStrapiApi.Dispose();
        await Task.Delay(100);
    }
}