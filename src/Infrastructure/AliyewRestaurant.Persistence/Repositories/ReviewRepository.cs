using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AliyewRestaurant.Application.Abstracts.Repositories;
using AliyewRestaurant.Domain.Entites;
using AliyewRestaurant.Persistence.Contexts;

namespace AliyewRestaurant.Persistence.Repositories;

public class ReviewRepository : Repository<Review>, IReviewRepository
{
    private readonly AliyewRestaurantDbContext _context;

    public ReviewRepository(AliyewRestaurantDbContext context) : base(context)
    {
        _context = context;
    }
}
