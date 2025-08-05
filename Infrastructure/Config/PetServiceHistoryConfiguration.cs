using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config;

public class PetServiceHistoryConfiguration : IEntityTypeConfiguration<PetServiceHistory>
{
    public void Configure(EntityTypeBuilder<PetServiceHistory> builder)
    {
        builder.HasOne(p => p.Service).WithMany().HasForeignKey(p => p.ServiceId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(p => p.Pet).WithMany().HasForeignKey(p => p.PetId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(p => p.Clinic).WithMany().HasForeignKey(p => p.ClinicId).OnDelete(DeleteBehavior.Restrict);
    }
}
