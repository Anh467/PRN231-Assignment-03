using BusinessObject.Models;
using BusinessObject.Utils.MyContansts;
using DataAccess.DataBase;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess.Core.Data
{
    public static class SeedData
    {
        public static async Task CreateRolesAndUsers(IServiceProvider services)
        {
            using var context = new ApplicationDBContext();
            var userManager = services.GetService<UserManager<ApplicationUser>>();
            var roleManager = services.GetService<RoleManager<IdentityRole>>();

            // create admin
            var user = new ApplicationUser
            {
                UserName = "admin.@gmail.com",
                Email = "admin.@gmail.com",
                EmailConfirmed = true,
            };
            var userInDb = await userManager.FindByEmailAsync(user.Email);

            if (userInDb != null)
                return;

            // seed role
            await roleManager.CreateAsync(new IdentityRole(nameof(Roles.Admin)));
            await roleManager.CreateAsync(new IdentityRole(nameof(Roles.Staff)));
            await roleManager.CreateAsync(new IdentityRole(nameof(Roles.User)));

            // add rolde
            await userManager.CreateAsync(user, "Admin@123");
            await userManager.AddToRoleAsync(user, Roles.Admin.ToString());
        }
    }
}
