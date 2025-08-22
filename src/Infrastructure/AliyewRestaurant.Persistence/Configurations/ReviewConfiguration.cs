using AliyewRestaurant.Domain.Entites;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AliyewRestaurant.Persistence.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("Reviews");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.CommentBody)
               .IsRequired()
               .HasMaxLength(1000);

        builder.Property(r => r.Rating)
               .IsRequired();

        builder.HasOne(r => r.MenuItem)
               .WithMany(m => m.Reviews)
               .HasForeignKey(r => r.MenuItemId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.User)
               .WithMany(u => u.Reviews)
               .HasForeignKey(r => r.UserId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
