

using Infrastructure.DAL.Contracts;
using Infrastructure.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DAL.Repository;

public class ProductRepository : Repository<Product>, IProductRepository
{
    private readonly AppDbContext _appDbContext;
    public ProductRepository(AppDbContext appDbContext) : base(appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category)
    {
        return await _appDbContext.Products.Where(p => p.ProductCategory == category).ToListAsync();
    }
}
