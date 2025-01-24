using sgchdAPI.Models;

namespace sgchdAPI.Data.Seeds
{
	public static class CursoSeed
	{
		public static void Seed(ApplicationDbContext context)
		{
			if (!context.Cursos.Any())
			{
				context.Cursos.AddRange(new Curso(1, "TSI"), new Curso(2, "ADM"));
				context.SaveChanges();
			}
		}
	}
}
