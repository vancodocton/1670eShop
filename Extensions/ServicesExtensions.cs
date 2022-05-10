using Microsoft.AspNetCore.Identity;
using WebApplication1.Data;

namespace WebApplication1.Extensions
{
    public static class ServicesExtensions
    {
        public static async Task<IServiceProvider> InitializeRolesAsync(this IServiceProvider services)
        {

            var roleManager = services.CreateAsyncScope().ServiceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            await roleManager.CreateAsync(new IdentityRole(Role.Seller));
            await roleManager.CreateAsync(new IdentityRole(Role.Seller));

            return services;
        }
    }
}
