using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sgchdAPI.Data;
using sgchdAPI.Models;

namespace sgchdAPI.Controllers
{
	[Authorize(Roles = "ADMIN")] //
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

		[HttpGet("{id:int}")]
		public IActionResult GetById(int id)
		{
			var abonamento = _context.Abonamentos.Find(id);

			if (abonamento == null)
			{
				return NotFound();
			}

			return Ok(abonamento);
		}

		[HttpGet("docente/{docenteId:int}")]
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
			// Validação dos campos obrigatórios
			if (docenteId <= 0)
			{
				return BadRequest("O campo 'docenteId' é obrigatório e deve ser maior que zero.");
			}
			if (string.IsNullOrEmpty(titulo))
			{
				return BadRequest("O campo 'titulo' é obrigatório.");
			}
			if (string.IsNullOrEmpty(descricao))
			{
				return BadRequest("O campo 'descricao' é obrigatório.");
			}
			if (duracao <= 0)
			{
				return BadRequest("O campo 'duracao' é obrigatório e deve ser maior que zero.");
			}
			if (dataInicio == default)
			{
				return BadRequest("O campo 'dataInicio' é obrigatório.");
			}

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
			[FromForm] int? docenteId,
			[FromForm] string titulo,
			[FromForm] string descricao,
			[FromForm] int? duracao
		)
		{
			var existingAbonamento = _context.Abonamentos.Find(id);
			if (existingAbonamento == null)
			{
				return NotFound("Esse abonamento não existe.");
			}

			// Atualizar somente os valores fornecidos
			if (docenteId.HasValue)
			{
				existingAbonamento.DocenteId = docenteId.Value;
			}
			if (!string.IsNullOrEmpty(titulo))
			{
				existingAbonamento.Titulo = titulo;
			}
			if (!string.IsNullOrEmpty(descricao))
			{
				existingAbonamento.Descricao = descricao;
			}
			if (duracao.HasValue)
			{
				existingAbonamento.Duracao = duracao.Value;
			}

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
				abonamento.UrlPdf ?? string.Empty
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
