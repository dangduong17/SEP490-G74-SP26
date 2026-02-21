using Microsoft.AspNetCore.Identity;
using vn.edu.fpt.entity;

namespace vn.edu.fpt.data
{
    public static class DbInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var context = serviceProvider.GetRequiredService<RJMSDbContext>();

            // 1. Create Roles
            string[] roleNames = { "Admin", "Candidate", "Recruiter" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // 2. Create Admin User
            var adminEmail = "admin@gmail.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new User
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "System",
                    LastName = "Admin",
                    EmailConfirmed = true,
                    CreatedAt = vn.edu.fpt.helper.DateTimeHelper.NowVietnam
                };

                var createResult = await userManager.CreateAsync(adminUser, "12345678");
                
                if (createResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");

                    // Create Admin Profile
                    var adminProfile = new Admin
                    {
                        UserId = adminUser.Id,
                        FullName = "System Admin",
                        Department = "IT",
                        CreatedAt = vn.edu.fpt.helper.DateTimeHelper.NowVietnam
                    };
                    
                    context.Admins.Add(adminProfile);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}

