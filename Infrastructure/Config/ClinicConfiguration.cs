using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config;

public class ClinicConfiguration : IEntityTypeConfiguration<Clinic>
{
    public void Configure(EntityTypeBuilder<Clinic> builder)
    {
        builder.Property(c => c.ClinicName).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Address).IsRequired().HasMaxLength(200);
        builder.Property(c => c.PhoneNumber).HasMaxLength(20);
        builder.Property(c => c.Email).HasMaxLength(100);
        builder.Property(c => c.CreateAt).HasDefaultValueSql("GETUTCDATE()");
        builder.Property(c => c.UpdatedAt).HasDefaultValueSql("GETUTCDATE()").ValueGeneratedOnAddOrUpdate();
        builder.HasMany(c => c.Services).WithOne(s => s.Clinic).HasForeignKey(s => s.ClinicId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(c => c.StaffMembers).WithOne(s => s.Clinic).HasForeignKey(s => s.ClinicId).OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(c => c.ClinicName).IsUnique(true);
        builder.HasIndex(c => c.OwnerId);
        builder.HasOne<AppUser>().WithMany().HasForeignKey(c => c.OwnerId).OnDelete(DeleteBehavior.SetNull);
    }
}
