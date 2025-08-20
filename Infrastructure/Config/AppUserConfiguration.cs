using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.Property(u => u.FirstName).HasMaxLength(100).IsRequired();
        builder.Property(u => u.LastName).HasMaxLength(100).IsRequired();
        builder.Property(u => u.Contact).HasMaxLength(20);
        builder.HasIndex(u => u.Contact).IsUnique(false);
        builder.HasIndex(u => u.ClinicId);

        builder.HasMany(u => u.Pets).WithOne(p => p.Owner).HasForeignKey(p => p.OwnerId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(u => u.Appointments).WithOne(a => a.Owner).HasForeignKey(a => a.OwnerId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(u => u.Clinic).WithMany(c => c.Staff).HasForeignKey(u => u.ClinicId).OnDelete(DeleteBehavior.Restrict);
    }
}
