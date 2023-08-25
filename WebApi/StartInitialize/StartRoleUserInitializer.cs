using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.StartInitialize
{
    public class StartRoleUserInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminName = "admin";
            string adminPassword = "admin";
            string adminRole = "admin";

            if (await roleManager.FindByNameAsync(adminRole) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(adminRole));
            }

            if (await userManager.FindByNameAsync(adminName) == null)
            {
                User user = new User { UserName = adminName };
                IdentityResult identityResult = await userManager.CreateAsync(user, adminPassword);
                if (identityResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, adminRole);
                }
            }
        }
    }
}
