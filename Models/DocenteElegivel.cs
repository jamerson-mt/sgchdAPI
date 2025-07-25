namespace sgchdAPI.Models
{
	public class DocenteElegivel
	{
		public int DisciplinaId { get; set; }
		public Disciplina Disciplina { get; set; }

		public int DocenteId { get; set; }
		public Docente Docente { get; set; }
	}
}
