namespace Phonebook.API.Models
{
    public class Phonebook
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public User User { get; set; }
    }
}