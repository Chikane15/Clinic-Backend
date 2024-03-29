using Microsoft.AspNetCore.Identity;

namespace Assignment_05_03.DefaultRoles
{
    public class Default_User_Roles
    {
        public static async Task CreateDefaultAdmin_Role(IServiceProvider serviceProvider)
        {
            try
            {
                // retrive instances of the RoleManager and UserManager 
                //from the Dependency Container
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

                IdentityResult result;
                // add a new Administrator role for the application
                //Check if it alreay exists
                var isRoleExist = await roleManager.RoleExistsAsync("Administrator");
                if (!isRoleExist)
                {
                    // create Administrator Role and add it in Database
                    result = await roleManager.CreateAsync(new IdentityRole("Administrator"));
                }

                // code to create a default user and add it to Administrator Role
                var user = await userManager.FindByEmailAsync("Admin123@gmail.com");
                //Check if it already exist or not
                if (user == null)
                {
                    var defaultUser = new IdentityUser()
                    {
                        UserName = "Admin123@gmail.com",
                        Email = "Admin123@gmail.com"
                    };
                    var regUser = await userManager.CreateAsync(defaultUser, "Admin@123");
                    await userManager.AddToRoleAsync(defaultUser, "Administrator");
                }
            }
            catch (Exception ex)
            {
                var str = ex.Message;
            }

        }
    }
}
