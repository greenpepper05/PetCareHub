using Core.Entities;
using Infrastructure.Config;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class PetHubContext(DbContextOptions options) : IdentityDbContext<AppUser>(options)
{
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Pet> Pets { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<Clinic> Clinics { get; set; }
    public DbSet<ServiceRecord> ServiceRecords { get; set; }
    public DbSet<Staff> Staffs { get; set; }
    public DbSet<Procedure> Procedures { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppointmentConfiguration).Assembly);
    }
}
