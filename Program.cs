using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
});

builder.Services.ConfigureApplicationCookie(options =>
{
	options.LoginPath = "/Account/Login"; // Rota para a página de login
	options.AccessDeniedPath = "/Account/AccessDenied"; // Rota para acesso negado
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

		var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
		await RolesSeed.SeedRoles(roleManager);
	}
	catch (Exception ex)
	{
		Console.WriteLine($"Erro ao executar o seeding: {ex.Message}");
	}
}

app.Run();
