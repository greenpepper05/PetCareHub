using System;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config;

public class ServiceRecordConfiguration : IEntityTypeConfiguration<ServiceRecord>
{
    public void Configure(EntityTypeBuilder<ServiceRecord> builder)
    {
        builder.Property(sr => sr.Note).HasMaxLength(500);
        builder.Property(sr => sr.Price).HasColumnType("decimal(18,2)");
        builder.Property(sr => sr.Status).HasMaxLength(50).IsRequired();
        builder.HasOne(sr => sr.Pet).WithMany(p => p.ServiceRecords).HasForeignKey(sr => sr.PetId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(sr => sr.Service).WithMany().HasForeignKey(sr => sr.ServiceId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(sr => sr.Clinic).WithMany(c => c.ServiceRecords).HasForeignKey(sr => sr.ClinicId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(sr => sr.Appointment).WithMany(a => a.ServiceRecords).HasForeignKey(sr => sr.AppointmentId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(sr => sr.Staff).WithMany(a => a.ServiceRecord).HasForeignKey(sr => sr.StaffId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(sr => sr.Steps).WithOne(step => step.ServiceRecord).HasForeignKey(step => step.ServiceRecordId).OnDelete(DeleteBehavior.Restrict);

    }
}
