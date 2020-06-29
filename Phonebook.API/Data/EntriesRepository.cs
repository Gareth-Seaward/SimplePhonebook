using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Phonebook.API.Helpers;
using Phonebook.API.Models;

namespace Phonebook.API.Data
{
  public class EntriesRepository : CommonRepository, IEntriesRepository
  {
    public EntriesRepository(DataContext context) : base(context)
    {
    }

    public async Task<Entry> GetEntry(int entryId)
    {
      return await _context.Entries
        .Include(e => e.Phonebook)
        .FirstOrDefaultAsync(e => e.Id == entryId);
    }

    public async Task AddEntry(Models.Entry entry)
    {
      await _context.Entries.AddAsync(entry);
      await _context.SaveChangesAsync();
    }

    public async Task DeleteEntry(Entry entry)
    {
       _context.Entries.Remove(entry);
       await _context.SaveChangesAsync();
    }

    public async Task<PagedList<Entry>> GetEntries(int phonebookId, EntryParams entryParams)
    {
      var entries = _context.Entries.Include(e => e.Phonebook)
        .Where(e => e.Phonebook.Id == phonebookId)
        .OrderBy(e => e.Name)
        .AsQueryable();

      FilterByStartsWith(entryParams, ref entries);
      FilterByMatch(entryParams, ref entries);

      return await PagedList<Entry>.CreateAsync(entries, entryParams.PageNumber, entryParams.PageSize);
    }

    private void FilterByStartsWith(EntryParams entryParams,ref IQueryable<Entry> entriesList)
    {
      if (string.IsNullOrEmpty(entryParams.StartsWith)) return;

      entriesList = entriesList.Where(e => e.Name.ToLower().StartsWith(entryParams.StartsWith.ToLower()));
    }

    private void FilterByMatch(EntryParams entryParams, ref IQueryable<Entry> entriesList)
    {
      if (string.IsNullOrEmpty(entryParams.Match)) return;

      entriesList = entriesList.Where(e => e.Name.ToLower().Contains(entryParams.Match.ToLower()));
    }
  }
}