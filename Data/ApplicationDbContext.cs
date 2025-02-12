using Microsoft.EntityFrameworkCore;
using sgchdAPI.Data.Seeds;
using sgchdAPI.Models;

namespace sgchdAPI.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
			// // Chame o método de seed aqui
			CursoSeed.Seed(this);
			DisciplinaSeed.Seed(this);
			DocenteSeed.Seed(this);
			DocenteElegivelSeed.Seed(this);
			AbonamentoSeed.Seed(this);
		}

		public DbSet<Curso> Cursos { get; set; }
		public DbSet<Docente> Docentes { get; set; }
		public DbSet<Disciplina> Disciplinas { get; set; }
		public DbSet<DisciplinaDocente> DisciplinaDocentes { get; set; }
		public DbSet<DocenteElegivel> DocentesElegiveis { get; set; }
		public DbSet<Abonamento> Abonamentos { get; set; }
		public DbSet<Atividade> Atividades { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

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
