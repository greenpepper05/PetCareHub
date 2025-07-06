using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config;

public class AppointmentServicesConfiguration : IEntityTypeConfiguration<AppointmentService>
{
    public void Configure(EntityTypeBuilder<AppointmentService> builder)
    {
        builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
    }
}
