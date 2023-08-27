namespace Catalog.Core.Models;

public class DetailedProductDto : ProductDto
{
    public DetailedProductDto(ProductDto productDto)
    {
        Id = productDto.Id;
        Name = productDto.Name;
        Description = productDto.Description;
        UpdatedAt = productDto.UpdatedAt;
        Image = productDto.Image;
        Body = string.Empty;
        Price = 0;
        Qty = 0;
        Gallery = new List<Image>();
    }
    public string Body { get; set; }
    public decimal Price { get; set; }
    public int Qty { get; set; }
    public List<Image> Gallery { get; set; }
}