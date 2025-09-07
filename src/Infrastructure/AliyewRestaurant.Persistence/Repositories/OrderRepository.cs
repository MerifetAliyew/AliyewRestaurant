using AliyewRestaurant.Application.Abstracts.Repositories;
using AliyewRestaurant.Domain.Entites;
using AliyewRestaurant.Persistence.Contexts;

namespace AliyewRestaurant.Persistence.Repositories;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    private readonly AliyewRestaurantDbContext _context;
    public OrderRepository(AliyewRestaurantDbContext context) : base(context)
    {
        _context = context;
    }
}


