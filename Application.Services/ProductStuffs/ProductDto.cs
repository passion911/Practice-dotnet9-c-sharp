

using System.ComponentModel;

namespace Application.Services.ProductStuffs;

public class ProductDto
{
    public int ProductId { get; set; }

    [DisplayName("Name of product")]
    public required string ProductName { get; set; }
    public string? ProductCategory { get; set; }
}
