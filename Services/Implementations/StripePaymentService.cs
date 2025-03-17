using Domain.Models;
using Services.Contracts;

namespace Services.Implementations;

public class StripePaymentService : IPaymentService
{
    public async Task ProcessPaymentAsync(OrderModel order)
    {
        //Implement logic here
    }
}
