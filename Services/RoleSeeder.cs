using Microsoft.AspNetCore.Identity;

namespace sgchdAPI.Services
{
	public static class RoleSeeder
	{
		public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
		{
			var roles = new[] { "Admin", "User", "Manager" };
			foreach (var role in roles)
			{
				if (!await roleManager.RoleExistsAsync(role))
				{
					await roleManager.CreateAsync(new IdentityRole(role));
				}
			}
		}
	}
}
