using sgchdAPI.Enums;

namespace sgchdAPI.Models
{
	public class Atividade
	{
		public int Id { get; set; }
		public int DocenteId { get; set; }
		public Docente? Docente { get; set; }
		public string Titulo { get; set; }
		public string? Descricao { get; set; }
		public int Duracao { get; set; }
		public TypeActivity Tipo { get; set; }

		public Atividade(
			int docenteId,
			string titulo,
			string descricao,
			int duracao,
			TypeActivity tipo
		)
		{
			DocenteId = docenteId;
			Titulo = titulo;
			Descricao = descricao;
			Duracao = duracao;
			Tipo = tipo;
		}
	}
}
