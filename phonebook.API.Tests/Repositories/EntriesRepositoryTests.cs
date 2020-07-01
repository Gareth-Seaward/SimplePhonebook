using System.Threading.Tasks;
using AutoMapper;
using Moq;
using NUnit.Framework;
using phonebook.API.Tests.Fakes;
using Phonebook.API.Data;
using Phonebook.API.Helpers;
using Phonebook.API.Models;

namespace phonebook.API.Tests.Repositories
{
  [TestFixture]
  public class EntriesRepositoryTests
  {
    private DataContext fakeContext;

    [SetUp]
    public void Setup()
    {

      fakeContext = FakeContextGenerator.Context;
    }

    [Test]
    public async Task GetEntry_From_Context()
    {
      var repo = new EntriesRepository(fakeContext);

      var result = await repo.GetEntry(1);

      Assert.That(result.Name, Is.EqualTo("ATestEntry1"));
    }

    [Test]
    public async Task AddsEntry_To_Context()
    {
      var testEntryToAdd = new Entry { Name = "TestNameToAdd", PhoneNumber = "TestPhoneNumber" };
      var repo = new EntriesRepository(fakeContext);

      await repo.AddEntry(testEntryToAdd);

      CollectionAssert.Contains(fakeContext.Entries, testEntryToAdd);
    }

    [Test]
    public async Task DeletesEntry_From_Context()
    {
      var testEntryToDelete = new Entry { Name = "TestNameToDelete", PhoneNumber = "TestPhoneNumber" };
      fakeContext.Entries.Add(testEntryToDelete);

      var repo = new EntriesRepository(fakeContext);

      await repo.DeleteEntry(testEntryToDelete);

      CollectionAssert.DoesNotContain(fakeContext.Entries, testEntryToDelete);
    }

    [Test]
    public async Task GetsAllEntrysPaged_For_Phonebook()
    {
      var repo = new EntriesRepository(fakeContext);
      var testParams = new EntryParams
      {
        PageNumber = 1,
        PageSize = 5
      };

      var result = await repo.GetEntries(1, testParams);

      Assert.That(result.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task GetsAllEntriesPaged_StartingWithCharacterInName()
    {
      var repo = new EntriesRepository(fakeContext);
      var testParams = new EntryParams
      {
        PageNumber = 1,
        PageSize = 5,
        StartsWith = "A"
      };

      var result = await repo.GetEntries(1, testParams);

      Assert.That(result.Count, Is.EqualTo(1));
    }

    [Test]
    public async Task GetsAllEntriesPaged_ContainingSubstringInName()
    {
      var repo = new EntriesRepository(fakeContext);
      var testParams = new EntryParams
      {
        PageNumber = 1,
        PageSize = 5,
        Match = "Entry2"
      };

      var result = await repo.GetEntries(1, testParams);

      Assert.That(result.Count, Is.EqualTo(1));
    }
  }
}