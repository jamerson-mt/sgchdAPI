using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

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
		public IActionResult GetByDoscente(int doscenteId)
		{
			var docentesElegiveis = _context.DocentesElegiveis
				.Where(d => d.DoscenteId == doscenteId)
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
