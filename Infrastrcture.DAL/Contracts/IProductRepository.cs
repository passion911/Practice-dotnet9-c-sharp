

using Infrastructure.DAL.Entities;

namespace Infrastructure.DAL.Contracts;

public interface IProductRepository : IRepository<Product>
{
    Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category);
}
