using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sgchdAPI.Data;
using sgchdAPI.Models;

namespace sgchdAPI.Controllers
{
	[Authorize(Roles = "Admin")]
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

		[HttpPost]
		public IActionResult Create(Curso curso)
		{
			_context.Cursos.Add(curso);
			_context.SaveChanges();
			return StatusCode(201, curso);
		}

		[HttpPut("{id}")]
		public IActionResult Update(int id, Curso curso)
		{
			var cursoDB = _context.Cursos.FirstOrDefault(c => c.Id == id);
			if (cursoDB == null)
			{
				return NotFound("Curso não encontrado");
			}

			cursoDB.Name = curso.Name; // Atualiza o nome do curso
			_context.SaveChanges(); // Salva as alterações
			return Ok(cursoDB); // Retorna o curso atualizado
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			var curso = _context.Cursos.FirstOrDefault(c => c.Id == id);
			if (curso == null)
			{
				return NotFound("Curso não encontrado");
			}
			_context.Cursos.Remove(curso);
			_context.SaveChanges();
			return NoContent();
		}
	}
}
