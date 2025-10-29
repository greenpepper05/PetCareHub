using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config;

public class StepConfiguration : IEntityTypeConfiguration<ServiceRecordStep>
{
    public void Configure(EntityTypeBuilder<ServiceRecordStep> builder)
    {
        builder.Property(s => s.IsCompleted).IsRequired();
        builder.Property(s => s.CompletedAt).IsRequired(false);
        builder.HasOne(s => s.ServiceRecord).WithMany(r => r.Steps).HasForeignKey(s => s.ServiceRecordId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(s => s.Procedure).WithMany().HasForeignKey(s => s.ProcedureId).OnDelete(DeleteBehavior.SetNull);
    }
}
