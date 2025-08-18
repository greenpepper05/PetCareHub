using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Data;

public class PetHubContextSeed
{
    public static async Task SeedAsync(PetHubContext context,
        UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {

        if (!context.Clinics.Any())
        {
            var clinic = new Clinic
            {
                OwnerId = "2",
                ClinicName = "Pawsionate Hands Veterinary Clinic",
                Address = "Unit 2, Fairview Maliwalo Bldg., Zone 1, Maliwalo, Tarlac City",
                PhoneNumber = "09193855281",
                Email = "pawsionatehands@gmail.com"
                
            };

            context.Clinics.AddRange(clinic);
            await context.SaveChangesAsync();
        }
        
        if (!roleManager.Roles.Any())
        {
            string[] roles = ["SuperAdmin", "Admin", "Customer", "Staff"];
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        if (!userManager.Users.Any(x => x.UserName == "admin4@test.com"))
        {
            var user = new AppUser
            {
                Id = "1",
                UserName = "admin4@test.com",
                Email = "admin4@test.com",
                ClinicId = 1
            };

            await userManager.CreateAsync(user, "Pa$$w0rd");
            await userManager.AddToRoleAsync(user, "SuperAdmin");
        }

        if (!userManager.Users.Any(x => x.UserName == "admin5@test.com"))
        {
            var user = new AppUser
            {
                Id = "2",
                UserName = "admin5@test.com",
                Email = "admin5@test.com",
                ClinicId = 1
            };

            await userManager.CreateAsync(user, "Pa$$w0rd");
            await userManager.AddToRoleAsync(user, "Admin");
        }

        if (!context.Pets.Any())
        {
            var pet = new Pet
            {
                Name = "Default Pet",
                Breed = "Mixed",
                Species = "Dog",
                Birthdate = new DateTime(2022, 1, 1),
                Gender = "Male",
                OwnerId = "1"
            };

            await context.Pets.AddAsync(pet);
            await context.SaveChangesAsync();
        }

        

        if (!context.Services.Any())
        {
            var services = new List<Service>
            {
                new() { Name = "Consultation", Description = "General pet consultation"},
                new() { Name = "Vaccination", Description = "Scheduled vaccination"},
                new() { Name = "Deworming", Description = "Parasite treatment"},
                new() { Name = "Pet Grooming", Description = "Bath, trim, and brush"},
                new() { Name = "Surgery", Description = "Pre-scheduled veterinary surgery"},
            };

            context.Services.AddRange(services);
            await context.SaveChangesAsync();
        }
        
    }
}
