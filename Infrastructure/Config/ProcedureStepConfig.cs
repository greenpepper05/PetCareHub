using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config;

public class ProcedureStepConfig : IEntityTypeConfiguration<ProcedureStep>
{

    public void Configure(EntityTypeBuilder<ProcedureStep> builder)
    {
        builder.HasOne(p => p.Appointment).WithMany(a => a.ProcedureSteps).HasForeignKey(p => p.AppointmentId);
    }
}
