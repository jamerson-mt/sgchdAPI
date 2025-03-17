using Microsoft.EntityFrameworkCore;

namespace sgchdAPI.Models
{
	[Index(nameof(Email), IsUnique = true)]
	public class Docente
	{
		public Docente(int id, string name, string email)
		{
			Id = id;
			Name = name;
			Email = email;
		}

		public int Id { get; set; }
		public string Name { get; set; }

		// coloque o email como unico no banco de dados
		public string Email { get; set; }


		public ICollection<DisciplinaDocente>? DisciplinaDocentes { get; set; }
		public ICollection<DocenteElegivel>? DisciplinasElegiveis { get; set; }
		public ICollection<Abonamento>? Abonamentos { get; set; }
		public ICollection<Atividade>? Atividades { get; set; }
	}
}
