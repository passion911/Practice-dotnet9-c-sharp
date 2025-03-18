using Services.Contracts;

namespace Services.Implementations.Cars;

public class EconomyHatchback : ICar
{
    public EconomyHatchback()
    {
        Console.WriteLine($"A economy hatchback was created!");
    }

    public async Task Drive()
    {
        Console.WriteLine("Driving a fuel-efficient Economy Hatchback.");
    }
}
