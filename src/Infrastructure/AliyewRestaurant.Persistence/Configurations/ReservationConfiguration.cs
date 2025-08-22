using AliyewRestaurant.Domain.Entites;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AliyewRestaurant.Persistence.Configurations;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.ToTable("Reservations");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.ReservedAt).IsRequired();
        builder.Property(r => r.EndTime).IsRequired();
        builder.Property(r => r.NumberOfPeople).IsRequired();

        builder.Property(r => r.Notes)
               .HasMaxLength(500);

        builder.HasOne(r => r.Table)
               .WithMany(t => t.Reservations)
               .HasForeignKey(r => r.TableId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.User)
               .WithMany(u => u.Reservations)
               .HasForeignKey(r => r.UserId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}