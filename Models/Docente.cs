namespace sgchdAPI.Models
{
    public class Docente(int id, string name, string email)
    {
        public int Id { get; set; } = id;
        public string Name { get; set; } = name;
        public string Email { get; set; } = email;

        public ICollection<DisciplinaDocente>? DisciplinaDocentes { get; set; }
    }
}
