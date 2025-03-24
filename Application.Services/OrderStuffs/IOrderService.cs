

namespace Application.Services.OrderStuffs;

public interface IOrderService
{
    Task PlaceOrderAsync(PlaceOrderDto orderDto);
}
