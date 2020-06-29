using System.Threading.Tasks;

namespace Phonebook.API.Data
{
  public abstract class CommonRepository
  {
    protected readonly DataContext _context;
    public CommonRepository(DataContext context)
    {
      _context = context;

    }
    public async Task SaveAsync()
    {
      await _context.SaveChangesAsync();
    }
  }
}