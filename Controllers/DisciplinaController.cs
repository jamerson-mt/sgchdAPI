using Microsoft.AspNetCore.Mvc;
using sgchdAPI.Data;

namespace sgchdAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class DisciplinaController(ApplicationDbContext context) : ControllerBase
	{
		private readonly ApplicationDbContext _context = context;

		[HttpGet]
		public IActionResult GetAll()
		{
			var disciplinas = _context.Disciplinas.ToList();
			return Ok(disciplinas);
		}

		[HttpGet("{id}")]
		public IActionResult GetById(int id)
		{
			var disciplina = _context.Disciplinas.FirstOrDefault(d => d.Id == id);
			if (disciplina == null)
			{
				return NotFound("Disciplina não encontrada");
			}
			return Ok(disciplina);
		}
	}
}
