namespace Catalog.Infrastructure;

public class StrapiCmsOptions
{
    public const string SectionName = "StrapiCms";
    public string HttpClientName { get; set; }
    public string HttpClientBaseUrl { get; set; }
    
    public string GraphQlBaseUrl { get; set; }
}