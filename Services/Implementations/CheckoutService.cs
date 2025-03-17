using Domain.Models;
using Services.Contracts;

namespace Services.Implementations;

public class CheckoutService : ICheckoutService
{
    public async Task CheckoutAsync(OrderModel order, IPaymentService paymentMethod)
    {
        await paymentMethod.ProcessPaymentAsync(order);
    }
}
