using Microsoft.AspNetCore.Mvc;
using sgchdAPI.Data;
using sgchdAPI.Models;
using System.Linq;

namespace sgchdAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
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

    }
}
