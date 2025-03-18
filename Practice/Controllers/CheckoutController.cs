using Application.Services.Email;
using Domain.Models;
using Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Practice.Filters;
using Services.Contracts;
using Services.Implementations;
using Services.Implementations.Cars;

namespace Practice.Controllers;

[ApiController]
[Route("[controller]")]
[ServiceFilter(typeof(ApiCustomFilter))]
public class CheckoutController : ControllerBase
{
    private readonly ICheckoutService _checkoutSer;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _environment;
    public static string ApplicationAreaHeader = "APPLICATION_AREA";

    public CheckoutController(
        IWebHostEnvironment environment,
        ICheckoutService checkoutSer,
        IHttpContextAccessor httpContextAccessor
        )
    {
        _environment = environment;
        _httpContextAccessor = httpContextAccessor;
        _checkoutSer = checkoutSer;
    }

    [HttpPost(Name = "Checkout")]
    public async Task<IActionResult> Checkout([FromBody] OrderViewModel order, [FromServices] IEmailService emailSer)
    {
        var appArea = GetParticularDataExample();

        var convertedOrder = new OrderModel()
        {
            OrderId = order.OrderId,
            OrderDetail = order.OrderDetail
        };

        var paymentMethod = new StripePaymentService();

        await _checkoutSer.CheckoutAsync(convertedOrder, paymentMethod);

        await emailSer.SendMailAsync();

        //assumption that process successfully!
        return Ok(new
        {
            Message = "Checkout successful"
        });
    }

    [HttpGet(Name = "Checkout")]
    public async Task<IActionResult> Checkout(string factoryType = "eco", string carType = "eco-sedan")
    {
        ICarFactory carFactory = factoryType.ToLower() switch
        {
            "lux" => new LuxuryCarFactory(),
            "eco" => new EconomyCarFactory(),
            _ => throw new ArgumentException("Invalid factory type")
        };

        ICar car = await carFactory.CreateCar(carType);
        await car.Drive();

        ICar nextCarType = await carFactory.CreateCar("eco-hatchback");
        await nextCarType.Drive();

        if (carFactory is IEconomyCarFactory economyFactory)
        {
            Console.WriteLine($"Is fuel-efficient? {await economyFactory.IsFuelEfficient()}");
        }

        //Continue creating luxury car
        factoryType = "lux";
        carType = "lux-suv";
        ICarFactory nextCarFactory = factoryType.ToLower() switch
        {
            "lux" => new LuxuryCarFactory(),
            "eco" => new EconomyCarFactory(),
            _ => throw new ArgumentException("Invalid factory type")
        };

        ICar nextCar = await nextCarFactory.CreateCar(carType);
     
        if (nextCarFactory is ILuxuryCarFactory luxuryCarFactory)
        {
            Console.WriteLine($"Has premium features? {await luxuryCarFactory.HasPremiumFeatures()}");
        }

        return Ok("Produced necessary cars!");
    }

    private string GetParticularDataExample()
    {
        if ( _httpContextAccessor?.HttpContext == null )
        {
            throw new UnauthorizedAccessException();
        }
        var test = _environment.EnvironmentName;
        _httpContextAccessor.HttpContext.Request.Headers.TryGetValue(ApplicationAreaHeader.ToLower(), out StringValues applicationArea);

        string searchString = ".";
        int index = applicationArea.ToString().IndexOf(searchString);

        if ( index >= 0 )
        {
            string result = applicationArea.ToString().Remove(index, searchString.Length);
            return result;
        }

        return applicationArea!;
    }
}
