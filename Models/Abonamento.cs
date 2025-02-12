namespace sgchdAPI.Models
{
	public class Abonamento
	{
		public int Id { get; set; }
		public int DocenteId { get; set; }
		public Docente? Docente { get; set; }
		public string Titulo { get; set; }
		public string? Descricao { get; set; }
		public int Duracao { get; set; }
		public string? UrlPdf { get; set; }
		public DateTime DataInicio { get; set; }

		public Abonamento() { }

		// Construtor com parâmetros
		public Abonamento(

			int docenteId,
			string titulo,
			string descricao,
			int duracao,
			string urlPdf
		)
		{

			Titulo = titulo;
			Descricao = descricao;
			Duracao = duracao;
			UrlPdf = urlPdf;
			DocenteId = docenteId;
		}
	}
}
