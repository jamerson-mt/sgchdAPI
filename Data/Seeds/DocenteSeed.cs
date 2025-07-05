using sgchdAPI.Models;

namespace sgchdAPI.Data.Seeds
{
	public static class DocenteSeed
	{
		public static void Seed(ApplicationDbContext context)
		{
			if (!context.Docentes.Any())
			{
				context.Docentes.AddRange(
					new Docente(0, "Jose Jamerson", "jjamersonmt@gmail.com"),
					new Docente(0, "Maria Joana", "mariajoana@gmail.com"),
					new Docente(0, "Joao Pedro", "joaopedro@gmail.com")
				);
				context.SaveChanges();
			}
		}
	}
}
