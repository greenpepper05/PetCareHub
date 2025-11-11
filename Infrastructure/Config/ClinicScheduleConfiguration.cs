using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config;

public class ClinicScheduleConfiguration : IEntityTypeConfiguration<ClinicSchedule>
{
    public void Configure(EntityTypeBuilder<ClinicSchedule> builder)
    {
        builder.Property(s => s.DayOfWeek).IsRequired();
        builder.Property(s => s.OpeningTime).IsRequired();
        builder.Property(s => s.ClosingTime).IsRequired();
        builder.Property(s => s.IsOpen).HasDefaultValue(true);
        builder.HasOne(s => s.Clinic).WithMany(c => c.Schedules).HasForeignKey(s => s.ClinicId).OnDelete(DeleteBehavior.Cascade);
    }
}
