using Domain.Models;

namespace Services.Contracts;

public interface IPaymentService
{
    public Task ProcessPaymentAsync(OrderModel orderorder);
}
