namespace sgchdAPI.Models.Auth
{
	public class RegisterRequest
	{
		public required string Email { get; set; }
		public required string Password { get; set; }
		public string? Name { get; set; }
	}

	public class LoginRequest
	{
		public required string Email { get; set; }
		public required string Password { get; set; }
	}
}
