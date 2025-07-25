using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using sgchdAPI.Data.Seeds;
using sgchdAPI.Models;

namespace sgchdAPI.Data
{
	public class ApplicationDbContext : IdentityDbContext<IdentityUser, IdentityRole, string> // Usando Identity para autenticação e autorização
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) // Injetando as opções de configuração do DbContext
			: base(options) { }

		// Método dedicado para executar os seeds
		// como executar os seeds
		// https://docs.microsoft.com/pt-br/ef/core/managing-schemas/migrations/seeding


		public void SeedDatabase()
		{
			if (!Cursos.Any())
			{
				CursoSeed.Seed(this);
			}

			if (!Disciplinas.Any())
			{
				DisciplinaSeed.Seed(this);
			}

			if (!Docentes.Any())
			{
				DocenteSeed.Seed(this);
			}

			if (!DocentesElegiveis.Any())
			{
				DocenteElegivelSeed.Seed(this);
			}

			//fazer o seed de roles

			// Adicione verificações semelhantes para outros seeds, se necessário
			// if (!Abonamentos.Any()) { AbonamentoSeed.Seed(this); }
		}

		public DbSet<Curso> Cursos { get; set; } //
		public DbSet<Docente> Docentes { get; set; }
		public DbSet<Disciplina> Disciplinas { get; set; }
		public DbSet<DisciplinaDocente> DisciplinaDocentes { get; set; }
		public DbSet<DocenteElegivel> DocentesElegiveis { get; set; }
		public DbSet<Abonamento> Abonamentos { get; set; }
		public DbSet<Atividade> Atividades { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Configurações adicionais para Identity
			modelBuilder.Entity<IdentityUser>(entity =>
			{
				entity.ToTable("Usuarios"); // Nome da tabela para usuários
				entity.HasKey(u => u.Id);
			});

			modelBuilder.Entity<IdentityRole>(entity =>
			{
				entity.ToTable("Roles"); // Nome da tabela para roles
				entity.HasKey(r => r.Id);
			});

			modelBuilder.Entity<IdentityUserRole<string>>(entity =>
			{
				entity.ToTable("UsuarioRoles"); // Nome da tabela para associação entre usuários e roles
				entity.HasKey(ur => new { ur.UserId, ur.RoleId });
			});

			modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
			{
				entity.ToTable("UsuarioClaims"); // Nome da tabela para claims de usuários
				entity.HasKey(uc => uc.Id);
			});

			modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
			{
				entity.ToTable("UsuarioLogins"); // Nome da tabela para logins de usuários
				entity.HasKey(ul => new { ul.LoginProvider, ul.ProviderKey });
			});

			modelBuilder.Entity<IdentityRoleClaim<string>>(entity =>
			{
				entity.ToTable("RoleClaims");
				entity.HasKey(rc => rc.Id);
			});

			modelBuilder.Entity<IdentityUserToken<string>>(entity =>
			{
				entity.ToTable("UsuarioTokens"); // Nome da tabela para tokens de usuários
				entity.HasKey(ut => new
				{
					ut.UserId,
					ut.LoginProvider,
					ut.Name,
				});
			});

			modelBuilder.Entity<Curso>(entity =>
			{
				entity.ToTable("Cursos");
				entity.HasKey(e => e.Id);
				// Adicione outras configurações de propriedade conforme necessário
			});

			modelBuilder
				.Entity<DisciplinaDocente>()
				.HasKey(dd => new { dd.DisciplinaId, dd.DocenteId });

			modelBuilder
				.Entity<DisciplinaDocente>()
				.HasOne(dd => dd.Disciplina)
				.WithMany(d => d.DisciplinaDocentes)
				.HasForeignKey(dd => dd.DisciplinaId);

			modelBuilder
				.Entity<DisciplinaDocente>()
				.HasOne(dd => dd.Docente)
				.WithMany(d => d.DisciplinaDocentes)
				.HasForeignKey(dd => dd.DocenteId);

			// Configuracoes de DocentesElegiveis
			modelBuilder
				.Entity<DocenteElegivel>()
				.HasKey(de => new { de.DisciplinaId, de.DocenteId });

			modelBuilder
				.Entity<DocenteElegivel>()
				.HasOne(de => de.Disciplina)
				.WithMany(d => d.DocentesElegiveis)
				.HasForeignKey(de => de.DisciplinaId);

			modelBuilder
				.Entity<DocenteElegivel>()
				.HasOne(de => de.Docente)
				.WithMany(d => d.DisciplinasElegiveis)
				.HasForeignKey(de => de.DocenteId);

			modelBuilder
				.Entity<Curso>()
				.HasMany(c => c.Disciplinas)
				.WithOne(d => d.Curso)
				.HasForeignKey(d => d.CursoId);

			modelBuilder
				.Entity<Abonamento>()
				.HasOne(a => a.Docente)
				.WithMany(d => d.Abonamentos)
				.HasForeignKey(a => a.DocenteId);

			modelBuilder
				.Entity<Docente>()
				.HasMany(d => d.Abonamentos)
				.WithOne(a => a.Docente)
				.HasForeignKey(a => a.DocenteId);

			modelBuilder.Entity<Atividade>().HasKey(a => a.Id);

			modelBuilder
				.Entity<Atividade>()
				.HasOne(a => a.Docente)
				.WithMany(d => d.Atividades)
				.HasForeignKey(a => a.DocenteId);
		}
	}
}
