using Services.Contracts;

namespace Services.Implementations.Cars;

public class EconomySedan : ICar
{
    public EconomySedan()
    {
        Console.WriteLine($"A economy sedan was created!");
    }

    public async Task Drive()
    {
        Console.WriteLine("Driving an Economy Sedan with low maintenance cost.");
    }
}
