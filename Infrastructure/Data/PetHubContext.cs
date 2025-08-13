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
    public DbSet<PetServiceHistory> PetServiceHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // modelBuilder.ApplyConfigurationsFromAssembly(typeof(RoleConfiguration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppointmentConfiguration).Assembly);
    }
}
