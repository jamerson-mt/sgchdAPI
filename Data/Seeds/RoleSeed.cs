using Microsoft.AspNetCore.Identity;

namespace sgchdAPI.Data.Seeds
{
	public static class SeedRoles
	{
		/// <summary>
		/// Insere os papéis no banco de dados caso eles ainda não existam.
		/// </summary>
		public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
		{
			var roles = new[] { "Admin", "Docente", "Aluno", "User" };
			foreach (var role in roles)
			{
				if (!await roleManager.RoleExistsAsync(role)) // checa se o papel já existe
				{
					await roleManager.CreateAsync(new IdentityRole(role)); // cria o papel
				}
			}
		}
	}
}
