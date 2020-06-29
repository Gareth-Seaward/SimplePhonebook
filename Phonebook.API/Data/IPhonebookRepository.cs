using System.Threading.Tasks;
using Models = Phonebook.API.Models;

namespace Phonebook.API.Data
{
    public interface IPhonebookRepository
    {
      Task CreatePhonebook(Models.Phonebook phonebookToCreate);
      Task<Models.Phonebook> GetPhonebook(int id);
      Task SaveAsync();
    }
}