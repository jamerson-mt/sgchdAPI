using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sgchdAPI.Data;
using sgchdAPI.Data.Seeds;
using sgchdAPI.Models;

namespace sgchdAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AbonamentoController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private readonly string _path;

		public AbonamentoController(ApplicationDbContext context)
		{
			_context = context;
			_path = Path.Combine(Directory.GetCurrentDirectory(), "upload", "pdf");

			//valide se o diretorio existe
			if (!Directory.Exists(_path))
			{
				Directory.CreateDirectory(_path);
			}
		}

		[HttpGet]
		public IActionResult Get()
		{
			return Ok(_context.Abonamentos);
		}

		[HttpGet("{id}")]
		public IActionResult GetById(int id)
		{
			var abonamento = _context.Abonamentos.Find(id);

			if (abonamento == null)
			{
				return NotFound();
			}

			return Ok(abonamento);
		}

		[HttpGet("{docenteId}")]
		public IActionResult GetByDocenteId(int docenteId)
		{
			var abonamento = _context.Abonamentos.ToList().Where(a => a.DocenteId == docenteId);

			if (abonamento == null)
			{
				return NotFound();
			}

			return Ok(abonamento);
		}

		[HttpPost]
		public IActionResult Create([FromForm] Abonamento abonamento, [FromForm] IFormFile file)
		{
			// validacao de obrigatorio
			if (file == null || file.Length == 0)
			{
				return BadRequest("O arquivo PDF é obrigatório");
			}

			if (file.Length > 10485760)
			{
				return BadRequest("O arquivo deve ter no máximo 10MB");
			}

			// salvar pdf
			var fileName = Path.GetFileName(file.FileName);
			var filePath = Path.Combine(_path, fileName);

			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				file.CopyTo(stream);
			}

			// Salvar o caminho relativo
			var relativePath = Path.Combine("Upload", fileName);
			abonamento.UrlPdf = relativePath;

			_context.Abonamentos.Add(abonamento);
			_context.SaveChanges();

			return CreatedAtAction(nameof(GetById), new { id = abonamento.Id }, abonamento);
		}

		[HttpPut("{id}")]
		public IActionResult Update(
			int id,
			[FromForm] Abonamento abonamento,
			[FromForm] IFormFile file
		)
		{
			var existingAbonamento = _context.Abonamentos.Find(id);
			if (existingAbonamento == null)
			{
				return NotFound("esse abonamento não existe");
			}

			if (file != null)
			{
				// validacao de tamanho (10mb)
				if (file.Length > 10485760)
				{
					return BadRequest("O arquivo deve ter no máximo 10MB");
				}

				// salvar pdf
				var fileName = Path.GetFileName(file.FileName);
				var filePath = Path.Combine(_path, fileName);

				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					file.CopyTo(stream);
					System.IO.File.Delete(existingAbonamento.UrlPdf); //deletar o arquivo antigo
				}

				// Salvar o caminho relativo
				var relativePath = Path.Combine("Upload", fileName);
				existingAbonamento.UrlPdf = relativePath;
			}

			existingAbonamento.DocenteId = abonamento.DocenteId;
			existingAbonamento.Titulo = abonamento.Titulo;
			existingAbonamento.Descricao = abonamento.Descricao;
			existingAbonamento.Duracao = abonamento.Duracao;

			_context.SaveChanges();

			return NoContent();
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			var abonamento = _context.Abonamentos.Find(id);

			if (abonamento == null)
			{
				return NotFound();
			}

			_context.Abonamentos.Remove(abonamento);
			_context.SaveChanges();

			return NoContent();
		}
	}
}
