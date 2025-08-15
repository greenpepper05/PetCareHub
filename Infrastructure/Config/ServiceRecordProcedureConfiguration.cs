using Core.Entities;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config;

public class ServiceRecordProcedureConfiguration : IEntityTypeConfiguration<ServiceRecordProcedures>
{
    public void Configure(EntityTypeBuilder<ServiceRecordProcedures> builder)
    {
        builder.Property(x => x.DateOfService).IsRequired();
        builder.Property(x => x.Notes).HasMaxLength(1000);
        builder.Property(x => x.VisitType).HasConversion<string>().IsRequired();
        builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
        builder.Property(x => x.Status).HasMaxLength(50).HasDefaultValue(ServiceStatus.Scheduled);
        builder.HasOne(x => x.Pet).WithMany().HasForeignKey(x => x.PetId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Service).WithMany().HasForeignKey(x => x.ServiceId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Appointment).WithMany().HasForeignKey(x => x.AppointmentId).OnDelete(DeleteBehavior.SetNull);
        builder.HasOne(x => x.Clinic).WithMany().HasForeignKey(x => x.ClinicId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Staff).WithMany().HasForeignKey(x => x.StaffId).OnDelete(DeleteBehavior.SetNull);
        builder.HasMany(x => x.ProcedureSteps).WithOne(x => x.ServiceRecordProcedures).HasForeignKey(x => x.ServiceProcedureId).OnDelete(DeleteBehavior.Cascade);
    }
}
