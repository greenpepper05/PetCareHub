using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class PetHubContextSeed
{
    public static async Task SeedAsync(PetHubContext context,
        UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        
        if (!await roleManager.RoleExistsAsync("SuperAdmin"))
        {
            await roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
        }
         if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        if (!await roleManager.RoleExistsAsync("Staff"))
        {
            await roleManager.CreateAsync(new IdentityRole("Staff"));
        }

        if (!await roleManager.RoleExistsAsync("Customer"))
        {
            await roleManager.CreateAsync(new IdentityRole("Customer"));
        }

        if (!userManager.Users.Any(x => x.UserName == "admin4@test.com"))
        {
            var user = new AppUser
            {
                Id = "1",
                UserName = "admin4@test.com",
                Email = "admin4@test.com",
                FirstName = "admin4",
                LastName = "test4"
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
                FirstName = "admin5",
                LastName = "test5",
            };

            await userManager.CreateAsync(user, "Pa$$w0rd");
            await userManager.AddToRoleAsync(user, "Admin");
        }

        var owner = await userManager.FindByEmailAsync("admin5@test.com");

        if (!context.Clinics.Any())
        {
            var clinic = new Clinic
            {
                OwnerId = owner?.Id,
                ClinicName = "Pawsionate Hands Veterinary Clinic",
                Address = "Unit 2, Fairview Maliwalo Bldg., Zone 1, Maliwalo, Tarlac City",
                PhoneNumber = "09193855281",
                Email = "pawsionatehands@gmail.com"

            };

            context.Clinics.AddRange(clinic);
            await context.SaveChangesAsync();
        }

        if (!context.Clinics.Any(x => x.Id == 0))
        {
            var clinic = new Clinic
            {
                Id = 0,
                OwnerId = "system",
                ClinicName = "Default",
                Address = "Admin",
                PhoneNumber = "00000000000",
                Email = "petcarehub2025@gmail.com"
            };
        }

        if (!context.Pets.Any())
        {
            var pet = new Pet
            {
                Name = "Default Pet",
                Breed = "Default",
                Species = "Default",
                Birthdate = new DateTime(2022, 1, 1),
                Gender = "Male",
                OwnerId = "1"
            };

            await context.Pets.AddAsync(pet);
            await context.SaveChangesAsync();
        }

        

        if (!context.Services.Any())
        {
            var clinic = await context.Clinics.FirstAsync();
            var services = new List<Service>
            {
                new() { Name = "Consultation", Description = "General pet consultation", ClinicId = clinic.Id },
                new() { Name = "Vaccination", Description = "Scheduled vaccination", ClinicId = clinic.Id},
                new() { Name = "Deworming", Description = "Parasite treatment", ClinicId = clinic.Id},
                new() { Name = "Pet Grooming", Description = "Bath, trim, and brush", ClinicId = clinic.Id},
                new() { Name = "Surgery", Description = "Pre-scheduled veterinary surgery", ClinicId = clinic.Id},
            };

            context.Services.AddRange(services);
            await context.SaveChangesAsync();
        }
        
    }
}
