using Phonebook.API.Models;

namespace Phonebook.API.Utils
{
    public interface IJwtTokenGenerator
    {
         string GetJwt(User user);
    }
}