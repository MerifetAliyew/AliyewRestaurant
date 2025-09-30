using AliyewRestaurant.Application.Abstracts.Repositories;
using AliyewRestaurant.Domain.Entites;
using AliyewRestaurant.Persistence.Contexts;

namespace AliyewRestaurant.Persistence.Repositories;

public class ReservationRepository : Repository<Reservation>, IReservationRepository
{
    private readonly AliyewRestaurantDbContext _context;

    public ReservationRepository(AliyewRestaurantDbContext context) : base(context)
    {
        _context = context;
    }
}
