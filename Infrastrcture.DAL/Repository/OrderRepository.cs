

using Infrastructure.DAL.Contracts;
using Infrastructure.DAL.Entities;

namespace Infrastructure.DAL.Repository;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    private readonly AppDbContext _appDbContext;
    public OrderRepository(AppDbContext appDbContext) : base(appDbContext)
    {
        _appDbContext = appDbContext;
    }


}
