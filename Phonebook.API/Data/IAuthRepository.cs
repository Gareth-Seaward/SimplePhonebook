using System.Threading.Tasks;
using Phonebook.API.Models;

namespace Phonebook.API.Data
{
    public interface IAuthRepository
    {
         Task<User> Register(User user, string password);
         Task<User> Login(string username, string password);
         Task<bool> DoesUserExist(string username);
    }
}