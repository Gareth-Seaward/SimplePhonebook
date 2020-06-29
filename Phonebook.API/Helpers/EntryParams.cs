namespace Phonebook.API.Helpers
{
    public class EntryParams : PagingParams
    {
        public int EntryId { get; set; }
        public string StartsWith { get; set; }
        public string Match { get; set; }
    }
}