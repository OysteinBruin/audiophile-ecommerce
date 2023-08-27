namespace Catalog.Core.Models;

public class Image
{
    public int Width { get; set; }
    public int Height { get; set; }
    public string Url { get; set; }
}

public enum ImageFormat
{
    Thumbnail,
    Small,
    Medium,
    Large,
    Default
}