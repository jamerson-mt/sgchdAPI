using Microsoft.AspNetCore.Mvc;
using sgchdAPI.Data;
using sgchdAPI.Models;

namespace sgchdAPI.Controllers
{
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
			var docentesElegiveis = _context.DocentesElegiveis
				.Where(d => d.DocenteId == docenteId)
				.ToList();
			return Ok(docentesElegiveis);
		}

		[HttpGet("disciplina/{disciplinaId}")] // Rota para buscar docentes elegíveis passando o id da disciplina
		public IActionResult GetByDisciplina(int disciplinaId)
		{
			var docentesElegiveis = _context.DocentesElegiveis
				.Where(d => d.DisciplinaId == disciplinaId)
				.ToList();
			return Ok(docentesElegiveis);
		}

    }
}
