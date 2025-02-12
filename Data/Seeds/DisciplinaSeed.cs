using sgchdAPI.Models;

namespace sgchdAPI.Data.Seeds
{
	public static class DisciplinaSeed
	{
		public static void Seed(ApplicationDbContext context)
		{
			if (!context.Disciplinas.Any())
			{
				context.Disciplinas.AddRange(
					new Disciplina(1, "WEB 1", 1, 6, 1),
					new Disciplina(2, "WEB 2", 1, 2, 1),
					new Disciplina(3, "POO", 1, 3, 1)
				);
				context.SaveChanges();
			}
		}
	}
}
