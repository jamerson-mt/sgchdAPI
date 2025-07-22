using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

public class ClaimsService
{
	private readonly UserManager<ApplicationUser> _userManager;

	public ClaimsService(UserManager<ApplicationUser> userManager)
	{
		_userManager = userManager;
	}

	public async Task AddRoleClaimsAsync(ApplicationUser user, ClaimsPrincipal principal)
	{
		var roles = await _userManager.GetRolesAsync(user); // Carrega roles do banco
		var identity = principal.Identity as ClaimsIdentity;

		if (identity != null)
		{
			foreach (var role in roles)
			{
				identity.AddClaim(new Claim(ClaimTypes.Role, role)); // Adiciona roles como claims
			}
		}
	}
}
