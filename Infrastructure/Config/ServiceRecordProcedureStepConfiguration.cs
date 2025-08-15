using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config;

public class ServiceRecordProcedureStepConfiguration : IEntityTypeConfiguration<ServiceRecordProcedureStep>
{
    public void Configure(EntityTypeBuilder<ServiceRecordProcedureStep> builder)
    {
        builder.Property(x => x.IsCompleted).IsRequired();
        builder.Property(x => x.CompletedAt).IsRequired(false);
        builder.HasOne(x => x.ServiceRecordProcedures).WithMany(srp => srp.ProcedureSteps).HasForeignKey(x => x.ServiceProcedureId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Procedure).WithMany().HasForeignKey(x => x.ProcedureId).OnDelete(DeleteBehavior.Restrict);
    }
}
