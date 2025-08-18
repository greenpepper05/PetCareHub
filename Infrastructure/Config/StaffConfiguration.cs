using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config;

public class StaffConfiguration : IEntityTypeConfiguration<Staff>
{
    public void Configure(EntityTypeBuilder<Staff> builder)
    {
        builder.Property(s => s.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(s => s.LastName).IsRequired().HasMaxLength(100);
        builder.Property(s => s.Email).IsRequired().HasMaxLength(255);
        builder.HasIndex(s => s.Email).IsUnique();
        builder.Property(s => s.PhoneNumber).IsRequired().HasMaxLength(20);
        builder.Property(s => s.StaffRole).IsRequired().HasConversion<string>().HasMaxLength(50);
        builder.HasOne(s => s.Clinic).WithMany(c => c.StaffMembers).HasForeignKey(c => c.ClinicId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(s => s.ServiceRecord).WithOne(sr => sr.Staff).HasForeignKey(sr => sr.StaffId).OnDelete(DeleteBehavior.SetNull);
        builder.Property(s => s.UserId).HasMaxLength(450);
    }
}
