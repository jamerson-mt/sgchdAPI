using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace sgchdAPI.Models
{
	public class DisciplinaDocente
	{
		public DisciplinaDocente(
			int docenteId,
			int disciplinaId,
			int oldDocenteId,
			int newDocenteId
		)
		{
			DisciplinaId = disciplinaId;
			DocenteId = docenteId;
			this.oldDocenteId = oldDocenteId;
			this.newDocenteId = newDocenteId;
		}

		// Novo construtor sem parâmetros
		public DisciplinaDocente() { }

		public int DisciplinaId { get; set; }
		public Disciplina? Disciplina { get; set; }
		public int DocenteId { get; set; }
		public Docente? Docente { get; set; }

		[NotMapped]
		public int? oldDocenteId { get; set; }

		[NotMapped]
		public int? newDocenteId { get; set; }
	}
}
