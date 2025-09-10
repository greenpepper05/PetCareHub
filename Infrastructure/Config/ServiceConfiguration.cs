using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config;

public class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.Property(s => s.Name).IsRequired().HasMaxLength(100);
        builder.Property(s => s.Description).HasMaxLength(500);
        builder.Property(s => s.Price).HasColumnType("decimal(18,2)");
        builder.HasOne(s => s.Clinic).WithMany(c => c.Services).HasForeignKey(s => s.ClinicId).OnDelete(DeleteBehavior.Restrict);
    }
}
