using Services.Contracts;
using Services.Implementations;

namespace Services.CarFactory;

public class CarFactory
{
    public async Task<ICar> CreateCar(string carType)
    {
        return carType.ToLower() switch
        {
            "sedan" => new Sedan(),
            "suv" => new SUV(),
            "sportscar" => new SportsCar(),
            _ => throw new ArgumentException("Invalid car type")
        };
    }
}
