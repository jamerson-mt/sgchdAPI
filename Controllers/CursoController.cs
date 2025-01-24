using Microsoft.AspNetCore.Mvc;
using sgchdAPI.Data;

namespace sgchdAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CursoController(ApplicationDbContext context) : ControllerBase
	{
		private readonly ApplicationDbContext _context = context;

		[HttpGet]
		public IActionResult GetAll()
		{
			var cursos = _context.Cursos.ToList();
			return Ok(cursos);
		}

		[HttpGet("{id}")]
		public IActionResult GetById(int id)
		{
			var curso = _context.Cursos.FirstOrDefault(c => c.Id == id);
			if (curso == null)
			{
				return NotFound("Curso não encontrado");
			}
			return Ok(curso);
		}
	}
}
