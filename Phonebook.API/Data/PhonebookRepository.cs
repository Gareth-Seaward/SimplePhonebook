using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Phonebook.API.Data
{
  public class PhonebookRepository : CommonRepository, IPhonebookRepository
  {

    public PhonebookRepository(DataContext context) 
    :base(context)
    {    }
    public async Task CreatePhonebook(Models.Phonebook phonebookToCreate)
    {
      await _context.Phonebooks.AddAsync(phonebookToCreate);
      await _context.SaveChangesAsync();
    }

    public async Task<Models.Phonebook> GetPhonebook(int id)
    {
      return await _context.Phonebooks
        .Include(p => p.User)
        .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Models.Phonebook> GetPhonebookForUser(int userId)
    {
      return await _context.Phonebooks
        .Include(p => p.User)
        .FirstOrDefaultAsync(p => p.User.Id == userId);
    }


  }
}