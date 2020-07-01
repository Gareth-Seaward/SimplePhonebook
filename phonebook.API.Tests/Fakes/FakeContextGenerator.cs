using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using Phonebook.API.Data;
using Models = Phonebook.API.Models;

namespace phonebook.API.Tests.Fakes
{
    public static class FakeContextGenerator
    {
      private static DataContext _context;

      public static DataContext Context
      { get {return _context ?? (_context = CreateDbContext());}}
        private static DataContext CreateDbContext()
        {

          var options = new DbContextOptionsBuilder<DataContext>()
                        .UseInMemoryDatabase(databaseName: "InMemoryArticleDatabase")
                        .Options;

          var context = new DataContext(options);

          SetupData(context);

          return context;
        }

        private static void SetupData(DataContext context)
        {
          var users = GetTestUsers();
         context.Users.AddRange( users);
         var phonebooks = GetTestPhonebooks(users.FirstOrDefault());
         context.Phonebooks.AddRange(phonebooks);
         var entries = GetTestEntries(phonebooks.FirstOrDefault());
         context.Entries.AddRange(entries);

         context.SaveChanges();
        }

        private static List<Models.User> GetTestUsers()
        {
          return new List<Models.User> 
          {
            new Models.User 
            { 
              Id = 1,
              Username = "Test1", 
              PasswordHash = System.Text.Encoding.UTF8.GetBytes("TestHash1"),
              PasswordSalt = System.Text.Encoding.UTF8.GetBytes("TestSalt1")
            },
            new Models.User 
            { 
              Id = 2,
              Username = "Test2", 
              PasswordHash = System.Text.Encoding.UTF8.GetBytes("TestHash2"),
              PasswordSalt = System.Text.Encoding.UTF8.GetBytes("TestSalt2")
            }
          };
        }
  

        private static List<Models.Phonebook> GetTestPhonebooks(Models.User user)
        {
          return new List<Models.Phonebook> 
          {
            new Models.Phonebook {Id = 1, Name = "TestPhonebook1", User = user}
          };
        }

        private static List<Models.Entry> GetTestEntries(Models.Phonebook phonebook)
        {
          return new List<Models.Entry> 
          {
            new Models.Entry {Id = 1, Name = "ATestEntry1", PhoneNumber = "011 123 4567", Phonebook = phonebook},
            new Models.Entry {Id = 2, Name = "BTestEntry2", PhoneNumber = "011 765 4321", Phonebook = phonebook},
          };
        }
    }
}