using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sgchdAPI.Data;
using sgchdAPI.Models;

namespace sgchdAPI.Controllers
{
	[Authorize] // Apenas usuários com as roles ADMIN ou DOCENTE podem acessar este controller
	[ApiController]
	[Route("api/[controller]")]
	public class DocenteController(ApplicationDbContext context) : ControllerBase
	{
		private readonly ApplicationDbContext _context = context;

		[HttpGet]
		public IActionResult GetAll()
		{
			var docentes = _context.Docentes.ToList();
			return Ok(docentes);
		}

		[HttpGet("{id}")]
		public IActionResult GetById(int id)
		{
			var docente = _context.Docentes.FirstOrDefault(d => d.Id == id);
			if (docente == null)
			{
				return NotFound("Docente não encontrado");
			}
			return Ok(docente);
		}

		[HttpPost]
		public IActionResult Create(Docente docente)
		{
			_context.Docentes.Add(docente);
			_context.SaveChanges();
			return CreatedAtAction(nameof(GetById), new { id = docente.Id }, docente);
		}

		[HttpPut("{id}")]
		public IActionResult Update(int id, Docente docente)
		{
			var docenteExistente = _context.Docentes.FirstOrDefault(d => d.Id == id);
			if (docenteExistente == null)
			{
				return NotFound("Docente não encontrado");
			}

			docenteExistente.Name = docente.Name;
			docenteExistente.Email = docente.Email;

			_context.Docentes.Update(docenteExistente);
			_context.SaveChanges();
			return NoContent();
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			var docente = _context.Docentes.FirstOrDefault(d => d.Id == id);
			if (docente == null)
			{
				return NotFound("Docente não encontrado");
			}

			_context.Docentes.Remove(docente);
			_context.SaveChanges();
			return NoContent();
		}
	}
}
