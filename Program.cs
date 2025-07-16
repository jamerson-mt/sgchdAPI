using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using sgchdAPI.Data;
using sgchdAPI.Data.Seeds;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
	options.UseNpgsql("Host=localhost;Database=sgchd;Username=postgres;Password=21301");
});

builder
	.Services.AddIdentity<IdentityUser, IdentityRole>() // Configuração do Identity
	.AddEntityFrameworkStores<ApplicationDbContext>()
	.AddDefaultTokenProviders();

builder
	.Services.AddAuthentication(options =>
	{
		options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme; // Esquema de autenticação padrão, que é o esquema de cookie que faz a autenticação baseada em cookies
		options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme; // Esquema de desafio padrão, que é o esquema de cookie que faz a autenticação baseada em cookies
	})
	.AddCookie(options =>
	{
		options.LoginPath = "/api/account/try-login"; // Rota para a página de login
		options.AccessDeniedPath = "/api/account/access-denied"; // Rota para acesso negado
	});

builder.Services.Configure<IdentityOptions>(options =>
{
	options.Password.RequireDigit = true;
	options.Password.RequiredLength = 8;
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequireUppercase = true;
	options.Password.RequireLowercase = true;
	options.ClaimsIdentity.RoleClaimType = ClaimTypes.Role; // Mapeia a claim de role corretamente
});

builder.Services.ConfigureApplicationCookie(options =>
{
	options.LoginPath = "/api/account/try-login"; // Rota para a página de login
	options.AccessDeniedPath = "/api/account/access-denied"; // Rota para acesso negado,
});

// Configuração do CORS
builder.Services.AddCors(options =>
{
	options.AddPolicy(
		"AllowAll",
		builder =>
		{
			builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
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
app.UseCors("AllowAll");

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

using (var scope = app.Services.CreateScope()) //
{
	var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
	await SeedRolesAsync(roleManager);
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
