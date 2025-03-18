namespace Services.Contracts;

public interface ILuxuryCarFactory : ICarFactory
{
    public Task<bool> HasPremiumFeatures();
}
