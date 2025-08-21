using System.Security.Claims;
using DotNetEnv;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using sgchdAPI.Data;
using sgchdAPI.Data.Seeds;
using sgchdAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// ---------- CONFIGURAÇÃO DE SERVIÇOS ----------

// Carregar variáveis de ambiente do arquivo .env
Env.Load();

// Configuração do banco de dados
var username = Environment.GetEnvironmentVariable("DB_USERNAME");
var password = Environment.GetEnvironmentVariable("DB_PASSWORD");

if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
{
	throw new InvalidOperationException(
		"As variáveis de ambiente 'DB_USERNAME' e 'DB_PASSWORD' devem estar configuradas."
	);
}

var connectionString =
	builder
		.Configuration.GetConnectionString("DefaultConnection")
		?.Replace("{DB_USERNAME}", username)
		.Replace("{DB_PASSWORD}", password)
	?? throw new InvalidOperationException("Connection string is null.");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

// Configuração do Identity
builder
	.Services.AddIdentity<IdentityUser, IdentityRole>()
	.AddEntityFrameworkStores<ApplicationDbContext>()
	.AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
	options.Password.RequireDigit = true;
	options.Password.RequiredLength = 8;
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequireUppercase = true;
	options.Password.RequireLowercase = true;
	options.ClaimsIdentity.RoleClaimType = ClaimTypes.Role;
});

builder.Services.ConfigureApplicationCookie(options =>
{
	options.LoginPath = "/api/account/try-login";
	options.AccessDeniedPath = "/api/account/access-denied";
});

builder
	.Services.AddAuthentication(options =>
	{
		options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
		options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
	})
	.AddCookie(options =>
	{
		options.LoginPath = "/auth/login";
		options.AccessDeniedPath = "/api/account/access-denied";
	});

// Configuração de autorização
builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
	options.AddPolicy("ManagerPolicy", policy => policy.RequireRole("Manager"));
	// options.AddPolicy("UserPolicy", policy => policy.RequireRole("User"));

	// Exemplo de política personalizada
	options.AddPolicy(
		"AuthenticatedUserPolicy",
		policy =>
		{
			policy.RequireAuthenticatedUser();
			policy.RequireRole("Admin", "Manager"); //
		}
	);
});

// Configuração do CORS
builder.Services.AddCors(options =>
{
	options.AddPolicy(
		"AllowSpecificOrigin",
		builder =>
		{
			builder
				.WithOrigins("http://workload.cigr.ifpe.edu", "http://localhost:5173")
				.AllowAnyHeader()
				.AllowAnyMethod()
				.AllowCredentials();
		}
	);
});

// Configuração de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adicionar controladores
builder.Services.AddControllers();

// ---------- CONFIGURAÇÃO DO PIPELINE ----------

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// EXECUTA O SEEDING DO BANCO DE DADOS

// await DatabaseSeeder.SeedAsync(app.Services);

app.Run();
