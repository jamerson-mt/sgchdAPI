using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sgchdAPI.Data;
using sgchdAPI.Models;

namespace sgchdAPI.Controllers
{
	[Authorize(Roles = "Admin")]
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

		[HttpPost]
		public IActionResult Create(Disciplina disciplina)
		{
			// verifique se existe disciplina com o nome informado

			var disciplinaExistente = _context.Disciplinas.FirstOrDefault(d =>
				d.Name == disciplina.Name
			);

			if (disciplinaExistente != null)
			{
				return Conflict("Já existe uma disciplina com esse nome");
			}

			_context.Disciplinas.Add(disciplina);
			_context.SaveChanges();
			return CreatedAtAction(nameof(GetById), new { id = disciplina.Id }, disciplina);
		}

		[HttpPut("{id}")]
		public IActionResult Update(int id, Disciplina disciplina)
		{
			var disciplinaExistente = _context.Disciplinas.FirstOrDefault(d => d.Id == id);
			if (disciplinaExistente == null)
			{
				return NotFound("Disciplina não encontrada");
			}
			disciplinaExistente.Name = disciplina.Name;
			disciplinaExistente.CursoId = disciplina.CursoId;
			disciplinaExistente.CargaHoraria = disciplina.CargaHoraria;
			disciplinaExistente.Periodo = disciplina.Periodo;
			_context.SaveChanges();
			return NoContent();
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			var disciplina = _context.Disciplinas.FirstOrDefault(d => d.Id == id);
			if (disciplina == null)
			{
				return NotFound("Disciplina não encontrada");
			}
			_context.Disciplinas.Remove(disciplina);
			_context.SaveChanges();
			return NoContent();
		}
	}
}
