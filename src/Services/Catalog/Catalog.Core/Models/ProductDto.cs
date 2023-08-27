namespace Catalog.Core.Models;


public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    public Image? Image { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
