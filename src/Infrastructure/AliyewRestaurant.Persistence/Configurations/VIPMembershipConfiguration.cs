using AliyewRestaurant.Domain.Entites;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AliyewRestaurant.Persistence.Configurations;

public class VIPMembershipConfiguration : IEntityTypeConfiguration<VIPMembership>
{
    public void Configure(EntityTypeBuilder<VIPMembership> builder)
    {
        builder.ToTable("VIPMemberships");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.TotalSpent)
               .HasColumnType("decimal(18,2)");

        builder.HasOne(v => v.User)
               .WithOne(u => u.VIPMembership)
               .HasForeignKey<VIPMembership>(v => v.UserId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
