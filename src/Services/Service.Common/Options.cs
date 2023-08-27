namespace Service.Common;

public class OpenApiOptions
{
    public const string SectionName = "OpenApi";
    public required Endpoint Endpoint { get; set; }
    public required Document Document { get; set; }
    public required Auth Auth { get; set; }
}

public class Endpoint
{
    public required string Name { get; set; }
}

public class Document
{
    public required string Description { get; set; }
    public required string Title { get; set; }
    public required string Version { get; set; }
}

public class Auth
{
    public required string ClientId { get; set; }
    public required string AppName { get; set; }
}