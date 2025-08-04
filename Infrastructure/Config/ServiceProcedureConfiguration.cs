// using Core.Entities;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Metadata.Builders;

// namespace Infrastructure.Config;

// public class ServiceProcedureConfiguration : IEntityTypeConfiguration<ServiceProcedure>
// {
//     public void Configure(EntityTypeBuilder<ServiceProcedure> builder)
//     {
//         builder.HasOne(sp => sp.Appointment)
//             .WithMany(s => s.Procedures)
//             .HasForeignKey(sp => sp.AppointmentServiceId);
//     }
// }
