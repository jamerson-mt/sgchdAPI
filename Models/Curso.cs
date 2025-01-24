namespace sgchdAPI.Models
{
	public class Curso
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public ICollection<Disciplina>? Disciplinas { get; set; } // Coleção de Disciplinas

		public Curso(int id, string name)
		{
			Id = id;
			Name = name;
			Disciplinas = new List<Disciplina>();
		}
	}
}
