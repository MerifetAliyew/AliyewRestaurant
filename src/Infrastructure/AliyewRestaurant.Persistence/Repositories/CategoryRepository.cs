using AliyewRestaurant.Application.Abstracts.Repositories;
using AliyewRestaurant.Domain.Entites;
using AliyewRestaurant.Persistence.Contexts;

namespace AliyewRestaurant.Persistence.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    private readonly AliyewRestaurantDbContext _context;

    public CategoryRepository(AliyewRestaurantDbContext context) : base(context)
    {
        _context = context;
    }
}
