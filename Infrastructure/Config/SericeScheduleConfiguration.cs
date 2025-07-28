using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config;

public class SericeScheduleConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.HasMany(s => s.Schedules).WithOne(ss => ss.Service!).HasForeignKey(ss => ss.ServiceId);
    }
}
