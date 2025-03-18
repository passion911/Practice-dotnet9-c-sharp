using Services.Contracts;

namespace Services.Implementations.Cars;

public class LuxuryCarFactory : ILuxuryCarFactory
{
    public async Task<ICar> CreateCar(string carType)
    {
        
        return carType.ToLower() switch
        {
            "lux-sedan" => new LuxurySedan(),
            "lux-suv" => new LuxurySUV(),
            _ => throw new ArgumentException("Invalid car type")
        };
    }

    public async Task<bool> HasPremiumFeatures()
    {
        return true; // Luxury cars have premium features like autopilot, leather seats, etc.
    }
}
