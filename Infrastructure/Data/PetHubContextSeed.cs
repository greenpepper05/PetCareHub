using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Data;

public class PetHubContextSeed
{
    public static async Task SeedAsync(PetHubContext context, UserManager<AppUser> userManager)
    {
        if (!userManager.Users.Any(x => x.UserName == "admin@text.com"))
        {
            var user = new AppUser
            {
                UserName = "admin@text.com",
                Email = "admin@text.com",
            };

            await userManager.CreateAsync(user, "Pa$$w0rd");
            await userManager.AddToRoleAsync(user, "SuperAdmin");
        }

    }
}
