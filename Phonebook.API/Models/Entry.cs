namespace Phonebook.API.Models
{
  public class Entry
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public Phonebook Phonebook { get; set; }
  }
}