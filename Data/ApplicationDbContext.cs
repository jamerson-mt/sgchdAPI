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
        }

        public DbSet<Docente> Docentes { get; set; }
        public DbSet<Disciplina> Disciplinas { get; set; }
        public DbSet<DisciplinaDocente> DisciplinaDocentes { get; set; }

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

            // Configurações adicionais, se necessário



        }


    }
}
