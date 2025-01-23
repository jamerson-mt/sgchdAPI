using Microsoft.EntityFrameworkCore;
using sgchdAPI.Models;
using sgchdAPI.Data.Seeds;

namespace sgchdAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            // Chame o método de seed aqui
            DisciplinaSeed.Seed(this);
			DocenteSeed.Seed(this);
			DocenteElegivelSeed.Seed(this);
        }

        public DbSet<Docente> Docentes { get; set; }
        public DbSet<Disciplina> Disciplinas { get; set; }
        public DbSet<DisciplinaDocente> DisciplinaDocentes { get; set; }
		public DbSet<DocenteElegivel> DocentesElegiveis { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DisciplinaDocente>()
                .HasKey(dd => new { dd.DisciplinaId, dd.DocenteId });

            modelBuilder.Entity<DisciplinaDocente>()
                .HasOne(dd => dd.Disciplina)
                .WithMany(d => d.DisciplinaDocentes)
                .HasForeignKey(dd => dd.DisciplinaId);

            modelBuilder.Entity<DisciplinaDocente>()
                .HasOne(dd => dd.Docente)
                .WithMany(d => d.DisciplinaDocentes)
                .HasForeignKey(dd => dd.DocenteId);

            // Configuracoes de DocentesElegiveis
            modelBuilder.Entity<DocenteElegivel>()
                .HasKey(de => new { de.DisciplinaId, de.DocenteId });

            modelBuilder.Entity<DocenteElegivel>()
                .HasOne(de => de.Disciplina)
                .WithMany(d => d.DocentesElegiveis)
                .HasForeignKey(de => de.DisciplinaId);

            modelBuilder.Entity<DocenteElegivel>()
                .HasOne(de => de.Docente)
                .WithMany(d => d.DisciplinasElegiveis)
                .HasForeignKey(de => de.DocenteId);

        }
    }
}
