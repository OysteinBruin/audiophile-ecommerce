using Catalog.Core.Interfaces;
using Catalog.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var strapiCmsOptions = configuration.GetSection(StrapiCmsOptions.SectionName).Get<StrapiCmsOptions>();
        
        if (strapiCmsOptions is null)
        {
            throw new ArgumentNullException(nameof(strapiCmsOptions), $"Configuration section {nameof(strapiCmsOptions)} is missing.");
        }

        services
            .AddScoped<IStrapiCmsService, StrapiCmsService>()
            //.AddScoped<IGraphQLClient>(s => new GraphQLHttpClient(strapiCmsOptions.GraphQlBaseUrl, new SystemTextJsonSerializer()))
            .AddHttpClient(strapiCmsOptions.HttpClientName, httpClient =>
            {
                httpClient.BaseAddress = new Uri(strapiCmsOptions.HttpClientBaseUrl);
            });
        
        return services;
    }

}