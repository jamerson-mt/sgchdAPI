using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sgchdAPI.Data;
using sgchdAPI.Models;

namespace sgchdAPI.Controllers
{
	[Authorize(Roles = "admin,manager")] // Apenas usuários com as roles ADMIN ou MANAGER podem acessar este controller
	[ApiController]
	[Route("api/[controller]")]
	public class DocenteElegivelController(ApplicationDbContext context) : ControllerBase
	{
		private readonly ApplicationDbContext _context = context;

		[HttpGet]
		public IActionResult GetAll()
		{
			var docentesElegiveis = _context.DocentesElegiveis.ToList();
			return Ok(docentesElegiveis);
		}

		[HttpGet("{docenteId}")] // GET /api/docenteelegivel/1
		public IActionResult GetByDocente(int docenteId)
		{
			var docentesElegiveis = _context
				.DocentesElegiveis.Where(d => d.DocenteId == docenteId)
				.ToList();
			return Ok(docentesElegiveis);
		}

		[HttpGet("disciplina/{disciplinaId}")] // Rota para buscar docentes elegíveis passando o id da disciplina
		public IActionResult GetByDisciplina(int disciplinaId)
		{
			var docentesElegiveis = _context
				.DocentesElegiveis.Where(d => d.DisciplinaId == disciplinaId)
				.ToList();
			return Ok(docentesElegiveis);
		}

		[HttpPost]
		public IActionResult Create(int docenteId, int disciplinaId)
		{
			var docenteElegivel = new DocenteElegivel
			{
				DocenteId = docenteId,
				DisciplinaId = disciplinaId,
			};

			_context.DocentesElegiveis.Add(docenteElegivel);
			_context.SaveChanges();
			return CreatedAtAction(
				nameof(GetByDocente),
				new { docenteId = docenteElegivel.DocenteId },
				docenteElegivel
			);
		}

		[HttpDelete("{docenteId}/{disciplinaId}")]
		public IActionResult Delete(int docenteId, int disciplinaId)
		{
			var docenteElegivel = _context.DocentesElegiveis.FirstOrDefault(de =>
				de.DocenteId == docenteId && de.DisciplinaId == disciplinaId
			);
			if (docenteElegivel == null)
			{
				return NotFound("Docente elegível não encontrado");
			}

			_context.DocentesElegiveis.Remove(docenteElegivel);
			_context.SaveChanges();
			return NoContent();
		}
	}
}
