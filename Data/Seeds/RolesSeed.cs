using Microsoft.AspNetCore.Identity;
using sgchdAPI.Models;

namespace sgchdAPI.Data.Seeds
{
	public static class RolesSeed
	{
		public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
		{
			if (!await roleManager.RoleExistsAsync("Admin"))
			{
				await roleManager.CreateAsync(new IdentityRole("Admin"));
			}

			if (!await roleManager.RoleExistsAsync("Docente"))
			{
				await roleManager.CreateAsync(new IdentityRole("Docente"));
			}

			if (!await roleManager.RoleExistsAsync("Aluno"))
			{
				await roleManager.CreateAsync(new IdentityRole("Aluno"));
			}

			if (!await roleManager.RoleExistsAsync("User"))
			{
				await roleManager.CreateAsync(new IdentityRole("User"));
			}
			if (!await roleManager.RoleExistsAsync("Guest"))
			{
				await roleManager.CreateAsync(new IdentityRole("Guest"));
			}
		}

		internal static object SeedRoles(object roleManager)
		{
			throw new NotImplementedException();
		}
	}
}
