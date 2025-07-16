using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using sgchdAPI.Models.Auth;

namespace sgchdAPI.Controllers.Auth
{
	[ApiController]
	[Route("api/[controller]")]
	public class AccountController : ControllerBase
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly SignInManager<IdentityUser> _signInManager;

		public AccountController(
			UserManager<IdentityUser> userManager,
			SignInManager<IdentityUser> signInManager
		)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

		[HttpPost("register")] // Rota para registrar um novo usuário ( /api/account/register )
		public async Task<IActionResult> Register([FromBody] RegisterRequest request)
		{
			var user = new IdentityUser { UserName = request.Email, Email = request.Email };
			var result = await _userManager.CreateAsync(user, request.Password);

			if (result.Succeeded)
			{
				return Ok(new { message = "Usuário registrado com sucesso!" });
			}

			return BadRequest(new { errors = result.Errors });
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequest request)
		{
			var result = await _signInManager.PasswordSignInAsync(
				request.Email,
				request.Password,
				false,
				false
			);

			if (result.Succeeded)
			{
				return Ok(new { message = "Login realizado com sucesso!" });
			}

			return Unauthorized(new { message = "Credenciais inválidas." });
		}

		[Authorize]
		[HttpPost("logout")]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return Ok(new { message = "Logout realizado com sucesso!" });
		}

		[Authorize] // Apenas usuários com a role 'Admin' podem acessar este endpoint
		[HttpPost("assign-role")]
		public async Task<IActionResult> AssignRole([FromBody] AssignRoleRequest request)
		{
			var user = await _userManager.FindByEmailAsync(request.Email);
			if (user == null)
			{
				return NotFound(new { message = "Usuário não encontrado." });
			}

			var result = await _userManager.AddToRoleAsync(user, request.Role);
			if (result.Succeeded)
			{
				return Ok(
					new
					{
						message = $"Role '{request.Role}' atribuída ao usuário '{request.Email}'.",
					}
				);
			}

			return BadRequest(new { errors = result.Errors });
		}

		[Authorize]
		[HttpGet("access-denied")]
		public IActionResult AccessDenied()
		{
			return Forbid(
				new { message = "Acesso negado. Você não tem permissão para acessar este recurso." }
			);
		}

		[HttpGet("try-login")]
		public IActionResult TryLogin()
		{
			return Forbid(new { message = "Usuário não autenticado." });
		}

		private IActionResult Forbid(object value)
		{
			return new ObjectResult(value) { StatusCode = StatusCodes.Status403Forbidden };
		}
	}
}
