namespace sgchdAPI.Models
{
    public class Disciplina
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CargaHoraria { get; set; }
        public string Periodo { get; set; }

        public Disciplina(int id, string name, string codigo, int cargaHoraria, string periodo) 
        {
            Id = id;
            Name = name;
            CargaHoraria = cargaHoraria;
            Periodo = periodo;
        }
        
    }
}