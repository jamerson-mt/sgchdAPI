using System.Security.Claims;
using DotNetEnv; // Adicione esta linha para usar DotNetEnv
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using sgchdAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// ---------- CONFIGURAÇÃO DO BANCO DE DADOS ----------
// Carregar variáveis de ambiente do arquivo .env
Env.Load();

// Configuração do Entity Framework Core com PostgreSQL
var username = Environment.GetEnvironmentVariable("DB_USERNAME"); // Carregar o nome de usuário da variável de ambiente
var password = Environment.GetEnvironmentVariable("DB_PASSWORD"); // Carregar a senha da variável de ambiente

// Verificar se as variáveis de ambiente estão configuradas
if (string.IsNullOrEmpty(username))
{
	throw new InvalidOperationException(
		"A variável de ambiente 'DB_USERNAME' não está configurada."
	);
}

if (string.IsNullOrEmpty(password))
{
	throw new InvalidOperationException(
		"A variável de ambiente 'DB_PASSWORD' não está configurada."
	);
}

var connectionString =
	builder
		.Configuration.GetConnectionString("DefaultConnection")
		?.Replace("{DB_USERNAME}", username)
		.Replace("{DB_PASSWORD}", password) // Substituir os placeholders na string de conexão
	?? throw new InvalidOperationException("Connection string is null.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
	options.UseNpgsql(connectionString);
});

// ---------- CONFIGURAÇÃO DO IDENTITY ----------
// Configuração do Identity com Entity Framework Core e IdentityUser/IdentityRole

builder
	.Services.AddIdentity<IdentityUser, IdentityRole>() // Configuração do Identity com IdentityUser e IdentityRole
	.AddEntityFrameworkStores<ApplicationDbContext>()
	.AddDefaultTokenProviders();

// Configuração do Identity para usar cookies de autenticação
builder
	.Services.AddAuthentication(options =>
	{
		options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme; // Esquema de autenticação padrão, que é o esquema de cookie que faz a autenticação baseada em cookies
		options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme; // Esquema de desafio padrão, que é o esquema de cookie que faz a autenticação baseada em cookies
	})
	.AddCookie(options =>
	{
		options.LoginPath = "/auth/login"; // Rota para a página de login
		options.AccessDeniedPath = "/api/account/access-denied"; // Rota para acesso negado
	});

// Configuração das opções do Identity
builder.Services.Configure<IdentityOptions>(options =>
{
	options.Password.RequireDigit = true;
	options.Password.RequiredLength = 8;
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequireUppercase = true;
	options.Password.RequireLowercase = true;
	options.ClaimsIdentity.RoleClaimType = ClaimTypes.Role; // Mapeia a claim de role corretamente
});

// Configuração do cookie de autenticação
builder.Services.ConfigureApplicationCookie(options =>
{
	options.LoginPath = "/api/account/try-login"; // Rota para a página de login
	options.AccessDeniedPath = "/api/account/access-denied"; // Rota para acesso negado,
});

// Configuração de autorização
builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin")); // Cria uma política de autorização que requer a role "Admin"
	options.AddPolicy("UserPolicy", policy => policy.RequireRole("User")); // Cria uma política de autorização que requer a role "User"
	options.AddPolicy(
		"AuthenticatedUserPolicy",
		policy =>
		{
			policy.RequireAuthenticatedUser(); // Cria uma política de autorização que requer um usuário autenticado
			policy.RequireRole("Admin", "User"); // Requer que o usuário tenha a role "Admin" ou "User"
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
				.WithOrigins("http://localhost:5173") // Substitua pelo domínio da sua aplicação frontend
				.AllowAnyHeader()
				.AllowAnyMethod()
				.AllowCredentials(); // Permite envio de cookies e credenciais
		}
	);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Removed AddIdentityApiEndpoints as it is not a valid method
// Identity services are already configured above

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Configuração para servir arquivos estáticos
app.UseStaticFiles(); //

// Aplicar a política de CORS
app.UseCors("AllowSpecificOrigin");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// // Executar o seeding do banco de dados

using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	try
	{
		var context = services.GetRequiredService<ApplicationDbContext>();
		context.Database.Migrate(); // Aplica as migrações pendentes
		context.SeedDatabase(); // Executa o seeding
	}
	catch (Exception ex)
	{
		Console.WriteLine($"Erro ao executar o seeding: {ex.Message}");
	}
}

// Seed roles into the database
using (var scope = app.Services.CreateScope()) //
{
	var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>(); //
	await SeedRolesAsync(roleManager); //
}

// Define the SeedRolesAsync method

async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager) //
{
	var roles = new[] { "Admin", "User" };
	foreach (var role in roles)
	{
		if (!await roleManager.RoleExistsAsync(role))
		{
			await roleManager.CreateAsync(new IdentityRole(role));
		}
	}
}

app.Run();
