using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Phonebook.API.Models;
using Phonebook.API.Utils;

namespace Phonebook.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        private readonly IPasswordHasher _passwordHasher;

        public AuthRepository(DataContext context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }
        public async Task<bool> DoesUserExist(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }

        public async Task<User> Login(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if(user is null) return null;
                

            if(!_passwordHasher.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

                return user;
        }

        public async Task<User> Register(User user, string password)
        {
            _passwordHasher.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }
    }
}