using AliyewRestaurant.Domain.Entites;
using AliyewRestaurant.Persistence.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace AliyewRestaurant.Persistence.Contexts;

public class AliyewRestaurantDbContext : IdentityDbContext<AppUser>
{
    public AliyewRestaurantDbContext(DbContextOptions<AliyewRestaurantDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CategoryConfiguration).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<MenuItem> MenuItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Table> Tables { get; set; }
    public DbSet<VIPMembership> VIPMemberships { get; set; }
}

