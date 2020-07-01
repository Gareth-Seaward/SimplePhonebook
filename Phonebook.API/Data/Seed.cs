using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Models = Phonebook.API.Models;

namespace Phonebook.API.Data
{
    public class Seed
  {
    private readonly DataContext context;
    public Seed(DataContext context)
    {
      this.context = context;

    }

    public static void SeedEntries(DataContext context)
    {
      if(context.Entries.Any() || !context.Phonebooks.Any()) return;
      var entryData = System.IO.File.ReadAllText("Data/UserEntryData.json");
      var entries = JsonConvert.DeserializeObject<List<Models.Entry>>(entryData);

      var phonebook = context.Phonebooks.FirstOrDefault();

      foreach(var entry in entries)
      {
        entry.Phonebook = phonebook;

        context.Entries.Add(entry);
      }

      context.SaveChanges();
    }

    public static void SeedUsers(DataContext context)
    {
      if(context.Users.Any()) return;
      var userData = System.IO.File.ReadAllText("Data/UserPhonebookData.json");
      var phonebooks = JsonConvert.DeserializeObject<List<Models.Phonebook>>(userData);
      foreach(var phonebook in phonebooks) 
      {
        byte[] passwordHash, passwordSalt;
        CreatePasswordHash("password", out passwordHash, out passwordSalt);

        var user = new Models.User
        {
          Username = phonebook.User.Username.ToLower(),
          PasswordHash = passwordHash,
          PasswordSalt = passwordSalt
        };

        context.Users.Add(user);
        phonebook.User = user;
        context.Phonebooks.Add(phonebook);
      }
      context.SaveChanges();
    }

    private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
      using (var hmac = new System.Security.Cryptography.HMACSHA512())
      {
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
      }
    }
  }
}