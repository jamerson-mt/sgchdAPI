namespace sgchdAPI.Models
{
    public class Disciplina(int id, string name, int cargaHoraria, int periodo)
    {
        public int Id { get; set; } = id;
        public string Name { get; set; } = name;
        public int Periodo { get; set; } = periodo;
        public int CargaHoraria { get; set; } = cargaHoraria;

        public ICollection<DisciplinaDocente>? DisciplinaDocentes { get; set; }
    }
}
