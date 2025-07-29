using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.HasOne(a => a.Pet).WithMany().HasForeignKey(a => a.PetId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(a => a.Owner).WithMany().HasForeignKey(a => a.OwnerId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(a => a.Service).WithMany().HasForeignKey(a => a.ServiceId).OnDelete(DeleteBehavior.Restrict);
    }
}
