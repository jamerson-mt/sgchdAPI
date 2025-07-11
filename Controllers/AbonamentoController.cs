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
			_path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "upload");

			// Valide se o diretório existe
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
		public IActionResult Create(
			[FromForm] int docenteId,
			[FromForm] string titulo,
			[FromForm] string descricao,
			[FromForm] int duracao,
			[FromForm] DateTime dataInicio,
			[FromForm] IFormFile file
		)
		{
			// Converter dataInicio para UTC
			dataInicio = DateTime.SpecifyKind(dataInicio, DateTimeKind.Utc);

			// Validação do arquivo PDF
			if (file == null || file.Length == 0)
			{
				return BadRequest("O arquivo PDF é obrigatório.");
			}

			if (file.Length > 10485760)
			{
				return BadRequest("O arquivo deve ter no máximo 10MB.");
			}

			// Gerar um nome único para o arquivo caso já exista
			var fileName = Path.GetFileName(file.FileName);
			var filePath = Path.Combine(_path, fileName);
			if (System.IO.File.Exists(filePath))
			{
				var uniqueId = Guid.NewGuid().ToString();
				fileName =
					$"{Path.GetFileNameWithoutExtension(fileName)}_{uniqueId}{Path.GetExtension(fileName)}";
				filePath = Path.Combine(_path, fileName);
			}

			// Salvar o arquivo PDF
			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				file.CopyTo(stream);
			}

			// Salvar o caminho relativo do arquivo
			var relativePath = Path.Combine("upload", fileName);

			// Criar o objeto Abonamento
			var abonamento = new Abonamento
			{
				DocenteId = docenteId,
				Titulo = titulo,
				Descricao = descricao,
				Duracao = duracao,
				DataInicio = dataInicio,
				UrlPdf = relativePath,
			};

			// Salvar no banco de dados
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
				return NotFound("Esse abonamento não existe");
			}

			if (file != null)
			{
				// validação de tamanho (10MB)
				if (file.Length > 10485760)
				{
					return BadRequest("O arquivo deve ter no máximo 10MB");
				}

				// Gerar um nome único para o arquivo caso já exista
				var fileName = Path.GetFileName(file.FileName);
				var filePath = Path.Combine(_path, fileName);
				if (System.IO.File.Exists(filePath))
				{
					var uniqueId = Guid.NewGuid().ToString();
					fileName =
						$"{Path.GetFileNameWithoutExtension(fileName)}_{uniqueId}{Path.GetExtension(fileName)}";
					filePath = Path.Combine(_path, fileName);
				}

				// salvar novo PDF
				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					file.CopyTo(stream);
				}

				// deletar o arquivo antigo
				var oldFilePath = Path.Combine(
					Directory.GetCurrentDirectory(),
					"wwwroot",
					existingAbonamento.UrlPdf
				);
				if (System.IO.File.Exists(oldFilePath))
				{
					System.IO.File.Delete(oldFilePath);
				}

				// salvar o caminho relativo do novo arquivo
				var relativePath = Path.Combine("upload", fileName);
				existingAbonamento.UrlPdf = relativePath;
			}

			existingAbonamento.DocenteId = abonamento.DocenteId;
			existingAbonamento.Titulo = abonamento.Titulo;
			existingAbonamento.Descricao = abonamento.Descricao;
			existingAbonamento.Duracao = abonamento.Duracao;
			existingAbonamento.DataInicio = DateTime.SpecifyKind(
				abonamento.DataInicio,
				DateTimeKind.Utc
			);

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

			// deletar o arquivo PDF associado
			var filePath = Path.Combine(
				Directory.GetCurrentDirectory(),
				"wwwroot",
				abonamento.UrlPdf
			);
			if (System.IO.File.Exists(filePath))
			{
				System.IO.File.Delete(filePath);
			}

			_context.Abonamentos.Remove(abonamento);
			_context.SaveChanges();

			return NoContent();
		}
	}
}
