namespace sgchdAPI.Models
{
    public class DisciplinaDocente(int disciplinaId, int docenteId)
    {
        public int DisciplinaId { get; set; } = disciplinaId;
        public Disciplina? Disciplina { get; set; }
        public int DocenteId { get; set; } = docenteId;
        public Docente? Docente { get; set; }
    }
}
