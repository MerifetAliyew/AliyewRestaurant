using AliyewRestaurant.Application.Abstracts.Repositories;
using AliyewRestaurant.Domain.Entites;
using AliyewRestaurant.Persistence.Contexts;
namespace AliyewRestaurant.Persistence.Repositories;

public class MenuItemRepository : Repository<MenuItem>, IMenuItemRepository
{
    private readonly AliyewRestaurantDbContext _context;
    public MenuItemRepository(AliyewRestaurantDbContext context) : base(context)
    {
        _context = context;
    }
}