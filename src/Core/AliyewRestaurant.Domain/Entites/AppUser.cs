using Microsoft.AspNetCore.Identity;

namespace AliyewRestaurant.Domain.Entites;


public class AppUser : IdentityUser
{
    public string FullName { get; set; }
    public string? RefreshToken { get; set; } = null!;
    public DateTime? ExpiryDate { get; set; }
    public ICollection<Reservation> Reservations { get; set; }
    public VIPMembership VIPMembership { get; set; }
    public ICollection<Order> Orders { get; set; }
    public ICollection<Review> Reviews { get; set; }
}
