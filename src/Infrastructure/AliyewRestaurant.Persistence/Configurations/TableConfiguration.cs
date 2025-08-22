using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using AliyewRestaurant.Domain.Entites;

namespace AliyewRestaurant.Persistence.Configurations;

public class TableConfiguration : IEntityTypeConfiguration<Table>
{
    public void Configure(EntityTypeBuilder<Table> builder)
    {
        builder.ToTable("Tables");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.TableNumber)
               .IsRequired();

        builder.Property(t => t.Seats)
               .IsRequired();
    }
}
