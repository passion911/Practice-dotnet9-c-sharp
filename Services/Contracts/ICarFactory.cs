namespace Services.Contracts;

public interface ICarFactory
{
    public Task<ICar> CreateCar(string carType);
}
