using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using sgchdAPI.Data.Seeds; // Importação do namespace atual
using sgchdAPI.Services; // Importação do RoleSeeder

namespace sgchdAPI.Data.Seeds
{
	public static class DatabaseSeeder
	{
		public static async Task SeedAsync(IServiceProvider serviceProvider)
		{
			using var scope = serviceProvider.CreateScope();
			var services = scope.ServiceProvider;

			try
			{
				// Aplica migrações pendentes
				var context = services.GetRequiredService<ApplicationDbContext>();
				await context.Database.MigrateAsync();

				// Executa o seeding de roles
				var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
				await RoleSeeder.SeedRolesAsync(roleManager);

				// Executa o seeding do banco de dados
				// SeedDatabase(context);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Erro ao executar o seeding: {ex.Message}");
			}
		}

		// CONFIGURAÇÃO DO SEEDING DO BANCO DE DADOS
		// Verifica se as tabelas estão vazias e executa o seeding
		private static void SeedDatabase(ApplicationDbContext context)
		{
			// if (!context.Cursos.Any())
			// {
			// 	CursoSeed.Seed(context);
			// }

			// if (!context.Disciplinas.Any())
			// {
			// 	DisciplinaSeed.Seed(context);
			// }

			// if (!context.Docentes.Any())
			// {
			// 	DocenteSeed.Seed(context);
			// }

			// if (!context.DocentesElegiveis.Any())
			// {
			// 	DocenteElegivelSeed.Seed(context);
			// }

			// Adicione verificações semelhantes para outros seeds, se necessário
			// if (!context.Abonamentos.Any()) { AbonamentoSeed.Seed(context); }
		}
	}
}
