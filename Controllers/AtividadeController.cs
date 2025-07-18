using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sgchdAPI.Data;
using sgchdAPI.Models;

namespace sgchdAPI.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public class AtividadeController(ApplicationDbContext context) : ControllerBase
	{
		private readonly ApplicationDbContext _context = context;

		[HttpGet]
		public IActionResult GetAll()
		{
			var atividades = _context.Atividades.ToList();
			return Ok(atividades);
		}

		[HttpGet("{id}")]
		public IActionResult GetById(int id)
		{
			var atividade = _context.Atividades.Find(id);
			if (atividade == null)
			{
				return NotFound();
			}
			return Ok(atividade);
		}

		[HttpGet("docente/{docenteId}")]
		public IActionResult GetByDocenteId(int docenteId)
		{
			var atividades = _context.Atividades.Where(a => a.DocenteId == docenteId).ToList();
			if (atividades == null)
			{
				return NotFound();
			}
			return Ok(atividades);
		}

		[HttpPost]
		public IActionResult Create(Atividade atividade)
		{
			_context.Atividades.Add(atividade);
			_context.SaveChanges();
			return CreatedAtAction(nameof(GetById), new { id = atividade.Id }, atividade);
		}

		[HttpPut("{id}")]
		public IActionResult Update(int id, Atividade atividade)
		{
			var existingAtividade = _context.Atividades.FirstOrDefault(d => d.Id == id);

			if (existingAtividade == null || id != existingAtividade.Id)
			{
				return BadRequest();
			}

			existingAtividade.DocenteId = atividade.DocenteId;
			existingAtividade.Titulo = atividade.Titulo;
			existingAtividade.Descricao = atividade.Descricao;
			existingAtividade.Duracao = atividade.Duracao;
			existingAtividade.Tipo = atividade.Tipo;

			_context.SaveChanges();
			return NoContent();
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			var atividade = _context.Atividades.Find(id);
			if (atividade == null)
			{
				return NotFound();
			}
			_context.Atividades.Remove(atividade);
			_context.SaveChanges();
			return NoContent();
		}
	}
}
