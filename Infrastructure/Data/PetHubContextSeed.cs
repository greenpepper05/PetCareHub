using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Data;

public class PetHubContextSeed
{
    public static async Task SeedAsync(PetHubContext context, UserManager<AppUser> userManager)
    {
        if (!userManager.Users.Any(x => x.UserName == "admin3@test.com"))
        {
            var user = new AppUser
            {
                UserName = "admin3@test.com",
                Email = "admin3@test.com",
            };

            await userManager.CreateAsync(user, "Pa$$w0rd");
            await userManager.AddToRoleAsync(user, "SuperAdmin");
        }

    }
}
