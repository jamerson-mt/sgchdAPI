using sgchdAPI.Models;

namespace sgchdAPI.Data.Seeds
{
	public static class DocenteElegivelSeed
	{
		public static void Seed(ApplicationDbContext context)
		{
			if (!context.DocentesElegiveis.Any())
			{
				context.DocentesElegiveis.AddRange(
					new DocenteElegivel { DisciplinaId = 1, DocenteId = 1 },
					new DocenteElegivel { DisciplinaId = 2, DocenteId = 1 },
					new DocenteElegivel { DisciplinaId = 3, DocenteId = 1 }
				);
				context.SaveChanges();
			}
		}
	}
}
