using Services.Contracts;

namespace Services.Implementations.Cars;

public class LuxurySUV : ICar
{
    public LuxurySUV()
    {
        Console.WriteLine($"A luxury SUV was created!");
    }

    public async Task Drive()
    {
        Console.WriteLine("Driving a Luxury SUV with leather seats and autopilot.");
    }
}
