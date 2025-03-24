namespace Infrastructure.DAL.Entities;

public class Product
{
    public int ProductId { get; set; }
    public required string ProductName { get; set; }
    public string? ProductCategory { get; set; }
    public int Stock { get; set; }
    public decimal Price { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; }
}
