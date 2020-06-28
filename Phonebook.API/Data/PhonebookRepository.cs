using System.Threading.Tasks;

namespace Phonebook.API.Data
{
  public class PhonebookRepository : IPhonebookRepository
  {
    private readonly DataContext _context;

    public PhonebookRepository(DataContext context)
    {
      _context = context;
    }
    public async Task CreatePhonebook(Models.Phonebook phonebookToCreate)
    {
      await _context.Phonebooks.AddAsync(phonebookToCreate);
      await _context.SaveChangesAsync();
    }
  }
}