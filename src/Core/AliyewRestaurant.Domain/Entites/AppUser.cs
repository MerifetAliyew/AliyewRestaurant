using Microsoft.AspNetCore.Identity;

namespace AliyewRestaurant.Domain.Entites;


public class AppUser : IdentityUser
{
    public string FullName { get; set; }

    // Navigation properties
    public ICollection<Reservation> Reservations { get; set; }
    public VIPMembership VIPMembership { get; set; }
    public ICollection<Order> Orders { get; set; }
    public ICollection<Review> Reviews { get; set; }
}
