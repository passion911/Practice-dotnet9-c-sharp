using Services.Contracts;

namespace Services.Implementations.Cars;

public class LuxurySedan : ICar
{
    public LuxurySedan()
    {
        Console.WriteLine($"A luxury sedan was created!");
    }

    public async Task Drive()
    {
        Console.WriteLine("Driving a Luxury Sedan with premium features.");
    }
}
