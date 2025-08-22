using AliyewRestaurant.Domain.Entites;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AliyewRestaurant.Persistence.Configurations;

public class MenuItemConfiguration : IEntityTypeConfiguration<MenuItem>
{
    public void Configure(EntityTypeBuilder<MenuItem> builder)
    {
        builder.ToTable("MenuItems");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(m => m.Description)
               .HasMaxLength(500);

        builder.Property(m => m.Price)
               .HasColumnType("decimal(18,2)");

        builder.HasOne(m => m.Category)
               .WithMany(c => c.MenuItems)
               .HasForeignKey(m => m.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(m => m.OrderItems)
               .WithOne(oi => oi.MenuItem)
               .HasForeignKey(oi => oi.MenuItemId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(m => m.Reviews)
               .WithOne(r => r.MenuItem)
               .HasForeignKey(r => r.MenuItemId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
