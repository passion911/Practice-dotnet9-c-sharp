using Application.Services.OrderStuffs;
using Microsoft.AspNetCore.Mvc;

namespace Practice.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost("place-order")]
    public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderDto orderDto)
    {
        await _orderService.PlaceOrderAsync(orderDto);
        return Ok("Order placed successfully.");
    }
}
