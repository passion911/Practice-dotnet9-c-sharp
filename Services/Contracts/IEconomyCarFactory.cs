namespace Services.Contracts;

public interface IEconomyCarFactory : ICarFactory
{
    public Task<bool> IsFuelEfficient();
}
