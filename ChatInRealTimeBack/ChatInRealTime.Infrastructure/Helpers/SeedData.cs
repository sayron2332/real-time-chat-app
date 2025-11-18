using ChatInRealTime.Core.Entites;
using ChatInRealTime.Core.Interfaces;
using ChatInRealTime.Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatInRealTime.Infrastructure.Helpers
{
    public class SeedData
    {
         public static async Task Initialize(IServiceProvider serviceProvider)
         {
            var context = serviceProvider.GetRequiredService<AppDbContext>();
            var roleService = serviceProvider.GetRequiredService<IRoleService>();


            async Task SeedUsersAndRoles()
            {
                var roleExist = await roleService.GetRoleByNameAsync("admin");
                if (roleExist == null)
                {
                    AppRole[] identityRoles =
                    {
                        new AppRole {Name = "user"},
                        new AppRole {Name = "admin"}
                    };
                    foreach (var role in identityRoles)
                    {
                        await context.AppRoles.AddAsync(role);
                    }
                    await context.SaveChangesAsync();
                }


                var AdminRole = await roleService.GetRoleByNameAsync("admin");
                if (!context.AppUsers.Any())
                {
                    var hasher = new PasswordHasher<AppUser>();
                    var user = new AppUser
                    {
                        Name = "Nazar",
                        Surname = "Kurylovych",
                        Email = "admin@example.com",
                        PasswordHash = hasher.HashPassword(null!, "Admin123!"),
                        AppRoleId = AdminRole!.Id
                    };

                    // Створення користувача
                    var result = await context.AppUsers.AddAsync(user);
                    await context.SaveChangesAsync();

                }
            }
            
            await SeedUsersAndRoles();

         }
    }
}

