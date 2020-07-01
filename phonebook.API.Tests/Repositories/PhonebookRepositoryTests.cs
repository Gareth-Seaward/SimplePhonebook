using System.Threading.Tasks;
using NUnit.Framework;
using phonebook.API.Tests.Fakes;
using Phonebook.API.Data;
using Models = Phonebook.API.Models;

namespace phonebook.API.Tests.Repositories
{
  [TestFixture]
  public class PhonebookRepositoryTests
  {
    private DataContext fakeContext;

    [SetUp]
    public void Setup()
    {
      fakeContext = FakeContextGenerator.Context;
    }

    [Test]
    public async Task CreatesPhoneBook_And_AddsToContext()
    {
      var testPhonebookToAdd = new Models.Phonebook { Name = "TestAddPhonebook" };
      var repo = new PhonebookRepository(fakeContext);

      await repo.CreatePhonebook(testPhonebookToAdd);

      CollectionAssert.Contains(fakeContext.Phonebooks, testPhonebookToAdd);
    }

    [Test]
    public async Task GetsPhonebook_From_Repo()
    {
      var repo = new PhonebookRepository(fakeContext);
      var result = await repo.GetPhonebook(1);

      Assert.That(result.Name, Is.EqualTo("TestPhonebook1"));
    }

    [Test]
    public async Task GetsPhonebooks_For_A_User()
    {
      var repo = new PhonebookRepository(fakeContext);
      var result = await repo.GetPhonebookForUser(1);

      Assert.That(result.Name, Is.EqualTo("TestPhonebook1"));
    }
  }
}