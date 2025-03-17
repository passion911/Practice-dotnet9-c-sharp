using Domain.Models;

namespace Services.Contracts;

public interface ICheckoutService
{
    public Task CheckoutAsync(OrderModel order, IPaymentService paymentMethod);
}
