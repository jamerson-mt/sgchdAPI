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
					new Disciplina(1, "Matematica", 6, 1),
					new Disciplina(2, "Portugues", 2, 1),
					new Disciplina(3, "Historia", 3, 1)
				);
				context.SaveChanges();
			}
		}
	}
}
