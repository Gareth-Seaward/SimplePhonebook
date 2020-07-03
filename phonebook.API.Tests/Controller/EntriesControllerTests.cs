using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Phonebook.API.Constants;
using Phonebook.API.Controllers;
using Phonebook.API.Data;
using Phonebook.API.Dtos;
using Phonebook.API.Helpers;
using Models = Phonebook.API.Models;

namespace phonebook.API.Tests.Controller
{
  [TestFixture]
  public class EntriesControllerTests
  {
    private Mock<IEntriesRepository> mockRepo;
    private Mock<IMapper> mockMapper;

    [SetUp]
    public void Setup()
    {
      mockRepo = new Mock<IEntriesRepository>();
      mockMapper = new Mock<IMapper>();
    }

    [Test]
    public async Task Get_Entries_For_Phonebook()
    {
      var httpContext = new Mock<HttpContext>(MockBehavior.Strict);
      var response = new Mock<HttpResponse>();
      response.SetupProperty(it => it.StatusCode);
      var headers = new Mock<IHeaderDictionary>();
      response.SetupGet(it => it.Headers).Returns(headers.Object);
      httpContext.Setup(hc => hc.Response).Returns(response.Object);

      var controller = new EntriesController(mockRepo.Object, mockMapper.Object);
      controller.ControllerContext = new ControllerContext
      {
        HttpContext = httpContext.Object
      };

      var testPhonebookId = 1;
      var testParams = new EntryParams();

      var testPagedEntries = GetTestPagedEntries();
      mockRepo.Setup(mr => mr.GetEntries(testPhonebookId, testParams)).Returns(Task.FromResult(testPagedEntries));

      var testResponseEntries = GetTestResponseEntries();
      mockMapper.Setup(mm => mm.Map<IEnumerable<EntryForResponseDto>>(testPagedEntries)).Returns(testResponseEntries);
      var result = await controller.GetEntries(testPhonebookId, testParams);

      Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task GetsEntryFromHttpContextCache_For_EntryId()
    {
      var httpContext = new Mock<HttpContext>(MockBehavior.Strict);
      var items = new Mock<IDictionary<object, object>>();
      httpContext.SetupGet(hc => hc.Items).Returns(items.Object);
      var controller = new EntriesController(mockRepo.Object, mockMapper.Object);
      controller.ControllerContext = new ControllerContext
      {
        HttpContext = httpContext.Object
      };

      var testEntry = new Models.Entry { Id = 1 };
      items.SetupGet(i => i[HttpContextConstants.EntryItem]).Returns(testEntry);

      var testResponseEntry = new EntryForResponseDto { Id = 1 };
      mockMapper.Setup(mm => mm.Map<Models.Entry, EntryForResponseDto>(testEntry)).Returns(testResponseEntry);

      var result = await controller.GetEntry(1, 1);

      Assert.That((result as OkObjectResult).Value, Is.EqualTo(testResponseEntry));
    }

    [Test]
    public async Task UpdatesEntry_On_Repo()
    {
      var httpContext = new Mock<HttpContext>(MockBehavior.Strict);
      var items = new Mock<IDictionary<object, object>>();
      httpContext.SetupGet(hc => hc.Items).Returns(items.Object);
      var controller = new EntriesController(mockRepo.Object, mockMapper.Object);
      controller.ControllerContext = new ControllerContext
      {
        HttpContext = httpContext.Object
      };

      var testEntry = new Models.Entry { Id = 1 };
      items.SetupGet(i => i[HttpContextConstants.EntryItem]).Returns(testEntry);

      var testEntryForUpdate = new EntryForUpdateDto { Name = "TestName", PhoneNumber = "TestPhoneNumber" };
      mockMapper.Setup(mm => mm.Map<EntryForUpdateDto, Models.Entry>(testEntryForUpdate, testEntry));

      var result = await controller.UpdateEntry(1, 2, testEntryForUpdate);

      mockRepo.Verify(mr => mr.SaveAsync());

      Assert.That(result, Is.InstanceOf<OkResult>());
    }

    [Test]
    public async Task CreatesNewEntry_For_givenInput()
    {
      var httpContext = new Mock<HttpContext>(MockBehavior.Strict);
      var items = new Mock<IDictionary<object, object>>();
      httpContext.SetupGet(hc => hc.Items).Returns(items.Object);
      var controller = new EntriesController(mockRepo.Object, mockMapper.Object);
      controller.ControllerContext = new ControllerContext
      {
        HttpContext = httpContext.Object
      };

      var testPhonebook = new Models.Phonebook { Id = 1, Name = "TestPhonebook" };
      items.SetupGet(i => i[HttpContextConstants.PhonebookItem]).Returns(testPhonebook);

      var testEntryForCreate = new EntryForUpdateDto { Name = "Test Name To Create", PhoneNumber = "Test phonenumber To Create" };
      var testEntryForRepo = new Models.Entry { Name = "Test name", PhoneNumber = "Test Phonenumber" };
      mockMapper.Setup(mm => mm.Map<EntryForUpdateDto, Models.Entry>(testEntryForCreate)).Returns(testEntryForRepo);

      var testEntryForResponse = new EntryForResponseDto { Id = 2, Name = "test", PhoneNumber = "Test" };
      mockMapper.Setup(mm => mm.Map<Models.Entry, EntryForResponseDto>(testEntryForRepo)).Returns(testEntryForResponse);
      var result = await controller.CreateEntry(1, testEntryForCreate);

      Assert.That(result, Is.InstanceOf<CreatedAtRouteResult>());
    }

    [Test]
    public async Task DeletesEntry_On_Repo()
    {
      var httpContext = new Mock<HttpContext>(MockBehavior.Strict);
      var items = new Mock<IDictionary<object, object>>();
      httpContext.SetupGet(hc => hc.Items).Returns(items.Object);
      var controller = new EntriesController(mockRepo.Object, mockMapper.Object);
      controller.ControllerContext = new ControllerContext
      {
        HttpContext = httpContext.Object
      };

      var testEntry = new Models.Entry { Id = 1 };
      items.SetupGet(i => i[HttpContextConstants.EntryItem]).Returns(testEntry);

      var result = await controller.DeleteEntry(1, 2);

      mockRepo.Verify(mr => mr.DeleteEntry(testEntry));
      Assert.That(result, Is.InstanceOf<OkResult>());
    }

    private PagedList<Models.Entry> GetTestPagedEntries()
    {
      var entries = new List<Models.Entry>
          {
            new Models.Entry{Id = 1},
            new Models.Entry{Id = 2},
            new Models.Entry{Id = 3},
          };
      return new PagedList<Models.Entry>(entries, 3, 1, 5);
    }

    private IEnumerable<EntryForResponseDto> GetTestResponseEntries()
    {
      return new List<EntryForResponseDto>
          {
            new EntryForResponseDto{Id = 1},
            new EntryForResponseDto {Id = 2},
            new EntryForResponseDto {Id = 3}
          };
    }
  }
}