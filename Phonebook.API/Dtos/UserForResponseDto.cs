namespace Phonebook.API.Dtos
{
    public class UserForResponseDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int PhonebookId { get; set; }
        public string PhonebookName { get; set; }
    }
}