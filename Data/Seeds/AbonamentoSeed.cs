using sgchdAPI.Models;

namespace sgchdAPI.Data.Seeds
{
	public static class AbonamentoSeed
	{
		public static void Seed(ApplicationDbContext context)
		{
			if (!context.Abonamentos.Any())
			{
				context.Abonamentos.AddRange(
					new Abonamento(1, "titulo", "descricao", 2, "default.pdf"),
					new Abonamento(1, "titulo2", "descricao2", 3, "default.pdf")
				);

				context.SaveChanges();
			}
		}
	}
}
