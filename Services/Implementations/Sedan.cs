using Services.Contracts;

namespace Services.Implementations;

public class Sedan : ICar
{
    public async Task Drive()
    {
        Console.WriteLine("Driving a Sedan.");
    }
}
