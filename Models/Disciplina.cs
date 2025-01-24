namespace sgchdAPI.Models
{
	public class Disciplina(int id, string name, int cursoId, int cargaHoraria, int periodo)
	{
		public int Id { get; set; } = id;
		public string Name { get; set; } = name;
		public int CursoId { get; set; } = cursoId;
		public Curso? Curso { get; set; } // Propriedade de navegação para Curso
		public int Periodo { get; set; } = periodo;
		public int CargaHoraria { get; set; } = cargaHoraria;

		public ICollection<DisciplinaDocente>? DisciplinaDocentes { get; set; }
		public ICollection<DocenteElegivel>? DocentesElegiveis { get; set; } //
	}
}
