

namespace Infrastructure.DAL.Entities;

public class Order
{
    public int OrderId { get; set; }

    public DateTime OrderDate { get; set; }

    public ICollection<OrderDetail> OrderDetails { get; set; }
}
