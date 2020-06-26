using Microsoft.EntityFrameworkCore;
using Models = Phonebook.API.Models;

namespace Phonebook.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {        }

        public DbSet<Models.Phonebook> Phonebooks { get; set; }
        public DbSet<Models.User> Users { get; set; }
    }
}