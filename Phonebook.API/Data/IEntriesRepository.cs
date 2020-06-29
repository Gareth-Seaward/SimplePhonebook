using System.Threading.Tasks;
using Phonebook.API.Helpers;
using Phonebook.API.Models;

namespace Phonebook.API.Data
{
    public interface IEntriesRepository
    {
         Task<Entry> GetEntry(int entryId);
         Task AddEntry(Models.Entry entry);
         Task DeleteEntry(Entry entry);
         Task<PagedList<Entry>> GetEntries(int phonebookId, EntryParams entryParams);
         Task SaveAsync();
    }
}