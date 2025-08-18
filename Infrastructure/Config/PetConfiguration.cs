using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config;

public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Breed).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Species).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Birthdate).IsRequired();
        builder.Property(p => p.Gender).IsRequired().HasMaxLength(10);
        builder.Property(p => p.OwnerId).IsRequired();
        builder.HasOne(p => p.Owner).WithMany(u => u.Pets).HasForeignKey(p => p.OwnerId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(p => p.Appointments).WithOne(a => a.Pet).HasForeignKey(a => a.PetId).OnDelete(DeleteBehavior.Cascade);
    }
}
