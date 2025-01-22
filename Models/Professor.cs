namespace sgchdAPI.Models
{
    public class Professor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // ...other properties...

        public Professor(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
