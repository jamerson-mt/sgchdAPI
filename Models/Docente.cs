namespace sgchdAPI.Models
{
    public class Docente
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public Docente(int id, string name, string email) 
        {
            Id = id;
            Name = name;
            Email = email;
        }
    }
}