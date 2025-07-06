using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Data;

public class PetHubContextSeed
{
    public static async Task SeedAsync(PetHubContext context, UserManager<AppUser> userManager)
    {
        if (!userManager.Users.Any(x => x.UserName == "admin4@test.com"))
        {
            var user = new AppUser
            {
                UserName = "admin4@test.com",
                Email = "admin4@test.com",
            };

            await userManager.CreateAsync(user, "Pa$$w0rd");
            await userManager.AddToRoleAsync(user, "SuperAdmin");
        }

        if (!userManager.Users.Any(x => x.UserName == "admin5@test.com"))
        {
            var user = new AppUser
            {
                UserName = "admin5@test.com",
                Email = "admin5@test.com"
            };

            await userManager.CreateAsync(user, "Pa$$w0rd");
            await userManager.AddToRoleAsync(user, "Admin");
        }

        if (!context.AppointmentServices.Any())
        {
            var services = new List<AppointmentService>
            {
                new() { Name = "Consultation", Description = "General pet consultation"},
                new() { Name = "Vaccination", Description = "Scheduled vaccination"},
                new() { Name = "Deworming", Description = "Parasite treatment"},
                new() { Name = "Pet Grooming", Description = "Bath, trim, and brush"},
                new() { Name = "Surgery", Description = "Pre-scheduled veterinary surgery"},
            };

            context.AppointmentServices.AddRange(services);
            await context.SaveChangesAsync();
        }
    }
}
