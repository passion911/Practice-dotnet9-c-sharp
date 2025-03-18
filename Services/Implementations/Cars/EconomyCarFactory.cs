using Services.Contracts;

namespace Services.Implementations.Cars;

public class EconomyCarFactory : IEconomyCarFactory
{
    public async Task<ICar> CreateCar(string carType)
    {
        return carType.ToLower() switch
        {
            "eco-sedan" => new EconomySedan(),
            "eco-hatchback" => new EconomyHatchback(),
            _ => throw new ArgumentException("Invalid car type")
        };
    }

    public async Task<bool> IsFuelEfficient()
    {
        //assumption true to simplifies process
        return true;// Economy cars prioritize fuel efficiency
    }
}
