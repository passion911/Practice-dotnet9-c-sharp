using Services.Contracts;

namespace Services.Implementations;

public class SUV : ICar
{
    public async Task Drive()
    {
        Console.WriteLine("Driving a SUV.");
    }
}
