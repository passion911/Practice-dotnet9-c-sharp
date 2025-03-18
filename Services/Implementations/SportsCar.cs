using Services.Contracts;

namespace Services.Implementations;

public class SportsCar : ICar
{
    public async Task Drive()
    {
        Console.WriteLine("Driving a Sports car.");
    }
}
