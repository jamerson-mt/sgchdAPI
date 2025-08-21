using System.Data.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sgchdAPI.Data;
using sgchdAPI.Models;

namespace sgchdAPI.Controllers
{
	[Authorize(Policy = "AuthenticatedUserPolicy")] // Apenas usuários autenticados podem acessar este endpoint
	[ApiController]
	[Route("api/[controller]")]
	public class DisciplinaDocenteController(ApplicationDbContext context) : ControllerBase
	{
		private readonly ApplicationDbContext _context = context;

		[HttpGet]
		public IActionResult GetAll()
		{
			var disciplinaDocentes = _context.DisciplinaDocentes.ToList();
			return Ok(disciplinaDocentes);
		}

		[HttpGet("docente/{docenteId}")]
		public IActionResult GetByDocenteId(int docenteId)
		{
			var disciplinaDocentes = _context
				.DisciplinaDocentes.Where(dd => dd.DocenteId == docenteId)
				.ToList();
			return Ok(disciplinaDocentes);
		}

		[HttpGet("disciplina/{disciplinaId}")]
		public IActionResult GetByDisciplinaId(int disciplinaId)
		{
			var disciplinaDocentes = _context
				.DisciplinaDocentes.Where(dd => dd.DisciplinaId == disciplinaId)
				.ToList();
			return Ok(disciplinaDocentes);
		}

		[HttpPost]
		public IActionResult Create(DisciplinaDocente disciplinaDocente)
		{
			_context.DisciplinaDocentes.Add(disciplinaDocente);
			_context.SaveChanges();
			return CreatedAtAction(
				nameof(GetByDocenteId),
				new { docenteId = disciplinaDocente.DocenteId },
				disciplinaDocente
			);
		}

		[HttpPut]
		public IActionResult Update(DisciplinaDocente request)
		{
			var existingEntity = _context.DisciplinaDocentes.FirstOrDefault(dd =>
				dd.DisciplinaId == request.DisciplinaId && dd.DocenteId == request.oldDocenteId
			);

			//valide se docente new existe
			var docente = _context.Docentes.Find(request.newDocenteId);
			if (docente == null)
			{
				return NotFound("Docente não encontrado");
			}

			if (existingEntity == null)
			{
				return NotFound();
			}

			// Remove the existing entity
			_context.DisciplinaDocentes.Remove(existingEntity);
			_context.SaveChanges();

			// Add the updated entity
			if (request.newDocenteId == null)
			{
				return BadRequest("newDocenteId não pode ser nulo");
			}

			request.DocenteId = (int)request.newDocenteId;

			_context.DisciplinaDocentes.Add(request);
			_context.SaveChanges();

			return NoContent();
		}

		[HttpDelete("{docenteId}/{disciplinaId}")]
		public IActionResult Delete(int docenteId, int disciplinaId)
		{
			var disciplinaDocente = _context.DisciplinaDocentes.FirstOrDefault(dd =>
				dd.DocenteId == docenteId && dd.DisciplinaId == disciplinaId
			);
			if (disciplinaDocente == null)
			{
				return NotFound("DisciplinaDocente não encontrada");
			}
			_context.DisciplinaDocentes.Remove(disciplinaDocente);
			_context.SaveChanges();
			return NoContent();
		}
	}

	public class UpdateDisciplinaDocenteRequest
	{
		public int OldDocenteId { get; set; }
		public int DisciplinaId { get; set; }
		public int NewDocenteId { get; set; }
	}
}
