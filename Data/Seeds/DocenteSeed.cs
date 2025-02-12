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
					new Docente(1, "Jose Jamerson", "jjamersonmt@gmail.com"),
					new Docente(2, "Maria Joana", "mariajoana@gmail.com"),
					new Docente(3, "Joao Pedro", "joaopedro@gmail.com")
				);
				context.SaveChanges();
			}
		}
	}
}
