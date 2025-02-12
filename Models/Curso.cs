namespace sgchdAPI.Models
{
	public class Curso(int id, string name)
	{
		public int Id { get; set; } = id;
		public string Name { get; set; } = name;
		public ICollection<Disciplina>? Disciplinas { get; set; } = new List<Disciplina>();
	}
}
